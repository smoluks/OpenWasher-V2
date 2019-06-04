using System;
using System.Collections.Generic;
using System.Windows.Forms;
using WindowsFormsClient.Entities;

namespace WindowsFormsClient
{
    internal partial class LogFrm : Form
    {
        private readonly List<Log> logs;

        public LogFrm(List<Log> logs)
        {
            InitializeComponent();
            this.logs = logs;      
        }

        private void LogFrm_Load(object sender, EventArgs e)
        {
            foreach(var log in logs)
            {
                listBoxLogs.Items.Add(log);
            }
        }

        internal void newLogMessage(Log log)
        {
            listBoxLogs.Invoke((MethodInvoker)(() => listBoxLogs.Items.Add(log)));
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            listBoxLogs.Items.Clear();
        }
    }
}
