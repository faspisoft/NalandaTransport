using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Forms;
using Microsoft.Win32;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Text;

namespace faspi
{
    public partial class Update : Form
    {
        string GUrl = "";
        public Update()
        {
            InitializeComponent();
            
        }

        public void LoadData(Version InstldVersion, Version CurrentVersion, string DialogTitle, string ChangeLog, string Url)
        {
            GUrl = Url;
            this.Text = DialogTitle;
            label1.Text = string.Format(label1.Text, "Marwari Transport Pro.");
            label2.Text = string.Format(label2.Text, "Marwari Transport Pro.", CurrentVersion, InstldVersion, Environment.NewLine);
            webBrowser1.Navigate(ChangeLog);
        }

        public override sealed  string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //GetExeUpdateInfo();
            
            var downloadDialog = new DownloadUpdateDialog(GUrl);
            try
            {
                downloadDialog.ShowDialog();
            }
            catch
            {

            }
        }

  
        //private void GetExeUpdateInfo()
        //{
        //    String address = "http://www.faspi.in/faspidata/exeupdateinfo_get.php?Dongle_no="+ Dongle.getDongleNumber()+ "";
        //    //String address = "http://localhost/faspidata/exeupdateinfo.php?Dongle_no="+ Dongle.getDongleNumber() + " ";
        //    WebRequest webRequest = WebRequest.Create(address);
        //    webRequest.CachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);
        //    WebResponse webResponse;


        //    try
        //    {
        //        webResponse = webRequest.GetResponse();
        //    }
        //    catch (Exception e1)
        //    {
        //        MessageBox.Show("Check Your Network Connectivity.");
        //        return;
        //    }


        //    Stream stream = webResponse.GetResponseStream();
        //    StreamReader reader = new StreamReader(stream, Encoding.UTF8);
        //    String str = reader.ReadToEnd();

        //    str = str.Replace("\n", "");
        //    str = str.Replace("\t", "");
           
        //}

    }
}
