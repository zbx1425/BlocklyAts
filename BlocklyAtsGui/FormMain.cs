using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using System.Xml.Linq;

namespace BlocklyATS {
    public partial class FormMain : Form {

        private HttpServer server = new HttpServer();

        public FormMain() {
            InitializeComponent();
        }

        private IBrowser mainWebBrowser;

        private bool CEFAvailable;
        private bool CEFInUse;
        private IBrowser builtinInstance, cefInstance;

        private void FormMain_Load(object sender, EventArgs e) {
            CEFAvailable = File.Exists(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), 
                "x86", "CefSharp.dll"));
            CEFInUse = CEFAvailable;
            createWebBrowser(CEFInUse);
            tsbtnToggleCEF.Visible = CEFAvailable;
            /*mainWebBrowser.DocumentText = @"<script>
                var ie = (function (){
                    if (window.ActiveXObject === undefined) return 'Gecko/Webkit'; //Not IE
                    if (!window.XMLHttpRequest) return 'IE6';
                    if (!document.querySelector) return 'IE7';
                    if (!document.addEventListener) return 'IE8';
                    if (!window.atob) return 'IE9';
                    if (!document.__proto__) return 'IE10';
                    return 'IE11';
                })();
            </script>
            <ul>
                <li>WebBrowser implemention is <b><script>document.write(ie);</script></b></li>";*/

        }

        private static string encodeJsString(string s) {
            StringBuilder sb = new StringBuilder();
            //sb.Append("\"");
            foreach (char c in s) {
                switch (c) {
                    case '\"':
                        sb.Append("\\\"");
                        break;
                    case '\\':
                        sb.Append("\\\\");
                        break;
                    case '\b':
                        sb.Append("\\b");
                        break;
                    case '\f':
                        sb.Append("\\f");
                        break;
                    case '\n':
                        sb.Append("\\n");
                        break;
                    case '\r':
                        sb.Append("\\r");
                        break;
                    case '\t':
                        sb.Append("\\t");
                        break;
                    default:
                        int i = (int)c;
                        if (i < 32 || i > 127) {
                            sb.AppendFormat("\\u{0:X04}", i);
                        } else {
                            sb.Append(c);
                        }
                        break;
                }
            }
            //sb.Append("\"");
            return sb.ToString();
        }

        const string JsClearWorkspace = "workspace.clear();";
        const string JsGetXml = "Blockly.Xml.domToText(Blockly.Xml.workspaceToDom(workspace));";
        const string JsSetXml = "workspace.clear();Blockly.Xml.domToWorkspace(Blockly.Xml.textToDom(\"{0}\"),workspace);";
        const string JsAfterBatsInit = "window.afterBatsInit = function(){{{0}}};";

        void createWebBrowser(bool useCEF = false) {
            string previousCode = "";

            if (mainWebBrowser != null && Controls.Contains(mainWebBrowser.GetControl())) {
                previousCode = mainWebBrowser.InvokeScript(JsGetXml).ToString();
                Controls.Remove(mainWebBrowser.GetControl());
            }
            string applicationDirectory = Path.GetDirectoryName(Application.ExecutablePath);
#if DEBUG
            string webDirectory = Path.Combine(Path.GetDirectoryName(applicationDirectory), "www");
            if (!Directory.Exists(webDirectory)) webDirectory = Path.Combine(applicationDirectory, "www");
#else
            string webDirectory = Path.Combine(applicationDirectory, "www");
#endif
            string myFile = Path.Combine(webDirectory, "index.html");

            bool newlyInitialized = false;
            if (CEFAvailable && useCEF) {
                if (cefInstance == null) {
                    cefInstance = new CefBrowser(myFile);
                    newlyInitialized = true;
                }
                mainWebBrowser = cefInstance;
            } else {
                if (builtinInstance == null) {
                    builtinInstance = new BuiltinBrowser(myFile);
                    newlyInitialized = true;
                }
                mainWebBrowser = builtinInstance;
            }
            mainWebBrowser.KeyDown += new PreviewKeyDownEventHandler(mainWebBrowser_PreviewKeyDown);
            mainWebBrowser.BindTo(this);

            if (previousCode != "") {
                EventHandler resumeCodeDelegate = null;
                if (newlyInitialized) {
                    resumeCodeDelegate = new EventHandler((sender, e) => {
                        mainWebBrowser.InvokeScript(
                            string.Format(JsAfterBatsInit, string.Format(JsSetXml, encodeJsString(previousCode)))
                        );
                        mainWebBrowser.PageFinished -= resumeCodeDelegate;
                    });
                    mainWebBrowser.PageFinished += resumeCodeDelegate;
                } else {
                    mainWebBrowser.InvokeScript(string.Format(JsSetXml, encodeJsString(previousCode)));
                }
            }
        }

        private void mainWebBrowser_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e) {
            if (e.KeyCode == Keys.F5) {
                mainWebBrowser.Reload();
            } else if (e.KeyCode == Keys.F12) {
                (mainWebBrowser as CefBrowser)?.ShowDevTools();
            }
        }

        private void tsbtnToggleCEF_Click(object sender, EventArgs e) {
            CEFInUse = !CEFInUse;
            createWebBrowser(CEFInUse);
            tsbtnToggleCEF.Text = CEFInUse ? "[CEF] / IE" : "CEF / [IE]";
        }

        private void tsbtnNew_Click(object sender, EventArgs e) {
            if (MessageBox.Show("All unsaved change will be discarded. Confirm?", "Clear workspace", MessageBoxButtons.YesNo)
                == DialogResult.Yes) {
                mainWebBrowser.InvokeScript(JsClearWorkspace);
            }
        }

        private void tsbtnSave_Click(object sender, EventArgs e) {
            var sfd = new SaveFileDialog() {
                Filter = "BlocklyAts XML|*.batsxml",
                Title = "Save Workspace"
            };
            if (sfd.ShowDialog() != DialogResult.OK) return;

            var blockXml = mainWebBrowser.InvokeScript(JsGetXml).ToString();
            XDocument blockDoc = XDocument.Parse(blockXml);
            blockDoc.Root.Name = "blocklyxml";
            blockDoc.Root.RemoveAttributes();
            XDocument fileDoc = new XDocument(
                new XElement("blocklyats",
                    new XElement("meta"),
                    blockDoc.Root
                )
            );
            string content = fileDoc.ToString(SaveOptions.DisableFormatting);

            File.WriteAllText(sfd.FileName, content, Encoding.UTF8);
        }

        private void tsbtnOpen_Click(object sender, EventArgs e) {
            var ofd = new OpenFileDialog() {
                Filter = "BlocklyAts XML|*.batsxml",
                Title = "Restore Workspace"
            };
            if (ofd.ShowDialog() != DialogResult.OK) return;

            var fileXml = File.ReadAllText(ofd.FileName, Encoding.UTF8);

            try {
                XDocument fileDoc = XDocument.Parse(fileXml);
                var blockDoc = fileDoc.Element("blocklyats").Element("blocklyxml");
                var content = blockDoc.ToString(SaveOptions.DisableFormatting);
                mainWebBrowser.InvokeScript(string.Format(JsSetXml, encodeJsString(content)));
            } catch {
                MessageBox.Show("This workspace savestate is malformed.");
            }
        }
    }
}
