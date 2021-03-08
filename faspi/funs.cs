using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;

namespace faspi
{
    class  funs
    {
        OleDbCommand cmd = new OleDbCommand();
        public static System.Windows.Forms.NotifyIcon notifyIcon=new System.Windows.Forms.NotifyIcon();
        public static void ShowBalloonTip(String BalloonTipTitle, string BalloonTipText)
        {
            GC.Collect();
            System.Drawing.Icon appIcon = System.Drawing.Icon.ExtractAssociatedIcon(Database.ServerPath + "\\Marwari.exe");
            notifyIcon.Icon = appIcon;
            notifyIcon.Visible = true;
            notifyIcon.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            notifyIcon.BalloonTipTitle = BalloonTipTitle;
            notifyIcon.BalloonTipText = BalloonTipText;
            notifyIcon.ShowBalloonTip(1000);
        }

        public static bool isDouble(String str)
        {
            double mydouble ;
            bool isnumber=double.TryParse(str, out mydouble);
            return isnumber;
        }

        public static string GetFixedLengthString(string input, int length)
        {
            string result = string.Empty;
            if (string.IsNullOrEmpty(input))
            {
                result = new string(' ', length);
            }
            else if (input.Length > length)
            {
                result = input.Substring(0, length);
            }
            else
            {
                result = input.PadRight(length);
            }
            return result;
        }

        public static String GetStrCombo(string accountTypes)
        {
            string strCombo = "";
            if (accountTypes == "*")
            {
                strCombo = "SELECT accounts.Name," + access_sql.accbalq + ", accounts.Address1, accounts.Address2, accounts.Phone, accounts.Tin_number, accountypes.Name AS [accounts Group] FROM ((((SELECT accounts.Ac_id,  " + access_sql.fnstring("accounts.Balance>0", "accounts.Balance", "0") + " AS Dr, " + access_sql.fnstring("accounts.Balance<0", "-1*(accounts.Balance)", "0") + " AS Cr FROM accounts union all SELECT JOURNALs.Ac_id, " + access_sql.fnstring("JOURNALs.Amount>0", "JOURNALs.Amount", "0") + " AS Dr, " + access_sql.fnstring("JOURNALs.Amount<0", "-1*(JOURNALs.Amount)", "0") + " AS Cr FROM JOURNALs,  VOUCHERINFOs , VOUCHERTYPEs where JOURNALs.Vi_id = VOUCHERINFOs.Vi_id  and VOUCHERINFOs.Vt_id = VOUCHERTYPEs.Vt_id and VOUCHERTYPEs.A=" + access_sql.Singlequote + "True" + access_sql.Singlequote + ")  AS balance LEFT JOIN accounts ON balance.Ac_id = accounts.Ac_id) LEFT JOIN accountypes ON accounts.Act_id = accountypes.Act_id) LEFT JOIN OTHERs ON accounts.Loc_id = OTHERs.Oth_id) LEFT JOIN CONTRACTORs ON accounts.Con_id = CONTRACTORs.Con_id where accounts.Name<>'' GROUP BY accounts.Name, accounts.Address1, accounts.Address2, accounts.Phone, accounts.Tin_number, accountypes.Name, OTHERs.Name, CONTRACTORs.Name";
            }
            else if (accountTypes != "*")
            {
                strCombo = "SELECT accounts.Name," + access_sql.accbalq + " , accounts.Address1, accounts.Address2, accounts.Phone, accounts.Tin_number, accountypes.Name AS [accounts Group] FROM ((((SELECT accounts.Ac_id,  " + access_sql.fnstring("accounts.Balance>0", "accounts.Balance", "0") + " AS Dr, " + access_sql.fnstring("accounts.Balance<0", "-1*(accounts.Balance)", "0") + " AS Cr FROM accounts union all SELECT JOURNALs.Ac_id, " + access_sql.fnstring("JOURNALs.Amount>0", "JOURNALs.Amount", "0") + " AS Dr, " + access_sql.fnstring("JOURNALs.Amount<0", "-1*(JOURNALs.Amount)", "0") + " AS Cr FROM JOURNALs,  VOUCHERINFOs , VOUCHERTYPEs where JOURNALs.Vi_id = VOUCHERINFOs.Vi_id  and VOUCHERINFOs.Vt_id = VOUCHERTYPEs.Vt_id and VOUCHERTYPEs.A=" + access_sql.Singlequote + "True" + access_sql.Singlequote + ")  AS balance LEFT JOIN accounts ON balance.Ac_id = accounts.Ac_id) LEFT JOIN accountypes ON accounts.Act_id = accountypes.Act_id) LEFT JOIN OTHERs ON accounts.Loc_id = OTHERs.Oth_id) LEFT JOIN CONTRACTORs ON accounts.Con_id = CONTRACTORs.Con_id WHERE ( " + accountTypes + ") and accounts.Name<>'' GROUP BY accounts.Name, accounts.Address1, accounts.Address2, accounts.Phone, accounts.Tin_number, accountypes.Name, OTHERs.Name, CONTRACTORs.Name";
            }
            return strCombo;
        }

        public static string Select_act_path(String name)
        {
            return Database.GetScalarText("select Path from AccountTypes where name='" + name + "'");
            
        }

        public static string Select_act_under(String name)
        {
            return Database.GetScalarText("select under from AccountTypes where name='" + name + "'");
        }

        public static int Select_act_level(String name)
        {
            return Database.GetScalarInt("select level from AccountTypes where name='" + name + "'");
        }

        public static double Select_item_bharti(String name)
        {
            return Database.GetScalarDecimal("select bharti from Items where name='" + name + "'");            
        }

