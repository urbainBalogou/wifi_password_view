using Microsoft.Maui.Controls;
using wifiCrack.Models;
using wifiCrack.Services;
using wifiCrack.ViewModels;

namespace wifiCrack.Views
{
    public partial class NetworkDetailPage : ContentPage
    {
        private readonly NetworkDetailViewModel _viewModel;

        public NetworkDetailPage(WifiNetwork network)
        {
            InitializeComponent();

#if ANDROID
            var wifiService = new Platforms.Android.WifiService();
#else
            var wifiService = new Services.DummyWifiService();
#endif
            var securityService = new SecurityAnalysisService();

            _viewModel = new NetworkDetailViewModel(securityService, wifiService);
            _viewModel.Network = network;

            BindingContext = _viewModel;
        }

        private async void OnGenerateReportClicked(object sender, System.EventArgs e)
        {
            if (_viewModel.Analysis == null)
            {
                await DisplayAlert("Rapport", "Analyse en cours...", "OK");
                return;
            }

            // Générer un rapport texte simple
            var report = GenerateTextReport();

            // Afficher ou partager le rapport
            await DisplayAlert("Rapport de Sécurité", report, "OK");

            // TODO: Implémenter l'export PDF
        }

        private string GenerateTextReport()
        {
            var report = $@"
=== RAPPORT D'AUDIT DE SÉCURITÉ WiFi ===

Réseau: {_viewModel.Network.Ssid}
BSSID: {_viewModel.Network.Bssid}
Date: {System.DateTime.Now:dd/MM/yyyy HH:mm}

--- SCORE DE SÉCURITÉ ---
Score global: {_viewModel.Network.SecurityScore}/10
Niveau de risque: {_viewModel.Analysis.RiskLevel}

--- INFORMATIONS TECHNIQUES ---
Type de sécurité: {_viewModel.Network.SecurityType}
Force du signal: {_viewModel.Network.SignalQuality} ({_viewModel.Network.SignalStrength} dBm)
Fréquence: {_viewModel.Network.Frequency} MHz
Canal: {_viewModel.Network.Channel}
WPS: {(_viewModel.Network.IsWpsEnabled ? "Activé ⚠️" : "Désactivé ✓")}

--- VULNÉRABILITÉS ({_viewModel.Vulnerabilities.Count}) ---
";

            foreach (var vuln in _viewModel.Vulnerabilities)
            {
                report += $@"
[{vuln.Severity}] {vuln.Title}
{vuln.Description}
Impact: {vuln.Impact}
";
            }

            report += $@"

--- RECOMMANDATIONS ({_viewModel.Recommendations.Count}) ---
";

            foreach (var rec in _viewModel.Recommendations)
            {
                report += $@"
[{rec.Priority}] {rec.Title}
{rec.Description}
";
            }

            report += @"

--- AVERTISSEMENT LÉGAL ---
Ce rapport est généré à des fins éducatives uniquement.
L'audit de réseaux WiFi sans autorisation est illégal.
";

            return report;
        }
    }
}
