using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using faspi;

namespace faspi
{
    class clsCashing
    {

        static DataTable dtAcc;
        static DateTime dtAcclastchange;
        static DataTable dtTransportDetail;
        static DataTable dtVoucherType;

        public static DataTable GetVoucherType(bool isReset=false)
        {
            if (dtVoucherType == null || isReset)
            {
                dtVoucherType = new DataTable();
                string strSql = "select * from vouchertypes";
                Database.GetSqlData(strSql, dtVoucherType);
            }
            return dtVoucherType;
        }


        public static DataTable GetTransPortDetail()
        {
            if (dtTransportDetail == null)
            {
                dtTransportDetail = new DataTable();
                string strSql = "select * from TransportDetails";
                Database.GetSqlData(strSql, dtTransportDetail);
            }
            return dtTransportDetail;
        }


       private static DataTable Get_Accounts()
        {
            dtAcc = new DataTable();

            string strSql = "SELECT ACCOUNTs.Name, ACCOUNTs.Printname, DeliveryPoints.Name AS Station, ACCOUNTs.Address1, ACCOUNTs.Address2, ACCOUNTs.Phone, ACCOUNTs.Tin_number, OTHERs.Name AS Staff, CONTRACTORs.Name AS Agent,ACCOUNTs.Act_id,ACCOUNTs.Ac_id FROM ACCOUNTs LEFT OUTER JOIN CONTRACTORs ON ACCOUNTs.Con_id = CONTRACTORs.Name LEFT OUTER JOIN OTHERs ON ACCOUNTs.Loc_id = OTHERs.Oth_id LEFT OUTER JOIN DeliveryPoints ON ACCOUNTs.SId = DeliveryPoints.DPId ORDER BY ACCOUNTs.Name";

            Database.GetSqlData(strSql, dtAcc);

            return dtAcc;
        }

       public static DataTable GetAccounts(long act_id)
       {
           DataRow[] dtRows = GetAccounts().Select("act_id=" + act_id.ToString());
           if (dtRows.Length == 0) { return GetAccounts().Clone(); }
              return  dtRows.CopyToDataTable();
       }



      public static DataTable GetAccounts()
        {
            string strSql = "";
            object objRes = "";
            DateTime dt;
            DataTable dtt = new DataTable();
            strSql = "select max(lu) as lu from(select max(modify_date) as lu from accounts union select max(modify_date) as lu from others union select max(modify_date) from CONTRACTORs union select max(modify_date) from DeliveryPoints) as a"; // " select max( last_user_update) as lu from sys.dm_db_index_usage_stats as ius inner join sys.objects as so on ius.object_id =so.object_id  where so.name = 'accounts' or so.name = 'CONTRACTORs' or so.name = 'DeliveryPoints' and last_user_update is not null";
            objRes = Database.GetScalar(strSql);
            if (objRes == null || objRes.ToString() == "") { dt = DateTime.Now; } else { dt = DateTime.Parse(objRes.ToString()); }
            
            if (dtAcc == null)
            {            
                dtAcc = Get_Accounts();
                dtAcclastchange = dt;
            }
            else if (dt != dtAcclastchange)
            {
                dtAcc = Get_Accounts();
                dtAcclastchange = dt;
            }
          
            return dtAcc;

        }


    }
}
