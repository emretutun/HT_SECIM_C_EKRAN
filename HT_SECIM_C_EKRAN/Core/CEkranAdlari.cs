using System.Globalization;

namespace HT_SECIM_C_EKRAN.Core
{
    /// <summary>
    /// Sahnedeki container adlari.
    ///
    /// Adlandirma duzenli ama UC AYRI KALIP var, karistirmamak lazim:
    ///
    ///   ALT    ->  ALT_1_SIRA{sira}_SAYFA{sayfa}_...     (once SIRA, sonra SAYFA)
    ///   UST    ->  UST_3_SAYFA{sayfa}_SIRA{sira}_...     (once SAYFA, sonra SIRA)
    ///   SOL    ->  SOL_1_SAYFA{sayfa}_ADAY{no}_...
    ///
    /// Baslik alanlarinda da alt cizgi tutarsiz: UST_3'un satirlari alt
    /// cizgili (UST_3_SAYFA1_...) ama basligi cizgisiz (UST3_ASS_SAYI1).
    /// Sahne boyle teslim edildi; adlari burada topladim ki kalan kod
    /// bu tutarsizlikla ugrasmasin.
    ///
    /// Oranlar ikiye bolunmus: ORAN1 tam kisim, ORAN2 tek ondalik hane.
    /// Virgul ve % isareti sahnede sabit container'larda duruyor, onlara
    /// dokunmuyoruz.
    /// </summary>
    public static class CEkranAdlari
    {
        // ------------------------------------------------------------ ALT

        /// <summary> Alt seritte gosterilen il adi. </summary>
        public static string AltIl(int sayfa)
        {
            return "ALT_1_SAYFA" + sayfa + "_IL_AD";
        }

        /// <summary> Alt seritteki acilan sandik orani. parca: 1 tam, 2 ondalik. </summary>
        public static string AltAcilanSandik(int sayfa, int parca)
        {
            return "ALT_1_SAYFA" + sayfa + "_ASS_ORAN" + parca;
        }

        public static string AltAcilanSandikVirgul(int sayfa)
        {
            return "ALT_1_SAYFA" + sayfa + "_ASS_VIRGUL";
        }

        /// <summary> Satirdaki ad. Sayfa 1'de aday, sayfa 2'de parti adi giriyor. </summary>
        public static string AltAd(int sayfa, int sira)
        {
            return "ALT_1_SIRA" + sira + "_SAYFA" + sayfa + "_PARTI_AD";
        }

        public static string AltOran(int sayfa, int sira, int parca)
        {
            return "ALT_1_SIRA" + sira + "_SAYFA" + sayfa + "_ORAN" + parca;
        }

        /// <summary> Satirin renk seridi. Her satirin kendi materyali var. </summary>
        public static string AltRenk(int sayfa, int sira)
        {
            return "ALT_1_SIRA" + sira + "_SAYFA" + sayfa + "_RENK";
        }

        // ------------------------------------------------------- UST 3 / 4

        /// <summary> UST_3 ve UST_4'te satirdaki parti adi. </summary>
        public static string UstAd(int duzen, int sayfa, int sira)
        {
            return "UST_" + duzen + "_SAYFA" + sayfa + "_SIRA" + sira + "_PARTI_AD";
        }

        public static string UstOran(int duzen, int sayfa, int sira, int parca)
        {
            return "UST_" + duzen + "_SAYFA" + sayfa + "_SIRA" + sira + "_ORAN" + parca;
        }

        /// <summary> Satirin renk seridi. Her satirin kendi materyali var. </summary>
        public static string UstRenk(int duzen, int sayfa, int sira)
        {
            return "UST_" + duzen + "_SAYFA" + sayfa + "_SIRA" + sira + "_RENK";
        }

        /// <summary>
        /// Milletvekili sayisi.
        /// UST_3'te yalnizca sayfa 3 ve 4'te var (sayfa 1-2 oran gosteriyor),
        /// UST_4'te her sayfada oranla birlikte duruyor.
        /// </summary>
        public static string UstMv(int duzen, int sayfa, int sira)
        {
            return "UST_" + duzen + "_SAYFA" + sayfa + "_SIRA" + sira + "_MV";
        }

        /// <summary> UST_3 / UST_4 baslik alanlari - burada alt cizgi yok: UST3_ASS_SAYI1 </summary>
        public static string UstAcilanSandik(int duzen, int parca)
        {
            return "UST" + duzen + "_ASS_SAYI" + parca;
        }

        public static string UstAcilanSandikVirgul(int duzen)
        {
            return "UST" + duzen + "_ASS_VIRGUL";
        }

        public static string UstKatilim(int duzen, int parca)
        {
            return "UST" + duzen + "_KO_SAYI" + parca;
        }

        public static string UstKaynak(int duzen)
        {
            return "UST" + duzen + "_KAYNAK_AD";
        }

        // ------------------------------------------------------- UST 5 / 6

        /// <summary> Ittifak adi. Sahnede iki satir tasarlanmis. </summary>
        public static string IttifakAd(int duzen, int sayfa, int sira)
        {
            return "UST_" + duzen + "_SAYFA" + sayfa + "_SIRA" + sira + "_AD";
        }

