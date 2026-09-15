using System;
using System.Collections.Generic;
using System.Globalization;

namespace HT_SECIM_C_EKRAN.Core
{
    /// <summary>
    /// Sahneyi surer: stage'i okur, director'leri oynatir, container'lara yazar.
    ///
    /// Reji uygulamasindaki VizCommander'in C ekrani karsiligi. Oradaki
    /// HAZIRLA / VER / AL akisi burada yok; sahne 24 saat yayinda duruyor,
    /// yapilan tek sey uzerine yazmak ve donus animasyonlarini tetiklemek.
    /// </summary>
    public class SahneSurucu
    {
        /// <summary> Kapali director'leri acarken en fazla kac tur denenecek. </summary>
        private const int ACMA_TURU = 6;

        private readonly VizEngine engine;
        private readonly SahneIndeksi indeks = new SahneIndeksi();

        /// <summary>
        /// Gorsel yolu -> UUID onbellegi. UUID'ler havuzda sabit, uygulama
        /// boyunca degismez; her sayfa donusunde engine'e sormak gereksiz.
        /// </summary>
        private readonly Dictionary<string, string> gorselUuid =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        public SahneSurucu(VizEngine engine)
        {
            this.engine = engine;
        }

        public SahneIndeksi Indeks { get { return indeks; } }

        public bool BagliMi { get { return engine != null && engine.isConnected; } }

        /// <summary> Sahne okundu ve director id'leri elimizde mi. </summary>
        public bool Hazir { get { return indeks.Hazir; } }

        /// <summary>
        /// Sahnenin stage agacini okur ve director id'lerini cikarir.
        ///
        /// "STAGE GET ALL" yalnizca acik satirlarin altini dokuyor; bu sahnede
        /// director'ler kapali geliyor. Kapali olanlar acilip tekrar okunuyor,
        /// ic ice dallar her turda bir kademe daha aciliyor.
        ///
        /// Engine yeniden baslarsa ya da sahne yeniden yuklenirse butun id'ler
        /// degisir; o yuzden baglanti her kurulusunda bu yeniden cagrilmali.
        /// </summary>
        public bool SahneyiOku()
        {
            indeks.Temizle();

            // Sahne yeniden yuklenmis olabilir; eski UUID'ler artik gecersiz.
            gorselUuid.Clear();

            if (!BagliMi)
            {
                CLog.Error("SAHNE OKUNAMADI", "engine bagli degil");
                return false;
            }

            string cevap = engine.SendAndWait(SahneAyarlari.Layer + "*STAGE GET ALL", 5000);

            if (string.IsNullOrEmpty(cevap))
            {
                CLog.Error("SAHNE OKUNAMADI", SahneAyarlari.Layer + " cevap vermedi");
                return false;
            }

            int acilan = 0;

            for (int tur = 0; tur < ACMA_TURU; tur++)
            {
                List<int> kapali = SahneIndeksi.KapaliDirectorler(cevap);
                if (kapali.Count == 0) break;

                foreach (int id in kapali) engine.Send("#" + id + "*OPEN SET 1");

                acilan += kapali.Count;

                cevap = engine.SendAndWait(SahneAyarlari.Layer + "*STAGE GET ALL", 5000);
                if (string.IsNullOrEmpty(cevap)) break;
            }

            indeks.Coz(cevap);

            CLog.Log("SAHNE OKUNDU",
                indeks.Sayi + " director" + (acilan > 0 ? " / " + acilan + " satir acildi" : ""));

            return indeks.Hazir;
        }

        /// <summary>
        /// Bir director'u basa alip oynatir.
        /// Ad yolu "UST_3/SAYFA1" gibi; ayni ad birden fazla yerde gectigi icin
        /// id ile adresleniyor.
        /// </summary>
        public bool Oynat(string adYolu)
        {
            if (!BagliMi)
            {
                CLog.Error("KOMUT GONDERILMEDI (engine bagli degil)", adYolu);
                return false;
            }

            int id = indeks.Id(adYolu);
            if (id <= 0) return false;

            engine.Send("#" + id + " START");
            CLog.Detail("DIRECTOR OYNATILDI", adYolu + " (#" + id + ")");

            return true;
        }

