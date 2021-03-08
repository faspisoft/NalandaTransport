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
    public partial class frm_dbill : Form
    {
        int vtid;
        int vno = 0;
        string gStr = "";
        string vid = "";
        DataTable dtVoucherInfo;
        Boolean RoffChanged = false;
        string Prelocationid = "";
        string strCombo = "";
        bool iscancel = false;

        DateTime create_date = DateTime.Parse(System.DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"));
        public frm_dbill()
        {
            InitializeComponent();
            dateTimePicker1.CustomFormat = Database.dformat;
            dateTimePicker1.MaxDate = Database.ldate;
            dateTimePicker1.MinDate = Database.stDate;
            dateTimePicker1.Value = Database.ldate;

            dateTimePicker2.CustomFormat = Database.dformat;
            dateTimePicker2.MaxDate = Database.ldate;
            dateTimePicker2.MinDate = Database.stDate;
            dateTimePicker2.Value = Database.stDate;

            dateTimePicker3.CustomFormat = Database.dformat;
            dateTimePicker3.MaxDate = Database.ldate;
            dateTimePicker3.MinDate = Database.stDate;
            dateTimePicker3.Value = Database.ldate;
        }

        private void frm_dbill_Load(object sender, EventArgs e)
        {
            SideFill();
           
        }


        public void LoadData(string vi_id, String frmCaption)
        {
            gStr = vi_id.ToString();
            vid = vi_id;
            vtid = funs.Select_vt_id("DBill");
            if (vi_id == "0")
            {
                SetVno();
            }

            dtVoucherInfo = new DataTable("Voucherinfos");
            Database.GetSqlData("select * from Voucherinfos where Vi_id='" + vi_id + "'", dtVoucherInfo);

            if (dtVoucherInfo.Rows.Count == 0)
            {
                textBox6.Text = "";
                textBox1.Text = "0";
                textBox2.Text = "0.00";
                textBox7.Text = "0.00";
                textBox8.Text = "0.00";
                txtTotalWeight.Text = "0.00";
                ansGridView1.Rows.Clear();
                iscancel = false;
                label28.Visible = false;
            }
            else
            {
                vtid = int.Parse(dtVoucherInfo.Rows[0]["Vt_id"].ToString());
                dateTimePicker1.Value = DateTime.Parse(dtVoucherInfo.Rows[0]["Vdate"].ToString());
                vno = int.Parse(dtVoucherInfo.Rows[0]["Vnumber"].ToString());
                label10.Text = vno.ToString();
                RoffChanged = bool.Parse(dtVoucherInfo.Rows[0]["RoffChanged"].ToString());
                Prelocationid = dtVoucherInfo.Rows[0]["Locationid"].ToString();
                textBox6.Text = funs.Select_ac_nm(dtVoucherInfo.Rows[0]["ac_id"].ToString());
                create_date = DateTime.Parse(dtVoucherInfo.Rows[0]["create_date"].ToString());
                dateTimePicker2.Value = DateTime.Parse(dtVoucherInfo.Rows[0]["Period_from"].ToString());
                dateTimePicker3.Value = DateTime.Parse(dtVoucherInfo.Rows[0]["Period_to"].ToString());
                if (bool.Parse(dtVoucherInfo.Rows[0]["Iscancel"].ToString()) == true)
                {
                    label28.Visible = true;
                    label28.Text = "Cancelled";
                    iscancel = bool.Parse(dtVoucherInfo.Rows[0]["Iscancel"].ToString());
                }

                DataTable dtVoucherdet = new DataTable("Voucherdet");
                Database.GetSqlData("Select * from Voucherdets where Vi_id='" + vi_id + "'order by itemsr ", dtVoucherdet);

                fillGrid();

                //DataTable dt = new DataTable();
               
                //Database.GetSqlData("SELECT VOUCHERINFOs.Vi_id, CONVERT(nvarchar, VOUCHERINFOs.Vdate, 106) AS Booking_date, VOUCHERINFOs.Vnumber AS GRno, ACCOUNTs.name AS Consigner, ACCOUNTs_1.name AS Consignee, DeliveryPoints_1.Name AS source, DeliveryPoints.Name AS destination, VOUCHERINFOs.DeliveryType, VOUCHERINFOs.PaymentMode AS GR_type, VOUCHERINFOs.Transport1 AS Private, VOUCHERINFOs.Transport5 AS Remark, SUM(Voucherdets.Quantity) AS Total_quantity, SUM(Voucherdets.weight) AS Total_weight, VOUCHERINFOs.Totalamount AS total_amount, SUM(Voucherdets.Rate_am) AS Freight, SUM(Voucherdets.exp4amt) AS door_delivery,SUM(Voucherdets.exp8amt) as exp8amt FROM Voucherdets RIGHT OUTER JOIN VOUCHERINFOs ON Voucherdets.Vi_id = VOUCHERINFOs.Vi_id LEFT OUTER JOIN Voucherdets AS Voucherdets_1 ON VOUCHERINFOs.Vi_id = Voucherdets_1.Bill_booking_id LEFT OUTER JOIN VOUCHERTYPEs ON VOUCHERINFOs.Vt_id = VOUCHERTYPEs.Vt_id LEFT OUTER JOIN DeliveryPoints ON VOUCHERINFOs.SId = DeliveryPoints.DPId LEFT OUTER JOIN ACCOUNTs ON VOUCHERINFOs.Ac_id = ACCOUNTs.ac_id LEFT OUTER JOIN ACCOUNTs AS ACCOUNTs_1 ON VOUCHERINFOs.Ac_id2 = ACCOUNTs_1.ac_id LEFT OUTER JOIN DeliveryPoints AS DeliveryPoints_1 ON VOUCHERINFOs.Consigner_id = DeliveryPoints_1.DPId WHERE (VOUCHERTYPEs.Type = 'Booking') AND (Voucherdets_1.Vi_id = '" + vid + "')  GROUP BY VOUCHERINFOs.Vi_id, VOUCHERINFOs.Vdate, VOUCHERINFOs.Vnumber, ACCOUNTs.name, ACCOUNTs_1.name, DeliveryPoints_1.Name, DeliveryPoints.Name, VOUCHERINFOs.DeliveryType, VOUCHERINFOs.PaymentMode, VOUCHERINFOs.Transport1, VOUCHERINFOs.Transport5, VOUCHERINFOs.Totalamount HAVING (VOUCHERINFOs.PaymentMode = 'T.B.B.') AND (ACCOUNTs.name = '" + textBox6.Text + "') AND (dbo.VOUCHERINFOs.Vdate >= " + access_sql.Hash + "" + dateTimePicker2.Value.Date.ToString(Database.dformat) + "" + access_sql.Hash + ") AND  (dbo.VOUCHERINFOs.Vdate <= " + access_sql.Hash + "" + dateTimePicker3.Value.Date.ToString(Database.dformat) + "" + access_sql.Hash + ") ORDER BY GRno DESC", dt);

                //for (int i = 0; i < dt.Rows.Count; i++)
                //{
                //    ansGridView1.Rows.Add();
                //    ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["select"].Value = true;
                   
                //    ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["vi_id1"].Value = dt.Rows[i]["Vi_id"].ToString();
                //    ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["booking_date1"].Value = dt.Rows[i]["Booking_date"].ToString();
                //    ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["grno1"].Value = dt.Rows[i]["GRno"].ToString();
                //    ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["consigner1"].Value = dt.Rows[i]["Consigner"].ToString();
                //    ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["consignee1"].Value = dt.Rows[i]["Consignee"].ToString();
                //    ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["source1"].Value = dt.Rows[i]["source"].ToString();
                //    ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["destination1"].Value = dt.Rows[i]["destination"].ToString();
                //    ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["delivery1"].Value = dt.Rows[i]["DeliveryType"].ToString();
                //    ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["grtype1"].Value = dt.Rows[i]["GR_type"].ToString();
                //    ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["private1"].Value = dt.Rows[i]["Private"].ToString();
                //    ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["remark1"].Value = dt.Rows[i]["Remark"].ToString();
                //    ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["qty1"].Value = dt.Rows[i]["Total_quantity"].ToString();
                //    ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["wt1"].Value = dt.Rows[i]["Total_weight"].ToString();
                //    ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["amt1"].Value = dt.Rows[i]["total_amount"].ToString();
                //    ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["freight1"].Value = dt.Rows[i]["Freight"].ToString();

                //    if (dt.Rows[i]["exp8amt"].ToString() == "")
                //    {
                //        dt.Rows[i]["exp8amt"] = 0;
                //    }
                //    if (dt.Rows[i]["Door_delivery"].ToString() == "")
                //    {
                //        dt.Rows[i]["Door_delivery"] = 0;
                //    }

                    
                //    ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["dd1"].Value = (double.Parse(dt.Rows[i]["Door_delivery"].ToString()) + double.Parse(dt.Rows[i]["exp8amt"].ToString())).ToString();
                   
                //}

                // dateTimePicker1.Select();











             
            }
        }

        private void fillGrid()
        {
            ansGridView1.Rows.Clear();
            string str = "";
            if (vid != "0")
            {
                str = "SELECT CONVERT(nvarchar, VOUCHERINFOs_1.Vdate, 106) AS Vdate, VOUCHERINFOs_1.Invoiceno, Stocks.GRNo, Stocks.Private,   Stocks.Remark, Stocks.TotPkts, Stocks.Freight,   SUM(Voucherdets_1.exp1amt + Voucherdets_1.exp2amt + Voucherdets_1.exp3amt + Voucherdets_1.exp4amt) AS Dcharge, Stocks.Freight +   SUM(Voucherdets_1.exp1amt + Voucherdets_1.exp2amt + Voucherdets_1.exp3amt + Voucherdets_1.exp4amt) AS Amount,  Voucherdets.Delivery_id AS vi_id, VOUCHERINFOs.Vdate AS Deldate, VOUCHERINFOs.Invoiceno AS DelInvno,   VOUCHERINFOs.Vnumber AS Delvno FROM ACCOUNTs AS ACCOUNTs_1 FULL OUTER JOIN  VOUCHERTYPEs RIGHT OUTER JOIN  VOUCHERINFOs AS VOUCHERINFOs_1 ON VOUCHERTYPEs.Vt_id = VOUCHERINFOs_1.Vt_id FULL OUTER JOIN  Voucherdets FULL OUTER JOIN  VOUCHERINFOs RIGHT OUTER JOIN  Voucherdets AS Voucherdets_1 ON VOUCHERINFOs.Vi_id = Voucherdets_1.Vi_id ON Voucherdets.Delivery_id = Voucherdets_1.Vi_id ON   VOUCHERINFOs_1.Vi_id = Voucherdets.Vi_id FULL OUTER JOIN  ACCOUNTs FULL OUTER JOIN  Stocks ON ACCOUNTs.ac_id = Stocks.Consigner_id ON Voucherdets_1.Vi_id = Stocks.vid ON ACCOUNTs_1.ac_id = Stocks.Consignee_id WHERE (VOUCHERINFOs_1.LocationId = '" + Database.LocationId + "') AND (ACCOUNTs_1.name = '" + textBox6.Text + "') AND (VOUCHERINFOs_1.Vi_id = '" + vid + "') GROUP BY VOUCHERINFOs_1.Vdate, VOUCHERINFOs_1.Invoiceno,  Stocks.GRNo, VOUCHERINFOs_1.Vnumber, Stocks.Private,  Stocks.Remark, Stocks.TotPkts, Stocks.Freight, Voucherdets.Delivery_id, VOUCHERINFOs.Vdate, VOUCHERINFOs.Invoiceno,   VOUCHERINFOs.Vnumber HAVING (VOUCHERINFOs_1.Vdate >= '" + dateTimePicker2.Value.Date.ToString(Database.dformat) + "') AND (VOUCHERINFOs_1.Vdate <= '" + dateTimePicker3.Value.Date.ToString(Database.dformat) + "') ORDER BY Vdate DESC, VOUCHERINFOs_1.Vnumber DESC ";
                //str = "SELECT CONVERT(nvarchar, VOUCHERINFOs_1.Vdate, 106) AS Vdate, VOUCHERINFOs_1.Invoiceno, Stocks.GRNo, Stocks.Private,  Stocks.Remark, Stocks.TotPkts, Stocks.Freight, SUM(Voucherdets_1.exp1amt + Voucherdets_1.exp2amt + Voucherdets_1.exp3amt + Voucherdets_1.exp4amt) AS Dcharge, VOUCHERINFOs_1.Totalamount AS Amount,  Voucherdets.Delivery_id AS vi_id FROM ACCOUNTs AS ACCOUNTs_1 RIGHT OUTER JOIN ACCOUNTs RIGHT OUTER JOIN  Stocks ON ACCOUNTs.ac_id = Stocks.Consigner_id ON ACCOUNTs_1.ac_id = Stocks.Consignee_id FULL OUTER JOIN Voucherdets AS Voucherdets_1 RIGHT OUTER JOIN  Voucherdets ON Voucherdets_1.Vi_id = Voucherdets.Delivery_id RIGHT OUTER JOIN  VOUCHERTYPEs RIGHT OUTER JOIN  VOUCHERINFOs AS VOUCHERINFOs_1 ON VOUCHERTYPEs.Vt_id = VOUCHERINFOs_1.Vt_id ON Voucherdets.Vi_id = VOUCHERINFOs_1.Vi_id ON   Stocks.vid = Voucherdets_1.Vi_id WHERE (VOUCHERINFOs_1.LocationId = '" + Database.LocationId + "') AND (ACCOUNTs_1.name = '" + textBox6.Text + "') AND (VOUCHERINFOs_1.Vi_id = '" + vid + "') GROUP BY VOUCHERINFOs_1.Vdate, VOUCHERINFOs_1.Invoiceno, VOUCHERINFOs_1.Totalamount, Stocks.GRNo, VOUCHERINFOs_1.Vnumber, Stocks.Private,   Stocks.Remark, Stocks.TotPkts, Stocks.Freight, Voucherdets.Delivery_id HAVING (VOUCHERINFOs_1.Vdate >=  '" + dateTimePicker2.Value.Date.ToString(Database.dformat) + "') AND (VOUCHERINFOs_1.Vdate <=  '" + dateTimePicker3.Value.Date.ToString(Database.dformat) + "') ORDER BY Vdate DESC, VOUCHERINFOs_1.Vnumber DESC ";
            }
            else
            {
                str = "SELECT  CONVERT(nvarchar, VOUCHERINFOs_1.Vdate, 106) AS  Deldate, VOUCHERINFOs_1.Invoiceno as DelInvno, Stocks.GRNo, Stocks.Private,  Stocks.Remark, Stocks.TotPkts, Stocks.Freight,   SUM( Voucherdets.exp1amt + Voucherdets.exp2amt + Voucherdets.exp3amt + Voucherdets.exp4amt) AS Dcharge,  VOUCHERINFOs_1.Totalamount AS Amount, VOUCHERINFOs_1.Vi_id FROM VOUCHERTYPEs RIGHT OUTER JOIN  VOUCHERINFOs AS VOUCHERINFOs_1 ON VOUCHERTYPEs.Vt_id = VOUCHERINFOs_1.Vt_id FULL OUTER JOIN  ACCOUNTs AS ACCOUNTs_1 RIGHT OUTER JOIN ACCOUNTs RIGHT OUTER JOIN  Stocks ON ACCOUNTs.ac_id = Stocks.Consigner_id ON ACCOUNTs_1.ac_id = Stocks.Consignee_id FULL OUTER JOIN  Voucherdets ON Stocks.vid = Voucherdets.Vi_id ON VOUCHERINFOs_1.Vi_id = Voucherdets.Vi_id WHERE (VOUCHERINFOs_1.LocationId = '" + Database.LocationId + "') and dbilled='false'  AND (ACCOUNTs_1.name = '" + textBox6.Text + "') AND (VOUCHERINFOs_1.PaymentMode = 'Credit') GROUP BY VOUCHERTYPEs.Type, VOUCHERINFOs_1.Vdate, VOUCHERINFOs_1.Invoiceno, VOUCHERINFOs_1.Totalamount, VOUCHERINFOs_1.Vi_id, Stocks.GRNo, VOUCHERINFOs_1.Vnumber, Stocks.Private, Stocks.Remark, Stocks.TotPkts, Stocks.Freight HAVING ( VOUCHERTYPEs.Type = 'Delivery') AND (VOUCHERINFOs_1.Vdate >= '" + dateTimePicker2.Value.Date.ToString(Database.dformat) + "') AND (VOUCHERINFOs_1.Vdate <= '" + dateTimePicker3.Value.Date.ToString(Database.dformat) + "') ORDER BY Vdate DESC, Vnumber DESC ";
            }
            DataTable dtfill = new DataTable();
            Database.GetSqlData(str, dtfill);

            for (int m = 0; m < dtfill.Rows.Count; m++)
            {
                ansGridView1.Rows.Add();
                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["select"].Value = true;
                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["vi_id"].Value = dtfill.Rows[m]["vi_id"].ToString();
                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["deliverydate"].Value = dtfill.Rows[m]["Deldate"].ToString();
                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["grno"].Value = dtfill.Rows[m]["GRno"].ToString();
                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["receiptno"].Value = dtfill.Rows[m]["DelInvno"].ToString();
            
             
                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["private1"].Value = dtfill.Rows[m]["Private"].ToString();
                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["remark1"].Value = dtfill.Rows[m]["Remark"].ToString();
                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["qty1"].Value = dtfill.Rows[m]["TotPkts"].ToString();
               
                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["amt1"].Value = dtfill.Rows[m]["amount"].ToString();
                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["freight1"].Value = dtfill.Rows[m]["Freight"].ToString();
                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["dcharge"].Value = dtfill.Rows[m]["dcharge"].ToString();
            }
            weightCalc();
        }

        private void weightCalc()
        {
            double Totalfreight = 0;
            double TotalDcharge = 0;
            double Totalqty = 0;
            double Totalamt = 0;
            int gr = 0;

            for (int i = 0; i < ansGridView1.Rows.Count; i++)
            {
                if (bool.Parse(ansGridView1.Rows[i].Cells["select"].Value.ToString()) == true)
                {
                    gr++;
                    if (ansGridView1.Rows[i].Cells["freight1"].Value.ToString() == "")
                    {
                        ansGridView1.Rows[i].Cells["freight1"].Value = 0;
                    }


                    Totalfreight += double.Parse(ansGridView1.Rows[i].Cells["freight1"].Value.ToString());
                    if (ansGridView1.Rows[i].Cells["dcharge"].Value.ToString() == "")
                    {
                        ansGridView1.Rows[i].Cells["dcharge"].Value = 0;
                    }
                    TotalDcharge += double.Parse(ansGridView1.Rows[i].Cells["dcharge"].Value.ToString());
                    if (ansGridView1.Rows[i].Cells["qty1"].Value.ToString() == "")
                    {
                        ansGridView1.Rows[i].Cells["qty1"].Value = 0;
                    }
                    Totalqty += double.Parse(ansGridView1.Rows[i].Cells["qty1"].Value.ToString());
                    if (ansGridView1.Rows[i].Cells["amt1"].Value.ToString() == "")
                    {
                        ansGridView1.Rows[i].Cells["amt1"].Value = 0;
                    }
                    Totalamt += double.Parse(ansGridView1.Rows[i].Cells["amt1"].Value.ToString());
                }
            }

            textBox1.Text = gr.ToString();
            textBox2.Text = TotalDcharge.ToString();
            textBox7.Text = Totalqty.ToString();
            textBox8.Text = Totalamt.ToString();
            txtTotalWeight.Text = Totalfreight.ToString();
        }
        private void save()
        {
            string prefix = "";
            string postfix = "";
            int padding = 0;

          
            prefix = Database.GetScalarText("Select prefix from Location where LocationId='" + Database.LocationId + "'");

            if (vno == 0)
            {
                vno = funs.GenerateVno(vtid, dateTimePicker1.Value.ToString("dd-MMM-yyyy"), vid);
            }

            if (dtVoucherInfo.Rows.Count == 0)
            {
                dtVoucherInfo.Rows.Add();
            }

            if (vid == "0")
            {
                DataTable dtCount = new DataTable();
                Database.GetSqlData("select count(*) from VOUCHERINFOs where locationid='" + Database.LocationId + "'", dtCount);

                if (int.Parse(dtCount.Rows[0][0].ToString()) == 0)
                {
                    dtVoucherInfo.Rows[0]["Vi_id"] = Database.LocationId + "1";
                    dtVoucherInfo.Rows[0]["Nid"] = 1;
                    dtVoucherInfo.Rows[0]["LocationId"] = Database.LocationId;
                    Prelocationid = Database.LocationId;
                }
                else
                {
                    DataTable dtid = new DataTable();
                    Database.GetSqlData("select max(Nid) as Nid from VOUCHERINFOs where locationid='" + Database.LocationId + "'", dtid);
                    int Nid = int.Parse(dtid.Rows[0][0].ToString());
                    dtVoucherInfo.Rows[0]["Vi_id"] = Database.LocationId + (Nid + 1);
                    dtVoucherInfo.Rows[0]["Nid"] = (Nid + 1);
                    dtVoucherInfo.Rows[0]["LocationId"] = Database.LocationId;
                    Prelocationid = Database.LocationId;
                }
            }

            string invoiceno = vno.ToString();
            dtVoucherInfo.Rows[0]["Invoiceno"] = prefix + invoiceno.PadLeft(padding, '0') + postfix;
            dtVoucherInfo.Rows[0]["Vt_id"] = vtid;
            dtVoucherInfo.Rows[0]["Vnumber"] = vno;
            dtVoucherInfo.Rows[0]["Vdate"] = dateTimePicker1.Value.Date;
            dtVoucherInfo.Rows[0]["Narr"] = "Delivery Billing";
            dtVoucherInfo.Rows[0]["RoffChanged"] = RoffChanged;
            dtVoucherInfo.Rows[0]["Roff"] = 0;
            dtVoucherInfo.Rows[0]["Totalamount"] = textBox8.Text;
            dtVoucherInfo.Rows[0]["Tdtype"] = false;
            dtVoucherInfo.Rows[0]["TaxChanged"] = false;
            dtVoucherInfo.Rows[0]["DR"] = 0;
            dtVoucherInfo.Rows[0]["DD"] = 0;
            dtVoucherInfo.Rows[0]["formC"] = false;
            dtVoucherInfo.Rows[0]["DeliveryType"] = "";
            dtVoucherInfo.Rows[0]["As_Per"] = "";
            dtVoucherInfo.Rows[0]["Delivery_adrs"] = "";
            dtVoucherInfo.Rows[0]["PaymentMode"] = "";
            //dtVoucherInfo.Rows[0]["SId"] = "";
            //dtVoucherInfo.Rows[0]["Consigner_id"] = "";
            dtVoucherInfo.Rows[0]["ac_id"] = funs.Select_ac_id(textBox6.Text);//consignee
            dtVoucherInfo.Rows[0]["Period_from"] = dateTimePicker2.Value.Date;
            dtVoucherInfo.Rows[0]["Period_to"] = dateTimePicker3.Value.Date;
            dtVoucherInfo.Rows[0]["Iscancel"] = iscancel;
            if (vid == "0")
            {
                dtVoucherInfo.Rows[0]["create_date"] = create_date;
                dtVoucherInfo.Rows[0]["CreTime"] = System.DateTime.Now.ToString("HH:mm:ss");
                dtVoucherInfo.Rows[0]["user_id"] = Database.user_id;
            }
            if (vid != "0")
            {
                dtVoucherInfo.Rows[0]["modifyby_id"] = Database.user_id;
            }
            dtVoucherInfo.Rows[0]["modify_date"] = System.DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
            dtVoucherInfo.Rows[0]["ModTime"] = System.DateTime.Now.ToString("HH:mm:ss");

            Database.SaveData(dtVoucherInfo);

            if (vid == "0")
            {

                vid = dtVoucherInfo.Rows[0]["Vi_id"].ToString();
            }

            DataTable dtVoucherdet = new DataTable("Voucherdets");
            Database.GetSqlData("Select * from Voucherdets where Vi_id='" + vid + "'", dtVoucherdet);
            for (int j = 0; j < dtVoucherdet.Rows.Count; j++)
            {
                dtVoucherdet.Rows[j].Delete();
            }
            Database.SaveData(dtVoucherdet);

            dtVoucherdet = new DataTable("Voucherdets");
            Database.GetSqlData("Select * from Voucherdets where Vi_id='" + vid + "'", dtVoucherdet);

            int Nid2 = 1;
            DataTable dtidvd = new DataTable();
            Database.GetSqlData("select max(Nid) as Nid from Voucherdets where locationid='" + Database.LocationId + "'", dtidvd);
            if (dtidvd.Rows[0][0].ToString() != "")
            {
                Nid2 = int.Parse(dtidvd.Rows[0][0].ToString()) + 1;
            }

            for (int i = 0; i < ansGridView1.Rows.Count; i++)
            {
                if (bool.Parse(ansGridView1.Rows[i].Cells["select"].Value.ToString()) == true)
                {
                    dtVoucherdet.Rows.Add();
                    dtVoucherdet.Rows[dtVoucherdet.Rows.Count - 1]["Nid"] = Nid2;
                    dtVoucherdet.Rows[dtVoucherdet.Rows.Count - 1]["LocationId"] = Prelocationid;
                    dtVoucherdet.Rows[dtVoucherdet.Rows.Count - 1]["vd_id"] = Prelocationid + dtVoucherdet.Rows[dtVoucherdet.Rows.Count - 1]["nid"].ToString();
                    dtVoucherdet.Rows[dtVoucherdet.Rows.Count - 1]["Vi_id"] = vid;
                    dtVoucherdet.Rows[dtVoucherdet.Rows.Count - 1]["Itemsr"] = i + 1;



                    dtVoucherdet.Rows[dtVoucherdet.Rows.Count - 1]["Delivery_id"] = ansGridView1.Rows[i].Cells["vi_id"].Value.ToString();
                   
                    dtVoucherdet.Rows[dtVoucherdet.Rows.Count - 1]["remarkreq"] = false;
                    dtVoucherdet.Rows[dtVoucherdet.Rows.Count - 1]["create_date"] = create_date;
                    dtVoucherdet.Rows[dtVoucherdet.Rows.Count - 1]["modify_date"] = System.DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                    dtVoucherdet.Rows[dtVoucherdet.Rows.Count - 1]["multiplier"] = 1;
                    dtVoucherdet.Rows[dtVoucherdet.Rows.Count - 1]["Amount"] = 0;
                    Nid2++;
                }
            }
            for (int i = 0; i < ansGridView1.Rows.Count; i++)
            {
                if (bool.Parse(ansGridView1.Rows[i].Cells["select"].Value.ToString()) == true)
                {

                    Database.CommandExecutor("update voucherinfos set Dbilled='true'  where vi_id='" + ansGridView1.Rows[i].Cells["vi_id"].Value.ToString() + "' ");
                    //    dtVoucherdet.Rows[dtVoucherdet.Rows.Count - 1]["Delivery_id"] = ansGridView1.Rows[i].Cells["vi_id"].Value.ToString();


                }
            }
            Database.SaveData(dtVoucherdet);
            MessageBox.Show("Saved Successfully");



        }

        private void clear()
        {
            if (gStr == "0")
            {
                LoadData("0", "DBill");
            }
            else
            {
                this.Close();
                this.Dispose();
            }
        }

        private void Print()
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

            //print
            dtsidefill.Rows.Add();
            dtsidefill.Rows[1]["Name"] = "Print";
            dtsidefill.Rows[1]["DisplayName"] = "Print";
            dtsidefill.Rows[1]["ShortcutKey"] = "^P";
            dtsidefill.Rows[1]["Visible"] = true;



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
                if (vid != "0")
                {
                    if (bool.Parse(dtVoucherInfo.Rows[0]["Iscancel"].ToString()) == true)
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
                if (vid != "0")
                {
                    if (bool.Parse(dtVoucherInfo.Rows[0]["Iscancel"].ToString()) == true)
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






            //print
            dtsidefill.Rows.Add();
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Name"] = "vnumber";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["DisplayName"] = "Chng BillNo";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["ShortcutKey"] = "^F12";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = true;



            //close
            dtsidefill.Rows.Add();
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Name"] = "quit";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["DisplayName"] = "Quit";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["ShortcutKey"] = "Esc";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = true;

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
        private bool validate()
        {
            if (textBox6.Text == "")
            {
                textBox6.Focus();
                return false;
            }
            if (funs.GenerateVno(vtid, dateTimePicker1.Value.ToString("dd-MMM-yyyy"), vid) == 0 && vno == 0)
            {
                MessageBox.Show("Voucher Number can't be created on this date.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (ansGridView1.Rows.Count == 0)
            {
                MessageBox.Show("Please enter at least one item");
                ansGridView1.Focus();
            }
            int numtype = funs.chkNumType(vtid);
            if (vid != "")
            {
            }
            else if (numtype != 1)
            {
                vid = Database.GetScalarText("Select Vi_id from voucherinfo where Vt_id='" + vtid + "' and Vnumber=" + vno + " and Vdate=" + access_sql.Hash + dateTimePicker1.Value.Date.ToString(Database.dformat) + access_sql.Hash);
            }
            else
            {
                if (vid == "")
                {
                    string tempvid = "";
                    tempvid = Database.GetScalarText("Select Vi_id from voucherinfo where Vt_id='" + vtid + "' and Vnumber=" + vno);
                    if (tempvid != "")
                    {
                        MessageBox.Show("Voucher can't be created on this No.");
                        return false;
                    }
                    else
                    {
                        vid = tempvid;
                    }
                }
            }
            return true;
        }

        private void SetVno()
        {
            int numtype = funs.Select_NumType(vtid);

            if (numtype == 3 && vno != 0 && vid != "0")
            {
                DateTime dt1 = dateTimePicker1.Value;
                DateTime dt2 = DateTime.Parse(Database.GetScalarDate("select vdate from voucherinfo where vi_id='" + vid + "'"));

                if (dt1 != dt2)
                {

                    vno = funs.GenerateVno(vtid, dateTimePicker1.Value.ToString("dd-MMM-yyyy"), vid);
                    label10.Text = vno.ToString();
                }
                return;
            }

            if (vtid == 0 || (vno != 0 && vid != "0"))
            {
                return;
            }
            vno = funs.GenerateVno(vtid, dateTimePicker1.Value.ToString("dd-MMM-yyyy"), vid);
            label10.Text = vno.ToString();
        }
        void btn_Click(object sender, EventArgs e)
        {
            Button tbtn = (Button)sender;
            string name = tbtn.Name.ToString();

            if (name == "save")
            {
                if (validate() == true)
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
            else if (name == "Print")
            {
                if (validate() == true)
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
            else if (name == "quit")
            {
                this.Close();
                this.Dispose();
            }

            else if (name == "cancel")
            {
                iscancel = true;
                if (validate() == true)
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
                if (validate() == true)
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

                    label10.Text = vno.ToString();
                    int numtype = funs.chkNumType(vtid);
                    if (numtype != 1)
                    {
                        vid = Database.GetScalarText("Select Vi_id from voucherinfos where LocationId='" + Database.LocationId + "' and  Vt_id=" + vtid + " and Vnumber=" + vno + " and Vdate=" + access_sql.Hash + dateTimePicker1.Value.Date.ToString(Database.dformat) + access_sql.Hash);
                        if (vid == "")
                        {
                            vid = "0";
                        }
                    }
                    else
                    {
                        string tempvid = "";
                        tempvid = Database.GetScalarText("Select Vi_id from voucherinfos where LocationId='" + Database.LocationId + "' and Vt_id=" + vtid + " and Vnumber=" + vno);
                        if (tempvid != "")
                        {
                            MessageBox.Show("Voucher can't be created on this No.");
                            SetVno();
                            return;
                        }
                    }

                }
                else
                {
                    MessageBox.Show("Invalid password");
                }
            }



        }



        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {

            strCombo = "SELECT   ACCOUNTs_1.name AS Consignee FROM  VOUCHERTYPEs RIGHT OUTER JOIN    VOUCHERINFOs AS VOUCHERINFOs_1 ON  VOUCHERTYPEs.Vt_id = VOUCHERINFOs_1.Vt_id FULL OUTER JOIN  ACCOUNTs AS ACCOUNTs_1 RIGHT OUTER JOIN    Stocks ON ACCOUNTs_1.ac_id =  Stocks.Consignee_id FULL OUTER JOIN    Voucherdets ON  Stocks.vid =  Voucherdets.Vi_id ON VOUCHERINFOs_1.Vi_id =  Voucherdets.Vi_id WHERE   (VOUCHERINFOs_1.LocationId = '" + Database.LocationId + "') GROUP BY  VOUCHERTYPEs.Type, VOUCHERINFOs_1.Vdate, ACCOUNTs_1.name HAVING  ( VOUCHERTYPEs.Type = 'Delivery') AND (VOUCHERINFOs_1.Vdate >= '" + dateTimePicker2.Value.Date.ToString(Database.dformat) + "') AND (VOUCHERINFOs_1.Vdate <= '" + dateTimePicker3.Value.Date.ToString(Database.dformat) + "') ORDER BY Consignee "; 
            textBox6.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
            fillGrid();
        }

    }
}
