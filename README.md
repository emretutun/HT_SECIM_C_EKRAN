# HT SEÇİM — C EKRANI

Habertürk seçim yayınının **C ekranı** kontrol uygulaması. Alt şerit, üst şerit ve
sol sütundan oluşan (ekranda "C" harfi şeklinde duran) grafik 24 saat yayında kalır;
bu uygulama o sahneye veri yazar ve sayfa dönüşlerini tetikler.

Sahne: `/HT_SECIM/C_EKRAN/HT_SECIM_2023_C_EKRAN_ALT_CB_3_LU`
Veri : [HT_SECIM_API](../../HT_SECIM_API) üzerinden, 25 saniyede bir sürüm kontrolü ile

---

## Reji uygulamasından farkı

Reji uygulaması sahneyi yükler, HAZIRLA / VER / AL akışını sürer ve iş bitince
sahneyi düşürür. Burada öyle bir akış yok:

- **Sahne hiç düşürülmez.** 24 saat ekranda durur, uygulama üzerine yazar.
- **Operatör başında olmayabilir.** Yeni veri geldiğinde onay beklenmez, bir
  sonraki sayfa dönüşünde kendiliğinden yansır.
- **Üç şerit birbirinden bağımsız döner.** Her birinin kendi zamanlayıcısı vardır.

---

## Nasıl çalışır

```
API  ──sürüm──>  VeriKaynagi  ──>  DataService  ──>  Sayfalar  ──>  SahneSurucu  ──>  Viz Engine
                                                         ^                              (TCP 6100)
                                                         │
                                              Serit (zamanlayıcı) ×3
                                                         │
                                                   DonusMotoru
                                                    (il imleci)
```

**Sayfa** yalnızca komut üretir, hiçbir şey göndermez. Gönderme ve director
tetikleme `Serit`'te; metinler ve `#id START` **tek pakette** gider, böylece
animasyon doğru değerlerle başlar. Ters sırada olsaydı ekranda bir an eski sayı
görünürdü.

**Sayfa listeleri seçeneklerden kurulur.** Operatör bir kutuyu değiştirdiğinde
liste baştan kurulur; hangi sayfanın hangi koşulda listeye girdiğini ayrıca takip
etmeye çalışmaktan çok daha az hataya açıktır.

**Verisi olmayan sayfa beklemeden atlanır.** Dokuz partilik bir seçimde
"partiler 7-12" sayfası hiç açılmaz. Bir şeridin bütün sayfaları boşsa şerit
durur ve log'a yazar.

### Şeritler ve sayfalar

| Şerit | Düzen | Sayfa × satır | İçerik |
|---|---|---|---|
| ALT | ALT | 1 × 4 | Cumhurbaşkanı adayları |
| | | 2 × 6 | 5 parti + DİĞER |
| ÜST | UST_3 | 4 × 6 | s1-2 oran, s3-4 oran + vekil |
| | UST_4 | 2 × 5 | oran + vekil |
| | UST_5 | 2 × 2 | ittifaklar |
| | UST_6 | 2 × 3 | ittifaklar |
| SOL | SOL_1 | 2 × 2 | aday, oran, alınan oy, fotoğraf |
| | SOL_2 | 2 × 3 | aday, oran, fotoğraf |

Bu sayılar sahne ağacından ölçüldü, tahmin değil.

SOL şerit **her zaman Türkiye genelini** gösterir. ALT ve ÜST aynı ili gösterir —
altta Ankara üstte İzmir olmaz. İl imleci ALT listesi başa döndüğünde ilerler:
her il için önce adaylar, sonra partiler görünür, sonra sıradaki il.

---

## Kurulum

**Gerekenler**

