using System;
using System.IO;

namespace HT_SECIM_C_EKRAN.Core
{
    /// <summary>
    /// Uygulamanin yanindaki (bin\Debug) config dosyalarinin yollari.
    ///
    /// Ayarlar duz metin dosyalarinda tutuluyor: engine adresi, sahne yolu ya da
    /// API adresi degisince yeniden derlemek gerekmiyor, dosya duzenlenip
    /// uygulama yeniden baslatiliyor. C ekrani 24 saat calisacagi icin bu
    /// esneklik reji uygulamasindan da onemli.
    /// </summary>
    public static class ConfigPaths
    {
        public static string RootFolder
        {
            get { return AppDomain.CurrentDomain.BaseDirectory; }
        }

        /// <summary> Viz Engine adresleri. </summary>
        public static string IpListFile { get { return Path.Combine(RootFolder, "iplist"); } }

        /// <summary> Sahne yolu ve donus ayarlari. </summary>
        public static string SceneFile { get { return Path.Combine(RootFolder, "sahne"); } }

        /// <summary> API adresi ve anahtari. </summary>
        public static string ApiFile { get { return Path.Combine(RootFolder, "api"); } }

        /// <summary> Sayfa sureleri, hangi secim, gosterim secenekleri - elle duzenlenir. </summary>
        public static string DonusFile { get { return Path.Combine(RootFolder, "donus"); } }

        /// <summary> Il gruplari ve kotalari - elle duzenlenir. </summary>
        public static string IllerFile { get { return Path.Combine(RootFolder, "iller"); } }

        /// <summary> Dar basliklarda kullanilacak kisa il adlari. </summary>
        public static string IlKisaFile { get { return Path.Combine(RootFolder, "il_kisa"); } }

        /// <summary> Ittifak ve aday isim plakalarinin gorselleri. </summary>
        public static string GorsellerFile { get { return Path.Combine(RootFolder, "gorseller"); } }

        /// <summary>
        /// Operatorun arayuzde yaptigi son secimler. Uygulama yazar,
        /// acilista "donus" ve "iller" dosyalarinin uzerine okur.
        /// Elle duzenlemek icin degil.
        /// </summary>
        public static string SonAyarFile { get { return Path.Combine(RootFolder, "son_ayar"); } }

        /// <summary>
        /// api dosyasinda KAYNAK = JSON secilirse okunan yerel veri dosyasi.
        /// Acil cikis kapisi: API yayin gecesi tuhaflik yaparsa tek satirla
        /// buraya donuluyor.
        /// </summary>
        public static string DataFile { get { return Path.Combine(RootFolder, "veri.json"); } }

        /// <summary>
        /// API'den gelen son saglam veri. Uygulama API kapaliyken acilirsa
        /// bununla basliyor; 24 saat calisan bir ekranin bos acilmasi
        /// kabul edilemez.
        /// </summary>
        public static string CacheFile { get { return Path.Combine(RootFolder, "veri_cache.json"); } }
    }
}
