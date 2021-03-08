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
    public partial class frm_stk : Form
    {
        string strCombo = "";
        public frm_stk()
        {
            InitializeComponent();
        }

        private void frm_stk_Load(object sender, EventArgs e)
        {
          

            textBox1.Text = Database.GetScalarText("Select nick_name from location where LocationId='"+Database.LocationId+"'");
            textBox2.Text = "Booked";
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            DataTable dt = new DataTable();
            Database.GetSqlData("select nick_name as Name from Location  order by nick_name", dt);
            textBox1.Text= SelectCombo.ComboDt(this, dt, 0);
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
           DataTable dt = new DataTable();
           dt.Columns.Add("Category",typeof(string));
           dt.Rows.Add();
           dt.Rows[0][0] = "Booked";
           dt.Rows.Add();
           dt.Rows[1][0] = "To Be Delivered";
           textBox2.Text = SelectCombo.ComboDt(this, dt, 0);
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox1);
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox2);
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox2);
        }

        private void textBox2_ImeModeChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox1);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string str = "";
            Report gg = new Report();
            gg.MdiParent = this.MdiParent;


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
            if (textBox3.Text.Trim() != "")
            {
                str = str + " and DeliveryPoints_1.Name = '" + textBox3.Text + "'";
            }
            if (textBox25.Text.Trim() != "")
            {
                str = str + " and Stocks.DeliveryType = '" + textBox25.Text + "'";
            }
            if (textBox24.Text.Trim() != "")
            {
                str = str + " and Stocks.GRType  = '" + textBox24.Text + "'";
            }
            if (textBox6.Text.Trim() != "")
            {
                str = str + " and Stocks.Private = '" + textBox6.Text + "'";
            }
            if (textBox7.Text.Trim() != "")
            {
                str = str + " and Stocks.Remark  = '" + textBox7.Text + "'";
            }

            gg.Stock(Database.stDate, Database.enDate, textBox1.Text, textBox2.Text, str);
            gg.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frm_stk_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
                this.Dispose();


            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

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

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            strCombo = "SELECT [name] from DeliveryPoints";
            textBox3.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            strCombo = "SELECT [name] from DeliveryPoints";
            textBox4.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
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

        private void textBox10_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox10);
        }

        private void textBox10_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox10);
        }

        private void textBox10_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox6_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox7_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

    }
}
