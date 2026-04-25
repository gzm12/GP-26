Proje: Basit 3D düþman-toplama oyunu (Unity)

Bu dosya projedeki baþlýca sýnýflarý ve kullanýlan OOP/Unity desenlerini öðretici þekilde açýklar.

Genel mimari
- Kod büyük ölçüde `MonoBehaviour` türetilmiþ bileþenler etrafýnda toplanmýþ. Sahne yönetimi ve oyun mantýðý `GameManager` içinde toplanmýþ.
- UI yönetimi için bir soyutlama (abstract base) kullanýlmýþ: `BaseUIManager` -> `UIManager`, `AdvancedUIManager`.
- Basit arayüzler (interfaces) ile sözleþme saðlanmýþ: `IDamageable`, `ISpawnable`, `IUIUpdatable`.
- Olay-temelli (event/delegate) iletiþim `GameManager` ile UI arasýnda kullanýlmýþ (`OnScoreChanged`, `OnGameOver`).

Ana dosyalar ve sorumluluklarý
- `Assets/Scripts/GameManager.cs`
  - Oyun döngüsü, oyuncu ve düþman yaratma, puan yönetimi, çarpýþma tespiti.
  - `IDamageable` arayüzünü uygular (`TakeDamage`).
  - `OnScoreChanged` ve `OnGameOver` delegate/event'lerini yayýnlayarak diðer bileþenlerle gevþek baðlý iletiþim kurar.
  - Unity API: `Instantiate`, `Destroy`, `GetComponent`, `FindWithTag`, `Resources.Load`, `Camera.main`, `Rigidbody` iþlemleri.
  - Not: Kodda `linearVelocity` kullanýmý var; Unity `Rigidbody` için standart özellik `velocity`'dir — derleme/çalýþma zamaný hatasý riski olabilir.

- `Assets/Scripts/EnemyCollisionDetector.cs`
  - Bir düþman objesinin tetikleyici çarpýþmalarýný dinler.
  - Çarpýþmada `IDamageable` üzerinden hasar uygulamaya çalýþýyor (þu an `gameManager` üzerinden cast yapýlýyor).
  - `FindObjectOfType<GameManager>()` ile `GameManager` referansý alýr (baðýmlýlýk enjeksiyonu daha temiz olur).

- `Assets/Scripts/IDamageable.cs` ve `Assets/Scripts/ISpawnable.cs`
  - Basit arayüzler. `IDamageable` => `TakeDamage(float)`; `ISpawnable` => `OnSpawned()` / `OnDespawned()`.
  - Bu arayüzler polymorphism saðlar: farklý nesneler ayný sözleþme üzerinden yönetilebilir.

- `Assets/Scripts/IUIUpdatable.cs`, `Assets/Scripts/BaseUIManager.cs`, `Assets/Scripts/UIManager.cs`, `Assets/Scripts/AdvancedUIManager.cs`
  - `IUIUpdatable` UI bileþenlerinin güncellenme/show/hide sözleþmesini verir.
  - `BaseUIManager` soyut sýnýfý, ortak davranýþlarý ve event hooklarýný tanýmlar (template method tarzý).
  - `UIManager` ve `AdvancedUIManager` bu soyut sýnýfý geniþleterek farklý görsel/animasyon davranýþlarý saðlar (kalýtým + override kullanýmý).

Kullanýlan OOP kavramlarý (özet)
- Sýnýflar (Classes): `GameManager`, `UIManager`, `AdvancedUIManager`, `EnemyCollisionDetector`, vb.
- Kalýtým (Inheritance): `UIManager` ve `AdvancedUIManager` `BaseUIManager`'ý extend eder. Abstract/override ile davranýþ özelleþtirilir.
- Arayüzler (Interfaces): `IDamageable`, `ISpawnable`, `IUIUpdatable` — polymorphism ve gevþek baðlýlýk saðlar.
- Polymorphism: `GameManager` bir `IDamageable` olarak kullanýlabilir; farklý UI yöneticileri `BaseUIManager` üzerinden ele alýnabilir.
- Olaylar / Delegeler (Events/Delegates): `GameManager` puan ve oyun bitti olaylarýný yayýnlar; UI bunlara abone olur.
- Encapsulation: Alanlarýn çoðu `private` veya `protected` ve `SerializeField` ile inspector üzerinden atama desteklenmiþ.
- Kompozisyon: Bileþenler `GetComponent` ve sahne referanslarý ile birbirine baðlanýr. Prefab kullanýmý ile runtime nesne oluþturma.

Takip edilebilecek geliþtirme notlarý
- `EnemyCollisionDetector` doðrudan `gameManager as IDamageable` yapýyor. Daha güvenli: çarpýþan `other` üzerinde `GetComponent<IDamageable>()` çaðrýsý yaparak hedef nesneden hasar uygula.
- `linearVelocity` yerine Unity `Rigidbody.velocity` kullanýlmalý.
- `FindObjectOfType<>` ve `FindWithTag` sýklýðý performans etkileyebilir; referanslarý inspector üzerinden atamak veya baþlangýçta cachelemek daha iyi.
- `GameManager` çok fazla sorumluluk taþýyor; oyuncu yönetimi, düþman yönetimi ve skor gibi sorumluluklar ayrý yöneticilere (PlayerManager, EnemyManager, ScoreManager) bölünebilir (Single Responsibility Principle).

Sonuç
Bu kod tabaný Unity standart desenlerini (MonoBehaviour, prefabler, GetComponent, SerializeField) ve temel OOP ilkelerini (arayinlar, kalýtým, polymorphism, event) öðretici bir þekilde bir araya getiriyor. Yukarýdaki küçük iyileþtirmelerle kod daha saðlam ve sürdürülebilir olur.

Dosyalar (hýzlý referans)
- `Assets/Scripts/GameManager.cs`  — merkez oyun mantýðý
- `Assets/Scripts/EnemyCollisionDetector.cs` — çarpýþma/hasar
- `Assets/Scripts/IDamageable.cs`, `ISpawnable.cs`, `IUIUpdatable.cs` — arayüzler
- `Assets/Scripts/BaseUIManager.cs`, `UIManager.cs`, `AdvancedUIManager.cs` — UI altyapýsý

Ýstersen bu dosyalarýn her birinde otomatik yorum satýrlarý, UML diyagramý veya kod örnekleri ekleyerek daha derin bir eðitim dokümaný hazýrlayabilirim.