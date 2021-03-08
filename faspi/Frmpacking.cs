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
    public partial class Frmpacking : Form
    {
        DataTable dtpacking;
        string dtName;
        public bool calledIndirect = false;
        public string PackingName;
        string gStr;

        public Frmpacking()
        {
            InitializeComponent();
        }

        private void Frmpacking_Load(object sender, EventArgs e)
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
            dtName = "packings";
            this.Text = frmCaption;
            TextBox1.Focus();
            dtpacking = new DataTable(dtName);
            Database.GetSqlData("select * from " + dtName + " where p_id='" + str + "'", dtpacking);
            
            if (dtpacking.Rows.Count == 0)
            {
                dtpacking.Rows.Add(0);
                TextBox1.Text = "";
            }
            else
            {
                TextBox1.Text = dtpacking.Rows[0]["name"].ToString();                
            }
        }

        private void save()
        {
            PackingName = TextBox1.Text;

            if (gStr == "0")
            {
                DataTable dtCount = new DataTable();
                Database.GetSqlData("select count(*) from Packings where locationid='" + Database.LocationId + "'", dtCount);

                if (int.Parse(dtCount.Rows[0][0].ToString()) == 0)
                {
                    dtpacking.Rows[0]["p_id"] = Database.LocationId + "1";
                    dtpacking.Rows[0]["Nid"] = 1;
                    dtpacking.Rows[0]["LocationId"] = Database.LocationId;
                }
                else
                {
                    DataTable dtid = new DataTable();
                    Database.GetSqlData("select max(Nid) as Nid from Packings where locationid='" + Database.LocationId + "'", dtid);
                    int Nid = int.Parse(dtid.Rows[0][0].ToString());
                    dtpacking.Rows[0]["p_id"] = Database.LocationId + (Nid + 1);
                    dtpacking.Rows[0]["Nid"] = (Nid + 1);
                    dtpacking.Rows[0]["LocationId"] = Database.LocationId;
                }
            }

            dtpacking.Rows[0]["name"] = TextBox1.Text;
            if (gStr == "0")
            {
                dtpacking.Rows[0]["create_date"] = System.DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
            }
            dtpacking.Rows[0]["modify_date"] = System.DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");

            Database.SaveData(dtpacking);
            MessageBox.Show("Saved Successfully");
            if (calledIndirect == true)
            {
                this.Close();
                this.Dispose();
            }
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

        private bool validate()
        {
            if (TextBox1.Text == "")
            {
                TextBox1.Focus();
                return false;
            }          
            if (funs.Select_broker_id(TextBox1.Text) != "" && funs.Select_broker_id(TextBox1.Text) != gStr)
            {
                MessageBox.Show("Packing Name Already Exists");
                return false;
            }

            return true;
        }

        private void Frmpacking_KeyDown(object sender, KeyEventArgs e)
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
    }
}
