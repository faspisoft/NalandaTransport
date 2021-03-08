using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace faspi
{
    public partial class frm_NewAcc : Form
    {
        DataTable dtAcc;
        String dtName;
        string act_name = "";
        public bool calledIndirect = false;
        public String AccName;
        public String AccType;
        String strCombo;
        public string gStr = "";

        public frm_NewAcc()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (Feature.Available("Display Forwarding GRDetails") == "Yes")
            {
                groupBox8.Visible = true;
            }
            else
            {
                groupBox8.Visible = false;
            }
            if (Feature.Available("Send SMS") == "Yes")
            {
                label19.Visible = true;
                textBox30.Visible = true;
            }
            else
            {
                label19.Visible = false;
                textBox30.Visible = false;
              
            }
            SideFill();
            if (Feature.Available("Taxation Applicable") == "VAT")
            {
                groupBox5.Text = "TIN";
            }
            else
            {
                groupBox5.Text = "GSTIN";
            }            
            groupBox10.Text = Feature.Available("Show Text on AadhaarNo");
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
            if (gStr != "0")
            {
                if (Database.utype == "User")
                {
                    dtsidefill.Rows[0]["Visible"] = false;
                }
                else
                {
                    dtsidefill.Rows[0]["Visible"] = true;
                }
            }
            else
            {
                dtsidefill.Rows[0]["Visible"] = true;
            }

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
                    if (Database.utype == "Admin")
                    {
                        save();
                    }
                    else if (gStr == "0")
                    {
                        save();
                    }
                }
            }

            else if (name == "quit")
            {
                this.Close();
                this.Dispose();
            }
        }

        private void frm_NewAcc_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
            {
                if (validate() == true)
                {
                    if (Database.utype == "Admin")
                    {
                        save();                        
                    }
                    else if (gStr == "0")
                    {
                        save();
                    }
                }
            }
            else if (e.KeyCode == Keys.Escape)
            {
                if (TextBox1.Text != "")
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

        public void LoadData(String str, String frmCaption)
        {
            gStr = str;
            this.Text = frmCaption;

            if (AccType == "Bank")
            {
                textBox10.Text = AccType;
                textBox10.Enabled = false;
            }
            else if (AccType == "Cash")
            {
                textBox10.Text = AccType;
                textBox10.Enabled = false;
            }

            dtName = "accounts";
            dtAcc = new DataTable(dtName);
            Database.GetSqlData("select * from " + dtName + " where ac_id='" + str + "'", dtAcc);
            
            if (AccType == null && dtAcc.Rows.Count == 0)
            {
                dtAcc.Rows.Add(0);
                TextBox1.Select();
                TextBox1.Text = "";
                textBox2.Text = "0";
                textBox3.Text = "0";
                textBox4.Text = "0";
                TextBox5.Text = "";
                TextBox6.Text = "";
                TextBox7.Text = "";
                TextBox8.Text = "";
                textBox9.Text = "";
                textBox13.Text = "";
                textBox17.Text = "";
                textBox18.Text = "";
                textBox20.Text = "";
                textBox28.Text = "";
                textBox29.Text = "";
                textBox30.Text = "0";
                textBox21.Text = "Unregistered";
                string state_id = Database.CompanyState_id;
                //string state_id = Database.GetScalarText("Select CState_id from Company");
                textBox19.Text = funs.Select_state_nm(state_id);
                textBox16.Text = "0";
                textBox11.Text = "";
                textBox12.Text = "";
                textBox22.Text = "";
                textBox10.Text = "SUNDRY DEBTORS";
            }
            else if (AccType != null && dtAcc.Rows.Count == 0)
            {
                dtAcc.Rows.Add(0);
                TextBox1.Select();
                TextBox1.Text = "";
                textBox2.Text = "0";
                textBox3.Text = "0";
                textBox4.Text = "0";
                TextBox5.Text = "";
                TextBox6.Text = "";
                TextBox7.Text = "";
                TextBox8.Text = "";
                textBox9.Text = "";
                textBox12.Text = "";
                textBox13.Text = "";
                textBox17.Text = "";
                textBox18.Text = "";
                textBox20.Text = "";
                textBox28.Text = "";
                textBox30.Text = "0";
                textBox21.Text = "Unregistered";
                string state_id = Database.CompanyState_id;
                //string state_id = Database.GetScalarText("Select CState_id from Company");
                textBox19.Text = funs.Select_state_nm(state_id);
                textBox11.Text = "";
                textBox16.Text = "0";
                textBox22.Text = "";
                textBox29.Text = "";
            }
            else
            {
                TextBox1.Select();
                TextBox1.Text = dtAcc.Rows[0]["name"].ToString();
                textBox10.Text = funs.Select_act_nm(int.Parse(dtAcc.Rows[0]["act_id"].ToString()));
                if (double.Parse(dtAcc.Rows[0]["Balance"].ToString()) >= 0)
                {
                    textBox2.Text = funs.DecimalPoint(double.Parse(dtAcc.Rows[0]["Balance"].ToString()), 2);
                    radioButton1.Checked = true;
                }
                else
                {
                    textBox2.Text = funs.DecimalPoint(-1 * double.Parse(dtAcc.Rows[0]["Balance"].ToString()), 2);
                    radioButton2.Checked = true;
                }
                textBox29.Text = funs.Select_ac_nm(dtAcc.Rows[0]["Transporter_id"].ToString());
                textBox26.Text = dtAcc.Rows[0]["contact_person"].ToString();
                textBox28.Text = dtAcc.Rows[0]["password"].ToString();
                textBox3.Text = funs.DecimalPoint(dtAcc.Rows[0]["Blimit"]);
                TextBox5.Text = dtAcc.Rows[0]["address1"].ToString();
                TextBox6.Text = dtAcc.Rows[0]["address2"].ToString();
                TextBox7.Text = dtAcc.Rows[0]["phone"].ToString();
                TextBox8.Text = dtAcc.Rows[0]["email"].ToString();
                textBox9.Text = dtAcc.Rows[0]["Tin_number"].ToString();
                textBox17.Text = dtAcc.Rows[0]["PAN"].ToString();
                textBox20.Text = dtAcc.Rows[0]["Aadhaarno"].ToString();
                textBox21.Text = dtAcc.Rows[0]["RegStatus"].ToString();
                textBox30.Text = dtAcc.Rows[0]["SMSMobile"].ToString();
                textBox11.Text = "";

                textBox11.Text = funs.Select_oth_nm(dtAcc.Rows[0]["loc_id"].ToString());

                if (dtAcc.Rows[0]["State_id"].ToString() == "")
                {
                    string state_id = Database.CompanyState_id;
                    //string state_id = Database.GetScalarText("Select CState_id from Company");
                    textBox19.Text = funs.Select_state_nm(state_id);
                }
                else
                {
                    textBox19.Text = funs.Select_state_nm(dtAcc.Rows[0]["State_id"].ToString());
                }

                if (dtAcc.Rows[0]["con_id"].ToString() == "")
                {
                    dtAcc.Rows[0]["con_id"] = "0";
                }
                else
                {
                    textBox12.Text = funs.Select_con_nm(dtAcc.Rows[0]["con_id"].ToString());
                }                
                textBox13.Text = dtAcc.Rows[0]["Note"].ToString();
                textBox16.Text = funs.DecimalPoint(dtAcc.Rows[0]["Closing_Bal"], 2);
                textBox18.Text = dtAcc.Rows[0]["Printname"].ToString();

                textBox14.Text = dtAcc.Rows[0]["Delivery_type"].ToString();
                textBox27.Text = dtAcc.Rows[0]["GR_type"].ToString();

                act_name = Database.GetScalarText("Select Name from accountypes where Name='" + textBox10.Text + "'");
                if (act_name == "Stock")
                {
                    textBox4.Text = funs.DecimalPoint(dtAcc.Rows[0]["Closing_Bal2"], 2);
                }
                else
                {
                    textBox4.Text = dtAcc.Rows[0]["Dlimit"].ToString();
                }

                textBox22.Text = funs.Select_dp_nm(dtAcc.Rows[0]["SId"].ToString());
            }
            act_name = Database.GetScalarText("Select Name from accountypes where Name='" + textBox10.Text + "'");
            Displaysetting("SUNDRY DEBTORS");

            if (Feature.Available("Group Credit Limits") == "No")
            {
                textBox11.Enabled = false;
            }
            if (Feature.Available("Broker Wise Report") == "No")
            {
                textBox12.Enabled = false;
            }
        }

        private void save()
        {
            AccName = TextBox1.Text;

            if (gStr == "0")
            {
                DataTable dtCount = new DataTable();
                Database.GetSqlData("select count(*) from accounts where locationid='" + Database.LocationId + "'", dtCount);
                if (int.Parse(dtCount.Rows[0][0].ToString()) == 0)
                {
                    dtAcc.Rows[0]["ac_id"] = Database.LocationId + "1";
                    dtAcc.Rows[0]["Nid"] = 1;
                    dtAcc.Rows[0]["LocationId"] = Database.LocationId;
                }
                else
                {
                    DataTable dtAcid = new DataTable();
                    Database.GetSqlData("select max(Nid) as Nid from accounts where locationid='" + Database.LocationId + "'", dtAcid);
                    int Nid = int.Parse(dtAcid.Rows[0][0].ToString());
                    dtAcc.Rows[0]["ac_id"] = Database.LocationId + (Nid + 1);
                    dtAcc.Rows[0]["Nid"] = (Nid + 1);
                    dtAcc.Rows[0]["LocationId"] = Database.LocationId;
                }
            }
            dtAcc.Rows[0]["act_id"] = funs.Select_act_id(textBox10.Text);
            dtAcc.Rows[0]["name"] = TextBox1.Text;
            dtAcc.Rows[0]["Address1"] = TextBox5.Text;
            dtAcc.Rows[0]["Address2"] = TextBox6.Text;
            dtAcc.Rows[0]["phone"] = TextBox7.Text;
            dtAcc.Rows[0]["email"] = TextBox8.Text;
            dtAcc.Rows[0]["tin_number"] = textBox9.Text;
            dtAcc.Rows[0]["PAN"] = textBox17.Text;
            dtAcc.Rows[0]["password"] = textBox28.Text;
            dtAcc.Rows[0]["con_id"] = funs.Select_con_id(textBox12.Text);
            dtAcc.Rows[0]["userid"] = Database.user_id;

            dtAcc.Rows[0]["SMSMobile"] = textBox30.Text;
            if (textBox19.Text == "")
            {
                dtAcc.Rows[0]["State_id"] = Database.CompanyState_id;
            }
            else
            {
                dtAcc.Rows[0]["State_id"] = funs.Select_state_id(textBox19.Text);
            }
            dtAcc.Rows[0]["Transporter_id"] = funs.Select_ac_id(textBox29.Text);
            dtAcc.Rows[0]["Aadhaarno"] = textBox20.Text;
            dtAcc.Rows[0]["contact_person"]=textBox26.Text;
            if (textBox11.Text != "")
            {
                dtAcc.Rows[0]["loc_id"] = funs.Select_oth_id(textBox11.Text);
            }
            else
            {
                dtAcc.Rows[0]["loc_id"] = 0;
            }
            if (radioButton1.Checked == true)
            {
                dtAcc.Rows[0]["Balance"] = funs.DecimalPoint(textBox2.Text, 2);
            }
            else
            {
                dtAcc.Rows[0]["Balance"] = -1 * double.Parse(textBox2.Text);
            }
            
            if (textBox3.Text != "")
            {
                dtAcc.Rows[0]["Blimit"] = textBox3.Text;
            }
            else
            {
                dtAcc.Rows[0]["Blimit"] = "0.00";
            }
            dtAcc.Rows[0]["Closing_Bal"] = textBox16.Text;
            if (textBox4.Text == "")
            {
                textBox4.Text = "0";
            }
            dtAcc.Rows[0]["Dlimit"] = double.Parse(textBox4.Text);
            dtAcc.Rows[0]["RegStatus"] = textBox21.Text;
            dtAcc.Rows[0]["note"] = textBox13.Text;
            dtAcc.Rows[0]["Printname"] = textBox18.Text;
            dtAcc.Rows[0]["Closing_Bal"] = textBox16.Text;
            dtAcc.Rows[0]["Delivery_type"] = textBox14.Text;
            dtAcc.Rows[0]["GR_type"] = textBox27.Text;
            string shortcode = "";
            string[] ar = TextBox1.Text.Split(' ');
            for (int i = 0; i < ar.Length; i++)
            {
                if (ar[i] != "")
                {
                    shortcode = shortcode + ar[i].Substring(0, 1);
                }
            }
            dtAcc.Rows[0]["shortcode"] = shortcode;
            dtAcc.Rows[0]["SId"] = funs.Select_dp_id(textBox22.Text);
            
            if (gStr == "0")
            {
                dtAcc.Rows[0]["create_date"] = System.DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
            }
            dtAcc.Rows[0]["modify_date"] = System.DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");

            Database.SaveData(dtAcc);
            //funs.ShowBalloonTip("Saved", "Saved Successfully");
            MessageBox.Show("Saved Successfully");
            if (calledIndirect == true)
            {
                this.Close();
                this.Dispose();
            }
            else if (gStr == "0")
            {
                LoadData("0", this.Text);
            }
            else
            {
                this.Close();
                this.Dispose();
            }            
        }

        private void Displaysetting(string act_name)
        {
            if (act_name == "STOCK-IN-HAND")
            {
                textBox16.Visible = true;
                textBox16.Enabled = true;
                textBox3.Visible = false;
                textBox4.Visible = false;
                label10.Visible = false;
                label4.Text = "Balance";
                groupBox3.Text = "Stock Closing Balance";
            }
            else
            {
                textBox16.Visible = false;
                textBox3.Visible = true;
                textBox4.Visible = true;
                label10.Visible = true;
                if (Feature.Available("Customer Credit Limits") == "No")
                {
                    textBox3.Enabled = false;
                    textBox4.Enabled = false;
                }
                label4.Text = "Rs.";
                groupBox3.Text = "Credit Limit";
            }
            if (act_name == "CASH-IN-HAND" || act_name == "Reserves & Surplus" || act_name == "Tax" || act_name == "Suspense" || act_name == "Provisions")
            {
                textBox19.Enabled = true;
            }
            else if (act_name == "SUNDRY DEBTORS" || act_name == "SUNDRY CREDITORS" || act_name == "Godown" || act_name == "GODOWN")
            {
                textBox3.Enabled = true;
                textBox4.Enabled = true;
                textBox20.Enabled = true;
                TextBox5.Enabled = true;
                TextBox6.Enabled = true;
                TextBox7.Enabled = true;
                TextBox8.Enabled = true;
                textBox9.Enabled = true;
                textBox17.Enabled = true;
                textBox19.Enabled = true;
                textBox11.Enabled = true;
                textBox12.Enabled = true;
                textBox13.Enabled = true;                
            }
            else if (act_name == "STOCK-IN-HAND")
            {
                textBox16.Enabled = true;
            }
            else if (act_name == "Libilities" || act_name == "Bank" || act_name == "Fixed Assets" || act_name == "Unregistered Supplier" || act_name == "Investments" || act_name == "Security & Deposit (Asset)" || act_name == "Loan & Advances" || act_name == "Capital" || act_name == "Bank Occ" || act_name == "Unsecure Loans" || act_name == "Secure loans")
            {
                TextBox5.Enabled = true;
                TextBox6.Enabled = true;
                TextBox7.Enabled = true;
                TextBox8.Enabled = true;
                textBox13.Enabled = true;
            }
            else
            {
                TextBox5.Enabled = false;
                TextBox6.Enabled = false;
                TextBox7.Enabled = false;
                TextBox8.Enabled = false;
                textBox9.Enabled = false;
                textBox11.Enabled = false;
                textBox12.Enabled = false;
                textBox13.Enabled = false;
                textBox17.Enabled = false;
            }
            if (textBox21.Text != "Unregistered")
            {
                textBox9.Enabled = true;
            }
            else
            {
                textBox9.Enabled = false;
            }
        }

        private bool validate()
        {
            if (act_name == "STOCK-IN-HAND")
            {
                if (textBox4.Text == "")
                {
                    textBox4.Text = "";
                }
                else if (textBox4.Text != "")
                {
                    textBox4.Text = textBox4.Text;
                }
                else
                {
                    textBox4.Text = "0";
                }
            }
            if (textBox9.Text == "")
            {
                textBox9.Text = "0";
            }
            if (textBox10.Text == "")
            {
                textBox10.Text = "";
                textBox10.Focus();
                return false;
            }
            if (textBox2.Text == "")
            {
                textBox2.Text = "0";
            }
            if (textBox3.Text == "")
            {
                textBox3.Text = "0";
            }
            if (textBox21.Text == "")
            {
                textBox21.Text = "";
                textBox21.Focus();
                return false;
            }
            if (textBox21.Text == "Composition Dealer" || textBox21.Text == "Regular Registration")
            {
                if (textBox9.Text.Trim() == "" || textBox9.Text == "0")
                {
                    textBox9.Focus();
                    return false;
                }
            }
            if (TextBox1.Text == "")
            {
                TextBox1.Focus();
                return false;
            }
            else if (funs.isDouble(textBox2.Text) == false)
            {
                textBox2.Focus();
                return false;
            }           
            else if (funs.isDouble(textBox3.Text) == false)
            {
                textBox3.Focus();
                return false;
            }
            if (funs.Select_ac_id(TextBox1.Text) != "" && funs.Select_ac_id(TextBox1.Text) != gStr)
            {
                MessageBox.Show("AccountName Already Exists.");
                return false;
            }
            if (textBox19.Text == "")
            {
                textBox19.Text = funs.Select_state_nm(Database.CompanyState_id.ToString());
            }
            if (Feature.Available("Taxation Applicable") == "VAT")
            {
                if (textBox19.Text == "")
                {
                    textBox19.Text = funs.Select_state_nm(Database.CompanyState_id.ToString());
                }
            }
            else
            {
                if (textBox19.Text == "")
                {
                    if (act_name == "SUNDRY DEBTORS" || act_name == "SUNDRY CREDITORS")
                    {
                        MessageBox.Show("Please Select State with this A/c");
                        textBox19.Focus();
                        textBox19.Focus();
                        return false;
                    }
                }
            }

            if (Feature.Available("Taxation Applicable") != "VAT")
            {
                if (textBox21.Text == "Composition Dealer" || textBox21.Text == "Regular Registration")
                {

                    string statecode = "";
                    statecode = funs.Select_state_GST(textBox19.Text);


                    Regex obj = new Regex("^[0-9]{2}[A-Z]{5}[0-9]{4}[A-Z]{1}[0-9A-Z]{1}Z[0-9A-Z]{1}$");
                    if (textBox9.Text.Trim() == "" || textBox9.Text == "0")
                    {
                        textBox9.Focus();
                        return false;
                    }
                    else if (obj.IsMatch(textBox9.Text) == false)
                    {
                        MessageBox.Show("GSTIN is Not Correct");
                        return false;
                    }
                }
            }
            if (Feature.Available("Taxation Applicable") != "VAT")
            {

                if (textBox21.Text == "Composition Dealer" || textBox21.Text == "Regular Registration")
                {
                    string statecode = "";
                    statecode = funs.Select_state_GST(textBox19.Text);
                    if (statecode == "")
                    {
                        textBox9.Focus();
                        MessageBox.Show("Please Enter State First");
                        return false;
                    }


                    else if (textBox9.Text.Trim() != "")
                    {
                        string code = textBox9.Text.Substring(0, 2);
                        if (statecode.ToString() != code)
                        {
                            textBox9.Focus();
                            MessageBox.Show("State Name and GSTIN No. not match");
                            return false;
                        }

                    }
                }
            }




            //if (act_name == "SUNDRY DEBTORS" || act_name == "SUNDRY CREDITORS")
            //{
                if (textBox22.Text == "")
                {
                    textBox22.Focus();
                    textBox22.Focus();
                    return false;
                }
            //}
            return true;
        }

        private void textBox10_KeyPress(object sender, KeyPressEventArgs e)
        {
            string acnm = e.KeyChar.ToString();
            if (textBox10.Text != "")
            {
                acnm = textBox10.Text;
            }
            if (AccType == null || AccType == "*")
            {
                if (Feature.Available("Multi-Godown") == "Yes")
                {
                    strCombo = "select Name from accountypes where type='Account' order by Name";
                }
                else
                {
                    strCombo = "select Name from accountypes where type='Account' and Name<>'Godown' order by Name";
                }

                textBox10.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, acnm, 0);
            }
            else
            {
                if (Feature.Available("Multi-Godown") == "Yes")
                {
                    strCombo = "select Name from accountypes where type='Account' and Act_id " + AccType;
                }
                else
                {
                    strCombo = "select Name from accountypes where type='Account' and Name<>'Godown' and Act_id " + AccType;
                }
                textBox10.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, acnm, 0);
            }
            act_name = textBox10.Text;
            //act_name = Database.GetScalarText("Select Name from accountypes where Name='" + textBox10.Text + "'");
            Displaysetting(act_name);
            Load1();
        }

        private void Load1()
        {
            if (Feature.Available("Customer Credit Limits") == "No")
            {
                textBox3.Enabled = false;
                if (act_name != "STOCK-IN-HAND")
                {
                    textBox4.Enabled = false;
                }
            }
            if (Feature.Available("Customer Credit Limits") == "No" && act_name == "Stock")
            {
                textBox4.Enabled = true;
            }
            if (Feature.Available("Group Credit Limits") == "No")
            {
                textBox11.Enabled = false;
            }
        }

        private void textBox11_KeyPress(object sender, KeyPressEventArgs e)
        {
            strCombo = "select [name] from others where type=17 order by [name]";
            textBox11.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, textBox11.Text, 0);
        }

        private void textBox11_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C)
            {
                textBox11.Text = funs.AddGroup();
            }
            else if (e.Control && e.KeyCode == Keys.A)
            {
                if (textBox11.Text != "")
                {
                    textBox11.Text = funs.EditGroup(textBox11.Text);
                }
            }
        }

        private void TextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void radioButton1_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void radioButton2_KeyDown(object sender, KeyEventArgs e)
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

        private void TextBox5_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void TextBox6_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void TextBox7_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void TextBox8_KeyDown(object sender, KeyEventArgs e)
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

        private void TextBox1_Enter(object sender, EventArgs e)
        {
            Database.setFocus(TextBox1);
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox2);
        }

        private void textBox3_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox3);
        }

        private void textBox4_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox4);
        }

        private void TextBox5_Enter(object sender, EventArgs e)
        {
            Database.setFocus(TextBox5);
        }

        private void TextBox6_Enter(object sender, EventArgs e)
        {
            Database.setFocus(TextBox6);
        }

        private void TextBox7_Enter(object sender, EventArgs e)
        {
            Database.setFocus(TextBox7);
        }

        private void TextBox8_Enter(object sender, EventArgs e)
        {
            Database.setFocus(TextBox8);
        }

        private void textBox11_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox11);
        }

        private void textBox9_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox9);
        }

        private void textBox9_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox9);
        }

        private void textBox11_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox11);
        }

        private void TextBox8_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(TextBox8);
        }

        private void TextBox7_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(TextBox7);
        }

        private void TextBox6_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(TextBox6);
        }

        private void TextBox5_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(TextBox5);
        }

        private void textBox4_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox4);
        }

        private void textBox3_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox3);
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox2);
        }

        private void TextBox1_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(TextBox1);
            if (textBox18.Text == "")
            {
                textBox18.Text = TextBox1.Text;
            }
        }

        private void textBox12_KeyPress(object sender, KeyPressEventArgs e)
        {
            strCombo = "select [name] from CONTRACTORs order by [name]";
            textBox12.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, textBox12.Text, 0);
        }

        private void textBox12_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C)
            {
                textBox12.Text = funs.AddBroker();
            }
            else if (e.Control && e.KeyCode == Keys.A)
            {
                if (textBox12.Text != "")
                {
                    textBox12.Text = funs.EditBroker(textBox12.Text); ;
                }
            }
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && !(e.KeyChar.ToString() == ".") && !(e.KeyChar.ToString() == "-");
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && !(e.KeyChar.ToString() == ".") && !(e.KeyChar.ToString() == "-");
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && !(e.KeyChar.ToString() == ".") && !(e.KeyChar.ToString() == "-");
        }

        private void textBox16_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox16);
        }

        private void textBox16_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox16);
        }

        private void textBox16_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && !(e.KeyChar.ToString() == ".") && !(e.KeyChar.ToString() == "-");
        }

        private void textBox16_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox17_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox17_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox17);
        }

        private void textBox17_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox17);
        }

        private void TextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\'')
            {
                e.Handled = true;
            }
        }

        private void textBox18_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox18_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox18);
        }

        private void textBox18_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox18);
        }

        private void textBox19_KeyPress(object sender, KeyPressEventArgs e)
        {
            strCombo = "select Sname As State from States order by Sname";
            textBox19.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
        }

        private void textBox19_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox19);
        }

        private void textBox19_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox19);
        }

        private void checkBox1_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox19_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C)
            {
                textBox19.Text = funs.AddState();
            }
            else if (e.Control && e.KeyCode == Keys.A)
            {
                if (textBox19.Text != "")
                {
                    textBox19.Text = funs.EditState(textBox19.Text);
                }
            }
        }

        private void radioButton6_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void radioButton5_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox20_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox20);
        }

        private void textBox20_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox20_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox20);
        }

        private void textBox21_TextChanged(object sender, EventArgs e)
        {
            if (textBox21.Text == "Unregistered")
            {
                textBox9.Text = "";
                textBox9.Enabled = false;
            }
            else if (textBox21.Text == "Regular Registration" || textBox21.Text == "Composition Dealer")
            {
                textBox9.Enabled = true;
            }
        }

        private void textBox21_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox21_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox21);
        }

        private void textBox21_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox21);
        }

        private void textBox22_KeyPress(object sender, KeyPressEventArgs e)
        {
            strCombo = "SELECT [name] from DeliveryPoints order by [name]";
            textBox22.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
        }

        private void textBox22_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox22);
        }

        private void textBox22_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox22);
        }

        private void textBox22_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C)
            {
                textBox22.Text = funs.AddDP();
            }
            else if (e.Control && e.KeyCode == Keys.A)
            {
                if (textBox22.Text != "")
                {
                    textBox22.Text = funs.EditDP(textBox22.Text);
                }
            }
        }

        private void textBox2_KeyDown_1(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox21_KeyPress(object sender, KeyPressEventArgs e)
        {
            DataTable dtcombo = new DataTable();
            dtcombo.Columns.Add("Registration Status");
            dtcombo.Rows.Add();
            dtcombo.Rows[0][0] = "Unregistered";

            dtcombo.Rows.Add();
            dtcombo.Rows[1][0] = "Regular Registration";
            dtcombo.Rows.Add();
            dtcombo.Rows[2][0] = "Composition Dealer";

            textBox21.Text = SelectCombo.ComboDt(this, dtcombo, 0);
            SendKeys.Send("{tab}");
        }

        private void textBox26_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox26);
        }

        private void textBox26_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox26);
        }

        private void textBox26_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox14_KeyPress(object sender, KeyPressEventArgs e)
        {
            DataTable dtcombo = new DataTable();
            dtcombo.Columns.Add("DeliveryType", typeof(string));
            dtcombo.Columns["DeliveryType"].ColumnName = "DeliveryType";

            dtcombo.Rows.Add();
            dtcombo.Rows[0][0] = "Godown";

            dtcombo.Rows.Add();
            dtcombo.Rows[1][0] = "Door Delivery";

            textBox14.Text = SelectCombo.ComboDt(this, dtcombo, 0);
            SendKeys.Send("{tab}");
        }

        private void textBox27_KeyPress(object sender, KeyPressEventArgs e)
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

            textBox27.Text = SelectCombo.ComboDt(this, dtcombo, 0);
            SendKeys.Send("{tab}");
        }

        private void textBox14_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox14);
        }

        private void textBox27_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox27);
        }

        private void textBox14_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox14);
        }

        private void textBox27_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox27);
        }

        private void textBox29_KeyPress(object sender, KeyPressEventArgs e)
        {
            strCombo = "SELECT    Name FROM ACCOUNTs WHERE     (act_id = 40) order by name";
            textBox29.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, textBox29.Text, 0);
        }

        private void textBox29_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox29);
        }

        private void textBox29_Layout(object sender, LayoutEventArgs e)
        {

        }

        private void textBox29_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox29);
        }

        private void textBox29_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C)
            {
                textBox29.Text = funs.AddAccount();
            }
            else if (e.Control && e.KeyCode == Keys.A)
            {
                textBox29.Text = funs.EditAccount(textBox29.Text);
            }
        }

        private void textBox30_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox30);
        }

        private void textBox30_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox30);
        }

        private void textBox30_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void GroupBox4_Enter(object sender, EventArgs e)
        {

        }
    }
}
