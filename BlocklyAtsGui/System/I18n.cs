using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BlocklyAts {
    static class I18n {

        public static Dictionary<string, Dictionary<string, string>> languages = new Dictionary<string, Dictionary<string, string>>();

        public static List<KeyValuePair<string, string>> LanguageDisplayList = new List<KeyValuePair<string, string>>();

        static I18n() {
            foreach (DictionaryEntry entry in Properties.Resources.ResourceManager.GetResourceSet(CultureInfo.CurrentUICulture, true, true)) {
                if (!entry.Key.ToString().StartsWith("lang_", StringComparison.Ordinal)) continue;
                languages.Add(
                    entry.Key.ToString().Replace("lang_", ""),
                    entry.Value.ToString().Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries)
                        .Where(s => !string.IsNullOrEmpty(s.Trim()))
                        .ToDictionary(l => l.Split(new[] { '=' }, 2)[0].Trim(), 
                            l => l.Split(new[] { '=' }, 2)[1].Trim().Replace("\\n", Environment.NewLine)));
            }

            // Ensure English is the first
            LanguageDisplayList.Add(new KeyValuePair<string, string>("en", languages["en"]["Name"]));
            foreach (var pair in languages) {
                if (pair.Key == "en") continue;
                LanguageDisplayList.Add(new KeyValuePair<string, string>(pair.Key, pair.Value["Name"]));
            }

            if (File.Exists("Preference.xml")) {
                try {
                    XDocument xdoc = XDocument.Load("Preference.xml");
                    SelectedLanguage = xdoc.Root.Element("Language")?.Value;
                } catch {
                    // Unable to load configuration is not a big deal.
                }
                if (string.IsNullOrEmpty(SelectedLanguage)) SelectedLanguage = "en";
            }
            if (!languages.ContainsKey(SelectedLanguage)) SelectedLanguage = "en";
        }

        public static string SelectedLanguage = "en";

        public static void SavePreference() {
            // TODO: A class for preference
            // Currently manually building XDocument, since only one preference is selected
            XDocument xdoc = new XDocument(
                new XElement("Preference",
                    new XElement("Language", SelectedLanguage)
                )
            );
            try {
                xdoc.Save("Preference.xml");
            } catch {
                // Unable to save configuration is not a big deal.
            }
        }

        public static bool CanTranslate(string key) {
            return languages[SelectedLanguage].ContainsKey(key);
        }

        public static string Translate(string key, object param = null) {
            if (param == null) {
                return languages[SelectedLanguage][key];
            } else {
                return string.Format(languages[SelectedLanguage][key], param);
            }
        }
    }
}
