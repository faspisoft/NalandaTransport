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
    public partial class frm_selector : Form
    {
        string strCombo = "";
        public string typ = "";

        public frm_selector()
        {
            InitializeComponent();
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox1);
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            strCombo = "SELECT ACCOUNTs.name, ACCOUNTs.Printname, DeliveryPoints.Name AS Station, ACCOUNTs.Address1, ACCOUNTs.Address2,  ACCOUNTs.phone, ACCOUNTs.tin_number, OTHERs.Name AS Staff, CONTRACTORs.Name AS Agent, ACCOUNTs.ac_id FROM ACCOUNTs LEFT OUTER JOIN  ACCOUNTYPEs ON ACCOUNTs.act_id = ACCOUNTYPEs.Act_id LEFT OUTER JOIN CONTRACTORs ON ACCOUNTs.con_id = CONTRACTORs.Name LEFT OUTER JOIN OTHERs ON ACCOUNTs.loc_id = OTHERs.Oth_id LEFT OUTER JOIN DeliveryPoints ON ACCOUNTs.SId = DeliveryPoints.DPId WHERE ( ACCOUNTYPEs.Path LIKE '1;39;%') ORDER BY ACCOUNTs.name ";
                        
            textBox1.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 2);
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox1);
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox2);
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            strCombo = "SELECT ACCOUNTs.name, ACCOUNTs.Printname, DeliveryPoints.Name AS Station, ACCOUNTs.Address1, ACCOUNTs.Address2,  ACCOUNTs.phone, ACCOUNTs.tin_number, OTHERs.Name AS Staff, CONTRACTORs.Name AS Agent, ACCOUNTs.ac_id FROM ACCOUNTs LEFT OUTER JOIN  ACCOUNTYPEs ON ACCOUNTs.act_id = ACCOUNTYPEs.Act_id LEFT OUTER JOIN CONTRACTORs ON ACCOUNTs.con_id = CONTRACTORs.Name LEFT OUTER JOIN OTHERs ON ACCOUNTs.loc_id = OTHERs.Oth_id LEFT OUTER JOIN DeliveryPoints ON ACCOUNTs.SId = DeliveryPoints.DPId WHERE ( ACCOUNTYPEs.Path LIKE '1;39;%') ORDER BY ACCOUNTs.name ";
            textBox2.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 2);
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox2);
        }

        private void textBox3_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox3);
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            strCombo = "SELECT [name] from DeliveryPoints";
            textBox3.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
        }

        private void textBox3_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox3);
        }

        private void textBox4_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox4);
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            strCombo = "SELECT [name] from DeliveryPoints";
            textBox4.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
        }

        private void textBox4_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox4);
        }

        private void textBox25_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox25);
        }

        private void textBox25_KeyPress(object sender, KeyPressEventArgs e)
        {
            DataTable dtcombo = new DataTable();
            dtcombo.Columns.Add("DeliveryType", typeof(string));

            dtcombo.Columns["DeliveryType"].ColumnName = "DeliveryType";

            dtcombo.Rows.Add();
            dtcombo.Rows[0][0] = "Godown";

            dtcombo.Rows.Add();
            dtcombo.Rows[1][0] = "Door Delivery";

            textBox25.Text = SelectCombo.ComboDt(this, dtcombo, 0);
            SendKeys.Send("{tab}"); 
        }

        private void textBox25_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox25);
        }

        private void textBox24_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox24);
        }

        private void textBox24_KeyPress(object sender, KeyPressEventArgs e)
        {
            DataTable dtcombo = new DataTable();
            dtcombo.Columns.Add("PaymentMode", typeof(string));

            dtcombo.Columns["PaymentMode"].ColumnName = "PaymentMode";

            dtcombo.Rows.Add();
            dtcombo.Rows[0][0] = "Paid";
            dtcombo.Rows.Add();
            dtcombo.Rows[1][0] = "FOC";
            dtcombo.Rows.Add();
            dtcombo.Rows[2][0] = "T.B.B.";
            dtcombo.Rows.Add();
            dtcombo.Rows[3][0] = "To Pay";

            textBox24.Text = SelectCombo.ComboDt(this, dtcombo, 0);
            SendKeys.Send("{tab}");
        }

        private void textBox24_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox24);
        }

        private void textBox5_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox5);
        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void textBox5_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox5);
        }

        private void textBox6_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox6);
        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void textBox6_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox6);
        }

        private void textBox7_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox7);
        }

        private void textBox7_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void textBox7_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox7);
        }

        private void textBox8_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox8);
        }

        private void textBox8_KeyPress(object sender, KeyPressEventArgs e)
        {
            strCombo = "SELECT name FROM items ORDER BY name";
            textBox8.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
        }

        private void textBox8_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox8);
        }

        private void textBox9_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox9);
        }

        private void textBox9_KeyPress(object sender, KeyPressEventArgs e)
        {
            strCombo = "select Name from packings order by Name";
            textBox9.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
        }

        private void textBox9_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox9);
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            string str = "", str2 = "";

            //if (textBox10.Text.Trim() != "")
            //{
            //    str = str + " and VOUCHERINFOs_1.Invoiceno = '" + textBox10.Text + "'";
            //}
            //if (textBox13.Text.Trim() != "")
            //{
            //    str = str + " and VOUCHERINFOs.Invoiceno = '" + textBox13.Text + "'";
            //}
            //if (textBox1.Text.Trim() != "")
            //{
            //    str = str + " and ACCOUNTs.name = '" + textBox1.Text + "'";
            //}
            //if (textBox2.Text.Trim() != "")
            //{
            //    str = str + " and ACCOUNTs_1.name = '" + textBox2.Text + "'";
            //}
            //if (textBox3.Text.Trim() != "")
            //{
            //    str = str + " and DeliveryPoints.Name = '" + textBox3.Text + "'";
            //}
            //if (textBox4.Text.Trim() != "")
            //{
            //    str = str + " and DeliveryPoints_1.Name = '" + textBox4.Text + "'";
            //}
            //if (textBox25.Text.Trim() != "")
            //{
            //    str = str + " and VOUCHERINFOs_1.DeliveryType = '" + textBox25.Text + "'";
            //}
            //if (textBox24.Text.Trim() != "")
            //{
            //    str = str + " and VOUCHERINFOs_1.PaymentMode = '" + textBox24.Text + "'";
            //}

            //if (textBox5.Text.Trim() != "")
            //{
            //    str = str + " and Voucherdets.Quantity = " + textBox5.Text;
            //}
            //if (textBox6.Text.Trim() != "")
            //{
            //    str = str + " and VOUCHERINFOs_1.Transport1 = '" + textBox6.Text + "'";
            //}
            //if (textBox7.Text.Trim() != "")
            //{
            //    str = str + " and VOUCHERINFOs_1.Transport5 = '" + textBox7.Text + "'";
            //}
            //if (textBox8.Text.Trim() != "")
            //{
            //    str = str + " and Voucherdets_1.Description = '" + textBox8.Text + "'";
            //}
            //if (textBox9.Text.Trim() != "")
            //{
            //    str = str + " and Voucherdets_1.packing = '" + textBox9.Text + "'";
            //}
            //if (textBox11.Text.Trim() != "")
            //{
            //    str = str + " AND VOUCHERINFOs_1.LocationId = '" + funs.Select_locationId(textBox11.Text) + "'";
            //}
            //if (radioButton2.Checked == true)
            //{
            //    str = str + " and VOUCHERINFOs.Invoiceno IS NULL";
            //}
            //else  if (radioButton3.Checked == true)
            //{
            //    str = str + " and VOUCHERINFOs.Invoiceno IS NOT NULL";
            //}
            //if (textBox12.Text.Trim() != "")
            //{
            //    str2 = " (ACCOUNTs.name = '" + textBox12.Text + "' or ACCOUNTs_1.name = '" + textBox12.Text + "') and ";
            //}
            if (textBox10.Text.Trim() != "")
            {
                str = str + " and Stocks.GRNo = '" + textBox10.Text + "'";
            }
            if (textBox13.Text.Trim() != "")
            {
                str = str + " and VOUCHERINFOs.Invoiceno = '" + textBox13.Text + "'";
            }
            if (textBox1.Text.Trim() != "")
            {
                str = str + " and ACCOUNTs.name = '" + textBox1.Text + "'";
            }
            if (textBox2.Text.Trim() != "")
            {
                str = str + " and ACCOUNTs_1.name = '" + textBox2.Text + "'";
            }
            if (textBox3.Text.Trim() != "")
            {
                str = str + " and DeliveryPoints.Name = '" + textBox3.Text + "'";
            }
            if (textBox4.Text.Trim() != "")
            {
                str = str + " and DeliveryPoints_1.Name = '" + textBox4.Text + "'";
            }
            if (textBox25.Text.Trim() != "")
            {
                str = str + " and Stocks.DeliveryType = '" + textBox25.Text + "'";
            }
            if (textBox24.Text.Trim() != "")
            {
                str = str + " and Stocks.GRType  = '" + textBox24.Text + "'";
            }

            if (textBox5.Text.Trim() != "")
            {
                str = str + " and Stocks.Totpkts = " + textBox5.Text;
            }
            if (textBox6.Text.Trim() != "")
            {
                str = str + " and Stocks.Private = '" + textBox6.Text + "'";
            }
            if (textBox7.Text.Trim() != "")
            {
                str = str + " and Stocks.Remark = '" + textBox7.Text + "'";
            }
            if (textBox8.Text.Trim() != "")
            {
                str = str + " and Stocks.Itemname = '" + textBox8.Text + "'";
            }
            if (textBox9.Text.Trim() != "")
            {
                str = str + " and Stocks.packing = '" + textBox9.Text + "'";
            }
            if (textBox11.Text.Trim() != "")
            {
                str = str + " AND VOUCHERINFOs.LocationId = '" + funs.Select_locationId(textBox11.Text) + "'";
            }
            if (radioButton2.Checked == true)
            {
                str = str + " and VOUCHERINFOs.Invoiceno IS NULL";
            }
            else if (radioButton3.Checked == true)
            {
                str = str + " and VOUCHERINFOs.Invoiceno IS NOT NULL";
            }
            if (textBox12.Text.Trim() != "")
            {
                str2 = " (ACCOUNTs.name = '" + textBox12.Text + "' or ACCOUNTs_1.name = '" + textBox12.Text + "') and ";
            }
            Report gg = new Report();
          //  gg.BookingRegisterold(dateTimePicker1.Value, dateTimePicker2.Value, str, str2);
            gg.BookingRegister(dateTimePicker1.Value, dateTimePicker2.Value, str, str2);
            gg.MdiParent = this.MdiParent;
            gg.Show();
        }

        private void frm_selector_Load(object sender, EventArgs e)
        {
            textBox11.Text = funs.Select_location_name(Database.LocationId);
            this.Text = "Register";
            dateTimePicker1.CustomFormat = Database.dformat;
            dateTimePicker2.CustomFormat = Database.dformat;
            dateTimePicker1.Value = Database.stDate;
            dateTimePicker2.Value = Database.ldate;
            dateTimePicker1.MinDate = Database.stDate;
            dateTimePicker2.MinDate = Database.stDate;
            dateTimePicker1.MaxDate = Database.ldate;
            dateTimePicker2.MaxDate = Database.ldate;
        }

        private void dateTimePicker1_Enter(object sender, EventArgs e)
        {
            Database.setFocus(dateTimePicker1);
        }

        private void dateTimePicker1_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void dateTimePicker1_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(dateTimePicker1);
        }

        private void dateTimePicker2_Enter(object sender, EventArgs e)
        {
            Database.setFocus(dateTimePicker2);
        }

        private void dateTimePicker2_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void dateTimePicker2_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(dateTimePicker2);
        }

        private void frm_selector_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
                this.Dispose();
            }
        }

        private void textBox11_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox11);
        }

        private void textBox11_KeyPress(object sender, KeyPressEventArgs e)
        {
            strCombo = "SELECT nick_name as Location FROM Location ORDER BY nick_name";
            textBox11.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
        }

        private void textBox11_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox11);
        }

       

       
        private void textBox13_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox13);
        }

        private void textBox13_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox13);
        }

        private void textBox13_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox12_KeyPress(object sender, KeyPressEventArgs e)
        {
            strCombo = "SELECT ACCOUNTs.name, ACCOUNTs.Printname, DeliveryPoints.Name AS Station, ACCOUNTs.Address1, ACCOUNTs.Address2,  ACCOUNTs.phone, ACCOUNTs.tin_number, OTHERs.Name AS Staff, CONTRACTORs.Name AS Agent, ACCOUNTs.ac_id FROM ACCOUNTs LEFT OUTER JOIN  ACCOUNTYPEs ON ACCOUNTs.act_id = ACCOUNTYPEs.Act_id LEFT OUTER JOIN CONTRACTORs ON ACCOUNTs.con_id = CONTRACTORs.Name LEFT OUTER JOIN OTHERs ON ACCOUNTs.loc_id = OTHERs.Oth_id LEFT OUTER JOIN DeliveryPoints ON ACCOUNTs.SId = DeliveryPoints.DPId WHERE ( ACCOUNTYPEs.Path LIKE '1;39;%') ORDER BY ACCOUNTs.name ";
            
          //  strCombo = "SELECT ACCOUNTs.Name, ACCOUNTs.Printname, DeliveryPoints.Name AS Station, ACCOUNTs.Address1, ACCOUNTs.Address2, ACCOUNTs.Phone, ACCOUNTs.Tin_number, OTHERs.Name AS Staff, CONTRACTORs.Name AS Agent                  FROM ACCOUNTs LEFT OUTER JOIN CONTRACTORs ON ACCOUNTs.Con_id = CONTRACTORs.Name LEFT OUTER JOIN OTHERs ON ACCOUNTs.Loc_id = OTHERs.Oth_id LEFT OUTER JOIN DeliveryPoints ON ACCOUNTs.SId = DeliveryPoints.DPId WHERE ACCOUNTs.Act_id = 39 ORDER BY ACCOUNTs.Name";
            textBox12.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 2);
        }

        private void textBox12_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox12);
        }

        private void textBox12_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox12);
        }
    }
}