        /// <summary>
        /// Container'a metin yazma komutu uretir.
        ///
        /// C ekranindaki veri alanlarinin adlari sahne genelinde tekil
        /// (UST_3_SAYFA1_SIRA1_PARTI_AD gibi), o yuzden "$ad" ile adreslemek
        /// guvenli - director'lerdeki gibi id cikarmaya gerek yok.
        /// </summary>
        public static string MetinKomutu(string container, string deger)
        {
            return SahneAyarlari.Layer + "*TREE*$" + container + "*GEOM*TEXT SET " + (deger ?? "");
        }

        /// <summary> Sayisal yolla adreslenen container (kup yuzleri, twitter). </summary>
        public static string YolMetinKomutu(string yol, string deger)
        {
            return SahneAyarlari.Layer + "*TREE*" + yol + "*GEOM*TEXT SET " + (deger ?? "");
        }

        /// <summary>
        /// Satirin renk seridini boyar.
        ///
        /// DIKKAT - IKI AYRI OLCEK VAR: container'in GEOM rengi 0-255
        /// aliyor ama MATERIAL*COLOR 0-1 arasi ondalik istiyor. Buradaki
        /// renk seritleri materyalli, o yuzden bolunuyor.
        ///
        /// Sayilar nokta ile yazilmali; Turkce kulturde virgul cikar ve
        /// engine komutu sessizce yok sayar.
        ///
        /// Renk cozulemezse komut uretilmiyor: sahnedeki mevcut renk
        /// kalir, siyah bir serit gorunmez.
        /// </summary>
        public static string RenkKomutu(string container, string renk)
        {
            if (string.IsNullOrEmpty(container) || string.IsNullOrEmpty(renk)) return null;

            string[] parca = renk.Split(';');
            if (parca.Length < 3) return null;

            int r, g, b;

            if (!int.TryParse(parca[0].Trim(), out r)) return null;
            if (!int.TryParse(parca[1].Trim(), out g)) return null;
            if (!int.TryParse(parca[2].Trim(), out b)) return null;

            CultureInfo n = CultureInfo.InvariantCulture;

            return SahneAyarlari.Layer + "*TREE*$" + container + "*MATERIAL*COLOR SET " +
                   (r / 255.0).ToString("0.000", n) + " " +
                   (g / 255.0).ToString("0.000", n) + " " +
                   (b / 255.0).ToString("0.000", n) + " 1.000";
        }

