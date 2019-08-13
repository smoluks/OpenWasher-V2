using OpenWasherHardwareLibrary;
using OpenWasherHardwareLibrary.Commands;
using OpenWasherHardwareLibrary.Entity;
using OpenWasherHardwareLibrary.Enums;
using System;
using System.Windows.Forms;
using WindowsFormsClient.Entities;
using WindowsFormsClient.Managers;

namespace WindowsFormsClient
{
    public partial class Main : Form
    {
        private readonly HardwareLibrary hardwareLibrary;
        private readonly Localizator localizator;
        private readonly ConfigManager configManager = new ConfigManager();
        private bool isWashing;

        private LogFrm logFrm;
        private SettingsFrm settingsForm;
        private AboutFrm aboutForm;
        private FirmwareFrm firmwareFrm;

        public Main()
        {
            InitializeComponent();            
            hardwareLibrary = new HardwareLibrary(configManager.Port, MessageManager.MessageHandler, ErrorHandler, EventHandler, ConnectionEventHandler, configManager.LogEnable);
            localizator = new Localizator(configManager.Locale);
            SetStatusText(localizator.GetString("Status_Connecting", "Connecting"));
        }

        private void Main_Load(object sender, EventArgs e)
        {
            foreach (var program in EnumManager.GetValues<WashProgram>())
            {
                listBoxPrograms.Items.Add(new WashingProgram(
                    program,
                    localizator.GetString($"Program_{(byte)program}", $"{program}")));
            }
        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (logFrm != null && !logFrm.IsDisposed)
                logFrm.Close();
            if (settingsForm != null && !settingsForm.IsDisposed)
                settingsForm.Close();
            if (aboutForm != null && !aboutForm.IsDisposed)
                aboutForm.Close();
            if (firmwareFrm != null && !firmwareFrm.IsDisposed)
                firmwareFrm.Close();

            hardwareLibrary.Dispose();
        }

        private async void timerPoll_Tick(object sender, EventArgs e)
        {
            try
            {
                var status = await hardwareLibrary.SendCommandAsync(new GetStatus());

                lblTemp.Text = $"{status.temperature}°C";
                if (status.program != WashProgram.Nothing)
                {
                    SetStatusText(string.Format(
                        localizator.GetString("Status_Washing", "{0}: {1}"),
                        localizator.GetString($"Program_{status.program}", $"Program {status.program}"),
                        localizator.GetString($"Stage_{status.stage}", $"Stage {status.stage}")));

                    listBoxPrograms.Enabled = false;
                    groupBoxOptions.Enabled = false;
                    groupBoxOptions.Enabled = false;
                    isWashing = true;
                }
                else 
                {
                    
                    listBoxPrograms.Enabled = true;
                    groupBoxOptions.Enabled = true;                 
                    logToolStripMenuItem.Enabled = true;
                    if (isWashing)
                    {
                        isWashing = false;
                        SetStatusText(localizator.GetString("Status_Stopped", "Stopped"));
                    }
                }
            }
            catch (TimeoutException)
            {
                btnRunProgram.Enabled = false;
                groupBoxOptions.Enabled = false;
                SetStatusText(localizator.GetString("Status_ConnectionBreak", "Connection break"));
            }
        }

        private async void BtnRunProgram_Click(object sender, EventArgs e)
        {
            if (isWashing)
            {
                DialogResult dialogResult = MessageBox.Show("Really stop?", "Stop program", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    await hardwareLibrary.SendCommandAsync(new StopProgram());
                    groupBoxOptions.Enabled = true;
                }
            }
            else
            {
                if (listBoxPrograms.SelectedIndex != -1)
                {
                    var program = listBoxPrograms.SelectedItem as WashingProgram;
                    await hardwareLibrary.SendCommandAsync(new StartProgram(program.Program, GetOptions()));
                    groupBoxOptions.Enabled = false;
                }
            }
        }

