using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;

namespace faspi
{
    class SelectCombo
    {
        static SelectAcc Objfrm = new SelectAcc();

        public static String ComboKeydown(System.Windows.Forms.Form thisFrm, Keys keyCode, String query, String selectedText, int uptoIndex)
        {
            //keydown
            String str = "";
            if (keyCode == Keys.F4 || keyCode == Keys.Down || keyCode == Keys.F10 || keyCode == Keys.Enter)
            {
                str = callFrm(thisFrm, query, selectedText, uptoIndex);
            }
            else if (keyCode == Keys.Delete)
            {
                str = "";
            }
            else
            {
                str = selectedText;
            }
            if (thisFrm.ActiveControl != null)
            {
                if (str != "" && thisFrm.ActiveControl.GetType() != typeof(faspiGrid.ansGridView))
                {
                    thisFrm.SelectNextControl(thisFrm.ActiveControl, true, true, true, true);
                }
            }

            thisFrm.Activate();
            return str;
        }

        public static String ComboKeypress(System.Windows.Forms.Form thisFrm, char keyChar, String query, String selectedText, int uptoIndex)
        {
            //keypress
            String str = "";
            if (char.IsLetter(keyChar) || char.IsNumber(keyChar) || keyChar == ' ' || keyChar.ToString() == "\r" || keyChar.ToString() == "\t")
            {

                str = callFrm(thisFrm, query, selectedText, uptoIndex);
                if (thisFrm.ActiveControl != null)
                {
                    if (str != "" && thisFrm.ActiveControl.GetType() != typeof(faspiGrid.ansGridView))
                    {
                        thisFrm.SelectNextControl(thisFrm.ActiveControl, true, true, true, true);
                    }
                }

                thisFrm.Activate();
                
            }
            return str;


        }


        public static String ComboDt(System.Windows.Forms.Form thisFrm, DataTable dt, int uptoIndex)
        {
            string str = "";
            Objfrm.Select(dt, "", uptoIndex);
         
            Objfrm.ShowDialog();
            Objfrm.Visible = false;
         
            thisFrm.Activate();
            if (Objfrm.outStr != null)
            {
                str = Objfrm.outStr;
            }
            else
            {
                str = "";
            }
            return str;

        }





        public static String ComboDt1(System.Windows.Forms.Form thisFrm, DataTable dt, int uptoIndex)
        {
            string str = "";
            Objfrm.Select(dt, "", uptoIndex);

       
            thisFrm.Activate();
            if (Objfrm.outStr != null)
            {
                str = Objfrm.outStr;
            }
            else
            {
                str = "";
            }
            return str;

        }




        public static String CallHelp(System.Windows.Forms.Form thisFrm, DataTable dt, String selectedText, int uptoIndex)
        {
            
            String str;
           

            Objfrm.Select(dt, selectedText, uptoIndex);
            
            
            Objfrm.ShowDialog(thisFrm);
            thisFrm.Activate();
            if (Objfrm.outStr != null)
            {
                str = Objfrm.outStr;
            }
            else
            {
                str = "";
            }
            
            return str;
        }

        private static String callFrm(System.Windows.Forms.Form thisFrm, String query, String selectedText, int uptoIndex)
        {

            String str;
            DataTable dtFirm = new DataTable();
            //if (thisFrm.Name == "frmLogin")
            //{
            //    Database.GetOtherSqlData(query, dtFirm);
            //}
            //else
            //{
                Database.GetSqlData(query, dtFirm);
           // }
            //SelectAcc frm;
            //if (selectedText == "")
            //{

            
            
            Objfrm.Select(dtFirm, selectedText, uptoIndex);
            //}

            Objfrm.ShowDialog(thisFrm);
            thisFrm.Activate();
            if (Objfrm.outStr != null)
            {
                str = Objfrm.outStr;
            }
            else
            {
                str = "";
            }
            return str;
        }

       
        public static void IsEnter(Form thisfrm,  Keys keyCode)
        {
            if (keyCode == Keys.Enter)
            {
                thisfrm.SelectNextControl(thisfrm.ActiveControl, true, true, true, true);
            }
            thisfrm.Activate();
           // thisFrm.TopMost = true;
        }
        public static void Isbackspace(Form thisfrm, Keys keyCode)
        {
            if (keyCode == Keys.Back)
            {
                

                thisfrm.SelectNextControl(thisfrm.ActiveControl,false, true, true, true);
            }
            thisfrm.Activate();
            // thisFrm.TopMost = true;
        }
    }
}