        public static string IttifakOran(int duzen, int sayfa, int sira, int parca)
        {
            return "UST_" + duzen + "_SAYFA" + sayfa + "_SIRA" + sira + "_ORAN" + parca;
        }

        public static string IttifakMv(int duzen, int sayfa, int sira)
        {
            return "UST_" + duzen + "_SAYFA" + sayfa + "_SIRA" + sira + "_MV_SAYI";
        }

        /// <summary>
        /// Ittifak adinin arkasindaki renkli plaka.
        /// Boyanabilen bir materyali yok, ittifaka gore GORSEL giydiriliyor.
        /// </summary>
        public static string IttifakPlaka(int duzen, int sayfa, int sira)
        {
            return "UST_" + duzen + "_SAYFA" + sayfa + "_SIRA" + sira + "_ISIM_BACK";
        }

        /// <summary> UST_5 / UST_6 basliginda acilan sandik - burada alt cizgi VAR. </summary>
        public static string IttifakAcilanSandik(int duzen, int parca)
        {
            return "UST_" + duzen + "_ASS_ORAN" + parca;
        }

        public static string IttifakAcilanSandikVirgul(int duzen)
        {
            return "UST_" + duzen + "_ASS_VIRGUL";
        }

        // ------------------------------------------------------------ SOL

        public static string SolAd(int duzen, int sayfa, int aday)
        {
            return "SOL_" + duzen + "_SAYFA" + sayfa + "_ADAY" + aday + "_AD";
        }

        public static string SolFoto(int duzen, int sayfa, int aday)
        {
            return "SOL_" + duzen + "_SAYFA" + sayfa + "_ADAY" + aday + "_FOTO";
        }

        public static string SolOran(int duzen, int sayfa, int aday, int parca)
        {
            return "SOL_" + duzen + "_SAYFA" + sayfa + "_ADAY" + aday + "_ORAN" + parca;
        }

        /// <summary>
        /// Aday adinin arkasindaki renkli plaka.
        /// Ittifak plakasi gibi, boyanmiyor - aday basina gorsel giydiriliyor.
        /// </summary>
        public static string SolPlaka(int duzen, int sayfa, int aday)
        {
            return "SOL_" + duzen + "_SAYFA" + sayfa + "_ADAY" + aday + "_BACK";
        }

        /// <summary> Adayin aldigi oy sayisi. Yalnizca SOL_1 duzeninde var. </summary>
        public static string SolAlinanOy(int duzen, int sayfa, int aday)
        {
            return "SOL_" + duzen + "_SAYFA" + sayfa + "_ADAY" + aday + "_ALINAN_OY";
        }

        /// <summary> Sol basliktaki acilan sandik orani. </summary>
        public static string SolAcilanSandik(int parca)
        {
            return "SOL_ASS_SAYI" + parca;
        }

        public const string SOL_ASS_VIRGUL = "SOL_ASS_VIRGUL";

        /// <summary> Sol ustteki iki satirlik baslik. </summary>
        public const string SOL_BASLIK = "BASLIK_YAZI";

        // ------------------------------------------------------ tek alanlar

        /// <summary> Donen kupteki yazi. Dort yuz de ayni metni gosteriyor. </summary>
        public static readonly string[] KUP_YUZLERI =
        {
            "4/4/1/1/1/1",   // YUZ_1  Canli_normal
            "4/4/2/1/1/1",   // YUZ_2  Lokasyon_normal
            "4/4/3/1/1/1",   // YUZ_3  Canli_normal
            "4/4/4/1/1/1"    // YUZ_4  Lokasyon_normal
        };

        /// <summary> Twitter kutusundaki yazi. Sayisal yolla adresleniyor. </summary>
        public const string TWITTER_YOLU = "5/1/2/1/1/1";

        // ---------------------------------------------------------- yardim

        /// <summary>
        /// Yuzde x100 degerini sahnenin bekledigi iki parcaya boler.
        /// 2874 (yani %28,74) -> tam "28", ondalik "7"
        ///
        /// Sahnede tek ondalik hane var; once bir haneye yuvarlaniyor ki
        /// 28,75 -> 28,8 olsun, 28,7 degil.
        /// </summary>
        public static void OranParcala(int oranX100, bool ciftHane, out string tam, out string ondalik)
        {
            if (ciftHane)
            {
                // Veri zaten x100 tutuluyor, yuvarlamaya gerek yok.
                tam     = (oranX100 / 100).ToString(CultureInfo.InvariantCulture);
                ondalik = System.Math.Abs(oranX100 % 100).ToString("00", CultureInfo.InvariantCulture);
                return;
            }

            int yuvarlanmis = (int)System.Math.Round(oranX100 / 10.0, System.MidpointRounding.AwayFromZero);

            tam     = (yuvarlanmis / 10).ToString(CultureInfo.InvariantCulture);
            ondalik = (yuvarlanmis % 10).ToString(CultureInfo.InvariantCulture);
        }

        /// <summary> 11945807 -> "11.945.807" (sahnede binlik ayraci nokta). </summary>
        public static string OySayisi(long oy)
        {
            return oy.ToString("#,##0", CultureInfo.GetCultureInfo("tr-TR")).Replace(',', '.');
        }
    }
}
