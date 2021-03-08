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
    public partial class frm_merge : Form
    {
       public string typ = "";
        string strCombo = "";

        public frm_merge()
        {
            InitializeComponent();
        }

        private void frm_merge_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
                this.Dispose();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                textBox1.Focus();
                return;
            }
            if (textBox2.Text == "")
            {
                textBox2.Focus();
                return;
            }

            string idfrom = "";
            string idto = "";

            if (typ == "Account")
            {
                idfrom = funs.Select_ac_id(textBox1.Text);
                idto = funs.Select_ac_id(textBox2.Text);
                if (idfrom == idto)
                {
                    idto = Database.GetScalarText("Select Ac_id from accounts where Ac_id<>'"+idfrom+"' and Name='"+textBox2.Text+"'");
                }

                Database.CommandExecutor("update voucherinfos set ac_id='" + idto + "' where ac_id='" + idfrom + "'");
                Database.CommandExecutor("update voucherinfos set ac_id2='" + idto + "' where ac_id2='" + idfrom + "'");

                Database.CommandExecutor("update Stocks set Consigner_id='" + idto + "' where Consigner_id='" + idfrom + "'");
                Database.CommandExecutor("update Stocks set Consignee_id='" + idto + "' where Consignee_id='" + idfrom + "'");

                Database.CommandExecutor("update ChallanUnloadings set Consigner_id='" + idto + "' where Consigner_id='" + idfrom + "'");
                Database.CommandExecutor("update ChallanUnloadings set Consignee_id='" + idto + "' where Consignee_id='" + idfrom + "'");

                Database.CommandExecutor("update voucherinfos set driver_name='" + idto + "' where driver_name='" + idfrom + "'");
                Database.CommandExecutor("update Journals set ac_id='" + idto + "' where ac_id='" + idfrom + "'");
                Database.CommandExecutor("update Journals set Opp_ac_id='" + idto + "' where Opp_ac_id='" + idfrom + "'");

                Database.CommandExecutor("update Voucheractotals set accid='" + idto + "' where accid='" + idfrom + "'");

              //  Database.CommandExecutor("delete from Journals where ac_id='" + idfrom + "'");
                
                Database.CommandExecutor("delete from partyrates where ac_id='" + idfrom + "'");
                Database.CommandExecutor("delete from accounts where ac_id='" + idfrom + "'");
                MessageBox.Show("Done");
                this.Close();
                this.Dispose();
            }
            else if (typ == "Item")
            {
                idfrom = funs.Select_item_id(textBox1.Text);
                idto = funs.Select_item_id(textBox2.Text);
                Database.CommandExecutor("update voucherdets set des_ac_id='" + idto + "' where des_ac_id='" + idfrom + "'");
                Database.CommandExecutor("delete from partyrates where des_id='" + idfrom + "'");
                Database.CommandExecutor("delete from items where id='" + idfrom + "'");
                Database.CommandExecutor("delete from Itemdetails where Item_id='" + idfrom + "'");
                MessageBox.Show("Done");
                this.Close();
                this.Dispose();
            }
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox1);
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (typ == "Account")
            {
                strCombo = "SELECT name FROM ACCOUNTs ORDER BY name";
            }
            else if (typ == "Item")
            {
                strCombo = "SELECT name FROM items ORDER BY name";
            }
            textBox1.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, "", 0);
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
            if (typ == "Account")
            {
                strCombo = "SELECT name FROM ACCOUNTs ORDER BY name";
            }
            else if (typ == "Item")
            {
                strCombo = "SELECT name FROM items ORDER BY name";
            }
            textBox2.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, "", 0);
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox2);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }
    }
}
