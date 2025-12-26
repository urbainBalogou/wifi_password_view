using System.Collections.Generic;
using System.Threading.Tasks;
using wifiCrack.Models;

namespace wifiCrack.Services
{
    public interface IWifiService
    {
        Task<List<WifiNetwork>> ScanNetworksAsync();
        Task<List<SavedWifiCredential>> GetSavedNetworksAsync();
        Task<bool> RequestLocationPermissionAsync();
        Task<SavedWifiCredential> GetNetworkCredentialAsync(string ssid);
        bool IsWifiEnabled();
    }
}
