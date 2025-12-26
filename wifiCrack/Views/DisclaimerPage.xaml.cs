using Microsoft.Maui.Controls;

namespace wifiCrack.Views
{
    public partial class DisclaimerPage : ContentPage
    {
        public DisclaimerPage()
        {
            InitializeComponent();
        }

        private async void OnAcceptClicked(object sender, System.EventArgs e)
        {
            // Sauvegarder l'acceptation
            Preferences.Set("DisclaimerAccepted", true);
            Preferences.Set("DisclaimerAcceptedDate", System.DateTime.Now.ToString("o"));

            // Naviguer vers la page principale
            await Shell.Current.GoToAsync("//MainPage");
        }

        private async void OnDeclineClicked(object sender, System.EventArgs e)
        {
            bool confirm = await DisplayAlert(
                "Quitter l'application",
                "Vous devez accepter les conditions d'utilisation pour utiliser cette application.",
                "Quitter",
                "Retour");

            if (confirm)
            {
                // Quitter l'application
#if ANDROID
                Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
#elif IOS
                // iOS ne permet pas de quitter programmatiquement
                // L'utilisateur doit utiliser le bouton home
#elif WINDOWS
                Microsoft.Maui.Controls.Application.Current.Quit();
#endif
            }
        }

        protected override bool OnBackButtonPressed()
        {
            // Empêcher le retour arrière
            return true;
        }
    }
}