        /// <summary>
        /// Metni yazar ve KUTUYA SIGDIRIR.
        ///
        /// Sahnedeki baslik kutulari sabit genislikte ve Viz yaziyi
        /// kendiliginden kucultmuyor: uzun bir il adi yan kutunun uzerine
        /// tasiyor. Burada yazi, harf sayisi butcesini asarsa yatayda
        /// oranla sikistiriliyor.
        ///
        /// TASARIM OLCEGI DISARIDAN VERILIYOR, sahneden okunmuyor. Okusaydik
        /// uygulama bir kere sikistirdiktan sonra sikismis degeri "tasarim"
        /// sanip her turda biraz daha kucultur, yazi eriyip giderdi.
        /// Degerler sahneden bir kere olculup CEkranYollari'na yazildi.
        /// </summary>
        /// <param name="tasarimOlcek">container'in sahnedeki SCALING*X degeri</param>
        /// <param name="sigacakHarf">bu olcekte kutuya sigan harf sayisi</param>
        public static void SigdirEkle(List<string> komutlar, string yol, string metin,
                                      double tasarimOlcek, int sigacakHarf)
        {
            if (yol == null) return;

            metin = metin ?? "";
            komutlar.Add(YolMetinKomutu(yol, metin));

            if (tasarimOlcek <= 0 || sigacakHarf <= 0) return;

            double olcek = tasarimOlcek;

            if (metin.Length > sigacakHarf)
                olcek = tasarimOlcek * sigacakHarf / metin.Length;

            komutlar.Add(SahneAyarlari.Layer + "*TREE*" + yol + "*TRANSFORMATION*SCALING*X SET " +
                         olcek.ToString("0.0000", CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Renk komutunu listeye ekler; renk cozulemezse hicbir sey eklemez.
        /// Cagiran her yerde null denetimi yapmasin diye.
        /// </summary>
        public static void RenkEkle(List<string> komutlar, string container, string renk)
        {
            string komut = RenkKomutu(container, renk);
            if (komut != null) komutlar.Add(komut);
        }

        /// <summary> Adiyla adreslenen container'i acar / kapatir. </summary>
        public static string ActiveKomutu(string container, bool acik)
        {
            return SahneAyarlari.Layer + "*TREE*$" + container + "*ACTIVE SET " + (acik ? "1" : "0");
        }

        /// <summary>
        /// Sayisal yolla adreslenen container'i acar / kapatir.
        ///
        /// Yalnizca SERIT KOKLERI ve UST duzeni icin kullaniliyor. Sayfa
        /// container'larina dokunulmuyor: onlari sahnenin kendi director'leri
        /// aciyor, disaridan karismak animasyonu bozar.
        /// </summary>
        public static string YolActiveKomutu(string yol, bool acik)
        {
            return SahneAyarlari.Layer + "*TREE*" + yol + "*ACTIVE SET " + (acik ? "1" : "0");
        }

        /// <summary>
        /// Container'in dokusuna Viz image havuzundan gorsel giydirir.
        ///
        /// Yol UUID'ye cevriliyor: havuzda ayni isim birden fazla klasorde
        /// gecebiliyor ve Viz tam yol verilse bile ismi havuz genelinde
        /// cozup baska klasordekini getirebiliyor. UUID tek kesin adres.
        /// Cevrilemezse yol oldugu gibi kullanilir (eski davranis).
        /// </summary>
        public string GorselKomutu(string container, string vizImage)
        {
            if (string.IsNullOrEmpty(vizImage)) return null;

            return SahneAyarlari.Layer + "*TREE*$" + container +
                   "*TEXTURE*IMAGE SET IMAGE*" + GorselAdresi(vizImage.TrimStart('/'));
        }

        private string GorselAdresi(string yol)
        {
            // Zaten UUID verilmisse dokunma.
            if (yol.StartsWith("<", StringComparison.Ordinal)) return yol;

            string uuid;
            if (gorselUuid.TryGetValue(yol, out uuid)) return uuid ?? yol;

            uuid = null;

            if (BagliMi)
            {
                string cevap = engine.SendAndWait("IMAGE*" + yol + "*UUID GET", 3000);

                if (!string.IsNullOrEmpty(cevap))
                {
                    cevap = cevap.Trim();

                    if (cevap.StartsWith("<", StringComparison.Ordinal)) uuid = cevap;
                    else CLog.Error("GORSEL UUID ALINAMADI", yol + " -> " + cevap);
                }
            }

            // Basarisiz sorgu da onbellege giriyor: her sayfa donusunde
            // olmayan bir gorsel icin engine'i mesgul etmenin anlami yok.
            gorselUuid[yol] = uuid;

            if (uuid != null) CLog.Detail("GORSEL COZULDU", yol + " -> " + uuid);

            return uuid ?? yol;
        }

        /// <summary>
        /// Aday / parti / ittifak adi gibi SONUC bilgisi.
        /// Secim yasagi suresince bos doner.
        /// </summary>
        public static string VeriMetni(string deger)
        {
            return CEkranSecenekleri.YasakModu ? "" : (deger ?? "");
        }

        /// <summary>
        /// Bir sonuc oranini sahnenin bekledigi iki parcaya bolup yazar.
        /// tamContainer tam kismi, ondalikContainer ondalik haneyi aliyor.
        ///
        /// Secim yasagi suresince iki kutu da bosaltiliyor - sahnedeki
        /// virgul ve yuzde isareti sabit container'larda duruyor, onlara
        /// dokunmuyoruz.
        /// </summary>
        public static void OranKomutlari(List<string> komutlar, string tamContainer,
                                         string ondalikContainer, int oranX100)
        {
            if (CEkranSecenekleri.YasakModu)
            {
                komutlar.Add(MetinKomutu(tamContainer, ""));
                komutlar.Add(MetinKomutu(ondalikContainer, ""));
                return;
            }

            string tam, ondalik;
            CEkranAdlari.OranParcala(oranX100, CEkranSecenekleri.CiftHane, out tam, out ondalik);

            komutlar.Add(MetinKomutu(tamContainer, tam));
            komutlar.Add(MetinKomutu(ondalikContainer, ondalik));
        }

        /// <summary>
        /// Baslikta duran acilan sandik / katilim orani.
        ///
        /// SAHNE TUZAGI: basliklardaki ONDALIK kutusu (ASS_ORAN2 / ASS_SAYI2)
        /// sahnede ACTIVE = 0 birakilmis ve virgul de sabit bir isaret degil,
        /// icine yazilan bir metin kutusu. Yani ondalik haneyi yazmak
        /// yetmiyor: kutuyu acmak ve virgulu de yazmak gerekiyor, yoksa
        /// ekranda "98" gorunuyor, "98,2" degil.
        ///
        /// Sahne bu kutulari yer yer acik yer yer kapali teslim edilmis
        /// (UST_4 acik, UST_3 kapali...), o yuzden her sayfada acikca
        /// ayarlaniyor - sahnenin o anki haline guvenilmiyor.
        ///
        /// Satirlardaki oranlarda bu sorun yok; onlarda ayirac NOKTA_SABIT
        /// ve ondalik kutulari zaten acik.
        /// </summary>
        /// <param name="virgulContainer">virgul metin kutusu, yoksa null</param>
        /// <param name="goster">deger yazilacak mi, yoksa alan bosaltilacak mi</param>
        public static void BaslikOranKomutlari(List<string> komutlar, string tamContainer,
                                               string ondalikContainer, string virgulContainer,
                                               int oranX100, bool goster)
        {
            if (!goster)
            {
                komutlar.Add(MetinKomutu(tamContainer, ""));
                komutlar.Add(MetinKomutu(ondalikContainer, ""));
                komutlar.Add(ActiveKomutu(ondalikContainer, false));

                if (virgulContainer != null) komutlar.Add(MetinKomutu(virgulContainer, ""));
                return;
            }

            string tam, ondalik;
            CEkranAdlari.OranParcala(oranX100, CEkranSecenekleri.CiftHane, out tam, out ondalik);

            komutlar.Add(MetinKomutu(tamContainer, tam));
            komutlar.Add(MetinKomutu(ondalikContainer, ondalik));
            komutlar.Add(ActiveKomutu(ondalikContainer, true));

            if (virgulContainer != null) komutlar.Add(MetinKomutu(virgulContainer, ","));
        }

        /// <summary>
        /// Komutlari TEK pakette gonderir.
        ///
        /// Sira onemli: once metinler, sonra director. Tersi olursa animasyon
        /// eski degerlerle basliyor ve ekranda bir an yanlis sayi goruluyor.
        /// Tek pakette gittikleri icin engine hepsini ayni karede isliyor.
        /// </summary>
        public bool Gonder(List<string> komutlar)
        {
            if (komutlar == null || komutlar.Count == 0) return true;

            if (!BagliMi)
            {
                CLog.Error("KOMUT GONDERILMEDI (engine bagli degil)", komutlar.Count + " komut");
                return false;
            }

            bool sonuc = engine.SendMany(komutlar);

            CLog.Detail("KOMUT GONDERILDI", komutlar.Count + " komut tek pakette");
            return sonuc;
        }

        /// <summary>
        /// Metinleri yazip hemen ardindan director'u oynatir.
        /// Ikisi tek pakette gider, boylece animasyon dogru degerlerle baslar.
        /// </summary>
        public bool YazVeOynat(List<string> komutlar, string directorYolu)
        {
            int id = indeks.Id(directorYolu);
            if (id <= 0) return false;

            List<string> paket = new List<string>(komutlar);
            paket.Add("#" + id + " START");

            return Gonder(paket);
        }

        /// <summary>
        /// Bir director'un o anki zamani. Sifirdan buyukse oynamis demektir;
        /// uzunluguna esitse bitmis.
        /// </summary>
        public double Zaman(string adYolu)
        {
            if (!BagliMi) return -1;

            int id = indeks.Id(adYolu);
            if (id <= 0) return -1;

            return OndalikOku(engine.SendAndWait("#" + id + "*TIME GET", 2000));
        }

        /// <summary> Bir director'un toplam uzunlugu, saniye. </summary>
        public double Uzunluk(string adYolu)
        {
            if (!BagliMi) return -1;

            int id = indeks.Id(adYolu);
            if (id <= 0) return -1;

            return OndalikOku(engine.SendAndWait("#" + id + "*LENGTH GET", 2000));
        }

        /// <summary>
        /// "1 0.78000000000002" gibi bir cevaptan sayiyi cikarir.
        /// Viz her zaman nokta ile yaziyor, kultur bagimsiz cozulmeli.
        /// </summary>
        private static double OndalikOku(string cevap)
        {
            if (string.IsNullOrEmpty(cevap)) return -1;

            string[] parcalar = cevap.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = parcalar.Length - 1; i >= 0; i--)
            {
                double deger;
                if (double.TryParse(parcalar[i],
                        System.Globalization.NumberStyles.Float,
                        System.Globalization.CultureInfo.InvariantCulture,
                        out deger))
                    return deger;
            }

            return -1;
        }
    }
}
