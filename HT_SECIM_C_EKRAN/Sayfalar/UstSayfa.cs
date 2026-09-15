using HT_SECIM_C_EKRAN.Core;
using HT_SECIM_C_EKRAN.Data;
using System.Collections.Generic;

namespace HT_SECIM_C_EKRAN.Sayfalar
{
    /// <summary>
    /// Ust seritteki parti sayfalari (UST_3 ve UST_4).
    ///
    /// Sahne agacindan olculen duzenler:
    ///
    ///   UST_3 : 4 sayfa x 6 satir. Sayfa 1-2'de yalnizca oran container'i
    ///           var, sayfa 3-4'te oranin yaninda MV container'i da var.
    ///   UST_4 : 2 sayfa x 5 satir. Her sayfada oran + MV birlikte.
    ///
    /// Sayfa, siralamanin KACINCI diliminden beslendigini kendisi biliyor:
    /// UST_3 sayfa 2 partilerin 7-12'sini gosterir. O dilimde parti
    /// kalmadiysa Hazirla false doner ve Serit sayfayi atlar - dokuz
    /// partilik bir secimde bos ikinci sayfa acilmaz.
    /// </summary>
    public class UstPartilerSayfasi : CEkranSayfasi
    {
        private readonly int duzen;
        private readonly int sayfa;
        private readonly int satirSayisi;
        private readonly int atla;
        private readonly bool vekilGoster;

        /// <param name="duzen">3 veya 4</param>
        /// <param name="sayfa">duzenin kacinci sayfasi</param>
        /// <param name="satirSayisi">o duzendeki satir sayisi (UST_3: 6, UST_4: 5)</param>
        /// <param name="atla">siralamada kac parti atlanacak (ikinci sayfa icin)</param>
        /// <param name="vekilGoster">MV container'i doldurulacak mi</param>
        public UstPartilerSayfasi(int duzen, int sayfa, int satirSayisi, int atla, bool vekilGoster)
        {
            this.duzen       = duzen;
            this.sayfa       = sayfa;
            this.satirSayisi = satirSayisi;
            this.atla        = atla;
            this.vekilGoster = vekilGoster;
        }

        public override string Ad
        {
            get { return "UST_" + duzen + " s" + sayfa + (vekilGoster ? " (vekil)" : " (oran)"); }
        }

        /// <summary> Ilk dilim birinci sayfa suresini, kalanlar ikinci sayfa suresini alir. </summary>
        public override int Sure
        {
            get { return atla == 0 ? CEkranSecenekleri.UstSure1 : CEkranSecenekleri.UstSure2; }
        }

        public override string DirectorYolu { get { return "UST_" + duzen + "/SAYFA" + sayfa; } }

        public override string GirisDirectorYolu { get { return "UST_" + duzen + "_GIRIS"; } }

        public override string DuzenYolu { get { return CEkranYollari.UstDuzeni(duzen); } }

