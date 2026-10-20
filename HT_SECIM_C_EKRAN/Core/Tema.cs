using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace HT_SECIM_C_EKRAN.Core
{
    /// <summary>
    /// Arayuzun renkleri ve gorunumu tek yerde.
    ///
    /// Yerlesim Designer'da kaliyor; bu sinif yalnizca boyuyor. Form
    /// acilirken Uygula(this) butun kontrolleri gezip koyu temaya
    /// ceviriyor. Calisma aninda olusturulan kontroller (il kutulari)
    /// icin Uygula ayrica cagrilmali.
    ///
    /// Durum renkleri (BAGLI / EKRANDA / API YOK ...) de buradan
    /// okunuyor ki ayni anlam her yerde ayni renkle gorunsun.
    /// </summary>
    public static class Tema
    {
        // ---------------------------------------------------- zemin

        public static readonly Color Zemin      = Color.FromArgb(240, 243, 247);
        public static readonly Color Kart       = Color.White;
        public static readonly Color Yuzey      = Color.White;
        public static readonly Color YuzeyHover = Color.FromArgb(232, 240, 252);
        public static readonly Color Kenar      = Color.FromArgb(208, 215, 225);
        public static readonly Color DurumZemin = Color.FromArgb(243, 246, 250);

        // ---------------------------------------------------- metin

        public static readonly Color Metin      = Color.FromArgb(31, 41, 55);
        public static readonly Color SolukMetin = Color.FromArgb(100, 110, 125);
        public static readonly Color Baslik     = Color.FromArgb(30, 90, 180);

        // ---------------------------------------------------- durum

        public static readonly Color Yesil   = Color.FromArgb(30, 150, 80);
        public static readonly Color Kirmizi = Color.FromArgb(205, 50, 45);
        public static readonly Color Sari    = Color.FromArgb(245, 190, 60);
        public static readonly Color Notr    = Color.FromArgb(105, 115, 130);

        public static readonly Color LogZemin = Color.White;
        public static readonly Color LogMetin = Color.FromArgb(55, 65, 81);

        /// <summary> Rengi verilmis dugmeler; pasif olunca soluklasiyorlar. </summary>
        private static readonly Dictionary<Button, Color> renkliDugmeler =
            new Dictionary<Button, Color>();

        #region Uygula

        /// <summary>
        /// Kontrolu ve butun alt kontrollerini temaya cevirir.
        /// Ayni kontrol iki kez gecse de sorun yok.
        /// </summary>
        public static void Uygula(Control kok)
        {
            if (kok is Form)
            {
                kok.BackColor = Zemin;
                kok.ForeColor = Metin;
            }

            Boya(kok);

            foreach (Control c in kok.Controls) Uygula(c);
        }

        private static void Boya(Control c)
        {
            if (c is GroupBox)              GrupKutusu((GroupBox)c);
            else if (c is Button)           Dugme((Button)c);
            else if (c is CheckedListBox)   Liste((ListBox)c);
            else if (c is ListBox)          LogKonsolu((ListBox)c);
            else if (c is ComboBox)         Acilir((ComboBox)c);
            else if (c is NumericUpDown)    Girdi(c);
            else if (c is TextBox)          Girdi(c);
            else if (c is Label)            Etiket((Label)c);

            // CheckBox / RadioButton bilerek sistem gorunumunde birakildi:
            // yazi rengi ebeveynden geliyor, isaret kutusu her temada okunur.
        }

        private static void Etiket(Label lbl)
        {
            // Designer'daki "ipucu" etiketleri DimGray; koyu zeminde
            // okunmuyorlar.
            if (lbl.ForeColor == Color.DimGray) lbl.ForeColor = SolukMetin;

            // Durum seritleri (ALT / UST / SOL) acik gri zeminliydi.
            if (lbl.BackColor == Color.Gainsboro)
            {
                lbl.BackColor = DurumZemin;
                lbl.ForeColor = Metin;
            }

            // Durum etiketleri (BAGLI, VERI, EKRANDA) kodla boyaniyor;
            // burada yalnizca baslangic grisi temaya cekiliyor.
            if (lbl.BackColor == Color.Gray || lbl.BackColor == Color.DimGray)
                lbl.BackColor = Notr;
        }

        private static void Girdi(Control c)
        {
            c.BackColor = Yuzey;
            c.ForeColor = Metin;

            TextBox txt = c as TextBox;
            if (txt != null) txt.BorderStyle = BorderStyle.FixedSingle;

            NumericUpDown num = c as NumericUpDown;
            if (num != null) num.BorderStyle = BorderStyle.FixedSingle;
        }

        private static void Acilir(ComboBox cmb)
        {
            cmb.FlatStyle = FlatStyle.Flat;
            cmb.BackColor = Yuzey;
            cmb.ForeColor = Metin;
        }

        private static void Liste(ListBox lst)
        {
            // Beyaz kartin icinde beyaz liste: kenar olmadan sinir kayboluyor.
            lst.BackColor = Yuzey;
            lst.ForeColor = Metin;
            lst.BorderStyle = BorderStyle.FixedSingle;
        }

        private static void LogKonsolu(ListBox lst)
        {
            lst.BackColor = LogZemin;
            lst.ForeColor = LogMetin;
            lst.BorderStyle = BorderStyle.FixedSingle;
        }

        #endregion

        #region Dugmeler

        private static void Dugme(Button btn)
        {
            if (renkliDugmeler.ContainsKey(btn)) return;

            btn.FlatStyle = FlatStyle.Flat;
            btn.UseVisualStyleBackColor = false;
            btn.BackColor = Yuzey;
            btn.ForeColor = Metin;
            btn.Cursor = Cursors.Hand;
            btn.FlatAppearance.BorderColor = Kenar;
            btn.FlatAppearance.BorderSize = 1;
            btn.FlatAppearance.MouseOverBackColor = YuzeyHover;
            btn.FlatAppearance.MouseDownBackColor = Kenar;
        }

        /// <summary>
        /// Anlami olan dugmeler (VER / AL / HAZIRLA) icin dolu renk.
        /// Pasifken renk soluklasiyor; aksi halde basilamayan bir VER
        /// dugmesi hala yesil parlar ve operatoru yaniltir.
        /// </summary>
        public static void RenkliDugme(Button btn, Color renk, Color yazi)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.UseVisualStyleBackColor = false;
            btn.ForeColor = yazi;
            btn.Cursor = Cursors.Hand;
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = Karistir(renk, Color.White, 0.12f);
            btn.FlatAppearance.MouseDownBackColor = Karistir(renk, Color.Black, 0.15f);

            if (!renkliDugmeler.ContainsKey(btn))
                btn.EnabledChanged += delegate { RenkliDugmeyiBoya(btn); };

            renkliDugmeler[btn] = renk;
            RenkliDugmeyiBoya(btn);
        }

        private static void RenkliDugmeyiBoya(Button btn)
        {
            Color renk = renkliDugmeler[btn];
            btn.BackColor = btn.Enabled ? renk : Karistir(renk, Kart, 0.7f);
        }

        #endregion

        #region Grup kutulari

        /// <summary>
        /// Standart GroupBox koyu zeminde acik gri bir cizgi ciziyor ve
        /// basligi siyah yaziyor. Yerine yuvarlak koseli bir kart
        /// ciziliyor; alt kontrollerin yerleri degismiyor.
        /// </summary>
        private static void GrupKutusu(GroupBox kutu)
        {
            kutu.BackColor = Kart;
            kutu.ForeColor = Metin;

            // Paint iki kez baglanmasin.
            kutu.Paint -= GrupKutusu_Paint;
            kutu.Paint += GrupKutusu_Paint;
        }

        private static void GrupKutusu_Paint(object sender, PaintEventArgs e)
        {
            GroupBox kutu = (GroupBox)sender;
            Graphics g = e.Graphics;

            Color disZemin = kutu.Parent != null ? kutu.Parent.BackColor : Zemin;
            g.Clear(disZemin);
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle alan = new Rectangle(0, 0, kutu.Width - 1, kutu.Height - 1);

            using (GraphicsPath yol = YuvarlakDikdortgen(alan, 6))
            using (SolidBrush firca = new SolidBrush(Kart))
            using (Pen kalem = new Pen(Kenar))
            {
                g.FillPath(firca, yol);
                g.DrawPath(kalem, yol);
            }

            if (string.IsNullOrEmpty(kutu.Text)) return;

            using (Font yazi = new Font(kutu.Font.FontFamily, 9F, FontStyle.Bold))
            {
                TextRenderer.DrawText(g, kutu.Text, yazi, new Point(9, 4), Baslik);
            }
        }

        private static GraphicsPath YuvarlakDikdortgen(Rectangle r, int yaricap)
        {
            int d = yaricap * 2;
            GraphicsPath yol = new GraphicsPath();
            yol.AddArc(r.X, r.Y, d, d, 180, 90);
            yol.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            yol.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            yol.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            yol.CloseFigure();
            return yol;
        }

        #endregion

        /// <summary> a'dan b'ye dogru oran kadar karistirir (0 = a, 1 = b). </summary>
        public static Color Karistir(Color a, Color b, float oran)
        {
            return Color.FromArgb(
                (int)(a.R + (b.R - a.R) * oran),
                (int)(a.G + (b.G - a.G) * oran),
                (int)(a.B + (b.B - a.B) * oran));
        }
    }
}
