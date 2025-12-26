# 🏗️ Architecture Technique - WiFi Security Auditor

## Vue d'Ensemble

WiFi Security Auditor est construit avec **.NET MAUI** (Multi-platform App UI) et suit le pattern architectural **MVVM** (Model-View-ViewModel) pour garantir une séparation claire des responsabilités et faciliter la maintenabilité.

## Stack Technologique

### Framework Principal
- **.NET 6.0** - Framework multiplateforme
- **.NET MAUI** - UI multiplateforme (évolution de Xamarin.Forms)
- **C# 10** - Langage de programmation

### Plateformes Cibles
- **Android** API 21+ (Android 5.0 Lollipop)
- **iOS** 14.2+
- **macOS Catalyst** 14.0+
- **Windows** 10 (Build 17763+)

### Dépendances
- **Microsoft.Maui.Controls** - Composants UI
- **System.ComponentModel** - INotifyPropertyChanged
- Aucune dépendance tierce pour garantir la sécurité

## Architecture MVVM

```
┌─────────────────────────────────────────────────────┐
│                      View (XAML)                     │
│  ┌─────────────┐  ┌──────────────┐  ┌────────────┐ │
│  │ MainPage    │  │ NetworkDetail│  │Educational │ │
│  │             │  │     Page     │  │    Page    │ │
│  └──────┬──────┘  └───────┬──────┘  └──────┬─────┘ │
└─────────┼──────────────────┼────────────────┼───────┘
          │ Data Binding     │                │
          ▼                  ▼                ▼
┌─────────────────────────────────────────────────────┐
│                   ViewModel Layer                    │
│  ┌─────────────┐  ┌──────────────┐  ┌────────────┐ │
│  │   Main      │  │NetworkDetail │  │Educational │ │
│  │ ViewModel   │  │  ViewModel   │  │ ViewModel  │ │
│  └──────┬──────┘  └───────┬──────┘  └──────┬─────┘ │
└─────────┼──────────────────┼────────────────┼───────┘
          │ Uses Services    │                │
          ▼                  ▼                ▼
┌─────────────────────────────────────────────────────┐
│                   Service Layer                      │
│  ┌──────────────┐  ┌────────────────┐  ┌─────────┐ │
│  │ WifiService  │  │SecurityAnalysis│  │Educational││
│  │              │  │    Service     │  │  Service  │ │
│  └──────┬───────┘  └────────┬───────┘  └────┬────┘ │
└─────────┼──────────────────┼─────────────────┼──────┘
          │ Uses Models      │                 │
          ▼                  ▼                 ▼
┌─────────────────────────────────────────────────────┐
│                    Model Layer                       │
│  ┌──────────────┐  ┌────────────────┐  ┌─────────┐ │
│  │ WifiNetwork  │  │SecurityAnalysis│  │Educational││
│  │              │  │                │  │  Content  │ │
│  └──────────────┘  └────────────────┘  └─────────┘ │
└─────────────────────────────────────────────────────┘
```

## Structure des Dossiers

