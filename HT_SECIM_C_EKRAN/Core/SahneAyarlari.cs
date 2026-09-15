using System;

namespace HT_SECIM_C_EKRAN.Core
{
    /// <summary>
    /// Exe'nin yanindaki "sahne" dosyasi: katman, sahne yolu ve yukleme davranisi.
    ///
    /// C ekrani reji uygulamasindan farkli calisiyor: burada tek bir sahne var,
    /// 24 saat yayinda duruyor ve hic dusurulmuyor. Uygulama sahneyi yuklemek
    /// yerine uzerine yaziyor.
    /// </summary>
    public static class SahneAyarlari
    {
        /// <summary> "RENDERER*MAIN_LAYER" </summary>
        public static string Layer { private set; get; }

        /// <summary> Viz havuzundaki tam sahne yolu. </summary>
        public static string SahneYolu { private set; get; }

        /// <summary>
        /// Baglanti kurulunca sahne motora yuklensin mi.
        /// Yayin makinesinde kapali olmali: sahne zaten ekranda dururken
        /// yeniden yuklemek goruntuyu sifirlar.
        /// </summary>
        public static bool OtomatikYukle { private set; get; }

        public static void Load()
        {
            Layer         = "RENDERER*MAIN_LAYER";
            SahneYolu     = "";
            OtomatikYukle = false;

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

                    case "OTOMATIK_YUKLE":
                        OtomatikYukle = (deger == "1");
                        break;
                }
            }

            if (SahneYolu.Length == 0)
                CLog.Error("SAHNE YOLU YOK", "sahne dosyasinda SAHNE satiri bos");

            CLog.Log("SAHNE AYARLARI", Layer + " / " + SahneYolu +
                (OtomatikYukle ? " / otomatik yukleme acik" : ""));
        }

        /// <summary> Sahneyi katmana yukleyen komut. </summary>
        public static string YuklemeKomutu()
        {
            return Layer + " SET_OBJECT SCENE*" + SahneYolu;
        }
    }
}
