namespace BlocklyAts.UserInterface {
    partial class FormDebug {
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
            this.flpMain = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.rbBve456 = new System.Windows.Forms.RadioButton();
            this.rbOpenBve = new System.Windows.Forms.RadioButton();
            this.flpMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // flpMain
            // 
            this.flpMain.AutoSize = true;
            this.flpMain.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpMain.Controls.Add(this.rbBve456);
            this.flpMain.Controls.Add(this.rbOpenBve);
            this.flpMain.Dock = System.Windows.Forms.DockStyle.Top;
            this.flpMain.Location = new System.Drawing.Point(0, 0);
            this.flpMain.Name = "flpMain";
            this.flpMain.Padding = new System.Windows.Forms.Padding(10);
            this.flpMain.Size = new System.Drawing.Size(784, 42);
            this.flpMain.TabIndex = 2;
            // 
            // pnlMain
            // 
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 42);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(784, 419);
            this.pnlMain.TabIndex = 3;
            // 
            // rbBve456
            // 
            this.rbBve456.AutoSize = true;
            this.rbBve456.Checked = true;
            this.rbBve456.Location = new System.Drawing.Point(13, 13);
            this.rbBve456.Name = "rbBve456";
            this.rbBve456.Size = new System.Drawing.Size(77, 16);
            this.rbBve456.TabIndex = 0;
            this.rbBve456.TabStop = true;
            this.rbBve456.Text = "BVE 4/5/6";
            this.rbBve456.UseVisualStyleBackColor = true;
            this.rbBve456.CheckedChanged += new System.EventHandler(this.rbCheckedChanged);
            // 
            // rbOpenBve
            // 
            this.rbOpenBve.AutoSize = true;
            this.rbOpenBve.Location = new System.Drawing.Point(96, 13);
            this.rbOpenBve.Name = "rbOpenBve";
            this.rbOpenBve.Size = new System.Drawing.Size(65, 16);
            this.rbOpenBve.TabIndex = 1;
            this.rbOpenBve.TabStop = true;
            this.rbOpenBve.Text = "OpenBVE";
            this.rbOpenBve.UseVisualStyleBackColor = true;
            this.rbOpenBve.CheckedChanged += new System.EventHandler(this.rbCheckedChanged);
            // 
            // FormDebug
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 461);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.flpMain);
            this.Name = "FormDebug";
            this.Text = "BlocklyAts Code Generator Debug Information";
            this.flpMain.ResumeLayout(false);
            this.flpMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.FlowLayoutPanel flpMain;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.RadioButton rbBve456;
        private System.Windows.Forms.RadioButton rbOpenBve;
    }
}