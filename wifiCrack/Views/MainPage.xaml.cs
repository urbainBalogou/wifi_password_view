using Microsoft.Maui.Controls;
using wifiCrack.Models;
using wifiCrack.ViewModels;
using wifiCrack.Services;

namespace wifiCrack.Views
{
    public partial class MainPage : ContentPage
    {
        private readonly MainViewModel _viewModel;

        public MainPage()
        {
            InitializeComponent();

            // Initialiser le ViewModel
#if ANDROID
            var wifiService = new Platforms.Android.WifiService();
#else
            var wifiService = new Services.DummyWifiService(); // Pour les autres plateformes
#endif
            var securityService = new SecurityAnalysisService();

            _viewModel = new MainViewModel(wifiService, securityService);
            BindingContext = _viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Scanner automatiquement au démarrage
            if (_viewModel.Networks.Count == 0)
            {
                await _viewModel.ScanNetworksAsync();
            }
        }

        private async void OnNetworkSelected(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is WifiNetwork network)
            {
                await Navigation.PushAsync(new NetworkDetailPage(network));
            }
        }

        private async void OnNetworkTapped(object sender, EventArgs e)
        {
            if (sender is Frame frame && frame.BindingContext is WifiNetwork network)
            {
                await Navigation.PushAsync(new NetworkDetailPage(network));
            }
        }

        private async void OnEducationalClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EducationalPage());
        }

        private async void OnSettingsClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Paramètres", "Fonctionnalité à venir", "OK");
        }
    }
}
