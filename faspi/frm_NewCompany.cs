using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data.OleDb;
using System.Data.SqlClient;
using Microsoft.SqlServer.Management.Smo;



namespace faspi
{
    public partial class frm_NewCompany : Form
    {
        DataTable dtCompany;
        DataTable dtFirm;
        String strCombo;
        OleDbConnection NewAccessConn;
        public String frmMenuTyp;
        int gid;
        OleDbDataAdapter da;
        
        public frm_NewCompany()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (validate() == true)
            {
                save();
            }
        }

        private void save()
        {
            DataTable dtTable = new DataTable();
            dtFirm.Rows[0]["Firm_name"] = textBox1.Text;
            dtFirm.Rows[0]["Firm_Period_name"] = textBox7.Text;
            dtFirm.Rows[0]["Firm_database"] = textBox8.Text;
            dtFirm.Rows[0]["Firm_odate"] = dateTimePicker1.Value.Date;
            dtFirm.Rows[0]["Firm_edate"] = dateTimePicker2.Value.Date;
            dtFirm.Rows[0]["Gststatus"] = true;
            try
            {
                Database.SaveOtherData(dtFirm);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            if (frmMenuTyp == "New Financial Year" || frmMenuTyp=="GST")
            {
               
                FileInfo sf = new FileInfo(Environment.CurrentDirectory + "\\Database\\" + Database.databaseName + ".mdb");
                Database.fyear = "";
                this.Text = "";
                Database.databaseName = "";
                Database.CloseConnection();
                FileInfo df = new FileInfo("\\Database\\" + textBox8.Text + ".mdb");
                
                bool exist = df.Exists;
                if (exist == false)
                {


                    sf.CopyTo(Environment.CurrentDirectory + "\\Database\\" + textBox8.Text + ".mdb");
                    Form[] frms = this.MdiChildren;
                    foreach (Form frm in frms)
                    {
                        frm.Dispose();
                    }
                   
                }

            }

            if (frmMenuTyp == "New Company")
            {
                FileInfo sf = new FileInfo(Environment.CurrentDirectory + "\\Database\\Template.mdb");
                FileInfo df = new FileInfo("\\Database\\" + textBox8.Text + ".mdb");
                bool exist = df.Exists;
                if (exist == false)
                {
                    sf.CopyTo(Environment.CurrentDirectory + "\\Database\\" + textBox8.Text + ".mdb");
                }                       
            }

            //NewAccessConn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Environment.CurrentDirectory + "\\Database\\" + textBox8.Text + ".mdb;Persist Security Info=true;Jet OLEDB:Database Password=ptsoft9358524971");
            //string Path = Environment.CurrentDirectory + "\\Database\\" ;
            //string db=textBox8.Text + ".mdb";
            //if (gid == 0)
            //{
            //    da = new OleDbDataAdapter("select * from company", NewAccessConn);
            //    OleDbCommandBuilder cb = new OleDbCommandBuilder();
            //    cb.QuotePrefix = "[";
            //    cb.QuoteSuffix = "]";
            //    cb.DataAdapter = da;
            //    dtCompany = new DataTable("company");
            //    da.Fill(dtCompany);
            //    for (int i = 0; i < dtCompany.Rows.Count; i++)
            //    {
            //        dtCompany.Rows[i].Delete();
            //        da.Update(dtCompany);
            //    }
            //    dtCompany.Rows.Add(0);
            //}

            if (frmMenuTyp != "GST" && frmMenuTyp != "New Financial Year")
            {
                dtCompany.Rows[0]["Name"] = textBox1.Text;
                dtCompany.Rows[0]["Firm_Period_name"] = textBox7.Text;
                dtCompany.Rows[0]["Start_from"] = dateTimePicker1.Value.Date;
                dtCompany.Rows[0]["End_at"] = dateTimePicker2.Value.Date;
                dtCompany.Rows[0]["Cst_no"] = textBox6.Text;
                dtCompany.Rows[0]["Tin_no"] = textBox5.Text;
                dtCompany.Rows[0]["Email"] = textBox4.Text;
                dtCompany.Rows[0]["Contactno"] = textBox9.Text;
                dtCompany.Rows[0]["Address1"] = textBox2.Text;
                dtCompany.Rows[0]["Address2"] = textBox3.Text;
                dtCompany.Rows[0]["CState_id"] = funs.Select_state_id(textBox10.Text);
                dtCompany.Rows[0]["BankName"] = textBox12.Text;
                dtCompany.Rows[0]["IFSC"] = textBox11.Text;
                dtCompany.Rows[0]["AccountNo"] = textBox13.Text;
                dtCompany.Rows[0]["SId"] = funs.Select_dp_id(textBox14.Text);

                Database.SaveData(dtCompany);
            }
         
            if (gid == 0)
            {
                
                DataTable dtid = new DataTable("FIRMINFO");
                Database.GetOtherSqlData("select max(F_id) from FIRMINFO", dtid);
                int fid = int.Parse(dtid.Rows[0][0].ToString());
             
                
                if (frmMenuTyp == "New Financial Year")
                {
                    //Database.setVariable(textBox1.Text, textBox7.Text, Database.uname, Database.upass ,Database.utype, textBox8.Text, dateTimePicker1.Value, dateTimePicker2.Value);
                    Database.ldate = DateTime.Parse(dateTimePicker1.Value.ToString("dd-MMM-yyyy"));
                    Database.OpenConnection();
                    Database.CommandExecutor("Delete from voucherinfo");
                    Database.CommandExecutor("Delete from voucheractotal");
                    Database.CommandExecutor("Delete from journal");
                    Database.CommandExecutor("Delete from itemtax");
                    Database.CommandExecutor("Delete from stock");
                    Database.CommandExecutor("Delete from BILLBYBILL");
                    Database.CommandExecutor("Delete from logbook");
                    Database.CommandExecutor("Delete from SMSLOG");
                    Database.CommandExecutor("Delete from voucherdet");
                    Database.CommandExecutor("Delete from voucharges");
                    Database.CommandExecutor("Delete from itemcharges");
                    Database.CommandExecutor("Update Account set Balance=0,Balance2=0");
                    Database.CommandExecutor("Update Description set Open_stock=0,Open_stock2=0");



                    Database.CommandExecutor("Update Company set Firm_Period_name='" + textBox7.Text + "', Start_from=#" + dateTimePicker1.Value + "#  , End_at=#" + dateTimePicker2.Value + "#  ");





                    Database.OpenConnection();
                    // CompactDatabase(Path,db,"ptsoft9358524971");
                }
                else if (frmMenuTyp == "GST")
                {
                    //Database.setVariable(textBox1.Text, textBox7.Text, Database.uname, Database.upass, Database.utype, textBox8.Text, dateTimePicker1.Value, dateTimePicker2.Value);
                    Database.ldate = DateTime.Parse(dateTimePicker1.Value.ToString("dd-MMM-yyyy"));
                    Database.OpenConnection();
                    Database.CommandExecutor("Delete from voucherinfo");
                    Database.CommandExecutor("Delete from voucheractotal");
                    Database.CommandExecutor("Delete from journal");
                    Database.CommandExecutor("Delete from itemtax");
                    Database.CommandExecutor("Delete from stock");
                    Database.CommandExecutor("Delete from BILLBYBILL");
                    Database.CommandExecutor("Delete from logbook");
                    Database.CommandExecutor("Delete from SMSLOG");
                    Database.CommandExecutor("Delete from voucherdet");
                    Database.CommandExecutor("Delete from voucharges");
                    Database.CommandExecutor("Delete from itemcharges");
                    Database.CommandExecutor("Update Account set Balance=0,Balance2=0");
                    Database.CommandExecutor("Update Description set Open_stock=0,Open_stock2=0");

                    Database.CommandExecutor("Update Company set Firm_Period_name='" + textBox7.Text + "', Start_from=#" + dateTimePicker1.Value + "#  , End_at=#"+ dateTimePicker2.Value+"#  ");

                    Database.CommandExecutor("Delete from FirmSetup where Features='Barcode'");
                    Database.CommandExecutor("Delete from FirmSetup where Features='Pending Invoice'");
                    Database.CommandExecutor("Delete from FirmSetup where Features='Batch Code'");
                    Database.CommandExecutor("Delete from FirmSetup where Features='Amt=Weight*Qty*Rate'");
                   
                    Database.CommandExecutor("Delete from FirmSetup where Features='Weight required in Billing'");
                    Database.CommandExecutor("Delete from FirmSetup where Features='Purchase Invoice (Ex-State)'");
                  
                    Database.CommandExecutor("Delete from FirmSetup where Features='Price Variation Report'");
                    Database.CommandExecutor("Delete from FirmSetup where Features='Grid Report'");
                    Database.CommandExecutor("Delete from FirmSetup where Features='Unregistered Purchase'");
                    Database.CommandExecutor("Delete from FirmSetup where Features='Sale Including Tax'");
                    Database.CommandExecutor("Delete from FirmSetup where Features='Dot Matrix'");
                    Database.CommandExecutor("Delete from FirmSetup where Features='Transaction Report in Crystal Report'");
                    Database.CommandExecutor("Delete from FirmSetup where Features='Production'");
                    Database.CommandExecutor("Update FirmSetup set selected_value='GST' where Features='Taxation Applicable'  ");


                    Database.CommandExecutor("Drop table items");
                    Database.CommandExecutor("Drop table ITEMTAX");
                    Database.CommandExecutor("Drop table Voucherdet1");
                    Database.CommandExecutor("Drop table Journal1");
                    Database.CommandExecutor("Drop table Packing");
                    Database.CommandExecutor("Drop table BASERATE");
                    Database.CommandExecutor("Drop table TAXCATEGORYDETAIL");
                    Database.CommandExecutor("Drop table USERACC");
                    Database.CommandExecutor("Delete from Vouchertype where type='Report' and Name<>'Ledger'");
                    Database.CommandExecutor("Delete from Vouchertype where Type='Sale' and A=true");
                    Database.CommandExecutor("Delete from Vouchertype where Type='Return' and A=true ");
                    
                    Database.CommandExecutor("Delete from Vouchertype where Name='Stock Issue'");
                    Database.CommandExecutor("Delete from Vouchertype where Name='Stock Receive'");
                    Database.CommandExecutor("Delete from Vouchertype where Name='Bank Payment'");
                    Database.CommandExecutor("Delete from Vouchertype where Name='Bank Receipt'");
                    Database.CommandExecutor("Delete from Vouchertype where Name='Purchase(Ex State)'");
                    Database.CommandExecutor("Delete from Vouchertype where Name='Purchase Return(Ex State)'");

                    Database.CommandExecutor("Update Vouchertype set Name='Payment Voucher',AliasName='Payment Voucher' where Name='Cash Payment'");
                    Database.CommandExecutor("Update Vouchertype set Name='Receipt Voucher',AliasName='Receipt Voucher' where Name='Cash Receipt'");

                    Database.CommandExecutor("Update Vouchertype set Name='Bill of Supply',AliasName='Bill of Supply',CashTransaction='Only Allowed',Prefix='B-',Padding=6,printcopy='Original for Recipient,True;Duplicate for Transporter,True;Triplicate for Supplier,True;',Code='SLB',Short='SLB',ReportName='GSTBOSA4.rpt' where Name='Bill'");

                    Database.CommandExecutor("Update Vouchertype set CashTransaction='Not Allowed',printcopy='Original for Recipient,True;Duplicate for Transporter,True;Triplicate for Supplier,True;',Prefix='T-',padding=6,Code='SLT',Short='SLT',ReportName='GSTTIA4.rpt' where Name='Tax Invoice'");

                    Database.CommandExecutor("Update Vouchertype set Name='Contra Voucher',AliasName='Contra Voucher' where Name='Contra'");
                    
                    if (Database.DatabaseType == "access")
                    {
                        Database.CommandExecutor("Alter table Vouchertype Drop Column AllowedAcc");
                    }
                    else
                    {
                        Database.CommandExecutor("Alter table Vouchertype Drop AllowedAcc");
                    }

                    Database.CommandExecutor("insert into VOUCHERTYPE ([Name],[Type],[Stationary],[Numtype],[Short],[AliasName],[Default1],[Default2],[Default3],[ReportName],[Code],[Effect_On_Stock],[Effect_On_Acc],[IncludingTax],[ExcludingTax],[ExState],[TaxInvoice],[Unregistered],[Active],[PaperSize],[SmsTemplate],[A],[B],[printcopy],[CashTransaction],[Calculation],[Postfix],[Prefix],[Padding]) values('Bill of Supply','Sale',true,1,'SLB','Bill of Supply','Original Copy','Duplicate Copy','Office Copy','GSTBOSA4.rpt','SLB','Y','Y',true,true,false,false,false,true,'A4','Dear Customer, Thanks For Purchasing With Us, Your Last Bill No is: {Vno}',true,true,'Original for Recipient,True;Duplicate for Transporter,True;Triplicate for Supplier,True;','Only Allowed','Default Excluding Tax','','B-',6)");
                    Database.CommandExecutor("insert into VOUCHERTYPE ([Name],[Type],[Stationary],[Numtype],[Short],[AliasName],[Default1],[Default2],[Default3],[ReportName],[Code],[Effect_On_Stock],[Effect_On_Acc],[IncludingTax],[ExcludingTax],[ExState],[TaxInvoice],[Unregistered],[Active],[PaperSize],[SmsTemplate],[A],[B],[printcopy],[CashTransaction],[Calculation],[Postfix],[Prefix],[Padding]) values('Bill of Supply Return','Return',true,1,'REB','Bill of Supply Return','Original Copy','Duplicate Copy','Office Copy','GSTBOSA4.rpt','REB','Y','Y',true,true,false,false,false,true,'A4','Dear Customer, Thanks For Purchasing With Us, Your Last Bill No is: {Vno}',true,true,'Original for Recipient,True;Duplicate for Transporter,True;Triplicate for Supplier,True;','Only Allowed','Default Excluding Tax','','BR-',6)");


                    Database.CommandExecutor("insert into VOUCHERTYPE ([Name],[Type],[Stationary],[Numtype],[Short],[AliasName],[Default1],[Default2],[Default3],[ReportName],[Code],[Effect_On_Stock],[Effect_On_Acc],[IncludingTax],[ExcludingTax],[ExState],[TaxInvoice],[Unregistered],[Active],[PaperSize],[SmsTemplate],[A],[B],[printcopy],[CashTransaction],[Calculation],[Postfix],[Prefix],[Padding]) values('Tax Invoice','Sale',true,1,'SLT','Tax Invoice','Original Copy','Duplicate Copy','Office Copy','GSTTIA4.rpt','SLT','Y','Y',true,true,false,true,false,true,'A4','Dear Customer, Thanks For Purchasing With Us, Your Last Bill No is: {Vno}',true,true,'Original for Recipient,True;Duplicate for Transporter,True;Triplicate for Supplier,True;','Not Allowed','Default Excluding Tax','','T-',6)");
                    Database.CommandExecutor("insert into VOUCHERTYPE ([Name],[Type],[Stationary],[Numtype],[Short],[AliasName],[Default1],[Default2],[Default3],[ReportName],[Code],[Effect_On_Stock],[Effect_On_Acc],[IncludingTax],[ExcludingTax],[ExState],[TaxInvoice],[Unregistered],[Active],[PaperSize],[SmsTemplate],[A],[B],[printcopy],[CashTransaction],[Calculation],[Postfix],[Prefix],[Padding]) values('Tax Invoice Return','Return',true,1,'RET','Tax Invoice Return','Original Copy','Duplicate Copy','Office Copy','GSTTIA4.rpt','RET','Y','Y',true,true,false,true,false,true,'A4','Dear Customer, Thanks For Purchasing With Us, Your Last Bill No is: {Vno}',true,true,'Original for Recipient,True;Duplicate for Transporter,True;Triplicate for Supplier,True;','Not Allowed','Default Excluding Tax','','TR-',6)");


                    Database.CloseConnection();
                    MessageBox.Show("New GST Financial Year is created Successfully.");
                    Environment.Exit(0);

                }
                if (frmMenuTyp == "New Company")
                {
                    textBox1.Text = "";
                    textBox2.Text = "";
                    textBox3.Text = "";
                    textBox4.Text = "";
                    textBox5.Text = "";
                    textBox6.Text = "";
                    textBox7.Text = "";
                    textBox8.Text = "";
                    textBox9.Text = "";
                    textBox10.Text = "";
                }
                else
                {
                    this.Close();
                    this.Dispose();
                }

            }

            else
            {
                MessageBox.Show("Company modified");
                Database.fname = textBox1.Text;
                Database.fyear = textBox7.Text;
                Database.CompanyState_id = Database.GetScalarText("Select CState_id from Company");   
                this.Close();
                this.Dispose();
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



            //close
            dtsidefill.Rows.Add();
            dtsidefill.Rows[1]["Name"] = "quit";
            dtsidefill.Rows[1]["DisplayName"] = "Quit";
            dtsidefill.Rows[1]["ShortcutKey"] = "Esc";
            dtsidefill.Rows[1]["Visible"] = true;




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

                    if (frmMenuTyp == "GST")
                    {
                        DateTime olddate = dateTimePicker1.Value.AddDays(-1);


                        Database.CommandExecutorOther("Update Firminfo set Firm_edate=#" + olddate + "# where Firm_name='" + Database.fname + "'  and Firm_Period_name='"+ Database.fyear+"' ");
                        Database.CommandExecutor("Update Company set End_at=#" + olddate + "#");
                    }

                    save();
                 
                 
                }
            }




            if (name == "quit")
            {
                this.Close();
                this.Dispose();
            }


        }


