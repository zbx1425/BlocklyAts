using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace BlocklyAts {

    // For backward-compatibility of my stupid temporary idea
    [XmlRoot(ElementName = "blocklyats")]
    public class Workspace {

        [XmlIgnore()]
        public string SaveFilePath { get; set; }

        public string EditorVersion { get; set; }

        [XmlElement("config")]
        public CompilerConfig Config { get; set; }

        [XmlElement("blocklyxml")]
        public FPXElement BlocklyXml { get; set; }

        public Workspace() {
            Config = new CompilerConfig();
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
                new XmlSerializer(typeof(Workspace)).Serialize(xmlWriter, this);
            }
        }

        public static Workspace LoadFromFile(string path) {
            using (FileStream inStream = new FileStream(path, FileMode.Open, FileAccess.Read))
            using (StreamReader reader = new StreamReader(inStream, Encoding.UTF8)) {
                var wksp = (Workspace)new XmlSerializer(typeof(Workspace)).Deserialize(reader);
                wksp.SaveFilePath = path;
                return wksp;
            }
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
