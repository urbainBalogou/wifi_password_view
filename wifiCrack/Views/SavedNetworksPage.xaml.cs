using Microsoft.Maui.Controls;
using wifiCrack.ViewModels;
using wifiCrack.Services;

namespace wifiCrack.Views
{
    public partial class SavedNetworksPage : ContentPage
    {
        private readonly SavedNetworksViewModel _viewModel;

        public SavedNetworksPage()
        {
            InitializeComponent();

            // Créer le service et le ViewModel
#if ANDROID
            var wifiService = new Platforms.Android.WifiService();
#else
            var wifiService = new DummyWifiService();
#endif

            _viewModel = new SavedNetworksViewModel(wifiService);
            BindingContext = _viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.LoadNetworksAsync();
        }
    }
}
