using OpenWasherHardwareLibrary.Enums;
using System.Windows.Forms;

namespace WindowsFormsClient
{
    public partial class Main
    {
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
                        this.Disconnect();
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
