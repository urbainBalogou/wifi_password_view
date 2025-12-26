namespace wifiCrack;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        // Vérifier si le disclaimer a été accepté
        Loaded += OnShellLoaded;
    }

    private async void OnShellLoaded(object sender, EventArgs e)
    {
        var disclaimerAccepted = Preferences.Get("DisclaimerAccepted", false);

        if (disclaimerAccepted)
        {
            // Aller directement à la page principale
            await Shell.Current.GoToAsync("//MainPage");
        }
        else
        {
            // Afficher le disclaimer
            await Shell.Current.GoToAsync("//DisclaimerPage");
        }
    }
}
