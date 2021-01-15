using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using System.Xml.Linq;

namespace BlocklyATS {
    public partial class FormMain : Form {

        public FormMain() {
            InitializeComponent();
        }

        private BaseBrowser mainWebBrowser;

        private string _currentFile;
        private string CurrentFile {
            get {
                return _currentFile;
            }
            set {
                if (string.IsNullOrEmpty(value)) {
                    _currentFile = "";
                    this.Text = "BlocklyATS: [Not yet saved]";
                    tsbtnSave.Enabled = false;
                    tsbtnCompile.Enabled = false;
                    tsbtnCompileRun.Enabled = false;
                } else {
                    this.Text = "BlocklyATS: " + value;
                    tsbtnSave.Enabled = true;
                    tsbtnCompile.Enabled = true;
                    tsbtnCompileRun.Enabled = true;
                }
                _currentFile = value;
            }
        }

        private void FormMain_Load(object sender, EventArgs e) {
#if DEBUG
            string webDirectory = Path.Combine(Path.GetDirectoryName(CompilerFunction.appDir), "www");
            if (!Directory.Exists(webDirectory)) webDirectory = Path.Combine(CompilerFunction.appDir, "www");
#else
            string webDirectory = Path.Combine(CompilerFunction.appDir, "www");
#endif
            string pageURL = Path.Combine(webDirectory, "index.html") + string.Format("?ver={0}",
                Assembly.GetExecutingAssembly().GetName().Version.ToString());
            mainWebBrowser = BaseBrowser.AcquireInstance(pageURL);
            mainWebBrowser.KeyDown += new PreviewKeyDownEventHandler(mainWebBrowser_PreviewKeyDown);
            mainWebBrowser.BindTo(this);
            CurrentFile = "";
        }

        private void mainWebBrowser_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e) {
            if (e.KeyCode == Keys.F5) {
                mainWebBrowser.Reload();
            } else if (e.KeyCode == Keys.F12) {
                (mainWebBrowser as CefBrowser)?.ShowDevTools();
            }
        }

        private void tsbtnNew_Click(object sender, EventArgs e) {
            if (MessageBox.Show("All unsaved change will be discarded. Confirm?", "Clear workspace", MessageBoxButtons.YesNo)
                == DialogResult.Yes) {
                mainWebBrowser.BkyResetWorkspace();
            }
        }

        private void tsbtnSaveAs_Click(object sender, EventArgs e) {
            var sfd = new SaveFileDialog() {
                Filter = "BlocklyAts XML|*.batsxml",
                Title = "Save Workspace"
            };
            if (sfd.ShowDialog() != DialogResult.OK) return;

            var blockXml = mainWebBrowser.BkySaveWorkspace();
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
            CurrentFile = sfd.FileName;
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
                mainWebBrowser.BkyLoadWorkspace(content);
                CurrentFile = ofd.FileName;
            } catch (Exception ex) {
                MessageBox.Show("This workspace savestate is malformed:\n" + ex.Message);
            }
        }

        private async void tsbtnCompile_Click(object sender, EventArgs e) {
            if (string.IsNullOrEmpty(CurrentFile)) return;
            if (ModifierKeys.HasFlag(Keys.Control) && ModifierKeys.HasFlag(Keys.Shift)) {
                Clipboard.SetText(mainWebBrowser.BkyExportLua());
                return;
            }
            tsbtnCompile.Enabled = false;
            string targetDllName = Path.Combine(
                Path.GetDirectoryName(CurrentFile),
                Path.GetFileNameWithoutExtension(CurrentFile) + "_x86.dll"
            );
            try {
                await CompilerFunction.CompileLua(mainWebBrowser.BkyExportLua(), targetDllName, "x86");
                MessageBox.Show("Compilation finished.\nSaved to: " + targetDllName);
            } catch (Exception ex) {
                MessageBox.Show("Compilation failed:\n" + ex.Message);
            }
            tsbtnCompile.Enabled = true;
        }

        private void tsbtnSave_Click(object sender, EventArgs e) {
            if (string.IsNullOrEmpty(CurrentFile)) return;
            var blockXml = mainWebBrowser.BkySaveWorkspace();
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

            File.WriteAllText(CurrentFile, content, Encoding.UTF8);
        }
    }
}
