using BlocklyAts.Host;
using BlocklyAts.Workspace;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BlocklyAts.UserInterface {
    public partial class FormBuildRunConfig : Form {

        public BuildRunConfig Config {
            get {
                var cfg = new BuildRunConfig {
                    ShouldCompilex86 = cbShouldx86.Checked,
                    ShouldCompilex64 = cbShouldx64.Checked,
                    ShouldCompileAnyCpu = cbShouldNet.Checked,
                    IncludeDebugInfo = cbIncludeDebugInfo.Checked,
                    CompilePathx86 = cbCustomx86.Checked ? tbx86.Text : null,
                    CompilePathx64 = cbCustomx64.Checked ? tbx64.Text : null,
                    CompilePathAnyCpu = cbCustomNet.Checked ? tbNet.Text : null,
                    GameType = BuildRunConfig.BveImpl.Unspecified,
                    GamePath = !string.IsNullOrEmpty(tbGamePath.Text) ? tbGamePath.Text : null,
                    GameArgs = !string.IsNullOrEmpty(tbGameArgs.Text) ? tbGameArgs.Text : null
                };
                if (rbGameBve5.Checked) cfg.GameType = BuildRunConfig.BveImpl.BveTs5;
                if (rbGameBve6.Checked) cfg.GameType = BuildRunConfig.BveImpl.BveTs6;
                if (rbGameOpenBve.Checked) cfg.GameType = BuildRunConfig.BveImpl.OpenBve;
                if (rbGameCustom.Checked) cfg.GameType = BuildRunConfig.BveImpl.Custom;
                return cfg;
            }
            set {
                cbShouldx86.Checked = value.ShouldCompilex86;
                cbShouldx64.Checked = value.ShouldCompilex64;
                cbShouldNet.Checked = value.ShouldCompileAnyCpu;
                cbCustomx86.Checked = !string.IsNullOrEmpty(value.CompilePathx86);
                cbCustomx64.Checked = !string.IsNullOrEmpty(value.CompilePathx64);
                cbCustomNet.Checked = !string.IsNullOrEmpty(value.CompilePathAnyCpu);
                cbIncludeDebugInfo.Checked = value.IncludeDebugInfo;
                tbx86.Text = value.CompilePathx86;
                tbx64.Text = value.CompilePathx64;
                tbNet.Text = value.CompilePathAnyCpu;
                tbGamePath.Text = value.GamePath;
                tbGameArgs.Text = value.GameArgs;
                rbGameBve5.Checked = value.GameType == BuildRunConfig.BveImpl.BveTs5;
                rbGameBve6.Checked = value.GameType == BuildRunConfig.BveImpl.BveTs6;
                rbGameOpenBve.Checked = value.GameType == BuildRunConfig.BveImpl.OpenBve;
                rbGameCustom.Checked = value.GameType == BuildRunConfig.BveImpl.Custom;
                cbShould_CheckedChanged(cbShouldx86, null);
                cbShould_CheckedChanged(cbShouldx64, null);
                cbShould_CheckedChanged(cbShouldNet, null);
                cbCustom_CheckedChanged(cbCustomx86, null);
                cbCustom_CheckedChanged(cbCustomx64, null);
                cbCustom_CheckedChanged(cbCustomNet, null);
                rbGame_CheckedChanged(rbGameCustom, null);
            }
        }

        public FormBuildRunConfig() {
            InitializeComponent();
            tbGameArgs.Width = 0;
            rbGameBve5.Enabled = GameDetection.BveTs5Path != null;
            rbGameBve6.Enabled = GameDetection.BveTs6Path != null;
            rbGameOpenBve.Enabled = GameDetection.OpenBvePath != null;
        }

        private void cbCustom_CheckedChanged(object sender, EventArgs e) {
            var sendercb = sender as CheckBox;
            var arch = sendercb.Name.Replace("cbCustom", "");
            tlpMain.Controls["btnBrowse" + arch].Enabled = sendercb.Checked;
            tlpMain.Controls["tb" + arch].Enabled = sendercb.Checked;
            var notifText = "Default: <FileName>_" + arch.ToLowerInvariant() + ".dll";
            if (sendercb.Checked) {
                if (tlpMain.Controls["tb" + arch].Text == notifText) {
                    tlpMain.Controls["tb" + arch].Text = "";
                }
            } else {
                tlpMain.Controls["tb" + arch].Text = notifText;
            }
        }

        private void cbShould_CheckedChanged(object sender, EventArgs e) {
            var sendercb = sender as CheckBox;
            var arch = sendercb.Name.Replace("cbShould", "");
            tlpMain.Controls["cbCustom" + arch].Enabled = sendercb.Checked;
            if (!sendercb.Checked) {
                (tlpMain.Controls["cbCustom" + arch] as CheckBox).Checked = false;
                cbCustom_CheckedChanged(tlpMain.Controls["cbCustom" + arch], null);
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e) {
            var senderbtn = sender as Button;
            var arch = senderbtn.Name.Replace("btnBrowse", "");
            var sfd = new SaveFileDialog {
                Title = "Select DLL Path",
                Filter = sender.Equals(btnBrowseGamePath) ? "Executable|*.exe" : "Dynamic-link library|*.dll"
            };
            if (sfd.ShowDialog() != DialogResult.OK) return;
            tlpMain.Controls["tb" + arch].Text = sfd.FileName;
        }

        private void btnOK_Click(object sender, EventArgs e) {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e) {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void FormCompilerConfig_Load(object sender, EventArgs e) {
            foreach (Control ctrl in tlpMain.Controls) {
                if (ctrl.Name.StartsWith("cbCustom")) {
                    ctrl.Text = I18n.Translate("FormCompilerConfig.cbCustom");
                } else if (ctrl.Name.StartsWith("btnBrowse")) {
                    ctrl.Text = I18n.Translate("FormCompilerConfig.btnBrowse");
                } else if (I18n.CanTranslate("FormCompilerConfig." + ctrl.Name)) {
                    ctrl.Text = I18n.Translate("FormCompilerConfig." + ctrl.Name);
                }
            }
            foreach (Control ctrl in flpActBtn.Controls) {
                if (I18n.CanTranslate("FormCompilerConfig." + ctrl.Name)) {
                    ctrl.Text = I18n.Translate("FormCompilerConfig." + ctrl.Name);
                }
            }
            foreach (Control ctrl in flpRbGame.Controls) {
                if (I18n.CanTranslate("FormCompilerConfig." + ctrl.Name)) {
                    ctrl.Text = I18n.Translate("FormCompilerConfig." + ctrl.Name);
                }
            }
        }

        private void rbGame_CheckedChanged(object sender, EventArgs e) {
            tbGamePath.Enabled = btnBrowseGamePath.Enabled = rbGameCustom.Checked;
            btnGenerateArgs.Enabled = rbGameBve5.Checked || rbGameBve6.Checked || rbGameOpenBve.Checked;
        }

        private void btnGenerateArgs_Click(object sender, EventArgs e) {
            if (rbGameBve5.Checked || rbGameBve6.Checked) {
                var ofd = new OpenFileDialog() {
                    Title = I18n.Translate("Text.GenArg.SelectScenario"),
                    Filter = "BveTs5/6 Scenario TXT|*.txt"
                };
                var bveScenarioPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    "BveTs", "Scenarios"
                );
                if (Directory.Exists(bveScenarioPath)) ofd.InitialDirectory = bveScenarioPath;
                if (ofd.ShowDialog() != DialogResult.OK) return;
                // "!= true" is necessary because it's a nullable bool ("bool?")
                if (File.ReadLines(ofd.FileName).FirstOrDefault()
                    ?.StartsWith("BveTs Scenario", StringComparison.OrdinalIgnoreCase) != true) {
                    MessageBox.Show(I18n.Translate("Msg.GenArgInvalidSelect"), "Invalid Selection",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                tbGameArgs.Text = string.Format("\"{0}\"", ofd.FileName);
            } else if (rbGameOpenBve.Checked) {
                var ofd = new OpenFileDialog() {
                    Title = I18n.Translate("Text.GenArg.SelectRoute"),
                    Filter = "BveTs4/5/OpenBve Route CSV|*.csv"
                };
                if (ofd.ShowDialog() != DialogResult.OK) return;
                var routeContent = File.ReadAllText(ofd.FileName);
                // A simple check
                if (routeContent.IndexOf("Structure", StringComparison.OrdinalIgnoreCase) < 0 ||
                    routeContent.IndexOf("Track", StringComparison.OrdinalIgnoreCase) < 0) {
                    MessageBox.Show(I18n.Translate("Msg.GenArgInvalidSelect"), "Invalid Selection",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var fsd = new FolderSelectDialog() {
                    Title = I18n.Translate("Text.GenArg.SelectTrain")
                };
                if (!fsd.ShowDialog()) return;
                // To be more *nix-compatible?
                var trainFiles = Directory.GetFiles(fsd.FileName).Select(f => Path.GetFileName(f).ToLowerInvariant());
                if (!trainFiles.Contains("train.dat")) {
                    MessageBox.Show(I18n.Translate("Msg.GenArgInvalidSelect"), "Invalid Selection",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                tbGameArgs.Text = string.Format("\"/route={0}\" \"/train={1}\"", ofd.FileName, fsd.FileName);
            }
        }
    }
}
