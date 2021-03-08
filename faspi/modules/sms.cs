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
using System.Data;

namespace faspi
{
    class sms
    {
        public String gmatter= "";
        public string AuthKey = "";
        public string SenderID = "";
        public string Footer = "";
        public string GPname = "";

        public string gph;
        public void send(String matter, string ph, string Pname)
        {

            System.Data.DataTable dtSmsInfo = new System.Data.DataTable();
           
            Database.GetSqlData("select * from smssetups", dtSmsInfo);
            
            
            if (dtSmsInfo.Rows.Count > 0)
            {
                AuthKey = dtSmsInfo.Rows[0]["uid"].ToString();
                SenderID = dtSmsInfo.Rows[0]["sender"].ToString();

                Footer = dtSmsInfo.Rows[0]["pin"].ToString();
                Footer = Footer.Replace(" ", "%20");
                Footer = Footer.Replace("(", "%28");
                Footer = Footer.Replace("(", "%29");
                Footer = Footer.Replace(",", "%2C");
                Footer = Footer.Replace(":", "%3a");

                gmatter = gmatter.Replace(" ", "%20");
                gmatter = gmatter.Replace("&", "%26");
                gmatter = gmatter.Replace("(", "%28");
                gmatter = gmatter.Replace("(", "%29");
                gmatter = gmatter.Replace(",", "%2C");
                gmatter = gmatter.Replace("\n", "%0A");
                gmatter = gmatter.Replace(":", "%3a");
            }
            else
            {
                return;
            }

            GPname = Pname;
            gmatter = matter;
            gph = ph;
            var backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += BackgroundWorkerDoWork;
            backgroundWorker.RunWorkerAsync();

        }
        private void BackgroundWorkerDoWork(object sender, DoWorkEventArgs e)
        {

            String address = "http://sms.faspi.in/rest/services/sendSMS/sendGroupSms?AUTH_KEY=" + AuthKey + "&message=" + gmatter + "%0A" + Footer + "&senderId=" + SenderID + "&routeId=1&mobileNos=" + gph + "&smsContentType=english";
           
            WebRequest webRequest = WebRequest.Create(address);
            webRequest.CachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);
            WebResponse webResponse;

