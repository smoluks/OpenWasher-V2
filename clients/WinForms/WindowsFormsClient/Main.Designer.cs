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
            this.FirmwareUpdateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.AboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.timerPoll = new System.Windows.Forms.Timer(this.components);
            this.trayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.btnRunProgram = new System.Windows.Forms.Button();
            this.groupBoxOptions = new System.Windows.Forms.GroupBox();
            this.labelWaterLevel = new System.Windows.Forms.Label();
            this.labelRinsingCycles = new System.Windows.Forms.Label();
            this.labelSpinningSpeed = new System.Windows.Forms.Label();
            this.labelWashingSpeed = new System.Windows.Forms.Label();
            this.labelDuration = new System.Windows.Forms.Label();
            this.labelTemperature = new System.Windows.Forms.Label();
            this.textBoxProgramDescription = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.nUDWashingSpeed = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.trackBarWashingSpeed = new System.Windows.Forms.TrackBar();
            this.nUDWaterLevel = new System.Windows.Forms.NumericUpDown();
            this.nUDRinsingCycles = new System.Windows.Forms.NumericUpDown();
            this.nUDSpinningSpeed = new System.Windows.Forms.NumericUpDown();
            this.nUDDuration = new System.Windows.Forms.NumericUpDown();
            this.nUDTemperature = new System.Windows.Forms.NumericUpDown();
            this.label13 = new System.Windows.Forms.Label();
            this.trackBarWaterLevel = new System.Windows.Forms.TrackBar();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
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
            this.statusStrip1.Location = new System.Drawing.Point(0, 376);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(647, 24);
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
            this.lblTemp.Image = global::OpenWasherClient.Properties.Resources.thermometer;
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
            this.FirmwareUpdateToolStripMenuItem,
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
            // FirmwareUpdateToolStripMenuItem
            // 
            this.FirmwareUpdateToolStripMenuItem.Name = "FirmwareUpdateToolStripMenuItem";
            this.FirmwareUpdateToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.FirmwareUpdateToolStripMenuItem.Text = "Firmware update";
            this.FirmwareUpdateToolStripMenuItem.Click += new System.EventHandler(this.FirmwareUpdateToolStripMenuItem_Click);
            // 
            // logToolStripMenuItem
            // 
            this.logToolStripMenuItem.Enabled = false;
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
            this.btnRunProgram.BackgroundImage = global::OpenWasherClient.Properties.Resources.start;
            this.btnRunProgram.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnRunProgram.Enabled = false;
            this.btnRunProgram.Location = new System.Drawing.Point(12, 333);
            this.btnRunProgram.Name = "btnRunProgram";
            this.btnRunProgram.Size = new System.Drawing.Size(179, 40);
            this.btnRunProgram.TabIndex = 13;
            this.btnRunProgram.UseVisualStyleBackColor = true;
            this.btnRunProgram.Click += new System.EventHandler(this.BtnRunProgram_Click);
            // 
            // groupBoxOptions
            // 
            this.groupBoxOptions.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxOptions.Controls.Add(this.labelWaterLevel);
            this.groupBoxOptions.Controls.Add(this.labelRinsingCycles);
            this.groupBoxOptions.Controls.Add(this.labelSpinningSpeed);
            this.groupBoxOptions.Controls.Add(this.labelWashingSpeed);
            this.groupBoxOptions.Controls.Add(this.labelDuration);
            this.groupBoxOptions.Controls.Add(this.labelTemperature);
            this.groupBoxOptions.Controls.Add(this.textBoxProgramDescription);
            this.groupBoxOptions.Controls.Add(this.label3);
            this.groupBoxOptions.Controls.Add(this.label14);
            this.groupBoxOptions.Controls.Add(this.label10);
            this.groupBoxOptions.Controls.Add(this.nUDWashingSpeed);
            this.groupBoxOptions.Controls.Add(this.label2);
            this.groupBoxOptions.Controls.Add(this.trackBarWashingSpeed);
            this.groupBoxOptions.Controls.Add(this.nUDWaterLevel);
            this.groupBoxOptions.Controls.Add(this.nUDRinsingCycles);
            this.groupBoxOptions.Controls.Add(this.nUDSpinningSpeed);
            this.groupBoxOptions.Controls.Add(this.nUDDuration);
            this.groupBoxOptions.Controls.Add(this.nUDTemperature);
            this.groupBoxOptions.Controls.Add(this.label13);
            this.groupBoxOptions.Controls.Add(this.trackBarWaterLevel);
            this.groupBoxOptions.Controls.Add(this.label11);
            this.groupBoxOptions.Controls.Add(this.label12);
            this.groupBoxOptions.Controls.Add(this.label9);
            this.groupBoxOptions.Controls.Add(this.label7);
            this.groupBoxOptions.Controls.Add(this.label8);
            this.groupBoxOptions.Controls.Add(this.label6);
            this.groupBoxOptions.Controls.Add(this.label1);
            this.groupBoxOptions.Controls.Add(this.trackBarRinsingCycles);
            this.groupBoxOptions.Controls.Add(this.trackBarSpinningSpeed);
            this.groupBoxOptions.Controls.Add(this.trackBarDuration);
            this.groupBoxOptions.Controls.Add(this.trackBarTemperature);
            this.groupBoxOptions.Enabled = false;
            this.groupBoxOptions.Location = new System.Drawing.Point(197, 7);
            this.groupBoxOptions.Name = "groupBoxOptions";
            this.groupBoxOptions.Size = new System.Drawing.Size(439, 366);
            this.groupBoxOptions.TabIndex = 15;
            this.groupBoxOptions.TabStop = false;
            // 
            // labelWaterLevel
            // 
            this.labelWaterLevel.AutoSize = true;
            this.labelWaterLevel.Location = new System.Drawing.Point(7, 308);
            this.labelWaterLevel.Name = "labelWaterLevel";
            this.labelWaterLevel.Size = new System.Drawing.Size(75, 13);
            this.labelWaterLevel.TabIndex = 56;
            this.labelWaterLevel.Text = "Water level, %";
            // 
            // labelRinsingCycles
            // 
            this.labelRinsingCycles.AutoSize = true;
            this.labelRinsingCycles.Location = new System.Drawing.Point(7, 261);
            this.labelRinsingCycles.Name = "labelRinsingCycles";
            this.labelRinsingCycles.Size = new System.Drawing.Size(75, 13);
            this.labelRinsingCycles.TabIndex = 55;
            this.labelRinsingCycles.Text = "Rinsing cycles";
            // 
            // labelSpinningSpeed
            // 
            this.labelSpinningSpeed.AutoSize = true;
            this.labelSpinningSpeed.Location = new System.Drawing.Point(7, 213);
            this.labelSpinningSpeed.Name = "labelSpinningSpeed";
            this.labelSpinningSpeed.Size = new System.Drawing.Size(108, 13);
            this.labelSpinningSpeed.TabIndex = 54;
            this.labelSpinningSpeed.Text = "Spinning speed, RPS";
            // 
            // labelWashingSpeed
            // 
            this.labelWashingSpeed.AutoSize = true;
            this.labelWashingSpeed.Location = new System.Drawing.Point(6, 166);
            this.labelWashingSpeed.Name = "labelWashingSpeed";
            this.labelWashingSpeed.Size = new System.Drawing.Size(109, 13);
            this.labelWashingSpeed.TabIndex = 53;
            this.labelWashingSpeed.Text = "Washing speed, RPS";
            // 
            // labelDuration
            // 
            this.labelDuration.AutoSize = true;
            this.labelDuration.Location = new System.Drawing.Point(6, 118);
            this.labelDuration.Name = "labelDuration";
            this.labelDuration.Size = new System.Drawing.Size(69, 13);
            this.labelDuration.TabIndex = 52;
            this.labelDuration.Text = "Duration, min";
            // 
            // labelTemperature
            // 
            this.labelTemperature.AutoSize = true;
            this.labelTemperature.Location = new System.Drawing.Point(6, 70);
            this.labelTemperature.Name = "labelTemperature";
            this.labelTemperature.Size = new System.Drawing.Size(84, 13);
            this.labelTemperature.TabIndex = 51;
            this.labelTemperature.Text = "Temperature, °C";
            // 
            // textBoxProgramDescription
            // 
            this.textBoxProgramDescription.BackColor = System.Drawing.SystemColors.Control;
            this.textBoxProgramDescription.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxProgramDescription.Location = new System.Drawing.Point(19, 19);
            this.textBoxProgramDescription.Multiline = true;
            this.textBoxProgramDescription.Name = "textBoxProgramDescription";
            this.textBoxProgramDescription.Size = new System.Drawing.Size(408, 41);
            this.textBoxProgramDescription.TabIndex = 50;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(230, 335);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(13, 13);
            this.label3.TabIndex = 49;
            this.label3.Text = "0";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(230, 196);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(13, 13);
            this.label14.TabIndex = 22;
            this.label14.Text = "0";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(230, 291);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(13, 13);
            this.label10.TabIndex = 17;
            this.label10.Text = "0";
            // 
            // nUDWashingSpeed
            // 
            this.nUDWashingSpeed.Location = new System.Drawing.Point(153, 164);
            this.nUDWashingSpeed.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nUDWashingSpeed.Name = "nUDWashingSpeed";
            this.nUDWashingSpeed.Size = new System.Drawing.Size(63, 20);
            this.nUDWashingSpeed.TabIndex = 0;
            this.nUDWashingSpeed.ValueChanged += new System.EventHandler(this.NUDWashingSpeed_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(410, 195);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(19, 13);
            this.label2.TabIndex = 37;
            this.label2.Text = "10";
            // 
            // trackBarWashingSpeed
            // 
            this.trackBarWashingSpeed.Location = new System.Drawing.Point(222, 164);
            this.trackBarWashingSpeed.Name = "trackBarWashingSpeed";
            this.trackBarWashingSpeed.Size = new System.Drawing.Size(212, 45);
            this.trackBarWashingSpeed.TabIndex = 38;
            this.trackBarWashingSpeed.Scroll += new System.EventHandler(this.TrackBarWashingSpeed_Scroll);
            // 
            // nUDWaterLevel
            // 
            this.nUDWaterLevel.Location = new System.Drawing.Point(153, 307);
            this.nUDWaterLevel.Name = "nUDWaterLevel";
            this.nUDWaterLevel.Size = new System.Drawing.Size(63, 20);
            this.nUDWaterLevel.TabIndex = 39;
            this.nUDWaterLevel.ValueChanged += new System.EventHandler(this.NUDWaterLevel_ValueChanged);
            // 
            // nUDRinsingCycles
            // 
            this.nUDRinsingCycles.Location = new System.Drawing.Point(153, 259);
            this.nUDRinsingCycles.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.nUDRinsingCycles.Name = "nUDRinsingCycles";
            this.nUDRinsingCycles.Size = new System.Drawing.Size(63, 20);
            this.nUDRinsingCycles.TabIndex = 40;
            this.nUDRinsingCycles.ValueChanged += new System.EventHandler(this.NUDRinsingCycles_ValueChanged);
            // 
            // nUDSpinningSpeed
            // 
            this.nUDSpinningSpeed.Location = new System.Drawing.Point(153, 211);
            this.nUDSpinningSpeed.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.nUDSpinningSpeed.Name = "nUDSpinningSpeed";
            this.nUDSpinningSpeed.Size = new System.Drawing.Size(63, 20);
            this.nUDSpinningSpeed.TabIndex = 41;
            this.nUDSpinningSpeed.ValueChanged += new System.EventHandler(this.NUDSpinningSpeed_ValueChanged);
            // 
            // nUDDuration
            // 
            this.nUDDuration.Location = new System.Drawing.Point(153, 116);
            this.nUDDuration.Maximum = new decimal(new int[] {
            180,
            0,
            0,
            0});
            this.nUDDuration.Minimum = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.nUDDuration.Name = "nUDDuration";
            this.nUDDuration.Size = new System.Drawing.Size(63, 20);
            this.nUDDuration.TabIndex = 42;
            this.nUDDuration.Value = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.nUDDuration.ValueChanged += new System.EventHandler(this.NUDDuration_ValueChanged);
            // 
            // nUDTemperature
            // 
            this.nUDTemperature.Location = new System.Drawing.Point(153, 68);
            this.nUDTemperature.Maximum = new decimal(new int[] {
            80,
            0,
            0,
            0});
            this.nUDTemperature.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nUDTemperature.Name = "nUDTemperature";
            this.nUDTemperature.Size = new System.Drawing.Size(63, 20);
            this.nUDTemperature.TabIndex = 43;
            this.nUDTemperature.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nUDTemperature.ValueChanged += new System.EventHandler(this.NUDTemperature_ValueChanged);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(404, 339);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(25, 13);
            this.label13.TabIndex = 23;
            this.label13.Text = "100";
            // 
            // trackBarWaterLevel
            // 
            this.trackBarWaterLevel.Location = new System.Drawing.Point(222, 307);
            this.trackBarWaterLevel.Maximum = 100;
            this.trackBarWaterLevel.Name = "trackBarWaterLevel";
            this.trackBarWaterLevel.Size = new System.Drawing.Size(212, 45);
            this.trackBarWaterLevel.TabIndex = 44;
            this.trackBarWaterLevel.Scroll += new System.EventHandler(this.TrackBarWaterLevel_Scroll);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(413, 291);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(13, 13);
            this.label11.TabIndex = 20;
            this.label11.Text = "5";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(230, 243);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(13, 13);
            this.label12.TabIndex = 19;
            this.label12.Text = "0";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(410, 243);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(19, 13);
            this.label9.TabIndex = 18;
            this.label9.Text = "20";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(407, 148);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(25, 13);
            this.label7.TabIndex = 16;
            this.label7.Text = "180";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(227, 148);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(19, 13);
            this.label8.TabIndex = 15;
            this.label8.Text = "15";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(413, 100);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(19, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "80";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(225, 100);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(19, 13);
            this.label1.TabIndex = 13;
            this.label1.Text = "10";
            // 
            // trackBarRinsingCycles
            // 
            this.trackBarRinsingCycles.Location = new System.Drawing.Point(222, 259);
            this.trackBarRinsingCycles.Maximum = 5;
            this.trackBarRinsingCycles.Name = "trackBarRinsingCycles";
            this.trackBarRinsingCycles.Size = new System.Drawing.Size(212, 45);
            this.trackBarRinsingCycles.TabIndex = 45;
            this.trackBarRinsingCycles.Scroll += new System.EventHandler(this.TrackBarRinsingCycles_Scroll);
            // 
            // trackBarSpinningSpeed
            // 
            this.trackBarSpinningSpeed.Location = new System.Drawing.Point(222, 211);
            this.trackBarSpinningSpeed.Maximum = 20;
            this.trackBarSpinningSpeed.Name = "trackBarSpinningSpeed";
            this.trackBarSpinningSpeed.Size = new System.Drawing.Size(212, 45);
            this.trackBarSpinningSpeed.TabIndex = 46;
            this.trackBarSpinningSpeed.Scroll += new System.EventHandler(this.TrackBarSpinningSpeed_Scroll);
            // 
            // trackBarDuration
            // 
            this.trackBarDuration.Location = new System.Drawing.Point(222, 116);
            this.trackBarDuration.Maximum = 180;
            this.trackBarDuration.Minimum = 15;
            this.trackBarDuration.Name = "trackBarDuration";
            this.trackBarDuration.Size = new System.Drawing.Size(212, 45);
            this.trackBarDuration.TabIndex = 47;
            this.trackBarDuration.TickFrequency = 5;
            this.trackBarDuration.Value = 15;
            this.trackBarDuration.Scroll += new System.EventHandler(this.TrackBarDuration_Scroll);
            // 
            // trackBarTemperature
            // 
            this.trackBarTemperature.LargeChange = 10;
            this.trackBarTemperature.Location = new System.Drawing.Point(222, 68);
            this.trackBarTemperature.Maximum = 80;
            this.trackBarTemperature.Minimum = 10;
            this.trackBarTemperature.Name = "trackBarTemperature";
            this.trackBarTemperature.Size = new System.Drawing.Size(212, 45);
            this.trackBarTemperature.SmallChange = 5;
            this.trackBarTemperature.TabIndex = 48;
            this.trackBarTemperature.Value = 10;
            this.trackBarTemperature.Scroll += new System.EventHandler(this.TrackBarTemperature_Scroll);
            // 
            // listBoxPrograms
            // 
            this.listBoxPrograms.Enabled = false;
            this.listBoxPrograms.FormattingEnabled = true;
            this.listBoxPrograms.Location = new System.Drawing.Point(12, 12);
            this.listBoxPrograms.Name = "listBoxPrograms";
            this.listBoxPrograms.Size = new System.Drawing.Size(179, 316);
            this.listBoxPrograms.TabIndex = 16;
            this.listBoxPrograms.SelectedIndexChanged += new System.EventHandler(this.ListBoxPrograms_SelectedIndexChanged);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(647, 400);
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
        private System.Windows.Forms.ToolStripMenuItem FirmwareUpdateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem logToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel lblTemp;
        private System.Windows.Forms.NotifyIcon trayIcon;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem AboutToolStripMenuItem;
        private System.Windows.Forms.Button btnRunProgram;
        private System.Windows.Forms.GroupBox groupBoxOptions;
        private System.Windows.Forms.NumericUpDown nUDWashingSpeed;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TrackBar trackBarWashingSpeed;
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
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxProgramDescription;
        private System.Windows.Forms.Label labelWaterLevel;
        private System.Windows.Forms.Label labelRinsingCycles;
        private System.Windows.Forms.Label labelSpinningSpeed;
        private System.Windows.Forms.Label labelWashingSpeed;
        private System.Windows.Forms.Label labelDuration;
        private System.Windows.Forms.Label labelTemperature;
    }
}

