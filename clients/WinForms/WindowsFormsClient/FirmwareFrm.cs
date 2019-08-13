using Coshx.IntelHexParser;
using OpenWasherHardwareLibrary;
using System;
using System.IO;
using System.Windows.Forms;

namespace WindowsFormsClient
{
    public partial class FirmwareFrm : Form
    {
        private HardwareLibrary hardwareLibrary;
        private Serializer serializer = new Serializer();
        private byte[] firmware;
        private bool loaded;

        public FirmwareFrm(HardwareLibrary hardwareLibrary)
        {
            InitializeComponent();
            this.hardwareLibrary = hardwareLibrary;
        }

        private void ButtonInstallFirmware_Click(object sender, System.EventArgs e)
        {
            if(loaded)
                hardwareLibrary.updateFirmware(firmware);
        }

        private void ButtonOpenFirmware_Click(object sender, System.EventArgs e)
        {
            if(firmwareOpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var intelHex = File.ReadAllLines(firmwareOpenFileDialog.FileName);
                    var binaries = serializer.Deserialize(intelHex);
                    if(!binaries.ContainsKey(0x8))
                    {
                        MessageBox.Show("Segment 0x80000 not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    firmware = binaries[0x8];
                    textBoxFilePath.Text = firmwareOpenFileDialog.FileName;
                    loaded = true;
                }
                catch(Exception ex)
                {
                    loaded = false;
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
