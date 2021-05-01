using BlocklyAts.Host;
using BlocklyAts.WebView;
using BlocklyAts.Workspace;
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

namespace BlocklyAts.UserInterface {
    public partial class FormMain : Form {

        public FormMain() {
            InitializeComponent();

            for (int i = 0; i < I18n.LanguageDisplayList.Count; i++) {
                tscbLanguage.Items.Add(I18n.LanguageDisplayList[i].Value);
                if (I18n.LanguageDisplayList[i].Key == PreferenceManager.CurrentPreference.Language) {
                    tscbLanguage.SelectedIndex = i;
                    tscbLanguage.Text = I18n.LanguageDisplayList[i].Value;
                }
            }
        }

        public FormMain(string workspacePath) : this() {
            if (!File.Exists(workspacePath)) return;
            try {
                currentWorkspace = SaveState.LoadFromFile(workspacePath);
                if (currentWorkspace == null) currentWorkspace = new SaveState();
                updateSaveFileState();
            } catch {
                // Give up, if it is not possible to load the workspace.
            }
        }

        private BaseBrowser mainWebBrowser;

        private SaveState currentWorkspace = new SaveState();

        private async void FormMain_Load(object sender, EventArgs e) {
            ApplyLanguage();

            if (PreferenceManager.FirstStartup) {
                this.Shown += (s2, e2) => {
                    MessageBox.Show(I18n.TranslateAllLang("Msg.BugReport"), "BlocklyAts",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                };
            }

            await Task.Run(async () => {
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

        private void ApplyLanguage() {
            foreach (ToolStripItem item in mainToolStrip.Items) {
                if (I18n.CanTranslate("FormMain." + item.Name)) {
                    item.Text = I18n.Translate("FormMain." + item.Name);
                }
            }
            updateSaveFileState();
#if DEBUG
            string webDirectory = Path.Combine(Path.GetDirectoryName(PlatformFunction.AppDir), "www");
            if (!Directory.Exists(webDirectory)) webDirectory = Path.Combine(PlatformFunction.AppDir, "www");
#else
            string webDirectory = Path.Combine(CompilerFunction.appDir, "www");
#endif
            
            string pageURL = Path.Combine(webDirectory, "index.html") + string.Format("?ver={0}&lang={1}",
                PlatformFunction.VersionString,
                I18n.Translate("BlocklyName")
            );
            if (mainWebBrowser == null) {
                mainWebBrowser = BaseBrowser.AcquireInstance(pageURL);
                int tsbSize;
                if (mainWebBrowser is ExternalBrowser) {
                    this.TopMost = true;
                    this.MinimumSize = new Size(700, 70);
                    this.Size = new Size(700, 200);
                    tsbSize = 40;
                } else {
                    tsbSize = 26;
                }
                foreach (ToolStripItem item in mainToolStrip.Items) {
                    if (item is ToolStripSeparator) continue;
                    item.ImageScaling = ToolStripItemImageScaling.SizeToFit;
                    var iconPath = Path.Combine(PlatformFunction.AppDir, "resource", "icon-light", item.Name + ".png");
                    if (!File.Exists(iconPath)) continue;
                    item.Image = Image.FromFile(iconPath);
                }
                foreach (ToolStripItem item in tsddbInfo.DropDownItems) {
                    if (item is ToolStripSeparator) continue;
                    item.ImageScaling = ToolStripItemImageScaling.SizeToFit;
                    var iconPath = Path.Combine(PlatformFunction.AppDir, "resource", "icon-light", item.Name + ".png");
                    if (!File.Exists(iconPath)) continue;
                    item.Image = Image.FromFile(iconPath);
                }
                mainToolStrip.ImageScalingSize = new Size(tsbSize, tsbSize);
                mainToolStrip.Height = tsbSize;
                mainWebBrowser.KeyDown += mainWebBrowser_PreviewKeyDown;
                this.PreviewKeyDown += mainWebBrowser_PreviewKeyDown;
                mainWebBrowser.PageFinished += mainWebBrowser_PageFinished;
                mainWebBrowser.BindTo(this);
            } else {
                MessageBox.Show(I18n.Translate("Msg.LanguageChange"));
                return;
            }
        }

        private void mainWebBrowser_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e) {
            if (e.KeyCode == Keys.F5) {
                if (ModifierKeys.HasFlag(Keys.Control) && ModifierKeys.HasFlag(Keys.Shift)) {
                    // Debug function.
                    ApplyLanguage();
                    mainWebBrowser.Reload();
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

        private async void mainWebBrowser_PageFinished(object sender, EventArgs e) {
            if (currentWorkspace == null || currentWorkspace.BlocklyXml == null) return;
            await mainWebBrowser.BkyLoadInitWorkspace(currentWorkspace.BlocklyXml);
        }

        private void updateSaveFileState() {
            if (string.IsNullOrEmpty(currentWorkspace.SaveFilePath)) {
                this.Text = "BlocklyAts: " + I18n.Translate("Text.NotSaved");
                tsbtnSave.Enabled = false;
            } else {
                this.Text = "BlocklyAts: " + currentWorkspace.SaveFilePath;
                tsbtnSave.Enabled = true;
            }
        }

        private async void tsbtnNew_Click(object sender, EventArgs e) {
            if (MessageBox.Show(I18n.Translate("Msg.DiscardChange"), "Clear workspace", MessageBoxButtons.YesNo)
                == DialogResult.Yes) {
                currentWorkspace = new SaveState();
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
            
            await saveWorkspace(sfd.FileName);
        }

        private async void tsbtnOpen_Click(object sender, EventArgs e) {
            var ofd = new OpenFileDialog() {
                Filter = "BlocklyAts XML|*.batsxml",
                Title = "Load Workspace"
            };
            if (ofd.ShowDialog() != DialogResult.OK) return;
            
            try {
                SaveState newWorkspace  = SaveState.LoadFromFile(ofd.FileName);
                if (newWorkspace != null) {
                    currentWorkspace = newWorkspace;
                    await mainWebBrowser.BkyLoadWorkspace(currentWorkspace.BlocklyXml);
                    updateSaveFileState();
                }
            } catch (Exception ex) {
                MessageBox.Show("This workspace savestate is malformed:\n" + ex.ToString());
            }
        }

        private async Task saveWorkspace(string path = null) {
            if (path == null && string.IsNullOrEmpty(currentWorkspace.SaveFilePath)) return;
            var workspaceState = await mainWebBrowser.BkySaveWorkspace();
            if (workspaceState == null) return;
            currentWorkspace.BlocklyXml = new FPXElement(workspaceState);
            currentWorkspace.SaveToFile(path);
            updateSaveFileState();
        }

        private async Task<string> buildAllPlatforms() {
            await saveWorkspace(); // Autosave
            flashSaveBtn();

            var cSharpCode = await mainWebBrowser.BkyExportCSharp();
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
                    CompilerFunction.CompileCSharpOpenBve(
                        cSharpCode, pair.Item2, currentWorkspace.Config.IncludeDebugInfo
                    );
                } else {
                    CompilerFunction.CompileCSharpUnmanaged(
                        cSharpCode, pair.Item2, pair.Item1, currentWorkspace.Config.IncludeDebugInfo
                    );
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
                Clipboard.SetText(await mainWebBrowser.BkyExportCSharp());
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
            if (!tsbtnSave.Enabled) return;
            await saveWorkspace();
            flashSaveBtn();
        }

        private void flashSaveBtn() {
            new System.Threading.Thread(() => {
                var iconPathSrc = Path.Combine(PlatformFunction.AppDir, "resource", "icon-light", "tsbtnSave.png");
                var iconPathOk = Path.Combine(PlatformFunction.AppDir, "resource", "icon-light", "tsbtnSave_Ok.png");
                Image sourceIcon = Image.FromFile(iconPathSrc);
                Image flashingIcon = Image.FromFile(iconPathOk);
                for (int i = 0; i <= 5; i++) {
                    this.BeginInvoke((Action)(() => {
                        if (i % 2 == 0) {
                            tsbtnSave.Image = flashingIcon;
                        } else {
                            tsbtnSave.Image = sourceIcon;
                        }
                        tsbtnSave.Invalidate();
                    }));
                    System.Threading.Thread.Sleep(100);
                }
            }).Start();
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
            var ccd = new FormBuildRunConfig {
                Config = currentWorkspace.Config
            };
            if (ccd.ShowDialog() == DialogResult.OK) {
                currentWorkspace.Config = ccd.Config;
            }
        }

        private async void tsbtnDebugWindow_Click(object sender, EventArgs e) {
                tsbtnDebugWindow.Enabled = false;
                var codeCSharp = await mainWebBrowser.BkyExportCSharp();
                if (codeCSharp == null) {
                    tsbtnDebugWindow.Enabled = true;
                    return;
                }
                tsbtnDebugWindow.Enabled = true;
                this.Invoke((Action)(() => {
                    var formDebug = new FormDebug(codeCSharp);
                    formDebug.ShowDialog(this);
                }));
        }

        private void tsbtnAbout_Click(object sender, EventArgs e) {
            new FormAbout().ShowDialog();
        }

        private void tscbLanguage_SelectedIndexChanged(object sender, EventArgs e) {
            var newlang = I18n.LanguageDisplayList[tscbLanguage.SelectedIndex].Key;
            if (newlang != PreferenceManager.CurrentPreference.Language) {
                PreferenceManager.CurrentPreference.Language = newlang;
                ApplyLanguage();
            }
        }

        private void tsbtnHelp_Click(object sender, EventArgs e) {
            PlatformFunction.CallBrowser("https://github.com/zbx1425/BlocklyAts/wiki");
        }

        private void tsbtnBugReport_Click(object sender, EventArgs e) {
            new FormBugReport().ShowDialog();
        }
    }
}
