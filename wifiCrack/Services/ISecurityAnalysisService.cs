using System.Threading.Tasks;
using wifiCrack.Models;

namespace wifiCrack.Services
{
    public interface ISecurityAnalysisService
    {
        Task<SecurityAnalysis> AnalyzeNetworkAsync(WifiNetwork network);
        SecurityLevel DetermineSecurityLevel(string securityType);
        int CalculateChannelFromFrequency(int frequency);
    }
}
