using System.Collections.Generic;
using System.Linq;
using wifiCrack.Models;

namespace wifiCrack.Services
{
    public class EducationalService : IEducationalService
    {
        private readonly List<EducationalContent> _content;
        private readonly List<QuizQuestion> _quizQuestions;

        public EducationalService()
        {
            _content = InitializeContent();
            _quizQuestions = InitializeQuizQuestions();
        }

        public List<EducationalContent> GetAllContent() => _content;

        public EducationalContent GetContentByCategory(EducationalCategory category)
        {
            return _content.FirstOrDefault(c => c.Category == category);
        }

        public List<QuizQuestion> GetQuizQuestions() => _quizQuestions;

        private List<EducationalContent> InitializeContent()
        {
            return new List<EducationalContent>
            {
                new EducationalContent
                {
                    Title = "Protocoles de sécurité WiFi",
                    Category = EducationalCategory.Protocols,
                    Description = "Comprendre les différents protocoles de sécurité WiFi",
                    KeyPoints = new List<string>
                    {
                        "WEP : Obsolète et facilement cassable (1997)",
                        "WPA : Amélioration de WEP avec TKIP (2003)",
                        "WPA2 : Standard actuel avec AES-CCMP (2004)",
                        "WPA3 : Dernière génération avec SAE (2018)"
                    },
                    DetailedExplanation = @"
**WEP (Wired Equivalent Privacy)**
- Premier protocole de sécurité WiFi
- Utilise RC4 avec clés de 64 ou 128 bits
- Vulnérable aux attaques par injection et analyse statistique
- Peut être cassé en 5-10 minutes

**WPA (Wi-Fi Protected Access)**
- Introduit TKIP pour corriger les failles de WEP
- Utilise toujours RC4 mais avec rotation des clés
- Vulnérable à certaines attaques contre TKIP

**WPA2**
- Utilise AES-CCMP (plus fort que RC4)
- Vulnérable aux attaques par dictionnaire sur PSK
- Sensible à KRACK (Key Reinstallation Attack) en 2017

**WPA3**
- Utilise SAE (Simultaneous Authentication of Equals)
- Protection contre les attaques par dictionnaire offline
- Forward secrecy
- Chiffrement amélioré de 192 bits (mode Enterprise)
"
                },
                new EducationalContent
                {
                    Title = "Types d'attaques WiFi",
                    Category = EducationalCategory.Attacks,
                    Description = "Les principales attaques contre les réseaux WiFi",
                    KeyPoints = new List<string>
                    {
                        "Attaques passives : Écoute du trafic",
                        "Attaques actives : Injection de paquets",
                        "Attaques par force brute : Test de mots de passe",
                        "Attaques Evil Twin : Faux point d'accès"
                    },
                    DetailedExplanation = @"
**Attaques contre WEP**
- Capture d'IV (Initialization Vectors)
- Injection de paquets ARP
- Analyse statistique avec Aircrack-ng

**Attaques contre WPA/WPA2**
- Capture du handshake 4-way
- Attaque par dictionnaire offline
- Attaque PMKID (sans handshake complet)
- KRACK (réinstallation de clé)

**Attaques WPS**
- Pixie Dust : Exploit de l'implémentation
- Force brute PIN : ~11,000 essais maximum

**Evil Twin / Rogue AP**
- Création d'un faux point d'accès
- Usurpation du SSID légitime
- Interception du trafic (MITM)

**IMPORTANT : Ces informations sont à but ÉDUCATIF uniquement.**
Tester la sécurité d'un réseau sans autorisation est ILLÉGAL.
"
                },
                new EducationalContent
                {
                    Title = "Bonnes pratiques de sécurité",
                    Category = EducationalCategory.BestPractices,
                    Description = "Comment sécuriser votre réseau WiFi",
                    KeyPoints = new List<string>
                    {
                        "Utiliser WPA3 ou WPA2-AES",
                        "Mot de passe fort (12+ caractères)",
                        "Désactiver WPS",
                        "Mettre à jour le firmware du routeur",
                        "Changer les identifiants par défaut",
                        "Segmenter le réseau (invités séparés)"
                    },
                    DetailedExplanation = @"
**Configuration du routeur**
1. Changer le mot de passe admin par défaut
2. Mettre à jour le firmware régulièrement
3. Désactiver la gestion à distance si non nécessaire
4. Désactiver UPnP si possible

**Configuration WiFi**
1. Utiliser WPA3 ou WPA2-AES uniquement
2. Désactiver WPS absolument
3. SSID : éviter les informations personnelles
4. Créer un réseau invité séparé

**Mot de passe**
- Minimum 12 caractères
- Mélange de caractères (a-Z, 0-9, symboles)
- Pas de mots du dictionnaire
- Unique (ne pas réutiliser)

**Surveillance**
1. Vérifier régulièrement les appareils connectés
2. Activer les logs
3. Utiliser un IDS/IPS si possible
"
                },
                new EducationalContent
                {
                    Title = "Outils de test WiFi",
                    Category = EducationalCategory.Tools,
                    Description = "Outils utilisés pour l'audit de sécurité WiFi",
                    KeyPoints = new List<string>
                    {
                        "Aircrack-ng : Suite complète d'audit WiFi",
                        "Wireshark : Analyseur de paquets",
                        "Wash : Détection WPS",
                        "Reaver : Attaque WPS",
                        "Hashcat : Cracking de hash offline"
                    },
                    DetailedExplanation = @"
**Aircrack-ng Suite**
- airmon-ng : Mode moniteur
- airodump-ng : Capture de paquets
- aireplay-ng : Injection de paquets
- aircrack-ng : Cracking de clés

**Wireshark**
- Analyse de protocoles
- Inspection de paquets
- Détection d'anomalies

**Outils WPS**
- Wash : Scanner WPS
- Reaver : Attaque force brute PIN
- Bully : Alternative à Reaver
- Pixiewps : Attaque Pixie Dust

**IMPORTANT**
Ces outils doivent être utilisés UNIQUEMENT :
- Sur vos propres réseaux
- Avec autorisation écrite du propriétaire
- Dans un cadre légal (pentest, CTF)
- À des fins éducatives en environnement contrôlé
"
                },
                new EducationalContent
                {
                    Title = "Aspects légaux",
                    Category = EducationalCategory.Legal,
                    Description = "Cadre légal de l'audit de sécurité WiFi",
                    KeyPoints = new List<string>
                    {
                        "Accès non autorisé = ILLÉGAL",
                        "Autorisation écrite obligatoire",
                        "Pénalités : amendes et prison",
                        "Cadre légal : pentest autorisé, CTF, recherche"
                    },
                    DetailedExplanation = @"
**Législation française**
- Article 323-1 du Code pénal
- 'Accès frauduleux à un système de traitement automatisé'
- Jusqu'à 2 ans de prison et 60 000€ d'amende
- Aggravé en cas de données supprimées/modifiées

**Ce qui est ILLÉGAL**
❌ Scanner des réseaux WiFi qui ne vous appartiennent pas
❌ Tenter de se connecter sans autorisation
❌ Capturer du trafic réseau d'autrui
❌ Utiliser des outils d'attaque sur des cibles non autorisées

**Ce qui est LÉGAL**
✓ Tester votre propre réseau
✓ Audit avec autorisation écrite du propriétaire
✓ CTF et compétitions de sécurité
✓ Recherche en environnement contrôlé
✓ Formation sur réseaux de test

**Recommandations**
1. Toujours obtenir une autorisation ÉCRITE
2. Définir clairement le périmètre des tests
3. Documenter toutes les actions
4. Respecter la confidentialité des données
5. Signaler les vulnérabilités de manière responsable
"
                }
            };
        }

