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
    public partial class frm_Challan : Form
    {

        int vtid;
        int vno = 0;
        string gStr = "";
        string vid = "";
        public Boolean gresave = false;
        Boolean f12used = false;
        DataTable dtVoucherinfo;
        DataTable dtVoucherDet;
        string Prelocationid = "";
        Boolean RoffChanged = false;
        bool iscancel = false;


        DateTime create_date = DateTime.Parse(System.DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"));




        public frm_Challan()
        {
            InitializeComponent();
           // SideFill();
            dateTimePicker1.CustomFormat = Database.dformat;
            dateTimePicker1.MaxDate = Database.ldate;
            dateTimePicker1.MinDate = Database.stDate;
        }

        private void txtTruckNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            string strCombo = "Select Gaddi_name from Gaddis order by Gaddi_name";
            txtTruckNo.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, "", 0);
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            string wheresrt = "(Path LIKE '1;39;41;%')  ";
            string strCombo = funs.GetStrCombo(wheresrt);
            textBox2.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 1);
            //string strCombo = "SELECT ACCOUNTs.Name FROM ACCOUNTs LEFT JOIN ACCOUNTYPEs ON ACCOUNTs.Act_id = ACCOUNTYPEs.Act_id WHERE ACCOUNTYPEs.Name='DRIVER' ORDER BY ACCOUNTs.Name";
            //textBox2.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, "", 0);
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            string strCombo = "SELECT [name] from DeliveryPoints";
            string st = e.KeyChar.ToString();
            if (textBox3.Text != "")
            {
                st = textBox3.Text;
            }
            textBox3.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, st, 0);
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            string strCombo = "SELECT Name FROM DeliveryPoints WHERE     (Name NOT IN    (SELECT     DeliveryPoints_1.Name        FROM          dbo.Location LEFT OUTER JOIN     dbo.DeliveryPoints AS DeliveryPoints_1 ON dbo.Location.Dp_id = DeliveryPoints_1.DPId  WHERE      (dbo.Location.LocationId <> '"+Database.LocationId+"'))) ORDER BY Name";
            string st = e.KeyChar.ToString();
            if (textBox4.Text != "")
            {
                st = textBox4.Text;
            }
            textBox4.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, st, 0);
        }

        private void textBox18_KeyPress(object sender, KeyPressEventArgs e)
        {
            string strCombo = "select nick_name from Location order by nick_name";
            string st = e.KeyChar.ToString();
            if (textBox18.Text != "")
            {
                st = textBox18.Text;
            }
            textBox18.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, st, 0);
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            string strCombo = "select name from ACCOUNTs where  act_id=40 or act_id=41 order by name";
            string st = e.KeyChar.ToString();
            if (textBox1.Text != "")
            {
                st = textBox1.Text;
            }
            textBox1.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, st, 0);
        }
        private void SetVno()
        {
            int numtype = funs.Select_NumType(vtid);

            if (numtype == 3 && vno != 0 && vid != "0")
            {
                DateTime dt1 = dateTimePicker1.Value;
                DateTime dt2 = DateTime.Parse(Database.GetScalarDate("select vdate from voucherinfos where vi_id='" + vid + "'"));

                if (dt1 != dt2)
                {
                    vno = funs.GenerateVno(vtid, dateTimePicker1.Value.ToString("dd-MMM-yyyy"), vid);
                    label1.Text = vno.ToString();
                }
                return;
            }
            if (vtid == 0 || (vno != 0 && vid != "0"))
            {
                return;
            }

            vno = funs.GenerateVno(vtid, dateTimePicker1.Value.ToString("dd-MMM-yyyy"), vid);
            label1.Text = vno.ToString();
        }


        public void LoadData(string vi_id, string frmcaption)
        {

            foreach (DataGridViewColumn column in ansGridView1.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            foreach (DataGridViewColumn column in ansGridView5.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }


            dateTimePicker1.Select();
            this.Text = "Challan Voucher";
            gStr = vi_id.ToString();
            vid = vi_id;
            vtid = funs.Select_vt_id("Challan");

            DataTable dtfill = new DataTable();

        
          //  string str = "SELECT Stocks.GR_id AS Vi_id, CONVERT(nvarchar, VOUCHERINFOs.Vdate, 106) AS Booking_date, VOUCHERINFOs.Invoiceno AS GRno, ACCOUNTs_1.name AS Consigner, ACCOUNTs.name AS Consignee,   DeliveryPoints.Name AS Source, DeliveryPoints_1.Name AS Destination, VOUCHERINFOs.DeliveryType, VOUCHERINFOs.PaymentMode AS GR_Type,  VOUCHERINFOs.Transport1 AS Private, VOUCHERINFOs.Transport5 AS Remark, SUM(Voucherdets.Quantity) AS Total_quantity,  SUM(Voucherdets.weight) AS Total_weight, VOUCHERINFOs.Totalamount as total_amount, SUM(Voucherdets.Rate_am) AS Freight, SUM(Voucherdets.exp8amt)   AS door_delivery, CASE WHEN VOUCHERINFOs.PaymentMode = 'FOC' THEN VOUCHERINFOs.Totalamount ELSE 0 END AS total_foc,  CASE WHEN VOUCHERINFOs.PaymentMode = 'Paid' THEN VOUCHERINFOs.Totalamount ELSE 0 END AS total_paid,  CASE WHEN VOUCHERINFOs.PaymentMode = 'To Pay' THEN VOUCHERINFOs.Totalamount ELSE 0 END AS total_pay,   CASE WHEN VOUCHERINFOs.PaymentMode = 'T.B.B.' THEN VOUCHERINFOs.Totalamount ELSE 0 END AS total_Billed,Stocks.Step FROM DeliveryPoints RIGHT OUTER JOIN VOUCHERINFOs LEFT OUTER JOIN  Voucherdets ON VOUCHERINFOs.Vi_id = Voucherdets.Vi_id ON DeliveryPoints.DPId = VOUCHERINFOs.Consigner_id LEFT OUTER JOIN ACCOUNTs AS ACCOUNTs_1 ON VOUCHERINFOs.Ac_id = ACCOUNTs_1.ac_id LEFT OUTER JOIN ACCOUNTs ON VOUCHERINFOs.Ac_id2 = ACCOUNTs.ac_id LEFT OUTER JOIN  DeliveryPoints AS DeliveryPoints_1 ON VOUCHERINFOs.SId = DeliveryPoints_1.DPId RIGHT OUTER JOIN Stocks ON VOUCHERINFOs.Vi_id = Stocks.GR_id WHERE (Stocks.Godown_id = '" + Database.LocationId + "') AND (dbo.VOUCHERINFOs.SId NOT IN  (SELECT     Dp_id FROM          dbo.Location)) GROUP BY Stocks.GR_id, VOUCHERINFOs.Vdate, VOUCHERINFOs.Invoiceno, ACCOUNTs_1.name, ACCOUNTs.name, DeliveryPoints_1.Name,   DeliveryPoints.Name, VOUCHERINFOs.DeliveryType, VOUCHERINFOs.PaymentMode, VOUCHERINFOs.Transport1, VOUCHERINFOs.Transport5,  VOUCHERINFOs.Totalamount,Stocks.Step HAVING (SUM(Stocks.Quantity) > 0) ORDER BY VOUCHERINFOs.Vdate DESC, GRno DESC";
            //string str = "SELECT Stocks.GR_id AS Vi_id, CONVERT(nvarchar, Stocks.GRDate, 106) AS Booking_date, Stocks.GRNo AS GRno,   ACCOUNTs_1.name AS Consigner, ACCOUNTs.name AS Consignee, DeliveryPoints.Name AS Source, DeliveryPoints_1.Name AS Destination,Stocks.DeliveryType,  Stocks.GRType, Stocks.Private, Stocks.Remark, sum(Stocks.TotPkts) AS Total_quantity, sum(Stocks.TotWeight) AS Total_Weight, VOUCHERINFOs.Totalamount AS total_amount,  sum( Stocks.Freight) AS Freight, sum(Stocks.FOC) as total_foc, sum(Stocks.Paid) as total_paid , sum(Stocks.ToPay) as  total_pay, sum(Stocks.TBB) as total_Billed, Stocks.Step, sum(Stocks.GRCharge) as GRCharge, sum(Stocks.OthCharge) as Othcharge,  Stocks.ItemName, Stocks.Packing, Stocks.DeliveryType FROM ACCOUNTs AS ACCOUNTs_1 RIGHT OUTER JOIN  DeliveryPoints AS DeliveryPoints_1 RIGHT OUTER JOIN  DeliveryPoints RIGHT OUTER JOIN  VOUCHERINFOs INNER JOIN  Stocks ON VOUCHERINFOs.Vi_id = Stocks.GR_id ON DeliveryPoints.DPId = Stocks.Source_id ON   DeliveryPoints_1.DPId = Stocks.Destination_id LEFT OUTER JOIN  ACCOUNTs ON Stocks.Consignee_id = ACCOUNTs.ac_id ON ACCOUNTs_1.ac_id = Stocks.Consigner_id WHERE ( Stocks.Godown_id = '" + Database.LocationId + "') AND ( Stocks.Quantity > 0) AND ( Stocks.Destination_id NOT IN  (SELECT Dp_id FROM Location)) GROUP BY Stocks.GR_id, CONVERT(nvarchar, Stocks.GRDate, 106), Stocks.GRNo, ACCOUNTs_1.name, ACCOUNTs.name, DeliveryPoints.Name,   DeliveryPoints_1.Name, Stocks.GRType, Stocks.Private, Stocks.Remark, VOUCHERINFOs.Totalamount,  Stocks.Step,   Stocks.ItemName, Stocks.Packing, Stocks.GRDate, VOUCHERINFOs.Grno, Stocks.DeliveryType ORDER BY Stocks.GRDate DESC, VOUCHERINFOs.Grno DESC ";
            //string str = "SELECT  Stocks.GR_id AS Vi_id, CONVERT(nvarchar, Stocks.GRDate, 106) AS Booking_date, Stocks.GRNo AS GRno,   ACCOUNTs_1.name AS Consigner, ACCOUNTs.name AS Consignee, DeliveryPoints.Name AS Source, DeliveryPoints_1.Name AS Destination,   Stocks.DeliveryType, Stocks.GRType, Stocks.Private, Stocks.Remark, Stocks.TotPkts AS Total_quantity, Stocks.TotWeight AS Total_Weight,  VOUCHERINFOs.Totalamount AS total_amount, Stocks.Freight, Stocks.FOC AS total_foc, Stocks.Paid AS total_paid, Stocks.ToPay AS total_pay,   Stocks.TBB AS total_Billed, Stocks.Step, Stocks.GRCharge, Stocks.OthCharge AS Othcharge, Stocks.ItemName, Stocks.Packing,  Stocks.DeliveryType AS DeliveryType FROM ACCOUNTs AS ACCOUNTs_1 RIGHT OUTER JOIN DeliveryPoints AS DeliveryPoints_1 RIGHT OUTER JOIN  DeliveryPoints RIGHT OUTER JOIN VOUCHERINFOs INNER JOIN  Stocks ON VOUCHERINFOs.Vi_id = Stocks.GR_id ON DeliveryPoints.DPId = Stocks.Source_id ON   DeliveryPoints_1.DPId = Stocks.Destination_id LEFT OUTER JOIN ACCOUNTs ON Stocks.Consignee_id = ACCOUNTs.ac_id ON ACCOUNTs_1.ac_id = Stocks.Consigner_id WHERE (Stocks.Godown_id = '" + Database.LocationId + "')  AND ( Stocks.Destination_id NOT IN  (SELECT Dp_id FROM Location)) GROUP BY Stocks.GR_id, CONVERT(nvarchar, Stocks.GRDate, 106), Stocks.GRNo, ACCOUNTs_1.name, ACCOUNTs.name, DeliveryPoints.Name,  DeliveryPoints_1.Name, Stocks.DeliveryType, Stocks.GRType, Stocks.Private, Stocks.Remark, Stocks.TotPkts, Stocks.TotWeight,   VOUCHERINFOs.Totalamount, Stocks.Freight, Stocks.FOC, Stocks.Paid, Stocks.ToPay, Stocks.TBB, Stocks.Step, Stocks.GRCharge,   Stocks.OthCharge, Stocks.ItemName, Stocks.Packing, Stocks.GRDate, VOUCHERINFOs.Grno HAVING (SUM(Stocks.Quantity) > 0) ORDER BY Stocks.GRDate DESC, VOUCHERINFOs.Grno DESC";


            string str = "SELECT Stocks.GR_id AS Vi_id, CONVERT(nvarchar, Stocks.GRDate, 106) AS Booking_date, Stocks.GRNo AS GRno,   ACCOUNTs_1.name AS Consigner, ACCOUNTs.name AS Consignee, DeliveryPoints.Name AS Source, DeliveryPoints_1.Name AS Destination,   Stocks.DeliveryType, Stocks.GRType, Stocks.Private, Stocks.Remark, Stocks.TotPkts AS Total_quantity,   Stocks.FOC + Stocks.Paid + Stocks.ToPay + Stocks.TBB AS total_amount, Stocks.ActWeight AS ActWeight ,Stocks.TotWeight AS Total_Weight, Stocks.Freight,   Stocks.FOC AS total_foc, Stocks.Paid AS total_paid, Stocks.ToPay AS total_pay, Stocks.TBB AS total_Billed, Stocks.Step, Stocks.GRCharge,   Stocks.OthCharge AS Othcharge, Stocks.ItemName, Stocks.Packing, Stocks.DeliveryType AS Deliverytype FROM ACCOUNTs AS ACCOUNTs_1 RIGHT OUTER JOIN  DeliveryPoints AS DeliveryPoints_1 RIGHT OUTER JOIN  DeliveryPoints RIGHT OUTER JOIN  Stocks ON DeliveryPoints.DPId = Stocks.Source_id ON DeliveryPoints_1.DPId = Stocks.Destination_id LEFT OUTER JOIN  ACCOUNTs ON Stocks.Consignee_id = ACCOUNTs.ac_id ON ACCOUNTs_1.ac_id = Stocks.Consigner_id WHERE (Stocks.Godown_id = '" + Database.LocationId + "') AND ( Stocks.Destination_id NOT IN  (SELECT Dp_id FROM Location))  GROUP BY Stocks.GR_id, CONVERT(nvarchar, Stocks.GRDate, 106), Stocks.GRNo, ACCOUNTs_1.name, ACCOUNTs.name, DeliveryPoints.Name,   DeliveryPoints_1.Name, Stocks.DeliveryType, Stocks.GRType, Stocks.Private, Stocks.Remark, Stocks.TotPkts, Stocks.TotWeight, Stocks.ActWeight, Stocks.Freight, Stocks.FOC, Stocks.Paid, Stocks.ToPay, Stocks.TBB, Stocks.Step, Stocks.GRCharge, Stocks.OthCharge,  Stocks.ItemName, Stocks.Packing, Stocks.GRDate HAVING (SUM(Stocks.Quantity) > 0) ORDER BY Stocks.GRDate DESC";
            Database.GetSqlData(str, dtfill);


            for (int m = 0; m < dtfill.Rows.Count; m++)
            {
                ansGridView1.Rows.Add();
                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["vi_id1"].Value = dtfill.Rows[m]["vi_id"].ToString();
                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["step"].Value = dtfill.Rows[m]["step"].ToString();

                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["booking_date1"].Value = dtfill.Rows[m]["Booking_date"].ToString();
                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["grno1"].Value = dtfill.Rows[m]["GRno"].ToString();
                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["consigner1"].Value = dtfill.Rows[m]["Consigner"].ToString();
                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["consignee1"].Value = dtfill.Rows[m]["Consignee"].ToString();
                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["source1"].Value = dtfill.Rows[m]["source"].ToString();
                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["destination1"].Value = dtfill.Rows[m]["destination"].ToString();
                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["delivery1"].Value = dtfill.Rows[m]["DeliveryType"].ToString();
                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["grtype1"].Value = dtfill.Rows[m]["GRtype"].ToString();
                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["private1"].Value = dtfill.Rows[m]["Private"].ToString();
                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["remark1"].Value = dtfill.Rows[m]["Remark"].ToString();
                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["qty1"].Value = dtfill.Rows[m]["Total_quantity"].ToString();
                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["wt1"].Value = dtfill.Rows[m]["Total_weight"].ToString();
                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["actwt1"].Value = dtfill.Rows[m]["ActWeight"].ToString();

                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["amt1"].Value = dtfill.Rows[m]["total_amount"].ToString();
                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["freight1"].Value = dtfill.Rows[m]["Freight"].ToString();
           //     ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["dd1"].Value = dtfill.Rows[m]["Door_delivery"].ToString();
                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["pay1"].Value = dtfill.Rows[m]["total_pay"].ToString();
                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["foc1"].Value = dtfill.Rows[m]["total_foc"].ToString();
                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["paid1"].Value = dtfill.Rows[m]["total_paid"].ToString();
                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["billed1"].Value = dtfill.Rows[m]["total_billed"].ToString();
                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["itemname1"].Value = dtfill.Rows[m]["itemname"].ToString();
                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["packing1"].Value = dtfill.Rows[m]["packing"].ToString();
                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["Grcharge1"].Value = dtfill.Rows[m]["Grcharge"].ToString();
                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["Othcharge1"].Value = dtfill.Rows[m]["othcharge"].ToString();
            }


            if (vid == "0")
            {
                SetVno();
            }

            dtVoucherinfo = new DataTable("VOUCHERINFOs");
            Database.GetSqlData("select * from VOUCHERINFOs where vi_id='" + vid + "'", dtVoucherinfo);

            if (dtVoucherinfo.Rows.Count == 0)
            {
                if (dtVoucherinfo.Rows.Count == 0)
                {
                    dtVoucherinfo.Rows.Add();
                }
                dateTimePicker1.Value = Database.ldate;
                txtTruckNo.Text = "";
                textBox2.Text = "";
                textBox1.Text = "";
                string curlocation = Database.GetScalarText("Select DP_id from location where locationid='"+Database.LocationId+"'");

                textBox3.Text = funs.Select_dp_nm(curlocation);
                textBox3.Enabled = false;
                textBox4.Text = "";
                textBox5.Text = "0";
                textBox6.Text = "0";
                textBox12.Text = "0";
                textBox13.Text = "0";
                textBox14.Text = "0";
                textBox15.Text = "0";
                textBox18.Text = "";
                textBox17.Text = "0";
                textBox9.Text = "0";
                textBox10.Text = "0";
                label1.Text = vno.ToString();
                ansGridView5.Rows.Clear();
                iscancel = false;
                label28.Visible = false;
            }
            else
            {
                textBox3.Enabled = false;
                vno = int.Parse(dtVoucherinfo.Rows[0]["Vnumber"].ToString());
                label1.Text = vno.ToString();
                dateTimePicker1.Value = DateTime.Parse(dtVoucherinfo.Rows[0]["Vdate"].ToString());
                create_date = DateTime.Parse(dtVoucherinfo.Rows[0]["create_date"].ToString());
                txtTruckNo.Text = funs.Select_gaddi_nm(dtVoucherinfo.Rows[0]["Gaddi_id"].ToString());
                textBox1.Text = funs.Select_ac_nm(dtVoucherinfo.Rows[0]["Ac_id"].ToString());
                textBox2.Text = funs.Select_ac_nm(dtVoucherinfo.Rows[0]["Driver_name"].ToString());

                label1.Text = dtVoucherinfo.Rows[0]["Vnumber"].ToString();

                textBox18.Text = funs.Select_location_name(dtVoucherinfo.Rows[0]["unloadingpoint_id"].ToString());

                textBox3.Text = funs.Select_dp_nm(dtVoucherinfo.Rows[0]["Consigner_Id"].ToString());
                textBox4.Text = funs.Select_dp_nm(dtVoucherinfo.Rows[0]["SId"].ToString());
                textBox5.Text = dtVoucherinfo.Rows[0]["Transport2"].ToString();
                textBox6.Text = dtVoucherinfo.Rows[0]["Transport3"].ToString();
                textBox12.Text = dtVoucherinfo.Rows[0]["Transport4"].ToString();
                textBox13.Text = dtVoucherinfo.Rows[0]["Transport5"].ToString();
                textBox14.Text = dtVoucherinfo.Rows[0]["Transport6"].ToString();
                textBox15.Text = dtVoucherinfo.Rows[0]["DeliveryAt"].ToString();
                textBox17.Text = dtVoucherinfo.Rows[0]["Grno"].ToString();
                textBox9.Text = dtVoucherinfo.Rows[0]["DD"].ToString();
                textBox10.Text = dtVoucherinfo.Rows[0]["DR"].ToString();
                if (dtVoucherinfo.Rows[0]["Iscancel"].ToString() == "")
                {
                    dtVoucherinfo.Rows[0]["Iscancel"] = false;
                }
                if (bool.Parse(dtVoucherinfo.Rows[0]["Iscancel"].ToString()) == true)
                {
                    label28.Visible = true;
                    label28.Text = "Cancelled";
                    iscancel = bool.Parse(dtVoucherinfo.Rows[0]["Iscancel"].ToString());
                }

                ansGridView5.Rows.Clear();
                //dtVoucherDet = new DataTable();
                //Database.GetSqlData("select * from VOUCHERDETs where Vi_id='" + vid + "'", dtVoucherDet);
                
                //if (dtVoucherDet.Rows.Count > 0)
                //{
                    //for (int i = 0; i < dtVoucherDet.Rows.Count; i++)
                    //{
                        DataTable dt = new DataTable();
                       
              //  Database.GetSqlData("SELECT  VOUCHERINFOs.Vi_id, CONVERT(nvarchar, VOUCHERINFOs.Vdate, 106) AS Booking_date, VOUCHERINFOs.Invoiceno AS GRno,  ACCOUNTs.name AS Consigner, ACCOUNTs_1.name AS Consignee, DeliveryPoints_1.Name AS source, DeliveryPoints.Name AS destination,   VOUCHERINFOs.DeliveryType, VOUCHERINFOs.PaymentMode AS GR_type, VOUCHERINFOs.Transport1 AS Private,  VOUCHERINFOs.Transport5 AS Remark, SUM( Voucherdets.Quantity) AS Total_quantity, SUM( Voucherdets.weight) AS Total_weight,  VOUCHERINFOs.Totalamount AS total_amount, SUM( Voucherdets.Rate_am) AS Freight, Voucherdets.exp8amt AS door_delivery,   CASE WHEN VOUCHERINFOs.PaymentMode = 'FOC' THEN VOUCHERINFOs.Totalamount ELSE 0 END AS total_foc,  CASE WHEN VOUCHERINFOs.PaymentMode = 'Paid' THEN VOUCHERINFOs.Totalamount ELSE 0 END AS total_paid,  CASE WHEN VOUCHERINFOs.PaymentMode = 'To Pay' THEN VOUCHERINFOs.Totalamount ELSE 0 END AS total_pay,  CASE WHEN VOUCHERINFOs.PaymentMode = 'T.B.B.' THEN VOUCHERINFOs.Totalamount ELSE 0 END AS total_Billed FROM Voucherdets RIGHT OUTER JOIN  VOUCHERINFOs ON Voucherdets.Vi_id = VOUCHERINFOs.Vi_id LEFT OUTER JOIN  VOUCHERTYPEs ON VOUCHERINFOs.Vt_id = VOUCHERTYPEs.Vt_id LEFT OUTER JOIN  DeliveryPoints ON VOUCHERINFOs.SId = DeliveryPoints.DPId LEFT OUTER JOIN  ACCOUNTs ON VOUCHERINFOs.Ac_id = ACCOUNTs.ac_id LEFT OUTER JOIN ACCOUNTs AS ACCOUNTs_1 ON VOUCHERINFOs.Ac_id2 = ACCOUNTs_1.ac_id LEFT OUTER JOIN  DeliveryPoints AS DeliveryPoints_1 ON VOUCHERINFOs.Consigner_id = DeliveryPoints_1.DPId WHERE ( VOUCHERTYPEs.Type = 'Booking') GROUP BY VOUCHERINFOs.Vi_id, VOUCHERINFOs.Vdate, VOUCHERINFOs.Invoiceno, ACCOUNTs.name, ACCOUNTs_1.name, DeliveryPoints_1.Name,   DeliveryPoints.Name, VOUCHERINFOs.DeliveryType, VOUCHERINFOs.PaymentMode, VOUCHERINFOs.Transport1, VOUCHERINFOs.Transport5,  VOUCHERINFOs.Totalamount, Voucherdets.exp8amt HAVING ( VOUCHERINFOs.Vi_id = '" + dtVoucherDet.Rows[i]["Booking_id"].ToString() + "') ORDER BY Booking_date DESC, GRno DESC",dt);
               // Database.GetSqlData("SELECT  VOUCHERINFOs.Vi_id, CONVERT(nvarchar, Stocks.GRDate, 106) AS Booking_date, Stocks.GRNo AS GRno,    ACCOUNTs.name AS Consigner, ACCOUNTs_1.name AS Consignee, DeliveryPoints_1.Name AS Destination, DeliveryPoints.Name AS Source,    Stocks.DeliveryType, Stocks.GRType AS GR_type, Stocks.Private, Stocks.Remark, SUM(Stocks.TotPkts) AS Total_quantity,    SUM(Stocks.TotWeight) AS total_weight, VOUCHERINFOs.Totalamount AS total_amount, SUM(Stocks.Freight) AS Freight, SUM(Stocks.FOC)    AS total_foc, SUM(Stocks.Paid) AS total_paid, SUM(Stocks.ToPay) AS total_pay, SUM(Stocks.TBB) AS total_billed, SUM(Stocks.GRCharge)    AS GRCharge, SUM(Stocks.OthCharge) AS OthCharge, Stocks.ItemName, Stocks.Packing FROM Stocks RIGHT OUTER JOIN   VOUCHERINFOs ON Stocks.vid = VOUCHERINFOs.Vi_id LEFT OUTER JOIN   Voucherdets ON VOUCHERINFOs.Vi_id = Voucherdets.Vi_id LEFT OUTER JOIN   VOUCHERTYPEs ON VOUCHERINFOs.Vt_id = VOUCHERTYPEs.Vt_id LEFT OUTER JOIN  DeliveryPoints ON VOUCHERINFOs.SId = DeliveryPoints.DPId LEFT OUTER JOIN  ACCOUNTs ON VOUCHERINFOs.Ac_id = ACCOUNTs.ac_id LEFT OUTER JOIN  ACCOUNTs AS ACCOUNTs_1 ON VOUCHERINFOs.Ac_id2 = ACCOUNTs_1.ac_id LEFT OUTER JOIN  DeliveryPoints AS DeliveryPoints_1 ON VOUCHERINFOs.Consigner_id = DeliveryPoints_1.DPId WHERE (VOUCHERTYPEs.Type = 'Booking') GROUP BY VOUCHERINFOs.Vi_id, Stocks.GRDate, Stocks.GRNo, ACCOUNTs.name, ACCOUNTs_1.name, DeliveryPoints_1.Name, DeliveryPoints.Name,   Stocks.DeliveryType, Stocks.GRType, Stocks.Private, Stocks.Remark, VOUCHERINFOs.Totalamount, Stocks.ItemName,    Stocks.Packing HAVING  (VOUCHERINFOs.Vi_id = '" + dtVoucherDet.Rows[i]["Booking_id"].ToString() + "') ORDER BY Booking_date DESC, GRno DESC", dt);
                        Database.GetSqlData("SELECT CONVERT(nvarchar, Stocks.GRDate, 106) AS Booking_date, Stocks.GRNo AS GRno, Stocks.Step, ACCOUNTs.name AS Consigner,  ACCOUNTs_1.name AS Consignee, DeliveryPoints_1.Name AS Source, DeliveryPoints.Name AS Destination, Stocks.DeliveryType,   Stocks.GRType AS GR_type, Stocks.Private, Stocks.Remark, SUM(Stocks.TotPkts) AS Total_quantity, SUM(Stocks.ActWeight) AS Actweight,SUM(Stocks.TotWeight) AS total_weight  , SUM(Stocks.Freight) AS Freight, SUM(Stocks.FOC) AS total_foc, SUM(Stocks.Paid) AS total_paid, SUM(Stocks.ToPay) AS total_pay,   SUM(Stocks.TBB) AS total_billed, SUM(Stocks.GRCharge) AS GRCharge, SUM(Stocks.OthCharge) AS OthCharge, Stocks.ItemName,   Stocks.Packing, SUM(Stocks.TBB + Stocks.ToPay + Stocks.Paid + Stocks.FOC) AS Total_amount, Stocks.GR_id AS Vi_id FROM Stocks LEFT OUTER JOIN  DeliveryPoints AS DeliveryPoints_1 ON Stocks.Source_id = DeliveryPoints_1.DPId LEFT OUTER JOIN  DeliveryPoints ON Stocks.Destination_id = DeliveryPoints.DPId LEFT OUTER JOIN  ACCOUNTs AS ACCOUNTs_1 ON Stocks.Consignee_id = ACCOUNTs_1.ac_id LEFT OUTER JOIN  ACCOUNTs ON Stocks.Consigner_id = ACCOUNTs.ac_id WHERE (Stocks.vid = '" + vid + "') GROUP BY Stocks.GRDate, Stocks.Step, Stocks.GRNo, ACCOUNTs.name, ACCOUNTs_1.name, DeliveryPoints_1.Name, DeliveryPoints.Name,   Stocks.DeliveryType, Stocks.GRType, Stocks.Private, Stocks.Remark, Stocks.ItemName, Stocks.Packing, Stocks.GR_id ORDER BY Stocks.GRDate DESC, GRno DESC", dt);

                  for (int i = 0; i < dt.Rows.Count; i++)
                  {
                      ansGridView5.Rows.Add();
                      ansGridView5.Rows[ansGridView5.Rows.Count - 1].Cells["vi_id5"].Value = dt.Rows[i]["Vi_id"].ToString();
                      ansGridView5.Rows[ansGridView5.Rows.Count - 1].Cells["step2"].Value = dt.Rows[i]["step"].ToString();
                      ansGridView5.Rows[ansGridView5.Rows.Count - 1].Cells["booking_date5"].Value = dt.Rows[i]["Booking_date"].ToString();
                      ansGridView5.Rows[ansGridView5.Rows.Count - 1].Cells["grno5"].Value = dt.Rows[i]["GRno"].ToString();
                      ansGridView5.Rows[ansGridView5.Rows.Count - 1].Cells["consigner5"].Value = dt.Rows[i]["Consigner"].ToString();
                      ansGridView5.Rows[ansGridView5.Rows.Count - 1].Cells["consignee5"].Value = dt.Rows[i]["Consignee"].ToString();
                      ansGridView5.Rows[ansGridView5.Rows.Count - 1].Cells["source5"].Value = dt.Rows[i]["source"].ToString();
                      ansGridView5.Rows[ansGridView5.Rows.Count - 1].Cells["destination5"].Value = dt.Rows[i]["destination"].ToString();
                      ansGridView5.Rows[ansGridView5.Rows.Count - 1].Cells["delivery5"].Value = dt.Rows[i]["DeliveryType"].ToString();
                      ansGridView5.Rows[ansGridView5.Rows.Count - 1].Cells["grtype5"].Value = dt.Rows[i]["GR_type"].ToString();
                      ansGridView5.Rows[ansGridView5.Rows.Count - 1].Cells["private5"].Value = dt.Rows[i]["Private"].ToString();
                      ansGridView5.Rows[ansGridView5.Rows.Count - 1].Cells["remark5"].Value = dt.Rows[i]["Remark"].ToString();
                      ansGridView5.Rows[ansGridView5.Rows.Count - 1].Cells["qty5"].Value = dt.Rows[i]["Total_quantity"].ToString();

                      ansGridView5.Rows[ansGridView5.Rows.Count - 1].Cells["wt5"].Value = dt.Rows[i]["total_weight"].ToString();

                      ansGridView5.Rows[ansGridView5.Rows.Count - 1].Cells["actwt5"].Value = dt.Rows[i]["actweight"].ToString();
                      ansGridView5.Rows[ansGridView5.Rows.Count - 1].Cells["amt5"].Value = dt.Rows[i]["Total_amount"].ToString();
                      ansGridView5.Rows[ansGridView5.Rows.Count - 1].Cells["freight5"].Value = dt.Rows[i]["Freight"].ToString();
                      // ansGridView5.Rows[ansGridView5.Rows.Count - 1].Cells["dd5"].Value = dt.Rows[0]["Door_delivery"].ToString();
                      ansGridView5.Rows[ansGridView5.Rows.Count - 1].Cells["foc5"].Value = dt.Rows[i]["total_foc"].ToString();
                      ansGridView5.Rows[ansGridView5.Rows.Count - 1].Cells["pay5"].Value = dt.Rows[i]["total_pay"].ToString();
                      ansGridView5.Rows[ansGridView5.Rows.Count - 1].Cells["paid5"].Value = dt.Rows[i]["total_paid"].ToString();
                      ansGridView5.Rows[ansGridView5.Rows.Count - 1].Cells["billed5"].Value = dt.Rows[i]["total_billed"].ToString();

                      ansGridView5.Rows[ansGridView5.Rows.Count - 1].Cells["itemname5"].Value = dt.Rows[i]["itemname"].ToString();
                      ansGridView5.Rows[ansGridView5.Rows.Count - 1].Cells["packing5"].Value = dt.Rows[i]["packing"].ToString();
                      ansGridView5.Rows[ansGridView5.Rows.Count - 1].Cells["grcharge5"].Value = dt.Rows[i]["grcharge"].ToString();
                      ansGridView5.Rows[ansGridView5.Rows.Count - 1].Cells["othcharge5"].Value = dt.Rows[i]["othcharge"].ToString();




                  }


                      //  }
                      weightCalc();
                 
             //   }
            }



            if (gresave == true)
            {
                object sender = new object();
                EventArgs e = new EventArgs();
                btn_Click(sender, e);
            }
        }



        private void weightCalc()
        {
            double TotalWeight = 0;
            double Totalqty = 0;
            double Totalamt = 0;
            double Totalfoc = 0;
            double Totalpay = 0;
            double Totalbilled = 0;
            double Totalpaid = 0;

            for (int i = 0; i < ansGridView5.Rows.Count; i++)
            {
                TotalWeight += double.Parse(ansGridView5.Rows[i].Cells["wt5"].Value.ToString());
                Totalqty += double.Parse(ansGridView5.Rows[i].Cells["qty5"].Value.ToString());
                Totalamt += double.Parse(ansGridView5.Rows[i].Cells["amt5"].Value.ToString());
                Totalfoc += double.Parse(ansGridView5.Rows[i].Cells["foc5"].Value.ToString());
                Totalpay += double.Parse(ansGridView5.Rows[i].Cells["pay5"].Value.ToString());
                Totalbilled += double.Parse(ansGridView5.Rows[i].Cells["billed5"].Value.ToString());
                Totalpaid += double.Parse(ansGridView5.Rows[i].Cells["paid5"].Value.ToString());
            }

            textBox21.Text = ansGridView5.Rows.Count.ToString();
            textBox7.Text = Totalqty.ToString();
            textBox8.Text = Totalamt.ToString();
            txtTotalWeight.Text = TotalWeight.ToString();
            textBox11.Text = Totalfoc.ToString();
            textBox19.Text = Totalpay.ToString();
            textBox16.Text = Totalbilled.ToString();
            textBox20.Text = Totalpaid.ToString();
        }

        private void save()
        {
            if (vid == "0")
            {
                DataTable dtCount = new DataTable();
                Database.GetSqlData("select count(*) from VOUCHERINFOs where locationid='" + Database.LocationId + "'", dtCount);

                if (int.Parse(dtCount.Rows[0][0].ToString()) == 0)
                {
                    dtVoucherinfo.Rows[0]["Vi_id"] = Database.LocationId + "1";
                    dtVoucherinfo.Rows[0]["Nid"] = 1;
                    dtVoucherinfo.Rows[0]["LocationId"] = Database.LocationId;
                    Prelocationid = Database.LocationId;
                }
                else
                {
                    DataTable dtid = new DataTable();
                    Database.GetSqlData("select max(Nid) as Nid from VOUCHERINFOs where locationid='" + Database.LocationId + "'", dtid);
                    int Nid = int.Parse(dtid.Rows[0][0].ToString());
                    dtVoucherinfo.Rows[0]["Vi_id"] = Database.LocationId + (Nid + 1);
                    dtVoucherinfo.Rows[0]["Nid"] = (Nid + 1);
                    dtVoucherinfo.Rows[0]["LocationId"] = Database.LocationId;
                    Prelocationid = Database.LocationId;
                }
            }
            SetVno();

            if (vno == 0)
            {
                vno = funs.GenerateVno(vtid, dateTimePicker1.Value.ToString("dd-MMM-yyyy"), vid);
            }

            string prefix = "";
            string postfix = "";
            int padding = 0;
            string invoiceno = vno.ToString();

            prefix = Database.GetScalarText("Select prefix from Location where LocationId='" + Database.LocationId + "'");


           // prefix = Database.GetScalarText("Select prefix from Vouchertypes where vt_id=" + vtid);
            //postfix = Database.GetScalarText("Select postfix from Vouchertypes where vt_id=" + vtid);
            //padding = Database.GetScalarInt("Select padding from Vouchertypes where vt_id=" + vtid);

            dtVoucherinfo.Rows[0]["Invoiceno"] = prefix + invoiceno.PadLeft(padding, '0') + postfix;
            dtVoucherinfo.Rows[0]["Vdate"] = dateTimePicker1.Value.Date;
            dtVoucherinfo.Rows[0]["Vnumber"] = label1.Text;
            dtVoucherinfo.Rows[0]["RoffChanged"] = RoffChanged;
            dtVoucherinfo.Rows[0]["Gaddi_id"] = funs.Select_gaddi_id(txtTruckNo.Text);
            dtVoucherinfo.Rows[0]["Driver_name"] = funs.Select_ac_id(textBox2.Text);
            dtVoucherinfo.Rows[0]["Tdtype"] = false;
            dtVoucherinfo.Rows[0]["Vt_id"] = 91;

            dtVoucherinfo.Rows[0]["Ac_id"] = funs.Select_ac_id(textBox1.Text);
            dtVoucherinfo.Rows[0]["Narr"] = "Challan";
            dtVoucherinfo.Rows[0]["SId"] = funs.Select_dp_id(textBox4.Text);
            dtVoucherinfo.Rows[0]["Consigner_id"] = funs.Select_dp_id(textBox3.Text);

            dtVoucherinfo.Rows[0]["unloadingpoint_id"] = funs.Select_locationId(textBox18.Text);
            dtVoucherinfo.Rows[0]["Grno"] = textBox17.Text;
            dtVoucherinfo.Rows[0]["Transport2"] = textBox5.Text;
            dtVoucherinfo.Rows[0]["Transport5"] = textBox13.Text;
            dtVoucherinfo.Rows[0]["Transport6"] = textBox14.Text;
            dtVoucherinfo.Rows[0]["Transport3"] = textBox6.Text;
            dtVoucherinfo.Rows[0]["DeliveryAt"] = textBox15.Text;
            dtVoucherinfo.Rows[0]["DD"] = double.Parse(textBox9.Text);
            dtVoucherinfo.Rows[0]["Transport4"] = textBox12.Text;
            dtVoucherinfo.Rows[0]["DR"] = double.Parse(textBox10.Text);

            dtVoucherinfo.Rows[0]["totalamount"] = double.Parse(textBox8.Text);
            dtVoucherinfo.Rows[0]["roff"] = 0;
            dtVoucherinfo.Rows[0]["Transport1"] = "";

            dtVoucherinfo.Rows[0]["PaymentMode"] = "";
            dtVoucherinfo.Rows[0]["TaxChanged"] = false;
            dtVoucherinfo.Rows[0]["formC"] = false;
            dtVoucherinfo.Rows[0]["DeliveryType"] = "";
            dtVoucherinfo.Rows[0]["As_Per"] = "";
            dtVoucherinfo.Rows[0]["Delivery_adrs"] = "";
            dtVoucherinfo.Rows[0]["iscancel"] =iscancel ;
            if (vid == "0")
            {
                dtVoucherinfo.Rows[0]["create_date"] = create_date;
                dtVoucherinfo.Rows[0]["CreTime"] = System.DateTime.Now.ToString("HH:mm:ss");
                dtVoucherinfo.Rows[0]["user_id"] = Database.user_id;
            }

            if (vid != "0")
            {
                dtVoucherinfo.Rows[0]["modifyby_id"] = Database.user_id;
            }
            dtVoucherinfo.Rows[0]["modify_date"] = System.DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
            dtVoucherinfo.Rows[0]["ModTime"] = System.DateTime.Now.ToString("HH:mm:ss");
            Database.SaveData(dtVoucherinfo);

            if (vid == "0")
            {
                vid = dtVoucherinfo.Rows[0]["Vi_id"].ToString();
            }

            dtVoucherDet = new DataTable("Voucherdets");
            Database.GetSqlData("Select * from Voucherdets where Vi_id='" + vid + "'", dtVoucherDet);
            for (int j = 0; j < dtVoucherDet.Rows.Count; j++)
            {
                dtVoucherDet.Rows[j].Delete();
            }
            Database.SaveData(dtVoucherDet);

            dtVoucherDet = new DataTable("Voucherdets");
            Database.GetSqlData("Select * from Voucherdets where Vi_id='" + vid + "'", dtVoucherDet);
            int Nid2 = 1;
            DataTable dtidvd = new DataTable();
            Database.GetSqlData("select max(Nid) as Nid from Voucherdets where locationid='" + Database.LocationId + "'", dtidvd);





            if (dtidvd.Rows[0][0].ToString() != "")
            {
                Nid2 = int.Parse(dtidvd.Rows[0][0].ToString()) + 1;
            }


            for (int i = 0; i < ansGridView5.Rows.Count; i++)
            {
                dtVoucherDet.Rows.Add();
                dtVoucherDet.Rows[i]["Nid"] = Nid2;
                dtVoucherDet.Rows[i]["LocationId"] = Database.LocationId;
                dtVoucherDet.Rows[i]["vd_id"] = Database.LocationId + dtVoucherDet.Rows[i]["nid"].ToString();
                dtVoucherDet.Rows[i]["Vi_id"] = vid;
                dtVoucherDet.Rows[i]["Itemsr"] = i + 1;
                dtVoucherDet.Rows[i]["step"] = ansGridView5.Rows[i].Cells["step2"].Value.ToString();
                //dtVoucherDet.Rows[i]["Des_ac_id"] = "0";
                dtVoucherDet.Rows[i]["Booking_id"] = ansGridView5.Rows[i].Cells["vi_id5"].Value.ToString();
                dtVoucherDet.Rows[i]["remarkreq"] = false;
                dtVoucherDet.Rows[i]["create_date"] = create_date;
                dtVoucherDet.Rows[i]["modify_date"] = System.DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                dtVoucherDet.Rows[i]["multiplier"] = 1;
                dtVoucherDet.Rows[i]["Amount"] = 0;
                Nid2++;
            }

            Database.SaveData(dtVoucherDet);



            DataTable dtstocks = new DataTable("stocks");
            Database.GetSqlData("Select * from stocks where Vid='" + vid + "'", dtstocks);
            for (int j = 0; j < dtstocks.Rows.Count; j++)
            {
                dtstocks.Rows[j].Delete();
            }
            Database.SaveData(dtstocks);
            //  DataTable dtbookingdet = new DataTable();
            if (iscancel == false)
            {
                for (int i = 0; i < ansGridView5.Rows.Count; i++)
                {
                    string bookingid = ansGridView5.Rows[i].Cells["vi_id5"].Value.ToString();

                    dtstocks.Rows.Add();

                    dtstocks.Rows[dtstocks.Rows.Count - 1]["Vid"] = vid;
                    dtstocks.Rows[dtstocks.Rows.Count - 1]["Step"] = ansGridView5.Rows[i].Cells["step2"].Value.ToString();
                    dtstocks.Rows[dtstocks.Rows.Count - 1]["GR_id"] = bookingid;
                    dtstocks.Rows[dtstocks.Rows.Count - 1]["Quantity"] = -1;
                    //  dtstocks.Rows[dtstocks.Rows.Count - 1]["Step"] = "Step1";
                    dtstocks.Rows[dtstocks.Rows.Count - 1]["Godown_id"] = Database.LocationId;
                    string aliasname = Database.GetScalarText("Select Aliasname from vouchertypes where vt_id=" + vtid);


                    dtstocks.Rows[dtstocks.Rows.Count - 1]["Narration"] = aliasname + " To " + textBox4.Text;


                    dtstocks.Rows[dtstocks.Rows.Count - 1]["GRNo"] = ansGridView5.Rows[i].Cells["grno5"].Value.ToString();
                    dtstocks.Rows[dtstocks.Rows.Count - 1]["GRDate"] = DateTime.Parse(ansGridView5.Rows[i].Cells["booking_date5"].Value.ToString());

                    dtstocks.Rows[dtstocks.Rows.Count - 1]["Consigner_id"] = funs.Select_ac_id(ansGridView5.Rows[i].Cells["consigner5"].Value.ToString());
                    dtstocks.Rows[dtstocks.Rows.Count - 1]["Consignee_id"] = funs.Select_ac_id(ansGridView5.Rows[i].Cells["consignee5"].Value.ToString());


                    dtstocks.Rows[dtstocks.Rows.Count - 1]["Source_id"] = funs.Select_dp_id(ansGridView5.Rows[i].Cells["source5"].Value.ToString());
                    dtstocks.Rows[dtstocks.Rows.Count - 1]["Destination_id"] = funs.Select_dp_id(ansGridView5.Rows[i].Cells["destination5"].Value.ToString());

                    dtstocks.Rows[dtstocks.Rows.Count - 1]["ToPay"] = double.Parse(ansGridView5.Rows[i].Cells["pay5"].Value.ToString());
                    dtstocks.Rows[dtstocks.Rows.Count - 1]["TBB"] = double.Parse(ansGridView5.Rows[i].Cells["billed5"].Value.ToString());

                    dtstocks.Rows[dtstocks.Rows.Count - 1]["Paid"] = double.Parse(ansGridView5.Rows[i].Cells["Paid5"].Value.ToString());
                    dtstocks.Rows[dtstocks.Rows.Count - 1]["FOC"] = double.Parse(ansGridView5.Rows[i].Cells["Foc5"].Value.ToString());

                    dtstocks.Rows[dtstocks.Rows.Count - 1]["grcharge"] = double.Parse(ansGridView5.Rows[i].Cells["grcharge5"].Value.ToString());
                    dtstocks.Rows[dtstocks.Rows.Count - 1]["othcharge"] = double.Parse(ansGridView5.Rows[i].Cells["othcharge5"].Value.ToString());
                    dtstocks.Rows[dtstocks.Rows.Count - 1]["freight"] = double.Parse(ansGridView5.Rows[i].Cells["freight5"].Value.ToString());

                    dtstocks.Rows[dtstocks.Rows.Count - 1]["totpkts"] = double.Parse(ansGridView5.Rows[i].Cells["qty5"].Value.ToString());
                    dtstocks.Rows[dtstocks.Rows.Count - 1]["totweight"] = double.Parse(ansGridView5.Rows[i].Cells["wt5"].Value.ToString());

                    dtstocks.Rows[dtstocks.Rows.Count - 1]["actweight"] = double.Parse(ansGridView5.Rows[i].Cells["actwt5"].Value.ToString());
                    dtstocks.Rows[dtstocks.Rows.Count - 1]["itemname"] = ansGridView5.Rows[i].Cells["itemname5"].Value.ToString();
                    dtstocks.Rows[dtstocks.Rows.Count - 1]["packing"] = ansGridView5.Rows[i].Cells["packing5"].Value.ToString();
                    dtstocks.Rows[dtstocks.Rows.Count - 1]["private"] = ansGridView5.Rows[i].Cells["private5"].Value.ToString();
                    dtstocks.Rows[dtstocks.Rows.Count - 1]["remark"] = ansGridView5.Rows[i].Cells["remark5"].Value.ToString();
                    dtstocks.Rows[dtstocks.Rows.Count - 1]["grtype"] = ansGridView5.Rows[i].Cells["grtype5"].Value.ToString();
                    dtstocks.Rows[dtstocks.Rows.Count - 1]["deliverytype"] = ansGridView5.Rows[i].Cells["delivery5"].Value.ToString();




                }
                Database.SaveData(dtstocks);

            }


            //if (print == true && Database.printtype == "DOS")
            //{
            //    string str = DOSReport.voucherprintChallan(vid, "View");
            //}
            //else
            //{
            //    if (print == true)
            //    {
            //        if (Feature.Available("Ask Copies") == "No")
            //        {
            //            OtherReport rpt = new OtherReport();
            //            DataTable dtprintcopy = new DataTable();
            //            Database.GetSqlData("Select printcopy from Vouchertypes where Vt_id=" + vtid, dtprintcopy);
            //            String[] print_option = dtprintcopy.Rows[0]["printcopy"].ToString().Split(';');

            //            for (int j = 0; j < print_option.Length; j++)
            //            {
            //                if (print_option[j] != "")
            //                {

            //                    String[] defaultcopy = print_option[j].Split(',');

            //                    if (bool.Parse(defaultcopy[1]) == true)
            //                    {
            //                        rpt.voucherprint(this, vtid, vid, defaultcopy[0], true, "Print");
            //                    }
            //                }
            //            }
            //        }
            //        else
            //        {
            //            frm_printcopy frm = new frm_printcopy("Print", vid, vtid);
            //            frm.ShowDialog();
            //        }
            //    }
            //}

            //if (mode == "View")
            //{
            //    if (Database.printtype == "DOS")
            //    {
            //        string str = DOSReport.voucherprintChallan(vid, "View");
            //        str = str.Replace("\0", "").Replace("W1 ", "").Replace("W0", "").Replace("W1", "");
            //        frm_printpre frm = new frm_printpre();
            //        frm.str = str;
            //        frm.ShowDialog();
            //    }
            //    else
            //    {
            //        frm_printcopy frm = new frm_printcopy("View", vid, vtid);
            //        frm.ShowDialog();
            //    }
            //}

            //if (vid == "0")
            //{
            //    LoadData("0", "Challan");
            //}
            //else
            //{
            //    this.Close();
            //    this.Dispose();
            //}
        }

        private void clear()
        {
            if (vid == "0")
            {
                LoadData("0", "Challan");
            }
            else
            {
                this.Close();
                this.Dispose();
            }
        }
        private void view()
        {
          
                if (Database.printtype == "DOS")
                {
                    string str = DOSReport.voucherprintChallan(vid, "View");
                    str = str.Replace("\0", "").Replace("W1 ", "").Replace("W0", "").Replace("W1", "");
                    frm_printpre frm = new frm_printpre();
                    frm.str = str;
                    frm.ShowDialog();
                }
                else
                {
                    frm_printcopy frm = new frm_printcopy("View", vid, vtid);
                    frm.ShowDialog();
                }
           
        }

        private void Print()
        {
            if (Database.printtype == "DOS")
            {
                string str = DOSReport.voucherprintChallan(vid, "View");
            }
            else
            {
              
                    if (Feature.Available("Ask Copies") == "No")
                    {
                        OtherReport rpt = new OtherReport();
                        DataTable dtprintcopy = new DataTable();
                        Database.GetSqlData("Select printcopy from Vouchertypes where Vt_id=" + vtid, dtprintcopy);
                        String[] print_option = dtprintcopy.Rows[0]["printcopy"].ToString().Split(';');

                        for (int j = 0; j < print_option.Length; j++)
                        {
                            if (print_option[j] != "")
                            {

                                String[] defaultcopy = print_option[j].Split(',');

                                if (bool.Parse(defaultcopy[1]) == true)
                                {
                                    rpt.voucherprint(this, vtid, vid, defaultcopy[0], true, "Print");
                                }
                            }
                        }
                    }
                    else
                    {
                        frm_printcopy frm = new frm_printcopy("Print", vid, vtid);
                        frm.ShowDialog();
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

            //print
            dtsidefill.Rows.Add();
            dtsidefill.Rows[1]["Name"] = "Print";
            dtsidefill.Rows[1]["DisplayName"] = Database.printtype + " Print";
            dtsidefill.Rows[1]["ShortcutKey"] = "^P";
            dtsidefill.Rows[1]["Visible"] = true;

            //print preview
            dtsidefill.Rows.Add();
            dtsidefill.Rows[2]["Name"] = "PrintPre";
            dtsidefill.Rows[2]["DisplayName"] = "Print Preview";
            dtsidefill.Rows[2]["ShortcutKey"] = "^W";
            dtsidefill.Rows[2]["Visible"] = true;
            //Iscancel
            dtsidefill.Rows.Add();
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Name"] = "cancel";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["DisplayName"] = "Cancel";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["ShortcutKey"] = "";

            if (Database.utype == "User")
            {
                dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = false;
            }
            else
            {
                if (vid!="0")
                {
                    if (bool.Parse(dtVoucherinfo.Rows[0]["Iscancel"].ToString()) == true)
                    {
                        dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = false;
                    }
                    else
                    {
                        dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = true;
                    }

                }
                else
                {
                    dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = false;
                }
            }

            //takeback
            dtsidefill.Rows.Add();
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Name"] = "takeback";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["DisplayName"] = "TakeBack";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["ShortcutKey"] = "";

            if (Database.utype == "User")
            {
                dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = false;
            }
            else
            {
                if (vid!="0")
                {
                    if (bool.Parse(dtVoucherinfo.Rows[0]["Iscancel"].ToString()) == true)
                    {
                        dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = true;
                    }
                    else
                    {
                        dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = false;
                    }

                }
                else
                {
                    dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = false;
                }
            }

            //close
            dtsidefill.Rows.Add();
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Name"] = "quit";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["DisplayName"] = "Quit";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["ShortcutKey"] = "Esc";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = true;

            //change vnumber
            dtsidefill.Rows.Add();
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Name"] = "vnumber";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["DisplayName"] = "Chng Ch No.";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["ShortcutKey"] = "";
            if (Database.utype == "User")
            {
                dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = false;
            }
            else
            {
                dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = true;
            }

            for (int i = 0; i < dtsidefill.Rows.Count; i++)
            {
                if (bool.Parse(dtsidefill.Rows[i]["Visible"].ToString()) == true)
                {
                    Button btn = new Button();
                    btn.Size = new Size(135, 30);
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

        public void btn_Click(object sender, EventArgs e)
        {
           // Button tbtn = (Button)sender;
            string name = "";
            if (gresave == false)
            {
                Button tbtn = (Button)sender;
                name = tbtn.Name.ToString();
            }
            else
            {
                name = "save";
            }
            if (name == "save")
            {
                if (validation() == true)
                {
                    try
                    {
                        Database.BeginTran();
                        if (gresave == false)
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
                        else
                        {
                            dtVoucherDet = new DataTable("Voucherdets");
                            Database.GetSqlData("Select * from Voucherdets where Vi_id='" + vid + "'", dtVoucherDet);
                            for (int j = 0; j < dtVoucherDet.Rows.Count; j++)
                            {
                                dtVoucherDet.Rows[j].Delete();
                            }
                            Database.SaveData(dtVoucherDet);

                            dtVoucherDet = new DataTable("Voucherdets");
                            Database.GetSqlData("Select * from Voucherdets where Vi_id='" + vid + "'", dtVoucherDet);
                            int Nid2 = 1;
                            DataTable dtidvd = new DataTable();
                            Database.GetSqlData("select max(Nid) as Nid from Voucherdets where locationid='" + Database.LocationId + "'", dtidvd);





                            if (dtidvd.Rows[0][0].ToString() != "")
                            {
                                Nid2 = int.Parse(dtidvd.Rows[0][0].ToString()) + 1;
                            }


                            for (int i = 0; i < ansGridView5.Rows.Count; i++)
                            {
                                dtVoucherDet.Rows.Add();
                                dtVoucherDet.Rows[i]["Nid"] = Nid2;
                                dtVoucherDet.Rows[i]["LocationId"] = Database.LocationId;
                                dtVoucherDet.Rows[i]["vd_id"] = Database.LocationId + dtVoucherDet.Rows[i]["nid"].ToString();
                                dtVoucherDet.Rows[i]["Vi_id"] = vid;
                                dtVoucherDet.Rows[i]["Itemsr"] = i + 1;
                                dtVoucherDet.Rows[i]["step"] = ansGridView5.Rows[i].Cells["step2"].Value.ToString();
                                //dtVoucherDet.Rows[i]["Des_ac_id"] = "0";
                                dtVoucherDet.Rows[i]["Booking_id"] = ansGridView5.Rows[i].Cells["vi_id5"].Value.ToString();
                                dtVoucherDet.Rows[i]["remarkreq"] = false;
                                dtVoucherDet.Rows[i]["create_date"] = create_date;
                                dtVoucherDet.Rows[i]["modify_date"] = System.DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                                dtVoucherDet.Rows[i]["multiplier"] = 1;
                                dtVoucherDet.Rows[i]["Amount"] = 0;
                                Nid2++;
                            }

                            Database.SaveData(dtVoucherDet);



                            DataTable dtstocks = new DataTable("stocks");
                            Database.GetSqlData("Select * from stocks where Vid='" + vid + "'", dtstocks);
                            for (int j = 0; j < dtstocks.Rows.Count; j++)
                            {
                                dtstocks.Rows[j].Delete();
                            }
                            Database.SaveData(dtstocks);
                            //  DataTable dtbookingdet = new DataTable();
                            if (iscancel == false)
                            {
                                for (int i = 0; i < ansGridView5.Rows.Count; i++)
                                {
                                    string bookingid = ansGridView5.Rows[i].Cells["vi_id5"].Value.ToString();

                                    dtstocks.Rows.Add();

                                    dtstocks.Rows[dtstocks.Rows.Count - 1]["Vid"] = vid;
                                    dtstocks.Rows[dtstocks.Rows.Count - 1]["Step"] = ansGridView5.Rows[i].Cells["step2"].Value.ToString();
                                    dtstocks.Rows[dtstocks.Rows.Count - 1]["GR_id"] = bookingid;
                                    dtstocks.Rows[dtstocks.Rows.Count - 1]["Quantity"] = -1;
                                    //  dtstocks.Rows[dtstocks.Rows.Count - 1]["Step"] = "Step1";
                                    dtstocks.Rows[dtstocks.Rows.Count - 1]["Godown_id"] = Database.LocationId;
                                    string aliasname = Database.GetScalarText("Select Aliasname from vouchertypes where vt_id=" + vtid);


                                    dtstocks.Rows[dtstocks.Rows.Count - 1]["Narration"] = aliasname + " To " + textBox4.Text;


                                    dtstocks.Rows[dtstocks.Rows.Count - 1]["GRNo"] = ansGridView5.Rows[i].Cells["grno5"].Value.ToString();
                                    dtstocks.Rows[dtstocks.Rows.Count - 1]["GRDate"] = DateTime.Parse(ansGridView5.Rows[i].Cells["booking_date5"].Value.ToString());

                                    dtstocks.Rows[dtstocks.Rows.Count - 1]["Consigner_id"] = funs.Select_ac_id(ansGridView5.Rows[i].Cells["consigner5"].Value.ToString());
                                    dtstocks.Rows[dtstocks.Rows.Count - 1]["Consignee_id"] = funs.Select_ac_id(ansGridView5.Rows[i].Cells["consignee5"].Value.ToString());

                                    dtstocks.Rows[dtstocks.Rows.Count - 1]["Source_id"] = funs.Select_dp_id(ansGridView5.Rows[i].Cells["destination5"].Value.ToString());
                                    dtstocks.Rows[dtstocks.Rows.Count - 1]["Destination_id"] = funs.Select_dp_id(ansGridView5.Rows[i].Cells["source5"].Value.ToString());
                                    //dtstocks.Rows[dtstocks.Rows.Count - 1]["Source_id"] = funs.Select_dp_id(ansGridView5.Rows[i].Cells["source5"].Value.ToString());
                                    //dtstocks.Rows[dtstocks.Rows.Count - 1]["Destination_id"] = funs.Select_dp_id(ansGridView5.Rows[i].Cells["destination5"].Value.ToString());

                                    dtstocks.Rows[dtstocks.Rows.Count - 1]["ToPay"] = double.Parse(ansGridView5.Rows[i].Cells["pay5"].Value.ToString());
                                    dtstocks.Rows[dtstocks.Rows.Count - 1]["TBB"] = double.Parse(ansGridView5.Rows[i].Cells["billed5"].Value.ToString());

                                    dtstocks.Rows[dtstocks.Rows.Count - 1]["Paid"] = double.Parse(ansGridView5.Rows[i].Cells["Paid5"].Value.ToString());
                                    dtstocks.Rows[dtstocks.Rows.Count - 1]["FOC"] = double.Parse(ansGridView5.Rows[i].Cells["Foc5"].Value.ToString());

                                    dtstocks.Rows[dtstocks.Rows.Count - 1]["grcharge"] = double.Parse(ansGridView5.Rows[i].Cells["grcharge5"].Value.ToString());
                                    dtstocks.Rows[dtstocks.Rows.Count - 1]["othcharge"] = double.Parse(ansGridView5.Rows[i].Cells["othcharge5"].Value.ToString());
                                    dtstocks.Rows[dtstocks.Rows.Count - 1]["freight"] = double.Parse(ansGridView5.Rows[i].Cells["freight5"].Value.ToString());

                                    dtstocks.Rows[dtstocks.Rows.Count - 1]["totpkts"] = double.Parse(ansGridView5.Rows[i].Cells["qty5"].Value.ToString());
                                    dtstocks.Rows[dtstocks.Rows.Count - 1]["totweight"] = double.Parse(ansGridView5.Rows[i].Cells["wt5"].Value.ToString());

                                    dtstocks.Rows[dtstocks.Rows.Count - 1]["itemname"] = ansGridView5.Rows[i].Cells["itemname5"].Value.ToString();
                                    dtstocks.Rows[dtstocks.Rows.Count - 1]["packing"] = ansGridView5.Rows[i].Cells["packing5"].Value.ToString();
                                    dtstocks.Rows[dtstocks.Rows.Count - 1]["private"] = ansGridView5.Rows[i].Cells["private5"].Value.ToString();
                                    dtstocks.Rows[dtstocks.Rows.Count - 1]["remark"] = ansGridView5.Rows[i].Cells["remark5"].Value.ToString();
                                    dtstocks.Rows[dtstocks.Rows.Count - 1]["grtype"] = ansGridView5.Rows[i].Cells["grtype5"].Value.ToString();
                                    dtstocks.Rows[dtstocks.Rows.Count - 1]["deliverytype"] = ansGridView5.Rows[i].Cells["delivery5"].Value.ToString();




                                }
                                Database.SaveData(dtstocks);

                            }

                        }

                        Database.CommitTran();
                    }
                    catch (Exception ex)
                    {
                        Database.RollbackTran();
                        MessageBox.Show("Not Saved Due to an Exception." + ex.Message);
                        this.Close();
                        this.Dispose();
                    }
                    clear();
                }
            }
            else if (name == "cancel")
            {
                iscancel = true;
                if (validation() == true)
                {
                    try
                    {
                        Database.BeginTran();


                        if (Database.utype == "Admin")
                        {
                            save();
                        }
                        else if (gStr == "0")
                        {
                            save();
                        }

                        Database.CommitTran();
                    }
                    catch (Exception ex)
                    {
                        Database.RollbackTran();
                        MessageBox.Show("Not Saved Due to an Exception." + ex.Message);
                        this.Close();
                        this.Dispose();
                    }
                    clear();
                }

                //Database.CommandExecutor("Update Vouucherinfos set iscancel='true' where vi_id='"+vid+"'");
            }


            else if (name == "takeback")
            {
                iscancel = false;
                if (validation() == true)
                {
                    try
                    {
                        Database.BeginTran();


                        if (Database.utype == "Admin")
                        {
                            save();
                        }
                        else if (gStr == "0")
                        {


                            save();
                        }

                        Database.CommitTran();
                    }
                    catch (Exception ex)
                    {
                        Database.RollbackTran();
                        MessageBox.Show("Not Saved Due to an Exception." + ex.Message);
                        this.Close();
                        this.Dispose();
                    }
                    clear();
                }

                //Database.CommandExecutor("Update Vouucherinfos set iscancel='false' where vi_id='" + vid + "'");
            }

            else if (name == "vnumber")
            {
                InputBox box = new InputBox("Enter Administrative password", "", true);
                box.ShowDialog(this);
                String pass = box.outStr;
                if (pass.ToLower() == "admin")
                {
                    box = new InputBox("Enter Voucher Number", "", false);
                    box.ShowDialog();
                    if (box.outStr == "")
                    {
                        vno = int.Parse(label10.Text);
                    }
                    else
                    {
                        vno = int.Parse(box.outStr);
                    }
                    label1.Text = vno.ToString();
                    int numtype = funs.chkNumType(vtid);
                    if (numtype != 1)
                    {
                        vid = Database.GetScalarText("Select Vi_id from voucherinfos where LocationId='" + Database.LocationId + "' and  Vt_id=" + vtid + " and Vnumber=" + vno + " and Vdate=" + access_sql.Hash + dateTimePicker1.Value.Date.ToString(Database.dformat) + access_sql.Hash);
                    }
                    else
                    {
                        string tempvid = "";
                        tempvid = Database.GetScalarText("Select Vi_id from voucherinfos where LocationId='" + Database.LocationId + "' and Vt_id=" + vtid + " and Vnumber=" + vno);
                        if (tempvid != "")
                        {
                            MessageBox.Show("Voucher can't be created on this No.");
                            vno = 0;
                            label1.Text = vno.ToString();
                            //SetVno();
                            return;
                        }
                    }
                    f12used = true;
                }
                else
                {
                    MessageBox.Show("Invalid password");
                }
            }

            else if (name == "Print")
            {
                if (validation() == true)
                {
                    try
                    {
                        Database.BeginTran();
                        if (Database.utype == "Admin")
                        {
                            save();
                        }
                        else if (gStr == "0")
                        {
                            save();
                        }
                        Database.CommitTran();
                    }
                    catch (Exception ex)
                    {
                        Database.RollbackTran();
                        MessageBox.Show("Not Saved Due to an Exception." + ex.Message);
                        this.Close();
                        this.Dispose();
                    }
                    if (vid != "0")
                    {
                        Print();
                    }
                    clear();
                }
            }
            else if (name == "PrintPre")
            {
                if (validation() == true)
                {
                    try
                    {
                        Database.BeginTran();
                        if (Database.utype == "Admin")
                        {
                            save();
                        }
                        else if (gStr == "0")
                        {
                            save();
                        }
                        Database.CommitTran();
                    }
                    catch (Exception ex)
                    {
                        Database.RollbackTran();
                        MessageBox.Show("Not Saved Due to an Exception." + ex.Message);
                        this.Close();
                        this.Dispose();
                    }
                    view();
                    clear();
                }
            }
            else if (name == "quit")
            {
                this.Close();
                this.Dispose();
            }
        }
        private bool validation()
        {
            if (txtTruckNo.Text.Trim() == "")
            {
                MessageBox.Show("Enter Truck Number");
                txtTruckNo.Focus();
                return false;
            }
            if (textBox1.Text.Trim() == "")
            {
                MessageBox.Show("Enter Party Name");
                textBox1.Focus();
                return false;
            }
            if (textBox2.Text.Trim() == "")
            {
                MessageBox.Show("Enter Driver Name");
                textBox2.Focus();
                return false;
            }
            if (ansGridView5.Rows.Count == 0)
            {
                MessageBox.Show("Enter some Values");
                button1.Focus();
                return false;
            }
            if (textBox3.Text.Trim() == "")
            {
                MessageBox.Show("Enter Station From");
                textBox3.Focus();
                return false;
            }
            if (textBox4.Text.Trim() == "")
            {
                MessageBox.Show("Enter Station To");
                textBox4.Focus();
                return false;
            }
            if (funs.GenerateVno(vtid, dateTimePicker1.Value.ToString("dd-MMM-yyyy"), vid) == 0 && vno == 0)
            {
                MessageBox.Show("Voucher Number can't be created on this date.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            for (int j = 0; j < ansGridView1.Rows.Count; j++)
            {
                if (Convert.ToBoolean(ansGridView1.Rows[j].Cells["select"].Value) == true)
                {
                    //add                    
                    ansGridView5.Rows.Add();
                    ansGridView5.Rows[ansGridView5.Rows.Count - 1].Cells["vi_id5"].Value = ansGridView1.Rows[j].Cells["vi_id1"].Value.ToString();
                    ansGridView5.Rows[ansGridView5.Rows.Count - 1].Cells["step2"].Value = ansGridView1.Rows[j].Cells["step"].Value.ToString();
                    ansGridView5.Rows[ansGridView5.Rows.Count - 1].Cells["booking_date5"].Value = ansGridView1.Rows[j].Cells["booking_date1"].Value.ToString();
                    ansGridView5.Rows[ansGridView5.Rows.Count - 1].Cells["grno5"].Value = ansGridView1.Rows[j].Cells["grno1"].Value.ToString();
                    ansGridView5.Rows[ansGridView5.Rows.Count - 1].Cells["consigner5"].Value = ansGridView1.Rows[j].Cells["consigner1"].Value.ToString();
                    ansGridView5.Rows[ansGridView5.Rows.Count - 1].Cells["consignee5"].Value = ansGridView1.Rows[j].Cells["consignee1"].Value.ToString();
                    ansGridView5.Rows[ansGridView5.Rows.Count - 1].Cells["source5"].Value = ansGridView1.Rows[j].Cells["source1"].Value.ToString();
                    ansGridView5.Rows[ansGridView5.Rows.Count - 1].Cells["destination5"].Value = ansGridView1.Rows[j].Cells["destination1"].Value.ToString();
                    ansGridView5.Rows[ansGridView5.Rows.Count - 1].Cells["delivery5"].Value = ansGridView1.Rows[j].Cells["delivery1"].Value.ToString();
                    ansGridView5.Rows[ansGridView5.Rows.Count - 1].Cells["grtype5"].Value = ansGridView1.Rows[j].Cells["grtype1"].Value.ToString();
                    ansGridView5.Rows[ansGridView5.Rows.Count - 1].Cells["private5"].Value = ansGridView1.Rows[j].Cells["private1"].Value.ToString();
                    ansGridView5.Rows[ansGridView5.Rows.Count - 1].Cells["remark5"].Value = ansGridView1.Rows[j].Cells["remark1"].Value.ToString();
                    ansGridView5.Rows[ansGridView5.Rows.Count - 1].Cells["qty5"].Value = ansGridView1.Rows[j].Cells["qty1"].Value.ToString();
                    ansGridView5.Rows[ansGridView5.Rows.Count - 1].Cells["wt5"].Value = ansGridView1.Rows[j].Cells["wt1"].Value.ToString();
                    ansGridView5.Rows[ansGridView5.Rows.Count - 1].Cells["actwt5"].Value = ansGridView1.Rows[j].Cells["actwt1"].Value.ToString();
                    ansGridView5.Rows[ansGridView5.Rows.Count - 1].Cells["amt5"].Value = ansGridView1.Rows[j].Cells["amt1"].Value.ToString();
                    ansGridView5.Rows[ansGridView5.Rows.Count - 1].Cells["freight5"].Value = ansGridView1.Rows[j].Cells["freight1"].Value.ToString();
                  //  ansGridView5.Rows[ansGridView5.Rows.Count - 1].Cells["dd5"].Value = ansGridView1.Rows[j].Cells["dd1"].Value.ToString();
                    ansGridView5.Rows[ansGridView5.Rows.Count - 1].Cells["foc5"].Value = ansGridView1.Rows[j].Cells["foc1"].Value.ToString();
                    ansGridView5.Rows[ansGridView5.Rows.Count - 1].Cells["pay5"].Value = ansGridView1.Rows[j].Cells["pay1"].Value.ToString();
                    ansGridView5.Rows[ansGridView5.Rows.Count - 1].Cells["billed5"].Value = ansGridView1.Rows[j].Cells["billed1"].Value.ToString();
                    ansGridView5.Rows[ansGridView5.Rows.Count - 1].Cells["paid5"].Value = ansGridView1.Rows[j].Cells["paid1"].Value.ToString();


                    ansGridView5.Rows[ansGridView5.Rows.Count - 1].Cells["itemname5"].Value = ansGridView1.Rows[j].Cells["itemname1"].Value.ToString();
                    ansGridView5.Rows[ansGridView5.Rows.Count - 1].Cells["packing5"].Value = ansGridView1.Rows[j].Cells["packing1"].Value.ToString();
                    ansGridView5.Rows[ansGridView5.Rows.Count - 1].Cells["grcharge5"].Value = ansGridView1.Rows[j].Cells["grcharge1"].Value.ToString();
                    ansGridView5.Rows[ansGridView5.Rows.Count - 1].Cells["othcharge5"].Value = ansGridView1.Rows[j].Cells["othcharge1"].Value.ToString();
                }
            }

            int rows = ansGridView1.Rows.Count - 1;
            for (int k = rows; k >= 0; k--)
            {
                if (Convert.ToBoolean(ansGridView1.Rows[k].Cells["select"].Value) == true)
                {
                    
                    ansGridView1.Rows.RemoveAt(k);
                }
            }

            weightCalc();
            if (ansGridView5.Rows.Count > 0)
            {
                ansGridView1.Sort(ansGridView1.Columns["grno1"], ListSortDirection.Descending);
                ansGridView5.Sort(ansGridView5.Columns["grno5"], ListSortDirection.Descending);
            }
        }

        private void frm_newvt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
                this.Dispose();
            }
            if (e.Control && e.KeyCode == Keys.S)
            {
                if (validation() == true)
                {
                    try
                    {
                        Database.BeginTran();
                        if (Database.utype == "Admin")
                        {
                            save();
                        }
                        else if (gStr == "0")
                        {
                            save();
                        }
                        Database.CommitTran();
                    }
                    catch (Exception ex)
                    {
                        Database.RollbackTran();
                        MessageBox.Show("Not Saved Due to an Exception." + ex.Message);
                        this.Close();
                        this.Dispose();
                    }
                    clear();
                }
            }

            if (e.Control && e.KeyCode == Keys.P)
            {
                if (validation() == true)
                {
                    try
                    {
                        Database.BeginTran();
                        if (Database.utype == "Admin")
                        {
                            save();
                        }
                        else if (gStr == "0")
                        {
                            save();
                        }
                        Database.CommitTran();
                    }
                    catch (Exception ex)
                    {
                        Database.RollbackTran();
                        MessageBox.Show("Not Saved Due to an Exception." + ex.Message);
                        this.Close();
                        this.Dispose();
                    }
                    if (vid != "0")
                    {
                        Print();
                    }
                    clear();
                }
            }

        }

        private void frm_newvt_Load(object sender, EventArgs e)
        {
            SideFill();
        }

        private void txtTruckNo_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(txtTruckNo);
        }

        private void txtTruckNo_Enter(object sender, EventArgs e)
        {
            Database.setFocus(txtTruckNo);
        }

        private void dateTimePicker1_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(dateTimePicker1);
        }

        private void dateTimePicker1_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void dateTimePicker1_Enter(object sender, EventArgs e)
        {
            Database.setFocus(dateTimePicker1);
        }

        private void txtTruckNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C)
            {
                txtTruckNo.Text = funs.AddGaddi();
            }
            else if (e.Control && e.KeyCode == Keys.A)
            {
                if (txtTruckNo.Text != "")
                {
                    txtTruckNo.Text = funs.EditGaddi(txtTruckNo.Text); ;
                }
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

        private void textBox1_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox1);
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox1);
        }

        private void textBox3_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox3);
        }

        private void textBox3_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox3);
        }

        private void textBox4_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox4);
        }

        private void textBox4_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox4);
        }

        private void textBox18_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox18);
        }

        private void textBox18_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox18_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox18);
        }

        private void textBox17_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox17);
        }

        private void textBox17_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox17_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox17);
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox5_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox5);
        }

        private void textBox5_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox5_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox5);
        }

        private void textBox13_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox13);
        }

        private void textBox13_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox13_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox13);
        }

        private void textBox14_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox14);
        }

        private void textBox14_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox14);
        }

        private void textBox14_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox6_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox6);
        }

        private void textBox6_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox6);
        }

        private void textBox6_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox15_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox15);
        }

        private void textBox15_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox15);
        }

        private void textBox15_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox9_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox9);
        }

        private void textBox9_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox9);
        }

        private void textBox9_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox12_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox12);
        }

        private void textBox12_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox12);
        }

        private void textBox12_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox10_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox10);
        }

        private void textBox10_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox10);
        }

        private void textBox10_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void ansGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (ansGridView1.CurrentCell.OwningColumn.Name == "grno1")
            {
                frmBooking frm = new frmBooking();
                frm.LoadData(ansGridView1.Rows[ansGridView1.CurrentRow.Index].Cells["Vi_id1"].Value.ToString(), "Booking");
                frm.MdiParent = this.MdiParent;
                frm.Show();
            }
        }

        private void ansGridView5_KeyDown(object sender, KeyEventArgs e)
        {
            if (ansGridView5.CurrentCell == null)
            {
                return;
            }
            if (e.KeyCode == Keys.Delete)
            {
               

                ansGridView1.Rows.Add();
             
                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["vi_id1"].Value = ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["vi_id5"].Value.ToString();
                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["booking_date1"].Value = ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["booking_date5"].Value.ToString();
                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["grno1"].Value = ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["grno5"].Value.ToString();
                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["consigner1"].Value = ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["consigner5"].Value.ToString();
                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["consignee1"].Value = ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["consignee5"].Value.ToString();
                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["source1"].Value = ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["source5"].Value.ToString();
                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["destination1"].Value = ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["destination5"].Value.ToString();
                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["delivery1"].Value = ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["delivery5"].Value.ToString();
                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["grtype1"].Value = ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["grtype5"].Value.ToString();
                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["private1"].Value = ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["private5"].Value.ToString();
                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["remark1"].Value = ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["remark5"].Value.ToString();
                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["qty1"].Value = ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["qty5"].Value.ToString();
                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["wt1"].Value = ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["wt5"].Value.ToString();
                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["actwt1"].Value = ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["actwt5"].Value.ToString();
                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["step"].Value = ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["step2"].Value.ToString();
                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["amt1"].Value = ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["amt5"].Value.ToString();
                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["freight1"].Value = ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["freight5"].Value.ToString();
              //  ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["dd1"].Value = ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["dd5"].Value.ToString();

                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["foc1"].Value = ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["foc5"].Value.ToString();
                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["pay1"].Value = ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["pay5"].Value.ToString();
                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["billed1"].Value = ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["billed5"].Value.ToString();
                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["paid1"].Value = ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["paid5"].Value.ToString();

                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["itemname1"].Value = ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["itemname5"].Value.ToString();
                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["packing1"].Value = ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["packing5"].Value.ToString();
                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["grcharge1"].Value = ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["grcharge5"].Value.ToString();
                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["othcharge1"].Value = ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["othcharge5"].Value.ToString();
                ansGridView5.Rows.RemoveAt(ansGridView5.CurrentRow.Index);

                ansGridView1.Sort(ansGridView1.Columns["grno1"], ListSortDirection.Descending);
                ansGridView5.Sort(ansGridView5.Columns["grno5"], ListSortDirection.Descending);
                weightCalc();
                return;
            }

        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C)
            {
                textBox2.Text = funs.AddAccount();
            }
            else if (e.Control && e.KeyCode == Keys.A)
            {
                if (textBox2.Text != "")
                {
                    textBox2.Text = funs.EditAccount(textBox2.Text); ;
                }
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            SetVno();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            for (int j = 0; j < ansGridView1.Rows.Count; j++)
            {
                if (checkBox1.Checked == true)
                {
                    ansGridView1.Rows[j].Cells["select"].Value = true;
                }
                else
                {
                    ansGridView1.Rows[j].Cells["select"].Value = false;
                }
            }
        }

        private void textBox17_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && !(e.KeyChar.ToString() == ".") && !(e.KeyChar.ToString() == "-");
        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && !(e.KeyChar.ToString() == ".") && !(e.KeyChar.ToString() == "-");
        }

        private void textBox13_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && !(e.KeyChar.ToString() == ".") && !(e.KeyChar.ToString() == "-");
        }

        private void textBox14_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && !(e.KeyChar.ToString() == ".") && !(e.KeyChar.ToString() == "-");
        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && !(e.KeyChar.ToString() == ".") && !(e.KeyChar.ToString() == "-");
        }

        private void textBox15_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && !(e.KeyChar.ToString() == ".") && !(e.KeyChar.ToString() == "-");
        }

        private void textBox9_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && !(e.KeyChar.ToString() == ".") && !(e.KeyChar.ToString() == "-");
        }

        private void textBox12_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && !(e.KeyChar.ToString() == ".") && !(e.KeyChar.ToString() == "-");
        }

        private void textBox10_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && !(e.KeyChar.ToString() == ".") && !(e.KeyChar.ToString() == "-");
        }

    }
}
