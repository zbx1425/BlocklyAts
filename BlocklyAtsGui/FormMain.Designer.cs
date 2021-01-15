namespace BlocklyATS {
    partial class FormMain {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.mainToolStrip = new System.Windows.Forms.ToolStrip();
            this.tsbtnNew = new System.Windows.Forms.ToolStripButton();
            this.tsbtnOpen = new System.Windows.Forms.ToolStripButton();
            this.tsbtnSaveAs = new System.Windows.Forms.ToolStripButton();
            this.tss1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbtnDebugWindow = new System.Windows.Forms.ToolStripButton();
            this.tsbtnCompile = new System.Windows.Forms.ToolStripButton();
            this.tsbtnCompileRun = new System.Windows.Forms.ToolStripButton();
            this.tsbtnCompileSetting = new System.Windows.Forms.ToolStripButton();
            this.tsbtnSave = new System.Windows.Forms.ToolStripButton();
            this.mainToolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainToolStrip
            // 
            this.mainToolStrip.GripMargin = new System.Windows.Forms.Padding(0);
            this.mainToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.mainToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbtnNew,
            this.tsbtnOpen,
            this.tsbtnSave,
            this.tsbtnSaveAs,
            this.tss1,
            this.tsbtnCompile,
            this.tsbtnCompileRun,
            this.tsbtnCompileSetting,
            this.tsbtnDebugWindow});
            this.mainToolStrip.Location = new System.Drawing.Point(0, 0);
            this.mainToolStrip.Name = "mainToolStrip";
            this.mainToolStrip.Size = new System.Drawing.Size(784, 25);
            this.mainToolStrip.TabIndex = 0;
            this.mainToolStrip.Text = "toolStrip1";
            // 
            // tsbtnNew
            // 
            this.tsbtnNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnNew.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnNew.Image")));
            this.tsbtnNew.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbtnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnNew.Margin = new System.Windows.Forms.Padding(5, 1, 0, 2);
            this.tsbtnNew.Name = "tsbtnNew";
            this.tsbtnNew.Size = new System.Drawing.Size(23, 22);
            this.tsbtnNew.Text = "New";
            this.tsbtnNew.Click += new System.EventHandler(this.tsbtnNew_Click);
            // 
            // tsbtnOpen
            // 
            this.tsbtnOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnOpen.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnOpen.Image")));
            this.tsbtnOpen.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbtnOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnOpen.Name = "tsbtnOpen";
            this.tsbtnOpen.Size = new System.Drawing.Size(23, 22);
            this.tsbtnOpen.Text = "Open";
            this.tsbtnOpen.Click += new System.EventHandler(this.tsbtnOpen_Click);
            // 
            // tsbtnSaveAs
            // 
            this.tsbtnSaveAs.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnSaveAs.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnSaveAs.Image")));
            this.tsbtnSaveAs.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbtnSaveAs.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnSaveAs.Name = "tsbtnSaveAs";
            this.tsbtnSaveAs.Size = new System.Drawing.Size(23, 22);
            this.tsbtnSaveAs.Text = "Save As";
            this.tsbtnSaveAs.Click += new System.EventHandler(this.tsbtnSaveAs_Click);
            // 
            // tss1
            // 
            this.tss1.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.tss1.Name = "tss1";
            this.tss1.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbtnDebugWindow
            // 
            this.tsbtnDebugWindow.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsbtnDebugWindow.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnDebugWindow.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnDebugWindow.Image")));
            this.tsbtnDebugWindow.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnDebugWindow.Margin = new System.Windows.Forms.Padding(0, 1, 10, 2);
            this.tsbtnDebugWindow.Name = "tsbtnDebugWindow";
            this.tsbtnDebugWindow.Size = new System.Drawing.Size(23, 22);
            this.tsbtnDebugWindow.Text = "Debug Window";
            // 
            // tsbtnCompile
            // 
            this.tsbtnCompile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnCompile.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnCompile.Image")));
            this.tsbtnCompile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnCompile.Name = "tsbtnCompile";
            this.tsbtnCompile.Size = new System.Drawing.Size(23, 22);
            this.tsbtnCompile.Text = "Compile";
            this.tsbtnCompile.Click += new System.EventHandler(this.tsbtnCompile_Click);
            // 
            // tsbtnCompileRun
            // 
            this.tsbtnCompileRun.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnCompileRun.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnCompileRun.Image")));
            this.tsbtnCompileRun.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnCompileRun.Name = "tsbtnCompileRun";
            this.tsbtnCompileRun.Size = new System.Drawing.Size(23, 22);
            this.tsbtnCompileRun.Text = "Compile and Run";
            // 
            // tsbtnCompileSetting
            // 
            this.tsbtnCompileSetting.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnCompileSetting.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnCompileSetting.Image")));
            this.tsbtnCompileSetting.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnCompileSetting.Name = "tsbtnCompileSetting";
            this.tsbtnCompileSetting.Size = new System.Drawing.Size(23, 22);
            this.tsbtnCompileSetting.Text = "Compile Settings";
            // 
            // tsbtnSave
            // 
            this.tsbtnSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnSave.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnSave.Image")));
            this.tsbtnSave.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbtnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnSave.Name = "tsbtnSave";
            this.tsbtnSave.Size = new System.Drawing.Size(23, 22);
            this.tsbtnSave.Text = "Save";
            this.tsbtnSave.Click += new System.EventHandler(this.tsbtnSave_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 441);
            this.Controls.Add(this.mainToolStrip);
            this.MinimumSize = new System.Drawing.Size(800, 480);
            this.Name = "FormMain";
            this.Text = "BlocklyATS";
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.mainWebBrowser_PreviewKeyDown);
            this.mainToolStrip.ResumeLayout(false);
            this.mainToolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip mainToolStrip;
        private System.Windows.Forms.ToolStripButton tsbtnNew;
        private System.Windows.Forms.ToolStripButton tsbtnOpen;
        private System.Windows.Forms.ToolStripButton tsbtnSaveAs;
        private System.Windows.Forms.ToolStripSeparator tss1;
        private System.Windows.Forms.ToolStripButton tsbtnDebugWindow;
        private System.Windows.Forms.ToolStripButton tsbtnCompile;
        private System.Windows.Forms.ToolStripButton tsbtnCompileRun;
        private System.Windows.Forms.ToolStripButton tsbtnCompileSetting;
        private System.Windows.Forms.ToolStripButton tsbtnSave;
    }
}

