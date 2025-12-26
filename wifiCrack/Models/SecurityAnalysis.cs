using System;
using System.Collections.Generic;

namespace wifiCrack.Models
{
    public class SecurityAnalysis
    {
        public WifiNetwork Network { get; set; }
        public List<Vulnerability> Vulnerabilities { get; set; } = new();
        public List<Recommendation> Recommendations { get; set; } = new();
        public int OverallScore { get; set; }
        public DateTime AnalysisDate { get; set; }

        public string RiskLevel => GetRiskLevel();

        private string GetRiskLevel()
        {
            return OverallScore switch
            {
                >= 8 => "Faible",
                >= 5 => "Moyen",
                >= 3 => "Élevé",
                _ => "Critique"
            };
        }
    }

    public class Vulnerability
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public VulnerabilitySeverity Severity { get; set; }
        public string Impact { get; set; }
        public string TechnicalDetails { get; set; }
    }

    public enum VulnerabilitySeverity
    {
        Info,
        Low,
        Medium,
        High,
        Critical
    }

    public class Recommendation
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public RecommendationPriority Priority { get; set; }
        public List<string> Steps { get; set; } = new();
    }

    public enum RecommendationPriority
    {
        Optional,
        Recommended,
        Important,
        Critical
    }
}