        public static bool Select_act_fixed(String name)
        {
            return Database.GetScalarBool("select fixed from accountTypes where name='" + name + "'");
        }

        public static string Select_act_nature(String name)
        {
            return Database.GetScalarText("select Nature from accountTypes where name='" + name + "'");
        }

        public static int Select_AccType_id(string name)
        {
            return Database.GetScalarInt("select Act_id from accountTypes where name='" + name + "'");
        }

        public static int Select_user_id(String uname)
        {
            return Database.GetScalarInt("Select U_id from USERs where UserName='" + uname + "'");
        }

        public static String GetStrCombonew(string wherestr, string Having)
        {
            string strCombo = "";
            strCombo = "SELECT accounts.Name,  " + access_sql.accbalq + ",  accounts.Address1, accounts.Address2, accounts.Phone, accounts.Tin_number, accountsYPE.Name AS [accounts Group] FROM ((((SELECT accounts.Ac_id, " + access_sql.fnstring("accounts.Balance>0", "accounts.Balance", "0") + "  AS Dr, " + access_sql.fnstring("accounts.Balance<0", "-1*(accounts.Balance)", "0") + " AS Cr FROM accounts union all SELECT JOURNAL.Ac_id, " + access_sql.fnstring("JOURNAL.Amount>0", "JOURNAL.Amount", "0") + " AS Dr, " + access_sql.fnstring("JOURNAL.Amount<0", "-1*(JOURNAL.Amount)", "0") + "  AS Cr FROM JOURNAL,  VOUCHERINFO , VOUCHERTYPEs where JOURNAL.Vi_id = VOUCHERINFO.Vi_id  and VOUCHERINFO.Vt_id = VOUCHERTYPEs.Vt_id and VOUCHERTYPEs.A=" + access_sql.Singlequote + "True" + access_sql.Singlequote + ")  AS balance LEFT JOIN accounts ON balance.Ac_id = accounts.Ac_id) LEFT JOIN accountsYPE ON accounts.Act_id = accountsYPE.Act_id) LEFT JOIN OTHER ON accounts.Loc_id = OTHER.Oth_id) LEFT JOIN CONTRACTOR ON accounts.Con_id = CONTRACTOR.Con_id  WHERE  " + wherestr + "  GROUP BY accounts.Name, accounts.Address1, accounts.Address2, accounts.Phone, accounts.Tin_number, accountsYPE.Name, OTHER.Name, CONTRACTOR.Name, accounts.Act_id, accounts.AllowPS, accounts.Status  " + Having;
            return strCombo;
        }

        public static String accbal(string ac_id, DateTime dt1)
        {
            String curbal;
            double opbal = 0, bal = 0;
            DataTable dtOpenBal = new DataTable();

            Database.GetSqlData("select Balance from accounts where Ac_id='" + ac_id + "'", dtOpenBal);

            if (dtOpenBal.Rows.Count > 0)
            {
                opbal = double.Parse(dtOpenBal.Rows[0]["Balance"].ToString());
            }
            string acname = funs.Select_ac_nm(ac_id);
            DataTable dtBal = new DataTable();

            Database.GetSqlData("SELECT Sum(QryJournal.Dr) AS Dramt, Sum(QryJournal.Cr) AS Cramt FROM QryJournal WHERE (((QryJournal.Name)='" + acname + "') AND ((QryJournal.Vdate)<=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + ")) GROUP BY QryJournal.A HAVING (((QryJournal.A)=" + access_sql.Singlequote + "True" + access_sql.Singlequote + "))", dtBal);

            if (dtBal.Rows.Count > 0)
            {
                if (dtBal.Rows[0]["Cramt"].ToString() == "" || dtBal.Rows[0]["Dramt"].ToString() == "")
                {
                    dtBal.Rows[0]["Cramt"] = 0;
                }
                if (double.Parse(dtBal.Rows[0]["Dramt"].ToString()) > double.Parse(dtBal.Rows[0]["Cramt"].ToString()))
                {
                    bal = double.Parse(dtBal.Rows[0]["Dramt"].ToString()) - double.Parse(dtBal.Rows[0]["Cramt"].ToString());
                }
                else
                {
                    bal = -(double.Parse(dtBal.Rows[0]["Cramt"].ToString()) - double.Parse(dtBal.Rows[0]["Dramt"].ToString()));
                }
            }
            curbal = (opbal + bal).ToString();

            if (double.Parse(curbal) >= 0)
            {
                curbal += " Dr.";
            }
            else
            {
                curbal = (-1 * double.Parse(curbal)).ToString();
                curbal += " Cr.";
            }
            return curbal;
        }

