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
    public partial class FormBugReport : Form {
        public FormBugReport() {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void llbGithub_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            PlatformFunction.CallBrowser("https://github.com/zbx1425/BlocklyAts/issues/new");
        }

        private void llbTwitter_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            PlatformFunction.CallBrowser("https://twitter.com/messages/compose?recipient_id=797350934653210624");
        }

        private void llbEmail_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            PlatformFunction.CallBrowser("mailto:feedback@zbx1425.cn?subject=BlocklyAts%20Bug%20Report");
        }

        private void FormBugReport_Load(object sender, EventArgs e) {
            lblHintText.Text = I18n.Translate("Msg.BugReport");
        }
    }
}
