using OpenWasherHardwareLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using WindowsFormsClient.Managers;

namespace WindowsFormsClient
{
    internal partial class SettingsFrm : Form
    {
        private ConfigManager _config;

        internal SettingsFrm(ConfigManager config)
        {
            _config = config;
            InitializeComponent();
        }

        private void SettingsFrm_Load(object sender, System.EventArgs e)
        {
            var ports = new List<string>() { "AUTO" };
            ports.AddRange(HardwareLibrary.GetComPorts());

            comboBoxPort.Items.AddRange(ports.ToArray());
            comboBoxPort.SelectedItem = ports.Where(x => x == _config.Port).FirstOrDefault();
        }
    }
}
