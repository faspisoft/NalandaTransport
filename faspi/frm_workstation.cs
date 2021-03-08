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
    public partial class frm_workstation : Form
    {
        DataTable dtAcc;
        String dtName;
        string act_name = "";
        public bool calledIndirect = false;
        public String AccName;
        public String AccType;
        String strCombo;
        public string gStr = "";

        public frm_workstation()
        {
            InitializeComponent();
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
        private bool validate()
        {
            return true;
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
        public void LoadData(String str, String frmCaption)
        {
            gStr = str;
            this.Text = frmCaption;



            dtName = "Workstations";
            dtAcc = new DataTable(dtName);
            Database.GetSqlData("select * from " + dtName + " where id='" + str + "'", dtAcc);

            if (dtAcc.Rows.Count == 0)
            {
                dtAcc.Rows.Add(0);
                TextBox1.Select();
                TextBox1.Text = "";
             
                textBox18.Text = "";
                checkBox1.Checked = false;
            }
         
            else
            {
                TextBox1.Select();
                TextBox1.Text = dtAcc.Rows[0]["Sys_name"].ToString();
              
                textBox18.Text = dtAcc.Rows[0]["Sys_code"].ToString();
                if (bool.Parse(dtAcc.Rows[0]["active"].ToString()) == true)
                {
                    checkBox1.Checked = true;
                }
                else
                {
                    checkBox1.Checked = false;
                }
              
            }
            
        }

        private void save()
        {
            AccName = TextBox1.Text;

           
            dtAcc.Rows[0]["sys_name"] = TextBox1.Text;
            dtAcc.Rows[0]["sys_code"] = textBox18.Text;
            if (checkBox1.Checked == true)
            {
                dtAcc.Rows[0]["Active"] = true;
            }
            else
            {
                dtAcc.Rows[0]["Active"] = false;
            }
            Database.SaveData(dtAcc);
          
            MessageBox.Show("Saved Successfully");
           
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

        private void frm_workstation_Load(object sender, EventArgs e)
        {
            SideFill();
        }

        private void frm_workstation_KeyDown(object sender, KeyEventArgs e)
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

        private void TextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox18_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void TextBox1_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(TextBox1);
        }

        private void textBox18_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox18);
        }

        private void TextBox1_Enter(object sender, EventArgs e)
        {
            Database.setFocus(TextBox1);
        }

        private void textBox18_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox18);
        }

    }
}
