using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;


namespace faspi
{
    class Database
    {
        public static DateTime ExeDate = DateTime.Parse("25-Feb-2021");
        //public static String prevUsr;
        public static String fname;
        public static String printtype;
        public static String fyear;
        public static String uname;
        public static String utype;
        public static String upass;
        public static String databaseName;
 
        public static String DatabaseType = "sql";
        public static int OTP;
        public static bool CloseAppImidate = false;
        public static String dformat = "dd-MMM-yyyy";
        public static DateTime ldate = new DateTime();
        public static DateTime stDate = new DateTime();
        public static DateTime enDate = new DateTime();

        public static SqlConnection SqlConn = new SqlConnection();
        public static SqlConnection SqlCnn = new SqlConnection();



        //public static OleDbConnection AccessConn = new OleDbConnection();
        //public static OleDbConnection AccessCnn = new OleDbConnection();
        //public static OleDbConnection MultiConn = new OleDbConnection();


        private static SqlCommand sqlcmd;
        //public static DataTable dtDeleteSync = new DataTable();        
        public static string inipathfile = "";
        public static string LocationId = "";
        public static string LocationNikName = "";
        public static string inipath = "";
        public static string sqlseverpwd = "";

        public static string sqlseveruser = "";
        public static int user_id;
        //private static OleDbCommand accesscmd;
        public static SqlTransaction sqlTran;
        private static SqlTransaction sqlTrana;
        //private static OleDbTransaction AccessTran;
        //private static OleDbTransaction AccessTrana;
        public static string ServerPath = "";
        public static string LastError = "";
        public static string CompanyState_id = "";
        public static string CompanyStation_id = "";
        public static bool LoginbyDb = false;
        public static string LocationCashAcc_id = "";
        public static string LocationExpAcc_id = "";
        public static int trimno = 1;
        public static string Dongleno;

        public static void setVariable(String Cityid,String Stateid, String fnm, String fyr, String unm, String upss, String utyp, String dbName, DateTime dt1, DateTime dt2, int uid)
        {

            CompanyState_id = Stateid;
            CompanyStation_id = Cityid;
            fname = fnm;
            fyear = fyr;
            uname = unm;
            utype = utyp;
            upass = upss;
            databaseName = dbName;
            stDate = DateTime.Parse(dt1.ToString("dd-MMM-yyyy"));
            enDate = DateTime.Parse(dt2.ToString("dd-MMM-yyyy"));
            access_sql.fnhashSinglequote();
            access_sql.fnaccbal();
         
            user_id = uid;
            update28May();
            frmSoftwareUpdates frm = new frmSoftwareUpdates();
            frm.Update();
        //    Dongleno = Database.GetOtherScalarText("Select Value from Activate where [Column]='Dongle'");
            Dongleno = "";
            try
            {
                double.Parse(funs.IndianCurr(123));
                trimno = 1;
            }
            catch (Exception e)
            {
                trimno = 2;
            }

        }

        private static void update28May()
        {
            CommandExecutor("update Accounts set GR_type='T.B.B.' where GR_type='To Be Billed'");
            CommandExecutor("update voucherinfos set PaymentMode='T.B.B.' where PaymentMode='To Be Billed'");
        }

        public static void OpenConnection()
        {
            if (SqlConn.State == ConnectionState.Closed && databaseName != null && databaseName != "")
            {
                SetPath();
                SqlConn.ConnectionString = @"Data Source=" + inipath + ";Initial Catalog=" + Database.databaseName + ";Persist Security Info=True;User ID=" + sqlseveruser + ";password=" + sqlseverpwd + ";Connection Timeout=800";
                SqlConn.Open();
            }
        }

        public static void SetPath()
        {
            ServerPath = Application.StartupPath;
        }

        public static void CloseConnection()
        {
            SqlConn.Close();
        }

        public static bool CommandExecutor(String str)
        {
            OpenConnection();
            sqlcmd = new SqlCommand(str, SqlConn);
            try
            {
                sqlcmd.Transaction = sqlTran;
                //if (sqlcmd.ExecuteScalar() != null)
                //{
                    sqlcmd.ExecuteNonQuery();
                //}
                return true;
            }
            catch (SqlException ex)
            {
                if (ex.Message.Substring(0, 5) != "Table" && (ex.Message != "Primary key already exists.") && ex.Message.Substring(0, 5) != "Field" && ex.Message.Substring(0, 10) == "The change")
                {

                }
                return false;
            }
        }

