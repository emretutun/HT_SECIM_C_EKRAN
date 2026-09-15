using HT_SECIM_C_EKRAN.Core;
using HT_SECIM_C_EKRAN.Data;
using System.Collections.Generic;

namespace HT_SECIM_C_EKRAN.Sayfalar
{
    /// <summary>
    /// Alt serit, sayfa 1: cumhurbaskani adaylari.
    ///
    /// Sahnede DORT satir var. 2023'te dort aday vardi ama Muharrem Ince
    /// cekildigi icin dorduncu satirin gozu kapatilmisti; biz satiri
    /// kullaniyoruz, aday yoksa satir bos kaliyor.
    ///
    /// Il adi YALNIZCA bu sayfada var: sahnede ALT_1_SAYFA2_IL_AD diye bir
    /// container yok, ikinci sayfada il yazmiyor.
    /// </summary>
    public class AltAdaylarSayfasi : CEkranSayfasi
    {
        private const int SAYFA = 1;
        private const int SATIR = 4;

        public override string Ad { get { return "ALT adaylar"; } }

        public override int Sure { get { return CEkranSecenekleri.AltSure; } }

        public override string DirectorYolu { get { return "ALT/SAYFA" + SAYFA; } }

        public override string GirisDirectorYolu { get { return "ALT/ALT_GIRIS"; } }

        public override bool Hazirla(CEkranBaglam baglam, SahneSurucu surucu, List<string> komutlar)
        {
            List<Oy> oylar = DataService.SiraliOylar(baglam.SecimCB, baglam.Plaka);
            if (oylar.Count == 0) return false;

            IlSonucu sonuc = DataService.SonucBul(baglam.SecimCB, baglam.Plaka);

            SahneSurucu.SigdirEkle(komutlar, CEkranYollari.ALT_IL_KUTUSU, baglam.IlAdi,
                CEkranYollari.ALT_IL_OLCEK, CEkranYollari.ALT_IL_HARF);

            SahneSurucu.BaslikOranKomutlari(komutlar,
                CEkranAdlari.AltAcilanSandik(SAYFA, 1),
                CEkranAdlari.AltAcilanSandik(SAYFA, 2),
                CEkranAdlari.AltAcilanSandikVirgul(SAYFA),
                sonuc == null ? 0 : sonuc.AcilanSandik,
                CEkranSecenekleri.AcilanSandikGoster);

            // Kutunun kendisi de aciliyor/kapaniyor: sahne bu bayraklari
            // onceki yayindan kalma halleriyle teslim edilmis, operatorun
            // isaretiyle tutmuyorlardi.
            komutlar.Add(SahneSurucu.YolActiveKomutu(CEkranYollari.AltAssGrubu(SAYFA),
                CEkranSecenekleri.AcilanSandikGoster));

            for (int sira = 1; sira <= SATIR; sira++)
            {
                // Aday sayisi satir sayisindan az olabilir; kalan satirlar
                // bosaltiliyor, yoksa onceki ilin adayi ekranda kaliyor.
                bool dolu = sira <= oylar.Count;
                Oy oy = dolu ? oylar[sira - 1] : null;

                komutlar.Add(SahneSurucu.MetinKomutu(CEkranAdlari.AltAd(SAYFA, sira),
                    dolu ? SahneSurucu.VeriMetni(DataService.OyAdi(baglam.SecimCB, oy.Kod)) : ""));

                SahneSurucu.OranKomutlari(komutlar,
                    CEkranAdlari.AltOran(SAYFA, sira, 1),
                    CEkranAdlari.AltOran(SAYFA, sira, 2),
                    dolu ? oy.Oran : 0);

                // Satirin renk seridi. Bos satirin rengine dokunulmuyor;
                // zaten adi da orani da bos, serit gorunmuyor.
                if (dolu)
                    SahneSurucu.RenkEkle(komutlar, CEkranAdlari.AltRenk(SAYFA, sira),
                        DataService.RenkKodu(baglam.SecimCB, oy.Kod));
            }

            return true;
        }
    }

