using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Data;

namespace faspi
{
    public partial class frm_upload : Form
    {
        OleDbConnection AccessConn = new OleDbConnection();
        OleDbDataAdapter da;
        DataTable dt;
        DataTable dtVoucherInfo;
        DataTable dtVoucherdet;
        DataTable dtVoucherCharges;

        public frm_upload()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AccessConn.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" +Database.ServerPath + "\\Database\\test.mdb;Persist Security Info=true;Jet OLEDB:Database Password=ptsoft9358524971";
            AccessConn.Open();
             dt = new DataTable();

            da = new OleDbDataAdapter("select * from gr order by GRNo", AccessConn);
            da.SelectCommand.CommandTimeout = 180;
            da.Fill(dt);


            for (int i = 500; i < 860; i++)
            {
                dtVoucherInfo = new DataTable("Voucherinfos");
                Database.GetSqlData("select * from Voucherinfos where  Vi_id='0'", dtVoucherInfo);

                if (dtVoucherInfo.Rows.Count == 0)
                {
                    dtVoucherInfo.Rows.Add();

                    string vid = "";
                    DataTable dtCount = new DataTable();
                    Database.GetSqlData("select count(*) from VOUCHERINFOs where locationid='" + Database.LocationId + "'", dtCount);

                    if (int.Parse(dtCount.Rows[0][0].ToString()) == 0)
                    {
                        dtVoucherInfo.Rows[0]["Vi_id"] = Database.LocationId + "1";
                        dtVoucherInfo.Rows[0]["Nid"] = 1;                        
                    }
                    else
                    {
                        DataTable dtid = new DataTable();
                        Database.GetSqlData("select max(Nid) as Nid from VOUCHERINFOs where locationid='" + Database.LocationId + "'", dtid);
                        int Nid = int.Parse(dtid.Rows[0][0].ToString());
                        dtVoucherInfo.Rows[0]["Vi_id"] = Database.LocationId + (Nid + 1);
                        dtVoucherInfo.Rows[0]["Nid"] = (Nid + 1);                       
                    }

                    string prefix = "";
                    string postfix = "";
                    int padding = 0;

                    prefix = Database.GetScalarText("Select Booking_prefix from Location where LocationId='" + Database.LocationId + "'");
                    vid = dtVoucherInfo.Rows[0]["Vi_id"].ToString();
                    string invoiceno = dt.Rows[i]["GRNo"].ToString();                    
                    dtVoucherInfo.Rows[0]["Vt_id"] = 86;
                    dtVoucherInfo.Rows[0]["Vnumber"] = int.Parse(dt.Rows[i]["GRNo"].ToString());
                    dtVoucherInfo.Rows[0]["Ac_id"] = dt.Rows[i]["CONSIGNER"].ToString();
                    dtVoucherInfo.Rows[0]["Conn_id"] = 0;
                    dtVoucherInfo.Rows[0]["Vdate"] = DateTime.Parse(dt.Rows[i]["Date"].ToString()).ToString("dd-MMM-yyyy");
                    dtVoucherInfo.Rows[0]["Duedate"] = DateTime.Parse(dt.Rows[i]["Date"].ToString()).ToString("dd-MMM-yyyy");
                    dtVoucherInfo.Rows[0]["TaxableAmount"] = 0;
                    dtVoucherInfo.Rows[0]["Roff"] = 0;
                    if (dt.Rows[i]["STCH"].ToString() == "")
                    {
                        dt.Rows[i]["STCH"] =0;
                    }
                    dtVoucherInfo.Rows[0]["Totalamount"] = double.Parse(dt.Rows[i]["Freight"].ToString()) + double.Parse(dt.Rows[i]["HAMALI"].ToString()) + double.Parse(dt.Rows[i]["LC"].ToString()) + double.Parse(dt.Rows[i]["PF"].ToString()) + double.Parse(dt.Rows[i]["DD"].ToString()) + double.Parse(dt.Rows[i]["STCH"].ToString());
                    dtVoucherInfo.Rows[0]["Narr"] = "Booking";
                    dtVoucherInfo.Rows[0]["Formno"] = "";
                    dtVoucherInfo.Rows[0]["Tdtype"] = false;
                    dtVoucherInfo.Rows[0]["Svnum"] = "";
                    dtVoucherInfo.Rows[0]["Svdate"] = DateTime.Parse(dt.Rows[i]["Date"].ToString()).ToString("dd-MMM-yyyy");
                    dtVoucherInfo.Rows[0]["RoffChanged"] = false;
                    dtVoucherInfo.Rows[0]["TaxChanged"] = false;
                    dtVoucherInfo.Rows[0]["Transport1"] = dt.Rows[i]["PRIVATEMARK"].ToString();
                    dtVoucherInfo.Rows[0]["Transport2"] = "";
                    dtVoucherInfo.Rows[0]["DeliveryAt"] = "";
                    dtVoucherInfo.Rows[0]["Grno"] = "";
                    dtVoucherInfo.Rows[0]["formC"] = false;
                    dtVoucherInfo.Rows[0]["Transport3"] = "";
                    dtVoucherInfo.Rows[0]["Transport4"] = "";
                    dtVoucherInfo.Rows[0]["Transport5"] = "";
                    dtVoucherInfo.Rows[0]["Transport6"] = "";
                    dtVoucherInfo.Rows[0]["rate"] = 0;
                    dtVoucherInfo.Rows[0]["printcount"] = 0;
                    dtVoucherInfo.Rows[0]["uploaddoc"] = "";
                    dtVoucherInfo.Rows[0]["Ac_id2"] = dt.Rows[i]["CONSIGNEE"].ToString();
                    dtVoucherInfo.Rows[0]["Invoiceno"] = prefix + invoiceno.PadLeft(padding, '0') + postfix;
                    dtVoucherInfo.Rows[0]["user_id"] = funs.Select_user_id(dt.Rows[i]["USERNAME"].ToString());
                    dtVoucherInfo.Rows[0]["CreTime"] = DateTime.Parse(dt.Rows[i]["CreateTime"].ToString()).ToString("HH:mm:ss");
                    dtVoucherInfo.Rows[0]["ModTime"] = DateTime.Parse(dt.Rows[i]["CreateTime"].ToString()).ToString("HH:mm:ss");
                    
                    if (dt.Rows[i]["PYMTMODE"].ToString() == "T.B.B.")
                    {
                        dtVoucherInfo.Rows[0]["PaymentMode"] = "T.B.B.";
                    }
                    else
                    {
                        dtVoucherInfo.Rows[0]["PaymentMode"] = dt.Rows[i]["PYMTMODE"].ToString();
                    }                    
                    dtVoucherInfo.Rows[0]["SId"] = dt.Rows[i]["DEST"].ToString();
                    dtVoucherInfo.Rows[0]["DeliveryType"] = dt.Rows[i]["DelType"].ToString();
                    dtVoucherInfo.Rows[0]["LocationId"] = Database.LocationId;
                    dtVoucherInfo.Rows[0]["Consigner_id"] = "SER78";
                    dtVoucherInfo.Rows[0]["As_Per"] = "Consigner";
                    dtVoucherInfo.Rows[0]["Delivery_adrs"] = dt.Rows[i]["DELIVERYAT"].ToString();
                    dtVoucherInfo.Rows[0]["Driver_name"] = "";
                    dtVoucherInfo.Rows[0]["DR"] = 0;
                    dtVoucherInfo.Rows[0]["DD"] = 0;
                    dtVoucherInfo.Rows[0]["create_date"] = DateTime.Parse(dt.Rows[i]["CreateDate"].ToString()).ToString("dd-MM-yyyy HH:mm:ss");
                    dtVoucherInfo.Rows[0]["modify_date"] = DateTime.Parse(dt.Rows[i]["CreateDate"].ToString()).ToString("dd-MM-yyyy HH:mm:ss");
                    dtVoucherInfo.Rows[0]["Gaddi_id"] = "";
                    dtVoucherInfo.Rows[0]["isself"] = false;
                    dtVoucherInfo.Rows[0]["db_id"] = "";
                    dtVoucherInfo.Rows[0]["remarks"] = "";
                    dtVoucherInfo.Rows[0]["Dr_ac_id"] = "";
                    dtVoucherInfo.Rows[0]["Cr_ac_id"] = "";
                    Database.SaveData(dtVoucherInfo);

                    dtVoucherdet = new DataTable("Voucherdets");
                    Database.GetSqlData("Select * from Voucherdets where Vi_id='" + vid + "'", dtVoucherdet);

                    if (dtVoucherdet.Rows.Count == 0)
                    {
                        dtVoucherdet.Rows.Add();
                        int Nid2 = 1;
                        DataTable dtidvd = new DataTable();
                        Database.GetSqlData("select max(Nid) as Nid from Voucherdets where locationid='" + Database.LocationId + "'", dtidvd);
                        if (dtidvd.Rows[0][0].ToString() != "")
                        {
                            Nid2 = int.Parse(dtidvd.Rows[0][0].ToString()) + 1;
                        }

                        dtVoucherdet.Rows[0]["Nid"] = Nid2;
                        dtVoucherdet.Rows[0]["vd_id"] = Database.LocationId + dtVoucherdet.Rows[0]["nid"].ToString();
                        dtVoucherdet.Rows[0]["Vi_id"] = vid;
                        dtVoucherdet.Rows[0]["Des_ac_id"] = dt.Rows[i]["ItemName"].ToString();
                        dtVoucherdet.Rows[0]["ItemSr"] = 1;
                        dtVoucherdet.Rows[0]["rate_am"] = double.Parse(dt.Rows[i]["Freight"].ToString());
                        dtVoucherdet.Rows[0]["Quantity"] = double.Parse(dt.Rows[i]["Qty"].ToString());
                        dtVoucherdet.Rows[0]["Description"] = funs.Select_item_nm(dt.Rows[i]["ItemName"].ToString());
                        dtVoucherdet.Rows[0]["Amount"] = double.Parse(dt.Rows[i]["Freight"].ToString());
                        dtVoucherdet.Rows[0]["Taxabelamount"] = 0;
                        dtVoucherdet.Rows[0]["Commission%"] = 0;
                        dtVoucherdet.Rows[0]["Commission@"] = 0;
                        dtVoucherdet.Rows[0]["Category_Id"] = 0;
                        dtVoucherdet.Rows[0]["weight"] = double.Parse(dt.Rows[i]["ActualWT"].ToString());
                        dtVoucherdet.Rows[0]["qd"] = 0;
                        dtVoucherdet.Rows[0]["cd"] = 0;
                        dtVoucherdet.Rows[0]["Batch_Code"] = "";
                        dtVoucherdet.Rows[0]["Cost"] = 0;
                        dtVoucherdet.Rows[0]["MRP"] = 0;
                        dtVoucherdet.Rows[0]["godown_id"] = "";
                        dtVoucherdet.Rows[0]["Rate_Unit"] = "";
                        dtVoucherdet.Rows[0]["Pvalue"] = 0;
                        dtVoucherdet.Rows[0]["packing"] = funs.Select_packing_nm(dt.Rows[i]["Packing"].ToString());
                        dtVoucherdet.Rows[0]["orgpacking"] = dtVoucherdet.Rows[0]["packing"].ToString();
                        dtVoucherdet.Rows[0]["comqty"] = "";
                        dtVoucherdet.Rows[0]["pur_sale_acc"] = "";
                        dtVoucherdet.Rows[0]["tax1"] = "";
                        dtVoucherdet.Rows[0]["tax2"] = "";
                        dtVoucherdet.Rows[0]["tax3"] = "";
                        dtVoucherdet.Rows[0]["tax4"] = "";
                        dtVoucherdet.Rows[0]["rate1"] = 0;
                        dtVoucherdet.Rows[0]["rate2"] = 0;
                        dtVoucherdet.Rows[0]["rate3"] = 0;
                        dtVoucherdet.Rows[0]["rate4"] = 0;
                        dtVoucherdet.Rows[0]["taxamt1"] = 0;
                        dtVoucherdet.Rows[0]["taxamt2"] = 0;
                        dtVoucherdet.Rows[0]["taxamt3"] = 0;
                        dtVoucherdet.Rows[0]["taxamt4"] = 0;
                        dtVoucherdet.Rows[0]["bottomdis"] = 0;
                        dtVoucherdet.Rows[0]["remark1"] = "";
                        dtVoucherdet.Rows[0]["remark2"] = "";
                        dtVoucherdet.Rows[0]["remarkreq"] = false;
                        dtVoucherdet.Rows[0]["ChargedWeight"] = double.Parse(dt.Rows[i]["CHRWT"].ToString());
                        dtVoucherdet.Rows[0]["Per"] = "Flat";
                        dtVoucherdet.Rows[0]["exp1rate"] = double.Parse(dt.Rows[i]["STCH"].ToString());
                        dtVoucherdet.Rows[0]["exp2rate"] = double.Parse(dt.Rows[i]["HAMALI"].ToString());
                        dtVoucherdet.Rows[0]["exp3rate"] = double.Parse(dt.Rows[i]["LC"].ToString());
                        dtVoucherdet.Rows[0]["exp4rate"] = double.Parse(dt.Rows[i]["DD"].ToString());
                        dtVoucherdet.Rows[0]["exp5rate"] = 0;
                        dtVoucherdet.Rows[0]["exp6rate"] = 0;
                        dtVoucherdet.Rows[0]["exp7rate"] = 0;
                        dtVoucherdet.Rows[0]["exp8rate"] = double.Parse(dt.Rows[i]["PF"].ToString());
                        dtVoucherdet.Rows[0]["exp9rate"] = 0;
                        dtVoucherdet.Rows[0]["exp10rate"] = 0;
                        dtVoucherdet.Rows[0]["exp1amt"] = double.Parse(dt.Rows[i]["STCH"].ToString());
                        dtVoucherdet.Rows[0]["exp2amt"] = double.Parse(dt.Rows[i]["HAMALI"].ToString());
                        dtVoucherdet.Rows[0]["exp3amt"] = double.Parse(dt.Rows[i]["LC"].ToString());
                        dtVoucherdet.Rows[0]["exp4amt"] = double.Parse(dt.Rows[i]["DD"].ToString());
                        dtVoucherdet.Rows[0]["exp5amt"] = 0;
                        dtVoucherdet.Rows[0]["exp6amt"] = 0;
                        dtVoucherdet.Rows[0]["exp7amt"] = 0;
                        dtVoucherdet.Rows[0]["exp8amt"] = double.Parse(dt.Rows[i]["PF"].ToString());
                        dtVoucherdet.Rows[0]["exp9amt"] = 0;
                        dtVoucherdet.Rows[0]["exp10amt"] = 0;
                        dtVoucherdet.Rows[0]["exp1mr"] = 0;
                        dtVoucherdet.Rows[0]["exp2mr"] = 0;
                        dtVoucherdet.Rows[0]["exp3mr"] = 0;
                        dtVoucherdet.Rows[0]["exp4mr"] = 0;
                        dtVoucherdet.Rows[0]["exp5mr"] = 0;
                        dtVoucherdet.Rows[0]["exp6mr"] = 0;
                        dtVoucherdet.Rows[0]["exp7mr"] = 0;
                        dtVoucherdet.Rows[0]["exp8mr"] = 0;
                        dtVoucherdet.Rows[0]["exp9mr"] = 0;
                        dtVoucherdet.Rows[0]["exp10mr"] = 0;
                        dtVoucherdet.Rows[0]["bharti"] = funs.Select_item_bharti(funs.Select_item_nm(dt.Rows[i]["ItemName"].ToString()));
                        dtVoucherdet.Rows[0]["freightmr"] = 0;
                        dtVoucherdet.Rows[0]["totexp"] = double.Parse(dt.Rows[i]["HAMALI"].ToString()) + double.Parse(dt.Rows[i]["LC"].ToString()) + double.Parse(dt.Rows[i]["DD"].ToString()) + double.Parse(dt.Rows[i]["PF"].ToString()) + double.Parse(dt.Rows[i]["STCH"].ToString());
                        dtVoucherdet.Rows[0]["ItemAmount"] = double.Parse(dt.Rows[i]["Freight"].ToString()) + double.Parse(dt.Rows[i]["HAMALI"].ToString()) + double.Parse(dt.Rows[i]["LC"].ToString()) + double.Parse(dt.Rows[i]["PF"].ToString()) + double.Parse(dt.Rows[i]["DD"].ToString()) + double.Parse(dt.Rows[i]["STCH"].ToString());
                        dtVoucherdet.Rows[0]["exp1type"] = "Flat";
                        dtVoucherdet.Rows[0]["exp2type"] = "Flat";
                        dtVoucherdet.Rows[0]["exp3type"] = "Flat";
                        dtVoucherdet.Rows[0]["exp4type"] = "Flat";
                        dtVoucherdet.Rows[0]["exp5type"] = "Flat";
                        dtVoucherdet.Rows[0]["exp6type"] = "Flat";
                        dtVoucherdet.Rows[0]["exp7type"] = "Flat";
                        dtVoucherdet.Rows[0]["exp8type"] = "Flat";
                        dtVoucherdet.Rows[0]["exp9type"] = "Flat";
                        dtVoucherdet.Rows[0]["exp10type"] = "Flat";
                        dtVoucherdet.Rows[0]["LocationId"] = Database.LocationId;
                        dtVoucherdet.Rows[0]["Booking_id"] = "";
                        dtVoucherdet.Rows[0]["consigner_id"] = "";
                        dtVoucherdet.Rows[0]["consignee_id"] = "";
                        dtVoucherdet.Rows[0]["grno"] = "";
                        dtVoucherdet.Rows[0]["booking_date"] = DateTime.Parse(dt.Rows[i]["Date"].ToString()).ToString("dd-MMM-yyyy");
                        dtVoucherdet.Rows[0]["multiplier"] = 1;
                        dtVoucherdet.Rows[0]["create_date"] = DateTime.Parse(dt.Rows[i]["CreateDate"].ToString()).ToString("dd-MM-yyyy HH:mm:ss");
                        dtVoucherdet.Rows[0]["modify_date"] = DateTime.Parse(dt.Rows[i]["CreateDate"].ToString()).ToString("dd-MM-yyyy HH:mm:ss");
                        dtVoucherdet.Rows[0]["Bill_booking_id"] = "";
                        dtVoucherdet.Rows[0]["Challan_id"] = "";
                        dtVoucherdet.Rows[0]["ch_id"] = "";
                        Database.SaveData(dtVoucherdet);
                    }

                     dtVoucherCharges = new DataTable("VOUCHARGESs");
                    Database.GetSqlData("Select * from VOUCHARGESs where Vi_id='" + vid + "'", dtVoucherCharges);

                    DataTable dtidv = new DataTable();
                    Database.GetSqlData("select max(Nid) as Nid from VOUCHARGESs where locationid='" + Database.LocationId + "'", dtidv);
                    int Nid3 = 1;
                    if (dtidv.Rows[0][0].ToString() != "")
                    {
                        Nid3 = int.Parse(dtidv.Rows[0][0].ToString()) + 1;
                    }

                    int sno = 0;

                    dtVoucherCharges.Rows.Add();
                    dtVoucherCharges.Rows[dtVoucherCharges.Rows.Count - 1]["Nid"] = Nid3;
                    dtVoucherCharges.Rows[dtVoucherCharges.Rows.Count - 1]["vc_id"] = Database.LocationId + dtVoucherCharges.Rows[dtVoucherCharges.Rows.Count - 1]["Nid"].ToString();
                    dtVoucherCharges.Rows[dtVoucherCharges.Rows.Count - 1]["Vi_id"] = vid;
                    dtVoucherCharges.Rows[dtVoucherCharges.Rows.Count - 1]["Srno"] = sno;
                    dtVoucherCharges.Rows[dtVoucherCharges.Rows.Count - 1]["Charg_Name"] = "Freight";
                    dtVoucherCharges.Rows[dtVoucherCharges.Rows.Count - 1]["Amount"] = 0;
                    dtVoucherCharges.Rows[dtVoucherCharges.Rows.Count - 1]["locationid"] = Database.LocationId;
                    dtVoucherCharges.Rows[dtVoucherCharges.Rows.Count - 1]["create_date"] = DateTime.Parse(dt.Rows[i]["CreateDate"].ToString()).ToString("dd-MM-yyyy HH:mm:ss");
                    dtVoucherCharges.Rows[dtVoucherCharges.Rows.Count - 1]["modify_date"] = DateTime.Parse(dt.Rows[i]["CreateDate"].ToString()).ToString("dd-MM-yyyy HH:mm:ss");
                    Nid3++;
                    sno++;

                    if (double.Parse(dt.Rows[i]["STCH"].ToString()) > 0.00)
                    {
                        dtVoucherCharges.Rows.Add();
                        dtVoucherCharges.Rows[dtVoucherCharges.Rows.Count - 1]["Nid"] = Nid3;
                        dtVoucherCharges.Rows[dtVoucherCharges.Rows.Count - 1]["vc_id"] = Database.LocationId + dtVoucherCharges.Rows[dtVoucherCharges.Rows.Count - 1]["Nid"].ToString();
                        dtVoucherCharges.Rows[dtVoucherCharges.Rows.Count - 1]["Vi_id"] = vid;
                        dtVoucherCharges.Rows[dtVoucherCharges.Rows.Count - 1]["Srno"] = sno;
                        dtVoucherCharges.Rows[dtVoucherCharges.Rows.Count - 1]["Charg_Name"] = "G.R. Charge";
                        dtVoucherCharges.Rows[dtVoucherCharges.Rows.Count - 1]["Amount"] = double.Parse(dt.Rows[i]["STCH"].ToString());
                        dtVoucherCharges.Rows[dtVoucherCharges.Rows.Count - 1]["locationid"] = Database.LocationId;
                        dtVoucherCharges.Rows[dtVoucherCharges.Rows.Count - 1]["create_date"] = DateTime.Parse(dt.Rows[i]["CreateDate"].ToString()).ToString("dd-MM-yyyy HH:mm:ss");
                        dtVoucherCharges.Rows[dtVoucherCharges.Rows.Count - 1]["modify_date"] = DateTime.Parse(dt.Rows[i]["CreateDate"].ToString()).ToString("dd-MM-yyyy HH:mm:ss");
                        Nid3++;
                        sno++;
                    }

                    if (double.Parse(dt.Rows[i]["HAMALI"].ToString()) > 0.00)
                    {
                        dtVoucherCharges.Rows.Add();
                        dtVoucherCharges.Rows[dtVoucherCharges.Rows.Count - 1]["Nid"] = Nid3;
                        dtVoucherCharges.Rows[dtVoucherCharges.Rows.Count - 1]["vc_id"] = Database.LocationId + dtVoucherCharges.Rows[dtVoucherCharges.Rows.Count - 1]["Nid"].ToString();
                        dtVoucherCharges.Rows[dtVoucherCharges.Rows.Count - 1]["Vi_id"] = vid;
                        dtVoucherCharges.Rows[dtVoucherCharges.Rows.Count - 1]["Srno"] = sno;
                        dtVoucherCharges.Rows[dtVoucherCharges.Rows.Count - 1]["Charg_Name"] = "Hamali";
                        dtVoucherCharges.Rows[dtVoucherCharges.Rows.Count - 1]["Amount"] = double.Parse(dt.Rows[i]["HAMALI"].ToString());
                        dtVoucherCharges.Rows[dtVoucherCharges.Rows.Count - 1]["locationid"] = Database.LocationId;
                        dtVoucherCharges.Rows[dtVoucherCharges.Rows.Count - 1]["create_date"] = DateTime.Parse(dt.Rows[i]["CreateDate"].ToString()).ToString("dd-MM-yyyy HH:mm:ss");
                        dtVoucherCharges.Rows[dtVoucherCharges.Rows.Count - 1]["modify_date"] = DateTime.Parse(dt.Rows[i]["CreateDate"].ToString()).ToString("dd-MM-yyyy HH:mm:ss");
                        Nid3++;
                        sno++;
                    }

                    if (double.Parse(dt.Rows[i]["LC"].ToString()) > 0.00)
                    {
                        dtVoucherCharges.Rows.Add();
                        dtVoucherCharges.Rows[dtVoucherCharges.Rows.Count - 1]["Nid"] = Nid3;
                        dtVoucherCharges.Rows[dtVoucherCharges.Rows.Count - 1]["vc_id"] = Database.LocationId + dtVoucherCharges.Rows[dtVoucherCharges.Rows.Count - 1]["Nid"].ToString();
                        dtVoucherCharges.Rows[dtVoucherCharges.Rows.Count - 1]["Vi_id"] = vid;
                        dtVoucherCharges.Rows[dtVoucherCharges.Rows.Count - 1]["Srno"] = sno;
                        dtVoucherCharges.Rows[dtVoucherCharges.Rows.Count - 1]["Charg_Name"] = "Local Cartage";
                        dtVoucherCharges.Rows[dtVoucherCharges.Rows.Count - 1]["Amount"] = double.Parse(dt.Rows[i]["LC"].ToString());
                        dtVoucherCharges.Rows[dtVoucherCharges.Rows.Count - 1]["locationid"] = Database.LocationId;
                        dtVoucherCharges.Rows[dtVoucherCharges.Rows.Count - 1]["create_date"] = DateTime.Parse(dt.Rows[i]["CreateDate"].ToString()).ToString("dd-MM-yyyy HH:mm:ss");
                        dtVoucherCharges.Rows[dtVoucherCharges.Rows.Count - 1]["modify_date"] = DateTime.Parse(dt.Rows[i]["CreateDate"].ToString()).ToString("dd-MM-yyyy HH:mm:ss");
                        Nid3++;
                        sno++;
                    }

                    if (double.Parse(dt.Rows[i]["DD"].ToString()) > 0.00)
                    {
                        dtVoucherCharges.Rows.Add();
                        dtVoucherCharges.Rows[dtVoucherCharges.Rows.Count - 1]["Nid"] = Nid3;
                        dtVoucherCharges.Rows[dtVoucherCharges.Rows.Count - 1]["vc_id"] = Database.LocationId + dtVoucherCharges.Rows[dtVoucherCharges.Rows.Count - 1]["Nid"].ToString();
                        dtVoucherCharges.Rows[dtVoucherCharges.Rows.Count - 1]["Vi_id"] = vid;
                        dtVoucherCharges.Rows[dtVoucherCharges.Rows.Count - 1]["Srno"] = sno;
                        dtVoucherCharges.Rows[dtVoucherCharges.Rows.Count - 1]["Charg_Name"] = "Door Delivery";
                        dtVoucherCharges.Rows[dtVoucherCharges.Rows.Count - 1]["Amount"] = double.Parse(dt.Rows[i]["DD"].ToString());
                        dtVoucherCharges.Rows[dtVoucherCharges.Rows.Count - 1]["locationid"] = Database.LocationId;
                        dtVoucherCharges.Rows[dtVoucherCharges.Rows.Count - 1]["create_date"] = DateTime.Parse(dt.Rows[i]["CreateDate"].ToString()).ToString("dd-MM-yyyy HH:mm:ss");
                        dtVoucherCharges.Rows[dtVoucherCharges.Rows.Count - 1]["modify_date"] = DateTime.Parse(dt.Rows[i]["CreateDate"].ToString()).ToString("dd-MM-yyyy HH:mm:ss");
                        Nid3++;
                        sno++;
                    }

                    if (double.Parse(dt.Rows[i]["PF"].ToString()) > 0.00)
                    {
                        dtVoucherCharges.Rows.Add();
                        dtVoucherCharges.Rows[dtVoucherCharges.Rows.Count - 1]["Nid"] = Nid3;
                        dtVoucherCharges.Rows[dtVoucherCharges.Rows.Count - 1]["vc_id"] = Database.LocationId + dtVoucherCharges.Rows[dtVoucherCharges.Rows.Count - 1]["Nid"].ToString();
                        dtVoucherCharges.Rows[dtVoucherCharges.Rows.Count - 1]["Vi_id"] = vid;
                        dtVoucherCharges.Rows[dtVoucherCharges.Rows.Count - 1]["Srno"] = sno;
                        dtVoucherCharges.Rows[dtVoucherCharges.Rows.Count - 1]["Charg_Name"] = "Other8";
                        dtVoucherCharges.Rows[dtVoucherCharges.Rows.Count - 1]["Amount"] = double.Parse(dt.Rows[i]["PF"].ToString());
                        dtVoucherCharges.Rows[dtVoucherCharges.Rows.Count - 1]["locationid"] = Database.LocationId;
                        dtVoucherCharges.Rows[dtVoucherCharges.Rows.Count - 1]["create_date"] = DateTime.Parse(dt.Rows[i]["CreateDate"].ToString()).ToString("dd-MM-yyyy HH:mm:ss");
                        dtVoucherCharges.Rows[dtVoucherCharges.Rows.Count - 1]["modify_date"] = DateTime.Parse(dt.Rows[i]["CreateDate"].ToString()).ToString("dd-MM-yyyy HH:mm:ss");
                    }

                    Database.SaveData(dtVoucherCharges);
                }
            }

            MessageBox.Show("Done");
            this.Close();
            this.Dispose();
        }
    }
}
