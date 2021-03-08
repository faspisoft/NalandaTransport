using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Data;
using System.Data.OleDb;

using System.IO;
using System.Management;
using System.Management.Instrumentation;
namespace faspi
{
    class Dongle
    {
        
        // Specify the COPYLOCK DLL to be used based on application mode (Single User or Multi-user)
        // you may also rename original default DLL file name and use this name here.
        //const String dll_COPYLOCK = "CL32.DLL"; //Standalone application
         static int dResult;
         static String strCmd;
        private static string server = "108.170.54.207";
        private static string database = "amangupt_cms";
        private static string uid = "amangupt_cms";
        private static string password = "admin@#123";
        static string connectionString = "SERVER=" + server + ";" + "DATABASE=" + database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";
     //  static MySqlConnection cn = new MySqlConnection(connectionString);
        const String dll_COPYLOCK = "NETCL32.DLL"; //Multiuser application

        [DllImport(dll_COPYLOCK)]
        public static extern int cl_login(string name);
        [DllImport(dll_COPYLOCK)]
        public static extern int cl_get_model([MarshalAs(UnmanagedType.LPArray)] byte[] dModel);
        [DllImport(dll_COPYLOCK)]
        public static extern int cl_lock_ok();
        [DllImport(dll_COPYLOCK)]
        public static extern int cl_get_id([MarshalAs(UnmanagedType.LPArray)] byte[] dID, String rPass);
        [DllImport(dll_COPYLOCK)]
        public static extern int cl_get_batch([MarshalAs(UnmanagedType.LPArray)] byte[] dBatch, String rPass);
        [DllImport(dll_COPYLOCK)]
        public static extern int cl_set_sign(String dSign, String wPass);
        [DllImport(dll_COPYLOCK)]
        public static extern int cl_get_sign([MarshalAs(UnmanagedType.LPArray)] byte[] dSign, String rPass);
        [DllImport(dll_COPYLOCK)]
        public static extern int cl_set_osign(String dSign, String wPass);
        [DllImport(dll_COPYLOCK)]
        public static extern int cl_get_osign([MarshalAs(UnmanagedType.LPArray)] byte[] dSign, String rPass);
        [DllImport(dll_COPYLOCK)]
        public static extern int cl_write_block(String dBuf, String wPass, int BlockNo, int StPos, int Count);
        [DllImport(dll_COPYLOCK)]
        public static extern int cl_read_block([MarshalAs(UnmanagedType.LPArray)] byte[] dBuf, String rPass, int BlockNo, int StPos, int Count);
        [DllImport(dll_COPYLOCK)]
        public static extern int cl_write_word(int dWord, String wPass, int BlockNo, int StPos);
        [DllImport(dll_COPYLOCK)]
        public static extern int cl_read_word(ref int dWord, String rPass, int BlockNo, int StPos);
        [DllImport(dll_COPYLOCK)]
        public static extern int cl_logout();

        

        private  static String DisplayResult(int dResult)
        {
            String res;
            switch (dResult)
            {
                case 1:
                    res = "Success";
                    break;

                case -1:
                    res = "Error: Lock is missing";
                    break;

                case -2:
                    res = "Error: Driver load Error";
                    break;

                case -3:
                    res = "Error: Incorrect password";
                    break;

                case -4:
                    res = "Error: Fascility N.A.";
                    break;

                case -5:
                    res = "Error: Write failed";
                    break;

                case -6:
                    res = "Error: Login users limit is over";
                    break;

                case -7:
                    res = "Error: Link to the server is not active";
                    break;

                case -9:
                    res = "Error: Cannot establish connection with server";
                    break;

                case -12:
                    res = "Error: Time period expired";
                    break;

                default:
                    res = "Error: Internal Error";
                    break;
            }
            return res;
        }
            
