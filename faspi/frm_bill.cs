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
    public partial class frm_bill : Form
    {
        int vtid;
        int vno = 0;
        string gStr = "";
        string vid = "";
        DataTable dtVoucherInfo;
        DataTable dtJournal;
        Boolean RoffChanged = false;
        string Prelocationid = "";
        string strCombo = "";
        bool iscancel = false;

        DateTime create_date = DateTime.Parse(System.DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"));

        public frm_bill()
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

        private void frm_bill_Load(object sender, EventArgs e)
        {
            SideFill();
          
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
                if (vid!="0")
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
                if (vid!="0")
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
            if ((Prelocationid == Database.LocationId) || (Prelocationid == "" && vid == "0"))
            {
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
        }

        private void weightCalc()
        {
            double TotalWeight = 0;
            double Totalqty = 0;
            double Totalamt = 0;
            int gr = 0;

            for (int i = 0; i < ansGridView1.Rows.Count; i++)
            {
                if (bool.Parse(ansGridView1.Rows[i].Cells["select"].Value.ToString()) == true)
                {
                    gr++;
                    if (ansGridView1.Rows[i].Cells["wt1"].Value.ToString() == "")
                    {
                        ansGridView1.Rows[i].Cells["wt1"].Value = 0;
                    }


                    TotalWeight += double.Parse(ansGridView1.Rows[i].Cells["wt1"].Value.ToString());
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
            //textBox1.Text = ansGridView1.Rows.Count.ToString();
            textBox7.Text = Totalqty.ToString();
            textBox8.Text = Totalamt.ToString();
            txtTotalWeight.Text = TotalWeight.ToString();
        }

        public void LoadData(string vi_id, String frmCaption)
        {
            gStr = vi_id.ToString();
            vid = vi_id;
            vtid = funs.Select_vt_id("Bill");
            if (vi_id == "0")
            {
                SetVno();
            }

            dtVoucherInfo = new DataTable("Voucherinfos");
            Database.GetSqlData("select * from Voucherinfos where Vi_id='" + vi_id + "'", dtVoucherInfo);
            dtJournal = new DataTable("Journals");
            Database.GetSqlData("select * from Journals where Vi_id='" + vi_id + "'", dtJournal);
            
            if (dtVoucherInfo.Rows.Count == 0)
            {                
                textBox6.Text = "";
                textBox1.Text = "0";
                textBox7.Text = "0";
                textBox8.Text = "0";
                txtTotalWeight.Text = "";
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

                DataTable dt = new DataTable();
              //  Database.GetSqlData("SELECT VOUCHERINFOs_1.Vi_id, CONVERT(nvarchar, VOUCHERINFOs.Vdate, 106) AS Booking_date, VOUCHERINFOs.Vnumber AS GRno, ACCOUNTs.name AS Consigner, ACCOUNTs_1.name AS Consignee, DeliveryPoints_1.Name AS source, DeliveryPoints.Name AS destination, VOUCHERINFOs.DeliveryType, VOUCHERINFOs.PaymentMode AS GR_type, VOUCHERINFOs.Transport1 AS Private, VOUCHERINFOs.Transport5 AS Remark, SUM(Voucherdets.Quantity) AS Total_quantity, SUM(Voucherdets.weight) AS Total_weight, VOUCHERINFOs.Totalamount as total_amount, SUM(Voucherdets.Rate_am) AS Freight, Voucherdets.exp4amt AS door_delivery, Voucherdets.exp8amt,Voucherdets_1.Bill_booking_id as Booking_id FROM Voucherdets AS Voucherdets_1 LEFT OUTER JOIN VOUCHERINFOs LEFT OUTER JOIN Voucherdets ON VOUCHERINFOs.Vi_id = Voucherdets.Vi_id LEFT OUTER JOIN DeliveryPoints ON VOUCHERINFOs.SId = DeliveryPoints.DPId LEFT OUTER JOIN DeliveryPoints AS DeliveryPoints_1 ON VOUCHERINFOs.Consigner_id = DeliveryPoints_1.DPId LEFT OUTER JOIN ACCOUNTs ON VOUCHERINFOs.Ac_id = ACCOUNTs.ac_id LEFT OUTER JOIN ACCOUNTs AS ACCOUNTs_1 ON VOUCHERINFOs.Ac_id2 = ACCOUNTs_1.ac_id ON Voucherdets_1.Bill_booking_id = VOUCHERINFOs.Vi_id RIGHT OUTER JOIN VOUCHERTYPEs RIGHT OUTER JOIN VOUCHERINFOs AS VOUCHERINFOs_1 ON VOUCHERTYPEs.Vt_id = VOUCHERINFOs_1.Vt_id ON Voucherdets_1.Vi_id = VOUCHERINFOs_1.Vi_id WHERE (VOUCHERTYPEs.Type = N'Sale') GROUP BY VOUCHERINFOs_1.Vi_id, VOUCHERINFOs.Vdate, VOUCHERINFOs.Vnumber, ACCOUNTs.name, ACCOUNTs_1.name, DeliveryPoints_1.Name, DeliveryPoints.Name, VOUCHERINFOs.DeliveryType, VOUCHERINFOs.PaymentMode, VOUCHERINFOs.Transport1, VOUCHERINFOs.Transport5, VOUCHERINFOs.Totalamount, VOUCHERINFOs_1.LocationId, Voucherdets.exp4amt, Voucherdets.exp8amt,Voucherdets_1.Bill_booking_id HAVING (VOUCHERINFOs_1.Vi_id = '" + vi_id + "')", dt);
                Database.GetSqlData("SELECT VOUCHERINFOs.Vi_id, CONVERT(nvarchar, VOUCHERINFOs.Vdate, 106) AS Booking_date, VOUCHERINFOs.Vnumber AS GRno, ACCOUNTs.name AS Consigner, ACCOUNTs_1.name AS Consignee, DeliveryPoints_1.Name AS source, DeliveryPoints.Name AS destination, VOUCHERINFOs.DeliveryType, VOUCHERINFOs.PaymentMode AS GR_type, VOUCHERINFOs.Transport1 AS Private, VOUCHERINFOs.Transport5 AS Remark, SUM(Voucherdets.Quantity) AS Total_quantity, SUM(Voucherdets.weight) AS Total_weight, VOUCHERINFOs.Totalamount AS total_amount, SUM(Voucherdets.Rate_am) AS Freight, SUM(Voucherdets.exp4amt) AS door_delivery,SUM(Voucherdets.exp8amt) as exp8amt FROM Voucherdets RIGHT OUTER JOIN VOUCHERINFOs ON Voucherdets.Vi_id = VOUCHERINFOs.Vi_id LEFT OUTER JOIN Voucherdets AS Voucherdets_1 ON VOUCHERINFOs.Vi_id = Voucherdets_1.Bill_booking_id LEFT OUTER JOIN VOUCHERTYPEs ON VOUCHERINFOs.Vt_id = VOUCHERTYPEs.Vt_id LEFT OUTER JOIN DeliveryPoints ON VOUCHERINFOs.SId = DeliveryPoints.DPId LEFT OUTER JOIN ACCOUNTs ON VOUCHERINFOs.Ac_id = ACCOUNTs.ac_id LEFT OUTER JOIN ACCOUNTs AS ACCOUNTs_1 ON VOUCHERINFOs.Ac_id2 = ACCOUNTs_1.ac_id LEFT OUTER JOIN DeliveryPoints AS DeliveryPoints_1 ON VOUCHERINFOs.Consigner_id = DeliveryPoints_1.DPId WHERE (VOUCHERTYPEs.Type = 'Booking') AND (Voucherdets_1.Vi_id = '" + vid + "')  GROUP BY VOUCHERINFOs.Vi_id, VOUCHERINFOs.Vdate, VOUCHERINFOs.Vnumber, ACCOUNTs.name, ACCOUNTs_1.name, DeliveryPoints_1.Name, DeliveryPoints.Name, VOUCHERINFOs.DeliveryType, VOUCHERINFOs.PaymentMode, VOUCHERINFOs.Transport1, VOUCHERINFOs.Transport5, VOUCHERINFOs.Totalamount HAVING (VOUCHERINFOs.PaymentMode = 'T.B.B.') AND (ACCOUNTs.name = '" + textBox6.Text + "') AND (dbo.VOUCHERINFOs.Vdate >= " + access_sql.Hash + "" + dateTimePicker2.Value.Date.ToString(Database.dformat) + "" + access_sql.Hash + ") AND  (dbo.VOUCHERINFOs.Vdate <= " + access_sql.Hash + "" + dateTimePicker3.Value.Date.ToString(Database.dformat) + "" + access_sql.Hash + ") ORDER BY GRno DESC",dt);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ansGridView1.Rows.Add();
                    ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["select"].Value = true;
                    //SendKeys.Send("{enter}");
                    ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["vi_id1"].Value = dt.Rows[i]["Vi_id"].ToString();
                    ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["booking_date1"].Value = dt.Rows[i]["Booking_date"].ToString();
                    ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["grno1"].Value = dt.Rows[i]["GRno"].ToString();
                    ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["consigner1"].Value = dt.Rows[i]["Consigner"].ToString();
                    ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["consignee1"].Value = dt.Rows[i]["Consignee"].ToString();
                    ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["source1"].Value = dt.Rows[i]["source"].ToString();
                    ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["destination1"].Value = dt.Rows[i]["destination"].ToString();
                    ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["delivery1"].Value = dt.Rows[i]["DeliveryType"].ToString();
                    ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["grtype1"].Value = dt.Rows[i]["GR_type"].ToString();
                    ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["private1"].Value = dt.Rows[i]["Private"].ToString();
                    ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["remark1"].Value = dt.Rows[i]["Remark"].ToString();
                    ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["qty1"].Value = dt.Rows[i]["Total_quantity"].ToString();
                    ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["wt1"].Value = dt.Rows[i]["Total_weight"].ToString();
                    ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["amt1"].Value = dt.Rows[i]["total_amount"].ToString();
                    ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["freight1"].Value = dt.Rows[i]["Freight"].ToString();

                    if (dt.Rows[i]["exp8amt"].ToString() == "")
                    {
                        dt.Rows[i]["exp8amt"] = 0;
                    }
                    if (dt.Rows[i]["Door_delivery"].ToString() == "")
                    {
                        dt.Rows[i]["Door_delivery"] = 0;
                    }

                    //ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["exp1"].Value = dt.Rows[i][exp1].ToString();
                    //ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["exp2"].Value = dt.Rows[i][exp2].ToString();
                    //ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["exp3"].Value = dt.Rows[i][exp3].ToString();
                    ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["dd1"].Value = (double.Parse(dt.Rows[i]["Door_delivery"].ToString()) + double.Parse(dt.Rows[i]["exp8amt"].ToString())).ToString();
                    //ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["dd1"].Value = dt.Rows[i][exp4].ToString();
                    //ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["exp5"].Value = dt.Rows[i][exp5].ToString();
                    //ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["exp6"].Value = dt.Rows[i][exp6].ToString();
                    //ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["exp7"].Value = dt.Rows[i][exp7].ToString();
                    //ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["exp8"].Value = dt.Rows[i][exp8].ToString();
                    //ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["exp9"].Value = dt.Rows[i][exp9].ToString();
                    //ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["exp10"].Value = dt.Rows[i][exp10].ToString();
                }

               // dateTimePicker1.Select();
                
                
                
                
                
                
                
                
                
                
                
                weightCalc();
            }
        }


        private void save()
        {
            string prefix = "";
            string postfix = "";
            int padding = 0;

            padding = Database.GetScalarInt("Select Bill_Padding from Location where LocationId='" + Database.LocationId + "'");
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
            dtVoucherInfo.Rows[0]["Narr"] = "Billing";
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
            dtVoucherInfo.Rows[0]["ac_id"] = funs.Select_ac_id(textBox6.Text);
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
                    dtVoucherdet.Rows[dtVoucherdet.Rows.Count - 1]["Bill_booking_id"] = ansGridView1.Rows[i].Cells["vi_id1"].Value.ToString();
                    dtVoucherdet.Rows[dtVoucherdet.Rows.Count - 1]["remarkreq"] = false;
                    dtVoucherdet.Rows[dtVoucherdet.Rows.Count - 1]["create_date"] = create_date;
                    dtVoucherdet.Rows[dtVoucherdet.Rows.Count - 1]["modify_date"] = System.DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                    dtVoucherdet.Rows[dtVoucherdet.Rows.Count - 1]["multiplier"] = 1;
                    dtVoucherdet.Rows[dtVoucherdet.Rows.Count - 1]["Amount"] = 0;
                    Nid2++;
                }
            }

            Database.SaveData(dtVoucherdet);
            DataTable dttemp = new DataTable("journals");
            Database.GetSqlData("Select * from journals where Vi_id='" + vid + "'", dttemp);
            for (int j = 0; j < dttemp.Rows.Count; j++)
            {
                dttemp.Rows[j].Delete();
            }
            Database.SaveData(dttemp);

            dtJournal = new DataTable("journals");
            Database.GetSqlData("Select * from journals where Vi_id='" + vid + "'", dtJournal);


            if (iscancel == false)
            {


                //debit
                DataRow dtrjou = dtJournal.Rows.Add();
                dtrjou["Vi_id"] = vid;
                dtrjou["vdate"] = dateTimePicker1.Value.Date.ToString(Database.dformat);
              
                dtrjou["Ac_id"] = funs.Select_ac_id(textBox6.Text);
                   

              

                dtrjou["Opp_Acid"] = Database.LocationExpAcc_id;

                dtrjou["Narr"] = "Bill";
                dtrjou["Sno"] = 1;
                dtrjou["LocationId"] = Prelocationid;
                dtrjou["Amount"] = double.Parse(textBox8.Text);

                dtrjou["Narr2"] = "Bill";
                dtrjou["Reffno"] = vno;


             
                //credit
                DataRow dtrjou1 = dtJournal.Rows.Add();
                dtrjou1["Vi_id"] = vid;
                dtrjou1["vdate"] = dateTimePicker1.Value.Date.ToString(Database.dformat);

                dtrjou1["Ac_id"] = Database.LocationExpAcc_id;


                dtrjou1["Narr"] = "Booking";
                dtrjou1["Sno"] = 2;
                dtrjou1["LocationId"] = Prelocationid;
                dtrjou1["Amount"] = -1 * double.Parse(textBox8.Text);
                dtrjou1["Opp_Acid"] = funs.Select_ac_id(textBox6.Text); ;
                dtrjou1["Narr2"] = "Bill";
                dtrjou1["Reffno"] = vno;
                Database.SaveData(dtJournal);
            }

         
            MessageBox.Show("Saved Successfully");


     
        }

        private void clear()
        {
            if (gStr == "0")
            {
                LoadData("0", "Sale");
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

        private void ansGridView1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (ansGridView1.CurrentCell == null)
            {
                return;
            }
            if (ansGridView1.CurrentCell.OwningColumn.Name == "item")
            {
                strCombo = "SELECT DISTINCT item.name FROM item RIGHT OUTER JOIN ItemDetail ON item.Id = ItemDetail.Item_id ORDER BY item.name";
                ansGridView1.CurrentCell.Value = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
            }
            if (ansGridView1.CurrentCell.OwningColumn.Name == "consigner1" || ansGridView1.CurrentCell.OwningColumn.Name == "consignee1")
            {
                 strCombo = "SELECT ACCOUNT.Name, ACCOUNT.Printname, DeliveryPoint.Name as Station, ACCOUNT.Address1, ACCOUNT.Address2, ACCOUNT.Phone, ACCOUNT.Tin_number, OTHER.Name as Staff, CONTRACTOR.Name as Agent FROM ((ACCOUNT LEFT JOIN OTHER ON ACCOUNT.Loc_id = OTHER.Oth_id) LEFT JOIN CONTRACTOR ON ACCOUNT.Con_id = CONTRACTOR.Con_id) LEFT JOIN DeliveryPoint ON ACCOUNT.SId = DeliveryPoint.DPId";
                ansGridView1.CurrentCell.Value = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
            }
        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
           // strCombo = "SELECT DISTINCT ACCOUNTs.name AS Consigner,dbo.VOUCHERINFOs.Vdate  FROM Voucherdets RIGHT OUTER JOIN VOUCHERINFOs ON Voucherdets.Vi_id = VOUCHERINFOs.Vi_id LEFT OUTER JOIN Voucherdets AS Voucherdets_1 ON VOUCHERINFOs.Vi_id = Voucherdets_1.Bill_booking_id LEFT OUTER JOIN VOUCHERTYPEs ON VOUCHERINFOs.Vt_id = VOUCHERTYPEs.Vt_id LEFT OUTER JOIN DeliveryPoints ON VOUCHERINFOs.SId = DeliveryPoints.DPId LEFT OUTER JOIN ACCOUNTs ON VOUCHERINFOs.Ac_id = ACCOUNTs.ac_id LEFT OUTER JOIN ACCOUNTs AS ACCOUNTs_1 ON VOUCHERINFOs.Ac_id2 = ACCOUNTs_1.ac_id LEFT OUTER JOIN DeliveryPoints AS DeliveryPoints_1 ON VOUCHERINFOs.Consigner_id = DeliveryPoints_1.DPId WHERE (VOUCHERTYPEs.Type = 'Booking') AND (Voucherdets_1.Bill_booking_id IS NULL) GROUP BY VOUCHERINFOs.Vi_id, VOUCHERINFOs.Vdate, VOUCHERINFOs.Vnumber, ACCOUNTs.name, ACCOUNTs_1.name, DeliveryPoints_1.Name, DeliveryPoints.Name, VOUCHERINFOs.DeliveryType, VOUCHERINFOs.PaymentMode, VOUCHERINFOs.Transport1, VOUCHERINFOs.Transport5, VOUCHERINFOs.Totalamount HAVING (VOUCHERINFOs.PaymentMode = 'T.B.B.')";
           strCombo = "SELECT DISTINCT ACCOUNTs.name AS Consigner  FROM Voucherdets RIGHT OUTER JOIN VOUCHERINFOs ON Voucherdets.Vi_id = VOUCHERINFOs.Vi_id LEFT OUTER JOIN Voucherdets AS Voucherdets_1 ON VOUCHERINFOs.Vi_id = Voucherdets_1.Bill_booking_id LEFT OUTER JOIN VOUCHERTYPEs ON VOUCHERINFOs.Vt_id = VOUCHERTYPEs.Vt_id LEFT OUTER JOIN DeliveryPoints ON VOUCHERINFOs.SId = DeliveryPoints.DPId LEFT OUTER JOIN ACCOUNTs ON VOUCHERINFOs.Ac_id = ACCOUNTs.ac_id LEFT OUTER JOIN ACCOUNTs AS ACCOUNTs_1 ON VOUCHERINFOs.Ac_id2 = ACCOUNTs_1.ac_id LEFT OUTER JOIN DeliveryPoints AS DeliveryPoints_1 ON VOUCHERINFOs.Consigner_id = DeliveryPoints_1.DPId WHERE (VOUCHERTYPEs.Type = 'Booking') AND (Voucherdets_1.Bill_booking_id IS NULL) GROUP BY VOUCHERINFOs.Vi_id, VOUCHERINFOs.Vdate, VOUCHERINFOs.Vnumber, ACCOUNTs.name, ACCOUNTs_1.name, DeliveryPoints_1.Name, DeliveryPoints.Name, VOUCHERINFOs.DeliveryType, VOUCHERINFOs.PaymentMode, VOUCHERINFOs.Transport1, VOUCHERINFOs.Transport5, VOUCHERINFOs.Totalamount HAVING (VOUCHERINFOs.PaymentMode = 'T.B.B.') AND (dbo.VOUCHERINFOs.Vdate >= " + access_sql.Hash + "" + dateTimePicker2.Value.Date.ToString(Database.dformat) + "" + access_sql.Hash + ") AND  (dbo.VOUCHERINFOs.Vdate <= " + access_sql.Hash + "" + dateTimePicker3.Value.Date.ToString(Database.dformat) + "" + access_sql.Hash + ")";
           // strCombo = "SELECT DISTINCT dbo.ACCOUNTs.name FROM  dbo.VOUCHERINFOs LEFT OUTER JOIN  dbo.ACCOUNTs ON dbo.VOUCHERINFOs.Ac_id = dbo.ACCOUNTs.ac_id LEFT OUTER JOIN        dbo.Voucherdets ON dbo.VOUCHERINFOs.Vi_id = dbo.Voucherdets.Bill_booking_id LEFT OUTER JOIN   dbo.VOUCHERTYPEs ON dbo.VOUCHERINFOs.Vt_id = dbo.VOUCHERTYPEs.Vt_id WHERE        (dbo.VOUCHERTYPEs.Type = 'Booking') AND (dbo.VOUCHERINFOs.PaymentMode = N'T.B.B.') AND (dbo.Voucherdets.Bill_booking_id IS NULL)";
            textBox6.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 1);
           // textBox1.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 2);
            fillGrid();
        }

        private void fillGrid()
        {
            ansGridView1.Rows.Clear();
            string str = "";
            if (vid != "0")
            {
                   
                str = "SELECT VOUCHERINFOs.Vi_id, CONVERT(nvarchar, VOUCHERINFOs.Vdate, 106) AS Booking_date, VOUCHERINFOs.Vnumber AS GRno, ACCOUNTs.name AS Consigner, ACCOUNTs_1.name AS Consignee, DeliveryPoints_1.Name AS source, DeliveryPoints.Name AS destination, VOUCHERINFOs.DeliveryType, VOUCHERINFOs.PaymentMode AS GR_type, VOUCHERINFOs.Transport1 AS Private, VOUCHERINFOs.Transport5 AS Remark, SUM(Voucherdets.Quantity) AS Total_quantity, SUM(Voucherdets.weight) AS Total_weight, VOUCHERINFOs.Totalamount AS total_amount, SUM(Voucherdets.Rate_am) AS Freight, SUM(Voucherdets.exp4amt) AS door_delivery,SUM(Voucherdets.exp8amt) as exp8amt FROM Voucherdets RIGHT OUTER JOIN VOUCHERINFOs ON Voucherdets.Vi_id = VOUCHERINFOs.Vi_id LEFT OUTER JOIN Voucherdets AS Voucherdets_1 ON VOUCHERINFOs.Vi_id = Voucherdets_1.Bill_booking_id LEFT OUTER JOIN VOUCHERTYPEs ON VOUCHERINFOs.Vt_id = VOUCHERTYPEs.Vt_id LEFT OUTER JOIN DeliveryPoints ON VOUCHERINFOs.SId = DeliveryPoints.DPId LEFT OUTER JOIN ACCOUNTs ON VOUCHERINFOs.Ac_id = ACCOUNTs.ac_id LEFT OUTER JOIN ACCOUNTs AS ACCOUNTs_1 ON VOUCHERINFOs.Ac_id2 = ACCOUNTs_1.ac_id LEFT OUTER JOIN DeliveryPoints AS DeliveryPoints_1 ON VOUCHERINFOs.Consigner_id = DeliveryPoints_1.DPId WHERE (VOUCHERTYPEs.Type = 'Booking') AND (Voucherdets_1.Vi_id = '" + vid + "')  GROUP BY VOUCHERINFOs.Vi_id, VOUCHERINFOs.Vdate, VOUCHERINFOs.Vnumber, ACCOUNTs.name, ACCOUNTs_1.name, DeliveryPoints_1.Name, DeliveryPoints.Name, VOUCHERINFOs.DeliveryType, VOUCHERINFOs.PaymentMode, VOUCHERINFOs.Transport1, VOUCHERINFOs.Transport5, VOUCHERINFOs.Totalamount HAVING (VOUCHERINFOs.PaymentMode = 'T.B.B.') AND (ACCOUNTs.name = '" + textBox6.Text + "') AND (dbo.VOUCHERINFOs.Vdate >= " + access_sql.Hash + "" + dateTimePicker2.Value.Date.ToString(Database.dformat) + "" + access_sql.Hash + ") AND  (dbo.VOUCHERINFOs.Vdate <= " + access_sql.Hash + "" + dateTimePicker3.Value.Date.ToString(Database.dformat) + "" + access_sql.Hash + ") ORDER BY GRno DESC";
            }
            else
            {
                str = "SELECT VOUCHERINFOs.Vi_id, CONVERT(nvarchar, VOUCHERINFOs.Vdate, 106) AS Booking_date, VOUCHERINFOs.Vnumber AS GRno, ACCOUNTs.name AS Consigner, ACCOUNTs_1.name AS Consignee, DeliveryPoints_1.Name AS source, DeliveryPoints.Name AS destination, VOUCHERINFOs.DeliveryType, VOUCHERINFOs.PaymentMode AS GR_type, VOUCHERINFOs.Transport1 AS Private, VOUCHERINFOs.Transport5 AS Remark, SUM(Voucherdets.Quantity) AS Total_quantity, SUM(Voucherdets.weight) AS Total_weight, VOUCHERINFOs.Totalamount AS total_amount, SUM(Voucherdets.Rate_am) AS Freight, SUM(Voucherdets.exp4amt) AS door_delivery,SUM(Voucherdets.exp8amt) as exp8amt FROM Voucherdets RIGHT OUTER JOIN VOUCHERINFOs ON Voucherdets.Vi_id = VOUCHERINFOs.Vi_id LEFT OUTER JOIN Voucherdets AS Voucherdets_1 ON VOUCHERINFOs.Vi_id = Voucherdets_1.Bill_booking_id LEFT OUTER JOIN VOUCHERTYPEs ON VOUCHERINFOs.Vt_id = VOUCHERTYPEs.Vt_id LEFT OUTER JOIN DeliveryPoints ON VOUCHERINFOs.SId = DeliveryPoints.DPId LEFT OUTER JOIN ACCOUNTs ON VOUCHERINFOs.Ac_id = ACCOUNTs.ac_id LEFT OUTER JOIN ACCOUNTs AS ACCOUNTs_1 ON VOUCHERINFOs.Ac_id2 = ACCOUNTs_1.ac_id LEFT OUTER JOIN DeliveryPoints AS DeliveryPoints_1 ON VOUCHERINFOs.Consigner_id = DeliveryPoints_1.DPId WHERE (VOUCHERTYPEs.Type = 'Booking') AND (Voucherdets_1.Vi_id is null) AND (dbo.VOUCHERINFOs.Iscancel = 0)  GROUP BY VOUCHERINFOs.Vi_id, VOUCHERINFOs.Vdate, VOUCHERINFOs.Vnumber, ACCOUNTs.name, ACCOUNTs_1.name, DeliveryPoints_1.Name, DeliveryPoints.Name, VOUCHERINFOs.DeliveryType, VOUCHERINFOs.PaymentMode, VOUCHERINFOs.Transport1, VOUCHERINFOs.Transport5, VOUCHERINFOs.Totalamount HAVING (VOUCHERINFOs.PaymentMode = 'T.B.B.') AND (ACCOUNTs.name = '" + textBox6.Text + "') AND (dbo.VOUCHERINFOs.Vdate >= " + access_sql.Hash + "" + dateTimePicker2.Value.Date.ToString(Database.dformat) + "" + access_sql.Hash + ") AND  (dbo.VOUCHERINFOs.Vdate <= " + access_sql.Hash + "" + dateTimePicker3.Value.Date.ToString(Database.dformat) + "" + access_sql.Hash + ") ORDER BY GRno DESC";
            }
            DataTable dtfill = new DataTable();
             Database.GetSqlData(str, dtfill);

            for (int m = 0; m < dtfill.Rows.Count; m++)
            {
                ansGridView1.Rows.Add();
                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["select"].Value = true;
                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["vi_id1"].Value = dtfill.Rows[m]["vi_id"].ToString();
                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["booking_date1"].Value = dtfill.Rows[m]["Booking_date"].ToString();
                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["grno1"].Value = dtfill.Rows[m]["GRno"].ToString();
                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["consigner1"].Value = dtfill.Rows[m]["Consigner"].ToString();
                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["consignee1"].Value = dtfill.Rows[m]["Consignee"].ToString();
                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["source1"].Value = dtfill.Rows[m]["source"].ToString();
                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["destination1"].Value = dtfill.Rows[m]["destination"].ToString();
                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["delivery1"].Value = dtfill.Rows[m]["DeliveryType"].ToString();
                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["grtype1"].Value = dtfill.Rows[m]["GR_type"].ToString();
                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["private1"].Value = dtfill.Rows[m]["Private"].ToString();
                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["remark1"].Value = dtfill.Rows[m]["Remark"].ToString();
                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["qty1"].Value = dtfill.Rows[m]["Total_quantity"].ToString();
                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["wt1"].Value = dtfill.Rows[m]["Total_weight"].ToString();
                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["amt1"].Value = dtfill.Rows[m]["total_amount"].ToString();
                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["freight1"].Value = dtfill.Rows[m]["Freight"].ToString();
                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["dd1"].Value = (double.Parse(dtfill.Rows[m]["Door_delivery"].ToString()) + double.Parse(dtfill.Rows[m]["exp8amt"].ToString())).ToString();
            }
            weightCalc();
        }

        private void ansGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (ansGridView1.CurrentCell.OwningColumn.Name == "sno")
            {
                SendKeys.Send("{right}");
                this.Activate();
            }
            ansGridView1.Rows[e.RowIndex].Cells["sno"].Value = e.RowIndex + 1;
        }

        private void ansGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (ansGridView1.CurrentCell == null)
            {
                return;
            }
            if (e.KeyCode == Keys.Delete)
            {
                if (ansGridView1.CurrentRow.Index == ansGridView1.Rows.Count - 1)
                {
                    ansGridView1.Rows[ansGridView1.CurrentRow.Index].Cells[1].Value = "";
                    ansGridView1.Rows[ansGridView1.CurrentRow.Index].Cells[2].Value = "";
                    return;
                }
                else
                {
                    ansGridView1.Rows.RemoveAt(ansGridView1.CurrentRow.Index);
                    for (int i = 0; i < ansGridView1.Rows.Count; i++)
                    {
                        ansGridView1.Rows[i].Cells["sno"].Value = (i + 1);
                    }
                    return;
                }
            }
        }

        private void dateTimePicker1_Enter(object sender, EventArgs e)
        {
            Database.setFocus(dateTimePicker1);            
        }

        private void dateTimePicker1_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);   
        }

        private void dateTimePicker1_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(dateTimePicker1);
        }

        private void textBox6_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox6);
        }

        private void textBox6_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox6);
        }

        private void ansGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            //weightCalc();
        }

        private void ansGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            weightCalc();
        }

        private void ansGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //weightCalc();
        }

        private void frm_bill_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
                this.Dispose();
            }
            if (e.Control && e.KeyCode == Keys.F12)
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
            else if (e.Control && e.KeyCode == Keys.S)
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

            else if (e.Control && e.KeyCode == Keys.P)
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
        }

        private void ansGridView1_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            //weightCalc();
        }

        private void dateTimePicker2_Enter(object sender, EventArgs e)
        {
            Database.setFocus(dateTimePicker2);
        }

        private void dateTimePicker2_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode); 
        }

        private void dateTimePicker2_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(dateTimePicker2);
        }

        private void dateTimePicker3_Enter(object sender, EventArgs e)
        {
            Database.setFocus(dateTimePicker3);
        }

        private void dateTimePicker3_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode); 
        }

        private void dateTimePicker3_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(dateTimePicker3);
        }
    }
}
