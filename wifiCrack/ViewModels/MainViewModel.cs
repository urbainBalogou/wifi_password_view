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
    public class MainViewModel : ObservableObject
    {
        private readonly IWifiService _wifiService;
        private readonly ISecurityAnalysisService _securityService;

        private bool _isScanning;
        private bool _isLoading;
        private string _statusMessage;
        private ObservableCollection<WifiNetwork> _networks;
        private WifiNetwork _selectedNetwork;

        public MainViewModel(IWifiService wifiService, ISecurityAnalysisService securityService)
        {
            _wifiService = wifiService;
            _securityService = securityService;

            Networks = new ObservableCollection<WifiNetwork>();
            StatusMessage = "Prêt à scanner";
            ScanCommand = new RelayCommand(async () => await ScanNetworksAsync(), () => !IsScanning);
            RefreshCommand = new RelayCommand(async () => await ScanNetworksAsync(), () => !IsScanning);
        }

        public ObservableCollection<WifiNetwork> Networks
        {
            get => _networks;
            set => SetProperty(ref _networks, value);
        }

        public WifiNetwork SelectedNetwork
        {
            get => _selectedNetwork;
            set => SetProperty(ref _selectedNetwork, value);
        }

        public bool IsScanning
        {
            get => _isScanning;
            set
            {
                SetProperty(ref _isScanning, value);
                ((RelayCommand)ScanCommand).RaiseCanExecuteChanged();
                ((RelayCommand)RefreshCommand).RaiseCanExecuteChanged();
            }
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public string StatusMessage
        {
            get => _statusMessage;
            set => SetProperty(ref _statusMessage, value);
        }

        public ICommand ScanCommand { get; }
        public ICommand RefreshCommand { get; }

        public async Task ScanNetworksAsync()
        {
            try
            {
                IsScanning = true;
                IsLoading = true;
                StatusMessage = "Vérification des permissions...";

                // Vérifier les permissions
                var hasPermission = await _wifiService.RequestLocationPermissionAsync();
                if (!hasPermission)
                {
                    StatusMessage = "Permission de localisation requise";
                    return;
                }

                // Vérifier que le WiFi est activé
                if (!_wifiService.IsWifiEnabled())
                {
                    StatusMessage = "Veuillez activer le Wi-Fi";
                    return;
                }

                StatusMessage = "Scan en cours...";

                // Scanner les réseaux
                var networks = await _wifiService.ScanNetworksAsync();

                // Trier par force du signal
                var sortedNetworks = networks.OrderByDescending(n => n.SignalStrength).ToList();

                // Mettre à jour la collection
                Networks.Clear();
                foreach (var network in sortedNetworks)
                {
                    Networks.Add(network);
                }

                StatusMessage = $"{networks.Count} réseau(x) trouvé(s)";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Erreur: {ex.Message}";
                System.Diagnostics.Debug.WriteLine($"[MainViewModel] Error: {ex}");
            }
            finally
            {
                IsScanning = false;
                IsLoading = false;
            }
        }
    }
}
