using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CrystalDecisions.Shared;
namespace faspi
{
    public partial class showReport : Form
    {
        //public showReport(Object rptsrc,Object pf)
        //{
        //    InitializeComponent();
        //    crystalReportViewer1.ShowRefreshButton = false;
        //    crystalReportViewer1.ParameterFieldInfo = (ParameterFields) pf;
        //    crystalReportViewer1.ReportSource=rptsrc;
            
        //}
        public showReport()
        {
            InitializeComponent();
            crystalReportViewer1.ShowRefreshButton = false;
            
        }

        private void showReport_Load(object sender, EventArgs e)
        {
             
            if (Feature.Available("Data Export") == "No")
            {
                crystalReportViewer1.ShowExportButton = false;
            }
        }

        private void crystalReportViewer1_Drill(object source, CrystalDecisions.Windows.Forms.DrillEventArgs e)
        {
            //MessageBox.Show(e.ToString());
        }

        

        private void crystalReportViewer1_MouseClick(object sender, MouseEventArgs e)
        {
            e.Location.ToString();
        }

        private void showReport_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Dispose();
            GC.Collect();
        }
        
    }
}
