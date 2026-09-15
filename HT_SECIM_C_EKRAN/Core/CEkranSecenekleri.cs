using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace HT_SECIM_C_EKRAN.Core
{
    /// <summary> UST seridinde parti satirlarinda ne gosterilecek. </summary>
    public enum UstGosterim
    {
        /// <summary> UST_3: alti satir, yalnizca oy orani. </summary>
        OyOrani,

        /// <summary> UST_4: bes satir, oran ve milletvekili birlikte. </summary>
        MvVeOran
    }

    /// <summary> UST seridindeki ittifak sayfalari. </summary>
    public enum IttifakModu
    {
        /// <summary> UST_5 sayfa 1: ilk iki ittifak. </summary>
        Ilk2,

        /// <summary> UST_5 iki sayfa: 1-2, sonra 3-4. </summary>
        IkiArtiIki,

        /// <summary> UST_6 sayfa 1: ilk uc ittifak. </summary>
        Ilk3,

        /// <summary> UST_5 sayfa 1 (iki ittifak), ardindan UST_6 sayfa 1 (uc ittifak). </summary>
        IkiArtiUc
    }

    /// <summary>
    /// UST_3 / UST_4 basligindaki tek kutuda ne gosterilecek.
    ///
    /// SAHNE TUZAGI: acilan sandik ve katilim kutulari AYNI YERDE duruyor,
    /// ikisi birden acilinca yazilar ust uste biniyor. Sahne de bunlari
    /// birbirini disliyacak sekilde teslim edilmis (UST_3'te katilim acik,
    /// UST_4'te acilan sandik acik). O yuzden secim tek: ya biri, ya digeri.
    /// </summary>
    public enum UstBaslikKutusu
    {
        AcilanSandik,
        Katilim,
        Yok
    }

    /// <summary> SOL seridinde kac aday, hangi duzende. </summary>
    public enum SolModu
    {
        /// <summary> SOL_1 sayfa 2: yalnizca lider ikili. </summary>
        Ilk2Aday,

        /// <summary> SOL_2 sayfa 1: uc aday yan yana. </summary>
        Ilk3Aday,

        /// <summary> SOL_1 iki sayfa: once 3.-4. aday, sonra lider ikili. </summary>
        IkiArtiIki
    }

    /// <summary>
    /// C ekraninin butun calisma secenekleri.
    ///
    /// IKI DOSYADAN besleniyor:
    ///
    ///   donus      - elle duzenlenen, aciklamali varsayilanlar dosyasi.
    ///                Uygulama buraya HIC YAZMAZ, aciklamalar korunur.
    ///   son_ayar   - operatorun arayuzde yaptigi son secimler.
    ///                Uygulama kapanirken yazar, acilirken donus'un
    ///                uzerine okur. Elle duzenlemek icin degil.
    ///
    /// Boylece yayin sorumlusu varsayilanlari aciklamalariyla birlikte
    /// ayarlayabiliyor, operatorun gece yaptigi degisiklikler de kaybolmuyor.
    /// </summary>
    public static class CEkranSecenekleri
    {
        #region UST

        public static bool UstAktif;

        /// <summary> Ilk sayfanin (ve MV sayfasinin ilk sayfasinin) ekran suresi, saniye. </summary>
        public static int UstSure1;

        /// <summary> Ikinci sayfanin ekran suresi, saniye. </summary>
        public static int UstSure2;

        /// <summary> Siralamada ilk diliminin disinda kalan partiler de gosterilsin mi. </summary>
        public static bool UstIkinciSayfa;

        /// <summary> UST_3'un son satiri "DİĞER" olarak toplansin mi. </summary>
        public static bool UstDigerSatiri;

        public static UstGosterim UstMod;

        /// <summary> UST_3 / UST_4 basligindaki tek kutuda ne gorunecek. </summary>
        public static UstBaslikKutusu UstBaslik;

        /// <summary> Oranin yaninda ayrica milletvekili sayfasi da donsun mu (UST_3 sayfa 3-4). </summary>
        public static bool UstMvSayisi;

        public static bool IttifakAktif;
        public static IttifakModu Ittifak;

        /// <summary> UST_3 / UST_4 basligindaki kaynak yazisi gorunsun mu. </summary>
        public static bool KaynakGoster;

        #endregion

        #region SOL

        public static bool SolAktif;

        /// <summary> Oran tek ondalik hane yerine iki hane yazilsin mi (%52,4 -> %52,43). </summary>
        public static bool CiftHane;

        public static SolModu Sol;

        public static int SolSure1;
        public static int SolSure2;

        #endregion

        #region ALT

        public static bool AltAktif;

        /// <summary> Alt seritte sayfa degistirme suresi, saniye. </summary>
        public static int AltSure;

        #endregion

        #region Genel

        /// <summary>
        /// Secim yasagi.
        ///
        /// Yasak suresince aday ve parti adlari ile butun oranlar bos
        /// birakilir; sahne ekranda kalir ama sayi gostermez. Acilan
        /// sandik orani ayri bir secenekle yonetiliyor, cunku yasak
        /// sirasinda genellikle o gosterilebiliyor.
        /// </summary>
        public static bool YasakModu;

        /// <summary> Acilan sandik orani yazilsin mi. </summary>
        public static bool AcilanSandikGoster;

        /// <summary> Baslikta gorunecek kaynak adi (ANADOLU AJANSI gibi). </summary>
        public static string KaynakAd;

        public static string SecimCB;
        public static string SecimMV;

        /// <summary> Baglanti kurulup sahne okunur okunmaz donus baslasin mi. </summary>
        public static bool OtomatikBasla;

        /// <summary> Serit kokleri ve UST duzeni ACTIVE bayragini uygulama yonetsin mi. </summary>
        public static bool ActiveYonet;

        #endregion

        /// <summary>
        /// Once aciklamali varsayilanlar, sonra operatorun son secimleri.
        /// Sira onemli: son_ayar donus'un uzerine yaziyor.
        /// </summary>
        public static void Load()
        {
            Varsayilanlar();

            Oku(ConfigPaths.DonusFile);
            Oku(ConfigPaths.SonAyarFile);

            CLog.Log("SECENEKLER",
                "UST " + UstSure1 + "/" + UstSure2 + "sn " + UstMod +
                " / SOL " + Sol + " " + SolSure1 + "/" + SolSure2 + "sn" +
                " / ALT " + AltSure + "sn" +
                (YasakModu ? " / YASAK MODU" : ""));
        }

        private static void Varsayilanlar()
        {
            UstAktif       = true;
            UstSure1       = 3;
            UstSure2       = 5;
            UstIkinciSayfa = true;
            UstDigerSatiri = true;
            UstMod         = UstGosterim.OyOrani;
            UstBaslik      = UstBaslikKutusu.AcilanSandik;
            UstMvSayisi    = false;
            IttifakAktif   = true;
            Ittifak        = IttifakModu.IkiArtiUc;
            KaynakGoster   = false;

            SolAktif  = true;
            CiftHane  = false;
            Sol       = SolModu.IkiArtiIki;
            SolSure1  = 10;
            SolSure2  = 5;

            AltAktif = true;
            AltSure  = 5;

            YasakModu          = false;
            AcilanSandikGoster = true;
            KaynakAd           = "";
            SecimCB            = "";
            SecimMV            = "";
            OtomatikBasla      = false;
            ActiveYonet        = true;
        }

        private static void Oku(string dosya)
        {
            foreach (string[] p in ConfigReader.ReadLines(dosya))
            {
                if (p.Length < 2) continue;

                string anahtar = p[0].Trim().ToUpperInvariant();
                string deger   = p[1].Trim();

                switch (anahtar)
                {
                    case "UST_AKTIF":        UstAktif       = Bayrak(deger); break;
                    case "UST_SURE_1":       UstSure1       = Sure(deger, UstSure1); break;
                    case "UST_SURE_2":       UstSure2       = Sure(deger, UstSure2); break;
                    case "UST_IKINCI_SAYFA": UstIkinciSayfa = Bayrak(deger); break;
                    case "UST_DIGER":        UstDigerSatiri = Bayrak(deger); break;
                    case "UST_MOD":          UstMod         = UstModCoz(deger); break;
                    case "UST_BASLIK":       UstBaslik      = UstBaslikCoz(deger); break;
                    case "UST_MV_SAYISI":    UstMvSayisi    = Bayrak(deger); break;
                    case "ITTIFAK_AKTIF":    IttifakAktif   = Bayrak(deger); break;
                    case "ITTIFAK_MOD":      Ittifak        = IttifakCoz(deger); break;
                    case "KAYNAK_GOSTER":    KaynakGoster   = Bayrak(deger); break;

                    case "SOL_AKTIF":  SolAktif = Bayrak(deger); break;
                    case "CIFT_HANE":  CiftHane = Bayrak(deger); break;
                    case "SOL_MOD":    Sol      = SolCoz(deger); break;
                    case "SOL_SURE_1": SolSure1 = Sure(deger, SolSure1); break;
                    case "SOL_SURE_2": SolSure2 = Sure(deger, SolSure2); break;

                    case "ALT_AKTIF": AltAktif = Bayrak(deger); break;
                    case "ALT_SURE":  AltSure  = Sure(deger, AltSure); break;

                    case "YASAK_MODU":     YasakModu          = Bayrak(deger); break;
                    case "ACILAN_SANDIK":  AcilanSandikGoster = Bayrak(deger); break;
                    case "KAYNAK_AD":      KaynakAd           = deger; break;
                    case "SECIM_CB":       SecimCB            = deger; break;
                    case "SECIM_MV":       SecimMV            = deger; break;
                    case "OTOMATIK_BASLA": OtomatikBasla      = Bayrak(deger); break;
                    case "ACTIVE_YONET":   ActiveYonet        = Bayrak(deger); break;
                }
            }
        }

        /// <summary>
        /// Operatorun son secimlerini yazar.
        ///
        /// YASAK MODU KASITLI OLARAK YAZILMIYOR. Yasak bir gunluk bir
        /// durumdur; ertesi gun uygulama acildiginda kendiliginden geri
        /// gelmesi kimsenin beklemedigi bir sey olurdu.
        /// </summary>
        public static void Kaydet()
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                sb.AppendLine("// Uygulamanin yazdigi dosya - elle duzenlemek icin 'donus' dosyasini kullanin.");
                sb.AppendLine("// Buradaki satirlar 'donus' dosyasindaki ayni satirlarin uzerine yazar.");
                sb.AppendLine();

                sb.AppendLine("UST_AKTIF        = " + Bayrak(UstAktif));
                sb.AppendLine("UST_SURE_1       = " + UstSure1);
                sb.AppendLine("UST_SURE_2       = " + UstSure2);
                sb.AppendLine("UST_IKINCI_SAYFA = " + Bayrak(UstIkinciSayfa));
                sb.AppendLine("UST_DIGER        = " + Bayrak(UstDigerSatiri));
                sb.AppendLine("UST_MOD          = " + (UstMod == UstGosterim.MvVeOran ? "MV_VE_ORAN" : "OY_ORANI"));
                sb.AppendLine("UST_BASLIK       = " + UstBaslikYazisi(UstBaslik));
                sb.AppendLine("UST_MV_SAYISI    = " + Bayrak(UstMvSayisi));
                sb.AppendLine("ITTIFAK_AKTIF    = " + Bayrak(IttifakAktif));
                sb.AppendLine("ITTIFAK_MOD      = " + IttifakYazisi(Ittifak));
                sb.AppendLine("KAYNAK_GOSTER    = " + Bayrak(KaynakGoster));
                sb.AppendLine();

                sb.AppendLine("SOL_AKTIF  = " + Bayrak(SolAktif));
                sb.AppendLine("CIFT_HANE  = " + Bayrak(CiftHane));
                sb.AppendLine("SOL_MOD    = " + SolYazisi(Sol));
                sb.AppendLine("SOL_SURE_1 = " + SolSure1);
                sb.AppendLine("SOL_SURE_2 = " + SolSure2);
                sb.AppendLine();

                sb.AppendLine("ALT_AKTIF = " + Bayrak(AltAktif));
                sb.AppendLine("ALT_SURE  = " + AltSure);
                sb.AppendLine();

                sb.AppendLine("ACILAN_SANDIK = " + Bayrak(AcilanSandikGoster));
                sb.AppendLine("KAYNAK_AD     = " + KaynakAd);
                sb.AppendLine("SECIM_CB      = " + SecimCB);
                sb.AppendLine("SECIM_MV      = " + SecimMV);

                File.WriteAllText(ConfigPaths.SonAyarFile, sb.ToString(), new UTF8Encoding(true));

                CLog.Detail("SON AYAR YAZILDI", ConfigPaths.SonAyarFile);
            }
            catch (Exception ex)
            {
                // Ayar yazamamak yayini durdurmaz; sadece bir sonraki
                // acilista varsayilanlarla baslanir.
                CLog.Error("SON AYAR YAZILAMADI", ex.Message);
            }
        }

        #region Cozumleme

        private static bool Bayrak(string deger)
        {
            return deger == "1" || deger.Equals("EVET", StringComparison.OrdinalIgnoreCase);
        }

        private static string Bayrak(bool deger)
        {
            return deger ? "1" : "0";
        }

        /// <summary>
        /// Sure okunamazsa eski deger kalir. 3 saniyenin altina inilmiyor:
        /// sahnenin giris animasyonu bitmeden sayfa degisirse ekranda
        /// hicbir sey okunamiyor.
        /// </summary>
        private static int Sure(string deger, int eski)
        {
            int sn;

            if (!int.TryParse(deger, NumberStyles.Integer, CultureInfo.InvariantCulture, out sn))
            {
                CLog.Error("SURE OKUNAMADI", deger + " -> " + eski + " sn kaldi");
                return eski;
            }

            if (sn < 3)
            {
                CLog.Error("SURE COK KISA", sn + " sn -> 3 sn'ye cekildi");
                return 3;
            }

            return sn;
        }

        private static UstGosterim UstModCoz(string deger)
        {
            return deger.ToUpperInvariant().StartsWith("MV", StringComparison.Ordinal)
                ? UstGosterim.MvVeOran
                : UstGosterim.OyOrani;
        }

        private static UstBaslikKutusu UstBaslikCoz(string deger)
        {
            switch (deger.ToUpperInvariant())
            {
                case "KATILIM": return UstBaslikKutusu.Katilim;
                case "YOK":     return UstBaslikKutusu.Yok;
                default:        return UstBaslikKutusu.AcilanSandik;
            }
        }

        private static string UstBaslikYazisi(UstBaslikKutusu kutu)
        {
            switch (kutu)
            {
                case UstBaslikKutusu.Katilim: return "KATILIM";
                case UstBaslikKutusu.Yok:     return "YOK";
                default:                      return "ACILAN_SANDIK";
            }
        }

        private static IttifakModu IttifakCoz(string deger)
        {
            switch (deger.ToUpperInvariant())
            {
                case "ILK_2": return IttifakModu.Ilk2;
                case "2_ARTI_2": return IttifakModu.IkiArtiIki;
                case "ILK_3": return IttifakModu.Ilk3;
                default: return IttifakModu.IkiArtiUc;
            }
        }

        private static string IttifakYazisi(IttifakModu mod)
        {
            switch (mod)
            {
                case IttifakModu.Ilk2: return "ILK_2";
                case IttifakModu.IkiArtiIki: return "2_ARTI_2";
                case IttifakModu.Ilk3: return "ILK_3";
                default: return "2_ARTI_3";
            }
        }

        private static SolModu SolCoz(string deger)
        {
            switch (deger.ToUpperInvariant())
            {
                case "ILK_2_ADAY": return SolModu.Ilk2Aday;
                case "ILK_3_ADAY": return SolModu.Ilk3Aday;
                default: return SolModu.IkiArtiIki;
            }
        }

        private static string SolYazisi(SolModu mod)
        {
            switch (mod)
            {
                case SolModu.Ilk2Aday: return "ILK_2_ADAY";
                case SolModu.Ilk3Aday: return "ILK_3_ADAY";
                default: return "2_ARTI_2";
            }
        }

        #endregion
    }
}
