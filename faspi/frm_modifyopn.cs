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
    public partial class frm_modifyopn : Form
    {
        String dtName;
        DataTable dtOpn;
    
        public frm_modifyopn()
        {
            InitializeComponent();
        }

        public void LoadData()
        {
            this.Text = "Opening Modify";
            dtName = "Account";
            dtOpn = new DataTable(dtName);
            Database.GetSqlData("SELECT * FROM  " + dtName + " order by Name ", dtOpn);

            dataGridView1.DataSource = dtOpn;
            dataGridView1.CurrentCell = dataGridView1.Rows[0].Cells[0];
         
            dataGridView1.Columns["Ac_id"].Visible = false;
            dataGridView1.Columns["Act_id"].Visible = false;
            dataGridView1.Columns["Address1"].Visible = false;
            dataGridView1.Columns["Address2"].Visible = false;
            dataGridView1.Columns["Phone"].Visible = false;
            dataGridView1.Columns["Email"].Visible = false;
            dataGridView1.Columns["Tin_number"].Visible = false;
            dataGridView1.Columns["Loc_id"].Visible = false;
            dataGridView1.Columns["Blimit"].Visible = false;
            dataGridView1.Columns["Dlimit"].Visible = false;
            dataGridView1.Columns["Con_id"].Visible = false;
            dataGridView1.Columns["note"].Visible = false;
            dataGridView1.Columns["Closing_Bal"].Visible = false;
            dataGridView1.Columns["Dr"].Visible = false;
            dataGridView1.Columns["Cr"].Visible = false;
            dataGridView1.Columns["Aadhaarno"].Visible = false;
            dataGridView1.Columns["RegStatus"].Visible = false;
            dataGridView1.Columns["PAN"].Visible = false;
            dataGridView1.Columns["state_id"].Visible = false;
            dataGridView1.Columns["Status"].Visible = false;
            dataGridView1.Columns["Printname"].Visible = false;
            dataGridView1.Columns["Allowps"].Visible = false;
            dataGridView1.Columns["Delivery_type"].Visible = false;
            dataGridView1.Columns["GR_type"].Visible = false;
            dataGridView1.Columns["Nid"].Visible = false;
            dataGridView1.Columns["LocationId"].Visible = false;
            dataGridView1.Columns["Terminal"].Visible = false;
            dataGridView1.Columns["Sid"].Visible = false;
            dataGridView1.Columns["Contact_person"].Visible = false;

            dataGridView1.Columns["Name"].DisplayIndex = 0;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.Columns["Name"].ReadOnly = true;

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1.Rows[i].Cells["Balance"].Value = funs.DecimalPoint(dataGridView1.Rows[i].Cells["Balance"].Value, 2);
            }
        }

        private void save()
        {
            try
            {
                int Deleted = dtOpn.Select("", "", DataViewRowState.Deleted).Length;
                int Modefied = dtOpn.Select("", "", DataViewRowState.ModifiedCurrent).Length;
                Database.SaveData(dtOpn);
                funs.ShowBalloonTip("Saved", "saved successfully, " + (Deleted + Modefied) + " item(s) effected");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            this.Close();
            this.Dispose();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            save();
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
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
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {

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
                save();
            }
            else if (name == "quit")
            {
                this.Close();
                this.Dispose();
            }
        }
        private void frm_modifyopn_Load(object sender, EventArgs e)
        {
            this.Size = this.MdiParent.Size;
            SideFill();
        }

        private void frm_modifyopn_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
            {
                save();
            }

            if (e.KeyCode == Keys.Escape)
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
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            dataGridView1.CurrentCell.Value = 0;
        }
    }
}
