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

namespace BlocklyAts {
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
            string pageURL = Path.Combine(webDirectory, "index.html") + string.Format("?ver={0}&lang={1}",
                Assembly.GetExecutingAssembly().GetName().Version.ToString(),
                I18n.Translate("BlocklyName")
            );
            mainWebBrowser = BaseBrowser.AcquireInstance(pageURL);
            mainWebBrowser.KeyDown += new PreviewKeyDownEventHandler(mainWebBrowser_PreviewKeyDown);
            this.PreviewKeyDown += new PreviewKeyDownEventHandler(mainWebBrowser_PreviewKeyDown);
            mainWebBrowser.BindTo(this);
            updateSaveFileState();

            foreach (ToolStripItem item in mainToolStrip.Items) {
                if (I18n.CanTranslate("FormMain." + item.Name)) {
                    item.Text = I18n.Translate("FormMain." + item.Name);
                }
            }
            for (int i = 0; i < I18n.LanguageDisplayList.Count; i++) {
                tscbLanguage.Items.Add(I18n.LanguageDisplayList[i].Value);
                if (I18n.LanguageDisplayList[i].Key == I18n.SelectedLanguage) {
                    tscbLanguage.SelectedIndex = i;
                    tscbLanguage.Text = I18n.LanguageDisplayList[i].Value;
                }
            }

