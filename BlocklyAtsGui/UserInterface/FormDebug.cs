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

        private FastColoredTextBoxNS.FastColoredTextBox tbCode;
        private TextBox fallbackTbCode;

        public readonly string codeCSharp;

        public FormDebug(string codeCSharp) {
            InitializeComponent();
            this.codeCSharp = CompilerFunction.CombineCodeForCSharp(codeCSharp, true);
            this.codeCSharp = this.codeCSharp.Replace("\n", Environment.NewLine);
            ResetTextbox();
        }

        private void ResetTextbox() {
            SuspendLayout();
            pnlMain.Controls.Clear();
            if (!PlatformFunction.IsMono) {
                ComponentResourceManager resources = new ComponentResourceManager(typeof(FormDebug));
                tbCode = new FastColoredTextBoxNS.FastColoredTextBox();
                ((ISupportInitialize)(tbCode)).BeginInit();
                tbCode.AutoScrollMinSize = new Size(27, 14);
                tbCode.BackBrush = null;
                tbCode.CharHeight = 14;
                tbCode.CharWidth = 8;
                tbCode.Cursor = Cursors.IBeam;
                tbCode.DisabledColor = Color.FromArgb(100, 180, 180, 180);
                tbCode.Font = new Font("Courier New", 9.75F);
                tbCode.IsReplaceMode = false;
                tbCode.Name = "tbCode";
                tbCode.ReadOnly = true;
                tbCode.TabLength = 2;
                tbCode.SelectionColor = Color.FromArgb(60, 0, 0, 255);
                tbCode.ServiceColors = new FastColoredTextBoxNS.ServiceColors();
                tbCode.Dock = DockStyle.Fill;
                tbCode.TabIndex = 2;
                tbCode.Zoom = 100;
                tbCode.Language = FastColoredTextBoxNS.Language.CSharp;
                tbCode.Text = codeCSharp;
                pnlMain.Controls.Add(tbCode);
                ((ISupportInitialize)(tbCode)).EndInit();
            } else {
                fallbackTbCode = new TextBox();
                fallbackTbCode.Cursor = Cursors.IBeam;
                fallbackTbCode.Font = new Font("Courier New", 9.75F);
                fallbackTbCode.Location = new Point(13, 50);
                fallbackTbCode.Name = "fallbackTbCode";
                fallbackTbCode.ReadOnly = true;
                fallbackTbCode.Multiline = true;
                fallbackTbCode.WordWrap = false;
                fallbackTbCode.ScrollBars = ScrollBars.Both;
                fallbackTbCode.Dock = DockStyle.Fill;
                fallbackTbCode.TabIndex = 2;
                fallbackTbCode.Text = codeCSharp;
                pnlMain.Controls.Add(fallbackTbCode);
            }
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