        public static String accbal(string ac_id)
        {
            String curbal;
            double opbal = 0, bal = 0;

            DataTable dtOpenBal = new DataTable();
            
                Database.GetSqlData("select Balance from accounts where Ac_id='" + ac_id + "'", dtOpenBal);
            
            if (dtOpenBal.Rows.Count > 0)
            {

                opbal = double.Parse(dtOpenBal.Rows[0]["Balance"].ToString());
            }
            string acname = funs.Select_ac_nm(ac_id);
            DataTable dtBal = new DataTable();
            
            Database.GetSqlData("SELECT Sum(QryJournal.Dr) AS Dramt, Sum(QryJournal.Cr) AS Cramt FROM QryJournal WHERE (((QryJournal.Name)='" + acname + "')) GROUP BY QryJournal.A HAVING (((QryJournal.A)=" + access_sql.Singlequote + "True" + access_sql.Singlequote + "))", dtBal);

            if (dtBal.Rows.Count > 0)
            {
                if (dtBal.Rows[0]["Cramt"].ToString() == "" || dtBal.Rows[0]["Dramt"].ToString() == "")
                {
                    dtBal.Rows[0]["Cramt"] = 0;
                }
                if (double.Parse(dtBal.Rows[0]["Dramt"].ToString()) > double.Parse(dtBal.Rows[0]["Cramt"].ToString()))
                {
                    bal = double.Parse(dtBal.Rows[0]["Dramt"].ToString()) - double.Parse(dtBal.Rows[0]["Cramt"].ToString());
                }
                else
                {
                    bal = -(double.Parse(dtBal.Rows[0]["Cramt"].ToString()) - double.Parse(dtBal.Rows[0]["Dramt"].ToString()));
                }
            }

            curbal = (opbal + bal).ToString();

            if (double.Parse(curbal) >= 0)
            {
                curbal += " Dr.";
            }
            else
            {
                curbal = (-1 * double.Parse(curbal)).ToString();
                curbal += " Cr.";
            }
            return curbal;
        }

        public static String AddAccount()
        {
            String accnm;
            frm_NewAcc frm = new frm_NewAcc();
            frm.calledIndirect = true;
            frm.LoadData("0", "account");
            frm.ShowDialog();
            accnm = frm.AccName;
            return accnm;
        }

        public static String AddDeliveredby()
        {
            String accnm;
            frm_deliveredby frm = new frm_deliveredby();
            frm.calledIndirect = true;
            frm.LoadData("0", "DeliveredBy");
            frm.ShowDialog();
            accnm = frm.DBName;
            return accnm;
        }

        public static String AddAccount(String acctyp)
        {
            String accnm;
            frm_NewAcc frm = new frm_NewAcc();
            frm.calledIndirect = true;
            frm.AccType = acctyp;
            frm.LoadData("0", "account");
            frm.ShowDialog();
            accnm = frm.AccName;
            return accnm;
        }

        public static string getmonth(int Month)
        {
            string month = new DateTime(1900, Month, 1).ToString("MMMM");
            return month;
        }

        public static String EditAccount(String accnm)
        {
            String newAccnm;
            String acid;
            DataTable dtCheckAcc = new DataTable();
            Database.GetSqlData("select * from accounts where [name]='" + accnm + "'", dtCheckAcc);
            if (dtCheckAcc.Rows.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("accounts does not exist");
                return "";
            }
            else
            {
                frm_NewAcc frm = new frm_NewAcc();
                frm.calledIndirect = true;
                acid = Select_ac_id(accnm).ToString();
                frm.LoadData(acid, "Edit accounts");
                frm.ShowDialog();
                newAccnm = frm.AccName;
                if (newAccnm == "" || newAccnm == null)
                {
                    return accnm;
                }
                else
                {
                    return newAccnm;
                }
            }
        }



        public static String EditDeliveredBy(String accnm)
        {
            String newAccnm;
            String acid;
            DataTable dtCheckAcc = new DataTable();
            Database.GetSqlData("select * from DeliveredBys where [name]='" + accnm + "'", dtCheckAcc);
            if (dtCheckAcc.Rows.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("Name does not exist");
                return "";
            }
            else
            {
                frm_deliveredby frm = new frm_deliveredby();
                frm.calledIndirect = true;
                acid = Select_db_id(accnm).ToString();
                frm.LoadData(acid, "Edit DeliveredBy");
                frm.ShowDialog();
                newAccnm = frm.DBName;
                if (newAccnm == "" || newAccnm == null)
                {
                    return accnm;
                }
                else
                {
                    return newAccnm;
                }
            }
        }

        public static String EditAccount(String accnm, String acctyp)
        {
            String newAccnm;
            String acid;
            DataTable dtCheckAcc = new DataTable();
            Database.GetSqlData("select * from accounts where [name]='" + accnm + "'", dtCheckAcc);
            if (dtCheckAcc.Rows.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("accounts does not exist");
                return "";
            }
            frm_NewAcc frm = new frm_NewAcc();
            frm.calledIndirect = true;
            frm.AccType = acctyp;
            acid = Select_ac_id(accnm).ToString();
            frm.LoadData(acid, "Edit accounts");
            frm.ShowDialog();
            newAccnm = frm.AccName;
            return newAccnm;
        }

        public static String AddBroker()
        {
            String bronm;
            frmBroker frm = new frmBroker();
            frm.calledIndirect = true;
            frm.LoadData("0", "Broker");
            frm.ShowDialog();
            bronm = frm.BrokerName;
            return bronm;
        }

        public static String AddGaddi()
        {
            String gaddinm;
            Gaddi frm = new Gaddi();
            frm.calledIndirect = true;
            frm.LoadData("0", "Gaddi");
            frm.ShowDialog();
            gaddinm = frm.gaddi;
            return gaddinm;
        }

        public static String EditBroker(String bronm)
        {
            String newBronm;
            String Broid;
            DataTable dtCheckBro = new DataTable();
            Database.GetSqlData("select * from contractors where [name]='" + bronm + "'", dtCheckBro);
            if (dtCheckBro.Rows.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("Broker does not exist");
                return "";
            }
            frmBroker frm = new frmBroker();
            frm.calledIndirect = true;
            Broid = Select_broker_id(bronm).ToString();
            frm.LoadData(Broid, "Edit Broker");
            frm.ShowDialog();
            newBronm = frm.BrokerName;
            if (newBronm == "" || newBronm == null)
            {
                return bronm;
            }
            else
            {
                return newBronm;
            }            
        }

