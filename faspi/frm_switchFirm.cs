using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace faspi
{
    public partial class frm_switchFirm : Form
    {
        DataTable dt;

        public frm_switchFirm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentCell.Value != null)
            {
                Database.LocationId = Database.GetScalarText("select LocationId from location where nick_name='" + dataGridView1.CurrentCell.Value.ToString() + "'");
                Database.LocationNikName = dataGridView1.CurrentCell.Value.ToString();
                Database.LocationCashAcc_id = Database.GetScalarText("select cashac_id from location where nick_name='" + dataGridView1.CurrentCell.Value.ToString() + "'");
                Database.LocationExpAcc_id = Database.GetScalarText("select expenseacc from location where nick_name='" + dataGridView1.CurrentCell.Value.ToString() + "'");
                this.Close();
                this.Dispose();
                //try
                //{
                //    double.Parse(funs.IndianCurr(123));
                //   Database.trimno = 1;
                //}
                //catch (Exception ex)
                //{
                //    Database.trimno = 2;
                //}


                //string stateid = Database.GetScalarText("Select State_id from location where nick_name='" + Database.LocationNikName + "'");
                //string CompanyStation_id = Database.GetScalarText("Select DP_id from location where nick_name='" + Database.LocationNikName + "'");




                //Database.setVariable(CompanyStation_id, stateid, Database.fname, Database.fyear, Database.uname, Database.upass, Database.utype, Database.databaseName, Database.stDate, Database.enDate, funs.Select_user_id(Database.uname));
                
            }
        }

        private void frm_switchFirm_Load(object sender, EventArgs e)
        {
            dt = new DataTable();
            Database.GetSqlData("SELECT nick_name FROM Location ORDER BY nick_name", dt);
            dataGridView1.DataSource = dt;
        }
    }
}
