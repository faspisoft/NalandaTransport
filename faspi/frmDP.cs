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
    public partial class frmDP : Form
    {
        DataTable dtItem;
        string dtName;
        public bool calledIndirect = false;
        public string DPName;
        public string Type;
        string gStr;

        public frmDP()
        {
            InitializeComponent();
        }

        private void frmItem_Load(object sender, EventArgs e)
        {
            SideFill();
        }

        public void LoadData(String str, String frmCaption)
        {
            gStr = str;
            dtName = "DeliveryPoints";
            dtItem = new DataTable(dtName);
            Database.GetSqlData("select * from " + dtName + " where [DPId]='" + str + "'", dtItem);

            if (dtItem.Rows.Count == 0)
            {
                dtItem.Rows.Add(0);
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                this.Text = frmCaption;
            }
            else
            {
                textBox1.Text = dtItem.Rows[0]["name"].ToString();
                textBox2.Text = dtItem.Rows[0]["address"].ToString();
                textBox3.Text = dtItem.Rows[0]["contactno"].ToString();
                this.Text = frmCaption;
            }
        }

        private void save()
        {
            DPName = textBox1.Text;
            if (gStr == "0")
            {
                DataTable dtCount = new DataTable();
                Database.GetSqlData("select count(*) from DeliveryPoints where locationid='" + Database.LocationId + "'", dtCount);
                if (int.Parse(dtCount.Rows[0][0].ToString()) == 0)
                {
                    dtItem.Rows[0]["DPId"] = Database.LocationId + "1";
                    dtItem.Rows[0]["Nid"] = 1;
                    dtItem.Rows[0]["LocationId"] = Database.LocationId;
                }
                else
                {
                    DataTable dtAcid = new DataTable();
                    Database.GetSqlData("select max(Nid) as Nid from DeliveryPoints where locationid='" + Database.LocationId + "'", dtAcid);
                    int Nid = int.Parse(dtAcid.Rows[0][0].ToString());
                    dtItem.Rows[0]["DPId"] = Database.LocationId + (Nid + 1);
                    dtItem.Rows[0]["Nid"] = (Nid + 1);
                    dtItem.Rows[0]["LocationId"] = Database.LocationId;
                }                
            }
            dtItem.Rows[0]["name"] = textBox1.Text;
            dtItem.Rows[0]["address"] = textBox2.Text;
            dtItem.Rows[0]["ContactNo"] = textBox3.Text;

            if (gStr == "0")
            {
                dtItem.Rows[0]["create_date"] = System.DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
            }
            dtItem.Rows[0]["modify_date"] = System.DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");

            Database.SaveData(dtItem);
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
            if (funs.Select_dp_id(textBox1.Text) != "" && funs.Select_dp_id(textBox1.Text) != gStr)
            {
                MessageBox.Show("DeliveryPoint Already Exists");
                return false;
            }
            return true;
        }

        private void frmItem_KeyDown(object sender, KeyEventArgs e)
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
                if (textBox1.Text != "")
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

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\'')
            {
                e.Handled = true;
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

        private void textBox2_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox2);
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox2);
        }

        private void textBox2_KeyDown_1(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox3_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox3);
        }

        private void textBox3_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox3);
        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }
    }
}
