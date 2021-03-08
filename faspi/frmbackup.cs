using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data.OleDb;
using System.Data.SqlClient;

namespace faspi
{
    public partial class frmbackup : Form
    {
        DataTable dtnew;
        DataTable dtFirmBackup;
        public String frmMenuTyp;
        public bool ret;
        OleDbConnection AccessCnnSource;
        OleDbConnection AccessCnnDest;
        OleDbDataAdapter da;
        OleDbCommand comm;
        OleDbDataReader dr;
        SqlConnection SqlCnnSource;
        SqlConnection SqlCnnDest;
        SqlDataAdapter Sqlda;
        SqlCommand Sqlcomm;
        SqlDataAdapter Sqldr;

        public frmbackup()
        {
            InitializeComponent();
        }

        private void frmbackup_Load(object sender, EventArgs e)
        {
            DataTable dtsamecre = new DataTable();
            dtsamecre.Columns.Add("f_id", typeof(int));
            dtFirmBackup = new DataTable("firminfo");
            Database.GetOtherSqlData("select f_id,Firm_name+'['+Firm_Period_name+']' as Firm,Firm_database as Firmdb from firminfo  where Gststatus=" + access_sql.Singlequote + "True" + access_sql.Singlequote + " order by Firm_name,Firm_Period_name desc", dtFirmBackup);

            for (int i = 0; i < dtFirmBackup.Rows.Count; i++)
            {
                if (Database.DatabaseType == "access")
                {
                    DataTable dttemp = new DataTable();
                    int fid = int.Parse(dtFirmBackup.Rows[i]["f_id"].ToString());
                    String DestdbName = dtFirmBackup.Rows[i]["Firmdb"].ToString();
              
                    AccessCnnDest = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Application.StartupPath + "\\Database\\" + DestdbName + ".mdb;Persist Security Info=true;Jet OLEDB:Database Password=ptsoft9358524971");
                    AccessCnnDest.Open();
                    da = new OleDbDataAdapter("select * from Userinfo  where Uname='" +Database.uname+ "' and Upass='"+ Database.upass+"'", AccessCnnDest);
                    da.Fill(dttemp);
                    if (dttemp.Rows.Count != 0)
                    {
                        dtsamecre.Rows.Add();
                        dtsamecre.Rows[dtsamecre.Rows.Count - 1]["f_id"] = fid;
                    }

                    AccessCnnDest.Close();
                }
                else
                {
                    DataTable dttemp = new DataTable();
                    int fid = int.Parse(dtFirmBackup.Rows[i]["f_id"].ToString());
                    String DestdbName = dtFirmBackup.Rows[i]["Firmdb"].ToString();
              
                    SqlCnnDest = new SqlConnection("Data Source=" + Database.inipath + ";Initial Catalog=" + DestdbName + ";Persist Security Info=True;User ID=sa;password=" + Database.sqlseverpwd + ";Connection Timeout=100");
                    SqlCnnDest.Open();

                    Sqlda = new SqlDataAdapter("select * from Userinfo  where Uname='" + Database.uname + "' and Upass='" + Database.upass + "'", SqlCnnDest);
                    Sqlda.Fill(dttemp);
                    if (dttemp.Rows.Count != 0)
                    {
                        dtsamecre.Rows.Add();
                        dtsamecre.Rows[dtsamecre.Rows.Count - 1]["f_id"] = fid;
                    }
                    SqlCnnDest.Close();
                }
            }

            dtnew = new DataTable();
            dtnew.Columns.Add("f_id",typeof(int));
            dtnew.Columns.Add("Firm", typeof(string));

            for (int i = 0; i < dtsamecre.Rows.Count; i++)
            {
                dtnew.Rows.Add();
                dtnew.Rows[dtnew.Rows.Count - 1]["f_id"] = dtsamecre.Rows[i]["f_id"].ToString();
                dtnew.Rows[dtnew.Rows.Count-1]["Firm"] =  Database.GetOtherScalarText("Select Firm_name+'['+Firm_Period_name+']' as Firm  from Firminfo where  f_id=" + dtsamecre.Rows[i]["f_id"].ToString());
               

                
            }





            ansGridView1.DataSource = dtnew;
            ansGridView1.Columns["Firm"].Width = 340;
            ansGridView1.Columns["f_id"].Visible = false;
           
            foreach(DataGridViewColumn column in ansGridView1.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            if (frmMenuTyp == "Backup")
            {
                Button1.Text = "Backup";
                groupBox3.Visible = false;
            }
           
            else if (frmMenuTyp == "Modify")
            {
                Button1.Text = "Modify";
                groupBox3.Visible = false;
            }
            else if (frmMenuTyp == "Use")
            {
                Button1.Text = "Ok";
                groupBox3.Visible = true;
            }
            else if (frmMenuTyp == "Delete")
            {
                Button1.Text = "Delete";
                groupBox3.Visible = false;
            }
            dateTimePicker1.CustomFormat = Database.dformat;
           // this.Size = this.MdiParent.Size;
           // SideFill();
        }



