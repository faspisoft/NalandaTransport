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
        public static string Available(String feature)
        {
            string found = "No";
            found = Database.GetScalarText("select selected_value from FirmSetups where [Features]='" + feature + "'");
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
