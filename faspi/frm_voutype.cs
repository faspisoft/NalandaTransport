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
    public partial class frm_voutype : Form
    {
        DataTable dt;
       
        public frm_voutype()
        {
            InitializeComponent();
        }

        private void frm_voutype_Load(object sender, EventArgs e)
        {
            dt = new DataTable();
            
                Database.GetSqlData("SELECT AliasName, Name, Short FROM VOUCHERTYPEs where A=" + access_sql.Singlequote + "true" + access_sql.Singlequote + " and Type<>'Report' ORDER BY AliasName, Name, Short", dt);
            
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dataGridView1.Rows.Add();
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells["Select"].Value = true;
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells["name"].Value = dt.Rows[i]["Name"].ToString();
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells["shrt"].Value = dt.Rows[i]["Short"].ToString();
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells["aliasname"].Value = dt.Rows[i]["AliasName"].ToString();
            }
            dateTimePicker1.MinDate = Database.stDate;
            dateTimePicker1.MaxDate = Database.ldate;
            dateTimePicker1.CustomFormat = Database.dformat;
            dateTimePicker1.Value = Database.ldate;
            dateTimePicker2.MinDate = Database.stDate;
            dateTimePicker2.MaxDate = Database.ldate;
            dateTimePicker2.CustomFormat = Database.dformat;
            dateTimePicker2.Value = Database.ldate;
        }

        private void frm_voutype_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
                this.Dispose();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string str = "";
            string loc = "";
            if (textBox11.Text != "")
            {
                loc = "  VOUCHERINFOs.LocationId = '" + funs.Select_locationId(textBox11.Text) + "'  And ";
            }
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (bool.Parse(dataGridView1.Rows[i].Cells["Select"].Value.ToString()) == true)
                {
                    int id = funs.Select_vt_id(dataGridView1.Rows[i].Cells["name"].Value.ToString());
                    if (id > 0)
                    {
                        str = str + " Or VOUCHERINFOs.Vt_id=" + id;
                    }
                }
            }



            if (str.Length > 5)
            {
                str = str.Remove(0, 4);
            }

            Report gg = new Report();
            gg.MdiParent = this.MdiParent;
            gg.Journal(dateTimePicker1.Value, dateTimePicker2.Value, str,loc);
            gg.Show();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dataGridView1.Rows[i].Cells["Select"].Value = true;
                }
            }
            else if (checkBox1.Checked == false)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dataGridView1.Rows[i].Cells["Select"].Value = false;
                }
            }
        }

        private void textBox11_KeyPress(object sender, KeyPressEventArgs e)
        {
            string strCombo = "SELECT nick_name as Location FROM Location ORDER BY nick_name";
            textBox11.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);

        }

        private void textBox11_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox11);
        }

        private void textBox11_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox11);
        }

        private void textBox11_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }
    }
}
