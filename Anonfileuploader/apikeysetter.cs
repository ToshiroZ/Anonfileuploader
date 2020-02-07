using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
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
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        private void Form1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
        private void apikeysetter_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("You need to input your api key.");
                return;
            }
            Form1.ApiKey = textBox1.Text.Trim();
            if (!Directory.Exists(Strings.DataDirectory))
            {
                Directory.CreateDirectory(Strings.DataDirectory);
            }
            if (!File.Exists(Strings.DataFile))
            {
                File.WriteAllText(Strings.DataFile, textBox1.Text);
            }
            this.Close();
        }

    }
}
