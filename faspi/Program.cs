using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DeviceId;
using RestSharp;

namespace faspi
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            access_sql.setconnection();
           
            if (MarwariCRM.Validate() == 0)
            {
                MessageBox.Show("System Not Registered or Licence is Not Active");
                Environment.Exit(0);
            }
            else
            {
                Application.Run(new frmLogin());
            }
        }


    

        static void Loaddll()
        {
            CrystalDecisions.CrystalReports.Engine.ReportDocument rptOther = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
            try
            {
                rptOther.Load(AppDomain.CurrentDomain.BaseDirectory + "\\Report.net\\LadgerA5.rpt");
            }
            catch
            {
                MessageBox.Show("Error: Loading Crystal Report Dll");
            }       
        }
    }
}