            try
            {
                webResponse = webRequest.GetResponse();
                Stream stream = webResponse.GetResponseStream();
                StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                String str = reader.ReadToEnd();
                str=str.Split(':')[1].Split(',')[0].Replace("\"", "").ToString();
                string RCode = str;
                String Status = "";
                if (str == "3001")
                {
                    str = "Successfully Sent to Server";
                    Status = "Send";
                }
                else if (str == "3002")
                {
                    str = "Invalid URL";
                    Status = "Fail";
                }
                else if (str == "3003")
                {
                    str = "Invalid User/Password";
                    Status = "Fail";
                }
                else if (str == "3004")
                {
                    str = "Invalid Message Type";
                    Status = "Fail";
                }
                else if (str == "3005")
                {
                    str = "Invalid Message";
                    Status = "Fail";
                }
                else if (str == "3006")
                {
                    str = "Invalid Destination";
                    Status = "Fail";
                }
                else if (str == "3007")
                {
                    str = "Invalid Source";
                    Status = "Fail";
                }
                else if (str == "3008")
                {
                    str = "Invalid DLR Field";
                    Status = "Fail";
                }
                else if (str == "3009")
                {
                    str = "Authentication Failed";
                    Status = "Fail";
                }
                else if (str == "3010")
                {
                    str = "Internal Error";
                    Status = "Fail";
                }
                else if (str == "3011")
                {
                    str = "Insufficient Balance";
                    Status = "Fail";
                }
                else if (str == "3012")
                {
                    str = "Responce Time Out";
                    Status = "Fail";
                }
                else if (str == "3013")
                {
                    str = "Invalid Request Content Type";
                    Status = "Fail";
                }
                else if (str == "3014")
                {
                    str = "Missing Mobile Number";
                    Status = "Fail";
                }
                else if (str == "3015")
                {
                    str = "SMs Content for Approval";
                    Status = "Fail";
                }
                else if (str == "3016")
                {
                    str = "Missing Required Parameter";
                    Status = "Fail";
                }
                else if (str == "3017")
                {
                    str = "FAIL";
                    Status = "Fail";
                }
                else if (str == "3018")
                {
                    str = "Expired Account";
                    Status = "Fail";
                }
                else if (str == "3019")
                {
                    str = "Null Pointer Exception code";
                    Status = "Fail";
                }
                else if (str == "3020")
                {
                    str = "Empty User Name";
                    Status = "Fail";
                }
                else if (str == "3021")
                {
                    str = "Empty Password";
                    Status = "Fail";
                }

                else if (str == "3022")
                {
                    str = "User Name not Available";
                    Status = "Fail";
                }

                //DataTable dtsms = new DataTable("SMSLOG");
                //Database.GetSqlData("select * from SMSLOG where id=0", dtsms);
                //dtsms.Rows.Add();
                //dtsms.Rows[0]["AccName"] = GPname;
                //dtsms.Rows[0]["MNumber"] = gph;
                //dtsms.Rows[0]["Message"] = gmatter;
                //dtsms.Rows[0]["SDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
                //dtsms.Rows[0]["STime"] = DateTime.Now.ToString("HH:mm");
                //dtsms.Rows[0]["RCode"] = RCode;
                //dtsms.Rows[0]["RDesc"] = str;
                //dtsms.Rows[0]["Status"] = Status;
                //dtsms.Rows[0]["URL"] = address;

                //Database.SaveData(dtsms);

                funs.ShowBalloonTip("SMS To:" + GPname + "(" + gph + ")", str);
            }
            catch (Exception e1)
            {

                //DataTable dtsms = new DataTable("SMSLOG");
                //Database.GetSqlData("select * from SMSLOG where id=0", dtsms);
                //dtsms.Rows.Add();
                //dtsms.Rows[0]["AccName"] = GPname;
                //dtsms.Rows[0]["MNumber"] = gph;
                //dtsms.Rows[0]["Message"] = gmatter;
                //dtsms.Rows[0]["SDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
                //dtsms.Rows[0]["STime"] = DateTime.Now.ToString("HH:mm");
                //dtsms.Rows[0]["RCode"] = "0000";
                //dtsms.Rows[0]["RDesc"] = "Internet Problem";
                //dtsms.Rows[0]["Status"] = "Not Send";
                //dtsms.Rows[0]["URL"] = address;

                //Database.SaveData(dtsms);
                funs.ShowBalloonTip("SMS To:" + gph, "Not Send To Server, Check Your Network Connectivity");
                return;
            }
        }



        public string GetBal()
        {
            string res = "", abc = ""; 
            System.Data.DataTable dtSmsInfo = new System.Data.DataTable();
            Database.GetSqlData("select * from smssetup", dtSmsInfo);
            string AuthKey = "";

            if (dtSmsInfo.Rows.Count > 0)
            {
                AuthKey = dtSmsInfo.Rows[0]["uid"].ToString();

            }
            else
            {
                return res;
            }
            String address = "http://66.70.200.49/rest/services/sendSMS/getClientRouteBalance?AUTH_KEY=" + AuthKey + "&clientId=2191";
            WebRequest webRequest = WebRequest.Create(address);
            webRequest.CachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);
            WebResponse webResponse;

            try
            {
                webResponse = webRequest.GetResponse();
                Stream stream = webResponse.GetResponseStream();
                StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                String str1 = reader.ReadToEnd();

                for (int i = 0; i < str1.Split('{').Length; i++)
                {
                    if (str1.Split('{')[i].IndexOf("Transactional Route") == -1)
                    {
                        continue;
                    }
                    else
                    {
                        abc = str1.Split('{')[i].ToString();
                        abc = abc.Substring(abc.IndexOf("routeBalance") + 13, 6);
                        abc = abc.Substring(1, abc.IndexOf(',') - 1);
                        abc  =  abc + " SMS";
                    }

                }



            }
            catch (Exception e1)
            {
                MessageBox.Show("Check Your Network Connectivity.");
                return res;
            }


            return abc;
        } 



    }
}
