# Proje Inheritance Yapýsý - Detaylý Analiz

## ?? Class Hierarchy Diyagramý

```
MonoBehaviour (Unity Base Class)
?
??? 1?? BaseUIManager (ABSTRACT)
?   ?
?   ??? 2?? UIManager (CONCRETE)
?   ?   ??? Özellikler: Basit UI gösterimi
?   ?
?   ??? 3?? AdvancedUIManager (CONCRETE)
?       ??? Özellikler: Animasyonlu UI gösterimi
?
??? 4?? GameManager (CONCRETE)
?   ??? Oyun mantýðýnýn ana kontrol noktasý
?
??? 5?? EnemyCollisionDetector (CONCRETE)
    ??? Düþman çarpýþma algýlama
```

---

## ?? Tüm Classes Tablosu

| # | Class Name | Base Class | Türü | Dosya |
|---|------------|-----------|------|-------|
| 1?? | `BaseUIManager` | `MonoBehaviour` | **ABSTRACT** | BaseUIManager.cs |
| 2?? | `UIManager` | `BaseUIManager` | Concrete | UIManager.cs |
| 3?? | `AdvancedUIManager` | `BaseUIManager` | Concrete | AdvancedUIManager.cs |
| 4?? | `GameManager` | `MonoBehaviour` | Concrete | GameManager.cs |
| 5?? | `EnemyCollisionDetector` | `MonoBehaviour` | Concrete | EnemyCollisionDetector.cs |

---

## ?? Inheritance Detaylarý

### 1?? **BaseUIManager** (ABSTRACT BASE CLASS)
```csharp
public abstract class BaseUIManager : MonoBehaviour
```

**Inheritance Yönü:** ?? MonoBehaviour'dan miras alýyor

**Özellikleri:**
- ? Abstract methods (zorunlu override)
  - `InitializeUI()`
  - `UpdateScoreDisplay()`
  - `OnGameOver(int finalScore)`
- ? Virtual methods (isteðe baðlý override)
  - `OnScoreChanged(int newScore)`
- ? Protected fields
  - `hasGameEnded`
  - `onGameOverEvent`

**Child Classes:**
- ?? `UIManager`
- ?? `AdvancedUIManager`

---

### 2?? **UIManager** (DERIVED CLASS)
```csharp
public class UIManager : BaseUIManager
```

**Inheritance Yönü:** ?? BaseUIManager'dan miras alýyor (???? MonoBehaviour)

**Override Ettiði Metodlar:**
- ? `InitializeUI()` - Concrete implementasyon
- ? `UpdateScoreDisplay()` - Concrete implementasyon
- ? `OnGameOver(int finalScore)` - Concrete implementasyon
- ? `OnScoreChanged(int newScore)` - Concrete implementasyon

**Ek Özellikler:**
- Event subscription (OnScoreChanged, OnGameOver)
- GameManager reference
- TextMeshPro UI references

**Kalýtsal Özellikler:**
- Tüm `BaseUIManager` protected members'a eriþim
- `MonoBehaviour` lifecycle methods

---

### 3?? **AdvancedUIManager** (DERIVED CLASS)
```csharp
public class AdvancedUIManager : BaseUIManager
```

**Inheritance Yönü:** ?? BaseUIManager'dan miras alýyor (???? MonoBehaviour)

**Override Ettiði Metodlar:**
- ? `InitializeUI()` - Concrete implementasyon
- ? `UpdateScoreDisplay()` - Concrete implementasyon
- ? `OnGameOver(int finalScore)` - Concrete implementasyon
- ? `OnScoreChanged(int newScore)` - Concrete implementasyon

**Ek Özellikler:**
- Score pop animasyonu
- Rich text formatting
- Animation timer

**Kalýtsal Özellikler:**
- Tüm `BaseUIManager` protected members'a eriþim
- `MonoBehaviour` lifecycle methods

---

### 4?? **GameManager** (STANDALONE CLASS)
```csharp
public class GameManager : MonoBehaviour
```

**Inheritance Yönü:** ?? MonoBehaviour'dan miras alýyor

**NOT:** Bu class hiçbir custom class'tan miras almýyor!

**Özellikleri:**
- Events: `OnScoreChanged`, `OnGameOver`
- Prefab references
- Player/Enemy management
- Camera setup
- 3D physics handling

**Diðer Classes Tarafýndan Referans Alýnýyor:**
- ?? `UIManager` tarafýndan kullanýlýyor
- ?? `EnemyCollisionDetector` tarafýndan kullanýlýyor

---

### 5?? **EnemyCollisionDetector** (STANDALONE CLASS)
```csharp
public class EnemyCollisionDetector : MonoBehaviour
```

**Inheritance Yönü:** ?? MonoBehaviour'dan miras alýyor

**NOT:** Bu class hiçbir custom class'tan miras almýyor!

**Özellikleri:**
- GameManager reference
- Collision detection
- Trigger events

**Baðýmlýlýðý:**
- ?? `GameManager` tarafýndan baðýmlý

---

## ?? Inheritance Chain (Derinlik)