        public static String EditGaddi(String gaddi)
        {
            String newgaddi;
            String gid;
            DataTable dtCheckBro = new DataTable();
            Database.GetSqlData("select * from Gaddis where [Gaddi_name]='" + gaddi + "'", dtCheckBro);
            if (dtCheckBro.Rows.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("Gaddi does not exist");
                return "";
            }
            Gaddi frm = new Gaddi();
            frm.calledIndirect = true;
            gid = Select_gaddi_id(gaddi).ToString();
            frm.LoadData(gid, "Edit Broker");
            frm.ShowDialog();
            newgaddi = frm.gaddi;
            if (newgaddi == "" || newgaddi == null)
            {
                return gaddi;
            }
            else
            {
                return newgaddi;
            }
        }

        public static String AddGroup()
        {
            String Gpnm;
            frm_NewGroup frm = new frm_NewGroup();
            frm.calledIndirect = true;
            frm.LoadData("0", "New Group");
            frm.ShowDialog();
            Gpnm = frm.GrpName;
            return Gpnm;
        }

        public static String AddDP()
        {
            String Stnm;
            frmDP frm = new frmDP();
            frm.LoadData("0", "Delivery Point");
            frm.calledIndirect = true;
            frm.ShowDialog();
            Stnm = frm.DPName;
            return Stnm;
        }

        public static String EditDP(String stnm)
        {
            String newstnm;
            String stid;
            DataTable dtCheckst = new DataTable();
            Database.GetSqlData("select DPId from DeliveryPoints where [Name]='" + stnm + "'", dtCheckst);
            if (dtCheckst.Rows.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("DeliveryPoint does not exist");
                return "";
            }
            frmDP frm = new frmDP();
            frm.calledIndirect = true;
            stid = Select_dp_id(stnm).ToString();
            frm.LoadData(stid, "Edit Delivery Point");
            frm.ShowDialog();
            newstnm = frm.DPName;
            if (newstnm == "" || newstnm == null)
            {
                return stnm;
            }
            else
            {
                return newstnm;
            }
        }

        public static String AddState()
        {
            String Stnm;
            frm_state frm = new frm_state();
            frm.calledIndirect= true;
            frm.LoadData("0", "New State");
            frm.ShowDialog();
            Stnm = frm.statename;
            return Stnm;
        }

        public static String EditState(String stnm)
        {
            String newstnm;
            String stid;
            DataTable dtCheckst = new DataTable();
            Database.GetSqlData("select State_id from States where [Sname]='" + stnm + "'", dtCheckst);
            if (dtCheckst.Rows.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("State does not exist");
                return "";
            }
            frm_state frm = new frm_state();
            frm.calledIndirect = true;
            stid = Select_state_id(stnm).ToString();
            frm.LoadData(stid, "Edit state");
            frm.ShowDialog();
            newstnm = frm.statename;
            if (newstnm == "" || newstnm == null)
            {
                return stnm;
            }
            else
            {
                return newstnm;
            }
        }

        public static String EditPacking(String stnm)
        {
            String newpacking;
            String pid;
            DataTable dtCheckst = new DataTable();
            Database.GetSqlData("select p_id from Packings where [Name]='" + stnm + "'", dtCheckst);
            if (dtCheckst.Rows.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("Packing does not exist");
                return "";
            }
            Frmpacking frm = new Frmpacking();
            frm.calledIndirect = true;
            pid = Select_packing_id(stnm).ToString();
            frm.LoadData(pid, "Edit Packing");
            frm.ShowDialog();
            newpacking = frm.PackingName;
            if (newpacking == "" || newpacking == null)
            {
                return stnm;
            }
            else
            {
                return newpacking;
            }
        }

        public static String EditGroup(String gpnm)
        {
            String newGpnm;
            String gpid;
            DataTable dtCheckGp = new DataTable();
            Database.GetSqlData("select * from others where [name]='" + gpnm + "' and [type]=17", dtCheckGp);
            if (dtCheckGp.Rows.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("Group does not exist");
                return "";
            }
            frm_NewGroup frm = new frm_NewGroup();
            frm.calledIndirect = true;
            gpid = Select_oth_id(gpnm).ToString();
            frm.LoadData(gpid, "Edit Group");
            frm.ShowDialog();
            newGpnm = frm.GrpName;
            if (newGpnm == "" || newGpnm == null)
            {
                return gpnm;
            }
            else
            {
                return newGpnm;
            }    
        }

        public static String AddItem()
        {
            String Itemnm;
            frmItem frm = new frmItem();
            frm.calledIndirect = true;
            frm.LoadData("0", "New Item");
            frm.ShowDialog();
            Itemnm = frm.itemName; 
            return Itemnm;
        }

        public static String AddPacking()
        {
            String packnm;
            Frmpacking frm = new Frmpacking();
            frm.calledIndirect = true;
            frm.LoadData("0", "New Packing");
            frm.ShowDialog();
            packnm = frm.PackingName;
            return packnm;
        }

        public static String EditItem(string itemnm)
        {
            String newItemnm;
            String Itemid;
            DataTable dtCheckItem = new DataTable();
            Database.GetSqlData("select * from items where [name]='" + itemnm + "'", dtCheckItem);
            if (dtCheckItem.Rows.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("Item does not exist");
                return "";
            }
            frmItem frm = new frmItem();
            frm.calledIndirect = true;
            Itemid = Select_item_id(itemnm).ToString();
            frm.LoadData(Itemid, "Edit Item");
            frm.ShowDialog();
            newItemnm = frm.itemName;
            if (newItemnm == "" || newItemnm == null)
            {
                return itemnm;
            }
            else
            {
                return newItemnm;
            }
        }

