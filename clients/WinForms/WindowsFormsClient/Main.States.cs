using OpenWasherHardwareLibrary.Enums;
using System.Threading.Tasks;

namespace WindowsFormsClient
{
    public partial class Main
    {
        void Connect()
        {
            SetStatusText(localizator.GetString("Status_Connecting", "Connecting"));

            Task.Run(async () =>
            {
                var (result, port, error) = await hardwareLibrary.ConnectAsync(token, configManager.CurrentConfig.Port.ToUpper() == "AUTO" ? null : configManager.CurrentConfig.Port);

                switch (result)
                {
                    case ConnectionEventType.Connected:
                        SetStatusText(string.Format(localizator.GetString("Status_Connected", "{0} Connected"), port));
                        btnRunProgram.Enabled = true;
                        timerPoll.Enabled = true;
                        timerPoll_Tick(null, null);
                        break;
                    case ConnectionEventType.ConnectFailed:
                        SetStatusText(string.Format(localizator.GetString("Status_ConnectFailed", "Connection error: {0}"), error));
                        break;
                    case ConnectionEventType.NotFound:
                        SetStatusText(localizator.GetString("Status_NotFound", "Washer not found"));
                        break;
                }
            });
        }

        void Disconnect()
        {
            timerPoll.Enabled = false;
            btnRunProgram.Enabled = false;
            groupBoxOptions.Enabled = false;
            cancelTokenSource.Cancel();
            hardwareLibrary.Disconnect();
        }
    }
}
