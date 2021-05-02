using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BlocklyAts.Host {
    static class I18n {

        public static Dictionary<string, Dictionary<string, string>> languages = new Dictionary<string, Dictionary<string, string>>();

        public static List<KeyValuePair<string, string>> LanguageDisplayList = new List<KeyValuePair<string, string>>();

        static I18n() {
            var langFileDir = Path.Combine(PlatformFunction.AppDir, "resource", "lang");
            var langFiles = Directory.GetFiles(langFileDir, "lang_*.txt");

            foreach (var file in langFiles) {
                var langContent = File.ReadAllText(file, Encoding.UTF8);
                languages.Add(
                    Path.GetFileNameWithoutExtension(file).Replace("lang_", ""),
                    langContent.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries)
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

            if (string.IsNullOrEmpty(PreferenceManager.Current.Language))
                PreferenceManager.Current.Language = "en";
            if (!languages.ContainsKey(PreferenceManager.Current.Language))
                PreferenceManager.Current.Language = "en";
        }

        public static bool CanTranslate(string key) {
            return languages[PreferenceManager.Current.Language].ContainsKey(key);
        }

        public static string Translate(string key, object param = null) {
            if (param == null) {
                return languages[PreferenceManager.Current.Language][key];
            } else {
                return string.Format(languages[PreferenceManager.Current.Language][key], param);
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
