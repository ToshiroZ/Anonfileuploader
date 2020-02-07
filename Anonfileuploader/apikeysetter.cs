using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Anonfileuploader
{
    public partial class apikeysetter : Form
    {
        public apikeysetter()
        {
            InitializeComponent();
        }

        private void apikeysetter_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        { 
            if(textBox1.Text == "")
            {
                MessageBox.Show("You need to input your api key.");
                return;
            }
            Form1.ApiKey = textBox1.Text.Trim();
            if(!Directory.Exists(Strings.DataDirectory))
            {
                Directory.CreateDirectory(Strings.DataDirectory);
            }
            if(!File.Exists(Strings.DataFile))
            {
                File.WriteAllText(Strings.DataFile, textBox1.Text);
            }
            this.Close();
        }
    }
}