        public static string Select_broker_id(String bronm)
        {
            return Database.GetScalarText("select Con_id from contractors where [Name]='" + bronm + "'");
        }

        public static string Select_packing_id(String pknm)
        {
            return Database.GetScalarText("select p_id from packings where [Name]='" + pknm + "'");
        }

        public static string Select_packing_nm(String pid)
        {
            return Database.GetScalarText("select Name from packings where p_id='" + pid + "'");
        }

        public static string Select_ac_id(String name)
        {
            return Database.GetScalarText("select ac_id from accounts where [Name]='" + name + "'");
        }

        public static string Select_ch_id(String name)
        {
            return Database.GetScalarText("select ch_id from Charges where [Name]='" + name + "'");
        }
        public static string Select_ch_nm(String cid)
        {
            return Database.GetScalarText("select Name from Charges where Ch_id='" + cid + "'");
        }


        public static string Select_ac_regstatus(string accid)
        {
            return Database.GetScalarText("select Regstatus from accounts where Ac_id='" + accid + "'");
        }

        public static string Select_state_id(String statename)
        {
            return Database.GetScalarText("select State_id from states where Sname='" + statename + "'");
        }

        public static int Select_ac_dlimit(String name)
        {
            return Database.GetScalarInt("select Dlimit from accounts where [Name]='" + name + "'");
        }

        public static string Select_Mobile(String name)
        {
            return Database.GetScalarText("select Phone from accounts where [Name]='" + name + "'");
        }
        public static string Select_SMSMobile(String name)
        {
            return Database.GetScalarText("select SMSMobile from accounts where [Name]='" + name + "'");
        }
        public static string Select_ac_state_id(String name)
        {
            return Database.GetScalarText("select State_id from accounts where [Name]='" + name + "'");
        }

        public static string Select_Print(String name)
        {
            return Database.GetScalarText("select Printname from accounts where [Name]='" + name + "'");
        }

        public static string Select_TIN(String name)
        {
            return Database.GetScalarText("select Tin_number from accounts where [Name]='" + name + "'");
        }

        public static string Select_PAN(String name)
        {
            return Database.GetScalarText("select PAN from accounts where [Name]='" + name + "'");
        }

        public static string Select_AAdhar(String name)
        {
            return Database.GetScalarText("select Aadhaarno from accounts where [Name]='" + name + "'");
        }

        public static string Select_locationId(String name)
        {
            return Database.GetScalarText("select locationId from Location where [nick_name]='" + name + "'");
        }

        public static string Select_location_name(String lid)
        {
            return Database.GetScalarText("select nick_name from Location where [locationId]='" + lid + "'");
        }

        public static string Select_GST(String name)
        {
            return Database.GetScalarText("select Tin_number from accounts where [Name]='" + name + "'");
        }

        public static string Select_Email(String name)
        {
            return Database.GetScalarText("select Email from accounts where [Name]='" + name + "'");
        }

        public static string Select_Address1(String name)
        {
            return Database.GetScalarText("select Address1 from accounts where [Name]='" + name + "'");
        }

        public static string Select_Address2(String name)
        {
            return Database.GetScalarText("select Address2 from accounts where [Name]='" + name + "'");
        }

        public static String Select_ac_nm(string ac_id)
        {
            return Database.GetScalarText("select [name] from accounts where ac_id='" + ac_id + "'");
        }

        public static string Select_state_nm(string state_id)
        {
            return Database.GetScalarText("Select sname from states where state_id ='" + state_id + "'");
        }

        public static String Select_state_GST(string statename)
        {
            return Database.GetScalarText("Select GSTCode from states where Sname ='" + statename + "'");
        }

        public static int Select_act_id(String name)
        {
            return Database.GetScalarInt("select act_id from accountypes where [name]='" + name + "'");
        }

        public static string Select_gaddi_id(String name)
        {
            return Database.GetScalarText("select Gaddi_id from Gaddis where [Gaddi_name]='" + name + "'");
        }

        public static int Select_Refineact_id(String name)
        {
            return Database.GetScalarInt("select act_id from accountypes where refinename='" + name + "'");
        }

        public static String Select_act_nm(int act_id)
        {
            return Database.GetScalarText("Select name from accountypes where act_id =" + act_id);
        }

        public static String Select_gaddi_nm(string act_id)
        {
            return Database.GetScalarText("Select Gaddi_name from Gaddis where Gaddi_id ='" + act_id + "' ");
        }