```
wifiCrack/
│
├── Models/                          # Modèles de données (POCO)
│   ├── WifiNetwork.cs              # Représentation d'un réseau WiFi
│   ├── SecurityAnalysis.cs         # Résultats d'analyse de sécurité
│   ├── Vulnerability.cs            # Détails d'une vulnérabilité
│   ├── Recommendation.cs           # Recommandation de sécurité
│   ├── SavedWifiCredential.cs      # Identifiants sauvegardés
│   └── EducationalContent.cs       # Contenu pédagogique
│
├── ViewModels/                      # Logique de présentation
│   ├── MainViewModel.cs            # VM de la page principale
│   ├── NetworkDetailViewModel.cs   # VM des détails réseau
│   └── EducationalViewModel.cs     # VM du mode éducatif
│
├── Views/                           # Interfaces utilisateur (XAML)
│   ├── DisclaimerPage.xaml         # Page d'avertissement légal
│   ├── MainPage.xaml               # Page principale (liste réseaux)
│   ├── NetworkDetailPage.xaml      # Détails et analyse d'un réseau
│   ├── EducationalPage.xaml        # Tutoriels et quiz
│   └── ContentDetailPage.xaml      # Détail d'un contenu éducatif
│
├── Services/                        # Logique métier
│   ├── IWifiService.cs             # Interface du service WiFi
│   ├── ISecurityAnalysisService.cs # Interface d'analyse
│   ├── IEducationalService.cs      # Interface contenu éducatif
│   ├── SecurityAnalysisService.cs  # Implémentation analyse
│   ├── EducationalService.cs       # Implémentation éducation
│   └── DummyWifiService.cs         # Service de démo
│
├── Platforms/                       # Code spécifique par plateforme
│   ├── Android/
│   │   ├── MainActivity.cs
│   │   ├── WifiService.cs          # Implémentation Android
│   │   └── NetworkSecurityService.cs
│   ├── iOS/
│   │   ├── AppDelegate.cs
│   │   └── (WifiService.cs - à implémenter)
│   ├── Windows/
│   │   └── App.xaml.cs
│   └── MacCatalyst/
│       └── AppDelegate.cs
│
├── Helpers/                         # Classes utilitaires
│   ├── ObservableObject.cs         # Classe de base pour MVVM
│   └── RelayCommand.cs             # Implémentation ICommand
│
├── Resources/                       # Ressources de l'application
│   ├── Styles/
│   │   ├── Colors.xaml             # Palette de couleurs
│   │   └── Styles.xaml             # Styles globaux
│   ├── Fonts/                      # Polices personnalisées
│   ├── Images/                     # Images et icônes
│   └── Raw/                        # Autres ressources
│
├── App.xaml                         # Application principale
├── AppShell.xaml                    # Navigation Shell
└── MauiProgram.cs                  # Point d'entrée et configuration

```

## Couches de l'Application

### 1. Model Layer (Modèles de Données)

**Responsabilité :** Représentation des données métier

#### WifiNetwork.cs
```csharp
public class WifiNetwork
{
    // Propriétés de base
    public string Ssid { get; set; }
    public string Bssid { get; set; }
    public int SignalStrength { get; set; }

    // Propriétés calculées
    public string SignalQuality => GetSignalQuality();
    public int SecurityScore => CalculateSecurityScore();

    // Logique métier dans le modèle (acceptable pour calculs simples)
    private int CalculateSecurityScore() { ... }
}
```

**Principes :**
- Classes POCO (Plain Old CLR Objects)
- Immutabilité quand possible (records)
- Logique métier minimale (uniquement calculs simples)
- Pas de dépendance vers d'autres couches

### 2. Service Layer (Services)

**Responsabilité :** Logique métier et accès aux données

#### IWifiService.cs (Interface)
```csharp
public interface IWifiService
{
    Task<List<WifiNetwork>> ScanNetworksAsync();
    Task<List<SavedWifiCredential>> GetSavedNetworksAsync();
    Task<bool> RequestLocationPermissionAsync();
    bool IsWifiEnabled();
}
```

#### WifiService.cs (Implémentation Android)
```csharp
public class WifiService : IWifiService
{
    private readonly WifiManager _wifiManager;

    public async Task<List<WifiNetwork>> ScanNetworksAsync()
    {
        // Utilisation des API Android natives
        // Aucune commande système dangereuse
        // Respect des permissions
    }
}
```

**Principes :**
- Définition par interface (Dependency Inversion)
- Implémentation par plateforme si nécessaire
- Gestion des erreurs
- Async/await pour opérations longues
- Aucun accès direct à l'UI

### 3. ViewModel Layer (Logique de Présentation)

**Responsabilité :** Liaison entre la vue et les services

#### MainViewModel.cs
```csharp
public class MainViewModel : ObservableObject
{
    private readonly IWifiService _wifiService;
    private ObservableCollection<WifiNetwork> _networks;

    // Property avec notification de changement
    public ObservableCollection<WifiNetwork> Networks
    {
        get => _networks;
        set => SetProperty(ref _networks, value);
    }

    // Command pour le binding
    public ICommand ScanCommand { get; }

    // Logique métier de présentation
    public async Task ScanNetworksAsync() { ... }
}
```

**Principes :**
- Hérite de `ObservableObject` (INotifyPropertyChanged)
- Expose des `ICommand` pour les actions utilisateur
- Utilise `ObservableCollection` pour les listes
- Pas de référence directe aux contrôles UI
- Injection de dépendances via constructeur

### 4. View Layer (Interface Utilisateur)

**Responsabilité :** Affichage et interaction utilisateur

