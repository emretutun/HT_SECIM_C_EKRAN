using HT_SECIM_C_EKRAN.Sayfalar;
using System;
using System.Collections.Generic;

namespace HT_SECIM_C_EKRAN.Core
{
    /// <summary>
    /// C ekraninin donus motoru: uc serit ve ortak il imleci.
    ///
    /// Seritler birbirinden bagimsiz doner ama AYNI ili gosterir - altta
    /// Ankara ustte Izmir olmaz. Il imleci ALT listesi basa dondugunde
    /// ilerliyor: her il icin once adaylar, sonra partiler gorunuyor,
    /// sonra sonraki il.
    ///
    /// SOL bu imlece bakmiyor, her zaman Turkiye genelini gosteriyor.
    ///
    /// Baglam static: sayfalar donus sirasinda ona bakiyor ve tek bir
    /// motor var. Iki motor calistirilacak olursa burasi ornege cevrilmeli.
    /// </summary>
    public class DonusMotoru
    {
        /// <summary> Sayfalarin okudugu ortak baglam. </summary>
        public static readonly CEkranBaglam Baglam = new CEkranBaglam();

        private readonly SahneSurucu surucu;
        private readonly Serit alt;
        private readonly Serit ust;
        private readonly Serit sol;

        /// <summary> Bir seritte sayfa degistiginde tetiklenir; arayuz gostergesi icin. </summary>
        public event Action<Serit, CEkranSayfasi> OnSayfaDegisti;

        public DonusMotoru(SahneSurucu surucu)
        {
            this.surucu = surucu;

            alt = new Serit("ALT", CEkranYollari.ALT, surucu);
            ust = new Serit("ÜST", CEkranYollari.UST, surucu);
            sol = new Serit("SOL", CEkranYollari.SOL, surucu);

            // Il imleci yalnizca ALT'a bagli: uc serit de ilerletseydi il
            // saniyede bir degisir, izleyici hicbirini okuyamazdi.
            alt.OnTurTamamlandi += delegate { IlIlerlet(); };

            foreach (Serit s in Seritler)
                s.OnSayfaDegisti += delegate(Serit serit, CEkranSayfasi sayfa)
                {
                    if (OnSayfaDegisti != null) OnSayfaDegisti(serit, sayfa);
                };
        }

        public Serit Alt { get { return alt; } }
        public Serit Ust { get { return ust; } }
        public Serit Sol { get { return sol; } }

        public Serit[] Seritler { get { return new Serit[] { alt, ust, sol }; } }

        /// <summary> Seritlerden en az biri ekranda mi. </summary>
        public bool Yayinda { get { return alt.Donuyor || ust.Donuyor || sol.Donuyor; } }

        #region Sayfa listeleri

        /// <summary>
        /// Sayfa listelerini SECENEKLERE gore yeniden kurar.
        ///
        /// Operator arayuzde bir secenegi degistirdiginde bu yeniden
        /// cagriliyor; listeler kucuk oldugu icin her seferinde bastan
        /// kurmak, hangi sayfanin hangi kosulda listeye girdigini
        /// takip etmeye calismaktan cok daha az hataya aciktir.
        ///
        /// Sahne agacindan olculen duzenler:
        ///   ALT    : sayfa1 4 satir (adaylar), sayfa2 6 satir (partiler)
        ///   UST_3  : 4 sayfa x 6 satir - sayfa 1-2 oran, sayfa 3-4 oran + vekil
        ///   UST_4  : 2 sayfa x 5 satir - oran + vekil
        ///   UST_5  : 2 sayfa x 2 ittifak     UST_6 : 2 sayfa x 3 ittifak
        ///   SOL_1  : 2 sayfa x 2 aday        SOL_2 : 2 sayfa x 3 aday
        /// </summary>
        public void SayfalariKur()
        {
            alt.Sayfalar.Clear();
            ust.Sayfalar.Clear();
            sol.Sayfalar.Clear();

            if (CEkranSecenekleri.AltAktif)
            {
                alt.Sayfalar.Add(new AltAdaylarSayfasi());
                alt.Sayfalar.Add(new AltPartilerSayfasi());
            }

            if (CEkranSecenekleri.UstAktif) UstSayfalari();
            if (CEkranSecenekleri.SolAktif) SolSayfalari();

            CLog.Detail("SAYFA LISTELERI",
                "ALT " + alt.Sayfalar.Count + " / ÜST " + ust.Sayfalar.Count +
                " / SOL " + sol.Sayfalar.Count);
        }

