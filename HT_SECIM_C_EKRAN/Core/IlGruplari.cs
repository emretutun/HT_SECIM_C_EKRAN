using HT_SECIM_C_EKRAN.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace HT_SECIM_C_EKRAN.Core
{
    /// <summary>
    /// Donuse girecek bir il.
    /// YAYIN kutusu kapatilirsa il listede kalir ama siraya girmez -
    /// operator bir ili gecici olarak cikarip geri alabilsin diye.
    /// </summary>
    public class IlSatiri
    {
        public int Plaka;
        public string Ad;
        public bool Yayin = true;

        public override string ToString() { return Ad; }
    }

    /// <summary>
    /// Bir il grubu: ILLER, BUYUK SEHIRLER, AFET BOLGELERI, DIGER.
    ///
    /// KOTA: bu gruptan arka arkaya kac il gosterilip sonraki gruba
    /// gecilecegi. Kotasi 0 olan grup donuse hic girmez - operator bir
    /// grubu tek hamlede devre disi birakabilsin diye boyle.
    /// </summary>
    public class IlGrubu
    {
        public string Ad;
        public int Kota;
        public readonly List<IlSatiri> Iller = new List<IlSatiri>();

        /// <summary> Grup icinde kaldigimiz yer; gruba geri donuldugunde buradan devam edilir. </summary>
        public int Imlec;

        public override string ToString() { return Ad; }

        /// <summary> Yayina acik il sayisi. </summary>
        public int YayindaOlan
        {
            get
            {
                int sayi = 0;
                foreach (IlSatiri il in Iller) if (il.Yayin) sayi++;
                return sayi;
            }
        }
    }

    /// <summary>
    /// Il donusunun sirasi.
    ///
    /// Gruplar sirayla dolasiliyor: ILLER grubundan kotasi kadar il, sonra
    /// BUYUK SEHIRLER'den kotasi kadar, sonra AFET BOLGELERI... Bir tur
    /// bitince basa donuluyor ve her grup KENDI kaldigi yerden devam ediyor.
    /// Boylece 81 ilin tamami sirayla gecerken arada duzenli olarak
    /// buyuksehirler ve afet illeri de ekrana geliyor.
    ///
    /// Gruplarin icerigi "iller" dosyasindan geliyor; yeni bir grup ya da
    /// farkli bir il listesi icin derleme gerekmiyor.
    /// </summary>
    public static class IlGruplari
    {
        private static readonly List<IlGrubu> gruplar = new List<IlGrubu>();

        private static int grupImleci;
        private static int grupSayaci;

        public static List<IlGrubu> Gruplar { get { return gruplar; } }

        /// <summary>
        /// "iller" dosyasini okur ve il adlarini veriyle esler.
        ///
        /// VERIDEN SONRA CAGRILMALI: il adlari DataService'ten aliniyor,
        /// veri yuklenmemisken cagrilirsa listede plaka numaralari gorunur.
        /// </summary>
        public static void Load()
        {
            gruplar.Clear();
            grupImleci = 0;
            grupSayaci = 0;

            foreach (string[] p in ConfigReader.ReadLines(ConfigPaths.IllerFile))
            {
                if (p.Length < 4) continue;
                if (!p[0].Trim().Equals("GRUP", StringComparison.OrdinalIgnoreCase)) continue;

                IlGrubu grup = new IlGrubu();
                grup.Ad   = p[1].Trim();
                grup.Kota = Sayi(p[2].Trim(), 0);

                foreach (string parca in p[3].Split(','))
                {
                    int plaka = Sayi(parca.Trim(), -1);
                    if (plaka < 0) continue;

                    IlSatiri satir = new IlSatiri();
                    satir.Plaka = plaka;
                    satir.Ad    = IlAdi(plaka);

                    grup.Iller.Add(satir);
                }

                gruplar.Add(grup);
            }

            if (gruplar.Count == 0)
                CLog.Error("IL GRUBU YOK", "iller dosyasi bos ya da okunamadi");
            else
                CLog.Log("IL GRUPLARI", Ozet());

            YayinDurumunuOku();
        }

        /// <summary>
        /// Plaka 0 veride "TÜRKİYE" olarak geciyor; ekranda ve listede
        /// "TÜRKİYE GENELİ" yazmasi daha anlasilir.
        /// </summary>
        private static string IlAdi(int plaka)
        {
            return IlAdlari.Ekranda(plaka);
        }

        private static string Ozet()
        {
            StringBuilder sb = new StringBuilder();

            foreach (IlGrubu g in gruplar)
            {
                if (sb.Length > 0) sb.Append(" / ");
                sb.Append(g.Ad).Append(" ").Append(g.Iller.Count).Append(" il, kota ").Append(g.Kota);
            }

            return sb.ToString();
        }

        #region Donus

        /// <summary> Donuse girecek en az bir il var mi. </summary>
        public static bool Hazir
        {
            get
            {
                foreach (IlGrubu g in gruplar)
                    if (g.Kota > 0 && g.YayindaOlan > 0) return true;

                return false;
            }
        }

        /// <summary> Imlecleri basa alir. Donus baslarken cagriliyor. </summary>
        public static void Basa()
        {
            grupImleci = 0;
            grupSayaci = 0;

            foreach (IlGrubu g in gruplar) g.Imlec = 0;
        }

        /// <summary>
        /// Siradaki ili verir. Gosterilecek il kalmadiysa -1 doner.
        ///
        /// Yayin kutusu kapali iller atlaniyor; hepsi kapaliysa gruptan
        /// cikiliyor. Butun gruplar bos olabilecegi icin tur sayisi sinirli,
        /// yoksa sonsuz donguye girer.
        /// </summary>
        public static int Sonraki()
        {
            if (gruplar.Count == 0) return -1;

            for (int deneme = 0; deneme < gruplar.Count * 2; deneme++)
            {
                IlGrubu grup = gruplar[grupImleci];

                if (grup.Kota <= 0 || grup.YayindaOlan == 0)
                {
                    GrubuDegistir();
                    continue;
                }

                int plaka = GruptanAl(grup);

                grupSayaci++;
                if (grupSayaci >= grup.Kota) GrubuDegistir();

                return plaka;
            }

            return -1;
        }

        /// <summary>
        /// Gruptaki siradaki yayina acik il.
        /// Grubun imleci grup icinde kaliyor: gruba tekrar gelindiginde
        /// bastan degil kaldigi yerden devam ediyor.
        /// </summary>
        private static int GruptanAl(IlGrubu grup)
        {
            for (int i = 0; i < grup.Iller.Count; i++)
            {
                if (grup.Imlec >= grup.Iller.Count) grup.Imlec = 0;

                IlSatiri satir = grup.Iller[grup.Imlec];
                grup.Imlec++;

                if (satir.Yayin) return satir.Plaka;
            }

            return -1;
        }

        private static void GrubuDegistir()
        {
            grupSayaci = 0;

            grupImleci++;
            if (grupImleci >= gruplar.Count) grupImleci = 0;
        }

        /// <summary>
        /// Donusu belirli bir ile tasir. "SEÇİLENDEN DEVAM ET" bunu kullaniyor:
        /// operator listeden bir il secip oradan devam ettirebiliyor.
        /// </summary>
        public static bool Konumlan(IlGrubu grup, IlSatiri il)
        {
            if (grup == null || il == null) return false;

            int grupYeri = gruplar.IndexOf(grup);
            int ilYeri   = grup.Iller.IndexOf(il);

            if (grupYeri < 0 || ilYeri < 0) return false;

            grupImleci = grupYeri;
            grupSayaci = 0;
            grup.Imlec = ilYeri;

            return true;
        }

        #endregion

        #region Yayin durumu

        /// <summary>
        /// Kapali il kutularini son_ayar'dan okur.
        ///
        /// Kapalilar yaziliyor, aciklar degil: varsayilan "hepsi acik"
        /// oldugu icin dosya boyle cok daha kisa kaliyor ve yeni bir il
        /// eklendiginde kendiliginden yayina giriyor.
        /// </summary>
        private static void YayinDurumunuOku()
        {
            foreach (string[] p in ConfigReader.ReadLines(ConfigPaths.SonAyarFile))
            {
                if (p.Length < 3) continue;
                if (!p[0].Trim().Equals("KAPALI", StringComparison.OrdinalIgnoreCase)) continue;

                IlGrubu grup = GrupBul(p[1].Trim());
                if (grup == null) continue;

                foreach (string parca in p[2].Split(','))
                {
                    int plaka = Sayi(parca.Trim(), -1);
                    if (plaka < 0) continue;

                    foreach (IlSatiri satir in grup.Iller)
                        if (satir.Plaka == plaka) satir.Yayin = false;
                }
            }
        }

        /// <summary> Kapali illeri ve kotalari son_ayar dosyasinin sonuna ekler. </summary>
        public static void Kaydet()
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                sb.AppendLine();
                sb.AppendLine("// Il gruplarinin kotalari ve yayina kapatilan iller.");

                foreach (IlGrubu grup in gruplar)
                {
                    sb.AppendLine("KOTA = " + grup.Ad + " = " + grup.Kota);

                    StringBuilder kapali = new StringBuilder();

                    foreach (IlSatiri satir in grup.Iller)
                    {
                        if (satir.Yayin) continue;

                        if (kapali.Length > 0) kapali.Append(",");
                        kapali.Append(satir.Plaka.ToString(CultureInfo.InvariantCulture));
                    }

                    if (kapali.Length > 0)
                        sb.AppendLine("KAPALI = " + grup.Ad + " = " + kapali);
                }

                // Dosyanin SONUNA ekleniyor: BOM'suz yazilmali, yoksa
                // dosyanin ortasina BOM karakterleri girer ve satirlar
                // bir daha okunamaz.
                File.AppendAllText(ConfigPaths.SonAyarFile, sb.ToString(), new UTF8Encoding(false));
            }
            catch (Exception ex)
            {
                CLog.Error("IL DURUMU YAZILAMADI", ex.Message);
            }
        }

        /// <summary> Kotalari son_ayar'dan okur. Gruplar kuruldugu icin ayri cagriliyor. </summary>
        public static void KotalariOku()
        {
            foreach (string[] p in ConfigReader.ReadLines(ConfigPaths.SonAyarFile))
            {
                if (p.Length < 3) continue;
                if (!p[0].Trim().Equals("KOTA", StringComparison.OrdinalIgnoreCase)) continue;

                IlGrubu grup = GrupBul(p[1].Trim());
                if (grup == null) continue;

                grup.Kota = Sayi(p[2].Trim(), grup.Kota);
            }
        }

        private static IlGrubu GrupBul(string ad)
        {
            foreach (IlGrubu g in gruplar)
                if (g.Ad.Equals(ad, StringComparison.OrdinalIgnoreCase)) return g;

            return null;
        }

        #endregion

        private static int Sayi(string metin, int varsayilan)
        {
            int deger;
            return int.TryParse(metin, NumberStyles.Integer, CultureInfo.InvariantCulture, out deger)
                ? deger : varsayilan;
        }
    }
}
