using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace BlocklyAts.Host {
    public class UpgradeInfo {

        public string Project;
        public Version LatestVersion;
        public string Changelog;
        public string WebUrl;

        public static async Task<UpgradeInfo> FetchOnline(string url, string name) {
            try {
                string remoteDocument;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.AutomaticDecompression = DecompressionMethods.GZip;
                HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();
                Stream stream = response.GetResponseStream();
                remoteDocument = await new StreamReader(stream).ReadToEndAsync();
                
                var xdoc = XDocument.Parse(remoteDocument);
                foreach (var project in xdoc.Root.Elements("Project")) {
                    if (project.Attribute("Name").Value == name) {
                        return new UpgradeInfo() {
                            Project = name,
                            LatestVersion = new Version(project.Element("LatestVersion").Value),
                            Changelog = project.Element("Changelog").Value.Replace("\\n", Environment.NewLine),
                            WebUrl = project.Element("WebUrl").Value
                        };
                    }
                }

                return null;
            } catch {
                // Failing to get upgrade info is nothing serious.
                return null;
            }
        }

        public void ShowPromptAsRequired() {
            if (LatestVersion > Assembly.GetExecutingAssembly().GetName().Version) {
                if (MessageBox.Show(
                    string.Format(I18n.Translate("Msg.UpgradeAvailable"), LatestVersion.ToString(), WebUrl, Changelog),
                    "Upgrade Available", MessageBoxButtons.OKCancel, MessageBoxIcon.Information
                ) == DialogResult.OK) {
                    PlatformFunction.CallBrowser(WebUrl);
                }
            }
        }
    }
}