        public static bool CommandExecutorOther(String str)
        {
            OpenConnection();
            sqlcmd = new SqlCommand(str, SqlCnn);
            try
            {
                sqlcmd.ExecuteNonQuery();
                return true;
            }
            catch (OleDbException ex)
            {
                if (ex.Message.Substring(0, 5) != "Table" && (ex.Message != "Primary key already exists.") && ex.Message.Substring(0, 5) != "Field" && ex.Message.Substring(0, 10) == "The change")
                {
                    MessageBox.Show(ex.Message);
                }
                return false;
            }
        }



        public static int GetOtherScalarInt(String str)
        {
            int res = 0;
            OpenConnection();
            SqlCommand cmd = new SqlCommand(str, SqlCnn);
            cmd.Transaction = sqlTrana;
            if (cmd.ExecuteScalar() != null && cmd.ExecuteScalar().ToString() != "")
            {
                res = int.Parse(cmd.ExecuteScalar().ToString());
            }
            else
            {
                res = 0;
            }
            if (sqlTrana == null || sqlTrana.Connection == null)
            {
                CloseConnection();
            }
            cmd.Dispose();
            return res;
        }

        public static long GetScalarLong(String str)
        {
            long res = 0;
            OpenConnection();
            SqlCommand cmd = new SqlCommand(str, SqlConn);
            cmd.Transaction = sqlTran;
            if (cmd.ExecuteScalar() != null)
            {
                long x = 0;
                if (long.TryParse(cmd.ExecuteScalar().ToString(), out x) == true)
                {
                    res = long.Parse(cmd.ExecuteScalar().ToString());
                }
            }
            cmd.Dispose();
            if (sqlTran == null || sqlTran.Connection == null)
            {
                CloseConnection();
            }
            return res;
        }

        public static int CommandExecutorInt(String str)
        {
            OpenConnection();
            sqlcmd = new SqlCommand(str, SqlConn);
            try
            {
                sqlcmd.Transaction = sqlTran;
                return sqlcmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                return 0;
            }
        }

        public static void SaveOtherData(DataTable dt)
        {
            SqlDataAdapter da = new SqlDataAdapter("select * from " + dt.TableName, SqlCnn);
            SqlCommandBuilder cb = new SqlCommandBuilder();
            cb.QuotePrefix = "[";
            cb.QuoteSuffix = "]";
            cb.DataAdapter = da;
            da.Update(dt);
        }


        public static void SaveData(DataTable dt)
        {
            SqlDataAdapter da = new SqlDataAdapter("select * from " + dt.TableName, SqlConn);
            da.UpdateBatchSize = 50;

            SqlCommandBuilder cb = new SqlCommandBuilder();
           // cb.ConflictOption = ConflictOption.CompareRowVersion;
            cb.QuotePrefix = "[";
            cb.QuoteSuffix = "]";
            cb.DataAdapter = da;
            da.SelectCommand.Transaction = sqlTran;
            
            da.Update(dt);
        }

        public static void SaveData(DataTable dt, String str)
        {
            SqlDataAdapter da = new SqlDataAdapter(str, SqlConn);
            SqlCommandBuilder cb = new SqlCommandBuilder(da);
            da.SelectCommand.Transaction = sqlTran;
            da.Update(dt);
        }

        public static void CommitTran()
        {
            sqlTran.Commit();

        }

        public static void RollbackTran()
        {
            sqlTran.Rollback();
        }

        public static void BeginTran()
        {
            if (SqlConn.State == ConnectionState.Closed)
            {
                SqlConn.Open();
            }
            sqlTran = SqlConn.BeginTransaction();
        }

