namespace BlocklyAts {
    partial class FormBugReport {
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
            this.lblHintText = new System.Windows.Forms.Label();
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.llbEmail = new System.Windows.Forms.LinkLabel();
            this.llbTwitter = new System.Windows.Forms.LinkLabel();
            this.lblGithub = new System.Windows.Forms.Label();
            this.lblTwitter = new System.Windows.Forms.Label();
            this.lblEmail = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.llbGithub = new System.Windows.Forms.LinkLabel();
            this.tlpMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblHintText
            // 
            this.lblHintText.AutoSize = true;
            this.tlpMain.SetColumnSpan(this.lblHintText, 2);
            this.lblHintText.Location = new System.Drawing.Point(10, 10);
            this.lblHintText.Margin = new System.Windows.Forms.Padding(10);
            this.lblHintText.Name = "lblHintText";
            this.lblHintText.Size = new System.Drawing.Size(480, 48);
            this.lblHintText.TabIndex = 0;
            this.lblHintText.Text = "BlocklyAts is currently in Beta.\r\nPlease report any bug you encountered to the de" +
    "veloper, so that we can fix them.";
            // 
            // tlpMain
            // 
            this.tlpMain.AutoSize = true;
            this.tlpMain.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tlpMain.ColumnCount = 2;
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMain.Controls.Add(this.llbEmail, 1, 3);
            this.tlpMain.Controls.Add(this.llbTwitter, 1, 2);
            this.tlpMain.Controls.Add(this.lblHintText, 0, 0);
            this.tlpMain.Controls.Add(this.lblGithub, 0, 1);
            this.tlpMain.Controls.Add(this.lblTwitter, 0, 2);
            this.tlpMain.Controls.Add(this.lblEmail, 0, 3);
            this.tlpMain.Controls.Add(this.btnOK, 1, 4);
            this.tlpMain.Controls.Add(this.llbGithub, 1, 1);
            this.tlpMain.Location = new System.Drawing.Point(12, 12);
            this.tlpMain.MaximumSize = new System.Drawing.Size(500, 0);
            this.tlpMain.MinimumSize = new System.Drawing.Size(500, 0);
            this.tlpMain.Name = "tlpMain";
            this.tlpMain.RowCount = 5;
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMain.Size = new System.Drawing.Size(500, 235);
            this.tlpMain.TabIndex = 1;
            // 
            // llbEmail
            // 
            this.llbEmail.AutoSize = true;
            this.llbEmail.LinkArea = new System.Windows.Forms.LinkArea(0, 19);
            this.llbEmail.Location = new System.Drawing.Point(94, 158);
            this.llbEmail.Margin = new System.Windows.Forms.Padding(10);
            this.llbEmail.Name = "llbEmail";
            this.llbEmail.Size = new System.Drawing.Size(270, 24);
            this.llbEmail.TabIndex = 7;
            this.llbEmail.TabStop = true;
            this.llbEmail.Text = "feedback@zbx1425.cn (As of 2021)";
            this.llbEmail.UseCompatibleTextRendering = true;
            this.llbEmail.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llbEmail_LinkClicked);
            // 
            // llbTwitter
            // 
            this.llbTwitter.AutoSize = true;
            this.llbTwitter.LinkArea = new System.Windows.Forms.LinkArea(0, 9);
            this.llbTwitter.Location = new System.Drawing.Point(94, 114);
            this.llbTwitter.Margin = new System.Windows.Forms.Padding(10);
            this.llbTwitter.Name = "llbTwitter";
            this.llbTwitter.Size = new System.Drawing.Size(187, 24);
            this.llbTwitter.TabIndex = 6;
            this.llbTwitter.TabStop = true;
            this.llbTwitter.Text = "@zhang_bx (As of 2021)";
            this.llbTwitter.UseCompatibleTextRendering = true;
            this.llbTwitter.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llbTwitter_LinkClicked);
            // 
            // lblGithub
            // 
            this.lblGithub.AutoSize = true;
            this.lblGithub.Location = new System.Drawing.Point(10, 78);
            this.lblGithub.Margin = new System.Windows.Forms.Padding(10);
            this.lblGithub.Name = "lblGithub";
            this.lblGithub.Size = new System.Drawing.Size(56, 16);
            this.lblGithub.TabIndex = 1;
            this.lblGithub.Text = "Github";
            // 
            // lblTwitter
            // 
            this.lblTwitter.AutoSize = true;
            this.lblTwitter.Location = new System.Drawing.Point(10, 114);
            this.lblTwitter.Margin = new System.Windows.Forms.Padding(10);
            this.lblTwitter.Name = "lblTwitter";
            this.lblTwitter.Size = new System.Drawing.Size(64, 16);
            this.lblTwitter.TabIndex = 2;
            this.lblTwitter.Text = "Twitter\r\n";
            // 
            // lblEmail
            // 
            this.lblEmail.AutoSize = true;
            this.lblEmail.Location = new System.Drawing.Point(10, 158);
            this.lblEmail.Margin = new System.Windows.Forms.Padding(10);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(48, 16);
            this.lblEmail.TabIndex = 3;
            this.lblEmail.Text = "Email";
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(415, 202);
            this.btnOK.Margin = new System.Windows.Forms.Padding(10);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // llbGithub
            // 
            this.llbGithub.AutoSize = true;
            this.llbGithub.Location = new System.Drawing.Point(94, 78);
            this.llbGithub.Margin = new System.Windows.Forms.Padding(10);
            this.llbGithub.Name = "llbGithub";
            this.llbGithub.Size = new System.Drawing.Size(240, 16);
            this.llbGithub.TabIndex = 5;
            this.llbGithub.TabStop = true;
            this.llbGithub.Text = "github.com/zbx1425/BlocklyAts";
            this.llbGithub.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llbGithub_LinkClicked);
            // 
            // FormBugReport
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(522, 450);
            this.Controls.Add(this.tlpMain);
            this.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormBugReport";
            this.Padding = new System.Windows.Forms.Padding(12);
            this.Text = "Bug Report";
            this.Load += new System.EventHandler(this.FormBugReport_Load);
            this.tlpMain.ResumeLayout(false);
            this.tlpMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblHintText;
        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private System.Windows.Forms.Label lblGithub;
        private System.Windows.Forms.Label lblTwitter;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.LinkLabel llbEmail;
        private System.Windows.Forms.LinkLabel llbTwitter;
        private System.Windows.Forms.LinkLabel llbGithub;
    }
}