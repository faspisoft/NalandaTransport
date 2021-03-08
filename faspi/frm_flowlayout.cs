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
    public partial class frm_flowlayout : Form
    {
        public frm_flowlayout()
        {
            InitializeComponent();
        }

        private void frm_flowlayout_Load(object sender, EventArgs e)
        {
            this.Dock = DockStyle.Right;
            SideFill();
            flowLayoutPanel1.AutoScroll = true;
            flowLayoutPanel1.Visible = true;
        }

        void btn_Click(object sender, EventArgs e)
        {
            Button tbtn = (Button)sender;
            string name = tbtn.Name.ToString();

            if (name == "Unloading")
            {
                frmMasterVou frm = new frmMasterVou();
                frm.MdiParent = this.MdiParent;
                frm.LoadData("Unloading", "Unloading");
                frm.Show(); 
            }
            else if (name == "Search")
            {
                frm_gr_search frm = new frm_gr_search();
                frm.MdiParent = this.MdiParent;
                frm.Show();
            }
            else if (name == "Billing")
            {
                frmMasterVou frm = new frmMasterVou();
                frm.MdiParent = this.MdiParent;
                frm.LoadData("Sale", "Sale");
                frm.Show();
            }
            else if (name == "Booking Register")
            {
                frm_selector frm = new frm_selector();
                frm.MdiParent = this.MdiParent;
                frm.Show();
            }
            else if (name == "Account")
            {
                frmMaster frm = new frmMaster();
                frm.MdiParent = this.MdiParent;
                frm.LoadData("Account", "Account");
                frm.WindowState = FormWindowState.Maximized;
                frm.Show();
            }
            else if (name == "Delivery Point")
            {
                frmMaster frm = new frmMaster();
                frm.MdiParent = this.MdiParent;
                frm.LoadData("Delivery Point", "Delivery Point");
                frm.Show();
            }
            else if (name == "Item")
            {
                frmMaster frm = new frmMaster();
                frm.MdiParent = this.MdiParent;
                frm.LoadData("Item", "Item");
                frm.Show();
            }
            else if (name == "Booking")
            {
                frmMasterVou frm = new frmMasterVou();
                frm.MdiParent = this.MdiParent;
                frm.LoadData("Booking", "Booking");
                frm.Show();
            }
            else if (name == "StockTransfer")
            {
                frmMasterVou frm = new frmMasterVou();
                frm.MdiParent = this.MdiParent;
                frm.LoadData("Stock Transfer", "Stock Transfer");
                frm.Show();
            }
            else if (name == "Bill")
            {
                frmBill frm = new frmBill();
                frm.MdiParent = this.MdiParent;
                frm.Show();
            }
            else if (name == "Control Room")
            {
                frmMaster frm = new frmMaster();
                frm.LoadData("Control Room", "Control Room");
                frm.MdiParent = this.MdiParent;
                frm.Show();
            }
            else if (name == "Vouchers Confg")
            {
                frmMaster frm = new frmMaster();
                frm.LoadData("TransactionSetup", "TransactionSetup");
                frm.MdiParent = this.MdiParent;
                frm.Show();
            }
            else if (name == "Loading")
            {
                string strCombo = "SELECT [name] from DeliveryPoints";
                char cg = 'a';
                string selected = SelectCombo.ComboKeypress(this, cg, strCombo, "", 0);
                if (selected != "")
                {
                    Report gg = new Report();
                    gg.DestinationWise(Database.ldate, Database.ldate, selected);
                    gg.MdiParent = this.MdiParent;
                    gg.Show();
                }
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

            //booking
            dtsidefill.Rows.Add();
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Name"] = "Booking";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["DisplayName"] = "Booking";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["ShortcutKey"] = "";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = true;            

            //Stock Transfer
            dtsidefill.Rows.Add();
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Name"] = "StockTransfer";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["DisplayName"] = "Stock Transfer";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["ShortcutKey"] = "";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = true;

            //Loading
            dtsidefill.Rows.Add();
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Name"] = "Loading";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["DisplayName"] = "Loading";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["ShortcutKey"] = "";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = true;
            
            //booking
            dtsidefill.Rows.Add();
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Name"] = "Booking Register";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["DisplayName"] = "Booking Register";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["ShortcutKey"] = "";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = true;

            //Billing
            dtsidefill.Rows.Add();
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Name"] = "Billing";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["DisplayName"] = "Billing";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["ShortcutKey"] = "";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = true;

            //search
            dtsidefill.Rows.Add();
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Name"] = "Search";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["DisplayName"] = "Search GRno";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["ShortcutKey"] = "";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = true;

            //unloading
            dtsidefill.Rows.Add();
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Name"] = "Unloading";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["DisplayName"] = "Unloading";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["ShortcutKey"] = "";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = true;

            for (int i = 0; i < dtsidefill.Rows.Count; i++)
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
}
