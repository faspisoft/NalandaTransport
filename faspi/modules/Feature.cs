using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;

namespace faspi
{
    class Feature
    {

       static  DataTable dtFeature = null;
                
        public static string Available(String feature)
        {

            if (dtFeature == null) {
                dtFeature = new DataTable();
                Database.GetSqlData("select selected_value,Features from FirmSetups", dtFeature);
            }

            string found = "No";
            //found = Database.GetScalarText("select selected_value from FirmSetups where [Features]='" + feature + "'");
            DataRow[] dtRows = dtFeature.Select("Features='" + feature + "'");
            if (dtRows.Length > 0) { found = dtRows[0]["selected_value"].ToString(); }
            return found;
        }

        //public static bool AvailableLogin(String feature)
        //{
        //    return Database.GetOtherScalarBool("select Active from feature where [Features]='" + feature + "'");
        //}

        //public static bool UserPower(String feature)
        //{
        //    return Database.GetOtherScalarBool("select Active from POWER where [Power]='" + feature + "'");
        //}
    }
}
