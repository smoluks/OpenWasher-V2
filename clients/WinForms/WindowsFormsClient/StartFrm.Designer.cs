namespace WindowsFormsClient
{
    partial class StartFrm
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
            this.btnRunProgram = new System.Windows.Forms.Button();
            this.cbPrograms = new System.Windows.Forms.ComboBox();
            this.groupBox = new System.Windows.Forms.GroupBox();
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
            this.printDocument1 = new System.Drawing.Printing.PrintDocument();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.checkBoxAdditional = new System.Windows.Forms.CheckBox();
            this.groupBox.SuspendLayout();
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
            // btnRunProgram
            // 
            this.btnRunProgram.Location = new System.Drawing.Point(358, 10);
            this.btnRunProgram.Name = "btnRunProgram";
            this.btnRunProgram.Size = new System.Drawing.Size(85, 21);
            this.btnRunProgram.TabIndex = 0;
            this.btnRunProgram.Text = "Start";
            this.btnRunProgram.UseVisualStyleBackColor = true;
            this.btnRunProgram.Click += new System.EventHandler(this.BtnRunProgram_Click);
            // 
            // cbPrograms
            // 
            this.cbPrograms.Location = new System.Drawing.Point(12, 11);
            this.cbPrograms.Name = "cbPrograms";
            this.cbPrograms.Size = new System.Drawing.Size(340, 21);
            this.cbPrograms.TabIndex = 1;
            this.cbPrograms.SelectedIndexChanged += new System.EventHandler(this.CbPrograms_SelectedIndexChanged);
            // 
            // groupBox
            // 
            this.groupBox.Controls.Add(this.cbWaterLevel);
            this.groupBox.Controls.Add(this.cbRinsingCycles);
            this.groupBox.Controls.Add(this.cbSpinningSpeed);
            this.groupBox.Controls.Add(this.cbDuration);
            this.groupBox.Controls.Add(this.cbTemperature);
            this.groupBox.Controls.Add(this.nUDWaterLevel);
            this.groupBox.Controls.Add(this.nUDRinsingCycles);
            this.groupBox.Controls.Add(this.nUDSpinningSpeed);
            this.groupBox.Controls.Add(this.nUDDuration);
            this.groupBox.Controls.Add(this.nUDTemperature);
            this.groupBox.Controls.Add(this.label13);
            this.groupBox.Controls.Add(this.label14);
            this.groupBox.Controls.Add(this.trackBarWaterLevel);
            this.groupBox.Controls.Add(this.label11);
            this.groupBox.Controls.Add(this.label12);
            this.groupBox.Controls.Add(this.label9);
            this.groupBox.Controls.Add(this.label10);
            this.groupBox.Controls.Add(this.label7);
            this.groupBox.Controls.Add(this.label8);
            this.groupBox.Controls.Add(this.label6);
            this.groupBox.Controls.Add(this.label1);
            this.groupBox.Controls.Add(this.trackBarRinsingCycles);
            this.groupBox.Controls.Add(this.trackBarSpinningSpeed);
            this.groupBox.Controls.Add(this.trackBarDuration);
            this.groupBox.Controls.Add(this.trackBarTemperature);
            this.groupBox.Enabled = false;
            this.groupBox.Location = new System.Drawing.Point(12, 39);
            this.groupBox.Name = "groupBox";
            this.groupBox.Size = new System.Drawing.Size(433, 296);
            this.groupBox.TabIndex = 2;
            this.groupBox.TabStop = false;
            // 
            // cbWaterLevel
            // 
            this.cbWaterLevel.AutoSize = true;
            this.cbWaterLevel.Location = new System.Drawing.Point(6, 230);
            this.cbWaterLevel.Name = "cbWaterLevel";
            this.cbWaterLevel.Size = new System.Drawing.Size(94, 17);
            this.cbWaterLevel.TabIndex = 33;
            this.cbWaterLevel.Text = "Water level, %";
            this.cbWaterLevel.UseVisualStyleBackColor = true;
            // 
            // cbRinsingCycles
            // 
            this.cbRinsingCycles.AutoSize = true;
            this.cbRinsingCycles.Location = new System.Drawing.Point(6, 179);
            this.cbRinsingCycles.Name = "cbRinsingCycles";
            this.cbRinsingCycles.Size = new System.Drawing.Size(94, 17);
            this.cbRinsingCycles.TabIndex = 32;
            this.cbRinsingCycles.Text = "Rinsing cycles";
            this.cbRinsingCycles.UseVisualStyleBackColor = true;
            // 
            // cbSpinningSpeed
            // 
            this.cbSpinningSpeed.AutoSize = true;
            this.cbSpinningSpeed.Location = new System.Drawing.Point(6, 129);
            this.cbSpinningSpeed.Name = "cbSpinningSpeed";
            this.cbSpinningSpeed.Size = new System.Drawing.Size(127, 17);
            this.cbSpinningSpeed.TabIndex = 31;
            this.cbSpinningSpeed.Text = "Spinning speed, RPS";
            this.cbSpinningSpeed.UseVisualStyleBackColor = true;
            // 
            // cbDuration
            // 
            this.cbDuration.AutoSize = true;
            this.cbDuration.Location = new System.Drawing.Point(6, 78);
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
            this.nUDWaterLevel.Location = new System.Drawing.Point(133, 229);
            this.nUDWaterLevel.Name = "nUDWaterLevel";
            this.nUDWaterLevel.Size = new System.Drawing.Size(46, 20);
            this.nUDWaterLevel.TabIndex = 28;
            // 
            // nUDRinsingCycles
            // 
            this.nUDRinsingCycles.Location = new System.Drawing.Point(133, 178);
            this.nUDRinsingCycles.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.nUDRinsingCycles.Name = "nUDRinsingCycles";
            this.nUDRinsingCycles.Size = new System.Drawing.Size(46, 20);
            this.nUDRinsingCycles.TabIndex = 27;
            // 
            // nUDSpinningSpeed
            // 
            this.nUDSpinningSpeed.Location = new System.Drawing.Point(133, 128);
            this.nUDSpinningSpeed.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.nUDSpinningSpeed.Name = "nUDSpinningSpeed";
            this.nUDSpinningSpeed.Size = new System.Drawing.Size(46, 20);
            this.nUDSpinningSpeed.TabIndex = 26;
            // 
            // nUDDuration
            // 
            this.nUDDuration.Location = new System.Drawing.Point(133, 77);
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
            this.nUDDuration.Size = new System.Drawing.Size(46, 20);
            this.nUDDuration.TabIndex = 25;
            this.nUDDuration.Value = new decimal(new int[] {
            15,
            0,
            0,
            0});
            // 
            // nUDTemperature
            // 
            this.nUDTemperature.Location = new System.Drawing.Point(133, 27);
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
            this.nUDTemperature.Size = new System.Drawing.Size(46, 20);
            this.nUDTemperature.TabIndex = 24;
            this.nUDTemperature.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(406, 261);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(25, 13);
            this.label13.TabIndex = 23;
            this.label13.Text = "100";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(184, 261);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(13, 13);
            this.label14.TabIndex = 22;
            this.label14.Text = "0";
            // 
            // trackBarWaterLevel
            // 
            this.trackBarWaterLevel.Location = new System.Drawing.Point(178, 229);
            this.trackBarWaterLevel.Maximum = 100;
            this.trackBarWaterLevel.Name = "trackBarWaterLevel";
            this.trackBarWaterLevel.Size = new System.Drawing.Size(247, 45);
            this.trackBarWaterLevel.TabIndex = 21;
            this.trackBarWaterLevel.TickFrequency = 5;
            this.trackBarWaterLevel.Scroll += new System.EventHandler(this.TrackBarWaterLevel_Scroll);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(406, 210);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(13, 13);
            this.label11.TabIndex = 20;
            this.label11.Text = "5";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(184, 210);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(13, 13);
            this.label12.TabIndex = 19;
            this.label12.Text = "0";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(406, 159);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(19, 13);
            this.label9.TabIndex = 18;
            this.label9.Text = "20";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(184, 159);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(13, 13);
            this.label10.TabIndex = 17;
            this.label10.Text = "0";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(400, 108);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(25, 13);
            this.label7.TabIndex = 16;
            this.label7.Text = "180";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(184, 108);
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
            this.trackBarRinsingCycles.Location = new System.Drawing.Point(178, 178);
            this.trackBarRinsingCycles.Maximum = 5;
            this.trackBarRinsingCycles.Name = "trackBarRinsingCycles";
            this.trackBarRinsingCycles.Size = new System.Drawing.Size(247, 45);
            this.trackBarRinsingCycles.TabIndex = 8;
            this.trackBarRinsingCycles.Scroll += new System.EventHandler(this.TrackBarRinsingCycles_Scroll);
            // 
            // trackBarSpinningSpeed
            // 
            this.trackBarSpinningSpeed.Location = new System.Drawing.Point(178, 128);
            this.trackBarSpinningSpeed.Maximum = 20;
            this.trackBarSpinningSpeed.Name = "trackBarSpinningSpeed";
            this.trackBarSpinningSpeed.Size = new System.Drawing.Size(247, 45);
            this.trackBarSpinningSpeed.TabIndex = 6;
            this.trackBarSpinningSpeed.Scroll += new System.EventHandler(this.TrackBarSpinningSpeed_Scroll);
            // 
            // trackBarDuration
            // 
            this.trackBarDuration.Location = new System.Drawing.Point(178, 77);
            this.trackBarDuration.Maximum = 180;
            this.trackBarDuration.Minimum = 15;
            this.trackBarDuration.Name = "trackBarDuration";
            this.trackBarDuration.Size = new System.Drawing.Size(247, 45);
            this.trackBarDuration.TabIndex = 5;
            this.trackBarDuration.TickFrequency = 10;
            this.trackBarDuration.Value = 15;
            this.trackBarDuration.Scroll += new System.EventHandler(this.TrackBarDuration_Scroll);
            // 
            // trackBarTemperature
            // 
            this.trackBarTemperature.Location = new System.Drawing.Point(178, 26);
            this.trackBarTemperature.Maximum = 80;
            this.trackBarTemperature.Minimum = 10;
            this.trackBarTemperature.Name = "trackBarTemperature";
            this.trackBarTemperature.Size = new System.Drawing.Size(250, 45);
            this.trackBarTemperature.TabIndex = 0;
            this.trackBarTemperature.TickFrequency = 5;
            this.trackBarTemperature.Value = 10;
            this.trackBarTemperature.Scroll += new System.EventHandler(this.TrackBarTemperature_Scroll);
            // 
            // checkBoxAdditional
            // 
            this.checkBoxAdditional.AutoSize = true;
            this.checkBoxAdditional.Location = new System.Drawing.Point(18, 39);
            this.checkBoxAdditional.Name = "checkBoxAdditional";
            this.checkBoxAdditional.Size = new System.Drawing.Size(72, 17);
            this.checkBoxAdditional.TabIndex = 12;
            this.checkBoxAdditional.Text = "Additional";
            this.checkBoxAdditional.UseVisualStyleBackColor = true;
            this.checkBoxAdditional.CheckedChanged += new System.EventHandler(this.CheckBoxAdditional_CheckedChanged_1);
            // 
            // StartFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(458, 347);
            this.Controls.Add(this.checkBoxAdditional);
            this.Controls.Add(this.groupBox);
            this.Controls.Add(this.cbPrograms);
            this.Controls.Add(this.btnRunProgram);
            this.Name = "StartFrm";
            this.Text = "StartFrm";
            this.Load += new System.EventHandler(this.StartFrm_Load);
            this.groupBox.ResumeLayout(false);
            this.groupBox.PerformLayout();
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

        private System.Windows.Forms.Button btnRunProgram;
        private System.Windows.Forms.GroupBox groupBox;
        private System.Windows.Forms.TrackBar trackBarTemperature;
        private System.Drawing.Printing.PrintDocument printDocument1;
        private System.Windows.Forms.TrackBar trackBarDuration;
        private System.Windows.Forms.TrackBar trackBarRinsingCycles;
        private System.Windows.Forms.TrackBar trackBarSpinningSpeed;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbPrograms;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TrackBar trackBarWaterLevel;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown nUDWaterLevel;
        private System.Windows.Forms.NumericUpDown nUDRinsingCycles;
        private System.Windows.Forms.NumericUpDown nUDSpinningSpeed;
        private System.Windows.Forms.NumericUpDown nUDDuration;
        private System.Windows.Forms.NumericUpDown nUDTemperature;
        private System.Windows.Forms.CheckBox checkBoxAdditional;
        private System.Windows.Forms.CheckBox cbWaterLevel;
        private System.Windows.Forms.CheckBox cbRinsingCycles;
        private System.Windows.Forms.CheckBox cbSpinningSpeed;
        private System.Windows.Forms.CheckBox cbDuration;
        private System.Windows.Forms.CheckBox cbTemperature;
    }
}