using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlocklyAts {

    static class PreferenceManager {

        public static Preference CurrentPreference;

        public static void ResetPreference() {
            CurrentPreference = new Preference() {
                Language = "en",
                PreferExternalBrowser = false
            };
        }

        public static bool LoadPreference(string path = "Preference.xml") {
            if (File.Exists(path)) {
                try {
                    CurrentPreference = Preference.LoadFromFile(path);
                } catch {
                    ResetPreference();
                    return false;
                }
                return true;
            } else {
                ResetPreference();
                return false;
            }
        }

        public static bool SavePreference(string path = "Preference.xml") {
            try {
                CurrentPreference.SaveToFile(path);
            } catch {
                return false;
            }
            return true;
        }
    }
}
