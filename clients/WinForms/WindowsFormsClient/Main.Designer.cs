namespace WindowsFormsClient
{
    partial class Main
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblTemp = new System.Windows.Forms.ToolStripStatusLabel();
            this.btnOptions = new System.Windows.Forms.ToolStripDropDownButton();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ffToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.AboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.timerPoll = new System.Windows.Forms.Timer(this.components);
            this.trayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.btnRunProgram = new System.Windows.Forms.Button();
            this.groupBoxOptions = new System.Windows.Forms.GroupBox();
            this.nUDWashingSpeed = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.cbWashingSpeed = new System.Windows.Forms.CheckBox();
            this.trackBarWashingSpeed = new System.Windows.Forms.TrackBar();
            this.cbWaterLevel = new System.Windows.Forms.CheckBox();
            this.cbRinsingCycles = new System.Windows.Forms.CheckBox();
            this.cbSpinningSpeed = new System.Windows.Forms.CheckBox();
            this.cbDuration = new System.Windows.Forms.CheckBox();
            this.cbTemperature = new System.Windows.Forms.CheckBox();
            this.nUDWaterLevel = new System.Windows.Forms.NumericUpDown();
            this.nUDRinsingCycles = new System.Windows.Forms.NumericUpDown();
            this.nUDSpinningSpeed = new System.Windows.Forms.NumericUpDown();
            this.nUDDuration = new System.Windows.Forms.NumericUpDown();
            this.nUDTemperature = new System.Windows.Forms.NumericUpDown();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.trackBarWaterLevel = new System.Windows.Forms.TrackBar();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.trackBarRinsingCycles = new System.Windows.Forms.TrackBar();
            this.trackBarSpinningSpeed = new System.Windows.Forms.TrackBar();
            this.trackBarDuration = new System.Windows.Forms.TrackBar();
            this.trackBarTemperature = new System.Windows.Forms.TrackBar();
            this.listBoxPrograms = new System.Windows.Forms.ListBox();
            this.statusStrip1.SuspendLayout();
            this.groupBoxOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nUDWashingSpeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarWashingSpeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nUDWaterLevel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nUDRinsingCycles)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nUDSpinningSpeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nUDDuration)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nUDTemperature)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarWaterLevel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarRinsingCycles)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarSpinningSpeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarDuration)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarTemperature)).BeginInit();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus,
            this.lblTemp,
            this.btnOptions});
            this.statusStrip1.Location = new System.Drawing.Point(0, 344);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(635, 24);
            this.statusStrip1.TabIndex = 4;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = false;
            this.lblStatus.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)));
            this.lblStatus.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(530, 19);
            this.lblStatus.Text = "Test text";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTemp
            // 
            this.lblTemp.AutoSize = false;
            this.lblTemp.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblTemp.Name = "lblTemp";
            this.lblTemp.Size = new System.Drawing.Size(51, 19);
            this.lblTemp.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnOptions
            // 
            this.btnOptions.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btnOptions.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnOptions.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingsToolStripMenuItem,
            this.ffToolStripMenuItem,
            this.logToolStripMenuItem,
            this.toolStripSeparator1,
            this.AboutToolStripMenuItem});
            this.btnOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnOptions.Image")));
            this.btnOptions.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnOptions.Name = "btnOptions";
            this.btnOptions.Size = new System.Drawing.Size(29, 22);
            this.btnOptions.Text = "toolStripDropDownButton1";
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.settingsToolStripMenuItem.Text = "Settings";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // ffToolStripMenuItem
            // 
            this.ffToolStripMenuItem.Name = "ffToolStripMenuItem";
            this.ffToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.ffToolStripMenuItem.Text = "Firmware update";
            // 
            // logToolStripMenuItem
            // 
            this.logToolStripMenuItem.Name = "logToolStripMenuItem";
            this.logToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.logToolStripMenuItem.Text = "Log";
            this.logToolStripMenuItem.Click += new System.EventHandler(this.LogToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(160, 6);
            // 
            // AboutToolStripMenuItem
            // 
            this.AboutToolStripMenuItem.Name = "AboutToolStripMenuItem";
            this.AboutToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.AboutToolStripMenuItem.Text = "About";
            // 
            // timerPoll
            // 
            this.timerPoll.Interval = 1000;
            this.timerPoll.Tick += new System.EventHandler(this.timerPoll_Tick);
            // 
            // trayIcon
            // 
            this.trayIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("trayIcon.Icon")));
            this.trayIcon.DoubleClick += new System.EventHandler(this.TrayIcon_DoubleClick);
            // 
            // btnRunProgram
            // 
            this.btnRunProgram.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnRunProgram.Enabled = false;
            this.btnRunProgram.Location = new System.Drawing.Point(12, 295);
            this.btnRunProgram.Name = "btnRunProgram";
            this.btnRunProgram.Size = new System.Drawing.Size(179, 40);
            this.btnRunProgram.TabIndex = 13;
            this.btnRunProgram.UseVisualStyleBackColor = true;
            this.btnRunProgram.Click += new System.EventHandler(this.BtnRunProgram_Click);
            // 
            // groupBoxOptions
            // 
            this.groupBoxOptions.Controls.Add(this.nUDWashingSpeed);
            this.groupBoxOptions.Controls.Add(this.label2);
            this.groupBoxOptions.Controls.Add(this.cbWashingSpeed);
            this.groupBoxOptions.Controls.Add(this.trackBarWashingSpeed);
            this.groupBoxOptions.Controls.Add(this.cbWaterLevel);
            this.groupBoxOptions.Controls.Add(this.cbRinsingCycles);
            this.groupBoxOptions.Controls.Add(this.cbSpinningSpeed);
            this.groupBoxOptions.Controls.Add(this.cbDuration);
            this.groupBoxOptions.Controls.Add(this.cbTemperature);
            this.groupBoxOptions.Controls.Add(this.nUDWaterLevel);
            this.groupBoxOptions.Controls.Add(this.nUDRinsingCycles);
            this.groupBoxOptions.Controls.Add(this.nUDSpinningSpeed);
            this.groupBoxOptions.Controls.Add(this.nUDDuration);
            this.groupBoxOptions.Controls.Add(this.nUDTemperature);
            this.groupBoxOptions.Controls.Add(this.label13);
            this.groupBoxOptions.Controls.Add(this.label14);
            this.groupBoxOptions.Controls.Add(this.trackBarWaterLevel);
            this.groupBoxOptions.Controls.Add(this.label11);
            this.groupBoxOptions.Controls.Add(this.label12);
            this.groupBoxOptions.Controls.Add(this.label9);
            this.groupBoxOptions.Controls.Add(this.label10);
            this.groupBoxOptions.Controls.Add(this.label7);
            this.groupBoxOptions.Controls.Add(this.label8);
            this.groupBoxOptions.Controls.Add(this.label6);
            this.groupBoxOptions.Controls.Add(this.label1);
            this.groupBoxOptions.Controls.Add(this.trackBarRinsingCycles);
            this.groupBoxOptions.Controls.Add(this.trackBarSpinningSpeed);
            this.groupBoxOptions.Controls.Add(this.trackBarDuration);
            this.groupBoxOptions.Controls.Add(this.trackBarTemperature);
            this.groupBoxOptions.Enabled = false;
            this.groupBoxOptions.Location = new System.Drawing.Point(197, 12);
            this.groupBoxOptions.Name = "groupBoxOptions";
            this.groupBoxOptions.Size = new System.Drawing.Size(427, 323);
            this.groupBoxOptions.TabIndex = 15;
            this.groupBoxOptions.TabStop = false;
            // 
            // nUDWashingSpeed
            // 
            this.nUDWashingSpeed.Location = new System.Drawing.Point(258, 69);
            this.nUDWashingSpeed.Name = "nUDWashingSpeed";
            this.nUDWashingSpeed.Size = new System.Drawing.Size(120, 20);
            this.nUDWashingSpeed.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(400, 148);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(19, 13);
            this.label2.TabIndex = 37;
            this.label2.Text = "10";
            // 
            // cbWashingSpeed
            // 
            this.cbWashingSpeed.AutoSize = true;
            this.cbWashingSpeed.Location = new System.Drawing.Point(6, 117);
            this.cbWashingSpeed.Name = "cbWashingSpeed";
            this.cbWashingSpeed.Size = new System.Drawing.Size(128, 17);
            this.cbWashingSpeed.TabIndex = 36;
            this.cbWashingSpeed.Text = "Washing speed, RPS";
            this.cbWashingSpeed.UseVisualStyleBackColor = true;
            // 
            // trackBarWashingSpeed
            // 
            this.trackBarWashingSpeed.Location = new System.Drawing.Point(258, 135);
            this.trackBarWashingSpeed.Name = "trackBarWashingSpeed";
            this.trackBarWashingSpeed.Size = new System.Drawing.Size(104, 45);
            this.trackBarWashingSpeed.TabIndex = 38;
            // 
            // cbWaterLevel
            // 
            this.cbWaterLevel.AutoSize = true;
            this.cbWaterLevel.Location = new System.Drawing.Point(6, 264);
            this.cbWaterLevel.Name = "cbWaterLevel";
            this.cbWaterLevel.Size = new System.Drawing.Size(94, 17);
            this.cbWaterLevel.TabIndex = 33;
            this.cbWaterLevel.Text = "Water level, %";
            this.cbWaterLevel.UseVisualStyleBackColor = true;
            // 
            // cbRinsingCycles
            // 
            this.cbRinsingCycles.AutoSize = true;
            this.cbRinsingCycles.Location = new System.Drawing.Point(6, 213);
            this.cbRinsingCycles.Name = "cbRinsingCycles";
            this.cbRinsingCycles.Size = new System.Drawing.Size(94, 17);
            this.cbRinsingCycles.TabIndex = 32;
            this.cbRinsingCycles.Text = "Rinsing cycles";
            this.cbRinsingCycles.UseVisualStyleBackColor = true;
            // 
            // cbSpinningSpeed
            // 
            this.cbSpinningSpeed.AutoSize = true;
            this.cbSpinningSpeed.Location = new System.Drawing.Point(6, 163);
            this.cbSpinningSpeed.Name = "cbSpinningSpeed";
            this.cbSpinningSpeed.Size = new System.Drawing.Size(127, 17);
            this.cbSpinningSpeed.TabIndex = 31;
            this.cbSpinningSpeed.Text = "Spinning speed, RPS";
            this.cbSpinningSpeed.UseVisualStyleBackColor = true;
            // 
            // cbDuration
            // 
            this.cbDuration.AutoSize = true;
            this.cbDuration.Location = new System.Drawing.Point(6, 70);
            this.cbDuration.Name = "cbDuration";
            this.cbDuration.Size = new System.Drawing.Size(88, 17);
            this.cbDuration.TabIndex = 30;
            this.cbDuration.Text = "Duration, min";
            this.cbDuration.UseVisualStyleBackColor = true;
            // 
            // cbTemperature
            // 
            this.cbTemperature.AutoSize = true;
            this.cbTemperature.Location = new System.Drawing.Point(6, 28);
            this.cbTemperature.Name = "cbTemperature";
            this.cbTemperature.Size = new System.Drawing.Size(103, 17);
            this.cbTemperature.TabIndex = 29;
            this.cbTemperature.Text = "Temperature, °C";
            this.cbTemperature.UseVisualStyleBackColor = true;
            // 
            // nUDWaterLevel
            // 
            this.nUDWaterLevel.Location = new System.Drawing.Point(155, 46);
            this.nUDWaterLevel.Name = "nUDWaterLevel";
            this.nUDWaterLevel.Size = new System.Drawing.Size(120, 20);
            this.nUDWaterLevel.TabIndex = 39;
            // 
            // nUDRinsingCycles
            // 
            this.nUDRinsingCycles.Location = new System.Drawing.Point(51, 19);
            this.nUDRinsingCycles.Name = "nUDRinsingCycles";
            this.nUDRinsingCycles.Size = new System.Drawing.Size(120, 20);
            this.nUDRinsingCycles.TabIndex = 40;
            // 
            // nUDSpinningSpeed
            // 
            this.nUDSpinningSpeed.Location = new System.Drawing.Point(0, 0);
            this.nUDSpinningSpeed.Name = "nUDSpinningSpeed";
            this.nUDSpinningSpeed.Size = new System.Drawing.Size(120, 20);
            this.nUDSpinningSpeed.TabIndex = 41;
            // 
            // nUDDuration
            // 
            this.nUDDuration.Location = new System.Drawing.Point(0, 0);
            this.nUDDuration.Name = "nUDDuration";
            this.nUDDuration.Size = new System.Drawing.Size(120, 20);
            this.nUDDuration.TabIndex = 42;
            // 
            // nUDTemperature
            // 
            this.nUDTemperature.Location = new System.Drawing.Point(0, 0);
            this.nUDTemperature.Name = "nUDTemperature";
            this.nUDTemperature.Size = new System.Drawing.Size(120, 20);
            this.nUDTemperature.TabIndex = 43;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(400, 295);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(25, 13);
            this.label13.TabIndex = 23;
            this.label13.Text = "100";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(184, 295);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(13, 13);
            this.label14.TabIndex = 22;
            this.label14.Text = "0";
            // 
            // trackBarWaterLevel
            // 
            this.trackBarWaterLevel.Location = new System.Drawing.Point(0, 0);
            this.trackBarWaterLevel.Name = "trackBarWaterLevel";
            this.trackBarWaterLevel.Size = new System.Drawing.Size(104, 45);
            this.trackBarWaterLevel.TabIndex = 44;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(406, 244);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(13, 13);
            this.label11.TabIndex = 20;
            this.label11.Text = "5";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(184, 244);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(13, 13);
            this.label12.TabIndex = 19;
            this.label12.Text = "0";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(400, 193);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(19, 13);
            this.label9.TabIndex = 18;
            this.label9.Text = "20";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(184, 193);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(13, 13);
            this.label10.TabIndex = 17;
            this.label10.Text = "0";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(400, 100);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(25, 13);
            this.label7.TabIndex = 16;
            this.label7.Text = "180";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(184, 100);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(19, 13);
            this.label8.TabIndex = 15;
            this.label8.Text = "15";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(406, 53);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(19, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "80";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(184, 53);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(19, 13);
            this.label1.TabIndex = 13;
            this.label1.Text = "10";
            // 
            // trackBarRinsingCycles
            // 
            this.trackBarRinsingCycles.Location = new System.Drawing.Point(0, 0);
            this.trackBarRinsingCycles.Name = "trackBarRinsingCycles";
            this.trackBarRinsingCycles.Size = new System.Drawing.Size(104, 45);
            this.trackBarRinsingCycles.TabIndex = 45;
            // 
            // trackBarSpinningSpeed
            // 
            this.trackBarSpinningSpeed.Location = new System.Drawing.Point(0, 0);
            this.trackBarSpinningSpeed.Name = "trackBarSpinningSpeed";
            this.trackBarSpinningSpeed.Size = new System.Drawing.Size(104, 45);
            this.trackBarSpinningSpeed.TabIndex = 46;
            // 
            // trackBarDuration
            // 
            this.trackBarDuration.Location = new System.Drawing.Point(0, 0);
            this.trackBarDuration.Name = "trackBarDuration";
            this.trackBarDuration.Size = new System.Drawing.Size(104, 45);
            this.trackBarDuration.TabIndex = 47;
            // 
            // trackBarTemperature
            // 
            this.trackBarTemperature.Location = new System.Drawing.Point(0, 0);
            this.trackBarTemperature.Name = "trackBarTemperature";
            this.trackBarTemperature.Size = new System.Drawing.Size(104, 45);
            this.trackBarTemperature.TabIndex = 48;
            // 
            // listBoxPrograms
            // 
            this.listBoxPrograms.FormattingEnabled = true;
            this.listBoxPrograms.Location = new System.Drawing.Point(12, 12);
            this.listBoxPrograms.Name = "listBoxPrograms";
            this.listBoxPrograms.Size = new System.Drawing.Size(179, 277);
            this.listBoxPrograms.TabIndex = 16;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(635, 368);
            this.Controls.Add(this.listBoxPrograms);
            this.Controls.Add(this.groupBoxOptions);
            this.Controls.Add(this.btnRunProgram);
            this.Controls.Add(this.statusStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "Main";
            this.Text = "OpenWasher";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Main_FormClosed);
            this.Load += new System.EventHandler(this.Main_Load);
            this.Resize += new System.EventHandler(this.Main_Resize);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.groupBoxOptions.ResumeLayout(false);
            this.groupBoxOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nUDWashingSpeed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarWashingSpeed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nUDWaterLevel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nUDRinsingCycles)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nUDSpinningSpeed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nUDDuration)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nUDTemperature)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarWaterLevel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarRinsingCycles)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarSpinningSpeed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarDuration)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarTemperature)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.ToolStripDropDownButton btnOptions;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.Timer timerPoll;
        private System.Windows.Forms.ToolStripMenuItem ffToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem logToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel lblTemp;
        private System.Windows.Forms.NotifyIcon trayIcon;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem AboutToolStripMenuItem;
        private System.Windows.Forms.Button btnRunProgram;
        private System.Windows.Forms.GroupBox groupBoxOptions;
        private System.Windows.Forms.NumericUpDown nUDWashingSpeed;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox cbWashingSpeed;
        private System.Windows.Forms.TrackBar trackBarWashingSpeed;
        private System.Windows.Forms.CheckBox cbWaterLevel;
        private System.Windows.Forms.CheckBox cbRinsingCycles;
        private System.Windows.Forms.CheckBox cbSpinningSpeed;
        private System.Windows.Forms.CheckBox cbDuration;
        private System.Windows.Forms.CheckBox cbTemperature;
        private System.Windows.Forms.NumericUpDown nUDWaterLevel;
        private System.Windows.Forms.NumericUpDown nUDRinsingCycles;
        private System.Windows.Forms.NumericUpDown nUDSpinningSpeed;
        private System.Windows.Forms.NumericUpDown nUDDuration;
        private System.Windows.Forms.NumericUpDown nUDTemperature;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TrackBar trackBarWaterLevel;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TrackBar trackBarRinsingCycles;
        private System.Windows.Forms.TrackBar trackBarSpinningSpeed;
        private System.Windows.Forms.TrackBar trackBarDuration;
        private System.Windows.Forms.TrackBar trackBarTemperature;
        private System.Windows.Forms.ListBox listBoxPrograms;
    }
}