- Visual Studio 2022 veya üzeri, .NET Framework 4.7.2 hedefi
- Newtonsoft.Json 13.0.4 (`packages.config`, çözüm klasöründeki `packages\` altında)
- Erişilebilir bir Viz Engine (varsayılan `127.0.0.1:6100`)
- Çalışan HT_SECIM_API (varsayılan `http://localhost:5188`)

**Adımlar**

1. `HT_SECIM_C_EKRAN.slnx` dosyasını aç, derle.
2. `bin\Debug` altındaki ayar dosyalarını kendi ortamına göre düzenle
   (aşağıdaki tabloya bak).
3. Uygulamayı çalıştır → **BAĞLAN** → **SAHNEYİ OKU** → **VER**.

> **Viz Artist açıkken hiçbir SET komutu çalışmaz.** Engine
> `the command is not allowed in this mode` döndürür. Test etmeden önce Artist'i kapat.

> Sahnenin **Stage ağacı bir kere Artist'te tamamen açılıp kaydedilmiş** olmalı;
> aksi halde `STAGE GET ALL` alt director'leri dökmez ve id'ler bulunamaz.

---

## Ayar dosyaları

Hepsi exe'nin yanında, düz metin, `anahtar = değer` biçiminde. `//` ile başlayan
satırlar yorumdur. Adres, sahne ya da süre değişince **yeniden derlemek gerekmez**.

| Dosya | Ne için | Kim düzenler |
|---|---|---|
| `iplist` | Viz Engine adresleri | elle |
| `sahne` | Katman, sahne yolu, otomatik yükleme | elle |
| `api` | API adresi, okuma anahtarı, sorgu aralığı | elle |
| `donus` | Sayfa süreleri, gösterim seçenekleri, yasak modu | elle |
| `iller` | İl grupları ve kotaları | elle |
| `il_kisa` | Dar başlıklarda kullanılacak kısa il adları | elle |
| `gorseller` | İttifak ve aday isim plakalarının görselleri | elle |
| `son_ayar` | Operatörün arayüzde yaptığı son seçimler | **uygulama yazar** |
| `veri_cache.json` | API'den gelen son sağlam veri | **uygulama yazar** |

`donus` ve `iller` dosyalarına uygulama **hiç yazmaz** — açıklamalar korunur.
Operatörün gece yaptığı değişiklikler `son_ayar`'a gider ve açılışta bu iki
dosyanın üzerine okunur. Yani `donus`, "yayın nasıl başlasın" dosyasıdır.

**Yasak modu kasıtlı olarak `son_ayar`'a yazılmaz.** Yasak bir günlük bir durumdur;
ertesi gün uygulama açıldığında kendiliğinden geri gelmesi kimsenin beklemediği
bir şey olurdu.

### İl grupları

```
GRUP = <ad> = <kota> = <plakalar, virgülle>
```

**Kota**, o gruptan arka arkaya kaç il gösterilip sonraki gruba geçileceğidir.
İLLER 5, BÜYÜK ŞEHİRLER 2 ise: sırası gelmiş 5 il, sonra 2 büyükşehir, sonra
tekrar 5 il… Her grup **kendi kaldığı yerden** devam eder, başa dönmez. Böylece
81 il sırayla geçerken arada düzenli olarak büyükşehirler de ekrana gelir.

Kotası 0 olan grup dönüşe hiç girmez. Arayüzden her ilin YAYIN kutusu tek tek
kapatılabilir; il listede kalır ama sıraya girmez.

---

## Arayüz

```
┌── sol ────────────┬── orta ──────────────────┬── sağ ──────────┐
│ ÜST ŞERİT kartı   │ İl grupları              │ Viz Engine      │
│ SOL ŞERİT kartı   │ (iller dosyasına göre    │ Seçim verisi    │
│ ALT ŞERİT kartı   │  çalışma anında kurulur) │ Yayın seçeneği  │
│                   │                          │ Ekranda         │
│                   │ SEÇİLENDEN DEVAM ET      │ HAZIRLA/VER/AL  │
└───────────────────┴──────────────────────────┴─────────────────┘
│ log konsolu                                                    │
└────────────────────────────────────────────────────────────────┘
```

**HAZIRLA** — sıradaki sayfanın değerlerini sahneye yazar ama director'ü
tetiklemez. VER'e basıldığında animasyon boş kutularla başlamaz.

**VER** — şerit köklerini açar, dönüşü başlatır.
**AL** — durdurur ve kökleri kapatır. Sahne 24 saat yayında olduğu için durdurup
bırakılsaydı son sayfa ekranda donmuş halde kalırdı.

**SEÇİLENDEN DEVAM ET** — dönüş listedeki seçilen ilden devam eder.
**SEÇİLENE GİT - DUR** — o ile gidilir ve orada kalınır; sayfalar dönmeye devam
eder, yalnızca il değişmez. **DEVAM ET** sabitlemeyi kaldırır.

**YASAK MODU** — aday/parti adları ve bütün oranlar boşaltılır, sahne ekranda
kalır. Bir sonraki sayfayı beklemez, anında uygulanır. Açılan sandık oranı ayrı
bir kutuyla yönetilir; yasak sırasında genellikle gösterilebiliyor.

---

## Sahneye dair bilinmesi gerekenler

Bu sahne 2023 için hazırlanmış ve bazı yerleri beklenenden farklı. Hepsi motora
sorularak ölçüldü; kodda ilgili yerlerde yorum olarak duruyor.

**Üç ayrı adlandırma kalıbı var.** `CEkranAdlari` bunları tek yerde topluyor:

```
ALT   ->  ALT_1_SIRA{sıra}_SAYFA{sayfa}_...     (önce SIRA)
UST   ->  UST_3_SAYFA{sayfa}_SIRA{sıra}_...     (önce SAYFA)
SOL   ->  SOL_1_SAYFA{sayfa}_ADAY{no}_...
```

Başlıklarda alt çizgi bile tutarsız: `UST3_ASS_SAYI1` ama `UST_5_ASS_ORAN1`.

**Her düzenin bir GİRİŞ director'ü var** (`ALT_GIRIS`, `UST_3_GIRIS`,
`UST_5_GIRIS`, `SOL_1_GIRIS`…). `ACTIVE SET 1` düzeni "var" yapar ama **ekrana
getirmez** — onu giriş animasyonu yapar. Yalnızca sayfa director'ü oynatılırsa
şerit görünürde tamamen kaybolur.

