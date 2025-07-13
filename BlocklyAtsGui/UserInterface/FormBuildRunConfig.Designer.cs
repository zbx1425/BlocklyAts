namespace BlocklyAts.UserInterface {
    partial class FormBuildRunConfig {
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
            this.tlpRun = new System.Windows.Forms.TableLayoutPanel();
            this.tbGameArgs = new System.Windows.Forms.TextBox();
            this.tbGamePath = new System.Windows.Forms.TextBox();
            this.btnBrowseNet = new System.Windows.Forms.Button();
            this.btnBrowsex64 = new System.Windows.Forms.Button();
            this.tbNet = new System.Windows.Forms.TextBox();
            this.tbx64 = new System.Windows.Forms.TextBox();
            this.cbShouldx86 = new System.Windows.Forms.CheckBox();
            this.cbShouldx64 = new System.Windows.Forms.CheckBox();
            this.cbShouldNet = new System.Windows.Forms.CheckBox();
            this.cbCustomx86 = new System.Windows.Forms.CheckBox();
            this.cbCustomx64 = new System.Windows.Forms.CheckBox();
            this.cbCustomNet = new System.Windows.Forms.CheckBox();
            this.tbx86 = new System.Windows.Forms.TextBox();
            this.btnBrowsex86 = new System.Windows.Forms.Button();
            this.lblGameArgs = new System.Windows.Forms.Label();
            this.btnBrowseGamePath = new System.Windows.Forms.Button();
            this.btnGenerateArgs = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.flpActBtn = new System.Windows.Forms.FlowLayoutPanel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.cbIncludeDebugInfo = new System.Windows.Forms.CheckBox();
            this.tlpOutput = new System.Windows.Forms.TableLayoutPanel();
            this.lblGamePath = new System.Windows.Forms.Label();
            this.rbGameCustom = new System.Windows.Forms.RadioButton();
            this.rbGameOpenBve = new System.Windows.Forms.RadioButton();
            this.rbGameBve6 = new System.Windows.Forms.RadioButton();
            this.rbGameBve5 = new System.Windows.Forms.RadioButton();
            this.flpRbGame = new System.Windows.Forms.FlowLayoutPanel();
            this.tlpRun.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.flpActBtn.SuspendLayout();
            this.tlpOutput.SuspendLayout();
            this.flpRbGame.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpRun
            // 
            this.tlpRun.AutoSize = true;
            this.tlpRun.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tlpRun.ColumnCount = 4;
            this.tlpRun.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpRun.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpRun.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpRun.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpRun.Controls.Add(this.flpRbGame, 1, 4);
            this.tlpRun.Controls.Add(this.tbGameArgs, 1, 6);
            this.tlpRun.Controls.Add(this.tbGamePath, 1, 5);
            this.tlpRun.Controls.Add(this.lblGamePath, 0, 4);
            this.tlpRun.Controls.Add(this.lblGameArgs, 0, 6);
            this.tlpRun.Controls.Add(this.btnBrowseGamePath, 3, 5);
            this.tlpRun.Controls.Add(this.btnGenerateArgs, 3, 6);
            this.tlpRun.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpRun.Location = new System.Drawing.Point(12, 25);
            this.tlpRun.Margin = new System.Windows.Forms.Padding(12);
            this.tlpRun.Name = "tlpRun";
            this.tlpRun.RowCount = 8;
            this.tlpRun.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpRun.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpRun.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpRun.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpRun.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpRun.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpRun.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpRun.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpRun.Size = new System.Drawing.Size(712, 196);
            this.tlpRun.TabIndex = 0;
            // 
            // tbGameArgs
            // 
            this.tlpRun.SetColumnSpan(this.tbGameArgs, 2);
            this.tbGameArgs.Dock = System.Windows.Forms.DockStyle.Top;
            this.tbGameArgs.Location = new System.Drawing.Point(215, 73);
            this.tbGameArgs.MinimumSize = new System.Drawing.Size(0, 120);
            this.tbGameArgs.Multiline = true;
            this.tbGameArgs.Name = "tbGameArgs";
            this.tbGameArgs.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbGameArgs.Size = new System.Drawing.Size(368, 120);
            this.tbGameArgs.TabIndex = 17;
            // 
            // tbGamePath
            // 
            this.tlpRun.SetColumnSpan(this.tbGamePath, 2);
            this.tbGamePath.Dock = System.Windows.Forms.DockStyle.Top;
            this.tbGamePath.Location = new System.Drawing.Point(215, 37);
            this.tbGamePath.Name = "tbGamePath";
            this.tbGamePath.Size = new System.Drawing.Size(368, 28);
            this.tbGamePath.TabIndex = 16;
            // 
            // btnBrowseNet
            // 
            this.btnBrowseNet.AutoSize = true;
            this.btnBrowseNet.Location = new System.Drawing.Point(629, 95);
            this.btnBrowseNet.Name = "btnBrowseNet";
            this.btnBrowseNet.Size = new System.Drawing.Size(80, 30);
            this.btnBrowseNet.TabIndex = 11;
            this.btnBrowseNet.Text = "Browse";
            this.btnBrowseNet.UseVisualStyleBackColor = true;
            this.btnBrowseNet.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // btnBrowsex64
            // 
            this.btnBrowsex64.AutoSize = true;
            this.btnBrowsex64.Location = new System.Drawing.Point(629, 49);
            this.btnBrowsex64.Name = "btnBrowsex64";
            this.btnBrowsex64.Size = new System.Drawing.Size(80, 30);
            this.btnBrowsex64.TabIndex = 7;
            this.btnBrowsex64.Text = "Browse";
            this.btnBrowsex64.UseVisualStyleBackColor = true;
            this.btnBrowsex64.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // tbNet
            // 
            this.tbNet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbNet.Location = new System.Drawing.Point(335, 95);
            this.tbNet.Name = "tbNet";
            this.tbNet.Size = new System.Drawing.Size(288, 28);
            this.tbNet.TabIndex = 10;
            // 
            // tbx64
            // 
            this.tbx64.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbx64.Location = new System.Drawing.Point(335, 49);
            this.tbx64.Name = "tbx64";
            this.tbx64.Size = new System.Drawing.Size(288, 28);
            this.tbx64.TabIndex = 6;
            // 
            // cbShouldx86
            // 
            this.cbShouldx86.AutoSize = true;
            this.cbShouldx86.Location = new System.Drawing.Point(3, 3);
            this.cbShouldx86.Name = "cbShouldx86";
            this.cbShouldx86.Size = new System.Drawing.Size(232, 40);
            this.cbShouldx86.TabIndex = 0;
            this.cbShouldx86.Text = "32-bit Windows DLL\r\nFor BVE4/5 and OpenBVE";
            this.cbShouldx86.UseVisualStyleBackColor = true;
            this.cbShouldx86.CheckedChanged += new System.EventHandler(this.cbShould_CheckedChanged);
            // 
            // cbShouldx64
            // 
            this.cbShouldx64.AutoSize = true;
            this.cbShouldx64.Location = new System.Drawing.Point(3, 49);
            this.cbShouldx64.Name = "cbShouldx64";
            this.cbShouldx64.Size = new System.Drawing.Size(196, 40);
            this.cbShouldx64.TabIndex = 4;
            this.cbShouldx64.Text = "64-bit Windows DLL\r\nFor BVE6\r\n";
            this.cbShouldx64.UseVisualStyleBackColor = true;
            this.cbShouldx64.CheckedChanged += new System.EventHandler(this.cbShould_CheckedChanged);
            // 
            // cbShouldNet
            // 
            this.cbShouldNet.AutoSize = true;
            this.cbShouldNet.Location = new System.Drawing.Point(3, 95);
            this.cbShouldNet.Name = "cbShouldNet";
            this.cbShouldNet.Size = new System.Drawing.Size(214, 40);
            this.cbShouldNet.TabIndex = 8;
            this.cbShouldNet.Text = "AnyCpu .NET Assembly\r\nFor OpenBVE\r\n";
            this.cbShouldNet.UseVisualStyleBackColor = true;
            this.cbShouldNet.CheckedChanged += new System.EventHandler(this.cbShould_CheckedChanged);
            // 
            // cbCustomx86
            // 
            this.cbCustomx86.AutoSize = true;
            this.cbCustomx86.Location = new System.Drawing.Point(241, 3);
            this.cbCustomx86.Name = "cbCustomx86";
            this.cbCustomx86.Size = new System.Drawing.Size(88, 40);
            this.cbCustomx86.TabIndex = 1;
            this.cbCustomx86.Text = "Custom\r\nPath";
            this.cbCustomx86.UseVisualStyleBackColor = true;
            this.cbCustomx86.CheckedChanged += new System.EventHandler(this.cbCustom_CheckedChanged);
            // 
            // cbCustomx64
            // 
            this.cbCustomx64.AutoSize = true;
            this.cbCustomx64.Location = new System.Drawing.Point(241, 49);
            this.cbCustomx64.Name = "cbCustomx64";
            this.cbCustomx64.Size = new System.Drawing.Size(88, 40);
            this.cbCustomx64.TabIndex = 5;
            this.cbCustomx64.Text = "Custom\r\nPath\r\n";
            this.cbCustomx64.UseVisualStyleBackColor = true;
            this.cbCustomx64.CheckedChanged += new System.EventHandler(this.cbCustom_CheckedChanged);
            // 
            // cbCustomNet
            // 
            this.cbCustomNet.AutoSize = true;
            this.cbCustomNet.Location = new System.Drawing.Point(241, 95);
            this.cbCustomNet.Name = "cbCustomNet";
            this.cbCustomNet.Size = new System.Drawing.Size(88, 40);
            this.cbCustomNet.TabIndex = 9;
            this.cbCustomNet.Text = "Custom\r\nPath\r\n";
            this.cbCustomNet.UseVisualStyleBackColor = true;
            this.cbCustomNet.CheckedChanged += new System.EventHandler(this.cbCustom_CheckedChanged);
            // 
            // tbx86
            // 
            this.tbx86.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbx86.Location = new System.Drawing.Point(335, 3);
            this.tbx86.Name = "tbx86";
            this.tbx86.Size = new System.Drawing.Size(288, 28);
            this.tbx86.TabIndex = 2;
            // 
            // btnBrowsex86
            // 
            this.btnBrowsex86.AutoSize = true;
            this.btnBrowsex86.Location = new System.Drawing.Point(629, 3);
            this.btnBrowsex86.Name = "btnBrowsex86";
            this.btnBrowsex86.Size = new System.Drawing.Size(80, 30);
            this.btnBrowsex86.TabIndex = 3;
            this.btnBrowsex86.Text = "Browse";
            this.btnBrowsex86.UseVisualStyleBackColor = true;
            this.btnBrowsex86.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // lblGameArgs
            // 
            this.lblGameArgs.AutoSize = true;
            this.lblGameArgs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblGameArgs.Location = new System.Drawing.Point(3, 70);
            this.lblGameArgs.Name = "lblGameArgs";
            this.lblGameArgs.Size = new System.Drawing.Size(206, 126);
            this.lblGameArgs.TabIndex = 14;
            this.lblGameArgs.Text = "Game Program Arguments";
            this.lblGameArgs.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnBrowseGamePath
            // 
            this.btnBrowseGamePath.AutoSize = true;
            this.btnBrowseGamePath.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnBrowseGamePath.Location = new System.Drawing.Point(589, 37);
            this.btnBrowseGamePath.MinimumSize = new System.Drawing.Size(120, 30);
            this.btnBrowseGamePath.Name = "btnBrowseGamePath";
            this.btnBrowseGamePath.Size = new System.Drawing.Size(120, 30);
            this.btnBrowseGamePath.TabIndex = 15;
            this.btnBrowseGamePath.Text = "Browse";
            this.btnBrowseGamePath.UseVisualStyleBackColor = true;
            this.btnBrowseGamePath.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // btnGenerateArgs
            // 
            this.btnGenerateArgs.AutoSize = true;
            this.btnGenerateArgs.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnGenerateArgs.Location = new System.Drawing.Point(589, 73);
            this.btnGenerateArgs.MinimumSize = new System.Drawing.Size(120, 60);
            this.btnGenerateArgs.Name = "btnGenerateArgs";
            this.btnGenerateArgs.Size = new System.Drawing.Size(120, 60);
            this.btnGenerateArgs.TabIndex = 15;
            this.btnGenerateArgs.Text = "Auto Start\r\nScenario\r\n";
            this.btnGenerateArgs.UseVisualStyleBackColor = true;
            this.btnGenerateArgs.Click += new System.EventHandler(this.btnGenerateArgs_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.flpActBtn, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.groupBox3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.groupBox2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(53, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(20, 8, 12, 8);
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(774, 584);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.AutoSize = true;
            this.groupBox1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox1.Controls.Add(this.tlpOutput);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(23, 11);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(12, 4, 12, 4);
            this.groupBox1.Size = new System.Drawing.Size(736, 167);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Output";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(131)))), ((int)(((byte)(221)))));
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(53, 584);
            this.panel1.TabIndex = 2;
            // 
            // groupBox2
            // 
            this.groupBox2.AutoSize = true;
            this.groupBox2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox2.Controls.Add(this.cbIncludeDebugInfo);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(23, 184);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(16, 4, 12, 4);
            this.groupBox2.Size = new System.Drawing.Size(736, 87);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Debug";
            // 
            // groupBox3
            // 
            this.groupBox3.AutoSize = true;
            this.groupBox3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox3.Controls.Add(this.tlpRun);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(23, 277);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(12, 4, 12, 4);
            this.groupBox3.Size = new System.Drawing.Size(736, 225);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Run";
            // 
            // flpActBtn
            // 
            this.flpActBtn.AutoSize = true;
            this.flpActBtn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.SetColumnSpan(this.flpActBtn, 3);
            this.flpActBtn.Controls.Add(this.btnCancel);
            this.flpActBtn.Controls.Add(this.btnOK);
            this.flpActBtn.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.flpActBtn.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flpActBtn.Location = new System.Drawing.Point(23, 515);
            this.flpActBtn.Name = "flpActBtn";
            this.flpActBtn.Padding = new System.Windows.Forms.Padding(12, 4, 12, 4);
            this.flpActBtn.Size = new System.Drawing.Size(736, 58);
            this.flpActBtn.TabIndex = 13;
            // 
            // btnCancel
            // 
            this.btnCancel.AutoSize = true;
            this.btnCancel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(609, 7);
            this.btnCancel.MinimumSize = new System.Drawing.Size(100, 30);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Padding = new System.Windows.Forms.Padding(0, 8, 0, 8);
            this.btnCancel.Size = new System.Drawing.Size(100, 44);
            this.btnCancel.TabIndex = 13;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.AutoSize = true;
            this.btnOK.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnOK.Location = new System.Drawing.Point(503, 7);
            this.btnOK.MinimumSize = new System.Drawing.Size(100, 30);
            this.btnOK.Name = "btnOK";
            this.btnOK.Padding = new System.Windows.Forms.Padding(0, 8, 0, 8);
            this.btnOK.Size = new System.Drawing.Size(100, 44);
            this.btnOK.TabIndex = 12;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // cbIncludeDebugInfo
            // 
            this.cbIncludeDebugInfo.AutoSize = true;
            this.cbIncludeDebugInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbIncludeDebugInfo.Location = new System.Drawing.Point(16, 25);
            this.cbIncludeDebugInfo.Name = "cbIncludeDebugInfo";
            this.cbIncludeDebugInfo.Size = new System.Drawing.Size(708, 58);
            this.cbIncludeDebugInfo.TabIndex = 21;
            this.cbIncludeDebugInfo.Text = "Include PDB\r\nDisplays the line number of the error when the plugin crashes.\r\nPlea" +
    "se turn it off when Releasing your plugin!";
            this.cbIncludeDebugInfo.UseVisualStyleBackColor = true;
            // 
            // tlpOutput
            // 
            this.tlpOutput.AutoSize = true;
            this.tlpOutput.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tlpOutput.ColumnCount = 4;
            this.tlpOutput.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpOutput.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpOutput.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpOutput.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpOutput.Controls.Add(this.tbx86, 2, 0);
            this.tlpOutput.Controls.Add(this.tbx64, 2, 1);
            this.tlpOutput.Controls.Add(this.tbNet, 2, 2);
            this.tlpOutput.Controls.Add(this.btnBrowseNet, 3, 2);
            this.tlpOutput.Controls.Add(this.cbCustomNet, 1, 2);
            this.tlpOutput.Controls.Add(this.btnBrowsex64, 3, 1);
            this.tlpOutput.Controls.Add(this.cbCustomx64, 1, 1);
            this.tlpOutput.Controls.Add(this.btnBrowsex86, 3, 0);
            this.tlpOutput.Controls.Add(this.cbShouldx86, 0, 0);
            this.tlpOutput.Controls.Add(this.cbCustomx86, 1, 0);
            this.tlpOutput.Controls.Add(this.cbShouldx64, 0, 1);
            this.tlpOutput.Controls.Add(this.cbShouldNet, 0, 2);
            this.tlpOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpOutput.Location = new System.Drawing.Point(12, 25);
            this.tlpOutput.Name = "tlpOutput";
            this.tlpOutput.RowCount = 3;
            this.tlpOutput.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpOutput.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpOutput.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpOutput.Size = new System.Drawing.Size(712, 138);
            this.tlpOutput.TabIndex = 0;
            // 
            // lblGamePath
            // 
            this.lblGamePath.AutoSize = true;
            this.lblGamePath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblGamePath.Location = new System.Drawing.Point(3, 0);
            this.lblGamePath.Name = "lblGamePath";
            this.tlpRun.SetRowSpan(this.lblGamePath, 2);
            this.lblGamePath.Size = new System.Drawing.Size(206, 70);
            this.lblGamePath.TabIndex = 13;
            this.lblGamePath.Text = "Game Program Path";
            this.lblGamePath.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // rbGameCustom
            // 
            this.rbGameCustom.AutoSize = true;
            this.rbGameCustom.Location = new System.Drawing.Point(255, 3);
            this.rbGameCustom.Name = "rbGameCustom";
            this.rbGameCustom.Size = new System.Drawing.Size(87, 22);
            this.rbGameCustom.TabIndex = 17;
            this.rbGameCustom.TabStop = true;
            this.rbGameCustom.Text = "Custom";
            this.rbGameCustom.UseVisualStyleBackColor = true;
            this.rbGameCustom.CheckedChanged += new System.EventHandler(this.rbGame_CheckedChanged);
            // 
            // rbGameOpenBve
            // 
            this.rbGameOpenBve.AutoSize = true;
            this.rbGameOpenBve.Location = new System.Drawing.Point(153, 3);
            this.rbGameOpenBve.Name = "rbGameOpenBve";
            this.rbGameOpenBve.Size = new System.Drawing.Size(96, 22);
            this.rbGameOpenBve.TabIndex = 16;
            this.rbGameOpenBve.TabStop = true;
            this.rbGameOpenBve.Text = "OpenBVE";
            this.rbGameOpenBve.UseVisualStyleBackColor = true;
            this.rbGameOpenBve.CheckedChanged += new System.EventHandler(this.rbGame_CheckedChanged);
            // 
            // rbGameBve6
            // 
            this.rbGameBve6.AutoSize = true;
            this.rbGameBve6.Location = new System.Drawing.Point(78, 3);
            this.rbGameBve6.Name = "rbGameBve6";
            this.rbGameBve6.Size = new System.Drawing.Size(69, 22);
            this.rbGameBve6.TabIndex = 15;
            this.rbGameBve6.TabStop = true;
            this.rbGameBve6.Text = "BVE6";
            this.rbGameBve6.UseVisualStyleBackColor = true;
            this.rbGameBve6.CheckedChanged += new System.EventHandler(this.rbGame_CheckedChanged);
            // 
            // rbGameBve5
            // 
            this.rbGameBve5.AutoSize = true;
            this.rbGameBve5.Location = new System.Drawing.Point(3, 3);
            this.rbGameBve5.Name = "rbGameBve5";
            this.rbGameBve5.Size = new System.Drawing.Size(69, 22);
            this.rbGameBve5.TabIndex = 14;
            this.rbGameBve5.TabStop = true;
            this.rbGameBve5.Text = "BVE5";
            this.rbGameBve5.UseVisualStyleBackColor = true;
            this.rbGameBve5.CheckedChanged += new System.EventHandler(this.rbGame_CheckedChanged);
            // 
            // flpRbGame
            // 
            this.flpRbGame.AutoSize = true;
            this.flpRbGame.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tlpRun.SetColumnSpan(this.flpRbGame, 3);
            this.flpRbGame.Controls.Add(this.rbGameBve5);
            this.flpRbGame.Controls.Add(this.rbGameBve6);
            this.flpRbGame.Controls.Add(this.rbGameOpenBve);
            this.flpRbGame.Controls.Add(this.rbGameCustom);
            this.flpRbGame.Location = new System.Drawing.Point(215, 3);
            this.flpRbGame.Name = "flpRbGame";
            this.flpRbGame.Size = new System.Drawing.Size(345, 28);
            this.flpRbGame.TabIndex = 19;
            // 
            // FormBuildRunConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(827, 584);
            this.ControlBox = false;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormBuildRunConfig";
            this.Text = "Compiler Configuration";
            this.Load += new System.EventHandler(this.FormCompilerConfig_Load);
            this.tlpRun.ResumeLayout(false);
            this.tlpRun.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.flpActBtn.ResumeLayout(false);
            this.flpActBtn.PerformLayout();
            this.tlpOutput.ResumeLayout(false);
            this.tlpOutput.PerformLayout();
            this.flpRbGame.ResumeLayout(false);
            this.flpRbGame.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpRun;
        private System.Windows.Forms.CheckBox cbShouldx86;
        private System.Windows.Forms.TextBox tbNet;
        private System.Windows.Forms.TextBox tbx64;
        private System.Windows.Forms.CheckBox cbShouldx64;
        private System.Windows.Forms.CheckBox cbShouldNet;
        private System.Windows.Forms.CheckBox cbCustomx86;
        private System.Windows.Forms.CheckBox cbCustomx64;
        private System.Windows.Forms.CheckBox cbCustomNet;
        private System.Windows.Forms.TextBox tbx86;
        private System.Windows.Forms.Button btnBrowseNet;
        private System.Windows.Forms.Button btnBrowsex64;
        private System.Windows.Forms.Button btnBrowsex86;
        private System.Windows.Forms.TextBox tbGameArgs;
        private System.Windows.Forms.TextBox tbGamePath;
        private System.Windows.Forms.Label lblGameArgs;
        private System.Windows.Forms.Button btnBrowseGamePath;
        private System.Windows.Forms.Button btnGenerateArgs;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel flpActBtn;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox cbIncludeDebugInfo;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TableLayoutPanel tlpOutput;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.FlowLayoutPanel flpRbGame;
        private System.Windows.Forms.RadioButton rbGameBve5;
        private System.Windows.Forms.RadioButton rbGameBve6;
        private System.Windows.Forms.RadioButton rbGameOpenBve;
        private System.Windows.Forms.RadioButton rbGameCustom;
        private System.Windows.Forms.Label lblGamePath;
    }
}