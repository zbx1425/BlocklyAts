using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BlocklyAts {
    public partial class FormCompilerConfig : Form {

        public CompilerConfig Config {
            get {
                var cfg = new CompilerConfig {
                    ShouldCompilex86 = cbShouldx86.Checked,
                    ShouldCompilex64 = cbShouldx64.Checked,
                    ShouldCompileAnyCpu = cbShouldAny.Checked,
                    CompilePathx86 = cbCustomx86.Checked ? tbx86.Text : null,
                    CompilePathx64 = cbCustomx64.Checked ? tbx64.Text : null,
                    CompilePathAnyCpu = cbCustomAny.Checked ? tbAny.Text : null,
                    GameType = CompilerConfig.BveImpl.Unspecified,
                    GamePath = !string.IsNullOrEmpty(tbGamePath.Text) ? tbGamePath.Text : null,
                    GameArgs = !string.IsNullOrEmpty(tbGameArgs.Text) ? tbGameArgs.Text : null
                };
                if (rbGameBve5.Checked) cfg.GameType = CompilerConfig.BveImpl.BveTs5;
                if (rbGameBve6.Checked) cfg.GameType = CompilerConfig.BveImpl.BveTs6;
                if (rbGameOpenBve.Checked) cfg.GameType = CompilerConfig.BveImpl.OpenBve;
                if (rbGameCustom.Checked) cfg.GameType = CompilerConfig.BveImpl.Custom;
                return cfg;
            }
            set {
                cbShouldx86.Checked = value.ShouldCompilex86;
                cbShouldx64.Checked = value.ShouldCompilex64;
                cbShouldAny.Checked = value.ShouldCompileAnyCpu;
                cbCustomx86.Checked = !string.IsNullOrEmpty(value.CompilePathx86);
                cbCustomx64.Checked = !string.IsNullOrEmpty(value.CompilePathx64);
                cbCustomAny.Checked = !string.IsNullOrEmpty(value.CompilePathAnyCpu);
                tbx86.Text = value.CompilePathx86;
                tbx64.Text = value.CompilePathx64;
                tbAny.Text = value.CompilePathAnyCpu;
                tbGamePath.Text = value.GamePath;
                tbGameArgs.Text = value.GameArgs;
                rbGameBve5.Checked = value.GameType == CompilerConfig.BveImpl.BveTs5;
                rbGameBve6.Checked = value.GameType == CompilerConfig.BveImpl.BveTs6;
                rbGameOpenBve.Checked = value.GameType == CompilerConfig.BveImpl.OpenBve;
                rbGameCustom.Checked = value.GameType == CompilerConfig.BveImpl.Custom;
                cbShould_CheckedChanged(cbShouldx86, null);
                cbShould_CheckedChanged(cbShouldx64, null);
                cbShould_CheckedChanged(cbShouldAny, null);
                cbCustom_CheckedChanged(cbCustomx86, null);
                cbCustom_CheckedChanged(cbCustomx64, null);
                cbCustom_CheckedChanged(cbCustomAny, null);
            }
        }

        public FormCompilerConfig() {
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
            if (sendercb.Checked) {
                if (tlpMain.Controls["tb" + arch].Text == "(Auto-detect)") {
                    tlpMain.Controls["tb" + arch].Text = "";
                }
            } else {
                tlpMain.Controls["tb" + arch].Text = "(Auto-detect)";
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
            foreach (Control ctrl in flp.Controls) {
                if (I18n.CanTranslate("FormCompilerConfig." + ctrl.Name)) {
                    ctrl.Text = I18n.Translate("FormCompilerConfig." + ctrl.Name);
                }
            }
        }
    }
}