**Başlıklardaki ondalık kutusu kapalı gelir.** `ASS_ORAN2` / `ASS_SAYI2`
container'ları `ACTIVE = 0` ve virgül sabit bir işaret değil, içine yazılan bir
metin kutusu. Ondalık haneyi yazmak yetmez; kutuyu açmak ve virgülü de yazmak
gerekir, yoksa ekranda "98" görünür, "98,2" değil.

**Açılan sandık ve katılım kutuları aynı yerde durur.** İkisi birden açılırsa
yazılar üst üste biner. Arayüzdeki "Başlık kutusu" seçimi bu yüzden tek seçimlidir.

**Bazı container adları sahne genelinde tekrar eder** (`UST_1_BASLIK1` hem
UST_3'te hem UST_4'te, director adları da öyle). Bunlar `$ad` ile adreslenemez;
sayısal yolla (`2/4/1/8/1/1/1`) ya da `#id` ile yazılır. `CEkranYollari` bu
yolları tek yerde tutuyor — **sahne değişirse orası yeniden ölçülmeli.**

**İsim plakaları boyanamaz.** İttifak ve aday adlarının arkasındaki renkli zemin
materyal değil, hazır görsel (`container holds no MATERIAL reference`). Renk
komutu işe yaramaz; doğru görseli giydirmek gerekir. Havuzda ittifak başına plaka
hazırlanmış (`UST_CUMHUR_ISIM_BACK`, `UST_MILLET_ISIM_BACK`…), eşleme
`gorseller` dosyasında.

**Satır renk şeritleri boyanabilir** ama iki farklı ölçek var: container'ın kendi
rengi 0-255, `MATERIAL*COLOR` ise 0-1 arası ondalık ister. Sayılar nokta ile
yazılmalı; Türkçe kültürde virgül çıkar ve engine komutu sessizce yok sayar.

**Görsel adları havuz genelinde çözülür.** Aynı isim birden fazla klasörde
geçebiliyor ve Viz tam yol verilse bile yanlış klasördekini getirebiliyor. Bu
yüzden her görsel yolu `IMAGE*<yol>*UUID GET` ile UUID'ye çevrilip öyle yazılıyor.

**Yazı kutuya sığdırılmaz.** Viz uzun metni kendiliğinden küçültmez, yan kutunun
üzerine taşar. Uygulama harf sayısı bütçeyi aşarsa `TRANSFORMATION*SCALING*X`
ile yatayda oranla sıkıştırır. Tasarım ölçekleri `CEkranYollari`'nda **sabit
yazılıdır, sahneden okunmaz** — okunsaydı bir kere sıkıştırdıktan sonra sıkışmış
değer "tasarım" sanılır ve yazı her turda biraz daha küçülürdü.

**Eksikler (sahne kaynaklı, kodla çözülemez):**

- ALT sayfa 2'de il adı container'ı yok; partiler sayfasında il yazmaz.
- UST_5 / UST_6'da ayrı il kutusu yok; oradaki tek metin kutusuna il adı yazılıyor,
  seçim adı gösterilmiyor.

---

## Veri

API `dbo.VeriSurum` tablosunda tek satırlık bir sürüm numarası tutar; on tabloda
`AFTER INSERT, UPDATE, DELETE` trigger'ı bu numarayı artırır. Tek bir oy değişse
sürüm artar.