#### MainPage.xaml
```xaml
<ContentPage x:DataType="viewmodels:MainViewModel">
    <CollectionView ItemsSource="{Binding Networks}">
        <CollectionView.ItemTemplate>
            <DataTemplate x:DataType="models:WifiNetwork">
                <Label Text="{Binding Ssid}"/>
            </DataTemplate>
        </CollectionView.ItemTemplate>
    </CollectionView>

    <Button Command="{Binding ScanCommand}" Text="Scanner"/>
</ContentPage>
```

**Principes :**
- Data binding bidirectionnel
- `x:DataType` pour binding typé (compilation)
- Séparation UI (XAML) / logique (code-behind minimal)
- Styles et ressources réutilisables

## Flux de Données

### Exemple : Scanner les Réseaux WiFi

```
1. User Action (View)
   └─> Button Click

2. Command Binding (ViewModel)
   └─> ScanCommand.Execute()
   └─> MainViewModel.ScanNetworksAsync()

3. Service Call (Service)
   └─> _wifiService.ScanNetworksAsync()

4. Platform API (Android/iOS)
   └─> WifiManager.StartScan() [Android]
   └─> NEHotspotNetwork [iOS]

5. Data Processing (Service)
   └─> Convert ScanResults to WifiNetwork models

6. ViewModel Update (ViewModel)
   └─> Networks.Clear()
   └─> Networks.Add(network)  // Déclenche INotifyPropertyChanged

7. UI Update (View)
   └─> CollectionView se rafraîchit automatiquement
```

## Gestion des Dépendances

### Injection de Dépendances Manuelle

Actuellement, l'application utilise une DI simple :

```csharp
// Dans MainPage.xaml.cs
var wifiService = new Platforms.Android.WifiService();
var securityService = new SecurityAnalysisService();
var viewModel = new MainViewModel(wifiService, securityService);
BindingContext = viewModel;
```

### Évolution Future : DI Container

Pour améliorer la testabilité et la maintenabilité :

```csharp
// MauiProgram.cs
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        // Enregistrer les services
        builder.Services.AddSingleton<IWifiService, WifiService>();
        builder.Services.AddSingleton<ISecurityAnalysisService, SecurityAnalysisService>();

        // Enregistrer les ViewModels
        builder.Services.AddTransient<MainViewModel>();

        // Enregistrer les Pages
        builder.Services.AddTransient<MainPage>();

        return builder.Build();
    }
}
```

## Patterns et Principes Appliqués

### SOLID Principles

#### Single Responsibility
- Chaque classe a une responsabilité unique
- `WifiService` : gestion WiFi
- `SecurityAnalysisService` : analyse de sécurité
- `EducationalService` : contenu éducatif

#### Open/Closed
- Extension via interfaces
- `IWifiService` peut avoir plusieurs implémentations (Android, iOS, Dummy)

#### Liskov Substitution
- Les implémentations d'interface sont interchangeables
- `DummyWifiService` peut remplacer `WifiService` sans casser le code

#### Interface Segregation
- Interfaces spécifiques et ciblées
- `IWifiService`, `ISecurityAnalysisService`, `IEducationalService`

#### Dependency Inversion
- Dépendance sur les abstractions (interfaces)
- ViewModels dépendent de `IWifiService`, pas de `WifiService`

### Autres Patterns

#### Repository Pattern
- Services encapsulent l'accès aux données
- Abstraction de la source de données (API, cache, fichiers)

#### Observer Pattern
- `INotifyPropertyChanged` pour mise à jour automatique de l'UI
- `ObservableCollection` pour les listes

#### Command Pattern
- `ICommand` pour les actions utilisateur
- Séparation de la logique d'exécution

#### Factory Pattern
- Création d'objets complexes (SecurityAnalysis)

## Sécurité de l'Architecture

### Approche "Secure by Design"

#### 1. Pas d'Exécution de Code Arbitraire
```csharp
// ❌ DANGEREUX - Non utilisé dans l'application
Process.Start("su", "-c malicious_command");

// ✅ SÛR - Uniquement API natives
var networks = await _wifiManager.ScanResultsAsync();
```

#### 2. Validation des Entrées
```csharp
public async Task<SavedWifiCredential> GetNetworkCredentialAsync(string ssid)
{
    if (string.IsNullOrWhiteSpace(ssid))
        throw new ArgumentException("SSID cannot be null or empty");

    // Validation du format SSID (32 caractères max)
    if (ssid.Length > 32)
        throw new ArgumentException("Invalid SSID length");

    // ...
}
```

