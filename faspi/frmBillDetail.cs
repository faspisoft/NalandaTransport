using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace faspi
{
    public partial class frmBillDetail : Form
    {
        string gStName = "";
        DateTime gDt1 = new DateTime();
        DateTime gDt2 = new DateTime();
        public frmBillDetail()
        {
            InitializeComponent();
        }

        public void LoadData(string stName, DateTime dt1, DateTime dt2)
        {
            gStName = stName;
            gDt1 = dt1;
            gDt2 = dt2;
            string st = "SELECT ACCOUNT.Name AS Consigner, ACCOUNT_1.Name AS Consignee,  VOUCHERINFO.Vnumber as GRNo,  Sum(Voucherdet.Quantity) AS NoOfPack, item.name AS Goods, Sum(Voucherdet.weight) AS Weight, VOUCHERINFO.Totalamount AS Freight FROM ((((VOUCHERINFO LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id) LEFT JOIN Voucherdet ON VOUCHERINFO.Vi_id = Voucherdet.Vi_id) LEFT JOIN item ON Voucherdet.Des_ac_id = item.Id) LEFT JOIN ACCOUNT ON VOUCHERINFO.Ac_id = ACCOUNT.Ac_id) LEFT JOIN ACCOUNT AS ACCOUNT_1 ON VOUCHERINFO.Ac_id2 = ACCOUNT_1.Ac_id where ACCOUNT.Name= '" + stName + "' and (((VOUCHERINFO.PaymentMode)='T.B.B.')) and VOUCHERINFO.Vdate>=#" + dt1.ToString(Database.dformat) + "# And VOUCHERINFO.Vdate<=#" + dt2.ToString(Database.dformat) + "# GROUP BY ACCOUNT.Name, VOUCHERINFO.Vnumber, VOUCHERINFO.Totalamount, item.name, ACCOUNT_1.Name";
            DataTable dtBillDetail = new DataTable();
            Database.GetSqlData(st, dtBillDetail);
            ansGridView5.DataSource = dtBillDetail;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataTable dtSelected = new DataTable();
            dtSelected.Columns.Add("Consigner", typeof(string));
            dtSelected.Columns.Add("Consignee", typeof(string));
            dtSelected.Columns.Add("Vnumber", typeof(string));
            dtSelected.Columns.Add("SumOfQuantity", typeof(string));
            dtSelected.Columns.Add("Goods", typeof(string));
            dtSelected.Columns.Add("Weight", typeof(string));
            dtSelected.Columns.Add("Freight", typeof(string));
            int i = 0;
            foreach (DataGridViewRow row in ansGridView5.Rows)
            {
                dtSelected.Rows.Add();
                dtSelected.Rows[i]["Vnumber"] = ansGridView5.Rows[row.Index].Cells["GRNo"].Value;
                dtSelected.Rows[i]["Consigner"] = ansGridView5.Rows[row.Index].Cells["Consigner"].Value;//
                dtSelected.Rows[i]["Consignee"] = ansGridView5.Rows[row.Index].Cells["Consignee"].Value;
                dtSelected.Rows[i]["SumOfQuantity"] = ansGridView5.Rows[row.Index].Cells["NoOfPack"].Value;
                dtSelected.Rows[i]["Goods"] = ansGridView5.Rows[row.Index].Cells["Goods"].Value;
                dtSelected.Rows[i]["Weight"] = ansGridView5.Rows[row.Index].Cells["Weight"].Value;
                dtSelected.Rows[i]["Freight"] = ansGridView5.Rows[row.Index].Cells["Freight"].Value;
                i++;
            }

            if (ansGridView5.Rows.Count > 0)
            {
                //Report frm = new Report();
                //frm.BillReport(dtSelected, gDt1, gDt2);
                //frm.MdiParent = this.MdiParent;
                //frm.Show();
                //this.Hide();
            }
        }

    }
}
