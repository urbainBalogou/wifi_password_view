# 🔐 WiFi Security Auditor

> **Application éducative d'audit de sécurité WiFi** - Version 2.0

[![Platform](https://img.shields.io/badge/Platform-.NET%20MAUI-512BD4)](https://dotnet.microsoft.com/apps/maui)
[![Framework](https://img.shields.io/badge/.NET-6.0-512BD4)](https://dotnet.microsoft.com/)
[![License](https://img.shields.io/badge/License-Educational-orange)](LICENSE)

## ⚠️ AVERTISSEMENT LÉGAL

**IMPORTANT : Cette application est destinée à des fins ÉDUCATIVES et de RECHERCHE uniquement.**

L'utilisation de cet outil pour tester la sécurité de réseaux WiFi qui ne vous appartiennent pas est **ILLÉGALE** et passible de sanctions pénales sévères.

### Cadre Légal

- **Code Pénal Français - Article 323-1**
- Accès frauduleux à un système informatique
- **Jusqu'à 2 ans de prison et 60 000€ d'amende**
- Peines aggravées en cas de modification ou suppression de données

### Utilisations Autorisées ✅

- Tester votre propre réseau WiFi
- Audit avec autorisation écrite du propriétaire
- Recherche en environnement contrôlé
- Formation et éducation
- Compétitions CTF et challenges de sécurité

## 📋 Description

WiFi Security Auditor est une application multiplateforme développée avec .NET MAUI qui permet d'auditer la sécurité des réseaux WiFi de manière **légale et éthique**.

### Contexte Académique

Cette application a été développée dans le cadre d'un **concours d'entrée à une université d'informatique** pour démontrer :
- La maîtrise du développement mobile multiplateforme
- La compréhension des protocoles de sécurité WiFi
- L'éthique en cybersécurité
- Les bonnes pratiques de développement logiciel

## ✨ Fonctionnalités

### 🔍 Analyse de Réseaux WiFi

- **Scan de réseaux** : Détection des points d'accès WiFi à proximité
- **Informations détaillées** : SSID, BSSID, type de sécurité, force du signal
- **Score de sécurité** : Évaluation de 0 à 10 basée sur les protocoles utilisés
- **Analyse des vulnérabilités** : Détection automatique des faiblesses de sécurité

### 🛡️ Audit de Sécurité

- **Détection WEP** : Identification des réseaux utilisant ce protocole obsolète
- **Analyse WPA/WPA2/WPA3** : Évaluation du niveau de sécurité
- **Détection WPS** : Identification des réseaux vulnérables à l'attaque Pixie Dust
- **Recommandations personnalisées** : Conseils pour améliorer la sécurité

### 💾 Accès aux Réseaux Sauvegardés (LÉGAL)

- **SANS ROOT** : Utilisation des API Android natives uniquement
- Liste des réseaux WiFi sauvegardés sur l'appareil
- **Note** : Sur Android 10+, les mots de passe ne sont pas accessibles sans root (restriction OS)

### 📚 Mode Éducatif

- **Tutoriels interactifs** sur les protocoles WiFi (WEP, WPA, WPA2, WPA3)
- **Explication des attaques** courantes (à but éducatif)
- **Bonnes pratiques de sécurité**
- **Quiz interactif** pour tester vos connaissances
- **Cadre légal** de l'audit de sécurité

### 📄 Génération de Rapports

- Rapports détaillés d'audit de sécurité
- Export en format texte
- Historique des analyses

## 🏗️ Architecture

L'application utilise le pattern **MVVM (Model-View-ViewModel)** pour une séparation claire des responsabilités :

```
wifiCrack/
├── Models/               # Modèles de données
│   ├── WifiNetwork.cs
│   ├── SecurityAnalysis.cs
│   ├── SavedWifiCredential.cs
│   └── EducationalContent.cs
│
├── ViewModels/          # Logique métier
│   ├── MainViewModel.cs
│   ├── NetworkDetailViewModel.cs
│   └── EducationalViewModel.cs
│
├── Views/               # Interfaces utilisateur
│   ├── DisclaimerPage.xaml
│   ├── MainPage.xaml
│   ├── NetworkDetailPage.xaml
│   ├── EducationalPage.xaml
│   └── ContentDetailPage.xaml
│
├── Services/            # Services métier
│   ├── IWifiService.cs
│   ├── ISecurityAnalysisService.cs
│   ├── SecurityAnalysisService.cs
│   └── EducationalService.cs
│
├── Platforms/           # Code spécifique par plateforme
│   └── Android/
│       ├── WifiService.cs
│       └── NetworkSecurityService.cs
│
└── Helpers/            # Utilitaires
    ├── ObservableObject.cs
    └── RelayCommand.cs
```

## 🚀 Installation

### Prérequis

- [.NET 6.0 SDK](https://dotnet.microsoft.com/download/dotnet/6.0) ou supérieur
- [Visual Studio 2022](https://visualstudio.microsoft.com/) avec :
  - Workload : développement mobile avec .NET (MAUI)
- Pour Android :
  - Android SDK API 21+ (Android 5.0 Lollipop)
- Pour iOS :
  - macOS avec Xcode
  - iOS 14.2+

### Compilation

```bash
# Cloner le repository
git clone https://github.com/votre-repo/wifi-security-auditor.git
cd wifi-security-auditor/wifiCrack

# Restaurer les dépendances
dotnet restore

# Compiler pour Android
dotnet build -f net6.0-android

# Compiler pour iOS
dotnet build -f net6.0-ios

# Compiler pour Windows
dotnet build -f net6.0-windows10.0.19041.0
```

### Exécution

```bash
# Android
dotnet run -f net6.0-android

# iOS (sur macOS uniquement)
dotnet run -f net6.0-ios

# Windows
dotnet run -f net6.0-windows10.0.19041.0
```

## 📱 Plateformes Supportées

| Plateforme      | Version Minimale | Statut |
|----------------|------------------|--------|
| Android        | 5.0 (API 21)     | ✅ Complet |
| iOS            | 14.2             | ✅ Complet |
| macOS Catalyst | 14.0             | ✅ Complet |
| Windows        | 10 (17763)       | ✅ Complet |

## 🔒 Sécurité et Éthique

### Approche Sans Root

Cette application **NE NÉCESSITE PAS** d'accès root pour fonctionner. Elle utilise uniquement les API publiques fournies par les systèmes d'exploitation :

- **Android** : `WifiManager`, `WifiNetworkSpecifier`
- **iOS** : `NEHotspotConfiguration`

### Limitations Volontaires

Pour respecter la vie privée et la sécurité :
- Pas d'accès aux mots de passe sans consentement utilisateur
- Pas d'exécution de commandes système dangereuses
- Pas d'attaques réelles sur les réseaux
- Simulations théoriques uniquement

### Disclaimer Obligatoire

Au premier lancement, l'utilisateur **DOIT** :
1. Lire et comprendre l'avertissement légal complet
2. Accepter explicitement les conditions d'utilisation
3. S'engager à utiliser l'outil de manière éthique

Sans acceptation, l'application ne peut pas être utilisée.

## 📖 Guide d'Utilisation

### 1. Premier Lancement

- Lisez attentivement le disclaimer
- Acceptez les conditions d'utilisation
- Accordez les permissions nécessaires (localisation pour Android)

### 2. Scanner les Réseaux

- Appuyez sur le bouton "🔍 Scanner"
- Attendez la fin du scan (quelques secondes)
- Consultez la liste des réseaux détectés

### 3. Analyser un Réseau

- Tapez sur un réseau dans la liste
- Consultez le score de sécurité
- Lisez les vulnérabilités détectées
- Suivez les recommandations de sécurisation

### 4. Mode Éducatif

- Accédez à l'onglet "📚" en bas de l'écran
- Explorez les tutoriels sur les protocoles WiFi
- Testez vos connaissances avec le quiz interactif

### 5. Générer un Rapport

- Depuis la page de détails d'un réseau
- Appuyez sur "📄 Générer un Rapport"
- Consultez ou exportez le rapport d'audit

## 🎓 Contenu Éducatif

L'application inclut des ressources pédagogiques complètes :

### Protocoles WiFi
- WEP : Pourquoi est-il obsolète
- WPA : Améliorations et limitations
- WPA2 : Standard actuel et vulnérabilités (KRACK)
- WPA3 : Dernière génération de sécurité

### Types d'Attaques (Théorie)
- Attaques passives vs actives
- Force brute et dictionnaire
- Attaque WPS (Pixie Dust)
- Evil Twin / Rogue AP
- KRACK (Key Reinstallation Attack)

### Bonnes Pratiques
- Configuration sécurisée du routeur
- Choix d'un mot de passe fort
- Désactivation de WPS
- Segmentation du réseau
- Mises à jour firmware

## 🤝 Contribution

Ce projet est développé dans un cadre académique. Les contributions sont les bienvenues pour :

- Corriger des bugs
- Améliorer la documentation
- Ajouter du contenu éducatif
- Optimiser le code
- Ajouter des tests unitaires

### Process de Contribution

1. Fork le projet
2. Créez une branche pour votre fonctionnalité (`git checkout -b feature/AmazingFeature`)
3. Committez vos changements (`git commit -m 'Add AmazingFeature'`)
4. Push vers la branche (`git push origin feature/AmazingFeature`)
5. Ouvrez une Pull Request

## 📄 Licence

Ce projet est sous licence **Educational Use Only**.

**Restrictions :**
- Usage éducatif et académique uniquement
- Interdiction d'utilisation malveillante
- Aucune garantie fournie

Voir le fichier [LICENSE](LICENSE) pour plus de détails.

## 👨‍💻 Auteur

Développé dans le cadre d'un concours d'entrée universitaire en informatique.

**Objectifs pédagogiques :**
- Démontrer la maîtrise du développement mobile
- Promouvoir l'éthique en cybersécurité
- Sensibiliser aux risques WiFi
- Enseigner les bonnes pratiques de sécurité

## 📞 Support

Pour toute question ou problème :
- Ouvrez une [Issue](https://github.com/votre-repo/wifi-security-auditor/issues)
- Consultez la [Documentation](docs/)
- Référez-vous au fichier [LEGAL.md](LEGAL.md)

## 🙏 Remerciements

- Communauté .NET MAUI
- Documentation officielle Android sur la sécurité WiFi
- Ressources éducatives en cybersécurité
- Tous les contributeurs du projet

---

**⚠️ Rappel Final :** Cette application est un outil éducatif. Son utilisation pour accéder à des réseaux sans autorisation est illégale. Respectez les lois et l'éthique.

**Version :** 2.0
**Dernière mise à jour :** Décembre 2025
