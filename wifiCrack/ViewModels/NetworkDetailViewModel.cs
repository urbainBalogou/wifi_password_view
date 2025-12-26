using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using wifiCrack.Helpers;
using wifiCrack.Models;
using wifiCrack.Services;

namespace wifiCrack.ViewModels
{
    public class NetworkDetailViewModel : ObservableObject
    {
        private readonly ISecurityAnalysisService _securityService;
        private readonly IWifiService _wifiService;

        private WifiNetwork _network;
        private SecurityAnalysis _analysis;
        private bool _isAnalyzing;
        private SavedWifiCredential _credential;

        public NetworkDetailViewModel(ISecurityAnalysisService securityService, IWifiService wifiService)
        {
            _securityService = securityService;
            _wifiService = wifiService;

            AnalyzeCommand = new RelayCommand(async () => await AnalyzeNetworkAsync());
            GetCredentialCommand = new RelayCommand(async () => await GetCredentialAsync());
        }

        public WifiNetwork Network
        {
            get => _network;
            set
            {
                SetProperty(ref _network, value);
                if (value != null)
                {
                    _ = AnalyzeNetworkAsync();
                    _ = GetCredentialAsync();
                }
            }
        }

        public SecurityAnalysis Analysis
        {
            get => _analysis;
            set => SetProperty(ref _analysis, value);
        }

        public SavedWifiCredential Credential
        {
            get => _credential;
            set => SetProperty(ref _credential, value);
        }

        public bool IsAnalyzing
        {
            get => _isAnalyzing;
            set => SetProperty(ref _isAnalyzing, value);
        }

        public ObservableCollection<Vulnerability> Vulnerabilities =>
            Analysis != null ? new ObservableCollection<Vulnerability>(Analysis.Vulnerabilities) : new ObservableCollection<Vulnerability>();

        public ObservableCollection<Recommendation> Recommendations =>
            Analysis != null ? new ObservableCollection<Recommendation>(Analysis.Recommendations) : new ObservableCollection<Recommendation>();

        public ICommand AnalyzeCommand { get; }
        public ICommand GetCredentialCommand { get; }

        private async Task AnalyzeNetworkAsync()
        {
            if (Network == null) return;

            try
            {
                IsAnalyzing = true;
                Analysis = await _securityService.AnalyzeNetworkAsync(Network);
                OnPropertyChanged(nameof(Vulnerabilities));
                OnPropertyChanged(nameof(Recommendations));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[NetworkDetailViewModel] Analysis error: {ex}");
            }
            finally
            {
                IsAnalyzing = false;
            }
        }

        private async Task GetCredentialAsync()
        {
            if (Network == null || !Network.IsSaved) return;

            try
            {
                Credential = await _wifiService.GetNetworkCredentialAsync(Network.Ssid);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[NetworkDetailViewModel] Credential error: {ex}");
            }
        }
    }
}
