using OpenWasherHardwareLibrary;
using OpenWasherHardwareLibrary.Commands;
using OpenWasherHardwareLibrary.Enums;
using System;
using System.Windows.Forms;
using WindowsFormsClient.Entities;
using WindowsFormsClient.Managers;

namespace WindowsFormsClient
{
    public partial class Main : Form
    {
        readonly HardwareLibrary hardwareLibrary;
        readonly ConfigManager configManager = new ConfigManager();
        private bool isWashing;

        LogFrm logFrm;

        public Main()
        {
            InitializeComponent();
            hardwareLibrary = new HardwareLibrary(configManager.Port, MessageManager.MessageHandler, ErrorHandler, EventHandler, ConnectionEventHandler, configManager.LogEnable);
        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            hardwareLibrary.Dispose();
        }

        private async void timerPoll_Tick(object sender, EventArgs e)
        {
            try
            {
                var status = await hardwareLibrary.SendCommandAsync(new GetStatus());

                btnRunProgram.Enabled = true;
                lblTemp.Text = $"{status.temperature}°C";
                if(status.program != WashProgram.Nothing)
                {
                    SetStatusText(string.Format(
                        Localizator.GetString("Status_Washing", "{0}: {1}"),
                        Localizator.GetString($"Program_{status.program}", $"Program {status.program}"),
                        Localizator.GetString($"Stage_{status.stage}", $"Stage {status.stage}")));

                    groupBoxOptions.Enabled = false;
                }
                else if (isWashing)
                {
                    isWashing = false;
                    groupBoxOptions.Enabled = true;
                    SetStatusText(Localizator.GetString("Status_Stopped", "Stopped"));
                }
            }
            catch (TimeoutException)
            {
                btnRunProgram.Enabled = false;
                groupBoxOptions.Enabled = false;
                SetStatusText(Localizator.GetString("Status_ConnectionBreak", "Connection break"));
            }
        }

        private async void BtnRunProgram_Click(object sender, EventArgs e)
        {
            if (isWashing)
            {
            }
            else
            {
                if (listBoxPrograms.SelectedIndex != -1)
                {
                    var program = listBoxPrograms.SelectedItem as WashingProgram;
                    await hardwareLibrary.SendCommandAsync(new StartProgram(program.Program));
                    groupBoxOptions.Enabled = false;
                }
            }
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SettingsFrm settingsForm = new SettingsFrm(configManager);
            settingsForm.Show();
        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {

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

        private void LogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            logFrm = new LogFrm();
            logFrm.Show();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            foreach (var program in EnumManager.GetValues<WashProgram>())
            {
                listBoxPrograms.Items.Add(new WashingProgram()
                {
                    Program = program
                });
            }
        }

     }
}
