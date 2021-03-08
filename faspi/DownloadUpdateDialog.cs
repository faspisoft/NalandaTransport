using System;

using System.ComponentModel;
using System.Net.Cache;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Diagnostics;

namespace faspi
{
    
    public partial class DownloadUpdateDialog : Form
    {
        string GDownloadURL="";
        string path;
        WebClient webclient;
        public DownloadUpdateDialog(string DownloadURL)
        {
            InitializeComponent();
            GDownloadURL = DownloadURL;
            LoadData();
        }

        private void LoadData()
        {
            webclient= new WebClient();
            var uri = new Uri(GDownloadURL);
            if (File.Exists(Database.ServerPath + "\\"+ "Update.exe")==true)
            {
                File.Delete(Database.ServerPath + "\\"+ "Update.exe");
            }
            path = string.Format(@"{0}{1}", Database.ServerPath + "\\", "Update.exe");
            webclient.DownloadProgressChanged+= OnDownloadProgressChanged;
            webclient.DownloadFileCompleted += OnDownloadFileCompleted;
            webclient.DownloadFileAsync(uri, path);
        }
        
        private void OnDownloadProgressChanged(object sender,DownloadProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
            if (e.ProgressPercentage < 100)
            {
                return;
            }

            string batchfile = Database.ServerPath + "\\Update.bat";
            if (File.Exists(batchfile) == false)
            {

                if (File.Exists(batchfile) == false)
                {
                    StreamWriter sw = new StreamWriter(batchfile);
                    string str = "@echo off" + Environment.NewLine;
                    str = str + "set wait=1" + Environment.NewLine;
                    str = str + "echo Updating..." + Environment.NewLine;
                    str = str + "echo wscript.sleep %wait%000 > wait.vbs" + Environment.NewLine;
                    str = str + "wscript.exe wait.vbs" + Environment.NewLine;
                    str = str + "del wait.vbs" + Environment.NewLine;
                    str = str + "copy Update.exe Marwari.exe" + Environment.NewLine;
                    str = str + "start Marwari.exe" + Environment.NewLine;
                    str = str + "exit" + Environment.NewLine;
                    sw.WriteLine(str);
                    sw.Close();
                }
            }

            System.Diagnostics.Process.Start(batchfile);

            Environment.Exit(0);

        }
        private void OnDownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {

            Environment.Exit(0);

        }
    }
    
}
