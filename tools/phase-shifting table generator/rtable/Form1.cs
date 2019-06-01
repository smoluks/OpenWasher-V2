using System;
using System.Text;
using System.Windows.Forms;

namespace rtable
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var max = (double)numericUpDown1.Value;
            int steps = (int)numericUpDown2.Value;

            var array = GetArray(steps, max);
            textBox1.Clear();

            StringBuilder s = new StringBuilder($"const int a[{steps}] = {{ ");
            for (int i = 0; i < steps; i++)
            {
                s.Append(array[i]);
                s.Append(", ");
                if(i % 10 == 9)
                    s.Append("\n");
            }
            s.Append("};");
            textBox1.Text = s.ToString();
        }

        private ushort[] GetArray(int steps, double maxvalue)
        {
            var result = new ushort[steps];
            for(int i = 0; i < steps; i++)
            {
                result[i] = (ushort)(maxvalue - (GetValue((double)i * 2 / steps) * maxvalue));
            }
            return result;
        }

        private double GetValue(double level)
        {
            return Math.Acos(1 - level)/Math.PI;
        }
    }
}
