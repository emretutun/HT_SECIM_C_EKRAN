namespace HT_SECIM_C_EKRAN.Core
{
    /// <summary>
    /// Sahnenin ust duzeyindeki sayisal container yollari.
    ///
    /// Bu isimler ("ALT", "SAYFA1", "UST_3"...) sahne genelinde defalarca
    /// geciyor, "$ad" ile adreslenemez. Yollar sahne agacindan okunarak
    /// cikarildi:
    ///
    ///   2/3   ALT          2/4   UST          2/5   SOL
    ///   2/3/2 ALT SAYFA1   2/4/1 UST_3        2/5/2 SOL_1
    ///   2/3/3 ALT SAYFA2   2/4/2 UST_4        2/5/3 SOL_2
    ///                      2/4/3 UST_5
    ///                      2/4/4 UST_6
    ///
    /// SAHNE DEGISIRSE BURASI DEGISIR. Sahneye yeni bir container eklenip
    /// araya girerse butun yollar kayar; o yuzden yeni sahne teslim
    /// alindiginda agac yeniden okunup bu tablo dogrulanmali.
    /// </summary>
    public static class CEkranYollari
    {
        /// <summary> Alt serit kokü. </summary>
        public const string ALT = "2/3";

        /// <summary> Ust serit kokü. </summary>
        public const string UST = "2/4";

        /// <summary> Sol serit kokü. </summary>
        public const string SOL = "2/5";

        /// <summary> UST duzenleri: UST_3, UST_4, UST_5, UST_6. </summary>
        public static readonly string[] UST_DUZENLERI = { "2/4/1", "2/4/2", "2/4/3", "2/4/4" };

        /// <summary> SOL duzenleri: SOL_1, SOL_2. </summary>
        public static readonly string[] SOL_DUZENLERI = { "2/5/2", "2/5/3" };

        /// <summary> duzen numarasi (3..6) -> container yolu. </summary>
        public static string UstDuzeni(int duzen)
        {
            int sira = duzen - 3;
            return (sira >= 0 && sira < UST_DUZENLERI.Length) ? UST_DUZENLERI[sira] : null;
        }

        /// <summary> duzen numarasi (1..2) -> container yolu. </summary>
        public static string SolDuzeni(int duzen)
        {
            int sira = duzen - 1;
            return (sira >= 0 && sira < SOL_DUZENLERI.Length) ? SOL_DUZENLERI[sira] : null;
        }

        // ------------------------------------------------- UST basliklari

        /// <summary>
        /// UST_3 / UST_4 basligindaki iki metin kutusu.
        ///
        /// Ustteki satirda il adi ve secim adi burada duruyor. Ikisinin de
        /// adi HER IKI DUZENDE DE "UST_1_BASLIK1" / "UST_1_BASLIK2", yani
        /// "$ad" ile adreslenemiyorlar; sayisal yolla yazilmalari sart.
        ///
        /// Bu kutular yazilmazsa onceki yayindan kalan metin ekranda durur -
        /// Ankara'nin rakamlari "TÜRKİYE GENELİ" basligiyla gorunur.
        /// </summary>
        private static readonly string[] UST_BASLIK_KOKU = { "2/4/1/8/1/1", "2/4/2/7/1/1" };

        /// <summary> Baslik satirinin ilk kutusu: il adi. </summary>
        public static string UstBaslikIl(int duzen)
        {
            string kok = BaslikKoku(duzen);
            return kok == null ? null : kok + "/1";
        }

        /// <summary> Baslik satirinin ikinci kutusu: secim adi. </summary>
        public static string UstBaslikSecim(int duzen)
        {
            string kok = BaslikKoku(duzen);
            return kok == null ? null : kok + "/2";
        }

        private static string BaslikKoku(int duzen)
        {
            int sira = duzen - 3;
            return (sira >= 0 && sira < UST_BASLIK_KOKU.Length) ? UST_BASLIK_KOKU[sira] : null;
        }

        /// <summary>
        /// UST_5 / UST_6 basligindaki tek metin kutusu.
        ///
        /// Sahnede "MİLLETVEKİLİ SEÇİMİ" yaziliydi ama kutu o metin icin
        /// tasarlanmamis: yazi olcegi 0.538, UST_3 / UST_4'un baslik
        /// kutularinin iki katindan genis. Uzun metin yandaki acilan
        /// sandik kutusunun uzerine tasiyordu.
        ///
        /// Bu duzenlerde ayri bir il kutusu olmadigi icin IL ADI buraya
        /// yaziliyor - ittifak sayfasinda da hangi il oldugu gorunsun.
        /// Hangi secim oldugu zaten ittifak ve vekil sayilarindan belli.
        /// </summary>
        public static string IttifakBaslik(int duzen)
        {
            if (duzen == 5) return "2/4/3/1/2/1/1/1";
            if (duzen == 6) return "2/4/4/1/2/1/1/1";
            return null;
        }

        // ------------------------------------------- yazi sigdirma degerleri
        //
        // Sahneden bir kere olculen SCALING*X degerleri ve o olcekte
        // kutuya sigan harf sayisi. Uygulama bunlari sahneden OKUMUYOR:
        // bir kere sikistirdiktan sonra sikismis degeri tasarim sanip
        // yaziyi her turda biraz daha kucultmemesi icin.
        //
        // SAHNE DEGISIRSE BU DEGERLER DE YENIDEN OLCULMELI.

        /// <summary> UST_5 / UST_6 baslik kutusunun tasarim olcegi. </summary>
        public const double ITTIFAK_BASLIK_OLCEK = 0.538;

        /// <summary> O olcekte ittifak basligina sigan harf sayisi. </summary>
        public const int ITTIFAK_BASLIK_HARF = 10;

        /// <summary> UST_3 / UST_4 baslik kutularinin tasarim olcegi. </summary>
        public const double UST_BASLIK_OLCEK = 0.258;

        public const int UST_BASLIK_HARF = 20;

        /// <summary>
        /// Alt seritteki il kutusu. Adiyla da adreslenebilir
        /// (ALT_1_SAYFA1_IL_AD) ama sigdirma komutu sayisal yol istiyor.
        /// </summary>
        public const string ALT_IL_KUTUSU = "2/3/2/1/4/1/2/1";

        /// <summary> Alt seritteki il kutusunun tasarim olcegi. </summary>
        public const double ALT_IL_OLCEK = 0.518;

        public const int ALT_IL_HARF = 12;

        // --------------------------------------------- baslik kutu gruplari

        /// <summary>
        /// Alt seritteki acilan sandik kutusu. Adi iki sayfada da "ASS",
        /// o yuzden sayisal yol.
        /// </summary>
        public static string AltAssGrubu(int sayfa)
        {
            if (sayfa == 1) return "2/3/2/1/4/1";
            if (sayfa == 2) return "2/3/3/1/2";
            return null;
        }
    }
}
