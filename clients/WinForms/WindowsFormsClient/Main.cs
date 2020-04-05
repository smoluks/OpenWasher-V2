using OpenWasherClient.Entities;
using OpenWasherHardwareLibrary;
using OpenWasherHardwareLibrary.Entity;
using OpenWasherHardwareLibrary.Enums;
using System;
using System.Threading;
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
        //private FirmwareFrm firmwareFrm;

        readonly CancellationTokenSource cancelTokenSource;
        CancellationToken token;

        public Main()
        {
            InitializeComponent();

            cancelTokenSource = new CancellationTokenSource();
            token = cancelTokenSource.Token;

            localizator = new Localizator(configManager.CurrentConfig.Locale);

            hardwareLibrary = new HardwareLibrary(MessageManager.MessageHandler, ErrorHandler, EventHandler, configManager.CurrentConfig.LogEnable);
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

            labelTemperature.Text = localizator.GetString("Label_Temperature", labelTemperature.Text);
            labelDuration.Text = localizator.GetString("Label_Duration", labelDuration.Text);
            labelWashingSpeed.Text = localizator.GetString("Label_WashingSpeed", labelWashingSpeed.Text);
            labelSpinningSpeed.Text = localizator.GetString("Label_SpinningSpeed", labelSpinningSpeed.Text);
            labelRinsingCycles.Text = localizator.GetString("Label_RinsingCycles", labelRinsingCycles.Text);
            labelWaterLevel.Text = localizator.GetString("Label_WaterLevel", labelWaterLevel.Text);

            Connect();
        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            timerPoll.Enabled = false;
            connectCancelTokenSource?.Cancel();
            cancelTokenSource.Cancel();

            if (logFrm != null && !logFrm.IsDisposed)
                logFrm.Close();
            if (settingsForm != null && !settingsForm.IsDisposed)
                settingsForm.Close();
            if (aboutForm != null && !aboutForm.IsDisposed)
                aboutForm.Close();
            //if (firmwareFrm != null && !firmwareFrm.IsDisposed)
                //firmwareFrm.Close();

            hardwareLibrary.Dispose();
        }

        private async void timerPoll_Tick(object sender, EventArgs e)
        {
            try
            {
                var status = await hardwareLibrary.GetStatusAsync(token);

                lblTemp.Text = $"{status.temperature}°C";
                lblRPS.Text = $"{status.spinningSpeed}";

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
            catch (OperationCanceledException) { }
            catch (TimeoutException)
            {
                Disconnect();
                SetStatusText(localizator.GetString("Status_ConnectionBreak", "Connection break"));
            }
            catch (Exception ex)
            {
                Disconnect();
                SetStatusText(localizator.GetString("Status_ConnectionBreak", "Connection break"));
                MessageBox.Show(ex.Message);
            }
        }

        private async void BtnRunProgram_Click(object sender, EventArgs e)
        {
            if (isWashing)
            {
                DialogResult dialogResult = MessageBox.Show("Really stop?", "Stop program", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    try
                    {
                        await hardwareLibrary.StopProgramAsync(token);
                        groupBoxOptions.Enabled = true;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Command error");
                    }
                }
            }
            else
            {
                if (listBoxPrograms.SelectedIndex != -1)
                {
                    var program = listBoxPrograms.SelectedItem as WashingProgram;
                    try
                    {
                        await hardwareLibrary.StartProgramAsync(token, program.Program, GetOptions());

                        groupBoxOptions.Enabled = false;
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Command error");
                    }
                }
            }
        }

        private ProgramOptions GetOptions()
        {
            return new ProgramOptions
            {
                temperature = nUDTemperature.Enabled ? (byte?)nUDTemperature.Value : null,
                duration = nUDDuration.Enabled ? (byte?)nUDDuration.Value : null,
                washingSpeed = nUDWashingSpeed.Enabled ? (byte?)nUDWashingSpeed.Value : null,
                spinningSpeed = nUDSpinningSpeed.Enabled ? (byte?)nUDSpinningSpeed.Value : null,
                rinsingCycles = nUDRinsingCycles.Enabled ? (byte?)nUDRinsingCycles.Value : null,
                waterLevel = nUDWaterLevel.Enabled ? (byte?)nUDWaterLevel.Value : null
            };
        }

        private void ConfigChangeHandler(Config newConfig)
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
                    hardwareLibrary = new HardwareLibrary(MessageManager.MessageHandler, ErrorHandler, EventHandler, newConfig.LogEnable);

                    Connect();
                }
                else if (currectConfig.LogEnable != newConfig.LogEnable)
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
                settingsForm = new SettingsFrm(configManager.CurrentConfig, ConfigChangeHandler);
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

        private async void ListBoxPrograms_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxPrograms.SelectedIndex == -1)
                return;

            textBoxProgramDescription.Text = localizator.GetString($"Program_{listBoxPrograms.SelectedIndex}_Comment",
                localizator.GetString($"Program_{listBoxPrograms.SelectedIndex}",
                $"Program {(WashProgram)listBoxPrograms.SelectedIndex}"));

            var defaultOptions = await hardwareLibrary.GetDefaultOptionsAsync(token, (WashProgram)listBoxPrograms.SelectedIndex);

            if (defaultOptions.temperature.HasValue)
            {
                trackBarTemperature.Enabled = true;
                trackBarTemperature.Value = defaultOptions.temperature.Value;
                nUDTemperature.Enabled = true;
                nUDTemperature.Value = defaultOptions.temperature.Value;
            }
            else
            {
                trackBarTemperature.Enabled = false;
                trackBarTemperature.Value = 10;
                nUDTemperature.Enabled = false;
                nUDTemperature.Value = 10;
            }

            if (defaultOptions.duration.HasValue)
            {
                trackBarDuration.Enabled = true;
                trackBarDuration.Value = defaultOptions.duration.Value;
                nUDDuration.Enabled = true;
                nUDDuration.Value = defaultOptions.duration.Value;
            }
            else
            {
                trackBarDuration.Enabled = false;
                trackBarDuration.Value = 0;
                nUDDuration.Enabled = false;
                nUDDuration.Value = 0;
            }

            if (defaultOptions.washingSpeed.HasValue)
            {
                trackBarWashingSpeed.Enabled = true;
                trackBarWashingSpeed.Value = defaultOptions.washingSpeed.Value;
                nUDWashingSpeed.Enabled = true;
                nUDWashingSpeed.Value = defaultOptions.washingSpeed.Value;
            }
            else
            {
                trackBarWashingSpeed.Enabled = false;
                trackBarWashingSpeed.Value = 0;
                nUDWashingSpeed.Enabled = false;
                nUDWashingSpeed.Value = 0;
            }

            if (defaultOptions.spinningSpeed.HasValue)
            {
                trackBarSpinningSpeed.Enabled = true;
                trackBarSpinningSpeed.Value = defaultOptions.spinningSpeed.Value;
                nUDSpinningSpeed.Enabled = true;
                nUDSpinningSpeed.Value = defaultOptions.spinningSpeed.Value;
            }
            else
            {
                trackBarSpinningSpeed.Enabled = false;
                trackBarSpinningSpeed.Value = 0;
                nUDSpinningSpeed.Enabled = false;
                nUDSpinningSpeed.Value = 0;
            }

            if (defaultOptions.rinsingCycles.HasValue)
            {
                trackBarRinsingCycles.Enabled = true;
                trackBarRinsingCycles.Value = defaultOptions.rinsingCycles.Value;
                nUDRinsingCycles.Enabled = true;
                nUDRinsingCycles.Value = defaultOptions.rinsingCycles.Value;
            }
            else
            {
                trackBarRinsingCycles.Enabled = false;
                trackBarRinsingCycles.Value = 0;
                nUDRinsingCycles.Enabled = false;
                nUDRinsingCycles.Value = 0;
            }

            if (defaultOptions.waterLevel.HasValue)
            {
                trackBarWaterLevel.Enabled = true;
                trackBarWaterLevel.Value = defaultOptions.waterLevel.Value;
                nUDWaterLevel.Enabled = true;
                nUDWaterLevel.Value = defaultOptions.waterLevel.Value;
            }
            else
            {
                trackBarWaterLevel.Enabled = false;
                trackBarWaterLevel.Value = 0;
                nUDWaterLevel.Enabled = false;
                nUDWaterLevel.Value = 0;
            }
        }

        private void connectButton_Click(object sender, EventArgs e)
        {
            Connect();
        }

        private void disconnectButton_Click(object sender, EventArgs e)
        {
            Disconnect();
        }
        private async void goToBootloaderButton_Click(object sender, EventArgs e)
        {
            await hardwareLibrary.SwitchToBootloaderAsync(token);
        }
    }
}
