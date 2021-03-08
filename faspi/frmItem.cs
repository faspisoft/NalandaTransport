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
    public partial class frmItem : Form
    {
        DataTable dtItem;
        DataTable dtItemdetail;
        string dtName;
        string itemid;
        public bool calledIndirect = false;
        public string itemName;
        string gStr;
        string strCombo = "";
        DateTime create_date = DateTime.Parse(System.DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"));

        public frmItem()
        {
            InitializeComponent();
        }

        private void frmBroker_Load(object sender, EventArgs e)
        {
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
                    save();                    
                }
            }

            else if (name == "quit")
            {
                this.Close();
                this.Dispose();
            }
        }

        public void LoadData(String str, String frmCaption)
        {
            gStr = str;
            dtName = "items";
            dtItem = new DataTable(dtName);
            dtItemdetail = new DataTable("Itemdetails");
            Database.GetSqlData("select * from " + dtName + " where id='" + str + "'", dtItem);
            Database.GetSqlData("select * from Itemdetails where Item_id='" + str + "'", dtItemdetail);
            this.Text = frmCaption;

            if (dtItem.Rows.Count == 0)
            {
                itemid = "";
                dtItem.Rows.Add(0);
                TextBox1.Text = "";
                textBox2.Text = "0";
                TextBox3.Text = "0";
                textBox4.Text = "0";
            }
            else
            {
                TextBox1.Text = dtItem.Rows[0]["name"].ToString();
                TextBox3.Text = funs.IndianCurr(double.Parse(dtItem.Rows[0]["bharti"].ToString()));
                itemid = dtItem.Rows[0]["id"].ToString();

                create_date = DateTime.Parse(dtItem.Rows[0]["create_date"].ToString());

                textBox2.Text = funs.IndianCurr(Convert.ToDouble(dtItem.Rows[0]["Mini_weight"]));
                textBox4.Text = funs.IndianCurr(Convert.ToDouble(dtItem.Rows[0]["Charged_weight"]));

                if (Convert.ToInt32(dtItem.Rows[0]["Rounding_ex"]) == 0)
                {
                    radioButton1.Checked = true;
                }
                if (Convert.ToInt32(dtItem.Rows[0]["Rounding_ex"]) == 1)
                {
                    radioButton2.Checked = true;
                }
                if (Convert.ToInt32(dtItem.Rows[0]["Rounding_ex"]) == 5)
                {
                    radioButton3.Checked = true;
                }
                if (Convert.ToInt32(dtItem.Rows[0]["Rounding_ex"]) == 10)
                {
                    radioButton4.Checked = true;
                }

                if (Convert.ToInt32(dtItem.Rows[0]["Rounding_ch"]) == 0)
                {
                    radioButton8.Checked = true;
                }
                if (Convert.ToInt32(dtItem.Rows[0]["Rounding_ch"]) == 1)
                {
                    radioButton7.Checked = true;
                }
                if (Convert.ToInt32(dtItem.Rows[0]["Rounding_ch"]) == 5)
                {
                    radioButton6.Checked = true;
                }
                if (Convert.ToInt32(dtItem.Rows[0]["Rounding_ch"]) == 10)
                {
                    radioButton5.Checked = true;
                }
                if (Convert.ToInt32(dtItem.Rows[0]["Rounding_ch"]) == 25)
                {
                    radioButton11.Checked = true;
                }
                if (Convert.ToInt32(dtItem.Rows[0]["Rounding_ch"]) == 50)
                {
                    radioButton10.Checked = true;
                }
                if (Convert.ToInt32(dtItem.Rows[0]["Rounding_ch"]) == 100)
                {
                    radioButton9.Checked = true;
                }

                for (int i = 0; i < dtItemdetail.Rows.Count; i++)
                {
                    ansGridView1.Rows.Add();
                    ansGridView1.Rows[i].Cells["SNo"].Value = i + 1;
                    ansGridView1.Rows[i].Cells["station"].Value = funs.Select_dp_nm(dtItemdetail.Rows[i]["Source_id"].ToString());
                    ansGridView1.Rows[i].Cells["destination"].Value = funs.Select_dp_nm(dtItemdetail.Rows[i]["Destination_id"].ToString());

                    ansGridView1.Rows[i].Cells["rate0"].Value = funs.IndianCurr(double.Parse(dtItemdetail.Rows[i]["FreightRate"].ToString()));
                    ansGridView1.Rows[i].Cells["rate1"].Value = funs.IndianCurr(double.Parse(dtItemdetail.Rows[i]["Expense1"].ToString()));
                    ansGridView1.Rows[i].Cells["rate2"].Value = funs.IndianCurr(double.Parse(dtItemdetail.Rows[i]["Expense2"].ToString()));
                    ansGridView1.Rows[i].Cells["rate3"].Value = funs.IndianCurr(double.Parse(dtItemdetail.Rows[i]["Expense3"].ToString()));
                    ansGridView1.Rows[i].Cells["rate4"].Value = funs.IndianCurr(double.Parse(dtItemdetail.Rows[i]["Expense4"].ToString()));
                    ansGridView1.Rows[i].Cells["rate5"].Value = funs.IndianCurr(double.Parse(dtItemdetail.Rows[i]["Expense5"].ToString()));
                    ansGridView1.Rows[i].Cells["rate6"].Value = funs.IndianCurr(double.Parse(dtItemdetail.Rows[i]["Expense6"].ToString()));
                    ansGridView1.Rows[i].Cells["rate7"].Value = funs.IndianCurr(double.Parse(dtItemdetail.Rows[i]["Expense7"].ToString()));
                    ansGridView1.Rows[i].Cells["rate8"].Value = funs.IndianCurr(double.Parse(dtItemdetail.Rows[i]["Expense8"].ToString()));
                    ansGridView1.Rows[i].Cells["rate9"].Value = funs.IndianCurr(double.Parse(dtItemdetail.Rows[i]["Expense9"].ToString()));
                    ansGridView1.Rows[i].Cells["rate10"].Value = funs.IndianCurr(double.Parse(dtItemdetail.Rows[i]["Expense10"].ToString()));
                    ansGridView1.Rows[i].Cells["rate11"].Value = funs.IndianCurr(double.Parse(dtItemdetail.Rows[i]["Expense11"].ToString()));

                    ansGridView1.Rows[i].Cells["mini0"].Value = funs.IndianCurr(double.Parse(dtItemdetail.Rows[i]["MRFreight"].ToString()));
                    ansGridView1.Rows[i].Cells["mini1"].Value = funs.IndianCurr(double.Parse(dtItemdetail.Rows[i]["MRExpense1"].ToString()));
                    ansGridView1.Rows[i].Cells["mini2"].Value = funs.IndianCurr(double.Parse(dtItemdetail.Rows[i]["MRExpense2"].ToString()));
                    ansGridView1.Rows[i].Cells["mini3"].Value = funs.IndianCurr(double.Parse(dtItemdetail.Rows[i]["MRExpense3"].ToString()));
                    ansGridView1.Rows[i].Cells["mini4"].Value = funs.IndianCurr(double.Parse(dtItemdetail.Rows[i]["MRExpense4"].ToString()));
                    ansGridView1.Rows[i].Cells["mini5"].Value = funs.IndianCurr(double.Parse(dtItemdetail.Rows[i]["MRExpense5"].ToString()));
                    ansGridView1.Rows[i].Cells["mini6"].Value = funs.IndianCurr(double.Parse(dtItemdetail.Rows[i]["MRExpense6"].ToString()));
                    ansGridView1.Rows[i].Cells["mini7"].Value = funs.IndianCurr(double.Parse(dtItemdetail.Rows[i]["MRExpense7"].ToString()));
                    ansGridView1.Rows[i].Cells["mini8"].Value = funs.IndianCurr(double.Parse(dtItemdetail.Rows[i]["MRExpense8"].ToString()));
                    ansGridView1.Rows[i].Cells["mini9"].Value = funs.IndianCurr(double.Parse(dtItemdetail.Rows[i]["MRExpense9"].ToString()));
                    ansGridView1.Rows[i].Cells["mini10"].Value = funs.IndianCurr(double.Parse(dtItemdetail.Rows[i]["MRExpense10"].ToString()));
                    ansGridView1.Rows[i].Cells["mini11"].Value = funs.IndianCurr(double.Parse(dtItemdetail.Rows[i]["MRExpense11"].ToString()));

                    ansGridView1.Rows[i].Cells["type0"].Value = dtItemdetail.Rows[i]["Freightper"];
                    ansGridView1.Rows[i].Cells["type1"].Value = dtItemdetail.Rows[i]["ExpenseType1"];
                    ansGridView1.Rows[i].Cells["type2"].Value = dtItemdetail.Rows[i]["ExpenseType2"];
                    ansGridView1.Rows[i].Cells["type3"].Value = dtItemdetail.Rows[i]["ExpenseType3"];
                    ansGridView1.Rows[i].Cells["type4"].Value = dtItemdetail.Rows[i]["ExpenseType4"];
                    ansGridView1.Rows[i].Cells["type5"].Value = dtItemdetail.Rows[i]["ExpenseType5"];
                    ansGridView1.Rows[i].Cells["type6"].Value = dtItemdetail.Rows[i]["ExpenseType6"];
                    ansGridView1.Rows[i].Cells["type7"].Value = dtItemdetail.Rows[i]["ExpenseType7"];
                    ansGridView1.Rows[i].Cells["type8"].Value = dtItemdetail.Rows[i]["ExpenseType8"];
                    ansGridView1.Rows[i].Cells["type9"].Value = dtItemdetail.Rows[i]["ExpenseType9"];
                    ansGridView1.Rows[i].Cells["type10"].Value = dtItemdetail.Rows[i]["ExpenseType10"];
                    ansGridView1.Rows[i].Cells["type11"].Value = dtItemdetail.Rows[i]["ExpenseType11"];
                }
            }

            foreach (DataGridViewColumn column in ansGridView1.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        private void save()
        {
            itemName = TextBox1.Text;

            if (gStr == "0")
            {
                DataTable dtCount = new DataTable();
                Database.GetSqlData("select count(*) from Items where locationid='" + Database.LocationId + "'", dtCount);
                if (int.Parse(dtCount.Rows[0][0].ToString()) == 0)
                {
                    dtItem.Rows[0]["id"] = Database.LocationId + "1";
                    dtItem.Rows[0]["Nid"] = 1;
                    dtItem.Rows[0]["LocationId"] = Database.LocationId;
                    itemid = dtItem.Rows[0]["id"].ToString();
                }
                else
                {
                    DataTable dtAcid = new DataTable();
                    Database.GetSqlData("select max(Nid) as Nid from Items where locationid='" + Database.LocationId + "'", dtAcid);
                    int Nid = int.Parse(dtAcid.Rows[0][0].ToString());
                    dtItem.Rows[0]["id"] = Database.LocationId + (Nid + 1);
                    dtItem.Rows[0]["Nid"] = (Nid + 1);
                    dtItem.Rows[0]["LocationId"] = Database.LocationId;
                    itemid = dtItem.Rows[0]["id"].ToString();
                }   
            }

            dtItem.Rows[0]["name"] = TextBox1.Text;
            dtItem.Rows[0]["bharti"] = TextBox3.Text;
            dtItem.Rows[0]["Mini_weight"] = textBox2.Text;
            dtItem.Rows[0]["Charged_weight"] = textBox4.Text;

            if (radioButton1.Checked == true)
            {
                dtItem.Rows[0]["Rounding_ex"] = 0;
            }
            if (radioButton2.Checked == true)
            {
                dtItem.Rows[0]["Rounding_ex"] = 1;
            }
            if (radioButton3.Checked == true)
            {
                dtItem.Rows[0]["Rounding_ex"] = 5;
            }
            if (radioButton4.Checked == true)
            {
                dtItem.Rows[0]["Rounding_ex"] = 10;
            }

            if (radioButton8.Checked == true)
            {
                dtItem.Rows[0]["Rounding_ch"] = 0;
            }
            if (radioButton7.Checked == true)
            {
                dtItem.Rows[0]["Rounding_ch"] = 1;
            }
            if (radioButton6.Checked == true)
            {
                dtItem.Rows[0]["Rounding_ch"] = 5;
            }
            if (radioButton5.Checked == true)
            {
                dtItem.Rows[0]["Rounding_ch"] = 10;
            }
            if (radioButton11.Checked == true)
            {
                dtItem.Rows[0]["Rounding_ch"] = 25;
            }
            if (radioButton10.Checked == true)
            {
                dtItem.Rows[0]["Rounding_ch"] = 50;
            }
            if (radioButton9.Checked == true)
            {
                dtItem.Rows[0]["Rounding_ch"] = 100;
            }

            dtItem.Rows[0]["create_date"] = create_date;
            
            dtItem.Rows[0]["modify_date"] = System.DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");

            Database.SaveData(dtItem);

            DataTable dtTemp = new DataTable("Itemdetails");
            Database.GetSqlData("select * from Itemdetails where Item_id='" + itemid + "'", dtTemp);
            for (int i = 0; i < dtTemp.Rows.Count; i++)
            {
                dtTemp.Rows[i].Delete();
            }
            Database.SaveData(dtTemp);

            dtItemdetail = new DataTable("Itemdetails");
            Database.GetSqlData("select * from Itemdetails where Item_id='" + itemid + "'", dtItemdetail);

            int Nid2 = 1;
            DataTable dtidvd = new DataTable();
            Database.GetSqlData("select max(Nid) as Nid from Itemdetails where locationid='" + Database.LocationId + "'", dtidvd);
            if (dtidvd.Rows[0][0].ToString() != "")
            {
                Nid2 = int.Parse(dtidvd.Rows[0][0].ToString()) + 1;
            }

            for (int i = 0; i < ansGridView1.Rows.Count - 1; i++)
            {
                dtItemdetail.Rows.Add();

                dtItemdetail.Rows[i]["Nid"] = Nid2;
                dtItemdetail.Rows[i]["LocationId"] = Database.LocationId;
                dtItemdetail.Rows[i]["IdetID"] = Database.LocationId + dtItemdetail.Rows[i]["nid"].ToString();

                dtItemdetail.Rows[i]["create_date"] = create_date;
                dtItemdetail.Rows[i]["modify_date"] = System.DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");

                dtItemdetail.Rows[i]["Item_id"] = itemid;           
                dtItemdetail.Rows[i]["Source_id"] = funs.Select_dp_id(ansGridView1.Rows[i].Cells["station"].Value.ToString());
                dtItemdetail.Rows[i]["Destination_id"] = funs.Select_dp_id(ansGridView1.Rows[i].Cells["destination"].Value.ToString());

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
                dtItemdetail.Rows[i]["FreightRate"] = ansGridView1.Rows[i].Cells["rate0"].Value;
                dtItemdetail.Rows[i]["Expense1"] = ansGridView1.Rows[i].Cells["rate1"].Value;
                dtItemdetail.Rows[i]["Expense2"] = ansGridView1.Rows[i].Cells["rate2"].Value;
                dtItemdetail.Rows[i]["Expense3"] = ansGridView1.Rows[i].Cells["rate3"].Value;
                dtItemdetail.Rows[i]["Expense4"] = ansGridView1.Rows[i].Cells["rate4"].Value;
                dtItemdetail.Rows[i]["Expense5"] = ansGridView1.Rows[i].Cells["rate5"].Value;
                dtItemdetail.Rows[i]["Expense6"] = ansGridView1.Rows[i].Cells["rate6"].Value;
                dtItemdetail.Rows[i]["Expense7"] = ansGridView1.Rows[i].Cells["rate7"].Value;
                dtItemdetail.Rows[i]["Expense8"] = ansGridView1.Rows[i].Cells["rate8"].Value;
                dtItemdetail.Rows[i]["Expense9"] = ansGridView1.Rows[i].Cells["rate9"].Value;
                dtItemdetail.Rows[i]["Expense10"] = ansGridView1.Rows[i].Cells["rate10"].Value;
                dtItemdetail.Rows[i]["Expense11"] = ansGridView1.Rows[i].Cells["rate11"].Value;

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
                dtItemdetail.Rows[i]["MRFreight"] = ansGridView1.Rows[i].Cells["mini0"].Value;
                dtItemdetail.Rows[i]["MRExpense1"] = ansGridView1.Rows[i].Cells["mini1"].Value;
                dtItemdetail.Rows[i]["MRExpense2"] = ansGridView1.Rows[i].Cells["mini2"].Value;
                dtItemdetail.Rows[i]["MRExpense3"] = ansGridView1.Rows[i].Cells["mini3"].Value;
                dtItemdetail.Rows[i]["MRExpense4"] = ansGridView1.Rows[i].Cells["mini4"].Value;
                dtItemdetail.Rows[i]["MRExpense5"] = ansGridView1.Rows[i].Cells["mini5"].Value;
                dtItemdetail.Rows[i]["MRExpense6"] = ansGridView1.Rows[i].Cells["mini6"].Value;
                dtItemdetail.Rows[i]["MRExpense7"] = ansGridView1.Rows[i].Cells["mini7"].Value;
                dtItemdetail.Rows[i]["MRExpense8"] = ansGridView1.Rows[i].Cells["mini8"].Value;
                dtItemdetail.Rows[i]["MRExpense9"] = ansGridView1.Rows[i].Cells["mini9"].Value;
                dtItemdetail.Rows[i]["MRExpense10"] = ansGridView1.Rows[i].Cells["mini10"].Value;
                dtItemdetail.Rows[i]["MRExpense11"] = ansGridView1.Rows[i].Cells["mini11"].Value;

                dtItemdetail.Rows[i]["Freightper"] = ansGridView1.Rows[i].Cells["type0"].Value.ToString();
                dtItemdetail.Rows[i]["ExpenseType1"] = ansGridView1.Rows[i].Cells["type1"].Value.ToString();
                dtItemdetail.Rows[i]["ExpenseType2"] = ansGridView1.Rows[i].Cells["type2"].Value.ToString();
                dtItemdetail.Rows[i]["ExpenseType3"] = ansGridView1.Rows[i].Cells["type3"].Value.ToString();
                dtItemdetail.Rows[i]["ExpenseType4"] = ansGridView1.Rows[i].Cells["type4"].Value.ToString();
                dtItemdetail.Rows[i]["ExpenseType5"] = ansGridView1.Rows[i].Cells["type5"].Value.ToString();
                dtItemdetail.Rows[i]["ExpenseType6"] = ansGridView1.Rows[i].Cells["type6"].Value.ToString();
                dtItemdetail.Rows[i]["ExpenseType7"] = ansGridView1.Rows[i].Cells["type7"].Value.ToString();
                dtItemdetail.Rows[i]["ExpenseType8"] = ansGridView1.Rows[i].Cells["type8"].Value.ToString();
                dtItemdetail.Rows[i]["ExpenseType9"] = ansGridView1.Rows[i].Cells["type9"].Value.ToString();
                dtItemdetail.Rows[i]["ExpenseType10"] = ansGridView1.Rows[i].Cells["type10"].Value.ToString();
                dtItemdetail.Rows[i]["ExpenseType11"] = ansGridView1.Rows[i].Cells["type11"].Value.ToString();

                Nid2++;
            }

            Database.SaveData(dtItemdetail);

            MessageBox.Show("Saved Successfully");
            
            if (calledIndirect==true)
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

        private bool validate()
        {
            if (TextBox1.Text == "")
            {
                TextBox1.Focus();
                return false;
            }           
            if (TextBox3.Text.Trim() == "")
            {
                TextBox3.Text = "0";
            }
            if (textBox2.Text.Trim() == "")
            {
                textBox2.Text = "0";
            }
            if (textBox4.Text.Trim() == "")
            {
                textBox4.Text = "0";
            }

            for (int i = 0; i < ansGridView1.Rows.Count-1; i++)
            {
                if (ansGridView1.Rows[i].Cells["type0"].Value == "" || ansGridView1.Rows[i].Cells["type0"].Value == null)
                {
                    ansGridView1.Rows[i].Cells["type0"].Value = "Flat";
                }
                if (ansGridView1.Rows[i].Cells["type1"].Value == "" || ansGridView1.Rows[i].Cells["type1"].Value == null)
                {
                    ansGridView1.Rows[i].Cells["type1"].Value = "Flat";
                }
                if (ansGridView1.Rows[i].Cells["type2"].Value == "" || ansGridView1.Rows[i].Cells["type2"].Value == null)
                {
                    ansGridView1.Rows[i].Cells["type2"].Value = "Flat";
                }
                if (ansGridView1.Rows[i].Cells["type3"].Value == "" || ansGridView1.Rows[i].Cells["type3"].Value == null)
                {
                    ansGridView1.Rows[i].Cells["type3"].Value = "Flat";
                }
                if (ansGridView1.Rows[i].Cells["type4"].Value == "" || ansGridView1.Rows[i].Cells["type4"].Value == null)
                {
                    ansGridView1.Rows[i].Cells["type4"].Value = "Flat";
                }
                if (ansGridView1.Rows[i].Cells["type5"].Value == "" || ansGridView1.Rows[i].Cells["type5"].Value == null)
                {
                    ansGridView1.Rows[i].Cells["type5"].Value = "Flat";
                }
                if (ansGridView1.Rows[i].Cells["type6"].Value== "" || ansGridView1.Rows[i].Cells["type6"].Value == null)
                {
                    ansGridView1.Rows[i].Cells["type6"].Value = "Flat";
                }
                if (ansGridView1.Rows[i].Cells["type7"].Value== "" || ansGridView1.Rows[i].Cells["type7"].Value == null)
                {
                    ansGridView1.Rows[i].Cells["type7"].Value = "Flat";
                }
                if (ansGridView1.Rows[i].Cells["type8"].Value == "" || ansGridView1.Rows[i].Cells["type8"].Value == null)
                {
                    ansGridView1.Rows[i].Cells["type8"].Value = "Flat";
                }
                if (ansGridView1.Rows[i].Cells["type9"].Value == "" || ansGridView1.Rows[i].Cells["type9"].Value == null)
                {
                    ansGridView1.Rows[i].Cells["type9"].Value = "Flat";
                }
                if (ansGridView1.Rows[i].Cells["type10"].Value == "" || ansGridView1.Rows[i].Cells["type10"].Value == null)
                {
                    ansGridView1.Rows[i].Cells["type10"].Value = "Flat";
                }
                if (ansGridView1.Rows[i].Cells["type11"].Value == "" || ansGridView1.Rows[i].Cells["type11"].Value == null)
                {
                    ansGridView1.Rows[i].Cells["type11"].Value = "Flat";
                }
            }
           
            if (funs.Select_item_id(TextBox1.Text) != "" && funs.Select_item_id(TextBox1.Text) != gStr)
            {
                MessageBox.Show("ItemName Already Exists.");
                return false;
            }
            return true;
        }

        private void frmBroker_KeyDown(object sender, KeyEventArgs e)
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

            if (e.KeyCode == Keys.Escape)
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

        private void TextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void TextBox2_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void TextBox3_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void TextBox4_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }
       
        private void TextBox1_Enter(object sender, EventArgs e)
        {
            Database.setFocus(TextBox1);
        }     

        private void TextBox3_Enter(object sender, EventArgs e)
        {
            Database.setFocus(TextBox3);
        }
     
        private void TextBox3_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(TextBox3);
        }       

        private void TextBox1_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(TextBox1);
        }

        private void textBox5_KeyDown_1(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
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

        private void TextBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && !(e.KeyChar.ToString() == ".") && !(e.KeyChar.ToString() == "-");
        }

        private void ansGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            ansGridView1.Rows[e.RowIndex].Cells["SNo"].Value = e.RowIndex + 1;
            if (ansGridView1.CurrentCell.OwningColumn.Name == "SNo")
            {
                SendKeys.Send("{right}");
            }
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

        private void ansGridView1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsLetter(e.KeyChar) || char.IsNumber(e.KeyChar) || e.KeyChar == ' ')
            {
                if (ansGridView1.CurrentCell.OwningColumn.Name == "destination")
                {
                    strCombo = "SELECT distinct name from DeliveryPoints order by name";
                    ansGridView1.CurrentCell.Value = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
                    ansGridView1.CurrentCell = ansGridView1["rate0", ansGridView1.CurrentCell.RowIndex];
                }
                else if (ansGridView1.CurrentCell.OwningColumn.Name == "station")
                {
                    strCombo = "SELECT distinct name from DeliveryPoints order by name";
                    ansGridView1.CurrentCell.Value = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
                    ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["rate0"].Value = 0;
                    ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["rate1"].Value = 0;
                    ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["rate2"].Value = 0;
                    ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["rate3"].Value = 0;
                    ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["rate4"].Value = 0;
                    ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["rate5"].Value =0;
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
                    ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["mini6"].Value =0;
                    ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["mini7"].Value =0;
                    ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["mini8"].Value = 0;
                    ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["mini9"].Value = 0;
                    ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["mini10"].Value = 0;
                    ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["mini11"].Value = 0;
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
                else if (ansGridView1.CurrentCell.OwningColumn.Name == "Type11")
                {
                    DataTable dtcombo = TypeDt();
                    ansGridView1.CurrentCell.Value = SelectCombo.ComboDt(this, dtcombo, 0);
                }
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && !(e.KeyChar.ToString() == ".") && !(e.KeyChar.ToString() == "-");
        }

        private void ansGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            ansGridView1.CurrentCell.Value = 0;
        }

        private void ansGridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
        }
    }
}