        public static void GetSqlData(String str, DataTable dt)
        {
           // OpenConnection();
            dt.Clear();
            SqlDataAdapter da = new SqlDataAdapter(str, SqlConn);
            da.SelectCommand.CommandTimeout = 180;
            da.SelectCommand.Transaction = sqlTran;
            da.Fill(dt);
            //CloseConnection();
        }
        public static void GetSqlData(String str, DataSet ds)
        {
            // OpenConnection();
            ds.Tables.Clear();
            SqlDataAdapter da = new SqlDataAdapter(str, SqlConn);
            da.SelectCommand.CommandTimeout = 180;
            da.SelectCommand.Transaction = sqlTran;
            da.Fill(ds);
            //CloseConnection();
        }

        public static void GetOtherSqlData(String str, DataTable dt)
        {
            dt.Clear();
            SetPath();
            if (SqlCnn.ConnectionString == "")
            {
                SqlCnn.ConnectionString = @"Data Source=" + inipath + ";Initial Catalog=loginfo;Persist Security Info=True;User ID=sa;password=" + sqlseverpwd + ";Connection Timeout=100";
            }
            SqlDataAdapter da = new SqlDataAdapter(str, SqlCnn);
            da.Fill(dt);
        }

        public static object GetScalar(String str)
        {
            object res;
            OpenConnection();
            SqlCommand cmd = new SqlCommand(str, SqlConn);
            cmd.Transaction = sqlTran;
            res = cmd.ExecuteScalar();
            cmd.Dispose();

            if (sqlTran == null || sqlTran.Connection == null)
            {
                CloseConnection();
            }
            return res;

        }

        public static String GetScalarText(String str)
        {
            object res = "";
            OpenConnection();
            using (SqlCommand cmd = new SqlCommand(str, SqlConn))
            {
                cmd.Transaction = sqlTran;

                //if (cmd.ExecuteScalar() != null)
                //{
                res = cmd.ExecuteScalar();
                if (res == null) { res = ""; }
                //}
            }
            //cmd.Dispose();
            if (sqlTran == null || sqlTran.Connection == null)
            {
                CloseConnection();
            }
            return res.ToString();
        }

        public static String GetOtherScalarText(String str)
        {
            object res = "";
            using (SqlCommand cmd = new SqlCommand(str, SqlCnn))
            {
                //if (cmd.ExecuteScalar() != null)
                //{
                res = cmd.ExecuteScalar();
                if (res == null) { res = ""; }
                //}
            }
            //cmd.Dispose();
            return res.ToString();
        }

        public static int GetScalarInt(String str)
        {
            int res = 0;
            OpenConnection();
            using (SqlCommand cmd = new SqlCommand(str, SqlConn))
            {
                cmd.Transaction = sqlTran;
                object objRes = cmd.ExecuteScalar();
                //if (cmd.ExecuteScalar() != null && cmd.ExecuteScalar().ToString() != "")
                if(objRes!=null && objRes.ToString()!="")
                {
                    res = int.Parse(objRes.ToString()); //cmd.ExecuteScalar().ToString());
                }
                else
                {
                    res = 0;
                }
            }

            //cmd.Dispose();
            if (sqlTran == null || sqlTran.Connection == null)
            {
                CloseConnection();
            }
            return res;
        }

        public static String GetScalarDate(String str)
        {
            object res = "";
            OpenConnection();
            using (SqlCommand cmd = new SqlCommand(str, SqlConn))
            {
                cmd.Transaction = sqlTran;
                res = cmd.ExecuteScalar();

                //if (cmd.ExecuteScalar().ToString() != null && cmd.ExecuteScalar().ToString() != "")
                if(res!=null && res.ToString()!="")
                {
                    res = DateTime.Parse(res.ToString()).ToString("dd-MMM-yyyy");//cmd.ExecuteScalar().ToString()).ToString("dd-MMM-yyyy");
                }
                else
                {
                    res = "";
                }
            }
            //cmd.Dispose();
            if (sqlTran == null || sqlTran.Connection == null)
            {
                CloseConnection();
            }
            return res.ToString();
        }

