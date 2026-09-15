using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace HT_SECIM_C_EKRAN.Core
{
    /// <summary>
    /// Tek bir seridin (ALT / UST / SOL) donusu.
    ///
    /// Her seridin kendi zamanlayicisi var; biri digerini beklemiyor.
    /// Zaman dolunca sonraki sayfanin metinleri yazilir ve director TEK
    /// PAKETTE tetiklenir, boylece giris animasyonu dogru degerlerle baslar.
    ///
    /// UI THREAD'INDE CALISIR. Windows.Forms.Timer kullaniliyor: sayfalar
    /// DataService index'lerini okuyor ve o sozlukler yeni veri gelince UI
    /// thread'inde yeniden kuruluyor. Arka plan zamanlayicisi olsaydi tam
    /// o anda yarim tabloyla karsilasabilirdi.
    /// </summary>
    public class Serit
    {
        /// <summary>
        /// Bir tik'te en fazla kac bos sayfa atlanacak.
        /// Butun sayfalar bossa sonsuz donguye girmemek icin siniri var.
        /// </summary>
        private const int ATLAMA_SINIRI = 32;

        private readonly string ad;
        private readonly string kokYolu;
        private readonly List<CEkranSayfasi> sayfalar = new List<CEkranSayfasi>();
        private readonly Timer zamanlayici = new Timer();
        private readonly SahneSurucu surucu;

        private int sira = -1;
        private string acikDuzen;

        /// <summary> O an ekranda olan duzenin giris director'u. </summary>
        private string oynatilanGiris;

        /// <summary> Sayfa listesi basa dondugunde tetiklenir. Il imleci buna bagli. </summary>
        public event Action<Serit> OnTurTamamlandi;

        /// <summary> Bir sayfa ekrana verildiginde tetiklenir. Arayuzdeki gosterge icin. </summary>
        public event Action<Serit, CEkranSayfasi> OnSayfaDegisti;

        /// <param name="ad">log ve arayuzde gorunecek ad</param>
        /// <param name="kokYolu">seridin kok container yolu, ACTIVE icin</param>
        public Serit(string ad, string kokYolu, SahneSurucu surucu)
        {
            this.ad      = ad;
            this.kokYolu = kokYolu;
            this.surucu  = surucu;

            zamanlayici.Tick += delegate { SonrakiSayfa(); };
        }

        public string Ad { get { return ad; } }

        public bool Donuyor { get { return zamanlayici.Enabled; } }

        /// <summary> Sayfa listesi. Motor kurulusta dolduruyor. </summary>
        public List<CEkranSayfasi> Sayfalar { get { return sayfalar; } }

        /// <summary> O an ekranda olan sayfa. Hic sayfa verilmediyse null. </summary>
        public CEkranSayfasi Sayfa
        {
            get { return (sira >= 0 && sira < sayfalar.Count) ? sayfalar[sira] : null; }
        }

        /// <summary>
        /// O an ekranda olan sayfanin suresi, saniye.
        ///
        /// Sure serit degil SAYFA basina tutuluyor: eski C ekraninda da
        /// birinci ve ikinci sayfanin sureleri ayriydi. Zamanlayici her
        /// sayfa gecisinde o sayfanin suresine kuruluyor, boylece arayuzde
        /// degistirilen sure bir sonraki sayfada yuruyor.
        /// </summary>
        public int Sure
        {
            get { return Sayfa == null ? 0 : Sayfa.Sure; }
        }

        private void SureyiKur(CEkranSayfasi sayfa)
        {
            zamanlayici.Interval = Math.Max(3, sayfa.Sure) * 1000;
        }

        /// <summary>
        /// Seridi baslatir.
        ///
        /// Ilk sayfa BEKLENMEDEN veriliyor: operator BASLAT'a bastiginda
        /// ekranda bir sey gorunmesi icin 12 saniye beklemek istemiyor.
        /// </summary>
        public void Baslat()
        {
            if (Donuyor) return;

            // Durdurulup yeniden baslatilirken duzen bayraklarinin ve giris
            // animasyonunun gercekten yeniden gonderilmesi icin hatirlananlar
            // unutuluyor - serit ekrana yeniden ucarak gelmeli.
            acikDuzen = null;
            oynatilanGiris = null;

            if (sayfalar.Count == 0)
            {
                CLog.Error("SERIT BASLAMADI", ad + " - sayfa listesi bos");
                return;
            }

            if (CEkranSecenekleri.ActiveYonet && kokYolu != null)
                surucu.Gonder(new List<string> { SahneSurucu.YolActiveKomutu(kokYolu, true) });

            sira = -1;
            zamanlayici.Start();

            CLog.Log("SERIT BASLADI", ad + " / " + sayfalar.Count + " sayfa");

            SonrakiSayfa();
        }

        /// <summary>
        /// Seridi durdurur ve kokunu kapatir.
        ///
        /// Kok kapatiliyor cunku sahne 24 saat yayinda: durdurup birakirsak
        /// son sayfa ekranda donmus halde kalir.
        /// </summary>
        public void Durdur()
        {
            if (!Donuyor) return;

            zamanlayici.Stop();

            if (CEkranSecenekleri.ActiveYonet && kokYolu != null)
                surucu.Gonder(new List<string> { SahneSurucu.YolActiveKomutu(kokYolu, false) });

            CLog.Log("SERIT DURDU", ad);
        }

        /// <summary>
        /// HAZIRLA: siradaki sayfanin degerlerini sahneye yazar ama
        /// director'u TETIKLEMEZ.
        ///
        /// Serit ekranda degilken kutular dogru degerlerle doldurulmus
        /// olur; VER'e basildiginda giris animasyonu bos kutularla
        /// baslamaz. Donerken cagrilmasinin anlami yok, o yuzden
        /// yalnizca duruyorken calisiyor.
        /// </summary>
        public bool Hazirla()
        {
            if (Donuyor || sayfalar.Count == 0) return false;

            for (int deneme = 0; deneme < ATLAMA_SINIRI; deneme++)
            {
                int yer = (sira + 1 + deneme) % sayfalar.Count;
                CEkranSayfasi sayfa = sayfalar[yer];

                List<string> komutlar = new List<string>();
                if (!sayfa.Hazirla(DonusMotoru.Baglam, surucu, komutlar)) continue;

                DuzeniSec(sayfa, komutlar);

                if (!surucu.Gonder(komutlar)) return false;

                // Sira bir geri birakiliyor ki VER'e basildiginda ilk
                // oynatilan sayfa hazirlanan sayfa olsun.
                sira = yer - 1;

                CLog.Log("HAZIRLANDI", ad + " -> " + sayfa.Ad + " / " + DonusMotoru.Baglam.IlAdi);
                return true;
            }

            CLog.Error("HAZIRLANAMADI", ad + " - gosterilecek veri yok");
            return false;
        }

        /// <summary> Bekleme suresini sifirlayip hemen sonraki sayfaya gecer. </summary>
        public void Atla()
        {
            if (!Donuyor) return;

            // Timer'i durdurup baslatmak sayaci sifirliyor; yoksa yeni sayfa
            // kalan sureyi devraliyor ve bir anda geciyor.
            zamanlayici.Stop();
            SonrakiSayfa();
            zamanlayici.Start();
        }

        /// <summary>
        /// Ekrandaki sayfayi AYNI yerinde yeniden yazar.
        ///
        /// Il elle degistirildiginde bir sonraki donusu beklemeden
        /// goruntunun tazelenmesi icin. Sayfa ilerlemiyor: ilerletseydi
        /// ALT listesi basa dondugunde il imleci de kayar ve operatorun
        /// sectigi il elinden kacardi.
        ///
        /// Tazelenen sayfada artik veri yoksa normal akisa donup sonraki
        /// anlamli sayfaya geciliyor.
        /// </summary>
        public void Tazele()
        {
            if (!Donuyor || Sayfa == null) return;

            zamanlayici.Stop();

            if (!SayfayiVer(Sayfa)) SonrakiSayfa();

            zamanlayici.Start();
        }

        /// <summary>
        /// Sonraki anlamli sayfayi ekrana verir.
        ///
        /// Verisi olmayan sayfa BEKLEMEDEN atlaniyor; ekranda bos satirlarla
        /// dolu bir sayfanin 12 saniye durmasindansa hic acilmamasi iyi.
        /// </summary>
        private void SonrakiSayfa()
        {
            for (int deneme = 0; deneme < ATLAMA_SINIRI; deneme++)
            {
                sira++;

                if (sira >= sayfalar.Count)
                {
                    sira = 0;

                    // Tur tamamlandi: il imleci burada ilerliyor.
                    if (OnTurTamamlandi != null) OnTurTamamlandi(this);
                }

                if (SayfayiVer(sayfalar[sira])) return;
            }

            // Buraya dusmek "hicbir sayfada veri yok" demektir.
            zamanlayici.Stop();
            CLog.Error("SERIT DURDURULDU", ad + " - hicbir sayfada gosterilecek veri yok");
        }

        /// <summary>
        /// Bir sayfayi ekrana verir. Veri yoksa ya da director bulunamazsa
        /// false doner; cagiran sonraki sayfaya gecer.
        ///
        /// Metinler ve "#id START" TEK PAKETTE gidiyor - tersi olsaydi
        /// animasyon eski degerlerle baslar ve ekranda bir an yanlis sayi
        /// gorunurdu.
        /// </summary>
        private bool SayfayiVer(CEkranSayfasi sayfa)
        {
            List<string> komutlar = new List<string>();

            if (!sayfa.Hazirla(DonusMotoru.Baglam, surucu, komutlar))
            {
                CLog.Detail("SAYFA ATLANDI", ad + " / " + sayfa.Ad + " - veri yok");
                return false;
            }

            DuzeniSec(sayfa, komutlar);
            GirisiOynat(sayfa, komutlar);

            if (!surucu.YazVeOynat(komutlar, sayfa.DirectorYolu))
            {
                CLog.Error("SAYFA VERILEMEDI", ad + " / " + sayfa.Ad +
                           " - director bulunamadi: " + sayfa.DirectorYolu);
                return false;
            }

            // Duzen ve giris ancak komutlar gerceketen gittikten sonra
            // "acik" sayiliyor; yoksa basarisiz bir gonderimden sonra bir
            // daha acilmazlar.
            if (sayfa.DuzenYolu != null) acikDuzen = sayfa.DuzenYolu;
            if (sayfa.GirisDirectorYolu != null) oynatilanGiris = sayfa.GirisDirectorYolu;

            SureyiKur(sayfa);

            CLog.Log("SAYFA", ad + " -> " + sayfa.Ad + " / " + DonusMotoru.Baglam.IlAdi);

            if (OnSayfaDegisti != null) OnSayfaDegisti(this, sayfa);
            return true;
        }

        /// <summary>
        /// Duzen degistiyse o duzenin GIRIS animasyonunu da baslatir.
        ///
        /// Sahnede her duzenin ayri bir giris director'u var. Yalnizca
        /// sayfa director'u oynatilirsa duzen hic ekrana gelmiyor: serit
        /// gorunurde "tamamen kayboluyor". Ozellikle ittifak sayfalarina
        /// (UST_5 / UST_6) gecerken bu yasaniyordu.
        ///
        /// Giris her sayfada degil yalnizca duzen degisince oynatiliyor;
        /// yoksa serit her sayfa basinda yeniden ucup gelirdi.
        /// </summary>
        private void GirisiOynat(CEkranSayfasi sayfa, List<string> komutlar)
        {
            string giris = sayfa.GirisDirectorYolu;

            if (giris == null || giris == oynatilanGiris) return;

            int id = surucu.Indeks.Id(giris);
            if (id <= 0) return;

            komutlar.Add("#" + id + " START");
            CLog.Detail("GIRIS OYNATILDI", ad + " / " + giris);
        }

        /// <summary>
        /// Sayfanin duzenini acar, kardeslerini kapatir.
        ///
        /// UST'te dort duzen ust uste duruyor (UST_3/4/5/6); yanlislari
        /// kapatilmazsa hepsi ayni anda gorunur. SAYFA container'larina
        /// dokunulmuyor: onlar director'un isi.
        /// </summary>
        private void DuzeniSec(CEkranSayfasi sayfa, List<string> komutlar)
        {
            string yeni = sayfa.DuzenYolu;

            if (!CEkranSecenekleri.ActiveYonet || yeni == null) return;
            if (yeni == acikDuzen) return;

            // Ayni duzen birden fazla sayfada geciyor (UST_3'un dort sayfasi
            // gibi); kapatma komutu bir kere gonderilsin.
            List<string> kapatilan = new List<string>();

            foreach (CEkranSayfasi s in sayfalar)
            {
                string yol = s.DuzenYolu;
                if (yol == null || yol == yeni || kapatilan.Contains(yol)) continue;

                kapatilan.Add(yol);
                komutlar.Add(SahneSurucu.YolActiveKomutu(yol, false));
            }

            komutlar.Add(SahneSurucu.YolActiveKomutu(yeni, true));
        }
    }
}
