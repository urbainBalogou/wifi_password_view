using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using wifiCrack.Models;

namespace wifiCrack.Platforms.Android
{
    /// <summary>
    /// Lecteur de mots de passe WiFi avec accès ROOT
    /// ATTENTION: Nécessite appareil rooté et autorisation utilisateur
    /// </summary>
    public class RootWifiPasswordReader
    {
        private const string WPA_SUPPLICANT_PATH = "/data/misc/wifi/wpa_supplicant.conf";
        private const string WPA_SUPPLICANT_PATH_ALT = "/data/wifi/bcmdhd.cal";
        private const string WPA_SUPPLICANT_PATH_ALT2 = "/data/misc/wifi/WifiConfigStore.xml";

        /// <summary>
        /// Vérifie si l'appareil a accès root
        /// </summary>
        public async Task<bool> CheckRootAccessAsync()
        {
            try
            {
                var result = await ExecuteShellCommandAsync("su -c 'id'");
                return result.Contains("uid=0");
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Lit les mots de passe WiFi depuis le fichier système (NÉCESSITE ROOT)
        /// </summary>
        public async Task<List<SavedWifiCredential>> ReadRealPasswordsAsync()
        {
            var credentials = new List<SavedWifiCredential>();

            try
            {
                // Vérifier l'accès root
                if (!await CheckRootAccessAsync())
                {
                    Debug.WriteLine("[RootWifiPasswordReader] Pas d'accès root - impossible de lire les mots de passe");
                    return credentials;
                }

                // Essayer différents chemins selon la version Android
                string content = null;

                // Essayer le chemin principal
                content = await ReadFileWithRootAsync(WPA_SUPPLICANT_PATH);
                if (!string.IsNullOrEmpty(content))
                {
                    credentials.AddRange(ParseWpaSupplicant(content));
                }

                // Essayer le chemin alternatif (Android 10+)
                if (credentials.Count == 0)
                {
                    content = await ReadFileWithRootAsync(WPA_SUPPLICANT_PATH_ALT2);
                    if (!string.IsNullOrEmpty(content))
                    {
                        credentials.AddRange(ParseWifiConfigStore(content));
                    }
                }

                Debug.WriteLine($"[RootWifiPasswordReader] {credentials.Count} réseaux trouvés");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[RootWifiPasswordReader] Erreur: {ex.Message}");
            }

            return credentials;
        }

        /// <summary>
        /// Lit un fichier avec accès root
        /// </summary>
        private async Task<string> ReadFileWithRootAsync(string filePath)
        {
            try
            {
                var command = $"su -c 'cat {filePath}'";
                return await ExecuteShellCommandAsync(command);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[RootWifiPasswordReader] Erreur lecture {filePath}: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Parse le fichier wpa_supplicant.conf (Android 9 et inférieur)
        /// Format:
        /// network={
        ///     ssid="MonWiFi"
        ///     psk="motdepasse123"
        /// }
        /// </summary>
        private List<SavedWifiCredential> ParseWpaSupplicant(string content)
        {
            var credentials = new List<SavedWifiCredential>();

            try
            {
                // Regex pour extraire les blocs network
                var networkBlocks = Regex.Matches(content, @"network=\{([^}]+)\}", RegexOptions.Singleline);

                foreach (Match block in networkBlocks)
                {
                    var networkContent = block.Groups[1].Value;

                    // Extraire SSID
                    var ssidMatch = Regex.Match(networkContent, @"ssid=""([^""]+)""");
                    if (!ssidMatch.Success) continue;
                    var ssid = ssidMatch.Groups[1].Value;

                    // Extraire mot de passe (psk)
                    var pskMatch = Regex.Match(networkContent, @"psk=""([^""]+)""");
                    string password = null;

                    if (pskMatch.Success)
                    {
                        password = pskMatch.Groups[1].Value;
                    }
                    else
                    {
                        // Si pas de psk entre guillemets, chercher hash
                        var pskHashMatch = Regex.Match(networkContent, @"psk=([a-f0-9]{64})");
                        if (pskHashMatch.Success)
                        {
                            password = $"[Hash: {pskHashMatch.Groups[1].Value.Substring(0, 16)}...]";
                        }
                    }

                    // Extraire type de sécurité
                    string securityType = "WPA2-PSK";
                    if (networkContent.Contains("key_mgmt=NONE"))
                        securityType = "Open";
                    else if (networkContent.Contains("key_mgmt=WPA-EAP"))
                        securityType = "WPA2-Enterprise";

                    credentials.Add(new SavedWifiCredential
                    {
                        Ssid = ssid,
                        Password = password ?? "[Aucun mot de passe]",
                        SecurityType = securityType,
                        SavedDate = DateTime.Now,
                        NetworkId = credentials.Count.ToString(),
                        IsCurrentNetwork = false
                    });
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[RootWifiPasswordReader] Erreur parse wpa_supplicant: {ex.Message}");
            }

            return credentials;
        }

        /// <summary>
        /// Parse le fichier WifiConfigStore.xml (Android 10+)
        /// Format XML avec ConfigKey et PreSharedKey
        /// </summary>
        private List<SavedWifiCredential> ParseWifiConfigStore(string content)
        {
            var credentials = new List<SavedWifiCredential>();

            try
            {
                // Regex pour extraire SSID et mot de passe du XML
                var ssidMatches = Regex.Matches(content, @"<string name=""SSID"">""([^""]+)""</string>");
                var pskMatches = Regex.Matches(content, @"<string name=""PreSharedKey"">""([^""]+)""</string>");

                // Android 10+ utilise un format différent
                if (ssidMatches.Count == 0)
                {
                    // Essayer le format alternatif
                    ssidMatches = Regex.Matches(content, @"ConfigKey=""([^""]+)""");
                }

                for (int i = 0; i < Math.Min(ssidMatches.Count, pskMatches.Count); i++)
                {
                    var ssid = ssidMatches[i].Groups[1].Value;
                    var password = pskMatches[i].Groups[1].Value;

                    credentials.Add(new SavedWifiCredential
                    {
                        Ssid = ssid,
                        Password = password,
                        SecurityType = "WPA2-PSK",
                        SavedDate = DateTime.Now,
                        NetworkId = i.ToString(),
                        IsCurrentNetwork = false
                    });
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[RootWifiPasswordReader] Erreur parse WifiConfigStore: {ex.Message}");
            }

            return credentials;
        }

        /// <summary>
        /// Exécute une commande shell
        /// </summary>
        private async Task<string> ExecuteShellCommandAsync(string command)
        {
            try
            {
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "/system/bin/sh",
                        Arguments = $"-c \"{command}\"",
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };

                process.Start();

                var output = await process.StandardOutput.ReadToEndAsync();
                var error = await process.StandardError.ReadToEndAsync();

                await process.WaitForExitAsync();

                if (!string.IsNullOrEmpty(error))
                {
                    Debug.WriteLine($"[RootWifiPasswordReader] Erreur commande: {error}");
                }

                return output;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[RootWifiPasswordReader] Erreur exécution: {ex.Message}");
                throw;
            }
        }
    }
}