        private void Button1_Click(object sender, EventArgs e)
        {
            DataTable dtDbName = new DataTable("firminfo");

            Database.GetOtherSqlData("select * from firminfo where f_id=" + dtnew.Rows[ansGridView1.SelectedCells[0].RowIndex]["f_id"], dtDbName);

            if (frmMenuTyp == "Backup")
            {
                MessageBox.Show(" Database Backup Can Not Take On Window Drive ");
                saveFileDialog1.Filter = "Text files (*.bak)|*.bak|All files (*.*)|*.*";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        Database.CommandExecutor("Backup database " + dtDbName.Rows[0]["Firm_database"] + " to disk='" + saveFileDialog1.FileName + "'");
                        MessageBox.Show("Database BackUp has been created successful.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }

            else if (frmMenuTyp == "Use")
            {
                if (Validate() == true)
                {
                    String strCmd = "SELECT FIRMINFO.Firm_name, FIRMINFO.Firm_database, FIRMINFO.Firm_Period_name,Firm_odate,Firm_edate FROM FIRMINFO  WHERE FIRMINFO.f_id=" + dtnew.Rows[ansGridView1.SelectedCells[0].RowIndex]["f_id"];
                    DataTable dtInfo = new DataTable();
                    Database.GetOtherSqlData(strCmd, dtInfo);
                    Database.ldate = dateTimePicker1.Value;
                   
                    ret = true;
                    this.Close();
                }
            }

        }

        //private void SideFill()
        //{
        //    flowLayoutPanel1.Controls.Clear();
        //    DataTable dtsidefill = new DataTable();
        //    dtsidefill.Columns.Add("Name", typeof(string));
        //    dtsidefill.Columns.Add("DisplayName", typeof(string));
        //    dtsidefill.Columns.Add("ShortcutKey", typeof(string));
        //    dtsidefill.Columns.Add("Visible", typeof(bool));
        //    //save
        //    dtsidefill.Rows.Add();
        //    dtsidefill.Rows[0]["Name"] = "save";
        //    dtsidefill.Rows[0]["DisplayName"] = "Save";
        //    dtsidefill.Rows[0]["ShortcutKey"] = "^S";
        //    dtsidefill.Rows[0]["Visible"] = true;



        //    //close
        //    dtsidefill.Rows.Add();
        //    dtsidefill.Rows[1]["Name"] = "quit";
        //    dtsidefill.Rows[1]["DisplayName"] = "Quit";
        //    dtsidefill.Rows[1]["ShortcutKey"] = "Esc";
        //    dtsidefill.Rows[1]["Visible"] = true;






        //    for (int i = 0; i < dtsidefill.Rows.Count; i++)
        //    {


        //        if (bool.Parse(dtsidefill.Rows[i]["Visible"].ToString()) == true)
        //        {

        //            Button btn = new Button();
        //            btn.Size = new Size(150, 30);
        //            btn.Name = dtsidefill.Rows[i]["Name"].ToString();
        //            btn.Text = "";


        //            Bitmap bmp = new Bitmap(btn.ClientRectangle.Width, btn.ClientRectangle.Height);
        //            Graphics G = Graphics.FromImage(bmp);
        //            G.Clear(btn.BackColor);
        //            string line1 = dtsidefill.Rows[i]["ShortcutKey"].ToString();
        //            string line2 = dtsidefill.Rows[i]["DisplayName"].ToString();

        //            StringFormat SF = new StringFormat();
        //            SF.Alignment = StringAlignment.Near;
        //            SF.LineAlignment = StringAlignment.Center;
        //            Rectangle RC = btn.ClientRectangle;
        //            Font font = new Font("Arial", 12);
        //            G.DrawString(line1, font, Brushes.Red, RC, SF);
        //            G.DrawString("".PadLeft(line1.Length * 2 + 1) + line2, font, Brushes.Black, RC, SF);

        //            btn.Image = bmp;

        //            btn.Click += new EventHandler(btn_Click);
        //            flowLayoutPanel1.Controls.Add(btn);
        //        }

        //    }


        //}
        //void btn_Click(object sender, EventArgs e)
        //{
        //    Button tbtn = (Button)sender;
        //    string name = tbtn.Name.ToString();

        //    if (name == "save")
        //    {
        //        this.Close();
        //        this.Dispose();
        //    }

        //    if (name == "quit")
        //    {
        //        this.Close();
        //        this.Dispose();
        //    }


        //}

        private void Button2_Click(object sender, EventArgs e)
        {
            ret = false;
            this.Close();
        }

        private void frmbackup_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2 )
            {
                Button1_Click(sender, e);
            }
            
        }

        private void frmbackup_FormClosing(object sender, FormClosingEventArgs e)
        {
            //ret = false;
        }

        private void dateTimePicker1_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void ansGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Button1_Click(sender, e);
        }

        private void ansGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            DataTable dtdate = new DataTable();

            Database.GetOtherSqlData("select * from firminfo where f_id=" + dtnew.Rows[e.RowIndex]["f_id"], dtdate);
            DateTime dtfrom = DateTime.Parse(dtdate.Rows[0]["Firm_odate"].ToString());
            DateTime dtto = DateTime.Parse(dtdate.Rows[0]["Firm_edate"].ToString());
            if (Database.ldate >= dtfrom && Database.ldate <= dtto)
            {
                dateTimePicker1.Value = Database.ldate;
            }
            else if(dtto >= DateTime.Today)
            {
                dateTimePicker1.Value = DateTime.Today;
            }
            else
            {
                dateTimePicker1.Value = dtto;
            }
        }


        private bool Validate()
        {
            DataTable dtdate = new DataTable();

            Database.GetOtherSqlData("select * from firminfo where f_id=" + dtnew.Rows[ansGridView1.SelectedCells[0].RowIndex]["f_id"], dtdate);
            DateTime dtfrom = DateTime.Parse(dtdate.Rows[0]["Firm_odate"].ToString());
            DateTime dtto = DateTime.Parse(dtdate.Rows[0]["Firm_edate"].ToString());
            if (dateTimePicker1.Value < dtfrom)
            {
                return false;
            }
            if (dateTimePicker1.Value > dtto)
            {
                return false;
            }

            return true;
        }
        
    }
}
