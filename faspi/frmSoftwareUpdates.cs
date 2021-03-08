using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace faspi
{
    public partial class frmSoftwareUpdates : Form
    {
        public frmSoftwareUpdates()
        {
            InitializeComponent();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }

        private void frmSoftwareUpdates_Load(object sender, EventArgs e)
        {
            DataTable dtSoftwareUpdates = new DataTable();
            Database.GetSqlData("select * from SoftUpdates order by Date,SNo", dtSoftwareUpdates);
            ansGridView1.DataSource = dtSoftwareUpdates;
            ansGridView1.Columns["Date"].Width = 75;
            ansGridView1.Columns["SNo"].Width = 40;
            ansGridView1.Columns["Update"].Width = 200;
            ansGridView1.Columns["Details"].Width = 750;
        }

        public void Update()
        {
            string LastUpdate;
            Database.OpenConnection();
            LastUpdate = Database.GetScalarDate("SELECT SoftUpdates.Date as Udate FROM SoftUpdates WHERE [Update]='Update Upto' ");
            if (DateTime.Parse(LastUpdate).ToString(Database.dformat) == Database.ExeDate.ToString(Database.dformat))
            {
                return;
            }
            else if (DateTime.Parse(LastUpdate) > DateTime.Parse(Database.ExeDate.ToString(Database.dformat)))
            {
                DialogResult ch = MessageBox.Show(null, "You are using older version. Required update.", "Update");
                //if (ch == DialogResult.Yes)
                //{
                AutoUpdater.Start("http://faspi.in/MarwariTransport/FaspiTransportPro.xml");
                //}
                //else
                //{
                //    MessageBox.Show("Please Contact your Administrator");
                //    Environment.Exit(0);
                //}

                // 
                
            }
            else
            {
                Stock();
                Unloadingpoint();
                AutoChargesload();
                BranchId();
                Forwardingdet();
                Onlinepwd();
                Transporter_id();
                Narration();
                iscancel();
                GRbychallan();
                ShortCode();
                SMSFeature();
                Workstation();
               // autobackup();
                ChargeinVou();
                ChargeinStk();
                Expense11();
                DelExtra();
                //changes for Surat
                Origin();
                Chweight();
                Dbill();
                Isnumeric();
                PaidoptinVou();
                expenseacc();
                Updateexe();
            }
            Database.CloseConnection();
        }

        private void Dbill()
        {
            if (Database.GetScalarInt("select count(*) from Vouchertypes where Name='DBill' ") == 0)
            {
                Database.CommandExecutor("insert into Vouchertypes (Vt_id,Name,Type,Stationary,NumType,Short,AliasName,Default1,Default2,Default3,ReportName,Code,Effect_On_Stock,Effect_On_Acc,IncludingTax,[ExcludingTax],[ExState],[TaxInvoice],[Unregistered],[Active],[AllowedAcc],[PaperSize],[SmsTemplate],[A],[B],[Calculation],[CashTransaction],[Print],[printcopy],[starting_no],[Postfix],[Padding],[Prefix]) values(93,'DBill','DBill','true',1,'DBL','DBill','None','None','None','Dbill.rpt','DBL','N','N','true','true','false','false','false','true','N/A','A4','Dear','true','true','Default','Allowed','','Office Copy,True;',1,'',0,'')");
            }
        }
        private void GRbychallan()
        {
            if (Database.GetScalarInt("select count(*) from Vouchertypes where Name='GRbyChallan' ") == 0)
            {
                Database.CommandExecutor("insert into Vouchertypes (Vt_id,Name,Type,Stationary,NumType,Short,AliasName,Default1,Default2,Default3,ReportName,Code,Effect_On_Stock,Effect_On_Acc,IncludingTax,[ExcludingTax],[ExState],[TaxInvoice],[Unregistered],[Active],[AllowedAcc],[PaperSize],[SmsTemplate],[A],[B],[Calculation],[CashTransaction],[Print],[printcopy],[starting_no],[Postfix],[Padding],[Prefix]) values(92,'GRbyChallan','GRbyChallan','true',1,'GRC','GRbyChallan','None','None','None','GRByChallan.rpt','GRC','N','N','true','true','false','false','false','true','N/A','A4','Dear','true','true','Default','Allowed','','Office Copy,True;',1,'',0,'')");
            }
        }

        private void Isnumeric()
        {
            if (Database.CommandExecutor("Alter table Transportdetails add Isnumeric bit ") == true)
            {
                Database.CommandExecutor("update Transportdetails set Isnumeric='False'");
            }

          
        }

        private void DelExtra()
        {
           
                Database.CommandExecutor("Delete from Accountypes where type<>'Account'");
          

        }

        private void expenseacc()
        {
            if (Database.CommandExecutor("Alter table location add expenseacc nvarchar(20) ") == true)
            {
                Database.CommandExecutor("update location set expenseacc=''");
            }

           
        }
    
        private void ChargeinStk()
        {
            if (Database.CommandExecutor("Alter table ChallanUnloadings add ActWeight money ") == true)
            {
                Database.CommandExecutor("update ChallanUnloadings set ChallanUnloadings.ActWeight=Weight");
            }

            if (Database.CommandExecutor("Alter table Stocks add ActWeight money ") == true)
            {
                Database.CommandExecutor("update Stocks set Stocks.ActWeight=0");
            }
        }
        private void ChargeinVou()
        {
            if (Database.CommandExecutor("Alter table Voucherinfos add ActWeight money") == true)
            {
                Database.CommandExecutor("update voucherinfos set VOUCHERINFOs.ActWeight= (select SUM(dbo.Voucherdets.weight) from Voucherdets where Voucherdets.Vi_id=VOUCHERINFOs.vi_id) where (VOUCHERINFOs.Vt_id = 86) OR  (VOUCHERINFOs.Vt_id = 87) OR (VOUCHERINFOs.Vt_id = 88)");
               
            }
        }


        private void PaidoptinVou()
        {
            if (Database.CommandExecutor("Alter table Voucherinfos add Paidopt nvarchar(10)") == true)
            {
                Database.CommandExecutor("update voucherinfos set Paidopt= 'Credit'");
                Database.CommandExecutor("update voucherinfos set Paidopt= 'Cash' where Paymentmode='Paid'");


            }
        }



        private void Unloadingpoint()
        {
            if (Database.CommandExecutor("Alter table Voucherinfos add unloadingpoint_id nvarchar(255) ") == true)
            {
                Database.CommandExecutor("Update Voucherinfos set unloadingpoint_id='TPN' where Sid='SER78' and Vt_id=63");
                Database.CommandExecutor("Update Voucherinfos set unloadingpoint_id='VNS' where Sid='SER1' and Vt_id=63");
            }
        }
        private void iscancel()
        {
            if (Database.CommandExecutor("Alter table Voucherinfos add Iscancel bit ") == true)
            {
                Database.CommandExecutor("Update Voucherinfos set Iscancel='false' ");
            }
        }


        private void BranchId()
        {
            if (Database.CommandExecutor("Alter table USERs add location_id nvarchar(255) ") == true)
            {
                Database.CommandExecutor("Update USERs set location_id=''");
            }
        }
        private void AutoChargesload()
        {
            if (Database.CommandExecutor("Alter table Charges add AutoLoad bit ") == true)
            {
                Database.CommandExecutor("Update Charges set AutoLoad='false'");
            }
        }


        private void Transporter_id()
        {
            if (Database.CommandExecutor("Alter table Accounts add Transporter_id nvarchar(255) ") == true)
            {
                Database.CommandExecutor("Update Accounts set Transporter_id=''");
            }
            if (Database.CommandExecutor("Alter table Accounts add password nvarchar(255) ") == true)
            {
                Database.CommandExecutor("Update Accounts set password=tin_number");
            }
        }
        private void Onlinepwd()
        {
            
            if (Database.CommandExecutor("Alter table Accounts add password nvarchar(255) ") == true)
            {
                Database.CommandExecutor("Update Accounts set password=tin_number");
            }
        }
        private void ShortCode()
        {

            if (Database.CommandExecutor("Alter table Accounts add shortcode nvarchar(255) ") == true)
            {

                DataTable dtacc = new DataTable("Accounts");
                Database.GetSqlData("Select * from Accounts",dtacc);
                
                for (int i = 0; i < dtacc.Rows.Count; i++)
                {
                    string shortcode = "";
                    string[] ar = dtacc.Rows[i]["Name"].ToString().Split(' ');
                    for (int k = 0; k < ar.Length; k++)
                    {
                        if (ar[k] != "")
                        {
                            shortcode = shortcode + ar[k].Substring(0, 1);
                        }
                    }
                    dtacc.Rows[i]["shortcode"] = shortcode;
                }
                Database.SaveData(dtacc);
                //Database.CommandExecutor("Update Accounts set shortcode=");
            }
        }
        private void Narration()
        {

            if (Database.CommandExecutor("Alter table Stocks add Narration nvarchar(255) ") == true)
            {
                Database.CommandExecutor("Update Stocks set Narration=''");
            }
        }


        private void autobackup()
        {
            AddFeatureGST("Transaction", "Auto Backup", "Required Auto Backup", false, false, "No;Yes;", "Yes", "ComboBox", 101);
        }

        private void Origin()
        {
            AddFeatureGST("Transaction", "Origin is same as Login Location", "Origin is same as Login Location", false, false, "No;Yes;", "No", "ComboBox", 102);
        }

        private void Chweight()
        {
            AddFeatureGST("Transaction", "Required Charged Weight", "Required Charged Weight", false, false, "No;Yes;", "Yes", "ComboBox", 103);
        }
        private void Expense11()
        {
            AddFeatureGST("Expense", "Name of Expense11", "Name of Expense11", false, false, "Toll Charges", "Toll Charges", "Textbox", 101);
            
            if (Database.CommandExecutor("Alter table ItemDetails add Expense11 money") == true)
            {
                Database.CommandExecutor("Alter table ItemDetails add MRExpense11 money");
                Database.CommandExecutor("Alter table ItemDetails add ExpenseType11 nvarchar(50)");
                Database.CommandExecutor("update ItemDetails set Expense11=0");
                Database.CommandExecutor("update ItemDetails set MRExpense11=0");
                Database.CommandExecutor("update ItemDetails set ExpenseType11='Flat'");

                Database.CommandExecutor("Alter table PARTYRATEs add Expense11 money");
                Database.CommandExecutor("Alter table PARTYRATEs add MRExpense11 money");
                Database.CommandExecutor("Alter table PARTYRATEs add ExpenseType11 nvarchar(50)");
                Database.CommandExecutor("update PARTYRATEs set Expense11=0");
                Database.CommandExecutor("update PARTYRATEs set MRExpense11=0");
                Database.CommandExecutor("update PARTYRATEs set ExpenseType11='Flat'");


                Database.CommandExecutor("Alter table Voucherdets add exp11rate money");
                Database.CommandExecutor("Alter table Voucherdets add exp11amt money");
                Database.CommandExecutor("Alter table Voucherdets add exp11mr money");
                Database.CommandExecutor("Alter table Voucherdets add exp11type nvarchar(50)");
                Database.CommandExecutor("update Voucherdets set exp11rate=0");
                Database.CommandExecutor("update Voucherdets set exp11amt=0");
                Database.CommandExecutor("update Voucherdets set exp11mr=0");
                Database.CommandExecutor("update Voucherdets set exp11type='Flat'");

            }
       


        }


        private void SMSFeature()
        {
            DataTable dtbranch = new DataTable();
            Database.GetSqlData("SELECT 1 AS Expr1 FROM  sys.tables WHERE (name = 'SMSSETUPS')", dtbranch);
            if (dtbranch.Rows.Count == 0)
            {
                if (Database.CommandExecutor("create table SMSSETUPS (uid nvarchar(50),[pin] nvarchar(50),Sender nvarchar(255)  CONSTRAINT sms PRIMARY KEY(uid))") == true)
                {


                }
            }
            AddFeatureGST("Transaction", "Send SMS", "Required Send SMS", false, false, "No;Yes;", "No", "ComboBox", 65);
            if (Database.CommandExecutor("Alter table Accounts add SMSMobile nvarchar(255) ") == true)
            {

                Database.CommandExecutor("Update Accounts set SMSMobile='0'");
             
            }
        }



        private void Workstation()
        {
            DataTable dtbranch = new DataTable();
            Database.GetSqlData("SELECT 1 AS Expr1 FROM  sys.tables WHERE (name = 'WorkStations')", dtbranch);
            if (dtbranch.Rows.Count == 0)
            {
                if (Database.CommandExecutor("create table WorkStations (id int Identity ,Sys_Name nvarchar(50),Sys_Code nvarchar(255),Active bit  CONSTRAINT cd PRIMARY KEY(Sys_Code))") == true)
                {


                }
            }
          
        }
        private void Forwardingdet()
        {
            AddFeatureGST("Transaction", "Details on Booking Acc to Consigner", "Details on Booking Acc to Consigner", false, false, "No;Yes;", "Yes", "ComboBox", 61);
            AddFeatureGST("Transaction", "Display all Items", "Display all Items", false, false, "No;Yes;", "No", "ComboBox", 59);
            AddFeatureGST("Transaction", "Display Forwarding GRDetails", "Display Forwarding GRDetails", false, false, "No;Yes;", "No", "ComboBox", 60);
            if (Database.CommandExecutor("Alter table Voucherinfos add transporter_id nvarchar(255) ") == true)
            {
                Database.CommandExecutor("Alter table Voucherinfos add ForGrno nvarchar(255) ");
                Database.CommandExecutor("Alter table Voucherinfos add ForGRdate DateTime ");
                Database.CommandExecutor("Update Voucherinfos set ForGRdate=vdate");
               // Database.CommandExecutor("Update Charges set AutoLoad='false'");
            }
        }

        private void Stock()
        {
            //try
            //{
            //    Database.BeginTran();

                //if (Database.DatabaseType == "sql")
                //{
                    DataTable dtbranch = new DataTable();
                    Database.GetSqlData("SELECT 1 AS Expr1 FROM  sys.tables WHERE (name = 'Stocks')", dtbranch);
                    if (dtbranch.Rows.Count == 0)
                    {
                        if (Database.CommandExecutor("create table Stocks (vid nvarchar(50),[GR_id] nvarchar(50),Quantity money,Step nvarchar(255),Godown_id nvarchar(50)  CONSTRAINT stks PRIMARY KEY(vid,GR_id))") == true)
                        {


                        }
                    }
               // }
            //    Database.CommitTran();
            //}
            //catch (Exception es)
            //{
            //    Database.RollbackTran();
            //}
        }
        private void Updateexe()
        {
            try
            {
                Database.BeginTran();
                if (Database.GetScalarInt("select count(*) from SoftUpdates where [Update]='Update Upto'") == 0)
                {
                    Database.CommandExecutor("insert into SoftUpdates values( " + access_sql.Hash + Database.ExeDate.ToString("dd-MMM-yyyy") + access_sql.Hash + ",'1','Update Upto','UpDated Exe')");
                }
                else
                {
                    Database.CommandExecutor("UPDATE SoftUpdates SET [Date] = " + access_sql.Hash + Database.ExeDate.ToString("dd-MMM-yyyy") + access_sql.Hash + " WHERE [Update]='Update Upto'");
                }

                Database.CommitTran();
            }
            catch (Exception ex)
            {
                Database.RollbackTran();
            }
        }

        private bool AddFeatureGST(String Grp, String Features, String Description, Boolean Default, Boolean disabled, string OptionValue, string selected, string gType,int v)
        {
            try
            {
                String Str = "";

                if (Database.DatabaseType == "access")
                {
                    Str = "select count(Features) as cnt from firmsetups where [Features]='" + Features + "'";
                }
                else if (Database.DatabaseType == "sql")
                {
                    Str = "select count(Features) as cnt from [dbo].[FirmSetups] WHERE CONVERT(VARCHAR(255), Features) = '" + Features + "' ";
                }

                if (Database.GetScalarInt(Str) == 0)
                {
                    if (Database.DatabaseType == "access")
                    {
                        
                        Str = "INSERT INTO FirmSetups ([ID],[Group],[Features],[Description],[Active],[Disabled],[OptionValues],[selected_value],[Type],[Demo_id]) values(100,'" + Grp + "', '" + Features + "','" + Description + "'," + Default + "," + disabled + ",'" + OptionValue + "','" + selected + "','" + gType + "'," + v + ")";
                    }
                    else if (Database.DatabaseType == "sql")
                    {
                        int id = Database.GetScalarInt("select max(ID) from FirmSetups") + 1;
                        Str = "INSERT INTO FirmSetups (id,[Group],[Features],[Description],[Active],[Disabled],[OptionValues],[selected_value],[Type],[Demo_id]) values(" + id + ",'" + Grp + "', '" + Features + "','" + Description + "','" + Default + "','" + disabled + "','" + OptionValue + "','" + selected + "','" + gType + "'," + v + ")";
                    
                    }

                    Database.CommandExecutor(Str);
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        private void MasterDongle()
        {
            if (Database.GetOtherScalarInt("Select count(*) from Activate where [Column]='Referral'") == 0)
            {
                if (Dongle.getDongleNumber() == "AFFA1102" || Dongle.getDongleNumber() == "AFFA1104" || Dongle.getDongleNumber() == "AFFA1106" || Dongle.getDongleNumber() == "AFFA1082")
                {
                    Database.CommandExecutorOther("insert into Activate ([Column],[Value]) values('Referral','AFFA1110')");
                }
                else
                {
                    Database.CommandExecutorOther("insert into Activate ([Column],[Value]) values('Referral','')");
                }
            }
        }        
    }
}
