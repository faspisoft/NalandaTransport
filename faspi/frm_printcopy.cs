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
    public partial class frm_printcopy : Form
    {
        string gmode = "";
        string gVid = "";
        int gVt_id=0;
        DataTable dtprintcopy;
        public string copyname1 = "", directmode = "";
        
        public frm_printcopy(string mode, string vid, int vt_id)
        {
            InitializeComponent();
            gmode = mode;
            gVid = vid;
            gVt_id = vt_id;
        }

        private void frm_printcopy_Load(object sender, EventArgs e)
        {
            if (directmode == "")
            {
                ansGridView5.Columns["defaultcopy"].HeaderText = gmode;
                if (gmode == "View")
                {
                    dtprintcopy = new DataTable();
                    Database.GetSqlData("Select printcopy from Vouchertypes where Vt_id=" + gVt_id, dtprintcopy);
                    String[] print_option = dtprintcopy.Rows[0]["printcopy"].ToString().Split(';');

                    for (int j = 0; j < print_option.Length; j++)
                    {
                        if (print_option[j] != "")
                        {
                            ansGridView5.Rows.Add();
                            String[] defaultcopy = print_option[j].Split(',');
                            ansGridView5.Rows[j].Cells["copyname"].Value = defaultcopy[0];
                            ansGridView5.Rows[j].Cells["defaultcopy"].Value = false;
                        }
                    }
                }

                if (gmode == "Print")
                {
                    dtprintcopy = new DataTable();
                    Database.GetSqlData("Select printcopy from Vouchertypes where Vt_id=" + gVt_id, dtprintcopy);
                    String[] print_option = dtprintcopy.Rows[0]["printcopy"].ToString().Split(';');

                    for (int j = 0; j < print_option.Length; j++)
                    {
                        if (print_option[j] != "")
                        {
                            ansGridView5.Rows.Add();
                            String[] defaultcopy = print_option[j].Split(',');
                            ansGridView5.Rows[j].Cells["copyname"].Value = defaultcopy[0];
                            ansGridView5.Rows[j].Cells["defaultcopy"].Value = defaultcopy[1];
                        }
                    }
                }
                button1.Text = gmode;
                button2.Visible = false;
            }
            else
            {
                button1.Visible = false;

                button2.Visible = true;

                if (gmode == "Print")
                {
                    dtprintcopy = new DataTable();
                    Database.GetSqlData("Select printcopy from Vouchertypes where Vt_id=" + gVt_id, dtprintcopy);
                    String[] print_option = dtprintcopy.Rows[0]["printcopy"].ToString().Split(';');

                    for (int j = 0; j < print_option.Length; j++)
                    {
                        if (print_option[j] != "")
                        {
                            ansGridView5.Rows.Add();
                            String[] defaultcopy = print_option[j].Split(',');
                            ansGridView5.Rows[j].Cells["copyname"].Value = defaultcopy[0];
                            ansGridView5.Rows[j].Cells["defaultcopy"].Value = defaultcopy[1];
                        }
                    }
                }
            }     
        }

        private void frm_printcopy_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
                this.Dispose();
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            for (int i = 0; i < ansGridView5.Rows.Count; i++)
            {
                OtherReport rpt = new OtherReport();
                if (bool.Parse(ansGridView5.Rows[i].Cells["defaultcopy"].Value.ToString()) == true)
                {
                    rpt.voucherprint(this, gVt_id, gVid, ansGridView5.Rows[i].Cells["copyname"].Value.ToString(), true, gmode);
                }
            }

            this.Close();
            this.Dispose();
        }

        private void ansGridView5_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void ansGridView5_SelectionChanged(object sender, EventArgs e)
        {
            if (gmode == "View")
            {
                if (ansGridView5.CurrentCell.OwningColumn.Name == "defaultcopy")
                {
                    for (int i = 0; i < ansGridView5.Rows.Count; i++)
                    {
                        ansGridView5.Rows[i].Cells["defaultcopy"].Value = false;
                    }
                    ansGridView5.CurrentRow.Cells["defaultcopy"].Value = true;

                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < ansGridView5.Rows.Count; i++)
            {
                if (bool.Parse(ansGridView5.Rows[i].Cells["defaultcopy"].Value.ToString()) == true)
                {
                    //counter++;
                    copyname1 += ansGridView5.Rows[i].Cells["copyname"].Value.ToString() + ";";
                }
            }

            this.Close();
            this.Dispose();
        }
    }
}
