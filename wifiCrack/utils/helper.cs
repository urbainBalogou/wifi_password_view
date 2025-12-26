
using Android.Content;
using Android.Net.Wifi;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace wifiCrack.utils
{
    public record WifiNetwork(string Ssid, string Bssid, string SecurityType, int SignalStrength);
#if ANDROID
    public class WifiReceiver : Android.Content.BroadcastReceiver
    {
        private readonly WifiManager _wifiManager;
        private TaskCompletionSource<bool> _scanCompletion;
        private bool _scanStarted;
        public WifiReceiver()
        {
            _wifiManager = (WifiManager)Android.App.Application.Context.GetSystemService(Context.WifiService);
        }

        public async Task<List<WifiNetwork>> ScanNetworksAsync()
        {
            var context = Android.App.Application.Context;
            try
            {
                _scanCompletion = new TaskCompletionSource<bool>();
                

                // Enregistrer le receiver
                context.RegisterReceiver(this, new IntentFilter(WifiManager.ScanResultsAvailableAction));

                _scanStarted = _wifiManager.StartScan();
                if (!_scanStarted)
                {
                    throw new InvalidOperationException("Échec du démarrage du scan Wi-Fi");
                }
                    await _scanCompletion.Task.WaitAsync(TimeSpan.FromSeconds(20));
                    return ProcessResults();
            }
            finally
            {
                // Désenregistrer le receiver
                context.UnregisterReceiver(this);
            }
        }


        private string GetSecurityType(string capabilities)
        {
            if (capabilities.Contains("WEP")) return "WEP (Faible)";
            if (capabilities.Contains("WPA3")) return "WPA3 (Fort)";
            if (capabilities.Contains("WPA2")) return "WPA2 (Fort)";
            if (capabilities.Contains("WPA")) return "WPA (Moyen)";
            return "Non sécurisé";
        }

        public override void OnReceive(Context context, Intent intent)
        {
            if (intent.Action == WifiManager.ScanResultsAvailableAction)
            {
                if (_wifiManager.ScanResults == null)
                {
                    _scanCompletion.TrySetException(new InvalidOperationException("Aucun résultat de scan"));
                }
                else
                {
                    _scanCompletion.TrySetResult(true);
                }
            }
        }

        private List<WifiNetwork> ProcessResults()
        {
            return _wifiManager.ScanResults?
                .Where(r => !string.IsNullOrEmpty(r.Ssid))
                .Select(r => new WifiNetwork(
                    r.Ssid,
                    r.Bssid,
                    GetSecurityType(r.Capabilities),
                    r.Level))
                .ToList() ?? new List<WifiNetwork>();
        }








    }

    public interface INetworkSecurityService
    {
        Task<bool> IsWpsEnabled(string bssid);
    }

#endif

}


