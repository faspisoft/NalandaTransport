using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;

namespace faspi
{
     
    public partial class frm_companyinfo : Form
    {
        DataTable dtcom;
    
        public frm_companyinfo()
        {
            InitializeComponent();
        }
        public void LoadData(string id,string Caption)
        {
            this.Text = Caption;
            dtcom = new DataTable("Location");
            Database.GetSqlData("Select * from Location where locationid='"+id+"'",dtcom);
            if (dtcom.Rows.Count == 1)
            {
                textBox1.Text = dtcom.Rows[0]["Name"].ToString();
                textBox2.Text = dtcom.Rows[0]["Address1"].ToString();
                textBox3.Text = dtcom.Rows[0]["Address2"].ToString();
                
                textBox5.Text = funs.Select_state_nm(dtcom.Rows[0]["State_id"].ToString());

                textBox6.Text = dtcom.Rows[0]["PIN"].ToString();
                textBox4.Text = dtcom.Rows[0]["Email"].ToString();
                textBox8.Text = dtcom.Rows[0]["GST"].ToString();
                textBox9.Text = dtcom.Rows[0]["mobile"].ToString();
                textBox10.Text = dtcom.Rows[0]["nick_name"].ToString();
                textBox11.Text = dtcom.Rows[0]["Prefix"].ToString();
                if (dtcom.Rows[0]["Cashac_id"].ToString() == "")
                {
                    textBox7.Text = "";

                }
                else
                {
                    textBox7.Text = funs.Select_ac_nm(dtcom.Rows[0]["Cashac_id"].ToString());
                }
                if (dtcom.Rows[0]["expenseacc"].ToString() == "")
                {
                    textBox12.Text = "";

                }
                else
                {
                    textBox12.Text = funs.Select_ac_nm(dtcom.Rows[0]["expenseacc"].ToString());
                }
            }

        }

        private void SideFill()
        {
            flowLayoutPanel1.Controls.Clear();
            DataTable dtsidefill = new DataTable();
            dtsidefill.Columns.Add("Name", typeof(string));
            dtsidefill.Columns.Add("DisplayName", typeof(string));
            dtsidefill.Columns.Add("ShortcutKey", typeof(string));
            dtsidefill.Columns.Add("Visible", typeof(bool));

            //save
            dtsidefill.Rows.Add();
            dtsidefill.Rows[0]["Name"] = "save";
            dtsidefill.Rows[0]["DisplayName"] = "Save";
            dtsidefill.Rows[0]["ShortcutKey"] = "^S";
            
            dtsidefill.Rows[0]["Visible"] = true;
           

            //close
            dtsidefill.Rows.Add();
            dtsidefill.Rows[1]["Name"] = "quit";
            dtsidefill.Rows[1]["DisplayName"] = "Quit";
            dtsidefill.Rows[1]["ShortcutKey"] = "Esc";
            dtsidefill.Rows[1]["Visible"] = true;

            for (int i = 0; i < dtsidefill.Rows.Count; i++)
            {
                if (bool.Parse(dtsidefill.Rows[i]["Visible"].ToString()) == true)
                {
                    Button btn = new Button();
                    btn.Size = new Size(150, 30);
                    btn.Name = dtsidefill.Rows[i]["Name"].ToString();
                    btn.Text = "";
                    Bitmap bmp = new Bitmap(btn.ClientRectangle.Width, btn.ClientRectangle.Height);
                    Graphics G = Graphics.FromImage(bmp);
                    G.Clear(btn.BackColor);
                    string line1 = dtsidefill.Rows[i]["ShortcutKey"].ToString();
                    string line2 = dtsidefill.Rows[i]["DisplayName"].ToString();
                    StringFormat SF = new StringFormat();
                    SF.Alignment = StringAlignment.Near;
                    SF.LineAlignment = StringAlignment.Center;
                    Rectangle RC = btn.ClientRectangle;
                    Font font = new Font("Arial", 12);
                    G.DrawString(line1, font, Brushes.Red, RC, SF);
                    G.DrawString("".PadLeft(line1.Length * 2 + 1) + line2, font, Brushes.Black, RC, SF);
                    btn.Image = bmp;
                    btn.Click += new EventHandler(btn_Click);
                    flowLayoutPanel1.Controls.Add(btn);
                }
            }
        }

