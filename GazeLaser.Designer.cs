namespace GazeLaser
{
    partial class GazeLaser
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GazeLaser));
            this.ntiTrayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.cmsMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmOptions = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmCalibrate = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmToggleTracking = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmExit = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // ntiTrayIcon
            // 
            this.ntiTrayIcon.ContextMenuStrip = this.cmsMenu;
            this.ntiTrayIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("ntiTrayIcon.Icon")));
            this.ntiTrayIcon.Text = "GazeLaser";
            this.ntiTrayIcon.Visible = true;
            // 
            // cmsMenu
            // 
            this.cmsMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmOptions,
            this.tsmCalibrate,
            this.tsmToggleTracking,
            this.toolStripMenuItem1,
            this.tsmExit});
            this.cmsMenu.Name = "cmsMenu";
            this.cmsMenu.Size = new System.Drawing.Size(122, 98);
            // 
            // tsmOptions
            // 
            this.tsmOptions.Name = "tsmOptions";
            this.tsmOptions.Size = new System.Drawing.Size(121, 22);
            this.tsmOptions.Text = "Options";
            this.tsmOptions.Click += new System.EventHandler(this.tsmOptions_Click);
            // 
            // tsmCalibrate
            // 
            this.tsmCalibrate.Name = "tsmCalibrate";
            this.tsmCalibrate.Size = new System.Drawing.Size(121, 22);
            this.tsmCalibrate.Text = "Calibrate";
            this.tsmCalibrate.Click += new System.EventHandler(this.tsmCalibrate_Click);
            // 
            // tsmToggleTracking
            // 
            this.tsmToggleTracking.Name = "tsmToggleTracking";
            this.tsmToggleTracking.Size = new System.Drawing.Size(121, 22);
            this.tsmToggleTracking.Text = "Start";
            this.tsmToggleTracking.Click += new System.EventHandler(this.tsmToggleTracking_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(118, 6);
            // 
            // tsmExit
            // 
            this.tsmExit.Name = "tsmExit";
            this.tsmExit.Size = new System.Drawing.Size(121, 22);
            this.tsmExit.Text = "Exit";
            this.tsmExit.Click += new System.EventHandler(this.tsmExit_Click);
            // 
            // GazeLaser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GazeLaser";
            this.Text = "GazeLaser Options";
            this.cmsMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NotifyIcon ntiTrayIcon;
        private System.Windows.Forms.ContextMenuStrip cmsMenu;
        private System.Windows.Forms.ToolStripMenuItem tsmOptions;
        private System.Windows.Forms.ToolStripMenuItem tsmCalibrate;
        private System.Windows.Forms.ToolStripMenuItem tsmToggleTracking;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem tsmExit;
    }
}