            Task.Run(async () => {
                var info = await UpgradeInfo.FetchOnline(
                    "https://www.zbx1425.cn/nautilus/projectmeta.xml",
                    "BlocklyAts"
                );
                if (info != null) {
                    this.Invoke((Action)(() => {
                        info.ShowPromptAsRequired();
                    }));
                }
            });
        }

        private async void mainWebBrowser_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e) {
            if (e.KeyCode == Keys.F5) {
                if (ModifierKeys.HasFlag(Keys.Control) && ModifierKeys.HasFlag(Keys.Shift)) {
                    // Debug function.
                    currentWorkspace.BlocklyXml = new FPXElement(await mainWebBrowser.BkySaveWorkspace());
                    mainWebBrowser.Reload();
                    await Task.Delay(2000);
                    await mainWebBrowser.BkyLoadWorkspace(currentWorkspace.BlocklyXml);
                } else {
                    tsbtnCompileRun_Click(null, null);
                }
            } else if (e.KeyCode == Keys.F12) {
                mainWebBrowser.ShowDevTools();
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
                this.Text = "BlocklyAts: " + I18n.Translate("Msg.NotSaved");
                tsbtnSave.Enabled = false;
            } else {
                this.Text = "BlocklyAts: " + currentWorkspace.SaveFilePath;
                tsbtnSave.Enabled = true;
            }
        }

        private async void tsbtnNew_Click(object sender, EventArgs e) {
            if (MessageBox.Show(I18n.Translate("Msg.DiscardChange"), "Clear workspace", MessageBoxButtons.YesNo)
                == DialogResult.Yes) {
                currentWorkspace = new Workspace();
                await mainWebBrowser.BkyResetWorkspace();
                updateSaveFileState();
            }
        }

        private async void tsbtnSaveAs_Click(object sender, EventArgs e) {
            var sfd = new SaveFileDialog() {
                Filter = "BlocklyAts XML|*.batsxml",
                Title = "Save Workspace"
            };
            if (sfd.ShowDialog() != DialogResult.OK) return;

            currentWorkspace.BlocklyXml = new FPXElement(await mainWebBrowser.BkySaveWorkspace());
            currentWorkspace.SaveToFile(sfd.FileName);
            updateSaveFileState();
        }

        private async void tsbtnOpen_Click(object sender, EventArgs e) {
            var ofd = new OpenFileDialog() {
                Filter = "BlocklyAts XML|*.batsxml",
                Title = "Restore Workspace"
            };
            if (ofd.ShowDialog() != DialogResult.OK) return;

            try {
                currentWorkspace = Workspace.LoadFromFile(ofd.FileName);
                await mainWebBrowser.BkyLoadWorkspace(currentWorkspace.BlocklyXml);
                updateSaveFileState();
            } catch (Exception ex) {
                MessageBox.Show("This workspace savestate is malformed:\n" + ex.ToString());
            }
        }

        private async Task<string> buildAllPlatforms() {
            var luaCode = await mainWebBrowser.BkyExportLua();
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
                    CompilerFunction.CompileCSharp(await mainWebBrowser.BkyExportCSharp(), pair.Item2);
                } else {
                    await CompilerFunction.CompileLua(luaCode, pair.Item2, pair.Item1);
                }
            }
            return notifText;
        }

        private async void tsbtnCompile_Click(object sender, EventArgs e) {
            if (string.IsNullOrEmpty(currentWorkspace.SaveFilePath)) {
                MessageBox.Show(I18n.Translate("Msg.CompileNotSaved"));
                return;
            }

            if (ModifierKeys.HasFlag(Keys.Control) && ModifierKeys.HasFlag(Keys.Shift)) {
                Clipboard.SetText(await mainWebBrowser.BkyExportLua());
                return;
            }
            tsbtnCompile.Enabled = tsbtnCompileRun.Enabled = tsbtnCompileSetting.Enabled = false;
            try {
                var buildResult = await buildAllPlatforms();
                if (string.IsNullOrEmpty(buildResult)) {
                    MessageBox.Show(I18n.Translate("Msg.CompileNoTarget"));
                } else {
                    MessageBox.Show(I18n.Translate("Msg.CompileFinish") + buildResult);
                }
            } catch (Exception ex) {
                MessageBox.Show(I18n.Translate("Msg.CompileFail") + ex.Message);
            }
            tsbtnCompile.Enabled = tsbtnCompileRun.Enabled = tsbtnCompileSetting.Enabled = true;
        }

        private async void tsbtnSave_Click(object sender, EventArgs e) {
            if (string.IsNullOrEmpty(currentWorkspace.SaveFilePath)) return;
            currentWorkspace.BlocklyXml = new FPXElement(await mainWebBrowser.BkySaveWorkspace());
            currentWorkspace.SaveToFile();
        }

        private async void tsbtnCompileRun_Click(object sender, EventArgs e) {
            if (string.IsNullOrEmpty(currentWorkspace.SaveFilePath)) {
                MessageBox.Show(I18n.Translate("Msg.CompileNotSaved"));
                return;
            }

            tsbtnCompile.Enabled = tsbtnCompileRun.Enabled = tsbtnCompileSetting.Enabled = false;
            try {
                var buildResult = await buildAllPlatforms();
                if (string.IsNullOrEmpty(buildResult)) {
                    MessageBox.Show(I18n.Translate("Msg.CompileNoTarget"));
                } else {
                    var gameProcess = currentWorkspace.Config.GetGameProcess();
                    if (gameProcess == null) {
                        MessageBox.Show(I18n.Translate("Msg.NoGameProgram"));
                    } else {
                        gameProcess.Start();
                        await gameProcess.WaitForExitAsync();
                    }
                }
            } catch (Exception ex) {
                MessageBox.Show(I18n.Translate("Msg.CompileFail") + ex.Message);
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

        private async void tsbtnDebugWindow_Click(object sender, EventArgs e) {
            var formDebug = new FormDebug();
            formDebug.codeLua = (await mainWebBrowser.BkyExportLua()).Replace("\n", Environment.NewLine);
            formDebug.codeCSharp = (await mainWebBrowser.BkyExportCSharp()).Replace("\n", Environment.NewLine);
            formDebug.ShowDialog();
        }

        private void tsbtnAbout_Click(object sender, EventArgs e) {
            new FormAbout().ShowDialog();
        }

        private void tscbLanguage_SelectedIndexChanged(object sender, EventArgs e) {
            var newlang = I18n.LanguageDisplayList[tscbLanguage.SelectedIndex].Key;
            if (newlang != I18n.SelectedLanguage) {
                I18n.SelectedLanguage = newlang;
                MessageBox.Show(I18n.Translate("Msg.LanguageChange"));
            }
        }
    }
}
