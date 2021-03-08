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
    public partial class frm_state : Form
    {
        DataTable dtstate;
        string gstr = "";
        string strCombo;
        public string statename;
        public bool calledIndirect = false;

        public frm_state()
        {
            InitializeComponent();
        }

        private void frm_state_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
            {
                if (validate() == true)
                {
                    if (Database.utype == "Admin")
                    {
                        save();
                    }
                    else if (gstr == "0")
                    {
                        save();
                    }
                }
            }
            else if (e.KeyCode == Keys.Escape)
            {
                this.Close();
                this.Dispose();
            }
        }

        public void LoadData(string str, string FrmCaption)
        {
            gstr = str;
            dtstate = new DataTable("States");
            Database.GetSqlData("Select * From States Where State_id='" + str + "'", dtstate);
            this.Text = FrmCaption;
            gstr = str;
            if (str == "0")
            {
                TextBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
            }
            else
            {
                TextBox1.Text = dtstate.Rows[0]["Sname"].ToString();
                textBox2.Text = dtstate.Rows[0]["SPrintName"].ToString();
                textBox3.Text = dtstate.Rows[0]["GSTCode"].ToString();
            }
        }

        private void save()
        {
            statename = TextBox1.Text;
            if (dtstate.Rows.Count == 0)
            {
                dtstate.Rows.Add();
            }
            if (gstr == "0")
            {
                DataTable dtCount = new DataTable();
                Database.GetSqlData("select count(*) from States where locationid='" + Database.LocationId + "'", dtCount);

                if (int.Parse(dtCount.Rows[0][0].ToString()) == 0)
                {
                    dtstate.Rows[0]["State_id"] = Database.LocationId + "1";
                    dtstate.Rows[0]["Nid"] = 1;
                    dtstate.Rows[0]["LocationId"] = Database.LocationId;
                }
                else
                {
                    DataTable dtid = new DataTable();
                    Database.GetSqlData("select max(Nid) as Nid from States where locationid='" + Database.LocationId + "'", dtid);
                    int Nid = int.Parse(dtid.Rows[0][0].ToString());
                    dtstate.Rows[0]["State_id"] = Database.LocationId + (Nid + 1);
                    dtstate.Rows[0]["Nid"] = (Nid + 1);
                    dtstate.Rows[0]["LocationId"] = Database.LocationId;
                }
            }
            dtstate.Rows[0]["Sname"] = TextBox1.Text;
            dtstate.Rows[0]["SPrintName"] = textBox2.Text;
            dtstate.Rows[0]["GSTCode"] = textBox3.Text;

            if (gstr == "0")
            {
                dtstate.Rows[0]["create_date"] = System.DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
            }
            dtstate.Rows[0]["modify_date"] = System.DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");

            Database.SaveData(dtstate);
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

        private bool validate()
        {
            if (TextBox1.Text == "")
            {
                MessageBox.Show("Please Enter State Name");
                TextBox1.Focus();
                return false;
            }
            else if (textBox2.Text == "")
            {
                MessageBox.Show("Please Enter Print Name");
                textBox2.Focus();
                return false;
            }
            if (funs.Select_state_id(TextBox1.Text) != "" && funs.Select_state_id(TextBox1.Text) != gstr)
            {
                MessageBox.Show("StateName Already Exists.");
                return false;
            }

            return true;
        }

        private void TextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.Isbackspace(this, e.KeyCode);
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void TextBox1_Leave(object sender, EventArgs e)
        {
            if (textBox2.Text == "")
            {
                textBox2.Text = TextBox1.Text;
            }

            Database.lostFocus(TextBox1);
        }

        private void TextBox1_Enter(object sender, EventArgs e)
        {
            Database.setFocus(TextBox1);
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {

            SelectCombo.Isbackspace(this, e.KeyCode);
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox2);
            if (textBox2.Text == "")
            {
                textBox2.Text = TextBox1.Text;
            }
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox2);
        }

        private void frm_state_Load(object sender, EventArgs e)
        {
            SideFill();
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
            if (gstr != "0")
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

        private void textBox3_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox3);
        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {

            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox3_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox3);
        }
    }
}
