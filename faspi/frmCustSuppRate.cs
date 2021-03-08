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
    public partial class frmCustSuppRate : Form
    {
        DataTable dtCustSuppRate;
        string dtName;
        string Ac_id = "";
        string des_id = "";
        string strCombo;
        DateTime create_date = DateTime.Parse(System.DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"));

        public frmCustSuppRate()
        {
            InitializeComponent();
        }

        public void LoadData(string acid, string did, string frmcaption)
        {
            Ac_id = acid;
            des_id = did;
            this.Text = frmcaption;
            dtName = "PARTYRATE";
            dtCustSuppRate = new DataTable(dtName);
            Database.GetSqlData("SELECT * from PARTYRATEs where Ac_id = '" + acid + "' and Des_id='" + did + "'", dtCustSuppRate);

            if (dtCustSuppRate.Rows.Count == 0)
            {
                ansGridView1.Rows.Clear();
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "0";
                textBox4.Text = "0";
                textBox5.Text = "0";
            }
            else
            {
                textBox1.Enabled = false;
                textBox2.Enabled = false;
                textBox1.Text = funs.Select_ac_nm(acid);
                textBox2.Text = funs.Select_item_nm(did);

                for (int i = 0; i < dtCustSuppRate.Rows.Count; i++)
                {
                    textBox3.Text = funs.IndianCurr(Convert.ToDouble(dtCustSuppRate.Rows[0]["Mini_weight"]));
                    textBox4.Text = funs.IndianCurr(Convert.ToDouble(dtCustSuppRate.Rows[0]["Charged_weight"]));
                    textBox5.Text = funs.IndianCurr(Convert.ToDouble(dtCustSuppRate.Rows[0]["St_weight"]));

                    create_date = DateTime.Parse(dtCustSuppRate.Rows[0]["create_date"].ToString());

                    if (Convert.ToInt32(dtCustSuppRate.Rows[0]["Rounding_ex"]) == 0)
                    {
                        radioButton1.Checked = true;
                    }
                    if (Convert.ToInt32(dtCustSuppRate.Rows[0]["Rounding_ex"]) == 1)
                    {
                        radioButton2.Checked = true;
                    }
                    if (Convert.ToInt32(dtCustSuppRate.Rows[0]["Rounding_ex"]) == 5)
                    {
                        radioButton3.Checked = true;
                    }
                    if (Convert.ToInt32(dtCustSuppRate.Rows[0]["Rounding_ex"]) == 10)
                    {
                        radioButton4.Checked = true;
                    }

                    if (Convert.ToInt32(dtCustSuppRate.Rows[0]["Rounding_ch"]) == 0)
                    {
                        radioButton8.Checked = true;
                    }
                    if (Convert.ToInt32(dtCustSuppRate.Rows[0]["Rounding_ch"]) == 1)
                    {
                        radioButton7.Checked = true;
                    }
                    if (Convert.ToInt32(dtCustSuppRate.Rows[0]["Rounding_ch"]) == 5)
                    {
                        radioButton6.Checked = true;
                    }
                    if (Convert.ToInt32(dtCustSuppRate.Rows[0]["Rounding_ch"]) == 10)
                    {
                        radioButton5.Checked = true;
                    }
                    if (Convert.ToInt32(dtCustSuppRate.Rows[0]["Rounding_ch"]) == 25)
                    {
                        radioButton11.Checked = true;
                    }
                    if (Convert.ToInt32(dtCustSuppRate.Rows[0]["Rounding_ch"]) == 50)
                    {
                        radioButton10.Checked = true;
                    }
                    if (Convert.ToInt32(dtCustSuppRate.Rows[0]["Rounding_ch"]) == 100)
                    {
                        radioButton9.Checked = true;
                    }

                    ansGridView1.Rows.Add();
                    ansGridView1.Rows[i].Cells["SNo"].Value = i + 1;
                    //ansGridView1.Rows[i].Cells["SNo"].Value = dtCustSuppRate.Rows[i]["Itemsr"].ToString();
                    ansGridView1.Rows[i].Cells["station"].Value = funs.Select_dp_nm(dtCustSuppRate.Rows[i]["Source_id"].ToString());
                    ansGridView1.Rows[i].Cells["destination"].Value = funs.Select_dp_nm(dtCustSuppRate.Rows[i]["Destination_id"].ToString());

                    ansGridView1.Rows[i].Cells["rate0"].Value = funs.IndianCurr(double.Parse(dtCustSuppRate.Rows[i]["Expense0"].ToString()));
                    ansGridView1.Rows[i].Cells["rate1"].Value = funs.IndianCurr(double.Parse(dtCustSuppRate.Rows[i]["Expense1"].ToString()));
                    ansGridView1.Rows[i].Cells["rate2"].Value = funs.IndianCurr(double.Parse(dtCustSuppRate.Rows[i]["Expense2"].ToString()));
                    ansGridView1.Rows[i].Cells["rate3"].Value = funs.IndianCurr(double.Parse(dtCustSuppRate.Rows[i]["Expense3"].ToString()));
                    ansGridView1.Rows[i].Cells["rate4"].Value = funs.IndianCurr(double.Parse(dtCustSuppRate.Rows[i]["Expense4"].ToString()));
                    ansGridView1.Rows[i].Cells["rate5"].Value = funs.IndianCurr(double.Parse(dtCustSuppRate.Rows[i]["Expense5"].ToString()));
                    ansGridView1.Rows[i].Cells["rate6"].Value = funs.IndianCurr(double.Parse(dtCustSuppRate.Rows[i]["Expense6"].ToString()));
                    ansGridView1.Rows[i].Cells["rate7"].Value = funs.IndianCurr(double.Parse(dtCustSuppRate.Rows[i]["Expense7"].ToString()));
                    ansGridView1.Rows[i].Cells["rate8"].Value = funs.IndianCurr(double.Parse(dtCustSuppRate.Rows[i]["Expense8"].ToString()));
                    ansGridView1.Rows[i].Cells["rate9"].Value = funs.IndianCurr(double.Parse(dtCustSuppRate.Rows[i]["Expense9"].ToString()));
                    ansGridView1.Rows[i].Cells["rate10"].Value = funs.IndianCurr(double.Parse(dtCustSuppRate.Rows[i]["Expense10"].ToString()));
                    ansGridView1.Rows[i].Cells["rate11"].Value = funs.IndianCurr(double.Parse(dtCustSuppRate.Rows[i]["Expense11"].ToString()));


                    ansGridView1.Rows[i].Cells["mini0"].Value = funs.IndianCurr(double.Parse(dtCustSuppRate.Rows[i]["MRExpense0"].ToString()));
                    ansGridView1.Rows[i].Cells["mini1"].Value = funs.IndianCurr(double.Parse(dtCustSuppRate.Rows[i]["MRExpense1"].ToString()));
                    ansGridView1.Rows[i].Cells["mini2"].Value = funs.IndianCurr(double.Parse(dtCustSuppRate.Rows[i]["MRExpense2"].ToString()));
                    ansGridView1.Rows[i].Cells["mini3"].Value = funs.IndianCurr(double.Parse(dtCustSuppRate.Rows[i]["MRExpense3"].ToString()));
                    ansGridView1.Rows[i].Cells["mini4"].Value = funs.IndianCurr(double.Parse(dtCustSuppRate.Rows[i]["MRExpense4"].ToString()));
                    ansGridView1.Rows[i].Cells["mini5"].Value = funs.IndianCurr(double.Parse(dtCustSuppRate.Rows[i]["MRExpense5"].ToString()));
                    ansGridView1.Rows[i].Cells["mini6"].Value = funs.IndianCurr(double.Parse(dtCustSuppRate.Rows[i]["MRExpense6"].ToString()));
                    ansGridView1.Rows[i].Cells["mini7"].Value = funs.IndianCurr(double.Parse(dtCustSuppRate.Rows[i]["MRExpense7"].ToString()));
                    ansGridView1.Rows[i].Cells["mini8"].Value = funs.IndianCurr(double.Parse(dtCustSuppRate.Rows[i]["MRExpense8"].ToString()));
                    ansGridView1.Rows[i].Cells["mini9"].Value = funs.IndianCurr(double.Parse(dtCustSuppRate.Rows[i]["MRExpense9"].ToString()));
                    ansGridView1.Rows[i].Cells["mini10"].Value = funs.IndianCurr(double.Parse(dtCustSuppRate.Rows[i]["MRExpense10"].ToString()));
                    ansGridView1.Rows[i].Cells["mini11"].Value = funs.IndianCurr(double.Parse(dtCustSuppRate.Rows[i]["MRExpense11"].ToString()));


                    ansGridView1.Rows[i].Cells["type0"].Value = dtCustSuppRate.Rows[i]["ExpenseType0"];
                    ansGridView1.Rows[i].Cells["type1"].Value = dtCustSuppRate.Rows[i]["ExpenseType1"];
                    ansGridView1.Rows[i].Cells["type2"].Value = dtCustSuppRate.Rows[i]["ExpenseType2"];
                    ansGridView1.Rows[i].Cells["type3"].Value = dtCustSuppRate.Rows[i]["ExpenseType3"];
                    ansGridView1.Rows[i].Cells["type4"].Value = dtCustSuppRate.Rows[i]["ExpenseType4"];
                    ansGridView1.Rows[i].Cells["type5"].Value = dtCustSuppRate.Rows[i]["ExpenseType5"];
                    ansGridView1.Rows[i].Cells["type6"].Value = dtCustSuppRate.Rows[i]["ExpenseType6"];
                    ansGridView1.Rows[i].Cells["type7"].Value = dtCustSuppRate.Rows[i]["ExpenseType7"];
                    ansGridView1.Rows[i].Cells["type8"].Value = dtCustSuppRate.Rows[i]["ExpenseType8"];
                    ansGridView1.Rows[i].Cells["type9"].Value = dtCustSuppRate.Rows[i]["ExpenseType9"];
                    ansGridView1.Rows[i].Cells["type10"].Value = dtCustSuppRate.Rows[i]["ExpenseType10"];
                    ansGridView1.Rows[i].Cells["type11"].Value = dtCustSuppRate.Rows[i]["ExpenseType11"];
                }
            }

            ansGridView1.Columns["rate0"].CellTemplate.ValueType = typeof(double);
            ansGridView1.Columns["rate11"].CellTemplate.ValueType = typeof(double);
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
                    //try
                    //{
                    //    Database.BeginTran();
                        save();
                    //    Database.CommitTran();
                    //}
                    //catch (Exception ex)
                    //{
                    //    MessageBox.Show("Not Saved", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //    Database.RollbackTran();
                    //}
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
            string did = funs.Select_item_name_pack_id(textBox2.Text);

            DataTable dtTemp = new DataTable("PARTYRATEs");
            Database.GetSqlData("select * from PARTYRATEs where Ac_id='" + funs.Select_ac_id(textBox1.Text) + "' and Des_id='" + funs.Select_item_name_pack_id(textBox2.Text) + "'", dtTemp);
            for (int i = 0; i < dtTemp.Rows.Count; i++)
            {
                dtTemp.Rows[i].Delete();
            }
            Database.SaveData(dtTemp);

            dtCustSuppRate = new DataTable("PARTYRATEs");
            Database.GetSqlData("select * from PARTYRATEs where Ac_id='" + funs.Select_ac_id(textBox1.Text) + "' and Des_id='" + funs.Select_item_name_pack_id(textBox2.Text) + "'", dtCustSuppRate);

            int Nid2 = 1;
            DataTable dtidvd = new DataTable();
            Database.GetSqlData("select max(Nid) as Nid from PARTYRATEs where locationid='" + Database.LocationId + "'", dtidvd);
            if (dtidvd.Rows[0][0].ToString() != "")
            {
                Nid2 = int.Parse(dtidvd.Rows[0][0].ToString()) + 1;
            }

            for (int i = 0; i < ansGridView1.Rows.Count - 1; i++)
            {
                dtCustSuppRate.Rows.Add();

                dtCustSuppRate.Rows[i]["Nid"] = Nid2;
                dtCustSuppRate.Rows[i]["LocationId"] = Database.LocationId;
                dtCustSuppRate.Rows[i]["PRateID"] = Database.LocationId + dtCustSuppRate.Rows[i]["nid"].ToString();

                dtCustSuppRate.Rows[i]["create_date"] = create_date;
                dtCustSuppRate.Rows[i]["modify_date"] = System.DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");

                //dtCustSuppRate.Rows[i]["Itemsr"] = i + 1;
                dtCustSuppRate.Rows[i]["Ac_id"] = funs.Select_ac_id(textBox1.Text);
                dtCustSuppRate.Rows[i]["Des_id"] = did;
                
                dtCustSuppRate.Rows[i]["Source_id"] = funs.Select_dp_id(ansGridView1.Rows[i].Cells["station"].Value.ToString());
                dtCustSuppRate.Rows[i]["Destination_id"] = funs.Select_dp_id(ansGridView1.Rows[i].Cells["destination"].Value.ToString());

                if (ansGridView1.Rows[i].Cells["rate0"].Value == null)
                {
                    ansGridView1.Rows[i].Cells["rate0"].Value = 0;
                }
                if (ansGridView1.Rows[i].Cells["rate1"].Value == null)
                {
                    ansGridView1.Rows[i].Cells["rate1"].Value = 0;
                }
                if (ansGridView1.Rows[i].Cells["rate2"].Value == null)
                {
                    ansGridView1.Rows[i].Cells["rate2"].Value = 0;
                }
                if (ansGridView1.Rows[i].Cells["rate3"].Value == null)
                {
                    ansGridView1.Rows[i].Cells["rate3"].Value = 0;
                }
                if (ansGridView1.Rows[i].Cells["rate4"].Value == null)
                {
                    ansGridView1.Rows[i].Cells["rate4"].Value = 0;
                }
                if (ansGridView1.Rows[i].Cells["rate5"].Value == null)
                {
                    ansGridView1.Rows[i].Cells["rate5"].Value = 0;
                }
                if (ansGridView1.Rows[i].Cells["rate6"].Value == null)
                {
                    ansGridView1.Rows[i].Cells["rate6"].Value = 0;
                }
                if (ansGridView1.Rows[i].Cells["rate7"].Value == null)
                {
                    ansGridView1.Rows[i].Cells["rate7"].Value = 0;
                }
                if (ansGridView1.Rows[i].Cells["rate8"].Value == null)
                {
                    ansGridView1.Rows[i].Cells["rate8"].Value = 0;
                }
                if (ansGridView1.Rows[i].Cells["rate9"].Value == null)
                {
                    ansGridView1.Rows[i].Cells["rate9"].Value = 0;
                }
                if (ansGridView1.Rows[i].Cells["rate10"].Value == null)
                {
                    ansGridView1.Rows[i].Cells["rate10"].Value = 0;
                }

                if (ansGridView1.Rows[i].Cells["rate11"].Value == null)
                {
                    ansGridView1.Rows[i].Cells["rate11"].Value = 0;
                }
                dtCustSuppRate.Rows[i]["Expense0"] = ansGridView1.Rows[i].Cells["rate0"].Value;
                dtCustSuppRate.Rows[i]["Expense1"] = ansGridView1.Rows[i].Cells["rate1"].Value;
                dtCustSuppRate.Rows[i]["Expense2"] = ansGridView1.Rows[i].Cells["rate2"].Value;
                dtCustSuppRate.Rows[i]["Expense3"] = ansGridView1.Rows[i].Cells["rate3"].Value;
                dtCustSuppRate.Rows[i]["Expense4"] = ansGridView1.Rows[i].Cells["rate4"].Value;
                dtCustSuppRate.Rows[i]["Expense5"] = ansGridView1.Rows[i].Cells["rate5"].Value;
                dtCustSuppRate.Rows[i]["Expense6"] = ansGridView1.Rows[i].Cells["rate6"].Value;
                dtCustSuppRate.Rows[i]["Expense7"] = ansGridView1.Rows[i].Cells["rate7"].Value;
                dtCustSuppRate.Rows[i]["Expense8"] = ansGridView1.Rows[i].Cells["rate8"].Value;
                dtCustSuppRate.Rows[i]["Expense9"] = ansGridView1.Rows[i].Cells["rate9"].Value;
                dtCustSuppRate.Rows[i]["Expense10"] = ansGridView1.Rows[i].Cells["rate10"].Value;
                dtCustSuppRate.Rows[i]["Expense11"] = ansGridView1.Rows[i].Cells["rate11"].Value;

                if (ansGridView1.Rows[i].Cells["mini0"].Value == null)
                {
                    ansGridView1.Rows[i].Cells["mini0"].Value = 0;
                }
                if (ansGridView1.Rows[i].Cells["mini1"].Value == null)
                {
                    ansGridView1.Rows[i].Cells["mini1"].Value = 0;
                }
                if (ansGridView1.Rows[i].Cells["mini2"].Value == null)
                {
                    ansGridView1.Rows[i].Cells["mini2"].Value = 0;
                }
                if (ansGridView1.Rows[i].Cells["mini3"].Value == null)
                {
                    ansGridView1.Rows[i].Cells["mini3"].Value = 0;
                }
                if (ansGridView1.Rows[i].Cells["mini4"].Value == null)
                {
                    ansGridView1.Rows[i].Cells["mini4"].Value = 0;
                }
                if (ansGridView1.Rows[i].Cells["mini5"].Value == null)
                {
                    ansGridView1.Rows[i].Cells["mini5"].Value = 0;
                }
                if (ansGridView1.Rows[i].Cells["mini6"].Value == null)
                {
                    ansGridView1.Rows[i].Cells["mini6"].Value = 0;
                }
                if (ansGridView1.Rows[i].Cells["mini7"].Value == null)
                {
                    ansGridView1.Rows[i].Cells["mini7"].Value = 0;
                }
                if (ansGridView1.Rows[i].Cells["mini8"].Value == null)
                {
                    ansGridView1.Rows[i].Cells["mini8"].Value = 0;
                }
                if (ansGridView1.Rows[i].Cells["mini9"].Value == null)
                {
                    ansGridView1.Rows[i].Cells["mini9"].Value = 0;
                }
                if (ansGridView1.Rows[i].Cells["mini10"].Value == null)
                {
                    ansGridView1.Rows[i].Cells["mini10"].Value = 0;
                }
                if (ansGridView1.Rows[i].Cells["mini11"].Value == null)
                {
                    ansGridView1.Rows[i].Cells["mini11"].Value = 0;
                }

                dtCustSuppRate.Rows[i]["MRExpense0"] = ansGridView1.Rows[i].Cells["mini0"].Value;
                dtCustSuppRate.Rows[i]["MRExpense1"] = ansGridView1.Rows[i].Cells["mini1"].Value;
                dtCustSuppRate.Rows[i]["MRExpense2"] = ansGridView1.Rows[i].Cells["mini2"].Value;
                dtCustSuppRate.Rows[i]["MRExpense3"] = ansGridView1.Rows[i].Cells["mini3"].Value;
                dtCustSuppRate.Rows[i]["MRExpense4"] = ansGridView1.Rows[i].Cells["mini4"].Value;
                dtCustSuppRate.Rows[i]["MRExpense5"] = ansGridView1.Rows[i].Cells["mini5"].Value;
                dtCustSuppRate.Rows[i]["MRExpense6"] = ansGridView1.Rows[i].Cells["mini6"].Value;
                dtCustSuppRate.Rows[i]["MRExpense7"] = ansGridView1.Rows[i].Cells["mini7"].Value;
                dtCustSuppRate.Rows[i]["MRExpense8"] = ansGridView1.Rows[i].Cells["mini8"].Value;
                dtCustSuppRate.Rows[i]["MRExpense9"] = ansGridView1.Rows[i].Cells["mini9"].Value;
                dtCustSuppRate.Rows[i]["MRExpense10"] = ansGridView1.Rows[i].Cells["mini10"].Value;
                dtCustSuppRate.Rows[i]["MRExpense11"] = ansGridView1.Rows[i].Cells["mini11"].Value;

                dtCustSuppRate.Rows[i]["ExpenseType0"] = ansGridView1.Rows[i].Cells["type0"].Value.ToString();
                dtCustSuppRate.Rows[i]["ExpenseType1"] = ansGridView1.Rows[i].Cells["type1"].Value.ToString();
                dtCustSuppRate.Rows[i]["ExpenseType2"] = ansGridView1.Rows[i].Cells["type2"].Value.ToString();
                dtCustSuppRate.Rows[i]["ExpenseType3"] = ansGridView1.Rows[i].Cells["type3"].Value.ToString();
                dtCustSuppRate.Rows[i]["ExpenseType4"] = ansGridView1.Rows[i].Cells["type4"].Value.ToString();
                dtCustSuppRate.Rows[i]["ExpenseType5"] = ansGridView1.Rows[i].Cells["type5"].Value.ToString();
                dtCustSuppRate.Rows[i]["ExpenseType6"] = ansGridView1.Rows[i].Cells["type6"].Value.ToString();
                dtCustSuppRate.Rows[i]["ExpenseType7"] = ansGridView1.Rows[i].Cells["type7"].Value.ToString();
                dtCustSuppRate.Rows[i]["ExpenseType8"] = ansGridView1.Rows[i].Cells["type8"].Value.ToString();
                dtCustSuppRate.Rows[i]["ExpenseType9"] = ansGridView1.Rows[i].Cells["type9"].Value.ToString();
                dtCustSuppRate.Rows[i]["ExpenseType10"] = ansGridView1.Rows[i].Cells["type10"].Value.ToString();
                dtCustSuppRate.Rows[i]["ExpenseType11"] = ansGridView1.Rows[i].Cells["type11"].Value.ToString();

                dtCustSuppRate.Rows[i]["Mini_weight"] = textBox3.Text;
                dtCustSuppRate.Rows[i]["Charged_weight"] = textBox4.Text;
                dtCustSuppRate.Rows[i]["St_weight"] = textBox5.Text;

                if (radioButton1.Checked == true)
                {
                    dtCustSuppRate.Rows[i]["Rounding_ex"] = 0;
                }
                if (radioButton2.Checked == true)
                {
                    dtCustSuppRate.Rows[i]["Rounding_ex"] = 1;
                }
                if (radioButton3.Checked == true)
                {
                    dtCustSuppRate.Rows[i]["Rounding_ex"] = 5;
                }
                if (radioButton4.Checked == true)
                {
                    dtCustSuppRate.Rows[i]["Rounding_ex"] = 10;
                }

                if (radioButton8.Checked == true)
                {
                    dtCustSuppRate.Rows[i]["Rounding_ch"] = 0;
                }
                if (radioButton7.Checked == true)
                {
                    dtCustSuppRate.Rows[i]["Rounding_ch"] = 1;
                }
                if (radioButton6.Checked == true)
                {
                    dtCustSuppRate.Rows[i]["Rounding_ch"] = 5;
                }
                if (radioButton5.Checked == true)
                {
                    dtCustSuppRate.Rows[i]["Rounding_ch"] = 10;
                }
                if (radioButton11.Checked == true)
                {
                    dtCustSuppRate.Rows[i]["Rounding_ch"] = 25;
                }
                if (radioButton10.Checked == true)
                {
                    dtCustSuppRate.Rows[i]["Rounding_ch"] = 50;
                }
                if (radioButton9.Checked == true)
                {
                    dtCustSuppRate.Rows[i]["Rounding_ch"] = 100;
                }

                Nid2++;
            
            }

            Database.SaveData(dtCustSuppRate);
            MessageBox.Show("Saved Successfully");

            if (Ac_id == "0" && des_id == "0")
            {
                LoadData("0", "0", this.Text);
            }
            else
            {
                this.Close();
                this.Dispose();
            }
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

            if (textBox3.Text.Trim() == "")
            {
                textBox3.Text = "0";
            }
            if (textBox4.Text.Trim() == "")
            {
                textBox4.Text = "0";
            }
           
            if (ansGridView1.Rows.Count - 1 == 0)
            {
                MessageBox.Show("Enter some Values");
                return false;
            }
            return true;
        }       

        private void frmCustSuppRate_Load(object sender, EventArgs e)
        {
            this.Size = this.MdiParent.Size;
            SideFill();
            
            string exp1 = Feature.Available("Name of Expense1");
            string exp2 = Feature.Available("Name of Expense2");
            string exp3 = Feature.Available("Name of Expense3");
            string exp4 = Feature.Available("Name of Expense4");
            string exp5 = Feature.Available("Name of Expense5");
            string exp6 = Feature.Available("Name of Expense6");
            string exp7 = Feature.Available("Name of Expense7");
            string exp8 = Feature.Available("Name of Expense8");
            string exp9 = Feature.Available("Name of Expense9");
            string exp10 = Feature.Available("Name of Expense10");
            string exp11 = Feature.Available("Name of Expense11");
            ansGridView1.Columns["rate1"].HeaderText = exp1 + " Rate";
            ansGridView1.Columns["rate2"].HeaderText = exp2 + " Rate";
            ansGridView1.Columns["rate3"].HeaderText = exp3 + " Rate";
            ansGridView1.Columns["rate4"].HeaderText = exp4 + " Rate";
            ansGridView1.Columns["rate5"].HeaderText = exp5 + " Rate";
            ansGridView1.Columns["rate6"].HeaderText = exp6 + " Rate";
            ansGridView1.Columns["rate7"].HeaderText = exp7 + " Rate";
            ansGridView1.Columns["rate8"].HeaderText = exp8 + " Rate";
            ansGridView1.Columns["rate9"].HeaderText = exp9 + " Rate";
            ansGridView1.Columns["rate10"].HeaderText = exp10 + " Rate";
            ansGridView1.Columns["rate11"].HeaderText = exp11 + " Rate";

            ansGridView1.Columns["type1"].HeaderText = exp1 + " Type";
            ansGridView1.Columns["type2"].HeaderText = exp2 + " Type";
            ansGridView1.Columns["type3"].HeaderText = exp3 + " Type";
            ansGridView1.Columns["type4"].HeaderText = exp4 + " Type";
            ansGridView1.Columns["type5"].HeaderText = exp5 + " Type";
            ansGridView1.Columns["type6"].HeaderText = exp6 + " Type";
            ansGridView1.Columns["type7"].HeaderText = exp7 + " Type";
            ansGridView1.Columns["type8"].HeaderText = exp8 + " Type";
            ansGridView1.Columns["type9"].HeaderText = exp9 + " Type";
            ansGridView1.Columns["type10"].HeaderText = exp10 + " Type";
            ansGridView1.Columns["type11"].HeaderText = exp11 + " Type";

            ansGridView1.Columns["mini1"].HeaderText = exp1 + " Minimum";
            ansGridView1.Columns["mini2"].HeaderText = exp2 + " Minimum";
            ansGridView1.Columns["mini3"].HeaderText = exp3 + " Minimum";
            ansGridView1.Columns["mini4"].HeaderText = exp4 + " Minimum";
            ansGridView1.Columns["mini5"].HeaderText = exp5 + " Minimum";
            ansGridView1.Columns["mini6"].HeaderText = exp6 + " Minimum";
            ansGridView1.Columns["mini7"].HeaderText = exp7 + " Minimum";
            ansGridView1.Columns["mini8"].HeaderText = exp8 + " Minimum";
            ansGridView1.Columns["mini9"].HeaderText = exp9 + " Minimum";
            ansGridView1.Columns["mini10"].HeaderText = exp10 + " Minimum";
            ansGridView1.Columns["mini11"].HeaderText = exp11 + " Minimum";
        }

        private void frmCustSuppRate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
            {
                if (validate() == true)
                {
                    //try
                    //{
                    //    Database.BeginTran();
                       save();
                    //    Database.CommitTran();
                    //}
                    //catch (Exception ex)
                    //{
                    //    MessageBox.Show("Not Saved", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //    Database.RollbackTran();
                    //}
                }
            }
            else if (e.KeyCode == Keys.Escape)
            {
                this.Close();
                this.Dispose();
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            strCombo = "select distinct name from items order by name";
            textBox2.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
            
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C)
            {
                textBox1.Text = funs.AddAccount();
            }
            else if (e.Control && e.KeyCode == Keys.A)
            {
                textBox1.Text = funs.EditAccount(textBox1.Text);
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            string wheresrt = "(Path LIKE '1;39;%')  ";
            strCombo = funs.GetStrCombo(wheresrt);
            textBox1.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 1);
           // strCombo = "SELECT [name] from accounts where act_id=39";
           // textBox1.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
        }

        private void ansGridView1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsLetter(e.KeyChar) || char.IsNumber(e.KeyChar) || e.KeyChar == ' ')
            {
                if (ansGridView1.CurrentCell.OwningColumn.Name == "destination")
                {
                    strCombo = "SELECT distinct name from DeliveryPoints order by name";
                    ansGridView1.CurrentCell.Value = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
                    ansGridView1.CurrentCell = ansGridView1["rate0", ansGridView1.CurrentCell.RowIndex];

                    if (ansGridView1.CurrentRow.Cells["destination"].Value != null && ansGridView1.CurrentRow.Cells["station"].Value != null)
                    {
                        if (textBox2.Text != "")
                        {
                            DataTable dtdes = new DataTable();
                            Database.GetSqlData("select * from itemdetails where Item_id='" + funs.Select_item_name_pack_id(textBox2.Text) + "' and source_id='" + funs.Select_dp_id(ansGridView1.CurrentRow.Cells["station"].Value.ToString()) + "' and destination_id='" + funs.Select_dp_id(ansGridView1.CurrentRow.Cells["destination"].Value.ToString()) + "' ", dtdes);
                            if (dtdes.Rows.Count == 1)
                            {
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["rate0"].Value = funs.IndianCurr(double.Parse(dtdes.Rows[0]["FreightRate"].ToString()));
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["rate1"].Value = funs.IndianCurr(double.Parse(dtdes.Rows[0]["Expense1"].ToString()));
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["rate2"].Value = funs.IndianCurr(double.Parse(dtdes.Rows[0]["Expense2"].ToString()));
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["rate3"].Value = funs.IndianCurr(double.Parse(dtdes.Rows[0]["Expense3"].ToString()));
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["rate4"].Value = funs.IndianCurr(double.Parse(dtdes.Rows[0]["Expense4"].ToString()));
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["rate5"].Value = funs.IndianCurr(double.Parse(dtdes.Rows[0]["Expense5"].ToString()));
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["rate6"].Value = funs.IndianCurr(double.Parse(dtdes.Rows[0]["Expense6"].ToString()));
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["rate7"].Value = funs.IndianCurr(double.Parse(dtdes.Rows[0]["Expense7"].ToString()));
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["rate8"].Value = funs.IndianCurr(double.Parse(dtdes.Rows[0]["Expense8"].ToString()));
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["rate9"].Value = funs.IndianCurr(double.Parse(dtdes.Rows[0]["Expense9"].ToString()));
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["rate10"].Value = funs.IndianCurr(double.Parse(dtdes.Rows[0]["Expense10"].ToString()));
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["rate11"].Value = funs.IndianCurr(double.Parse(dtdes.Rows[0]["Expense11"].ToString()));

                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["mini0"].Value = funs.IndianCurr(double.Parse(dtdes.Rows[0]["MRFreight"].ToString()));
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["mini1"].Value = funs.IndianCurr(double.Parse(dtdes.Rows[0]["MRExpense1"].ToString()));
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["mini2"].Value = funs.IndianCurr(double.Parse(dtdes.Rows[0]["MRExpense2"].ToString()));
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["mini3"].Value = funs.IndianCurr(double.Parse(dtdes.Rows[0]["MRExpense3"].ToString()));
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["mini4"].Value = funs.IndianCurr(double.Parse(dtdes.Rows[0]["MRExpense4"].ToString()));
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["mini5"].Value = funs.IndianCurr(double.Parse(dtdes.Rows[0]["MRExpense5"].ToString()));
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["mini6"].Value = funs.IndianCurr(double.Parse(dtdes.Rows[0]["MRExpense6"].ToString()));
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["mini7"].Value = funs.IndianCurr(double.Parse(dtdes.Rows[0]["MRExpense7"].ToString()));
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["mini8"].Value = funs.IndianCurr(double.Parse(dtdes.Rows[0]["MRExpense8"].ToString()));
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["mini9"].Value = funs.IndianCurr(double.Parse(dtdes.Rows[0]["MRExpense9"].ToString()));
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["mini10"].Value = funs.IndianCurr(double.Parse(dtdes.Rows[0]["MRExpense10"].ToString()));
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["mini11"].Value = funs.IndianCurr(double.Parse(dtdes.Rows[0]["MRExpense11"].ToString()));

                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["type0"].Value = dtdes.Rows[0]["Freightper"];
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["type1"].Value = dtdes.Rows[0]["ExpenseType1"];
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["type2"].Value = dtdes.Rows[0]["ExpenseType2"];
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["type3"].Value = dtdes.Rows[0]["ExpenseType3"];
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["type4"].Value = dtdes.Rows[0]["ExpenseType4"];
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["type5"].Value = dtdes.Rows[0]["ExpenseType5"];
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["type6"].Value = dtdes.Rows[0]["ExpenseType6"];
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["type7"].Value = dtdes.Rows[0]["ExpenseType7"];
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["type8"].Value = dtdes.Rows[0]["ExpenseType8"];
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["type9"].Value = dtdes.Rows[0]["ExpenseType9"];
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["type10"].Value = dtdes.Rows[0]["ExpenseType10"];
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["type11"].Value = dtdes.Rows[0]["ExpenseType11"];

                                ansGridView1.CurrentCell = ansGridView1["rate0", ansGridView1.CurrentCell.RowIndex];
                            }
                            else
                            {
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["rate0"].Value = 0;
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["rate1"].Value = 0;
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["rate2"].Value = 0;
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["rate3"].Value = 0;
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["rate4"].Value = 0;
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["rate5"].Value = 0;
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["rate6"].Value = 0;
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["rate7"].Value = 0;
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["rate8"].Value = 0;
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["rate9"].Value = 0;
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["rate10"].Value = 0;
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["rate11"].Value = 0;

                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["mini0"].Value = 0;
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["mini1"].Value = 0;
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["mini2"].Value = 0;
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["mini3"].Value = 0;
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["mini4"].Value = 0;
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["mini5"].Value = 0;
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["mini6"].Value = 0;
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["mini7"].Value = 0;
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["mini8"].Value = 0;
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["mini9"].Value = 0;
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["mini10"].Value = 0;
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["mini11"].Value = 0;

                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["type0"].Value = "Flat";
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["type1"].Value = "Flat";
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["type2"].Value = "Flat";
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["type3"].Value = "Flat";
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["type4"].Value = "Flat";
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["type5"].Value = "Flat";
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["type6"].Value = "Flat";
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["type7"].Value = "Flat";
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["type8"].Value = "Flat";
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["type9"].Value = "Flat"; ;
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["type10"].Value = "Flat";
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["type11"].Value = "Flat";
                                ansGridView1.CurrentCell = ansGridView1["rate0", ansGridView1.CurrentCell.RowIndex];
                            }
                        }
                    }
                }
                else if (ansGridView1.CurrentCell.OwningColumn.Name == "station")
                {
                    strCombo = "SELECT distinct name from DeliveryPoints order by name";
                    ansGridView1.CurrentCell.Value = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
                    ansGridView1.CurrentCell = ansGridView1["destination", ansGridView1.CurrentCell.RowIndex];

                    if (ansGridView1.CurrentRow.Cells["destination"].Value != null && ansGridView1.CurrentRow.Cells["station"].Value != null)
                    {
                        if (textBox2.Text != "")
                        {
                            DataTable dtdes = new DataTable();
                            Database.GetSqlData("select * from itemdetails where Item_id='" + funs.Select_item_name_pack_id(textBox2.Text) + "' and source_id='" + funs.Select_dp_id(ansGridView1.CurrentRow.Cells["station"].Value.ToString()) + "' and destination_id='" + funs.Select_dp_id(ansGridView1.CurrentRow.Cells["destination"].Value.ToString()) + "' ", dtdes);
                            if (dtdes.Rows.Count == 1)
                            {
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["rate0"].Value = funs.IndianCurr(double.Parse(dtdes.Rows[0]["FreightRate"].ToString()));
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["rate1"].Value = funs.IndianCurr(double.Parse(dtdes.Rows[0]["Expense1"].ToString()));
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["rate2"].Value = funs.IndianCurr(double.Parse(dtdes.Rows[0]["Expense2"].ToString()));
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["rate3"].Value = funs.IndianCurr(double.Parse(dtdes.Rows[0]["Expense3"].ToString()));
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["rate4"].Value = funs.IndianCurr(double.Parse(dtdes.Rows[0]["Expense4"].ToString()));
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["rate5"].Value = funs.IndianCurr(double.Parse(dtdes.Rows[0]["Expense5"].ToString()));
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["rate6"].Value = funs.IndianCurr(double.Parse(dtdes.Rows[0]["Expense6"].ToString()));
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["rate7"].Value = funs.IndianCurr(double.Parse(dtdes.Rows[0]["Expense7"].ToString()));
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["rate8"].Value = funs.IndianCurr(double.Parse(dtdes.Rows[0]["Expense8"].ToString()));
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["rate9"].Value = funs.IndianCurr(double.Parse(dtdes.Rows[0]["Expense9"].ToString()));
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["rate10"].Value = funs.IndianCurr(double.Parse(dtdes.Rows[0]["Expense10"].ToString()));
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["rate11"].Value = funs.IndianCurr(double.Parse(dtdes.Rows[0]["Expense11"].ToString()));

                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["mini0"].Value = funs.IndianCurr(double.Parse(dtdes.Rows[0]["MRFreight"].ToString()));
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["mini1"].Value = funs.IndianCurr(double.Parse(dtdes.Rows[0]["MRExpense1"].ToString()));
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["mini2"].Value = funs.IndianCurr(double.Parse(dtdes.Rows[0]["MRExpense2"].ToString()));
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["mini3"].Value = funs.IndianCurr(double.Parse(dtdes.Rows[0]["MRExpense3"].ToString()));
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["mini4"].Value = funs.IndianCurr(double.Parse(dtdes.Rows[0]["MRExpense4"].ToString()));
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["mini5"].Value = funs.IndianCurr(double.Parse(dtdes.Rows[0]["MRExpense5"].ToString()));
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["mini6"].Value = funs.IndianCurr(double.Parse(dtdes.Rows[0]["MRExpense6"].ToString()));
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["mini7"].Value = funs.IndianCurr(double.Parse(dtdes.Rows[0]["MRExpense7"].ToString()));
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["mini8"].Value = funs.IndianCurr(double.Parse(dtdes.Rows[0]["MRExpense8"].ToString()));
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["mini9"].Value = funs.IndianCurr(double.Parse(dtdes.Rows[0]["MRExpense9"].ToString()));
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["mini10"].Value = funs.IndianCurr(double.Parse(dtdes.Rows[0]["MRExpense10"].ToString()));
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["mini11"].Value = funs.IndianCurr(double.Parse(dtdes.Rows[0]["MRExpense11"].ToString()));

                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["type0"].Value = dtdes.Rows[0]["Freightper"];
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["type1"].Value = dtdes.Rows[0]["ExpenseType1"];
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["type2"].Value = dtdes.Rows[0]["ExpenseType2"];
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["type3"].Value = dtdes.Rows[0]["ExpenseType3"];
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["type4"].Value = dtdes.Rows[0]["ExpenseType4"];
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["type5"].Value = dtdes.Rows[0]["ExpenseType5"];
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["type6"].Value = dtdes.Rows[0]["ExpenseType6"];
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["type7"].Value = dtdes.Rows[0]["ExpenseType7"];
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["type8"].Value = dtdes.Rows[0]["ExpenseType8"];
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["type9"].Value = dtdes.Rows[0]["ExpenseType9"];
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["type10"].Value = dtdes.Rows[0]["ExpenseType10"];
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["type11"].Value = dtdes.Rows[0]["ExpenseType11"];

                                ansGridView1.CurrentCell = ansGridView1["rate0", ansGridView1.CurrentCell.RowIndex];
                            }
                            else
                            {
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["rate0"].Value = 0;
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["rate1"].Value = 0;
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["rate2"].Value = 0;
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["rate3"].Value = 0;
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["rate4"].Value = 0;
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["rate5"].Value = 0;
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["rate6"].Value = 0;
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["rate7"].Value = 0;
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["rate8"].Value = 0;
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["rate9"].Value = 0;
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["rate10"].Value = 0;
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["rate11"].Value = 0;

                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["mini0"].Value = 0;
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["mini1"].Value = 0;
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["mini2"].Value = 0;
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["mini3"].Value = 0;
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["mini4"].Value = 0;
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["mini5"].Value = 0;
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["mini6"].Value = 0;
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["mini7"].Value = 0;
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["mini8"].Value = 0;
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["mini9"].Value = 0;
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["mini10"].Value = 0;
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["mini11"].Value = 0;

                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["type0"].Value = "Flat";
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["type1"].Value = "Flat";
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["type2"].Value = "Flat";
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["type3"].Value = "Flat";
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["type4"].Value = "Flat";
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["type5"].Value = "Flat";
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["type6"].Value = "Flat";
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["type7"].Value = "Flat";
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["type8"].Value = "Flat";
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["type9"].Value = "Flat"; ;
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["type10"].Value = "Flat";
                                ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["type11"].Value = "Flat";

                                ansGridView1.CurrentCell = ansGridView1["rate0", ansGridView1.CurrentCell.RowIndex];
                            }
                        }
                    }

                }
                else if (ansGridView1.CurrentCell.OwningColumn.Name == "type0")
                {
                    DataTable dtcombo = new DataTable();
                    dtcombo.Columns.Add("RatePer", typeof(string));
                    dtcombo.Columns["RatePer"].ColumnName = "RatePer";
                    dtcombo.Rows.Add();
                    dtcombo.Rows[0][0] = "/Nug";
                    dtcombo.Rows.Add();
                    dtcombo.Rows[1][0] = "/Weight";
                    dtcombo.Rows.Add();
                    dtcombo.Rows[2][0] = "Flat";
                    ansGridView1.CurrentCell.Value = SelectCombo.ComboDt(this, dtcombo, 0);
                }
                else if (ansGridView1.CurrentCell.OwningColumn.Name == "type1")
                {
                    DataTable dtcombo = TypeDt();
                    ansGridView1.CurrentCell.Value = SelectCombo.ComboDt(this, dtcombo, 0);
                }
                else if (ansGridView1.CurrentCell.OwningColumn.Name == "type2")
                {
                    DataTable dtcombo = TypeDt();
                    ansGridView1.CurrentCell.Value = SelectCombo.ComboDt(this, dtcombo, 0);
                }
                else if (ansGridView1.CurrentCell.OwningColumn.Name == "type3")
                {
                    DataTable dtcombo = TypeDt();
                    ansGridView1.CurrentCell.Value = SelectCombo.ComboDt(this, dtcombo, 0);
                }
                else if (ansGridView1.CurrentCell.OwningColumn.Name == "type4")
                {
                    DataTable dtcombo = TypeDt();
                    ansGridView1.CurrentCell.Value = SelectCombo.ComboDt(this, dtcombo, 0);
                }
                else if (ansGridView1.CurrentCell.OwningColumn.Name == "type5")
                {
                    DataTable dtcombo = TypeDt();
                    ansGridView1.CurrentCell.Value = SelectCombo.ComboDt(this, dtcombo, 0);
                }
                else if (ansGridView1.CurrentCell.OwningColumn.Name == "type6")
                {
                    DataTable dtcombo = TypeDt();
                    ansGridView1.CurrentCell.Value = SelectCombo.ComboDt(this, dtcombo, 0);
                }
                else if (ansGridView1.CurrentCell.OwningColumn.Name == "type7")
                {
                    DataTable dtcombo = TypeDt();
                    ansGridView1.CurrentCell.Value = SelectCombo.ComboDt(this, dtcombo, 0);
                }
                else if (ansGridView1.CurrentCell.OwningColumn.Name == "type8")
                {
                    DataTable dtcombo = TypeDt();
                    ansGridView1.CurrentCell.Value = SelectCombo.ComboDt(this, dtcombo, 0);
                }
                else if (ansGridView1.CurrentCell.OwningColumn.Name == "type9")
                {
                    DataTable dtcombo = TypeDt();
                    ansGridView1.CurrentCell.Value = SelectCombo.ComboDt(this, dtcombo, 0);
                }
                else if (ansGridView1.CurrentCell.OwningColumn.Name == "type10")
                {
                    DataTable dtcombo = TypeDt();
                    ansGridView1.CurrentCell.Value = SelectCombo.ComboDt(this, dtcombo, 0);
                }
                else if (ansGridView1.CurrentCell.OwningColumn.Name == "type11")
                {
                    DataTable dtcombo = TypeDt();
                    ansGridView1.CurrentCell.Value = SelectCombo.ComboDt(this, dtcombo, 0);
                }
            }
        }

        private DataTable TypeDt()
        {
            DataTable dtcombo = new DataTable();
            dtcombo.Columns.Add("RatePer", typeof(string));

            dtcombo.Columns["RatePer"].ColumnName = "RatePer";

            dtcombo.Rows.Add();
            dtcombo.Rows[0][0] = "/Nug";

            dtcombo.Rows.Add();
            dtcombo.Rows[1][0] = "/Weight";

            dtcombo.Rows.Add();
            dtcombo.Rows[2][0] = "Flat";

            dtcombo.Rows.Add();
            dtcombo.Rows[3][0] = "% of Freight";

            dtcombo.Rows.Add();
            dtcombo.Rows[4][0] = "% of Expenses";

            return dtcombo;
        }

        private void ansGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (ansGridView1.CurrentCell == null)
            {
                return;
            }
            else if (e.KeyCode == Keys.Delete)
            {
                if (ansGridView1.CurrentRow.Index == ansGridView1.Rows.Count - 1)
                {
                    for (int i = 1; i < ansGridView1.Columns.Count; i++)
                    {
                        ansGridView1.Rows[ansGridView1.CurrentRow.Index].Cells[i].Value = null;
                    }
                }
                else
                {
                    int rindex = ansGridView1.CurrentRow.Index;
                    ansGridView1.Rows.RemoveAt(rindex);
                    for (int i = 0; i < ansGridView1.Rows.Count; i++)
                    {
                        ansGridView1.Rows[i].Cells["SNo"].Value = (i + 1);
                    }
                    return;
                }
            }
        }

        private void ansGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            ansGridView1.Rows[e.RowIndex].Cells["SNo"].Value = e.RowIndex + 1;
            if (ansGridView1.CurrentCell.OwningColumn.Name == "SNo")
            {
                SendKeys.Send("{right}");
            }
        }

        private void ansGridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (ansGridView1.Columns[e.ColumnIndex].Name == "Rate")
            {
                double dbl;
                if (double.TryParse(e.FormattedValue.ToString(), out dbl) == false)
                {
                    e.Cancel = true;
                }
                else if (double.TryParse(e.FormattedValue.ToString(), out dbl) == true)
                {
                    if (dbl < 0)
                    {
                        e.Cancel = true;
                    }
                }
            }
        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && !(e.KeyChar.ToString() == ".") && !(e.KeyChar.ToString() == "-");
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && !(e.KeyChar.ToString() == ".") && !(e.KeyChar.ToString() == "-");
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && !(e.KeyChar.ToString() == ".") && !(e.KeyChar.ToString() == "-");
        }
    }
}
