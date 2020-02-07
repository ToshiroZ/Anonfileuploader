using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Anonfileuploader
{
    public partial class Form1 : Form
    {
        public Form1()
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
        public static string ApiKey { get; set; } = null;
        private void textBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var result = openFileDialog1.ShowDialog();
            bool wassuccess = result == DialogResult.OK;
            if(wassuccess)
            {
                textBox1.Text = openFileDialog1.FileName;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text == "")
            {
                MessageBox.Show("You need to select a file. Try double clicking on the text box");
                return;
            }
            else if(!File.Exists(textBox1.Text))
            {
                MessageBox.Show("You need to select a file. Try double clicking on the text box");
                return;
            }
            if(ApiKey == null)
            {
                var setter = new apikeysetter();
                var result = setter.ShowDialog(); // maybe check if the form was closed by the button idk
            }
            UploadFile(textBox1.Text);
        }
        private void UploadFile(string filepath)
        {
            using (WebClient client = new WebClient())
            {
                string formattedapikey = "?token=" + ApiKey;
                client.UploadFileAsync(new Uri("https://api.anonfile.com/upload" + formattedapikey), textBox1.Text);
                client.UploadFileCompleted += UploadFileCompleted;
                client.UploadProgressChanged += UploadProgressChanged;
            }
        }
        private void UploadProgressChanged(object sender, UploadProgressChangedEventArgs e)
        {
            progresslabel.Text = e.ProgressPercentage.ToString();
        }

        private void UploadFileCompleted(object sender, UploadFileCompletedEventArgs e)
        {
            progresslabel.Text = "";
            if(e.Error != null)
            {
                MessageBox.Show(e.Error.Message);
            }
            var response = Encoding.UTF8.GetString(e.Result);
            MessageBox.Show(response);
            if(response.Contains("error"))
            {
                var upload = Anon.AnonUploadError.FromJson(response);
                MessageBox.Show($"Upload Error \nReason: {upload.Error.Message}");
            }
            else
            {
                var upload = Anon.AnonUploadSuccess.FromJson(response);
                OpenUrl(upload.Data.File.Url.Short.ToString());
            }
            // MessageBox.Show("Upload Complete");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (File.Exists(Strings.DataFile))
            {
                ApiKey = File.ReadAllText(Strings.DataFile);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void OpenUrl(string url)
        {
            try
            {
                Process.Start(url);
            }
            catch
            {
                // hack because of this: https://github.com/dotnet/corefx/issues/10361
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    url = url.Replace("&", "^&");
                    Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Process.Start("xdg-open", url);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Process.Start("open", url);
                }
                else
                {
                    throw;
                }
            }
        }
        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (string file in files)
            {
                UploadFile(file);
            }
        }
    }
}

