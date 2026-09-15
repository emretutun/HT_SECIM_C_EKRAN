namespace HT_SECIM_C_EKRAN
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Arayuz uc sutuna ayrilmis:
        ///
        ///   sol   - uc seridin (UST / SOL / ALT) kendi secenekleri
        ///   orta  - il gruplari; kutular "iller" dosyasina gore
        ///           CALISMA ANINDA olusturuluyor, burada yalnizca
        ///           tasiyici panel var
        ///   sag   - baglanti, secim, yayin kontrolu ve durum
        ///
        /// Altta tam genislikte log konsolu.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            this.pnl_orta = new System.Windows.Forms.Panel();
            this.pnl_iller = new System.Windows.Forms.Panel();
            this.pnl_ilDugme = new System.Windows.Forms.Panel();
            this.btn_secilendenDevam = new System.Windows.Forms.Button();
            this.btn_secileneGit = new System.Windows.Forms.Button();
            this.btn_devam = new System.Windows.Forms.Button();

            this.pnl_sol = new System.Windows.Forms.Panel();
            this.grp_ust = new System.Windows.Forms.GroupBox();
            this.chk_ustAktif = new System.Windows.Forms.CheckBox();
            this.lbl_ustSure1 = new System.Windows.Forms.Label();
            this.num_ustSure1 = new System.Windows.Forms.NumericUpDown();
            this.lbl_ustSure2 = new System.Windows.Forms.Label();
            this.num_ustSure2 = new System.Windows.Forms.NumericUpDown();
            this.chk_ustIkinci = new System.Windows.Forms.CheckBox();
            this.chk_ustDiger = new System.Windows.Forms.CheckBox();
            this.rad_oyOrani = new System.Windows.Forms.RadioButton();
            this.rad_mvOran = new System.Windows.Forms.RadioButton();
            this.chk_mvSayisi = new System.Windows.Forms.CheckBox();
            this.pnl_ustGosterim = new System.Windows.Forms.Panel();
            this.pnl_ustBaslik = new System.Windows.Forms.Panel();
            this.lbl_ustBaslik = new System.Windows.Forms.Label();
            this.rad_baslikAss = new System.Windows.Forms.RadioButton();
            this.rad_baslikKo = new System.Windows.Forms.RadioButton();
            this.rad_baslikYok = new System.Windows.Forms.RadioButton();
            this.grp_ittifak = new System.Windows.Forms.GroupBox();
            this.chk_ittifakAktif = new System.Windows.Forms.CheckBox();
            this.rad_ilk2 = new System.Windows.Forms.RadioButton();
            this.rad_ikiArtiIki = new System.Windows.Forms.RadioButton();
            this.rad_ilk3 = new System.Windows.Forms.RadioButton();
            this.rad_ikiArtiUc = new System.Windows.Forms.RadioButton();
            this.chk_kaynak = new System.Windows.Forms.CheckBox();
            this.txt_kaynak = new System.Windows.Forms.TextBox();

            this.grp_solSerit = new System.Windows.Forms.GroupBox();
            this.chk_solAktif = new System.Windows.Forms.CheckBox();
            this.chk_ciftHane = new System.Windows.Forms.CheckBox();
            this.rad_sol2Aday = new System.Windows.Forms.RadioButton();
            this.rad_sol3Aday = new System.Windows.Forms.RadioButton();
            this.rad_solDongu = new System.Windows.Forms.RadioButton();
            this.lbl_solSure1 = new System.Windows.Forms.Label();
            this.num_solSure1 = new System.Windows.Forms.NumericUpDown();
            this.lbl_solSure2 = new System.Windows.Forms.Label();
            this.num_solSure2 = new System.Windows.Forms.NumericUpDown();

            this.grp_altSerit = new System.Windows.Forms.GroupBox();
            this.chk_altAktif = new System.Windows.Forms.CheckBox();
            this.lbl_altSure = new System.Windows.Forms.Label();
            this.num_altSure = new System.Windows.Forms.NumericUpDown();

            this.pnl_sag = new System.Windows.Forms.Panel();
            this.grp_baglanti = new System.Windows.Forms.GroupBox();
            this.cmb_engine = new System.Windows.Forms.ComboBox();
            this.btn_baglan = new System.Windows.Forms.Button();
            this.lbl_durum = new System.Windows.Forms.Label();
            this.btn_oku = new System.Windows.Forms.Button();
            this.lbl_indeks = new System.Windows.Forms.Label();
            this.lbl_sahne = new System.Windows.Forms.Label();

            this.grp_secimKutu = new System.Windows.Forms.GroupBox();
            this.lbl_cb = new System.Windows.Forms.Label();
            this.cmb_cb = new System.Windows.Forms.ComboBox();
            this.lbl_mv = new System.Windows.Forms.Label();
            this.cmb_mv = new System.Windows.Forms.ComboBox();
            this.lbl_veri = new System.Windows.Forms.Label();

            this.grp_yayinSecenek = new System.Windows.Forms.GroupBox();
            this.chk_yasak = new System.Windows.Forms.CheckBox();
            this.chk_acilanSandik = new System.Windows.Forms.CheckBox();

            this.grp_durum = new System.Windows.Forms.GroupBox();
            this.lbl_altDurum = new System.Windows.Forms.Label();
            this.lbl_ustDurum = new System.Windows.Forms.Label();
            this.lbl_solDurum = new System.Windows.Forms.Label();
            this.btn_altAtla = new System.Windows.Forms.Button();
            this.btn_ustAtla = new System.Windows.Forms.Button();
            this.btn_solAtla = new System.Windows.Forms.Button();

            this.pnl_yayin = new System.Windows.Forms.Panel();
            this.lbl_onair = new System.Windows.Forms.Label();
            this.btn_hazirla = new System.Windows.Forms.Button();
            this.btn_ver = new System.Windows.Forms.Button();
            this.btn_al = new System.Windows.Forms.Button();

            this.lst_log = new System.Windows.Forms.ListBox();

            this.pnl_orta.SuspendLayout();
            this.pnl_ilDugme.SuspendLayout();
            this.pnl_sol.SuspendLayout();
            this.grp_ust.SuspendLayout();
            this.grp_ittifak.SuspendLayout();
            this.pnl_ustGosterim.SuspendLayout();
            this.pnl_ustBaslik.SuspendLayout();
            this.grp_solSerit.SuspendLayout();
            this.grp_altSerit.SuspendLayout();
            this.pnl_sag.SuspendLayout();
            this.grp_baglanti.SuspendLayout();
            this.grp_secimKutu.SuspendLayout();
            this.grp_yayinSecenek.SuspendLayout();
            this.grp_durum.SuspendLayout();
            this.pnl_yayin.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_ustSure1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_ustSure2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_solSure1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_solSure2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_altSure)).BeginInit();
            this.SuspendLayout();

            // ================================================== SOL SUTUN

            //
            // grp_ust
            //
            this.grp_ust.Controls.Add(this.txt_kaynak);
            this.grp_ust.Controls.Add(this.chk_kaynak);
            this.grp_ust.Controls.Add(this.grp_ittifak);
            this.grp_ust.Controls.Add(this.chk_mvSayisi);
            this.grp_ust.Controls.Add(this.pnl_ustBaslik);
            this.grp_ust.Controls.Add(this.pnl_ustGosterim);
            this.grp_ust.Controls.Add(this.chk_ustDiger);
            this.grp_ust.Controls.Add(this.chk_ustIkinci);
            this.grp_ust.Controls.Add(this.num_ustSure2);
            this.grp_ust.Controls.Add(this.lbl_ustSure2);
            this.grp_ust.Controls.Add(this.num_ustSure1);
            this.grp_ust.Controls.Add(this.lbl_ustSure1);
            this.grp_ust.Controls.Add(this.chk_ustAktif);
            this.grp_ust.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grp_ust.Location = new System.Drawing.Point(10, 8);
            this.grp_ust.Name = "grp_ust";
            this.grp_ust.Size = new System.Drawing.Size(308, 392);
            this.grp_ust.TabIndex = 0;
            this.grp_ust.TabStop = false;
            this.grp_ust.Text = "ÜST ŞERİT";
            //
            // chk_ustAktif
            //
            this.chk_ustAktif.AutoSize = true;
            this.chk_ustAktif.Location = new System.Drawing.Point(14, 24);
            this.chk_ustAktif.Name = "chk_ustAktif";
            this.chk_ustAktif.Size = new System.Drawing.Size(56, 19);
            this.chk_ustAktif.TabIndex = 0;
            this.chk_ustAktif.Text = "AKTİF";
            this.chk_ustAktif.UseVisualStyleBackColor = true;
            //
            // lbl_ustSure1
            //
            this.lbl_ustSure1.AutoSize = true;
            this.lbl_ustSure1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lbl_ustSure1.Location = new System.Drawing.Point(12, 56);
            this.lbl_ustSure1.Name = "lbl_ustSure1";
            this.lbl_ustSure1.Size = new System.Drawing.Size(140, 15);
            this.lbl_ustSure1.TabIndex = 1;
            this.lbl_ustSure1.Text = "1. sayfa ekran süresi (sn)";
            //
            // num_ustSure1
            //
            this.num_ustSure1.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.num_ustSure1.Location = new System.Drawing.Point(232, 52);
            this.num_ustSure1.Maximum = new decimal(new int[] { 600, 0, 0, 0 });
            this.num_ustSure1.Minimum = new decimal(new int[] { 3, 0, 0, 0 });
            this.num_ustSure1.Name = "num_ustSure1";
            this.num_ustSure1.Size = new System.Drawing.Size(60, 25);
            this.num_ustSure1.TabIndex = 2;
            this.num_ustSure1.Value = new decimal(new int[] { 3, 0, 0, 0 });
            //
            // lbl_ustSure2
            //
            this.lbl_ustSure2.AutoSize = true;
            this.lbl_ustSure2.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lbl_ustSure2.Location = new System.Drawing.Point(12, 88);
            this.lbl_ustSure2.Name = "lbl_ustSure2";
            this.lbl_ustSure2.Size = new System.Drawing.Size(140, 15);
            this.lbl_ustSure2.TabIndex = 3;
            this.lbl_ustSure2.Text = "2. sayfa ekran süresi (sn)";
            //
            // num_ustSure2
            //
            this.num_ustSure2.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.num_ustSure2.Location = new System.Drawing.Point(232, 84);
            this.num_ustSure2.Maximum = new decimal(new int[] { 600, 0, 0, 0 });
            this.num_ustSure2.Minimum = new decimal(new int[] { 3, 0, 0, 0 });
            this.num_ustSure2.Name = "num_ustSure2";
            this.num_ustSure2.Size = new System.Drawing.Size(60, 25);
            this.num_ustSure2.TabIndex = 4;
            this.num_ustSure2.Value = new decimal(new int[] { 5, 0, 0, 0 });
            //
            // chk_ustIkinci
            //
            this.chk_ustIkinci.AutoSize = true;
            this.chk_ustIkinci.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.chk_ustIkinci.Location = new System.Drawing.Point(14, 118);
            this.chk_ustIkinci.Name = "chk_ustIkinci";
            this.chk_ustIkinci.Size = new System.Drawing.Size(180, 19);
            this.chk_ustIkinci.TabIndex = 5;
            this.chk_ustIkinci.Text = "İkinci sayfa (kalan partiler)";
            this.chk_ustIkinci.UseVisualStyleBackColor = true;
            //
            // chk_ustDiger
            //
            this.chk_ustDiger.AutoSize = true;
            this.chk_ustDiger.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.chk_ustDiger.Location = new System.Drawing.Point(14, 142);
            this.chk_ustDiger.Name = "chk_ustDiger";
            this.chk_ustDiger.Size = new System.Drawing.Size(190, 19);
            this.chk_ustDiger.TabIndex = 6;
            this.chk_ustDiger.Text = "Son satır \"DİĞER\" olarak toplansın";
            this.chk_ustDiger.UseVisualStyleBackColor = true;
            //
            // rad_oyOrani
            //
            this.rad_oyOrani.AutoSize = true;
            this.rad_oyOrani.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.rad_oyOrani.Location = new System.Drawing.Point(2, 2);
            this.rad_oyOrani.Name = "rad_oyOrani";
            this.rad_oyOrani.Size = new System.Drawing.Size(130, 19);
            this.rad_oyOrani.TabIndex = 7;
            this.rad_oyOrani.Text = "OY ORANI (6 satır)";
            this.rad_oyOrani.UseVisualStyleBackColor = true;
            //
            // rad_mvOran
            //
            this.rad_mvOran.AutoSize = true;
            this.rad_mvOran.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.rad_mvOran.Location = new System.Drawing.Point(146, 2);
            this.rad_mvOran.Name = "rad_mvOran";
            this.rad_mvOran.Size = new System.Drawing.Size(134, 19);
            this.rad_mvOran.TabIndex = 8;
            this.rad_mvOran.Text = "MV ve ORAN (5 satır)";
            this.rad_mvOran.UseVisualStyleBackColor = true;
            //
            // pnl_ustGosterim
            //
            // Gosterim radyolari kendi panelinde duruyor. Ayni kapta olan
            // RadioButton'lar TEK GRUP sayiliyor; basliktaki radyolarla
            // ayni yere konsalardi birbirlerini kapatirlardi.
            //
            this.pnl_ustGosterim.Controls.Add(this.rad_mvOran);
            this.pnl_ustGosterim.Controls.Add(this.rad_oyOrani);
            this.pnl_ustGosterim.Location = new System.Drawing.Point(12, 170);
            this.pnl_ustGosterim.Name = "pnl_ustGosterim";
            this.pnl_ustGosterim.Size = new System.Drawing.Size(286, 26);
            this.pnl_ustGosterim.TabIndex = 7;
            //
            // pnl_ustBaslik
            //
            this.pnl_ustBaslik.Controls.Add(this.rad_baslikYok);
            this.pnl_ustBaslik.Controls.Add(this.rad_baslikKo);
            this.pnl_ustBaslik.Controls.Add(this.rad_baslikAss);
            this.pnl_ustBaslik.Controls.Add(this.lbl_ustBaslik);
            this.pnl_ustBaslik.Location = new System.Drawing.Point(12, 226);
            this.pnl_ustBaslik.Name = "pnl_ustBaslik";
            this.pnl_ustBaslik.Size = new System.Drawing.Size(286, 58);
            this.pnl_ustBaslik.TabIndex = 10;
            //
            // lbl_ustBaslik
            //
            this.lbl_ustBaslik.AutoSize = true;
            this.lbl_ustBaslik.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.lbl_ustBaslik.ForeColor = System.Drawing.Color.DimGray;
            this.lbl_ustBaslik.Location = new System.Drawing.Point(2, 2);
            this.lbl_ustBaslik.Name = "lbl_ustBaslik";
            this.lbl_ustBaslik.Size = new System.Drawing.Size(240, 13);
            this.lbl_ustBaslik.TabIndex = 0;
            this.lbl_ustBaslik.Text = "Başlık kutusu — ikisi sahnede aynı yerde:";
            //
            // rad_baslikAss
            //
            this.rad_baslikAss.AutoSize = true;
            this.rad_baslikAss.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.rad_baslikAss.Location = new System.Drawing.Point(2, 22);
            this.rad_baslikAss.Name = "rad_baslikAss";
            this.rad_baslikAss.Size = new System.Drawing.Size(118, 19);
            this.rad_baslikAss.TabIndex = 1;
            this.rad_baslikAss.Text = "AÇILAN SANDIK";
            this.rad_baslikAss.UseVisualStyleBackColor = true;
            //
            // rad_baslikKo
            //
            this.rad_baslikKo.AutoSize = true;
            this.rad_baslikKo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.rad_baslikKo.Location = new System.Drawing.Point(128, 22);
            this.rad_baslikKo.Name = "rad_baslikKo";
            this.rad_baslikKo.Size = new System.Drawing.Size(90, 19);
            this.rad_baslikKo.TabIndex = 2;
            this.rad_baslikKo.Text = "KATILIM";
            this.rad_baslikKo.UseVisualStyleBackColor = true;
            //
            // rad_baslikYok
            //
            this.rad_baslikYok.AutoSize = true;
            this.rad_baslikYok.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.rad_baslikYok.Location = new System.Drawing.Point(222, 22);
            this.rad_baslikYok.Name = "rad_baslikYok";
            this.rad_baslikYok.Size = new System.Drawing.Size(60, 19);
            this.rad_baslikYok.TabIndex = 3;
            this.rad_baslikYok.Text = "YOK";
            this.rad_baslikYok.UseVisualStyleBackColor = true;
            //
            // chk_mvSayisi
            //
            this.chk_mvSayisi.AutoSize = true;
            this.chk_mvSayisi.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.chk_mvSayisi.Location = new System.Drawing.Point(14, 200);
            this.chk_mvSayisi.Name = "chk_mvSayisi";
            this.chk_mvSayisi.Size = new System.Drawing.Size(210, 19);
            this.chk_mvSayisi.TabIndex = 9;
            this.chk_mvSayisi.Text = "Ayrıca MV SAYISI sayfaları da dönsün";
            this.chk_mvSayisi.UseVisualStyleBackColor = true;
            //
            // grp_ittifak
            //
            this.grp_ittifak.Controls.Add(this.rad_ikiArtiUc);
            this.grp_ittifak.Controls.Add(this.rad_ilk3);
            this.grp_ittifak.Controls.Add(this.rad_ikiArtiIki);
            this.grp_ittifak.Controls.Add(this.rad_ilk2);
            this.grp_ittifak.Controls.Add(this.chk_ittifakAktif);
            this.grp_ittifak.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.grp_ittifak.Location = new System.Drawing.Point(10, 290);
            this.grp_ittifak.Name = "grp_ittifak";
            this.grp_ittifak.Size = new System.Drawing.Size(288, 68);
            this.grp_ittifak.TabIndex = 10;
            this.grp_ittifak.TabStop = false;
            this.grp_ittifak.Text = "İTTİFAK SAYFALARI";
            //
            // chk_ittifakAktif
            //
            this.chk_ittifakAktif.AutoSize = true;
            this.chk_ittifakAktif.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.chk_ittifakAktif.Location = new System.Drawing.Point(10, 18);
            this.chk_ittifakAktif.Name = "chk_ittifakAktif";
            this.chk_ittifakAktif.Size = new System.Drawing.Size(56, 19);
            this.chk_ittifakAktif.TabIndex = 0;
            this.chk_ittifakAktif.Text = "AKTİF";
            this.chk_ittifakAktif.UseVisualStyleBackColor = true;
            //
            // rad_ilk2
            //
            this.rad_ilk2.AutoSize = true;
            this.rad_ilk2.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.rad_ilk2.Location = new System.Drawing.Point(90, 18);
            this.rad_ilk2.Name = "rad_ilk2";
            this.rad_ilk2.Size = new System.Drawing.Size(70, 19);
            this.rad_ilk2.TabIndex = 1;
            this.rad_ilk2.Text = "İLK 2";
            this.rad_ilk2.UseVisualStyleBackColor = true;
            //
            // rad_ikiArtiIki
            //
            this.rad_ikiArtiIki.AutoSize = true;
            this.rad_ikiArtiIki.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.rad_ikiArtiIki.Location = new System.Drawing.Point(196, 18);
            this.rad_ikiArtiIki.Name = "rad_ikiArtiIki";
            this.rad_ikiArtiIki.Size = new System.Drawing.Size(70, 19);
            this.rad_ikiArtiIki.TabIndex = 2;
            this.rad_ikiArtiIki.Text = "2 + 2";
            this.rad_ikiArtiIki.UseVisualStyleBackColor = true;
            //
            // rad_ilk3
            //
            this.rad_ilk3.AutoSize = true;
            this.rad_ilk3.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.rad_ilk3.Location = new System.Drawing.Point(90, 42);
            this.rad_ilk3.Name = "rad_ilk3";
            this.rad_ilk3.Size = new System.Drawing.Size(70, 19);
            this.rad_ilk3.TabIndex = 3;
            this.rad_ilk3.Text = "İLK 3";
            this.rad_ilk3.UseVisualStyleBackColor = true;
            //
            // rad_ikiArtiUc
            //
            this.rad_ikiArtiUc.AutoSize = true;
            this.rad_ikiArtiUc.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.rad_ikiArtiUc.Location = new System.Drawing.Point(196, 42);
            this.rad_ikiArtiUc.Name = "rad_ikiArtiUc";
            this.rad_ikiArtiUc.Size = new System.Drawing.Size(70, 19);
            this.rad_ikiArtiUc.TabIndex = 4;
            this.rad_ikiArtiUc.Text = "2 + 3";
            this.rad_ikiArtiUc.UseVisualStyleBackColor = true;
            //
            // chk_kaynak
            //
            this.chk_kaynak.AutoSize = true;
            this.chk_kaynak.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.chk_kaynak.Location = new System.Drawing.Point(14, 364);
            this.chk_kaynak.Name = "chk_kaynak";
            this.chk_kaynak.Size = new System.Drawing.Size(110, 19);
            this.chk_kaynak.TabIndex = 11;
            this.chk_kaynak.Text = "KAYNAK GÖSTER";
            this.chk_kaynak.UseVisualStyleBackColor = true;
            //
            // txt_kaynak
            //
            this.txt_kaynak.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txt_kaynak.Location = new System.Drawing.Point(150, 362);
            this.txt_kaynak.Name = "txt_kaynak";
            this.txt_kaynak.Size = new System.Drawing.Size(142, 23);
            this.txt_kaynak.TabIndex = 12;
            //
            // grp_solSerit
            //
            this.grp_solSerit.Controls.Add(this.num_solSure2);
            this.grp_solSerit.Controls.Add(this.lbl_solSure2);
            this.grp_solSerit.Controls.Add(this.num_solSure1);
            this.grp_solSerit.Controls.Add(this.lbl_solSure1);
            this.grp_solSerit.Controls.Add(this.rad_solDongu);
            this.grp_solSerit.Controls.Add(this.rad_sol3Aday);
            this.grp_solSerit.Controls.Add(this.rad_sol2Aday);
            this.grp_solSerit.Controls.Add(this.chk_ciftHane);
            this.grp_solSerit.Controls.Add(this.chk_solAktif);
            this.grp_solSerit.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grp_solSerit.Location = new System.Drawing.Point(10, 408);
            this.grp_solSerit.Name = "grp_solSerit";
            this.grp_solSerit.Size = new System.Drawing.Size(308, 210);
            this.grp_solSerit.TabIndex = 1;
            this.grp_solSerit.TabStop = false;
            this.grp_solSerit.Text = "SOL ŞERİT  (her zaman Türkiye geneli)";
            //
            // chk_solAktif
            //
            this.chk_solAktif.AutoSize = true;
            this.chk_solAktif.Location = new System.Drawing.Point(14, 24);
            this.chk_solAktif.Name = "chk_solAktif";
            this.chk_solAktif.Size = new System.Drawing.Size(56, 19);
            this.chk_solAktif.TabIndex = 0;
            this.chk_solAktif.Text = "AKTİF";
            this.chk_solAktif.UseVisualStyleBackColor = true;
            //
            // chk_ciftHane
            //
            this.chk_ciftHane.AutoSize = true;
            this.chk_ciftHane.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.chk_ciftHane.Location = new System.Drawing.Point(120, 24);
            this.chk_ciftHane.Name = "chk_ciftHane";
            this.chk_ciftHane.Size = new System.Drawing.Size(180, 19);
            this.chk_ciftHane.TabIndex = 1;
            this.chk_ciftHane.Text = "ÇİFT HANE  (%52,43)";
            this.chk_ciftHane.UseVisualStyleBackColor = true;
            //
            // rad_sol2Aday
            //
            this.rad_sol2Aday.AutoSize = true;
            this.rad_sol2Aday.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.rad_sol2Aday.Location = new System.Drawing.Point(14, 56);
            this.rad_sol2Aday.Name = "rad_sol2Aday";
            this.rad_sol2Aday.Size = new System.Drawing.Size(240, 19);
            this.rad_sol2Aday.TabIndex = 2;
            this.rad_sol2Aday.Text = "İLK 2 ADAY  —  tek sayfa, lider ikili";
            this.rad_sol2Aday.UseVisualStyleBackColor = true;
            //
            // rad_sol3Aday
            //
            this.rad_sol3Aday.AutoSize = true;
            this.rad_sol3Aday.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.rad_sol3Aday.Location = new System.Drawing.Point(14, 80);
            this.rad_sol3Aday.Name = "rad_sol3Aday";
            this.rad_sol3Aday.Size = new System.Drawing.Size(240, 19);
            this.rad_sol3Aday.TabIndex = 3;
            this.rad_sol3Aday.Text = "İLK 3 ADAY  —  SOL_2 düzeni";
            this.rad_sol3Aday.UseVisualStyleBackColor = true;
            //
            // rad_solDongu
            //
            this.rad_solDongu.AutoSize = true;
            this.rad_solDongu.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.rad_solDongu.Location = new System.Drawing.Point(14, 104);
            this.rad_solDongu.Name = "rad_solDongu";
            this.rad_solDongu.Size = new System.Drawing.Size(260, 19);
            this.rad_solDongu.TabIndex = 4;
            this.rad_solDongu.Text = "2 + 2 DÖNGÜ  —  3.-4. aday, sonra lider ikili";
            this.rad_solDongu.UseVisualStyleBackColor = true;
            //
            // lbl_solSure1
            //
            this.lbl_solSure1.AutoSize = true;
            this.lbl_solSure1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lbl_solSure1.Location = new System.Drawing.Point(12, 140);
            this.lbl_solSure1.Name = "lbl_solSure1";
            this.lbl_solSure1.Size = new System.Drawing.Size(140, 15);
            this.lbl_solSure1.TabIndex = 5;
            this.lbl_solSure1.Text = "1. sayfa ekran süresi (sn)";
            //
            // num_solSure1
            //
            this.num_solSure1.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.num_solSure1.Location = new System.Drawing.Point(232, 136);
            this.num_solSure1.Maximum = new decimal(new int[] { 600, 0, 0, 0 });
            this.num_solSure1.Minimum = new decimal(new int[] { 3, 0, 0, 0 });
            this.num_solSure1.Name = "num_solSure1";
            this.num_solSure1.Size = new System.Drawing.Size(60, 25);
            this.num_solSure1.TabIndex = 6;
            this.num_solSure1.Value = new decimal(new int[] { 10, 0, 0, 0 });
            //
            // lbl_solSure2
            //
            this.lbl_solSure2.AutoSize = true;
            this.lbl_solSure2.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lbl_solSure2.Location = new System.Drawing.Point(12, 172);
            this.lbl_solSure2.Name = "lbl_solSure2";
            this.lbl_solSure2.Size = new System.Drawing.Size(140, 15);
            this.lbl_solSure2.TabIndex = 7;
            this.lbl_solSure2.Text = "2. sayfa ekran süresi (sn)";
            //
            // num_solSure2
            //
            this.num_solSure2.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.num_solSure2.Location = new System.Drawing.Point(232, 168);
            this.num_solSure2.Maximum = new decimal(new int[] { 600, 0, 0, 0 });
            this.num_solSure2.Minimum = new decimal(new int[] { 3, 0, 0, 0 });
            this.num_solSure2.Name = "num_solSure2";
            this.num_solSure2.Size = new System.Drawing.Size(60, 25);
            this.num_solSure2.TabIndex = 8;
            this.num_solSure2.Value = new decimal(new int[] { 5, 0, 0, 0 });
            //
            // grp_altSerit
            //
            this.grp_altSerit.Controls.Add(this.num_altSure);
            this.grp_altSerit.Controls.Add(this.lbl_altSure);
            this.grp_altSerit.Controls.Add(this.chk_altAktif);
            this.grp_altSerit.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grp_altSerit.Location = new System.Drawing.Point(10, 626);
            this.grp_altSerit.Name = "grp_altSerit";
            this.grp_altSerit.Size = new System.Drawing.Size(308, 92);
            this.grp_altSerit.TabIndex = 2;
            this.grp_altSerit.TabStop = false;
            this.grp_altSerit.Text = "ALT ŞERİT";
            //
            // chk_altAktif
            //
            this.chk_altAktif.AutoSize = true;
            this.chk_altAktif.Location = new System.Drawing.Point(14, 24);
            this.chk_altAktif.Name = "chk_altAktif";
            this.chk_altAktif.Size = new System.Drawing.Size(56, 19);
            this.chk_altAktif.TabIndex = 0;
            this.chk_altAktif.Text = "AKTİF";
            this.chk_altAktif.UseVisualStyleBackColor = true;
            //
            // lbl_altSure
            //
            this.lbl_altSure.AutoSize = true;
            this.lbl_altSure.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lbl_altSure.Location = new System.Drawing.Point(12, 60);
            this.lbl_altSure.Name = "lbl_altSure";
            this.lbl_altSure.Size = new System.Drawing.Size(140, 15);
            this.lbl_altSure.TabIndex = 1;
            this.lbl_altSure.Text = "Değiştirme süresi (sn)";
            //
            // num_altSure
            //
            this.num_altSure.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.num_altSure.Location = new System.Drawing.Point(232, 56);
            this.num_altSure.Maximum = new decimal(new int[] { 600, 0, 0, 0 });
            this.num_altSure.Minimum = new decimal(new int[] { 3, 0, 0, 0 });
            this.num_altSure.Name = "num_altSure";
            this.num_altSure.Size = new System.Drawing.Size(60, 25);
            this.num_altSure.TabIndex = 2;
            this.num_altSure.Value = new decimal(new int[] { 5, 0, 0, 0 });
            //
            // pnl_sol
            //
            this.pnl_sol.AutoScroll = true;
            this.pnl_sol.Controls.Add(this.grp_altSerit);
            this.pnl_sol.Controls.Add(this.grp_solSerit);
            this.pnl_sol.Controls.Add(this.grp_ust);
            this.pnl_sol.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnl_sol.Location = new System.Drawing.Point(0, 0);
            this.pnl_sol.Name = "pnl_sol";
            this.pnl_sol.Size = new System.Drawing.Size(332, 740);
            this.pnl_sol.TabIndex = 1;

            // ================================================== SAG SUTUN

            //
            // grp_baglanti
            //
            this.grp_baglanti.Controls.Add(this.lbl_sahne);
            this.grp_baglanti.Controls.Add(this.lbl_indeks);
            this.grp_baglanti.Controls.Add(this.btn_oku);
            this.grp_baglanti.Controls.Add(this.lbl_durum);
            this.grp_baglanti.Controls.Add(this.btn_baglan);
            this.grp_baglanti.Controls.Add(this.cmb_engine);
            this.grp_baglanti.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grp_baglanti.Location = new System.Drawing.Point(10, 8);
            this.grp_baglanti.Name = "grp_baglanti";
            this.grp_baglanti.Size = new System.Drawing.Size(308, 156);
            this.grp_baglanti.TabIndex = 0;
            this.grp_baglanti.TabStop = false;
            this.grp_baglanti.Text = "VIZ ENGINE";
            //
            // cmb_engine
            //
            this.cmb_engine.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_engine.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cmb_engine.FormattingEnabled = true;
            this.cmb_engine.Location = new System.Drawing.Point(12, 24);
            this.cmb_engine.Name = "cmb_engine";
            this.cmb_engine.Size = new System.Drawing.Size(168, 23);
            this.cmb_engine.TabIndex = 0;
            //
            // btn_baglan
            //
            this.btn_baglan.Location = new System.Drawing.Point(188, 23);
            this.btn_baglan.Name = "btn_baglan";
            this.btn_baglan.Size = new System.Drawing.Size(104, 25);
            this.btn_baglan.TabIndex = 1;
            this.btn_baglan.Text = "BAĞLAN";
            this.btn_baglan.UseVisualStyleBackColor = true;
            //
            // lbl_durum
            //
            this.lbl_durum.BackColor = System.Drawing.Color.Gray;
            this.lbl_durum.ForeColor = System.Drawing.Color.White;
            this.lbl_durum.Location = new System.Drawing.Point(12, 56);
            this.lbl_durum.Name = "lbl_durum";
            this.lbl_durum.Size = new System.Drawing.Size(280, 24);
            this.lbl_durum.TabIndex = 2;
            this.lbl_durum.Text = "BAĞLI DEĞİL";
            this.lbl_durum.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // btn_oku
            //
            this.btn_oku.Location = new System.Drawing.Point(12, 88);
            this.btn_oku.Name = "btn_oku";
            this.btn_oku.Size = new System.Drawing.Size(168, 26);
            this.btn_oku.TabIndex = 3;
            this.btn_oku.Text = "SAHNEYİ OKU";
            this.btn_oku.UseVisualStyleBackColor = true;
            //
            // lbl_indeks
            //
            this.lbl_indeks.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lbl_indeks.ForeColor = System.Drawing.Color.DimGray;
            this.lbl_indeks.Location = new System.Drawing.Point(188, 94);
            this.lbl_indeks.Name = "lbl_indeks";
            this.lbl_indeks.Size = new System.Drawing.Size(104, 16);
            this.lbl_indeks.TabIndex = 4;
            this.lbl_indeks.Text = "okunmadı";
            //
            // lbl_sahne
            //
            this.lbl_sahne.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.lbl_sahne.ForeColor = System.Drawing.Color.DimGray;
            this.lbl_sahne.Location = new System.Drawing.Point(12, 120);
            this.lbl_sahne.Name = "lbl_sahne";
            this.lbl_sahne.Size = new System.Drawing.Size(280, 30);
            this.lbl_sahne.TabIndex = 5;
            this.lbl_sahne.Text = "sahne";
            //
            // grp_secimKutu
            //
            this.grp_secimKutu.Controls.Add(this.lbl_veri);
            this.grp_secimKutu.Controls.Add(this.cmb_mv);
            this.grp_secimKutu.Controls.Add(this.lbl_mv);
            this.grp_secimKutu.Controls.Add(this.cmb_cb);
            this.grp_secimKutu.Controls.Add(this.lbl_cb);
            this.grp_secimKutu.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grp_secimKutu.Location = new System.Drawing.Point(10, 172);
            this.grp_secimKutu.Name = "grp_secimKutu";
            this.grp_secimKutu.Size = new System.Drawing.Size(308, 128);
            this.grp_secimKutu.TabIndex = 1;
            this.grp_secimKutu.TabStop = false;
            this.grp_secimKutu.Text = "SEÇİM VERİSİ";
            //
            // lbl_cb
            //
            this.lbl_cb.AutoSize = true;
            this.lbl_cb.Location = new System.Drawing.Point(12, 28);
            this.lbl_cb.Name = "lbl_cb";
            this.lbl_cb.Size = new System.Drawing.Size(26, 15);
            this.lbl_cb.TabIndex = 0;
            this.lbl_cb.Text = "CB";
            //
            // cmb_cb
            //
            this.cmb_cb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_cb.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cmb_cb.FormattingEnabled = true;
            this.cmb_cb.Location = new System.Drawing.Point(48, 24);
            this.cmb_cb.Name = "cmb_cb";
            this.cmb_cb.Size = new System.Drawing.Size(244, 23);
            this.cmb_cb.TabIndex = 1;
            //
            // lbl_mv
            //
            this.lbl_mv.AutoSize = true;
            this.lbl_mv.Location = new System.Drawing.Point(12, 60);
            this.lbl_mv.Name = "lbl_mv";
            this.lbl_mv.Size = new System.Drawing.Size(28, 15);
            this.lbl_mv.TabIndex = 2;
            this.lbl_mv.Text = "MV";
            //
            // cmb_mv
            //
            this.cmb_mv.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_mv.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cmb_mv.FormattingEnabled = true;
            this.cmb_mv.Location = new System.Drawing.Point(48, 56);
            this.cmb_mv.Name = "cmb_mv";
            this.cmb_mv.Size = new System.Drawing.Size(244, 23);
            this.cmb_mv.TabIndex = 3;
            //
            // lbl_veri
            //
            this.lbl_veri.BackColor = System.Drawing.Color.Gray;
            this.lbl_veri.ForeColor = System.Drawing.Color.White;
            this.lbl_veri.Location = new System.Drawing.Point(12, 92);
            this.lbl_veri.Name = "lbl_veri";
            this.lbl_veri.Size = new System.Drawing.Size(280, 24);
            this.lbl_veri.TabIndex = 4;
            this.lbl_veri.Text = "VERİ";
            this.lbl_veri.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // grp_yayinSecenek
            //
            this.grp_yayinSecenek.Controls.Add(this.chk_acilanSandik);
            this.grp_yayinSecenek.Controls.Add(this.chk_yasak);
            this.grp_yayinSecenek.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grp_yayinSecenek.Location = new System.Drawing.Point(10, 308);
            this.grp_yayinSecenek.Name = "grp_yayinSecenek";
            this.grp_yayinSecenek.Size = new System.Drawing.Size(308, 86);
            this.grp_yayinSecenek.TabIndex = 2;
            this.grp_yayinSecenek.TabStop = false;
            this.grp_yayinSecenek.Text = "YAYIN";
            //
            // chk_yasak
            //
            this.chk_yasak.AutoSize = true;
            this.chk_yasak.ForeColor = System.Drawing.Color.FromArgb(176, 48, 44);
            this.chk_yasak.Location = new System.Drawing.Point(14, 24);
            this.chk_yasak.Name = "chk_yasak";
            this.chk_yasak.Size = new System.Drawing.Size(260, 19);
            this.chk_yasak.TabIndex = 0;
            this.chk_yasak.Text = "YASAK MODU  —  adayları ve oranları gizle";
            this.chk_yasak.UseVisualStyleBackColor = true;
            //
            // chk_acilanSandik
            //
            this.chk_acilanSandik.AutoSize = true;
            this.chk_acilanSandik.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.chk_acilanSandik.Location = new System.Drawing.Point(14, 52);
            this.chk_acilanSandik.Name = "chk_acilanSandik";
            this.chk_acilanSandik.Size = new System.Drawing.Size(200, 19);
            this.chk_acilanSandik.TabIndex = 1;
            this.chk_acilanSandik.Text = "Açılan sandık oranı (ALT / SOL)";
            this.chk_acilanSandik.UseVisualStyleBackColor = true;
            //
            // grp_durum
            //
            this.grp_durum.Controls.Add(this.btn_solAtla);
            this.grp_durum.Controls.Add(this.btn_ustAtla);
            this.grp_durum.Controls.Add(this.btn_altAtla);
            this.grp_durum.Controls.Add(this.lbl_solDurum);
            this.grp_durum.Controls.Add(this.lbl_ustDurum);
            this.grp_durum.Controls.Add(this.lbl_altDurum);
            this.grp_durum.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grp_durum.Location = new System.Drawing.Point(10, 402);
            this.grp_durum.Name = "grp_durum";
            this.grp_durum.Size = new System.Drawing.Size(308, 122);
            this.grp_durum.TabIndex = 3;
            this.grp_durum.TabStop = false;
            this.grp_durum.Text = "EKRANDA";
            //
            // lbl_altDurum
            //
            this.lbl_altDurum.BackColor = System.Drawing.Color.Gainsboro;
            this.lbl_altDurum.Font = new System.Drawing.Font("Consolas", 8.25F);
            this.lbl_altDurum.Location = new System.Drawing.Point(12, 24);
            this.lbl_altDurum.Name = "lbl_altDurum";
            this.lbl_altDurum.Size = new System.Drawing.Size(212, 26);
            this.lbl_altDurum.TabIndex = 0;
            this.lbl_altDurum.Text = "ALT  —";
            this.lbl_altDurum.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // lbl_ustDurum
            //
            this.lbl_ustDurum.BackColor = System.Drawing.Color.Gainsboro;
            this.lbl_ustDurum.Font = new System.Drawing.Font("Consolas", 8.25F);
            this.lbl_ustDurum.Location = new System.Drawing.Point(12, 56);
            this.lbl_ustDurum.Name = "lbl_ustDurum";
            this.lbl_ustDurum.Size = new System.Drawing.Size(212, 26);
            this.lbl_ustDurum.TabIndex = 1;
            this.lbl_ustDurum.Text = "ÜST  —";
            this.lbl_ustDurum.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // lbl_solDurum
            //
            this.lbl_solDurum.BackColor = System.Drawing.Color.Gainsboro;
            this.lbl_solDurum.Font = new System.Drawing.Font("Consolas", 8.25F);
            this.lbl_solDurum.Location = new System.Drawing.Point(12, 88);
            this.lbl_solDurum.Name = "lbl_solDurum";
            this.lbl_solDurum.Size = new System.Drawing.Size(212, 26);
            this.lbl_solDurum.TabIndex = 2;
            this.lbl_solDurum.Text = "SOL  —";
            this.lbl_solDurum.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // btn_altAtla
            //
            this.btn_altAtla.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.btn_altAtla.Location = new System.Drawing.Point(232, 24);
            this.btn_altAtla.Name = "btn_altAtla";
            this.btn_altAtla.Size = new System.Drawing.Size(60, 26);
            this.btn_altAtla.TabIndex = 3;
            this.btn_altAtla.Text = "ATLA";
            this.btn_altAtla.UseVisualStyleBackColor = true;
            //
            // btn_ustAtla
            //
            this.btn_ustAtla.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.btn_ustAtla.Location = new System.Drawing.Point(232, 56);
            this.btn_ustAtla.Name = "btn_ustAtla";
            this.btn_ustAtla.Size = new System.Drawing.Size(60, 26);
            this.btn_ustAtla.TabIndex = 4;
            this.btn_ustAtla.Text = "ATLA";
            this.btn_ustAtla.UseVisualStyleBackColor = true;
            //
            // btn_solAtla
            //
            this.btn_solAtla.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.btn_solAtla.Location = new System.Drawing.Point(232, 88);
            this.btn_solAtla.Name = "btn_solAtla";
            this.btn_solAtla.Size = new System.Drawing.Size(60, 26);
            this.btn_solAtla.TabIndex = 5;
            this.btn_solAtla.Text = "ATLA";
            this.btn_solAtla.UseVisualStyleBackColor = true;
            //
            // lbl_onair
            //
            this.lbl_onair.BackColor = System.Drawing.Color.DimGray;
            this.lbl_onair.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbl_onair.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lbl_onair.ForeColor = System.Drawing.Color.White;
            this.lbl_onair.Location = new System.Drawing.Point(0, 0);
            this.lbl_onair.Name = "lbl_onair";
            this.lbl_onair.Size = new System.Drawing.Size(308, 40);
            this.lbl_onair.TabIndex = 0;
            this.lbl_onair.Text = "EKRANDA DEĞİL";
            this.lbl_onair.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // btn_hazirla
            //
            this.btn_hazirla.BackColor = System.Drawing.Color.Gold;
            this.btn_hazirla.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btn_hazirla.Location = new System.Drawing.Point(0, 48);
            this.btn_hazirla.Name = "btn_hazirla";
            this.btn_hazirla.Size = new System.Drawing.Size(308, 38);
            this.btn_hazirla.TabIndex = 1;
            this.btn_hazirla.Text = "HAZIRLA";
            this.btn_hazirla.UseVisualStyleBackColor = false;
            //
            // btn_ver
            //
            this.btn_ver.BackColor = System.Drawing.Color.FromArgb(26, 160, 74);
            this.btn_ver.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.btn_ver.ForeColor = System.Drawing.Color.White;
            this.btn_ver.Location = new System.Drawing.Point(0, 92);
            this.btn_ver.Name = "btn_ver";
            this.btn_ver.Size = new System.Drawing.Size(150, 52);
            this.btn_ver.TabIndex = 2;
            this.btn_ver.Text = "VER";
            this.btn_ver.UseVisualStyleBackColor = false;
            //
            // btn_al
            //
            this.btn_al.BackColor = System.Drawing.Color.FromArgb(176, 48, 44);
            this.btn_al.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.btn_al.ForeColor = System.Drawing.Color.White;
            this.btn_al.Location = new System.Drawing.Point(158, 92);
            this.btn_al.Name = "btn_al";
            this.btn_al.Size = new System.Drawing.Size(150, 52);
            this.btn_al.TabIndex = 3;
            this.btn_al.Text = "AL";
            this.btn_al.UseVisualStyleBackColor = false;
            //
            // pnl_yayin
            //
            this.pnl_yayin.Controls.Add(this.btn_al);
            this.pnl_yayin.Controls.Add(this.btn_ver);
            this.pnl_yayin.Controls.Add(this.btn_hazirla);
            this.pnl_yayin.Controls.Add(this.lbl_onair);
            this.pnl_yayin.Location = new System.Drawing.Point(10, 532);
            this.pnl_yayin.Name = "pnl_yayin";
            this.pnl_yayin.Size = new System.Drawing.Size(308, 150);
            this.pnl_yayin.TabIndex = 4;
            //
            // pnl_sag
            //
            this.pnl_sag.AutoScroll = true;
            this.pnl_sag.Controls.Add(this.pnl_yayin);
            this.pnl_sag.Controls.Add(this.grp_durum);
            this.pnl_sag.Controls.Add(this.grp_yayinSecenek);
            this.pnl_sag.Controls.Add(this.grp_secimKutu);
            this.pnl_sag.Controls.Add(this.grp_baglanti);
            this.pnl_sag.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnl_sag.Location = new System.Drawing.Point(1168, 0);
            this.pnl_sag.Name = "pnl_sag";
            this.pnl_sag.Size = new System.Drawing.Size(332, 740);
            this.pnl_sag.TabIndex = 2;

            // ================================================= ORTA SUTUN

            //
            // btn_secilendenDevam
            //
            this.btn_secilendenDevam.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btn_secilendenDevam.Location = new System.Drawing.Point(8, 8);
            this.btn_secilendenDevam.Name = "btn_secilendenDevam";
            this.btn_secilendenDevam.Size = new System.Drawing.Size(180, 30);
            this.btn_secilendenDevam.TabIndex = 0;
            this.btn_secilendenDevam.Text = "SEÇİLENDEN DEVAM ET";
            this.btn_secilendenDevam.UseVisualStyleBackColor = true;
            //
            // btn_secileneGit
            //
            this.btn_secileneGit.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btn_secileneGit.Location = new System.Drawing.Point(196, 8);
            this.btn_secileneGit.Name = "btn_secileneGit";
            this.btn_secileneGit.Size = new System.Drawing.Size(180, 30);
            this.btn_secileneGit.TabIndex = 1;
            this.btn_secileneGit.Text = "SEÇİLENE GİT - DUR";
            this.btn_secileneGit.UseVisualStyleBackColor = true;
            //
            // btn_devam
            //
            this.btn_devam.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btn_devam.Location = new System.Drawing.Point(384, 8);
            this.btn_devam.Name = "btn_devam";
            this.btn_devam.Size = new System.Drawing.Size(180, 30);
            this.btn_devam.TabIndex = 2;
            this.btn_devam.Text = "DEVAM ET";
            this.btn_devam.UseVisualStyleBackColor = true;
            //
            // pnl_ilDugme
            //
            this.pnl_ilDugme.Controls.Add(this.btn_devam);
            this.pnl_ilDugme.Controls.Add(this.btn_secileneGit);
            this.pnl_ilDugme.Controls.Add(this.btn_secilendenDevam);
            this.pnl_ilDugme.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnl_ilDugme.Location = new System.Drawing.Point(0, 694);
            this.pnl_ilDugme.Name = "pnl_ilDugme";
            this.pnl_ilDugme.Size = new System.Drawing.Size(836, 46);
            this.pnl_ilDugme.TabIndex = 1;
            //
            // pnl_iller
            //
            this.pnl_iller.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnl_iller.Location = new System.Drawing.Point(0, 0);
            this.pnl_iller.Name = "pnl_iller";
            this.pnl_iller.Padding = new System.Windows.Forms.Padding(6);
            this.pnl_iller.Size = new System.Drawing.Size(836, 694);
            this.pnl_iller.TabIndex = 0;
            //
            // pnl_orta
            //
            this.pnl_orta.Controls.Add(this.pnl_iller);
            this.pnl_orta.Controls.Add(this.pnl_ilDugme);
            this.pnl_orta.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnl_orta.Location = new System.Drawing.Point(332, 0);
            this.pnl_orta.Name = "pnl_orta";
            this.pnl_orta.Size = new System.Drawing.Size(836, 740);
            this.pnl_orta.TabIndex = 0;
            //
            // lst_log
            //
            this.lst_log.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lst_log.Font = new System.Drawing.Font("Consolas", 8.25F);
            this.lst_log.FormattingEnabled = true;
            this.lst_log.ItemHeight = 13;
            this.lst_log.Location = new System.Drawing.Point(0, 740);
            this.lst_log.Name = "lst_log";
            this.lst_log.Size = new System.Drawing.Size(1500, 160);
            this.lst_log.TabIndex = 3;
            //
            // Form1
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1500, 900);
            this.Controls.Add(this.pnl_orta);
            this.Controls.Add(this.pnl_sol);
            this.Controls.Add(this.pnl_sag);
            this.Controls.Add(this.lst_log);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.MinimumSize = new System.Drawing.Size(1200, 700);
            this.Name = "Form1";
            this.Text = "HT SEÇİM — C EKRANI";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);

            ((System.ComponentModel.ISupportInitialize)(this.num_ustSure1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_ustSure2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_solSure1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_solSure2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_altSure)).EndInit();
            this.pnl_yayin.ResumeLayout(false);
            this.grp_durum.ResumeLayout(false);
            this.grp_yayinSecenek.ResumeLayout(false);
            this.grp_yayinSecenek.PerformLayout();
            this.grp_secimKutu.ResumeLayout(false);
            this.grp_secimKutu.PerformLayout();
            this.grp_baglanti.ResumeLayout(false);
            this.pnl_sag.ResumeLayout(false);
            this.grp_altSerit.ResumeLayout(false);
            this.grp_altSerit.PerformLayout();
            this.grp_solSerit.ResumeLayout(false);
            this.grp_solSerit.PerformLayout();
            this.grp_ittifak.ResumeLayout(false);
            this.grp_ittifak.PerformLayout();
            this.pnl_ustGosterim.ResumeLayout(false);
            this.pnl_ustGosterim.PerformLayout();
            this.pnl_ustBaslik.ResumeLayout(false);
            this.pnl_ustBaslik.PerformLayout();
            this.grp_ust.ResumeLayout(false);
            this.grp_ust.PerformLayout();
            this.pnl_sol.ResumeLayout(false);
            this.pnl_ilDugme.ResumeLayout(false);
            this.pnl_orta.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pnl_orta;
        private System.Windows.Forms.Panel pnl_iller;
        private System.Windows.Forms.Panel pnl_ilDugme;
        private System.Windows.Forms.Button btn_secilendenDevam;
        private System.Windows.Forms.Button btn_secileneGit;
        private System.Windows.Forms.Button btn_devam;

        private System.Windows.Forms.Panel pnl_sol;
        private System.Windows.Forms.GroupBox grp_ust;
        private System.Windows.Forms.CheckBox chk_ustAktif;
        private System.Windows.Forms.Label lbl_ustSure1;
        private System.Windows.Forms.NumericUpDown num_ustSure1;
        private System.Windows.Forms.Label lbl_ustSure2;
        private System.Windows.Forms.NumericUpDown num_ustSure2;
        private System.Windows.Forms.CheckBox chk_ustIkinci;
        private System.Windows.Forms.CheckBox chk_ustDiger;
        private System.Windows.Forms.RadioButton rad_oyOrani;
        private System.Windows.Forms.RadioButton rad_mvOran;
        private System.Windows.Forms.CheckBox chk_mvSayisi;
        private System.Windows.Forms.Panel pnl_ustGosterim;
        private System.Windows.Forms.Panel pnl_ustBaslik;
        private System.Windows.Forms.Label lbl_ustBaslik;
        private System.Windows.Forms.RadioButton rad_baslikAss;
        private System.Windows.Forms.RadioButton rad_baslikKo;
        private System.Windows.Forms.RadioButton rad_baslikYok;
        private System.Windows.Forms.GroupBox grp_ittifak;
        private System.Windows.Forms.CheckBox chk_ittifakAktif;
        private System.Windows.Forms.RadioButton rad_ilk2;
        private System.Windows.Forms.RadioButton rad_ikiArtiIki;
        private System.Windows.Forms.RadioButton rad_ilk3;
        private System.Windows.Forms.RadioButton rad_ikiArtiUc;
        private System.Windows.Forms.CheckBox chk_kaynak;
        private System.Windows.Forms.TextBox txt_kaynak;

        private System.Windows.Forms.GroupBox grp_solSerit;
        private System.Windows.Forms.CheckBox chk_solAktif;
        private System.Windows.Forms.CheckBox chk_ciftHane;
        private System.Windows.Forms.RadioButton rad_sol2Aday;
        private System.Windows.Forms.RadioButton rad_sol3Aday;
        private System.Windows.Forms.RadioButton rad_solDongu;
        private System.Windows.Forms.Label lbl_solSure1;
        private System.Windows.Forms.NumericUpDown num_solSure1;
        private System.Windows.Forms.Label lbl_solSure2;
        private System.Windows.Forms.NumericUpDown num_solSure2;

        private System.Windows.Forms.GroupBox grp_altSerit;
        private System.Windows.Forms.CheckBox chk_altAktif;
        private System.Windows.Forms.Label lbl_altSure;
        private System.Windows.Forms.NumericUpDown num_altSure;

        private System.Windows.Forms.Panel pnl_sag;
        private System.Windows.Forms.GroupBox grp_baglanti;
        private System.Windows.Forms.ComboBox cmb_engine;
        private System.Windows.Forms.Button btn_baglan;
        private System.Windows.Forms.Label lbl_durum;
        private System.Windows.Forms.Button btn_oku;
        private System.Windows.Forms.Label lbl_indeks;
        private System.Windows.Forms.Label lbl_sahne;

        private System.Windows.Forms.GroupBox grp_secimKutu;
        private System.Windows.Forms.Label lbl_cb;
        private System.Windows.Forms.ComboBox cmb_cb;
        private System.Windows.Forms.Label lbl_mv;
        private System.Windows.Forms.ComboBox cmb_mv;
        private System.Windows.Forms.Label lbl_veri;

        private System.Windows.Forms.GroupBox grp_yayinSecenek;
        private System.Windows.Forms.CheckBox chk_yasak;
        private System.Windows.Forms.CheckBox chk_acilanSandik;

        private System.Windows.Forms.GroupBox grp_durum;
        private System.Windows.Forms.Label lbl_altDurum;
        private System.Windows.Forms.Label lbl_ustDurum;
        private System.Windows.Forms.Label lbl_solDurum;
        private System.Windows.Forms.Button btn_altAtla;
        private System.Windows.Forms.Button btn_ustAtla;
        private System.Windows.Forms.Button btn_solAtla;

        private System.Windows.Forms.Panel pnl_yayin;
        private System.Windows.Forms.Label lbl_onair;
        private System.Windows.Forms.Button btn_hazirla;
        private System.Windows.Forms.Button btn_ver;
        private System.Windows.Forms.Button btn_al;

        private System.Windows.Forms.ListBox lst_log;
    }
}
