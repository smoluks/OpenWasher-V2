using OpenWasherClient.Entities;
using OpenWasherHardwareLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using WindowsFormsClient.Managers;

namespace WindowsFormsClient
{
    internal partial class SettingsFrm : Form
    {
        private readonly Config newConfig;

        internal delegate void ApplyNewConfigDelegate(Config newConfig);
        private readonly ApplyNewConfigDelegate configChangeCallback;

        public bool isChanged { get; private set; }

        internal SettingsFrm(Config config, ApplyNewConfigDelegate configChangeCallback)
        {
            this.configChangeCallback = configChangeCallback;
            this.newConfig = config.Clone();
            InitializeComponent();
        }

        private void SettingsFrm_Load(object sender, System.EventArgs e)
        {
            var ports = new List<string>() { "AUTO" };
            ports.AddRange(HardwareLibrary.GetComPorts());
            comboBoxPort.Items.AddRange(ports.ToArray());
            comboBoxPort.SelectedItem = ports.Where(x => x == newConfig.Port).FirstOrDefault();

            var languages = Localizator.GetAvaliableLanguages();
            comboBoxLanguage.Items.AddRange(languages.ToArray());
            comboBoxLanguage.SelectedItem = languages.Where(x => x == newConfig.Locale).FirstOrDefault();

            checkBoxLog.Checked = newConfig.LogEnable;
        }

        private void ButtonOK_Click(object sender, System.EventArgs e)
        {
            if(isChanged)
                configChangeCallback?.Invoke(newConfig);

            this.Close();
        }

        private void ButtonCancel_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void ComboBoxPort_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if ((string)comboBoxPort.SelectedItem == newConfig.Port)
                return;

            newConfig.Port = (string)comboBoxPort.SelectedItem;
            isChanged = true;                       
        }

        private void ComboBoxLanguage_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if ((string)comboBoxLanguage.SelectedItem == newConfig.Locale)
                return;

            newConfig.Locale = (string)comboBoxLanguage.SelectedItem;
            isChanged = true;
        }

        private void CheckBoxLog_CheckedChanged(object sender, System.EventArgs e)
        {
            if (checkBoxLog.Checked == newConfig.LogEnable)
                return;

            newConfig.LogEnable = checkBoxLog.Checked;
            isChanged = true;
        }
    }
}
