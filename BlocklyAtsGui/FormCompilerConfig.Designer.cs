namespace BlocklyAts {
    partial class FormCompilerConfig {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.tbGameArgs = new System.Windows.Forms.TextBox();
            this.tbGamePath = new System.Windows.Forms.TextBox();
            this.btnBrowseAny = new System.Windows.Forms.Button();
            this.btnBrowsex64 = new System.Windows.Forms.Button();
            this.tbAny = new System.Windows.Forms.TextBox();
            this.tbx64 = new System.Windows.Forms.TextBox();
            this.cbShouldx86 = new System.Windows.Forms.CheckBox();
            this.cbShouldx64 = new System.Windows.Forms.CheckBox();
            this.cbShouldAny = new System.Windows.Forms.CheckBox();
            this.cbCustomx86 = new System.Windows.Forms.CheckBox();
            this.cbCustomx64 = new System.Windows.Forms.CheckBox();
            this.cbCustomAny = new System.Windows.Forms.CheckBox();
            this.tbx86 = new System.Windows.Forms.TextBox();
            this.flp = new System.Windows.Forms.FlowLayoutPanel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnBrowsex86 = new System.Windows.Forms.Button();
            this.lblGamePath = new System.Windows.Forms.Label();
            this.lblGameArgs = new System.Windows.Forms.Label();
            this.btnBrowseGamePath = new System.Windows.Forms.Button();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.rbGameBve5 = new System.Windows.Forms.RadioButton();
            this.rbGameBve6 = new System.Windows.Forms.RadioButton();
            this.rbGameOpenBve = new System.Windows.Forms.RadioButton();
            this.rbGameCustom = new System.Windows.Forms.RadioButton();
            this.tlpMain.SuspendLayout();
            this.flp.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpMain
            // 
            this.tlpMain.AutoSize = true;
            this.tlpMain.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tlpMain.ColumnCount = 4;
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpMain.Controls.Add(this.flowLayoutPanel1, 0, 3);
            this.tlpMain.Controls.Add(this.tbGameArgs, 1, 5);
            this.tlpMain.Controls.Add(this.tbGamePath, 1, 4);
            this.tlpMain.Controls.Add(this.btnBrowseAny, 3, 2);
            this.tlpMain.Controls.Add(this.btnBrowsex64, 3, 1);
            this.tlpMain.Controls.Add(this.tbAny, 2, 2);
            this.tlpMain.Controls.Add(this.tbx64, 2, 1);
            this.tlpMain.Controls.Add(this.cbShouldx86, 0, 0);
            this.tlpMain.Controls.Add(this.cbShouldx64, 0, 1);
            this.tlpMain.Controls.Add(this.cbShouldAny, 0, 2);
            this.tlpMain.Controls.Add(this.cbCustomx86, 1, 0);
            this.tlpMain.Controls.Add(this.cbCustomx64, 1, 1);
            this.tlpMain.Controls.Add(this.cbCustomAny, 1, 2);
            this.tlpMain.Controls.Add(this.tbx86, 2, 0);
            this.tlpMain.Controls.Add(this.flp, 2, 6);
            this.tlpMain.Controls.Add(this.btnBrowsex86, 3, 0);
            this.tlpMain.Controls.Add(this.lblGamePath, 0, 3);
            this.tlpMain.Controls.Add(this.lblGameArgs, 0, 5);
            this.tlpMain.Controls.Add(this.btnBrowseGamePath, 3, 4);
            this.tlpMain.Location = new System.Drawing.Point(12, 12);
            this.tlpMain.Margin = new System.Windows.Forms.Padding(12);
            this.tlpMain.Name = "tlpMain";
            this.tlpMain.RowCount = 7;
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMain.Size = new System.Drawing.Size(682, 309);
            this.tlpMain.TabIndex = 0;
            // 
            // tbGameArgs
            // 
            this.tlpMain.SetColumnSpan(this.tbGameArgs, 3);
            this.tbGameArgs.Dock = System.Windows.Forms.DockStyle.Top;
            this.tbGameArgs.Location = new System.Drawing.Point(212, 208);
            this.tbGameArgs.Multiline = true;
            this.tbGameArgs.Name = "tbGameArgs";
            this.tbGameArgs.Size = new System.Drawing.Size(467, 60);
            this.tbGameArgs.TabIndex = 17;
            // 
            // tbGamePath
            // 
            this.tlpMain.SetColumnSpan(this.tbGamePath, 2);
            this.tbGamePath.Dock = System.Windows.Forms.DockStyle.Top;
            this.tbGamePath.Location = new System.Drawing.Point(212, 176);
            this.tbGamePath.Name = "tbGamePath";
            this.tbGamePath.Size = new System.Drawing.Size(381, 26);
            this.tbGamePath.TabIndex = 16;
            // 
            // btnBrowseAny
            // 
            this.btnBrowseAny.AutoSize = true;
            this.btnBrowseAny.Location = new System.Drawing.Point(599, 87);
            this.btnBrowseAny.Name = "btnBrowseAny";
            this.btnBrowseAny.Size = new System.Drawing.Size(80, 26);
            this.btnBrowseAny.TabIndex = 11;
            this.btnBrowseAny.Text = "Browse";
            this.btnBrowseAny.UseVisualStyleBackColor = true;
            this.btnBrowseAny.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // btnBrowsex64
            // 
            this.btnBrowsex64.AutoSize = true;
            this.btnBrowsex64.Location = new System.Drawing.Point(599, 45);
            this.btnBrowsex64.Name = "btnBrowsex64";
            this.btnBrowsex64.Size = new System.Drawing.Size(80, 26);
            this.btnBrowsex64.TabIndex = 7;
            this.btnBrowsex64.Text = "Browse";
            this.btnBrowsex64.UseVisualStyleBackColor = true;
            this.btnBrowsex64.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // tbAny
            // 
            this.tbAny.Location = new System.Drawing.Point(293, 87);
            this.tbAny.Name = "tbAny";
            this.tbAny.Size = new System.Drawing.Size(300, 26);
            this.tbAny.TabIndex = 10;
            // 
            // tbx64
            // 
            this.tbx64.Location = new System.Drawing.Point(293, 45);
            this.tbx64.Name = "tbx64";
            this.tbx64.Size = new System.Drawing.Size(300, 26);
            this.tbx64.TabIndex = 6;
            // 
            // cbShouldx86
            // 
            this.cbShouldx86.AutoSize = true;
            this.cbShouldx86.Location = new System.Drawing.Point(3, 3);
            this.cbShouldx86.Name = "cbShouldx86";
            this.cbShouldx86.Size = new System.Drawing.Size(203, 36);
            this.cbShouldx86.TabIndex = 0;
            this.cbShouldx86.Text = "32-bit Windows DLL\r\nFor BVE4/5 and OpenBVE";
            this.cbShouldx86.UseVisualStyleBackColor = true;
            this.cbShouldx86.CheckedChanged += new System.EventHandler(this.cbShould_CheckedChanged);
            // 
            // cbShouldx64
            // 
            this.cbShouldx64.AutoSize = true;
            this.cbShouldx64.Location = new System.Drawing.Point(3, 45);
            this.cbShouldx64.Name = "cbShouldx64";
            this.cbShouldx64.Size = new System.Drawing.Size(171, 36);
            this.cbShouldx64.TabIndex = 4;
            this.cbShouldx64.Text = "64-bit Windows DLL\r\nFor BVE6\r\n";
            this.cbShouldx64.UseVisualStyleBackColor = true;
            this.cbShouldx64.CheckedChanged += new System.EventHandler(this.cbShould_CheckedChanged);
            // 
            // cbShouldAny
            // 
            this.cbShouldAny.AutoSize = true;
            this.cbShouldAny.Location = new System.Drawing.Point(3, 87);
            this.cbShouldAny.Name = "cbShouldAny";
            this.cbShouldAny.Size = new System.Drawing.Size(187, 36);
            this.cbShouldAny.TabIndex = 8;
            this.cbShouldAny.Text = "AnyCpu .NET Assembly\r\nFor OpenBVE\r\n";
            this.cbShouldAny.UseVisualStyleBackColor = true;
            this.cbShouldAny.CheckedChanged += new System.EventHandler(this.cbShould_CheckedChanged);
            // 
            // cbCustomx86
            // 
            this.cbCustomx86.AutoSize = true;
            this.cbCustomx86.Location = new System.Drawing.Point(212, 3);
            this.cbCustomx86.Name = "cbCustomx86";
            this.cbCustomx86.Size = new System.Drawing.Size(75, 36);
            this.cbCustomx86.TabIndex = 1;
            this.cbCustomx86.Text = "Custom\r\nPath";
            this.cbCustomx86.UseVisualStyleBackColor = true;
            this.cbCustomx86.CheckedChanged += new System.EventHandler(this.cbCustom_CheckedChanged);
            // 
            // cbCustomx64
            // 
            this.cbCustomx64.AutoSize = true;
            this.cbCustomx64.Location = new System.Drawing.Point(212, 45);
            this.cbCustomx64.Name = "cbCustomx64";
            this.cbCustomx64.Size = new System.Drawing.Size(75, 36);
            this.cbCustomx64.TabIndex = 5;
            this.cbCustomx64.Text = "Custom\r\nPath\r\n";
            this.cbCustomx64.UseVisualStyleBackColor = true;
            this.cbCustomx64.CheckedChanged += new System.EventHandler(this.cbCustom_CheckedChanged);
            // 
            // cbCustomAny
            // 
            this.cbCustomAny.AutoSize = true;
            this.cbCustomAny.Location = new System.Drawing.Point(212, 87);
            this.cbCustomAny.Name = "cbCustomAny";
            this.cbCustomAny.Size = new System.Drawing.Size(75, 36);
            this.cbCustomAny.TabIndex = 9;
            this.cbCustomAny.Text = "Custom\r\nPath\r\n";
            this.cbCustomAny.UseVisualStyleBackColor = true;
            this.cbCustomAny.CheckedChanged += new System.EventHandler(this.cbCustom_CheckedChanged);
            // 
            // tbx86
            // 
            this.tbx86.Location = new System.Drawing.Point(293, 3);
            this.tbx86.Name = "tbx86";
            this.tbx86.Size = new System.Drawing.Size(300, 26);
            this.tbx86.TabIndex = 2;
            // 
            // flp
            // 
            this.flp.AutoSize = true;
            this.flp.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tlpMain.SetColumnSpan(this.flp, 2);
            this.flp.Controls.Add(this.btnCancel);
            this.flp.Controls.Add(this.btnOK);
            this.flp.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.flp.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flp.Location = new System.Drawing.Point(293, 274);
            this.flp.Name = "flp";
            this.flp.Size = new System.Drawing.Size(386, 32);
            this.flp.TabIndex = 12;
            // 
            // btnCancel
            // 
            this.btnCancel.AutoSize = true;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(303, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 26);
            this.btnCancel.TabIndex = 13;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.AutoSize = true;
            this.btnOK.Location = new System.Drawing.Point(217, 3);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(80, 26);
            this.btnOK.TabIndex = 12;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnBrowsex86
            // 
            this.btnBrowsex86.AutoSize = true;
            this.btnBrowsex86.Location = new System.Drawing.Point(599, 3);
            this.btnBrowsex86.Name = "btnBrowsex86";
            this.btnBrowsex86.Size = new System.Drawing.Size(80, 26);
            this.btnBrowsex86.TabIndex = 3;
            this.btnBrowsex86.Text = "Browse";
            this.btnBrowsex86.UseVisualStyleBackColor = true;
            this.btnBrowsex86.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // lblGamePath
            // 
            this.lblGamePath.AutoSize = true;
            this.lblGamePath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblGamePath.Location = new System.Drawing.Point(3, 126);
            this.lblGamePath.Name = "lblGamePath";
            this.tlpMain.SetRowSpan(this.lblGamePath, 2);
            this.lblGamePath.Size = new System.Drawing.Size(203, 79);
            this.lblGamePath.TabIndex = 13;
            this.lblGamePath.Text = "Game Program Path";
            this.lblGamePath.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblGameArgs
            // 
            this.lblGameArgs.AutoSize = true;
            this.lblGameArgs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblGameArgs.Location = new System.Drawing.Point(3, 205);
            this.lblGameArgs.Name = "lblGameArgs";
            this.lblGameArgs.Size = new System.Drawing.Size(203, 66);
            this.lblGameArgs.TabIndex = 14;
            this.lblGameArgs.Text = "Game Program Arguments";
            this.lblGameArgs.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnBrowseGamePath
            // 
            this.btnBrowseGamePath.AutoSize = true;
            this.btnBrowseGamePath.Location = new System.Drawing.Point(599, 176);
            this.btnBrowseGamePath.Name = "btnBrowseGamePath";
            this.btnBrowseGamePath.Size = new System.Drawing.Size(80, 26);
            this.btnBrowseGamePath.TabIndex = 15;
            this.btnBrowseGamePath.Text = "Browse";
            this.btnBrowseGamePath.UseVisualStyleBackColor = true;
            this.btnBrowseGamePath.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tlpMain.SetColumnSpan(this.flowLayoutPanel1, 3);
            this.flowLayoutPanel1.Controls.Add(this.rbGameBve5);
            this.flowLayoutPanel1.Controls.Add(this.rbGameBve6);
            this.flowLayoutPanel1.Controls.Add(this.rbGameOpenBve);
            this.flowLayoutPanel1.Controls.Add(this.rbGameCustom);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(212, 129);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Padding = new System.Windows.Forms.Padding(0, 15, 0, 0);
            this.flowLayoutPanel1.Size = new System.Drawing.Size(467, 41);
            this.flowLayoutPanel1.TabIndex = 19;
            // 
            // rbGameBve5
            // 
            this.rbGameBve5.AutoSize = true;
            this.rbGameBve5.Location = new System.Drawing.Point(3, 18);
            this.rbGameBve5.Name = "rbGameBve5";
            this.rbGameBve5.Size = new System.Drawing.Size(58, 20);
            this.rbGameBve5.TabIndex = 14;
            this.rbGameBve5.TabStop = true;
            this.rbGameBve5.Text = "BVE5";
            this.rbGameBve5.UseVisualStyleBackColor = true;
            // 
            // rbGameBve6
            // 
            this.rbGameBve6.AutoSize = true;
            this.rbGameBve6.Location = new System.Drawing.Point(67, 18);
            this.rbGameBve6.Name = "rbGameBve6";
            this.rbGameBve6.Size = new System.Drawing.Size(58, 20);
            this.rbGameBve6.TabIndex = 15;
            this.rbGameBve6.TabStop = true;
            this.rbGameBve6.Text = "BVE6";
            this.rbGameBve6.UseVisualStyleBackColor = true;
            // 
            // rbGameOpenBve
            // 
            this.rbGameOpenBve.AutoSize = true;
            this.rbGameOpenBve.Location = new System.Drawing.Point(131, 18);
            this.rbGameOpenBve.Name = "rbGameOpenBve";
            this.rbGameOpenBve.Size = new System.Drawing.Size(82, 20);
            this.rbGameOpenBve.TabIndex = 16;
            this.rbGameOpenBve.TabStop = true;
            this.rbGameOpenBve.Text = "OpenBVE";
            this.rbGameOpenBve.UseVisualStyleBackColor = true;
            // 
            // rbGameCustom
            // 
            this.rbGameCustom.AutoSize = true;
            this.rbGameCustom.Location = new System.Drawing.Point(219, 18);
            this.rbGameCustom.Name = "rbGameCustom";
            this.rbGameCustom.Size = new System.Drawing.Size(74, 20);
            this.rbGameCustom.TabIndex = 17;
            this.rbGameCustom.TabStop = true;
            this.rbGameCustom.Text = "Custom";
            this.rbGameCustom.UseVisualStyleBackColor = true;
            // 
            // FormCompilerConfig
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.ControlBox = false;
            this.Controls.Add(this.tlpMain);
            this.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormCompilerConfig";
            this.Text = "Compiler Configuration";
            this.Load += new System.EventHandler(this.FormCompilerConfig_Load);
            this.tlpMain.ResumeLayout(false);
            this.tlpMain.PerformLayout();
            this.flp.ResumeLayout(false);
            this.flp.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private System.Windows.Forms.CheckBox cbShouldx86;
        private System.Windows.Forms.TextBox tbAny;
        private System.Windows.Forms.TextBox tbx64;
        private System.Windows.Forms.CheckBox cbShouldx64;
        private System.Windows.Forms.CheckBox cbShouldAny;
        private System.Windows.Forms.CheckBox cbCustomx86;
        private System.Windows.Forms.CheckBox cbCustomx64;
        private System.Windows.Forms.CheckBox cbCustomAny;
        private System.Windows.Forms.TextBox tbx86;
        private System.Windows.Forms.FlowLayoutPanel flp;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnBrowseAny;
        private System.Windows.Forms.Button btnBrowsex64;
        private System.Windows.Forms.Button btnBrowsex86;
        private System.Windows.Forms.TextBox tbGameArgs;
        private System.Windows.Forms.TextBox tbGamePath;
        private System.Windows.Forms.Label lblGamePath;
        private System.Windows.Forms.Label lblGameArgs;
        private System.Windows.Forms.Button btnBrowseGamePath;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.RadioButton rbGameBve5;
        private System.Windows.Forms.RadioButton rbGameBve6;
        private System.Windows.Forms.RadioButton rbGameOpenBve;
        private System.Windows.Forms.RadioButton rbGameCustom;
    }
}