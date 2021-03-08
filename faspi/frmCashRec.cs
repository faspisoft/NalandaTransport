using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;

namespace faspi
{
    public partial class frmCashRec : Form
    {        
        DataTable dtVoucherInfo;
        DataTable dtVoucheractotal;
        DataTable dtJournal;
        DateTime dtm;
        public String recpay;
        public String cmdnm;
        Boolean generateVno = false;
        int vtid, vno = 0, cashac_id;
        string cmbVouTyp = "";
        string vid = "";
        DateTime chkDt = new DateTime();        
        String strCombo;
        Boolean f12used = false;
        string Prelocationid = "";

        public frmCashRec()
        {
            InitializeComponent();
            dateTimePicker1.MinDate = Database.stDate;
            dateTimePicker1.MaxDate = Database.ldate;
            dateTimePicker1.CustomFormat = Database.dformat;
            dateTimePicker1.Value = Database.ldate;
        }

        private void frmCashRec_Load(object sender, EventArgs e)
        {
            ansGridView1.Columns["Amount"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            SideFill();
        }

        private void frmCashRec_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
            {
                if (validate() == true)
                {
                    if (Database.utype == "Admin")
                    {
                        SaveMethod(false);
                    }
                    else if (vid == "0")
                    {
                        SaveMethod(false);
                    }
                }
            }
            else if (e.Control && e.KeyCode == Keys.P)
            {
                if (validate() == true)
                {
                    if (Database.utype == "Admin")
                    {
                        SaveMethod(true);
                    }
                    else if (vid == "0")
                    {
                        SaveMethod(true);
                    }
                }
            }
            else if (e.Control && e.KeyCode == Keys.D)
            {
                if (vid != "0")
                {
                    if (Database.utype == "Admin")
                    {
                        if (MessageBox.Show("Are You Sure To Delete This Voucher", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Error, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                        {
                           // Database.BeginTran();
                            if (Feature.Available("Freeze Transaction") == "No")
                            {
                                delete();
                            }
                            else
                            {
                                DataTable dtfrz = new DataTable();
                                Database.GetSqlData("Select selected_value from Firmsetups where Features='Freeze Transaction'", dtfrz);
                                if (dateTimePicker1.Value > DateTime.Parse(dtfrz.Rows[0][0].ToString()))
                                {
                                    delete();
                                }
                                else
                                {
                                    MessageBox.Show("Your Voucher is Freezed");
                                }
                            }
                          //  Database.CommitTran();
                            this.Close();
                            this.Dispose();
                        }
                    }
                }
            }

            else if (e.KeyCode == Keys.Escape)
            {
                if (label4.Text != "0")
                {
                    DialogResult chk = MessageBox.Show("Are u sure?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                    if (chk == DialogResult.No)
                    {
                        e.Handled = false;
                    }
                    else
                    {
                        this.Close();
                        this.Dispose();
                    }
                }
                else
                {
                    this.Close();
                    this.Dispose();
                }
            }
            //else if (e.KeyCode == Keys.F12)
            //{
            //    InputBox box = new InputBox("Enter Administrative password", "", true);
            //    box.ShowDialog(this);
            //   // MessageBox.Show(box.outStr);
            //    String pass = box.outStr;
            //    if (pass.ToLower() == "admin")
            //    {
            //        box = new InputBox("Enter Voucher Number", "", false);
            //        box.ShowDialog();
            //        vno = int.Parse(box.outStr);
            //        label2.Text = vno.ToString();
            //        generateVno = true;
            //    }
            //    else
            //    {
            //        MessageBox.Show("Invalid Password");
            //    }
            //}
        }

        private void DisplayData()
        {
            dtVoucherInfo = new DataTable("Voucherinfos");

            Database.GetSqlData("select * from voucherinfos where vi_id='" + vid + "'", dtVoucherInfo);

            if (dtVoucherInfo.Rows.Count > 0)
            {
                textBox1.Text = funs.Select_ac_nm(dtVoucherInfo.Rows[0]["Ac_id"].ToString());
                textBox2.Text = dtVoucherInfo.Rows[0]["Narr"].ToString();
                textBox3.Enabled = false;
                textBox3.Text = funs.Select_vt_nm(int.Parse(dtVoucherInfo.Rows[0]["Vt_id"].ToString()));
                vtid = int.Parse(dtVoucherInfo.Rows[0]["Vt_id"].ToString());
                dateTimePicker1.Value = DateTime.Parse(dtVoucherInfo.Rows[0]["Vdate"].ToString());
                vno = int.Parse(dtVoucherInfo.Rows[0]["Vnumber"].ToString());
                label2.Text = vno.ToString();
                chkDt = DateTime.Parse(dtVoucherInfo.Rows[0]["Vdate"].ToString());
                Prelocationid = dtVoucherInfo.Rows[0]["Locationid"].ToString();
            }

            dtVoucheractotal = new DataTable("Voucheractotals");
            Database.GetSqlData("select * from voucheractotals where vi_id='" + vid + "' order by Srno", dtVoucheractotal);

            for (int i = 0; i < dtVoucheractotal.Rows.Count; i++)
            {
                ansGridView1.Rows.Add();
                ansGridView1.Rows[i].Cells["sno"].Value = dtVoucheractotal.Rows[i]["Srno"];
                ansGridView1.Rows[i].Cells["acc"].Value = funs.Select_ac_nm(dtVoucheractotal.Rows[i]["Accid"].ToString());
                ansGridView1.Rows[i].Cells["instrumentno"].Value = dtVoucheractotal.Rows[i]["Chkno"];
                if (dtVoucheractotal.Rows[i]["Cdate"].ToString() == "")
                {
                    dtVoucheractotal.Rows[i]["Cdate"] = dtVoucherInfo.Rows[0]["Vdate"].ToString();
                }
                ansGridView1.Rows[i].Cells["instrumentdt"].Value = DateTime.Parse(dtVoucheractotal.Rows[i]["Cdate"].ToString()).ToString("dd / MM / yyyy");
                ansGridView1.Rows[i].Cells["Amount"].Value = dtVoucheractotal.Rows[i]["Amount"];
            }

            dtJournal = new DataTable("Journals");
            Database.GetSqlData("select * from Journals where vi_id='" + vid + "'", dtJournal);

            foreach (DataGridViewColumn column in ansGridView1.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            ansGridView1.Columns["Amount"].CellTemplate.ValueType = typeof(double);
            Database.CloseConnection();
        }

        public void LoadData(String str, String frmCaption)
        {
            vid = str;
            this.Text = frmCaption;
            vtid = funs.Select_vt_id(textBox3.Text);
            Display();
            DisplayData();
            SetVno();
            calcTot();
        }

        private void delete()
        {
            try
            {
                DataTable dtTemp = new DataTable("voucherinfos");
                Database.GetSqlData("select * from voucherinfos where vi_id='" + vid + "'", dtTemp);
                
                for (int i = 0; i < dtTemp.Rows.Count; i++)
                {
                    dtTemp.Rows[i].Delete();
                }
                Database.SaveData(dtTemp);

                dtTemp = new DataTable("voucheractotals");
                Database.GetSqlData("select * from voucheractotals where vi_id='" + vid + "'", dtTemp);
                
                for (int i = 0; i < dtTemp.Rows.Count; i++)
                {
                    dtTemp.Rows[i].Delete();
                }
                Database.SaveData(dtTemp);

                dtTemp = new DataTable("journals");
                Database.GetSqlData("select * from journals where vi_id='" + vid + "'", dtTemp);
                
                for (int i = 0; i < dtTemp.Rows.Count; i++)
                {
                    dtTemp.Rows[i].Delete();
                }
                Database.SaveData(dtTemp);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SaveMethod(bool prnt)
        {
            try
            {
                Database.BeginTran();
                if (Feature.Available("Freeze Transaction") == "No")
                {
                    if (save(prnt) == true)
                    {
                        if (vid != "0")
                        {
                            this.Close();
                            this.Dispose();
                        }
                        else
                        {
                            clear();
                        }
                    }
                }
                else
                {
                    DataTable dtfrz = new DataTable();
                    Database.GetSqlData("Select selected_value from Firmsetups where Features='Freeze Transaction'", dtfrz);
                    if (dateTimePicker1.Value > DateTime.Parse(dtfrz.Rows[0][0].ToString()))
                    {
                        if (save(prnt) == true)
                        {
                            if (vid != "0")
                            {
                                this.Close();
                                this.Dispose();
                            }
                            else
                            {
                                clear();
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Your Voucher is Freezed");
                    }
                }
                Database.CommitTran();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Voucher Not Saved, Due To An Exception", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Database.RollbackTran();
            }
        }

        private bool save(bool print)
        {
            ansGridView1.EndEdit();
            string actname = "";
            string cashac_id = funs.Select_ac_id(textBox1.Text);
            int conn_id;
            String narr = "";

            if (textBox2.Text == "")
            {
                int act_id = funs.Select_AccTypeid(textBox1.Text);
                actname = funs.Select_act_nm(act_id);

                if (actname == "CASH-IN-HAND")
                {
                    if (recpay == "Payment")
                    {
                        narr = "Being Cash Paid";
                    }
                    else if (recpay == "Receipt")
                    {
                        narr = "Being Cash Receipt";
                    }
                    else if (recpay == "Contra")
                    {
                        narr = "Contra Voucher";
                    }
                }
                else
                {
                    if (recpay == "Payment")
                    {
                        narr = "Being Payment By Bank";
                    }
                    else if (recpay == "Receipt")
                    {
                        narr = "Cheque/D.D. Deposit";
                    }
                    else if (recpay == "Contra")
                    {
                        narr = "Contra Voucher";
                    }
                }
            }
            else
            {
                narr = textBox2.Text;
            }

            conn_id = 0;
            SetVno();
            String[] dtmCheck = { "", "", "" };
            if (vno == 0)
            {
                vno = funs.GenerateVno(vtid, dateTimePicker1.Value.ToString("dd-MMM-yyyy"), vid);
                label2.Text = vno.ToString();
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
            
            string prefix = "";
            string postfix = "";
            int padding = 0;

            prefix = Database.GetScalarText("Select prefix from Vouchertypes where vt_id=" + vtid);
            postfix = Database.GetScalarText("Select postfix from Vouchertypes where vt_id=" + vtid);
            padding = Database.GetScalarInt("Select padding from Vouchertypes where vt_id=" + vtid);
            string invoiceno = vno.ToString();

            dtVoucherInfo.Rows[0]["Invoiceno"] = prefix + invoiceno.PadLeft(padding, '0') + postfix;
            dtVoucherInfo.Rows[0]["Invoiceno"] = invoiceno;
            dtVoucherInfo.Rows[0]["user_id"] = Database.user_id;
            dtVoucherInfo.Rows[0]["Vt_id"] = vtid;
            dtVoucherInfo.Rows[0]["Vnumber"] = vno;
            dtVoucherInfo.Rows[0]["Ac_id2"] = null;
            dtVoucherInfo.Rows[0]["ac_id"] = cashac_id;
            dtVoucherInfo.Rows[0]["Vdate"] = dateTimePicker1.Value.Date.ToString("dd-MMM-yyyy");
            dtVoucherInfo.Rows[0]["Narr"] = narr;
            dtVoucherInfo.Rows[0]["Totalamount"] = label4.Text;
            dtVoucherInfo.Rows[0]["Conn_id"] = conn_id;
            dtVoucherInfo.Rows[0]["Roff"] = 0;
            dtVoucherInfo.Rows[0]["FormC"] = false;
            dtVoucherInfo.Rows[0]["DR"] = 0;
            dtVoucherInfo.Rows[0]["DD"] = 0;
            dtVoucherInfo.Rows[0]["TaxChanged"] = false;
            dtVoucherInfo.Rows[0]["Tdtype"] = false;
            dtVoucherInfo.Rows[0]["RoffChanged"] = false;
            dtVoucherInfo.Rows[0]["As_Per"] = "";
            dtVoucherInfo.Rows[0]["PaymentMode"] = "";
            dtVoucherInfo.Rows[0]["DeliveryType"] = "";
            dtVoucherInfo.Rows[0]["Delivery_adrs"] = "";
            dtVoucherInfo.Rows[0]["SId"] = null;
            dtVoucherInfo.Rows[0]["Consigner_id"] = null;
            dtVoucherInfo.Rows[0]["Db_id"] = 0;
            dtVoucherInfo.Rows[0]["iscancel"] = false;
            if (vid == "0")
            {
                dtVoucherInfo.Rows[0]["CreTime"] = System.DateTime.Now.ToString("HH:mm:ss");
                dtVoucherInfo.Rows[0]["create_date"] = System.DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                dtVoucherInfo.Rows[0]["user_id"] = Database.user_id;
            }
            dtVoucherInfo.Rows[0]["modify_date"] = System.DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
            dtVoucherInfo.Rows[0]["ModTime"] = System.DateTime.Now.ToString("HH:mm:ss");
            dtVoucherInfo.Rows[0]["modifyby_id"] = Database.user_id;


            if (recpay == "Payment" || recpay == "Contra")
            {
                dtVoucherInfo.Rows[0]["dr_ac_id"] = funs.Select_ac_id(ansGridView1.Rows[0].Cells["acc"].Value.ToString());
                dtVoucherInfo.Rows[0]["cr_ac_id"] = cashac_id;

            }
            else
            {
                dtVoucherInfo.Rows[0]["dr_ac_id"] = cashac_id;
                dtVoucherInfo.Rows[0]["cr_ac_id"] = funs.Select_ac_id(ansGridView1.Rows[0].Cells["acc"].Value.ToString());

            }

            Database.SaveData(dtVoucherInfo);

            if (vid == "0")
            {
                vid = dtVoucherInfo.Rows[0]["Vi_id"].ToString();
            }

            DataTable dtTemp = new DataTable("Voucheractotals");
            Database.GetSqlData("select * from Voucheractotals where vi_id='" + vid + "'", dtTemp);
            for (int i = 0; i < dtTemp.Rows.Count; i++)
            {
                dtTemp.Rows[i].Delete();
            }
            Database.SaveData(dtTemp);

            dtVoucheractotal = new DataTable("Voucheractotals");
            Database.GetSqlData("select * from Voucheractotals where vi_id='" + vid + "'", dtVoucheractotal);
            
            //Voucheractotal

            for (int i = 0; i < ansGridView1.Rows.Count - 1; i++)
            {
                if (ansGridView1.Rows[i].Cells["instrumentdt"].Value == null)
                {
                    dtmCheck[0] = dateTimePicker1.Value.Day.ToString();
                    dtmCheck[1] = dateTimePicker1.Value.Month.ToString();
                    dtmCheck[2] = dateTimePicker1.Value.Year.ToString();
                }
                else if (ansGridView1.Rows[i].Cells["instrumentdt"].Value.ToString().Split('/').Length == 3)
                {
                    dtmCheck = ansGridView1.Rows[i].Cells["instrumentdt"].Value.ToString().Split('/');
                }
                else if (ansGridView1.Rows[i].Cells["instrumentdt"].Value.ToString().Split('-').Length == 3)
                {
                    dtmCheck = ansGridView1.Rows[i].Cells["instrumentdt"].Value.ToString().Split('-');
                }
                else if (ansGridView1.Rows[i].Cells["instrumentdt"].Value.ToString().Split('.').Length == 3)
                {
                    dtmCheck = ansGridView1.Rows[i].Cells["instrumentdt"].Value.ToString().Split('.');
                }

                dtm = new DateTime(int.Parse(dtmCheck[2]), int.Parse(dtmCheck[1]), int.Parse(dtmCheck[0]));
                if (dtmCheck[2].Length == 2)
                {

                    dtmCheck[2] = "20" + dtmCheck[2];
                    dtm = new DateTime(int.Parse(dtmCheck[2]), int.Parse(dtmCheck[1]), int.Parse(dtmCheck[0]));
                }
                string chkno;
                if (ansGridView1.Rows[i].Cells["instrumentno"].Value != null)
                {
                    chkno = ansGridView1.Rows[i].Cells["instrumentno"].Value.ToString();
                }
                else
                {
                    chkno = "";
                }

                dtVoucheractotal.Rows.Add();
                dtVoucheractotal.Rows[i]["vi_id"] = vid;
                dtVoucheractotal.Rows[i]["Srno"] = ansGridView1.Rows[i].Cells["sno"].Value.ToString();
                dtVoucheractotal.Rows[i]["Chkno"] = chkno;
                dtVoucheractotal.Rows[i]["Cdate"] = dtm.ToString("dd-MMM-yyyy");
                dtVoucheractotal.Rows[i]["Accid"] = funs.Select_ac_id(ansGridView1.Rows[i].Cells["acc"].Value.ToString());
                dtVoucheractotal.Rows[i]["Amount"] = ansGridView1.Rows[i].Cells["Amount"].Value;
            }
            Database.SaveData(dtVoucheractotal);

            //Journal
            
            dtTemp = new DataTable("Journals");
            Database.GetSqlData("Select * from Journals where Vi_id='" + vid + "'", dtTemp);
            for (int j = 0; j < dtTemp.Rows.Count; j++)
            {
                dtTemp.Rows[j].Delete();
            }
            Database.SaveData(dtTemp);

            DataTable dtJournal = new DataTable("Journals");
            Database.GetSqlData("Select * from Journals where Vi_id='" + vid + "'", dtJournal);
            for (int i = 0; i < ansGridView1.Rows.Count - 1; i++)
            {
                double amount = double.Parse(ansGridView1.Rows[i].Cells["Amount"].Value.ToString());
                string jnarr = narr;



                //textbox
                dtJournal.Rows.Add();
                dtJournal.Rows[dtJournal.Rows.Count - 1]["vdate"] = dateTimePicker1.Value.ToString("dd-MMM-yyyy");
                dtJournal.Rows[dtJournal.Rows.Count - 1]["Vi_id"] = vid;
                dtJournal.Rows[dtJournal.Rows.Count - 1]["Ac_id"] = cashac_id;
                dtJournal.Rows[dtJournal.Rows.Count - 1]["Opp_acid"] = funs.Select_ac_id(ansGridView1.Rows[i].Cells["acc"].Value.ToString()); ;
                dtJournal.Rows[dtJournal.Rows.Count - 1]["Narr"] = jnarr;
                dtJournal.Rows[dtJournal.Rows.Count - 1]["Narr2"] = jnarr;
                dtJournal.Rows[dtJournal.Rows.Count - 1]["Sno"] = i + 1;
                dtJournal.Rows[dtJournal.Rows.Count - 1]["LocationId"] = Database.LocationId;
                if (ansGridView1.Rows[i].Cells["instrumentno"].Value == null)
                {
                    ansGridView1.Rows[i].Cells["instrumentno"].Value = "";
                }
                dtJournal.Rows[dtJournal.Rows.Count - 1]["Reffno"] = ansGridView1.Rows[i].Cells["instrumentno"].Value.ToString();

                if (recpay == "Receipt")
                {
                    dtJournal.Rows[dtJournal.Rows.Count - 1]["Amount"] = amount;
                }
                else if (recpay == "Payment")
                {
                    dtJournal.Rows[dtJournal.Rows.Count - 1]["Amount"] = -1 * amount;
                }
                else if (recpay == "Contra")
                {

                    dtJournal.Rows[dtJournal.Rows.Count - 1]["Amount"] = -1 * amount;
                }


                //grid
                dtJournal.Rows.Add();
                dtJournal.Rows[dtJournal.Rows.Count - 1]["vdate"] = dateTimePicker1.Value.ToString("dd-MMM-yyyy");
                dtJournal.Rows[dtJournal.Rows.Count - 1]["Vi_id"] = vid;
                dtJournal.Rows[dtJournal.Rows.Count - 1]["Ac_id"] = funs.Select_ac_id(ansGridView1.Rows[i].Cells["acc"].Value.ToString());
                dtJournal.Rows[dtJournal.Rows.Count - 1]["Opp_acid"] = cashac_id;
                dtJournal.Rows[dtJournal.Rows.Count - 1]["Narr"] = jnarr;
                dtJournal.Rows[dtJournal.Rows.Count - 1]["Narr2"] = jnarr;
                dtJournal.Rows[dtJournal.Rows.Count - 1]["Sno"] = i + 1;
                dtJournal.Rows[dtJournal.Rows.Count - 1]["LocationId"] = Database.LocationId;
                dtJournal.Rows[dtJournal.Rows.Count - 1]["Reffno"] = ansGridView1.Rows[i].Cells["instrumentno"].Value.ToString();

                if (recpay == "Receipt")
                {
                    dtJournal.Rows[dtJournal.Rows.Count - 1]["Amount"] = -1 * amount;
                }
                else if (recpay == "Payment")
                {
                    dtJournal.Rows[dtJournal.Rows.Count - 1]["Amount"] = amount;
                }
                else if (recpay == "Contra")
                {

                    dtJournal.Rows[dtJournal.Rows.Count - 1]["Amount"] = amount;
                }
                Database.SaveData(dtJournal);
            }
            
            funs.ShowBalloonTip("Saved", "Saved Successfully");

            if (print == true)
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
            return true;
        }

        private void SetVnoold()
        {
            if (vtid == 0 || (vno != 0 && vid != "0") || f12used == true)
            {
                return;
            }
            vno = funs.GenerateVno(vtid, dateTimePicker1.Value.ToString("dd-MMM-yyyy"), vid);
            label2.Text = vno.ToString();
        }

        private void SetVno()
        {
            int numtype = funs.Select_NumType(vtid);
            if ((Prelocationid == Database.LocationId) || (Prelocationid=="" && vid=="0"))
            {
                if (numtype == 3 && vno != 0 && vid != "0")
                {
                    DateTime dt1 = dateTimePicker1.Value;
                    DateTime dt2 = DateTime.Parse(Database.GetScalarDate("select vdate from voucherinfos where LocationId='" + Database.LocationId + "' and vi_id='" + vid + "'"));
                    if (dt1 != dt2)
                    {
                        vno = funs.GenerateVno(vtid, dateTimePicker1.Value.ToString("dd-MMM-yyyy"), vid);
                        label2.Text = vno.ToString();
                    }
                    return;
                }

                if (vtid == 0 || (vno != 0 && vid != "0"))
                {
                    return;
                }

                vno = funs.GenerateVno(vtid, dateTimePicker1.Value.ToString("dd-MMM-yyyy"), vid);
                label2.Text = vno.ToString();
            }
        }


        private void calcTot()
        {
            double total = 0.0;
            for (int i = 0; i < ansGridView1.RowCount - 1; i++)
            {
                total += double.Parse(ansGridView1.Rows[i].Cells["Amount"].Value.ToString());
            }
            label4.Text = total.ToString();
        }

        private void ansGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (ansGridView1.CurrentCell.OwningColumn.Name == "Amount")
            {
                try
                {
                    double amt1 = double.Parse(ansGridView1.Rows[e.RowIndex].Cells["Amount"].Value.ToString());
                    calcTot();
                }
                catch (Exception ex)
                {
                    ansGridView1.Rows[e.RowIndex].Cells["Amount"].Value = "0.00";
                    return;
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
            if (vid != "0")
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
            dtsidefill.Rows[1]["Name"] = "print";
            dtsidefill.Rows[1]["DisplayName"] = "Print";
            dtsidefill.Rows[1]["ShortcutKey"] = "^P";
            if (vid != "0")
            {
                if (Database.utype == "User")
                {
                    dtsidefill.Rows[1]["Visible"] = false;
                }
                else
                {
                    dtsidefill.Rows[1]["Visible"] = true;
                }
            }
            else
            {
                dtsidefill.Rows[1]["Visible"] = true;
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
                    btn.Size = new Size(150, 30);
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
                    if (Database.utype == "Admin")
                    {
                        SaveMethod(false);
                    }
                    else if (vid == "0")
                    {
                        SaveMethod(false);
                    }
                }
            }
            else if (name == "print")
            {
                if (validate() == true)
                {
                    if (Database.utype == "Admin")
                    {
                        SaveMethod(true);
                    }
                    else if (vid == "0")
                    {
                        SaveMethod(true);
                    }
                }
            }            
            else if (name == "quit")
            {
                this.Close();
                this.Dispose();
            }
        }

        private void ansGridView1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsLetter(e.KeyChar) || char.IsNumber(e.KeyChar) || e.KeyChar == ' ' || Convert.ToInt32(e.KeyChar) == 13)
            {
            }
            else
            {
                return;
            }

            string cash = "";
            cash = funs.Select_vt_Cashtran(vtid);            
            if (ansGridView1.CurrentCell.OwningColumn.Name == "acc")
            {
                string wheresrt = "Not (Path  LIKE '1;3;%'  or Path  like '1;28;%')";
                strCombo = funs.GetStrCombo(wheresrt);
                if (cash == "Allowed")
                {
                    wheresrt = " not Path like '1;28;%'  ";
                    strCombo = funs.GetStrCombo(wheresrt);
                }
                else if (cash == "Not Allowed")
                {
                    wheresrt = " not (Path  LIKE '1;3;%'  or Path  like '1;28;%')  ";
                    strCombo = funs.GetStrCombo(wheresrt);
                }
                else if (cash == "Only Allowed")
                {
                    wheresrt = "Not Path LIKE '1;3;%'";
                    strCombo = funs.GetStrCombo(wheresrt);
                }
                if (recpay == "Contra")
                {
                    wheresrt = " Path  LIKE '1;3;%'  or Path  like '1;2;%'  ";
                    strCombo = funs.GetStrCombo(wheresrt);
                }

                ansGridView1.CurrentCell.Value = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 1);
            }
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
                    for (int i = 1; i < ansGridView1.Columns.Count; i++)
                    {
                        ansGridView1.Rows[ansGridView1.CurrentRow.Index].Cells[i].Value = "";
                    }
                    return;
                }
                else
                {
                    ansGridView1.Rows.RemoveAt(ansGridView1.CurrentRow.Index);
                    for (int i = 0; i < ansGridView1.Rows.Count; i++)
                    {
                        ansGridView1.Rows[i].Cells["sno"].Value = (i + 1);
                    }
                    calcTot();
                    return;
                }
            }

            ansGridView1.CurrentCell.OwningRow.Cells["sno"].Value = ansGridView1.CurrentCell.OwningRow.Index + 1;
            if (ansGridView1.CurrentCell.OwningColumn.Name == "instrumentdt")
            {
                if (ansGridView1.Rows[ansGridView1.CurrentRow.Index].Cells["Amount"].Value == null)
                {
                    if (ansGridView1.Rows[ansGridView1.CurrentRow.Index].Cells["Amount"].Value == null || ansGridView1.Rows[ansGridView1.CurrentRow.Index].Cells["Amount"].Value.ToString() == "")
                    {
                        SendKeys.Send("{tab}");
                    }
                }
            }

            if (ansGridView1.CurrentCell.OwningColumn.Name == "acc")
            {
                if (e.Control && e.KeyCode == Keys.A)
                {
                    if (ansGridView1.CurrentCell.Value.ToString() != "")
                    {
                        ansGridView1.CurrentCell.Value = funs.EditAccount(ansGridView1.CurrentCell.Value.ToString());
                    }
                }
                else if (e.Control && e.KeyCode == Keys.C)
                {
                    ansGridView1.CurrentCell.Value = funs.AddAccount();
                }
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            string wheresrt = "(Path LIKE '1;3;%')  OR   (Path LIKE '1;2;%')";
            strCombo = funs.GetStrCombo(wheresrt);
            textBox1.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 1);
        }

        private void ansGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            ansGridView1.CurrentCell.Value = 0;
        }

        private void lastVoucher()
        {
            DataTable dtLastTran = new DataTable();
            if (this.Text != "")
            {
                Database.GetSqlData("SELECT temp.[name], temp.vnumber, temp.vdate, VOUCHERINFOs.Totalamount FROM (SELECT VOUCHERTYPEs.Name, Max(VOUCHERINFOs.Vnumber) AS Vnumber, Max(VOUCHERINFOs.Vdate) AS Vdate FROM VOUCHERINFOs INNER JOIN VOUCHERTYPEs ON VOUCHERINFOs.Vt_id = VOUCHERTYPEs.Vt_id WHERE (((VOUCHERTYPEs.Name)='" + this.Text + "' )) GROUP BY VOUCHERTYPEs.Name)  AS temp INNER JOIN (VOUCHERINFOs INNER JOIN VOUCHERTYPEs ON VOUCHERINFOs.Vt_id = VOUCHERTYPEs.Vt_id) ON (temp.Vdate = VOUCHERINFOs.Vdate) AND (temp.Vnumber = VOUCHERINFOs.Vnumber) AND (temp.Name = VOUCHERTYPEs.Name)", dtLastTran);
            }
            frm_main.clearDisplay2();
            frm_main.dtDisplay2.Columns.Add("Item");
            frm_main.dtDisplay2.Columns.Add("Description");

            if (dtLastTran.Rows.Count > 0)
            {
                frm_main.dtDisplay2.Rows.Add();
                frm_main.dtDisplay2.Rows[0]["Item"] = "Type";
                frm_main.dtDisplay2.Rows[0]["Description"] = dtLastTran.Rows[0]["name"];
                frm_main.dtDisplay2.Rows.Add();
                frm_main.dtDisplay2.Rows[1]["Item"] = "Voucher No.";
                frm_main.dtDisplay2.Rows[1]["Description"] = dtLastTran.Rows[0]["vnumber"];
                frm_main.dtDisplay2.Rows.Add();
                frm_main.dtDisplay2.Rows[2]["Item"] = "Voucher Date";
                frm_main.dtDisplay2.Rows[2]["Description"] = DateTime.Parse(dtLastTran.Rows[0]["vdate"].ToString()).ToString("dd-MMM-yyyy");
                frm_main.dtDisplay2.Rows.Add();
                frm_main.dtDisplay2.Rows[3]["Item"] = "Total Amount";
                frm_main.dtDisplay2.Rows[3]["Description"] = dtLastTran.Rows[0]["Totalamount"];
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            string wheresrt  = "in(3,2,31)";
            
            if (e.Control && e.KeyCode == Keys.A)
            {
                if (textBox1.Text != "")
                {
                    textBox1.Text = funs.EditAccount(textBox1.Text, wheresrt);
                }
            }
            else if (e.Control && e.KeyCode == Keys.C)
            {
                textBox1.Text = funs.AddAccount(wheresrt);
            }
        }

        private int VouchertypeId(String str)
        {
            int vouTypId;
            vouTypId = funs.Select_vt_id(str);
            return vouTypId;
        }

        private void ansGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (ansGridView1.CurrentCell.OwningColumn.Name == "sno")
            {
                ansGridView1.Rows[e.RowIndex].Cells["sno"].Value = e.RowIndex + 1;
                SendKeys.Send("{tab}");
            }
        }

        private void dateTimePicker1_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control == true && e.Alt == true && e.KeyCode == Keys.N)
            {
                textBox2.ReadOnly = true;
                DataTable dtcombo = new DataTable();
                strCombo = "Select Distinct(Narr) from Voucherinfos where Narr<>' ' order by Narr";
                Database.GetSqlData(strCombo, dtcombo);
                textBox2.Text = SelectCombo.ComboDt(this, dtcombo, 0);
                textBox2.ReadOnly = false;
                SendKeys.Send("{End}");
            }
            else
            {
                SelectCombo.IsEnter(this, e.KeyCode);
            }
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox1);
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox2);
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox1);
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox2);
        }

        private void clear()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            label4.Text = "0";
            ansGridView1.Rows.Clear();
            dateTimePicker1.Focus();
            vid = "0";
            vno = 0;
            dtVoucherInfo.Rows.Clear();
            dtVoucheractotal.Rows.Clear();
            cmbVouTyp = "";
            cashac_id = 0;
        }

        private bool validate()
        {
            ansGridView1.EndEdit();
            //if (vid != "0")
            //{
            //    int count = Database.GetScalarInt("SELECT Count([Vnumber]) AS Expr1 FROM VOUCHERINFOs WHERE (((VOUCHERINFOs.Vt_id)=" + vtid + ") AND ((VOUCHERINFOs.Vi_id)<>'" + vid + "') AND ((VOUCHERINFOs.Vnumber)=" + vno + ") AND ((VOUCHERINFOs.Vdate)=" + access_sql.Hash + dateTimePicker1.Value.Date.ToString(Database.dformat) + access_sql.Hash + "))");
            //    if (count != 0)
            //    {
            //        vno = funs.GenerateVno(vtid, dateTimePicker1.Value.ToString("dd-MMM-yyyy"), vid);
            //    }
            //}
            if (label4.Text == "0")
            {
                MessageBox.Show("Please enter some value");
                textBox1.Focus();
                return false;
            }
            if (textBox1.Text == "")
            {
                textBox1.Focus();
                return false;
            }
            if (funs.Select_ac_id(textBox1.Text) == "")
            {
                textBox1.Focus();
                MessageBox.Show("Enter Valid Account Name");
                return false;
            }
            for (int i = 0; i < ansGridView1.RowCount - 1; i++)
            {
                if (ansGridView1.Rows[i].Cells["Acc"].Value.ToString() == "")
                {
                    ansGridView1.CurrentCell = ansGridView1["Acc", ansGridView1.CurrentCell.RowIndex];
                    MessageBox.Show("Enter Account Name");
                    return false;
                }
                if (funs.Select_ac_id(ansGridView1.Rows[i].Cells["Acc"].Value.ToString()) == "")
                {
                    ansGridView1.CurrentCell = ansGridView1["Acc", ansGridView1.CurrentCell.RowIndex];
                    MessageBox.Show("Enter Valid Account Name");
                    return false;
                }
            }


            if (vid != "0")
            {

                if (Prelocationid != Database.LocationId)
                {
                    MessageBox.Show("Vouchers Can't be Save.. Location must be Same..");
                    return false;
                }

            }

            SetVno();
           
            if (vno == 0)
            {
                vno = funs.GenerateVno(vtid, dateTimePicker1.Value.ToString("dd-MMM-yyyy"), vid);
            }
            if (vno == 0)
            {
                MessageBox.Show("Voucher Number can't be created on this date.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
           
            if (vid == "")
            {
                int numtype = funs.chkNumType(vtid);
                if (numtype != 1)
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
            }


            //if (vno == 0)
            //{
            //    MessageBox.Show("Vouchers Can't be Save on 0..");
            //    return false;
            //}
            return true;
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            cmbVouTyp = "select [name] from vouchertypes where active=" + access_sql.Singlequote + "true" + access_sql.Singlequote + " and type='" + recpay + "' and A=" + access_sql.Singlequote + "true" + access_sql.Singlequote + "";

            textBox3.Text = SelectCombo.ComboKeypress(this, e.KeyChar, cmbVouTyp, e.KeyChar.ToString(), 0);
            vtid = funs.Select_vt_id(textBox3.Text);
            if (textBox3.Text != "")
            {
                textBox1.Enabled = true;
            }
            SetVno();
        }

        private void Display()
        {
            DataTable dtvt = new DataTable();
            string cmbVouTyp3 = "";
            
                cmbVouTyp3 = " and A=" + access_sql.Singlequote + "True" + access_sql.Singlequote;
           
            cmbVouTyp = "select [name] from vouchertypes where active=" + access_sql.Singlequote + "true" + access_sql.Singlequote + " and type='" + recpay + "'";
            cmbVouTyp = cmbVouTyp + cmbVouTyp3;
            Database.GetSqlData(cmbVouTyp, dtvt);
            if (dtvt.Rows.Count == 1)
            {
                textBox3.Text = dtvt.Rows[0]["name"].ToString();
                vtid = funs.Select_vt_id(textBox3.Text);
                textBox3.Enabled = false;
            }
            else
            {
                textBox3.Enabled = true;
            }
            if (recpay == "Contra")
            {
                groupBox3.Text = "Credit Account";
                ansGridView1.Columns["acc"].HeaderText = "Debit Account Name";
                //ansGridView1.Columns["instrumentno"].Visible = false;
                //ansGridView1.Columns["instrumentdt"].Visible = false;
            }
            SetVno();
        }

        private void dateTimePicker1_Enter(object sender, EventArgs e)
        {
            Database.setFocus(dateTimePicker1);
        }

        private void dateTimePicker1_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(dateTimePicker1);
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            SetVno();
        }
    }
}
