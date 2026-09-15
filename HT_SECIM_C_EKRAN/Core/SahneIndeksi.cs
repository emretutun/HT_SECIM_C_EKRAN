using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace HT_SECIM_C_EKRAN.Core
{
    /// <summary>
    /// "STAGE GET ALL" ciktisini cozup director'lerin id'lerini cikarir.
    ///
    /// NEDEN ID GEREKIYOR
    /// ------------------
    /// C ekraninda "SAYFA1" adi YEDI ayri yerde geciyor: ALT, UST_3, UST_4,
    /// UST_5, UST_6, SOL_1 ve SOL_2. Motorda denendi:
    ///
    ///   DIRECTOR*SAYFA2 START        -> yanlis yer oynuyor, belirsiz
    ///   DIRECTOR*UST_3*SAYFA2 START  -> invalid sublocation, boyle bir sozdizimi yok
    ///   #2066686 START               -> calisiyor
    ///
    /// Tek kesin adres id. Id'ler sahne her yuklendiginde degistigi icin
    /// sahne okundugunda burasi yeniden kuruluyor.
    ///
    /// ADRESLEME
    /// ---------
    /// Director'ler AD YOLU ile araniyor: "UST_3/SAYFA1" gibi. Sondan
    /// eslesme yapiliyor, yani "SAYFA1" tek basina da yazilabilir ama
    /// birden fazla eslesme cikarsa hata veriliyor - sessizce yanlis
    /// director'u oynatmaktansa sikayet etmek daha iyi.
    /// </summary>
    public class SahneIndeksi
    {
        private static readonly Regex DIRECTOR_SATIRI = new Regex(
            @"\{\s*(?<yol>[\d/]+)\s+CDirectorTree\s+#(?<id>\d+)\s+""(?<ad>[^""]*)""\s+(?<acik>-?\d+)",
            RegexOptions.Compiled);

        public class Director
        {
            /// <summary> Stage'deki sayisal yol: "1/0/1" </summary>
            public string Yol { set; get; }

            public int Id { set; get; }

            /// <summary> Kendi adi: "SAYFA1" </summary>
            public string Ad { set; get; }

            /// <summary> Ust director'lerle birlikte: "UST/UST_3/SAYFA1" </summary>
            public string AdYolu { set; get; }

            public override string ToString() { return AdYolu; }
        }

        private readonly List<Director> directorler = new List<Director>();

        public IList<Director> Directorler { get { return directorler; } }

        public int Sayi { get { return directorler.Count; } }

        /// <summary> Sahne okunmadan once true dondurmemeli. </summary>
        public bool Hazir { get { return directorler.Count > 0; } }

        public void Temizle()
        {
            directorler.Clear();
        }

        /// <summary> "STAGE GET ALL" ciktisini cozer. </summary>
        public void Coz(string stageCiktisi)
        {
            directorler.Clear();

            if (string.IsNullOrEmpty(stageCiktisi)) return;

            // Once sayisal yol -> ad haritasi; ad yolunu kurarken ust satirlar lazim.
            Dictionary<string, Director> yolaGore = new Dictionary<string, Director>(StringComparer.Ordinal);

            foreach (Match m in DIRECTOR_SATIRI.Matches(stageCiktisi))
            {
                Director d = new Director();
                d.Yol = m.Groups["yol"].Value;
                d.Id  = SayiOku(m.Groups["id"].Value);
                d.Ad  = m.Groups["ad"].Value;

                if (d.Id <= 0) continue;

                yolaGore[d.Yol] = d;
                directorler.Add(d);
            }

            foreach (Director d in directorler)
                d.AdYolu = AdYoluKur(d, yolaGore);
        }

        /// <summary>
        /// Sayisal yolu parca parca kisaltip ust director'lerin adlarini toplar.
        /// "1/0/1" -> ust "1/0" (UST_3), onun ustu "1" (UST) -> "UST/UST_3/SAYFA1"
        /// </summary>
        private static string AdYoluKur(Director d, Dictionary<string, Director> yolaGore)
        {
            List<string> parcalar = new List<string>();
            parcalar.Add(d.Ad);

            string yol = d.Yol;

            while (true)
            {
                int son = yol.LastIndexOf('/');
                if (son <= 0) break;

                yol = yol.Substring(0, son);

                Director ust;
                if (yolaGore.TryGetValue(yol, out ust)) parcalar.Insert(0, ust.Ad);
            }

            return string.Join("/", parcalar.ToArray());
        }

        /// <summary>
        /// Ad yoluna gore director bulur. Bulamazsa ya da birden fazla
        /// eslesme varsa null doner ve sebebini loglar.
        /// </summary>
        public Director Bul(string adYolu)
        {
            if (string.IsNullOrEmpty(adYolu)) return null;

            string[] aranan = Parcala(adYolu);
            if (aranan.Length == 0) return null;

            List<Director> eslesenler = new List<Director>();

            foreach (Director d in directorler)
                if (SondanEslesir(Parcala(d.AdYolu), aranan)) eslesenler.Add(d);

            if (eslesenler.Count == 1) return eslesenler[0];

            if (eslesenler.Count == 0)
            {
                CLog.Error("DIRECTOR BULUNAMADI", adYolu);
                return null;
            }

            // Belirsizlik sessizce yanlis director'u oynatmaktan iyidir.
            List<string> adlar = new List<string>();
            foreach (Director d in eslesenler) adlar.Add(d.AdYolu);

            CLog.Error("DIRECTOR BELIRSIZ",
                adYolu + " -> " + string.Join(" | ", adlar.ToArray()));

            return null;
        }

        /// <summary> Bir director'un id'si; bulunamazsa 0. </summary>
        public int Id(string adYolu)
        {
            Director d = Bul(adYolu);
            return d == null ? 0 : d.Id;
        }

        private static string[] Parcala(string yol)
        {
            return yol.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary> Aranan parcalar, adayin sonunda aynen geciyor mu. </summary>
        private static bool SondanEslesir(string[] aday, string[] aranan)
        {
            if (aranan.Length > aday.Length) return false;

            int fark = aday.Length - aranan.Length;

            for (int i = 0; i < aranan.Length; i++)
            {
                if (!string.Equals(aday[fark + i], aranan[i], StringComparison.OrdinalIgnoreCase))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Dopesheet'te kapali duran director satirlarinin id'leri.
        ///
        /// "STAGE GET ALL" yalnizca ACIK satirlarin altini dokuyor. Kapali bir
        /// director'un altindaki sayfalar hic gorunmez, dolayisiyla id'leri de
        /// bilinmez. Director satirlari komutla acilabiliyor (container
        /// satirlari acilamiyor, o ayri dert).
        /// </summary>
        public static List<int> KapaliDirectorler(string stageCiktisi)
        {
            List<int> idler = new List<int>();

            if (string.IsNullOrEmpty(stageCiktisi)) return idler;

            foreach (Match m in DIRECTOR_SATIRI.Matches(stageCiktisi))
            {
                if (m.Groups["acik"].Value != "0") continue;

                int id = SayiOku(m.Groups["id"].Value);
                if (id > 0) idler.Add(id);
            }

            return idler;
        }

        private static int SayiOku(string metin)
        {
            int deger;
            int.TryParse(metin, out deger);
            return deger;
        }
    }
}
