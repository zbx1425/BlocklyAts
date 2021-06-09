using BlocklyAts.Host;
using BlocklyAts.Workspace;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BlocklyAts.UserInterface {
    public partial class FormCompileError : Form {

        public FormCompileError(CompilerFunction.CompileException ex) {
            InitializeComponent();

            lblMessage.Text = I18n.Translate("Msg.CompileFail");
            foreach (var error in ex.Errors) {
                listView1.Items.Add(new ListViewItem(new string[] {
                    error.ErrorNumber,
                    error.Line.ToString(),
                    error.Column.ToString(),
                    error.ErrorText
                }));
            }
        }

        private void btnOK_Click(object sender, EventArgs e) {
            this.Close();
        }
    }
}