        private ProgramOptions GetOptions()
        {
            var options = new ProgramOptions();

            if (cbTemperature.Checked)
                options.temperature = (byte)nUDTemperature.Value;
            if (cbDuration.Checked)
                options.duration = (byte)nUDDuration.Value;
            if (cbWashingSpeed.Checked)
                options.washingspeed = (byte)nUDWashingSpeed.Value;
            if (cbSpinningSpeed.Checked)
                options.spinningspeed = (byte)nUDSpinningSpeed.Value;
            if (cbRinsingCycles.Checked)
                options.rinsingCycles = (byte)nUDRinsingCycles.Value;
            if (cbWaterLevel.Checked)
                options.waterlevel = (byte)nUDWaterLevel.Value;

            return options;
        }

        private void LogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (logFrm == null || logFrm.IsDisposed)
            {
                logFrm = new LogFrm();
                logFrm.Show();
            }
            else
                logFrm.BringToFront();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (settingsForm == null || settingsForm.IsDisposed)
            {
                settingsForm = new SettingsFrm(configManager);
                settingsForm.Show();
            }
            else
                settingsForm.BringToFront();
        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (aboutForm == null || aboutForm.IsDisposed)
            {
                aboutForm = new AboutFrm();
                aboutForm.Show();
            }
            else
                aboutForm.BringToFront();
        }

        private void FirmwareUpdateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (firmwareFrm == null || firmwareFrm.IsDisposed)
            {
                firmwareFrm = new FirmwareFrm(hardwareLibrary);
                firmwareFrm.Show();
            }
            else
                firmwareFrm.BringToFront();
        }

        private void Main_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                Hide();
                trayIcon.Visible = true;
            }
        }

        private void TrayIcon_DoubleClick(object sender, EventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
            trayIcon.Visible = false;
        }

        private void TrackBarTemperature_Scroll(object sender, EventArgs e)
        {
            nUDTemperature.Value = trackBarTemperature.Value;
        }

        private void TrackBarDuration_Scroll(object sender, EventArgs e)
        {
            nUDDuration.Value = trackBarDuration.Value;
        }

        private void TrackBarWashingSpeed_Scroll(object sender, EventArgs e)
        {
            nUDWashingSpeed.Value = trackBarWashingSpeed.Value;
        }

        private void TrackBarSpinningSpeed_Scroll(object sender, EventArgs e)
        {
            nUDSpinningSpeed.Value = trackBarSpinningSpeed.Value;
        }

        private void TrackBarRinsingCycles_Scroll(object sender, EventArgs e)
        {
            nUDRinsingCycles.Value = trackBarRinsingCycles.Value;
        }

        private void TrackBarWaterLevel_Scroll(object sender, EventArgs e)
        {
            nUDWaterLevel.Value = trackBarWaterLevel.Value;
        }

        private void NUDTemperature_ValueChanged(object sender, EventArgs e)
        {
            trackBarTemperature.Value = (int)nUDTemperature.Value;
        }

        private void NUDDuration_ValueChanged(object sender, EventArgs e)
        {
            trackBarDuration.Value = (int)nUDDuration.Value;
        }

        private void NUDWashingSpeed_ValueChanged(object sender, EventArgs e)
        {
            trackBarWashingSpeed.Value = (int)nUDWashingSpeed.Value;
        }

        private void NUDSpinningSpeed_ValueChanged(object sender, EventArgs e)
        {
            trackBarSpinningSpeed.Value = (int)nUDSpinningSpeed.Value;
        }

        private void NUDRinsingCycles_ValueChanged(object sender, EventArgs e)
        {
            trackBarRinsingCycles.Value = (int)nUDRinsingCycles.Value;
        }

        private void NUDWaterLevel_ValueChanged(object sender, EventArgs e)
        {
            trackBarWaterLevel.Value = (int)nUDWaterLevel.Value;
        }
    }
}