### **UIManager**
```
MonoBehaviour (Unity)
    ?
BaseUIManager (Seviye 1 - ABSTRACT)
    ?
UIManager (Seviye 2 - CONCRETE)
```
**Derinlik:** 2 level

### **AdvancedUIManager**
```
MonoBehaviour (Unity)
    ?
BaseUIManager (Seviye 1 - ABSTRACT)
    ?
AdvancedUIManager (Seviye 2 - CONCRETE)
```
**Derinlik:** 2 level

### **GameManager**
```
MonoBehaviour (Unity)
    ?
GameManager (Seviye 1 - CONCRETE)
```
**Derinlik:** 1 level

### **EnemyCollisionDetector**
```
MonoBehaviour (Unity)
    ?
EnemyCollisionDetector (Seviye 1 - CONCRETE)
```
**Derinlik:** 1 level

---

## ?? Inheritance Ýstatistikleri

| Metrik | Deðer |
|--------|-------|
| **Toplam Classes** | 5 |
| **Abstract Classes** | 1 (BaseUIManager) |
| **Concrete Classes** | 4 |
| **Direct MonoBehaviour Inheritors** | 4 |
| **Custom Inheritance Var** | ? Evet (BaseUIManager) |
| **Max Inheritance Depth** | 2 (UI Classes) |
| **Inheritance Yapan Classes** | 2 (UIManager, AdvancedUIManager) |

---

## ?? Dependency Graph

```
UIManager ?????????? BaseUIManager
   ?                      ?
   ?                      ?
   ????????????????????????
        Extends

AdvancedUIManager ?????? BaseUIManager
   ?                        ?
   ?                        ?
   ??????????????????????????
         Extends

GameManager ????????? MonoBehaviour (Unity)
   ?
   ???? Referensi: UIManager, EnemyCollisionDetector

EnemyCollisionDetector ?? MonoBehaviour (Unity)
   ?
   ???? Referensi: GameManager
```

---

## ? Inheritance Avantajlarý Projede

| Avantaj | Açýklama | Kullanýlan Yer |
|---------|----------|-----------------|
| **Polimorfizm** | Ayný metodun farklý implementasyonlarý | UIManager vs AdvancedUIManager |
| **Kodun Yeniden Kullanýmý** | Ortak fonksiyonlar base class'ta | BaseUIManager.InitializeUI() |
| **Single Responsibility** | Her class tek iþi yapýyor | UI logic ayrý, game logic ayrý |
| **Geniþletilebilirlik** | Yeni UI manager'lar kolayca eklenebilir | Yeni class + BaseUIManager extend |
| **Type Safety** | Compile-time kontrol | BaseUIManager ? UIManager type checking |

---

## ?? Inheritance Deseni: Template Method Pattern

```csharp
// BaseUIManager (ABSTRACT)
public abstract class BaseUIManager : MonoBehaviour
{
    protected abstract void InitializeUI();     // ? Subclass'lar override eder
    public abstract void UpdateScoreDisplay();  // ? Subclass'lar override eder
    public abstract void OnGameOver(int score); // ? Subclass'lar override eder
    
    public virtual void OnScoreChanged(int score) // ? Ýsteðe baðlý override
    {
        UpdateScoreDisplay();
    }
}

// UIManager (CONCRETE)
public class UIManager : BaseUIManager
{
    protected override void InitializeUI() { /* UI setup */ }
    public override void UpdateScoreDisplay() { /* Display score */ }
    public override void OnGameOver(int score) { /* Show game over */ }
}

// AdvancedUIManager (CONCRETE)
public class AdvancedUIManager : BaseUIManager
{
    protected override void InitializeUI() { /* Advanced setup */ }
    public override void UpdateScoreDisplay() { /* Advanced display */ }
    public override void OnGameOver(int score) { /* Advanced game over */ }
}
```

---

## ?? Gelecekteki Inheritance Fýrsatlarý

**Eklenebilecek Abstract Classes:**

1. **IGameComponent (Interface)**
   ```csharp
   public interface IGameComponent
   {
       void Initialize();
       void Update();
       void Cleanup();
   }
   ```

2. **BaseCollisionDetector (Abstract)**
   ```csharp
   public abstract class BaseCollisionDetector : MonoBehaviour
   {
       // Common collision logic
   }
   ```
   - ?? EnemyCollisionDetector extend edebilir
   - ?? PlayerCollisionDetector eklenebilir

3. **BaseManager (Abstract)**
   ```csharp
   public abstract class BaseManager : MonoBehaviour
   {
       // Common manager logic
   }
   ```
   - ?? GameManager extend edebilir
   - ?? AudioManager eklenebilir
   - ?? SaveManager eklenebilir

---

## ?? Sonuç

? **Inheritance Yapan Classes:** 2 adet
- UIManager (BaseUIManager'dan miras)
- AdvancedUIManager (BaseUIManager'dan miras)

? **Abstract Classes:** 1 adet
- BaseUIManager

? **Inheritance Pattern:** Polimorfizm ve Template Method Pattern

? **Kod Kalitesi:** Ýyi organize edilmiþ, geniþletilebilir yapý
