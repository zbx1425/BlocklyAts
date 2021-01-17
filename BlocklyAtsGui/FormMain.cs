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

        private Workspace currentWorkspace = new Workspace();

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
            updateSaveFileState();
        }

        private async void mainWebBrowser_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e) {
            if (e.KeyCode == Keys.F5) {
                if (ModifierKeys.HasFlag(Keys.Control) && ModifierKeys.HasFlag(Keys.Shift)) {
                    // Debug function.
                    currentWorkspace.BlocklyXml = new FPXElement(mainWebBrowser.BkySaveWorkspace());
                    mainWebBrowser.Reload();
                    await Task.Delay(2000);
                    mainWebBrowser.BkyLoadWorkspace(currentWorkspace.BlocklyXml);
                } else {
                    tsbtnCompileRun_Click(null, null);
                }
            } else if (e.KeyCode == Keys.F12) {
#if !MONO
                (mainWebBrowser as CefBrowser)?.ShowDevTools();
#endif
            } else if (e.KeyCode == Keys.S) {
                if (ModifierKeys.HasFlag(Keys.Control)) {
                    if (ModifierKeys.HasFlag(Keys.Shift)) {
                        tsbtnSaveAs_Click(null, null);
                    } else {
                        tsbtnSave_Click(null, null);
                    }
                }
            } else if (e.KeyCode == Keys.O) {
                if (ModifierKeys.HasFlag(Keys.Control)) {
                    tsbtnOpen_Click(null, null);
                }
            } else if (e.KeyCode == Keys.N) {
                if (ModifierKeys.HasFlag(Keys.Control)) {
                    tsbtnNew_Click(null, null);
                }
            } else if (e.KeyCode == Keys.B) {
                if (ModifierKeys.HasFlag(Keys.Control) && ModifierKeys.HasFlag(Keys.Shift)) {
                    tsbtnCompile_Click(null, null);
                }
            }
        }

        private void updateSaveFileState() {
            if (string.IsNullOrEmpty(currentWorkspace.SaveFilePath)) {
                this.Text = "BlocklyATS: [Not yet saved]";
                tsbtnSave.Enabled = false;
                tsbtnCompile.Enabled = false;
                tsbtnCompileRun.Enabled = false;
            } else {
                this.Text = "BlocklyATS: " + currentWorkspace.SaveFilePath;
                tsbtnSave.Enabled = true;
                tsbtnCompile.Enabled = true;
                tsbtnCompileRun.Enabled = true;
            }
        }

        private void tsbtnNew_Click(object sender, EventArgs e) {
            if (MessageBox.Show("All unsaved change will be discarded. Confirm?", "Clear workspace", MessageBoxButtons.YesNo)
                == DialogResult.Yes) {
                currentWorkspace = new Workspace();
                mainWebBrowser.BkyResetWorkspace();
                updateSaveFileState();
            }
        }

        private void tsbtnSaveAs_Click(object sender, EventArgs e) {
            var sfd = new SaveFileDialog() {
                Filter = "BlocklyAts XML|*.batsxml",
                Title = "Save Workspace"
            };
            if (sfd.ShowDialog() != DialogResult.OK) return;

            currentWorkspace.BlocklyXml = new FPXElement(mainWebBrowser.BkySaveWorkspace());
            currentWorkspace.SaveToFile(sfd.FileName);
            updateSaveFileState();
        }

        private void tsbtnOpen_Click(object sender, EventArgs e) {
            var ofd = new OpenFileDialog() {
                Filter = "BlocklyAts XML|*.batsxml",
                Title = "Restore Workspace"
            };
            if (ofd.ShowDialog() != DialogResult.OK) return;

            //try {
                currentWorkspace = Workspace.LoadFromFile(ofd.FileName);
                mainWebBrowser.BkyLoadWorkspace(currentWorkspace.BlocklyXml);
                updateSaveFileState();
            //} catch (Exception ex) {
            //    MessageBox.Show("This workspace savestate is malformed:\n" + ex.ToString());
            //}
        }

        private async Task<string> buildAllPlatforms() {
            var luaCode = mainWebBrowser.BkyExportLua();
            var outputList = new List<Tuple<string, string>>();
            if (currentWorkspace.Config.ShouldCompilex86) {
                outputList.Add(new Tuple<string, string>("x86", currentWorkspace.GetCompilePathx86()));
            }
            if (currentWorkspace.Config.ShouldCompilex64) {
                outputList.Add(new Tuple<string, string>("x64", currentWorkspace.GetCompilePathx64()));
            }
            if (currentWorkspace.Config.ShouldCompileAnyCpu) {
                outputList.Add(new Tuple<string, string>("net", currentWorkspace.GetCompilePathAnyCpu()));
            }
            var notifText = "";
            foreach (var pair in outputList) {
                notifText += "\n" + pair.Item1 + ": " + pair.Item2;
                if (pair.Item1 == "net") {
                    CompilerFunction.CompileCSharp(mainWebBrowser.BkyExportCSharp(), pair.Item2);
                } else {
                    await CompilerFunction.CompileLua(luaCode, pair.Item2, pair.Item1);
                }
            }
            return notifText;
        }

        private async void tsbtnCompile_Click(object sender, EventArgs e) {
            if (string.IsNullOrEmpty(currentWorkspace.SaveFilePath)) return;

            if (ModifierKeys.HasFlag(Keys.Control) && ModifierKeys.HasFlag(Keys.Shift)) {
                Clipboard.SetText(mainWebBrowser.BkyExportLua());
                return;
            }
            tsbtnCompile.Enabled = tsbtnCompileRun.Enabled = tsbtnCompileSetting.Enabled = false;
            try {
                var buildResult = await buildAllPlatforms();
                if (string.IsNullOrEmpty(buildResult)) {
                    MessageBox.Show("No build target was selected. Check your build configuraion!");
                } else {
                    MessageBox.Show("Compilation finished. Saved to: " + buildResult);
                }
            } catch (Exception ex) {
                MessageBox.Show("Compilation failed:\n" + ex.Message);
            }
            tsbtnCompile.Enabled = tsbtnCompileRun.Enabled = tsbtnCompileSetting.Enabled = true;
        }

        private void tsbtnSave_Click(object sender, EventArgs e) {
            if (string.IsNullOrEmpty(currentWorkspace.SaveFilePath)) return;
            currentWorkspace.BlocklyXml = new FPXElement(mainWebBrowser.BkySaveWorkspace());
            currentWorkspace.SaveToFile();
        }

        private async void tsbtnCompileRun_Click(object sender, EventArgs e) {
            if (string.IsNullOrEmpty(currentWorkspace.SaveFilePath)) return;
            tsbtnCompile.Enabled = tsbtnCompileRun.Enabled = tsbtnCompileSetting.Enabled = false;
            try {
                var buildResult = await buildAllPlatforms();
                if (string.IsNullOrEmpty(buildResult)) {
                    MessageBox.Show("No build target was selected. Check your build configuraion!");
                } else {
                    var gameProcess = currentWorkspace.Config.GetGameProcess();
                    gameProcess.Start();
                    await gameProcess.WaitForExitAsync();
                }
            } catch (Exception ex) {
                MessageBox.Show("Compilation failed:\n" + ex.Message);
            }
            tsbtnCompile.Enabled = tsbtnCompileRun.Enabled = tsbtnCompileSetting.Enabled = true;
        }

        private void tsbtnCompileSetting_Click(object sender, EventArgs e) {
            var ccd = new FormCompilerConfig();
            ccd.Config = currentWorkspace.Config;
            if (ccd.ShowDialog() == DialogResult.OK) {
                currentWorkspace.Config = ccd.Config;
            }
        }

        private void tsbtnDebugWindow_Click(object sender, EventArgs e) {
            var formDebug = new FormDebug();
            formDebug.codeLua = mainWebBrowser.BkyExportLua().Replace("\n", Environment.NewLine);
            formDebug.codeCSharp = mainWebBrowser.BkyExportCSharp().Replace("\n", Environment.NewLine);
            formDebug.ShowDialog();
        }

        private void tsbtnAbout_Click(object sender, EventArgs e) {
            new FormAbout().ShowDialog();
        }
    }
}
