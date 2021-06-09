using BlocklyAts.Host;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace BlocklyAts.Workspace {

    // For backward-compatibility of my stupid temporary idea
    [XmlRoot(ElementName = "blocklyats")]
    public class SaveState {

        [XmlIgnore()]
        public string SaveFilePath { get; set; }

        [XmlIgnore()]
        public bool IsDirty { get; set; } = false;

        public string EditorVersion { get; set; }

        [XmlElement("config")]
        public BuildRunConfig Config { get; set; }

        [XmlElement("blocklyxml")]
        public FPXElement BlocklyXml { get; set; }

        public SaveState() {
            Config = new BuildRunConfig();
            Config.ShouldCompileAnyCpu = true;
            Config.ShouldCompilex64 = true;
            Config.ShouldCompilex86 = true;
        }

        public void SaveToFile(string path = null) {
            EditorVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            if (string.IsNullOrEmpty(path)) {
                path = this.SaveFilePath;
            } else {
                this.SaveFilePath = path;
            }
            using (FileStream outStream = new FileStream(path, FileMode.Create, FileAccess.Write))
            using (StreamWriter writer = new StreamWriter(outStream, Encoding.UTF8)) {
            using (XmlWriter xmlWriter = XmlWriter.Create(writer, new XmlWriterSettings { Indent = false }))
                new XmlSerializer(typeof(SaveState)).Serialize(xmlWriter, this);
            }
            IsDirty = false;
        }

        public static SaveState LoadFromFile(string path) {
            using (FileStream inStream = new FileStream(path, FileMode.Open, FileAccess.Read))
            using (StreamReader reader = new StreamReader(inStream, Encoding.UTF8)) {
                var wksp = (SaveState)new XmlSerializer(typeof(SaveState)).Deserialize(reader);
                wksp.SaveFilePath = path;
                if (new Version(wksp.EditorVersion) > Assembly.GetExecutingAssembly().GetName().Version) {
                    // Maybe this UI code should be moved somewhere else?
                    if (MessageBox.Show(I18n.Translate("Msg.LoadHigherVersion"), "Low Version", 
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) {
                        return null;
                    }
                }
                if (new Version(wksp.EditorVersion) <= new Version(1, 0, 6, 0)) {
                    // Upgrade ID from field to block
                    string[] upgradeList = {
                        "bve_sound_stop", "bve_sound_play_once", "bve_sound_play_loop",
                        "bve_get_sound_internal", "bve_set_sound_internal",
                        "bve_set_panel", "bve_get_panel"
                    };
                    var targetBlocks = wksp.BlocklyXml.Descendants()
                        .Where(e => e.Name.LocalName == "block")
                        .Where(e => upgradeList.Contains(e.Attribute("type").Value))
                        .ToList();
                    foreach (var block in targetBlocks) {
                        var valueBlock = block.Descendants()
                            .Where(e => e.Name.LocalName == "field")
                            .FirstOrDefault(e => e.Attribute("name").Value == "ID");
                        if (valueBlock == null) continue;
                        block.AddFirst(
                            new XElement("value", new XAttribute("name", "ID"),
                                new XElement("block", new XAttribute("type", "math_number"), 
                                    new XAttribute("id", GenerateBlocklyUid()),
                                    new XElement("field", new XAttribute("name", "NUM"),
                                        valueBlock.Value
                                    )
                                )
                            )
                        );
                        valueBlock.Remove();
                    }
                }
                return wksp;
            }
        }

        private static string GenerateBlocklyUid() {
            string soup = "!#$%()*+,-./:;=?@[]^_`{|}~ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var length = 20;
            var random = new Random();
            var sb = new StringBuilder();
            for (var i = 0; i < length; i++) sb.Append(soup[random.Next(0, soup.Length)]);
            return sb.ToString();
        }

        private string SelectDefaultCompilePath(string compilePath, string suffix) {
            if (string.IsNullOrEmpty(compilePath)) {
                return Path.Combine(
                    Path.GetDirectoryName(SaveFilePath),
                    Path.GetFileNameWithoutExtension(SaveFilePath) + suffix
                );
            } else {
                return compilePath;
            }
        }

        public string GetCompilePathAnyCpu() {
            return SelectDefaultCompilePath(Config.CompilePathAnyCpu, "_net.dll");
        }

        public string GetCompilePathx86() {
            return SelectDefaultCompilePath(Config.CompilePathx86, "_x86.dll");
        }

        public string GetCompilePathx64() {
            return SelectDefaultCompilePath(Config.CompilePathx64, "_x64.dll");
        }
    }

    public class FPXElement : XElement, IXmlSerializable {
        // When the built-in XNode is deserialized, you only get the first child
        // Is there a better way to do this?
        public FPXElement() : base("default") { }
        public FPXElement(XName name) : base(name) { }
        public FPXElement(XElement other) : base(other) { }
        public FPXElement(XStreamingElement other) : base(other) { }
        public FPXElement(XName name, object content) : base(name, content) { }
        public FPXElement(XName name, params object[] content) : base(name, content) { }

        public void ReadXml(XmlReader reader) {
            typeof(XElement).GetMethod("ReadElementFrom", BindingFlags.NonPublic | BindingFlags.Instance)
                .Invoke(this, new object[] { reader, LoadOptions.None });
        }
        public void WriteXml(XmlWriter writer) {
            foreach (var child in this.Elements()) {
                child.WriteTo(writer);
            }
        }
    }
}
