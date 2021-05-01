using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlocklyAts.Host {

    static class PreferenceManager {

        public static Preference CurrentPreference;

        public static string DataDirectory = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "zbx1425", "BlocklyAts"
        );
        public static string PreferencePath = Path.Combine(DataDirectory, "Preference.xml");
        public static string WebView2UserDataPath = Path.Combine(DataDirectory, "WebView2UserData");

        public static bool FirstStartup = true;

        public static void ResetPreference() {
            CurrentPreference = new Preference() {
                Language = "en",
                PreferExternalBrowser = false
            };
        }

        public static bool LoadPreference() {
            try {
                if (!Directory.Exists(DataDirectory)) Directory.CreateDirectory(DataDirectory);
            } catch {
                return false;
            }
            if (File.Exists(PreferencePath)) {
                try {
                    CurrentPreference = Preference.LoadFromFile(PreferencePath);
                    FirstStartup = false;
                } catch {
                    ResetPreference();
                    return false;
                }
                return true;
            } else {
                ResetPreference();
                FirstStartup = true;
                return false;
            }
        }

        public static bool SavePreference() {
            try {
                if (!Directory.Exists(DataDirectory)) Directory.CreateDirectory(DataDirectory);
                CurrentPreference.SaveToFile(PreferencePath);
            } catch {
                return false;
            }
            return true;
        }
    }
}
