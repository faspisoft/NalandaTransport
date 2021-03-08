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
    public partial class frm_deliveryrpt : Form
    {
        string strCombo = "";
        public frm_deliveryrpt()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (textBox11.Text == "")
            {
                MessageBox.Show("Select Location");
            }
            else
            {
                string str = "";



                if (textBox10.Text.Trim() != "")
                {
                    str = str + " and Stocks.GRNo = '" + textBox10.Text + "'";
                }


                if (textBox5.Text.Trim() != "")
                {
                    str = str + " and ACCOUNTs.name = '" + textBox5.Text + "'";
                }
                if (textBox8.Text.Trim() != "")
                {
                    str = str + " and ACCOUNTs_1.name = '" + textBox8.Text + "'";
                }
                if (textBox4.Text.Trim() != "")
                {
                    str = str + " and DeliveryPoints.Name = '" + textBox4.Text + "'";
                }
               
                
                if (textBox7.Text.Trim() != "")
                {
                    str = str + " and VOUCHERINFOs_1.Remarks  = '" + textBox7.Text + "'";
                }

                if (textBox1.Text.Trim() != "")
                {
                    str = str + " and VOUCHERINFOs_1.Paymentmode  = '" + textBox1.Text + "'";
                }
                if (textBox2.Text.Trim() != "")
                {
                    str = str + " and DeliveredBys.Name   = '" + textBox2.Text + "'";
                }

                Report gg = new Report();
                gg.Delivery(dateTimePicker1.Value, dateTimePicker2.Value,funs.Select_locationId(textBox11.Text),str);
                gg.MdiParent = this.MdiParent;
                gg.Show();
            }
               
        }

        private void textBox11_KeyPress(object sender, KeyPressEventArgs e)
        {
            string strCombo = "SELECT nick_name as Location FROM Location ORDER BY nick_name";
            textBox11.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);

        }

        private void frm_deliveryrpt_Load(object sender, EventArgs e)
        {
            textBox11.Text = funs.Select_location_name(Database.LocationId);
            this.Text = "Delivery Report";
            dateTimePicker1.CustomFormat = Database.dformat;
            dateTimePicker2.CustomFormat = Database.dformat;
            dateTimePicker1.Value = Database.stDate;
            dateTimePicker2.Value = Database.ldate;
            dateTimePicker1.MinDate = Database.stDate;
            dateTimePicker2.MinDate = Database.stDate;
            dateTimePicker1.MaxDate = Database.ldate;
            dateTimePicker2.MaxDate = Database.ldate;
        }

        private void textBox8_KeyPress(object sender, KeyPressEventArgs e)
        {
            strCombo = "SELECT ACCOUNTs.Name, ACCOUNTs.Printname, DeliveryPoints.Name AS Station, ACCOUNTs.Address1, ACCOUNTs.Address2, ACCOUNTs.Phone, ACCOUNTs.Tin_number, OTHERs.Name AS Staff, CONTRACTORs.Name AS Agent FROM ACCOUNTs LEFT OUTER JOIN CONTRACTORs ON ACCOUNTs.Con_id = CONTRACTORs.Name LEFT OUTER JOIN OTHERs ON ACCOUNTs.Loc_id = OTHERs.Oth_id LEFT OUTER JOIN DeliveryPoints ON ACCOUNTs.SId = DeliveryPoints.DPId WHERE ACCOUNTs.Act_id = 39 ORDER BY ACCOUNTs.Name";
            textBox8.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 2);
        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            strCombo = "SELECT ACCOUNTs.Name, ACCOUNTs.Printname, DeliveryPoints.Name AS Station, ACCOUNTs.Address1, ACCOUNTs.Address2, ACCOUNTs.Phone, ACCOUNTs.Tin_number, OTHERs.Name AS Staff, CONTRACTORs.Name AS Agent FROM ACCOUNTs LEFT OUTER JOIN CONTRACTORs ON ACCOUNTs.Con_id = CONTRACTORs.Name LEFT OUTER JOIN OTHERs ON ACCOUNTs.Loc_id = OTHERs.Oth_id LEFT OUTER JOIN DeliveryPoints ON ACCOUNTs.SId = DeliveryPoints.DPId WHERE ACCOUNTs.Act_id = 39 ORDER BY ACCOUNTs.Name";
            textBox5.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 2);
        }

      

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            strCombo = "SELECT [name] from DeliveryPoints";
            textBox4.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
        }

        private void textBox10_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox7_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void dateTimePicker1_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void dateTimePicker2_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            DataTable dtcombo = new DataTable();
            dtcombo.Columns.Add("Mode of Payment", typeof(string));

            dtcombo.Columns["Mode of Payment"].ColumnName = "Mode of Payment";

            dtcombo.Rows.Add();
            dtcombo.Rows[0][0] = "Cash";

            dtcombo.Rows.Add();
            dtcombo.Rows[1][0] = "Credit";


            textBox1.Text = SelectCombo.ComboDt(this, dtcombo, 0);
           // SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox1);
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox1);
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            string cmbVouTyp = "SELECT name from DeliveredBys where locationid='" + funs.Select_locationId(textBox11.Text) + "' order by Name";
            textBox2.Text = SelectCombo.ComboKeypress(this, e.KeyChar, cmbVouTyp, textBox2.Text, 0);
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox2);
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox2);
        }

      

       
    }
}
