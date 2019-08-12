using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using WindowsFormsClient.Managers;

namespace WindowsFormsClient
{
    internal partial class LogFrm : Form
    {
        public LogFrm()
        {
            InitializeComponent();   
        }

        private void LogFrm_Load(object sender, EventArgs e)
        {
            MessageManager.NewLogMessageEvent += newMessageHandler;
            foreach (var log in MessageManager.GetMessages())
            {
                listBoxLogs.Items.Add(log);
            }
        }

        private void LogFrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            MessageManager.NewLogMessageEvent += newMessageHandler;
        }

        private void newMessageHandler(Entities.Message log)
        {
            listBoxLogs.Invoke((MethodInvoker)(() => listBoxLogs.Items.Add(log)));
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            listBoxLogs.Items.Clear();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (saveLogFileDialog.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllLines(saveLogFileDialog.FileName, listBoxLogs.Items.Cast<Message>().Select(x => x.ToString()));
            }
        }
    }
}
