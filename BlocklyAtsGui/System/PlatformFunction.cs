using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BlocklyAts {
    static class PlatformFunction {

        static readonly string appName = System.IO.Path.GetFileName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);

        static readonly string featureControlRegKey = @"Software\Microsoft\Internet Explorer\Main\FeatureControl\";

        public static bool IsMono { get; } = Type.GetType("Mono.Runtime") != null;

        public static void CallBrowser(string url) {
            try { if (Process.GetProcessById(Process.Start(url).Id) != null) return; } catch { }
            try { if (Process.GetProcessById(Process.Start("xdg-open " + url).Id) != null) return; } catch { }
            try { if (Process.GetProcessById(Process.Start("gnome-open " + url).Id) != null) return; } catch { }
            try { if (Process.GetProcessById(Process.Start("sensible-browser " + url).Id) != null) return; } catch { }
            MessageBox.Show(url, "Failed to call a browser", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static string VersionString {
            get {
                var version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                string versionString = version.ToString();
                if (version.Revision > 100) {
                    int rcNum = version.Revision - 100;
                    version = new Version(version.Major, version.Minor, version.Build + 1, 0);
                    versionString = version.ToString() + "-rc" + rcNum;
                }
                return versionString;
            }
        }

        public static void SetWebBrowserFeatures() {
            if (IsMono) return;
            // don't change the registry if running in-proc inside Visual Studio
            if (LicenseManager.UsageMode != LicenseUsageMode.Runtime) return;

            Registry.CurrentUser.CreateSubKey(featureControlRegKey + "FEATURE_BROWSER_EMULATION", true)
                .SetValue(appName, GetBrowserEmulationMode(), RegistryValueKind.DWord);

            // enable the features which are "On" for the full Internet Explorer browser
            Registry.CurrentUser.CreateSubKey(featureControlRegKey + "FEATURE_ENABLE_CLIPCHILDREN_OPTIMIZATION", true)
                .SetValue(appName, 1, RegistryValueKind.DWord);
            Registry.CurrentUser.CreateSubKey(featureControlRegKey + "FEATURE_AJAX_CONNECTIONEVENTS", true)
                .SetValue(appName, 1, RegistryValueKind.DWord);
            Registry.CurrentUser.CreateSubKey(featureControlRegKey + "FEATURE_GPU_RENDERING", true)
                .SetValue(appName, 1, RegistryValueKind.DWord);
            //Registry.CurrentUser.CreateSubKey(featureControlRegKey + "FEATURE_LOCALMACHINE_LOCKDOWN", true)
            //    .SetValue(appName, 0, RegistryValueKind.DWord);
            Registry.CurrentUser.CreateSubKey(featureControlRegKey + "FEATURE_NINPUT_LEGACYMODE", true)
                .SetValue(appName, 0, RegistryValueKind.DWord);
        }

        public static void UnsetWebBrowserFeatures() {
            if (IsMono) return;
            // don't change the registry if running in-proc inside Visual Studio
            if (LicenseManager.UsageMode != LicenseUsageMode.Runtime) return;

            Registry.CurrentUser.OpenSubKey(featureControlRegKey + "FEATURE_BROWSER_EMULATION", true)
                ?.DeleteValue(appName);

            // enable the features which are "On" for the full Internet Explorer browser
            Registry.CurrentUser.OpenSubKey(featureControlRegKey + "FEATURE_ENABLE_CLIPCHILDREN_OPTIMIZATION", true)
                ?.DeleteValue(appName);
            Registry.CurrentUser.OpenSubKey(featureControlRegKey + "FEATURE_AJAX_CONNECTIONEVENTS", true)
                ?.DeleteValue(appName);
            Registry.CurrentUser.OpenSubKey(featureControlRegKey + "FEATURE_GPU_RENDERING", true)
                ?.DeleteValue(appName);
            //Registry.CurrentUser.OpenSubKey(featureControlRegKey + "FEATURE_LOCALMACHINE_LOCKDOWN", true)
            //    ?.DeleteValue(appName);
            Registry.CurrentUser.OpenSubKey(featureControlRegKey + "FEATURE_NINPUT_LEGACYMODE", true)
                ?.DeleteValue(appName);
        }

        private static UInt32 GetBrowserEmulationMode() {
            if (IsMono) return 11000;
            int browserVersion = 0;
            using (var ieKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Internet Explorer",
                RegistryKeyPermissionCheck.ReadSubTree,
                System.Security.AccessControl.RegistryRights.QueryValues)) {
                var version = ieKey.GetValue("svcVersion");
                if (null == version) {
                    version = ieKey.GetValue("Version");
                    if (null == version)
                        return 7000;
                }
                int.TryParse(version.ToString().Split('.')[0], out browserVersion);
            }

            if (browserVersion < 7) {
                return 7000;
            }

            UInt32 mode = 11000; // Internet Explorer 11. Webpages containing standards-based !DOCTYPE directives are displayed in IE11 Standards mode. 

            switch (browserVersion) {
                case 7:
                    mode = 7000; // Webpages containing standards-based !DOCTYPE directives are displayed in IE7 Standards mode. 
                    break;
                case 8:
                    mode = 8000; // Webpages containing standards-based !DOCTYPE directives are displayed in IE8 mode. 
                    break;
                case 9:
                    mode = 9000; // Internet Explorer 9. Webpages containing standards-based !DOCTYPE directives are displayed in IE9 mode.                    
                    break;
                case 10:
                    mode = 10000; // Internet Explorer 10.
                    break;
            }

            return mode;
        }
    }
}
