using System.Collections.Generic;
using System.Threading.Tasks;
using wifiCrack.Models;

namespace wifiCrack.Services
{
    // Service de test pour les plateformes non-Android
    public class DummyWifiService : IWifiService
    {
        public async Task<List<WifiNetwork>> ScanNetworksAsync()
        {
            await Task.Delay(1000);

            return new List<WifiNetwork>
            {
                new WifiNetwork
                {
                    Ssid = "Demo_WPA3_Network",
                    Bssid = "00:11:22:33:44:55",
                    SecurityType = "WPA3 (Fort)",
                    SignalStrength = -45,
                    Frequency = 5180,
                    Channel = 36,
                    IsWpsEnabled = false,
                    IsSaved = false,
                    LastSeen = System.DateTime.Now,
                    SecurityLevel = SecurityLevel.High,
                    Capabilities = "WPA3-Personal"
                },
                new WifiNetwork
                {
                    Ssid = "Demo_WPA2_Network",
                    Bssid = "00:11:22:33:44:56",
                    SecurityType = "WPA2 (Fort)",
                    SignalStrength = -55,
                    Frequency = 2437,
                    Channel = 6,
                    IsWpsEnabled = true,
                    IsSaved = true,
                    LastSeen = System.DateTime.Now,
                    SecurityLevel = SecurityLevel.High,
                    Capabilities = "WPA2-Personal"
                },
                new WifiNetwork
                {
                    Ssid = "Demo_WEP_Network",
                    Bssid = "00:11:22:33:44:57",
                    SecurityType = "WEP (Faible)",
                    SignalStrength = -70,
                    Frequency = 2412,
                    Channel = 1,
                    IsWpsEnabled = false,
                    IsSaved = false,
                    LastSeen = System.DateTime.Now,
                    SecurityLevel = SecurityLevel.Low,
                    Capabilities = "WEP"
                }
            };
        }

        public async Task<List<SavedWifiCredential>> GetSavedNetworksAsync()
        {
            await Task.Delay(500);

            // SIMULATION ÉDUCATIVE - Données fictives pour démonstration
            return new List<SavedWifiCredential>
            {
                new SavedWifiCredential
                {
                    Ssid = "Demo_WPA2_Network",
                    Password = "demo_password_123",  // SIMULATION UNIQUEMENT
                    SecurityType = "WPA2-PSK",
                    SavedDate = System.DateTime.Now.AddDays(-7),
                    IsCurrentNetwork = true,
                    NetworkId = "1"
                },
                new SavedWifiCredential
                {
                    Ssid = "HomeNetwork_5G",
                    Password = "MySecurePass2024!",  // SIMULATION UNIQUEMENT
                    SecurityType = "WPA2-PSK",
                    SavedDate = System.DateTime.Now.AddDays(-30),
                    IsCurrentNetwork = false,
                    NetworkId = "2"
                },
                new SavedWifiCredential
                {
                    Ssid = "CoffeeShop_WiFi",
                    Password = "coffee2024",  // SIMULATION UNIQUEMENT
                    SecurityType = "WPA2-PSK",
                    SavedDate = System.DateTime.Now.AddDays(-3),
                    IsCurrentNetwork = false,
                    NetworkId = "3"
                }
            };
        }

        public async Task<SavedWifiCredential> GetNetworkCredentialAsync(string ssid)
        {
            var savedNetworks = await GetSavedNetworksAsync();
            return savedNetworks.Find(n => n.Ssid == ssid);
        }

        public async Task<bool> RequestLocationPermissionAsync()
        {
            await Task.Delay(100);
            return true;
        }

        public bool IsWifiEnabled()
        {
            return true;
        }
    }
}