        public static String Select_Refineact_nm(int act_id)
        {
            return Database.GetScalarText("Select refinename from ACCOUNTYPEs where act_id =" + act_id);
        }
        public static DateTime? Stringtodate(string valdate)
        {
            string val = valdate;
            //current system date format
            string sysdatefor = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
            DateTime dateout;
            if (val.IndexOf('-') > 0)
            {
                System.Globalization.DateTimeFormatInfo mfi = new System.Globalization.DateTimeFormatInfo();
                string[] arr = val.Split('-');
                if (funs.isDouble(arr[1]))
                {
                    if (int.Parse(arr[1]) >= 1 && int.Parse(arr[1]) <= 12)
                    {
                        val = arr[0] + "-" + mfi.GetMonthName(int.Parse(arr[1])).ToString();
                    }
                }
                else
                {
                    val = arr[0] + "-" + arr[1];
                }
                for (int i = 2; i < arr.Length; i++)
                {
                    val += "-" + arr[i];
                }

            }
            else if (val.IndexOf('/') > 0)
            {
                System.Globalization.DateTimeFormatInfo mfi = new System.Globalization.DateTimeFormatInfo();
                string[] arr = val.Split('/');

                if (funs.isDouble(arr[1]))
                {
                    if (int.Parse(arr[1]) >= 1 && int.Parse(arr[1]) <= 12)
                    {
                        val = arr[0] + "-" + mfi.GetMonthName(int.Parse(arr[1])).ToString();
                    }
                }
                else
                {
                    val = arr[0] + "-" + arr[1];
                }
                for (int i = 2; i < arr.Length; i++)
                {
                    val += "-" + arr[i];
                }
            }
            else if (val.IndexOf('.') > 0)
            {
                System.Globalization.DateTimeFormatInfo mfi = new System.Globalization.DateTimeFormatInfo();

                string[] arr = val.Split('.');

                if (funs.isDouble(arr[1]))
                {
                    if (int.Parse(arr[1]) >= 1 && int.Parse(arr[1]) <= 12)
                    {
                        val = arr[0] + "-" + mfi.GetMonthName(int.Parse(arr[1])).ToString();
                    }
                }
                else
                {
                    val = arr[0] + "-" + arr[1];
                }
                for (int i = 2; i < arr.Length; i++)
                {
                    val += "-" + arr[i];
                }
            }

            if (DateTime.TryParse(val, out  dateout))
            {
                return DateTime.Parse(val);

            }
            else
            {


                return null;

            }
        }



        public static string Select_othtype_id(String OtherTypeName)
        {
            return Database.GetScalarText("Select oth_id from Others where [oth_name] ='" + OtherTypeName + "'");
        }

        public static string Select_dp_id(String Name)
        {
            return Database.GetScalarText("Select DPId from DeliveryPoints where [name] ='" + Name + "'");
        }
        public static int Select_ws_id(String Code)
        {
            return Database.GetScalarInt("Select Id from Workstations where [sys_code] ='" + Code + "'");
        }
        public static string Select_db_id(String Name)
        {
            return Database.GetScalarText("Select D_Id from DeliveredBys where [name] ='" + Name + "'");
        }

        public static string Select_db_nm(string db_id)
        {
            return Database.GetScalarText("Select name from DeliveredBys where D_id ='" + db_id + "'");
        }


        public static string Select_dp_nm(string dp_id)
        {
            return Database.GetScalarText("Select name from DeliveryPoints where DPId ='" + dp_id + "'");
        }

        public static string Select_item_id(string des_name)
        {
            return Database.GetScalarText("Select Id from items where [name]='" + des_name + "'");
        }

        public static string Select_item_nm(string id)
        {
            return Database.GetScalarText("select [name] from items where Id='" + id + "'");
        }

        public static string Select_item_name_pack_id(string name)
        {
            return Database.GetScalarText("Select Id from items where [name]='" + name + "'");
        }

        public static string Select_oth_id(String OtherName)
        {
            return Database.GetScalarText("select Oth_id from Others where [Name]='" + OtherName + "'");
        }

        public static String Select_oth_nm(string other_id)
        {
            return Database.GetScalarText("select [Name] from Others where Oth_id='" + other_id + "'");
        }

        public static string Select_con_id(String con_name)
        {
           return Database.GetScalarText("select con_id from CONTRACTORs where [name]='" + con_name + "'");  
        }

        public static String Select_con_nm(string con_id)
        {
            return Database.GetScalarText("select [name] from CONTRACTORs where con_id ='" + con_id + "'");
        }

        public static int Select_controlroom_id(String Feature)
        {
            return Database.GetScalarInt("select ID from FirmSetups where [Features]='" + Feature + "'");
        }

        public static int Select_vt_id(int vi_id)
        {
            return Database.GetScalarInt("Select Vt_id from Voucherinfos where  Vi_id="+vi_id);
        }

        public static int Select_vtid(string vi_id)
        {
            return Database.GetScalarInt("Select Vt_id from Voucherinfos where  Vi_id='" + vi_id + "'");
        }
        public static int Select_vt_id_nm(string name)
        {

            return Database.GetScalarInt("Select Vt_id from Vouchertypes where  name='" + name + "'");
            
        }

        public static int Select_vt_id(String vt_name)
        {
            return Database.GetScalarInt("Select Vt_id from VOUCHERTYPEs where [name]='" + vt_name + "'");
        }

        public static string Select_vt_short(String vt_name)
        {
            return Database.GetScalarText("select Short from VOUCHERTYPEs where name='" + vt_name + "'");
        }

        public static int Select_NumType(int vt_id)
        {
            return Database.GetScalarInt("select Numtype from VOUCHERTYPEs where Vt_id=" + vt_id );
        }

        public static int Select_AccTypeid(string accountsName)
        {
            return Database.GetScalarInt("select Act_id from accounts where [Name]='" + accountsName + "'");
        }

        public static bool Select_vt_taxinvoice(int vt_id)
        {
            return Database.GetScalarBool("select TaxInvoice from VOUCHERTYPEs where Vt_id=" + vt_id);
        }

        public static bool Select_vt_Exstate(int vt_id)
        {
            return Database.GetScalarBool("select ExState from VOUCHERTYPEs where Vt_id=" + vt_id);
        }

        public static string Select_vt_Exempted(int vt_id)
        {
            return Database.GetScalarText("select Exempted from VOUCHERTYPEs where Vt_id=" + vt_id);
        }

