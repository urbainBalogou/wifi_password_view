using System;

namespace wifiCrack.Models
{
    public class SavedWifiCredential
    {
        public string Ssid { get; set; }
        public string Password { get; set; }
        public string SecurityType { get; set; }
        public DateTime SavedDate { get; set; }
        public bool IsCurrentNetwork { get; set; }
        public string NetworkId { get; set; }
    }
}
