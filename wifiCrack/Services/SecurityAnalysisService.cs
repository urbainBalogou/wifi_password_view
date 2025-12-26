using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using wifiCrack.Models;

namespace wifiCrack.Services
{
    public class SecurityAnalysisService : ISecurityAnalysisService
    {
        public async Task<SecurityAnalysis> AnalyzeNetworkAsync(WifiNetwork network)
        {
            await Task.Delay(100); // Simulation d'analyse

            var analysis = new SecurityAnalysis
            {
                Network = network,
                AnalysisDate = DateTime.Now,
                OverallScore = network.SecurityScore
            };

            // Analyse des vulnérabilités
            AnalyzeVulnerabilities(network, analysis);

            // Génération des recommandations
            GenerateRecommendations(network, analysis);

            return analysis;
        }

        private void AnalyzeVulnerabilities(WifiNetwork network, SecurityAnalysis analysis)
        {
            // WEP
            if (network.SecurityType.Contains("WEP"))
            {
                analysis.Vulnerabilities.Add(new Vulnerability
                {
                    Title = "Protocole WEP obsolète",
                    Description = "Le protocole WEP (Wired Equivalent Privacy) est cassable en quelques minutes",
                    Severity = VulnerabilitySeverity.Critical,
                    Impact = "Un attaquant peut capturer le trafic et déchiffrer la clé WEP en 5-10 minutes avec des outils comme Aircrack-ng",
                    TechnicalDetails = "WEP utilise RC4 avec une IV de 24 bits, permettant des attaques par injection de paquets et analyse statistique"
                });
            }

            // WPS
            if (network.IsWpsEnabled)
            {
                analysis.Vulnerabilities.Add(new Vulnerability
                {
                    Title = "WPS (Wi-Fi Protected Setup) activé",
                    Description = "Le WPS est vulnérable aux attaques par force brute",
                    Severity = VulnerabilitySeverity.High,
                    Impact = "Un attaquant peut récupérer la clé WPA/WPA2 via l'attaque Pixie Dust ou force brute sur le PIN WPS",
                    TechnicalDetails = "Le PIN WPS ne contient que 8 chiffres (dont 1 checksum), réduisant l'espace de recherche à ~11,000 combinaisons"
                });
            }

            // WPA (non WPA2/WPA3)
            if (network.SecurityType.Contains("WPA") && !network.SecurityType.Contains("WPA2") && !network.SecurityType.Contains("WPA3"))
            {
                analysis.Vulnerabilities.Add(new Vulnerability
                {
                    Title = "WPA version 1 détecté",
                    Description = "WPA1 présente des faiblesses cryptographiques",
                    Severity = VulnerabilitySeverity.Medium,
                    Impact = "Vulnérable aux attaques TKIP et dégradation forcée du protocole",
                    TechnicalDetails = "WPA1 utilise TKIP qui peut être exploité via des attaques de type chopchop"
                });
            }

            // Réseau ouvert
            if (network.SecurityType.Contains("Open") || network.SecurityType.Contains("Non sécurisé"))
            {
                analysis.Vulnerabilities.Add(new Vulnerability
                {
                    Title = "Réseau ouvert sans chiffrement",
                    Description = "Aucune sécurité - tout le trafic est en clair",
                    Severity = VulnerabilitySeverity.Critical,
                    Impact = "Toutes les données transmises peuvent être interceptées (attaque Man-in-the-Middle)",
                    TechnicalDetails = "Absence totale de chiffrement permettant l'écoute passive et active du trafic"
                });
            }

            // Signal faible
            if (network.SignalStrength < -80)
            {
                analysis.Vulnerabilities.Add(new Vulnerability
                {
                    Title = "Signal très faible",
                    Description = "La connexion sera instable",
                    Severity = VulnerabilitySeverity.Info,
                    Impact = "Déconnexions fréquentes, faible débit",
                    TechnicalDetails = "Signal inférieur à -80 dBm indique une distance importante ou des obstacles"
                });
            }
        }