        public static bool GetScalarBool(String str)
        {
            bool res = false;
            OpenConnection();
            using (SqlCommand cmd = new SqlCommand(str, SqlConn))
            {
                cmd.Transaction = sqlTran;
                object objRes = cmd.ExecuteScalar();
                //if (cmd.ExecuteScalar() != null && cmd.ExecuteScalar().ToString() != "")
                if(objRes!=null && objRes.ToString()!="")
                {
                    res = bool.Parse(objRes.ToString());//cmd.ExecuteScalar().ToString());
                }
                else
                {
                    res = false;
                }
            }
            //cmd.Dispose();

            if (sqlTran == null || sqlTran.Connection == null)
            {
                CloseConnection();
            }
            return res;
        }

        //public static bool GetOtherScalarBool(String str)
        //{
        //    bool res = false;
        //    OpenConnection();
        //    SqlCommand cmd = new SqlCommand(str, SqlCnn);
        //    cmd.Transaction = sqlTran;
        //    if (cmd.ExecuteScalar() != null && cmd.ExecuteScalar().ToString() != "")
        //    {
        //        res = bool.Parse(cmd.ExecuteScalar().ToString());
        //    }
        //    else
        //    {
        //        res = false;
        //    }
        //    cmd.Dispose();
        //    if (sqlTran == null || sqlTran.Connection == null)
        //    {
        //        CloseConnection();
        //    }
        //    return res;
        //}

        public static Double GetScalarDecimal(String str)
        {
            Double res = 0;
            OpenConnection();
            using (SqlCommand cmd = new SqlCommand(str, SqlConn))
            {
                cmd.Transaction = sqlTran;
                object objRes = cmd.ExecuteScalar();
                if (objRes  != null)
                {

                    Double.TryParse(objRes.ToString(), out res);

                    //if (funs.isDouble(objRes.ToString()))
                    //{
                    //    res = Double.Parse(objRes.ToString());
                    //}
                    //else
                    //{
                    //    res = 0;
                    //}
                }
            }
            //cmd.Dispose();
            if (sqlTran == null || sqlTran.Connection == null)
            {
                CloseConnection();
            }
            return res;
        }

        public static void setFocus(TextBox tb)
        {
            tb.BackColor = System.Drawing.Color.AntiqueWhite;
            tb.ForeColor = System.Drawing.Color.Black;
        }

        public static void lostFocus(TextBox tb)
        {
            tb.BackColor = System.Drawing.Color.White;
            tb.ForeColor = System.Drawing.Color.Black;
        }

        public static void setFocus(DateTimePicker dtp)
        {
            dtp.BackColor = System.Drawing.Color.AntiqueWhite;
            dtp.CalendarMonthBackground = System.Drawing.Color.Black;
        }

        public static void lostFocus(DateTimePicker dtp)
        {
            dtp.BackColor = System.Drawing.Color.White;
            dtp.ForeColor = System.Drawing.Color.Black;
        }

        public static void setFocus(DataGridViewCell cell)
        {
            cell.Style.BackColor = System.Drawing.Color.AntiqueWhite;
            cell.Style.ForeColor = System.Drawing.Color.Black;
        }

        public static void lostFocus(DataGridViewCell cell)
        {
            cell.Style.BackColor = System.Drawing.Color.White;
            cell.Style.ForeColor = System.Drawing.Color.Black;
        }

        public static void FillList(ListBox lb, String str)
        {
            DataTable dtList = new DataTable();
            dtList.Clear();
            GetSqlData(str, dtList);
            lb.DataSource = dtList;
            lb.DisplayMember = dtList.Columns[0].ColumnName;

        }

        public static void FillCombo(ComboBox cb, String str)
        {
            DataTable dtCombo = new DataTable();
            dtCombo.Clear();
            GetSqlData(str, dtCombo);
            cb.DataSource = dtCombo;
            cb.DisplayMember = dtCombo.Columns[0].ColumnName;
        }

        public static void FillCombo(DataGridViewComboBoxColumn gvcb, String str)
        {
            DataTable dtCombo = new DataTable();
            dtCombo.Clear();
            GetSqlData(str, dtCombo);
            gvcb.DataSource = dtCombo;
            gvcb.DisplayMember = dtCombo.Columns[0].ColumnName;
        }

        public static void FillCombo(ComboBox cb, DataTable dtStr, String colName)
        {
            cb.DataSource = dtStr;
            cb.DisplayMember = dtStr.Columns[colName].ColumnName;
        }
    }
}
