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
    public partial class Frm_GrSearch : Form
    {
        public Frm_GrSearch()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
          //  string sql = "SELECT VOUCHERINFOs.Invoiceno, VOUCHERINFOs.Vdate, ACCOUNTs.name AS Consigner, ACCOUNTs_1.name AS Consignee,  DeliveryPoints_1.Name AS Source, DeliveryPoints.Name AS Destination, SUM(Voucherdets.Quantity) AS Quantity, SUM(Voucherdets.weight) AS Weight,  SUM(Voucherdets.ChargedWeight) AS Cweight, dbo.VOUCHERINFOs.Vi_id FROM VOUCHERINFOs INNER JOIN  VOUCHERTYPEs ON VOUCHERINFOs.Vt_id = VOUCHERTYPEs.Vt_id LEFT OUTER JOIN  Voucherdets ON VOUCHERINFOs.Vi_id = Voucherdets.Vi_id LEFT OUTER JOIN  DeliveryPoints ON VOUCHERINFOs.SId = DeliveryPoints.DPId LEFT OUTER JOIN  ACCOUNTs AS ACCOUNTs_1 ON VOUCHERINFOs.Ac_id2 = ACCOUNTs_1.ac_id LEFT OUTER JOIN  ACCOUNTs ON VOUCHERINFOs.Ac_id = ACCOUNTs.ac_id LEFT OUTER JOIN  DeliveryPoints AS DeliveryPoints_1 ON VOUCHERINFOs.Consigner_id = DeliveryPoints_1.DPId WHERE (VOUCHERTYPEs.Type = N'Booking') AND (dbo.VOUCHERINFOs.Iscancel = 0) GROUP BY VOUCHERINFOs.Invoiceno, VOUCHERINFOs.Vdate, ACCOUNTs.name, ACCOUNTs_1.name, DeliveryPoints_1.Name, DeliveryPoints.Name, dbo.VOUCHERINFOs.Vi_id  HAVING (VOUCHERINFOs.Invoiceno = '" + textBox10.Text + "')  ORDER BY VOUCHERINFOs.Vdate ";
            string sql = "SELECT  VOUCHERINFOs.Invoiceno, VOUCHERINFOs.Vdate, ACCOUNTs.name AS Consigner, ACCOUNTs_1.name AS Consignee,  DeliveryPoints_1.Name AS Source, DeliveryPoints.Name AS Destination, Stocks.TotPkts AS Quantity, Stocks.TotWeight  AS Weight, VOUCHERINFOs.Vi_id FROM VOUCHERINFOs INNER JOIN  VOUCHERTYPEs ON VOUCHERINFOs.Vt_id = VOUCHERTYPEs.Vt_id LEFT OUTER JOIN  Stocks ON VOUCHERINFOs.Vi_id = Stocks.vid LEFT OUTER JOIN  DeliveryPoints ON VOUCHERINFOs.SId = DeliveryPoints.DPId LEFT OUTER JOIN  ACCOUNTs AS ACCOUNTs_1 ON VOUCHERINFOs.Ac_id2 = ACCOUNTs_1.ac_id LEFT OUTER JOIN  ACCOUNTs ON VOUCHERINFOs.Ac_id = ACCOUNTs.ac_id LEFT OUTER JOIN  DeliveryPoints AS DeliveryPoints_1 ON VOUCHERINFOs.Consigner_id = DeliveryPoints_1.DPId WHERE ( VOUCHERTYPEs.Type = N'Booking') AND ( VOUCHERINFOs.Iscancel = 0) GROUP BY VOUCHERINFOs.Invoiceno, VOUCHERINFOs.Vdate, ACCOUNTs.name, ACCOUNTs_1.name, DeliveryPoints_1.Name, DeliveryPoints.Name,   VOUCHERINFOs.Vi_id, Stocks.TotPkts, Stocks.TotWeight HAVING ( VOUCHERINFOs.Invoiceno = '" + textBox10.Text + "') ORDER BY VOUCHERINFOs.Vdate ";
            DataTable dt = new DataTable();
            Database.GetSqlData(sql,dt);
            ansGridView1.Rows.Clear();
            ansGridView2.Rows.Clear();
            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("This GR doesn't Exists.");
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {

                ansGridView1.Rows.Add();
                ansGridView1.Rows[i].Cells["sno"].Value = i + 1;

                ansGridView1.Rows[i].Cells["grno"].Value = dt.Rows[i]["Invoiceno"].ToString();
                ansGridView1.Rows[i].Cells["bookingdate"].Value =DateTime.Parse(dt.Rows[i]["Vdate"].ToString()).ToString(Database.dformat);
                ansGridView1.Rows[i].Cells["Consigner"].Value = dt.Rows[i]["Consigner"].ToString();
                ansGridView1.Rows[i].Cells["Consignee"].Value = dt.Rows[i]["Consignee"].ToString();
                ansGridView1.Rows[i].Cells["source"].Value = dt.Rows[i]["source"].ToString();
                ansGridView1.Rows[i].Cells["destination"].Value = dt.Rows[i]["destination"].ToString();
                ansGridView1.Rows[i].Cells["vi_id"].Value = dt.Rows[i]["vi_id"].ToString();
                ansGridView1.Rows[i].Cells["totquantity"].Value = dt.Rows[i]["Quantity"].ToString();
                ansGridView1.Rows[i].Cells["totweight"].Value = dt.Rows[i]["Weight"].ToString();
           //     ansGridView1.Rows[i].Cells["ctotweight"].Value = dt.Rows[i]["CWeight"].ToString();
                //ansGridView1.Rows[i].Cells["cancel"].Value = dt.Rows[i]["Iscancel"].ToString();

            }
        }

        private void ansGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (ansGridView1.CurrentCell == null)
            {
                return;
            }

            if (ansGridView1.CurrentCell.OwningColumn.Name == "details")
            {

               
                    string vid = ansGridView1.Rows[ansGridView1.CurrentRow.Index].Cells["Vi_id"].Value.ToString();
                    ansGridView2.Rows.Clear();

                    string sql1 = "SELECT      VOUCHERINFOs.Vdate,  Location.nick_name AS Location,  VOUCHERTYPEs.Type AS EntryType,  VOUCHERINFOs.Invoiceno AS ReffNo,  VOUCHERINFOs.create_date AS EntryDate,  USERs.UserName AS EnteredBy,VOUCHERINFOs.Vi_id as Vi_id FROM          Stocks INNER JOIN   VOUCHERINFOs ON  Stocks.vid =  VOUCHERINFOs.Vi_id INNER JOIN                       VOUCHERTYPEs ON  VOUCHERINFOs.Vt_id =  VOUCHERTYPEs.Vt_id INNER JOIN    USERs ON  VOUCHERINFOs.user_id =  USERs.u_id INNER JOIN Location ON  VOUCHERINFOs.LocationId =  Location.LocationId WHERE     ( Stocks.GR_id = '" + vid + "') ORDER BY EntryDate";

                    DataTable dtdet = new DataTable();
                    Database.GetSqlData(sql1, dtdet);
                    for (int i = 0; i < dtdet.Rows.Count; i++)
                    {
                        ansGridView2.Rows.Add();
                        ansGridView2.Rows[i].Cells["Vdate"].Value = DateTime.Parse(dtdet.Rows[i]["Vdate"].ToString()).ToString(Database.dformat);
                        ansGridView2.Rows[i].Cells["Location"].Value = dtdet.Rows[i]["Location"].ToString();
                        ansGridView2.Rows[i].Cells["EntryType"].Value = dtdet.Rows[i]["EntryType"].ToString();
                        ansGridView2.Rows[i].Cells["ReffNo"].Value = dtdet.Rows[i]["ReffNo"].ToString();
                        ansGridView2.Rows[i].Cells["EntryDate"].Value = DateTime.Parse(dtdet.Rows[i]["EntryDate"].ToString()).ToString(Database.dformat);
                        ansGridView2.Rows[i].Cells["Enteredby"].Value = dtdet.Rows[i]["Enteredby"].ToString();
                        ansGridView2.Rows[i].Cells["viid"].Value = dtdet.Rows[i]["vi_id"].ToString();

                    }
                
               

            }
        }

        private void Frm_GrSearch_Load(object sender, EventArgs e)
        {

        }

        private void Frm_GrSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
                this.Dispose();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        private void ansGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (ansGridView2.CurrentCell == null)
            {
                return;
            }
            if (ansGridView2.Rows[ansGridView2.CurrentRow.Index].Cells["Viid"].Value.ToString() == "0")
            {
                return;
            }
            if (ansGridView2.CurrentCell.OwningColumn.Name == "detail")
            {


                string vid = ansGridView2.Rows[ansGridView2.CurrentRow.Index].Cells["Viid"].Value.ToString();


                frm_printcopy frm = new frm_printcopy("View", ansGridView2.Rows[ansGridView2.CurrentRow.Index].Cells["Viid"].Value.ToString(), funs.Select_vtid(ansGridView2.Rows[ansGridView2.CurrentRow.Index].Cells["Viid"].Value.ToString()));
                frm.Show();

            }
        }
    }
}
