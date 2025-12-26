# 🔨 Guide de Compilation et Déploiement

## Prérequis

### 🖥️ Environnement de Développement

#### Windows
- **Visual Studio 2022** (17.3 ou supérieur)
- Workloads requis :
  - ✅ Développement mobile avec .NET (MAUI)
  - ✅ Développement .NET Desktop
  - ✅ Développement ASP.NET et web (optionnel)

#### macOS
- **Visual Studio 2022 for Mac** (17.3 ou supérieur)
- **Xcode** (14.0 ou supérieur) pour iOS/macOS
- **Command Line Tools** pour Xcode

### 📦 SDKs Requis

```bash
# Vérifier l'installation de .NET
dotnet --version
# Devrait afficher 6.0 ou supérieur

# Lister les workloads MAUI installés
dotnet workload list

# Installer MAUI si nécessaire
dotnet workload install maui

# Installer les workloads Android
dotnet workload install android

# Installer les workloads iOS (macOS uniquement)
dotnet workload install ios

# Installer les workloads macOS (macOS uniquement)
dotnet workload install maccatalyst
```

## 📋 Étapes de Compilation

### 1. Cloner le Repository

```bash
git clone <votre-repo-url>
cd wifi_password_view
```

### 2. Restaurer les Dépendances

```bash
cd wifiCrack
dotnet restore
```

### 3. Vérifier la Configuration

```bash
# Lister les frameworks cibles
dotnet build --help
```

## 🤖 Compilation Android

### Configuration Android

1. **Android SDK Manager**
   - Ouvrir Visual Studio
   - Tools → Android → Android SDK Manager
   - Installer :
     - Android SDK Platform API 21+ (minimum)
     - Android SDK Platform API 33 (recommandé)
     - Android SDK Build-Tools 33.0.0

2. **Émulateurs**
   - Tools → Android → Android Device Manager
   - Créer un émulateur avec API Level 21+

### Build en Ligne de Commande

```bash
# Debug build
dotnet build -f net6.0-android -c Debug

# Release build
dotnet build -f net6.0-android -c Release

# Build et Run sur émulateur
dotnet build -t:Run -f net6.0-android
```

### Création d'un APK

```bash
# Debug APK
dotnet publish -f net6.0-android -c Debug

# Release APK (non signé)
dotnet publish -f net6.0-android -c Release

# APK signé pour distribution
dotnet publish -f net6.0-android -c Release \
    /p:AndroidKeyStore=true \
    /p:AndroidSigningKeyStore=<chemin-keystore> \
    /p:AndroidSigningKeyAlias=<alias> \
    /p:AndroidSigningKeyPass=<password> \
    /p:AndroidSigningStorePass=<password>
```

**Emplacement de l'APK :**
```
wifiCrack/bin/Debug/net6.0-android/publish/com.security.wifisecurityauditor-Signed.apk
```

### Déploiement sur Appareil Android

```bash
# Lister les appareils connectés
adb devices

# Installer l'APK
adb install -r wifiCrack/bin/Debug/net6.0-android/publish/*.apk

# Lancer l'application
adb shell am start -n com.security.wifisecurityauditor/.MainActivity
```

## 🍎 Compilation iOS

### Configuration iOS (macOS uniquement)

1. **Compte Apple Developer**
   - Gratuit pour tests sur appareil physique
   - Payant ($99/an) pour distribution App Store

2. **Provisioning Profile**
   - Visual Studio → Préférences → Apple Developer Account
   - Ajouter votre compte
   - Créer un profil de provisioning

### Build en Ligne de Commande

```bash
# Debug build pour simulateur
dotnet build -f net6.0-ios -c Debug \
    /p:_DeviceName=:v2:udid=<simulator-udid>

# Release build
dotnet build -f net6.0-ios -c Release
```

### Création d'un IPA

```bash
# Build pour distribution
dotnet publish -f net6.0-ios -c Release \
    /p:ArchiveOnBuild=true \
    /p:CodesignProvision="<provisioning-profile-name>" \
    /p:CodesignKey="iPhone Distribution"
```

## 🪟 Compilation Windows

### Build en Ligne de Commande

```bash
# Debug build
dotnet build -f net6.0-windows10.0.19041.0 -c Debug

# Release build
dotnet build -f net6.0-windows10.0.19041.0 -c Release
```

### Création d'un Package MSIX

```bash
dotnet publish -f net6.0-windows10.0.19041.0 -c Release \
    /p:GenerateAppxPackageOnBuild=true
```

## 🖥️ Compilation macOS Catalyst

### Build en Ligne de Commande

```bash
# Debug build
dotnet build -f net6.0-maccatalyst -c Debug

# Release build
dotnet build -f net6.0-maccatalyst -c Release
```

## 🧪 Tests

### Tests Unitaires

```bash
# Exécuter tous les tests
dotnet test

# Avec couverture de code
dotnet test /p:CollectCoverage=true
```

### Tests sur Émulateur/Simulateur

```bash
# Android
dotnet build -t:Run -f net6.0-android

# iOS (simulateur)
dotnet build -t:Run -f net6.0-ios
```

## 🚀 Déploiement

### Android - Google Play Store

1. **Créer un keystore de production**

