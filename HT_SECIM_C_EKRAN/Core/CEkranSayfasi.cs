using HT_SECIM_C_EKRAN.Data;
using System.Collections.Generic;

namespace HT_SECIM_C_EKRAN.Core
{
    /// <summary>
    /// Bir sayfanin hangi ili ve hangi secimi gosterecegi.
    ///
    /// Serit her sayfa donusunde bunu yeniden kuruyor; sayfa nesneleri
    /// durum tutmuyor, sadece verilen baglami komuta ceviriyor.
    /// </summary>
    public class CEkranBaglam
    {
        /// <summary> 0 = Turkiye geneli, 900 = yurtdisi, 901 = gumruk. </summary>
        public int Plaka;

        /// <summary> Cumhurbaskani secimi kodu. </summary>
        public string SecimCB;

        /// <summary> Milletvekili secimi kodu. </summary>
        public string SecimMV;

        /// <summary>
        /// Ekranda gosterilecek il adi.
        /// "il_kisa" dosyasinda kisaltmasi varsa o kullaniliyor; dar
        /// basliklarda KAHRAMANMARAŞ yerine K.MARAŞ yaziyor.
        /// </summary>
        public string IlAdi
        {
            get { return IlAdlari.Ekranda(Plaka); }
        }
    }

    /// <summary>
    /// Donusteki tek bir sayfa.
    ///
    /// Sayfa yalnizca KOMUT URETIR, hicbir sey gondermez. Gonderme ve
    /// director tetikleme isi Serit'te; boylece metinler ve "#id START"
    /// tek pakette gidiyor ve animasyon dogru degerlerle basliyor.
    /// </summary>
    public abstract class CEkranSayfasi
    {
        /// <summary> Log'da ve arayuzde gorunecek kisa ad. </summary>
        public abstract string Ad { get; }

        /// <summary>
        /// Bu sayfanin ekranda kalma suresi, saniye.
        ///
        /// Sayfa basina ayri tutuluyor: eski C ekraninda da birinci ve
        /// ikinci sayfanin sureleri ayriydi. Ilk sayfa lider tabloyu
        /// gosterdigi icin genelde daha uzun duruyor.
        /// </summary>
        public abstract int Sure { get; }

        /// <summary> Oynatilacak director'un ad yolu, "UST_3/SAYFA1" gibi. </summary>
        public abstract string DirectorYolu { get; }

        /// <summary>
        /// Sayfanin ait oldugu duzen container'i. Serit sayfayi oynatmadan
        /// once bunu acip kardeslerini kapatiyor. Duzen secimi olmayan
        /// seritlerde (ALT) null.
        /// </summary>
        public virtual string DuzenYolu { get { return null; } }

        /// <summary>
        /// Duzeni ekrana getiren GIRIS director'u.
        ///
        /// Sahnede her duzenin bir girisi var (ALT_GIRIS, UST_3_GIRIS,
        /// UST_5_GIRIS, SOL_1_GIRIS...). Serit yalnizca sayfa director'unu
        /// oynatirsa o duzen hic ekrana gelmiyor ve serit bombos kaliyor.
        ///
        /// Giris her sayfada degil, DUZEN DEGISTIGINDE oynatiliyor.
        /// </summary>
        public virtual string GirisDirectorYolu { get { return null; } }

        /// <summary>
        /// Sayfanin metinlerini uretir.
        ///
        /// false donerse sayfada gosterilecek anlamli veri yok demektir;
        /// Serit o sayfayi BEKLEMEDEN atlar. Ornegin 12 partilik ikinci
        /// sayfa, secimde 9 parti varsa hic acilmaz.
        /// </summary>
        public abstract bool Hazirla(CEkranBaglam baglam, SahneSurucu surucu, List<string> komutlar);
    }
}