        private void GenerateRecommendations(WifiNetwork network, SecurityAnalysis analysis)
        {
            // Recommandations basées sur les vulnérabilités
            if (network.SecurityType.Contains("WEP"))
            {
                analysis.Recommendations.Add(new Recommendation
                {
                    Title = "Passer à WPA3",
                    Description = "Migrez immédiatement vers WPA3 ou au minimum WPA2",
                    Priority = RecommendationPriority.Critical,
                    Steps = new List<string>
                    {
                        "Accéder à l'interface d'administration du routeur",
                        "Aller dans les paramètres WiFi/Sécurité",
                        "Sélectionner WPA2-PSK (AES) ou WPA3",
                        "Définir un mot de passe fort (12+ caractères)",
                        "Sauvegarder et redémarrer le routeur"
                    }
                });
            }

            if (network.IsWpsEnabled)
            {
                analysis.Recommendations.Add(new Recommendation
                {
                    Title = "Désactiver WPS",
                    Description = "Le WPS n'est pas nécessaire et représente un risque majeur",
                    Priority = RecommendationPriority.Important,
                    Steps = new List<string>
                    {
                        "Accéder à l'interface du routeur",
                        "Chercher 'WPS' ou 'Wi-Fi Protected Setup'",
                        "Désactiver cette fonctionnalité",
                        "Sauvegarder les paramètres"
                    }
                });
            }

            if (network.SecurityType.Contains("WPA2"))
            {
                analysis.Recommendations.Add(new Recommendation
                {
                    Title = "Envisager WPA3",
                    Description = "WPA3 offre une meilleure protection",
                    Priority = RecommendationPriority.Recommended,
                    Steps = new List<string>
                    {
                        "Vérifier si votre routeur supporte WPA3",
                        "Mettre à jour le firmware si nécessaire",
                        "Activer WPA3 ou le mode transitoire WPA2/WPA3",
                        "Tester la compatibilité avec vos appareils"
                    }
                });
            }

            // Recommandations générales
            analysis.Recommendations.Add(new Recommendation
            {
                Title = "Utiliser un mot de passe fort",
                Description = "Un mot de passe complexe protège contre les attaques par dictionnaire",
                Priority = RecommendationPriority.Important,
                Steps = new List<string>
                {
                    "Minimum 12 caractères",
                    "Mélanger majuscules, minuscules, chiffres et symboles",
                    "Éviter les mots du dictionnaire",
                    "Ne pas réutiliser d'autres mots de passe"
                }
            });

            analysis.Recommendations.Add(new Recommendation
            {
                Title = "Masquer le SSID (optionnel)",
                Description = "Rend le réseau moins visible mais ne constitue pas une vraie sécurité",
                Priority = RecommendationPriority.Optional,
                Steps = new List<string>
                {
                    "Cette mesure offre une 'sécurité par l'obscurité'",
                    "N'empêche pas les scans actifs",
                    "Peut causer des problèmes de connexion",
                    "À utiliser en complément, pas comme seule protection"
                }
            });
        }

        public SecurityLevel DetermineSecurityLevel(string securityType)
        {
            if (securityType.Contains("WPA3")) return SecurityLevel.High;
            if (securityType.Contains("WPA2")) return SecurityLevel.High;
            if (securityType.Contains("WPA")) return SecurityLevel.Medium;
            if (securityType.Contains("WEP")) return SecurityLevel.Low;
            return SecurityLevel.None;
        }

        public int CalculateChannelFromFrequency(int frequency)
        {
            // Bande 2.4 GHz
            if (frequency >= 2412 && frequency <= 2484)
            {
                return (frequency - 2412) / 5 + 1;
            }
            // Bande 5 GHz
            else if (frequency >= 5170 && frequency <= 5825)
            {
                return (frequency - 5170) / 5 + 34;
            }
            return 0;
        }
    }
}
