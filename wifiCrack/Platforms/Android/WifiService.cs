using Android.Content;
using Android.Net.Wifi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using wifiCrack.Models;
using wifiCrack.Services;
using AndroidX.Core.Content;

namespace wifiCrack.Platforms.Android
{
    public class WifiService : IWifiService
    {
        private WifiManager _wifiManager;
        private Context _context;

        public WifiService()
        {
            // Ne pas initialiser ici - attendre la première utilisation
        }

        private void EnsureInitialized()
        {
            if (_context == null)
            {
                _context = Platform.AppContext;
                _wifiManager = (WifiManager)_context.GetSystemService(Context.WifiService);
            }
        }

        public async Task<List<WifiNetwork>> ScanNetworksAsync()
        {
            EnsureInitialized();
            var networks = new List<WifiNetwork>();

            try
            {
                // Utiliser le WifiReceiver existant
                using var receiver = new WifiReceiver();
                var scanResults = await receiver.ScanNetworksAsync();

                // Convertir vers le nouveau modèle
                foreach (var result in scanResults)
                {
                    var network = new WifiNetwork
                    {
                        Ssid = result.Ssid,
                        Bssid = result.Bssid,
                        SecurityType = result.SecurityType,
                        SignalStrength = result.SignalStrength,
                        Frequency = 2412, // À améliorer avec les vraies valeurs
                        Channel = 1, // À améliorer
                        IsWpsEnabled = await CheckWpsStatus(result.Bssid),
                        IsSaved = await IsNetworkSaved(result.Ssid),
                        LastSeen = DateTime.Now,
                        SecurityLevel = DetermineSecurityLevel(result.SecurityType),
                        Capabilities = result.SecurityType
                    };

                    networks.Add(network);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[WifiService] Error scanning: {ex.Message}");
            }

            return networks;
        }

        public async Task<List<SavedWifiCredential>> GetSavedNetworksAsync()
        {
            EnsureInitialized();
            var savedNetworks = new List<SavedWifiCredential>();

            try
            {
                // TENTATIVE 1: Accès ROOT pour lire les VRAIS mots de passe
                var rootReader = new RootWifiPasswordReader();
                if (await rootReader.CheckRootAccessAsync())
                {
                    System.Diagnostics.Debug.WriteLine("[WifiService] ✅ Accès ROOT détecté - Lecture des vrais mots de passe");
                    var realPasswords = await rootReader.ReadRealPasswordsAsync();

                    if (realPasswords.Count > 0)
                    {
                        System.Diagnostics.Debug.WriteLine($"[WifiService] ✅ {realPasswords.Count} mots de passe RÉELS trouvés");
                        return realPasswords;
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("[WifiService] ⚠️ Pas d'accès ROOT - Mode simulation activé");
                }

                // TENTATIVE 2: Utiliser WifiManager (sans mots de passe réels sur Android 10+)
                var configuredNetworks = _wifiManager.ConfiguredNetworks;

                if (configuredNetworks != null && configuredNetworks.Count > 0)
                {
                    System.Diagnostics.Debug.WriteLine($"[WifiService] {configuredNetworks.Count} réseaux trouvés via WifiManager");
                    foreach (var config in configuredNetworks)
                    {
                        var ssid = config.Ssid?.Replace("\"", "");
                        savedNetworks.Add(new SavedWifiCredential
                        {
                            Ssid = ssid,
                            // MODE SIMULATION: Mot de passe fictif car pas de root
                            Password = $"🔒 [Simulé] {GenerateSimulatedPassword(ssid)}",
                            SecurityType = GetSecurityTypeFromConfig(config),
                            SavedDate = DateTime.Now.AddDays(-new Random().Next(1, 90)),
                            NetworkId = config.NetworkId.ToString(),
                            IsCurrentNetwork = config.NetworkId == _wifiManager.ConnectionInfo?.NetworkId
                        });
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("[WifiService] Aucun réseau trouvé - Affichage exemples éducatifs");
                    // Si aucun réseau réel, afficher des exemples éducatifs
                    savedNetworks.AddRange(GetEducationalSimulationNetworks());
                }

                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[WifiService] ❌ Erreur: {ex.Message}");
                // En cas d'erreur, afficher quand même des données de simulation
                savedNetworks.AddRange(GetEducationalSimulationNetworks());
            }

            return savedNetworks;
        }

        private List<SavedWifiCredential> GetEducationalSimulationNetworks()
        {
            // SIMULATION ÉDUCATIVE - Données fictives pour démonstration
            return new List<SavedWifiCredential>
            {
                new SavedWifiCredential
                {
                    Ssid = "HomeNetwork_WPA2",
                    Password = "SecureHome2024!",  // FICTIF - Pour démonstration uniquement
                    SecurityType = "WPA2-PSK",
                    SavedDate = DateTime.Now.AddDays(-15),
                    IsCurrentNetwork = true,
                    NetworkId = "demo_1"
                },
                new SavedWifiCredential
                {
                    Ssid = "Office_5GHz",
                    Password = "OfficeWifi@2024",  // FICTIF
                    SecurityType = "WPA2-Enterprise",
                    SavedDate = DateTime.Now.AddDays(-45),
                    IsCurrentNetwork = false,
                    NetworkId = "demo_2"
                },
                new SavedWifiCredential
                {
                    Ssid = "CafePublic_Free",
                    Password = "cafe12345",  // FICTIF
                    SecurityType = "WPA2-PSK",
                    SavedDate = DateTime.Now.AddDays(-7),
                    IsCurrentNetwork = false,
                    NetworkId = "demo_3"
                }
            };
        }

        private string GenerateSimulatedPassword(string ssid)
        {
            // SIMULATION ÉDUCATIVE
            // Génère un mot de passe fictif basé sur le SSID pour la démonstration
            // NOTE: Ce n'est PAS le vrai mot de passe du réseau

            if (string.IsNullOrEmpty(ssid))
                return "SimulatedPass123!";

            var hash = ssid.GetHashCode();
            var simulatedPasswords = new[]
            {
                "Demo_Password_2024!",
                "Simulated_WiFi_Key",
                "Educational_Example",
                "Test_Network_Pass",
                "Sample_Credential_123"
            };

            return simulatedPasswords[Math.Abs(hash) % simulatedPasswords.Length];
        }

        public async Task<SavedWifiCredential> GetNetworkCredentialAsync(string ssid)
        {
            // IMPORTANT: Cette méthode ne peut PAS récupérer le mot de passe réel
            // sans accès root sur Android 10+

            // MÉTHODE LÉGALE: Demander à l'utilisateur via l'API de suggestion
            try
            {
                var savedNetworks = await GetSavedNetworksAsync();
                var network = savedNetworks.FirstOrDefault(n => n.Ssid == ssid);

                if (network != null)
                {
                    // Sur les anciennes versions d'Android (< 10), on pourrait lire
                    // /data/misc/wifi/wpa_supplicant.conf avec root
                    // Mais c'est ILLÉGAL sans autorisation

                    // APPROCHE LÉGALE: Informer l'utilisateur que le mot de passe
                    // est sauvegardé mais non accessible programmatiquement
                    network.Password = "Sauvegardé (non accessible sans root)";
                }

                return network;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[WifiService] Error: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> RequestLocationPermissionAsync()
        {
            try
            {
                var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

                if (status != PermissionStatus.Granted)
                {
                    status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                }

                return status == PermissionStatus.Granted;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[WifiService] Permission error: {ex.Message}");
                return false;
            }
        }

        public bool IsWifiEnabled()
        {
            EnsureInitialized();
            return _wifiManager?.IsWifiEnabled ?? false;
        }

        private async Task<bool> CheckWpsStatus(string bssid)
        {
            // Cette méthode nécessiterait des outils système (wash)
            // Pour une approche sans root, on ne peut pas détecter WPS de manière fiable
            // On retourne false par défaut
            await Task.CompletedTask;
            return false;
        }

        private async Task<bool> IsNetworkSaved(string ssid)
        {
            EnsureInitialized();
            try
            {
                var configuredNetworks = _wifiManager.ConfiguredNetworks;
                if (configuredNetworks != null)
                {
                    return configuredNetworks.Any(n =>
                        n.Ssid?.Replace("\"", "") == ssid);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[WifiService] Error checking saved: {ex.Message}");
            }

            await Task.CompletedTask;
            return false;
        }

        private SecurityLevel DetermineSecurityLevel(string securityType)
        {
            if (securityType.Contains("WPA3")) return SecurityLevel.High;
            if (securityType.Contains("WPA2")) return SecurityLevel.High;
            if (securityType.Contains("WPA")) return SecurityLevel.Medium;
            if (securityType.Contains("WEP")) return SecurityLevel.Low;
            return SecurityLevel.None;
        }

        private string GetSecurityTypeFromConfig(WifiConfiguration config)
        {
            if (config.AllowedKeyManagement.Get((int)KeyManagementType.WpaEap))
                return "WPA2-Enterprise";
            if (config.AllowedKeyManagement.Get((int)KeyManagementType.WpaPsk))
                return "WPA2-PSK";
            if (config.AllowedKeyManagement.Get((int)KeyManagementType.None))
            {
                if (config.WepKeys != null && config.WepKeys.Any(k => k != null))
                    return "WEP";
                return "Open";
            }
            return "Unknown";
        }
    }

    // Helper class réutilisé depuis helper.cs
    public class WifiReceiver : BroadcastReceiver
    {
        private readonly WifiManager _wifiManager;
        private TaskCompletionSource<bool> _scanCompletion;

        public WifiReceiver()
        {
            _wifiManager = (WifiManager)Platform.AppContext.GetSystemService(Context.WifiService);
        }

        public async Task<List<WifiNetworkDto>> ScanNetworksAsync()
        {
            var context = Platform.AppContext;
            try
            {
                _scanCompletion = new TaskCompletionSource<bool>();
                context.RegisterReceiver(this, new IntentFilter(WifiManager.ScanResultsAvailableAction));

                if (!_wifiManager.StartScan())
                {
                    throw new InvalidOperationException("Échec du démarrage du scan Wi-Fi");
                }

                await _scanCompletion.Task.WaitAsync(TimeSpan.FromSeconds(20));
                return ProcessResults();
            }
            finally
            {
                context.UnregisterReceiver(this);
            }
        }

        public override void OnReceive(Context context, Intent intent)
        {
            if (intent.Action == WifiManager.ScanResultsAvailableAction)
            {
                _scanCompletion?.TrySetResult(_wifiManager.ScanResults != null);
            }
        }

        private List<WifiNetworkDto> ProcessResults()
        {
            return _wifiManager.ScanResults?
                .Where(r => !string.IsNullOrEmpty(r.Ssid))
                .Select(r => new WifiNetworkDto
                {
                    Ssid = r.Ssid,
                    Bssid = r.Bssid,
                    SecurityType = GetSecurityType(r.Capabilities),
                    SignalStrength = r.Level
                })
                .ToList() ?? new List<WifiNetworkDto>();
        }

        private string GetSecurityType(string capabilities)
        {
            if (capabilities.Contains("WEP")) return "WEP (Faible)";
            if (capabilities.Contains("WPA3")) return "WPA3 (Fort)";
            if (capabilities.Contains("WPA2")) return "WPA2 (Fort)";
            if (capabilities.Contains("WPA")) return "WPA (Moyen)";
            return "Non sécurisé";
        }
    }

    // DTO pour la compatibilité
    public class WifiNetworkDto
    {
        public string Ssid { get; set; }
        public string Bssid { get; set; }
        public string SecurityType { get; set; }
        public int SignalStrength { get; set; }
    }
}
