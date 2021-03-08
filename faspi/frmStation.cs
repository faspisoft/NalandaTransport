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
    public partial class frmStation : Form
    {
        DataTable dtStation;
        String dtName;
         
        public bool calledIndirect = false;
        public String BrokerName;
        public string stationame;
        
        String gStr;
        public frmStation()
        {
            InitializeComponent();
        }

        private void frmBroker_Load(object sender, EventArgs e)
        {
           //Dongle.lockOk();
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
            if (name == "quit")
            {
                this.Close();
                this.Dispose();
            }
        }

        public void LoadData(String str, String frmCaption)
        {
            gStr = str;
            dtName = "Station";
            dtStation = new DataTable(dtName);
            Database.GetSqlData("select * from " + dtName + " where [SId]='" + str + "'", dtStation);
            this.Text = frmCaption;
            if (dtStation.Rows.Count == 0)
            {
                dtStation.Rows.Add(0);
                TextBox1.Text = "";
                TextBox2.Text = "";

            }
            else
            {
                
                TextBox1.Text = dtStation.Rows[0]["name"].ToString();
                TextBox2.Text = funs.Select_dp_nm(dtStation.Rows[0]["DPId"].ToString());
                
            }
        }

        private void save()
        {
            stationame = TextBox1.Text;
            if (gStr == "0")
            {
                DataTable dtCount = new DataTable();
                Database.GetSqlData("select count(*) from Station", dtCount);
                if (int.Parse(dtCount.Rows[0][0].ToString()) == 0)
                {
                    dtStation.Rows[0]["SId"] = Database.LocationId + "1";
                }
                else
                {
                    DataTable dtSId = new DataTable();
                    Database.GetSqlData("select max(cast(substring(SId,4,2) as int)) from Station", dtSId);
                    int stid = int.Parse(dtSId.Rows[0][0].ToString());
                    dtStation.Rows[0]["SId"] = Database.LocationId + (stid + 1);
                }
            }
            dtStation.Rows[0]["name"] = TextBox1.Text;
            dtStation.Rows[0]["DPId"] = funs.Select_dp_id(TextBox2.Text);
            
            Database.SaveData(dtStation);
             funs.ShowBalloonTip("Saved", "Saved Successfully");
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
            
            if (TextBox1.Text == "")
            {
                TextBox1.Focus();
                return false;
            }
            else if (TextBox2.Text == "")
            {
                TextBox2.Focus();
                return false;
            }
            if (funs.Select_dp_id(TextBox1.Text) != "" && funs.Select_dp_id(TextBox1.Text) != gStr)
            {
                MessageBox.Show("Station Name Already Exists");
                return false;
            }
    
            return true;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (validate() == true)
            {
                save();
               
            }
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
                if(TextBox1.Text!="")
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

        private void Button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void TextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void TextBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C)
            {
                TextBox2.Text = funs.AddDP();
            }

            if (e.Control && e.KeyCode == Keys.A)
            {
                if (TextBox2.Text != "")
                {
                    TextBox2.Text = funs.EditDP(TextBox2.Text);
                }
            }
        }

        private void TextBox3_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void TextBox4_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void TextBox5_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void TextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\'')
            {
                e.Handled = true;
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void TextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void TextBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void TextBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void TextBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void TextBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            string strCombo = "SELECT [name] from DeliveryPoint order by name";
            TextBox2.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
        }
    }
}
