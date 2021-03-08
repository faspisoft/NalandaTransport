using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace faspi
{
    class AutoUpdater
    {
        public static string Gstr = "";
        public static Version InstldVersion = new Version(Database.ExeDate.ToString("yy.M.d"));
        public static Version CurrentVersion = new Version();
        public static string DialogTitle = "";
        public static string ChangeLog = "";
        public static string Url = "";

        public static void Start(string str)
        {
            Gstr = str;
            var backgroundWorker = new BackgroundWorker();
       
            backgroundWorker.DoWork += BackgroundWorkerDoWork;
            backgroundWorker.RunWorkerAsync();
        }

        private static void BackgroundWorkerDoWork(object sender,DoWorkEventArgs e)
        {
            WebRequest webRequest = WebRequest.Create(Gstr);
            webRequest.CachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);
            WebResponse webResponse;

            try
            {               
                webResponse = webRequest.GetResponse();
            }
            catch (Exception e1)
            {
                MessageBox.Show("Check Your Network Connectivity." + e1.Message);
                return;
            }
            Stream CastStream = webResponse.GetResponseStream();
            var RecCastDoc = new XmlDocument();
            if (CastStream != null)
            {
                RecCastDoc.Load(CastStream);
            }
            else
            {
                return;
            }

            XmlNodeList CastItems = RecCastDoc.SelectNodes("item");
            if (CastItems != null)
            {
                foreach (XmlNode item in CastItems)
                {
                    XmlNode CastVersion = item.SelectSingleNode("version");
                    if (CastVersion != null)
                    {
                        string Version = CastVersion.InnerText;
                        CurrentVersion = new Version(Version);

                    }
                    else
                    {
                        continue;
                    }
                    XmlNode CastTitle = item.SelectSingleNode("title");
                    DialogTitle = CastTitle != null ? CastTitle.InnerText : "";

                    XmlNode CastChangeLog = item.SelectSingleNode("changelog");
                    ChangeLog = CastChangeLog != null ? CastChangeLog.InnerText : "";

                    XmlNode CastUrl = item.SelectSingleNode("url");
                    Url = CastUrl != null ? CastUrl.InnerText : "";
                }
                if (CurrentVersion >= InstldVersion)
                {
                    var thread = new Thread(ShowUI);
                    thread.SetApartmentState(ApartmentState.STA);
                    thread.Start();
                }
                else
                {
                    MessageBox.Show("No New Update is Available!" + Environment.NewLine + Environment.NewLine + "You are Using Latest Version of Software", "Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private static void ShowUI()
        {
            var UpdateForm = new Update();
            UpdateForm.LoadData(InstldVersion, CurrentVersion, DialogTitle, ChangeLog, Url);
            UpdateForm.ShowDialog();
        }
    }
}