        private void UstSayfalari()
        {
            bool ikinci = CEkranSecenekleri.UstIkinciSayfa;

            if (CEkranSecenekleri.UstMod == UstGosterim.OyOrani)
            {
                // UST_3 sayfa 1-2: alti satir, yalnizca oran.
                ust.Sayfalar.Add(new UstPartilerSayfasi(3, 1, 6, 0, false));
                if (ikinci) ust.Sayfalar.Add(new UstPartilerSayfasi(3, 2, 6, 6, false));
            }
            else
            {
                // UST_4: bes satir, oran ve vekil birlikte.
                ust.Sayfalar.Add(new UstPartilerSayfasi(4, 1, 5, 0, true));
                if (ikinci) ust.Sayfalar.Add(new UstPartilerSayfasi(4, 2, 5, 5, true));
            }

            // Vekil sayfalari yukaridaki moddan bagimsiz: ikisi birlikte
            // secilirse once oran, sonra vekil tablosu donuyor.
            if (CEkranSecenekleri.UstMvSayisi)
            {
                ust.Sayfalar.Add(new UstPartilerSayfasi(3, 3, 6, 0, true));
                if (ikinci) ust.Sayfalar.Add(new UstPartilerSayfasi(3, 4, 6, 6, true));
            }

            if (!CEkranSecenekleri.IttifakAktif) return;

            switch (CEkranSecenekleri.Ittifak)
            {
                case IttifakModu.Ilk2:
                    ust.Sayfalar.Add(new UstIttifakSayfasi(5, 1, 2, 0));
                    break;

                case IttifakModu.IkiArtiIki:
                    ust.Sayfalar.Add(new UstIttifakSayfasi(5, 1, 2, 0));
                    ust.Sayfalar.Add(new UstIttifakSayfasi(5, 2, 2, 2));
                    break;

                case IttifakModu.Ilk3:
                    ust.Sayfalar.Add(new UstIttifakSayfasi(6, 1, 3, 0));
                    break;

                default:
                    // 2+3: once iki ittifaklik duzen, sonra uc ittifaklik.
                    ust.Sayfalar.Add(new UstIttifakSayfasi(5, 1, 2, 0));
                    ust.Sayfalar.Add(new UstIttifakSayfasi(6, 1, 3, 0));
                    break;
            }
        }

        private void SolSayfalari()
        {
            switch (CEkranSecenekleri.Sol)
            {
                case SolModu.Ilk2Aday:
                    // SOL_1 sayfa 2 lider ikiliyi gosteriyor.
                    sol.Sayfalar.Add(new SolAdaylarSayfasi(1, 2, 2, 0));
                    break;

                case SolModu.Ilk3Aday:
                    sol.Sayfalar.Add(new SolAdaylarSayfasi(2, 1, 3, 0));
                    break;

                default:
                    // Sahne once ucuncu-dorduncu adayi, sonra lider ikiliyi
                    // gosterecek sekilde tasarlanmis.
                    sol.Sayfalar.Add(new SolAdaylarSayfasi(1, 1, 2, 2));
                    sol.Sayfalar.Add(new SolAdaylarSayfasi(1, 2, 2, 0));
                    break;
            }
        }

        #endregion

        #region VER / AL / HAZIRLA

        /// <summary>
        /// HAZIRLA: seritleri ekrana sokmadan kutulari doldurur.
        /// VER'e basildiginda animasyon bos kutularla baslamasin diye.
        /// </summary>
        public void Hazirla()
        {
            if (!Hazirlanabilir()) return;

            SayfalariKur();
            AyarlariUygula();
            IlGruplari.Basa();
            IlBasa();

            foreach (Serit s in Seritler) s.Hazirla();
        }

