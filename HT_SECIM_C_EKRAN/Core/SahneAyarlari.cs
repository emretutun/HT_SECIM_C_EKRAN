using System;

namespace HT_SECIM_C_EKRAN.Core
{
    /// <summary> Baglanti kurulunca sahnenin ne zaman yuklenecegi. </summary>
    public enum YuklemeModu
    {
        /// <summary> Katman bossa yuklenir, doluysa dokunulmaz. </summary>
        Bossa,

        /// <summary> Her baglantida yeniden yuklenir. </summary>
        Her,

        /// <summary> Hic yuklenmez. </summary>
        Elle
    }

    /// <summary>
    /// Exe'nin yanindaki "sahne" dosyasi: katman, sahne yolu ve yukleme davranisi.
    ///
    /// C ekrani reji uygulamasindan farkli calisiyor: burada tek bir sahne var,
    /// 24 saat yayinda duruyor ve hic dusurulmuyor. Uygulama sahneyi yuklemek
    /// yerine uzerine yaziyor.
    ///
    /// KATMAN AYRIMI: C ekrani FRONT_LAYER'a, reji uygulamasi MAIN_LAYER'a
    /// yukleniyor. Bir katmanda ayni anda tek sahne durabildigi icin ikisi
    /// ayni katmanda olsaydi biri digerini motordan dusururdu.
    /// </summary>
    public static class SahneAyarlari
    {
        /// <summary> "RENDERER*FRONT_LAYER" </summary>
        public static string Layer { private set; get; }

        /// <summary> Viz havuzundaki tam sahne yolu. </summary>
        public static string SahneYolu { private set; get; }

        public static YuklemeModu Yukleme { private set; get; }

        public static void Load()
        {
            Layer     = "RENDERER*FRONT_LAYER";
            SahneYolu = "";
            Yukleme   = YuklemeModu.Bossa;

            foreach (string[] p in ConfigReader.ReadLines(ConfigPaths.SceneFile))
            {
                if (p.Length < 2) continue;

                string anahtar = p[0].Trim().ToUpperInvariant();
                string deger   = p[1].Trim();

                switch (anahtar)
                {
                    case "LAYER":
                        if (deger.Length > 0) Layer = deger;
                        break;

                    case "SAHNE":
                        SahneYolu = deger;
                        break;

                    case "YUKLEME":
                        Yukleme = YuklemeCoz(deger);
                        break;

                    // Eski ayar adi: OTOMATIK_YUKLE = 0 / 1
                    case "OTOMATIK_YUKLE":
                        Yukleme = (deger == "1") ? YuklemeModu.Her : YuklemeModu.Elle;
                        break;
                }
            }

            if (SahneYolu.Length == 0)
                CLog.Error("SAHNE YOLU YOK", "sahne dosyasinda SAHNE satiri bos");

            CLog.Log("SAHNE AYARLARI", Layer + " / " + SahneYolu + " / yukleme: " + Yukleme);
        }

        /// <summary>
        /// Taninmayan deger BOSSA'ya duser: yazim hatasi yuzunden sahnenin
        /// hic yuklenmemesindense, bos katmana yuklenmesi daha az zararli.
        /// </summary>
        private static YuklemeModu YuklemeCoz(string deger)
        {
            switch (deger.ToUpperInvariant())
            {
                case "HER":  return YuklemeModu.Her;
                case "ELLE": return YuklemeModu.Elle;
                default:     return YuklemeModu.Bossa;
            }
        }

        /// <summary> Sahneyi katmana yukleyen komut. </summary>
        public static string YuklemeKomutu()
        {
            return Layer + " SET_OBJECT SCENE*" + SahneYolu;
        }

        /// <summary> Katmanin agacini soran komut. Cevabi bossa katman bostur. </summary>
        public static string AgacKomutu()
        {
            return Layer + "*TREE GET";
        }
    }
}
