namespace WindowsFormsClient
{
    partial class FirmwareFrm
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
            this.textBoxFilePath = new System.Windows.Forms.TextBox();
            this.buttonOpenFirmware = new System.Windows.Forms.Button();
            this.buttonInstallFirmware = new System.Windows.Forms.Button();
            this.firmwareOpenFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // textBoxFilePath
            // 
            this.textBoxFilePath.Location = new System.Drawing.Point(12, 12);
            this.textBoxFilePath.Name = "textBoxFilePath";
            this.textBoxFilePath.Size = new System.Drawing.Size(479, 20);
            this.textBoxFilePath.TabIndex = 0;
            // 
            // buttonOpenFirmware
            // 
            this.buttonOpenFirmware.Location = new System.Drawing.Point(497, 10);
            this.buttonOpenFirmware.Name = "buttonOpenFirmware";
            this.buttonOpenFirmware.Size = new System.Drawing.Size(75, 23);
            this.buttonOpenFirmware.TabIndex = 1;
            this.buttonOpenFirmware.Text = "Open";
            this.buttonOpenFirmware.UseVisualStyleBackColor = true;
            this.buttonOpenFirmware.Click += new System.EventHandler(this.ButtonOpenFirmware_Click);
            // 
            // buttonInstallFirmware
            // 
            this.buttonInstallFirmware.Location = new System.Drawing.Point(479, 58);
            this.buttonInstallFirmware.Name = "buttonInstallFirmware";
            this.buttonInstallFirmware.Size = new System.Drawing.Size(96, 23);
            this.buttonInstallFirmware.TabIndex = 2;
            this.buttonInstallFirmware.Text = "Install";
            this.buttonInstallFirmware.UseVisualStyleBackColor = true;
            this.buttonInstallFirmware.Click += new System.EventHandler(this.ButtonInstallFirmware_Click);
            // 
            // firmwareOpenFileDialog
            // 
            this.firmwareOpenFileDialog.FileName = "openFileDialog1";
            // 
            // FirmwareFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(587, 94);
            this.Controls.Add(this.buttonInstallFirmware);
            this.Controls.Add(this.buttonOpenFirmware);
            this.Controls.Add(this.textBoxFilePath);
            this.Name = "FirmwareFrm";
            this.Text = "Firmware update";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxFilePath;
        private System.Windows.Forms.Button buttonOpenFirmware;
        private System.Windows.Forms.Button buttonInstallFirmware;
        private System.Windows.Forms.OpenFileDialog firmwareOpenFileDialog;
    }
}