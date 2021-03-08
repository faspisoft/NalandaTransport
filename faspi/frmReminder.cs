using FaspiLicenceModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace faspi
{
    public partial class frmReminder : Form
    {
        VMProductmsg objRes;
        public frmReminder()
        {
            InitializeComponent();

            panel1.BackColor = Color.FromArgb(53, 77, 87);
            flowLayoutPanel1.BackColor = Color.FromArgb(53, 77, 87);
        }
        public frmReminder(VMProductmsg _objRes)
        {
            InitializeComponent();
            panel1.BackColor = Color.FromArgb(53, 77, 87);
            flowLayoutPanel1.BackColor = Color.FromArgb(53, 77, 87);

            objRes = _objRes;
        }


        private void Form1_Load(object sender, EventArgs e)
        {

            this.Text = objRes.MessageTitle;
            //label1.Text = objRes.MessageBody;
            webBrowser1.DocumentText = objRes.MessageBody;// +objRes.MessageBody + objRes.MessageBody;

            if (objRes.PayAmount > 0)
            {
                btnPay.Visible = true;
            }
            btnOk.Text = objRes.Button1Label;


        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();

        }
        public static string GetCodedString(string strSource)
        {
            string source = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(strSource));
            return source;
        }
        private void button3_Click(object sender, EventArgs e)
        {

            string strUri = "https://faspi.in/payment/pay.aspx?";
            
            Dictionary<string, object> dicp = new Dictionary<string, object>();
            dicp.Add("prdkey", objRes.ProductKey);
            dicp.Add("payamt", objRes.PayAmount.ToString("0.00"));

            string strData = Newtonsoft.Json.JsonConvert.SerializeObject(dicp);
            strUri += "data=" + GetCodedString(strData);

            System.Diagnostics.Process.Start(strUri);
            this.Close();

        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (objRes.StopSoftware)
            {
                Database.CloseAppImidate = true;
                Application.Exit();
            }
        }


    }
}
