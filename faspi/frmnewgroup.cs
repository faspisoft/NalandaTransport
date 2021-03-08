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
    public partial class frmnewgroup : Form
    {
        DataTable dtGrp;
        String dtName;

        public bool calledIndirect = false;
        public String GrpName;

        String gStr;

        public frmnewgroup()
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
        void btn_Click(object sender, EventArgs e)
        {
            Button tbtn = (Button)sender;
            string name = tbtn.Name.ToString();

            if (name == "save")
            {
                if (validate() == true)
                {
                    save();
             
                    if (calledIndirect == true)
                    {
                        this.Close();
                    }
                }
            }




            if (name == "quit")
            {
                this.Close();
                this.Dispose();
            }







        }

        private void frmnewgroup_Load(object sender, EventArgs e)
        {
            SideFill();
        }

        public void LoadData(String str, String frmCaption)
        {
            gStr = str;
            dtName = "Accountypes";
            dtGrp = new DataTable(dtName);
            Database.GetSqlData("select * from " + dtName + " where Act_id=" + int.Parse(str), dtGrp);




            textBox1.Focus();

            this.Text = frmCaption;
            if (dtGrp.Rows.Count == 0)
            {
                dtGrp.Rows.Add(0);
                textBox1.Text = "";
                textBox2.Text = "";

            }
            else
            {
                textBox1.Text = dtGrp.Rows[0]["name"].ToString();


                textBox2.Text = funs.Select_act_nm(int.Parse(dtGrp.Rows[0]["under"].ToString()));




            }
        }

        private void save()
        {
            GrpName = textBox1.Text;
            dtGrp.Rows[0]["name"] = textBox1.Text;
            dtGrp.Rows[0]["RefineName"] = textBox1.Text;
            dtGrp.Rows[0]["Type"] = "Account";
            dtGrp.Rows[0]["under"] = funs.Select_act_id(textBox2.Text);
            dtGrp.Rows[0]["Nature"] = funs.Select_act_nature(textBox2.Text);
            dtGrp.Rows[0]["fixed"] = false;
            string path = funs.Select_act_path(textBox2.Text);
            dtGrp.Rows[0]["Path"] = "";
            int level = funs.Select_act_level(textBox2.Text) + 1;
            dtGrp.Rows[0]["level"] = level;
            dtGrp.Rows[0]["Sequence"] = 0;

            if (gStr == "0")
            {
                dtGrp.Rows[0]["create_date"] = System.DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
            }
            dtGrp.Rows[0]["modify_date"] = System.DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");

            Database.SaveData(dtGrp);

            int actid = funs.Select_AccType_id(textBox1.Text);
            path = path + actid + ";";
            Database.CommandExecutor("Update Accountypes set Path='" + path + "' where Act_id=" + actid);

          //  funs.ShowBalloonTip("Saved", "Saved Successfully");
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
            if (calledIndirect == true)
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

            if (funs.Select_AccType_id(textBox1.Text) != 0 && funs.Select_AccType_id(textBox1.Text) != int.Parse(gStr))
            {
                MessageBox.Show("Account Group Already Exists");
                return false;
            }

            return true;
        }
        private void frmnewgroup_KeyDown(object sender, KeyEventArgs e)
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
            else
            {
                textBox1.BackColor = Color.White;

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

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            string strCombo = "Select Name from Accountypes order by Name";

            textBox2.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);


          
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox2);
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

    }
}
