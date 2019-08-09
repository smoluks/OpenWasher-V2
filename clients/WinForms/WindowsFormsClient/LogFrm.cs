using System;
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

        private void newMessageHandler(Entities.Message log)
        {
            listBoxLogs.Invoke((MethodInvoker)(() => listBoxLogs.Items.Add(log)));
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            listBoxLogs.Items.Clear();
        }

        private void LogFrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            MessageManager.NewLogMessageEvent += newMessageHandler;
        }
    }
}