        public override bool Hazirla(CEkranBaglam baglam, SahneSurucu surucu, List<string> komutlar)
        {
            List<Oy> oylar = DataService.SiraliOylar(baglam.SecimMV, baglam.Plaka);

            // Bu dilimde hic parti yoksa sayfanin acilmasinin anlami yok.
            if (oylar.Count <= atla) return false;

            IlSonucu sonuc = DataService.SonucBul(baglam.SecimMV, baglam.Plaka);

            Basligi(baglam, komutlar);

            SahneSurucu.BaslikOranKomutlari(komutlar,
                CEkranAdlari.UstAcilanSandik(duzen, 1),
                CEkranAdlari.UstAcilanSandik(duzen, 2),
                CEkranAdlari.UstAcilanSandikVirgul(duzen),
                sonuc == null ? 0 : sonuc.AcilanSandik, true);

            // Katilim oraninin ayiraci sahnede sabit (UST3_NOKTA_SABIT),
            // yazilacak bir virgul kutusu yok.
            SahneSurucu.BaslikOranKomutlari(komutlar,
                CEkranAdlari.UstKatilim(duzen, 1),
                CEkranAdlari.UstKatilim(duzen, 2),
                null,
                sonuc == null ? 0 : sonuc.Katilim, true);

            komutlar.Add(SahneSurucu.MetinKomutu(CEkranAdlari.UstKaynak(duzen),
                CEkranSecenekleri.KaynakGoster ? CEkranSecenekleri.KaynakAd : ""));

            // Son satir kalan partileri toplayacaksa parti satiri bir eksik.
            bool digerVar = CEkranSecenekleri.UstDigerSatiri && SonSayfaMi(oylar.Count);
            int partiSatiri = digerVar ? satirSayisi - 1 : satirSayisi;
            int gosterilen = 0;

            for (int sira = 1; sira <= partiSatiri; sira++)
            {
                int indeks = atla + sira - 1;
                bool dolu = indeks < oylar.Count;
                Oy oy = dolu ? oylar[indeks] : null;

                komutlar.Add(SahneSurucu.MetinKomutu(CEkranAdlari.UstAd(duzen, sayfa, sira),
                    dolu ? SahneSurucu.VeriMetni(DataService.OyAdi(baglam.SecimMV, oy.Kod)) : ""));

                SahneSurucu.OranKomutlari(komutlar,
                    CEkranAdlari.UstOran(duzen, sayfa, sira, 1),
                    CEkranAdlari.UstOran(duzen, sayfa, sira, 2),
                    dolu ? oy.Oran : 0);

                if (vekilGoster)
                    komutlar.Add(SahneSurucu.MetinKomutu(CEkranAdlari.UstMv(duzen, sayfa, sira),
                        dolu ? SahneSurucu.VeriMetni(oy.Vekil.ToString()) : ""));

                if (!dolu) continue;

                SahneSurucu.RenkEkle(komutlar, CEkranAdlari.UstRenk(duzen, sayfa, sira),
                    DataService.RenkKodu(baglam.SecimMV, oy.Kod));

                gosterilen += oy.Oran;
            }

            if (digerVar) DigerSatiri(komutlar, oylar, gosterilen);

            return true;
        }

        /// <summary>
        /// Baslik satiri: il adi, secim adi ve hangi kutularin acik olacagi.
        ///
        /// IL ADI HER SAYFADA YAZILMALI. Sahnede bu kutu yazilmazsa onceki
        /// yayindan kalan metin ekranda kalir: Ankara'nin rakamlari
        /// "TÜRKİYE GENELİ" basligiyla gorunur.
        ///
        /// Kutu gruplarinin ACTIVE bayraklari da her sayfada aciktan
        /// ayarlaniyor. Sahne bunlari karisik teslim edilmis (UST_3'te
        /// acilan sandik kapali, UST_4'te katilim kapali...), operatorun
        /// isaretledigi secenekle sahnenin hali tutmuyordu.
        /// </summary>
        private void Basligi(CEkranBaglam baglam, List<string> komutlar)
        {
            string ilKutusu    = CEkranYollari.UstBaslikIl(duzen);
            string secimKutusu = CEkranYollari.UstBaslikSecim(duzen);

            SahneSurucu.SigdirEkle(komutlar, ilKutusu, baglam.IlAdi,
                CEkranYollari.UST_BASLIK_OLCEK, CEkranYollari.UST_BASLIK_HARF);

            SahneSurucu.SigdirEkle(komutlar, secimKutusu,
                DataService.SecimBasligi(baglam.SecimMV),
                CEkranYollari.UST_BASLIK_OLCEK, CEkranYollari.UST_BASLIK_HARF);

            // Acilan sandik ve katilim kutulari AYNI YERDE duruyor; ikisi
            // birden acilirsa yazilar ust uste biniyor. O yuzden en fazla
            // biri aciliyor, hangisi oldugunu operator seciyor.
            bool assKutusu = (CEkranSecenekleri.UstBaslik == UstBaslikKutusu.AcilanSandik);
            bool koKutusu  = (CEkranSecenekleri.UstBaslik == UstBaslikKutusu.Katilim);

            komutlar.Add(SahneSurucu.ActiveKomutu("UST" + duzen + "_ASS", assKutusu));
            komutlar.Add(SahneSurucu.ActiveKomutu("UST" + duzen + "_KO", koKutusu));

            komutlar.Add(SahneSurucu.ActiveKomutu("UST" + duzen + "_KAYNAK",
                CEkranSecenekleri.KaynakGoster));
        }

