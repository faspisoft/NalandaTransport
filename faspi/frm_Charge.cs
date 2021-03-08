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
    public partial class frm_Charge : Form
    {
        DataTable dtCharges;
        String dtName;


        String strCombo;
        public bool calledIndirect = false;
        public string chrgname;

        String gStr;
        public frm_Charge()
        {
            InitializeComponent();
        }




        public void LoadData(String str, String frmCaption)
        {
            gStr = str;
            dtName = "charges";
            dtCharges = new DataTable(dtName);
            Database.GetSqlData("select * from charges where ch_id='"+str+"' ", dtCharges);

            //Database.FillCombo(comboBox1, "select [name] from account where act_id=6 or act_id=7");

            this.Text = frmCaption;
            if (dtCharges.Rows.Count == 0)
            {
                dtCharges.Rows.Add(0);
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                radioButton6.Checked = true;
                checkBox1.Checked = false;
            }
            else
            {
                textBox1.Text = dtCharges.Rows[0]["name"].ToString();
                textBox3.Text = dtCharges.Rows[0]["Charge_type"].ToString();
                if (dtCharges.Rows[0]["ac_id"].ToString() == "")
                {
                   // radioButton2.Checked = true;
                }
                else
                {
                   // radioButton1.Checked = true;
                    textBox2.Text = funs.Select_ac_nm(dtCharges.Rows[0]["ac_id"].ToString());
                }
                if (bool.Parse(dtCharges.Rows[0]["AutoLoad"].ToString()) == true)
                {
                   checkBox1.Checked = true;
                }
                else
                {
                    checkBox1.Checked = false;
                }
                if (int.Parse(dtCharges.Rows[0]["add_sub"].ToString()) == 4)
                {
                    radioButton6.Checked = true;
                }
                else
                {
                    radioButton7.Checked = true;
                }
            }
        }

        private void save()
        {
            chrgname = textBox1.Text;

            if (gStr == "0")
            {
                DataTable dtCount = new DataTable();
                Database.GetSqlData("select count(*) from Charges where locationid='" + Database.LocationId + "'", dtCount);
                if (int.Parse(dtCount.Rows[0][0].ToString()) == 0)
                {
                    dtCharges.Rows[0]["ch_id"] = Database.LocationId + "1";
                    dtCharges.Rows[0]["Nid"] = 1;
                    dtCharges.Rows[0]["LocationId"] = Database.LocationId;
                }
                else
                {
                    DataTable dtAcid = new DataTable();
                    Database.GetSqlData("select max(Nid) as Nid from charges where locationid='" + Database.LocationId + "'", dtAcid);
                    int Nid = int.Parse(dtAcid.Rows[0][0].ToString());
                    dtCharges.Rows[0]["ch_id"] = Database.LocationId + (Nid + 1);
                    dtCharges.Rows[0]["Nid"] = (Nid + 1);
                    dtCharges.Rows[0]["LocationId"] = Database.LocationId;
                }
            }



            dtCharges.Rows[0]["name"] = textBox1.Text;
            if (textBox2.Text != "")
            {
                dtCharges.Rows[0]["ac_id"] = funs.Select_ac_id(textBox2.Text);
            }
            else
            {
                dtCharges.Rows[0]["ac_id"] = 0;
            }

            dtCharges.Rows[0]["charge_type"] = textBox3.Text;
            if (checkBox1.Checked == true)
            {
                dtCharges.Rows[0]["AutoLoad"] = true;
            }
            else
            {
                dtCharges.Rows[0]["AutoLoad"] = false;
            }

            if (radioButton6.Checked == true)
            {
                dtCharges.Rows[0]["add_sub"] = 4;
            }
            else if (radioButton7.Checked == true)
            {
                dtCharges.Rows[0]["add_sub"] = 5;
            }
           // dtCharges.Rows[0]["LocationId"] = Database.LocationId;
            dtCharges.Rows[0]["userid"] = Database.user_id;
            if (gStr == "0")
            {
                dtCharges.Rows[0]["create_date"] = System.DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
            }
            dtCharges.Rows[0]["modify_date"] = System.DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");


            Database.SaveData(dtCharges);
           
            funs.ShowBalloonTip("Saved", "Saved Successfully");

            if (calledIndirect == true)
            {
                this.Close();
                this.Dispose();

            }
            // MessageBox.Show("Saved successfully");
            if (gStr == "0")
            {
                LoadData("0", this.Text);
            }
            else
            {
                this.Close();
                this.Dispose();
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
            if (name == "quit")
            {
                this.Close();
                this.Dispose();
            }
        }


        private bool validate()
        {
            if (textBox1.Text == "")
            {
                textBox1.BackColor = Color.Aqua;
                textBox1.Focus();
                return false;
            }
            if (textBox3.Text == "")
            {
                textBox3.BackColor = Color.Aqua;
                textBox3.Focus();
                return false;
            }

            if (funs.Select_ch_id(textBox1.Text) != "" && funs.Select_ch_id(textBox1.Text) != gStr)
            {
                MessageBox.Show("Charges Name Already Exists");
                return false;
            }

            //if (radioButton1.Checked == true)
            //{
                if (textBox2.Text == "")
                {
                    MessageBox.Show("Please Select A/c Name.");
                    textBox2.Focus();
                    return false;
                }
           // }

            return true;
        }

        private void frm_Charge_Load(object sender, EventArgs e)
        {
            SideFill();
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
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

            textBox3.Text = SelectCombo.ComboDt(this, dtcombo, 0);
            SendKeys.Send("{tab}");
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            strCombo = "select [name] from accounts where act_id=6 or act_id=7 or act_id=3 or act_id=37 or act_id=12 or act_id=30";
            textBox2.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
        }

        private void frm_Charge_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
            {
                if (validate() == true)
                {
                    if (Database.utype == "SuperAdmin")
                    {
                        save();


                        if (gStr == "0")
                        {
                            LoadData("0", this.Text);
                        }
                        else
                        {
                            this.Close();
                            this.Dispose();
                        }
                        if (calledIndirect == true)
                        {
                            this.Close();
                            this.Dispose();
                        }
                    }

                    else if (gStr == "0")
                    {
                        save();

                        if (gStr == "0")
                        {
                            LoadData("0", this.Text);
                        }
                        else
                        {
                            this.Close();
                            this.Dispose();
                        }
                        if (calledIndirect == true)
                        {
                            this.Close();
                            this.Dispose();
                        }
                    }

                }
            }
            
            if (e.KeyCode == Keys.Escape)
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
                        this.Dispose();
                    }
                }
                else
                {
                    this.Dispose();
                }
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox1);
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

        private void textBox3_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox3);
        }

        private void textBox3_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox3);
        }

    }
}