        public static void cllogin(bool isActivating)
        {
            if (CheckOldDongle(getDongleFromData()) == true)
            {
                return;
            }
          
            if (isActivated() == false && isActivating == false)
            {
                return;
            }
            
            
            dResult = cl_login("091330904510090212090222");
            
            if (dResult != 1)
            {
                DialogResult ch = MessageBox.Show(DisplayResult(dResult), "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                if (ch == DialogResult.Retry)
                {
                    cllogin(isActivating);
                }
                else
                {
                    Environment.Exit(0);
                }
            }
            else
            {
                if (getDongleNumber() == "AFFA2001" && isActivating == false)
                {
                    return;
                }
                else if ((getDongleNumber() != getDongleFromData()) && isActivating == false)
                {
                    DialogResult ch = MessageBox.Show("Wrong Dongle Found", "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                    if (ch == DialogResult.Retry)
                    {
                        cllogin(isActivating);
                    }
                    else
                    {
                        Environment.Exit(0);
                    }
                }
               
                else if (isActivating == true)
                {
                    return;
                }
                

            }
        }

        public static void cllogout()
        {
            cl_logout();
        }

        public  static void lockOk()
        {
            if (CheckOldDongle(getDongleFromData()) == true)
            {
                return;
            }
            
            if (isActivated() == false)
            {
                return;
            }
            dResult = cl_lock_ok();
            if (dResult != 1)
            {
                DialogResult ch = MessageBox.Show(DisplayResult(dResult), "Error", MessageBoxButtons.RetryCancel);
                if (ch == DialogResult.Retry)
                {
                    lockOk();
                }
                else
                {
                    Environment.Exit(0);
                }
            }
            
        }

        public static String getDongleNumber()
        {
            byte[] dBuf = new byte[50];
            dResult = cl_get_osign(dBuf, "guptar");
            String dn;
            dn = System.Text.Encoding.ASCII.GetString(dBuf, 0, 8);
            return dn;
        }

        //public bool ourCustomer()
        //{
        //    String dnum = getDongleNumber();
        //    if (dnum == Connection.dno)
        //        return true;
        //    else
        //        return false;
        //}

        public static bool activate()
        { 
            //try
            //{
            //    cn.Open();
            //}
            //catch (MySqlException ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
            //String dongleNum = "";
            //bool ch = ourCustomer();
            //if (ch == false)
            //{
            //    MessageBox.Show("Dongle not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return false;
            //}
            //else
            //{
            //    dongleNum = getDongleNumber();
            //}
            //cllogin(true);
            //DataTable dtActivate = new DataTable();
            //dtActivate.Clear();
            //MySqlDataAdapter mysqlda = new MySqlDataAdapter("select * from registered where dongle='" + getDongleNumber() + "'", cn);
            //mysqlda.Fill(dtActivate);
            //cn.Close();
            //if (dtActivate.Rows.Count > 0)
            //{
            //    //MessageBox.Show("read");
            //    bool res;
            //    Database.OpenConnection();
            //    strCmd = "delete from registered";
            //    OleDbCommand cmd = new OleDbCommand(strCmd, Database.AccessCnn);
            //    res = bool.Parse(cmd.ExecuteNonQuery().ToString());
            //    strCmd = "insert into registered values('" + dtActivate.Rows[0][0] + "','" + dtActivate.Rows[0][1] + "','" + dtActivate.Rows[0][2] + "','" + dtActivate.Rows[0][3] + "','" + dtActivate.Rows[0][4] + "'," + dtActivate.Rows[0][5] + ",'" + dtActivate.Rows[0][6] + "','" + dtActivate.Rows[0][7] + "','" + dtActivate.Rows[0][8] + "','" + dtActivate.Rows[0][9] + "')";
            //    cmd = new OleDbCommand(strCmd, Database.AccessCnn);
            //    res = bool.Parse(cmd.ExecuteNonQuery().ToString());
            //    if (res == true)
            //    {
            //        MessageBox.Show("Thanks Mr. " + dtActivate.Rows[0][1] + " " + dtActivate.Rows[0][0] + "(" + dtActivate.Rows[0][4] + ") for activating our product");
            //        cn.Close();
            //        frm_main frm = new frm_main();
            //        //frm.setMenu();
            //        return true;
            //    }
            //    cn.Close();
            //}
            //else
            //{
            //    MessageBox.Show("No data found on server.Please contact your vendor", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
            //return false;
            return true;
        }

        public static bool isActivated()
        {
            Database.OpenConnection();
            strCmd = "select Active from feature where features='Activated'";
            bool active = false;
            DataTable dtChkActivated = new DataTable();
            Database.GetSqlData(strCmd, dtChkActivated);
            if (dtChkActivated.Rows.Count > 0)
            {
                active = bool.Parse(dtChkActivated.Rows[0][0].ToString());
            }

            Database.CloseConnection();
            return active;
        }

        public static String getDongleFromData()
        {
            string dStr="";
            DataTable dtStr=new DataTable();
           // strCmd = "select * from activate where [Column]='Dongle' or [Column]='Referral'";
            strCmd="SELECT  [Column], [Value] FROM ACTIVATE WHERE     ([Column] = 'Dongle' OR [Column] = 'Referral') AND (Value <> '')";
            Database.GetSqlData(strCmd, dtStr);
            if (dtStr.Rows.Count > 0)
            {
                DataTable tdt = new DataTable();
                DataRow[] drow;

                drow = dtStr.Select("Value='" + getDongleNumber() + "'");

                if (drow.GetLength(0) > 0)
                {
                    tdt = drow.CopyToDataTable();
                    dStr = tdt.Rows[0][1].ToString();
                }

                
                //for (int i = 0; i < dtStr.Rows.Count; i++)
                //{
                //    if (getDongleNumber() == dtStr.Rows[i]["value"].ToString())
                //    {
                //        dStr = dtStr.Rows[i]["Value"] + "";
                //        return dStr;
                //    }
                   
                //}
                
            }

            return dStr;

        }

        private static bool CheckOldDongle(string Number)
        {
            bool reval = false;
            DriveInfo[] allDrives = DriveInfo.GetDrives();

            foreach (DriveInfo d in allDrives)
            {
                string drive = d.Name.Substring(0, 1);
                ManagementObject disk = new ManagementObject("win32_logicaldisk.deviceid=\"" + drive + ":\"");
                disk.Get();
                if (disk["VolumeSerialNumber"] != null)
                {
                    if (disk["VolumeSerialNumber"].ToString() == Number)
                    {
                        reval = true;
                    }

                }

            }
            return reval;
        }

    }
}
