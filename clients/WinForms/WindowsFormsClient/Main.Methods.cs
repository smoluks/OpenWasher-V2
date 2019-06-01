using OpenWasherHardwareLibrary.Entity;
using OpenWasherHardwareLibrary.Enums;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsClient.Managers;

namespace WindowsFormsClient
{
    public partial class Main : Form
    {
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
                    break;
                case EventType.BreakProgram:
                    break;
                case EventType.GoToBootloader:
                    Disconnect();
                    break;
            }
        }

        private void ErrorHandler(ErrorType type, byte[] data)
        {
            MessageBox.Show(EnumManager.GetEnumDescription(type));
        }

        private class Log
        {
            string message;
            DateTime timestamp;

            public Log(string message)
            {
                this.message = message;
                timestamp = DateTime.Now;
            }
        }

        private void MessageHandler(string message)
        {
            _logs.Add(new Log(message));
        }
    }
}
