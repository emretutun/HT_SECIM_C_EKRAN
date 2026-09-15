using HT_SECIM_C_EKRAN.Data;
using System.Collections.Generic;
using System.Globalization;

namespace HT_SECIM_C_EKRAN.Core
{
    /// <summary>
    /// Il adlarinin ekranda gorunecek hali.
    ///
    /// Sahnedeki baslik kutulari dar; KAHRAMANMARAŞ ya da ŞANLIURFA gibi
    /// uzun adlar tasiyor. Iki kademeli cozum var:
    ///
    ///   1. Bu sinif - "il_kisa" dosyasinda kisa karsiligi yazili olan
    ///      iller kisa adiyla gosteriliyor (K.MARAŞ, Ş.URFA).
    ///   2. SahneSurucu.SigdirEkle - kisa ad da sigmiyorsa yazi yatayda
    ///      oranla sikistiriliyor, boylece hicbir ad kutudan tasmiyor.
    ///
    /// Kisaltma BUTUN seritlerde ayni: bir il alt seritte uzun, ust
    /// seritte kisa yazilmasin.
    /// </summary>
    public static class IlAdlari
    {
        private static readonly Dictionary<int, string> kisaltmalar = new Dictionary<int, string>();

        public static void Load()
        {
            kisaltmalar.Clear();

            foreach (string[] p in ConfigReader.ReadLines(ConfigPaths.IlKisaFile))
            {
                if (p.Length < 2) continue;

                int plaka;
                if (!int.TryParse(p[0].Trim(), NumberStyles.Integer,
                                  CultureInfo.InvariantCulture, out plaka)) continue;

                string ad = p[1].Trim();
                if (ad.Length == 0) continue;

                kisaltmalar[plaka] = ad;
            }

            CLog.Log("IL KISALTMALARI", kisaltmalar.Count + " il");
        }

        /// <summary>
        /// Ekranda gorunecek il adi.
        /// Kisaltma tanimliysa o, degilse verideki tam ad.
        /// </summary>
        public static string Ekranda(int plaka)
        {
            string kisa;
            if (kisaltmalar.TryGetValue(plaka, out kisa)) return kisa;

            if (plaka == 0) return "TÜRKİYE GENELİ";

            string ad = DataService.IlAdi(plaka);
            return string.IsNullOrEmpty(ad) ? ("PLAKA " + plaka) : ad;
        }
    }
}
