using OpenWasherHardwareLibrary;
using OpenWasherHardwareLibrary.Entity;
using OpenWasherHardwareLibrary.Enums;
using System;
using System.Windows.Forms;
using WindowsFormsClient.Managers;

namespace WindowsFormsClient
{
    public partial class StartFrm : Form
    {
        HardwareLibrary _hardwareLibrary;

        public StartFrm(HardwareLibrary hardwareLibrary)
        {
            InitializeComponent();
            _hardwareLibrary = hardwareLibrary;
        }

        private void StartFrm_Load(object sender, EventArgs e)
        {
            groupBox.Height = checkBoxAdditional.Checked ? 280 : 18;
            this.Height = checkBoxAdditional.Checked ? 367 : 110;
            foreach (Programs program in Enum.GetValues(typeof(Programs)))
            {
                cbPrograms.Items.Add(ResourceString.GetString($"Program_{(int)program}", EnumManager.GetEnumDescription(program)));
            }
        }

        private void CbPrograms_SelectedIndexChanged(object sender, EventArgs e)
        {
            toolTip.SetToolTip(cbPrograms, ResourceString.GetString($"Program_{cbPrograms.SelectedIndex}_Comment"));
        }

        private void CheckBoxAdditional_CheckedChanged_1(object sender, EventArgs e)
        {
            groupBox.Enabled = checkBoxAdditional.Checked;
            groupBox.Height = checkBoxAdditional.Checked ? 280 : 18;
            this.Height = checkBoxAdditional.Checked ? 367 : 110;
        }

        private async void BtnRunProgram_Click(object sender, EventArgs e)
        {
            if (cbPrograms.SelectedIndex == -1)
                return;

            this.Enabled = false;

            if (!checkBoxAdditional.Checked)
                await _hardwareLibrary.StartProgramAsync((Programs)cbPrograms.SelectedIndex);
            else
                await _hardwareLibrary.StartProgramAsync((Programs)cbPrograms.SelectedIndex
                    , new ProgramOptions
                    {
                        temperature = cbTemperature.Checked ? (byte)nUDTemperature.Value : (byte?)null,
                        duration = cbDuration.Checked ? (byte)nUDDuration.Value : (byte?)null,
                        spinningspeed = cbSpinningSpeed. Checked ? (byte)nUDSpinningSpeed.Value : (byte?)null,
                        waterlevel = cbWaterLevel.Checked ? (byte)nUDWaterLevel.Value : (byte?)null,
                        rinsingCycles = cbRinsingCycles.Checked ? (byte)nUDRinsingCycles.Value : (byte?)null,
                    });

            this.Close();
        }

        private void TrackBarTemperature_Scroll(object sender, EventArgs e)
        {
            nUDTemperature.Value = trackBarTemperature.Value;
        }

        private void TrackBarDuration_Scroll(object sender, EventArgs e)
        {
            nUDDuration.Value = trackBarDuration.Value;
        }

        private void TrackBarSpinningSpeed_Scroll(object sender, EventArgs e)
        {
            nUDSpinningSpeed.Value = trackBarSpinningSpeed.Value;
        }

        private void TrackBarRinsingCycles_Scroll(object sender, EventArgs e)
        {
            nUDRinsingCycles.Value = trackBarRinsingCycles.Value;
        }

        private void TrackBarWaterLevel_Scroll(object sender, EventArgs e)
        {
            nUDWaterLevel.Value = trackBarWaterLevel.Value;
        }
    }
}
