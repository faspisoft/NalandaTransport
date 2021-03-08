using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Data.OleDb;
using Microsoft.Office.Core;
using Excel = Microsoft.Office.Interop.Excel;
using System.Web.Script.Serialization;
using Microsoft.SqlServer.Management.Smo;
using System.Data.SqlClient;
using FaspiLicenceModel;

using Marwari;

namespace faspi
{
    public partial class frm_main : Form
    {
        public static DataTable dtDisplay1 = new DataTable();
        public static DataTable dtDisplay2 = new DataTable();
        public int random;
        public string createledger = "";
        FlowLayoutPanel flp;

        public frm_main()
        {
            InitializeComponent();
        }

        private void accountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmMaster frm = new frmMaster();
            frm.MdiParent = this;
            frm.LoadData("Account", "Account");
            frm.Show();
        }
       
        private void cashBookToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report gg = new Report();
            gg.MdiParent = this;
            gg.CashBook(Database.stDate, Database.ldate);
            gg.Show();
        }

        private void ledgerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report gg = new Report();
            gg.MdiParent = this;
            string strCombo = funs.GetStrCombo("*");
            char cg = 'a';
            string selected = SelectCombo.ComboKeypress(this, cg, strCombo, "", 1);
            gg.Ledger(Database.stDate, Database.ldate, selected);
            gg.Show();
        }

        private void movedAccountSummaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report gg = new Report();
            gg.MdiParent = this;
            gg.MovedAccountSummary(Database.stDate, Database.ldate);
            gg.Show();
        }

        private void balanceSheetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report gg = new Report();
            gg.MdiParent = this;
            gg.AccountGroupBalance(Database.stDate, Database.ldate);
            gg.Show();
        }

        private void standardTrialBalanceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report gg = new Report();
            gg.MdiParent = this;
            gg.StandardTrial(Database.stDate, Database.ldate);
            gg.Show();
        }

        public void frm_main_Load(object sender, EventArgs e)
        {
            DirectoryInfo dInfo = new System.IO.DirectoryInfo(Application.StartupPath + "\\System");

            if (dInfo.Exists == false)
            {
                Directory.CreateDirectory(Application.StartupPath + "\\System");
            }

            setMenu();
            
            statusStrip1.Items[0].Text = "Faspi Enterprises Pvt. Ltd.";
            this.Text = Database.fname + "[" + Database.fyear + "]";
            statusStrip1.Items[2].Text = Database.ExeDate.ToString("yy.M.d");
            statusStrip1.Items[4].Text = Database.uname;
            statusStrip1.Items[6].Text = Database.ldate.ToString(Database.dformat);

            statusStrip1.Items[9].Text = "+91 83070 71699";
            statusStrip1.Items[11].Text = Database.fyear;

            FileInfo fInfo = new FileInfo(Application.StartupPath + "\\System\\" + Database.fname + ".jpg");
            if (fInfo.Exists)
            {
                this.BackgroundImage = new Bitmap(Application.StartupPath + "\\System\\" + Database.fname + ".jpg");
                this.BackgroundImageLayout = ImageLayout.Stretch;
            }

            fInfo = null;
            bwReminder.RunWorkerAsync();

        }

        private void frm_main_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult ch = MessageBox.Show(null, "Are you sure to exit?", "Confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (ch == DialogResult.OK)
            {
                Dongle.cllogout();
                funs.notifyIcon.Visible = false;
                GC.Collect();
                Environment.Exit(0);
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void balanceSheetToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Report gg = new Report();
            gg.MdiParent = this;
            gg.BalanceSheet(Database.stDate, Database.ldate);
            gg.Show();
        }

        private void openingTrialBalanceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report gg = new Report();
            gg.MdiParent = this;
            gg.OpeningTrial(Database.stDate, Database.stDate);
            gg.Show();
        }

        private void groupedTrialBalanceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report gg = new Report();
            gg.MdiParent = this;
            gg.GroupedTrial(Database.stDate, Database.ldate);
            gg.Show();
        }

        private void profitLossStatementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report gg = new Report();
            gg.MdiParent = this;
            gg.ProfitAndLoss(Database.stDate, Database.ldate);
            gg.Show();
        }

        public static void clearDisplay()
        {
            dtDisplay1.Clear();
            dtDisplay2.Clear();
            dtDisplay1.Columns.Clear();
            dtDisplay2.Columns.Clear();
        }

        public static void clearDisplay1()
        {
            dtDisplay1.Clear();
            dtDisplay1.Columns.Clear();
        }

        public static void clearDisplay2()
        {
            dtDisplay2.Clear();
            dtDisplay2.Columns.Clear();
        }

        private void newFirmToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frm_NewCompany frm = new frm_NewCompany();
            frm.MdiParent = this;
            frm.frmMenuTyp = "New Company";
            frm.LoadData(0, "New Company");
            frm.Show();
        }

        private void databaseBackupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmbackup frm = new frmbackup();
            frm.MdiParent = this;
            frm.frmMenuTyp = "Backup";
            frm.Text = "Backup Firm";
            frm.Show();
        }

        private void deleteFirmToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmbackup frm = new frmbackup();
            frm.MdiParent = this;
            frm.frmMenuTyp = "Delete";
            frm.Text = "Delete Firm";
            frm.Show();
        }

        private void toolStripMenuItem26_Click(object sender, EventArgs e)
        {
            frm_voutype frm = new frm_voutype();
            frm.MdiParent = this;
            frm.Show();
        }

        public void setMenu()
        {
            if (Database.databaseName != "")
            {
                frm_flowlayout frmn = new frm_flowlayout();
                frmn.MdiParent = this;
                frmn.Show();
                statusStrip1.Items[6].Text = Database.ldate.ToString(Database.dformat);
                statusStrip1.Items[11].Text = Database.fyear;
                this.Text = Database.fname;
                statusStrip1.Items[2].Text = Database.ExeDate.ToString("yy.M.d");
                statusStrip1.Items[4].Text = Database.uname;
                //setupToolStripMenuItem.Visible = true;
                saleToolStripMenuItem.Visible = true;
                toolToolStripMenuItem.Visible = true;
                exitToolStripMenuItem.Visible = true;
                settingsToolStripMenuItem.Visible = true;
                statusStrip1.Items[12].Text = Database.LocationNikName;

                if (flp != null)
                {
                    flp.Dispose();
                }
                flp = new FlowLayoutPanel();
                


                if (Database.DatabaseType == "sql")
                {
                    createNewFinancialYearToolStripMenuItem.Visible = false;
                    deleteFirmToolStripMenuItem.Visible = false;
                    dataRestoreToolStripMenuItem.Visible = false;
                }
                else
                {
                    createNewFinancialYearToolStripMenuItem.Visible = true;
                    deleteFirmToolStripMenuItem.Visible = true;
                    dataRestoreToolStripMenuItem.Visible = true;
                }

                if (Database.utype == "Admin")
                {
                    workStationsToolStripMenuItem.Visible = true;

                }
             


                DataTable dtdiff = new DataTable();
                Database.GetSqlData("select distinct Vdate from voucherinfos", dtdiff);
                int count = 0;
                count = dtdiff.Rows.Count;
               // MessageBox.Show(count + "");
                //if (count >= 1035)
                //{
                //    setupToolStripMenuItem.Visible = false;
                //    settingsToolStripMenuItem.Visible = false;
                //    toolToolStripMenuItem.Visible = false;
                //    exitToolStripMenuItem.Visible = false;
                //    setupToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.None;
                //    settingsToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.None;
                //    toolToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.None;
                //    exitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.None;
                //    setupToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.None;

                //    frmn.Hide();
                //}
                //if (count >= 1060)
                //{
                //    setupToolStripMenuItem.Visible = false;
                //    settingsToolStripMenuItem.Visible = false;
                //    toolToolStripMenuItem.Visible = false;
                //    exitToolStripMenuItem.Visible = false;
                //    saleToolStripMenuItem.Visible = false;
                //    setupToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.None;
                //    settingsToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.None;
                //    toolToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.None;
                //    exitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.None;
                //    saleToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.None;

                //    frmn.Hide();
                //}
            }

            else if (Database.databaseName == "")
            {
                saleToolStripMenuItem.Visible = false;
                setupToolStripMenuItem.Visible = false;
                toolToolStripMenuItem.Visible = false;
                exitToolStripMenuItem.Visible = false;
                settingsToolStripMenuItem.Visible = false;
            }            
            else if (Dongle.getDongleNumber() == "AFFA1041")
            {
              
            }
            if (Database.utype == "Admin")
            {
                userManagementToolStripMenuItem.Visible = true;
            }
            else
            {
                userManagementToolStripMenuItem.Visible = false;
            }
        }

        private void desktopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ofd.Filter = "JPEG Files(*.jpg) | *.jpg";
            if (DialogResult.OK == ofd.ShowDialog())
            {
                this.BackgroundImage = new Bitmap(ofd.FileName);
                this.BackgroundImageLayout = ImageLayout.Stretch;
                GC.Collect();
                File.Copy(ofd.FileName, Application.StartupPath + "\\System\\" + Database.fname + ".jpg", true);
                MessageBox.Show("Done");
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            statusStrip1.Items[7].Text = DateTime.Now.ToLongTimeString();
        }

        private void Restore()
        {
            DialogResult val = ofd.ShowDialog(this);
            DataTable dt = new DataTable();
            if (val == DialogResult.OK)
            {
                System.Data.OleDb.OleDbConnection Conn = new System.Data.OleDb.OleDbConnection();
                Conn.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + ofd.FileName + ";Persist Security Info=true;Jet OLEDB:Database Password=ptsoft9358524971";
                Conn.Open();
                string str = "select * from company";
                System.Data.OleDb.OleDbDataAdapter da = new System.Data.OleDb.OleDbDataAdapter(str, Conn);

                da.Fill(dt);

                Conn.Close();
                DataTable dtckeck = new DataTable();

                Database.GetOtherSqlData("select * from firminfo where Firm_name= '" + dt.Rows[0]["Name"] + "' and Firm_Period_name= '" + dt.Rows[0]["Firm_Period_name"] + "'", dtckeck);

                if (dtckeck.Rows.Count != 0)
                {
                    DirectoryInfo dir = new DirectoryInfo(Application.StartupPath + "\\System");
                    bool ch = dir.Exists;
                    if (ch == false)
                    {
                        dir.Create();
                    }
                    string PathtoRestoreFrom;
                    PathtoRestoreFrom = ofd.FileName;
                    File.Copy(Application.StartupPath + "\\Database\\" + dtckeck.Rows[0]["Firm_database"] + ".mdb", Application.StartupPath + "\\System\\" + DateTime.Now.ToString("yyyyMMddHHmmss"));
                    File.Copy(PathtoRestoreFrom, Application.StartupPath + "\\Database\\" + dtckeck.Rows[0]["Firm_database"] + ".mdb", true);
                    MessageBox.Show("Restore Successfull");

                }
                else
                {
                    MessageBox.Show("Firm/Company Not Found in Database" + Environment.NewLine + "Firm Name: " + dt.Rows[0]["Name"] + Environment.NewLine + "Period: " + dt.Rows[0]["Firm_Period_name"]);
                }
            }
        }

        private void testToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PdfReader frm = new PdfReader();
            frm.Show();
        }

        private void groupLedgerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report gg = new Report();
            gg.MdiParent = this;
            gg.GroupLedger(Database.stDate, Database.ldate);
            gg.Show();
        }

        private void mToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frm_modifyopn frm = new frm_modifyopn();
            frm.MdiParent = this;
            frm.LoadData();
            frm.Show();
        }

        private void frm_main_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {


                DataTable dt = new DataTable();
                Database.GetSqlData("select * from USERs where UserName='" + Database.uname + "' and Password='" + Database.upass + "'", dt);
                string location = "";
                if (dt.Rows.Count == 1)
                {
                    location = Database.GetScalarText("Select location_id from USERs where UserName='" + Database.uname + "' and Password='" + Database.upass + "'");
                }
                if (location == "")
                {
                    Form[] frms = this.MdiChildren;
                    foreach (Form frm in frms)
                    {
                        frm.Dispose();
                    }
                    frm_switchFirm frmS = new frm_switchFirm();
                    frmS.ShowDialog();
               
                    setMenu();
                 
                }
            }
            else if (e.Control == false && e.KeyCode == Keys.F11)
            {
                frmMaster frm = new frmMaster();
                frm.MdiParent = this;
                frm.LoadData("Control Room", "Control Room");
                frm.Show();
            }

            else if (e.Control == false && e.KeyCode == Keys.F12)
            {
                frmMaster frm = new frmMaster();
                frm.MdiParent = this;
                frm.LoadData("TransactionSetup", "TransactionSetup");
                frm.Show();
            }

           

            else if (e.Control && e.Alt && e.KeyCode == Keys.U)
            {
                InputBox box = new InputBox("Enter Password", "", true);
                box.outStr = "";
                box.ShowInTaskbar = false;
                box.ShowDialog(this);

                if (box.outStr == "admin")
                {
                    if (Database.DatabaseType == "access")
                    {
                        Database.CommandExecutor("UPDATE Voucherdet LEFT JOIN Description ON Voucherdet.Des_ac_id = Description.Des_id SET Voucherdet.Rate_Unit = [description].[Rate_Unit],Voucherdet.Pvalue = [description].[Pvalue]");
                    }
                    else
                    {
                        Database.CommandExecutor("UPDATE Voucherdet SET Voucherdet.Rate_Unit = [description].[Rate_Unit] FROM   description, Voucherdet WHERE description.des_id = voucherdet.des_ac_id,,Voucherdet.Pvalue = [description].[Pvalue]");
                    }
                    MessageBox.Show("Done");
                }
                else
                {
                    MessageBox.Show("Wrong Password.");
                }
            }

            else if (e.Control && e.Alt && e.KeyCode == Keys.R)
            {
                InputBox box = new InputBox("Enter Password", "", true);
                box.outStr = "";
                box.ShowInTaskbar = false;
                box.ShowDialog(this);

                if (box.outStr == "SURE")
                {
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
                    Database.CommandExecutor("Delete from Vouchertype where Type='Sale' and A=" + access_sql.Singlequote + "true" + access_sql.Singlequote + "");
                    Database.CommandExecutor("Delete from Vouchertype where Type='Return' and A=" + access_sql.Singlequote + "true" + access_sql.Singlequote + " ");
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
                    Database.CommandExecutor("Alter table Vouchertype Drop AllowedAcc");
                    Database.CommandExecutor("insert into VOUCHERTYPE ([Name],[Type],[Stationary],[Numtype],[Short],[AliasName],[Default1],[Default2],[Default3],[ReportName],[Code],[Effect_On_Stock],[Effect_On_Acc],[IncludingTax],[ExcludingTax],[ExState],[TaxInvoice],[Unregistered],[Active],[PaperSize],[SmsTemplate],[A],[B],[printcopy],[CashTransaction],[Calculation],[Postfix],[Prefix],[Padding],[Exempted]) values('Bill of Supply','Sale'," + access_sql.Singlequote + "true" + access_sql.Singlequote + ",1,'SLB','Bill of Supply','Original Copy','Duplicate Copy','Office Copy','GSTBOSA4.rpt','SLB','Y','Y'," + access_sql.Singlequote + "true" + access_sql.Singlequote + "," + access_sql.Singlequote + "true" + access_sql.Singlequote + "," + access_sql.Singlequote + "false" + access_sql.Singlequote + "," + access_sql.Singlequote + "false" + access_sql.Singlequote + "," + access_sql.Singlequote + "false" + access_sql.Singlequote + "," + access_sql.Singlequote + "true" + access_sql.Singlequote + ",'A4','Dear Customer, Thanks For Purchasing With Us, Your Last Bill No is: {Vno}'," + access_sql.Singlequote + "true" + access_sql.Singlequote + "," + access_sql.Singlequote + "true" + access_sql.Singlequote + ",'Original for Recipient,True;Duplicate for Transporter,True;Triplicate for Supplier,True;','Only Allowed','Default Excluding Tax','','B-',6,'Allowed')");
                    Database.CommandExecutor("insert into VOUCHERTYPE ([Name],[Type],[Stationary],[Numtype],[Short],[AliasName],[Default1],[Default2],[Default3],[ReportName],[Code],[Effect_On_Stock],[Effect_On_Acc],[IncludingTax],[ExcludingTax],[ExState],[TaxInvoice],[Unregistered],[Active],[PaperSize],[SmsTemplate],[A],[B],[printcopy],[CashTransaction],[Calculation],[Postfix],[Prefix],[Padding],[Exempted]) values('Bill of Supply Return','Return'," + access_sql.Singlequote + "true" + access_sql.Singlequote + ",1,'REB','Bill of Supply Return','Original Copy','Duplicate Copy','Office Copy','GSTBOSA4.rpt','REB','Y','Y'," + access_sql.Singlequote + "true" + access_sql.Singlequote + "," + access_sql.Singlequote + "true" + access_sql.Singlequote + "," + access_sql.Singlequote + "false" + access_sql.Singlequote + "," + access_sql.Singlequote + "false" + access_sql.Singlequote + "," + access_sql.Singlequote + "false" + access_sql.Singlequote + "," + access_sql.Singlequote + "true" + access_sql.Singlequote + ",'A4','Dear Customer, Thanks For Purchasing With Us, Your Last Bill No is: {Vno}'," + access_sql.Singlequote + "true" + access_sql.Singlequote + "," + access_sql.Singlequote + "true" + access_sql.Singlequote + ",'Original for Recipient,True;Duplicate for Transporter,True;Triplicate for Supplier,True;','Only Allowed','Default Excluding Tax','','BR-',6,'Allowed')");
                    Database.CommandExecutor("insert into VOUCHERTYPE ([Name],[Type],[Stationary],[Numtype],[Short],[AliasName],[Default1],[Default2],[Default3],[ReportName],[Code],[Effect_On_Stock],[Effect_On_Acc],[IncludingTax],[ExcludingTax],[ExState],[TaxInvoice],[Unregistered],[Active],[PaperSize],[SmsTemplate],[A],[B],[printcopy],[CashTransaction],[Calculation],[Postfix],[Prefix],[Padding],[Exempted]) values('Tax Invoice','Sale'," + access_sql.Singlequote + "true" + access_sql.Singlequote + ",1,'SLT','Tax Invoice','Original Copy','Duplicate Copy','Office Copy','GSTTIA4.rpt','SLT','Y','Y'," + access_sql.Singlequote + "true" + access_sql.Singlequote + "," + access_sql.Singlequote + "true" + access_sql.Singlequote + "," + access_sql.Singlequote + "false" + access_sql.Singlequote + "," + access_sql.Singlequote + "true" + access_sql.Singlequote + "," + access_sql.Singlequote + "false" + access_sql.Singlequote + "," + access_sql.Singlequote + "true" + access_sql.Singlequote + ",'A4','Dear Customer, Thanks For Purchasing With Us, Your Last Bill No is: {Vno}'," + access_sql.Singlequote + "true" + access_sql.Singlequote + "," + access_sql.Singlequote + "true" + access_sql.Singlequote + ",'Original for Recipient,True;Duplicate for Transporter,True;Triplicate for Supplier,True;','Not Allowed','Default Excluding Tax','','T-',6,'Not Allowed')");
                    Database.CommandExecutor("insert into VOUCHERTYPE ([Name],[Type],[Stationary],[Numtype],[Short],[AliasName],[Default1],[Default2],[Default3],[ReportName],[Code],[Effect_On_Stock],[Effect_On_Acc],[IncludingTax],[ExcludingTax],[ExState],[TaxInvoice],[Unregistered],[Active],[PaperSize],[SmsTemplate],[A],[B],[printcopy],[CashTransaction],[Calculation],[Postfix],[Prefix],[Padding],[Exempted]) values('Tax Invoice Return','Return'," + access_sql.Singlequote + "true" + access_sql.Singlequote + ",1,'RET','Tax Invoice Return','Original Copy','Duplicate Copy','Office Copy','GSTTIA4.rpt','RET','Y','Y'," + access_sql.Singlequote + "true" + access_sql.Singlequote + "," + access_sql.Singlequote + "true" + access_sql.Singlequote + "," + access_sql.Singlequote + "false" + access_sql.Singlequote + "," + access_sql.Singlequote + "true" + access_sql.Singlequote + "," + access_sql.Singlequote + "false" + access_sql.Singlequote + "," + access_sql.Singlequote + "true" + access_sql.Singlequote + ",'A4','Dear Customer, Thanks For Purchasing With Us, Your Last Bill No is: {Vno}'," + access_sql.Singlequote + "true" + access_sql.Singlequote + "," + access_sql.Singlequote + "true" + access_sql.Singlequote + ",'Original for Recipient,True;Duplicate for Transporter,True;Triplicate for Supplier,True;','Not Allowed','Default Excluding Tax','','TR-',6,'Not Allowed')");


                    MessageBox.Show("Repaired");
                }
            }
        }

        private void detailLedgerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report gg = new Report();
            gg.MdiParent = this;
            string strCombo = funs.GetStrCombo("*");
            char cg = 'a';
            string selected = SelectCombo.ComboKeypress(this, cg, strCombo, "", 1);
            gg.DetailLedger(Database.stDate, Database.ldate, selected);
            gg.Show();
        }

        private void tBalanceSheetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report gg = new Report();
            gg.MdiParent = this;
            gg.BalanceSheet(Database.stDate, Database.ldate);
            gg.Show();
        }

        private void tProfitLossToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report gg = new Report();
            gg.MdiParent = this;
            gg.ProfitAndLoss(Database.stDate, Database.ldate);
            gg.Show();
        }

        private void exitToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            DialogResult ch = MessageBox.Show(null, "Are you sure to exit?", "Confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (ch == DialogResult.OK)
            {
                int remainder = int.Parse(DateTime.Now.ToString("dd")) % 2;
                if (Database.databaseName != "")
                {
                    if (Database.DatabaseType == "access")
                    {
                        if (Feature.Available("Auto Backup") == "Yes")
                        {
                            if (Feature.Available("Auto Backup Style") == "Smart")
                            {

                                File.Copy(Application.StartupPath + "\\Database\\" + Database.databaseName + ".mdb", Application.StartupPath + "\\Backup\\" + Database.databaseName + "M" + DateTime.Now.ToString("MM"), true);
                                File.Copy(Application.StartupPath + "\\Database\\" + Database.databaseName + ".mdb", Application.StartupPath + "\\Backup\\" + Database.databaseName + "D" + DateTime.Now.ToString("dd"), true);
                            }
                            else if (Feature.Available("Auto Backup Style") == "Even-Odd")
                            {
                                File.Copy(Application.StartupPath + "\\Database\\" + Database.databaseName + ".mdb", Application.StartupPath + "\\Backup\\" + Database.databaseName + remainder, true);
                            }
                        }
                        else if (Feature.Available("Auto Backup") == "Ask")
                        {
                            DialogResult chbackup = MessageBox.Show(null, "Are you want to take Backup of this Firm?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (chbackup == DialogResult.Yes)
                            {
                                if (Feature.Available("Auto Backup Style") == "Smart")
                                {

                                    File.Copy(Application.StartupPath + "\\Database\\" + Database.databaseName + ".mdb", Application.StartupPath + "\\Backup\\" + Database.databaseName + "M" + DateTime.Now.ToString("MM"), true);
                                    File.Copy(Application.StartupPath + "\\Database\\" + Database.databaseName + ".mdb", Application.StartupPath + "\\Backup\\" + Database.databaseName + "D" + DateTime.Now.ToString("dd"), true);
                                }
                                else if (Feature.Available("Auto Backup Style") == "Even-Odd")
                                {
                                    File.Copy(Application.StartupPath + "\\Database\\" + Database.databaseName + ".mdb", Application.StartupPath + "\\Backup\\" + Database.databaseName + remainder, true);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (Feature.Available("Auto Backup") == "Yes")
                        {
                            if (Feature.Available("Auto Backup Style") == "Smart")
                            {

                                Database.CommandExecutor("Backup database " + Database.databaseName + " to disk='" + Application.StartupPath + "\\Backup\\S" + Database.databaseName + "M" + DateTime.Now.ToString("MM") + "' ");
                                Database.CommandExecutor("Backup database " + Database.databaseName + " to disk='" + Application.StartupPath + "\\Backup\\S" + Database.databaseName + "D" + DateTime.Now.ToString("dd") + "' ");
                            }
                            else if (Feature.Available("Auto Backup Style") == "Even-Odd")
                            {

                                Database.CommandExecutor("Backup database " + Database.databaseName + " to disk='" + Application.StartupPath + "\\Backup\\S" + Database.databaseName + remainder + "' ");
                            }

                        }
                        else if (Feature.Available("Auto Backup") == "Ask")
                        {
                            DialogResult chbackup = MessageBox.Show(null, "Are you want to take Backup of this Firm?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (chbackup == DialogResult.Yes)
                            {
                                if (Feature.Available("Auto Backup Style") == "Smart")
                                {

                                    Database.CommandExecutor("Backup database " + Database.databaseName + " to disk='" + Application.StartupPath + "\\Backup\\S" + Database.databaseName + "M" + DateTime.Now.ToString("MM") + "' ");
                                    Database.CommandExecutor("Backup database " + Database.databaseName + " to disk='" + Application.StartupPath + "\\Backup\\S" + Database.databaseName + "D" + DateTime.Now.ToString("dd") + "' ");
                                }
                                else if (Feature.Available("Auto Backup Style") == "Even-Odd")
                                {

                                    Database.CommandExecutor("Backup database " + Database.databaseName + " to disk='" + Application.StartupPath + "\\Backup\\S" + Database.databaseName + remainder + "' ");
                                }

                            }
                        }
                    }
                }
                Dongle.cllogout();
                funs.notifyIcon.Visible = false;
                GC.Collect();
                Environment.Exit(0);
            }
        }

        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmChangePass frm = new frmChangePass();
            frm.MdiParent = this;
            frm.LoadData(Database.uname, "Change Password");
            frm.Show();
        }

        private void accountToolStripMenuItem1_Click_1(object sender, EventArgs e)
        {
            frmMaster frm = new frmMaster();
            frm.MdiParent = this;
            frm.LoadData("Account", "Account");
            frm.Show();
        }

        private void agentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmMaster frm = new frmMaster();
            frm.MdiParent = this;
            frm.LoadData("Broker", "Broker");
            frm.Show();
        }

        private void taxCategoyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmMaster frm = new frmMaster();
            frm.MdiParent = this;
            frm.LoadData("TaxCategory", "TaxCategory");
            frm.Show();
        }

        private void discountChargesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmMaster frm = new frmMaster();
            frm.MdiParent = this;
            frm.LoadData("Charges", "Charges");
            frm.Show();
        }

        private void dataRestoreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Restore();
        }

        private void changeBackgroundImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ofd.Filter = "JPEG Files(*.jpg) | *.jpg";
            if (DialogResult.OK == ofd.ShowDialog())
            {
                this.BackgroundImage = new Bitmap(ofd.FileName);
                this.BackgroundImageLayout = ImageLayout.Stretch;
                GC.Collect();
                File.Copy(ofd.FileName, Application.StartupPath + "\\System\\" + Database.fname + ".jpg", true);
                MessageBox.Show("Done");
            }
        }

        private void createNewFinancialYearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frm_NewCompany frm = new frm_NewCompany();
            frm.frmMenuTyp = "New Financial Year";
            frm.NewFinancial("New Fianncial Year");
            frm.ShowDialog();

            Form[] frms = this.MdiChildren;
            foreach (Form frm1 in frms)
            {
                frm1.Dispose();
            }

            setMenu();
            statusStrip1.Items[2].Text = Database.ExeDate.ToString("yy.M.d");
            statusStrip1.Items[4].Text = Database.ldate.ToString(Database.dformat);
            statusStrip1.Items[9].Text = "+91 83070 71699";
            this.Text = Database.fname + "[" + Database.fyear + "]";
        }

        private void firmInformationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frm_companyinfo frm = new frm_companyinfo();
            DataTable dtFirm = new DataTable("Location");
            Database.GetSqlData("select * from Location where locationid='" + Database.LocationId + "'", dtFirm);
            if (dtFirm.Rows.Count > 0)
            {
                frm.LoadData(dtFirm.Rows[0]["locationid"].ToString(), "Modify BranchInfo");
                
                frm.ShowDialog(this);
                this.Text = Database.fname + "[" + Database.fyear + "]";
            }
        }

        private void dataBackupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SqlConnection SqlConn = new SqlConnection();
            SqlDataAdapter da;
            DataTable ds = new DataTable();

            SqlConn.ConnectionString = @"Data Source=" + Database.inipath + ";Initial Catalog=" + Database.databaseName + ";Persist Security Info=True;User ID=" + Database.sqlseveruser + ";password=" + Database.sqlseverpwd + ";Connection Timeout=100";
            SqlConn.Open();

            string folder = "Backup\\" + Database.ldate.ToString("yyyyMMddHHmsss") + "Backup";
            System.IO.Directory.CreateDirectory(folder);

            ds = new DataTable("Accounts");
            da = new SqlDataAdapter("select * from Accounts", SqlConn);
            da.Fill(ds);
            ds.WriteXml(folder + "\\" + ds.TableName + ".xml");

            ds = new DataTable("CONTRACTORs");
            da = new SqlDataAdapter("select * from CONTRACTORs", SqlConn);
            da.Fill(ds);
            ds.WriteXml(folder + "\\" + ds.TableName + ".xml");


            ds = new DataTable("DeliveryPoints");
            da = new SqlDataAdapter("select * from DeliveryPoints", SqlConn);
            da.Fill(ds);
            ds.WriteXml(folder + "\\" + ds.TableName + ".xml");

            ds = new DataTable("Gaddis");
            da = new SqlDataAdapter("select * from Gaddis", SqlConn);
            da.Fill(ds);
            ds.WriteXml(folder + "\\" + ds.TableName + ".xml");

            ds = new DataTable("ItemDetails");
            da = new SqlDataAdapter("select * from ItemDetails", SqlConn);
            da.Fill(ds);
            ds.WriteXml(folder + "\\" + ds.TableName + ".xml");

            ds = new DataTable("items");
            da = new SqlDataAdapter("select * from items", SqlConn);
            da.Fill(ds);
            ds.WriteXml(folder + "\\" + ds.TableName + ".xml");

            ds = new DataTable("Location");
            da = new SqlDataAdapter("select * from Location", SqlConn);
            da.Fill(ds);
            ds.WriteXml(folder + "\\" + ds.TableName + ".xml");

            ds = new DataTable("OTHERs");
            da = new SqlDataAdapter("select * from OTHERs", SqlConn);
            da.Fill(ds);
            ds.WriteXml(folder + "\\" + ds.TableName + ".xml");

            ds = new DataTable("Packings");
            da = new SqlDataAdapter("select * from Packings", SqlConn);
            da.Fill(ds);
            ds.WriteXml(folder + "\\" + ds.TableName + ".xml");            

            ds = new DataTable("PARTYRATEs");
            da = new SqlDataAdapter("select * from PARTYRATEs", SqlConn);
            da.Fill(ds);
            ds.WriteXml(folder + "\\" + ds.TableName + ".xml");

            ds = new DataTable("States");
            da = new SqlDataAdapter("select * from States", SqlConn);
            da.Fill(ds);
            ds.WriteXml(folder + "\\" + ds.TableName + ".xml");

            ds = new DataTable("USERs");
            da = new SqlDataAdapter("select * from USERs", SqlConn);
            da.Fill(ds);
            ds.WriteXml(folder + "\\" + ds.TableName + ".xml");            

            ds = new DataTable("VOUCHARGESs");
            da = new SqlDataAdapter("select * from VOUCHARGESs", SqlConn);
            da.Fill(ds);
            ds.WriteXml(folder + "\\" + ds.TableName + ".xml");            

            ds = new DataTable("Voucherdets");
            da = new SqlDataAdapter("select * from Voucherdets", SqlConn);
            da.Fill(ds);
            ds.WriteXml(folder + "\\" + ds.TableName + ".xml");            

            ds = new DataTable("VOUCHERINFOs");
            da = new SqlDataAdapter("select * from VOUCHERINFOs", SqlConn);
            da.Fill(ds);
            ds.WriteXml(folder + "\\" + ds.TableName + ".xml");

            SqlConn.Close();

            MessageBox.Show("Backup Successfull");
            //frmbackup frm = new frmbackup();
            //frm.MdiParent = this;
            //frm.frmMenuTyp = "Backup";
            //frm.Text = "Backup Firm";
            //frm.Show();
        }

        private void controlRoomToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmMaster frm = new frmMaster();
            frm.LoadData("Control Room", "Control Room");
            frm.MdiParent = this;
            frm.Show();
        }

        private void tranjectionSetupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmMaster frm = new frmMaster();
            frm.LoadData("TransactionSetup", "TransactionSetup");
            frm.MdiParent = this;
            frm.Show();
        }

        private void newFinancialYearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frm_NewCompany frm = new frm_NewCompany();
            frm.frmMenuTyp = "New Financial Year";
            frm.NewFinancial("New Fianncial Year");
            frm.ShowDialog();
            setMenu();
            statusStrip1.Items[2].Text = Database.ExeDate.ToString("yy.M.d");
            statusStrip1.Items[4].Text = Database.ldate.ToString(Database.dformat);
            statusStrip1.Items[9].Text = "+91 83070 71699";
            this.Text = Database.fname + "[" + Database.fyear + "]";
        }

        private void deleteFirmToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            DialogResult ch = MessageBox.Show(null, "Are you sure to Delete? \n All Data must be Lost of Current Login Firm.", "Confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (ch == DialogResult.OK)
            {
                if (Database.databaseName != "")
                {
                    if (Database.SqlCnn.State == ConnectionState.Open)
                    {
                        Database.CloseConnection();
                    }
                    string pathbackup = Application.StartupPath + "\\System\\" + Database.databaseName + DateTime.Now.ToString("yyyyMMddhmmff") + ".bak";
                    Database.CommandExecutor("Backup database " + Database.databaseName + " to disk='" + pathbackup + "'");
                }
                funs.notifyIcon.Visible = false;
                GC.Collect();
                Environment.Exit(0);
            }
        }

        private void controlRoomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmMaster frm = new frmMaster();
            frm.MdiParent = this;
            frm.LoadData("Control Room", "Control Room");
            frm.Show();
        }

        private void transactionSetupToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            frmMaster frm = new frmMaster();
            frm.MdiParent = this;
            frm.LoadData("TransactionSetup", "TransactionSetup");
            frm.Show();
        }

        private void receiptToolStripMenuItem_Click(object sender, EventArgs e)
        {

            frmMasterVou frm = new frmMasterVou();
            frm.MdiParent = this;
            frm.LoadData("Receipt", "Receipt Vouchers");
            frm.Show();
        }

        private void paymentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmMasterVou frm = new frmMasterVou();
            frm.MdiParent = this;
            frm.LoadData("Payment", "Payment Vouchers");
            frm.Show();
        }

        private void journalToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            frmMasterVou frm = new frmMasterVou();
            frm.MdiParent = this;
            frm.LoadData("Journal", "Journal Vouchers");
            frm.Show();
        }

        private void stateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmMaster frm = new frmMaster();
            frm.MdiParent = this;
            frm.LoadData("State", "State");
            frm.Show();
        }

        private void otherDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frm_gridotherdet frm = new frm_gridotherdet();
            frm.MdiParent = this;
            frm.Show();
        }

        private void contraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmMasterVou frm = new frmMasterVou();
            frm.MdiParent = this;
            frm.LoadData("Contra", "Contra Vouchers");
            frm.Show();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmMaster frm = new frmMaster();
            frm.MdiParent = this;
            frm.LoadData("Tax", "TaxCategory");
            frm.Show();
        }

        private void paymentCollectorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmMaster frm = new frmMaster();
            frm.MdiParent = this;
            frm.LoadData("Staff", "Staff");
            frm.Show();
        }

        private void itemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmMaster frm = new frmMaster();
            frm.MdiParent = this;
            frm.LoadData("Item", "Item");
            frm.WindowState = FormWindowState.Maximized;
            frm.Show();
        }

        private void bookingToolStripMenuItem_Click(object sender, EventArgs e)
        {                        
            frmMasterVou frm = new frmMasterVou();
            frm.MdiParent = this;
            frm.LoadData("Booking", "Booking");
            frm.Show();
        }

        private void otherDetailsToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            frmOtherDetails frm = new frmOtherDetails();
            frm.MdiParent = this;
            frm.Show();
        }

        private void challanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmMasterVou frm = new frmMasterVou();
            frm.MdiParent = this;
            frm.LoadData("Stock Transfer", "Stock Transfer Voucher");
            frm.Show();
        }

        private void deliveryPointToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmMaster frm = new frmMaster();
            frm.MdiParent = this;
            frm.LoadData("Delivery Point", "Delivery Point");
            frm.Show();
        }

        private void reportFormatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmMaster frm = new frmMaster();
            frm.MdiParent = this;
            frm.LoadData("Format", "Format");
            frm.Show();
        }

        private void calculatorToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("calc");
        }

        private void brandItemGroupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmMaster frm = new frmMaster();
            frm.MdiParent = this;
            frm.LoadData("Item", "Item");
            frm.Show();
        }

        private void partyWiseRateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmMaster frm = new frmMaster();
            frm.MdiParent = this;
            frm.LoadData("Customer/Supplier Rate", "Customer/Supplier Rate");
            frm.Show();            
        }

        private void pAckingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmMaster frm = new frmMaster();
            frm.MdiParent = this;
            frm.LoadData("Packing", "Packing");
            frm.Show(); 
        }

        private void gaddiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmMaster frm = new frmMaster();
            frm.MdiParent = this;
            frm.LoadData("Gaddi", "Gaddi");
            frm.Show();
        }

        private void insuranceDueDateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report gg = new Report();
            gg.Insurance(Database.ldate, Database.ldate);
            gg.MdiParent = this;
            gg.Show();
        }

        private void permitDueDateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report gg = new Report();
            gg.permit(Database.ldate, Database.ldate);
            gg.MdiParent = this;
            gg.Show();
        }

        private void fitnessDueDateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report gg = new Report();
            gg.Fitness(Database.ldate, Database.ldate);
            gg.MdiParent = this;
            gg.Show();
        }

        private void fiveYearDueDateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report gg = new Report();
            gg.Fiveyears(Database.ldate, Database.ldate);
            gg.MdiParent = this;
            gg.Show();
        }

        private void pollutionDueDateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report gg = new Report();
            gg.Pollution(Database.ldate, Database.ldate);
            gg.MdiParent = this;
            gg.Show();
        }

        private void unloadingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmMasterVou frm = new frmMasterVou();
            frm.MdiParent = this;
            frm.LoadData("Sale", "Sale");
            frm.Show();
        }

        private void registerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frm_selector frm = new frm_selector();
            frm.MdiParent = this;
            frm.Show();
        }

        private void checkForUpdateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AutoUpdater.Start("http://faspi.in/MarwariTransport/FaspiTransportPro.xml");
        }

        private void searchGRNOToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //frm_gr_search frm = new frm_gr_search();
            //frm.MdiParent = this;
            //frm.Show();
            Frm_GrSearch frm = new Frm_GrSearch();
            frm.MdiParent = this;
            frm.Show();
        }

        private void restoreDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SqlConnection SqlConn = new SqlConnection();
            SqlConn.ConnectionString = @"Data Source=" + Database.inipath + ";Initial Catalog=" + Database.databaseName + ";Persist Security Info=True;User ID=" + Database.sqlseveruser + ";password=" + Database.sqlseverpwd + ";Connection Timeout=100";
            SqlConn.Open();
            DataTable ds = new DataTable();

            DialogResult val = folderBrowserDialog1.ShowDialog(this);
            if (val == DialogResult.OK && folderBrowserDialog1.SelectedPath != "")
            {
                DirectoryInfo d = new DirectoryInfo(folderBrowserDialog1.SelectedPath);
                FileInfo[] Files = d.GetFiles("*.xml");
                foreach (FileInfo file in Files)
                {
                    ds = new DataTable();
                    using (SqlBulkCopy sbc = new SqlBulkCopy(SqlConn.ConnectionString))
                    {
                        string[] fname = file.Name.Split('.');
                        sbc.DestinationTableName = fname[0];
                        sbc.WriteToServer(ds);
                    }
                }                
            }
            MessageBox.Show("Done");

            
            //ds.ReadXml("Backup\\bkup.xml");
            //using (SqlBulkCopy sbc = new SqlBulkCopy(SqlConn))
            //{
            //    for (int i = 0; i < ds.Tables.Count; i++)
            //    {
            //        sbc.DestinationTableName = ds.Tables[i].TableName;
            //        sbc.WriteToServer(ds.Tables[ds.Tables[i].TableName]);
            //    }
            //    sbc.Close();
            //}
        }

        private void uploadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frm_upload frm = new frm_upload();
            frm.Show();
        }

        private void userManagementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmMaster frm = new frmMaster();
            frm.MdiParent = this;
            frm.LoadData("User", "User");
            frm.Show();
        }

        private void unloadingToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmMasterVou frm = new frmMasterVou();
            frm.MdiParent = this;
            frm.LoadData("Unloading", "Unloading");
            frm.Show();            
        }

        private void listsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void chargesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmMaster frm = new frmMaster();
            frm.MdiParent = this;
            frm.LoadData("Charges", "Charges");
            frm.Show();
        }

        private void deliveryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmMasterVou frm = new frmMasterVou();
            frm.MdiParent = this;
            frm.LoadData("Delivery", "Delivery");
            frm.Show(); 
        }

        private void deliveredByToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmMaster frm = new frmMaster();
            frm.MdiParent = this;
            frm.LoadData("DeliveredBy", "DeliveredBy");
            frm.Show();
        }

        private void contraVoucherToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmMasterVou frm = new frmMasterVou();
            frm.MdiParent = this;
            frm.LoadData("Contra", "Contra Vouchers");
            frm.Show();
        }

        private void stockReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frm_stk frm = new frm_stk();
            frm.MdiParent = this;
            frm.Show();
        }

        private void challanRegisterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frm_StkTranfereg frm = new frm_StkTranfereg();
            frm.MdiParent = this;
            frm.Text = "Challan Register";
            frm.Show();
        }

        private void newVtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmMasterVou frm = new frmMasterVou();
            frm.MdiParent = this;
            frm.LoadData("Challan", "Challan");
            frm.Show(); 
        }

        private void deliveryReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frm_deliveryrpt frm = new frm_deliveryrpt();
            frm.MdiParent = this;
            frm.Show();
        }

        private void gRByChallanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmMasterVou frm = new frmMasterVou();
            frm.MdiParent = this;
            frm.LoadData("GRByChallan", "GRByChallan");
            
            frm.Show();
          
        }

        private void stockTransferRegisterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frm_StkTranfereg frm = new frm_StkTranfereg();
            frm.MdiParent = this;
            frm.Text = "Stock Transfer Register";
            frm.Show();
        }

        private void workStationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmMaster frm = new frmMaster();
            frm.MdiParent = this;
            frm.LoadData("WorkStations", "WorkStations");
            frm.Show();
        }

        private void ubookingToolStripMenuItem_Click(object sender, EventArgs e)
       {
            DateTime dtfrom = DateTime.Parse("01/11/2018");
            DateTime dtend = DateTime.Parse("05/04/2019");

            DataTable dtstk = new DataTable("Stocks");
            Database.GetSqlData("SELECT     Stocks.* FROM VOUCHERTYPEs RIGHT OUTER JOIN VOUCHERINFOs ON dbo.VOUCHERTYPEs.Vt_id = dbo.VOUCHERINFOs.Vt_id RIGHT OUTER JOIN  dbo.Stocks ON dbo.VOUCHERINFOs.Vi_id = dbo.Stocks.GR_id WHERE     (dbo.VOUCHERINFOs.Vdate >= CONVERT(DATETIME, '" + dtfrom.ToString(Database.dformat) + "', 102)) AND (dbo.VOUCHERINFOs.Vdate <= CONVERT(DATETIME, '" + dtend.ToString(Database.dformat) + "', 102)) AND (dbo.VOUCHERTYPEs.Type = N'Booking') ORDER BY dbo.Stocks.GR_id", dtstk);
            DataTable dtvouinfo = new DataTable();
            Database.GetSqlData("SELECT Voucherinfos.* FROM VOUCHERINFOs LEFT OUTER JOIN VOUCHERTYPEs ON dbo.VOUCHERINFOs.Vt_id = dbo.VOUCHERTYPEs.Vt_id WHERE  (dbo.VOUCHERTYPEs.Type = N'Booking') AND (dbo.VOUCHERINFOs.Vdate >= CONVERT(DATETIME, '" + dtfrom.ToString(Database.dformat) + "', 102) AND   VOUCHERINFOs.Vdate <= CONVERT(DATETIME, '" + dtend.ToString(Database.dformat) + "', 102))", dtvouinfo);

            DataTable dtvoudet = new DataTable();
            Database.GetSqlData("SELECT     Voucherdets.*  FROM  VOUCHERTYPEs RIGHT OUTER JOIN VOUCHERINFOs ON dbo.VOUCHERTYPEs.Vt_id = dbo.VOUCHERINFOs.Vt_id RIGHT OUTER JOIN  dbo.Voucherdets ON dbo.VOUCHERINFOs.Vi_id = dbo.Voucherdets.Vi_id WHERE     (dbo.VOUCHERTYPEs.Type = N'Booking') AND (dbo.VOUCHERINFOs.Vdate >= CONVERT(DATETIME,  '" + dtfrom.ToString(Database.dformat) + "', 102) AND   dbo.VOUCHERINFOs.Vdate <= CONVERT(DATETIME, '" + dtend.ToString(Database.dformat) + "', 102))", dtvoudet);

            for (int i = 0; i < dtvouinfo.Rows.Count; i++)
            {

              DataRow[] dr;

              dr = dtstk.Select("GR_id='" + dtvouinfo.Rows[i]["Vi_id"].ToString() + "'");

              for (int j = 0; j < dr.Length; j++)
              {
                  dr[j]["ActWeight"] = double.Parse(dtvoudet.Compute("sum(Weight)", "Vi_id='" + dtvouinfo.Rows[i]["Vi_id"].ToString() + "'").ToString());
                  //dr[j]["Consigner_id"] = dtvouinfo.Rows[i]["Ac_id"].ToString();
                  //dr[j]["Consignee_id"] = dtvouinfo.Rows[i]["Ac_id2"].ToString() ;
                  //dr[j]["Source_id"] = dtvouinfo.Rows[i]["Consigner_id"].ToString();
                  //dr[j]["Destination_id"] = dtvouinfo.Rows[i]["Sid"].ToString();

                  //if (dtvouinfo.Rows[i]["PaymentMode"].ToString() == "")
                  //{
                  //    dr[j]["GRtype"] = "FOC";
                  //}
                  //else
                  //{
                  //    dr[j]["GRtype"] = dtvouinfo.Rows[i]["PaymentMode"].ToString();
                  //}


                  //dr[j]["Deliverytype"] = dtvouinfo.Rows[i]["DeliveryType"].ToString();
                  //dr[j]["GRdate"] = dtvouinfo.Rows[i]["Vdate"].ToString();
                  //dr[j]["GRNo"] = dtvouinfo.Rows[i]["Invoiceno"].ToString();
                  //dr[j]["Private"] = dtvouinfo.Rows[i]["Transport1"].ToString();
                  //dr[j]["Remark"] = dtvouinfo.Rows[i]["DeliveryAt"].ToString();
                  //if (dr[j]["GRtype"].ToString() == "To Pay")
                  //{
                  //    dr[j]["ToPay"] = double.Parse(dtvouinfo.Rows[i]["totalamount"].ToString());
                  //    dr[j]["TBB"] = 0;
                  //    dr[j]["Paid"] = 0;
                  //    dr[j]["FOC"] = 0;
                  //}
                  //else if (dr[j]["GRtype"].ToString() == "FOC")
                  //{
                  //    dr[j]["ToPay"] = 0;
                  //    dr[j]["TBB"] = 0;
                  //    dr[j]["Paid"] = 0;
                  //    dr[j]["FOC"] = double.Parse(dtvouinfo.Rows[i]["totalamount"].ToString());
                  //}
                  //else if (dr[j]["GRtype"].ToString() == "Paid")
                  //{
                  //    dr[j]["ToPay"] = 0;
                  //    dr[j]["TBB"] = 0;
                  //    dr[j]["Paid"] = double.Parse(dtvouinfo.Rows[i]["totalamount"].ToString());
                  //    dr[j]["FOC"] = 0;
                  //}
                  //else if (dr[j]["GRtype"].ToString() == "T.B.B.")
                  //{
                  //    dr[j]["ToPay"] = 0;
                  //    dr[j]["TBB"] = double.Parse(dtvouinfo.Rows[i]["totalamount"].ToString());
                  //    dr[j]["Paid"] = 0;
                  //    dr[j]["FOC"] = 0;
                  //}
     
                  //dr[j]["totpkts"] = double.Parse(dtvoudet.Compute("sum(Quantity)", "Vi_id='" + dtvouinfo.Rows[i]["Vi_id"].ToString() + "'").ToString());
                  //dr[j]["totWeight"] = double.Parse(dtvoudet.Compute("sum(ChargedWeight)", "Vi_id='" + dtvouinfo.Rows[i]["Vi_id"].ToString() + "'").ToString());
                  //double othch = 0;
                  //othch = double.Parse(dtvouinfo.Rows[i]["Roff"].ToString()) + double.Parse(dtvoudet.Compute("sum(exp2amt)", "Vi_id='" + dtvouinfo.Rows[i]["Vi_id"].ToString() + "'").ToString()) + double.Parse(dtvoudet.Compute("sum(exp3amt)", "Vi_id='" + dtvouinfo.Rows[i]["Vi_id"].ToString() + "'").ToString()) + double.Parse(dtvoudet.Compute("sum(exp4amt)", "Vi_id='" + dtvouinfo.Rows[i]["Vi_id"].ToString() + "'").ToString()) + double.Parse(dtvoudet.Compute("sum(exp5amt)", "Vi_id='" + dtvouinfo.Rows[i]["Vi_id"].ToString() + "'").ToString()) + double.Parse(dtvoudet.Compute("sum(exp6amt)", "Vi_id='" + dtvouinfo.Rows[i]["Vi_id"].ToString() + "'").ToString()) + double.Parse(dtvoudet.Compute("sum(exp7amt)", "Vi_id='" + dtvouinfo.Rows[i]["Vi_id"].ToString() + "'").ToString()) + double.Parse(dtvoudet.Compute("sum(exp8amt)", "Vi_id='" + dtvouinfo.Rows[i]["Vi_id"].ToString() + "'").ToString()) + double.Parse(dtvoudet.Compute("sum(exp9amt)", "Vi_id='" + dtvouinfo.Rows[i]["Vi_id"].ToString() + "'").ToString()) + double.Parse(dtvoudet.Compute("sum(exp10amt)", "Vi_id='" + dtvouinfo.Rows[i]["Vi_id"].ToString() + "'").ToString());
                  //dr[j]["OthCharge"] = othch;
                  //dr[j]["GRCharge"] = double.Parse(dtvoudet.Compute("sum(exp1amt)", "Vi_id='" + dtvouinfo.Rows[i]["Vi_id"].ToString() + "'").ToString());
                  //dr[j]["Freight"] = double.Parse(dtvoudet.Compute("sum(Amount)", "Vi_id='" + dtvouinfo.Rows[i]["Vi_id"].ToString() + "'").ToString());
                  //string itemname = "";
                  //if (dtvoudet.Select("Vi_id='" + dtvouinfo.Rows[i]["Vi_id"].ToString() + "'").Length == 1)
                  //{
                  //    itemname = dtvoudet.Select("Vi_id='" + dtvouinfo.Rows[i]["Vi_id"].ToString() + "'  And Itemsr=1").FirstOrDefault()["Description"].ToString();
                  //}
                  //else
                  //{
                  //    int l =  dtvoudet.Select("Vi_id='" + dtvouinfo.Rows[i]["Vi_id"].ToString() + "'").Length - 1;
                  //    itemname = dtvoudet.Select("Vi_id='" + dtvouinfo.Rows[i]["Vi_id"].ToString() + "' And Itemsr=1").FirstOrDefault()["Description"].ToString() + " + "+ l.ToString();
                  //}
                  
                  //dr[j]["itemname"] = itemname;
                  //dr[j]["Packing"] = dtvoudet.Select("Vi_id='" + dtvouinfo.Rows[i]["Vi_id"].ToString() + "' And Itemsr=1").FirstOrDefault()["packing"];
              }

            }

            Database.SaveData(dtstk);
            MessageBox.Show("Done");

        }

        private void gSTReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
        }

        private void bookingToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Report gg = new Report();
            gg.MdiParent = this;
            gg.GSTBooking(Database.ldate, Database.ldate);
            gg.Show();
        }

        private void unloadingChallanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report gg = new Report();
            gg.MdiParent = this;
            gg.GSTUnloadingChallan(Database.ldate, Database.ldate);
            gg.Show();
        }

        private void deliveryBillToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmMasterVou frm = new frmMasterVou();
            frm.MdiParent = this;
            frm.LoadData("DBill", "Delivery Bill");
            frm.Show();
        }
        private List<VMProductmsg> LoadReminder()
        {
            try
            {
                List<VMProductmsg> objResList = new List<VMProductmsg>();
                string prdKey = Database.Dongleno;
                if (prdKey == "")
                {
                    return objResList;
                }
                FaspiApiHandler.BLPrdMessage ob = new FaspiApiHandler.BLPrdMessage();
                List<FaspiLicenceModel.VMProductmsg> ol = ob.GetPrdKeyMessages(prdKey);

                if (ol != null)
                {
                    string strJSON = (new JavaScriptSerializer()).Serialize(ol);
                    strJSON = FaspiBL.Functions.Encrypt(strJSON);
                    System.IO.File.WriteAllText(Application.StartupPath + "\\sysconfig.dll", strJSON);
                }

                if (System.IO.File.Exists(Application.StartupPath + "\\sysconfig.dll"))
                {
                    string strJSON = System.IO.File.ReadAllText(Application.StartupPath + "\\sysconfig.dll");
                    strJSON = FaspiBL.Functions.Decrypt(strJSON);
                    objResList = (new JavaScriptSerializer()).Deserialize<List<VMProductmsg>>(strJSON);

                    return objResList.Where(w => w.IsSuspend == false && w.ProductKey == prdKey && w.StartOn <= DateTime.Now && w.ExpireOn >= DateTime.Now).OrderBy(o => o.Priority).ToList();

                }

            }
            catch (Exception ex)
            {

            }
            return null;
        }


        private void bwReminder_DoWork(object sender, DoWorkEventArgs e)
        {
            e.Result = LoadReminder();
        }

        private void bwReminder_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            List<VMProductmsg> objList = null;
            if (e.Result == null)
            {
                return;
            }

            objList = (List<VMProductmsg>)e.Result;
            if (objList != null && objList.Count > 0)
            {
                foreach (VMProductmsg obj in objList)
                {
                    frmReminder objF = new frmReminder(obj);
                    objF.ShowDialog(this);
                }
            }
        }

        private void sevnoToolStripMenuItem_Click(object sender, EventArgs e)
        {
           // int vt_id = 6;
            int vt_id = 7;
            //int vt_id = 8;
            //int vt_id = 9;
           // int vt_id = 10;

            //yearly
            //if (radioButton1.Checked == true)
            //{
            //    string prefix1 = "";
            //    string postfix1 = "";
            //    int padding1 = 0;

            //    prefix1 = Database.GetScalarText("Select prefix from Vouchertype where vt_id=" + vt_id);
            //    postfix1 = Database.GetScalarText("Select postfix from Vouchertype where vt_id=" + vt_id);
            //    padding1 = Database.GetScalarInt("Select padding from Vouchertype where vt_id=" + vt_id);


            //    DataTable dtvou = new DataTable("VOUCHERINFO");
            //    Database.GetSqlData("SELECT * FROM VOUCHERINFO WHERE Vt_id = " + gstr + " ORDER BY Vdate desc,Vnumber desc", dtvou);
            //    for (int i = 0; i < dtvou.Rows.Count; i++)
            //    {
            //        dtvou.Rows[i]["vnumber"] = dtvou.Rows.Count - i;
            //        string invoiceno = (dtvou.Rows.Count - i).ToString();
            //        dtvou.Rows[i]["Invoiceno"] = prefix1 + invoiceno.PadLeft(padding1, '0') + postfix1;
            //    }
            //    Database.SaveData(dtvou);
            //}
       


            //daily
            string vtype = funs.Select_vt_nm(vt_id);
            int numtype = Database.GetScalarInt("Select numtype from Vouchertypes where vt_id="+ vt_id);
            if (numtype==3)
            {

               
                // int taxvno = 1;
                DateTime vdate = new DateTime(1801, 4, 01);
                DataTable dtvou = new DataTable();
                Database.GetSqlData("SELECT * FROM VOUCHERINFOs WHERE Vt_id = " + vt_id + " and LocationId='" + Database.LocationId + "' ORDER BY Vdate,Vnumber", dtvou);
                int vno = 0;

                for (int i = 0; i < dtvou.Rows.Count; i++)
                {


                    if (vdate == DateTime.Parse(dtvou.Rows[i]["Vdate"].ToString()))
                    {
                        vno++;
                    }
                    else
                    {
                        vno = 1;
                    }

                    Database.CommandExecutor("Update Voucherinfos set vnumber=" + vno + " where Vi_id='" + dtvou.Rows[i]["Vi_id"].ToString()+"'");

                    vdate = DateTime.Parse(dtvou.Rows[i]["Vdate"].ToString());
                }

            }

            int vtid = vt_id;
            string prefix = "";
            string postfix = "";
            int padding = 0;
            prefix = Database.GetScalarText("Select prefix from Vouchertypes where vt_id=" + vtid);
            postfix = Database.GetScalarText("Select postfix from Vouchertypes where vt_id=" + vtid);
            padding = Database.GetScalarInt("Select padding from Vouchertypes where vt_id=" + vtid);


            DataTable dtvouinvoice = new DataTable();
            Database.GetSqlData("SELECT * FROM VOUCHERINFOs WHERE Vt_id = " + vtid+" and locationid='"+ Database.LocationId+"'", dtvouinvoice);
            for (int i = 0; i < dtvouinvoice.Rows.Count; i++)
            {

                int vno = int.Parse(dtvouinvoice.Rows[i]["vnumber"].ToString());
                string invoiceno = vno.ToString();
                string inv_no = prefix + invoiceno.PadLeft(padding, '0') + postfix;
                Database.CommandExecutor("Update Voucherinfos set Invoiceno='" + inv_no + "' where Vi_id='" + dtvouinvoice.Rows[i]["Vi_id"].ToString()+"'");
            }
            MessageBox.Show("Vouchers of "+  vtype +" ReArrange Successfully.");

           
        }

        private void accountGroupToolStripMenuItem_Click(object sender, EventArgs e)
        {

            frmMaster frm = new frmMaster();
            frm.MdiParent = this;
            frm.LoadData("Account Group", "Account Group");
            frm.Show();
        }

        private void surrenderMyLicenceToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
            DialogResult ch = MessageBox.Show(null, "Are you sure want to Surrender Your Licence?", "Confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (ch == DialogResult.OK)
            {
                if (MarwariCRM.Surrender() == 1)
                {

                    funs.notifyIcon.Visible = false;
                    GC.Collect();
                    Environment.Exit(0);
                }
               
            }
            
        } 
    }
}
