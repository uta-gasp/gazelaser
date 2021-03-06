﻿namespace GazeLaser
{
    partial class PointerWidget
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
            this.pcbImage = new System.Windows.Forms.PictureBox();
            this.tmrPositionInspector = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pcbImage)).BeginInit();
            this.SuspendLayout();
            // 
            // pcbImage
            // 
            this.pcbImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pcbImage.Location = new System.Drawing.Point(0, 0);
            this.pcbImage.Margin = new System.Windows.Forms.Padding(0);
            this.pcbImage.Name = "pcbImage";
            this.pcbImage.Size = new System.Drawing.Size(100, 100);
            this.pcbImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pcbImage.TabIndex = 0;
            this.pcbImage.TabStop = false;
            // 
            // tmrPositionInspector
            // 
            this.tmrPositionInspector.Enabled = true;
            this.tmrPositionInspector.Interval = 500;
            this.tmrPositionInspector.Tick += new System.EventHandler(this.tmrPositionInspector_Tick);
            // 
            // PointerWidget
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(100, 100);
            this.Controls.Add(this.pcbImage);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "PointerWidget";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Pointer";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.pcbImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pcbImage;
        private System.Windows.Forms.Timer tmrPositionInspector;
    }
}