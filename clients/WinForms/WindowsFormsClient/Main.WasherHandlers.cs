using OpenWasherHardwareLibrary.Enums;
using System.Windows.Forms;

namespace WindowsFormsClient
{
    public partial class Main : Form
    {
        private void ConnectionEventHandler(ConnectionEventType type, string text)
        {
            this.Invoke((MethodInvoker)(() =>
            {
                switch (type)
                {
                    case ConnectionEventType.Connected:
                        SetStatusText(string.Format(localizator.GetString("Status_Connected", "{0} Connected"), text));
                        btnRunProgram.Enabled = true;
                        timerPoll.Enabled = true;
                        timerPoll_Tick(null, null);
                        break;
                    case ConnectionEventType.ConnectFailed:
                        SetStatusText(localizator.GetString("Status_ConnectFailed", text));
                        break;
                    case ConnectionEventType.NotFound:
                        SetStatusText(localizator.GetString("Status_NotFound", "Not found"));
                        break;
                }
            }));
        }

        private void EventHandler(EventType type, byte[] data)
        {
            this.Invoke((MethodInvoker)(() =>
            {
                switch (type)
                {
                    case EventType.PowerOn:
                        break;
                    case EventType.StartProgram:
                        //after reset (on firmware update & smth else)
                        groupBoxOptions.Enabled = false;
                        isWashing = true;
                        break;
                    case EventType.StopProgram:
                        groupBoxOptions.Enabled = true;
                        isWashing = false;
                        break;
                    case EventType.BreakProgram:
                        groupBoxOptions.Enabled = true;
                        isWashing = false;
                        break;
                    case EventType.GoToBootloader:
                        this.Close();
                        isWashing = false;
                        break;
                }
            }));
        }

        private void ErrorHandler(ErrorType error, byte[] data)
        {
            string text = $"Error: {error}";
            this.Invoke((MethodInvoker)(() =>
            {
                SetStatusText(text);
                MessageBox.Show(text, "Critical failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }));
        }

        private void SetStatusText(string text)
        {
            lblStatus.Text = text;
            trayIcon.Text = text;
        }
    }
}
