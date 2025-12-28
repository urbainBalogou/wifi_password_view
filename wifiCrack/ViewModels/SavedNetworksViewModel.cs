using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using wifiCrack.Helpers;
using wifiCrack.Models;
using wifiCrack.Services;

namespace wifiCrack.ViewModels
{
    public class SavedNetworksViewModel : ObservableObject
    {
        private readonly IWifiService _wifiService;
        private bool _isLoading;
        private bool _isRefreshing;

        public ObservableCollection<SavedWifiCredential> SavedNetworks { get; }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }

        public ICommand LoadNetworksCommand { get; }
        public ICommand RefreshCommand { get; }
        public ICommand CopyPasswordCommand { get; }

        public SavedNetworksViewModel(IWifiService wifiService)
        {
            _wifiService = wifiService;
            SavedNetworks = new ObservableCollection<SavedWifiCredential>();

            LoadNetworksCommand = new RelayCommand(async () => await LoadNetworksAsync());
            RefreshCommand = new RelayCommand(async () => await RefreshNetworksAsync());
            CopyPasswordCommand = new RelayCommand<SavedWifiCredential>(CopyPassword);
        }

        public async Task LoadNetworksAsync()
        {
            if (IsLoading)
                return;

            IsLoading = true;

            try
            {
                var networks = await _wifiService.GetSavedNetworksAsync();

                SavedNetworks.Clear();
                foreach (var network in networks)
                {
                    SavedNetworks.Add(network);
                }
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task RefreshNetworksAsync()
        {
            IsRefreshing = true;

            try
            {
                await LoadNetworksAsync();
            }
            finally
            {
                IsRefreshing = false;
            }
        }

        private async void CopyPassword(SavedWifiCredential network)
        {
            if (network != null && !string.IsNullOrEmpty(network.Password))
            {
                await Clipboard.SetTextAsync(network.Password);
            }
        }
    }
}
