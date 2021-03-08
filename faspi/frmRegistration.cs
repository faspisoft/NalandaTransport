using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Data.OleDb;

namespace faspi
{
    public partial class frmRegistration : Form
    {
        
        DataTable dtRegis;
        String dtName;
        
        private string server;
        private string database;
        private string uid;
        private string password;
     //   MySqlConnection cn;

        public frmRegistration()
        {
            InitializeComponent();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        public void LoadData(string DongleNo, string CName, string FName, string Address, string City,string Contact, DateTime Doa,DateTime Doamc, DateTime Today)
        {
           
                textBox1.Text = FName;
                textBox2.Text = CName;
                textBox3.Text = Today.ToString("dd-MMM-yyyy");
                textBox4.Text = Contact;
                textBox5.Text = Address;
                textBox6.Text = City;
                textBox7.Text = DongleNo;
                textBox9.Text = Doa.ToString("dd-MMM-yyyy");
                textBox10.Text = Doamc.ToString("dd-MMM-yyyy");
                if (Doa >= Today)
                {
                    textBox8.Text = "Active";
                }
                else
                {
                    textBox8.Text = "Disable(Please Renew your AMC for Continue getting Updates)";
                }
           
          
        }

        private void save()
        {
            //DataTable dtTemp = new DataTable("activate");
            //Database.GetSqlData("select * from activate", dtTemp);
            //for (int i = 0; i < dtTemp.Rows.Count; i++)
            //{
            //    dtTemp.Rows[i].Delete();
            //}
            //Database.SaveData(dtTemp);
            Database.CommandExecutorOther("delete from activate");
            Database.CommandExecutorOther("Update FEATURE set Active=true where Features='Activated'");
            Database.CommandExecutorOther("insert into ACTIVATE ([Column],[Value]) values('Dongle','" +  textBox7.Text + "')");
            MessageBox.Show("Software Activated successfully");
            this.Dispose();
            Form[] frms = this.MdiChildren;
            foreach (Form frm in frms)
            {
                frm.Dispose();
            }
            Environment.Exit(0);
           
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            save();
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

        private void textBox4_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox5_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox6_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox7_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox8_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox9_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox10_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void frmRegistration_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                Button1_Click(sender, e);
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
                    this.Dispose();
                }
            }
        }

    }
}
