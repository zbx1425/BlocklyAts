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

            if (string.IsNullOrEmpty(PreferenceManager.CurrentPreference.Language))
                PreferenceManager.CurrentPreference.Language = "en";
            if (!languages.ContainsKey(PreferenceManager.CurrentPreference.Language))
                PreferenceManager.CurrentPreference.Language = "en";
        }

        public static bool CanTranslate(string key) {
            return languages[PreferenceManager.CurrentPreference.Language].ContainsKey(key);
        }

        public static string Translate(string key, object param = null) {
            if (param == null) {
                return languages[PreferenceManager.CurrentPreference.Language][key];
            } else {
                return string.Format(languages[PreferenceManager.CurrentPreference.Language][key], param);
            }
        }

        public static string TranslateAllLang(string key, object param = null) {
            var sb = new StringBuilder();
            foreach (var lang in LanguageDisplayList) {
                if (param == null) {
                    sb.AppendLine(languages[lang.Key][key]);
                    sb.AppendLine();
                } else {
                    sb.AppendLine(string.Format(languages[lang.Key][key], param));
                    sb.AppendLine();
                }
            }
            return sb.ToString().Trim();
        }
    }
}