        private List<QuizQuestion> InitializeQuizQuestions()
        {
            return new List<QuizQuestion>
            {
                new QuizQuestion
                {
                    Question = "Quel est le protocole WiFi le plus sécurisé actuellement ?",
                    Options = new List<string> { "WEP", "WPA", "WPA2", "WPA3" },
                    CorrectAnswerIndex = 3,
                    Explanation = "WPA3 est le protocole le plus récent et le plus sécurisé, introduit en 2018 avec SAE et protection contre les attaques par dictionnaire."
                },
                new QuizQuestion
                {
                    Question = "Combien de temps faut-il pour casser une clé WEP ?",
                    Options = new List<string> { "Plusieurs jours", "Quelques heures", "5-10 minutes", "Impossible à casser" },
                    CorrectAnswerIndex = 2,
                    Explanation = "Avec les outils modernes comme Aircrack-ng, une clé WEP peut être cassée en 5 à 10 minutes en capturant suffisamment d'IV."
                },
                new QuizQuestion
                {
                    Question = "Pourquoi WPS est-il considéré comme dangereux ?",
                    Options = new List<string> {
                        "Il ralentit le réseau",
                        "Il consomme trop d'énergie",
                        "Le PIN est vulnérable au force brute",
                        "Il nécessite un mot de passe complexe"
                    },
                    CorrectAnswerIndex = 2,
                    Explanation = "Le PIN WPS de 8 chiffres peut être forcé en testant seulement ~11,000 combinaisons grâce à une faille dans son implémentation."
                },
                new QuizQuestion
                {
                    Question = "Quelle est la longueur minimale recommandée pour un mot de passe WiFi ?",
                    Options = new List<string> { "6 caractères", "8 caractères", "12 caractères", "16 caractères" },
                    CorrectAnswerIndex = 2,
                    Explanation = "12 caractères minimum avec un mélange de types (majuscules, minuscules, chiffres, symboles) offre une bonne protection."
                },
                new QuizQuestion
                {
                    Question = "Qu'est-ce qu'une attaque 'Evil Twin' ?",
                    Options = new List<string> {
                        "Deux routeurs identiques",
                        "Un faux point d'accès imitant un réseau légitime",
                        "Une attaque sur deux réseaux simultanément",
                        "Un virus WiFi"
                    },
                    CorrectAnswerIndex = 1,
                    Explanation = "Evil Twin consiste à créer un faux point d'accès avec le même SSID qu'un réseau légitime pour intercepter le trafic."
                },
                new QuizQuestion
                {
                    Question = "Tester la sécurité d'un réseau WiFi sans autorisation est :",
                    Options = new List<string> {
                        "Légal si c'est pour apprendre",
                        "Légal si on ne modifie rien",
                        "Illégal dans tous les cas",
                        "Légal si le réseau est mal sécurisé"
                    },
                    CorrectAnswerIndex = 2,
                    Explanation = "L'accès non autorisé à un système informatique est illégal selon l'article 323-1 du Code pénal, passible de 2 ans de prison et 60 000€ d'amende."
                }
            };
        }
    }
}
