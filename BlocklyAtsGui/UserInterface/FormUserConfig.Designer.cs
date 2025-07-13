namespace BlocklyAts.UserInterface {
    partial class FormUserConfig {
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
            this.btnUninstall = new System.Windows.Forms.Button();
            this.btnClearRecentFiles = new System.Windows.Forms.Button();
            this.cbExternalWebView = new System.Windows.Forms.CheckBox();
            this.cbDarkTheme = new System.Windows.Forms.CheckBox();
            this.lblRequireRestart = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.flpActBtn = new System.Windows.Forms.FlowLayoutPanel();
            this.tlpMain.SuspendLayout();
            this.flpActBtn.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpMain
            // 
            this.tlpMain.AutoSize = true;
            this.tlpMain.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tlpMain.ColumnCount = 3;
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpMain.Controls.Add(this.btnUninstall, 2, 2);
            this.tlpMain.Controls.Add(this.btnClearRecentFiles, 2, 1);
            this.tlpMain.Controls.Add(this.cbExternalWebView, 0, 2);
            this.tlpMain.Controls.Add(this.cbDarkTheme, 0, 1);
            this.tlpMain.Controls.Add(this.lblRequireRestart, 0, 0);
            this.tlpMain.Controls.Add(this.flpActBtn, 2, 3);
            this.tlpMain.Location = new System.Drawing.Point(9, 9);
            this.tlpMain.Margin = new System.Windows.Forms.Padding(9);
            this.tlpMain.Name = "tlpMain";
            this.tlpMain.Padding = new System.Windows.Forms.Padding(12, 4, 8, 4);
            this.tlpMain.RowCount = 5;
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMain.Size = new System.Drawing.Size(533, 149);
            this.tlpMain.TabIndex = 0;
            // 
            // btnUninstall
            // 
            this.btnUninstall.AutoSize = true;
            this.btnUninstall.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnUninstall.Location = new System.Drawing.Point(282, 70);
            this.btnUninstall.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.btnUninstall.Name = "btnUninstall";
            this.btnUninstall.Size = new System.Drawing.Size(234, 28);
            this.btnUninstall.TabIndex = 20;
            this.btnUninstall.Text = "Clear All Data And Cache";
            this.btnUninstall.UseVisualStyleBackColor = true;
            this.btnUninstall.Click += new System.EventHandler(this.btnUninstall_Click);
            // 
            // btnClearRecentFiles
            // 
            this.btnClearRecentFiles.AutoSize = true;
            this.btnClearRecentFiles.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnClearRecentFiles.Location = new System.Drawing.Point(282, 42);
            this.btnClearRecentFiles.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.btnClearRecentFiles.Name = "btnClearRecentFiles";
            this.btnClearRecentFiles.Size = new System.Drawing.Size(243, 28);
            this.btnClearRecentFiles.TabIndex = 18;
            this.btnClearRecentFiles.Text = "Clear \"Recent Files\" List";
            this.btnClearRecentFiles.UseVisualStyleBackColor = true;
            this.btnClearRecentFiles.Click += new System.EventHandler(this.btnClearRecentFiles_Click);
            // 
            // cbExternalWebView
            // 
            this.cbExternalWebView.AutoSize = true;
            this.cbExternalWebView.Location = new System.Drawing.Point(12, 70);
            this.cbExternalWebView.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.cbExternalWebView.Name = "cbExternalWebView";
            this.cbExternalWebView.Size = new System.Drawing.Size(250, 22);
            this.cbExternalWebView.TabIndex = 17;
            this.cbExternalWebView.Text = "Use External Web Browser";
            this.cbExternalWebView.UseVisualStyleBackColor = true;
            // 
            // cbDarkTheme
            // 
            this.cbDarkTheme.AutoSize = true;
            this.cbDarkTheme.Location = new System.Drawing.Point(12, 42);
            this.cbDarkTheme.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.cbDarkTheme.Name = "cbDarkTheme";
            this.cbDarkTheme.Size = new System.Drawing.Size(124, 22);
            this.cbDarkTheme.TabIndex = 19;
            this.cbDarkTheme.Text = "Dark Theme";
            this.cbDarkTheme.UseVisualStyleBackColor = true;
            // 
            // lblRequireRestart
            // 
            this.lblRequireRestart.AutoSize = true;
            this.lblRequireRestart.Location = new System.Drawing.Point(14, 4);
            this.lblRequireRestart.Margin = new System.Windows.Forms.Padding(2, 0, 2, 20);
            this.lblRequireRestart.Name = "lblRequireRestart";
            this.lblRequireRestart.Size = new System.Drawing.Size(143, 18);
            this.lblRequireRestart.TabIndex = 21;
            this.lblRequireRestart.Text = "Require Restart";
            // 
            // btnOK
            // 
            this.btnOK.AutoSize = true;
            this.btnOK.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnOK.Location = new System.Drawing.Point(33, 13);
            this.btnOK.Margin = new System.Windows.Forms.Padding(2);
            this.btnOK.MinimumSize = new System.Drawing.Size(100, 0);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(100, 28);
            this.btnOK.TabIndex = 12;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.AutoSize = true;
            this.btnCancel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(137, 13);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(2);
            this.btnCancel.MinimumSize = new System.Drawing.Size(100, 0);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 28);
            this.btnCancel.TabIndex = 13;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // flpActBtn
            // 
            this.flpActBtn.AutoSize = true;
            this.flpActBtn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpActBtn.Controls.Add(this.btnCancel);
            this.flpActBtn.Controls.Add(this.btnOK);
            this.flpActBtn.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.flpActBtn.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flpActBtn.Location = new System.Drawing.Point(284, 100);
            this.flpActBtn.Margin = new System.Windows.Forms.Padding(2);
            this.flpActBtn.Name = "flpActBtn";
            this.flpActBtn.Padding = new System.Windows.Forms.Padding(0, 11, 0, 0);
            this.flpActBtn.Size = new System.Drawing.Size(239, 43);
            this.flpActBtn.TabIndex = 22;
            // 
            // FormUserConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(583, 338);
            this.ControlBox = false;
            this.Controls.Add(this.tlpMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormUserConfig";
            this.Text = "Preferences";
            this.Load += new System.EventHandler(this.FormUserConfig_Load);
            this.tlpMain.ResumeLayout(false);
            this.tlpMain.PerformLayout();
            this.flpActBtn.ResumeLayout(false);
            this.flpActBtn.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private System.Windows.Forms.CheckBox cbExternalWebView;
        private System.Windows.Forms.CheckBox cbDarkTheme;
        private System.Windows.Forms.Button btnClearRecentFiles;
        private System.Windows.Forms.Button btnUninstall;
        private System.Windows.Forms.Label lblRequireRestart;
        private System.Windows.Forms.FlowLayoutPanel flpActBtn;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
    }
}