Uygulama `ARALIK` saniyede bir `/api/v1/surum` sorar (birkaç yüz bayt). Numara
aynıysa hiçbir şey indirmez. Değiştiyse `/api/v1/veri` çekilir, arka planda
çözülür, UI thread'inde yerine konur ve `veri_cache.json`'a yazılır.

Veri **atomik** değişir: tablo komple yeni nesneyle değiştirilir, yarım tablo
okunma ihtimali yoktur.

**Açılışta önce önbellek okunur.** C ekranı 24 saat çalışır ve gece yarısı
yeniden başlatılabilir; API o an kapalıysa boş ekranla açılmak kabul edilemez.
Gerçek veri birkaç saniye sonra arka planda gelip üzerine yazar.

`api` dosyasında `KAYNAK = JSON` yapılırsa ağa hiç çıkılmaz, yanındaki
`veri.json` okunur. Yayın gecesi API tuhaflık yaparsa tek satırla eski düzene
dönmek içindir.

### Ölçülen değerler

| | |
|---|---|
| Sayfa başına komut | ALT 24-30, ÜST 21-46, SOL 18-19 (hepsi tek pakette) |
| Üç şerit birlikte | ortalama ~10 komut/saniye |
| 204 komut tek pakette | ≈ 52 ms |
| `/api/v1/veri` | 198 KB, 205 ms soğuk / 129 ms önbellekten |

Veri 5 dakikada bir tazelense bile sahneye ek yük binmez: yeni veri geldiğinde
tek komut gitmez, yalnızca bellekteki tablo değişir. Sahneye yazma sayfa
dönüşünde ve sadece o sayfanın alanları için olur.

---

## Log

`bin\Debug\LOG\` altında günlük dosyalar:

- `actions_YYYY_MM_DD.log` — sayfa geçişleri, operatör hareketleri, veri değişimi
- `debug_YYYY_MM_DD.log` — engine'e giden her komut
- `error_YYYY_MM_DD.log` — yalnızca hatalar

Bir sorunu incelerken önce `actions`, sonra `debug` dosyasına bakılır. Sahnede
beklenmedik bir şey görünüyorsa `debug` dosyasında o an hangi container'a ne
yazıldığı birebir durur.

---

## Kaynak düzeni

```
Core/
  VizClient.cs VizEngine.cs      TCP bağlantısı, komut gönderme
  EngineRepository.cs            iplist dosyası
  ConfigReader.cs ConfigPaths.cs ayar dosyası okuma
  CLog.cs                        log
  SahneAyarlari.cs               sahne dosyası
  SahneIndeksi.cs                STAGE GET ALL çözümleme, director id'leri
  SahneSurucu.cs                 komut üretme, görsel UUID, gönderme
  CEkranAdlari.cs                container adları (üç kalıp)
  CEkranYollari.cs               sayısal yollar ve tasarım ölçekleri
  CEkranSecenekleri.cs           bütün çalışma seçenekleri, donus + son_ayar
  CEkranGorselleri.cs            isim plakaları, gorseller dosyası
  CEkranSayfasi.cs               sayfa tabanı ve bağlam
  Serit.cs                       tek şeridin zamanlayıcısı ve sayfa akışı
  DonusMotoru.cs                 üç şerit, il imleci, VER / AL / HAZIRLA
  IlGruplari.cs IlAdlari.cs      il grupları, kısaltmalar
  ApiAyarlari.cs VeriKaynagi.cs  API sürüm yoklama ve indirme
Data/
  Models.cs DataService.cs       veri modeli ve sorgular
Sayfalar/
  AltSayfa.cs UstSayfa.cs SolSayfa.cs
Form1.cs / Form1.Designer.cs     arayüz
```

---

## Yayın öncesi kontrol listesi

- [ ] Viz Artist **kapalı**
- [ ] Sahnenin Stage ağacı bir kere açılıp kaydedilmiş
- [ ] `iplist`'te doğru engine, `sahne`'de doğru sahne yolu
- [ ] `api`'de doğru adres ve okuma anahtarı, veri göstergesi yeşil
- [ ] Seçim listelerinde doğru CB ve MV seçimi
- [ ] İl grubu kotaları ve yayına açık iller gözden geçirilmiş
- [ ] HAZIRLA → ekranda doğru değerler → VER
- [ ] Bir tam tur izlenmiş: ittifak sayfasına geçiş, uzun il adı (K.MARAŞ),
      parti renkleri, açılan sandık ondalığı
