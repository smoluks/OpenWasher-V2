using OpenWasherHardwareLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using WindowsFormsClient.Managers;

namespace WindowsFormsClient
{
    internal partial class SettingsFrm : Form
    {
        private ConfigManager config;

        internal SettingsFrm(ConfigManager config)
        {
            this.config = config;
            InitializeComponent();
        }

        private void SettingsFrm_Load(object sender, System.EventArgs e)
        {
            var ports = new List<string>() { "AUTO" };
            ports.AddRange(HardwareLibrary.GetComPorts());
            comboBoxPort.Items.AddRange(ports.ToArray());
            comboBoxPort.SelectedItem = ports.Where(x => x == config.Port).FirstOrDefault();

            var languages = Localizator.GetAvaliableLanguages();
            comboBoxLanguage.Items.AddRange(languages.ToArray());
            comboBoxLanguage.SelectedItem = languages.Where(x => x == config.Locale).FirstOrDefault();

            checkBoxLog.Checked = config.LogEnable;
        }

        private void ButtonOK_Click(object sender, System.EventArgs e)
        {
            config.Port = (string)comboBoxPort.SelectedItem;
            config.Locale = (string)comboBoxLanguage.SelectedItem;
            config.LogEnable = checkBoxLog.Checked;
            config.Save();

            this.Close();
        }

        private void ButtonCancel_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }
    }
}
