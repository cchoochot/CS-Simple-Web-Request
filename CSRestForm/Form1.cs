using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSRestForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            txtResponse.Text = "Downloading...";

            var client = new WebClient();

            client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(HandleDownload);

            var uri = new Uri(txtUrl.Text);
            client.DownloadStringAsync(uri);
        }

        private void HandleDownload(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                txtResponse.Text = "Error: " + e.Error.Message;
                return;
            }

            txtResponse.Text = e.Result;
        }




        private async void Button2_ClickAsync(object sender, EventArgs e)
        {
            txtResponse.Text = "Loading...";

            var app = new HttpRequest(txtUrl.Text);
            app.WebRequestCompleted += HandleUpdateResponseText;

            await app.InvokeAsync();
        }

        private void HandleUpdateResponseText(object sender, EventArgs e)
        {
            var evt = (WebRequestCompletedEventArgs)e;

            //TextBox.CheckForIllegalCrossThreadCalls = false;
            txtResponse.Text = evt.Content;
        }
    }
}
