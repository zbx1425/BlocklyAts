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
    public partial class FormDebug : Form {

        private FastColoredTextBoxNS.FastColoredTextBox tbCode;
        private TextBox fallbackTbCode;

        public readonly string codeOpenBve, codeBve456;

        public FormDebug(string codeCSharp) {
            InitializeComponent();
            this.codeOpenBve = CompilerFunction.CombineCode(codeCSharp, CompilerFunction.Platform.OpenBve);
            this.codeOpenBve = this.codeOpenBve.Replace("\n", Environment.NewLine);
            this.codeBve456 = CompilerFunction.CombineCode(codeCSharp, CompilerFunction.Platform.WinDll32);
            this.codeBve456 = this.codeBve456.Replace("\n", Environment.NewLine);
            ResetTextbox(codeBve456);
        }

        private void rbCheckedChanged(object sender, EventArgs e) {
            if (sender == rbBve456) {
                ResetTextbox(codeBve456);
            } else {
                ResetTextbox(codeOpenBve);
            }
        }

        private void ResetTextbox(string text) {
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
                tbCode.Text = text;
                tbCode.SelectionChanged += (object sender, EventArgs e) => {
                    var selection = tbCode.Selection.Start;
                    if (selection == null) {
                        selection = new FastColoredTextBoxNS.Place(0, 0);
                    }
                    lblCursorPos.Text = string.Format("Row: {0}; Col: {1}", 
                        selection.iLine + 1, selection.iChar + 1);
                };
                lblCursorPos.Text = "Row: 0; Col: 0";
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
                fallbackTbCode.Text = text;
                lblCursorPos.Text = "";
                pnlMain.Controls.Add(fallbackTbCode);
            }
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
