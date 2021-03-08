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
    public partial class frmOtherDetails : Form
    {
        DataTable transDetails = new DataTable();

        public frmOtherDetails()
        {
            InitializeComponent();
        }

        private void frmOtherDetails_Load(object sender, EventArgs e)
        {
            Database.GetSqlData("select * from TransportDetails", transDetails);
            for (int i = 0; i < transDetails.Rows.Count; i++)
            {               
                dataGridView1.Rows.Add();
                dataGridView1.Rows[i].Cells[0].Value = transDetails.Rows[i]["FName"];
                dataGridView1.Rows[i].Cells[1].Value = transDetails.Rows[i]["ShowingName"];
                dataGridView1.Rows[i].Cells[2].Value = transDetails.Rows[i]["status"];
            }
        }

        private void dataGridView1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsLetter(e.KeyChar) || char.IsNumber(e.KeyChar) || e.KeyChar == ' ')
            {
                if (dataGridView1.CurrentCell.OwningColumn.Name == "Status")
                {
                    DataTable dtStatus = new DataTable();
                    dtStatus.Columns.Add("Status", typeof(string));
                    dtStatus.Rows.Add();
                    dtStatus.Rows[0][0] = "Visible";
                    dtStatus.Rows.Add();
                    dtStatus.Rows[1][0] = "Not Visible";
                    string selected = SelectCombo.ComboDt(this, dtStatus, 0);
                    dataGridView1.CurrentCell.Value = selected;
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable("TransportDetails");
            Database.GetSqlData("Select * from Transportdetails", dt);

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dt.Rows[i]["FName"] = dataGridView1.Rows[i].Cells["fname"].Value.ToString();
                dt.Rows[i]["ShowingName"] = dataGridView1.Rows[i].Cells["ShowingText"].Value.ToString();
                dt.Rows[i]["status"] = dataGridView1.Rows[i].Cells["status"].Value.ToString();
            }

            Database.SaveData(dt);
            MessageBox.Show("Saved Successfully");
            this.Close();
            this.Dispose();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        private void frmOtherDetails_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
                this.Dispose();
            }
        }
    }
}