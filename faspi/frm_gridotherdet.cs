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
    public partial class frm_gridotherdet : Form
    {
        DataTable dt;
        public frm_gridotherdet()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        private void frm_gridotherdet_Load(object sender, EventArgs e)
        {
            dt = new DataTable("TransposrtDetails");
            Database.GetSqlData("Select * from TransportDetails order by FName", dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ansGridView1.Rows.Add();
                ansGridView1.Rows[i].Cells["SNo"].Value = (i + 1);
                ansGridView1.Rows[i].Cells["fName"].Value = dt.Rows[i]["FName"].ToString();
                ansGridView1.Rows[i].Cells["sname"].Value = dt.Rows[i]["ShowingName"].ToString();
                ansGridView1.Rows[i].Cells["status"].Value = dt.Rows[i]["status"].ToString();
            }
        }

        private void ansGridView1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsLetter(e.KeyChar) || char.IsNumber(e.KeyChar) || e.KeyChar == ' ')
            {
                if (ansGridView1.CurrentCell.OwningColumn.Name == "status")
                {
                    DataTable dtcombo = new DataTable();
                    dtcombo.Columns.Add("Status", typeof(string));

                    dtcombo.Rows.Add();
                    dtcombo.Rows[0][0] = "Inside";

                    dtcombo.Rows.Add();
                    dtcombo.Rows[1][0] = "Outside";

                    dtcombo.Rows.Add();
                    dtcombo.Rows[2][0] = "Not Visible";

                    string selected = SelectCombo.ComboDt(this, dtcombo, 0);
                    if (selected == "" || selected == null)
                    {
                        selected = "Not Visible";
                    }
                    ansGridView1.CurrentCell.Value = selected;
                }
            }
        }

        private void save()
        {
            DataTable dtTemp = new DataTable("TransportDetails");
            Database.GetSqlData("select * from TransportDetails", dtTemp);
            for (int i = 0; i < dtTemp.Rows.Count; i++)
            {
                dtTemp.Rows[i].Delete();
            }
            Database.SaveData(dtTemp);

            dt = new DataTable("TransportDetails");
            Database.GetSqlData("Select * from Transportdetails", dt);

            for (int i = 0; i < ansGridView1.Rows.Count; i++)
            {
                dt.Rows.Add();
                dt.Rows[i]["FName"] = ansGridView1.Rows[i].Cells["fname"].Value.ToString();
                dt.Rows[i]["ShowingName"] = ansGridView1.Rows[i].Cells["sname"].Value.ToString();
                dt.Rows[i]["status"] = ansGridView1.Rows[i].Cells["status"].Value.ToString();
            }
            Database.SaveData(dt);
            funs.ShowBalloonTip("Save", "Saved Successfully");
            this.Close();
            this.Dispose();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            save();
        }

        private void frm_gridotherdet_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
                this.Dispose();
            }
            else if (e.Control && e.KeyCode == Keys.S)
            {
                save();
            }
        }
    }
}
