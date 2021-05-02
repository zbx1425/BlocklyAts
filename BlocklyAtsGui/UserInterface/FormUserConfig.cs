using BlocklyAts.Host;
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
using System.Windows.Forms;

namespace BlocklyAts.UserInterface {
    public partial class FormUserConfig : Form {

        public FormUserConfig() {
            InitializeComponent();
            cbDarkTheme.Checked = PreferenceManager.Current.DarkMode;
            cbExternalWebView.Checked = PreferenceManager.Current.PreferExternalBrowser;
        }
        
        private void FormUserConfig_Load(object sender, EventArgs e) {
            foreach (Control ctrl in tlpMain.Controls) {
                if (I18n.CanTranslate("FormUserConfig." + ctrl.Name)) {
                    ctrl.Text = I18n.Translate("FormUserConfig." + ctrl.Name);
                }
            }
            foreach (Control ctrl in flpActBtn.Controls) {
                if (I18n.CanTranslate("Text." + ctrl.Name)) {
                    ctrl.Text = I18n.Translate("Text." + ctrl.Name);
                }
            }
            lblRequireRestart.Text = I18n.Translate("Msg.NeedRestart");
        }

        private void btnOK_Click(object sender, EventArgs e) {
            PreferenceManager.Current.DarkMode = cbDarkTheme.Checked;
            PreferenceManager.Current.PreferExternalBrowser = cbExternalWebView.Checked;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e) {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnClearRecentFiles_Click(object sender, EventArgs e) {
            PreferenceManager.Current.RecentFiles.Clear();
        }

        private void btnUninstall_Click(object sender, EventArgs e) {
            if (MessageBox.Show("Are you sure?", "Confirm", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No) return;
            this.DialogResult = DialogResult.Abort;
            this.Close();
        }
    }
}