#### 3. Gestion des Permissions
```csharp
// Demande explicite et vérification
public async Task<bool> RequestLocationPermissionAsync()
{
    var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

    if (status != PermissionStatus.Granted)
    {
        status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
    }

    return status == PermissionStatus.Granted;
}
```

#### 4. Pas de Stockage Sensible
```csharp
// ❌ Ne jamais faire
Preferences.Set("wifi_password", password); // En clair !

// ✅ Bonne pratique
// Ne pas stocker les mots de passe
// Laisser l'OS gérer via Keychain/Keystore
```

#### 5. Logging Sécurisé
```csharp
// ❌ Dangereux
Debug.WriteLine($"Password: {password}");

// ✅ Sûr
Debug.WriteLine($"Attempting connection to network {ssid}");
```

## Performance et Optimisation

### Async/Await
- Toutes les opérations longues sont asynchrones
- Pas de blocage de l'UI thread

### Lazy Loading
- Chargement des données à la demande
- Collections virtualisées avec CollectionView

### Caching
```csharp
private List<WifiNetwork> _cachedNetworks;
private DateTime _lastScanTime;

public async Task<List<WifiNetwork>> ScanNetworksAsync()
{
    // Cache de 30 secondes
    if (_cachedNetworks != null &&
        (DateTime.Now - _lastScanTime).TotalSeconds < 30)
    {
        return _cachedNetworks;
    }

    _cachedNetworks = await PerformScanAsync();
    _lastScanTime = DateTime.Now;
    return _cachedNetworks;
}
```

### Disposal Pattern
```csharp
public class WifiReceiver : BroadcastReceiver, IDisposable
{
    public void Dispose()
    {
        // Cleanup des ressources
        _context?.UnregisterReceiver(this);
    }
}

// Utilisation
using var receiver = new WifiReceiver();
```

## Testing

### Architecture Testable

```csharp
// Test du ViewModel sans UI
[Test]
public async Task ScanNetworks_ShouldPopulateNetworksList()
{
    // Arrange
    var mockWifiService = new Mock<IWifiService>();
    mockWifiService.Setup(s => s.ScanNetworksAsync())
        .ReturnsAsync(new List<WifiNetwork> { /* ... */ });

    var viewModel = new MainViewModel(mockWifiService.Object, null);

    // Act
    await viewModel.ScanNetworksAsync();

    // Assert
    Assert.IsTrue(viewModel.Networks.Count > 0);
}
```

### Tests Recommandés

1. **Unit Tests** : ViewModels, Services, Models
2. **Integration Tests** : Interaction entre couches
3. **UI Tests** : Scénarios utilisateur (Appium, Xamarin.UITest)

## Évolutions Futures

### Améliorations Architecturales

1. **Dependency Injection Container**
   - Microsoft.Extensions.DependencyInjection
   - Meilleure gestion du cycle de vie

2. **CQRS Pattern**
   - Séparation commands/queries
   - Pour analyses complexes

3. **Event Aggregator**
   - Communication découplée entre ViewModels
   - Notifications cross-page

4. **Repository + Unit of Work**
   - Si base de données locale ajoutée
   - Gestion transactionnelle

5. **State Management**
   - Redux-like pattern pour état global
   - Prism, ReactiveUI, ou MVVMCross

### Nouvelles Fonctionnalités

1. **Export PDF des rapports**
   - QuestPDF ou iTextSharp
   - Génération de rapports professionnels

2. **Base de données locale**
   - SQLite via Entity Framework Core
   - Historique des scans

3. **Synchronisation Cloud**
   - Azure Mobile Apps
   - Backup des paramètres

4. **Machine Learning**
   - ML.NET pour détecter patterns suspects
   - Prédiction de vulnérabilités

## Ressources et Documentation

- [.NET MAUI Documentation](https://docs.microsoft.com/dotnet/maui/)
- [MVVM Pattern](https://docs.microsoft.com/xamarin/xamarin-forms/enterprise-application-patterns/mvvm)
- [Android WiFi API](https://developer.android.com/reference/android/net/wifi/WifiManager)
- [iOS Network Framework](https://developer.apple.com/documentation/network)

---

**Version :** 2.0
**Dernière mise à jour :** Décembre 2025
