namespace wifiCrack;
using Android.Content;
using Android.Net.Wifi;
using Android.Util;
using System.Diagnostics;
using wifiCrack.Platforms.Android;
using wifiCrack.utils;


public partial class MainPage : ContentPage
{
    NetworkSecurityService NetworkService = new NetworkSecurityService();
    public MainPage()
	{
		InitializeComponent();

    }

	private async void OnScanClicked(object sender, EventArgs e)
	{
        Console.WriteLine("Button clické");
#if ANDROID
        await ScanAndroidNetwork();
#endif
    }

#if ANDROID
    private async Task ScanAndroidNetwork()
    {
        try
        {
            ScanButton.IsEnabled = false;
            LoadingIndicator.IsVisible = true;

            // Vérification permissions
            var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                if (status != PermissionStatus.Granted)
                {
                    await DisplayAlert("Permission requise", "La localisation est nécessaire", "OK");
                    return;
                }
            }

            using var wifiManager = (WifiManager)Android.App.Application.Context.GetSystemService(Context.WifiService)!;
            Log.Debug("WifiAudit", $"Wifi manager obtenu: {wifiManager}");

            if (!wifiManager.IsWifiEnabled)
            {
                await DisplayAlert("Erreur", "Activez le Wi-Fi", "OK");
                return;
            }

            using var receiver = new WifiReceiver();
            Log.Debug("WifiAudit", "Scan en cours...");

            var networks = await receiver.ScanNetworksAsync();
            NetworkList.ItemsSource = networks.OrderByDescending(n => n.SignalStrength);
        }
        catch (Exception ex)
        {
            Log.Error("WifiAudit", $"ERREUR: {ex}");
            await DisplayAlert("Erreur", ex.Message, "OK");
        }
        finally
        {
            ScanButton.IsEnabled = true;
            LoadingIndicator.IsVisible = false;
        }
    }

    private async void OnNetworkSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if(e.SelectedItem is not WifiNetwork wifi)
        {
            return;
        }
        var choice = await DisplayActionSheet(
            $"Réseau: {wifi.Ssid}",
           "Annuler",
           null,
           "Voir les vulnérabilités",
           "Simuler l'attaque WPS");


        if (choice == "Voir les vulnérabilités")
        {
            await DisplayVulnerabilities(wifi);
        }
        else if (choice == "Simuler l'attaque WPS (démonstration)")
        {
            await SimulateWpsAttack(wifi);
        }
    }

    private async Task SimulateWpsAttack(WifiNetwork network)
    {
        try
        {
            if (!await VerifyRootAccess())
                return;

            var result = await ExecuteShellCommand($"echo 'Simulation d'attaque sur {network.Bssid}'");

            await DisplayAlert("Simulation",
                $"Démonstration théorique terminée\nRéseau: {network.Ssid}\nVulnérabilités détectées: WPS Actif",
                "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erreur", $"Une erreur s'est produite: {ex.Message}","OK");
        }
    }

#if ANDROID
    private async Task<bool> VerifyRootAccess()
    {
        var result = await ExecuteShellCommand("su -c 'echo RootCheck'");
        if (!result.Contains("RootCheck"))
        {
            await DisplayAlert("Root requis", "L'appareil doit être rooté", "OK");
            return false;
        }
        return true;
    }

    private async Task<string> ExecuteShellCommand(string command)
    {
        using var process = new Process();
        process.StartInfo = new ProcessStartInfo
        {
            FileName = "sh",
            Arguments = $"-c \"{command}\"",
            RedirectStandardOutput = true,
            UseShellExecute = false
        };

        process.Start();
        string output = await process.StandardOutput.ReadToEndAsync();
        await process.WaitForExitAsync();

        return output;
    }

    private async Task DisplayVulnerabilities(WifiNetwork network)
    {
        var vulnerabilities = new List<string>();

        // Analyse basique
        if (network.SecurityType.Contains("WEP"))
            vulnerabilities.Add("WEP obsolète (cassable en 5 min)");

        Console.WriteLine($"networks.....{network.Bssid}");

        if (await NetworkService.IsWpsEnabled(network.Bssid))
            vulnerabilities.Add("WPS Actif (vulnérable à Pixie Dust)");

        await DisplayAlert("Vulnérabilités",
            string.Join("\n- ", vulnerabilities),
            "OK");
    }

    private void OnFilterClicked(object sender, EventArgs e)
    {

    }  
    
    private void OnNetworkTapped(object sender, EventArgs e)
    {

    }

#endif



#endif

}

