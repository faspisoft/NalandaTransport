using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Windows.Forms;

namespace faspi
{
    class DOSReport
    {
        public static void voucherprintOld(string vid)
        {
            DataTable dtFirm = new DataTable();
            Database.GetSqlData("SELECT COMPANY.Name as Name , COMPANY.Cst_no as CompanyMobileno , COMPANY.Tin_no as Tin_no, COMPANY.Email as CompanyEmail, COMPANY.Address1 as Address1, COMPANY.Address2 as Address2, COMPANY.Contactno as CompanyLandline, COMPANY.BankName as BankName,COMPANY.IFSC as IFSC,COMPANY.AccountNo as AccountNo,State.Sname as CompanyState, State.GSTCode as Statecode FROM COMPANY LEFT JOIN State ON COMPANY.CState_id = State.State_id", dtFirm);

            DataTable dtQryVoucher = new DataTable();
            Database.GetSqlData("select * from QryVoucher where vid='" + vid + "' order by Itemsr", dtQryVoucher);

            DataTable dtQryChallan= new DataTable();
            Database.GetSqlData("select * from Qryvoucherdes where vid='" + vid + "'", dtQryChallan);

            DOSPrint dmprnt = new DOSPrint();

            dmprnt.Inicio("abc.txt");

            string str = Environment.NewLine + Environment.NewLine + Environment.NewLine + Environment.NewLine + Environment.NewLine + Environment.NewLine;
            str += dmprnt.SetPageSize6Inch;
            str += dmprnt.HeadingOn;
            str += string.Format("{0,38}{1,-10}", " ", dtQryVoucher.Rows[0]["Invoiceno"].ToString());
            str += Environment.NewLine + Environment.NewLine + Environment.NewLine;

            str += string.Format("{0,37}{1,-11}", " ", DateTime.Parse(dtQryVoucher.Rows[0]["Vdate"].ToString()).ToString(Database.dformat));

            str += Environment.NewLine + Environment.NewLine + Environment.NewLine;
            str += string.Format("{0,35}{1,13}", " ", dtQryVoucher.Rows[0]["Source"].ToString());
            str += dmprnt.HeadingOff;
            str += Environment.NewLine + Environment.NewLine;

            str += string.Format("{0,18}{1,-55}", " ", dtQryVoucher.Rows[0]["Consigner"].ToString()+" "+dtQryVoucher.Rows[0]["Consigner_tin"].ToString());
            str += Environment.NewLine;

            str += dmprnt.HeadingOn;
            str += string.Format("{0,35}{1,13}", " ", dtQryVoucher.Rows[0]["Destination"].ToString());
            str += dmprnt.HeadingOff;
            str += Environment.NewLine;

            str += string.Format("{0,18}{1,-55}", " ", dtQryVoucher.Rows[0]["Consignee"].ToString()+" "+dtQryVoucher.Rows[0]["Consignee_tin"].ToString());
            str += Environment.NewLine;
            str += Environment.NewLine;
            str += Environment.NewLine;
            str += Environment.NewLine;
            str += Environment.NewLine;

            for (int i = 0; i < 10; i++)
            {
                string Qty = "";
                string Description = "";
                string wt = "";
                string chwt = "";
                string rate = "";
                string exp = "";
                string val = "";

                if (i < dtQryVoucher.Rows.Count)
                {
                    Qty = dtQryVoucher.Rows[i]["Quantity"].ToString();
                    Description = dtQryVoucher.Rows[i]["Description"].ToString();
                    wt = dtQryVoucher.Rows[i]["Weight"].ToString();
                    chwt = dtQryVoucher.Rows[i]["ChargedWeight"].ToString();
                    rate = dtQryVoucher.Rows[i]["Rate_am"].ToString();
                }
                if (i < dtQryChallan.Rows.Count)
                {
                    exp = dtQryChallan.Rows[i]["Name"].ToString();
                    val = funs.IndianCurr(double.Parse(dtQryChallan.Rows[i]["Value"].ToString()));
                }
                str += string.Format("{0,-6}{1,-38}{2,8}{3,10}{4,8}{5,2}{6,-15}{7,9}", Qty, Description, wt, chwt, rate, " ", exp, val);
                str += Environment.NewLine;

            }

            str += string.Format("{0,10}{1,-41}{2,-21}{3,-15}{4,9}"," ",dtQryVoucher.Rows[0]["Transport1"].ToString() ,dtQryVoucher.Rows[0]["PaymentMode"].ToString(),"G.Total",funs.IndianCurr(double.Parse(dtQryVoucher.Rows[0]["TotalAmount"].ToString())));
            str += Environment.NewLine;
            str += string.Format("{0,-8}{1,-35}", " ", dtQryVoucher.Rows[0]["Delivery_adrs"].ToString());
            dmprnt.Imp(str);
            dmprnt.Eject();
            dmprnt.Fim();


        }

