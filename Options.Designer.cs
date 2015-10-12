namespace GazeLaser
{
    partial class Options
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Options));
            this.label1 = new System.Windows.Forms.Label();
            this.cmbAppearance = new System.Windows.Forms.ComboBox();
            this.trbOpacity = new System.Windows.Forms.TrackBar();
            this.label2 = new System.Windows.Forms.Label();
            this.lblOpacity = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblSize = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.trbSize = new System.Windows.Forms.TrackBar();
            ((System.ComponentModel.ISupportInitialize)(this.trbOpacity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trbSize)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Appearance";
            // 
            // cmbAppearance
            // 
            this.cmbAppearance.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbAppearance.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAppearance.FormattingEnabled = true;
            this.cmbAppearance.Location = new System.Drawing.Point(151, 12);
            this.cmbAppearance.Name = "cmbAppearance";
            this.cmbAppearance.Size = new System.Drawing.Size(121, 21);
            this.cmbAppearance.TabIndex = 1;
            this.cmbAppearance.SelectedIndexChanged += new System.EventHandler(this.cmbAppearance_SelectedIndexChanged);
            // 
            // trbOpacity
            // 
            this.trbOpacity.Location = new System.Drawing.Point(151, 39);
            this.trbOpacity.Maximum = 9;
            this.trbOpacity.Minimum = 1;
            this.trbOpacity.Name = "trbOpacity";
            this.trbOpacity.Size = new System.Drawing.Size(121, 45);
            this.trbOpacity.TabIndex = 2;
            this.trbOpacity.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.trbOpacity.Value = 9;
            this.trbOpacity.ValueChanged += new System.EventHandler(this.trbOpacity_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Opacity";
            // 
            // lblOpacity
            // 
            this.lblOpacity.AutoSize = true;
            this.lblOpacity.Location = new System.Drawing.Point(132, 53);
            this.lblOpacity.Name = "lblOpacity";
            this.lblOpacity.Size = new System.Drawing.Size(13, 13);
            this.lblOpacity.TabIndex = 4;
            this.lblOpacity.Text = "0";
            this.lblOpacity.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(53, 153);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(135, 153);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblSize
            // 
            this.lblSize.AutoSize = true;
            this.lblSize.Location = new System.Drawing.Point(132, 104);
            this.lblSize.Name = "lblSize";
            this.lblSize.Size = new System.Drawing.Size(13, 13);
            this.lblSize.TabIndex = 9;
            this.lblSize.Text = "0";
            this.lblSize.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 104);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(27, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Size";
            // 
            // trbSize
            // 
            this.trbSize.Location = new System.Drawing.Point(151, 90);
            this.trbSize.Maximum = 30;
            this.trbSize.Minimum = 3;
            this.trbSize.Name = "trbSize";
            this.trbSize.Size = new System.Drawing.Size(121, 45);
            this.trbSize.TabIndex = 7;
            this.trbSize.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.trbSize.Value = 9;
            this.trbSize.ValueChanged += new System.EventHandler(this.trbSize_ValueChanged);
            // 
            // Options
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(284, 188);
            this.Controls.Add(this.lblSize);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.trbSize);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.lblOpacity);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.trbOpacity);
            this.Controls.Add(this.cmbAppearance);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Options";
            this.Text = "GazeLaser Options";
            ((System.ComponentModel.ISupportInitialize)(this.trbOpacity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trbSize)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbAppearance;
        private System.Windows.Forms.TrackBar trbOpacity;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblOpacity;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblSize;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TrackBar trbSize;

    }
}