        void btn_Click(object sender, EventArgs e)
        {
            Button tbtn = (Button)sender;
            string name = tbtn.Name.ToString();


            if (name == "save")
            {
                if (validate() == true)
                {        
                     save();  
                }
            }

            else if (name == "quit")
            {
                this.Close();
                this.Dispose();
            }
        }

        private void save()
        {

            dtcom.Rows[0]["Name"] =textBox1.Text;
            dtcom.Rows[0]["Address1"] = textBox2.Text;
            dtcom.Rows[0]["Address2"] = textBox3.Text;
            dtcom.Rows[0]["state_id"] = funs.Select_state_id(textBox5.Text);
            dtcom.Rows[0]["PIN"] = textBox6.Text;
            dtcom.Rows[0]["Email"] = textBox4.Text;
            dtcom.Rows[0]["GST"] = textBox8.Text;
            dtcom.Rows[0]["mobile"] = textBox9.Text;
            dtcom.Rows[0]["nick_name"] =textBox10.Text;
            dtcom.Rows[0]["Prefix"] = textBox11.Text;
            dtcom.Rows[0]["Cashac_id"] = funs.Select_ac_id(textBox7.Text);
            dtcom.Rows[0]["expenseacc"] = funs.Select_ac_id(textBox12.Text);
            Database.LocationExpAcc_id = funs.Select_ac_id(textBox12.Text);
            Database.LocationCashAcc_id = funs.Select_ac_id(textBox7.Text);
            Database.SaveData(dtcom);

            MessageBox.Show("Saved Successfully");
            Database.fname = textBox1.Text;
           
            this.Close();
            this.Dispose();
        }


        private bool validate()
        {

            if (textBox1.Text == "")
            {

                textBox1.Focus();

                return false;
            }
            if (textBox2.Text == "")
            {
                textBox2.Focus();

                return false;
            }
            if (textBox5.Text == "")
            {
                textBox5.Focus();

                return false;
            }

            if (textBox10.Text == "")
            {
                textBox10.Focus();

                return false;
            }
            if (textBox11.Text == "")
            {
                textBox11.Focus();

                return false;
            }

            return true;
        }

        private void frm_companyinfo_Load(object sender, EventArgs e)
        {
            SideFill();
        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
           string  strCombo = "select Sname As State from States order by Sname";
            textBox5.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox5_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox6_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox4_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox8_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox9_KeyDown(object sender, KeyEventArgs e)
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

        private void textBox1_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox1);
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox2);
        }

        private void textBox3_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox3);
        }

        private void textBox5_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox5);
        }

        private void textBox6_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox6);
        }

        private void textBox4_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox4);
        }

        private void textBox8_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox8);
        }

        private void textBox9_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox9);
        }

        private void textBox10_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox10);
        }

        private void textBox11_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox11);
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox1);
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox2);
        }

        private void textBox3_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox3);
        }

        private void textBox5_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox5);
        }

        private void textBox6_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox6);
        }

        private void textBox4_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox4);
        }

        private void textBox8_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox8);
        }

        private void textBox9_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox9);
        }

        private void textBox10_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox10);
        }

        private void textBox11_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox11);
        }

        private void frm_companyinfo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
            {
                if (validate() == true)
                {
                   
                        save();
                   
                }
            }
            else if (e.KeyCode == Keys.Escape)
            {
                if (textBox1.Text != "")
                {
                    DialogResult chk = MessageBox.Show("Are u sure?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                    if (chk == DialogResult.No)
                    {
                        e.Handled = false;
                    }
                    else
                    {
                        this.Close();
                        this.Dispose();
                    }
                }
                else
                {
                    this.Close();
                    this.Dispose();
                }
            }
        }

        private void textBox7_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void textBox7_KeyPress(object sender, KeyPressEventArgs e)
        {
            string strcombo = "Select Name from Accounts where act_id=3";
            textBox7.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strcombo, textBox7.Text, 0);
        }

        private void textBox12_KeyPress(object sender, KeyPressEventArgs e)
        {
            string strcombo = "Select Name from Accounts where act_id in(7,30) ";
            textBox12.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strcombo, textBox12.Text, 0);
        }

        private void textBox7_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox7);
        }

        private void textBox12_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox12);
        }

        private void textBox12_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox12);
        }

        private void textBox7_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox7);
        }
    }
}
