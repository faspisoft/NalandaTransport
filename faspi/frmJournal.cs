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
    public partial class frmJournal : Form
    {
        DataTable dtVoucherInfo;
        DataTable dtVoucheractotal;
        DataTable dtJournal;
        DateTime dtm;
        public String cmdmode;
        Boolean generateVno = false;
        int vno = 0, vtid;
        string vid = "";
        public String gFrmCaption;
        DateTime chkDt = new DateTime();
        DataTable dtFid = new DataTable();
        DataTable dtUid = new DataTable();
        int fid, uid;
        String strCombo;
        string Prelocationid = "";

        public frmJournal()
        {
            InitializeComponent();
            dateTimePicker1.MinDate = Database.stDate;
            dateTimePicker1.MaxDate = Database.ldate;
            dateTimePicker1.CustomFormat = Database.dformat;
            dateTimePicker1.Value = Database.ldate;
        }

        private void frmJournal_Load(object sender, EventArgs e)
        {
            SideFill();
        }

        private void ansGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (ansGridView1.CurrentCell.OwningColumn.Name == "sno")
            {
                ansGridView1.Rows[e.RowIndex].Cells["sno"].Value = e.RowIndex + 1;
                SendKeys.Send("{tab}");
            }
        }

        private void frmJournal_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                frmHelp frm = new frmHelp();
                frm.ShowDialog(this);
            }
            else if (e.Control && e.KeyCode == Keys.S)
            {
                if (Validate() == true)
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
                if (Validate() == true)
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
                            if (Feature.Available("Freeze Transaction") == "No")
                            {
                                modify();
                            }
                            else
                            {
                                DataTable dtfrz = new DataTable();
                                Database.GetSqlData("Select selected_value from Firmsetup where Features='Freeze Transaction'", dtfrz);
                                if (dateTimePicker1.Value > DateTime.Parse(dtfrz.Rows[0][0].ToString()))
                                {
                                    modify();
                                }
                                else
                                {
                                    MessageBox.Show("Your Voucher is Freezed");
                                }
                            }
                            this.Close();
                            this.Dispose();
                        }
                    }
                }
            }
            else if (e.KeyCode == Keys.Escape)
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
            //else if (e.KeyCode == Keys.F12)
            //{
            //    InputBox box = new InputBox("Enter Administrative password", "", true);
            //    box.ShowDialog(this);
            //    MessageBox.Show(box.outStr);
            //    String pass = box.outStr;
            //    if (pass.ToLower() == "admin")
            //    {
            //        box = new InputBox("Enter Voucher Number", "", false);
            //        box.ShowDialog();
            //        vno = int.Parse(box.outStr);
            //        generateVno = true;
            //    }
            //    else
            //    {
            //        MessageBox.Show("Invalid password");
            //    }
            //}
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
            dtsidefill.Rows[2]["Name"] = "quit";
            dtsidefill.Rows[2]["DisplayName"] = "Quit";
            dtsidefill.Rows[2]["ShortcutKey"] = "Esc";
            dtsidefill.Rows[2]["Visible"] = true;

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
                if (Validate() == true)
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
                if (Validate() == true)
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

        private void clear()
        {
            textBox2.Text = "";
            label3.Text = "0";
            label4.Text = "0";
            ansGridView1.Rows.Clear();
            dateTimePicker1.Focus();
            vno = 0;
            vid = "0";
            vtid = 0;
            dtVoucherInfo.Rows.Clear();
            dtVoucheractotal.Rows.Clear();
        }

        private void DisplayData()
        {
            dtVoucherInfo = new DataTable("Voucherinfos");
            Database.GetSqlData("select * from voucherinfos where vi_id='" + vid + "'", dtVoucherInfo);
            if (dtVoucherInfo.Rows.Count > 0)
            {
                textBox2.Text = dtVoucherInfo.Rows[0]["Narr"].ToString();
                dateTimePicker1.Value = DateTime.Parse(dtVoucherInfo.Rows[0]["Vdate"].ToString());
                vno = int.Parse(dtVoucherInfo.Rows[0]["Vnumber"].ToString());
                chkDt = DateTime.Parse(dtVoucherInfo.Rows[0]["Vdate"].ToString());
                Prelocationid = dtVoucherInfo.Rows[0]["Locationid"].ToString();
            }

            dtVoucheractotal = new DataTable("voucheractotals");
            Database.GetSqlData("select * from voucheractotals where vi_id='" + vid + "'", dtVoucheractotal);
            for (int i = 0; i < dtVoucheractotal.Rows.Count; i++)
            {
                ansGridView1.Rows.Add();
                ansGridView1.Rows[i].Cells["sno"].Value = dtVoucheractotal.Rows[i]["Srno"];
                ansGridView1.Rows[i].Cells["acc"].Value = funs.Select_ac_nm(dtVoucheractotal.Rows[i]["accid"].ToString());
                ansGridView1.Rows[i].Cells["dr"].Value = funs.DecimalPoint(dtVoucheractotal.Rows[i]["Amount"]);
                ansGridView1.Rows[i].Cells["cr"].Value = funs.DecimalPoint(dtVoucheractotal.Rows[i]["cam"]);

                ansGridView1.Rows[i].Cells["instrumentno"].Value = dtVoucheractotal.Rows[i]["Chkno"];
                if (dtVoucheractotal.Rows[i]["Cdate"].ToString() == "")
                {
                    dtVoucheractotal.Rows[i]["Cdate"] = dtVoucherInfo.Rows[0]["Vdate"].ToString();
                }
                ansGridView1.Rows[i].Cells["instrumentdt"].Value = DateTime.Parse(dtVoucheractotal.Rows[i]["Cdate"].ToString()).ToString("dd / MM / yyyy");


            }

            dtJournal = new DataTable("Journals");
            Database.GetSqlData("select * from Journals where vi_id='" + vid + "'", dtJournal);

            foreach (DataGridViewColumn column in ansGridView1.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            ansGridView1.Columns["dr"].CellTemplate.ValueType = typeof(double);
            ansGridView1.Columns["cr"].CellTemplate.ValueType = typeof(double);

            Database.CloseConnection();
        }

        public void LoadData(String str, String frmCaption)
        {
            gFrmCaption = frmCaption;
            this.Text = frmCaption;
            vid = str;
            ansGridView1.Rows[0].Cells["dr"].Value = "0.00";
            ansGridView1.Rows[0].Cells["cr"].Value = "0.00";
            dateTimePicker1.MinDate = Database.stDate;
            dateTimePicker1.MaxDate = Database.ldate;            
            vtid = funs.Select_vt_id(frmCaption);            
            DisplayData();
            SetVno();
            calcTot();            
        }

        private void modify()
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

                dtTemp = new DataTable("journal");
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
        private bool Validate()
        {
            //if (vid != "0")
            //{
            //    int count = Database.GetScalarInt("SELECT Count([Vnumber]) AS Expr1 FROM VOUCHERINFOs WHERE (((VOUCHERINFOs.Vt_id)=" + vtid + ") AND ((VOUCHERINFOs.Vi_id)<>'" + vid + "'" + ") AND ((VOUCHERINFOs.Vnumber)=" + vno + ") AND ((VOUCHERINFOs.Vdate)=" + access_sql.Hash + dateTimePicker1.Value.Date.ToString(Database.dformat) + access_sql.Hash + "))");
            //    if (count != 0)
            //    {
            //        vno = funs.GenerateVno(vtid, dateTimePicker1.Value.ToString("dd-MMM-yyyy"), vid);
            //    }
            //}
            if (label3.Text == "0" || label4.Text == "0")
            {
                MessageBox.Show("Please enter some value");
                textBox2.Focus();
                return false;
            }
            else if (label3.Text != label4.Text)
            {
                MessageBox.Show("Debit and Credit are not equal");
                ansGridView1.Focus();
                return false;
            }
            if (vid != "0")
            {

                if (Prelocationid != Database.LocationId)
                {
                    MessageBox.Show("Vouchers Can't be Save.. Location must be Same..");
                    return false;
                }

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

            return true;
        }

        private void SaveMethod(bool prnt)
        {
            try
            {
                Database.BeginTran();
                
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
                
                Database.CommitTran();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Voucher Not Saved, Due To An Exception", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Database.RollbackTran();
            }
        }
        private void SetVno()
        {
            int numtype = funs.Select_NumType(vtid);
            if ((Prelocationid == Database.LocationId) || (Prelocationid == "" && vid == "0"))
            {
                if (numtype == 3 && vno != 0 && vid != "0")
                {
                    DateTime dt1 = dateTimePicker1.Value;
                    DateTime dt2 = DateTime.Parse(Database.GetScalarDate("select vdate from voucherinfos where LocationId='" + Database.LocationId + "' and vi_id='" + vid + "'"));
                    if (dt1 != dt2)
                    {
                        vno = funs.GenerateVno(vtid, dateTimePicker1.Value.ToString("dd-MMM-yyyy"), vid);

                    }
                    return;
                }

                if (vtid == 0 || (vno != 0 && vid != "0"))
                {
                    return;
                }
                vno = funs.GenerateVno(vtid, dateTimePicker1.Value.ToString("dd-MMM-yyyy"), vid);
            }
        }


        private bool save(bool print)
        {
            int conn_id = 0;
            ansGridView1.EndEdit();
            calcTot();
            vtid = funs.Select_vt_id(gFrmCaption);
            String narr = "";
            if (textBox2.Text == "")
            {
                if (vtid == 10)
                {
                    narr = "Being Amount Transfer";
                }
            }
            else
            {
                narr = textBox2.Text;
            }

            //if (generateVno == false)
            //{
            //    int numType = 0;
            //    numType = funs.chkNumType(vtid);
            //    if (numType == 3)
            //    {
            //        if (vid == "0" || dateTimePicker1.Value.Date.ToString("dd-MMM-yyyy") != chkDt.ToString("dd-MMM-yyyy"))
            //        {
            //            vno = funs.GenerateVno(vtid, dateTimePicker1.Value.Date.ToString("dd-MMM-yyyy"), vid);
            //        }
            //    }
            //    else if (numType == 1)
            //    {
            //        if (vid == "0")
            //        {
            //            vno = funs.GenerateVno(vtid, dateTimePicker1.Value.Date.ToString("dd-MMM-yyyy"), vid);
            //        }
            //    }
            //    else if (numType == 2)
            //    {
            //        if (vid == "0" || dateTimePicker1.Value.Date.Month != chkDt.Month)
            //        {
            //            vno = funs.GenerateVno(vtid, dateTimePicker1.Value.Date.ToString("dd-MMM-yyyy"), vid);
            //        }
            //    }
            //}
            String[] dtmCheck = { "", "", "" };
            SetVno();
           
            if (vno == 0)
            {
                vno = funs.GenerateVno(vtid, dateTimePicker1.Value.ToString("dd-MMM-yyyy"), vid);
              
            }

            if (label3.Text == label4.Text)
            {

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
                dtVoucherInfo.Rows[0]["Invoiceno"] = invoiceno;
                dtVoucherInfo.Rows[0]["Invoiceno"] = prefix + invoiceno.PadLeft(padding, '0') + postfix;
                dtVoucherInfo.Rows[0]["user_id"] = Database.user_id;
                dtVoucherInfo.Rows[0]["As_Per"] = "";
                dtVoucherInfo.Rows[0]["Vt_id"] = vtid;
                dtVoucherInfo.Rows[0]["Vnumber"] = vno;
                dtVoucherInfo.Rows[0]["Conn_id"] = conn_id;
                dtVoucherInfo.Rows[0]["iscancel"] = false;
                dtVoucherInfo.Rows[0]["narr"] = narr;
                dtVoucherInfo.Rows[0]["narr"] = narr;
                dtVoucherInfo.Rows[0]["Vdate"] = dateTimePicker1.Value.Date.ToString("dd-MMM-yyyy");
                dtVoucherInfo.Rows[0]["Totalamount"] = label4.Text;
                dtVoucherInfo.Rows[0]["FormC"] = false;
                dtVoucherInfo.Rows[0]["Roff"] = false;
                dtVoucherInfo.Rows[0]["Paymentmode"] = "";
                dtVoucherInfo.Rows[0]["Deliverytype"] = "";
                dtVoucherInfo.Rows[0]["DD"] = 0;
                dtVoucherInfo.Rows[0]["Dr"] = 0;
                dtVoucherInfo.Rows[0]["RoffChanged"] = false;
                dtVoucherInfo.Rows[0]["Tdtype"] = false;
                dtVoucherInfo.Rows[0]["TaxChanged"] = false;
                dtVoucherInfo.Rows[0]["LocationId"] = Prelocationid;
                if (vid == "0")
                {
                    dtVoucherInfo.Rows[0]["CreTime"] = System.DateTime.Now.ToString("HH:mm:ss");
                    dtVoucherInfo.Rows[0]["create_date"] = System.DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                    dtVoucherInfo.Rows[0]["user_id"] = Database.user_id;
                }
                dtVoucherInfo.Rows[0]["modify_date"] = System.DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                dtVoucherInfo.Rows[0]["ModTime"] = System.DateTime.Now.ToString("HH:mm:ss");
              
                dtVoucherInfo.Rows[0]["modifyby_id"] = Database.user_id;

                dtVoucherInfo.Rows[0]["Ac_id2"] = null;
                if (vid == "0")
                {
                    dtVoucherInfo.Rows[0]["CreTime"] = System.DateTime.Now.ToString("HH:mm:ss");
                    dtVoucherInfo.Rows[0]["printcount"] = 0;
                }
                dtVoucherInfo.Rows[0]["ModTime"] = System.DateTime.Now.ToString("HH:mm:ss");
                if (ansGridView1.Rows.Count == 3)
                {
                    if (double.Parse(ansGridView1.Rows[0].Cells["Dr"].Value.ToString()) > 0)
                    {
                        dtVoucherInfo.Rows[0]["dr_ac_id"] = funs.Select_ac_id(ansGridView1.Rows[0].Cells["acc"].Value.ToString());
                        dtVoucherInfo.Rows[0]["cr_ac_id"] = funs.Select_ac_id(ansGridView1.Rows[1].Cells["acc"].Value.ToString());
                    }
                    else
                    {
                        dtVoucherInfo.Rows[0]["cr_ac_id"] = funs.Select_ac_id(ansGridView1.Rows[0].Cells["acc"].Value.ToString());
                        dtVoucherInfo.Rows[0]["dr_ac_id"] = funs.Select_ac_id(ansGridView1.Rows[1].Cells["acc"].Value.ToString());
                    }
                }
                if (vid == "0")
                {
                    
                }

                Database.SaveData(dtVoucherInfo);

                if (vid == "0")
                {
                    vid = dtVoucherInfo.Rows[0]["Vi_id"].ToString();
                    //DataTable dtvid = new DataTable();
                    //Database.GetSqlData("select max(cast(substring(Vi_id,4,len(Vi_id)-3) as int)) from VOUCHERINFO", dtvid);
                    //vid = Database.LocationId + dtvid.Rows[0][0].ToString();
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

                for (int i = 0; i < ansGridView1.Rows.Count - 1; i++)
                {
                    if (ansGridView1.Rows[i].Cells["dr"].Value == null)
                    {
                        ansGridView1.Rows[i].Cells["dr"].Value = "0.00";
                    }
                    else if (ansGridView1.Rows[i].Cells["cr"].Value == null)
                    {
                        ansGridView1.Rows[i].Cells["cr"].Value = "0.00";
                    }


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
                    dtVoucheractotal.Rows[i]["Accid"] = funs.Select_ac_id(ansGridView1.Rows[i].Cells["acc"].Value.ToString());
                    dtVoucheractotal.Rows[i]["Cam"] = ansGridView1.Rows[i].Cells["cr"].Value;
                    dtVoucheractotal.Rows[i]["Amount"] = ansGridView1.Rows[i].Cells["dr"].Value;
                    

                    dtVoucheractotal.Rows[i]["Chkno"] = chkno;
                    dtVoucheractotal.Rows[i]["Cdate"] = dtm.ToString("dd-MMM-yyyy");
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

                    dtJournal.Rows.Add();
                    dtJournal.Rows[dtJournal.Rows.Count - 1]["Vi_id"] = vid;

                    dtJournal.Rows[dtJournal.Rows.Count - 1]["Vdate"] = dateTimePicker1.Value.Date.ToString("dd-MMM-yyyy"); ;
                    dtJournal.Rows[dtJournal.Rows.Count - 1]["Ac_id"] = funs.Select_ac_id(ansGridView1.Rows[i].Cells["acc"].Value.ToString());
                    if (double.Parse(ansGridView1.Rows[i].Cells["Dr"].Value.ToString()) > 0)
                    {
                        dtJournal.Rows[dtJournal.Rows.Count - 1]["Amount"] = ansGridView1.Rows[i].Cells["dr"].Value.ToString();
                    }
                    else
                    {
                        dtJournal.Rows[dtJournal.Rows.Count - 1]["Amount"] = -1 * (double.Parse(ansGridView1.Rows[i].Cells["cr"].Value.ToString()));
                    }



                    dtJournal.Rows[dtJournal.Rows.Count - 1]["Narr"] = narr;
                    dtJournal.Rows[dtJournal.Rows.Count - 1]["Narr2"] = narr;
                    dtJournal.Rows[dtJournal.Rows.Count - 1]["Sno"] = ansGridView1.Rows[i].Cells["sno"].Value.ToString();
                    dtJournal.Rows[dtJournal.Rows.Count - 1]["LocationId"] = Database.LocationId;
                    if (ansGridView1.Rows.Count == 3)
                    {

                        if (i == 0)
                        {
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["Opp_acid"] = funs.Select_ac_id(ansGridView1.Rows[1].Cells["acc"].Value.ToString());
                        }
                        else if (i == 1)
                        {
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["Opp_acid"] = funs.Select_ac_id(ansGridView1.Rows[0].Cells["acc"].Value.ToString());
                        }
                    }


                    if (ansGridView1.Rows[i].Cells["instrumentno"].Value == null)
                    {
                        ansGridView1.Rows[i].Cells["instrumentno"].Value = "";
                    }
                    dtJournal.Rows[dtJournal.Rows.Count - 1]["Reffno"] = ansGridView1.Rows[i].Cells["instrumentno"].Value.ToString();


                }

                

                Database.SaveData(dtJournal);

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
            }
            return true;
        }

        private void ansGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (ansGridView1.CurrentCell.OwningColumn.Name == "dr")
            {
                try
                {
                    double amt1 = double.Parse(ansGridView1.Rows[e.RowIndex].Cells["dr"].Value.ToString());
                }
                catch (Exception ex)
                {
                    ansGridView1.Rows[e.RowIndex].Cells["dr"].Value = "0.00";
                    return;
                }
                calcTot();
            }
            else if (ansGridView1.CurrentCell.OwningColumn.Name == "cr")
            {
                try
                {
                    double amt1 = double.Parse(ansGridView1.Rows[e.RowIndex].Cells["cr"].Value.ToString());
                }
                catch (Exception ex)
                {
                    ansGridView1.Rows[e.RowIndex].Cells["cr"].Value = "0.00";
                    return;
                }

                calcTot();
            }
        }

        private void calcTot()
        {
            double dtot = 0.0, ctot = 0.0;
            for (int i = 0; i < ansGridView1.RowCount - 1; i++)
            {
                if (ansGridView1.Rows[i].Cells["dr"].Value != null)
                {
                    dtot += double.Parse(ansGridView1.Rows[i].Cells["dr"].Value.ToString());
                }
                if (ansGridView1.Rows[i].Cells["cr"].Value != null)
                {
                    ctot += double.Parse(ansGridView1.Rows[i].Cells["cr"].Value.ToString());
                }
            }
            label3.Text = dtot.ToString();
            label4.Text = ctot.ToString();
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

            if (ansGridView1.CurrentCell.OwningColumn.Name == "acc")
            {
                strCombo = funs.GetStrCombo("*");

                ansGridView1.CurrentCell.Value = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 1);
            }
        }

        private void ansGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            ansGridView1.CurrentCell.Value = 0;
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
                    ansGridView1.Rows[ansGridView1.CurrentRow.Index].Cells["dr"].Value = "0.00";
                    ansGridView1.Rows[ansGridView1.CurrentRow.Index].Cells["cr"].Value = "0.00";
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
            if (ansGridView1.CurrentCell.OwningColumn.Name == "instrumentdt")
            {
                if (ansGridView1.Rows[ansGridView1.CurrentRow.Index].Cells["dr"].Value == null && ansGridView1.Rows[ansGridView1.CurrentRow.Index].Cells["dr"].Value == null)
                {
                   
                        SendKeys.Send("{tab}");
                   
                }
            }
            //if (ansGridView1.CurrentCell.OwningColumn.Name == "cr")
            //{
            //    if (ansGridView1.CurrentCell.Value == null && ansGridView1.Rows[ansGridView1.CurrentRow.Index].Cells["dr"].Value == null)
            //    {
            //        SendKeys.Send("{tab}");
            //    }
            //}
        }

        private void dateTimePicker1_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox2);
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox2);
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            SetVno();
        }
    }
}