        public static string voucherprintChallan(string vid, string mode = "Print")
        {
            DataTable dtFirm = new DataTable();

            Database.GetSqlData("select * from location where LocationId='" + Database.LocationId + "'", dtFirm);


            //Database.GetSqlData("SELECT COMPANY.Name as Name , COMPANY.Cst_no as CompanyMobileno , COMPANY.Tin_no as Tin_no, COMPANY.Email as CompanyEmail, COMPANY.Address1 as Address1, COMPANY.Address2 as Address2, COMPANY.Contactno as CompanyLandline, COMPANY.BankName as BankName,COMPANY.IFSC as IFSC,COMPANY.AccountNo as AccountNo,State.Sname as CompanyState, State.GSTCode as Statecode FROM COMPANY LEFT JOIN State ON COMPANY.CState_id = State.State_id", dtFirm);

            DataTable dtQryVoucher = new DataTable();
            Database.GetSqlData("select * from QryVoucher where vid='" + vid + "' order by Itemsr", dtQryVoucher);

            DataTable dtQryChallan = new DataTable();
            Database.GetSqlData("select * from QryChallan where vid='" + vid + "'", dtQryChallan);

            DOSPrint dmprnt = new DOSPrint();
            if (mode == "Print")
            {
                dmprnt.Inicio("LPT1");
            }

            string str = "";
            // str += dmprnt.SetPageSize6Inch;

            str += dmprnt.HeadingOn;
            str += string.Format("{0,-8}", dtFirm.Rows[0]["Name"].ToString());
            str += dmprnt.HeadingOff;
            str += Environment.NewLine;

            str += dmprnt.HeadingOn;
            str += string.Format("{0,-8}", dtFirm.Rows[0]["Address1"].ToString());
            str += dmprnt.HeadingOff;
            str += Environment.NewLine;

            str += dmprnt.HeadingOn;
            str += string.Format("{0,-8}", dtFirm.Rows[0]["Address2"].ToString());
            str += dmprnt.HeadingOff;
            str += Environment.NewLine;

            str += string.Format("{0,40}{1,10}", " ", "Phone: " + dtFirm.Rows[0]["Mobile"].ToString());
            str += Environment.NewLine;

            str += string.Format("{0,40}{1,10}", " ", "GSTIN: " + dtFirm.Rows[0]["GST"].ToString());
            str += Environment.NewLine;

            str += string.Format("{0,40}{1,10}", " ", "Email: " + dtFirm.Rows[0]["Email"].ToString());
            str += Environment.NewLine;

            str += string.Format("{0,-2}{1,-20}{2,-20}", "Challan Number : " + dtQryVoucher.Rows[0]["Invoiceno"].ToString(), " Truck Number : " + dtQryChallan.Rows[0]["Truck_no"].ToString(), " Station From : " + dtQryVoucher.Rows[0]["Source"].ToString());
            str += Environment.NewLine + Environment.NewLine;

            str += string.Format("{0,-2}{1,-20}{2,-20}", "Challan Date : " + DateTime.Parse(dtQryVoucher.Rows[0]["Vdate"].ToString()).ToString(Database.dformat), " Driver Name : " + dtQryChallan.Rows[0]["Driver"].ToString(), " Station To : " + dtQryVoucher.Rows[0]["destination"].ToString());
            str += Environment.NewLine + Environment.NewLine;

            str += string.Format("{0,-8}{1,-8}{2,-4}{3,-4}{4,-8}{5,-16}{6,8}{7,8}{8,8}{9,8}", "GRno", "Date", "From", "To", "Content", "Party", "Nug", "Weight", "Freight", "ToPay");
            str += Environment.NewLine;

            double foc = 0, paid = 0, pay = 0, bill = 0, totqty = 0, totwt = 0, totfrt = 0, tot = 0;

            for (int i = 0; i < dtQryChallan.Rows.Count; i++)
            {
                int sno = i + 1;

                string grno = dtQryChallan.Rows[i]["grno"].ToString();
                string date = DateTime.Parse(dtQryChallan.Rows[i]["booking_date"].ToString()).ToString("dd-MMM");
                string source = dtQryChallan.Rows[i]["source"].ToString().Substring(0, 3);
                string destination = dtQryChallan.Rows[i]["destination"].ToString().Substring(0, 3);
                string Description = dtQryChallan.Rows[i]["Description"].ToString().Substring(0, Math.Min(dtQryChallan.Rows[i]["Description"].ToString().Length, 7));

                string consigner = dtQryChallan.Rows[i]["consigner"].ToString().Substring(0, Math.Min(dtQryChallan.Rows[i]["consigner"].ToString().Length, 15));
                string Qty = funs.IndianCurr(double.Parse(dtQryChallan.Rows[i]["Quantity"].ToString()));
                string wt = funs.IndianCurr(double.Parse(dtQryChallan.Rows[i]["weight"].ToString()));
                string rate = funs.IndianCurr(double.Parse(dtQryChallan.Rows[i]["Amount"].ToString()));
                string Topay = "";

                if (dtQryChallan.Rows[i]["gr_type"].ToString() == "To Pay")
                {
                    Topay = (Math.Ceiling(double.Parse(dtQryChallan.Rows[i]["ItemAmount"].ToString())) * 1).ToString();
                    //Topay = Math.Round(double.Parse(dtQryChallan.Rows[i]["ItemAmount"].ToString())).ToString();
                }

                str += string.Format("{0,-8}{1,-8}{2,-4}{3,-4}{4,-8}{5,-16}{6,8}{7,8}{8,8}{9,8}", grno, date, source, destination, Description, consigner, Qty, wt, rate, Topay);
                str += Environment.NewLine;

                if (Topay == "")
                {
                    Topay = "0";
                }
                totqty = totqty + double.Parse(Qty);
                totwt = totwt + double.Parse(wt);
                totfrt = totfrt + double.Parse(rate);
                tot = tot + double.Parse(Topay);

                if (dtQryChallan.Rows[i]["gr_type"].ToString() == "To Pay")
                {
                    pay = pay + Math.Ceiling(double.Parse(dtQryChallan.Rows[i]["ItemAmount"].ToString())) * 1;
                    //pay = pay + Math.Round(double.Parse(dtQryChallan.Rows[i]["ItemAmount"].ToString()));
                }
                else if (dtQryChallan.Rows[i]["gr_type"].ToString() == "Paid")
                {
                    paid = paid + Math.Ceiling(double.Parse(dtQryChallan.Rows[i]["ItemAmount"].ToString())) * 1;
                    //paid = paid + Math.Round(double.Parse(dtQryChallan.Rows[i]["ItemAmount"].ToString()));
                }
                else if (dtQryChallan.Rows[i]["gr_type"].ToString() == "T.B.B.")
                {
                    bill = bill + Math.Ceiling(double.Parse(dtQryChallan.Rows[i]["ItemAmount"].ToString())) * 1;
                    //bill = bill + Math.Round(double.Parse(dtQryChallan.Rows[i]["ItemAmount"].ToString()));
                }
                else if (dtQryChallan.Rows[i]["gr_type"].ToString() == "FOC")
                {
                    foc = foc + Math.Ceiling(double.Parse(dtQryChallan.Rows[i]["ItemAmount"].ToString())) *1;
                    //foc = foc + Math.Round(double.Parse(dtQryChallan.Rows[i]["ItemAmount"].ToString()));
                }
            }

            str += string.Format("{0,-8}", "--------------------------------------------------------------------------------");
            str += Environment.NewLine;

            str += string.Format("{0,-8}{1,-8}{2,-4}{3,-4}{4,-8}{5,-16}{6,8}{7,8}{8,8}{9,8}", "", "", "", "", "", "Total", funs.IndianCurr(totqty), funs.IndianCurr(totwt), funs.IndianCurr(totfrt), funs.IndianCurr(tot));
            str += Environment.NewLine;

            str += string.Format("{0,-8}", "--------------------------------------------------------------------------------");
            str += Environment.NewLine;

            str += string.Format("{0,-8}", "Total To Pay " + pay.ToString());
            str += Environment.NewLine;

            str += string.Format("{0,-8}", "Total Paid " + paid.ToString());
            str += Environment.NewLine;

            str += string.Format("{0,-8}", "Total T.B.B. " + bill.ToString());
            str += Environment.NewLine;

            str += string.Format("{0,-8}", "Total FOC " + foc.ToString());
            str += Environment.NewLine;
            str += Environment.NewLine;

            str += string.Format("{0,-8}", "Less DC " + dtQryChallan.Rows[0]["tp1"].ToString());
            str += Environment.NewLine;

            str += string.Format("{0,-8}", "Lorry Freight " + dtQryChallan.Rows[0]["transport2"].ToString());
            str += Environment.NewLine;

            str += string.Format("{0,-8}", "Advance Paid " + dtQryChallan.Rows[0]["transport5"].ToString());
            str += Environment.NewLine;

            str += string.Format("{0,-8}", "Balance Freight " + dtQryChallan.Rows[0]["transport6"].ToString());
            str += Environment.NewLine;

            str += string.Format("{0,-8}", "Freight Pay " + dtQryChallan.Rows[0]["transport6"].ToString());
            str += Environment.NewLine;

            str += string.Format("{0,-8}", "Crossing Charge " + dtQryChallan.Rows[0]["DeliveryAt"].ToString());
            str += Environment.NewLine;

            str += string.Format("{0,-8}", "DD " + dtQryChallan.Rows[0]["DD"].ToString());
            str += Environment.NewLine;

            str += string.Format("{0,-8}", "Paid Freight " + dtQryChallan.Rows[0]["Transport4"].ToString());
            str += Environment.NewLine;

            str += string.Format("{0,-8}", "DR " + dtQryChallan.Rows[0]["Dr"].ToString());
            str += Environment.NewLine;

            if (mode == "Print")
            {
                dmprnt.Imp(str);
                dmprnt.Eject();
                dmprnt.Fim();
                return "";
            }
            else
            {
                return str;
            }
        }


