using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace BlocklyAts.Host {

    [XmlRoot(ElementName = "Preference")]
    public class Preference {

        public string EditorVersion { get; set; }

        public string Language { get; set; }

        public bool PreferExternalBrowser { get; set; }

        public RecentFileList RecentFiles { get; set; } = new RecentFileList();

        public bool DarkMode { get; set; }

        public void SaveToFile(string path = null) {
            EditorVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            using (FileStream outStream = new FileStream(path, FileMode.Create, FileAccess.Write))
            using (StreamWriter writer = new StreamWriter(outStream, Encoding.UTF8)) {
                using (XmlWriter xmlWriter = XmlWriter.Create(writer, new XmlWriterSettings { Indent = false }))
                    new XmlSerializer(typeof(Preference)).Serialize(xmlWriter, this);
            }
        }

        public static Preference LoadFromFile(string path) {
            using (FileStream inStream = new FileStream(path, FileMode.Open, FileAccess.Read))
            using (StreamReader reader = new StreamReader(inStream, Encoding.UTF8)) {
                var pref = (Preference)new XmlSerializer(typeof(Preference)).Deserialize(reader);
                if (pref.RecentFiles == null) pref.RecentFiles = new RecentFileList();
                return pref;
            }
        }
    }
}