        public void LoadData(int id,String frmCaption)
        {
            gid = id;
            
            dtFirm = new DataTable("FirmInfo");
            Database.GetOtherSqlData("select * from firminfo where f_id=" + id, dtFirm);
            if (id != 0)
            {
                dtCompany = new DataTable("company");
             
                Database.GetSqlData("select * from company", dtCompany);


                // Conn.Close();
            }
            this.Text = frmCaption;

            if (dtFirm.Rows.Count == 0)
            {
                dtFirm.Rows.Add();
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";
                textBox5.Text = "";
                textBox6.Text = "";
                textBox7.Text = "";
                textBox8.Text = "";
                textBox9.Text = "";
                textBox10.Text = "";
                textBox12.Text = "";
                textBox11.Text = "";
                textBox13.Text = "";
                textBox14.Text = "";
                DateTime frmdate= new DateTime(System.DateTime.Today.Year,4,1);
                DateTime todate= new DateTime(System.DateTime.Today.Year+1,3,31);
                dateTimePicker1.Value = frmdate;
                dateTimePicker2.Value = todate;
            }
            else
            {
                textBox1.Text = dtCompany.Rows[0]["Name"].ToString();
                textBox2.Text = dtCompany.Rows[0]["Address1"].ToString();
                textBox3.Text = dtCompany.Rows[0]["Address2"].ToString();
                textBox4.Text = dtCompany.Rows[0]["Email"].ToString();
                textBox5.Text = dtCompany.Rows[0]["Tin_no"].ToString();
                textBox6.Text = dtCompany.Rows[0]["Cst_no"].ToString();
                textBox7.Text = dtCompany.Rows[0]["Firm_Period_name"].ToString();
                textBox8.Text = dtFirm.Rows[0]["Firm_database"].ToString();
                textBox9.Text = dtCompany.Rows[0]["Contactno"].ToString();
                textBox10.Text = funs.Select_state_nm(dtCompany.Rows[0]["CState_id"].ToString());
                textBox12.Text= dtCompany.Rows[0]["BankName"].ToString();
                textBox11.Text =dtCompany.Rows[0]["IFSC"].ToString();
                textBox13.Text = dtCompany.Rows[0]["AccountNo"].ToString();
                textBox8.Enabled = false;
                dateTimePicker1.Value = DateTime.Parse(dtFirm.Rows[0]["Firm_odate"].ToString());
                dateTimePicker2.Value = DateTime.Parse(dtFirm.Rows[0]["Firm_edate"].ToString());
                textBox14.Text = funs.Select_dp_nm(dtCompany.Rows[0]["SId"].ToString());
            }
        }


