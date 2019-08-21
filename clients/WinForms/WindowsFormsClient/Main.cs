using OpenWasherClient.Entities;
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
        private HardwareLibrary hardwareLibrary;
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
            hardwareLibrary = new HardwareLibrary(configManager.CurrentConfig.Port, MessageManager.MessageHandler, ErrorHandler, EventHandler, ConnectionEventHandler, configManager.CurrentConfig.LogEnable);
            localizator = new Localizator(configManager.CurrentConfig.Locale);
            SetStatusText(localizator.GetString("Status_Connecting", "Connecting"));
        }

        private void Main_Load(object sender, EventArgs e)
        {
            listBoxPrograms.Items.Clear();
            foreach (var program in EnumManager.GetValues<WashProgram>())
            {
                if (program != WashProgram.Nothing)
                {
                    listBoxPrograms.Items.Add(new WashingProgram(
                        program,
                        localizator.GetString($"Program_{(byte)program}", $"{program}")));
                }
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
                        localizator.GetString($"Program_{(byte)status.program}", $"Program {status.program}"),
                        localizator.GetString($"Stage_{(byte)status.stage}", $"Stage {status.stage}")));

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
            catch
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

            //if (cbTemperature.Checked)
                options.temperature = (byte)nUDTemperature.Value;
            //if (cbDuration.Checked)
                options.duration = (byte)nUDDuration.Value;
            //if (cbWashingSpeed.Checked)
                options.washingspeed = (byte)nUDWashingSpeed.Value;
            //if (cbSpinningSpeed.Checked)
                options.spinningspeed = (byte)nUDSpinningSpeed.Value;
            //if (cbRinsingCycles.Checked)
                options.rinsingCycles = (byte)nUDRinsingCycles.Value;
            //if (cbWaterLevel.Checked)
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
                settingsForm = new SettingsFrm(configManager.CurrentConfig, ConfigChangehandler);
                settingsForm.Show();
            }
            else
                settingsForm.BringToFront();
        }

        private void ConfigChangehandler(Config newConfig)
        {
            this.Invoke((MethodInvoker)(() =>
            {
                var currectConfig = configManager.CurrentConfig;
                if (currectConfig.Port != newConfig.Port)
                {
                    btnRunProgram.Enabled = false;
                    groupBoxOptions.Enabled = false;
                    SetStatusText(localizator.GetString("Status_Reconnecting", "Reconnecting"));
                    hardwareLibrary.Dispose();
                    hardwareLibrary = new HardwareLibrary(newConfig.Port, MessageManager.MessageHandler, ErrorHandler, EventHandler, ConnectionEventHandler, newConfig.LogEnable);
                }
                else if(currectConfig.LogEnable != newConfig.LogEnable)
                {
                    hardwareLibrary.LogEnable = newConfig.LogEnable;
                }

                if (currectConfig.Locale != newConfig.Locale)
                {
                    Main_Load(null, null);
                }
            }));

            configManager.UpdateConfig(newConfig);
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

        private void ListBoxPrograms_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxPrograms.SelectedIndex == -1)
                return;

            textBoxProgramDescription.Text = localizator.GetString($"Program_{listBoxPrograms.SelectedIndex}_Comment",
                localizator.GetString($"Program_{listBoxPrograms.SelectedIndex}",
                $"Program {(WashProgram)listBoxPrograms.SelectedIndex}"));

            var defaultOptions = HardwareLibrary.GetDefaultOptions((WashProgram)listBoxPrograms.SelectedIndex);
            if (defaultOptions == null)
            {
                nUDTemperature.Enabled = false;
                trackBarTemperature.Enabled = false;
                nUDDuration.Enabled = false;
                trackBarDuration.Enabled = false;
                nUDWashingSpeed.Enabled = false;
                trackBarWashingSpeed.Enabled = false;
                nUDSpinningSpeed.Enabled = false;
                trackBarSpinningSpeed.Enabled = false;
                nUDRinsingCycles.Enabled = false;
                trackBarRinsingCycles.Enabled = false;
                nUDWaterLevel.Enabled = false;
                trackBarWaterLevel.Enabled = false;
            }
            else
            {
                nUDTemperature.Enabled = true;
                trackBarTemperature.Enabled = true;
                nUDDuration.Enabled = true;
                trackBarDuration.Enabled = true;
                nUDWashingSpeed.Enabled = true;
                trackBarWashingSpeed.Enabled = true;
                nUDSpinningSpeed.Enabled = true;
                trackBarSpinningSpeed.Enabled = true;
                nUDRinsingCycles.Enabled = true;
                trackBarRinsingCycles.Enabled = true;
                nUDWaterLevel.Enabled = true;
                trackBarWaterLevel.Enabled = true;

                nUDTemperature.Value = defaultOptions.temperature ?? 10;
                trackBarTemperature.Value = defaultOptions.temperature ?? 10;

                nUDDuration.Value = defaultOptions.duration ?? 15;
                trackBarDuration.Value = defaultOptions.duration ?? 15;

                nUDWashingSpeed.Value = defaultOptions.washingspeed ?? 0;
                trackBarWashingSpeed.Value = defaultOptions.washingspeed ?? 0;

                nUDSpinningSpeed.Value = defaultOptions.spinningspeed ?? 0;
                trackBarSpinningSpeed.Value = defaultOptions.spinningspeed ?? 0;

                nUDRinsingCycles.Value = defaultOptions.rinsingCycles ?? 0;
                trackBarRinsingCycles.Value = defaultOptions.rinsingCycles ?? 0;

                nUDWaterLevel.Value = defaultOptions.waterlevel ?? 0;
                trackBarWaterLevel.Value = defaultOptions.waterlevel ?? 0;
            }
        }
    }
}
