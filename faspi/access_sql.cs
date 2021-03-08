using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Data.OleDb;

namespace faspi
{
    class access_sql
    {
        public static string accbalq = "";
        public static String Hash = "";
        public static String Singlequote = "";
        public static String QryJournal = "";
        public static String Docnumber = "";
        public static String Docnumber1 = "";
        public static String EditVoucher = "";
        public static String Svnum = "";
        public static String IsNull = "";
        public static String Concat = "";
        public static String DateFormat = "";

        public static void setconnection()
        {
            try
            {
                FileInfo fInfo = new FileInfo(Application.StartupPath + "\\connect.ini");
                if (fInfo.Exists)
                {
                    Database.inipathfile = System.IO.File.ReadAllText(Application.StartupPath + "\\connect.ini").Replace("\n", "");
                    Database.inipathfile = Database.inipathfile.Replace("\r", "");
                    if (Database.inipathfile == "access;")
                    {
                        TextWriter tw = new StreamWriter(Application.StartupPath + "\\connect.ini");
                        tw.WriteLine("access;loginfo;SER;");
                        tw.Close();
                    }
                }

                else
                {
                    //create ini file with text
                    File.Create(Application.StartupPath + "\\connect.ini").Dispose();
                    TextWriter tw = new StreamWriter(Application.StartupPath + "\\connect.ini");
                    tw.WriteLine("access;loginfo;SER;");
                    tw.Close();
                    Database.inipathfile = System.IO.File.ReadAllText(Application.StartupPath + "\\connect.ini").Replace("\n", "");
                    Database.inipathfile = Database.inipathfile.Replace("\r", "");
                }

                string stradd = System.IO.File.ReadAllText(Application.StartupPath + "\\connect.ini").Replace("\n", "");
                String[] val = stradd.Replace("\r", "").Split(';');

                int l = val.Length;
                Database.DatabaseType = val[0];
                Database.inipath = val[1];
                Database.sqlseverpwd = val[3];
                Database.sqlseveruser = val[2];
                Database.databaseName = val[4];
                if (l == 6)
                {
                    File.AppendAllText(Application.StartupPath + "\\connect.ini", ";DOS");
                    Database.printtype = "DOS";
                }
                else
                {
                    Database.printtype = val[6];
                }

                Activate();
                Feature();
            
            }
            catch (Exception ex)
            {
                MessageBox.Show("Connection File is not Available.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                Environment.Exit(0);
            }
        }

        public static void Activate()
        {
            DataTable dtbranch = new DataTable();
            Database.GetSqlData("SELECT 1 AS Expr1 FROM  sys.tables WHERE (name = 'ACTIVATE')", dtbranch);
            if (dtbranch.Rows.Count == 0)
            {
                if (Database.CommandExecutor("create table ACTIVATE (id int Identity ,[Column] nvarchar(50),[Value] nvarchar(50),DisplayToUser bit  CONSTRAINT Act PRIMARY KEY(id))") == true)
                {


                }
            }

        }
        public static void Feature()
        {
            DataTable dtbranch = new DataTable();
            Database.GetSqlData("SELECT 1 AS Expr1 FROM  sys.tables WHERE (name = 'Feature')", dtbranch);
            if (dtbranch.Rows.Count == 0)
            {
                if (Database.CommandExecutor("create table Feature (id int Identity ,[Features] nvarchar(50),Active bit  CONSTRAINT Fet PRIMARY KEY(Id))") == true)
                {


                }
            }

        }

        public static void fnhashSinglequote()
        {
            Hash = "'";
            Singlequote = "'";
            Docnumber = " VOUCHERTYPEs.Short + ' ' + CONVERT(nvarchar, VOUCHERINFOs.Vdate, 112) + ' ' + CAST(VOUCHERINFOs.Vnumber AS nvarchar(10)) ";
            Svnum = "' Bill No.' + Svnum + ' Dt. ' + CONVERT(nvarchar,Svdate, 106)";
            DateFormat = "CONVERT(nvarchar,Voucherinfos.Vdate, 112)";
            Docnumber1 = " VOUCHERTYPEs_1.Short + ' ' + CONVERT(nvarchar,JOURNAL_1.Vdate, 112) + ' ' + CAST(VOUCHERINFOs_1.Vnumber AS nvarchar(10)) AS DocNumber2 ";
            EditVoucher = "select CAST(act_id AS nvarchar(10)) As Code,Name as AccountType from accountypes where type='Account'";
            IsNull = " Is ";
            Concat = " + ";
        }

        public static string fnaccbal()
        {
            accbalq = " Case when Sum(balance.Dr)>Sum(balance.Cr) then cast((Sum(balance.Dr)-Sum(balance.Cr)) as nvarchar(20)) + ' Dr.'  else cast((Sum(balance.Cr)-Sum(balance.Dr))  as nvarchar(20)) + ' Cr.'  End as Balance ";
            return accbalq;
        }

        public static string fnstring(string con, string first, string second)
        {
            string res = "case when " + con + " then " + first + " Else " + second + " End ";
            return res;
        }

        public static string fnDatFormatting(string Fieldname, string format)
        {
            string res = "CONVERT(nvarchar, " + Fieldname + ", 106) ";
            return res;
        }
    }
}