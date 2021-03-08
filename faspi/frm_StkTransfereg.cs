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
    public partial class frm_StkTranfereg : Form
    {
        public frm_StkTranfereg()
        {
            InitializeComponent();
        }

        private void frm_challanreg_Load(object sender, EventArgs e)
        {
            textBox11.Text = funs.Select_location_name(Database.LocationId);
            if (this.Text == "Challan Register")
            {
                label11.Text = "Challan No";
            }
            else
            {
                label11.Text = "Stk Tran. No";
            }
            dateTimePicker1.CustomFormat = Database.dformat;
            dateTimePicker2.CustomFormat = Database.dformat;
            dateTimePicker1.Value = Database.stDate;
            dateTimePicker2.Value = Database.ldate;
            dateTimePicker1.MinDate = Database.stDate;
            dateTimePicker2.MinDate = Database.stDate;
            dateTimePicker1.MaxDate = Database.ldate;
            dateTimePicker2.MaxDate = Database.ldate;
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            string strCombo = "Select Gaddi_name from Gaddis order by Gaddi_name";
            textBox2.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, "", 0);
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            string strCombo = "SELECT ACCOUNTs.Name FROM ACCOUNTs LEFT JOIN ACCOUNTYPEs ON ACCOUNTs.Act_id = ACCOUNTYPEs.Act_id WHERE ACCOUNTYPEs.Name='DRIVER' ORDER BY ACCOUNTs.Name";
            textBox1.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, "", 0);
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            string strCombo = "SELECT [name] from DeliveryPoints";
            string st = e.KeyChar.ToString();
            if (textBox3.Text != "")
            {
                st = textBox3.Text;
            }
            textBox3.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, st, 0);
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            string strCombo = "SELECT [name] from DeliveryPoints";
            string st = e.KeyChar.ToString();
            if (textBox4.Text != "")
            {
                st = textBox4.Text;
            }
            textBox4.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, st, 0);
        }

        private void textBox11_KeyPress(object sender, KeyPressEventArgs e)
        {
            string strCombo = "SELECT nick_name as Location FROM Location ORDER BY nick_name";
            textBox11.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            string str = "", str2 = "";

            if (textBox10.Text.Trim() != "")
            {
                str = str + " and Invoiceno = '" + textBox10.Text + "'";
            }
          
            if (textBox3.Text.Trim() != "")
            {
                str = str + " and Source = '" + textBox3.Text + "'";
            }
            if (textBox4.Text.Trim() != "")
            {
                str = str + " and Destination = '" + textBox4.Text + "'";
            }

            if (textBox1.Text.Trim() != "")
            {
                str = str + " and DriverName = '" + textBox1.Text + "'";
            }
            if (textBox2.Text.Trim() != "")
            {
                str = str + " and Gaddino = '" + textBox2.Text + "'";
            }
            if (textBox11.Text.Trim() != "")
            {
                str = str + " AND LocationId = '" + funs.Select_locationId(textBox11.Text) + "'";
            }
            Report gg = new Report();
            if (this.Text == "Challan Register")
            {
                gg.ChallanRegister(dateTimePicker1.Value, dateTimePicker2.Value, str);
            }
            else
            {
                gg.StkTransRegister(dateTimePicker1.Value, dateTimePicker2.Value, str);
            }

            gg.MdiParent = this.MdiParent;
            gg.Show();
            //this.Close();
            //this.Dispose();
        }

        private void dateTimePicker1_Leave(object sender, EventArgs e)
        {
            //dateTimePicker1.lostFocus(dateTimePicker1);
        }

        private void dateTimePicker1_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void dateTimePicker2_KeyDown(object sender, KeyEventArgs e)
        {

            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox10_KeyDown(object sender, KeyEventArgs e)
        {

            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox11_KeyDown(object sender, KeyEventArgs e)
        {

            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {

            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox4_KeyDown(object sender, KeyEventArgs e)
        {

            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {

            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {

            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox10_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox10);
        }

        private void textBox10_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox10);
        }

        private void textBox11_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox11);
        }

        private void textBox11_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox11);
        }

        private void textBox3_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox3);
        }

        private void textBox4_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox4);
        }

        private void textBox4_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox4);
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox2);
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox2);
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox1);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        private void frm_challanreg_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
                this.Dispose();
            }
        }
    }
}
