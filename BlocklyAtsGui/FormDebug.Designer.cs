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
            this.tbCode = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // rbLua
            // 
            this.rbLua.AutoSize = true;
            this.rbLua.Checked = true;
            this.rbLua.Location = new System.Drawing.Point(13, 13);
            this.rbLua.Name = "rbLua";
            this.rbLua.Size = new System.Drawing.Size(41, 16);
            this.rbLua.TabIndex = 0;
            this.rbLua.TabStop = true;
            this.rbLua.Text = "Lua";
            this.rbLua.UseVisualStyleBackColor = true;
            this.rbLua.CheckedChanged += new System.EventHandler(this.rbLua_CheckedChanged);
            // 
            // rbCSharp
            // 
            this.rbCSharp.AutoSize = true;
            this.rbCSharp.Location = new System.Drawing.Point(109, 13);
            this.rbCSharp.Name = "rbCSharp";
            this.rbCSharp.Size = new System.Drawing.Size(59, 16);
            this.rbCSharp.TabIndex = 1;
            this.rbCSharp.Text = "CSharp";
            this.rbCSharp.UseVisualStyleBackColor = true;
            this.rbCSharp.CheckedChanged += new System.EventHandler(this.rbLua_CheckedChanged);
            // 
            // tbCode
            // 
            this.tbCode.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbCode.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbCode.Location = new System.Drawing.Point(13, 36);
            this.tbCode.Multiline = true;
            this.tbCode.Name = "tbCode";
            this.tbCode.ReadOnly = true;
            this.tbCode.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbCode.Size = new System.Drawing.Size(759, 413);
            this.tbCode.TabIndex = 2;
            this.tbCode.WordWrap = false;
            // 
            // FormDebug
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 461);
            this.Controls.Add(this.tbCode);
            this.Controls.Add(this.rbCSharp);
            this.Controls.Add(this.rbLua);
            this.Name = "FormDebug";
            this.Text = "BlocklyAts Code Generator Debug Information";
            this.Load += new System.EventHandler(this.FormDebug_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton rbLua;
        private System.Windows.Forms.RadioButton rbCSharp;
        private System.Windows.Forms.TextBox tbCode;
    }
}