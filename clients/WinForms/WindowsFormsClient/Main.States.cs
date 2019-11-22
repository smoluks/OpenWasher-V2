using OpenWasherHardwareLibrary.Enums;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsClient
{
    public partial class Main
    {
        CancellationTokenSource connectCancelTokenSource;
        CancellationToken connectToken;

        void Connect()
        {
            connectCancelTokenSource = new CancellationTokenSource();
            connectToken = connectCancelTokenSource.Token;

            connectButton.Visible = false;
            breakConnectButton.Visible = true;

            SetStatusText(localizator.GetString("Status_Connecting", "Connecting"));

            Task.Run(async () =>
            {
                try
                {
                    var (result, port, error) = await hardwareLibrary.ConnectAsync(connectToken, configManager.CurrentConfig.Port.ToUpper() == "AUTO" ? null : configManager.CurrentConfig.Port);

                    switch (result)
                    {
                        case ConnectionEventType.Connected:
                            SetStatusText(string.Format(localizator.GetString("Status_Connected", "{0} Connected"), port));
                            btnRunProgram.Enabled = true;
                            timerPoll.Enabled = true;
                            connectButton.Enabled = false;
                            disconnectButton.Enabled = true;
                            goToBootloaderButton.Enabled = true;
                            timerPoll_Tick(null, null);
                            break;
                        case ConnectionEventType.ConnectFailed:
                            SetStatusText(string.Format(localizator.GetString("Status_ConnectFailed", "Connection error: {0}"), error));
                            break;
                        case ConnectionEventType.NotFound:
                            SetStatusText(localizator.GetString("Status_NotFound", "Washer not found"));
                            break;
                    }
                }
                catch(Exception e)
                {
                    SetStatusText(localizator.GetString("Status_NotFound", "Washer not found"));
                }
                finally
                {
                    this.Invoke((MethodInvoker)(() =>
                    {
                        connectButton.Visible = true;
                        breakConnectButton.Visible = false;
                    }));
                }
            });
        }

        private void breakConnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            connectCancelTokenSource?.Cancel();
        }

        void Disconnect()
        {
            timerPoll.Enabled = false;
            btnRunProgram.Enabled = false;
            groupBoxOptions.Enabled = false;
            connectButton.Enabled = true;
            disconnectButton.Enabled = false;
            goToBootloaderButton.Enabled = false;
            cancelTokenSource.Cancel();
            hardwareLibrary.Disconnect();
        }
    }
}
