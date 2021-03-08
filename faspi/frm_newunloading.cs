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
    public partial class frm_newunloading : Form
    {
        string gch_vid = "";
        int vtid;
        int vno = 0;
        string gStr = "";
        string vid = "";
        DataTable dtVoucherinfo;
        DataTable dtchallanunl;
        public Boolean gresave = false;
        string Prelocationid = "";
        Boolean RoffChanged = false;
        bool iscancel = false;
        string strCombo = "";
        DateTime create_date = DateTime.Parse(System.DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"));

        public frm_newunloading()
        {
            InitializeComponent();
            dateTimePicker1.CustomFormat = Database.dformat;
            dateTimePicker1.MaxDate = Database.ldate;
            dateTimePicker1.MinDate = Database.stDate;

            dateTimePicker2.CustomFormat = Database.dformat;
            dateTimePicker2.MaxDate = Database.ldate;
            dateTimePicker2.MinDate = Database.stDate;
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
        }


        public void LoadData(string vi_id, string frmcaption)
        {
            gStr = vi_id.ToString();
            vid = vi_id;
            vtid = funs.Select_vt_id("GRByChallan");
            if (vid == "0")
            {
                SetVno();
            }
            foreach (DataGridViewColumn column in ansGridView1.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            ansGridView1.Columns["Quantity"].CellTemplate.ValueType = typeof(double);
            ansGridView1.Columns["weight"].CellTemplate.ValueType = typeof(double);
            ansGridView1.Columns["actweight"].CellTemplate.ValueType = typeof(double);
            ansGridView1.Columns["freight"].CellTemplate.ValueType = typeof(double);
            ansGridView1.Columns["grcharge"].CellTemplate.ValueType = typeof(double);
            ansGridView1.Columns["othcharge"].CellTemplate.ValueType = typeof(double);

            dtVoucherinfo = new DataTable("VOUCHERINFOs");
            Database.GetSqlData("select * from VOUCHERINFOs where vi_id='" + vid + "'", dtVoucherinfo);
            dtchallanunl = new DataTable("ChallanUnloadings");
            Database.GetSqlData("select * from ChallanUnloadings where vi_id='" + vid + "' order by Itemsr", dtchallanunl);



            if (dtVoucherinfo.Rows.Count == 0)
            {
                if (dtVoucherinfo.Rows.Count == 0)
                {
                    dtVoucherinfo.Rows.Add();
                }
                dateTimePicker1.Value = Database.ldate;
                dateTimePicker2.Value = Database.ldate;
                label1.Text = vno.ToString();
                textBox1.Text = "";
                textBox2.Text = "0";
                textBox3.Text = "";
                textBox4.Text = "";
                textBox5.Text = "0";
                textBox6.Text = "0";
                textBox7.Text = "0";
                textBox8.Text = "0";
                textBox9.Text = "0";
                textBox10.Text = "0";
                textBox11.Text = "0";
                txtTruckNo.Text = "";
                ansGridView1.Rows.Clear();
                iscancel = false;
            
            }
            else
            {
                vno = int.Parse(dtVoucherinfo.Rows[0]["Vnumber"].ToString());
                label1.Text = vno.ToString();
                dateTimePicker1.Value = DateTime.Parse(dtVoucherinfo.Rows[0]["Vdate"].ToString());
                dateTimePicker2.Value = DateTime.Parse(dtVoucherinfo.Rows[0]["Duedate"].ToString());
                create_date = DateTime.Parse(dtVoucherinfo.Rows[0]["create_date"].ToString());
                label1.Text = dtVoucherinfo.Rows[0]["Vnumber"].ToString();
                textBox1.Text = dtVoucherinfo.Rows[0]["Narr"].ToString();
                textBox2.Text = double.Parse(dtVoucherinfo.Rows[0]["totalamount"].ToString()).ToString();
                txtTruckNo.Text = funs.Select_gaddi_nm(dtVoucherinfo.Rows[0]["Gaddi_id"].ToString());
                textBox3.Text = funs.Select_ac_nm(dtVoucherinfo.Rows[0]["Driver_name"].ToString());
                textBox4.Text = funs.Select_ac_nm(dtVoucherinfo.Rows[0]["transporter_id"].ToString());


                for (int i = 0; i < dtchallanunl.Rows.Count; i++)
                {
                    ansGridView1.Rows.Add();
                    ansGridView1.Rows[i].Cells["sno"].Value = dtchallanunl.Rows[i]["ItemSr"];
                    ansGridView1.Rows[i].Cells["grno1"].Value = dtchallanunl.Rows[i]["grno"].ToString();
                    ansGridView1.Rows[i].Cells["destination1"].Value =funs.Select_dp_nm(dtchallanunl.Rows[i]["destination_id"].ToString());
                    ansGridView1.Rows[i].Cells["consigner1"].Value =funs.Select_ac_nm(dtchallanunl.Rows[i]["consigner_id"].ToString());
                    ansGridView1.Rows[i].Cells["source1"].Value = funs.Select_dp_nm(dtchallanunl.Rows[i]["source_id"].ToString());
                    ansGridView1.Rows[i].Cells["consignee1"].Value = funs.Select_ac_nm(dtchallanunl.Rows[i]["consignee_id"].ToString());
                    ansGridView1.Rows[i].Cells["description"].Value = funs.Select_item_nm(dtchallanunl.Rows[i]["Des_ac_id"].ToString());
                    ansGridView1.Rows[i].Cells["unt"].Value = dtchallanunl.Rows[i]["packing"];

                    ansGridView1.Rows[i].Cells["private"].Value = dtchallanunl.Rows[i]["private"];
                    ansGridView1.Rows[i].Cells["remark"].Value = dtchallanunl.Rows[i]["remark"];

                    ansGridView1.Rows[i].Cells["grdate"].Value = DateTime.Parse(dtchallanunl.Rows[i]["grdate"].ToString()).ToString(Database.dformat);
                    ansGridView1.Rows[i].Cells["grtype"].Value = dtchallanunl.Rows[i]["grtype"];
                    ansGridView1.Rows[i].Cells["deliverytype"].Value = dtchallanunl.Rows[i]["deliverytype"];


                    ansGridView1.Rows[i].Cells["Quantity"].Value = funs.DecimalPoint(dtchallanunl.Rows[i]["Quantity"], 2);
                    ansGridView1.Rows[i].Cells["actweight"].Value = funs.DecimalPoint(dtchallanunl.Rows[i]["actweight"], 3);
                    ansGridView1.Rows[i].Cells["weight"].Value = funs.DecimalPoint(dtchallanunl.Rows[i]["weight"], 3);
                    ansGridView1.Rows[i].Cells["freight"].Value = funs.DecimalPoint(dtchallanunl.Rows[i]["Rate_am"], 2);
                   
                    ansGridView1.Rows[i].Cells["gramt"].Value = funs.DecimalPoint(dtchallanunl.Rows[i]["Amount"], 2);
                    ansGridView1.Rows[i].Cells["grCharge"].Value = funs.DecimalPoint(dtchallanunl.Rows[i]["grCharge"], 2);
                    ansGridView1.Rows[i].Cells["Othcharge"].Value = funs.DecimalPoint(dtchallanunl.Rows[i]["Othcharge"], 2);
                  
                }
                calc();
               
            }
            if (gresave == true)
            {
                object sender = new object();
                EventArgs e = new EventArgs();
                btn_Click(sender, e);
            }
           
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

            string prefix = "";
            string postfix = "";
            int padding = 0;
            string invoiceno = vno.ToString();
            prefix = Database.GetScalarText("Select prefix from Location where LocationId='" + Database.LocationId + "'");
            dtVoucherinfo.Rows[0]["Invoiceno"] = prefix + invoiceno.PadLeft(padding, '0') + postfix;
            dtVoucherinfo.Rows[0]["transporter_id"] = funs.Select_ac_id(textBox4.Text);
            dtVoucherinfo.Rows[0]["Vdate"] = dateTimePicker1.Value.Date;
            dtVoucherinfo.Rows[0]["Duedate"] = dateTimePicker2.Value.Date;
            dtVoucherinfo.Rows[0]["Driver_name"] = funs.Select_ac_id(textBox3.Text);
            dtVoucherinfo.Rows[0]["Vnumber"] = label1.Text;
            dtVoucherinfo.Rows[0]["RoffChanged"] = RoffChanged;
            dtVoucherinfo.Rows[0]["Tdtype"] = false;
            dtVoucherinfo.Rows[0]["Vt_id"] = vtid;
            dtVoucherinfo.Rows[0]["iscancel"] = iscancel;
            dtVoucherinfo.Rows[0]["Narr"] = textBox1.Text;
            dtVoucherinfo.Rows[0]["Gaddi_id"] = funs.Select_gaddi_id(txtTruckNo.Text);
            dtVoucherinfo.Rows[0]["totalamount"] =  textBox2.Text;
            dtVoucherinfo.Rows[0]["roff"] = 0;
            dtVoucherinfo.Rows[0]["Transport1"] = "";
            dtVoucherinfo.Rows[0]["Transport2"] = "";
            dtVoucherinfo.Rows[0]["Transport3"] = "";
            dtVoucherinfo.Rows[0]["Transport4"] = "";
            dtVoucherinfo.Rows[0]["Transport5"] = "";
            dtVoucherinfo.Rows[0]["Transport6"] = "";
            dtVoucherinfo.Rows[0]["DeliveryAt"] = "";
            dtVoucherinfo.Rows[0]["Grno"] = "";
            dtVoucherinfo.Rows[0]["DD"] = 0;
            dtVoucherinfo.Rows[0]["DR"] = 0;


            dtVoucherinfo.Rows[0]["PaymentMode"] = "";
            dtVoucherinfo.Rows[0]["TaxChanged"] = false;
            dtVoucherinfo.Rows[0]["formC"] = false;
            dtVoucherinfo.Rows[0]["DeliveryType"] = "";
            dtVoucherinfo.Rows[0]["As_Per"] = "";
            dtVoucherinfo.Rows[0]["Delivery_adrs"] = "";

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
            string loca_dp_id = Database.GetScalarText("Select dp_id from Location where LocationId='" + Database.LocationId + "'");
            dtVoucherinfo.Rows[0]["Consigner_id"] = loca_dp_id;
            dtVoucherinfo.Rows[0]["modify_date"] = System.DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
            dtVoucherinfo.Rows[0]["ModTime"] = System.DateTime.Now.ToString("HH:mm:ss");

            Database.SaveData(dtVoucherinfo);
            if (vid == "0")
            {
                vid = dtVoucherinfo.Rows[0]["Vi_id"].ToString();
            }

            dtchallanunl = new DataTable("ChallanUnloadings");
            Database.GetSqlData("Select * from ChallanUnloadings where Vi_id='" + vid + "'", dtchallanunl);
            for (int j = 0; j < dtchallanunl.Rows.Count; j++)
            {
                dtchallanunl.Rows[j].Delete();
            }
            Database.SaveData(dtchallanunl);
            dtchallanunl = new DataTable("ChallanUnloadings");
            Database.GetSqlData("Select * from ChallanUnloadings where Vi_id='" + vid + "'", dtchallanunl);
                int Nid2 = 1;
            DataTable dtidvd = new DataTable();
            Database.GetSqlData("select max(Nid) as Nid from ChallanUnloadings where locationid='" + Database.LocationId + "'", dtidvd);
            if (dtidvd.Rows[0][0].ToString() != "")
            {
                Nid2 = int.Parse(dtidvd.Rows[0][0].ToString()) + 1;
            }

            for (int i = 0; i < ansGridView1.Rows.Count - 1; i++)
            {



                dtchallanunl.Rows.Add();
                dtchallanunl.Rows[i]["Nid"] = Nid2;
                dtchallanunl.Rows[i]["LocationId"] = Database.LocationId;
                dtchallanunl.Rows[i]["vd_id"] = Database.LocationId + dtchallanunl.Rows[i]["nid"].ToString();
             
                dtchallanunl.Rows[i]["Vi_id"] = vid;
                dtchallanunl.Rows[i]["ItemSr"] = ansGridView1.Rows[i].Cells["sno"].Value;
                dtchallanunl.Rows[i]["Des_ac_id"] = funs.Select_item_id(ansGridView1.Rows[i].Cells["description"].Value.ToString());
                dtchallanunl.Rows[i]["Description"] = ansGridView1.Rows[i].Cells["description"].Value.ToString();
                dtchallanunl.Rows[i]["packing"] = ansGridView1.Rows[i].Cells["unt"].Value;
                dtchallanunl.Rows[i]["Quantity"] = ansGridView1.Rows[i].Cells["Quantity"].Value;
                dtchallanunl.Rows[i]["actweight"] = ansGridView1.Rows[i].Cells["actweight"].Value;
                dtchallanunl.Rows[i]["weight"] = ansGridView1.Rows[i].Cells["weight"].Value;
                dtchallanunl.Rows[i]["rate_am"] = ansGridView1.Rows[i].Cells["freight"].Value;
                dtchallanunl.Rows[i]["Amount"] = ansGridView1.Rows[i].Cells["gramt"].Value;

                dtchallanunl.Rows[i]["grtype"] = ansGridView1.Rows[i].Cells["grtype"].Value;
                dtchallanunl.Rows[i]["deliverytype"] = ansGridView1.Rows[i].Cells["deliverytype"].Value;
                if (ansGridView1.Rows[i].Cells["private"].Value == null || ansGridView1.Rows[i].Cells["private"].Value.ToString()=="")
                {
                    ansGridView1.Rows[i].Cells["private"].Value = "";
                }
                dtchallanunl.Rows[i]["private"] = ansGridView1.Rows[i].Cells["private"].Value;
                if (ansGridView1.Rows[i].Cells["remark"].Value == null || ansGridView1.Rows[i].Cells["remark"].Value.ToString() == "")
                {
                    ansGridView1.Rows[i].Cells["remark"].Value = "";
                }
                dtchallanunl.Rows[i]["remark"] = ansGridView1.Rows[i].Cells["remark"].Value;
                dtchallanunl.Rows[i]["ChargedWeight"] = ansGridView1.Rows[i].Cells["Weight"].Value;

                dtchallanunl.Rows[i]["grno"] = ansGridView1.Rows[i].Cells["grno1"].Value;
                dtchallanunl.Rows[i]["grdate"] = ansGridView1.Rows[i].Cells["grdate"].Value;
                dtchallanunl.Rows[i]["source_id"] = funs.Select_dp_id(ansGridView1.Rows[i].Cells["source1"].Value.ToString());
                dtchallanunl.Rows[i]["destination_id"] = funs.Select_dp_id(ansGridView1.Rows[i].Cells["destination1"].Value.ToString());
                dtchallanunl.Rows[i]["consigner_id"] = funs.Select_ac_id(ansGridView1.Rows[i].Cells["consigner1"].Value.ToString());
                dtchallanunl.Rows[i]["consignee_id"] = funs.Select_ac_id(ansGridView1.Rows[i].Cells["consignee1"].Value.ToString());


             
               
              
                dtchallanunl.Rows[i]["grcharge"] = double.Parse(ansGridView1.Rows[i].Cells["grcharge"].Value.ToString());
                dtchallanunl.Rows[i]["othcharge"] = double.Parse(ansGridView1.Rows[i].Cells["othcharge"].Value.ToString());

                dtchallanunl.Rows[i]["booking_date"] = dateTimePicker1.Value.Date;
                dtchallanunl.Rows[i]["create_date"] = create_date;
                dtchallanunl.Rows[i]["modify_date"] = System.DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");




                Nid2++;


            }
            Database.SaveData(dtchallanunl);


            DataTable dtstocks = new DataTable("stocks");
            Database.GetSqlData("Select * from stocks where Vid='" + vid + "'", dtstocks);
            for (int j = 0; j < dtstocks.Rows.Count; j++)
            {
                dtstocks.Rows[j].Delete();
            }
            Database.SaveData(dtstocks);

            dtstocks = new DataTable("stocks");
            Database.GetSqlData("Select * from stocks where Vid='" + vid + "'", dtstocks);

            if (iscancel == false)
            {
                for (int i = 0; i < ansGridView1.Rows.Count - 1; i++)
                {
                    dtstocks.Rows.Add();

                    dtstocks.Rows[dtstocks.Rows.Count - 1]["Vid"] = vid;
                    dtstocks.Rows[dtstocks.Rows.Count - 1]["GR_id"] = vid + "/" + ansGridView1.Rows[i].Cells["sno"].Value.ToString();

                    dtstocks.Rows[dtstocks.Rows.Count - 1]["Quantity"] = 1;
                    dtstocks.Rows[dtstocks.Rows.Count - 1]["Step"] = "Step2";
                    dtstocks.Rows[dtstocks.Rows.Count - 1]["Godown_id"] = Database.LocationId;


                    string aliasname = Database.GetScalarText("Select Aliasname from vouchertypes where vt_id=" + vtid);
                    dtstocks.Rows[dtstocks.Rows.Count - 1]["Narration"] = aliasname;

                    dtstocks.Rows[dtstocks.Rows.Count - 1]["GRNo"] = ansGridView1.Rows[i].Cells["grno1"].Value.ToString();
                    dtstocks.Rows[dtstocks.Rows.Count - 1]["GRDate"] = ansGridView1.Rows[i].Cells["grdate"].Value.ToString();

                    dtstocks.Rows[dtstocks.Rows.Count - 1]["Consigner_id"] = funs.Select_ac_id(ansGridView1.Rows[i].Cells["consigner1"].Value.ToString());
                    dtstocks.Rows[dtstocks.Rows.Count - 1]["Consignee_id"] = funs.Select_ac_id(ansGridView1.Rows[i].Cells["consignee1"].Value.ToString());


                    dtstocks.Rows[dtstocks.Rows.Count - 1]["Source_id"] = funs.Select_dp_id(ansGridView1.Rows[i].Cells["Source1"].Value.ToString());
                    dtstocks.Rows[dtstocks.Rows.Count - 1]["Destination_id"] = funs.Select_dp_id(ansGridView1.Rows[i].Cells["destination1"].Value.ToString());

                    if (ansGridView1.Rows[i].Cells["GRType"].Value.ToString() == "To Pay")
                    {
                        dtstocks.Rows[dtstocks.Rows.Count - 1]["ToPay"] = double.Parse(ansGridView1.Rows[i].Cells["gramt"].Value.ToString());
                        dtstocks.Rows[dtstocks.Rows.Count - 1]["TBB"] = 0;

                        dtstocks.Rows[dtstocks.Rows.Count - 1]["Paid"] = 0;
                        dtstocks.Rows[dtstocks.Rows.Count - 1]["FOC"] = 0;
                    }
                    else if (ansGridView1.Rows[i].Cells["GRType"].Value.ToString() == "FOC")
                    {
                        dtstocks.Rows[dtstocks.Rows.Count - 1]["ToPay"] = 0;
                        dtstocks.Rows[dtstocks.Rows.Count - 1]["TBB"] = 0;

                        dtstocks.Rows[dtstocks.Rows.Count - 1]["Paid"] = 0;
                        dtstocks.Rows[dtstocks.Rows.Count - 1]["FOC"] = double.Parse(ansGridView1.Rows[i].Cells["gramt"].Value.ToString());
                    }
                    else if (ansGridView1.Rows[i].Cells["GRType"].Value.ToString() == "Paid")
                    {
                        dtstocks.Rows[dtstocks.Rows.Count - 1]["ToPay"] = 0;
                        dtstocks.Rows[dtstocks.Rows.Count - 1]["TBB"] = 0;

                        dtstocks.Rows[dtstocks.Rows.Count - 1]["Paid"] = double.Parse(ansGridView1.Rows[i].Cells["gramt"].Value.ToString());
                        dtstocks.Rows[dtstocks.Rows.Count - 1]["FOC"] = 0;
                    }
                    else if (ansGridView1.Rows[i].Cells["GRType"].Value.ToString() == "T.B.B.")
                    {
                        dtstocks.Rows[dtstocks.Rows.Count - 1]["ToPay"] = 0;
                        dtstocks.Rows[dtstocks.Rows.Count - 1]["TBB"] = double.Parse(ansGridView1.Rows[i].Cells["gramt"].Value.ToString());

                        dtstocks.Rows[dtstocks.Rows.Count - 1]["Paid"] = 0;
                        dtstocks.Rows[dtstocks.Rows.Count - 1]["FOC"] = 0;
                    }
                    dtstocks.Rows[dtstocks.Rows.Count - 1]["actWeight"] = double.Parse(ansGridView1.Rows[i].Cells["actweight"].Value.ToString());
                    dtstocks.Rows[dtstocks.Rows.Count - 1]["TotWeight"] =double.Parse( ansGridView1.Rows[i].Cells["weight"].Value.ToString());
                    dtstocks.Rows[dtstocks.Rows.Count - 1]["TotPkts"] = double.Parse(ansGridView1.Rows[i].Cells["quantity"].Value.ToString());


                    dtstocks.Rows[dtstocks.Rows.Count - 1]["GRCharge"] = double.Parse(ansGridView1.Rows[i].Cells["grcharge"].Value.ToString());

                    dtstocks.Rows[dtstocks.Rows.Count - 1]["GRType"] = ansGridView1.Rows[i].Cells["grtype"].Value.ToString();


                    dtstocks.Rows[dtstocks.Rows.Count - 1]["OthCharge"] = double.Parse(ansGridView1.Rows[i].Cells["othcharge"].Value.ToString());
                    dtstocks.Rows[dtstocks.Rows.Count - 1]["Freight"] = double.Parse(ansGridView1.Rows[i].Cells["freight"].Value.ToString());

                    dtstocks.Rows[dtstocks.Rows.Count - 1]["ItemName"] = ansGridView1.Rows[0].Cells["description"].Value.ToString();
                    dtstocks.Rows[dtstocks.Rows.Count - 1]["Packing"] = ansGridView1.Rows[0].Cells["unt"].Value.ToString();

                    if (ansGridView1.Rows[i].Cells["deliverytype"].Value == null || ansGridView1.Rows[i].Cells["deliverytype"].Value.ToString() == "")
                    {
                        ansGridView1.Rows[i].Cells["deliverytype"].Value = "";
                    }

                    dtstocks.Rows[dtstocks.Rows.Count - 1]["DeliveryType"] = ansGridView1.Rows[i].Cells["deliverytype"].Value.ToString();
                    dtstocks.Rows[dtstocks.Rows.Count - 1]["Private"] = ansGridView1.Rows[i].Cells["private"].Value.ToString();
                    dtstocks.Rows[dtstocks.Rows.Count - 1]["Remark"] = ansGridView1.Rows[i].Cells["remark"].Value.ToString();
                }
                Database.SaveData(dtstocks);
            }








            funs.ShowBalloonTip("Saved", "Voucher Number: " + vno + " Saved Successfully");


        }
        private void clear()
        {
            if (gStr == "0")
            {
                LoadData("0", "GRByChallan");
            }
            else
            {
                this.Close();
                this.Dispose();
            }
        }
        private void ansGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (ansGridView1.CurrentCell.OwningColumn.Name == "Sno")
            {
                SendKeys.Send("{right}");
                this.Activate();
            }
            ansGridView1.Rows[e.RowIndex].Cells["sno"].Value = e.RowIndex + 1;
        }

        private void ansGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
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
           



            //iscancel
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
               
                    dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = false;
               
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
               
                    dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = false;
                
            }

            //change vnumber
            dtsidefill.Rows.Add();
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Name"] = "vnumber";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["DisplayName"] = "Chng Vno";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["ShortcutKey"] = "^F12";
            if (Database.utype == "User")
            {
                dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = false;
            }
            else
            {
                dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = false;
            }


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

        public bool validate()
        {
            if (textBox1.Text.Trim() == "")
            {
                textBox1.Focus();
                return false;
            }
            if (txtTruckNo.Text.Trim() == "")
            {
                txtTruckNo.Focus();
                return false;
            }
            if (ansGridView1.Rows.Count == 1)
            {
                MessageBox.Show("Enter Data");
                return false;
            }
            for (int i = 0; i < ansGridView1.Rows.Count - 1; i++)
            {
                if (double.Parse(ansGridView1.Rows[i].Cells["quantity"].Value.ToString()) == 0)
                {
                    MessageBox.Show("Quantity must not Be Zero.");
                    return false;
                }
                if (ansGridView1.Rows[i].Cells["grtype"].Value ==null ||  ansGridView1.Rows[i].Cells["grtype"].Value.ToString() == "")
                {
                    MessageBox.Show("Enter GRType in grid.");
                    return false;
                }
                if (ansGridView1.Rows[i].Cells["description"].Value == null || ansGridView1.Rows[i].Cells["description"].Value.ToString() == "")
                {
                    MessageBox.Show("Enter Itemname in grid.");
                    return false;
                }
                if (ansGridView1.Rows[i].Cells["unt"].Value == null || ansGridView1.Rows[i].Cells["unt"].Value.ToString() == "")
                {
                    MessageBox.Show("Enter Items Packing in grid.");
                    return false;
                }
            }
            if (double.Parse(textBox2.Text)==0)
            {
                MessageBox.Show("Enter Items");
                return false;
            }
            return true;
        }
        public void btn_Click(object sender, EventArgs e)
        {
            Button tbtn = (Button)sender;
            string name = tbtn.Name.ToString();
           // string name = "save";
           
            if (name == "save")
            {
                if (validate() == true)
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
                            DataTable dtstocks = new DataTable("stocks");
                            Database.GetSqlData("Select * from stocks where Vid='" + vid + "'", dtstocks);
                            for (int j = 0; j < dtstocks.Rows.Count; j++)
                            {
                                dtstocks.Rows[j].Delete();
                            }
                            Database.SaveData(dtstocks);

                            dtstocks = new DataTable("stocks");
                            Database.GetSqlData("Select * from stocks where Vid='" + vid + "'", dtstocks);
                            if (iscancel == false)
                            {
                                for (int i = 0; i < ansGridView1.Rows.Count - 1; i++)
                                {
                                    dtstocks.Rows.Add();

                                    dtstocks.Rows[dtstocks.Rows.Count - 1]["Vid"] = vid;
                                    dtstocks.Rows[dtstocks.Rows.Count - 1]["GR_id"] = vid + "/" + ansGridView1.Rows[i].Cells["sno"].Value.ToString();

                                    dtstocks.Rows[dtstocks.Rows.Count - 1]["Quantity"] = 1;
                                    dtstocks.Rows[dtstocks.Rows.Count - 1]["Step"] = "Step2";
                                    dtstocks.Rows[dtstocks.Rows.Count - 1]["Godown_id"] = Database.LocationId;


                                    string aliasname = Database.GetScalarText("Select Aliasname from vouchertypes where vt_id=" + vtid);
                                    dtstocks.Rows[dtstocks.Rows.Count - 1]["Narration"] = aliasname;

                                    dtstocks.Rows[dtstocks.Rows.Count - 1]["GRNo"] = ansGridView1.Rows[i].Cells["grno1"].Value.ToString();
                                    dtstocks.Rows[dtstocks.Rows.Count - 1]["GRDate"] = ansGridView1.Rows[i].Cells["grdate"].Value.ToString();

                                    dtstocks.Rows[dtstocks.Rows.Count - 1]["Consigner_id"] = funs.Select_ac_id(ansGridView1.Rows[i].Cells["consigner1"].Value.ToString());
                                    dtstocks.Rows[dtstocks.Rows.Count - 1]["Consignee_id"] = funs.Select_ac_id(ansGridView1.Rows[i].Cells["consignee1"].Value.ToString());


                                    dtstocks.Rows[dtstocks.Rows.Count - 1]["Source_id"] = funs.Select_dp_id(ansGridView1.Rows[i].Cells["Source1"].Value.ToString());
                                    dtstocks.Rows[dtstocks.Rows.Count - 1]["Destination_id"] = funs.Select_dp_id(ansGridView1.Rows[i].Cells["destination1"].Value.ToString());

                                    if (ansGridView1.Rows[i].Cells["GRType"].Value.ToString() == "To Pay")
                                    {
                                        dtstocks.Rows[dtstocks.Rows.Count - 1]["ToPay"] = double.Parse(ansGridView1.Rows[i].Cells["gramt"].Value.ToString());
                                        dtstocks.Rows[dtstocks.Rows.Count - 1]["TBB"] = 0;

                                        dtstocks.Rows[dtstocks.Rows.Count - 1]["Paid"] = 0;
                                        dtstocks.Rows[dtstocks.Rows.Count - 1]["FOC"] = 0;
                                    }
                                    else if (ansGridView1.Rows[i].Cells["GRType"].Value.ToString() == "FOC")
                                    {
                                        dtstocks.Rows[dtstocks.Rows.Count - 1]["ToPay"] = 0;
                                        dtstocks.Rows[dtstocks.Rows.Count - 1]["TBB"] = 0;

                                        dtstocks.Rows[dtstocks.Rows.Count - 1]["Paid"] = 0;
                                        dtstocks.Rows[dtstocks.Rows.Count - 1]["FOC"] = double.Parse(ansGridView1.Rows[i].Cells["gramt"].Value.ToString());
                                    }
                                    else if (ansGridView1.Rows[i].Cells["GRType"].Value.ToString() == "Paid")
                                    {
                                        dtstocks.Rows[dtstocks.Rows.Count - 1]["ToPay"] = 0;
                                        dtstocks.Rows[dtstocks.Rows.Count - 1]["TBB"] = 0;

                                        dtstocks.Rows[dtstocks.Rows.Count - 1]["Paid"] = double.Parse(ansGridView1.Rows[i].Cells["gramt"].Value.ToString());
                                        dtstocks.Rows[dtstocks.Rows.Count - 1]["FOC"] = 0;
                                    }
                                    else if (ansGridView1.Rows[i].Cells["GRType"].Value.ToString() == "T.B.B.")
                                    {
                                        dtstocks.Rows[dtstocks.Rows.Count - 1]["ToPay"] = 0;
                                        dtstocks.Rows[dtstocks.Rows.Count - 1]["TBB"] = double.Parse(ansGridView1.Rows[i].Cells["gramt"].Value.ToString());

                                        dtstocks.Rows[dtstocks.Rows.Count - 1]["Paid"] = 0;
                                        dtstocks.Rows[dtstocks.Rows.Count - 1]["FOC"] = 0;
                                    }
                                    dtstocks.Rows[dtstocks.Rows.Count - 1]["TotWeight"] = double.Parse(ansGridView1.Rows[i].Cells["weight"].Value.ToString());
                                    dtstocks.Rows[dtstocks.Rows.Count - 1]["TotPkts"] = double.Parse(ansGridView1.Rows[i].Cells["quantity"].Value.ToString());


                                    dtstocks.Rows[dtstocks.Rows.Count - 1]["GRCharge"] = double.Parse(ansGridView1.Rows[i].Cells["grcharge"].Value.ToString());

                                    dtstocks.Rows[dtstocks.Rows.Count - 1]["GRType"] = ansGridView1.Rows[i].Cells["grtype"].Value.ToString();


                                    dtstocks.Rows[dtstocks.Rows.Count - 1]["OthCharge"] = double.Parse(ansGridView1.Rows[i].Cells["othcharge"].Value.ToString());
                                    dtstocks.Rows[dtstocks.Rows.Count - 1]["Freight"] = double.Parse(ansGridView1.Rows[i].Cells["freight"].Value.ToString());

                                    dtstocks.Rows[dtstocks.Rows.Count - 1]["ItemName"] = ansGridView1.Rows[0].Cells["description"].Value.ToString();
                                    dtstocks.Rows[dtstocks.Rows.Count - 1]["Packing"] = ansGridView1.Rows[0].Cells["unt"].Value.ToString();

                                    if (ansGridView1.Rows[i].Cells["deliverytype"].Value == null || ansGridView1.Rows[i].Cells["deliverytype"].Value.ToString() == "")
                                    {
                                        ansGridView1.Rows[i].Cells["deliverytype"].Value = "";
                                    }

                                    dtstocks.Rows[dtstocks.Rows.Count - 1]["DeliveryType"] = ansGridView1.Rows[i].Cells["deliverytype"].Value.ToString();
                                    dtstocks.Rows[dtstocks.Rows.Count - 1]["Private"] = ansGridView1.Rows[i].Cells["private"].Value.ToString();
                                    dtstocks.Rows[dtstocks.Rows.Count - 1]["Remark"] = ansGridView1.Rows[i].Cells["remark"].Value.ToString();
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

            
            else if (name == "Print")
            {
               
            }
           
            else if (name == "quit")
            {
                this.Close();
                this.Dispose();
            }
        }

        private void ansGridView1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (ansGridView1.CurrentCell == null)
            {
                return;
            }
            if (char.IsLetter(e.KeyChar) || char.IsNumber(e.KeyChar) || e.KeyChar == ' ' || Convert.ToInt32(e.KeyChar) == 13)
            {
            }
            else
            {
                return;
            }

            if (ansGridView1.CurrentCell.OwningColumn.Name == "DeliveryType")
            {
                DataTable dtcombo = new DataTable();
                dtcombo.Columns.Add("DeliveryType", typeof(string));


                dtcombo.Columns["DeliveryType"].ColumnName = "DeliveryType";
                dtcombo.Rows.Add();
                dtcombo.Rows[0][0] = "Godown";

                dtcombo.Rows.Add();
                dtcombo.Rows[1][0] = "Door Delivery";

                ansGridView1.CurrentCell.Value = SelectCombo.ComboDt(this, dtcombo, 0);
               
                SendKeys.Send("{tab}"); 
            }

            if (ansGridView1.CurrentCell.OwningColumn.Name == "GRType")
            {

                DataTable dtcombo = new DataTable();
                dtcombo.Columns.Add("GRType", typeof(string));

                dtcombo.Columns["GRType"].ColumnName = "GRType";
                dtcombo.Rows.Add();
                dtcombo.Rows[0][0] = "Paid";
                dtcombo.Rows.Add();
                dtcombo.Rows[1][0] = "FOC";
                dtcombo.Rows.Add();
                dtcombo.Rows[2][0] = "T.B.B.";
                dtcombo.Rows.Add();
                dtcombo.Rows[3][0] = "To Pay";

                ansGridView1.CurrentCell.Value = SelectCombo.ComboDt(this, dtcombo, 0);
                calc();
                SendKeys.Send("{tab}"); 
            }
            if (ansGridView1.CurrentCell.OwningColumn.Name == "consigner1")
            {
                string strCombo = "SELECT ACCOUNTs.Name, ACCOUNTs.Printname, DeliveryPoints.Name AS Station, ACCOUNTs.Address1, ACCOUNTs.Address2, ACCOUNTs.Phone, ACCOUNTs.Tin_number, OTHERs.Name AS Staff, CONTRACTORs.Name AS Agent FROM ACCOUNTs LEFT OUTER JOIN CONTRACTORs ON ACCOUNTs.Con_id = CONTRACTORs.Name LEFT OUTER JOIN OTHERs ON ACCOUNTs.Loc_id = OTHERs.Oth_id LEFT OUTER JOIN DeliveryPoints ON ACCOUNTs.SId = DeliveryPoints.DPId WHERE ACCOUNTs.Act_id = 39 ORDER BY ACCOUNTs.Name";
                ansGridView1.CurrentCell.Value = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 2);
                SendKeys.Send("{Enter}");


                if (ansGridView1.CurrentCell.Value != "")
                {
                    DataTable dtStation = new DataTable();
                    Database.GetSqlData("select SId from ACCOUNTs where [name]='" + ansGridView1.CurrentCell.Value.ToString() + "'", dtStation);
                    ansGridView1.CurrentRow.Cells["Source1"].Value = funs.Select_dp_nm(dtStation.Rows[0]["SId"].ToString());
                }
            }
            if (ansGridView1.CurrentCell.OwningColumn.Name == "consignee1")
            {
                string strCombo = "SELECT ACCOUNTs.Name, ACCOUNTs.Printname, DeliveryPoints.Name AS Station, ACCOUNTs.Address1, ACCOUNTs.Address2, ACCOUNTs.Phone, ACCOUNTs.Tin_number, OTHERs.Name AS Staff, CONTRACTORs.Name AS Agent FROM ACCOUNTs LEFT OUTER JOIN CONTRACTORs ON ACCOUNTs.Con_id = CONTRACTORs.Name LEFT OUTER JOIN OTHERs ON ACCOUNTs.Loc_id = OTHERs.Oth_id LEFT OUTER JOIN DeliveryPoints ON ACCOUNTs.SId = DeliveryPoints.DPId WHERE ACCOUNTs.Act_id = 39 ORDER BY ACCOUNTs.Name";
                ansGridView1.CurrentCell.Value = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 2);
                SendKeys.Send("{Enter}");
                if (ansGridView1.CurrentCell.Value != "")
                {
                    DataTable dtStation = new DataTable();
                    Database.GetSqlData("select SId from ACCOUNTs where [name]='" + ansGridView1.CurrentCell.Value.ToString() + "'", dtStation);
                    ansGridView1.CurrentRow.Cells["destination1"].Value = funs.Select_dp_nm(dtStation.Rows[0]["SId"].ToString());
                }
            }
            if (ansGridView1.CurrentCell.OwningColumn.Name == "destination1")
            {
                string strCombo = "SELECT [name] from DeliveryPoints";
                ansGridView1.CurrentCell.Value = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 2);
                SendKeys.Send("{Enter}");
            }
            if (ansGridView1.CurrentCell.OwningColumn.Name == "Source1")
            {
                string strCombo = "SELECT [name] from DeliveryPoints";
                ansGridView1.CurrentCell.Value = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 2);
                SendKeys.Send("{Enter}");
            }
            DataTable dt = new DataTable();
            if (ansGridView1.CurrentCell.OwningColumn.Name == "description")
            {


                if (Feature.Available("Display All items") == "No")
                {
                    strCombo = "SELECT DISTINCT items.name FROM items RIGHT OUTER JOIN ItemDetails ON items.Id = ItemDetails.Item_id ORDER BY items.name";
                }
                else
                {
                    strCombo = "SELECT DISTINCT items.name FROM items ORDER BY items.name";
                }
               
                ansGridView1.CurrentCell.Value = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);

                if (ansGridView1.CurrentCell.Value.ToString() != "")
                {
                    ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["quantity"].Value = 0;
                    ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["weight"].Value = 0;
                    ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["actweight"].Value = 0;
                    ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["freight"].Value = 0;
                    ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["GRcharge"].Value = 0;
                    ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["othcharge"].Value = 0;
                    ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["gramt"].Value = 0;


                    SendKeys.Send("{tab}"); 
                }
            }

            else if (ansGridView1.CurrentCell.OwningColumn.Name == "unt")
            {
                strCombo = "select Name from packings order by Name";
                ansGridView1.CurrentCell.Value = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, ansGridView1.CurrentCell.Value == null ? "" : ansGridView1.CurrentCell.Value.ToString(), 0);
                if (ansGridView1.CurrentCell.Value != "")
                {
                    SendKeys.Send("{tab}"); 
                }
            }
        }

        private void calc()
        {

            double gramt = 0, total = 0, qty = 0, wht = 0, actwht = 0, gr = 0, totfoc = 0, totpaid = 0, totpay = 0, tottbb = 0;
            for (int i = 0; i < ansGridView1.Rows.Count - 1; i++)
            {
                gr = ansGridView1.Rows.Count - 1;
                double grchg = 0;
                grchg = double.Parse(ansGridView1.Rows[i].Cells["grcharge"].Value.ToString());
                double freight = 0;
                freight = double.Parse(ansGridView1.Rows[i].Cells["freight"].Value.ToString());
                double othchg = 0;
                othchg = double.Parse(ansGridView1.Rows[i].Cells["othcharge"].Value.ToString());

                qty += double.Parse(ansGridView1.Rows[i].Cells["Quantity"].Value.ToString());
                actwht += double.Parse(ansGridView1.Rows[i].Cells["actweight"].Value.ToString());
                wht += double.Parse(ansGridView1.Rows[i].Cells["weight"].Value.ToString());
                ansGridView1.Rows[i].Cells["gramt"].Value = freight+grchg + othchg;
                total += double.Parse(ansGridView1.Rows[i].Cells["gramt"].Value.ToString());


                if (ansGridView1.Rows[i].Cells["grtype"].Value.ToString() == "Paid")
                {
                    totpaid  += double.Parse(ansGridView1.Rows[i].Cells["gramt"].Value.ToString());

                }
                else if (ansGridView1.Rows[i].Cells["grtype"].Value.ToString() == "FOC")
                {
                    totfoc += double.Parse(ansGridView1.Rows[i].Cells["gramt"].Value.ToString());

                }
                else if (ansGridView1.Rows[i].Cells["grtype"].Value.ToString() == "T.B.B.")
                {
                    tottbb += double.Parse(ansGridView1.Rows[i].Cells["gramt"].Value.ToString());

                }
                else if (ansGridView1.Rows[i].Cells["grtype"].Value.ToString() == "To Pay")
                {
                    totpay+= double.Parse(ansGridView1.Rows[i].Cells["gramt"].Value.ToString());

                }
              



            }
                textBox2.Text=funs.DecimalPoint(total,2);
                textBox5.Text = funs.DecimalPoint(qty, 2);
                textBox6.Text = funs.DecimalPoint(wht, 2);
                textBox7.Text = funs.DecimalPoint(totfoc, 2);
                textBox8.Text = funs.DecimalPoint(totpaid, 2);
                textBox9.Text = funs.DecimalPoint(totpay, 2);
                textBox10.Text = funs.DecimalPoint(tottbb, 2);
                textBox11.Text = funs.DecimalPoint(actwht, 2);
               

                label8.Text = funs.DecimalPoint(gr, 0);

        }
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        private void frm_newunloading_Load(object sender, EventArgs e)
        {
            SideFill();
        }

        private void ansGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (ansGridView1.CurrentCell.OwningColumn.Name == "Grdate" && ansGridView1.Rows[e.RowIndex].Cells["Grdate"].Value != null && ansGridView1.Rows[e.RowIndex].Cells["Grdate"].Value.ToString() != "")
            {
                if (funs.Stringtodate(ansGridView1.CurrentRow.Cells["Grdate"].Value.ToString()) == null)
                {
                    ansGridView1.CurrentRow.Cells["Grdate"].Value = null;
                }
                else
                {
                    ansGridView1.CurrentRow.Cells["Grdate"].Value = funs.Stringtodate(ansGridView1.CurrentRow.Cells["Grdate"].Value.ToString()).Value.ToString(Database.dformat);

                }
            }
            if (ansGridView1.CurrentCell.OwningColumn.Name == "quantity")
            {
                if (ansGridView1.Rows[e.RowIndex].Cells["Quantity"].Value.ToString() == "")
                {
                    ansGridView1.Rows[e.RowIndex].Cells["Quantity"].Value = 0;
                }
                else
                {
                    ansGridView1.Rows[e.RowIndex].Cells["Quantity"].Value = funs.DecimalPoint(ansGridView1.Rows[e.RowIndex].Cells["Quantity"].Value.ToString(), 2);
                }
                calc();
            }

            if (ansGridView1.CurrentCell.OwningColumn.Name == "GRType")
            {
               
                calc();
            }

            if (ansGridView1.CurrentCell.OwningColumn.Name == "weight")
            {
                if (ansGridView1.Rows[e.RowIndex].Cells["weight"].Value.ToString() == "")
                {
                    ansGridView1.Rows[e.RowIndex].Cells["weight"].Value = 0;
                }
                else
                {
                    ansGridView1.Rows[e.RowIndex].Cells["weight"].Value = funs.DecimalPoint(ansGridView1.Rows[e.RowIndex].Cells["weight"].Value.ToString(), 2);
                }
                calc();
            }
            if (ansGridView1.CurrentCell.OwningColumn.Name == "freight")
            {
                if (ansGridView1.Rows[e.RowIndex].Cells["freight"].Value.ToString() == "")
                {
                    ansGridView1.Rows[e.RowIndex].Cells["freight"].Value = 0;
                }
                else
                {
                    ansGridView1.Rows[e.RowIndex].Cells["freight"].Value = funs.DecimalPoint(ansGridView1.Rows[e.RowIndex].Cells["freight"].Value.ToString(), 2);
                }
                calc();
            }
            if (ansGridView1.CurrentCell.OwningColumn.Name == "grcharge")
            {
                if (ansGridView1.Rows[e.RowIndex].Cells["grcharge"].Value.ToString() == "")
                {
                    ansGridView1.Rows[e.RowIndex].Cells["grcharge"].Value = 0;
                }
                else
                {
                    ansGridView1.Rows[e.RowIndex].Cells["grcharge"].Value = funs.DecimalPoint(ansGridView1.Rows[e.RowIndex].Cells["grcharge"].Value.ToString(),2);
                }
                calc();
            }
            if (ansGridView1.CurrentCell.OwningColumn.Name == "othcharge")
            {
                if (ansGridView1.Rows[e.RowIndex].Cells["othcharge"].Value.ToString() == "")
                {
                    ansGridView1.Rows[e.RowIndex].Cells["othcharge"].Value = 0;
                }
                else
                {
                    ansGridView1.Rows[e.RowIndex].Cells["othcharge"].Value = funs.DecimalPoint(ansGridView1.Rows[e.RowIndex].Cells["othcharge"].Value.ToString(), 2);
                }
                calc();
            }
         
           

        }

        private void ansGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            ansGridView1.Rows[e.RowIndex].Cells["Quantity"].Value = 0;
            ansGridView1.Rows[e.RowIndex].Cells["weight"].Value = 0;
            ansGridView1.Rows[e.RowIndex].Cells["freight"].Value = 0;
            ansGridView1.Rows[e.RowIndex].Cells["grcharge"].Value = 0;
            ansGridView1.Rows[e.RowIndex].Cells["othcharge"].Value = 0;
         
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtTruckNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            string strCombo = "Select Gaddi_name from Gaddis order by Gaddi_name";
            txtTruckNo.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, "", 0);
        }

        private void frm_newunloading_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
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
            else if (e.KeyCode == Keys.Escape)
            {
                if (textBox1.Text != "")
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
                else
                {
                    this.Dispose();
                }

            }



        }

        private bool Validatedel(int rowindex)
        {

            if (vid != "0")
            {
                string tempgr_id = vid + "/" + ansGridView1.Rows[rowindex].Cells["sno"].Value.ToString();
                string GRNo = ansGridView1.Rows[rowindex].Cells["grno1"].Value.ToString();
                int gridcount = Database.GetScalarInt("Select Count(*) from Stocks where Gr_id='" + tempgr_id + "' and GRNO='" + GRNo + "' ");
                if (gridcount > 1)
                {
                    MessageBox.Show("It cann't be Deleted... It has been Dispatched");

                    return false;
                }


            }

            return true;
        }


        private void ansGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (ansGridView1.CurrentCell == null)
            {
                return;
            }
            if (e.KeyCode == Keys.Delete)
            {
                if (Validatedel(ansGridView1.CurrentRow.Index)  == true)
                {
                    if (ansGridView1.CurrentRow.Index == ansGridView1.Rows.Count - 1)
                    {
                        ansGridView1.Rows[ansGridView1.CurrentRow.Index].Cells[1].Value = "";
                        ansGridView1.Rows[ansGridView1.CurrentRow.Index].Cells[2].Value = "";
                        calc();
                        return;
                    }
                    else
                    {
                        ansGridView1.Rows.RemoveAt(ansGridView1.CurrentRow.Index);
                        for (int i = 0; i < ansGridView1.Rows.Count; i++)
                        {
                            ansGridView1.Rows[i].Cells["sno"].Value = (i + 1);
                        }
                        calc();
                        return;
                    }
                }
            }

            if (ansGridView1.CurrentCell.OwningColumn.Name == "Source1")
            {
                if (e.Control && e.KeyCode == Keys.A)
                {
                    if (ansGridView1.CurrentCell.Value != null)
                    {
                        ansGridView1.CurrentCell.Value = funs.EditDP(ansGridView1.CurrentCell.Value.ToString());
                    }
                }
                else if (e.Control && e.KeyCode == Keys.C)
                {
                    ansGridView1.CurrentCell.Value = funs.AddDP();
                }
            }
            if (ansGridView1.CurrentCell.OwningColumn.Name == "destination1")
            {
                if (e.Control && e.KeyCode == Keys.A)
                {
                    if (ansGridView1.CurrentCell.Value != null)
                    {
                        ansGridView1.CurrentCell.Value = funs.EditDP(ansGridView1.CurrentCell.Value.ToString());
                    }
                }
                else if (e.Control && e.KeyCode == Keys.C)
                {
                    ansGridView1.CurrentCell.Value = funs.AddDP();
                }
            }

            if (ansGridView1.CurrentCell.OwningColumn.Name == "description")
            {
                if (e.Control && e.KeyCode == Keys.A)
                {
                    if (ansGridView1.CurrentCell.Value != null)
                    {
                        ansGridView1.CurrentCell.Value = funs.EditItem(ansGridView1.CurrentCell.Value.ToString());
                    }
                }
                else if (e.Control && e.KeyCode == Keys.C)
                {
                    ansGridView1.CurrentCell.Value = funs.AddItem();
                }
            }
            else if (ansGridView1.CurrentCell.OwningColumn.Name == "unt")
            {
                if (e.Control && e.KeyCode == Keys.A)
                {
                    if (ansGridView1.CurrentCell.Value != null)
                    {
                        ansGridView1.CurrentCell.Value = funs.EditPacking(ansGridView1.CurrentCell.Value.ToString());
                    }
                }
                else if (e.Control && e.KeyCode == Keys.C)
                {
                    ansGridView1.CurrentCell.Value = funs.AddPacking();
                }
            }
            else if (ansGridView1.CurrentCell.OwningColumn.Name == "consigner1")
            {
                if (e.Control && e.KeyCode == Keys.A)
                {
                    if (ansGridView1.CurrentCell.Value != null)
                    {
                        ansGridView1.CurrentCell.Value = funs.EditAccount(ansGridView1.CurrentCell.Value.ToString());
                        


                      

                    }
                }
                else if (e.Control && e.KeyCode == Keys.C)
                {
                    ansGridView1.CurrentCell.Value = funs.AddAccount();
                }
                if (ansGridView1.CurrentCell.Value == null)
                {
                    return;
                }
                if (ansGridView1.CurrentCell.Value != "")
                {
                    DataTable dtStation = new DataTable();
                    Database.GetSqlData("select SId from ACCOUNTs where [name]='" + ansGridView1.CurrentCell.Value.ToString() + "'", dtStation);
                    ansGridView1.CurrentRow.Cells["Source1"].Value = funs.Select_dp_nm(dtStation.Rows[0]["SId"].ToString());
                    SendKeys.Send("{Enter}");
                }
            }
            else if (ansGridView1.CurrentCell.OwningColumn.Name == "consignee1")
            {
                if (e.Control && e.KeyCode == Keys.A)
                {
                    if (ansGridView1.CurrentCell.Value != null)
                    {
                        ansGridView1.CurrentCell.Value = funs.EditAccount(ansGridView1.CurrentCell.Value.ToString());
                    }
                }
                else if (e.Control && e.KeyCode == Keys.C)
                {
                    ansGridView1.CurrentCell.Value = funs.AddAccount();
                }
                if (ansGridView1.CurrentCell.Value == null)
                {
                    return;

                }
                if (ansGridView1.CurrentCell.Value != "")
                {
                    DataTable dtStation = new DataTable();
                    Database.GetSqlData("select SId from ACCOUNTs where [name]='" + ansGridView1.CurrentCell.Value.ToString() + "'", dtStation);
                    ansGridView1.CurrentRow.Cells["destination1"].Value = funs.Select_dp_nm(dtStation.Rows[0]["SId"].ToString());
                    SendKeys.Send("{Enter}");
                }
            }
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

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C)
            {
                textBox3.Text = funs.AddAccount();
            }
            else if (e.Control && e.KeyCode == Keys.A)
            {
                if (textBox3.Text != "")
                {
                    textBox3.Text = funs.EditAccount(textBox3.Text); ;
                }
            }
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            string strCombo = "SELECT ACCOUNTs.Name FROM ACCOUNTs LEFT JOIN ACCOUNTYPEs ON ACCOUNTs.Act_id = ACCOUNTYPEs.Act_id WHERE ACCOUNTYPEs.Name='DRIVER' ORDER BY ACCOUNTs.Name";
            textBox3.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, "", 0);
        }

        private void textBox4_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C)
            {
                textBox4.Text = funs.AddAccount();
            }

            if (e.Control && e.KeyCode == Keys.A)
            {
                if (textBox4.Text != "")
                {
                    textBox4.Text = funs.EditAccount(textBox4.Text);
                }
            }


            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {


            strCombo = "SELECT    Name FROM ACCOUNTs WHERE     (act_id = 40) order by name";
            textBox4.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, textBox4.Text, 0);
        }

        private void dateTimePicker1_Enter(object sender, EventArgs e)
        {
            Database.setFocus(dateTimePicker1);
        }

        private void dateTimePicker1_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(dateTimePicker1);
        }

        private void dateTimePicker1_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox1);
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox1);
        }

        private void dateTimePicker2_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void dateTimePicker2_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(dateTimePicker2);
        }

        private void dateTimePicker2_Enter(object sender, EventArgs e)
        {
            Database.setFocus(dateTimePicker2);

        }

        private void txtTruckNo_Enter(object sender, EventArgs e)
        {
            Database.setFocus(txtTruckNo);
        }

        private void txtTruckNo_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(txtTruckNo);
        }

        private void textBox3_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox3);
        }

        private void textBox3_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox3);
        }

        private void textBox4_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox4);
        }

        private void textBox4_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox4);
        }
    }
}
