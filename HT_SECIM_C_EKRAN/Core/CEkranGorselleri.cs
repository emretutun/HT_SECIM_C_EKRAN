using System;
using System.Collections.Generic;

namespace HT_SECIM_C_EKRAN.Core
{
    /// <summary>
    /// Isim plakalari: ittifak ve aday satirlarindaki renkli zemin.
    ///
    /// Bu zeminler boyanabilen bir materyal DEGIL, sahnede hazir birer
    /// gorsel (denendi: "container holds no MATERIAL reference"). O yuzden
    /// renk komutu ise yaramiyor; dogru gorseli giydirmek gerekiyor.
    /// Havuzda ittifak basina ayri plaka hazirlanmis, sahnenin kendi
    /// yontemi bu.
    ///
    /// Eslesme "gorseller" dosyasindan geliyor: yeni bir ittifak ya da
    /// aday icin gorseli havuza koyup dosyaya bir satir eklemek yetiyor.
    /// </summary>
    public static class CEkranGorselleri
    {
        /// <summary> Listede olmayan kodlar icin kullanilan satir. </summary>
        private const string VARSAYILAN = "*";

        private class Plaka
        {
            public string Genis;   // UST_6 / tek gorselli duzenler
            public string Dar;     // UST_5 ("2LI_ICIN")
        }

        private static readonly Dictionary<string, Plaka> ittifaklar =
            new Dictionary<string, Plaka>(StringComparer.OrdinalIgnoreCase);

        private static readonly Dictionary<string, string> adaylar =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        private static string ustKlasor = "";
        private static string solKlasor = "";

        public static void Load()
        {
            ittifaklar.Clear();
            adaylar.Clear();
            ustKlasor = "";
            solKlasor = "";

            foreach (string[] p in ConfigReader.ReadLines(ConfigPaths.GorsellerFile))
            {
                if (p.Length < 2) continue;

                string anahtar = p[0].Trim().ToUpperInvariant();

                switch (anahtar)
                {
                    case "UST_KLASOR": ustKlasor = p[1].Trim(); break;
                    case "SOL_KLASOR": solKlasor = p[1].Trim(); break;

                    case "ITTIFAK":
                        if (p.Length < 4) break;

                        Plaka plaka = new Plaka();
                        plaka.Genis = Yol(ustKlasor, p[2].Trim());
                        plaka.Dar   = Yol(ustKlasor, p[3].Trim());

                        ittifaklar[p[1].Trim()] = plaka;
                        break;

                    case "ADAY":
                        if (p.Length < 3) break;
                        adaylar[p[1].Trim()] = Yol(solKlasor, p[2].Trim());
                        break;
                }
            }

            CLog.Log("ISIM PLAKALARI", ittifaklar.Count + " ittifak / " + adaylar.Count + " aday");
        }

        private static string Yol(string klasor, string ad)
        {
            if (ad.Length == 0) return null;

            // Dosyada tam yol yazilmissa oldugu gibi kullaniliyor.
            if (ad.IndexOf('/') >= 0) return ad;

            return klasor.Length == 0 ? ad : (klasor.TrimEnd('/') + "/" + ad);
        }

        /// <summary>
        /// Ittifak isim plakasi.
        /// UST_5 iki satirlik oldugu icin "2LI_ICIN" plakasini istiyor;
        /// oranlari UST_6'ninkinden farkli.
        /// </summary>
        public static string IttifakPlakasi(string kod, int duzen)
        {
            Plaka plaka = IttifakBul(kod);
            if (plaka == null) return null;

            return (duzen == 5) ? plaka.Dar : plaka.Genis;
        }

        private static Plaka IttifakBul(string kod)
        {
            Plaka plaka;

            if (!string.IsNullOrEmpty(kod) && ittifaklar.TryGetValue(kod, out plaka)) return plaka;
            if (ittifaklar.TryGetValue(VARSAYILAN, out plaka)) return plaka;

            return null;
        }

        /// <summary> Sol seritteki aday isim plakasi. </summary>
        public static string AdayPlakasi(string kod)
        {
            string yol;

            if (!string.IsNullOrEmpty(kod) && adaylar.TryGetValue(kod, out yol)) return yol;
            if (adaylar.TryGetValue(VARSAYILAN, out yol)) return yol;

            return null;
        }
    }
}
