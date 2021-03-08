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
    public partial class frm_unloading : Form
    {
        string gch_vid = "";
        int vtid;
        int vno = 0;
        string gStr = "";
        string vid = "";
        DataTable dtVoucherinfo;
        DataTable dtVoucherDet;
        public Boolean gresave = false;
        string Prelocationid = "";
        Boolean RoffChanged = false;
        bool iscancel = false;


        DateTime create_date = DateTime.Parse(System.DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"));

        public frm_unloading()
        {
            InitializeComponent();
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
            gStr = vi_id.ToString();
            vid = vi_id;
            vtid = funs.Select_vt_id("Unloading");
            
            //if (vid == "0")
            //{
            //    SetVno();
            //}

            dtVoucherinfo = new DataTable("VOUCHERINFOs");
            Database.GetSqlData("select * from VOUCHERINFOs where vi_id='" + vid + "'", dtVoucherinfo);

            if (dtVoucherinfo.Rows.Count == 0)
            {
                if (dtVoucherinfo.Rows.Count == 0)
                {
                    dtVoucherinfo.Rows.Add();
                }
                dateTimePicker1.Value = Database.ldate;
                label1.Text = vno.ToString();
                txtTruckNo.Text = "";
                ansGridView1.Rows.Clear();
                iscancel = false;
                label28.Visible = false;
            }
            else
            {
                vno = int.Parse(dtVoucherinfo.Rows[0]["Vnumber"].ToString());
                label1.Text = vno.ToString();
                dateTimePicker1.Value = DateTime.Parse(dtVoucherinfo.Rows[0]["Vdate"].ToString());
                create_date = DateTime.Parse(dtVoucherinfo.Rows[0]["create_date"].ToString());
                label1.Text = dtVoucherinfo.Rows[0]["Vnumber"].ToString();
                textBox1.Text = dtVoucherinfo.Rows[0]["Narr"].ToString();

                txtTruckNo.Text = Database.GetScalarText("SELECT VOUCHERINFOs_1.Invoiceno FROM VOUCHERINFOs LEFT OUTER JOIN VOUCHERINFOs AS VOUCHERINFOs_1 ON VOUCHERINFOs.Challan_id = VOUCHERINFOs_1.Vi_id WHERE VOUCHERINFOs.Vi_id ='" + vi_id + "'").ToString();
                if (bool.Parse(dtVoucherinfo.Rows[0]["Iscancel"].ToString()) == true)
                {
                    label28.Visible = true;
                    label28.Text = "Cancelled";
                    iscancel = bool.Parse(dtVoucherinfo.Rows[0]["Iscancel"].ToString());
                }

                fillGrid(Database.GetScalarText("SELECT VOUCHERINFOs_1.Vi_id FROM VOUCHERINFOs LEFT OUTER JOIN VOUCHERINFOs AS VOUCHERINFOs_1 ON VOUCHERINFOs.Challan_id = VOUCHERINFOs_1.Vi_id WHERE VOUCHERINFOs.Vi_id = '" + vi_id + "'"));
            
            }
            if (gresave == true)
            {
                object sender = new object();
                EventArgs e = new EventArgs();
                btn_Click(sender, e);
            }
          
           SetVno();
           
        }


        private void frm_unloading_KeyDown(object sender, KeyEventArgs e)
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
                }
            }


            //if (e.Control && e.KeyCode == Keys.P)
            //{
            //    if (validation() == true)
            //    {
            //        try
            //        {
            //            Database.BeginTran();
            //            if (Database.utype == "Admin")
            //            {
            //                save(true);
            //            }
            //            else if (gStr == "0")
            //            {
            //                save(true);
            //            }
            //            Database.CommitTran();
            //        }
            //        catch (Exception ex)
            //        {
            //            Database.RollbackTran();
            //        }
            //    }
            //}


            //if (e.Control && e.KeyCode == Keys.W)
            //{
            //    if (validation() == true)
            //    {
            //        try
            //        {
            //            Database.BeginTran();
            //            if (Database.utype == "Admin")
            //            {
            //                save(false);
            //            }
            //            else if (gStr == "0")
            //            {
            //                save(false);
            //            }
            //            Database.CommitTran();
            //        }
            //        catch (Exception ex)
            //        {
            //            Database.RollbackTran();
            //        }
            //    }
            //}
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
                            vno = int.Parse(label1.Text);
                        }
                        else
                        {
                            vno = int.Parse(box.outStr);
                        }

                        label1.Text = vno.ToString();
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
                                label1.Text = vno.ToString();
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
            //dtsidefill.Rows.Add();
            //dtsidefill.Rows[1]["Name"] = "Print";
            //dtsidefill.Rows[1]["DisplayName"] = "Print";
            //dtsidefill.Rows[1]["ShortcutKey"] = "^P";
            //dtsidefill.Rows[1]["Visible"] = true;

            ////print preview
            //dtsidefill.Rows.Add();
            //dtsidefill.Rows[2]["Name"] = "PrintPre";
            //dtsidefill.Rows[2]["DisplayName"] = "Print Preview";
            //dtsidefill.Rows[2]["ShortcutKey"] = "^W";
            //dtsidefill.Rows[2]["Visible"] = true;




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
            dtsidefill.Rows[dtsidefill.Rows.Count-1]["Name"] = "vnumber";
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

        public void btn_Click(object sender, EventArgs e)
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

                            DataTable dtstocks = new DataTable("stocks");
                            Database.GetSqlData("Select * from stocks where Vid='" + vid + "'", dtstocks);
                            for (int j = 0; j < dtstocks.Rows.Count; j++)
                            {
                                dtstocks.Rows[j].Delete();
                            }
                            Database.SaveData(dtstocks);

                            if (iscancel == false)
                            {
                                string loca_dp_id = Database.GetScalarText("Select dp_id from Location where LocationId='" + Database.LocationId + "'");
                                for (int i = 0; i < ansGridView1.Rows.Count; i++)
                                {
                                    //string bookingid = ansGridView1.Rows[i].Cells["vi_id1"].Value.ToString();

                                    dtstocks.Rows.Add();

                                    dtstocks.Rows[dtstocks.Rows.Count - 1]["Vid"] = vid;

                                    dtstocks.Rows[dtstocks.Rows.Count - 1]["GR_id"] = ansGridView1.Rows[i].Cells["vi_id1"].Value.ToString();
                                    dtstocks.Rows[dtstocks.Rows.Count - 1]["Quantity"] = 1;

                                    dtstocks.Rows[dtstocks.Rows.Count - 1]["Step"] = "Step2";

                                    dtstocks.Rows[dtstocks.Rows.Count - 1]["Godown_id"] = Database.LocationId;
                                    string aliasname = Database.GetScalarText("Select Aliasname from vouchertypes where vt_id=" + vtid);
                                   
                                    dtstocks.Rows[dtstocks.Rows.Count - 1]["Narration"] = aliasname + " At " + funs.Select_dp_nm(loca_dp_id);


                                    dtstocks.Rows[dtstocks.Rows.Count - 1]["GRNo"] = ansGridView1.Rows[i].Cells["grno1"].Value.ToString();
                                    dtstocks.Rows[dtstocks.Rows.Count - 1]["GRDate"] = DateTime.Parse(ansGridView1.Rows[i].Cells["booking_date1"].Value.ToString());

                                    dtstocks.Rows[dtstocks.Rows.Count - 1]["Consigner_id"] = funs.Select_ac_id(ansGridView1.Rows[i].Cells["consigner1"].Value.ToString());
                                    dtstocks.Rows[dtstocks.Rows.Count - 1]["Consignee_id"] = funs.Select_ac_id(ansGridView1.Rows[i].Cells["consignee1"].Value.ToString());


                                    dtstocks.Rows[dtstocks.Rows.Count - 1]["Source_id"] = funs.Select_dp_id(ansGridView1.Rows[i].Cells["source1"].Value.ToString());
                                    dtstocks.Rows[dtstocks.Rows.Count - 1]["Destination_id"] = funs.Select_dp_id(ansGridView1.Rows[i].Cells["destination1"].Value.ToString());

                                    dtstocks.Rows[dtstocks.Rows.Count - 1]["ToPay"] = double.Parse(ansGridView1.Rows[i].Cells["pay1"].Value.ToString());
                                    dtstocks.Rows[dtstocks.Rows.Count - 1]["TBB"] = double.Parse(ansGridView1.Rows[i].Cells["billed1"].Value.ToString());

                                    dtstocks.Rows[dtstocks.Rows.Count - 1]["Paid"] = double.Parse(ansGridView1.Rows[i].Cells["Paid1"].Value.ToString());
                                    dtstocks.Rows[dtstocks.Rows.Count - 1]["FOC"] = double.Parse(ansGridView1.Rows[i].Cells["Foc1"].Value.ToString());



                                    dtstocks.Rows[dtstocks.Rows.Count - 1]["Totpkts"] = double.Parse(ansGridView1.Rows[i].Cells["qty1"].Value.ToString());
                                    dtstocks.Rows[dtstocks.Rows.Count - 1]["totweight"] = double.Parse(ansGridView1.Rows[i].Cells["wt1"].Value.ToString());

                                    dtstocks.Rows[dtstocks.Rows.Count - 1]["deliverytype"] = ansGridView1.Rows[i].Cells["delivery1"].Value.ToString();
                                    dtstocks.Rows[dtstocks.Rows.Count - 1]["grtype"] = ansGridView1.Rows[i].Cells["grtype1"].Value.ToString();

                                    dtstocks.Rows[dtstocks.Rows.Count - 1]["itemname"] = ansGridView1.Rows[i].Cells["itemname"].Value.ToString();
                                    dtstocks.Rows[dtstocks.Rows.Count - 1]["packing"] = ansGridView1.Rows[i].Cells["packing"].Value.ToString();


                                    dtstocks.Rows[dtstocks.Rows.Count - 1]["private"] = ansGridView1.Rows[i].Cells["private"].Value.ToString();
                                    dtstocks.Rows[dtstocks.Rows.Count - 1]["remark"] = ansGridView1.Rows[i].Cells["remark"].Value.ToString();

                                    dtstocks.Rows[dtstocks.Rows.Count - 1]["grcharge"] = double.Parse(ansGridView1.Rows[i].Cells["grcharge"].Value.ToString());
                                    dtstocks.Rows[dtstocks.Rows.Count - 1]["othcharge"] = double.Parse(ansGridView1.Rows[i].Cells["othcharge"].Value.ToString());

                                    dtstocks.Rows[dtstocks.Rows.Count - 1]["freight"] = double.Parse(ansGridView1.Rows[i].Cells["freight"].Value.ToString());










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
                        vno = int.Parse(label1.Text);
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
                            label1.Text = vno.ToString();
                           // SetVno();
                            return;
                        }
                    }
                  //  f12used = true;
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

        private void weightCalc()
        {
            double TotalWeight = 0;
            double Totalqty = 0;
            double Totalamt = 0;
            double Totalfoc = 0;
            double Totalpay = 0;
            double Totalbilled = 0;
            double Totalpaid = 0;

            for (int i = 0; i < ansGridView1.Rows.Count; i++)
            {
                TotalWeight += double.Parse(ansGridView1.Rows[i].Cells["wt1"].Value.ToString());
                Totalqty += double.Parse(ansGridView1.Rows[i].Cells["qty1"].Value.ToString());
                //Totalamt += double.Parse(ansGridView1.Rows[i].Cells["amt1"].Value.ToString());
                Totalfoc += double.Parse(ansGridView1.Rows[i].Cells["foc1"].Value.ToString());
                Totalpay += double.Parse(ansGridView1.Rows[i].Cells["pay1"].Value.ToString());
                Totalbilled += double.Parse(ansGridView1.Rows[i].Cells["billed1"].Value.ToString());
                Totalpaid += double.Parse(ansGridView1.Rows[i].Cells["paid1"].Value.ToString());
            }

            textBox2.Text = ansGridView1.Rows.Count.ToString();
            textBox7.Text = Totalqty.ToString();
            txtTotalWeight.Text = TotalWeight.ToString();
            textBox19.Text = Totalpay.ToString();
            textBox20.Text = (Totalamt + Totalbilled + Totalpaid).ToString();
        }


        private void fillGrid(string ch_vid)
        {
            ansGridView1.Rows.Clear();
            gch_vid = ch_vid;

            //string str = "SELECT VOUCHERINFOs.Vi_id, VOUCHERINFOs.Vt_id, VOUCHERINFOs_1.Vdate AS grdate, VOUCHERINFOs_1.Invoiceno AS grno, ACCOUNTs.name AS consigner, ACCOUNTs_1.name AS consignee,  VOUCHERINFOs_1.DeliveryType, VOUCHERINFOs_1.PaymentMode, VOUCHERINFOs_1.Delivery_adrs, items.name, Voucherdets_1.Quantity,  Voucherdets_1.weight, Voucherdets_1.ChargedWeight, CASE WHEN VOUCHERINFOs_1.PaymentMode = 'FOC' THEN VOUCHERINFOs_1.Totalamount ELSE 0 END AS total_foc, CASE WHEN VOUCHERINFOs_1.PaymentMode = 'Paid' THEN VOUCHERINFOs_1.Totalamount ELSE 0 END AS total_paid, CASE WHEN VOUCHERINFOs_1.PaymentMode = 'To Pay' THEN VOUCHERINFOs_1.Totalamount ELSE 0 END AS total_pay, CASE WHEN VOUCHERINFOs_1.PaymentMode = 'T.B.B.' THEN VOUCHERINFOs_1.Totalamount ELSE 0 END AS total_Billed, Voucherdets_1.Itemsr FROM VOUCHERINFOs LEFT OUTER JOIN items RIGHT OUTER JOIN Voucherdets AS Voucherdets_1 ON items.Id = Voucherdets_1.Des_ac_id RIGHT OUTER JOIN VOUCHERINFOs AS VOUCHERINFOs_1 LEFT OUTER JOIN ACCOUNTs AS ACCOUNTs_1 ON VOUCHERINFOs_1.Ac_id2 = ACCOUNTs_1.ac_id LEFT OUTER JOIN ACCOUNTs ON VOUCHERINFOs_1.Ac_id = ACCOUNTs.ac_id ON Voucherdets_1.Vi_id = VOUCHERINFOs_1.Vi_id RIGHT OUTER JOIN Voucherdets ON VOUCHERINFOs_1.Vi_id = Voucherdets.Booking_id ON VOUCHERINFOs.Vi_id = Voucherdets.Vi_id WHERE (VOUCHERINFOs.Vt_id = 63) AND (VOUCHERINFOs.Vi_id = '" + ch_vid + "') ORDER BY grdate, grno";
           // string str = "SELECT VOUCHERINFOs_1.Vdate AS grdate, VOUCHERINFOs_1.Invoiceno AS grno, ACCOUNTs.name AS consigner,                       ACCOUNTs_1.name AS consignee, VOUCHERINFOs_1.DeliveryType, VOUCHERINFOs_1.PaymentMode, VOUCHERINFOs_1.Delivery_adrs, SUM(Voucherdets_1.Quantity) AS Quantity, SUM(Voucherdets_1.weight) AS weight, SUM(Voucherdets_1.ChargedWeight) AS Expr3,                       CASE WHEN VOUCHERINFOs_1.PaymentMode = 'FOC' THEN SUM(VOUCHERINFOs_1.Totalamount) ELSE 0 END AS total_foc,                       CASE WHEN VOUCHERINFOs_1.PaymentMode = 'Paid' THEN SUM(VOUCHERINFOs_1.Totalamount) ELSE 0 END AS total_paid,                       CASE WHEN VOUCHERINFOs_1.PaymentMode = 'To Pay' THEN SUM(VOUCHERINFOs_1.Totalamount) ELSE 0 END AS total_pay,                       CASE WHEN VOUCHERINFOs_1.PaymentMode = 'T.B.B.' THEN SUM(VOUCHERINFOs_1.Totalamount) ELSE 0 END AS total_Billed, VOUCHERINFOs_1.Vi_id FROM         VOUCHERINFOs LEFT OUTER JOIN         items RIGHT OUTER JOIN Voucherdets AS Voucherdets_1 ON items.Id = Voucherdets_1.Des_ac_id RIGHT OUTER JOIN  VOUCHERINFOs AS VOUCHERINFOs_1 LEFT OUTER JOIN                      ACCOUNTs AS ACCOUNTs_1 ON VOUCHERINFOs_1.Ac_id2 = ACCOUNTs_1.ac_id LEFT OUTER JOIN  ACCOUNTs ON VOUCHERINFOs_1.Ac_id = ACCOUNTs.ac_id ON Voucherdets_1.Vi_id = VOUCHERINFOs_1.Vi_id RIGHT OUTER JOIN                      Voucherdets ON VOUCHERINFOs_1.Vi_id = Voucherdets.Booking_id ON VOUCHERINFOs.Vi_id = Voucherdets.Vi_id WHERE     (VOUCHERINFOs.Vi_id = '" + gch_vid + "') GROUP BY VOUCHERINFOs_1.Vdate, VOUCHERINFOs_1.Invoiceno, ACCOUNTs.name, ACCOUNTs_1.name, VOUCHERINFOs_1.DeliveryType,   VOUCHERINFOs_1.PaymentMode, VOUCHERINFOs_1.Delivery_adrs, VOUCHERINFOs_1.Vi_id ORDER BY grdate, grno";
           // string str = "SELECT  Stocks.GRDate AS Expr2, Stocks.GRNo AS Invoiceno, ACCOUNTs.name AS consigner, ACCOUNTs_1.name AS consignee,   Stocks.DeliveryType, Stocks.GRType AS PaymentMode, Stocks.TotPkts AS Quantity, Stocks.TotWeight AS Weight, Stocks.FOC, Stocks.Paid,   Stocks.ToPay, Stocks.TBB, Stocks.vid FROM VOUCHERINFOs LEFT OUTER JOIN  VOUCHERINFOs AS VOUCHERINFOs_1 LEFT OUTER JOIN  Voucherdets AS Voucherdets_1 ON VOUCHERINFOs_1.Vi_id = Voucherdets_1.Vi_id RIGHT OUTER JOIN  Voucherdets ON VOUCHERINFOs_1.Vi_id = Voucherdets.Booking_id ON VOUCHERINFOs.Vi_id = Voucherdets.Vi_id CROSS JOIN  ACCOUNTs RIGHT OUTER JOIN  Stocks ON ACCOUNTs.ac_id = Stocks.Consigner_id LEFT OUTER JOIN  ACCOUNTs AS ACCOUNTs_1 ON Stocks.Consignee_id = ACCOUNTs_1.ac_id GROUP BY ACCOUNTs.name, ACCOUNTs_1.name, Stocks.GRDate, Stocks.GRNo, Stocks.DeliveryType, Stocks.GRType, Stocks.TotPkts,   Stocks.TotWeight, Stocks.FOC, Stocks.Paid, Stocks.ToPay, Stocks.TBB, Stocks.vid HAVING (Stocks.vid = '" + gch_vid + "') ";
           // string str = "SELECT  Stocks.GRDate, Stocks.GRNo, ACCOUNTs_1.name AS Consigner, ACCOUNTs.name AS Consignee, Stocks.vid AS vi_id,   Stocks.DeliveryType, Stocks.GRType, SUM( Stocks.TotPkts) AS Quantity, SUM( Stocks.TotWeight) AS Weight, SUM( Stocks.FOC) AS total_foc,   SUM( Stocks.Paid) AS total_paid, SUM( Stocks.ToPay) AS total_pay, SUM( Stocks.TBB) AS total_tbb FROM Stocks LEFT OUTER JOIN  ACCOUNTs ON Stocks.Consignee_id = ACCOUNTs.ac_id LEFT OUTER JOIN  ACCOUNTs AS ACCOUNTs_1 ON Stocks.Consigner_id = ACCOUNTs_1.ac_id GROUP BY Stocks.vid, Stocks.GRNo, Stocks.GRDate, ACCOUNTs_1.name, ACCOUNTs.name, Stocks.DeliveryType, Stocks.GRType HAVING ( Stocks.vid = '"+ gch_vid+"') ORDER BY Stocks.GRDate, Stocks.GRNo ";
            //string str = "SELECT  Stocks.GRDate, Stocks.GRNo, ACCOUNTs_1.name AS Consigner, ACCOUNTs.name AS Consignee, Stocks.vid AS vi_id,   Stocks.DeliveryType, Stocks.GRType, SUM( Stocks.TotPkts) AS Quantity, SUM( Stocks.TotWeight) AS Weight, SUM( Stocks.FOC) AS total_foc,   SUM( Stocks.Paid) AS total_paid, SUM( Stocks.ToPay) AS total_pay, SUM( Stocks.TBB) AS total_tbb, Stocks.GRCharge, Stocks.OthCharge,   SUM( Stocks.Freight) AS Freight, Stocks.ItemName, Stocks.Packing, Stocks.Private, Stocks.Remark, Stocks.Source_id,   Stocks.Destination_id FROM Stocks LEFT OUTER JOIN  ACCOUNTs ON Stocks.Consignee_id = ACCOUNTs.ac_id LEFT OUTER JOIN  ACCOUNTs AS ACCOUNTs_1 ON Stocks.Consigner_id = ACCOUNTs_1.ac_id GROUP BY Stocks.vid, Stocks.GRNo, Stocks.GRDate, ACCOUNTs_1.name, ACCOUNTs.name, Stocks.DeliveryType, Stocks.GRType,   Stocks.GRCharge, Stocks.OthCharge, Stocks.ItemName, Stocks.Packing, Stocks.Private, Stocks.Remark, Stocks.Source_id,   Stocks.Destination_id HAVING ( Stocks.vid = '" + gch_vid + "') ORDER BY Stocks.GRDate, Stocks.GRNo";
            string str = "SELECT  Stocks.GRDate,Stocks.GRNo, ACCOUNTs_1.name AS Consigner, ACCOUNTs.name AS Consignee, Stocks.DeliveryType,   Stocks.GRType, SUM( Stocks.TotPkts) AS Quantity,SUM( Stocks.ActWeight) AS ActWeight ,SUM( Stocks.TotWeight) AS Weight, SUM( Stocks.FOC) AS total_foc, SUM( Stocks.Paid)   AS total_paid, SUM( Stocks.ToPay) AS total_pay, SUM( Stocks.TBB) AS total_tbb, Stocks.GRCharge, Stocks.OthCharge, SUM( Stocks.Freight)   AS Freight, Stocks.ItemName, Stocks.Packing, Stocks.Private, Stocks.Remark, Stocks.Source_id, Stocks.Destination_id,   Stocks.GR_id as Vi_id FROM Stocks LEFT OUTER JOIN  ACCOUNTs ON Stocks.Consignee_id = ACCOUNTs.ac_id LEFT OUTER JOIN  ACCOUNTs AS ACCOUNTs_1 ON Stocks.Consigner_id = ACCOUNTs_1.ac_id GROUP BY Stocks.vid, Stocks.GRNo, Stocks.GRDate, ACCOUNTs_1.name, ACCOUNTs.name, Stocks.DeliveryType, Stocks.GRType,   Stocks.GRCharge, Stocks.OthCharge, Stocks.ItemName, Stocks.Packing, Stocks.Private, Stocks.Remark, Stocks.Source_id,  Stocks.Destination_id, Stocks.GR_id HAVING ( Stocks.vid = '" + gch_vid + "') ORDER BY Stocks.GRDate, Stocks.GRNo";

            DataTable dtfill = new DataTable();
            Database.GetSqlData(str, dtfill);

            for (int m = 0; m < dtfill.Rows.Count; m++)
            {
                ansGridView1.Rows.Add();
                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["vi_id1"].Value = dtfill.Rows[m]["Vi_id"].ToString();
                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["booking_date1"].Value = DateTime.Parse(dtfill.Rows[m]["grdate"].ToString()).ToString(Database.dformat);
                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["grno1"].Value = dtfill.Rows[m]["grno"].ToString();
                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["consigner1"].Value = dtfill.Rows[m]["consigner"].ToString();
                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["consignee1"].Value = dtfill.Rows[m]["consignee"].ToString();
             //   ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["item"].Value = dtfill.Rows[m]["name"].ToString();
                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["source1"].Value = funs.Select_dp_nm(dtfill.Rows[m]["source_id"].ToString());
                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["destination1"].Value = funs.Select_dp_nm(dtfill.Rows[m]["destination_id"].ToString());
                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["delivery1"].Value = dtfill.Rows[m]["DeliveryType"].ToString();
                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["grtype1"].Value = dtfill.Rows[m]["grtype"].ToString();
                //ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["private1"].Value = dtfill.Rows[m]["Private"].ToString();
                //ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["remark1"].Value = dtfill.Rows[m]["Remark"].ToString();
                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["qty1"].Value = funs.IndianCurr(double.Parse(dtfill.Rows[m]["Quantity"].ToString()));
                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["Actweight"].Value = double.Parse(dtfill.Rows[m]["Actweight"].ToString());
                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["wt1"].Value = double.Parse(dtfill.Rows[m]["weight"].ToString());
                //ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["amt1"].Value = dtfill.Rows[m]["total_amount"].ToString();
                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["freight"].Value = dtfill.Rows[m]["Freight"].ToString();
               
                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["itemname"].Value = dtfill.Rows[m]["itemname"].ToString();
                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["packing"].Value = dtfill.Rows[m]["packing"].ToString();
                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["private"].Value = dtfill.Rows[m]["private"].ToString();
                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["remark"].Value = dtfill.Rows[m]["remark"].ToString();

                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["grcharge"].Value = funs.IndianCurr(double.Parse(dtfill.Rows[m]["grcharge"].ToString()));
                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["othcharge"].Value = funs.IndianCurr(double.Parse(dtfill.Rows[m]["othcharge"].ToString()));

                    ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["pay1"].Value = funs.IndianCurr(double.Parse(dtfill.Rows[m]["total_pay"].ToString()));
                    ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["foc1"].Value = funs.IndianCurr(double.Parse(dtfill.Rows[m]["total_foc"].ToString()));
                    ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["paid1"].Value = funs.IndianCurr(double.Parse(dtfill.Rows[m]["total_paid"].ToString()));
                    ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["billed1"].Value = funs.IndianCurr(double.Parse(dtfill.Rows[m]["total_tbb"].ToString()));
               
            }

            weightCalc();
        }

        private void frm_unloading_Load(object sender, EventArgs e)
        {
            SideFill();
        }

        private bool validation()
        {

            if (txtTruckNo.Text == "")
            {
                MessageBox.Show("Enter Challan No");
                txtTruckNo.Focus();

                return false;
            }
            if (ansGridView1.Rows.Count == 0)
            {
                MessageBox.Show("Enter Valid Challan No");
                return false;
            }

            if (funs.GenerateVno(vtid, dateTimePicker1.Value.ToString("dd-MMM-yyyy"), vid) == 0 && vno == 0)
            {
                MessageBox.Show("Voucher Number can't be created on this date.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            //if (gStr == "0")
            //{

            //    for (int i = 0; i < ansGridView1.Rows.Count; i++)
            //    {
            //        if (Database.GetScalarInt("select count(*) from Stocks where step='Step2' and Gr_id='" + ansGridView1.Rows[i].Cells["vi_id1"].Value.ToString() + "' and Quantity=1") != 0)
            //        {
            //            MessageBox.Show("GRNo " + ansGridView1.Rows[i].Cells["grno1"].Value.ToString() + " already Exists with Unloading.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //            return false;
            //        }
            //    }
            //}
            int numtype = funs.chkNumType(vtid);
            if (vid != "")
            {

            }
            else if (numtype != 1)
            {
                vid = Database.GetScalarText("Select Vi_id from voucherinfos where Vt_id='" + vtid + "' and Vnumber=" + vno + " and Vdate=" + access_sql.Hash + dateTimePicker1.Value.Date.ToString(Database.dformat) + access_sql.Hash);
                if (vid == "")
                {
                    vid = "0";
                }
            }
            else
            {
                if (vid == "0")
                {
                    string tempvid = "";
                    tempvid = Database.GetScalarText("Select Vi_id from voucherinfos where Vt_id='" + vtid + "' and Vnumber=" + vno);
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
            SetVno();

            if (vno == 0)
            {
                vno = funs.GenerateVno(vtid, dateTimePicker1.Value.ToString("dd-MMM-yyyy"), vid);
            }
            string invoiceno = vno.ToString();

            prefix = Database.GetScalarText("Select prefix from Location where LocationId='" + Database.LocationId + "'");

          
            dtVoucherinfo.Rows[0]["Invoiceno"] = prefix + invoiceno.PadLeft(padding, '0') + postfix;
            dtVoucherinfo.Rows[0]["Vdate"] = dateTimePicker1.Value.Date;
            dtVoucherinfo.Rows[0]["Vnumber"] = label1.Text;
            dtVoucherinfo.Rows[0]["RoffChanged"] = RoffChanged;
            dtVoucherinfo.Rows[0]["Tdtype"] = false;
            dtVoucherinfo.Rows[0]["Vt_id"] = vtid;
            dtVoucherinfo.Rows[0]["iscancel"] = iscancel;
            dtVoucherinfo.Rows[0]["Narr"] = textBox1.Text;
            dtVoucherinfo.Rows[0]["totalamount"] = 0;
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

            dtVoucherinfo.Rows[0]["Challan_id"] = gch_vid;

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

            DataTable dtstocks = new DataTable("stocks");
            Database.GetSqlData("Select * from stocks where Vid='" + vid + "'", dtstocks);
            for (int j = 0; j < dtstocks.Rows.Count; j++)
            {
                dtstocks.Rows[j].Delete();
            }
            Database.SaveData(dtstocks);
        
            if (iscancel == false)
            {

                for (int i = 0; i < ansGridView1.Rows.Count; i++)
                {
                    //string bookingid = ansGridView1.Rows[i].Cells["vi_id1"].Value.ToString();

                    dtstocks.Rows.Add();

                    dtstocks.Rows[dtstocks.Rows.Count - 1]["Vid"] = vid;

                    dtstocks.Rows[dtstocks.Rows.Count - 1]["GR_id"] = ansGridView1.Rows[i].Cells["vi_id1"].Value.ToString();
                    dtstocks.Rows[dtstocks.Rows.Count - 1]["Quantity"] = 1;

                    dtstocks.Rows[dtstocks.Rows.Count - 1]["Step"] = "Step2";

                    dtstocks.Rows[dtstocks.Rows.Count - 1]["Godown_id"] = Database.LocationId;
                    string aliasname = Database.GetScalarText("Select Aliasname from vouchertypes where vt_id=" + vtid);
                    dtstocks.Rows[dtstocks.Rows.Count - 1]["Narration"] = aliasname + " At " + funs.Select_dp_nm(loca_dp_id);


                    dtstocks.Rows[dtstocks.Rows.Count - 1]["GRNo"] = ansGridView1.Rows[i].Cells["grno1"].Value.ToString();
                    dtstocks.Rows[dtstocks.Rows.Count - 1]["GRDate"] = DateTime.Parse(ansGridView1.Rows[i].Cells["booking_date1"].Value.ToString());

                    dtstocks.Rows[dtstocks.Rows.Count - 1]["Consigner_id"] = funs.Select_ac_id(ansGridView1.Rows[i].Cells["consigner1"].Value.ToString());
                    dtstocks.Rows[dtstocks.Rows.Count - 1]["Consignee_id"] = funs.Select_ac_id(ansGridView1.Rows[i].Cells["consignee1"].Value.ToString());


                    dtstocks.Rows[dtstocks.Rows.Count - 1]["Source_id"] = funs.Select_dp_id(ansGridView1.Rows[i].Cells["source1"].Value.ToString());
                    dtstocks.Rows[dtstocks.Rows.Count - 1]["Destination_id"] = funs.Select_dp_id(ansGridView1.Rows[i].Cells["destination1"].Value.ToString());

                    dtstocks.Rows[dtstocks.Rows.Count - 1]["ToPay"] = double.Parse(ansGridView1.Rows[i].Cells["pay1"].Value.ToString());
                    dtstocks.Rows[dtstocks.Rows.Count - 1]["TBB"] = double.Parse(ansGridView1.Rows[i].Cells["billed1"].Value.ToString());

                    dtstocks.Rows[dtstocks.Rows.Count - 1]["Paid"] = double.Parse(ansGridView1.Rows[i].Cells["Paid1"].Value.ToString());
                    dtstocks.Rows[dtstocks.Rows.Count - 1]["FOC"] = double.Parse(ansGridView1.Rows[i].Cells["Foc1"].Value.ToString());



                    dtstocks.Rows[dtstocks.Rows.Count - 1]["Totpkts"] = double.Parse(ansGridView1.Rows[i].Cells["qty1"].Value.ToString());
                    dtstocks.Rows[dtstocks.Rows.Count - 1]["Actweight"] = double.Parse(ansGridView1.Rows[i].Cells["actweight"].Value.ToString());
                    dtstocks.Rows[dtstocks.Rows.Count - 1]["totweight"] = double.Parse(ansGridView1.Rows[i].Cells["wt1"].Value.ToString());

                    dtstocks.Rows[dtstocks.Rows.Count - 1]["deliverytype"] = ansGridView1.Rows[i].Cells["delivery1"].Value.ToString();
                    dtstocks.Rows[dtstocks.Rows.Count - 1]["grtype"] = ansGridView1.Rows[i].Cells["grtype1"].Value.ToString();

                    dtstocks.Rows[dtstocks.Rows.Count - 1]["itemname"] = ansGridView1.Rows[i].Cells["itemname"].Value.ToString();
                    dtstocks.Rows[dtstocks.Rows.Count - 1]["packing"] = ansGridView1.Rows[i].Cells["packing"].Value.ToString();
                  

                    dtstocks.Rows[dtstocks.Rows.Count - 1]["private"] = ansGridView1.Rows[i].Cells["private"].Value.ToString();
                    dtstocks.Rows[dtstocks.Rows.Count - 1]["remark"] = ansGridView1.Rows[i].Cells["remark"].Value.ToString();

                    dtstocks.Rows[dtstocks.Rows.Count - 1]["grcharge"] = double.Parse(ansGridView1.Rows[i].Cells["grcharge"].Value.ToString());
                    dtstocks.Rows[dtstocks.Rows.Count - 1]["othcharge"] = double.Parse(ansGridView1.Rows[i].Cells["othcharge"].Value.ToString());

                    dtstocks.Rows[dtstocks.Rows.Count - 1]["freight"] = double.Parse(ansGridView1.Rows[i].Cells["freight"].Value.ToString());










                }
                Database.SaveData(dtstocks);

            }


            //if (vid == "0")
            //{
            //    vid = dtVoucherinfo.Rows[0]["Vi_id"].ToString();
            //}

            //dtVoucherDet = new DataTable("Voucherdets");
            //Database.GetSqlData("Select * from Voucherdets where Vi_id='" + vid + "'", dtVoucherDet);

            //for (int j = 0; j < dtVoucherDet.Rows.Count; j++)
            //{
            //    dtVoucherDet.Rows[j].Delete();
            //}
            //Database.SaveData(dtVoucherDet);

            //dtVoucherDet = new DataTable("Voucherdets");
            //Database.GetSqlData("Select * from Voucherdets where Vi_id='" + vid + "'", dtVoucherDet);

            //int Nid2 = 1;
            //DataTable dtidvd = new DataTable();
            //Database.GetSqlData("select max(Nid) as Nid from Voucherdets where locationid='" + Database.LocationId + "'", dtidvd);
            //if (dtidvd.Rows[0][0].ToString() != "")
            //{
            //    Nid2 = int.Parse(dtidvd.Rows[0][0].ToString()) + 1;
            //}

            //for (int i = 0; i < ansGridView1.Rows.Count; i++)
            //{
            //    dtVoucherDet.Rows.Add();
            //    dtVoucherDet.Rows[i]["Nid"] = Nid2;
            //    dtVoucherDet.Rows[i]["LocationId"] = Database.LocationId;
            //    dtVoucherDet.Rows[i]["vd_id"] = Database.LocationId + dtVoucherDet.Rows[i]["nid"].ToString();
            //    dtVoucherDet.Rows[i]["Vi_id"] = vid;
            //    dtVoucherDet.Rows[i]["Itemsr"] = i + 1;
            //    //dtVoucherDet.Rows[i]["Des_ac_id"] = "0";
            //    //dtVoucherDet.Rows[i]["Challan_id"] = ansGridView1.Rows[i].Cells["vi_id"].Value.ToString();
            //    dtVoucherDet.Rows[i]["remarkreq"] = false;
            //    dtVoucherDet.Rows[i]["create_date"] = create_date;
            //    dtVoucherDet.Rows[i]["modify_date"] = System.DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
            //    dtVoucherDet.Rows[i]["multiplier"] = 1;
            //    dtVoucherDet.Rows[i]["Amount"] = 0;
            //    Nid2++;
            //}

            //Database.SaveData(dtVoucherDet);

            //if (print == true && Feature.Available("Printing") == "Dos")
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
            //    if (Feature.Available("Printing") == "Dos")
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
            //    LoadData("0", "Unloading");
            //}
            //else
            //{
            //    this.Close();
            //    this.Dispose();
            //}
        }
        private void view()
        {
            
                if (Feature.Available("Printing") == "Dos")
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
        private void clear()
        {
            if (vid == "0")
            {
                LoadData("0", "Unloading");
            }
            else
            {
                this.Close();
                this.Dispose();
            }
        }
        private void Print()
        {
            if (Feature.Available("Printing") == "Dos")
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
        private void txtTruckNo_TextChanged(object sender, EventArgs e)
        {
           // fillGrid("0");
        }

        private void txtTruckNo_Enter(object sender, EventArgs e)
        {
            Database.setFocus(txtTruckNo);
        }

        private void txtTruckNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            //string str = "SELECT VOUCHERTYPEs.Short + ' ' + CONVERT(nvarchar, VOUCHERINFOs_1.Vdate, 112) + ' ' + CAST(VOUCHERINFOs_1.Vnumber AS nvarchar(10)) AS DocNumber, CONVERT(nvarchar, VOUCHERINFOs_1.Vdate, 106) AS vdate, VOUCHERINFOs_1.Invoiceno, Gaddis.Gaddi_name, ACCOUNTs.name AS driver, DeliveryPoints.Name AS source, DeliveryPoints_1.Name AS destination, VOUCHERINFOs_1.Grno AS lessdc, VOUCHERINFOs_1.Transport2 AS lorryfreight, VOUCHERINFOs_1.Transport5 AS ap, VOUCHERINFOs_1.Transport6 AS bf, VOUCHERINFOs_1.Transport3 AS fp, VOUCHERINFOs_1.DeliveryAt AS cc, VOUCHERINFOs_1.DD, VOUCHERINFOs_1.Transport4 AS paid, VOUCHERINFOs_1.DR, Location.nick_name FROM VOUCHERTYPEs RIGHT OUTER JOIN VOUCHERINFOs AS VOUCHERINFOs_1 LEFT OUTER JOIN Location ON VOUCHERINFOs_1.LocationId = Location.LocationId LEFT OUTER JOIN DeliveryPoints AS DeliveryPoints_1 ON VOUCHERINFOs_1.SId = DeliveryPoints_1.DPId LEFT OUTER JOIN DeliveryPoints ON VOUCHERINFOs_1.Consigner_id = DeliveryPoints.DPId LEFT OUTER JOIN ACCOUNTs ON VOUCHERINFOs_1.Driver_name = ACCOUNTs.ac_id LEFT OUTER JOIN Gaddis ON VOUCHERINFOs_1.Gaddi_id = Gaddis.Gaddi_id ON VOUCHERTYPEs.Vt_id = VOUCHERINFOs_1.Vt_id LEFT OUTER JOIN VOUCHERINFOs ON VOUCHERINFOs_1.Vi_id = VOUCHERINFOs.Challan_id WHERE (VOUCHERINFOs_1.Vt_id = 63) AND (VOUCHERINFOs.Challan_id IS NULL) ORDER BY DocNumber";
            string str = "SELECT VOUCHERINFOs_1.Vi_id, VOUCHERINFOs_1.Invoiceno as Challan_no, VOUCHERTYPEs.Short + ' ' + CONVERT(nvarchar, VOUCHERINFOs_1.Vdate, 112) + ' ' + CAST(VOUCHERINFOs_1.Vnumber AS nvarchar(10)) AS DocNumber, CONVERT(nvarchar, VOUCHERINFOs_1.Vdate, 106) AS vdate, Gaddis.Gaddi_name, ACCOUNTs.name AS driver, DeliveryPoints.Name AS source, DeliveryPoints_1.Name AS destination, Location.nick_name as Booked_By_Loc FROM VOUCHERTYPEs RIGHT OUTER JOIN VOUCHERINFOs AS VOUCHERINFOs_1 LEFT OUTER JOIN Location ON VOUCHERINFOs_1.LocationId = Location.LocationId LEFT OUTER JOIN DeliveryPoints AS DeliveryPoints_1 ON VOUCHERINFOs_1.SId = DeliveryPoints_1.DPId LEFT OUTER JOIN DeliveryPoints ON VOUCHERINFOs_1.Consigner_id = DeliveryPoints.DPId LEFT OUTER JOIN ACCOUNTs ON VOUCHERINFOs_1.Driver_name = ACCOUNTs.ac_id LEFT OUTER JOIN Gaddis ON VOUCHERINFOs_1.Gaddi_id = Gaddis.Gaddi_id ON VOUCHERTYPEs.Vt_id = VOUCHERINFOs_1.Vt_id LEFT OUTER JOIN VOUCHERINFOs ON VOUCHERINFOs_1.Vi_id = VOUCHERINFOs.Challan_id WHERE (dbo.VOUCHERTYPEs.Type = 'Stock Transfer') And (VOUCHERINFOs.Challan_id IS NULL)  AND (VOUCHERINFOs_1.unloadingpoint_id = '" + Database.LocationId + "') AND   (VOUCHERINFOs_1.Iscancel = 0) ORDER BY DocNumber";
           // txtTruckNo.Text = SelectCombo.ComboKeypress(this, e.KeyChar, str, "", 10);
          //  string vid = Database.GetScalarText("Select Vi_id from Voucherinfos where Invoiceno='" + txtTruckNo.Text + "' and vt_id=63");
            string vid = SelectCombo.ComboKeypress(this, e.KeyChar, str, "", 10);


            txtTruckNo.Text = Database.GetScalarText("Select Invoiceno from Voucherinfos where vi_id='" + vid + "'");

            fillGrid(vid);


            //fillGrid(IsDocumentNumber(txtTruckNo.Text));
        }

        private void txtTruckNo_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(txtTruckNo);
        }

        private string IsDocumentNumber(String str)
        {
            return Database.GetScalarText("SELECT DISTINCT VOUCHERINFOs.Vi_id, " + access_sql.Docnumber + " AS DocNumber FROM (VOUCHERINFOs LEFT JOIN ACCOUNTs ON VOUCHERINFOs.Ac_id = ACCOUNTs.Ac_id) LEFT JOIN VOUCHERTYPEs ON VOUCHERINFOs.Vt_id = VOUCHERTYPEs.Vt_id WHERE (((VOUCHERINFOs.Vt_id)=[VOUCHERTYPEs].[Vt_id]) AND (" + access_sql.Docnumber + "='" + str + "'))");
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            SetVno();
        }

        private void txtTruckNo_TextChanged_1(object sender, EventArgs e)
        {

        }
    }
}