        /// <summary>
        /// Bu sayfa siralamanin son dilimi mi.
        /// "DİĞER" yalnizca son sayfada anlamli: ilk sayfada kullanilirsa
        /// ikinci sayfada gosterilecek partiler zaten "diger"e sayilmis olur.
        /// </summary>
        private bool SonSayfaMi(int toplamParti)
        {
            if (!CEkranSecenekleri.UstIkinciSayfa) return true;

            return toplamParti <= atla + satirSayisi;
        }

        /// <summary>
        /// Kalan partileri tek satirda toplar.
        /// Oran, gosterilen partilerin toplamindan cikariliyor; veri eksik
        /// gelse bile ekrandaki satirlar %100'e tamamlanir.
        /// </summary>
        private void DigerSatiri(List<string> komutlar, List<Oy> oylar, int gosterilen)
        {
            int diger;

            if (atla == 0 && !CEkranSecenekleri.UstIkinciSayfa)
            {
                // Tek sayfa gosteriliyor: geri kalan her sey DIGER.
                diger = 10000 - gosterilen;
            }
            else
            {
                // Ekranda gorunen butun partiler (onceki sayfalar dahil)
                // toplamdan dusuluyor.
                int oncekiler = 0;
                for (int i = 0; i < atla && i < oylar.Count; i++) oncekiler += oylar[i].Oran;

                diger = 10000 - oncekiler - gosterilen;
            }

            if (diger < 0) diger = 0;

            komutlar.Add(SahneSurucu.MetinKomutu(CEkranAdlari.UstAd(duzen, sayfa, satirSayisi),
                SahneSurucu.VeriMetni("DİĞER")));

            SahneSurucu.OranKomutlari(komutlar,
                CEkranAdlari.UstOran(duzen, sayfa, satirSayisi, 1),
                CEkranAdlari.UstOran(duzen, sayfa, satirSayisi, 2), diger);

            SahneSurucu.RenkEkle(komutlar, CEkranAdlari.UstRenk(duzen, sayfa, satirSayisi),
                DataService.RenkKodu(null, "DIGER"));

            if (vekilGoster)
                komutlar.Add(SahneSurucu.MetinKomutu(
                    CEkranAdlari.UstMv(duzen, sayfa, satirSayisi), ""));
        }
    }

    /// <summary>
    /// Ust seritteki ittifak sayfalari (UST_5 ve UST_6).
    ///
    ///   UST_5 : 2 sayfa x 2 ittifak
    ///   UST_6 : 2 sayfa x 3 ittifak
    ///
    /// Bu duzenlerde katilim ve kaynak container'i yok, yalnizca acilan
    /// sandik orani var.
    /// </summary>
    public class UstIttifakSayfasi : CEkranSayfasi
    {
        private readonly int duzen;
        private readonly int sayfa;
        private readonly int satirSayisi;
        private readonly int atla;

        public UstIttifakSayfasi(int duzen, int sayfa, int satirSayisi, int atla)
        {
            this.duzen       = duzen;
            this.sayfa       = sayfa;
            this.satirSayisi = satirSayisi;
            this.atla        = atla;
        }

        public override string Ad { get { return "UST_" + duzen + " s" + sayfa + " ittifak"; } }

        public override int Sure
        {
            get { return atla == 0 ? CEkranSecenekleri.UstSure1 : CEkranSecenekleri.UstSure2; }
        }

        public override string DirectorYolu { get { return "UST_" + duzen + "/SAYFA" + sayfa; } }

