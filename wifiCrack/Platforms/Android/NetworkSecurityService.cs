using System;
using System.Collections.Generic;
using System.Linq;
using Java.Lang;
using System.Text;
using System.Threading.Tasks;
using wifiCrack.utils;
using Android.Content;
using System.Diagnostics;
using Microsoft.Maui.Controls;
using Process = System.Diagnostics.Process;
using wifiCrack.Platforms.Android;
using Exception = System.Exception;

[assembly: Dependency(typeof(NetworkSecurityService))]

namespace wifiCrack.Platforms.Android
{
    public class NetworkSecurityService : INetworkSecurityService
    {

        public NetworkSecurityService() { }

        public async Task<bool> IsWpsEnabled(string bssid)
        {
            try
            {
                var process = new Process();
                process.StartInfo.FileName = "/system/bin/sh";
                process.StartInfo.Arguments = $"-c \"wash -i wlan0mon | grep {bssid}\"";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;

                process.Start();

                string output = await process.StandardOutput.ReadToEndAsync();
                process.WaitForExit();
                Console.WriteLine($"output: {output}");

                return output.ToLower().Contains("wps");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[WPS Detection Error] {ex.Message}");
                return false;
            }
        }
    }
}