        public void NewFinancial(String frmCaption)
        {
            dtFirm = new DataTable("FirmInfo");
            Database.GetOtherSqlData("select * from firminfo where Firm_name ='" + Database.fname + "' and Firm_Period_name='" + Database.fyear +"'"   , dtFirm);
            if (dtFirm.Rows.Count == 0)
            {
                return;
            }
            else
            {
                dtCompany = new DataTable("company");
                OleDbConnection Conn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Application.StartupPath + "\\Database\\" + dtFirm.Rows[0]["Firm_database"] + ".mdb;Persist Security Info=true;Jet OLEDB:Database Password=ptsoft9358524971");
                Conn.Open();
                da = new OleDbDataAdapter("select * from company", Conn);
                OleDbCommandBuilder cb = new OleDbCommandBuilder();
                cb.DataAdapter = da;
                da.Fill(dtCompany);
                Conn.Close();
            }

            this.Text = frmCaption;

            if (dtFirm.Rows.Count != 0)
            {
                dtFirm.Rows.Clear();
                dtFirm.Rows.Add();
                textBox1.Text = dtCompany.Rows[0]["Name"].ToString();
                textBox1.Enabled = false;
                textBox2.Text = dtCompany.Rows[0]["Address1"].ToString();
                textBox2.Enabled = false;
                textBox3.Text = dtCompany.Rows[0]["Address2"].ToString();
                textBox3.Enabled = false;
                textBox4.Text = dtCompany.Rows[0]["Email"].ToString();
                textBox4.Enabled = false;
                textBox5.Text = dtCompany.Rows[0]["Tin_no"].ToString();
                textBox5.Enabled = false;
                textBox6.Text = dtCompany.Rows[0]["Cst_no"].ToString();
                textBox6.Enabled = false;
                textBox9.Text = dtCompany.Rows[0]["Contactno"].ToString();
                textBox9.Enabled = false;
                textBox10.Text = funs.Select_state_nm(dtCompany.Rows[0]["CState_id"].ToString());
                textBox10.Enabled = false;


                textBox11.Text = dtCompany.Rows[0]["IFSC"].ToString();
                textBox11.Enabled = false; 
                textBox12.Text = dtCompany.Rows[0]["Bankname"].ToString();
                textBox12.Enabled = false;
                textBox13.Text = dtCompany.Rows[0]["Accountno"].ToString();
                textBox13.Enabled = false;


                string dbname = "";
                string[] ar = Database.fname.Split(' ');
                for (int i = 0; i < ar.Length; i++)
                {
                    textBox8.Text = textBox8.Text + ar[i].Substring(0, 1);
                }
                textBox8.Text = textBox8.Text + System.DateTime.Today.Year.ToString().Substring(2, 2) + (System.DateTime.Today.Year + 1).ToString().Substring(2, 2);
                textBox8.Text = textBox8.Text.Replace("&", "");
                textBox8.Text = textBox8.Text.Replace("(", "");
                textBox8.Text = textBox8.Text.Replace(")", "");
                textBox8.Text = textBox8.Text.Replace(",", "");
                textBox8.Text = textBox8.Text.Replace(".", "");
                textBox8.Text = textBox8.Text.Replace(",", "");
                textBox8.Text = textBox8.Text.Replace("[", "");
                textBox8.Text = textBox8.Text.Replace("]", "");
                DateTime frmdate;
                if (frmMenuTyp == "GST")
                {
                    frmdate = new DateTime(System.DateTime.Today.Year, 7, 1);
                    textBox8.Text = textBox8.Text + "GST";
                    textBox7.Text = System.DateTime.Today.Year + "-" + (System.DateTime.Today.Year + 1) + "(GST)";
                }
                else
                {
                    frmdate = new DateTime(System.DateTime.Today.Year, 4, 1);
                    textBox7.Text = System.DateTime.Today.Year + "-" + (System.DateTime.Today.Year + 1);
                }
                
                DateTime todate = new DateTime(System.DateTime.Today.Year + 1, 3, 31);
                dateTimePicker1.Value = frmdate;
                dateTimePicker2.Value = todate;
            }
            

        }

        private void Button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frm_NewCompany_Load(object sender, EventArgs e)
        {
            dateTimePicker1.CustomFormat = Database.dformat;
            dateTimePicker2.CustomFormat = Database.dformat;
         //   this.Size = this.MdiParent.Size;
            SideFill();

            if (Feature.Available("Taxation Applicable") == "VAT")
            {
                groupBox4.Text = "TIN";
            }
            else
            {
                groupBox4.Text = "GSTN";
            }

        }

        private void frm_NewCompany_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control &&  e.KeyCode == Keys.S)
            {
                if (validate() == true)
                {
                    if (frmMenuTyp == "GST")
                    {
                        DateTime olddate = dateTimePicker1.Value.AddDays(-1);


                        Database.CommandExecutorOther("Update Firminfo set Firm_edate=#" + olddate + "# where Firm_name='" + Database.fname + "'  and Firm_Period_name='" + Database.fyear + "' ");
                        Database.CommandExecutor("Update Company set End_at=#" + olddate + "#");
                    }

                    save();
                } 
            }
           
            if (e.KeyCode == Keys.Escape)
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
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox1);
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox2);
        }

        private void textBox3_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox3);
        }

        private void textBox4_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox4);
        }

        private void textBox5_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox5);
        }

        private void textBox6_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox6);
        }

        private void textBox7_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox7);
        }

        private void textBox8_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox8);
        }

        private void textBox8_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox8);
        }

        private void textBox7_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox7);
        }

        private void textBox6_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox6);
        }

        private void textBox5_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox5);
        }

        private void textBox4_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox4);
        }

        private void textBox3_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox3);
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox2);
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox1);
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox4_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox5_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox6_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox7_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox8_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void dateTimePicker1_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void dateTimePicker2_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }


        private bool validate()
        {
            if (textBox1.Text == "")
            {
                textBox1.Focus();
                return false;
            }

            else if (textBox7.Text == "")
            {
                textBox7.Focus();
                return false;
            }

            else if (textBox8.Text == "")
            {
                textBox8.Focus();
                return false;
            }
            else if (textBox10.Text == "")
            {
                textBox10.Focus();
                return false;
            }          
               
                
            return true;
        }



        private void CompactDatabase(string Path, string Database,string Password)
        {
            
            string Database2 = Database + "1";
            string oldmdbfile = "";
            string newmdbfile = "";

            if (Password != "")
            {
                oldmdbfile = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Path + Database + ";Persist Security Info=True;Jet OLEDB:Database Password='" + Password + "'";
                newmdbfile = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Path + Database2 + ";Persist Security Info=True;Jet OLEDB:Database Password='" + Password + "'";
                
            }
            

            string oldmdbfilepath = Path +  Database;
            string newmdbfilepath = Path + Database2;

            //JRO.JetEngine engine = new JetEngine();
            //engine.CompactDatabase(oldmdbfile, newmdbfile);
            File.Delete(oldmdbfilepath);
            File.Move(newmdbfilepath, oldmdbfilepath);
            MessageBox.Show("Database compact and repaired successfully ");

        }

        private void textBox9_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox9);
        }

        private void textBox9_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this,e.KeyCode);
        }

        private void textBox9_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox9);
        }

        private void groupBox7_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox4_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox5_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox12_Enter(object sender, EventArgs e)
        {

        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void textBox10_KeyPress(object sender, KeyPressEventArgs e)
        {
            strCombo = "select Sname As State from State order by Sname";
            

            textBox10.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);


        }

        private void textBox10_KeyDown(object sender, KeyEventArgs e)
        {
            ////strCombo = "select Sname As State from State order by Sname";


            ////textBox10.Text = SelectCombo.ComboKeydown(this, e.KeyCode, strCombo, e.KeyCode.ToString(), 0);
            
           

        }

        private void textBox10_Leave(object sender, EventArgs e)
        {
            //Database.lostFocus(textBox10);
        }

        private void textBox10_Enter(object sender, EventArgs e)
        {
            //Database.setFocus(textBox10);
        }

        private void groupBox11_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox16_Enter(object sender, EventArgs e)
        {

        }

        private void textBox12_KeyDown(object sender, KeyEventArgs e)
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

        private void textBox11_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox11);
        }

        private void textBox11_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox11_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox11);
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

        private void textBox14_KeyPress(object sender, KeyPressEventArgs e)
        {
            strCombo = "SELECT [name] from DeliveryPoint";
            textBox14.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
        }

        private void textBox14_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox14);
        }

        private void textBox14_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox14);
        }

    }
}
