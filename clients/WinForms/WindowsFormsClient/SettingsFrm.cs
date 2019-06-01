using System.Windows.Forms;
using WindowsFormsClient.Managers;

namespace WindowsFormsClient
{
    internal partial class SettingsFrm : Form
    {
        private ConfigManager _config;

        internal SettingsFrm()
        {
            InitializeComponent();
        }

        internal SettingsFrm(ConfigManager config)
        {
            _config = config;
        }
    }
}