    /// <summary>
    /// Alt serit, sayfa 2: en cok oy alan partiler.
    ///
    /// Sahnede alti satir var. Son satir, secenek acikken kalan butun
    /// partileri "DİĞER" olarak topluyor; kapaliysa alti parti birden
    /// gosteriliyor.
    ///
    /// DIGER orani ilk besin toplamindan cikariliyor, tek tek toplanmiyor:
    /// veri eksik gelse bile ekrandaki alti satir %100'e tamamlanir.
    /// </summary>
    public class AltPartilerSayfasi : CEkranSayfasi
    {
        private const int SAYFA = 2;
        private const int SATIR = 6;

        public override string Ad { get { return "ALT partiler"; } }

        public override int Sure { get { return CEkranSecenekleri.AltSure; } }

        public override string DirectorYolu { get { return "ALT/SAYFA" + SAYFA; } }

        public override string GirisDirectorYolu { get { return "ALT/ALT_GIRIS"; } }

        public override bool Hazirla(CEkranBaglam baglam, SahneSurucu surucu, List<string> komutlar)
        {
            List<Oy> oylar = DataService.SiraliOylar(baglam.SecimMV, baglam.Plaka);
            if (oylar.Count == 0) return false;

            IlSonucu sonuc = DataService.SonucBul(baglam.SecimMV, baglam.Plaka);

            SahneSurucu.BaslikOranKomutlari(komutlar,
                CEkranAdlari.AltAcilanSandik(SAYFA, 1),
                CEkranAdlari.AltAcilanSandik(SAYFA, 2),
                CEkranAdlari.AltAcilanSandikVirgul(SAYFA),
                sonuc == null ? 0 : sonuc.AcilanSandik,
                CEkranSecenekleri.AcilanSandikGoster);

            // Kutunun kendisi de aciliyor/kapaniyor: sahne bu bayraklari
            // onceki yayindan kalma halleriyle teslim edilmis, operatorun
            // isaretiyle tutmuyorlardi.
            komutlar.Add(SahneSurucu.YolActiveKomutu(CEkranYollari.AltAssGrubu(SAYFA),
                CEkranSecenekleri.AcilanSandikGoster));

            bool digerVar = CEkranSecenekleri.UstDigerSatiri;
            int  partiSatiri = digerVar ? SATIR - 1 : SATIR;
            int  gosterilen = 0;

            for (int sira = 1; sira <= partiSatiri; sira++)
            {
                bool dolu = sira <= oylar.Count;
                Oy oy = dolu ? oylar[sira - 1] : null;

                komutlar.Add(SahneSurucu.MetinKomutu(CEkranAdlari.AltAd(SAYFA, sira),
                    dolu ? SahneSurucu.VeriMetni(DataService.OyAdi(baglam.SecimMV, oy.Kod)) : ""));

                SahneSurucu.OranKomutlari(komutlar,
                    CEkranAdlari.AltOran(SAYFA, sira, 1),
                    CEkranAdlari.AltOran(SAYFA, sira, 2),
                    dolu ? oy.Oran : 0);

                if (!dolu) continue;

                SahneSurucu.RenkEkle(komutlar, CEkranAdlari.AltRenk(SAYFA, sira),
                    DataService.RenkKodu(baglam.SecimMV, oy.Kod));

                gosterilen += oy.Oran;
            }

            if (!digerVar) return true;

            // Yuvarlama artiklari yuzunden eksiye dusebilir; sifira cekiliyor.
            int diger = 10000 - gosterilen;
            if (diger < 0) diger = 0;

            komutlar.Add(SahneSurucu.MetinKomutu(CEkranAdlari.AltAd(SAYFA, SATIR),
                SahneSurucu.VeriMetni("DİĞER")));

            SahneSurucu.OranKomutlari(komutlar,
                CEkranAdlari.AltOran(SAYFA, SATIR, 1),
                CEkranAdlari.AltOran(SAYFA, SATIR, 2), diger);

            SahneSurucu.RenkEkle(komutlar, CEkranAdlari.AltRenk(SAYFA, SATIR),
                DataService.RenkKodu(baglam.SecimMV, "DIGER"));

            return true;
        }
    }
}
