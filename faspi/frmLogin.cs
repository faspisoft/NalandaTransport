using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Threading;
using CrystalDecisions.Shared;
using System.IO;
using FoxLearn.License;

namespace faspi
{
    public partial class frmLogin : Form
    {
        String strCmd;
        String strCombo;




        public frmLogin()
        {
            InitializeComponent();
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            //string val = "";

            //val = "Soft;" + ComputerInfo.GetComputerId();
            textBox4.Text = ComputerInfo.GetComputerId();
            Database.OpenConnection();
            Database.CloseConnection();

            System.Diagnostics.Stopwatch objStop = new System.Diagnostics.Stopwatch();
            objStop.Start();
            
            clsCashing.GetAccounts();

            objStop.Stop();
            //MessageBox.Show(objStop.Elapsed.ToString());

            //objStop.Reset();
            //objStop.Start();
            //clsCashing.GetAccounts();
            //objStop.Stop();
            //MessageBox.Show(objStop.Elapsed.ToString());

            //objStop.Reset();
            //objStop.Start();
            //clsCashing.GetAccounts(39);
            //objStop.Stop();
            //MessageBox.Show(objStop.Elapsed.ToString());

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            int randomno;
            Random ran = new Random();
            randomno = ran.Next(999999, 9999999);

            if (textBox1.Text.Trim() == "")
            {
                MessageBox.Show("Enter username");
                textBox1.Focus();
                return;
            }
            if (textBox2.Text.Trim() == "")
            {
                MessageBox.Show("Enter password");
                textBox2.Focus();
                return;
            }
            if (textBox3.Text.Trim() == "")
            {
                MessageBox.Show("Enter Location");
                textBox3.Focus();
                return;
            }
            //bool active = false;




            DataTable dt = new DataTable();
            Database.GetSqlData("select * from USERs where UserName='" + textBox1.Text.Trim() + "' and Password='" + textBox2.Text.Trim() + "'", dt);

            if (dt.Rows.Count > 0)
            {
                
                //if (Feature.Available("Send Sms") == "Yes")
                //{

                //    bool active = Database.GetScalarBool("SELECT Active FROM dbo.WorkStations WHERE     (Sys_Code = '" + textBox4.Text + "')");
                //    if (active == false)
                //    {

                //        active = Database.GetScalarBool("SELECT Active FROM dbo.WorkStations WHERE     (Sys_Code = 'Others System')");
                //        if (active == false)
                //        {
                //            sms objsms = new sms();
                //            string msg = "Workstation {0} is not Registered, Login Failed by {1}";
                //            msg = string.Format(msg, textBox4.Text, textBox1.Text);
                //            objsms.send(msg, "9889401111", "-By Marwari Software");
                //            //MessageBox.Show(msg);
                //            return;
                //        }
                //        else
                //        {
                //            sms objsms = new sms();
                //            string msg = "Workstation {0} is not Registered, Login by {1} as Other System";
                //            msg = string.Format(msg, textBox4.Text, textBox1.Text);
                //            objsms.send(msg, "9889401111", "-By Marwari Software");
                //            // MessageBox.Show(msg);
                //        }

                //    }
                //}




                string fname = Database.GetScalarText("Select Name from location where nick_name='"+ textBox3.Text +"'");
                string fyear = "2019-2020";
                string stateid = Database.GetScalarText("Select State_id from location where nick_name='" + textBox3.Text + "'");
                string curlocation = Database.GetScalarText("Select DP_id from location where nick_name='" + textBox3.Text + "'");

                if (Database.GetScalarInt("SELECT     COUNT(COLUMN_NAME) AS Count FROM INFORMATION_SCHEMA.COLUMNS WHERE (TABLE_NAME = 'USERs')") == 4)
                {
                   // MessageBox.Show("count>0");

                    String dtdate = Database.GetScalarText("select getdate()");
                    Database.ldate = funs.GetIndianTime(DateTime.Parse(dtdate));

                    //MessageBox.Show("login date");\
                    DateTime st = DateTime.Parse("01/04/2018");
                    DateTime end = DateTime.Parse("31/03/2019");
                    //MessageBox.Show("Variable Unset");
                    Database.setVariable(curlocation,stateid, fname, fyear, dt.Rows[0]["UserName"].ToString(), dt.Rows[0]["Password"].ToString(), dt.Rows[0]["UserType"].ToString(), Database.databaseName, DateTime.Parse(st.ToString(Database.dformat)), DateTime.Parse(end.ToString(Database.dformat)), int.Parse(dt.Rows[0]["u_id"].ToString()));
                    //MessageBox.Show("Variable set");
                    Database.LocationId = Database.GetScalarText("select LocationId from location where nick_name='" + textBox3.Text + "'");
                    Database.LocationNikName = textBox3.Text;
                    Database.LocationCashAcc_id = Database.GetScalarText("select cashac_id from location where nick_name='" + textBox3.Text + "'");
                    Database.LocationExpAcc_id = Database.GetScalarText("select expenseacc from location where nick_name='" + textBox3.Text + "'");
                    frm_main frm = new frm_main();
                    //MessageBox.Show("Main form open");
                    frm.random = randomno;
                    frm.Show();
                    this.Hide();
                }
                else
                {
                    string locationid = Database.GetScalarText("Select Location_id from USERs where UserName='" + textBox1.Text.Trim() + "' and Password='" + textBox2.Text.Trim() + "' ");
                    if (locationid == "")
                    
                    {
                        //MessageBox.Show("location bla");
                        String dtdate = Database.GetScalarText("select getdate()");
                        Database.ldate = funs.GetIndianTime(DateTime.Parse(dtdate));

                        //Database.ldate = DateTime.Parse(DateTime.Parse(dtdate).ToString(Database.dformat));

                       // MessageBox.Show("login date set");
                        DateTime st = DateTime.Parse("01/04/2018");
                        DateTime endd = DateTime.Parse("31/03/2020");
                       // MessageBox.Show("st  date set");
                        //MessageBox.Show(DateTime.Parse(st.Date.ToString("dd-MM-yyyy")).ToString());
                        Database.setVariable(curlocation, stateid, fname, fyear, dt.Rows[0]["UserName"].ToString(), dt.Rows[0]["Password"].ToString(), dt.Rows[0]["UserType"].ToString(), Database.databaseName, DateTime.Parse(st.Date.ToString("dd-MMM-yyyy")), DateTime.Parse(endd.Date.ToString("dd-MMM-yyyy")), int.Parse(dt.Rows[0]["u_id"].ToString()));
                       // MessageBox.Show("set var ");
                        Database.LocationId = Database.GetScalarText("select LocationId from location where nick_name='" + textBox3.Text + "'");
                        Database.LocationNikName = textBox3.Text;
                        Database.LocationCashAcc_id = Database.GetScalarText("select cashac_id from location where nick_name='" + textBox3.Text + "'");

                        Database.LocationExpAcc_id = Database.GetScalarText("select expenseacc from location where nick_name='" + textBox3.Text + "'");

                        frm_main frm = new frm_main();
                        frm.random = randomno;
                        frm.Show();
                        this.Hide();

                    }

                    else
                    {
                        Database.LocationId = Database.GetScalarText("select LocationId from location where nick_name='" + textBox3.Text + "'");
                        if (locationid == Database.LocationId)
                        {
                          //  MessageBox.Show("location not bla");
                            String dtdate = Database.GetScalarText("select getdate()");
                            Database.ldate = funs.GetIndianTime(DateTime.Parse(dtdate));


                            DateTime st = DateTime.Parse("01/04/2018");
                            DateTime end = DateTime.Parse("31/03/2019");
                            Database.setVariable(curlocation,stateid, fname, fyear, dt.Rows[0]["UserName"].ToString(), dt.Rows[0]["Password"].ToString(), dt.Rows[0]["UserType"].ToString(), Database.databaseName, st, end, int.Parse(dt.Rows[0]["u_id"].ToString()));
                            Database.LocationId = Database.GetScalarText("select LocationId from location where nick_name='" + textBox3.Text + "'");
                            Database.LocationNikName = textBox3.Text;
                            Database.LocationCashAcc_id = Database.GetScalarText("select cashac_id from location where nick_name='" + textBox3.Text + "'");
                            Database.LocationExpAcc_id = Database.GetScalarText("select expenseacc from location where nick_name='" + textBox3.Text + "'");
                            frm_main frm = new frm_main();
                            frm.random = randomno;
                            frm.Show();
                            this.Hide();

                        }
                        else
                        {
                            
                            MessageBox.Show("Enter Valid Location");
                            textBox3.Focus();
                        }
                    }
                }
                
                
                
            }

            else
            {
                MessageBox.Show("Invalid username or password");
                textBox1.Focus();
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
                Environment.Exit(0);
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox1);
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox1);
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox2);
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox2);
        }

        private void textBox3_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox3);
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            strCombo = "select nick_name As Location from Location order by nick_name";
            textBox3.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
        }

        private void textBox3_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox3);
        }
    }
}
