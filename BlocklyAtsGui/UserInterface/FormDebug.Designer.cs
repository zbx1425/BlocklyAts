namespace BlocklyAts {
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
            this.rbLua = new System.Windows.Forms.RadioButton();
            this.rbCSharp = new System.Windows.Forms.RadioButton();
            this.flpMain = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.flpMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // rbLua
            // 
            this.rbLua.AutoSize = true;
            this.rbLua.Checked = true;
            this.rbLua.Location = new System.Drawing.Point(13, 13);
            this.rbLua.Name = "rbLua";
            this.rbLua.Size = new System.Drawing.Size(107, 16);
            this.rbLua.TabIndex = 0;
            this.rbLua.TabStop = true;
            this.rbLua.Text = "Lua (BVE4/5/6)";
            this.rbLua.UseVisualStyleBackColor = true;
            this.rbLua.CheckedChanged += new System.EventHandler(this.rbLua_CheckedChanged);
            // 
            // rbCSharp
            // 
            this.rbCSharp.AutoSize = true;
            this.rbCSharp.Location = new System.Drawing.Point(143, 13);
            this.rbCSharp.Margin = new System.Windows.Forms.Padding(20, 3, 3, 3);
            this.rbCSharp.Name = "rbCSharp";
            this.rbCSharp.Size = new System.Drawing.Size(119, 16);
            this.rbCSharp.TabIndex = 1;
            this.rbCSharp.Text = "CSharp (OpenBVE)";
            this.rbCSharp.UseVisualStyleBackColor = true;
            this.rbCSharp.CheckedChanged += new System.EventHandler(this.rbLua_CheckedChanged);
            // 
            // flpMain
            // 
            this.flpMain.AutoSize = true;
            this.flpMain.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpMain.Controls.Add(this.rbLua);
            this.flpMain.Controls.Add(this.rbCSharp);
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
            // FormDebug
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 461);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.flpMain);
            this.Name = "FormDebug";
            this.Text = "BlocklyAts Code Generator Debug Information";
            this.Load += new System.EventHandler(this.FormDebug_Load);
            this.flpMain.ResumeLayout(false);
            this.flpMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton rbLua;
        private System.Windows.Forms.RadioButton rbCSharp;
        private System.Windows.Forms.FlowLayoutPanel flpMain;
        private System.Windows.Forms.Panel pnlMain;
    }
}