        public static bool Select_vt_Excludungtax(int vt_id)
        {
            return Database.GetScalarBool("select ExcludingTax from VOUCHERTYPEs where Vt_id=" + vt_id);
        }

        public static string Select_vt_CalculationType(int vt_id)
        {
            return Database.GetScalarText("select Calculation from VOUCHERTYPEs where Vt_id=" + vt_id);
        }

        public static string Select_vt_Cashtran(int vt_id)
        {
            return Database.GetScalarText("select CashTransaction from VOUCHERTYPEs where Vt_id=" + vt_id);
        }

        public static bool Select_vt_Includingtax(int vt_id)
        {
            return Database.GetScalarBool("select IncludingTax from VOUCHERTYPEs where Vt_id=" + vt_id);
        }

        public static bool Select_vt_Unregistered(int vt_id)
        {
            return Database.GetScalarBool("select Unregistered from VOUCHERTYPEs where Vt_id=" + vt_id);
        }

        public static String Select_vt_nm(int vt_id)
        {
            return Database.GetScalarText("select [name] from VOUCHERTYPEs where Vt_id=" + vt_id);
        }

        public static String Select_vt_Alias(int vt_id)
        {
            return Database.GetScalarText("select AliasName from VOUCHERTYPEs where Vt_id=" + vt_id);
        }

        public static string Select_vi_id(int vnm, int id, String dt)
        {
            return Database.GetScalarText("select vi_id voucherinfos where vnumber=" + vnm + " and vt_id=" + id);
        }

        public static String DecimalPoint(Object o, int count)
        {
            string str=".";
            for (int i = 0; i < count; i++)
            {
                str += "0"; 
            }
            if (count == 0)
            {
                str = "";
            }
            String conVal;
            conVal = String.Format("{0:0" + str + "}", o);
            return conVal;
        }

        public static String DecimalPoint(Object o)
        {
            return DecimalPoint(o, 2);
        }
        
        public static string IndianCurr(double o)
        {
            System.Globalization.CultureInfo cuInfo = new System.Globalization.CultureInfo("hi-IN");
            return (o.ToString("C", cuInfo)).Remove(0, 2).Trim();
        }

        public static int chkNumType(int vtid)
        {
            return Database.GetScalarInt("select Numtype from VOUCHERTYPEs where vt_id=" + vtid);
        }

        public static int GenerateVno(int vtid, String dt, string vid)
        {
            string wherstr = "";
            int prospective = 0;
            int numtype = funs.Select_NumType(vtid);
            if (numtype == 1)//yearly
            {
                wherstr = "";
            }
            else if (numtype == 2) //monthly
            {
                wherstr = " and (month(vdate)=" + DateTime.Parse(dt).Month + ")";
            }
            else if (numtype == 3) //daily
            {
                wherstr = " and Vdate= " + access_sql.Hash + dt + access_sql.Hash;
            }
            prospective = Database.GetScalarInt("SELECT Max(Vnumber) AS Expr1 FROM VOUCHERINFOs, VOUCHERTYPEs WHERE VOUCHERINFOs.Vt_id=VOUCHERTYPEs.Vt_id AND VOUCHERTYPEs.Code= (SELECT VOUCHERTYPEs.Code FROM VOUCHERTYPEs WHERE LocationId='" + Database.LocationId + "' and VOUCHERTYPEs.Vt_id=" + vtid + wherstr + ")") + 1;

            if (prospective == 1)
            {
                prospective = Database.GetScalarInt("Select starting_no from VOUCHERTYPEs where vt_id=" + vtid); 
                //prospective = Database.GetScalarInt("Select starting_no from VOUCHERTYPEs where vt_id=" + vtid); 
            }

            //date verification
            String Pre = "";
            String nex = "";

            if (numtype == 1)  //yearly
            {
                Pre = Database.GetScalarDate("SELECT Max(VOUCHERINFOs.Vdate) As Vdate FROM VOUCHERINFOs WHERE LocationId='" + Database.LocationId + "' and (((VOUCHERINFOs.Vnumber)<" + prospective + ") AND ((VOUCHERINFOs.Vt_id)=" + vtid + ") and vi_id <>'" + vid + "')");
                nex = Database.GetScalarDate("SELECT Min(VOUCHERINFOs.Vdate) AS Vdate FROM VOUCHERINFOs WHERE LocationId='" + Database.LocationId + "' and (((VOUCHERINFOs.Vnumber)>" + prospective + ") AND ((VOUCHERINFOs.Vt_id)=" + vtid + ") and vi_id <>'" + vid + "')");
            }
            else if (numtype == 2) //monthly
            {
                Pre = Database.GetScalarDate("SELECT Max(VOUCHERINFOs.Vdate) As Vdate FROM VOUCHERINFOs WHERE LocationId='" + Database.LocationId + "' and VOUCHERINFOs.Vnumber<" + prospective + " AND VOUCHERINFOs.Vt_id=" + vtid + " and (month(vdate)=" + DateTime.Parse(dt).Month + ") and vi_id <>'" + vid + "'");
                nex = Database.GetScalarDate("SELECT Min(VOUCHERINFOs.Vdate) As Vdate FROM VOUCHERINFOs WHERE LocationId='" + Database.LocationId + "' and VOUCHERINFOs.Vnumber>" + prospective + " AND VOUCHERINFOs.Vt_id=" + vtid + " and (month(vdate)=" + DateTime.Parse(dt).Month + ") and vi_id <>'" + vid + "'");
            }
            else if (numtype == 3) //daily
            {
                Pre = "";
                nex = "";
            }
            if (Pre == "" && nex == "")
            {
                return prospective;
            }
            else if (DateTime.Parse(dt) >= DateTime.Parse(Pre) && nex == "")
            {
                return prospective;
            }
            else if (DateTime.Parse(dt) >= DateTime.Parse(Pre) && DateTime.Parse(dt) <= DateTime.Parse(nex))
            {
                return prospective;
            }
            else
            {
                return 0;
            }
        }

