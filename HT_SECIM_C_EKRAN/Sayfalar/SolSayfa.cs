using HT_SECIM_C_EKRAN.Core;
using HT_SECIM_C_EKRAN.Data;
using System.Collections.Generic;

namespace HT_SECIM_C_EKRAN.Sayfalar
{
    /// <summary>
    /// Sol serit: cumhurbaskani adaylari.
    ///
    /// Iki duzen var, ikisi de kullanilabiliyor:
    ///
    ///   SOL_1 : sayfa basina iki aday, adin altinda alinan oy sayisi da var
    ///   SOL_2 : sayfa basina uc aday, alinan oy container'i YOK
    ///
    /// SOL_1'in sayfa sirasi bilerek ters: once ucuncu ve dorduncu aday,
    /// sonra lider ikili. Sahne boyle tasarlanmis, izleyici sonuca dogru
    /// yukseliyor.
    ///
    /// Sol serit HER ZAMAN Turkiye geneli gosterir; baglamdaki plakaya
    /// bakmaz. Alt ve ust seritler iller arasinda donerken solda genel
    /// tablonun sabit kalmasi istendi.
    /// </summary>
    public class SolAdaylarSayfasi : CEkranSayfasi
    {
        /// <summary> Sol serit her zaman Turkiye geneli. </summary>
        private const int PLAKA = 0;

        private readonly int duzen;
        private readonly int sayfa;
        private readonly int adaySayisi;
        private readonly int atla;

        /// <param name="duzen">1 = SOL_1 (iki aday), 2 = SOL_2 (uc aday)</param>
        /// <param name="sayfa">duzenin kacinci sayfasi</param>
        /// <param name="adaySayisi">sayfadaki aday kutusu sayisi</param>
        /// <param name="atla">siralamada kac aday atlanacak</param>
        public SolAdaylarSayfasi(int duzen, int sayfa, int adaySayisi, int atla)
        {
            this.duzen      = duzen;
            this.sayfa      = sayfa;
            this.adaySayisi = adaySayisi;
            this.atla       = atla;
        }

        public override string Ad { get { return "SOL_" + duzen + " s" + sayfa; } }

        public override int Sure
        {
            get { return atla == 0 ? CEkranSecenekleri.SolSure1 : CEkranSecenekleri.SolSure2; }
        }

        public override string DirectorYolu { get { return "SOL_" + duzen + "/SAYFA" + sayfa; } }

        public override string GirisDirectorYolu { get { return "SOL_" + duzen + "_GIRIS"; } }

        public override string DuzenYolu { get { return CEkranYollari.SolDuzeni(duzen); } }

        public override bool Hazirla(CEkranBaglam baglam, SahneSurucu surucu, List<string> komutlar)
        {
            List<Oy> oylar = DataService.SiraliOylar(baglam.SecimCB, PLAKA);
            if (oylar.Count <= atla) return false;

            IlSonucu sonuc = DataService.SonucBul(baglam.SecimCB, PLAKA);

            komutlar.Add(SahneSurucu.MetinKomutu(CEkranAdlari.SOL_BASLIK,
                DataService.SecimBasligiKisa(baglam.SecimCB) + "\nTÜRKİYE GENELİ"));

            SahneSurucu.BaslikOranKomutlari(komutlar,
                CEkranAdlari.SolAcilanSandik(1),
                CEkranAdlari.SolAcilanSandik(2),
                CEkranAdlari.SOL_ASS_VIRGUL,
                sonuc == null ? 0 : sonuc.AcilanSandik,
                CEkranSecenekleri.AcilanSandikGoster);

            komutlar.Add(SahneSurucu.ActiveKomutu("SOL_ASS",
                CEkranSecenekleri.AcilanSandikGoster));

            for (int aday = 1; aday <= adaySayisi; aday++)
            {
                int indeks = atla + aday - 1;
                bool dolu = indeks < oylar.Count;
                Oy oy = dolu ? oylar[indeks] : null;

                komutlar.Add(SahneSurucu.MetinKomutu(CEkranAdlari.SolAd(duzen, sayfa, aday),
                    dolu ? SahneSurucu.VeriMetni(DataService.OyAdi(baglam.SecimCB, oy.Kod)) : ""));

                SahneSurucu.OranKomutlari(komutlar,
                    CEkranAdlari.SolOran(duzen, sayfa, aday, 1),
                    CEkranAdlari.SolOran(duzen, sayfa, aday, 2),
                    dolu ? oy.Oran : 0);

                // Alinan oy kutusu yalnizca SOL_1 duzeninde var.
                if (duzen == 1)
                    komutlar.Add(SahneSurucu.MetinKomutu(CEkranAdlari.SolAlinanOy(duzen, sayfa, aday),
                        dolu ? SahneSurucu.VeriMetni(CEkranAdlari.OySayisi(oy.OySayisi)) : ""));

                FotoYaz(surucu, komutlar, aday, dolu ? oy.Kod : null);
                PlakaYaz(surucu, komutlar, aday, dolu ? oy.Kod : null);
            }

            return true;
        }

        /// <summary>
        /// Aday fotografi.
        ///
        /// Sahnede 2023 adaylarinin fotograflari gomulu duruyor; veriden
        /// gelen aday degisince fotograf da degismeli, yoksa isim bir
        /// adaya fotograf baskasina ait olur. Gorsel yolu UUID'ye
        /// cevriliyor: havuzda ayni isim birden fazla klasorde geciyor.
        ///
        /// Adayin vizImage'i tanimli degilse container'a DOKUNULMUYOR -
        /// sahnedeki fotograf kalir, bos kutu gorunmez.
        /// </summary>
        /// <summary>
        /// Adin altindaki renkli plaka.
        ///
        /// Ittifak plakalari gibi bu da boyanabilen bir materyal degil,
        /// aday basina hazirlanmis bir gorsel. Eslesme "gorseller"
        /// dosyasindan geliyor.
        /// </summary>
        private void PlakaYaz(SahneSurucu surucu, List<string> komutlar, int aday, string kod)
        {
            if (kod == null) return;

            string plaka = CEkranGorselleri.AdayPlakasi(kod);
            if (plaka == null) return;

            string komut = surucu.GorselKomutu(
                CEkranAdlari.SolPlaka(duzen, sayfa, aday), plaka);

            if (komut != null) komutlar.Add(komut);
        }

        private void FotoYaz(SahneSurucu surucu, List<string> komutlar, int aday, string kod)
        {
            if (kod == null) return;

            Aday kayit = DataService.AdayBul(kod);
            if (kayit == null || string.IsNullOrEmpty(kayit.VizImage)) return;

            string komut = surucu.GorselKomutu(
                CEkranAdlari.SolFoto(duzen, sayfa, aday), kayit.VizImage);

            if (komut != null) komutlar.Add(komut);
        }
    }
}