```bash
keytool -genkey -v \
    -keystore wifi-auditor.keystore \
    -alias wifi-auditor \
    -keyalg RSA \
    -keysize 2048 \
    -validity 10000
```

2. **Build Release signé**

```bash
dotnet publish -f net6.0-android -c Release \
    /p:AndroidKeyStore=true \
    /p:AndroidSigningKeyStore=wifi-auditor.keystore \
    /p:AndroidSigningKeyAlias=wifi-auditor \
    /p:AndroidSigningKeyPass=<password> \
    /p:AndroidSigningStorePass=<password>
```

3. **Upload sur Google Play Console**
   - Créer une app dans la console
   - Upload de l'AAB (Android App Bundle) recommandé
   - Remplir les métadonnées
   - Soumettre pour review

### iOS - App Store

1. **Archive l'application**
   - Visual Studio → Build → Archive for Publishing
   - Sélectionner le profil de distribution

2. **Upload via Transporter ou Xcode**

3. **App Store Connect**
   - Créer une fiche app
   - Sélectionner le build
   - Soumettre pour review

## ⚙️ Configuration de Build

### Variables d'Environnement

```bash
# Android SDK
export ANDROID_SDK_ROOT=/Users/<user>/Library/Android/sdk

# Java (pour Android)
export JAVA_HOME=/Library/Java/JavaVirtualMachines/jdk-11.jdk/Contents/Home
```

### Paramètres de Build Recommandés

#### Android Release

```xml
<PropertyGroup Condition="'$(Configuration)|$(TargetFramework)' == 'Release|net6.0-android'">
    <AndroidPackageFormat>aab</AndroidPackageFormat>
    <AndroidUseAapt2>true</AndroidUseAapt2>
    <AndroidLinkMode>Full</AndroidLinkMode>
    <EnableLLVM>true</EnableLLVM>
    <AndroidEnableProguard>true</AndroidEnableProguard>
    <AndroidDexTool>d8</AndroidDexTool>
</PropertyGroup>
```

#### iOS Release

```xml
<PropertyGroup Condition="'$(Configuration)|$(TargetFramework)' == 'Release|net6.0-ios'">
    <MtouchLink>Full</MtouchLink>
    <EnableSGenConc>true</EnableSGenConc>
    <MtouchUseLlvm>true</MtouchUseLlvm>
    <MtouchEnableSGenConc>true</MtouchEnableSGenConc>
</PropertyGroup>
```

## 🐛 Dépannage

### Erreurs Courantes

#### "Platform not found"

```bash
# Réinstaller la plateforme
dotnet workload repair
dotnet workload install maui
```

#### "Android SDK not found"

```bash
# Définir ANDROID_SDK_ROOT
export ANDROID_SDK_ROOT=$HOME/Library/Android/sdk

# Ou dans ~/.zshrc / ~/.bashrc
echo 'export ANDROID_SDK_ROOT=$HOME/Library/Android/sdk' >> ~/.zshrc
```

#### "Unable to find a valid Xcode"

```bash
# Sélectionner Xcode
sudo xcode-select --switch /Applications/Xcode.app/Contents/Developer
sudo xcode-select --install
```

#### "Build failed: NU1101"

```bash
# Nettoyer et restaurer
dotnet clean
dotnet restore
dotnet build
```

### Logs de Build

```bash
# Build verbose
dotnet build -v detailed > build.log 2>&1

# Android logs en direct
adb logcat | grep -i wifi

# iOS logs
xcrun simctl spawn booted log stream --predicate 'process == "wifiCrack"'
```

## 📊 Métriques de Build

### Taille des Packages

| Plateforme | Debug | Release | Release (Optimized) |
|------------|-------|---------|---------------------|
| Android    | ~45MB | ~30MB   | ~15MB              |
| iOS        | ~60MB | ~40MB   | ~20MB              |
| Windows    | ~50MB | ~35MB   | ~18MB              |

### Temps de Compilation (macOS M1)

| Plateforme | Clean Build | Incremental |
|------------|-------------|-------------|
| Android    | ~2 min      | ~20 sec     |
| iOS        | ~3 min      | ~30 sec     |
| Windows    | ~2.5 min    | ~25 sec     |

## 🔄 CI/CD

### GitHub Actions (Exemple)

```yaml
name: Build and Test

on:
  push:
    branches: [ main ]
  pull_request:
    branches: [ main ]

jobs:
  build-android:
    runs-on: ubuntu-latest
    steps:
    - uses: actions/checkout@v3
    - name: Setup .NET
      uses: actions/setup-dotnet@v3
      with:
        dotnet-version: 6.0.x
    - name: Install MAUI
      run: dotnet workload install maui
    - name: Restore
      run: dotnet restore
    - name: Build Android
      run: dotnet build -f net6.0-android -c Release
```

## 📚 Ressources

- [Documentation .NET MAUI](https://docs.microsoft.com/dotnet/maui/)
- [Android Developer Guide](https://developer.android.com/)
- [iOS Developer Guide](https://developer.apple.com/documentation/)
- [Windows App Development](https://docs.microsoft.com/windows/apps/)

---

**Besoin d'aide ?** Ouvrez une issue sur GitHub ou consultez la documentation officielle.