        /// <summary> VER: seritleri ekrana verir ve donusu baslatir. </summary>
        public void Ver()
        {
            if (!Hazirlanabilir()) return;

            SayfalariKur();
            AyarlariUygula();

            // Hazirla'dan sonra VER'e basilmis olabilir; o durumda imlecler
            // zaten yerinde, ama Basa() cagirmak da zarar vermez: sadece
            // donus il listesinin basindan baslar.
            if (!Yayinda)
            {
                IlGruplari.Basa();
                IlBasa();
            }

            foreach (Serit s in Seritler) s.Baslat();

            CLog.Log("YAYINA VERILDI", "ALT/ÜST/SOL");
        }

        /// <summary> AL: seritleri ekrandan cikarir ve donusu durdurur. </summary>
        public void Al()
        {
            foreach (Serit s in Seritler) s.Durdur();

            CLog.Log("YAYINDAN ALINDI", "ALT/ÜST/SOL");
        }

        private bool Hazirlanabilir()
        {
            if (!surucu.Hazir)
            {
                CLog.Error("SAHNE HAZIR DEGIL", "once sahne okunmali");
                return false;
            }

            if (!Data.DataService.Yuklendi)
            {
                CLog.Error("VERI YOK", "API'den veri gelmedi");
                return false;
            }

            return true;
        }

        /// <summary> Secim kodlarini baglama tasir. </summary>
        public void AyarlariUygula()
        {
            Baglam.SecimCB = CEkranSecenekleri.SecimCB;
            Baglam.SecimMV = CEkranSecenekleri.SecimMV;
        }

        #endregion

        #region Il donusu

        /// <summary>
        /// Il sabitlendi mi. Sabitken alt ve ust seritler sayfa
        /// degistirmeye devam eder ama il ilerlemez.
        /// </summary>
        public bool IlSabit { private set; get; }

        /// <summary>
        /// "SEÇİLENDEN DEVAM ET": donus listedeki secilen ilden devam eder.
        /// Varsa sabitleme de kalkar.
        /// </summary>
        public void IldenDevam(IlGrubu grup, IlSatiri il)
        {
            if (!IlGruplari.Konumlan(grup, il)) return;

            IlSabit = false;
            IlIlerlet();

            CLog.Log("İLDEN DEVAM", Baglam.IlAdi);

            alt.Tazele();
            ust.Tazele();
        }

        /// <summary>
        /// "SEÇİLENE GİT - DUR": secilen ile gidilir ve orada kalinir.
        /// Sayfalar donmeye devam eder, yalnizca il degismez.
        /// </summary>
        public void IlSabitle(IlGrubu grup, IlSatiri il)
        {
            if (il == null) return;

            IlGruplari.Konumlan(grup, il);

            Baglam.Plaka = il.Plaka;
            IlSabit = true;

            CLog.Log("İL SABİTLENDİ", Baglam.IlAdi);

            alt.Tazele();
            ust.Tazele();
        }

        /// <summary> "DEVAM ET": sabitleme kalkar, il yeniden donmeye baslar. </summary>
        public void IlSabitlemeyiKaldir()
        {
            if (!IlSabit) return;

            IlSabit = false;
            CLog.Log("İL DÖNÜŞÜ", "devam ediyor");
        }

        private void IlBasa()
        {
            IlSabit = false;

            int plaka = IlGruplari.Sonraki();
            Baglam.Plaka = plaka < 0 ? 0 : plaka;
        }

        private void IlIlerlet()
        {
            // Operator bir ile sabitlediyse donus ilerlemez.
            if (IlSabit) return;

            int plaka = IlGruplari.Sonraki();

            // Gosterilecek il kalmadiysa ekranda ne varsa o kalsin;
            // il listesini bosaltmak yayini karartmaktan iyidir.
            if (plaka < 0) return;

            Baglam.Plaka = plaka;

            CLog.Detail("IL DEGISTI", Baglam.IlAdi);
        }

        #endregion
    }
}
