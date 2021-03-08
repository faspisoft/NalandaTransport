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
    public partial class frm_gr_search : Form
    {
        public frm_gr_search()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            string str = textBox10.Text.Trim();
            DataTable dt = new DataTable();

            Database.GetSqlData("SELECT VOUCHERINFOs.Invoiceno, convert(nvarchar,VOUCHERINFOs.Vdate,106) as Vdate, ACCOUNTs.name AS consigner, ACCOUNTs_1.name AS consignee, DeliveryPoints.Name AS source, DeliveryPoints_1.Name AS destination, SUM(Voucherdets.Quantity) AS quantity, SUM(Voucherdets.weight) AS weight, SUM(Voucherdets.ChargedWeight) AS chweight FROM VOUCHERINFOs LEFT OUTER JOIN Voucherdets ON VOUCHERINFOs.Vi_id = Voucherdets.Vi_id LEFT OUTER JOIN DeliveryPoints AS DeliveryPoints_1 ON VOUCHERINFOs.SId = DeliveryPoints_1.DPId LEFT OUTER JOIN DeliveryPoints ON VOUCHERINFOs.Consigner_id = DeliveryPoints.DPId LEFT OUTER JOIN ACCOUNTs AS ACCOUNTs_1 ON VOUCHERINFOs.Ac_id2 = ACCOUNTs_1.ac_id LEFT OUTER JOIN ACCOUNTs ON VOUCHERINFOs.Ac_id = ACCOUNTs.ac_id LEFT OUTER JOIN VOUCHERTYPEs ON VOUCHERINFOs.Vt_id = VOUCHERTYPEs.Vt_id WHERE (VOUCHERTYPEs.Type = 'Booking') GROUP BY VOUCHERINFOs.Invoiceno, VOUCHERINFOs.Vdate, ACCOUNTs.name, ACCOUNTs_1.name, DeliveryPoints.Name, DeliveryPoints_1.Name HAVING (VOUCHERINFOs.Invoiceno = '" + str + "') ORDER BY VOUCHERINFOs.Vdate DESC", dt);

            if (dt.Rows.Count > 0)
            {
                textBox1.Text = dt.Rows[0]["Invoiceno"].ToString();
                textBox2.Text = dt.Rows[0]["Vdate"].ToString();
                textBox3.Text = dt.Rows[0]["consigner"].ToString();
                textBox4.Text = dt.Rows[0]["consignee"].ToString();
                textBox5.Text = dt.Rows[0]["source"].ToString();
                textBox6.Text = dt.Rows[0]["destination"].ToString();
                textBox7.Text = funs.IndianCurr(double.Parse(dt.Rows[0]["quantity"].ToString()));
                textBox8.Text = funs.IndianCurr(double.Parse(dt.Rows[0]["weight"].ToString()));
                textBox14.Text = funs.IndianCurr(double.Parse(dt.Rows[0]["chweight"].ToString()));

                DataTable dt2 = new DataTable();
                Database.GetSqlData("SELECT VOUCHERINFOs.Invoiceno, convert(nvarchar,VOUCHERINFOs.Vdate,106) as Vdate, Gaddis.Gaddi_name, ACCOUNTs.name AS Driver FROM VOUCHERINFOs AS VOUCHERINFOs_1 LEFT OUTER JOIN Voucherdets AS Voucherdets_1 ON VOUCHERINFOs_1.Vi_id = Voucherdets_1.Vi_id RIGHT OUTER JOIN Voucherdets ON VOUCHERINFOs_1.Vi_id = Voucherdets.Booking_id RIGHT OUTER JOIN VOUCHERINFOs ON Voucherdets.Vi_id = VOUCHERINFOs.Vi_id LEFT OUTER JOIN ACCOUNTs ON VOUCHERINFOs.Driver_name = ACCOUNTs.ac_id LEFT OUTER JOIN Gaddis ON VOUCHERINFOs.Gaddi_id = Gaddis.Gaddi_id WHERE (VOUCHERINFOs.Vt_id = 63) AND (VOUCHERINFOs_1.Invoiceno = '" + textBox10.Text + "') ORDER BY VOUCHERINFOs.Vdate DESC", dt2);

                if (dt2.Rows.Count > 0)
                {
                    textBox9.Text = dt2.Rows[0]["Invoiceno"].ToString();
                    textBox11.Text = dt2.Rows[0]["Vdate"].ToString();
                    textBox12.Text = dt2.Rows[0]["Gaddi_name"].ToString();
                    textBox13.Text = dt2.Rows[0]["Driver"].ToString();
                }
                else
                {
                    textBox9.Text = "";
                    textBox11.Text = "";
                    textBox12.Text = "";
                    textBox13.Text = "";
                }
            }
            else
            {
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";
                textBox5.Text = "";
                textBox6.Text = "";
                textBox7.Text = "";
                textBox8.Text = "";
                textBox14.Text = "";
                textBox9.Text = "";
                textBox11.Text = "";
                textBox12.Text = "";
                textBox13.Text = "";
                MessageBox.Show("This GRNO Not Exist");
            }
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void frm_gr_search_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
                this.Dispose();
            }
        }
    }
}
