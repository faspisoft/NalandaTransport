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
    public partial class frm_user : Form
    {
        DataTable dtuser;
        int gStr = 0;

        public frm_user()
        {
            InitializeComponent();
        }

        private void frm_user_KeyDown(object sender, KeyEventArgs e)
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

        private void frm_user_Load(object sender, EventArgs e)
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

        public void LoadData(String str, String frmCaption)
        {
            gStr = int.Parse(str);
            this.Text = frmCaption;
            dtuser = new DataTable("users");
            Database.GetSqlData("select * from Users where u_id=" + str, dtuser);
            if (dtuser.Rows.Count == 0)
            {
                dtuser.Rows.Add();
                TextBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";
            }
            else
            {
                TextBox1.Text = dtuser.Rows[0]["username"].ToString();
                textBox2.Text = dtuser.Rows[0]["password"].ToString();
                textBox3.Text = dtuser.Rows[0]["usertype"].ToString();
                textBox4.Text = funs.Select_location_name(dtuser.Rows[0]["location_id"].ToString());
            }
        }
        private void save()
        {
            if (gStr == 0)
            {
                int id = Database.GetScalarInt("select max(u_id) from users") + 1;
                dtuser.Rows[0]["u_id"] = id;
            }
            
            dtuser.Rows[0]["username"] = TextBox1.Text;
            dtuser.Rows[0]["password"] = textBox2.Text;
            dtuser.Rows[0]["usertype"] = textBox3.Text;
            dtuser.Rows[0]["location_id"] = funs.Select_locationId(textBox4.Text);
            Database.SaveData(dtuser);
            MessageBox.Show("Saved Successfully");
            if (gStr == 0)
            {
                LoadData("0", "User");
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
                MessageBox.Show("Enter User Name");
                TextBox1.Focus();
                return false;
            }
            if (textBox2.Text == "")
            {
                MessageBox.Show("Enter Password");
                textBox2.Focus();
                return false;
            }
            if (textBox3.Text == "")
            {
                MessageBox.Show("Enter User Type");
                textBox3.Focus();
                return false;
            }
            if (funs.Select_user_id(TextBox1.Text) != 0 && funs.Select_user_id(TextBox1.Text) != gStr)
            {
                MessageBox.Show("AccountName Already Exists.");
                return false;
            }
            return true;
        }

        private void TextBox1_Enter(object sender, EventArgs e)
        {
            Database.setFocus(TextBox1);
        }

        private void TextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void TextBox1_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(TextBox1);
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox2);
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox2);
        }

        private void textBox3_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox3);
        }

        private void textBox3_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox3);
        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            DataTable dtcombo = new DataTable();
            dtcombo.Columns.Add("Type", typeof(string));
            dtcombo.Columns["Type"].ColumnName = "Type";
            dtcombo.Rows.Add();
            dtcombo.Rows[0][0] = "Admin";
            dtcombo.Rows.Add();
            dtcombo.Rows[1][0] = "User";
            textBox3.Text = SelectCombo.ComboDt(this, dtcombo, 0);
            SendKeys.Send("{tab}");
        }

        private void textBox4_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox4);
        }

        private void textBox4_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox4);
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
           string strCombo = "SELECT nick_name as Location FROM Location ORDER BY nick_name";
            textBox4.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
        }
    }
}
