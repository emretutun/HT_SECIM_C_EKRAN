using HT_SECIM_C_EKRAN.Core;
using HT_SECIM_C_EKRAN.Data;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace HT_SECIM_C_EKRAN
{
    /// <summary>
    /// C ekrani kontrol uygulamasi.
    ///
    /// Reji uygulamasindan farki: burada tek bir sahne var ve 24 saat
    /// yayinda duruyor. Sahne yuklenip dusurulmuyor; uygulama uzerine
    /// yaziyor ve donus animasyonlarini zamaninda tetikliyor.
    ///
    /// Uc serit (ALT / UST / SOL) birbirinden bagimsiz doner; isi
    /// DonusMotoru yuruyor. Buradaki arayuz yalnizca secenekleri
    /// topluyor, seritleri ekrana verip aliyor ve ne gosterildigini
    /// yaziyor.
    ///
    /// Il grubu kutulari CALISMA ANINDA olusturuluyor: gruplar "iller"
    /// dosyasindan geliyor, sayilari sabit degil.
    /// </summary>
    public partial class Form1 : Form
    {
        private readonly VizEngine engine = new VizEngine();
        private readonly SahneSurucu sahne;
        private readonly DonusMotoru motor;

        /// <summary> Grup -> o grubun liste kutusu. Secili ili bulmak icin. </summary>
        private readonly Dictionary<IlGrubu, CheckedListBox> ilKutulari =
            new Dictionary<IlGrubu, CheckedListBox>();

        /// <summary>
        /// Arayuz kendi kendini tetiklemesin diye. Secenekler kutulara
        /// yazilirken her kutu bir "degisti" olayi firlatiyor; bu bayrak
        /// acikken o olaylar yok sayiliyor.
        /// </summary>
        private bool kutulariDolduruyor;

        public Form1()
        {
            InitializeComponent();

            sahne = new SahneSurucu(engine);
            motor = new DonusMotoru(sahne);
        }

        #region Form olaylari

        private void Form1_Load(object sender, EventArgs e)
        {
            CLog.DebugMode = true;
            CLog.OnLogWritten += CLog_OnLogWritten;
            LogKonsolunuKur();

            CLog.Log("UYGULAMA BASLADI", Application.ProductVersion);

            EngineRepository.Load();
            SahneAyarlari.Load();
            ApiAyarlari.Load();
            CEkranSecenekleri.Load();

            VeriyiIlkYukle();
            EngineleriYukle();

            // Il gruplari il ADLARINI veriden aliyor, veriden sonra kurulmali.
            IlAdlari.Load();
            CEkranGorselleri.Load();
            IlGruplari.Load();
            IlGruplari.KotalariOku();
            IlKutulariniKur();

            // SIRA ONEMLI: once secenekler kutulara yazilmali.
            // SecimleriYukle sonunda KutulardanOku cagiriyor; kutular
            // doldurulmamis olsaydi bos arayuz ayar dosyasinin uzerine
            // yazar ve butun AKTIF kutulari kapali gelirdi.
            SecenekleriKutularaYaz();
            SecimleriYukle();
            OlaylariBagla();

            lbl_sahne.Text = SahneAyarlari.Layer + "\n" + SahneAyarlari.SahneYolu;

            VeriKaynaginiKur();
            DurumuTazele();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            logZamanlayici.Stop();
            VeriKaynagi.Durdur();

            // Seritler ekrandan aliniyor; yoksa sahnede son sayfa donmus
            // halde kalir ve 24 saat oyle durur.
            motor.Al();

            CEkranSecenekleri.Kaydet();
            IlGruplari.Kaydet();

            engine.Disconnect();
            CLog.Log("UYGULAMA KAPANDI");
            CLog.Kapat();
        }

        private void OlaylariBagla()
        {
            btn_baglan.Click += Baglan_Click;
            btn_oku.Click    += Oku_Click;

            btn_hazirla.Click += delegate { motor.Hazirla(); DurumuTazele(); };
            btn_ver.Click     += Ver_Click;
            btn_al.Click      += delegate { motor.Al(); DurumuTazele(); };

            btn_altAtla.Click += delegate { motor.Alt.Atla(); };
            btn_ustAtla.Click += delegate { motor.Ust.Atla(); };
            btn_solAtla.Click += delegate { motor.Sol.Atla(); };

            btn_secilendenDevam.Click += SecilendenDevam_Click;
            btn_secileneGit.Click     += SecileneGit_Click;
            btn_devam.Click           += Devam_Click;

            engine.OnConnected    += Engine_OnConnected;
            engine.OnDisconnected += Engine_OnDisconnected;

            motor.OnSayfaDegisti += Motor_OnSayfaDegisti;

            // Butun secenek kutulari ayni ele gidiyor: kutulari oku,
            // sayfa listelerini yeniden kur, gostergeyi tazele.
            EventHandler degisti = Secenek_Degisti;

            chk_ustAktif.CheckedChanged     += degisti;
            num_ustSure1.ValueChanged       += degisti;
            num_ustSure2.ValueChanged       += degisti;
            chk_ustIkinci.CheckedChanged    += degisti;
            chk_ustDiger.CheckedChanged     += degisti;
            rad_oyOrani.CheckedChanged      += degisti;
            rad_mvOran.CheckedChanged       += degisti;
            chk_mvSayisi.CheckedChanged     += degisti;
            rad_baslikAss.CheckedChanged    += degisti;
            rad_baslikKo.CheckedChanged     += degisti;
            rad_baslikYok.CheckedChanged    += degisti;
            chk_ittifakAktif.CheckedChanged += degisti;
            rad_ilk2.CheckedChanged         += degisti;
            rad_ikiArtiIki.CheckedChanged   += degisti;
            rad_ilk3.CheckedChanged         += degisti;
            rad_ikiArtiUc.CheckedChanged    += degisti;
            chk_kaynak.CheckedChanged       += degisti;
            txt_kaynak.TextChanged          += degisti;

            chk_solAktif.CheckedChanged  += degisti;
            chk_ciftHane.CheckedChanged  += degisti;
            rad_sol2Aday.CheckedChanged  += degisti;
            rad_sol3Aday.CheckedChanged  += degisti;
            rad_solDongu.CheckedChanged  += degisti;
            num_solSure1.ValueChanged    += degisti;
            num_solSure2.ValueChanged    += degisti;

            chk_altAktif.CheckedChanged += degisti;
            num_altSure.ValueChanged    += degisti;

            chk_yasak.CheckedChanged        += Yasak_Degisti;
            chk_acilanSandik.CheckedChanged += Yasak_Degisti;

            cmb_cb.SelectedIndexChanged += Secim_Degisti;
            cmb_mv.SelectedIndexChanged += Secim_Degisti;
        }

        #endregion

        #region Secenekler

        /// <summary> Ayar dosyasindan okunan secenekleri kutulara yansitir. </summary>
        private void SecenekleriKutularaYaz()
        {
            kutulariDolduruyor = true;

            try
            {
                chk_ustAktif.Checked  = CEkranSecenekleri.UstAktif;
                num_ustSure1.Value    = CEkranSecenekleri.UstSure1;
                num_ustSure2.Value    = CEkranSecenekleri.UstSure2;
                chk_ustIkinci.Checked = CEkranSecenekleri.UstIkinciSayfa;
                chk_ustDiger.Checked  = CEkranSecenekleri.UstDigerSatiri;

                rad_oyOrani.Checked = (CEkranSecenekleri.UstMod == UstGosterim.OyOrani);
                rad_mvOran.Checked  = (CEkranSecenekleri.UstMod == UstGosterim.MvVeOran);

                rad_baslikAss.Checked = (CEkranSecenekleri.UstBaslik == UstBaslikKutusu.AcilanSandik);
                rad_baslikKo.Checked  = (CEkranSecenekleri.UstBaslik == UstBaslikKutusu.Katilim);
                rad_baslikYok.Checked = (CEkranSecenekleri.UstBaslik == UstBaslikKutusu.Yok);

                chk_mvSayisi.Checked     = CEkranSecenekleri.UstMvSayisi;
                chk_ittifakAktif.Checked = CEkranSecenekleri.IttifakAktif;

                rad_ilk2.Checked       = (CEkranSecenekleri.Ittifak == IttifakModu.Ilk2);
                rad_ikiArtiIki.Checked = (CEkranSecenekleri.Ittifak == IttifakModu.IkiArtiIki);
                rad_ilk3.Checked       = (CEkranSecenekleri.Ittifak == IttifakModu.Ilk3);
                rad_ikiArtiUc.Checked  = (CEkranSecenekleri.Ittifak == IttifakModu.IkiArtiUc);

                chk_kaynak.Checked = CEkranSecenekleri.KaynakGoster;
                txt_kaynak.Text    = CEkranSecenekleri.KaynakAd;

                chk_solAktif.Checked = CEkranSecenekleri.SolAktif;
                chk_ciftHane.Checked = CEkranSecenekleri.CiftHane;

                rad_sol2Aday.Checked = (CEkranSecenekleri.Sol == SolModu.Ilk2Aday);
                rad_sol3Aday.Checked = (CEkranSecenekleri.Sol == SolModu.Ilk3Aday);
                rad_solDongu.Checked = (CEkranSecenekleri.Sol == SolModu.IkiArtiIki);

                num_solSure1.Value = CEkranSecenekleri.SolSure1;
                num_solSure2.Value = CEkranSecenekleri.SolSure2;

                chk_altAktif.Checked = CEkranSecenekleri.AltAktif;
                num_altSure.Value    = CEkranSecenekleri.AltSure;

                chk_yasak.Checked        = CEkranSecenekleri.YasakModu;
                chk_acilanSandik.Checked = CEkranSecenekleri.AcilanSandikGoster;
            }
            finally
            {
                kutulariDolduruyor = false;
            }
        }

        /// <summary> Kutulardaki degerleri secenek nesnesine tasir. </summary>
        private void KutulardanOku()
        {
            CEkranSecenekleri.UstAktif       = chk_ustAktif.Checked;
            CEkranSecenekleri.UstSure1       = (int)num_ustSure1.Value;
            CEkranSecenekleri.UstSure2       = (int)num_ustSure2.Value;
            CEkranSecenekleri.UstIkinciSayfa = chk_ustIkinci.Checked;
            CEkranSecenekleri.UstDigerSatiri = chk_ustDiger.Checked;

            CEkranSecenekleri.UstMod = rad_mvOran.Checked
                ? UstGosterim.MvVeOran : UstGosterim.OyOrani;

            if (rad_baslikKo.Checked)       CEkranSecenekleri.UstBaslik = UstBaslikKutusu.Katilim;
            else if (rad_baslikYok.Checked) CEkranSecenekleri.UstBaslik = UstBaslikKutusu.Yok;
            else                            CEkranSecenekleri.UstBaslik = UstBaslikKutusu.AcilanSandik;

            CEkranSecenekleri.UstMvSayisi  = chk_mvSayisi.Checked;
            CEkranSecenekleri.IttifakAktif = chk_ittifakAktif.Checked;

            if (rad_ilk2.Checked)            CEkranSecenekleri.Ittifak = IttifakModu.Ilk2;
            else if (rad_ikiArtiIki.Checked) CEkranSecenekleri.Ittifak = IttifakModu.IkiArtiIki;
            else if (rad_ilk3.Checked)       CEkranSecenekleri.Ittifak = IttifakModu.Ilk3;
            else                             CEkranSecenekleri.Ittifak = IttifakModu.IkiArtiUc;

            CEkranSecenekleri.KaynakGoster = chk_kaynak.Checked;
            CEkranSecenekleri.KaynakAd     = txt_kaynak.Text.Trim();

            CEkranSecenekleri.SolAktif = chk_solAktif.Checked;
            CEkranSecenekleri.CiftHane = chk_ciftHane.Checked;

            if (rad_sol2Aday.Checked)      CEkranSecenekleri.Sol = SolModu.Ilk2Aday;
            else if (rad_sol3Aday.Checked) CEkranSecenekleri.Sol = SolModu.Ilk3Aday;
            else                           CEkranSecenekleri.Sol = SolModu.IkiArtiIki;

            CEkranSecenekleri.SolSure1 = (int)num_solSure1.Value;
            CEkranSecenekleri.SolSure2 = (int)num_solSure2.Value;

            CEkranSecenekleri.AltAktif = chk_altAktif.Checked;
            CEkranSecenekleri.AltSure  = (int)num_altSure.Value;

            CEkranSecenekleri.YasakModu          = chk_yasak.Checked;
            CEkranSecenekleri.AcilanSandikGoster = chk_acilanSandik.Checked;

            Secim cb = cmb_cb.SelectedItem as Secim;
            Secim mv = cmb_mv.SelectedItem as Secim;

            CEkranSecenekleri.SecimCB = cb == null ? "" : cb.Kod;
            CEkranSecenekleri.SecimMV = mv == null ? "" : mv.Kod;
        }

        /// <summary>
        /// Bir secenek degisti.
        ///
        /// Sayfa listeleri yeniden kuruluyor ama EKRANA DOKUNULMUYOR:
        /// degisiklik bir sonraki sayfa gecisinde yuruyor. Yayin
        /// sirasinda bir kutuya basildiginda goruntunun aninda sicramasi
        /// istenmez.
        /// </summary>
        private void Secenek_Degisti(object sender, EventArgs e)
        {
            if (kutulariDolduruyor) return;

            KutulardanOku();
            motor.SayfalariKur();
            motor.AyarlariUygula();

            DurumuTazele();
        }

        /// <summary>
        /// Yasak modu ve acilan sandik secenegi EKRANI HEMEN degistirir.
        /// Yasak basladiginda sayilarin bir sonraki sayfa gecisini
        /// beklemeden kaybolmasi gerekiyor.
        /// </summary>
        private void Yasak_Degisti(object sender, EventArgs e)
        {
            if (kutulariDolduruyor) return;

            KutulardanOku();

            if (chk_yasak.Checked) CLog.Log("YASAK MODU", "AÇIK - adaylar ve oranlar gizleniyor");
            else                   CLog.Log("YASAK MODU", "kapalı");

            foreach (Serit s in motor.Seritler) s.Tazele();

            DurumuTazele();
        }

        private void Secim_Degisti(object sender, EventArgs e)
        {
            if (kutulariDolduruyor) return;

            KutulardanOku();
            motor.AyarlariUygula();

            foreach (Serit s in motor.Seritler) s.Tazele();
        }

        /// <summary>
        /// Secim listelerini doldurur.
        ///
        /// Ayar dosyasinda bir kod yaziyorsa o secilir; yoksa her tipin
        /// EN YENI secimi one gelir. Uygulama ilk kez calistirildiginda
        /// operatorun liste karistirmasina gerek kalmasin diye.
        /// </summary>
        private void SecimleriYukle()
        {
            kutulariDolduruyor = true;

            try
            {
                cmb_cb.Items.Clear();
                cmb_mv.Items.Clear();

                Secim enYeniCB = null;
                Secim enYeniMV = null;

                foreach (Secim s in DataService.Veri.Secimler)
                {
                    if (s.IsCB)
                    {
                        cmb_cb.Items.Add(s);
                        if (enYeniCB == null || s.Yil > enYeniCB.Yil) enYeniCB = s;
                    }
                    else
                    {
                        cmb_mv.Items.Add(s);
                        if (enYeniMV == null || s.Yil > enYeniMV.Yil) enYeniMV = s;
                    }
                }

                Sec(cmb_cb, CEkranSecenekleri.SecimCB, enYeniCB);
                Sec(cmb_mv, CEkranSecenekleri.SecimMV, enYeniMV);
            }
            finally
            {
                kutulariDolduruyor = false;
            }

            KutulardanOku();
            motor.AyarlariUygula();
        }

        private static void Sec(ComboBox kutu, string kod, Secim varsayilan)
        {
            foreach (object oge in kutu.Items)
            {
                Secim s = oge as Secim;
                if (s != null && s.Kod == kod)
                {
                    kutu.SelectedItem = s;
                    return;
                }
            }

            if (varsayilan != null) kutu.SelectedItem = varsayilan;
            else if (kutu.Items.Count > 0) kutu.SelectedIndex = 0;
        }

        #endregion

        #region Il gruplari

        /// <summary>
        /// Il grubu kutularini olusturur.
        ///
        /// Gruplar "iller" dosyasindan geldigi icin sayilari sabit degil;
        /// kutular yan yana esit genislikte dagitiliyor. Designer'a sabit
        /// dort kutu koymak dosyaya yeni grup eklenince ise yaramazdi.
        /// </summary>
        private void IlKutulariniKur()
        {
            pnl_iller.Controls.Clear();
            ilKutulari.Clear();

            List<IlGrubu> gruplar = IlGruplari.Gruplar;
            if (gruplar.Count == 0) return;

            // Gruplar esit genislikte sutunlara dagitiliyor. Elle
            // koordinat vermek yerine TableLayoutPanel kullaniliyor:
            // grup sayisi "iller" dosyasina gore degisiyor ve pencere
            // yeniden boyutlandiginda sutunlarin da buyumesi gerekiyor.
            TableLayoutPanel tablo = new TableLayoutPanel();
            tablo.Dock = DockStyle.Fill;
            tablo.ColumnCount = gruplar.Count;
            tablo.RowCount = 1;

            foreach (IlGrubu grup in gruplar)
            {
                tablo.ColumnStyles.Add(
                    new ColumnStyle(SizeType.Percent, 100f / gruplar.Count));

                tablo.Controls.Add(GrupKutusu(grup));
            }

            pnl_iller.Controls.Add(tablo);
        }

        /// <summary>
        /// Bir il grubunun kutusu: ustte kota, ortada il listesi,
        /// altta toplu secim dugmeleri. Hepsi Dock ile yerlesiyor,
        /// pencere buyudugunde liste de buyuyor.
        /// </summary>
        private Control GrupKutusu(IlGrubu grup)
        {
            GroupBox kutu = new GroupBox();
            kutu.Text = grup.Ad + "   (" + grup.Iller.Count + ")";
            kutu.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            kutu.Dock = DockStyle.Fill;
            kutu.Margin = new Padding(3);
            kutu.Padding = new Padding(6, 4, 6, 6);

            CheckedListBox liste = new CheckedListBox();
            liste.CheckOnClick = true;
            liste.Dock = DockStyle.Fill;
            liste.Font = new Font("Segoe UI", 9F);
            liste.IntegralHeight = false;
            liste.Tag = grup;
            liste.ItemCheck += IlKutusu_ItemCheck;

            foreach (IlSatiri satir in grup.Iller)
                liste.Items.Add(satir, satir.Yayin);

            ilKutulari[grup] = liste;

            // Dock siralamasi: once Fill eklenir, Top ve Bottom sonra;
            // boylece liste ikisinin arasinda kalir.
            kutu.Controls.Add(liste);
            kutu.Controls.Add(DugmeSeridi(grup));
            kutu.Controls.Add(KotaSeridi(grup));

            return kutu;
        }

        private Control KotaSeridi(IlGrubu grup)
        {
            Panel serit = new Panel();
            serit.Dock = DockStyle.Top;
            serit.Height = 28;

            Label lbl = new Label();
            lbl.Text = "arka arkaya";
            lbl.Font = new Font("Segoe UI", 8.25F);
            lbl.Location = new Point(2, 6);
            lbl.Size = new Size(76, 16);

            NumericUpDown kota = new NumericUpDown();
            kota.Minimum = 0;
            kota.Maximum = 81;
            kota.Value = Math.Min(81, Math.Max(0, grup.Kota));
            kota.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            kota.Location = new Point(82, 2);
            kota.Size = new Size(54, 23);
            kota.Tag = grup;
            kota.ValueChanged += Kota_Degisti;

            serit.Controls.Add(lbl);
            serit.Controls.Add(kota);

            return serit;
        }

        private Control DugmeSeridi(IlGrubu grup)
        {
            TableLayoutPanel serit = new TableLayoutPanel();
            serit.Dock = DockStyle.Bottom;
            serit.Height = 28;
            serit.ColumnCount = 2;
            serit.RowCount = 1;
            serit.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            serit.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));

            serit.Controls.Add(YardimciDugme("Tümünü SEÇ", grup, true));
            serit.Controls.Add(YardimciDugme("Tümünü BIRAK", grup, false));

            return serit;
        }

        private Button YardimciDugme(string metin, IlGrubu grup, bool sec)
        {
            Button dugme = new Button();
            dugme.Text = metin;
            dugme.Font = new Font("Segoe UI", 8.25F);
            dugme.Dock = DockStyle.Fill;
            dugme.Margin = new Padding(1);

            dugme.Click += delegate
            {
                CheckedListBox liste = ilKutulari[grup];

                liste.BeginUpdate();
                for (int i = 0; i < liste.Items.Count; i++) liste.SetItemChecked(i, sec);
                liste.EndUpdate();
            };

            return dugme;
        }

        private void Kota_Degisti(object sender, EventArgs e)
        {
            NumericUpDown kutu = sender as NumericUpDown;
            if (kutu == null) return;

            IlGrubu grup = kutu.Tag as IlGrubu;
            if (grup == null) return;

            grup.Kota = (int)kutu.Value;

            CLog.Detail("KOTA DEGISTI", grup.Ad + " -> " + grup.Kota);
        }

        /// <summary>
        /// ItemCheck kutunun ESKI halinde tetikleniyor; yeni deger
        /// e.NewValue'da geliyor, listeden okumak yanlis sonuc verir.
        /// </summary>
        private void IlKutusu_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            CheckedListBox liste = sender as CheckedListBox;
            if (liste == null) return;

            IlSatiri satir = liste.Items[e.Index] as IlSatiri;
            if (satir == null) return;

            satir.Yayin = (e.NewValue == CheckState.Checked);
        }

        /// <summary> Listede secili satir ve grubu. Hicbiri secili degilse false. </summary>
        private bool SeciliIl(out IlGrubu grup, out IlSatiri il)
        {
            foreach (KeyValuePair<IlGrubu, CheckedListBox> giris in ilKutulari)
            {
                IlSatiri satir = giris.Value.SelectedItem as IlSatiri;

                if (satir != null)
                {
                    grup = giris.Key;
                    il   = satir;
                    return true;
                }
            }

            grup = null;
            il   = null;

            MessageBox.Show("Önce listeden bir il seçin.");
            return false;
        }

        /// <summary> Donus secilen ilden devam eder. </summary>
        private void SecilendenDevam_Click(object sender, EventArgs e)
        {
            IlGrubu grup;
            IlSatiri il;

            if (!SeciliIl(out grup, out il)) return;

            motor.IldenDevam(grup, il);
            DurumuTazele();
        }

        /// <summary>
        /// Secilen ile gidip ORADA KALIR: alt ve ust seritler sayfa
        /// degistirmeye devam eder ama il ilerlemez.
        /// </summary>
        private void SecileneGit_Click(object sender, EventArgs e)
        {
            IlGrubu grup;
            IlSatiri il;

            if (!SeciliIl(out grup, out il)) return;

            motor.IlSabitle(grup, il);
            DurumuTazele();
        }

        /// <summary> Sabitlemeyi kaldirir, il yeniden donmeye baslar. </summary>
        private void Devam_Click(object sender, EventArgs e)
        {
            motor.IlSabitlemeyiKaldir();
            DurumuTazele();
        }

        #endregion

        #region Veri kaynagi

        /// <summary>
        /// Acilista veri nereden gelecek.
        ///
        /// API modunda bile once ONBELLEK dosyasi okunuyor. C ekrani 24 saat
        /// calisiyor ve gece yarisi yeniden baslatilabiliyor; API o an kapaliysa
        /// bos ekranla acilmak kabul edilemez. Gercek veri birkac saniye sonra
        /// arka planda gelip uzerine yaziyor.
        /// </summary>
        private void VeriyiIlkYukle()
        {
            if (ApiAyarlari.ApiKullan && File.Exists(ConfigPaths.CacheFile))
            {
                if (DataService.DosyadanYukle(ConfigPaths.CacheFile))
                {
                    CLog.Log("VERI ONBELLEKTEN", ConfigPaths.CacheFile);
                    return;
                }

                CLog.Error("ONBELLEK OKUNAMADI", "API'den beklenecek");
            }

            if (!ApiAyarlari.ApiKullan) DataService.Load();
        }

        private void VeriKaynaginiKur()
        {
            VeriKaynagi.OnYeniVeri += VeriKaynagi_OnYeniVeri;
            VeriKaynagi.OnDurum    += VeriKaynagi_OnDurum;

            VeriKaynagi.Baslat();
            VeriGostergesiniTazele();
        }

        /// <summary>
        /// Arka planda yeni veri cozuldu.
        ///
        /// Nesneyi yerine koymak index sozluklerini bastan kuruyor; bu is UI
        /// thread'inde yapilmali, yoksa tam o anda okuyan donus motoru yarim
        /// tabloyla karsilasir.
        ///
        /// EKRANA DOKUNULMUYOR: yeni veri bir sonraki sayfa gecisinde
        /// kendiliginden yansiyor, operator onayi beklenmiyor.
        /// </summary>
        private void VeriKaynagi_OnYeniVeri(SecimVerisi veri, int surum, DateTime guncelleme)
        {
            SafeInvoke(delegate
            {
                DataService.Uygula(veri);
                VeriGostergesiniTazele();
            });
        }

        private void VeriKaynagi_OnDurum(KaynakDurumu durum, string aciklama)
        {
            SafeInvoke(VeriGostergesiniTazele);
        }

        private void VeriGostergesiniTazele()
        {
            if (!ApiAyarlari.ApiKullan)
            {
                lbl_veri.Text = "YEREL VERİ";
                lbl_veri.BackColor = Color.Gray;
                return;
            }

            switch (VeriKaynagi.Durum)
            {
                case KaynakDurumu.Bagli:
                    lbl_veri.Text = "VERİ  " + VeriKaynagi.SonGuncelleme.ToString("HH:mm") +
                                    "   (sürüm " + VeriKaynagi.SonSurum + ")";
                    lbl_veri.BackColor = Color.FromArgb(26, 160, 74);
                    break;

                case KaynakDurumu.Kopuk:
                    lbl_veri.Text = "API YOK";
                    lbl_veri.BackColor = Color.FromArgb(176, 48, 44);
                    break;

                default:
                    lbl_veri.Text = "VERİ BEKLENİYOR";
                    lbl_veri.BackColor = Color.Gray;
                    break;
            }
        }

        #endregion

        #region Baglanti ve sahne

        private void EngineleriYukle()
        {
            cmb_engine.Items.Clear();

            foreach (EngineInfo en in EngineRepository.Engines)
                cmb_engine.Items.Add(en);

            if (cmb_engine.Items.Count > 0) cmb_engine.SelectedIndex = 0;
        }

        private void Baglan_Click(object sender, EventArgs e)
        {
            if (engine.isConnected)
            {
                CLog.Log("OPERATOR", "BAGLANTIYI KES");
                engine.Disconnect();
                DurumuTazele();
                return;
            }

            EngineInfo secili = cmb_engine.SelectedItem as EngineInfo;

            if (secili == null)
            {
                MessageBox.Show("Önce listeden bir engine seçin.");
                return;
            }

            CLog.Log("OPERATOR", "BAGLAN " + secili.IP + ":" + secili.Port);
            engine.Connect(secili);

            DurumuTazele();
        }

        /// <summary>
        /// Baglanti kurulunca: sahne dosyasinda istenmisse sahneyi yukler.
        ///
        /// Yayin makinesinde OTOMATIK_YUKLE kapali olmali - sahne zaten
        /// ekranda dururken yeniden yuklemek goruntuyu sifirlar.
        /// </summary>
        private void Engine_OnConnected(VizEngine en)
        {
            SafeInvoke(delegate
            {
                DurumuTazele();

                if (!SahneAyarlari.OtomatikYukle) return;
                if (SahneAyarlari.SahneYolu.Length == 0) return;

                CLog.Log("SAHNE YUKLENIYOR", SahneAyarlari.SahneYolu);
                engine.Send(SahneAyarlari.YuklemeKomutu());
            });
        }

        private void Engine_OnDisconnected(VizEngine en)
        {
            SafeInvoke(delegate
            {
                // Engine gidince donusu durdurmak sart: zamanlayicilar
                // calismaya devam eder, her sayfada olmayan bir baglantiya
                // komut gondermeye calisip log'u hata ile doldurur.
                if (motor.Yayinda)
                {
                    CLog.Error("YAYIN DURDURULDU", "engine baglantisi koptu");
                    motor.Al();
                }

                DurumuTazele();
            });
        }

        /// <summary>
        /// Sahnenin stage'ini okur ve director listesini doldurur.
        ///
        /// Engine yeniden baslarsa ya da sahne yeniden yuklenirse butun id'ler
        /// degisir; o durumda bunun tekrar calismasi sart, yoksa uygulama
        /// sessizce olmayan id'lere komut gondermeye baslar.
        /// </summary>
        private void Oku_Click(object sender, EventArgs e)
        {
            if (!engine.isConnected)
            {
                MessageBox.Show("Önce engine'e bağlanın.");
                return;
            }

            Cursor = Cursors.WaitCursor;

            try
            {
                if (!sahne.SahneyiOku())
                {
                    lbl_indeks.Text = "okunamadı";
                    return;
                }

                lbl_indeks.Text = sahne.Indeks.Sayi + " director";

                if (CEkranSecenekleri.OtomatikBasla && !motor.Yayinda && DataService.Yuklendi)
                {
                    CLog.Log("YAYIN", "otomatik veriliyor");
                    motor.Ver();
                }
            }
            finally
            {
                Cursor = Cursors.Default;
                DurumuTazele();
            }
        }

        private void Ver_Click(object sender, EventArgs e)
        {
            if (!sahne.Hazir)
            {
                MessageBox.Show("Önce SAHNEYİ OKU.");
                return;
            }

            if (!DataService.Yuklendi)
            {
                MessageBox.Show("Veri yüklenmedi. API çalışıyor mu?");
                return;
            }

            if (!IlGruplari.Hazir)
            {
                MessageBox.Show("Dönüşe girecek il yok.\n\n" +
                                "Bir grubun \"arka arkaya\" sayısını 0'dan büyük yapın " +
                                "ve en az bir ilin kutusunu işaretleyin.");
                return;
            }

            motor.Ver();
            DurumuTazele();
        }

        #endregion

        #region Durum gostergesi

        private void Motor_OnSayfaDegisti(Serit serit, CEkranSayfasi sayfa)
        {
            DurumuTazele();
        }

        private void DurumuTazele()
        {
            bool bagli = engine.isConnected;

            btn_baglan.Text     = bagli ? "BAĞLANTIYI KES" : "BAĞLAN";
            lbl_durum.Text      = bagli ? "BAĞLI" : "BAĞLI DEĞİL";
            lbl_durum.BackColor = bagli ? Color.FromArgb(26, 160, 74) : Color.FromArgb(176, 48, 44);
            cmb_engine.Enabled  = !bagli;

            bool yayinda = motor.Yayinda;

            lbl_onair.Text      = yayinda ? "EKRANDA" : "EKRANDA DEĞİL";
            lbl_onair.BackColor = yayinda ? Color.FromArgb(176, 48, 44) : Color.DimGray;

            btn_hazirla.Enabled = bagli && !yayinda;

            lbl_altDurum.Text = SeritYazisi(motor.Alt);
            lbl_ustDurum.Text = SeritYazisi(motor.Ust);
            lbl_solDurum.Text = SeritYazisi(motor.Sol);

            btn_devam.Enabled = motor.IlSabit;
        }

        /// <summary>
        /// "ALT  ALT partiler       ANKARA  5sn"
        /// SOL hep Turkiye genelini gosteriyor, il yazisi ona yazilmiyor.
        /// </summary>
        private static string SeritYazisi(Serit serit)
        {
            if (!serit.Donuyor || serit.Sayfa == null)
                return serit.Ad.PadRight(5) + "—";

            string il = (serit.Ad == "SOL") ? "TÜRKİYE" : DonusMotoru.Baglam.IlAdi;

            return serit.Ad.PadRight(5) +
                   serit.Sayfa.Ad.PadRight(16) +
                   il.PadRight(14) +
                   serit.Sure + "sn";
        }

        #endregion

        #region Log konsolu

        /// <summary>
        /// Log satirlari dogrudan ListBox'a eklenmiyor, kuyruga giriyor.
        /// Tek pakette onlarca komut gonderildiginde her satir icin ayri
        /// ekleme + kaydirma yapmak ekrani gereksiz yere cizdiriyor.
        /// </summary>
        private readonly List<string> logKuyrugu = new List<string>();
        private readonly Timer logZamanlayici = new Timer();

        private void LogKonsolunuKur()
        {
            logZamanlayici.Interval = 250;
            logZamanlayici.Tick += LogKonsolunuTazele;
            logZamanlayici.Start();
        }

        private void CLog_OnLogWritten(string satir)
        {
            lock (logKuyrugu) logKuyrugu.Add(satir);
        }

        private void LogKonsolunuTazele(object sender, EventArgs e)
        {
            string[] yeni;

            lock (logKuyrugu)
            {
                if (logKuyrugu.Count == 0) return;

                yeni = logKuyrugu.ToArray();
                logKuyrugu.Clear();
            }

            lst_log.BeginUpdate();

            if (lst_log.Items.Count + yeni.Length > 500) lst_log.Items.Clear();

            lst_log.Items.AddRange(yeni);
            lst_log.TopIndex = lst_log.Items.Count - 1;

            lst_log.EndUpdate();
        }

        #endregion

        /// <summary>
        /// Soket thread'inden gelen olaylari UI thread'ine tasir.
        /// (Bu olmadan "cross-thread operation" hatasi alinir.)
        /// </summary>
        private void SafeInvoke(Action is_)
        {
            if (IsDisposed || !IsHandleCreated) return;

            if (InvokeRequired) BeginInvoke(is_);
            else is_();
        }
    }
}
