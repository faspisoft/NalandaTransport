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
    public partial class Gaddi : Form
    {
        string gstr = "";
        public bool calledIndirect = false;
        public string gaddi;
        DataTable dtacc;

        public Gaddi()
        {
            InitializeComponent();
        }

        private void Gaddi_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
                this.Dispose();
            }
            else if (e.Control && e.KeyCode == Keys.S)
            {
                if (validate() == true)
                {
                    save();                    
                }
            }
        }

        public void LoadData(string str, string FrmCaption)
        {
            this.Text = FrmCaption;
            gstr = str;
            textBox1.Focus();

            dtacc = new DataTable("Gaddis");
            Database.GetSqlData("Select * from Gaddis where Gaddi_id='" + str+"' ", dtacc);

            if (dtacc.Rows.Count == 0)
            {
                dtacc.Rows.Add();
                textBox1.Text = "";
                textBox2.Text = "";
                DateTime defaultdate = new DateTime(2000, 04, 01);
                dateTimePicker1.Value = defaultdate;
                dateTimePicker2.Value = defaultdate;
                dateTimePicker3.Value = defaultdate;
                dateTimePicker4.Value = defaultdate;
                dateTimePicker5.Value = defaultdate;
            }

            else
            {
                textBox2.Text = funs.Select_ac_nm(dtacc.Rows[0]["Driver_id"].ToString());
                textBox1.Text = dtacc.Rows[0]["Gaddi_name"].ToString();
                dateTimePicker1.Text = dtacc.Rows[0]["Induedate"].ToString();
                dateTimePicker2.Text = dtacc.Rows[0]["Perduedate"].ToString();
                dateTimePicker3.Text = dtacc.Rows[0]["fitduedate"].ToString();
                dateTimePicker4.Text = dtacc.Rows[0]["fiveduedate"].ToString();
                dateTimePicker5.Text = dtacc.Rows[0]["pollduedate"].ToString();
            }
        }

        private void save()
        {
            if (gstr == "0")
            {
                DataTable dtCount = new DataTable();
                Database.GetSqlData("select count(*) from Gaddis where locationid='" + Database.LocationId + "'", dtCount);

                if (int.Parse(dtCount.Rows[0][0].ToString()) == 0)
                {
                    dtacc.Rows[0]["Gaddi_id"] = Database.LocationId + "1";
                    dtacc.Rows[0]["Nid"] = 1;
                    dtacc.Rows[0]["LocationId"] = Database.LocationId;
                }
                else
                {
                    DataTable dtAcid = new DataTable();
                    Database.GetSqlData("select max(Nid) as Nid from Gaddis where locationid='" + Database.LocationId + "'", dtAcid);

                    int Nid = int.Parse(dtAcid.Rows[0][0].ToString());
                    dtacc.Rows[0]["Gaddi_id"] = Database.LocationId + (Nid + 1);
                    dtacc.Rows[0]["Nid"] = (Nid + 1);
                    dtacc.Rows[0]["LocationId"] = Database.LocationId;
                }
            }

            dtacc.Rows[0]["Driver_id"] = funs.Select_ac_id(textBox2.Text);
            dtacc.Rows[0]["Gaddi_name"] = textBox1.Text;
            dtacc.Rows[0]["Induedate"] = dateTimePicker1.Value.Date.ToString("dd-MMM-yyyy");
            dtacc.Rows[0]["Perduedate"] = dateTimePicker2.Value.Date.ToString("dd-MMM-yyyy");
            dtacc.Rows[0]["fitduedate"] = dateTimePicker3.Value.Date.ToString("dd-MMM-yyyy");
            dtacc.Rows[0]["fiveduedate"] = dateTimePicker4.Value.Date.ToString("dd-MMM-yyyy");
            dtacc.Rows[0]["pollduedate"] = dateTimePicker5.Value.Date.ToString("dd-MMM-yyyy");

            if (gstr == "0")
            {
                dtacc.Rows[0]["create_date"] = System.DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
            }
            dtacc.Rows[0]["modify_date"] = System.DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");

            gaddi = textBox1.Text;
            Database.SaveData(dtacc);
            MessageBox.Show("Saved Successfully");

            if (gstr == "0")
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

        private void SideFill()
        {
            flowLayoutPanel1.Controls.Clear();
            DataTable dtsidefill = new DataTable();
            dtsidefill.Columns.Add("Name", typeof(string));
            dtsidefill.Columns.Add("DisplayName", typeof(string));
            dtsidefill.Columns.Add("ShortcutKey", typeof(string));
            dtsidefill.Columns.Add("Visible", typeof(bool));

            //createnew
            dtsidefill.Rows.Add();
            dtsidefill.Rows[0]["Name"] = "save";
            dtsidefill.Rows[0]["DisplayName"] = "Save";
            dtsidefill.Rows[0]["ShortcutKey"] = "^S";
            dtsidefill.Rows[0]["Visible"] = true;

            dtsidefill.Rows.Add();
            dtsidefill.Rows[1]["Name"] = "print";
            dtsidefill.Rows[1]["DisplayName"] = "Print";
            dtsidefill.Rows[1]["ShortcutKey"] = "^P";
            dtsidefill.Rows[1]["Visible"] = false;

            //close
            dtsidefill.Rows.Add();
            dtsidefill.Rows[2]["Name"] = "close";
            dtsidefill.Rows[2]["DisplayName"] = "Quit";
            dtsidefill.Rows[2]["ShortcutKey"] = "Esc";
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
                }
            }
            else if (name == "close")
            {
                this.Close();
                this.Dispose();
            }
        }

        private bool validate()
        {
            if (textBox1.Text.Trim() == "")
            {
                MessageBox.Show("Please Enter Gaddi Number");
                textBox1.Focus();
                return false;
            }
            if (funs.Select_gaddi_id(textBox1.Text) != "" && funs.Select_gaddi_id(textBox1.Text) != gstr)
            {
                MessageBox.Show("Gaadi No Already Exists.");
                return false;
            }

            return true;
        }

        private void Gaddi_Load(object sender, EventArgs e)
        {
            SideFill();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox1);
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox1);
        }       

        private void dateTimePicker1_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(dateTimePicker1);
        }

        private void dateTimePicker1_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void dateTimePicker2_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void dateTimePicker1_Enter(object sender, EventArgs e)
        {
            Database.setFocus(dateTimePicker1);
        }

        private void dateTimePicker2_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(dateTimePicker2);
        }

        private void dateTimePicker2_Enter(object sender, EventArgs e)
        {
            Database.setFocus(dateTimePicker2);
        }

        private void dateTimePicker3_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(dateTimePicker3);
        }

        private void dateTimePicker3_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void dateTimePicker3_Enter(object sender, EventArgs e)
        {
            Database.setFocus(dateTimePicker3);
        }
        
        private void dateTimePicker4_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(dateTimePicker4);
        }

        private void dateTimePicker4_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void dateTimePicker4_Enter(object sender, EventArgs e)
        {
            Database.setFocus(dateTimePicker4);
        }

        private void dateTimePicker5_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(dateTimePicker5);
        }

        private void dateTimePicker5_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void dateTimePicker5_Enter(object sender, EventArgs e)
        {
            Database.setFocus(dateTimePicker5);
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
          string  strCombo = "SELECT ACCOUNTs.Name FROM ACCOUNTs LEFT JOIN ACCOUNTYPEs ON ACCOUNTs.Act_id = ACCOUNTYPEs.Act_id WHERE ACCOUNTYPEs.Name='DRIVER' ORDER BY ACCOUNTs.Name";
          textBox2.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, "", 0);
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {


            if (e.Control && e.KeyCode == Keys.C)
            {
                textBox2.Text = funs.AddAccount();
            }
            else if (e.Control && e.KeyCode == Keys.A)
            {
                if (textBox2.Text != "")
                {
                    textBox2.Text = funs.EditAccount(textBox2.Text); ;
                }
            }
        }
    }
}