        public static string voucherprint(string vid, string mode = "Print")
        {
            try
            {
                DataTable dtFirm = new DataTable();

                Database.GetSqlData("select * from location where LocationId='" + Database.LocationId + "'", dtFirm);
                
                DataTable dtQryVoucher = new DataTable();
                Database.GetSqlData("select * from QryVoucher where vid='" + vid + "' order by Itemsr", dtQryVoucher);

                DataTable dtQryChallan = new DataTable();
                Database.GetSqlData("select * from Qryvoucherdes where vid='" + vid + "'", dtQryChallan);

                DOSPrint dmprnt = new DOSPrint();
                if (mode == "Print")
                {
                    dmprnt.Inicio("LPT1");
                }

                string str = Environment.NewLine + Environment.NewLine + Environment.NewLine + Environment.NewLine + Environment.NewLine + Environment.NewLine;
                str += dmprnt.SetPageSize6Inch;

                str += dmprnt.HeadingOn;

                str += string.Format("{0,30}{1,10}", " ", dtQryVoucher.Rows[0]["Invoiceno"].ToString());
                str += Environment.NewLine + Environment.NewLine;

                str += string.Format("{0,29}{1,-11}", " ", DateTime.Parse(dtQryVoucher.Rows[0]["Vdate"].ToString()).ToString(Database.dformat));

                str += Environment.NewLine + Environment.NewLine + Environment.NewLine + Environment.NewLine;
                str += string.Format("{0,27}{1,13}", " ", dtQryVoucher.Rows[0]["Source"].ToString());
                str += dmprnt.HeadingOff;
                str += Environment.NewLine + Environment.NewLine;

                str += string.Format("{0,15}{1,-40}", " ", dtQryVoucher.Rows[0]["Consigner"].ToString() + " " + dtQryVoucher.Rows[0]["Consigner_tin"].ToString());
                str += Environment.NewLine;

                str += dmprnt.HeadingOn;
                str += string.Format("{0,27}{1,13}", " ", dtQryVoucher.Rows[0]["Destination"].ToString());
                str += dmprnt.HeadingOff;
                str += Environment.NewLine;

                if (bool.Parse(dtQryVoucher.Rows[0]["IsSelf"].ToString()) == true)
                {
                    str += string.Format("{0,15}{1,-40}", " ", "Self");
                }
                else
                {
                    str += string.Format("{0,15}{1,-40}", " ", dtQryVoucher.Rows[0]["Consignee"].ToString() + " " + dtQryVoucher.Rows[0]["Consignee_tin"].ToString());
                }
                str += Environment.NewLine;
                str += Environment.NewLine;
                str += Environment.NewLine;

                for (int i = 0; i < 11; i++)
                {
                    string Qty = "";
                    string Description = "";
                    string wt = "";
                    string chwt = "";
                    string rate = "";
                    string exp = "";
                    string val = "";

                    if (i > 1 && i < dtQryVoucher.Rows.Count + 2)
                    {
                        Qty = Math.Round(decimal.Parse(dtQryVoucher.Rows[i - 2]["Quantity"].ToString()), 0).ToString();
                        Description = dtQryVoucher.Rows[i - 2]["Description"].ToString();
                        wt = Math.Round(decimal.Parse(dtQryVoucher.Rows[i - 2]["Weight"].ToString()), 0).ToString();
                        chwt = Math.Round(decimal.Parse(dtQryVoucher.Rows[i - 2]["ChargedWeight"].ToString()), 0).ToString();
                        rate = Math.Round(decimal.Parse(dtQryVoucher.Rows[i - 2]["Rate_am"].ToString()), 2).ToString();
                    }

                    if (i < dtQryChallan.Rows.Count)
                    {
                        exp = dtQryChallan.Rows[i]["Name"].ToString();
                        val = funs.IndianCurr(double.Parse(dtQryChallan.Rows[i]["Value"].ToString()));
                    }

                    if (dtQryVoucher.Rows[0]["PaymentMode"].ToString() == "T.B.B.")
                    {
                        str += string.Format("{0,-4}{1,-28}{2,8}{3,8}{4,8}{5,2}{6,-13}{7,9}", Qty, Description, wt, chwt, "", " ", "", "");
                    }
                    else
                    {
                        str += string.Format("{0,-4}{1,-28}{2,8}{3,8}{4,8}{5,2}{6,-13}{7,9}", Qty, Description, wt, chwt, rate, " ", exp, val);
                    }
                    
                    str += Environment.NewLine;
                }

                str += dmprnt.HeadingOn;
                str += string.Format("{0,3}{1,-15}{2,-10}", " ", dtQryVoucher.Rows[0]["Transport1"].ToString(), dtQryVoucher.Rows[0]["PaymentMode"].ToString());
                str += dmprnt.HeadingOff;
                if (dtQryVoucher.Rows[0]["PaymentMode"].ToString() == "T.B.B.")
                {
                }
                else
                {
                    str += string.Format("{0,2}{1,-13}{2,9}", " ", "G.Total", funs.IndianCurr(double.Parse(dtQryVoucher.Rows[0]["TotalAmount"].ToString())));
                }
                str += Environment.NewLine;
                str += string.Format("{0,-7}{1,-28}", " ", dtQryVoucher.Rows[0]["Delivery_adrs"].ToString());

                if (mode == "Print")
                {
                    dmprnt.Imp(str);
                    dmprnt.Eject();
                    dmprnt.Fim();
                    return "";
                }
                else
                {
                    return str;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "";
            }
        }
    }
}