        public override string GirisDirectorYolu { get { return "UST_" + duzen + "_GIRIS"; } }

        public override string DuzenYolu { get { return CEkranYollari.UstDuzeni(duzen); } }

        public override bool Hazirla(CEkranBaglam baglam, SahneSurucu surucu, List<string> komutlar)
        {
            List<string> ittifaklar = AnlamliIttifaklar(baglam);
            if (ittifaklar.Count <= atla) return false;

            IlSonucu sonuc = DataService.SonucBul(baglam.SecimMV, baglam.Plaka);

            // Bu duzendeki tek metin kutusu; il adi buraya yaziliyor.
            SahneSurucu.SigdirEkle(komutlar, CEkranYollari.IttifakBaslik(duzen),
                baglam.IlAdi,
                CEkranYollari.ITTIFAK_BASLIK_OLCEK,
                CEkranYollari.ITTIFAK_BASLIK_HARF);

            SahneSurucu.BaslikOranKomutlari(komutlar,
                CEkranAdlari.IttifakAcilanSandik(duzen, 1),
                CEkranAdlari.IttifakAcilanSandik(duzen, 2),
                CEkranAdlari.IttifakAcilanSandikVirgul(duzen),
                sonuc == null ? 0 : sonuc.AcilanSandik,
                CEkranSecenekleri.AcilanSandikGoster);

            for (int sira = 1; sira <= satirSayisi; sira++)
            {
                int indeks = atla + sira - 1;
                bool dolu = indeks < ittifaklar.Count;
                string kod = dolu ? ittifaklar[indeks] : null;

                komutlar.Add(SahneSurucu.MetinKomutu(CEkranAdlari.IttifakAd(duzen, sayfa, sira),
                    dolu ? SahneSurucu.VeriMetni(DataService.IttifakAdi(kod)) : ""));

                SahneSurucu.OranKomutlari(komutlar,
                    CEkranAdlari.IttifakOran(duzen, sayfa, sira, 1),
                    CEkranAdlari.IttifakOran(duzen, sayfa, sira, 2),
                    dolu ? DataService.IttifakOrani(baglam.SecimMV, baglam.Plaka, kod) : 0);

                komutlar.Add(SahneSurucu.MetinKomutu(CEkranAdlari.IttifakMv(duzen, sayfa, sira),
                    dolu
                        ? SahneSurucu.VeriMetni(
                            DataService.IttifakVekil(baglam.SecimMV, baglam.Plaka, kod).ToString())
                        : ""));

                // Isim plakasinin rengi ittifaktan geliyor. Bos satirin
                // plakasina dokunulmuyor; zaten adi da orani da bos.
                if (!dolu) continue;

                string plaka = CEkranGorselleri.IttifakPlakasi(kod, duzen);

                if (plaka != null)
                {
                    string komut = surucu.GorselKomutu(
                        CEkranAdlari.IttifakPlaka(duzen, sayfa, sira), plaka);

                    if (komut != null) komutlar.Add(komut);
                }
            }

            return true;
        }

        /// <summary>
        /// Bu ilde gercekten oy almis ittifaklar, buyukten kucuge.
        /// Ittifaksiz partilerin bos kodu listeye girmiyor.
        /// </summary>
        private static List<string> AnlamliIttifaklar(CEkranBaglam baglam)
        {
            List<string> sonuc = new List<string>();

            foreach (string kod in DataService.Ittifaklar())
            {
                if (string.IsNullOrEmpty(kod)) continue;
                if (DataService.IttifakOrani(baglam.SecimMV, baglam.Plaka, kod) <= 0) continue;

                sonuc.Add(kod);
            }

            sonuc.Sort(delegate(string a, string b)
            {
                return DataService.IttifakOrani(baglam.SecimMV, baglam.Plaka, b)
                       .CompareTo(DataService.IttifakOrani(baglam.SecimMV, baglam.Plaka, a));
            });

            return sonuc;
        }
    }
}