        public static String select_rpt_copy(int vtid, int cpy)
        {
            string columnname = "";
            DataTable dtOptions = new DataTable();
            dtOptions.Clear();
            if (cpy == 1)
            {
                columnname = "Default1";
            }
            else if (cpy == 2)
            {
                columnname = "Default2";
            }
            else if (cpy == 3)
            {
                columnname = "Default3";
            }

            return Database.GetScalarText("select [" + columnname + "] from VoucherTypes where Vt_id=" + vtid);
        }

        public static double Roundoff(String tempamt)
        {
            double amt = 0;
            amt = double.Parse(tempamt);
            amt = Math.Round(amt);
            return amt;
        }

        public static void OpenFrm(System.Windows.Forms.Form thisfrm,string v_id,bool resave)
        {
            Boolean TdType = false;
            string frmName = "";
            DataTable dtTdType = new DataTable();
            Database.GetSqlData("select tdtype,VOUCHERTYPEs.type as vname from voucherinfos,VOUCHERTYPEs  where voucherinfos.vt_id=VOUCHERTYPEs.vt_id and vi_id='" + v_id.ToString()+"'" , dtTdType);
            if (dtTdType.Rows.Count > 0)
            {
                TdType = Boolean.Parse(dtTdType.Rows[0][0].ToString());
                frmName = dtTdType.Rows[0][1].ToString();
            }
            string vid = v_id;
            if (frmName == "Receipt")
            {
                frmCashRec frm = new frmCashRec();
                frm.recpay = "Receipt";
                frm.cmdnm = "edit";
                frm.Text = "Edit Receipt";
                frm.MdiParent = thisfrm.MdiParent;
                if (resave == true)
                {}
                else
                {
                    frm.Show();
                }
                frm.LoadData(vid.ToString(), frm.Text);
            }           
            else if (frmName == "Payment")
            {
                frmCashRec frm = new frmCashRec();
                frm.recpay = "Payment";
                frm.cmdnm = "edit";
                frm.Text = "Edit Payment";
                frm.MdiParent = thisfrm.MdiParent;
                if (resave == true)
                {}
                else
                {
                    frm.Show();
                }
                frm.LoadData(vid.ToString(), frm.Text);
            }
            else if (frmName == "Contra")
            {
                frmCashRec frm = new frmCashRec();
                frm.recpay = "Contra";
                frm.cmdnm = "edit";
                frm.Text = "Edit Contra";
                frm.MdiParent = thisfrm.MdiParent;
                if (resave == true)
                {
                
                }
                else
                {
                    frm.Show();
                }
                frm.LoadData(vid.ToString(), frm.Text);
            }

            else if (frmName == "Challan")
            {
               
                frm_Challan frm = new frm_Challan();
                frm.gresave = resave;
                frm.LoadData(vid.ToString(), "Challan");
                frm.MdiParent = thisfrm.MdiParent;
                if (resave == true)
                {

                }
                else
                {
                    frm.Show();
                }
                //frm.LoadData(vid.ToString(), frm.Text);
            }


            else if (frmName == "Booking")
            {

                frmBooking frm = new frmBooking();
                frm.gresave = resave;
                frm.LoadData(vid.ToString(), "Booking");
              
                frm.MdiParent = thisfrm.MdiParent;
                if (resave == true)
                {

                }
                else
                {
                    frm.Show();
                }
               // frm.LoadData(vid.ToString(), frm.Text);
            }


            else if (frmName == "Stock Transfer")
            {

                frmStockTransfer frm = new frmStockTransfer();
                frm.gresave = resave;
                frm.LoadData(vid.ToString(), "Stock Transfer");
                frm.MdiParent = thisfrm.MdiParent;
                if (resave == true)
                {

                }
                else
                {
                    frm.Show();
                }
               // frm.LoadData(vid.ToString(), frm.Text);
            }


            else if (frmName == "Unloading")
            {

                frm_unloading frm = new frm_unloading();
                frm.gresave = resave;
                frm.LoadData(vid.ToString(), "Unloading");
                frm.MdiParent = thisfrm.MdiParent;
                if (resave == true)
                {

                }
                else
                {
                    frm.Show();
                }
                //frm.LoadData(vid.ToString(), frm.Text);
            }


            else if (frmName == "Delivery")
            {

                frm_Delivery frm = new frm_Delivery();
                frm.gresave = resave;
                frm.LoadData(vid.ToString(), "Delivery");
                frm.MdiParent = thisfrm.MdiParent;
                if (resave == true)
                {

                }
                else
                {
                    frm.Show();
                }
               // frm.LoadData(vid.ToString(), frm.Text);
            }


            else if (frmName == "Journal")
            {
                frmJournal frm = new frmJournal();
                frm.Text = "Journal Voucher";
                frm.cmdmode = "edit";
                frm.MdiParent = thisfrm.MdiParent;
                if (resave == true)
                {
                }
                else
                {
                    frm.Show();
                }
                frm.LoadData(vid.ToString(), frm.Text);
            }
            else if (frmName == "issue" || frmName == "receive")
            {
                return;
            }
            if (Feature.Available("Close Form After Report") == "Yes")
            {
                thisfrm.Close();
            }
        }
    }
}
