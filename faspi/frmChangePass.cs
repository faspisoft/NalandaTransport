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
    public partial class frmChangePass : Form
    {
        DataTable dtUsr;
        String dtName;
        public String UserName;
        public string gStr = "";

        public frmChangePass()
        {
            InitializeComponent();
        }

        private void frmChangePass_KeyDown(object sender, KeyEventArgs e)
        {
           if (e.KeyCode == Keys.Escape)
            {
                this.Close();
                this.Dispose();
            }

            else if (e.Control && e.KeyCode == Keys.S)
            {
                if (Validate() == true)
                {
                    UserName = textBox1.Text;
                    dtUsr.Rows[0]["UserName"] = textBox1.Text;
                    dtUsr.Rows[0]["Password"] = textBox2.Text;
                    Database.uname = textBox1.Text;
                    Database.upass = textBox2.Text;
                    Database.SaveData(dtUsr);
                    this.Close();
                    this.Dispose();
                }
            }            
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

        public void LoadData(string uname, string FrmCaption)
        {
            gStr = uname;
            dtName = "USERs";
            dtUsr = new DataTable(dtName);
            this.Text = FrmCaption;
            Database.GetSqlData("select * from " + dtName + " where UserName='" + uname + "' ", dtUsr);
            if (dtUsr.Rows.Count == 0)
            {
                dtUsr.Rows.Add(0);
                textBox1.Text = "";
                textBox2.Text = "";
            }
            else
            {
                textBox1.Text = dtUsr.Rows[0]["UserName"].ToString();
                textBox2.Text = dtUsr.Rows[0]["Password"].ToString(); 
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
                if (Validate() == true)
                {
                    UserName = textBox1.Text;
                    dtUsr.Rows[0]["UserName"] = textBox1.Text;
                    dtUsr.Rows[0]["Password"] = textBox2.Text;
                    Database.uname = textBox1.Text;
                    Database.upass = textBox2.Text;
                    Database.SaveData(dtUsr);
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

        private bool Validate()
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

            if (funs.Select_user_id(textBox1.Text) != 0 && funs.Select_user_id(textBox1.Text) != funs.Select_user_id(gStr))
            {
                MessageBox.Show("AccountName Already Exists.");
                return false;
            }
            return true;
        }

        private void frmChangePass_Load(object sender, EventArgs e)
        {
            this.Size = this.MdiParent.Size;
            SideFill();
        }
    }
}
