using OpenWasherHardwareLibrary.Entity;
using OpenWasherHardwareLibrary.Enums;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsClient.Entities;
using WindowsFormsClient.Managers;

namespace WindowsFormsClient
{
    public partial class Main : Form
    {
        // Объявляем делегат
        public delegate void NewLogMessageDelegate(Log log);
        // Событие, возникающее при выводе денег
        public event NewLogMessageDelegate newLogMessageEvent;

        async Task ConnectAsync()
        {
            lblStatus.Text = ResourceString.GetString("Status_Connecting", "Connecting");            
            try
            {
                string port;

                if (_config.Port == "AUTO")
                {
                    port = await _hardwareLibrary.ConnectAsync();
                    if (port == null)
                    {
                        lblStatus.Text = ResourceString.GetString("Status_NotFound", "Not found");
                        trayIcon.Text = ResourceString.GetString("Status_NotFound", "Not found");
                        return;
                    }
                }
                else
                {
                    port = _config.Port;
                    await _hardwareLibrary.ConnectAsync(port);
                }

                lblStatus.Text = string.Format(ResourceString.GetString("Status_Connected", "{0} Connected"), port);
                btnStart.Enabled = true;
                trayIcon.Text = string.Format(ResourceString.GetString("Status_Connected", "{0} Connected"), port);
                timerPoll.Enabled = true;
            }
            catch(Exception e)
            {
                MessageBox.Show($"Connection failed: {e.Message}");
            }            
        }

        void Disconnect()
        {
            timerPoll.Enabled = false;
            _hardwareLibrary.Disconnect();

            lblStatus.Text = string.Format(ResourceString.GetString("Status_Disconnected", "Disconnected"));
            btnStart.Enabled = false;
            trayIcon.Text = string.Format(ResourceString.GetString("Status_Disconnected", "Disconnected"));
        }

        void DisconnectInvoke()
        {
            timerPoll.Enabled = false;
            _hardwareLibrary.Disconnect();

            this.Invoke((MethodInvoker)(() => lblStatus.Text = string.Format(ResourceString.GetString("Status_Disconnected", "Disconnected"))));
            this.Invoke((MethodInvoker)(() => btnStart.Enabled = false));
            this.Invoke((MethodInvoker)(() => trayIcon.Text = string.Format(ResourceString.GetString("Status_Disconnected", "Disconnected"))));
        }

        async Task<Status> GetStatusAsync()
        {
            return await _hardwareLibrary.GetStatusAsync();
        }

        private void EventHandler(EventType type, byte[] data)
        {
            switch(type)
            {
                case EventType.PowerOn:
                    break;
                case EventType.StartProgram:
                    break;
                case EventType.StopProgram:
                    ShowMessageInvoke("Program stopped");
                    break;
                case EventType.BreakProgram:
                    ShowMessageInvoke("Program breaked");
                    break;
                case EventType.GoToBootloader:
                    DisconnectInvoke();
                    break;
            }
        }

        private void ErrorHandler(ErrorType type, byte[] data)
        {
            ShowErrorкMessageInvoke(EnumManager.GetEnumDescription(type));
        }

        private void MessageHandler(string message)
        {
            var log = new Log(message);
            _logs.Add(log);
            logFrm?.newLogMessage(log);
        }

        private void ShowMessageInvoke(string text)
        {
            this.Invoke((MethodInvoker)(() => MessageBox.Show(text)));
        }

        private void ShowErrorкMessageInvoke(string text)
        {
            this.Invoke((MethodInvoker)(() => MessageBox.Show(text, "Critical failure", MessageBoxButtons.OK, MessageBoxIcon.Error)));
        }
    }
}
