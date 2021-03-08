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
    public partial class frm_Delivery : Form
    {
        string gvi_id = "0";
        string vid = "0";
        int vno=0;
        int vtid = 0;
        double chamt = 0;
        string GR_id = "0";
        string Prelocationid = "";
        public Boolean gresave = false;
        DataTable dtVoucherinfo;
        DataTable dtVoucharges;
        bool iscancel = false;
        DataTable dtVoucherdet;
        DateTime create_date = DateTime.Parse(System.DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"));


        public frm_Delivery()
        {
            InitializeComponent();
        }

        private void SetVno()
        {
            int numtype = funs.Select_NumType(vtid);
            if ((Prelocationid == Database.LocationId) || (Prelocationid == "" && vid == "0"))
            {
                if (numtype == 3 && vno != 0 && vid != "0")
                {
                    DateTime dt1 = dateTimePicker1.Value;
                    DateTime dt2 = DateTime.Parse(Database.GetScalarDate("select vdate from voucherinfos where vi_id='" + vid + "'"));

                    if (dt1 != dt2)
                    {
                        vno = funs.GenerateVno(vtid, dateTimePicker1.Value.ToString("dd-MMM-yyyy"), vid);
                        label11.Text = vno.ToString();
                    }
                    return;
                }
                if (vtid == 0 || (vno != 0 && vid != "0"))
                {
                    return;
                }

                vno = funs.GenerateVno(vtid, dateTimePicker1.Value.ToString("dd-MMM-yyyy"), vid);
                label11.Text = vno.ToString();
            }
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

        public void LoadData(string vi_id,string frmCaption)
       
        {
            gvi_id = vi_id;
            vid = gvi_id;
            this.Text = frmCaption;
            dateTimePicker1.MinDate = Database.stDate;
            dateTimePicker1.MaxDate = Database.ldate;
            dateTimePicker1.CustomFormat = Database.dformat;
            Displaysetting();
            SetVno();
            dtVoucherinfo = new DataTable("Voucherinfos");
            Database.GetSqlData("Select * from Voucherinfos where Vi_id='" + vid + "'", dtVoucherinfo);

            dtVoucherdet = new DataTable("Voucherdets");
            Database.GetSqlData("Select * from Voucherdets where Vi_id='" + vid + "' order by Itemsr", dtVoucherdet);
            //DataTable dt = new DataTable();
            //Database.GetSqlData("Select * from Charges where Autoload='true' order by Name", dt);

            ansGridView1.Columns["exp1rate"].HeaderText = "D.C.";
            ansGridView1.Columns["exp2rate"].HeaderText = "Misc.";
            ansGridView1.Columns["exp3rate"].HeaderText = "ST Charges";
            ansGridView1.Columns["exp4rate"].HeaderText = "X.RBT";

            ansGridView1.Columns["exp1rate"].CellTemplate.ValueType = typeof(double);
            ansGridView1.Columns["exp2rate"].CellTemplate.ValueType = typeof(double);
            ansGridView1.Columns["exp3rate"].CellTemplate.ValueType = typeof(double);
            ansGridView1.Columns["exp4rate"].CellTemplate.ValueType = typeof(double);

            if (gvi_id != "0")
            {
                Displaydata(vi_id);
            }
            else
            {
               // clear();
                textBox1.Text = "";
                textBox2.Text = "0";
                ansGridView1.Rows.Clear();
                textBox3.Text = "0";
                textBox4.Text = "";
                chamt = 0;
                vno = 0;
                label6.Text = "";
                label7.Text = "";
                label8.Text = "";
                label9.Text = "";
                label12.Text = "";
                label15.Text = "";
                ansGridView4.Rows.Clear();
                ansGridView4.Rows.Add();
                ansGridView4.Rows[0].Cells["Sno3"].Value = 1;
                ansGridView4.Rows[0].Cells["Charg_Name2"].Value = "D.C.";
                ansGridView4.Rows[0].Cells["Charg_id2"].Value = 0;
                ansGridView4.Rows[0].Cells["AmountB"].Value = 0;
                ansGridView4.Rows[0].Cells["CAmountB"].Value = 0;
                ansGridView4.Rows[0].Cells["Accid2"].Value = 0;
                ansGridView4.Rows[0].Cells["Addsub2"].Value = 0;
                ansGridView4.Rows[0].Cells["Ctype2"].Value = "";

                ansGridView4.Rows.Add();
                ansGridView4.Rows[1].Cells["Sno3"].Value = 2;
                ansGridView4.Rows[1].Cells["Charg_Name2"].Value = "Misc.";
                ansGridView4.Rows[1].Cells["Charg_id2"].Value = 0;
                ansGridView4.Rows[1].Cells["AmountB"].Value = 0;
                ansGridView4.Rows[1].Cells["CAmountB"].Value = 0;
                ansGridView4.Rows[1].Cells["Accid2"].Value = 0;
                ansGridView4.Rows[1].Cells["Addsub2"].Value = 0;
                ansGridView4.Rows[1].Cells["Ctype2"].Value = "";

                ansGridView4.Rows.Add();
                ansGridView4.Rows[2].Cells["Sno3"].Value = 3;
                ansGridView4.Rows[2].Cells["Charg_Name2"].Value ="ST Charges";
                ansGridView4.Rows[2].Cells["Charg_id2"].Value = 0;
                ansGridView4.Rows[2].Cells["AmountB"].Value = 0;
                ansGridView4.Rows[2].Cells["CAmountB"].Value = 0;
                ansGridView4.Rows[2].Cells["Accid2"].Value = 0;
                ansGridView4.Rows[2].Cells["Addsub2"].Value = 0;
                ansGridView4.Rows[2].Cells["Ctype2"].Value = "";

                ansGridView4.Rows.Add();
                ansGridView4.Rows[3].Cells["Sno3"].Value = 4;
                ansGridView4.Rows[3].Cells["Charg_Name2"].Value = "X.RBT";
                ansGridView4.Rows[3].Cells["Charg_id2"].Value = 0;
                ansGridView4.Rows[3].Cells["AmountB"].Value = 0;
                ansGridView4.Rows[3].Cells["CAmountB"].Value = 0;
                ansGridView4.Rows[3].Cells["Accid2"].Value = 0;
                ansGridView4.Rows[3].Cells["Addsub2"].Value = 0;
                ansGridView4.Rows[3].Cells["Ctype2"].Value = "";

                iscancel = false;
                label28.Visible = false;
                //for (int i = 0; i < dt.Rows.Count; i++)
                //{
                    //ansGridView4.Rows.Add();
                    //ansGridView4.Rows[i].Cells["Sno3"].Value = (i+1);
                    //ansGridView4.Rows[i].Cells["Charg_Name2"].Value = dt.Rows[i]["Name"].ToString();
                    //ansGridView4.Rows[i].Cells["Charg_id2"].Value = dt.Rows[i]["Ch_id"].ToString();
                    //ansGridView4.Rows[i].Cells["AmountB"].Value = 0;
                    //ansGridView4.Rows[i].Cells["CAmountB"].Value = 0;
                    //ansGridView4.Rows[i].Cells["Accid2"].Value = dt.Rows[i]["Ac_id"].ToString();
                    //ansGridView4.Rows[i].Cells["Addsub2"].Value = dt.Rows[i]["Add_sub"].ToString();
                    //ansGridView4.Rows[i].Cells["Ctype2"].Value = dt.Rows[i]["Charge_type"].ToString();
               // }
                SetVno();
               // dateTimePicker1.Select();
                textBox1.Select();
            }
            dateTimePicker1.Select();
            textBox1.Select();
         
            if (gresave == true)
            {
                object sender = new object();
                EventArgs e = new EventArgs();
                btn_Click(sender, e);
            }
          
        }


        private void clear1()
        {
            //vid = "0";
            textBox1.Text = "";
            textBox2.Text = "0";
            ansGridView1.Rows.Clear();
            textBox3.Text = "0";
            textBox4.Text = "";
            chamt = 0;
            vno = 0;
            label6.Text = "";
            label7.Text = "";
            label8.Text = "";
            label9.Text = "";
            label12.Text = "";
            label15.Text = "";
        }


        private void Save()
        {

            if (vid != "0")
            {
                string currLoc = Database.LocationId;
                string EditLoc = Database.GetScalarText("select locationId from voucherinfos where vi_id='" + vid + "'");
                if (currLoc == EditLoc)
                {


                }
                else
                {
                    MessageBox.Show("Your Current Location is " + funs.Select_location_name(currLoc) + " and You are Trying to Edit " + funs.Select_location_name(EditLoc) + "'s Booking. Sorry You Don't Have Permission to do This", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

            }
            string prefix = "";
            string postfix = "";
            int padding = 0;
            prefix = Database.GetScalarText("Select prefix from Location where LocationId='" + Database.LocationId + "'");
            
            if (vno == 0)
            {
                vno = funs.GenerateVno(vtid, dateTimePicker1.Value.ToString("dd-MMM-yyyy"), vid);
            }

            if (dtVoucherinfo.Rows.Count == 0)
            {
                dtVoucherinfo.Rows.Add();
            }


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
            
            string invoiceno = vno.ToString();
            dtVoucherinfo.Rows[0]["Invoiceno"] = prefix + invoiceno.PadLeft(padding, '0') + postfix;
            dtVoucherinfo.Rows[0]["Vt_id"] = vtid;
            dtVoucherinfo.Rows[0]["Vnumber"] = vno;
            dtVoucherinfo.Rows[0]["Roff"] = 0;
            dtVoucherinfo.Rows[0]["IsSelf"] = false;
            dtVoucherinfo.Rows[0]["Vdate"] = dateTimePicker1.Value.Date.ToString(Database.dformat);
            dtVoucherinfo.Rows[0]["vt_id"] = vtid;
            dtVoucherinfo.Rows[0]["Tdtype"] = false;
            dtVoucherinfo.Rows[0]["RoffChanged"] = false;
            dtVoucherinfo.Rows[0]["TaxChanged"] = false;
            dtVoucherinfo.Rows[0]["Formc"] = false;
            dtVoucherinfo.Rows[0]["Dbilled"] = false;
            dtVoucherinfo.Rows[0]["iscancel"] = iscancel;
            dtVoucherinfo.Rows[0]["As_Per"] = "";
            dtVoucherinfo.Rows[0]["DeliveryType"] = "";
            dtVoucherinfo.Rows[0]["Delivery_adrs"] = "";
            dtVoucherinfo.Rows[0]["DR"] = 0;
            dtVoucherinfo.Rows[0]["DD"] = 0;
            string loca_dp_id = Database.GetScalarText("Select dp_id from Location where LocationId='"+Database.LocationId+"'");
            dtVoucherinfo.Rows[0]["Consigner_id"] = loca_dp_id;
            dtVoucherinfo.Rows[0]["Grno"] = GR_id;
            dtVoucherinfo.Rows[0]["remarks"] = textBox5.Text;
            dtVoucherinfo.Rows[0]["Db_id"] =funs.Select_db_id(textBox4.Text);
            dtVoucherinfo.Rows[0]["TotalAmount"] = textBox3.Text;


            if (vid == "0")
            {
                dtVoucherinfo.Rows[0]["CreTime"] = System.DateTime.Now.ToString("HH:mm:ss");
                dtVoucherinfo.Rows[0]["create_date"] = System.DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                dtVoucherinfo.Rows[0]["user_id"] = Database.user_id;
            }
            if (vid != "0")
            {
                if (gresave == true)
                {
                    dtVoucherinfo.Rows[0]["modifyby_id"] = "";
                }
                else
                {
                    dtVoucherinfo.Rows[0]["modifyby_id"] = Database.user_id;
                }
            }
            dtVoucherinfo.Rows[0]["modify_date"] = System.DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
            dtVoucherinfo.Rows[0]["ModTime"] = System.DateTime.Now.ToString("HH:mm:ss");
            dtVoucherinfo.Rows[0]["PaymentMode"] = textBox6.Text;
            Database.SaveData(dtVoucherinfo);


            if (vid == "0")
            {
                vid = dtVoucherinfo.Rows[0]["Vi_id"].ToString();
            }
            DataTable dttemp = new DataTable("Voucherdets");
            Database.GetSqlData("Select * from Voucherdets where Vi_id='" + vid + "'", dttemp);
            for (int i = 0; i < dttemp.Rows.Count; i++)
            {
                dttemp.Rows[i].Delete();
            }
            Database.SaveData(dttemp);

            int Nid2 = 1;
            DataTable dtidvd = new DataTable();
            Database.GetSqlData("select max(Nid) as Nid from Voucherdets where locationid='" + Database.LocationId + "'", dtidvd);
            if (dtidvd.Rows[0][0].ToString() != "")
            {
                Nid2 = int.Parse(dtidvd.Rows[0][0].ToString()) + 1;
            }

            dtVoucherdet = new DataTable("Voucherdets");
            Database.GetSqlData("Select * from Voucherdets where Vi_id='" + vid + "' order by Itemsr", dtVoucherdet);

            dtVoucherdet.Rows.Clear();
            for (int i = 0; i < ansGridView1.Rows.Count; i++)
            {
                //dtVoucherdet.Rows.Add();
                //dtVoucherdet.Rows[i]["Nid"] = Nid2;
                //dtVoucherdet.Rows[i]["LocationId"] = Database.LocationId;
                //dtVoucherdet.Rows[i]["vd_id"] = Database.LocationId + dtVoucherdet.Rows[i]["nid"].ToString();
                //dtVoucherdet.Rows[i]["remarkreq"] = false;
                //dtVoucherdet.Rows[i]["Vi_id"] = vid;
                //dtVoucherdet.Rows[i]["LocationId"] = Prelocationid;

                //dtVoucherdet.Rows[i]["multiplier"] = false;
                //dtVoucherdet.Rows[i]["Itemsr"] = ansGridView4.Rows[i].Cells["sno3"].Value;
                //dtVoucherdet.Rows[i]["ch_id"] = ansGridView4.Rows[i].Cells["Charg_id2"].Value;
                //dtVoucherdet.Rows[i]["Rate_am"] = ansGridView4.Rows[i].Cells["AmountB"].Value;
                //dtVoucherdet.Rows[i]["Amount"] = ansGridView4.Rows[i].Cells["CAmountB"].Value;
                //dtVoucherdet.Rows[i]["pur_sale_acc"] = ansGridView4.Rows[i].Cells["Accid2"].Value;
                //dtVoucherdet.Rows[i]["description"]=ansGridView4.Rows[i].Cells["Ctype2"].Value ;
                //dtVoucherdet.Rows[i]["per"] = ansGridView4.Rows[i].Cells["Addsub2"].Value;
                //dtVoucherdet.Rows[i]["booking_date"] = dateTimePicker1.Value.Date;
                //dtVoucherdet.Rows[i]["create_date"] = create_date;
                
                //dtVoucherdet.Rows[i]["modify_date"] = System.DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");



                dtVoucherdet.Rows.Add();
                dtVoucherdet.Rows[i]["Nid"] = Nid2;
                dtVoucherdet.Rows[i]["LocationId"] = Database.LocationId;
                dtVoucherdet.Rows[i]["vd_id"] = Database.LocationId + dtVoucherdet.Rows[i]["nid"].ToString();
                dtVoucherdet.Rows[i]["remarkreq"] = false;
                dtVoucherdet.Rows[i]["Vi_id"] = vid;
                dtVoucherdet.Rows[i]["ItemSr"] = ansGridView1.Rows[i].Cells["sno"].Value;
                dtVoucherdet.Rows[i]["Des_ac_id"] = funs.Select_item_id(ansGridView1.Rows[i].Cells["description"].Value.ToString());
                dtVoucherdet.Rows[i]["Description"] = ansGridView1.Rows[i].Cells["description"].Value.ToString();
                dtVoucherdet.Rows[i]["packing"] = ansGridView1.Rows[i].Cells["unt"].Value;
                dtVoucherdet.Rows[i]["Quantity"] = ansGridView1.Rows[i].Cells["Quantity"].Value;
                dtVoucherdet.Rows[i]["weight"] = ansGridView1.Rows[i].Cells["weight"].Value;
                dtVoucherdet.Rows[i]["rate_am"] = ansGridView1.Rows[i].Cells["Rate_am"].Value;
                dtVoucherdet.Rows[i]["Amount"] = ansGridView1.Rows[i].Cells["Amount"].Value;
                dtVoucherdet.Rows[i]["ChargedWeight"] = ansGridView1.Rows[i].Cells["ChargedWeight"].Value;
                dtVoucherdet.Rows[i]["Per"] = ansGridView1.Rows[i].Cells["Per"].Value;
                dtVoucherdet.Rows[i]["bharti"] = 0;
                dtVoucherdet.Rows[i]["LocationId"] = Prelocationid;
                dtVoucherdet.Rows[i]["freightmr"] = 0;
                dtVoucherdet.Rows[i]["multiplier"] = double.Parse(ansGridView1.Rows[i].Cells["multiplier"].Value.ToString());

                dtVoucherdet.Rows[i]["exp1rate"] = double.Parse(ansGridView1.Rows[i].Cells["exp1rate"].Value.ToString());
                dtVoucherdet.Rows[i]["exp2rate"] = double.Parse(ansGridView1.Rows[i].Cells["exp2rate"].Value.ToString());
             
                
                dtVoucherdet.Rows[i]["exp3rate"] = double.Parse(ansGridView1.Rows[i].Cells["exp3rate"].Value.ToString());
                dtVoucherdet.Rows[i]["exp4rate"] = double.Parse(ansGridView1.Rows[i].Cells["exp4rate"].Value.ToString());
                dtVoucherdet.Rows[i]["exp5rate"] = 0;
                dtVoucherdet.Rows[i]["exp6rate"] = 0;
                dtVoucherdet.Rows[i]["exp7rate"] = 0;
                dtVoucherdet.Rows[i]["exp8rate"] = 0;
                dtVoucherdet.Rows[i]["exp9rate"] = 0;
                dtVoucherdet.Rows[i]["exp10rate"] = 0;
                dtVoucherdet.Rows[i]["exp11rate"] = 0;
                dtVoucherdet.Rows[i]["exp1amt"] = double.Parse(ansGridView1.Rows[i].Cells["exp1amt"].Value.ToString());
                dtVoucherdet.Rows[i]["exp2amt"] = double.Parse(ansGridView1.Rows[i].Cells["exp2amt"].Value.ToString());
                dtVoucherdet.Rows[i]["exp3amt"] = double.Parse(ansGridView1.Rows[i].Cells["exp3amt"].Value.ToString());
                dtVoucherdet.Rows[i]["exp4amt"] = double.Parse(ansGridView1.Rows[i].Cells["exp4amt"].Value.ToString());
                dtVoucherdet.Rows[i]["exp5amt"] = 0;
                dtVoucherdet.Rows[i]["exp6amt"] = 0;
                dtVoucherdet.Rows[i]["exp7amt"] = 0;
                dtVoucherdet.Rows[i]["exp8amt"] = 0;
                dtVoucherdet.Rows[i]["exp9amt"] = 0;
                dtVoucherdet.Rows[i]["exp10amt"] = 0;
                dtVoucherdet.Rows[i]["exp11amt"] = 0;
                dtVoucherdet.Rows[i]["exp1mr"] = double.Parse(ansGridView1.Rows[i].Cells["exp1mr"].Value.ToString());
                dtVoucherdet.Rows[i]["exp2mr"] = double.Parse(ansGridView1.Rows[i].Cells["exp2mr"].Value.ToString());
                dtVoucherdet.Rows[i]["exp3mr"] = double.Parse(ansGridView1.Rows[i].Cells["exp3mr"].Value.ToString());
                dtVoucherdet.Rows[i]["exp4mr"] = double.Parse(ansGridView1.Rows[i].Cells["exp4mr"].Value.ToString());
                dtVoucherdet.Rows[i]["exp5mr"] = 0;
                dtVoucherdet.Rows[i]["exp6mr"] = 0;
                dtVoucherdet.Rows[i]["exp7mr"] = 0;
                dtVoucherdet.Rows[i]["exp8mr"] = 0;
                dtVoucherdet.Rows[i]["exp9mr"] = 0;
                dtVoucherdet.Rows[i]["exp10mr"] = 0;
                dtVoucherdet.Rows[i]["exp11mr"] = 0;
                dtVoucherdet.Rows[i]["exp1type"] = ansGridView1.Rows[i].Cells["exp1type"].Value.ToString();
                dtVoucherdet.Rows[i]["exp2type"] = ansGridView1.Rows[i].Cells["exp2type"].Value.ToString();
                dtVoucherdet.Rows[i]["exp3type"] = ansGridView1.Rows[i].Cells["exp3type"].Value.ToString();
                dtVoucherdet.Rows[i]["exp4type"] = ansGridView1.Rows[i].Cells["exp4type"].Value.ToString();
                dtVoucherdet.Rows[i]["exp5type"] = "";
                dtVoucherdet.Rows[i]["exp6type"] = "";
                dtVoucherdet.Rows[i]["exp7type"] = "";
                dtVoucherdet.Rows[i]["exp8type"] = "";
                dtVoucherdet.Rows[i]["exp9type"] = "";
                dtVoucherdet.Rows[i]["exp10type"] = "";
                dtVoucherdet.Rows[i]["exp11type"] = "";
                dtVoucherdet.Rows[i]["totexp"] = 0;
                dtVoucherdet.Rows[i]["ItemAmount"] = 0;

                dtVoucherdet.Rows[i]["booking_date"] = dateTimePicker1.Value.Date;
                dtVoucherdet.Rows[i]["create_date"] = create_date;
                dtVoucherdet.Rows[i]["modify_date"] = System.DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                dtVoucherdet.Rows[i]["ch_id"] = null;
                Nid2++;


              //  Nid2++;
            }

            Database.SaveData(dtVoucherdet);

            DataTable dtstocks = new DataTable("stocks");
            Database.GetSqlData("Select * from stocks where Vid='" + vid + "'", dtstocks);
            for (int j = 0; j < dtstocks.Rows.Count; j++)
            {
                dtstocks.Rows[j].Delete();
            }
            Database.SaveData(dtstocks);
           // string grid = Database.GetScalarText("SELECT Voucherinfos.vi_id from voucherinfos  where       (CAST(dbo.VOUCHERINFOs.Vnumber AS varchar(10)) = '" + textBox1.Text + "')");

            if (iscancel == false)
            {
                dtstocks.Rows.Add();

                dtstocks.Rows[dtstocks.Rows.Count - 1]["Vid"] = vid;

                dtstocks.Rows[dtstocks.Rows.Count - 1]["GR_id"] = GR_id;
                dtstocks.Rows[dtstocks.Rows.Count - 1]["Quantity"] = -1;

                dtstocks.Rows[dtstocks.Rows.Count - 1]["Step"] = "Step2";
                dtstocks.Rows[dtstocks.Rows.Count - 1]["Godown_id"] = Database.LocationId;
                string aliasname = Database.GetScalarText("Select Aliasname from vouchertypes where vt_id=" + vtid);
                dtstocks.Rows[dtstocks.Rows.Count - 1]["Narration"] = aliasname + " At " + funs.Select_dp_nm(loca_dp_id);

                dtstocks.Rows[dtstocks.Rows.Count - 1]["GRNo"] = textBox1.Text;
                dtstocks.Rows[dtstocks.Rows.Count - 1]["GRDate"] = DateTime.Parse(label15.Text);

                dtstocks.Rows[dtstocks.Rows.Count - 1]["Consigner_id"] = funs.Select_ac_id(label6.Text);
                dtstocks.Rows[dtstocks.Rows.Count - 1]["Consignee_id"] = funs.Select_ac_id(label7.Text);


                dtstocks.Rows[dtstocks.Rows.Count - 1]["Source_id"] = funs.Select_dp_id(label13.Text);
                dtstocks.Rows[dtstocks.Rows.Count - 1]["Destination_id"] = funs.Select_dp_id(label20.Text);
                if (label16.Text == "To Pay")
                {
                    dtstocks.Rows[dtstocks.Rows.Count - 1]["ToPay"] = double.Parse(textBox2.Text);
                    dtstocks.Rows[dtstocks.Rows.Count - 1]["TBB"] = 0;

                    dtstocks.Rows[dtstocks.Rows.Count - 1]["Paid"] = 0;
                    dtstocks.Rows[dtstocks.Rows.Count - 1]["FOC"] = 0;
                }
                else if (label16.Text == "FOC")
                {
                    dtstocks.Rows[dtstocks.Rows.Count - 1]["ToPay"] = 0;
                    dtstocks.Rows[dtstocks.Rows.Count - 1]["TBB"] = 0;

                    dtstocks.Rows[dtstocks.Rows.Count - 1]["Paid"] = 0;
                    dtstocks.Rows[dtstocks.Rows.Count - 1]["FOC"] = double.Parse(textBox2.Text);
                }
                else if (label16.Text == "Paid")
                {
                    dtstocks.Rows[dtstocks.Rows.Count - 1]["ToPay"] = 0;
                    dtstocks.Rows[dtstocks.Rows.Count - 1]["TBB"] = 0;

                    dtstocks.Rows[dtstocks.Rows.Count - 1]["Paid"] = double.Parse(textBox2.Text);
                    dtstocks.Rows[dtstocks.Rows.Count - 1]["FOC"] = 0;
                }
                else if (label16.Text == "T.B.B.")
                {
                    dtstocks.Rows[dtstocks.Rows.Count - 1]["ToPay"] =0;
                    dtstocks.Rows[dtstocks.Rows.Count - 1]["TBB"] = double.Parse(textBox2.Text);

                    dtstocks.Rows[dtstocks.Rows.Count - 1]["Paid"] = 0;
                    dtstocks.Rows[dtstocks.Rows.Count - 1]["FOC"] = 0;
                }

                dtstocks.Rows[dtstocks.Rows.Count - 1]["grcharge"] = double.Parse(label22.Text);
                dtstocks.Rows[dtstocks.Rows.Count - 1]["othcharge"] = double.Parse(label24.Text);

                dtstocks.Rows[dtstocks.Rows.Count - 1]["totpkts"] = double.Parse(label8.Text);
                dtstocks.Rows[dtstocks.Rows.Count - 1]["totweight"] = double.Parse(label9.Text);
                dtstocks.Rows[dtstocks.Rows.Count - 1]["Actweight"] = double.Parse(label34.Text);
                dtstocks.Rows[dtstocks.Rows.Count - 1]["freight"] = double.Parse(label32.Text);
                dtstocks.Rows[dtstocks.Rows.Count - 1]["grtype"] = label16.Text;
                dtstocks.Rows[dtstocks.Rows.Count - 1]["private"] = label31.Text;
                int count = ansGridView1.Rows.Count;


                if (count == 1)
                {
                    dtstocks.Rows[dtstocks.Rows.Count - 1]["ItemName"] = ansGridView1.Rows[0].Cells["description"].Value.ToString();
                }
                else
                {
                    dtstocks.Rows[dtstocks.Rows.Count - 1]["ItemName"] = ansGridView1.Rows[0].Cells["description"].Value.ToString() + " + " + (count - 1).ToString();
                }
               // dtstocks.Rows[dtstocks.Rows.Count - 1]["itemname"] = ansGridView1.Rows[0].Cells["description"].Value.ToString();
                dtstocks.Rows[dtstocks.Rows.Count - 1]["packing"] = ansGridView1.Rows[0].Cells["unt"].Value.ToString();


                dtstocks.Rows[dtstocks.Rows.Count - 1]["remark"] = label29.Text;
                dtstocks.Rows[dtstocks.Rows.Count - 1]["deliverytype"] = label26.Text;
                Database.SaveData(dtstocks);
            }
            dttemp = new DataTable("Vouchargess");
             Database.GetSqlData("Select * from Vouchargess where Vi_id='" + vid + "'", dttemp);
            for (int i = 0; i < dttemp.Rows.Count; i++)
            {
                dttemp.Rows[i].Delete();
            }
            Database.SaveData(dttemp);


            dtVoucharges = new DataTable("Vouchargess");
            Database.GetSqlData("Select * from Vouchargess where Vi_id='" + vid + "' order by Srno", dtVoucharges);
            dtVoucharges.Rows.Clear();
            DataTable dtidv = new DataTable();
            Database.GetSqlData("select max(Nid) as Nid from VOUCHARGESs where locationid='" + Database.LocationId + "'", dtidv);
            int Nid3 = 1;
            if (dtidv.Rows[0][0].ToString() != "")
            {
                Nid3 = int.Parse(dtidv.Rows[0][0].ToString()) + 1;
            }

            for (int i = 0; i < ansGridView4.Rows.Count; i++)
            {
                dtVoucharges.Rows.Add();

                dtVoucharges.Rows[dtVoucharges.Rows.Count - 1]["Nid"] = Nid3;
                dtVoucharges.Rows[dtVoucharges.Rows.Count - 1]["Vi_id"] = vid;
               
                dtVoucharges.Rows[i]["Srno"] = ansGridView4.Rows[i].Cells["sno3"].Value.ToString();
                dtVoucharges.Rows[i]["Charg_Name"] = ansGridView4.Rows[i].Cells["Charg_Name2"].Value.ToString();

                dtVoucharges.Rows[i]["Amount"] = double.Parse(ansGridView4.Rows[i].Cells["CAmountB"].Value.ToString());
                dtVoucharges.Rows[dtVoucharges.Rows.Count - 1]["locationid"] = Prelocationid;
                dtVoucharges.Rows[dtVoucharges.Rows.Count - 1]["create_date"] = create_date;
                dtVoucharges.Rows[dtVoucharges.Rows.Count - 1]["modify_date"] = System.DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                dtVoucharges.Rows[dtVoucharges.Rows.Count - 1]["vc_id"] = dtVoucharges.Rows[dtVoucharges.Rows.Count - 1]["locationid"] + dtVoucharges.Rows[dtVoucharges.Rows.Count - 1]["Nid"].ToString();
                Nid3++;

               
            }
            Database.SaveData(dtVoucharges);

            dttemp = new DataTable("Journals");
            Database.GetSqlData("Select * from Journals where Vi_id='" + vid + "'", dttemp);
            for (int i = 0; i < dttemp.Rows.Count; i++)
            {
                dttemp.Rows[i].Delete();
            }
            Database.SaveData(dttemp);


            DataTable dtJournal = new DataTable("Journals");
            Database.GetSqlData("Select * from Journals where Vi_id='" + vid + "' ", dtJournal);
            string ac_id = "";

            if (textBox6.Text == "Cash")
            {
                ac_id = Database.LocationCashAcc_id;
            }
            else
            {
                ac_id = Database.GetScalarText("Select Ac_id from Accounts where name='"+ label7.Text+"'");
            }

            dtJournal.Rows.Add();
            dtJournal.Rows[dtJournal.Rows.Count - 1]["vdate"] = dateTimePicker1.Value.ToString("dd-MMM-yyyy");
            dtJournal.Rows[dtJournal.Rows.Count - 1]["Vi_id"] = vid;
            dtJournal.Rows[dtJournal.Rows.Count - 1]["Ac_id"] = ac_id;
            dtJournal.Rows[dtJournal.Rows.Count - 1]["Opp_acid"] = funs.Select_ac_id("Delivery Charges"); 
            dtJournal.Rows[dtJournal.Rows.Count - 1]["Narr"] = textBox5.Text; ;
            dtJournal.Rows[dtJournal.Rows.Count - 1]["Narr2"] = textBox5.Text; ;
            dtJournal.Rows[dtJournal.Rows.Count - 1]["Sno"] = 1;
            dtJournal.Rows[dtJournal.Rows.Count - 1]["LocationId"] = Database.LocationId;
            dtJournal.Rows[dtJournal.Rows.Count - 1]["Reffno"] = "";
            dtJournal.Rows[dtJournal.Rows.Count - 1]["Amount"] = double.Parse(textBox3.Text);

            dtJournal.Rows.Add();
            dtJournal.Rows[dtJournal.Rows.Count - 1]["vdate"] = dateTimePicker1.Value.ToString("dd-MMM-yyyy");
            dtJournal.Rows[dtJournal.Rows.Count - 1]["Vi_id"] = vid;
            dtJournal.Rows[dtJournal.Rows.Count - 1]["Ac_id"] = funs.Select_ac_id("Delivery Charges"); 
            dtJournal.Rows[dtJournal.Rows.Count - 1]["Opp_acid"] = ac_id;
            dtJournal.Rows[dtJournal.Rows.Count - 1]["Narr"] = textBox5.Text; ;
            dtJournal.Rows[dtJournal.Rows.Count - 1]["Narr2"] = textBox5.Text; ;
            dtJournal.Rows[dtJournal.Rows.Count - 1]["Sno"] = 1;
            dtJournal.Rows[dtJournal.Rows.Count - 1]["LocationId"] = Database.LocationId;
            dtJournal.Rows[dtJournal.Rows.Count - 1]["Reffno"] = "";
            dtJournal.Rows[dtJournal.Rows.Count - 1]["Amount"] = -1* double.Parse(textBox3.Text);


            Database.SaveData(dtJournal);

            funs.ShowBalloonTip("Saved Successfully","Saved");

            //if (print == true)
            //{
            //    frm_printcopy frm = new frm_printcopy("Print", vid, vtid);
            //    frm.ShowDialog();
            //}
           


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
            
                //frm_printcopy frm = new frm_printcopy("Print", vid, vtid);
                //frm.ShowDialog();
           
        }
        private void view()
        {

           
                frm_printcopy frm = new frm_printcopy("View", vid, vtid);
                frm.ShowDialog();
            
           

        }
        private void clear()
        {

            if (gvi_id == "0")
            {
                textBox1.Select();
                vid = "0";
                label11.Text = "";
                textBox1.Text = "";
                textBox2.Text = "0";
                ansGridView1.Rows.Clear();
                textBox3.Text = "0";
                textBox4.Text = "";
                chamt = 0;
                vno = 0;
                label6.Text = "";
                label12.Text = "";
                label7.Text = "";
                label8.Text = "";
                label9.Text = "";
                SetVno();
                label15.Text = "";
                dtVoucherinfo.Rows.Clear();
                dtVoucherdet.Rows.Clear();
                dtVoucharges.Rows.Clear();
             //   LoadData("0", "Delivery");
            }
            else
            {
                this.Close();
                this.Dispose();
            }


        }
        private bool validate()
        {
            if (textBox1.Text == "")
            {
                textBox1.Focus();
                MessageBox.Show("Please Enter some values");
                return false;
            }

            if (textBox6.Text == "")
            {
                textBox6.Focus();
                MessageBox.Show("Enter Mode");
                return false;
            }

            if (ansGridView1.Rows.Count == 0)
            {
                return false;
            }


            if (funs.GenerateVno(vtid, dateTimePicker1.Value.ToString("dd-MMM-yyyy"), vid) == 0 && vno == 0)
            {
                MessageBox.Show("Voucher Number can't be created on this date.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (vid != "0")
            {
                if (Database.GetScalarInt("SELECT count(*) FROM VOUCHERdets WHERE Delivery_id='" + vid + "' ") != 0)
                {
                    MessageBox.Show("This delivery is billed");
                    return false;
                }
            }
            return true;
        }


        public void Displaydata(string vid)
        {
            vid = gvi_id;
            dtVoucherinfo = new DataTable("Voucherinfos");
            Database.GetSqlData("Select * from Voucherinfos where Vi_id='"+ vid+"'",dtVoucherinfo);
            if (dtVoucherinfo.Rows.Count > 0)
            {
                dateTimePicker1.Value = DateTime.Parse(dtVoucherinfo.Rows[0]["Vdate"].ToString());
                textBox15.Text = funs.Select_vt_nm(int.Parse(dtVoucherinfo.Rows[0]["vt_id"].ToString()));

                vno = int.Parse(dtVoucherinfo.Rows[0]["vnumber"].ToString());
                label11.Text = vno.ToString();
                textBox3.Text = dtVoucherinfo.Rows[0]["totalamount"].ToString();
                textBox5.Text = dtVoucherinfo.Rows[0]["remarks"].ToString();
                GR_id = dtVoucherinfo.Rows[0]["Grno"].ToString();

                if (GR_id.Contains("/") == true)
                {

                    textBox1.Text = Database.GetScalarText("Select grno from stocks where GR_id='" + GR_id + "'");
                }
                else
                {
                    textBox1.Text = Database.GetScalarText("Select Invoiceno from Voucherinfos where vi_id='" + GR_id + "'");
                }
                textBox4.Text = funs.Select_db_nm(dtVoucherinfo.Rows[0]["Db_id"].ToString());
                Prelocationid = dtVoucherinfo.Rows[0]["Locationid"].ToString();
                create_date = DateTime.Parse(dtVoucherinfo.Rows[0]["create_date"].ToString());
                textBox6.Text=  dtVoucherinfo.Rows[0]["PaymentMode"].ToString();
                if (bool.Parse(dtVoucherinfo.Rows[0]["Iscancel"].ToString()) == true)
                {
                    label28.Visible = true;
                    label28.Text = "Cancelled";
                    iscancel = bool.Parse(dtVoucherinfo.Rows[0]["Iscancel"].ToString());
                }

            }

            
            dtVoucherdet = new DataTable("Voucherdets");
            Database.GetSqlData("Select * from Voucherdets where Vi_id='" + vid + "' order by Itemsr", dtVoucherdet);
            ansGridView1.Rows.Clear();
            for (int i = 0; i < dtVoucherdet.Rows.Count; i++)
            {
                //ansGridView4.Rows.Add();
                //ansGridView4.Rows[i].Cells["sno3"].Value = dtVoucherdet.Rows[i]["Itemsr"].ToString();
                //ansGridView4.Rows[i].Cells["Charg_id2"].Value = dtVoucherdet.Rows[i]["ch_id"].ToString();
                //ansGridView4.Rows[i].Cells["Charg_Name2"].Value = Database.GetScalarText("Select Name from Charges where Ch_id='" + dtVoucherdet.Rows[i]["ch_id"].ToString() + "'");
                //ansGridView4.Rows[i].Cells["AmountB"].Value = dtVoucherdet.Rows[i]["Rate_am"].ToString();
                //ansGridView4.Rows[i].Cells["CAmountB"].Value = dtVoucherdet.Rows[i]["Amount"].ToString();
                //ansGridView4.Rows[i].Cells["Accid2"].Value = dtVoucherdet.Rows[i]["pur_sale_acc"].ToString();
                //ansGridView4.Rows[i].Cells["Ctype2"].Value = dtVoucherdet.Rows[i]["description"].ToString();
                //ansGridView4.Rows[i].Cells["Addsub2"].Value = dtVoucherdet.Rows[i]["per"].ToString();



                ansGridView1.Rows.Add();
                ansGridView1.Rows[i].Cells["sno"].Value = dtVoucherdet.Rows[i]["ItemSr"];
                ansGridView1.Rows[i].Cells["description"].Value = funs.Select_item_nm(dtVoucherdet.Rows[i]["Des_ac_id"].ToString());
                ansGridView1.Rows[i].Cells["unt"].Value = dtVoucherdet.Rows[i]["packing"];
                ansGridView1.Rows[i].Cells["Quantity"].Value = funs.DecimalPoint(dtVoucherdet.Rows[i]["Quantity"], 2);
                ansGridView1.Rows[i].Cells["weight"].Value = funs.DecimalPoint(dtVoucherdet.Rows[i]["weight"], 3);
                ansGridView1.Rows[i].Cells["Rate_am"].Value = funs.DecimalPoint(dtVoucherdet.Rows[i]["Rate_am"], 2);
                ansGridView1.Rows[i].Cells["Amount"].Value = funs.DecimalPoint(dtVoucherdet.Rows[i]["Amount"], 2);
                ansGridView1.Rows[i].Cells["ChargedWeight"].Value = funs.DecimalPoint(dtVoucherdet.Rows[i]["ChargedWeight"], 3);
                ansGridView1.Rows[i].Cells["Per"].Value = dtVoucherdet.Rows[i]["Per"].ToString();
                ansGridView1.Rows[i].Cells["multiplier"].Value = dtVoucherdet.Rows[i]["multiplier"].ToString();

                ansGridView1.Rows[i].Cells["freightmr"].Value = funs.DecimalPoint(dtVoucherdet.Rows[i]["freightmr"], 2);
                ansGridView1.Rows[i].Cells["bharti"].Value = funs.DecimalPoint(dtVoucherdet.Rows[i]["bharti"], 2);
                ansGridView1.Rows[i].Cells["exp1rate"].Value = funs.DecimalPoint(dtVoucherdet.Rows[i]["exp1rate"], 2);
                ansGridView1.Rows[i].Cells["exp2rate"].Value = funs.DecimalPoint(dtVoucherdet.Rows[i]["exp2rate"], 2);
                ansGridView1.Rows[i].Cells["exp3rate"].Value = funs.DecimalPoint(dtVoucherdet.Rows[i]["exp3rate"], 2);
                ansGridView1.Rows[i].Cells["exp4rate"].Value = funs.DecimalPoint(dtVoucherdet.Rows[i]["exp4rate"], 2);
                ansGridView1.Rows[i].Cells["exp5rate"].Value = funs.DecimalPoint(dtVoucherdet.Rows[i]["exp5rate"], 2);
                ansGridView1.Rows[i].Cells["exp6rate"].Value = funs.DecimalPoint(dtVoucherdet.Rows[i]["exp6rate"], 2);
                ansGridView1.Rows[i].Cells["exp7rate"].Value = funs.DecimalPoint(dtVoucherdet.Rows[i]["exp7rate"], 2);
                ansGridView1.Rows[i].Cells["exp8rate"].Value = funs.DecimalPoint(dtVoucherdet.Rows[i]["exp8rate"], 2);
                ansGridView1.Rows[i].Cells["exp9rate"].Value = funs.DecimalPoint(dtVoucherdet.Rows[i]["exp9rate"], 2);
                ansGridView1.Rows[i].Cells["exp10rate"].Value = funs.DecimalPoint(dtVoucherdet.Rows[i]["exp10rate"], 2);

                ansGridView1.Rows[i].Cells["exp1amt"].Value = funs.DecimalPoint(dtVoucherdet.Rows[i]["exp1amt"], 2);
                ansGridView1.Rows[i].Cells["exp2amt"].Value = funs.DecimalPoint(dtVoucherdet.Rows[i]["exp2amt"], 2);
                ansGridView1.Rows[i].Cells["exp3amt"].Value = funs.DecimalPoint(dtVoucherdet.Rows[i]["exp3amt"], 2);
                ansGridView1.Rows[i].Cells["exp4amt"].Value = funs.DecimalPoint(dtVoucherdet.Rows[i]["exp4amt"], 2);
                ansGridView1.Rows[i].Cells["exp5amt"].Value = funs.DecimalPoint(dtVoucherdet.Rows[i]["exp5amt"], 2);
                ansGridView1.Rows[i].Cells["exp6amt"].Value = funs.DecimalPoint(dtVoucherdet.Rows[i]["exp6amt"], 2);
                ansGridView1.Rows[i].Cells["exp7amt"].Value = funs.DecimalPoint(dtVoucherdet.Rows[i]["exp7amt"], 2);
                ansGridView1.Rows[i].Cells["exp8amt"].Value = funs.DecimalPoint(dtVoucherdet.Rows[i]["exp8amt"], 2);
                ansGridView1.Rows[i].Cells["exp9amt"].Value = funs.DecimalPoint(dtVoucherdet.Rows[i]["exp9amt"], 2);
                ansGridView1.Rows[i].Cells["exp10amt"].Value = funs.DecimalPoint(dtVoucherdet.Rows[i]["exp10amt"], 2);

                ansGridView1.Rows[i].Cells["exp1mr"].Value = funs.DecimalPoint(dtVoucherdet.Rows[i]["exp1mr"], 2);
                ansGridView1.Rows[i].Cells["exp2mr"].Value = funs.DecimalPoint(dtVoucherdet.Rows[i]["exp2mr"], 2);
                ansGridView1.Rows[i].Cells["exp3mr"].Value = funs.DecimalPoint(dtVoucherdet.Rows[i]["exp3mr"], 2);
                ansGridView1.Rows[i].Cells["exp4mr"].Value = funs.DecimalPoint(dtVoucherdet.Rows[i]["exp4mr"], 2);
                ansGridView1.Rows[i].Cells["exp5mr"].Value = funs.DecimalPoint(dtVoucherdet.Rows[i]["exp5mr"], 2);
                ansGridView1.Rows[i].Cells["exp6mr"].Value = funs.DecimalPoint(dtVoucherdet.Rows[i]["exp6mr"], 2);
                ansGridView1.Rows[i].Cells["exp7mr"].Value = funs.DecimalPoint(dtVoucherdet.Rows[i]["exp7mr"], 2);
                ansGridView1.Rows[i].Cells["exp8mr"].Value = funs.DecimalPoint(dtVoucherdet.Rows[i]["exp8mr"], 2);
                ansGridView1.Rows[i].Cells["exp9mr"].Value = funs.DecimalPoint(dtVoucherdet.Rows[i]["exp9mr"], 2);
                ansGridView1.Rows[i].Cells["exp10mr"].Value = funs.DecimalPoint(dtVoucherdet.Rows[i]["exp10mr"], 2);

                ansGridView1.Rows[i].Cells["exp1type"].Value = dtVoucherdet.Rows[i]["exp1type"];
                ansGridView1.Rows[i].Cells["exp2type"].Value = dtVoucherdet.Rows[i]["exp2type"];
                ansGridView1.Rows[i].Cells["exp3type"].Value = dtVoucherdet.Rows[i]["exp3type"];
                ansGridView1.Rows[i].Cells["exp4type"].Value = dtVoucherdet.Rows[i]["exp4type"];
                ansGridView1.Rows[i].Cells["exp5type"].Value = dtVoucherdet.Rows[i]["exp5type"];
                ansGridView1.Rows[i].Cells["exp6type"].Value = dtVoucherdet.Rows[i]["exp6type"];
                ansGridView1.Rows[i].Cells["exp7type"].Value = dtVoucherdet.Rows[i]["exp7type"];
                ansGridView1.Rows[i].Cells["exp8type"].Value = dtVoucherdet.Rows[i]["exp8type"];
                ansGridView1.Rows[i].Cells["exp9type"].Value = dtVoucherdet.Rows[i]["exp9type"];
                ansGridView1.Rows[i].Cells["exp10type"].Value = dtVoucherdet.Rows[i]["exp10type"];


                ansGridView1.Rows[i].Cells["totexp"].Value = funs.DecimalPoint(dtVoucherdet.Rows[i]["totexp"], 2);
                ansGridView1.Rows[i].Cells["ItemAmount"].Value = funs.DecimalPoint(dtVoucherdet.Rows[i]["ItemAmount"], 2);

                string cmbVouTyp = "";
                DataTable dt = new DataTable();
                if (GR_id.Contains("/") == true)
                {

                    cmbVouTyp = "SELECT Stocks.ItemName AS description, Stocks.Packing, Stocks.TotPkts AS Quantity, Stocks.ActWeight AS ActWeight, Stocks.TotWeight AS Weight, Stocks.Freight AS Rate_am, Stocks.Grno, dbo.Stocks.ToPay + dbo.Stocks.TBB + dbo.Stocks.Paid + dbo.Stocks.FOC  AS Amount, ACCOUNTs_1.name AS Consigner, ACCOUNTs.name AS Consignee, Stocks.Private,Stocks.Remark,Stocks.grcharge,Stocks.Othcharge, Stocks.deliverytype,Stocks.GRDate AS Vdate,   Stocks.Source_id, Stocks.Destination_id, Stocks.GRType FROM ACCOUNTs RIGHT OUTER JOIN  Stocks ON ACCOUNTs.ac_id = Stocks.Consignee_id LEFT OUTER JOIN  ACCOUNTs AS ACCOUNTs_1 ON Stocks.Consigner_id = ACCOUNTs_1.ac_id LEFT OUTER JOIN  VOUCHERTYPEs RIGHT OUTER JOIN  VOUCHERINFOs ON VOUCHERTYPEs.Vt_id = VOUCHERINFOs.Vt_id ON Stocks.vid = VOUCHERINFOs.Vi_id WHERE ( Stocks.GR_id = '" + GR_id + "') AND ( VOUCHERTYPEs.Type = 'GRByChallan')";
                    Database.GetSqlData(cmbVouTyp, dt);
                }

                else
                {
                   // cmbVouTyp = "SELECT VOUCHERINFOs.Totalamount AS Amount, Stocks.TotPkts   AS Quantity, Stocks.TotWeight   AS Weight,Stocks.Freight as Rate_am,  ACCOUNTs_1.name AS Consigner, ACCOUNTs.name AS Consignee,Stocks.Grno, Stocks.Source_id, Stocks.Destination_id, Stocks.GRType,  Stocks.GRDate AS vdate, Stocks.Private,Stocks.Remark,Stocks.grcharge,Stocks.Othcharge, Stocks.deliverytype FROM ACCOUNTs RIGHT OUTER JOIN  VOUCHERINFOs ON ACCOUNTs.ac_id = VOUCHERINFOs.Ac_id2 LEFT OUTER JOIN  Stocks ON VOUCHERINFOs.Vi_id = Stocks.vid LEFT OUTER JOIN  VOUCHERTYPEs ON VOUCHERINFOs.Vt_id = VOUCHERTYPEs.Vt_id LEFT OUTER JOIN  ACCOUNTs AS ACCOUNTs_1 ON VOUCHERINFOs.Ac_id = ACCOUNTs_1.ac_id WHERE ( VOUCHERINFOs.Vi_id = '" + vid + "')";
                    cmbVouTyp = "SELECT  Stocks.TotPkts AS Quantity,  Stocks.ActWeight AS ActWeight, Stocks.TotWeight AS Weight,  Stocks.Freight AS Rate_am, ACCOUNTs_1.name AS Consigner,   ACCOUNTs.name AS Consignee,  Stocks.GRNo,  Stocks.Source_id,  Stocks.Destination_id,  Stocks.GRType,  Stocks.GRDate AS vdate,   Stocks.Private,  Stocks.Remark,  Stocks.GRCharge,  Stocks.OthCharge,  Stocks.DeliveryType,   Stocks.ToPay +  Stocks.TBB +  Stocks.Paid +  Stocks.FOC AS Amount FROM  VOUCHERINFOs LEFT OUTER JOIN  ACCOUNTs RIGHT OUTER JOIN  Stocks ON  ACCOUNTs.ac_id =  Stocks.Consignee_id LEFT OUTER JOIN ACCOUNTs AS ACCOUNTs_1 ON  Stocks.Consigner_id = ACCOUNTs_1.ac_id ON  VOUCHERINFOs.Vi_id =  Stocks.vid LEFT OUTER JOIN  VOUCHERTYPEs ON  VOUCHERINFOs.Vt_id =  VOUCHERTYPEs.Vt_id WHERE ( VOUCHERINFOs.Vi_id = '" + GR_id + "') ";
                    Database.GetSqlData(cmbVouTyp, dt);

                }








                if (dt.Rows.Count != 0)
                {
                   double  amt = double.Parse(dt.Rows[0]["amount"].ToString());
                    textBox2.Text = funs.DecimalPoint(amt, 2);
                    label6.Text = dt.Rows[0]["Consigner"].ToString();
                    label7.Text = dt.Rows[0]["Consignee"].ToString();
                    label8.Text = funs.DecimalPoint(dt.Rows[0]["Quantity"].ToString(), 3);
                    label9.Text = funs.DecimalPoint(dt.Rows[0]["Weight"].ToString(), 3);

                    label22.Text = funs.DecimalPoint(dt.Rows[0]["grcharge"].ToString(), 2);
                    label24.Text = funs.DecimalPoint(dt.Rows[0]["othcharge"].ToString(), 2);
                    label32.Text = funs.DecimalPoint(dt.Rows[0]["rate_am"].ToString(), 2);
                    label34.Text = funs.DecimalPoint(dt.Rows[0]["Actweight"].ToString(), 2);
                    label31.Text = dt.Rows[0]["Private"].ToString();
                    label29.Text = dt.Rows[0]["remark"].ToString();
                    label15.Text = DateTime.Parse(dt.Rows[0]["Vdate"].ToString()).ToString(Database.dformat);
                    label16.Text = dt.Rows[0]["GRType"].ToString();
                    label26.Text = dt.Rows[0]["deliverytype"].ToString();

                    label13.Text = funs.Select_dp_nm(dt.Rows[0]["Source_id"].ToString());
                    label20.Text = funs.Select_dp_nm(dt.Rows[0]["destination_id"].ToString());
                }
                else
                {
                    textBox2.Text = "0.00";
                    textBox5.Text = "";
                    label6.Text = "";
                    label7.Text = "";
                    label13.Text = "";
                    label20.Text = "";
                    label8.Text = "";
                    label9.Text = "";
                    label12.Text = "";
                    label15.Text = "";
                    label34.Text = "";
                    ansGridView1.Rows.Clear();
                    textBox3.Text = "0.00";

                }
              
            }

            dtVoucharges = new DataTable("Vouchargess");
            Database.GetSqlData("Select * from Vouchargess where Vi_id='" + vid + "' order by Srno", dtVoucharges);
            ansGridView4.Rows.Clear();
            for (int i = 0; i < dtVoucharges.Rows.Count; i++)
            {
                ansGridView4.Rows.Add();
                ansGridView4.Rows[i].Cells["Sno3"].Value = dtVoucharges.Rows[i]["Srno"].ToString();
                ansGridView4.Rows[i].Cells["Charg_Name2"].Value = dtVoucharges.Rows[i]["Charg_Name"].ToString();
                ansGridView4.Rows[i].Cells["Charg_id2"].Value = 0;


                ansGridView4.Rows[i].Cells["CAmountB"].Value = funs.DecimalPoint(dtVoucharges.Rows[i]["Amount"],2);
                ansGridView4.Rows[i].Cells["Accid2"].Value = 0;
                ansGridView4.Rows[i].Cells["Addsub2"].Value = 0;
                ansGridView4.Rows[i].Cells["Ctype2"].Value = "";

            }
            for (int i = 0; i < ansGridView1.Rows.Count; i++)
            {
                calc(i);
            }
        }


        private void frm_Delivery_Load(object sender, EventArgs e)
        {
            SideFill();
            ansGridView4.Columns["CamountB"].ReadOnly = true;
        }
        private void SideFill()
        {
            flowLayoutPanel2.Controls.Clear();
            DataTable dtsidefill = new DataTable();
            dtsidefill.Columns.Add("Name", typeof(string));
            dtsidefill.Columns.Add("DisplayName", typeof(string));
            dtsidefill.Columns.Add("ShortcutKey", typeof(string));
            dtsidefill.Columns.Add("Visible", typeof(bool));

            //save
            dtsidefill.Rows.Add();
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Name"] = "save";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["DisplayName"] = "Save";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["ShortcutKey"] = "^S";
            if (gvi_id != "0")
            {
                if (Database.utype == "User")
                {
                    if (dtVoucherinfo.Rows.Count == 1)
                    {
                        if (DateTime.Parse(dtVoucherinfo.Rows[0]["Vdate"].ToString()).ToString(Database.dformat) == Database.ldate.ToString(Database.dformat))
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
                else
                {
                    dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = true;
                }
            }
            else
            {
                dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = true;
            }
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
                dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = true;
            }

            if (Database.printtype == "WIN")
            {

                //print
                dtsidefill.Rows.Add();
                dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Name"] = "Print";
                dtsidefill.Rows[dtsidefill.Rows.Count - 1]["DisplayName"] = "Print";
                dtsidefill.Rows[dtsidefill.Rows.Count - 1]["ShortcutKey"] = "^P";


                if (gvi_id != "0")
                {
                    if (Database.utype == "User")
                    {
                        if (dtVoucherinfo.Rows.Count == 1)
                        {
                            if (DateTime.Parse(dtVoucherinfo.Rows[0]["Vdate"].ToString()).ToString(Database.dformat) == Database.ldate.ToString(Database.dformat))
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
                    else
                    {
                        dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = true;
                    }
                }
                else
                {
                    dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = true;
                }

                //print preview
                dtsidefill.Rows.Add();
                dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Name"] = "PrintPre";
                dtsidefill.Rows[dtsidefill.Rows.Count - 1]["DisplayName"] = "Print Preview";
                dtsidefill.Rows[dtsidefill.Rows.Count - 1]["ShortcutKey"] = "^W";
                if (gvi_id != "0")
                {
                    if (Database.utype == "User")
                    {
                        if (dtVoucherinfo.Rows.Count == 1)
                        {
                            if (DateTime.Parse(dtVoucherinfo.Rows[0]["Vdate"].ToString()).ToString(Database.dformat) == Database.ldate.ToString(Database.dformat))
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
                    else
                    {
                        dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = true;
                    }
                }
                else
                {
                    dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = true;
                }
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
                    flowLayoutPanel2.Controls.Add(btn);
                }
            }
        }
        void btn_Click(object sender, EventArgs e)
        {
            //Button tbtn = (Button)sender;
            //string name = tbtn.Name.ToString();
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
                if (validate() == true)
                {
                    try
                    {
                        Database.BeginTran();

                        if (gresave == false)
                        {

                            if (Database.utype == "Admin")
                            {
                                Save();
                            }
                            else if (gvi_id == "0")
                            {
                                Save();
                            }
                            else if (Database.utype == "User" && gvi_id != "0" && DateTime.Parse(dtVoucherinfo.Rows[0]["Vdate"].ToString()).ToString(Database.dformat) == Database.ldate.ToString(Database.dformat))
                            {
                                Save();
                            }


                        }
                        else
                        {
                            if (Database.utype == "Admin")
                            {
                                DataTable dttemp = new DataTable("Journals");
                                Database.GetSqlData("Select * from Journals where Vi_id='" + gvi_id + "'", dttemp);
                                for (int i = 0; i < dttemp.Rows.Count; i++)
                                {
                                    dttemp.Rows[i].Delete();
                                }
                                Database.SaveData(dttemp);


                                DataTable dtJournal = new DataTable("Journals");
                                Database.GetSqlData("Select * from Journals where Vi_id='" + gvi_id + "' ", dtJournal);
                                string ac_id = "";
                                if (textBox6.Text == "Cash")
                                {
                                    ac_id = Database.LocationCashAcc_id;
                                }
                                else
                                {
                                    ac_id = Database.GetScalarText("Select Ac_id from Accounts where name='" + label7.Text + "'");
                                }

                                dtJournal.Rows.Add();
                                dtJournal.Rows[dtJournal.Rows.Count - 1]["vdate"] = dateTimePicker1.Value.ToString("dd-MMM-yyyy");
                                dtJournal.Rows[dtJournal.Rows.Count - 1]["Vi_id"] = gvi_id;
                                dtJournal.Rows[dtJournal.Rows.Count - 1]["Ac_id"] = ac_id;
                                dtJournal.Rows[dtJournal.Rows.Count - 1]["Opp_acid"] = funs.Select_ac_id("Delivery Charges");
                                dtJournal.Rows[dtJournal.Rows.Count - 1]["Narr"] = textBox5.Text; ;
                                dtJournal.Rows[dtJournal.Rows.Count - 1]["Narr2"] = textBox5.Text; ;
                                dtJournal.Rows[dtJournal.Rows.Count - 1]["Sno"] = 1;
                                dtJournal.Rows[dtJournal.Rows.Count - 1]["LocationId"] = Database.LocationId;
                                dtJournal.Rows[dtJournal.Rows.Count - 1]["Reffno"] = "";
                                dtJournal.Rows[dtJournal.Rows.Count - 1]["Amount"] = double.Parse(textBox3.Text);

                                dtJournal.Rows.Add();
                                dtJournal.Rows[dtJournal.Rows.Count - 1]["vdate"] = dateTimePicker1.Value.ToString("dd-MMM-yyyy");
                                dtJournal.Rows[dtJournal.Rows.Count - 1]["Vi_id"] = gvi_id;
                                dtJournal.Rows[dtJournal.Rows.Count - 1]["Ac_id"] = funs.Select_ac_id("Delivery Charges");
                                dtJournal.Rows[dtJournal.Rows.Count - 1]["Opp_acid"] = ac_id;
                                dtJournal.Rows[dtJournal.Rows.Count - 1]["Narr"] = textBox5.Text; ;
                                dtJournal.Rows[dtJournal.Rows.Count - 1]["Narr2"] = textBox5.Text; ;
                                dtJournal.Rows[dtJournal.Rows.Count - 1]["Sno"] = 1;
                                dtJournal.Rows[dtJournal.Rows.Count - 1]["LocationId"] = Database.LocationId;
                                dtJournal.Rows[dtJournal.Rows.Count - 1]["Reffno"] = "";
                                dtJournal.Rows[dtJournal.Rows.Count - 1]["Amount"] = -1 * double.Parse(textBox3.Text);
                                Database.SaveData(dtJournal);


                            }
                            else if (gvi_id == "0")
                            {
                                Save();
                            }
                            //DataTable dtTemp = new DataTable("Stocks");
                            //Database.GetSqlData("Select * from Stocks where Vid='" + vid + "' ", dtTemp);
                            //for (int j = 0; j < dtTemp.Rows.Count; j++)
                            //{
                            //    dtTemp.Rows[j].Delete();
                            //}
                            //Database.SaveData(dtTemp);

                            //DataTable dtstocks = new DataTable("Stocks");
                            //Database.GetSqlData("select * from Stocks where Vid='" + vid + "'", dtstocks);
                            //dtstocks.Rows.Add();

                            //dtstocks.Rows[dtstocks.Rows.Count - 1]["Vid"] = vid;

                            //dtstocks.Rows[dtstocks.Rows.Count - 1]["GR_id"] = GR_id;
                            //dtstocks.Rows[dtstocks.Rows.Count - 1]["Quantity"] = -1;

                            //dtstocks.Rows[dtstocks.Rows.Count - 1]["Step"] = "Step2";
                            //dtstocks.Rows[dtstocks.Rows.Count - 1]["Godown_id"] = Database.LocationId;
                            //Database.SaveData(dtstocks);

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
                    SendSMS();
                    clear();
                }
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
                                Save();
                            }
                            else if (gvi_id == "0")
                            {
                                Save();
                            }
                            else if (Database.utype == "User" && gvi_id != "0" && DateTime.Parse(dtVoucherinfo.Rows[0]["Vdate"].ToString()).ToString(Database.dformat) == Database.ldate.ToString(Database.dformat))
                            {
                                Save();
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
                            Save();
                        }
                        else if (gvi_id == "0")
                        {
                            Save();
                        }
                        else if (Database.utype == "User" && gvi_id != "0" && DateTime.Parse(dtVoucherinfo.Rows[0]["Vdate"].ToString()).ToString(Database.dformat) == Database.ldate.ToString(Database.dformat))
                        {
                            Save();
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

            else if (name == "Print")
            {
                if (validate() == true)
                {
                    try
                    {
                        Database.BeginTran();

                        if (Database.utype == "Admin")
                        {
                            Save();
                        }
                        else if (gvi_id == "0")
                        {
                            Save();
                        }
                        else if (Database.utype == "User" && gvi_id != "0" && DateTime.Parse(dtVoucherinfo.Rows[0]["Vdate"].ToString()).ToString(Database.dformat) == Database.ldate.ToString(Database.dformat))
                        {
                            Save();
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
                    SendSMS();
                    clear();
                }
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
                        vno = int.Parse(label11.Text);
                    }
                    else
                    {
                        vno = int.Parse(box.outStr);
                    }



                    label11.Text = vno.ToString();
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
                            vno = 0;
                            label11.Text = vno.ToString();
                          //  SetVno();
                            return;
                        }
                    }
                    //f12used = true;
                }
                else
                {
                    MessageBox.Show("Invalid password");
                }
            }
            else if (name == "PrintPre")
            {
                if (validate() == true)
                {
                    try
                    {
                        Database.BeginTran();

                        if (Database.utype == "Admin")
                        {
                            Save();
                        }
                        else if (gvi_id == "0")
                        {
                            Save();
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
                        view();
                    }
                    clear();
                }
            }
            else if (name == "quit")
            {
                this.Close();
                this.Dispose();
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

        private void textBox15_KeyPress(object sender, KeyPressEventArgs e)
        {
            string cmbAcc = "";
            textBox15.Text = SelectCombo.ComboKeypress(this, e.KeyChar, cmbAcc, e.KeyChar.ToString(), 0);
            if (textBox15.Text == "")
            {
                return;
            }
            vtid = funs.Select_vt_id(textBox15.Text);
        }

        private void Displaysetting()
        {
            DataTable dtvt = new DataTable();
            string  cmbVouTyp = "select [name] from vouchertypes where active=" + access_sql.Singlequote + "true" + access_sql.Singlequote + "  and type='Delivery'";
            Database.GetSqlData(cmbVouTyp, dtvt);

            if (dtvt.Rows.Count == 1)
            {
                textBox15.Text = dtvt.Rows[0]["name"].ToString();
                vtid = funs.Select_vt_id(textBox15.Text);
               
                textBox15.Enabled = false;

       
                if (textBox15.Text == "")
                {
                    return;
                }
                vtid = funs.Select_vt_id(textBox15.Text);
       
            }
            else
            {
                textBox15.Enabled = true;
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {

            //string cmbVouTyp = "SELECT dbo.VOUCHERINFOs.Vi_id, CAST(VOUCHERINFOs.Invoiceno as varchar(10)) AS [GR No],  CONVERT(nvarchar, dbo.VOUCHERINFOs.Vdate, 106) AS Vdate, ACCOUNTs_1.name AS Consigner,  ACCOUNTs.name AS Consignee, CAST(SUM(Voucherdets.Quantity) as nvarchar(10)) AS Qty,  CAST(SUM(Voucherdets.weight) as nvarchar(10)) AS Weight, CAST(Voucherinfos.TotalAmount as nvarchar(10)) AS Amount,  VOUCHERINFOs.PaymentMode FROM          Voucherdets RIGHT OUTER JOIN VOUCHERINFOs ON  Voucherdets.Vi_id =  VOUCHERINFOs.Vi_id LEFT OUTER JOIN   ACCOUNTs ON  VOUCHERINFOs.Ac_id2 =  ACCOUNTs.ac_id RIGHT OUTER JOIN   Stocks ON  VOUCHERINFOs.Vi_id =  Stocks.GR_id LEFT OUTER JOIN ACCOUNTs AS ACCOUNTs_1 ON  VOUCHERINFOs.Ac_id = ACCOUNTs_1.ac_id WHERE     ( Stocks.Step = 'Step2') AND ( Stocks.Godown_id = '" + Database.LocationId + "') AND (dbo.VOUCHERINFOs.Iscancel = 0) GROUP BY  dbo.VOUCHERINFOs.Vi_id,VOUCHERINFOs.Invoiceno, Voucherinfos.totalamount, VOUCHERINFOs.Vdate, ACCOUNTs_1.name,  ACCOUNTs.name,  VOUCHERINFOs.PaymentMode HAVING      (SUM(dbo.Stocks.Quantity) > 0)";
            string cmbVouTyp = "SELECT    Stocks.Gr_id as Vi_id, Stocks.GRNo, CONVERT(nvarchar, Stocks.GRDate, 106) AS vdate, cast(Stocks.TotPkts as nvarchar(255)) AS Quantity, cast(Stocks.Actweight as nvarchar(255)) AS ActWeight, cast(Stocks.Totweight as nvarchar(255)) AS Weight, Stocks.GRType,   ACCOUNTs.name AS Consigner, ACCOUNTs_1.name AS Consignee, Stocks.Source_id, Stocks.Destination_id, Stocks.Private, Stocks.Remark FROM Stocks LEFT OUTER JOIN  ACCOUNTs AS ACCOUNTs_1 ON Stocks.Consignee_id = ACCOUNTs_1.ac_id LEFT OUTER JOIN  ACCOUNTs ON Stocks.Consigner_id = ACCOUNTs.ac_id LEFT OUTER JOIN  VOUCHERINFOs ON Stocks.vid = VOUCHERINFOs.Vi_id WHERE ( Stocks.Godown_id = '" + Database.LocationId + "') AND ( VOUCHERINFOs.Iscancel = 0) AND ( Stocks.Step = 'Step2') GROUP BY Stocks.GRNo, Stocks.GRDate, Stocks.TotPkts, Stocks.TotWeight, Stocks.ActWeight, Stocks.GRType, ACCOUNTs.name, ACCOUNTs_1.name,   Stocks.Source_id, Stocks.Destination_id, Stocks.Private, Stocks.Remark, Stocks.Gr_id HAVING (SUM( Stocks.Quantity) > 0)";
            string vid = SelectCombo.ComboKeypress(this, e.KeyChar, cmbVouTyp, textBox1.Text, 8);
            GR_id = vid;
            DataTable dtiteminfo = new DataTable();
            ansGridView1.Rows.Clear();

            textBox1.Text = Database.GetScalarText("Select GRno from stocks where GR_id='" + vid + "'");
            DataTable dt = new DataTable();

            if (GR_id.Contains("/") == true)
            {
                Database.GetSqlData("SELECT Stocks.GR_id, Stocks.ItemName AS description, Stocks.Packing, Stocks.TotPkts AS Quantity,Stocks.ActWeight AS ActWeight, Stocks.TotWeight AS Weight,   Stocks.Freight AS Rate_am, VOUCHERINFOs.Totalamount AS Amount FROM VOUCHERTYPEs RIGHT OUTER JOIN  VOUCHERINFOs ON VOUCHERTYPEs.Vt_id = VOUCHERINFOs.Vt_id RIGHT OUTER JOIN  Stocks ON VOUCHERINFOs.Vi_id = Stocks.vid WHERE ( Stocks.GR_id = '" + vid + "') AND ( VOUCHERTYPEs.Type = 'GRByChallan')", dtiteminfo);

                cmbVouTyp = "SELECT Stocks.ItemName AS description, Stocks.Packing, Stocks.TotPkts AS Quantity, Stocks.ActWeight AS ActWeight,Stocks.TotWeight AS Weight, Stocks.Freight AS Rate_am, Stocks.Grno, dbo.Stocks.ToPay + dbo.Stocks.TBB + dbo.Stocks.Paid + dbo.Stocks.FOC AS Amount, ACCOUNTs_1.name AS Consigner, ACCOUNTs.name AS Consignee, Stocks.Private,Stocks.Remark,Stocks.grcharge,Stocks.Othcharge, Stocks.deliverytype,Stocks.GRDate AS Vdate,   Stocks.Source_id, Stocks.Destination_id, Stocks.GRType FROM ACCOUNTs RIGHT OUTER JOIN  Stocks ON ACCOUNTs.ac_id = Stocks.Consignee_id LEFT OUTER JOIN  ACCOUNTs AS ACCOUNTs_1 ON Stocks.Consigner_id = ACCOUNTs_1.ac_id LEFT OUTER JOIN  VOUCHERTYPEs RIGHT OUTER JOIN  VOUCHERINFOs ON VOUCHERTYPEs.Vt_id = VOUCHERINFOs.Vt_id ON Stocks.vid = VOUCHERINFOs.Vi_id WHERE ( Stocks.GR_id = '" + GR_id + "') AND ( VOUCHERTYPEs.Type = 'GRByChallan')";
                Database.GetSqlData(cmbVouTyp, dt);
              
                for (int i = 0; i < dtiteminfo.Rows.Count; i++)
                {
                    ansGridView1.Rows.Add();
                    ansGridView1.Rows[i].Cells["Sno"].Value =1;
                    ansGridView1.Rows[i].Cells["Description"].Value = dtiteminfo.Rows[i]["description"].ToString();
                    ansGridView1.Rows[i].Cells["unt"].Value = dtiteminfo.Rows[i]["packing"].ToString();
                    ansGridView1.Rows[i].Cells["Quantity"].Value = dtiteminfo.Rows[i]["Quantity"].ToString();

                    ansGridView1.Rows[i].Cells["multiplier"].Value = 1;
                    ansGridView1.Rows[i].Cells["weight"].Value = dtiteminfo.Rows[i]["Actweight"].ToString();
                    ansGridView1.Rows[i].Cells["Chargedweight"].Value = dtiteminfo.Rows[i]["weight"].ToString();
                    ansGridView1.Rows[i].Cells["per"].Value = "Flat";
                    ansGridView1.Rows[i].Cells["Rate_am"].Value = dtiteminfo.Rows[i]["Rate_am"].ToString();
                    ansGridView1.Rows[i].Cells["Amount"].Value = dtiteminfo.Rows[i]["Amount"].ToString();



                    ansGridView1.Columns["Quantity"].ReadOnly = true;
                    ansGridView1.Columns["multiplier"].ReadOnly = true;
                    ansGridView1.Columns["weight"].ReadOnly = true;
                    ansGridView1.Columns["Chargedweight"].ReadOnly = true;


                }
            }
            else
            {
                Database.GetSqlData("SELECT Voucherdets.Itemsr,Voucherdets.Des_Ac_id, Voucherdets.packing,Voucherdets.Quantity, dbo.Voucherdets.multiplier, Voucherdets.weight, Voucherdets.ChargedWeight, dbo.Voucherdets.Per,  Voucherdets.Rate_am, dbo.Voucherdets.Amount AS Amount FROM         dbo.items RIGHT OUTER JOIN   dbo.Voucherdets ON dbo.items.Id = dbo.Voucherdets.Des_ac_id RIGHT OUTER JOIN   dbo.VOUCHERINFOs ON dbo.Voucherdets.Vi_id = dbo.VOUCHERINFOs.Vi_id LEFT OUTER JOIN VOUCHERTYPEs ON dbo.VOUCHERINFOs.Vt_id = dbo.VOUCHERTYPEs.Vt_id WHERE     (dbo.VOUCHERTYPEs.Type = 'Booking') AND (dbo.VOUCHERINFOs.Vi_id = '" + vid + "') ORDER BY dbo.Voucherdets.Itemsr", dtiteminfo);
                cmbVouTyp = "SELECT VOUCHERINFOs.Totalamount AS Amount, Stocks.TotPkts AS Quantity,Stocks.ActWeight AS ActWeight, Stocks.TotWeight AS Weight,  ACCOUNTs_1.name AS Consigner, ACCOUNTs.name AS Consignee,Stocks.Grno, Stocks.Source_id, Stocks.Destination_id, Stocks.GRType,  Stocks.GRDate AS vdate, Stocks.Freight as Rate_am, Stocks.Private,Stocks.Remark,Stocks.grcharge,Stocks.Othcharge, Stocks.deliverytype FROM ACCOUNTs RIGHT OUTER JOIN  VOUCHERINFOs ON ACCOUNTs.ac_id = VOUCHERINFOs.Ac_id2 LEFT OUTER JOIN  Stocks ON VOUCHERINFOs.Vi_id = Stocks.vid LEFT OUTER JOIN  VOUCHERTYPEs ON VOUCHERINFOs.Vt_id = VOUCHERTYPEs.Vt_id LEFT OUTER JOIN  ACCOUNTs AS ACCOUNTs_1 ON VOUCHERINFOs.Ac_id = ACCOUNTs_1.ac_id WHERE ( VOUCHERTYPEs.Type = 'Booking') AND ( VOUCHERINFOs.Vi_id = '" + vid + "')";
                Database.GetSqlData(cmbVouTyp, dt);


                for (int i = 0; i < dtiteminfo.Rows.Count; i++)
                {
                    ansGridView1.Rows.Add();
                    ansGridView1.Rows[i].Cells["Sno"].Value = dtiteminfo.Rows[i]["Itemsr"].ToString();
                    ansGridView1.Rows[i].Cells["Description"].Value = funs.Select_item_nm(dtiteminfo.Rows[i]["Des_Ac_id"].ToString());
                    ansGridView1.Rows[i].Cells["unt"].Value = dtiteminfo.Rows[i]["packing"].ToString();
                    ansGridView1.Rows[i].Cells["Quantity"].Value = dtiteminfo.Rows[i]["Quantity"].ToString();

                    ansGridView1.Rows[i].Cells["multiplier"].Value = dtiteminfo.Rows[i]["multiplier"].ToString();
                    ansGridView1.Rows[i].Cells["weight"].Value = dtiteminfo.Rows[i]["weight"].ToString();
                    ansGridView1.Rows[i].Cells["Chargedweight"].Value = dtiteminfo.Rows[i]["Chargedweight"].ToString();
                    ansGridView1.Rows[i].Cells["per"].Value = dtiteminfo.Rows[i]["per"].ToString();
                    ansGridView1.Rows[i].Cells["Rate_am"].Value = dtiteminfo.Rows[i]["Rate_am"].ToString();
                    ansGridView1.Rows[i].Cells["Amount"].Value = dtiteminfo.Rows[i]["Amount"].ToString();



                    ansGridView1.Columns["Quantity"].ReadOnly = true;
                    ansGridView1.Columns["multiplier"].ReadOnly = true;
                    ansGridView1.Columns["weight"].ReadOnly = true;
                    ansGridView1.Columns["Chargedweight"].ReadOnly = true;


                }
            }
          
          

          
       

            for (int i = 0; i < ansGridView1.Rows.Count; i++)
            {
               
              
                ansGridView1.Rows[i].Cells["exp1rate"].Value = 0;
                ansGridView1.Rows[i].Cells["exp1amt"].Value = 0;
                ansGridView1.Rows[i].Cells["exp1type"].Value ="Flat";
                ansGridView1.Rows[i].Cells["exp1mr"].Value = 4.00;

                ansGridView1.Rows[i].Cells["exp2rate"].Value = 0;
                ansGridView1.Rows[i].Cells["exp2amt"].Value = 0;
                ansGridView1.Rows[i].Cells["exp2type"].Value = "Flat";
                ansGridView1.Rows[i].Cells["exp2mr"].Value = 4.00;

                ansGridView1.Rows[i].Cells["exp3rate"].Value = 0;
                ansGridView1.Rows[i].Cells["exp3amt"].Value = 0;
                ansGridView1.Rows[i].Cells["exp3type"].Value = "Flat";
                ansGridView1.Rows[i].Cells["exp3mr"].Value = 4.00;

                ansGridView1.Rows[i].Cells["exp4rate"].Value = 0;
                ansGridView1.Rows[i].Cells["exp4amt"].Value = 0;
                ansGridView1.Rows[i].Cells["exp4type"].Value = "Flat";
                ansGridView1.Rows[i].Cells["exp4mr"].Value = 5.00;
              
            }



            double amt = 0, weight = 0, qty = 0;
            if (dt.Rows.Count != 0)
            {
                amt = double.Parse(dt.Rows[0]["amount"].ToString());
                textBox2.Text = funs.DecimalPoint(amt, 2);
                label6.Text = dt.Rows[0]["Consigner"].ToString();
                label7.Text = dt.Rows[0]["Consignee"].ToString();
                label8.Text = funs.DecimalPoint(dt.Rows[0]["Quantity"].ToString(), 3);
                label9.Text = funs.DecimalPoint(dt.Rows[0]["Weight"].ToString(), 3);

                label22.Text = funs.DecimalPoint(dt.Rows[0]["grcharge"].ToString(), 2);
                label24.Text = funs.DecimalPoint(dt.Rows[0]["othcharge"].ToString(), 2);
                label32.Text = funs.DecimalPoint(dt.Rows[0]["rate_am"].ToString(), 2);
                label34.Text = funs.DecimalPoint(dt.Rows[0]["Actweight"].ToString(), 2);
                label31.Text = dt.Rows[0]["Private"].ToString();
                label29.Text = dt.Rows[0]["remark"].ToString();
                label15.Text = DateTime.Parse(dt.Rows[0]["Vdate"].ToString()).ToString(Database.dformat);
                label16.Text = dt.Rows[0]["GRType"].ToString();
                label26.Text = dt.Rows[0]["deliverytype"].ToString();

                label13.Text = funs.Select_dp_nm(dt.Rows[0]["Source_id"].ToString());
                label20.Text = funs.Select_dp_nm(dt.Rows[0]["destination_id"].ToString());
            }
            else
            {
                textBox2.Text = "0.00";
                textBox5.Text = "";
                label6.Text = "";
                label7.Text = "";
                label13.Text = "";
                label34.Text = "";
                label20.Text = "";
                label8.Text = "";
                label9.Text = "";
                label12.Text = "";
                label15.Text = "";
                ansGridView1.Rows.Clear();
                textBox3.Text = "0.00";

            }
            for (int i = 0; i < ansGridView1.Rows.Count; i++)
            {
                calc(i);
            }
        }

        private void ansGridView4_KeyPress(object sender, KeyPressEventArgs e)
        {
           

        }

        private void SendSMS()
        {

            if (Feature.Available("Send Sms") == "Yes")
            {
                string msg = "Dear Sir Your Consignment no. " + textBox1.Text + " Booked on " + label15.Text + " from " + label6.Text + " PKGS." + funs.DecimalPoint(double.Parse(label8.Text), 0) + " INV.NO. " + label12.Text + " is taking out for delivery will be reached you shortly.";

                if (gvi_id != "0")
                {
                    DialogResult ch = MessageBox.Show(null, "Are you want to send SMS?", "Confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                    if (ch == DialogResult.OK)
                    {
                       
                        if (funs.isDouble(funs.Select_SMSMobile(label7.Text)) == true)
                        {
                            if (funs.Select_SMSMobile(label7.Text) != "0")
                            {
                                sms objsms = new sms();
                                objsms.send(msg, funs.Select_SMSMobile(label7.Text), label7.Text);
                               
                            }
                        }
                    }
                }
                else
                {
                  
                    if (funs.isDouble(funs.Select_SMSMobile(label7.Text)) == true)
                    {
                        if (funs.Select_SMSMobile(label7.Text) != "0")
                        {
                            sms objsms = new sms();
                            objsms.send(msg, funs.Select_SMSMobile(label7.Text), label7.Text);
                            // MessageBox.Show(msg);
                        }
                    }
                }

            }

        }
        private void calc(int rowindex)
        {
            
         

            double weight=0,qty=0;   
            qty = double.Parse(ansGridView1.Rows[rowindex].Cells["quantity"].Value.ToString());
                weight = double.Parse(ansGridView1.Rows[rowindex].Cells["weight"].Value.ToString());

                if (ansGridView1.Rows[rowindex].Cells["exp1rate"].Value == null || ansGridView1.Rows[rowindex].Cells["exp1rate"].Value.ToString() == "")
                {
                    ansGridView1.Rows[rowindex].Cells["exp1rate"].Value = 0;
                }
                if (ansGridView1.Rows[rowindex].Cells["exp2rate"].Value == null || ansGridView1.Rows[rowindex].Cells["exp2rate"].Value.ToString() == "")
                {
                    ansGridView1.Rows[rowindex].Cells["exp2rate"].Value = 0;
                }
                if (ansGridView1.Rows[rowindex].Cells["exp3rate"].Value == null || ansGridView1.Rows[rowindex].Cells["exp3rate"].Value.ToString() == "")
                {
                    ansGridView1.Rows[rowindex].Cells["exp3rate"].Value = 0;
                }
                if (ansGridView1.Rows[rowindex].Cells["exp4rate"].Value == null || ansGridView1.Rows[rowindex].Cells["exp4rate"].Value.ToString() == "")
                {
                    ansGridView1.Rows[rowindex].Cells["exp4rate"].Value = 0;
                }

          //  textBox2.Text = funs.DecimalPoint(amt, 2);

                if (ansGridView1.Rows[rowindex].Cells["exp1type"].Value.ToString() == "/Weight" && double.Parse(ansGridView1.Rows[rowindex].Cells["exp1mr"].Value.ToString()) == 4.00)
            {
                    
                ansGridView1.Rows[rowindex].Cells["exp1amt"].Value = funs.DecimalPoint((weight * double.Parse(ansGridView1.Rows[rowindex].Cells["exp1rate"].Value.ToString())), 2);
    
            }
            else if (ansGridView1.Rows[rowindex].Cells["exp1type"].Value.ToString() == "/Nug" && double.Parse(ansGridView1.Rows[rowindex].Cells["exp1mr"].Value.ToString()) == 4.00)
            {
          
                ansGridView1.Rows[rowindex].Cells["exp1amt"].Value = funs.DecimalPoint((qty * double.Parse(ansGridView1.Rows[rowindex].Cells["exp1rate"].Value.ToString())), 2);

             
            }
                else if (ansGridView1.Rows[rowindex].Cells["exp1type"].Value.ToString() == "Flat" && double.Parse(ansGridView1.Rows[rowindex].Cells["exp1mr"].Value.ToString()) == 4.00)
            {
            
                ansGridView1.Rows[rowindex].Cells["exp1amt"].Value = funs.DecimalPoint((double.Parse(ansGridView1.Rows[rowindex].Cells["exp1rate"].Value.ToString())), 2);

            
            }
                else if (ansGridView1.Rows[rowindex].Cells["exp1type"].Value.ToString() == "/Weight" && double.Parse(ansGridView1.Rows[rowindex].Cells["exp1mr"].Value.ToString()) == 5.00)
            {
             
                ansGridView1.Rows[rowindex].Cells["exp1amt"].Value = funs.DecimalPoint(-1 * (weight * double.Parse(ansGridView1.Rows[rowindex].Cells["exp1rate"].Value.ToString())), 2);

            
             }
                else if (ansGridView1.Rows[rowindex].Cells["exp1type"].Value.ToString() == "/Nug" && double.Parse(ansGridView1.Rows[rowindex].Cells["exp1mr"].Value.ToString()) == 5.00)
            {
            
                ansGridView1.Rows[rowindex].Cells["exp1amt"].Value = funs.DecimalPoint(-1 * (qty * double.Parse(ansGridView1.Rows[rowindex].Cells["exp1rate"].Value.ToString())), 2);

            }
                else if (ansGridView1.Rows[rowindex].Cells["exp1type"].Value.ToString() == "Flat" && double.Parse(ansGridView1.Rows[rowindex].Cells["exp1mr"].Value.ToString()) == 5.00)
            {
             
                ansGridView1.Rows[rowindex].Cells["exp1amt"].Value = funs.DecimalPoint((-1 * double.Parse(ansGridView1.Rows[rowindex].Cells["exp1rate"].Value.ToString())), 2);
            }




                if (ansGridView1.Rows[rowindex].Cells["exp2type"].Value.ToString() == "/Weight" && double.Parse(ansGridView1.Rows[rowindex].Cells["exp2mr"].Value.ToString()) == 4.00)
            {

                ansGridView1.Rows[rowindex].Cells["exp2amt"].Value = funs.DecimalPoint((weight * double.Parse(ansGridView1.Rows[rowindex].Cells["exp2rate"].Value.ToString())), 2);

            }
            else if (ansGridView1.Rows[rowindex].Cells["exp2type"].Value.ToString() == "/Nug" && double.Parse(ansGridView1.Rows[rowindex].Cells["exp2mr"].Value.ToString()) == 4.00)
            {

                ansGridView1.Rows[rowindex].Cells["exp2amt"].Value = funs.DecimalPoint((qty * double.Parse(ansGridView1.Rows[rowindex].Cells["exp2rate"].Value.ToString())), 2);


            }
            else if (ansGridView1.Rows[rowindex].Cells["exp2type"].Value.ToString() == "Flat" && double.Parse(ansGridView1.Rows[rowindex].Cells["exp2mr"].Value.ToString()) == 4.00)
            {

                ansGridView1.Rows[rowindex].Cells["exp2amt"].Value = funs.DecimalPoint((double.Parse(ansGridView1.Rows[rowindex].Cells["exp2rate"].Value.ToString())), 2);


            }
                else if (ansGridView1.Rows[rowindex].Cells["exp2type"].Value.ToString() == "/Weight" && double.Parse(ansGridView1.Rows[rowindex].Cells["exp2mr"].Value.ToString()) == 5.00)
            {

                ansGridView1.Rows[rowindex].Cells["exp2amt"].Value = funs.DecimalPoint(-1 * (weight * double.Parse(ansGridView1.Rows[rowindex].Cells["exp2rate"].Value.ToString())), 2);


            }
                else if (ansGridView1.Rows[rowindex].Cells["exp2type"].Value.ToString() == "/Nug" && double.Parse(ansGridView1.Rows[rowindex].Cells["exp2mr"].Value.ToString()) == 5.00)
            {

                ansGridView1.Rows[rowindex].Cells["exp2amt"].Value = funs.DecimalPoint(-1 * (qty * double.Parse(ansGridView1.Rows[rowindex].Cells["exp2rate"].Value.ToString())), 2);

            }
                else if (ansGridView1.Rows[rowindex].Cells["exp2type"].Value.ToString() == "Flat" && double.Parse(ansGridView1.Rows[rowindex].Cells["exp2mr"].Value.ToString()) == 5.00)
            {

                ansGridView1.Rows[rowindex].Cells["exp2amt"].Value = funs.DecimalPoint((-1 * double.Parse(ansGridView1.Rows[rowindex].Cells["exp2rate"].Value.ToString())), 2);
            }



                if (ansGridView1.Rows[rowindex].Cells["exp3type"].Value.ToString() == "/Weight" && double.Parse(ansGridView1.Rows[rowindex].Cells["exp3mr"].Value.ToString()) == 4.00)
            {

                ansGridView1.Rows[rowindex].Cells["exp3amt"].Value = funs.DecimalPoint((weight * double.Parse(ansGridView1.Rows[rowindex].Cells["exp3rate"].Value.ToString())), 2);

            }
                else if (ansGridView1.Rows[rowindex].Cells["exp3type"].Value.ToString() == "/Nug" && double.Parse(ansGridView1.Rows[rowindex].Cells["exp3mr"].Value.ToString()) == 4.00)
            {

                ansGridView1.Rows[rowindex].Cells["exp3amt"].Value = funs.DecimalPoint((qty * double.Parse(ansGridView1.Rows[rowindex].Cells["exp3rate"].Value.ToString())), 2);


            }
                else if (ansGridView1.Rows[rowindex].Cells["exp3type"].Value.ToString() == "Flat" && double.Parse(ansGridView1.Rows[rowindex].Cells["exp3mr"].Value.ToString()) == 4.00)
            {

                ansGridView1.Rows[rowindex].Cells["exp3amt"].Value = funs.DecimalPoint((double.Parse(ansGridView1.Rows[rowindex].Cells["exp3rate"].Value.ToString())), 2);


            }
                else if (ansGridView1.Rows[rowindex].Cells["exp3type"].Value.ToString() == "/Weight" && double.Parse(ansGridView1.Rows[rowindex].Cells["exp3mr"].Value.ToString()) == 5.00)
            {

                ansGridView1.Rows[rowindex].Cells["exp3amt"].Value = funs.DecimalPoint(-1 * (weight * double.Parse(ansGridView1.Rows[rowindex].Cells["exp3rate"].Value.ToString())), 2);


            }
                else if (ansGridView1.Rows[rowindex].Cells["exp3type"].Value.ToString() == "/Nug" && double.Parse(ansGridView1.Rows[rowindex].Cells["exp3mr"].Value.ToString()) == 5.00)
            {

                ansGridView1.Rows[rowindex].Cells["exp3amt"].Value = funs.DecimalPoint(-1 * (qty * double.Parse(ansGridView1.Rows[rowindex].Cells["exp3rate"].Value.ToString())), 2);

            }
                else if (ansGridView1.Rows[rowindex].Cells["exp3type"].Value.ToString() == "Flat" && double.Parse(ansGridView1.Rows[rowindex].Cells["exp3mr"].Value.ToString()) == 5.00)
            {

                ansGridView1.Rows[rowindex].Cells["exp3amt"].Value = funs.DecimalPoint((-1 * double.Parse(ansGridView1.Rows[rowindex].Cells["exp3rate"].Value.ToString())), 2);
            }



                if (ansGridView1.Rows[rowindex].Cells["exp4type"].Value.ToString() == "/Weight" && double.Parse(ansGridView1.Rows[rowindex].Cells["exp4mr"].Value.ToString()) == 4.00)
            {

                ansGridView1.Rows[rowindex].Cells["exp4amt"].Value = funs.DecimalPoint((weight * double.Parse(ansGridView1.Rows[rowindex].Cells["exp4rate"].Value.ToString())), 2);

            }
                else if (ansGridView1.Rows[rowindex].Cells["exp4type"].Value.ToString() == "/Nug" && double.Parse(ansGridView1.Rows[rowindex].Cells["exp4mr"].Value.ToString()) == 4.00)
            {

                ansGridView1.Rows[rowindex].Cells["exp4amt"].Value = funs.DecimalPoint((qty * double.Parse(ansGridView1.Rows[rowindex].Cells["exp4rate"].Value.ToString())), 2);


            }
            else if (ansGridView1.Rows[rowindex].Cells["exp4type"].Value.ToString() == "Flat" && double.Parse(ansGridView1.Rows[rowindex].Cells["exp4mr"].Value.ToString()) == 4.00)
            {

                ansGridView1.Rows[rowindex].Cells["exp1amt"].Value = funs.DecimalPoint((double.Parse(ansGridView1.Rows[rowindex].Cells["exp4rate"].Value.ToString())), 2);


            }
                else if (ansGridView1.Rows[rowindex].Cells["exp4type"].Value.ToString() == "/Weight" && double.Parse(ansGridView1.Rows[rowindex].Cells["exp4mr"].Value.ToString()) == 5.00)
            {

                ansGridView1.Rows[rowindex].Cells["exp4amt"].Value = funs.DecimalPoint(-1 * (weight * double.Parse(ansGridView1.Rows[rowindex].Cells["exp4rate"].Value.ToString())), 2);


            }
                else if (ansGridView1.Rows[rowindex].Cells["exp4type"].Value.ToString() == "/Nug" && double.Parse(ansGridView1.Rows[rowindex].Cells["exp4mr"].Value.ToString()) == 5.00)
            {

                ansGridView1.Rows[rowindex].Cells["exp4amt"].Value = funs.DecimalPoint(-1 * (qty * double.Parse(ansGridView1.Rows[rowindex].Cells["exp4rate"].Value.ToString())), 2);

            }
                else if (ansGridView1.Rows[rowindex].Cells["exp4type"].Value.ToString() == "Flat" && double.Parse(ansGridView1.Rows[rowindex].Cells["exp4mr"].Value.ToString()) == 5.00)
            {

                ansGridView1.Rows[rowindex].Cells["exp4amt"].Value = funs.DecimalPoint((-1 * double.Parse(ansGridView1.Rows[rowindex].Cells["exp4rate"].Value.ToString())), 2);
            }
                double exp1 = 0, exp2 = 0, exp3 = 0, exp4 = 0;
             chamt = 0;
             if (label16.Text == "To Pay")
             {
                 chamt = double.Parse(textBox2.Text);
             }
             for (int i = 0; i < ansGridView1.Rows.Count; i++)
             {
                 exp1 += double.Parse(ansGridView1.Rows[i].Cells["exp1amt"].Value.ToString());
                 exp2 += double.Parse(ansGridView1.Rows[i].Cells["exp2amt"].Value.ToString());
                 exp3 += double.Parse(ansGridView1.Rows[i].Cells["exp3amt"].Value.ToString());
                 exp4 += double.Parse(ansGridView1.Rows[i].Cells["exp4amt"].Value.ToString());
                 chamt += double.Parse(ansGridView1.Rows[i].Cells["exp1amt"].Value.ToString());
                 chamt += double.Parse(ansGridView1.Rows[i].Cells["exp2amt"].Value.ToString());
                 chamt += double.Parse(ansGridView1.Rows[i].Cells["exp3amt"].Value.ToString());
                 chamt += double.Parse(ansGridView1.Rows[i].Cells["exp4amt"].Value.ToString());
             }

             ansGridView4.Rows[0].Cells["CAmountB"].Value = exp1;
             ansGridView4.Rows[1].Cells["CAmountB"].Value = exp2;
             ansGridView4.Rows[2].Cells["CAmountB"].Value = exp3;
             ansGridView4.Rows[3].Cells["CAmountB"].Value = exp4;
             textBox3.Text = funs.DecimalPoint(chamt, 2); 
        }

        private void ansGridView4_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
           
        }

        private void ansGridView4_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            ansGridView4.CurrentCell.Value = 0;
        }

        private void ansGridView4_KeyDown(object sender, KeyEventArgs e)
        {
            
        }

        private void ansGridView4_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void frm_Delivery_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.P)
            {
                if (validate() == true)
                {
                    try
                    {
                        Database.BeginTran();

                        if (Database.utype == "Admin")
                        {
                            Save();
                        }

                        else if (gvi_id == "0")
                        {
                            Save();
                        }
                        else if (Database.utype == "User" && gvi_id != "0" && DateTime.Parse(dtVoucherinfo.Rows[0]["Vdate"].ToString()).ToString(Database.dformat) == Database.ldate.ToString(Database.dformat))
                        {
                            Save();
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
                    SendSMS();
                    clear();
                }
            }
            if (e.Control && e.KeyCode == Keys.S)
            {
                if (validate() == true)
                {
                    try
                    {
                        Database.BeginTran();

                        if (Database.utype == "Admin")
                        {
                            Save();
                        }

                        else if (gvi_id == "0")
                        {
                            Save();
                        }
                        else if (Database.utype == "User" && gvi_id != "0" && DateTime.Parse(dtVoucherinfo.Rows[0]["Vdate"].ToString()).ToString(Database.dformat) == Database.ldate.ToString(Database.dformat))
                        {
                            Save();
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
                    SendSMS();
                    clear();
                }
            }
            if (e.Control && e.KeyCode == Keys.F12)
            {
                if (Database.utype == "Admin")
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
                            vno = int.Parse(label11.Text);
                        }
                        else
                        {
                            vno = int.Parse(box.outStr);
                        }

                        label11.Text = vno.ToString();
                        int numtype = funs.chkNumType(vtid);
                        if (numtype != 1)
                        {
                            vid = Database.GetScalarText("Select Vi_id from voucherinfos where Vt_id=" + vtid + " and Vnumber=" + vno + " and Vdate=" + access_sql.Hash + dateTimePicker1.Value.Date.ToString(Database.dformat) + access_sql.Hash);
                            if (vid == "")
                            {
                                vid = "0";
                            }
                        }
                        else
                        {
                            string tempvid = "";
                            tempvid = Database.GetScalarText("Select Vi_id from voucherinfos where Vt_id=" + vtid + " and Vnumber=" + vno);
                            if (tempvid != "")
                            {
                                MessageBox.Show("Voucher can't be created on this No.");
                                vno = 0;
                                label11.Text = vno.ToString();
                                //SetVno();
                                return;
                            }
                        }
                       // f12used = true;
                    }
                    else
                    {
                        MessageBox.Show("Invalid password");
                    }
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

        private void textBox1_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox1);
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox1);
        }

        private void textBox4_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C)
            {
                textBox4.Text = funs.AddDeliveredby();
            }
            else if (e.Control && e.KeyCode == Keys.A)
            {
                textBox4.Text = funs.EditDeliveredBy(textBox4.Text);
            }
          //  SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox4_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox4);
        }

        private void textBox4_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox4);
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            string cmbVouTyp = "SELECT name from DeliveredBys where locationid='"+ Database.LocationId+"' order by Name";
            textBox4.Text = SelectCombo.ComboKeypress(this, e.KeyChar, cmbVouTyp, textBox4.Text, 0);
        }

        private void textBox5_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox5);
        }

        private void textBox5_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox5);
        }

        private void textBox5_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox6_KeyDown(object sender, KeyEventArgs e)
        {
            DataTable dtcombo = new DataTable();
            dtcombo.Columns.Add("Mode of Payment", typeof(string));

            dtcombo.Columns["Mode of Payment"].ColumnName = "Mode of Payment";

            dtcombo.Rows.Add();
            dtcombo.Rows[0][0] = "Cash";

            dtcombo.Rows.Add();
            dtcombo.Rows[1][0] = "Credit";


            textBox6.Text = SelectCombo.ComboDt(this, dtcombo, 0);
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

        private void ansGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            //if (ansGridView1.CurrentCell.OwningColumn.Name == "exp1rate")
            //{
                calc(e.RowIndex);
            //}
        }

        private void ansGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
           
            ansGridView1.CurrentCell.Value = 0;
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            SetVno();
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }


    }
}
