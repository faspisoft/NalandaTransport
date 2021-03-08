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
    public partial class frmBill : Form
    {
        public frmBill()
        {
            InitializeComponent();
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            string strCombo = "SELECT distinct ACCOUNT.Name FROM VOUCHERINFO INNER JOIN ACCOUNT ON VOUCHERINFO.Ac_id2 = ACCOUNT.Ac_id";
            textBox3.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //frmBillDetail frm = new frmBillDetail();
            //frm.LoadData(textBox3.Text, dateTimePicker1.Value.Date, dateTimePicker2.Value.Date);
            //frm.MdiParent = this.MdiParent;
            //frm.Show();
            //this.Hide();

            Report frm = new Report();
            frm.BillReport(textBox3.Text, dateTimePicker1.Value.Date, dateTimePicker2.Value.Date);
            frm.MdiParent = this.MdiParent;
            frm.Show();
            this.Hide();
        }

        private void dateTimePicker1_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void dateTimePicker2_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void frmBill_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
                this.Dispose();

            }
        }

        private void frmBill_Load(object sender, EventArgs e)
        {
            dateTimePicker1.Value = Database.ldate;
            dateTimePicker2.Value = Database.ldate;
            dateTimePicker1.CustomFormat = Database.dformat;
            dateTimePicker2.CustomFormat = Database.dformat;
            dateTimePicker1.MaxDate = Database.ldate;
            dateTimePicker2.MaxDate = Database.ldate;
            dateTimePicker1.MinDate = Database.stDate;
            dateTimePicker2.MinDate = Database.stDate;
        }
    }
}
