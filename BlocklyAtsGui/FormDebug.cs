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
    public partial class FormDebug : Form {
        public FormDebug() {
            InitializeComponent();
        }

        public string codeLua { get; set; }
        public string codeCSharp { get; set; }

        private void rbLua_CheckedChanged(object sender, EventArgs e) {
            if (sender == rbLua) {
                tbCode.Text = CompilerFunction.BoilerplateLua + codeLua;
            } else {
                tbCode.Text = CompilerFunction.BoilerplateCSharp + codeCSharp;
            }
        }

        private void FormDebug_Load(object sender, EventArgs e) {
            rbLua_CheckedChanged(rbLua, null);
        }
    }
}
