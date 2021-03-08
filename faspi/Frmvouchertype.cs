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
    public partial class Frmvouchertype : Form
    {
        DataTable dtVouchertype;
        string gstr = "";
        String strCombo;
        DataTable dtcal;
        DataTable dtcash;

        public Frmvouchertype()
        {
            InitializeComponent();
        }

        private void Frmvouchertype_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Control && e.KeyCode==Keys.S)
            {
                if (validate() == true)
                {
                    save();
                    if (gstr == "0")
                    {
                        LoadData("0", this.Text);
                    }
                    else
                    {
                        this.Close();
                        this.Dispose();
                    }
                }
            }
            if(e.KeyCode==Keys.Escape)
            {
                this.Close();
                this.Dispose();
            }
        }

        public void LoadData(string str,string FrmCaption)
        {
            dtVouchertype = new DataTable("VOUCHERTYPEs");
            Database.GetSqlData("Select * From Vouchertypes Where Vt_id="+str,dtVouchertype);
            this.Text = FrmCaption;
            gstr = str;

            if (dtVouchertype.Rows.Count < 0)
            {
                dtVouchertype.Rows.Add();
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";
                textBox5.Text = "";
                textBox6.Text = "";
                textBox7.Text = "";
                textBox8.Text = "";
                //textBox9.Text = "";
                textBox10.Text = "";
                textBox11.Text = "";
                //textBox12.Text = "";
                //textBox13.Text = "";
                textBox9.Text = "";
                textBox15.Text = "";
                textBox16.Text = "";
                textBox17.Text = "";
            }
            else
            {
                textBox1.Text = dtVouchertype.Rows[0]["Name"].ToString();
                textBox2.Text = dtVouchertype.Rows[0]["AliasName"].ToString();
                textBox3.Text = dtVouchertype.Rows[0]["Type"].ToString();
                textBox4.Text = dtVouchertype.Rows[0]["Short"].ToString();
                textBox5.Text = dtVouchertype.Rows[0]["Default1"].ToString();
                textBox6.Text = dtVouchertype.Rows[0]["Default3"].ToString();
                textBox7.Text = dtVouchertype.Rows[0]["ReportName"].ToString();
                textBox8.Text = dtVouchertype.Rows[0]["PaperSize"].ToString();
                //  textBox9.Text = dtVouchertype.Rows[0]["Exempted"].ToString();
                textBox11.Text = dtVouchertype.Rows[0]["Default2"].ToString();
                //textBox12.Text = dtVouchertype.Rows[0]["Calculation"].ToString();
                //textBox13.Text = dtVouchertype.Rows[0]["CashTransaction"].ToString();
                textBox14.Text = dtVouchertype.Rows[0]["Code"].ToString();
                textBox15.Text = dtVouchertype.Rows[0]["Prefix"].ToString();
                textBox17.Text = dtVouchertype.Rows[0]["Padding"].ToString();
                textBox16.Text = dtVouchertype.Rows[0]["Postfix"].ToString();
                textBox9.Text = dtVouchertype.Rows[0]["starting_no"].ToString();

                if (bool.Parse(dtVouchertype.Rows[0]["Stationary"].ToString()) == true)
                {
                    checkBox3.Checked = true;
                }
                if (bool.Parse(dtVouchertype.Rows[0]["ExState"].ToString()) == false)
                {
                    checkBox4.Checked = false;
                }
                else
                {
                    checkBox4.Checked = true;
                }
                if (bool.Parse(dtVouchertype.Rows[0]["TaxInvoice"].ToString()) == false)
                {
                    checkBox5.Checked = false;
                }
                else
                {
                    checkBox5.Checked = true;
                }
                if (bool.Parse(dtVouchertype.Rows[0]["Unregistered"].ToString()) == false)
                {
                    checkBox6.Checked = false;
                }
                else
                {
                    checkBox6.Checked = true;
                }
                if (bool.Parse(dtVouchertype.Rows[0]["Active"].ToString()) == false)
                {
                    checkBox7.Checked = false;
                }
                else
                {
                    checkBox7.Checked = true;
                }

                if (int.Parse(dtVouchertype.Rows[0]["NumType"].ToString()) == 1)
                {
                    radioButton1.Checked = true;
                }
                else if (int.Parse(dtVouchertype.Rows[0]["NumType"].ToString()) == 2)
                {
                    radioButton2.Checked = true;
                }
                else if (int.Parse(dtVouchertype.Rows[0]["NumType"].ToString()) == 3)
                {
                    radioButton3.Checked = true;
                }

                //if (dtVouchertype.Rows[0]["Effect_On_Stock"].ToString() == "Y")
                //{

                //    radioButton4.Checked = true;
                //}

                //else if (dtVouchertype.Rows[0]["Effect_On_Stock"].ToString() == "N")
                //{
                //    radioButton5.Checked = true;
                //}


                //if (dtVouchertype.Rows[0]["Effect_On_Acc"].ToString() =="Y")
                //{
                //    radioButton6.Checked = true;
                //}

                //else if (dtVouchertype.Rows[0]["Effect_On_Acc"].ToString() == "N")
                //{
                //    radioButton7.Checked = true;
                //}

                textBox10.Text = dtVouchertype.Rows[0]["Smstemplate"].ToString();

                String[] print_option = dtVouchertype.Rows[0]["printcopy"].ToString().Split(';');

                for (int j = 0; j < print_option.Length; j++)
                {
                    if (print_option[j] != "")
                    {
                        ansGridView5.Rows.Add();
                        String[] defaultcopy = print_option[j].Split(',');
                        ansGridView5.Rows[j].Cells["copyname"].Value = defaultcopy[0];
                        ansGridView5.Rows[j].Cells["defaultcopy"].Value = bool.Parse(defaultcopy[1].ToString());
                    }
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

            //close
            dtsidefill.Rows.Add();
            dtsidefill.Rows[2]["Name"] = "rearr";
            dtsidefill.Rows[2]["DisplayName"] = "Re-Arrange";
            dtsidefill.Rows[2]["ShortcutKey"] = "";
            dtsidefill.Rows[2]["Visible"] = true;

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
                    if (gstr == "0")
                    {
                        LoadData("0", this.Text);
                    }
                    else
                    {
                        this.Close();
                        this.Dispose();
                    }
                }
            }

            if (name == "rearr")
            {
                if (validate() == true)
                {
                    save();

                    //yearly
                    if (radioButton1.Checked == true)
                    {
                        int taxvno = 1;
                        DataTable dtvou = new DataTable();
                        Database.GetSqlData("SELECT * FROM VOUCHERINFOs WHERE Vt_id = " + gstr + " ORDER BY Vdate,Vnumber", dtvou);
                        for (int i = 0; i < dtvou.Rows.Count; i++)
                        {
                            Database.CommandExecutor("Update Voucherinfos set vnumber=" + taxvno + " where Vi_id=" + dtvou.Rows[i]["Vi_id"].ToString());
                            taxvno++;
                        }
                    }
                    //monthly

                    if (radioButton2.Checked == true)
                    {
                        DateTime vdate = new DateTime(1801, 04, 01);
                        DataTable dtvou = new DataTable();
                        int month = 0;
                        Database.GetSqlData("SELECT * FROM VOUCHERINFOs WHERE Vt_id = " + gstr + " ORDER BY Vdate,Vnumber", dtvou);
                        int vno = 0;
                        for (int i = 0; i < dtvou.Rows.Count; i++)
                        {
                            if (month == DateTime.Parse(dtvou.Rows[i]["Vdate"].ToString()).Month)
                            {
                                vno++;
                            }
                            else
                            {
                                vno = 1;
                            }
                            Database.CommandExecutor("Update Voucherinfos set vnumber=" + vno + " where Vi_id=" + dtvou.Rows[i]["Vi_id"].ToString());

                            month = DateTime.Parse(dtvou.Rows[i]["Vdate"].ToString()).Month;
                        }
                    }

                    //daily
                    if (radioButton3.Checked == true)
                    {
                        // int taxvno = 1;
                        DateTime vdate = new DateTime(1801, 4, 01);

                        DataTable dtvou = new DataTable();
                        Database.GetSqlData("SELECT * FROM VOUCHERINFOs WHERE Vt_id = " + gstr + " ORDER BY Vdate,Vnumber", dtvou);
                        int vno = 0;
                        for (int i = 0; i < dtvou.Rows.Count; i++)
                        {
                            if (vdate == DateTime.Parse(dtvou.Rows[i]["Vdate"].ToString()))
                            {
                                vno++;
                            }
                            else
                            {
                                vno = 1;
                            }

                            Database.CommandExecutor("Update Voucherinfos set vnumber=" + vno + " where Vi_id=" + dtvou.Rows[i]["Vi_id"].ToString());

                            vdate = DateTime.Parse(dtvou.Rows[i]["Vdate"].ToString());
                        }
                    }

                    int vtid = int.Parse(gstr);
                    string prefix = "";
                    string postfix = "";
                    int padding = 0;
                    prefix = Database.GetScalarText("Select prefix from Vouchertypes where vt_id=" + vtid);
                    postfix = Database.GetScalarText("Select postfix from Vouchertypes where vt_id=" + vtid);
                    padding = Database.GetScalarInt("Select padding from Vouchertypes where vt_id=" + vtid);

                    DataTable dtvouinvoice = new DataTable();
                    Database.GetSqlData("SELECT * FROM VOUCHERINFOs WHERE Vt_id = " + vtid, dtvouinvoice);
                    for (int i = 0; i < dtvouinvoice.Rows.Count; i++)
                    {
                        int vno = int.Parse(dtvouinvoice.Rows[i]["vnumber"].ToString());
                        string invoiceno = vno.ToString();
                        string inv_no = prefix + invoiceno.PadLeft(padding, '0') + postfix;
                        Database.CommandExecutor("Update Voucherinfos set Invoiceno='" + inv_no + "' where Vi_id=" + dtvouinvoice.Rows[i]["Vi_id"].ToString());
                    }
                    MessageBox.Show("Vouchers ReArrange Successfully.");

                    this.Close();
                    this.Dispose();
                }
            }
            if (name == "quit")
            {
                this.Close();
                this.Dispose();
            }
        }

        private void save()
        {
            dtVouchertype.Rows[0]["AliasName"] = textBox2.Text;
            dtVouchertype.Rows[0]["Short"] = textBox4.Text;
            dtVouchertype.Rows[0]["Default1"] = textBox5.Text;
            dtVouchertype.Rows[0]["Default3"] = textBox6.Text;
            dtVouchertype.Rows[0]["ReportName"] = textBox7.Text;
            dtVouchertype.Rows[0]["PaperSize"] = textBox8.Text;
            dtVouchertype.Rows[0]["Default2"] = textBox11.Text;
            //dtVouchertype.Rows[0]["Calculation"] = textBox12.Text;
            //dtVouchertype.Rows[0]["CashTransaction"] = textBox13.Text;
            dtVouchertype.Rows[0]["Code"] = textBox14.Text;
            dtVouchertype.Rows[0]["Prefix"] = textBox15.Text;
            dtVouchertype.Rows[0]["Padding"] = textBox17.Text;
            dtVouchertype.Rows[0]["Postfix"] = textBox16.Text;
            //dtVouchertype.Rows[0]["Exempted"] = textBox9.Text;
            dtVouchertype.Rows[0]["starting_no"] = textBox9.Text;
            if (checkBox3.Checked == true)
            {
                dtVouchertype.Rows[0]["Stationary"] = true;
            }
            else
            {
                dtVouchertype.Rows[0]["Stationary"] = false;
            }
            if (checkBox4.Checked == true)
            {
                dtVouchertype.Rows[0]["ExState"] = true;
            }
            else
            {
                dtVouchertype.Rows[0]["ExState"] = false;
            }
            if (checkBox5.Checked == false)
            {
                dtVouchertype.Rows[0]["TaxInvoice"] = false;
            }
            else
            {
                dtVouchertype.Rows[0]["TaxInvoice"] = true;
            }


            if (checkBox6.Checked == false)
            {
                dtVouchertype.Rows[0]["Unregistered"] = false;
            }
            else
            {
                dtVouchertype.Rows[0]["Unregistered"] = true;
            }

            if (checkBox7.Checked == false)
            {
                dtVouchertype.Rows[0]["Active"] = false;
            }
            else
            {
                dtVouchertype.Rows[0]["Active"] = true;
            }

            if(radioButton1.Checked==true)
            {
               dtVouchertype.Rows[0]["NumType"] = 1;
            }
            if(radioButton2.Checked==true)
            {
                dtVouchertype.Rows[0]["NumType"] = 2;
            }
            if (radioButton3.Checked == true)
            {
                dtVouchertype.Rows[0]["NumType"] = 3;
            }
            // if (radioButton4.Checked == true)
            //{
            //    dtVouchertype.Rows[0]["Effect_On_Stock"] = "Y";
            //}
            //if (radioButton5.Checked == true)
            //{
            //    dtVouchertype.Rows[0]["Effect_On_Stock"] = "N";
            //}
            //if (radioButton6.Checked == true)
            //{
            //    dtVouchertype.Rows[0]["Effect_On_Acc"] = "Y";
            //}
            //if (radioButton7.Checked == true)
            //{
            //    dtVouchertype.Rows[0]["Effect_On_Acc"] = "N";
            //}
            dtVouchertype.Rows[0]["Smstemplate"] = textBox10.Text;

            string printcopy = "";
            for (int i = 0; i < ansGridView5.Rows.Count; i++)
            {
                printcopy = printcopy + ansGridView5.Rows[i].Cells["copyname"].Value + "," + ansGridView5.Rows[i].Cells["defaultcopy"].Value+";";
            }
            dtVouchertype.Rows[0]["printcopy"] = printcopy;

            Database.SaveData(dtVouchertype);

            funs.ShowBalloonTip("Saved", "Saved Successfully");
        }


        private bool validate()
        {
            if (textBox2.Text == "")
            {
                MessageBox.Show("Please Enter Alias Name");
                return false;
            }
            //if (textBox9.Text == "")
            //{
            //    MessageBox.Show("Please Enter Exempted Goods Value");
            //    return false;
            //}
            else if(textBox8.Text=="")
            {
                MessageBox.Show("Please Enter Paper Size");
                return false;
            }


            return true;
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            //yearly
            if (radioButton1.Checked == true)
            {
                int taxvno = 1;
                DataTable dtvou = new DataTable();
                Database.GetSqlData("SELECT * FROM VOUCHERINFOs WHERE Vt_id = " + gstr + " ORDER BY Vdate,Vnumber", dtvou);
                for (int i = 0; i < dtvou.Rows.Count; i++)
                {
                    Database.CommandExecutor("Update Voucherinfos set vnumber=" + taxvno + " where Vi_id=" + dtvou.Rows[i]["Vi_id"].ToString());
                    taxvno++;
                }
            }
            //monthly

            if (radioButton2.Checked == true)
            {
               
                DateTime vdate = new DateTime(1801, 04, 01);
                DataTable dtvou = new DataTable();
                int month = 0;
                Database.GetSqlData("SELECT * FROM VOUCHERINFOs WHERE Vt_id = " + gstr + " ORDER BY Vdate,Vnumber", dtvou);
                int vno = 0;
                for (int i = 0; i < dtvou.Rows.Count; i++)
                {


                    if (month == DateTime.Parse(dtvou.Rows[i]["Vdate"].ToString()).Month)
                    {
                        vno++;
                    }
                    else
                    {
                        vno = 1;
                    }

                    Database.CommandExecutor("Update Voucherinfos set vnumber=" + vno + " where Vi_id=" + dtvou.Rows[i]["Vi_id"].ToString());



                    month = DateTime.Parse(dtvou.Rows[i]["Vdate"].ToString()).Month;
                }
                MessageBox.Show("Done");
            }


            //daily
            if (radioButton3.Checked == true)
            {
               // int taxvno = 1;
                DateTime vdate = new DateTime(1801, 4, 01);
              
                DataTable dtvou = new DataTable();
                Database.GetSqlData("SELECT * FROM VOUCHERINFOs WHERE Vt_id = " + gstr + " ORDER BY Vdate,Vnumber", dtvou);
                int vno = 0;
                for (int i = 0; i < dtvou.Rows.Count; i++)
                {


                    if (vdate == DateTime.Parse(dtvou.Rows[i]["Vdate"].ToString()))
                    {
                        vno++;
                    }
                    else
                    {
                        vno = 1;
                    }

                    Database.CommandExecutor("Update Voucherinfos set vnumber=" + vno + " where Vi_id=" + dtvou.Rows[i]["Vi_id"].ToString());
                  
                    vdate = DateTime.Parse(dtvou.Rows[i]["Vdate"].ToString());
                }
                
            }

            int vtid = int.Parse(gstr);
            string prefix = "";
            string postfix = "";
            int padding = 0;
            prefix = Database.GetScalarText("Select prefix from Vouchertypes where vt_id=" + vtid);
            postfix = Database.GetScalarText("Select postfix from Vouchertypes where vt_id=" + vtid);
            padding = Database.GetScalarInt("Select padding from Vouchertypes where vt_id=" + vtid);


            DataTable dtvouinvoice = new DataTable();
            Database.GetSqlData("SELECT * FROM VOUCHERINFOs WHERE Vt_id = " + vtid, dtvouinvoice);
            for (int i = 0; i < dtvouinvoice.Rows.Count; i++)
            {

                int vno = int.Parse(dtvouinvoice.Rows[i]["vnumber"].ToString());
                string invoiceno = vno.ToString();
                string inv_no = prefix + invoiceno.PadLeft(padding, '0') + postfix;
                Database.CommandExecutor("Update Voucherinfos set Invoiceno='" + inv_no + "' where Vi_id=" + dtvouinvoice.Rows[i]["Vi_id"].ToString());
            }
            MessageBox.Show("Done");

               
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if(validate()==true)
            {
                save();
                if (gstr == "0")
                {

                    LoadData("0", this.Text);
                }
                else
                {
                    this.Close();
                    this.Dispose();
                }

            }
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox4_KeyDown(object sender, KeyEventArgs e)
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

        private void textBox7_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox8_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox10_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void checkBox1_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void checkBox2_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void checkBox3_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void radioButton6_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void radioButton7_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void radioButton4_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void radioButton5_KeyDown(object sender, KeyEventArgs e)
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

        private void radioButton3_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void ChangeText()
        {
            dtVouchertype.Rows[0]["Smstemplate"] = textBox10.Text;
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox2);
        }

        private void textBox4_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox4);
        }

        private void textBox5_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox5);
        }

        private void textBox6_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox6);
        }

        private void textBox7_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox7);
        }

        private void textBox8_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox8);
        }

      

        private void textBox1_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox1);
        }

        private void textBox3_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox3);
        }

        private void textBox10_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox10);
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

        private void textBox4_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox4);
        }

        private void textBox5_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox5);
        }

        private void textBox6_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox6);
        }

        private void textBox7_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox7);
        }

        private void textBox8_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox8);
        }

        
        private void checkBox1_Leave(object sender, EventArgs e)
        {
           // Database.lostFocus(checkBox1);
        }

        private void checkBox1_Enter(object sender, EventArgs e)
        {
            //Database.setFocus(checkBox1);
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }


        private void checkBox4_Leave(object sender, EventArgs e)
        {
            
        }

        private void checkBox4_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void checkBox5_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this,e.KeyCode);
        }

        private void checkBox6_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void checkBox7_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void checkBox2_Enter(object sender, EventArgs e)
        {
           
        }

        private void textBox10_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox10);
        }

        private void textBox11_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox11);
        }

        private void textBox11_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox11_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox11);
        }

        private void textBox12_KeyDown(object sender, KeyEventArgs e)
        {
            dtcal = new DataTable();
            dtcal.Columns.Add("CalculationType",typeof(string));

            dtcal.Rows.Add();
            dtcal.Rows[0][0] = "Not Applicable";


            dtcal.Rows.Add();
            dtcal.Rows[1][0] = "Including Tax Only";

            dtcal.Rows.Add();
            dtcal.Rows[2][0] = "Excluding Tax Only";


            dtcal.Rows.Add();
            dtcal.Rows[3][0] = "Default Including Tax";

            dtcal.Rows.Add();
            dtcal.Rows[4][0] = "Default Excluding Tax";


            //textBox12.Text = SelectCombo.ComboDt(this, dtcal, 0);
        }

        private void textBox13_KeyDown(object sender, KeyEventArgs e)
        {
            dtcash = new DataTable();
            dtcash.Columns.Add("Cash Transaction", typeof(string));

            dtcash.Rows.Add();
            dtcash.Rows[0][0] = "Allowed";


            dtcash.Rows.Add();
            dtcash.Rows[1][0] = "Not Allowed";

            dtcash.Rows.Add();
            dtcash.Rows[2][0] = "Only Allowed";

            //textBox13.Text = SelectCombo.ComboDt(this, dtcash, 0);
        }

        private void Frmvouchertype_Load(object sender, EventArgs e)
        {
            this.Size = this.MdiParent.Size;
            SideFill();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox7_Enter(object sender, EventArgs e)
        {

        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void ansGridView5_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            ansGridView5.CurrentCell.Value = 0;
        }

        private void ansGridView5_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (ansGridView5.CurrentRow.Index == ansGridView5.Rows.Count)
                {
                    for (int i = 1; i < ansGridView5.Columns.Count; i++)
                    {
                        ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells[i].Value = null;
                        
                    }
                }
                else
                {
                    int rindex = ansGridView5.CurrentRow.Index;
                    ansGridView5.Rows.RemoveAt(rindex);
                    
                    return;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            InputBox box = new InputBox("Copy Name", "", false);
            box.ShowInTaskbar = false;
            box.ShowDialog(this);
            if (box.outStr != null || box.outStr != " ")
            {
                ansGridView5.Rows.Add();
                
                ansGridView5.Rows[ansGridView5.Rows.Count-1].Cells["copyname"].Value = box.outStr;
                ansGridView5.Rows[ansGridView5.Rows.Count - 1].Cells["defaultcopy"].Value = true;
                ansGridView5.CurrentCell = ansGridView5.Rows[ansGridView5.Rows.Count - 1].Cells[0];
            }
        }

        private void ansGridView5_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (ansGridView5.CurrentRow.Index == ansGridView5.Rows.Count)
            {

            }
            else
            {

            }

            int rindex = ansGridView5.CurrentRow.Index;
            ansGridView5.Rows.RemoveAt(rindex);

        }

        private void button3_Click(object sender, EventArgs e)
        {
           
        }

        private void textBox15_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox5_Enter(object sender, EventArgs e)
        {

        }

        private void textBox9_KeyDown(object sender, KeyEventArgs e)
        {
            dtcal = new DataTable();
            dtcal.Columns.Add("Exempted", typeof(string));

            dtcal.Rows.Add();
            dtcal.Rows[0][0] = "Allowed";


            dtcal.Rows.Add();
            dtcal.Rows[1][0] = "Not Allowed";

            dtcal.Rows.Add();
            dtcal.Rows[2][0] = "Only Allowed";


           
            //textBox9.Text = SelectCombo.ComboDt(this, dtcal, 0);
        }

      

    }
}
