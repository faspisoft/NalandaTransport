using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace faspi
{
    public class JSONResponse
    {
        public b2b[] b2b { get; set; }
        public string fp { get; set; }
        public cdn[] cdn { get; set; }
        public string gstin { get; set; }
    }

    public class fp1
    {
        
        public string financialperiod { get; set; }
       
    }

    public class b2b
    {
        public inv[] inv { get; set; }
        public string cfs { get; set; }
        public string ctin { get; set; }
    }
    public class cdn
    {
        public string cfs { get; set; }
        public nt[] nt { get; set; }
        public string ctin { get; set; }
    }
    public class nt
    {
        public string val { get; set; }
        public itms[] itms { get; set; }
        public string flag { get; set; }
        public string updby { get; set; }
        public string nt_num { get; set; }
        public string inum { get; set; }
        public string cflag { get; set; }
        public string rsn { get; set; }
        public string nt_dt { get; set; }
        public string p_gst { get; set; }
        public string idt { get; set; }
        public string ntty { get; set; }
        public string chksum { get; set; }
    }

    public class inv
    {
        public string val { get; set; }
        public itms[] itms { get; set; }
        public string inv_typ { get; set; }
        public string flag { get; set; }
        public string pos { get; set; }
        public string updby { get; set; }
        public string idt { get; set; }
        public string rchrg { get; set; }
        public string inum { get; set; }
        public string cflag { get; set; }
        public string chksum { get; set; }
    }
    public class itms
    {
        public string num { get; set; }
        public itc itc { get; set; }
        public itm_det itm_det { get; set; }



    }

    public class itc
    {
        public string elg { get; set; }
        public string tx_c { get; set; }
        public string tx_s { get; set; }
        public string tx_i { get; set; }
    }
    public class itm_det
    {
        public string samt { get; set; }
        public string rt { get; set; }
        public string txval { get; set; }
        public string camt { get; set; }
        public string iamt { get; set; }

        public string csamt { get; set; }
    }




}
