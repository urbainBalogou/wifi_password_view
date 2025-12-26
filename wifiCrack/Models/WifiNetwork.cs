using System;

namespace wifiCrack.Models
{
    public class WifiNetwork
    {
        public string Ssid { get; set; }
        public string Bssid { get; set; }
        public string SecurityType { get; set; }
        public int SignalStrength { get; set; }
        public int Frequency { get; set; }
        public int Channel { get; set; }
        public bool IsWpsEnabled { get; set; }
        public bool IsSaved { get; set; }
        public DateTime LastSeen { get; set; }
        public SecurityLevel SecurityLevel { get; set; }
        public string Capabilities { get; set; }

        // Propriétés calculées
        public string SignalQuality => GetSignalQuality();
        public string SecurityIcon => GetSecurityIcon();
        public int SecurityScore => CalculateSecurityScore();

        private string GetSignalQuality()
        {
            if (SignalStrength >= -50) return "Excellent";
            if (SignalStrength >= -60) return "Bon";
            if (SignalStrength >= -70) return "Moyen";
            return "Faible";
        }

        private string GetSecurityIcon()
        {
            return SecurityLevel switch
            {
                SecurityLevel.High => "🔒",
                SecurityLevel.Medium => "🔓",
                SecurityLevel.Low => "⚠️",
                SecurityLevel.None => "❌",
                _ => "❓"
            };
        }

        private int CalculateSecurityScore()
        {
            int score = 10;

            // Pénalités basées sur le type de sécurité
            if (SecurityType.Contains("WEP")) score = 1;
            else if (SecurityType.Contains("WPA3")) score = 10;
            else if (SecurityType.Contains("WPA2")) score = 8;
            else if (SecurityType.Contains("WPA")) score = 5;
            else if (SecurityType.Contains("Open") || SecurityType.Contains("Non sécurisé")) score = 0;

            // Pénalité pour WPS activé
            if (IsWpsEnabled) score = Math.Max(0, score - 3);

            return score;
        }
    }

    public enum SecurityLevel
    {
        None = 0,
        Low = 1,
        Medium = 2,
        High = 3
    }
}
