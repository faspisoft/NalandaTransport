using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
namespace faspi
{
    public partial class frmBooking : Form
    {
        Stopwatch objStop = new Stopwatch();
        ToolStripProgressBar tspb;
        string strCombo = "";
        int vtid;
        int vno = 0;
        string gStr = "";
        string vid = "0";
        string grno = "";
        bool dosprint = true;
        DataTable dtVoucherInfo;
        DataTable dtVoucherdet;
        DataTable dtJournal;
        DataTable dtCompany;
        bool instk = true;
        bool iscancel = false;
        Boolean RoffChanged = false;
        public Boolean gresave = false;
        Boolean f12used = false;
        string Prelocationid = "";
        public String field1 = "", field2 = "", field3 = "", field4 = "", field5 = "", field6 = "", field7 = "", field8 = "";
        DateTime create_date = DateTime.Parse(System.DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"));

        public frmBooking()
        {
            InitializeComponent();
        }

        private void ansGridView1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (ansGridView1.CurrentCell == null)
            {
                return;
            }
            if (char.IsLetter(e.KeyChar) || char.IsNumber(e.KeyChar) || e.KeyChar == ' ' || Convert.ToInt32(e.KeyChar) == 13)
            {
            }
            else
            {
                return;
            }
            if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "")
            {
                return;
            }

            DataTable dt = new DataTable();
            if (ansGridView1.CurrentCell.OwningColumn.Name == "description")
            {


                if (Feature.Available("Display All items").ToUpper() == "NO")
                {
                    strCombo = "SELECT DISTINCT items.name FROM items RIGHT OUTER JOIN ItemDetails ON items.Id = ItemDetails.Item_id ORDER BY items.name";
                }
                else
                {
                    strCombo = "SELECT DISTINCT items.name FROM items ORDER BY items.name";
                }
                //strCombo = "SELECT DISTINCT item.name FROM item RIGHT OUTER JOIN ItemDetail ON item.Id = ItemDetail.Item_id ORDER BY item.name";
                ansGridView1.CurrentCell.Value = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);

                if (ansGridView1.CurrentCell.Value.ToString() != "")
                {

                    string itemname = ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["description"].Value.ToString();

                    string strSql = "select ";
                    strSql += " isnull((Select Id from items where [name]='" + itemname + "'),'') as itemid,";
                    strSql += " isnull((Select DPId from DeliveryPoints where [name] ='" + textBox3.Text + "'),'') as sorc,";
                    strSql += " isnull((Select DPId from DeliveryPoints where [name] ='" + textBox4.Text + "'),'') as dest,";
                    strSql += " isnull((Select Bharti from Items where name='" + itemname + "'),0) as bharti,";
                    strSql += " isnull((select top 1 ac_id from ACCOUNTs where name='" + textBox1.Text + "' ),'') as ac1id,";
                    strSql += " isnull((select top 1 ac_id from ACCOUNTs where name='" + textBox2.Text + "' ),'') as ac2id";
                    DataTable dtothInfo = new DataTable();
                    Database.GetSqlData(strSql, dtothInfo);

                    string did = dtothInfo.Rows[0]["itemid"].ToString();// funs.Select_item_name_pack_id(ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["description"].Value.ToString());
                    string acid = "";
                    string source = dtothInfo.Rows[0]["sorc"].ToString();// funs.Select_dp_id(textBox3.Text);
                    string destination = dtothInfo.Rows[0]["dest"].ToString();// funs.Select_dp_id(textBox4.Text);

                    ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["itemid"].Value = did;
                    ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["bharti"].Value = dtothInfo.Rows[0]["bharti"];// Database.GetScalarDecimal("Select Bharti from Items where name='" + ansGridView1.CurrentCell.Value.ToString() + "'");
                    ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["multiplier"].Value = 1;

                    if (textBox23.Text == "Consignee")
                    {
                        acid = dtothInfo.Rows[0]["ac2id"].ToString();// funs.Select_ac_id(textBox2.Text);
                    }
                    else
                    {
                        acid = dtothInfo.Rows[0]["ac1id"].ToString();// funs.Select_ac_id(textBox1.Text);
                    }

                    DataTable DtPartyRate = new DataTable();
                    Database.GetSqlData("SELECT * FROM PARTYRATEs WHERE Ac_id = '" + acid + "' AND Des_id = '" + did + "' AND Source_id = '" + source + "' and Destination_id='" + destination + "'", DtPartyRate);

                    if (DtPartyRate.Rows.Count == 1)
                    {
                        ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["miniweight"].Value = funs.IndianCurr(double.Parse(DtPartyRate.Rows[0]["Mini_weight"].ToString()));
                        ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["charged_weight"].Value = funs.DecimalPoint(double.Parse(DtPartyRate.Rows[0]["charged_weight"].ToString()), 2);
                        ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["rounding_ch"].Value = DtPartyRate.Rows[0]["Rounding_ch"].ToString();
                        ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["rounding_ex"].Value = DtPartyRate.Rows[0]["Rounding_ex"].ToString();
                        ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["bharti"].Value = funs.IndianCurr(double.Parse(DtPartyRate.Rows[0]["St_weight"].ToString()));

                        ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["Rate_am"].Value = funs.IndianCurr(double.Parse(DtPartyRate.Rows[0]["Expense0"].ToString()));
                        ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp1rate"].Value = funs.DecimalPoint(double.Parse(DtPartyRate.Rows[0]["expense1"].ToString()), 2);
                        ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp2rate"].Value = funs.DecimalPoint(double.Parse(DtPartyRate.Rows[0]["expense2"].ToString()), 2);
                        ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp3rate"].Value = funs.DecimalPoint(double.Parse(DtPartyRate.Rows[0]["expense3"].ToString()), 2);
                        ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp4rate"].Value = funs.DecimalPoint(double.Parse(DtPartyRate.Rows[0]["expense4"].ToString()), 2);
                        ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp5rate"].Value = funs.DecimalPoint(double.Parse(DtPartyRate.Rows[0]["expense5"].ToString()), 2);
                        ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp6rate"].Value = funs.DecimalPoint(double.Parse(DtPartyRate.Rows[0]["expense6"].ToString()), 2);
                        ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp7rate"].Value = funs.DecimalPoint(double.Parse(DtPartyRate.Rows[0]["expense7"].ToString()), 2);
                        ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp8rate"].Value = funs.DecimalPoint(double.Parse(DtPartyRate.Rows[0]["expense8"].ToString()), 2);
                        ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp9rate"].Value = funs.DecimalPoint(double.Parse(DtPartyRate.Rows[0]["expense9"].ToString()), 2);
                        ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp10rate"].Value = funs.DecimalPoint(double.Parse(DtPartyRate.Rows[0]["expense10"].ToString()), 2);
                        ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp11rate"].Value = funs.DecimalPoint(double.Parse(DtPartyRate.Rows[0]["expense11"].ToString()), 2);

                        ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp1amt"].Value = 0;
                        ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp2amt"].Value = 0;
                        ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp3amt"].Value = 0;
                        ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp4amt"].Value = 0;
                        ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp5amt"].Value = 0;
                        ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp6amt"].Value = 0;
                        ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp7amt"].Value = 0;
                        ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp8amt"].Value = 0;
                        ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp9amt"].Value = 0;
                        ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp10amt"].Value = 0;
                        ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp11amt"].Value = 0;

                        ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["freightmr"].Value = funs.DecimalPoint(double.Parse(DtPartyRate.Rows[0]["mrexpense0"].ToString()), 2);
                        ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp1mr"].Value = funs.DecimalPoint(double.Parse(DtPartyRate.Rows[0]["mrexpense1"].ToString()), 2);
                        ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp2mr"].Value = funs.DecimalPoint(double.Parse(DtPartyRate.Rows[0]["mrexpense2"].ToString()), 2);
                        ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp3mr"].Value = funs.DecimalPoint(double.Parse(DtPartyRate.Rows[0]["mrexpense3"].ToString()), 2);
                        ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp4mr"].Value = funs.DecimalPoint(double.Parse(DtPartyRate.Rows[0]["mrexpense4"].ToString()), 2);
                        ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp5mr"].Value = funs.DecimalPoint(double.Parse(DtPartyRate.Rows[0]["mrexpense5"].ToString()), 2);
                        ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp6mr"].Value = funs.DecimalPoint(double.Parse(DtPartyRate.Rows[0]["mrexpense6"].ToString()), 2);
                        ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp7mr"].Value = funs.DecimalPoint(double.Parse(DtPartyRate.Rows[0]["mrexpense7"].ToString()), 2);
                        ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp8mr"].Value = funs.DecimalPoint(double.Parse(DtPartyRate.Rows[0]["mrexpense8"].ToString()), 2);
                        ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp9mr"].Value = funs.DecimalPoint(double.Parse(DtPartyRate.Rows[0]["mrexpense9"].ToString()), 2);
                        ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp10mr"].Value = funs.DecimalPoint(double.Parse(DtPartyRate.Rows[0]["mrexpense10"].ToString()), 2);
                        ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp11mr"].Value = funs.DecimalPoint(double.Parse(DtPartyRate.Rows[0]["mrexpense11"].ToString()), 2);

                        ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["per"].Value = DtPartyRate.Rows[0]["expenseType0"];
                        ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp1type"].Value = DtPartyRate.Rows[0]["expenseType1"];
                        ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp2type"].Value = DtPartyRate.Rows[0]["expenseType2"];
                        ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp3type"].Value = DtPartyRate.Rows[0]["expenseType3"];
                        ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp4type"].Value = DtPartyRate.Rows[0]["expenseType4"];
                        ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp5type"].Value = DtPartyRate.Rows[0]["expenseType5"];
                        ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp6type"].Value = DtPartyRate.Rows[0]["expenseType6"];
                        ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp7type"].Value = DtPartyRate.Rows[0]["expenseType7"];
                        ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp8type"].Value = DtPartyRate.Rows[0]["expenseType8"];
                        ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp9type"].Value = DtPartyRate.Rows[0]["expenseType9"];
                        ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp10type"].Value = DtPartyRate.Rows[0]["expenseType10"];
                        ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp11type"].Value = DtPartyRate.Rows[0]["expenseType11"];
                    }
                    else
                    {
                        DataTable dt1 = new DataTable();
                        //Database.GetSqlData("select * from itemdetail where Item_id='" + did + "'", dt1)
                        Database.GetSqlData("select itd.*,itm.Mini_weight,itm.charged_weight,itm.Rounding_ch,itm.Rounding_ex from itemdetails as itd left join Items as itm on itd.Item_id=itm.id where  itd.Item_id='" + did + "' and itd.Source_id='" + source + "' and itd.Destination_id='" + destination + "'", dt1);

                        //DataTable dtdes = new DataTable();
                        //Database.GetSqlData("select * from Items where  id='" + did + "'", dtdes);

                        if (dt1.Rows.Count == 1)
                        {
                            ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["miniweight"].Value = funs.IndianCurr(double.Parse(dt1.Rows[0]["Mini_weight"].ToString()));
                            ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["charged_weight"].Value = funs.DecimalPoint(double.Parse(dt1.Rows[0]["charged_weight"].ToString()), 2);
                            ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["rounding_ch"].Value = dt1.Rows[0]["Rounding_ch"].ToString();
                            ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["rounding_ex"].Value = dt1.Rows[0]["Rounding_ex"].ToString();

                            ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["Rate_am"].Value = funs.IndianCurr(double.Parse(dt1.Rows[0]["FreightRate"].ToString()));
                            ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp1rate"].Value = funs.DecimalPoint(double.Parse(dt1.Rows[0]["expense1"].ToString()), 2);
                            ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp2rate"].Value = funs.DecimalPoint(double.Parse(dt1.Rows[0]["expense2"].ToString()), 2);
                            ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp3rate"].Value = funs.DecimalPoint(double.Parse(dt1.Rows[0]["expense3"].ToString()), 2);
                            ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp4rate"].Value = funs.DecimalPoint(double.Parse(dt1.Rows[0]["expense4"].ToString()), 2);
                            ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp5rate"].Value = funs.DecimalPoint(double.Parse(dt1.Rows[0]["expense5"].ToString()), 2);
                            ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp6rate"].Value = funs.DecimalPoint(double.Parse(dt1.Rows[0]["expense6"].ToString()), 2);
                            ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp7rate"].Value = funs.DecimalPoint(double.Parse(dt1.Rows[0]["expense7"].ToString()), 2);
                            ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp8rate"].Value = funs.DecimalPoint(double.Parse(dt1.Rows[0]["expense8"].ToString()), 2);
                            ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp9rate"].Value = funs.DecimalPoint(double.Parse(dt1.Rows[0]["expense9"].ToString()), 2);
                            ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp10rate"].Value = funs.DecimalPoint(double.Parse(dt1.Rows[0]["expense10"].ToString()), 2);
                            ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp11rate"].Value = funs.DecimalPoint(double.Parse(dt1.Rows[0]["expense11"].ToString()), 2);


                            ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp1amt"].Value = 0;
                            ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp2amt"].Value = 0;
                            ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp3amt"].Value = 0;
                            ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp4amt"].Value = 0;
                            ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp5amt"].Value = 0;
                            ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp6amt"].Value = 0;
                            ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp7amt"].Value = 0;
                            ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp8amt"].Value = 0;
                            ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp9amt"].Value = 0;
                            ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp10amt"].Value = 0;
                            ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp11amt"].Value = 0;

                            ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["freightmr"].Value = funs.DecimalPoint(double.Parse(dt1.Rows[0]["MRFreight"].ToString()), 2);
                            ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp1mr"].Value = funs.DecimalPoint(double.Parse(dt1.Rows[0]["mrexpense1"].ToString()), 2);
                            ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp2mr"].Value = funs.DecimalPoint(double.Parse(dt1.Rows[0]["mrexpense2"].ToString()), 2);
                            ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp3mr"].Value = funs.DecimalPoint(double.Parse(dt1.Rows[0]["mrexpense3"].ToString()), 2);
                            ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp4mr"].Value = funs.DecimalPoint(double.Parse(dt1.Rows[0]["mrexpense4"].ToString()), 2);
                            ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp5mr"].Value = funs.DecimalPoint(double.Parse(dt1.Rows[0]["mrexpense5"].ToString()), 2);
                            ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp6mr"].Value = funs.DecimalPoint(double.Parse(dt1.Rows[0]["mrexpense6"].ToString()), 2);
                            ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp7mr"].Value = funs.DecimalPoint(double.Parse(dt1.Rows[0]["mrexpense7"].ToString()), 2);
                            ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp8mr"].Value = funs.DecimalPoint(double.Parse(dt1.Rows[0]["mrexpense8"].ToString()), 2);
                            ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp9mr"].Value = funs.DecimalPoint(double.Parse(dt1.Rows[0]["mrexpense9"].ToString()), 2);
                            ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp10mr"].Value = funs.DecimalPoint(double.Parse(dt1.Rows[0]["mrexpense10"].ToString()), 2);
                            ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp11mr"].Value = funs.DecimalPoint(double.Parse(dt1.Rows[0]["mrexpense11"].ToString()), 2);

                            ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["per"].Value = dt1.Rows[0]["Freightper"];
                            ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp1type"].Value = dt1.Rows[0]["expenseType1"];
                            ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp2type"].Value = dt1.Rows[0]["expenseType2"];
                            ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp3type"].Value = dt1.Rows[0]["expenseType3"];
                            ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp4type"].Value = dt1.Rows[0]["expenseType4"];
                            ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp5type"].Value = dt1.Rows[0]["expenseType5"];
                            ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp6type"].Value = dt1.Rows[0]["expenseType6"];
                            ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp7type"].Value = dt1.Rows[0]["expenseType7"];
                            ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp8type"].Value = dt1.Rows[0]["expenseType8"];
                            ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp9type"].Value = dt1.Rows[0]["expenseType9"];
                            ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp10type"].Value = dt1.Rows[0]["expenseType10"];
                            ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp11type"].Value = dt1.Rows[0]["expenseType11"];
                        }

                        else
                        {
                            ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["miniweight"].Value = 0;
                            ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["charged_weight"].Value = 0;
                            ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["rounding_ch"].Value = 0;
                            ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["rounding_ex"].Value = 0;

                            ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["Rate_am"].Value = 0;
                            ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp1rate"].Value = 0;
                            ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp2rate"].Value = 0;
                            ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp3rate"].Value = 0;
                            ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp4rate"].Value = 0;
                            ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp5rate"].Value = 0;
                            ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp6rate"].Value = 0;
                            ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp7rate"].Value = 0;
                            ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp8rate"].Value = 0;
                            ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp9rate"].Value = 0;
                            ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp10rate"].Value = 0;
                            ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp11rate"].Value = 0;

                            ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp1amt"].Value = 0;
                            ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp2amt"].Value = 0;
                            ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp3amt"].Value = 0;
                            ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp4amt"].Value = 0;
                            ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp5amt"].Value = 0;
                            ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp6amt"].Value = 0;
                            ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp7amt"].Value = 0;
                            ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp8amt"].Value = 0;
                            ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp9amt"].Value = 0;
                            ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp10amt"].Value = 0;
                            ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp11amt"].Value = 0;

                            ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["freightmr"].Value = 0;
                            ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp1mr"].Value = 0;
                            ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp2mr"].Value = 0;
                            ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp3mr"].Value = 0;
                            ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp4mr"].Value = 0;
                            ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp5mr"].Value = 0;
                            ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp6mr"].Value = 0;
                            ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp7mr"].Value = 0;
                            ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp8mr"].Value = 0;
                            ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp9mr"].Value = 0;
                            ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp10mr"].Value = 0;
                            ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp11mr"].Value = 0;

                            ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["per"].Value = "Flat";
                            ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp1type"].Value = "Flat";
                            ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp2type"].Value = "Flat";
                            ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp3type"].Value = "Flat";
                            ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp4type"].Value = "Flat";
                            ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp5type"].Value = "Flat";
                            ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp6type"].Value = "Flat";
                            ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp7type"].Value = "Flat";
                            ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp8type"].Value = "Flat";
                            ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp9type"].Value = "Flat";
                            ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp10type"].Value = "Flat";
                            ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["exp11type"].Value = "Flat";
                        }
                    }
                    ansGridView1.CurrentCell = ansGridView1["unt", ansGridView1.CurrentCell.RowIndex];
                }
            }

            else if (ansGridView1.CurrentCell.OwningColumn.Name == "unt")
            {
                strCombo = "select Name from packings order by Name";
                ansGridView1.CurrentCell.Value = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, ansGridView1.CurrentCell.Value == null ? "" : ansGridView1.CurrentCell.Value.ToString(), 0);
                if (ansGridView1.CurrentCell.Value != "")
                {
                    ansGridView1.CurrentCell = ansGridView1["Quantity", ansGridView1.CurrentCell.RowIndex];
                }
            }
        }


        void  txtbox_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && !(e.KeyChar.ToString() == ".") && !(e.KeyChar.ToString() == "-");
        }


        public void DisplayData()
        {
            ansGridView1.Columns["Quantity"].CellTemplate.ValueType = typeof(double);
            ansGridView1.Columns["Rate_am"].CellTemplate.ValueType = typeof(double);

            ansGridView1.Columns["multiplier"].CellTemplate.ValueType = typeof(double);
            ansGridView1.Columns["weight"].CellTemplate.ValueType = typeof(double);
            ansGridView1.Columns["Chargedweight"].CellTemplate.ValueType = typeof(double);
            ansGridView1.Columns["freightmr"].CellTemplate.ValueType = typeof(double);
            ansGridView1.Columns["amount"].CellTemplate.ValueType = typeof(double);
            flowLayoutPanel2.Controls.Clear();
            DataTable TransportDetails = clsCashing.GetTransPortDetail();// new DataTable();
            //Database.GetSqlData("select * from TransportDetails", TransportDetails);
            for (int i = 0; i < TransportDetails.Rows.Count; i++)
            {
                Label lbl = new Label();
                TextBox txtbox = new TextBox();
                lbl.Text = TransportDetails.Rows[i]["ShowingName"].ToString();
                lbl.Width = 100;
                txtbox.Name = TransportDetails.Rows[i]["FName"].ToString();
                txtbox.Width = 225;

                if (TransportDetails.Rows[i]["status"].ToString() == "Visible")
                {
                    lbl.Visible = true;
                    txtbox.Visible = true;
                }
                else
                {
                    lbl.Visible = false;
                    txtbox.Visible = false;
                }
                flowLayoutPanel2.Controls.Add(lbl);
                flowLayoutPanel2.Controls.Add(txtbox);
                if (bool.Parse(TransportDetails.Rows[i]["Isnumeric"].ToString()) == true)
                {
                    txtbox.KeyPress += new KeyPressEventHandler(txtbox_KeyPress);
                }
               
            }
        }

        private void SideFill()
        {
            flowLayoutPanel1.Controls.Clear();
            DataTable dtsidefill = new DataTable();
            dtsidefill.Columns.Add("Name", typeof(string));
            dtsidefill.Columns.Add("DisplayName", typeof(string));
            dtsidefill.Columns.Add("ShortcutKey", typeof(string));
            dtsidefill.Columns.Add("Visible", typeof(bool));

            //save
            dtsidefill.Rows.Add();
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Name"] = "save";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["DisplayName"] = "Save";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["ShortcutKey"] = "^S";
            if (gStr != "0")
            {
                if (Database.utype == "User")
                {
                    dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = false;
                }
                else
                {
                    dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = true;
                }
            }
            else
            {
                dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = true;
            }

            //print
            dtsidefill.Rows.Add();
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Name"] = "Print";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["DisplayName"] = Database.printtype + " Print";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["ShortcutKey"] = "^P";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = true;




            //change expenses
            dtsidefill.Rows.Add();
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Name"] = "Change Expenses";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["DisplayName"] = "Change Ex.";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["ShortcutKey"] = "";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = true;

            //print preview
            dtsidefill.Rows.Add();
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Name"] = "PrintPre";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["DisplayName"] = "Print Preview";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["ShortcutKey"] = "^W";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = true;


            //Iscancel
            dtsidefill.Rows.Add();
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Name"] = "cancel";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["DisplayName"] = "Cancel";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["ShortcutKey"] = "";

            if (Database.utype == "User")
            {
                dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = false;
            }
            else
            {
                if (vid != "0")
                {
                    if (bool.Parse(dtVoucherInfo.Rows[0]["Iscancel"].ToString()) == true)
                    {
                        dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = false;
                    }
                    else
                    {
                        dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = true;
                    }

                }
                else
                {
                    dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = false;
                }
            }

            //takeback
            dtsidefill.Rows.Add();
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Name"] = "takeback";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["DisplayName"] = "TakeBack";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["ShortcutKey"] = "";

            if (Database.utype == "User")
            {
                dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = false;
            }
            else
            {
                if (vid != "0")
                {
                    if (bool.Parse(dtVoucherInfo.Rows[0]["Iscancel"].ToString()) == true)
                    {
                        dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = true;
                    }
                    else
                    {
                        dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = false;
                    }

                }
                else
                {
                    dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = false;
                }
            }


            //change vnumber
            dtsidefill.Rows.Add();
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Name"] = "vnumber";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["DisplayName"] = "Chng GRno";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["ShortcutKey"] = "^F12";
            if (Database.utype == "User")
            {
                dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = false;
            }
            else
            {
                dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = true;
            }

            //close
            dtsidefill.Rows.Add();
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Name"] = "quit";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["DisplayName"] = "Quit";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["ShortcutKey"] = "Esc";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = true;

            for (int i = 0; i < dtsidefill.Rows.Count; i++)
            {
                if (bool.Parse(dtsidefill.Rows[i]["Visible"].ToString()) == true)
                {
                    Button btn = new Button();
                    btn.Size = new Size(135, 30);
                    btn.Name = dtsidefill.Rows[i]["Name"].ToString();
                    btn.Text = "";
                    Bitmap bmp = new Bitmap(btn.ClientRectangle.Width, btn.ClientRectangle.Height);
                    Graphics G = Graphics.FromImage(bmp);
                    G.Clear(btn.BackColor);
                    string line1 = dtsidefill.Rows[i]["ShortcutKey"].ToString();
                    string line2 = dtsidefill.Rows[i]["DisplayName"].ToString();
                    StringFormat SF = new StringFormat();
                    SF.Alignment = StringAlignment.Near;
                    SF.LineAlignment = StringAlignment.Center;
                    Rectangle RC = btn.ClientRectangle;
                    Font font = new Font("Arial", 12);
                    G.DrawString(line1, font, Brushes.Red, RC, SF);
                    G.DrawString("".PadLeft(line1.Length * 2 + 1) + line2, font, Brushes.Black, RC, SF);
                    btn.Image = bmp;
                    btn.Click += new EventHandler(btn_Click);
                    flowLayoutPanel1.Controls.Add(btn);
                }
            }
        }

        

        

        void btn_Click(object sender, EventArgs e)
        {
            //Button tbtn = (Button)sender;
            //string name = tbtn.Name.ToString();
            string name = "";
            if (gresave == false)
            {
                Button tbtn = (Button)sender;
                name = tbtn.Name.ToString();
            }
            else
            {
                name = "save";
            }
            if (name == "save")
            {
                objStop.Reset();
                objStop.Start();

                if (validate() == true)
                {
                    objStop.Stop();
                    //MessageBox.Show(objStop.Elapsed.ToString());
                    objStop.Start();

                    try
                    {
                        Database.BeginTran();
                        if (gresave == false)
                        {
                            if (Database.utype == "Admin" || gStr == "0")
                            {
                                save();
                            }
                        }
                        else
                        {
                            DataTable dtTemp = new DataTable("Stocks");
                            Database.GetSqlData("Select * from Stocks where Vid='" + vid + "' ", dtTemp);
                            for (int j = 0; j < dtTemp.Rows.Count; j++)
                            {
                                dtTemp.Rows[j].Delete();
                            }
                            Database.SaveData(dtTemp);

                            DataTable dtstocks = new DataTable("Stocks");
                            Database.GetSqlData("select * from Stocks where Vid='" + vid + "'", dtstocks);
                            //dtstocks.Rows.Add();
                            //dtstocks.Rows[0]["Vid"] = vid;
                            //dtstocks.Rows[0]["GR_id"] = vid;
                            //dtstocks.Rows[0]["Quantity"] = 1;
                            //dtstocks.Rows[0]["Step"] = "Step1";
                            //dtstocks.Rows[0]["Godown_id"] = Database.LocationId;
                            //Database.SaveData(dtstocks);
                            if (iscancel == false)
                            {

                                dtstocks.Rows.Add();
                                dtstocks.Rows[0]["Vid"] = vid;
                                dtstocks.Rows[0]["GR_id"] = vid;
                                dtstocks.Rows[0]["Quantity"] = 1;
                                dtstocks.Rows[0]["Step"] = "Step1";
                                dtstocks.Rows[0]["Godown_id"] = Database.LocationId;
                                string aliasname = Database.GetScalarText("Select Aliasname from vouchertypes where vt_id=" + vtid);
                                dtstocks.Rows[dtstocks.Rows.Count - 1]["Narration"] = aliasname + " At " + textBox3.Text;
                                //  dtstocks.Rows[dtstocks.Rows.Count - 1]["GRNo"] = grno;
                                dtstocks.Rows[dtstocks.Rows.Count - 1]["GRDate"] = dateTimePicker1.Value.Date;
                                dtstocks.Rows[dtstocks.Rows.Count - 1]["Consigner_id"] = funs.Select_ac_id(textBox1.Text);
                                dtstocks.Rows[dtstocks.Rows.Count - 1]["Consignee_id"] = funs.Select_ac_id(textBox2.Text);
                                dtstocks.Rows[dtstocks.Rows.Count - 1]["Source_id"] = funs.Select_dp_id(textBox3.Text);
                                dtstocks.Rows[dtstocks.Rows.Count - 1]["Destination_id"] = funs.Select_dp_id(textBox4.Text);
                                dtstocks.Rows[dtstocks.Rows.Count - 1]["grno"] = grno;

                                if (textBox24.Text == "To Pay")
                                {
                                    dtstocks.Rows[dtstocks.Rows.Count - 1]["ToPay"] = textBox10.Text;
                                    dtstocks.Rows[dtstocks.Rows.Count - 1]["TBB"] = 0;
                                    dtstocks.Rows[dtstocks.Rows.Count - 1]["Paid"] = 0;
                                    dtstocks.Rows[dtstocks.Rows.Count - 1]["FOC"] = 0;
                                }
                                else if (textBox24.Text == "FOC")
                                {
                                    dtstocks.Rows[dtstocks.Rows.Count - 1]["ToPay"] = 0;
                                    dtstocks.Rows[dtstocks.Rows.Count - 1]["TBB"] = 0;
                                    dtstocks.Rows[dtstocks.Rows.Count - 1]["Paid"] = 0;
                                    dtstocks.Rows[dtstocks.Rows.Count - 1]["FOC"] = textBox10.Text;
                                }
                                else if (textBox24.Text == "Paid")
                                {
                                    dtstocks.Rows[dtstocks.Rows.Count - 1]["ToPay"] = 0;
                                    dtstocks.Rows[dtstocks.Rows.Count - 1]["TBB"] = 0;
                                    dtstocks.Rows[dtstocks.Rows.Count - 1]["Paid"] = textBox10.Text;
                                    dtstocks.Rows[dtstocks.Rows.Count - 1]["FOC"] = 0;
                                }
                                else if (textBox24.Text == "T.B.B.")
                                {
                                    dtstocks.Rows[dtstocks.Rows.Count - 1]["ToPay"] = 0;
                                    dtstocks.Rows[dtstocks.Rows.Count - 1]["TBB"] = textBox10.Text;
                                    dtstocks.Rows[dtstocks.Rows.Count - 1]["Paid"] = 0;
                                    dtstocks.Rows[dtstocks.Rows.Count - 1]["FOC"] = 0;
                                }
                                dtstocks.Rows[dtstocks.Rows.Count - 1]["TotWeight"] = textBox7.Text;
                                dtstocks.Rows[dtstocks.Rows.Count - 1]["TotPkts"] = textBox16.Text;
                                dtstocks.Rows[dtstocks.Rows.Count - 1]["GRCharge"] = textBox6.Text;
                                dtstocks.Rows[dtstocks.Rows.Count - 1]["GRType"] = textBox24.Text;

                                double othch = 0;
                                othch = double.Parse(textBox26.Text) + double.Parse(textBox12.Text) + double.Parse(textBox13.Text) + double.Parse(textBox14.Text) + double.Parse(textBox15.Text) + double.Parse(textBox17.Text) + double.Parse(textBox18.Text) + double.Parse(textBox19.Text) + double.Parse(textBox21.Text) + double.Parse(textBox22.Text) + double.Parse(textBox9.Text);
                                dtstocks.Rows[dtstocks.Rows.Count - 1]["OthCharge"] = othch;
                                dtstocks.Rows[dtstocks.Rows.Count - 1]["Freight"] = textBox8.Text;
                                dtstocks.Rows[dtstocks.Rows.Count - 1]["ItemName"] = ansGridView1.Rows[0].Cells["description"].Value.ToString();
                                dtstocks.Rows[dtstocks.Rows.Count - 1]["Packing"] = ansGridView1.Rows[0].Cells["unt"].Value.ToString();
                                dtstocks.Rows[dtstocks.Rows.Count - 1]["DeliveryType"] = textBox25.Text;
                                dtstocks.Rows[dtstocks.Rows.Count - 1]["Private"] = field1;
                                dtstocks.Rows[dtstocks.Rows.Count - 1]["Remark"] = field7;
                                Database.SaveData(dtstocks);
                            }

                        }
                        Database.CommitTran();
                    }
                    catch (Exception ex)
                    {
                        Database.RollbackTran();
                        MessageBox.Show("Not Saved Due to an Exception." + ex.Message);
                        this.Close();
                        this.Dispose();
                    }

                    objStop.Stop();
                    //MessageBox.Show(objStop.Elapsed.ToString());

                    SendSMS();
                    clear();

                }
            }
            else if (name == "cancel")
            {
                iscancel = true;
                if (validate() == true)
                {
                    try
                    {
                        Database.BeginTran();


                        if (Database.utype == "Admin")
                        {
                            save();
                        }
                        else if (gStr == "0")
                        {
                            save();
                        }

                        Database.CommitTran();
                    }
                    catch (Exception ex)
                    {
                        Database.RollbackTran();
                        MessageBox.Show("Not Saved Due to an Exception." + ex.Message);
                        this.Close();
                        this.Dispose();
                    }
                    clear();

                }

                //Database.CommandExecutor("Update Vouucherinfos set iscancel='true' where vi_id='"+vid+"'");
            }


            else if (name == "takeback")
            {
                iscancel = false;
                if (validate() == true)
                {
                    try
                    {
                        Database.BeginTran();


                        if (Database.utype == "Admin")
                        {
                            save();
                        }
                        else if (gStr == "0")
                        {
                            save();
                        }

                        Database.CommitTran();
                    }
                    catch (Exception ex)
                    {
                        Database.RollbackTran();
                        MessageBox.Show("Not Saved Due to an Exception." + ex.Message);
                        this.Close();
                        this.Dispose();
                    }
                    clear();

                }

                //Database.CommandExecutor("Update Vouucherinfos set iscancel='false' where vi_id='" + vid + "'");
            }

            else if (name == "Print")
            {
                if (validate() == true)
                {
                    try
                    {
                        Database.BeginTran();
                        if (Database.utype == "Admin")
                        {

                            save();
                        }
                        else if (gStr == "0")
                        {
                            save();
                        }
                        Database.CommitTran();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Not Saved Due to an Exception." + ex.Message);
                        this.Close();
                        this.Dispose();
                    }
                    if (vid != "0")
                    {
                        Print();
                    }
                    SendSMS();
                    clear();
                }
            }


            else if (name == "PrintPre")
            {
                if (validate() == true)
                {
                    try
                    {
                        Database.BeginTran();
                        if (Database.utype == "Admin")
                        {
                            save();
                        }
                        else if (gStr == "0")
                        {
                            save();
                        }
                        Database.CommitTran();
                    }
                    catch (Exception ex)
                    {
                        Database.RollbackTran();
                        MessageBox.Show("Not Saved Due to an Exception." + ex.Message);
                        this.Close();
                        this.Dispose();
                    }
                    SendSMS();
                    view();
                    clear();
                }
            }
            else if (name == "Change Expenses")
            {
                if (ansGridView1.CurrentCell == null)
                {
                    return;
                }
                if (ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["description"].Value != "")
                {
                    frm_other_expenses frm = new frm_other_expenses();
                    frm.LoadDate(ansGridView1, Convert.ToInt32(ansGridView1.CurrentCell.RowIndex));
                    frm.ShowDialog();
                    gridValues(frm.gdt, ansGridView1.CurrentCell.RowIndex);
                }
            }
            else if (name == "quit")
            {
                this.Close();
                this.Dispose();
            }
            else if (name == "vnumber")
            {
                InputBox box = new InputBox("Enter Administrative password", "", true);
                box.ShowDialog(this);
                String pass = box.outStr;
                if (pass.ToLower() == "admin")
                {
                    box = new InputBox("Enter Voucher Number", "", false);
                    box.ShowDialog();
                    if (box.outStr == "")
                    {
                        vno = int.Parse(label10.Text);
                    }
                    else
                    {
                        vno = int.Parse(box.outStr);
                    }



                    label10.Text = vno.ToString();
                    int numtype = funs.chkNumType(vtid);
                    if (numtype != 1)
                    {
                        vid = Database.GetScalarText("Select Vi_id from voucherinfos where LocationId='" + Database.LocationId + "' and  Vt_id=" + vtid + " and Vnumber=" + vno + " and Vdate=" + access_sql.Hash + dateTimePicker1.Value.Date.ToString(Database.dformat) + access_sql.Hash);
                        if (vid == "")
                        {
                            vid = "0";
                        }
                    }
                    else
                    {
                        string tempvid = "";
                        tempvid = Database.GetScalarText("Select Vi_id from voucherinfos where LocationId='" + Database.LocationId + "' and Vt_id=" + vtid + " and Vnumber=" + vno);
                        if (tempvid != "")
                        {
                            MessageBox.Show("Voucher can't be created on this No.");

                            vno = 0;
                            label10.Text = vno.ToString();

                            //  SetVno();
                            return;
                        }
                    }
                    f12used = true;
                }
                else
                {
                    MessageBox.Show("Invalid password");
                }
            }
        }

        private void gridValues(DataGridView dt, int rowIndex)
        {
            ansGridView1.Rows[rowIndex].Cells["exp1rate"].Value = funs.DecimalPoint(double.Parse(dt.Rows[rowIndex].Cells["exp1rate"].Value.ToString()), 2);
            ansGridView1.Rows[rowIndex].Cells["exp2rate"].Value = funs.DecimalPoint(double.Parse(dt.Rows[rowIndex].Cells["exp2rate"].Value.ToString()), 2);
            ansGridView1.Rows[rowIndex].Cells["exp3rate"].Value = funs.DecimalPoint(double.Parse(dt.Rows[rowIndex].Cells["exp3rate"].Value.ToString()), 2);
            ansGridView1.Rows[rowIndex].Cells["exp4rate"].Value = funs.DecimalPoint(double.Parse(dt.Rows[rowIndex].Cells["exp4rate"].Value.ToString()), 2);
            ansGridView1.Rows[rowIndex].Cells["exp5rate"].Value = funs.DecimalPoint(double.Parse(dt.Rows[rowIndex].Cells["exp5rate"].Value.ToString()), 2);
            ansGridView1.Rows[rowIndex].Cells["exp6rate"].Value = funs.DecimalPoint(double.Parse(dt.Rows[rowIndex].Cells["exp6rate"].Value.ToString()), 2);
            ansGridView1.Rows[rowIndex].Cells["exp7rate"].Value = funs.DecimalPoint(double.Parse(dt.Rows[rowIndex].Cells["exp7rate"].Value.ToString()), 2);
            ansGridView1.Rows[rowIndex].Cells["exp8rate"].Value = funs.DecimalPoint(double.Parse(dt.Rows[rowIndex].Cells["exp8rate"].Value.ToString()), 2);
            ansGridView1.Rows[rowIndex].Cells["exp9rate"].Value = funs.DecimalPoint(double.Parse(dt.Rows[rowIndex].Cells["exp9rate"].Value.ToString()), 2);
            ansGridView1.Rows[rowIndex].Cells["exp10rate"].Value = funs.DecimalPoint(double.Parse(dt.Rows[rowIndex].Cells["exp10rate"].Value.ToString()), 2);
            ansGridView1.Rows[rowIndex].Cells["exp11rate"].Value = funs.DecimalPoint(double.Parse(dt.Rows[rowIndex].Cells["exp11rate"].Value.ToString()), 2);

            ansGridView1.Rows[rowIndex].Cells["Rate_am"].Value = funs.DecimalPoint(double.Parse(dt.Rows[rowIndex].Cells["Rate_am"].Value.ToString()), 2);

            ansGridView1.Rows[rowIndex].Cells["exp1amt"].Value = 0;
            ansGridView1.Rows[rowIndex].Cells["exp2amt"].Value = 0;
            ansGridView1.Rows[rowIndex].Cells["exp3amt"].Value = 0;
            ansGridView1.Rows[rowIndex].Cells["exp4amt"].Value = 0;
            ansGridView1.Rows[rowIndex].Cells["exp5amt"].Value = 0;
            ansGridView1.Rows[rowIndex].Cells["exp6amt"].Value = 0;
            ansGridView1.Rows[rowIndex].Cells["exp7amt"].Value = 0;
            ansGridView1.Rows[rowIndex].Cells["exp8amt"].Value = 0;
            ansGridView1.Rows[rowIndex].Cells["exp9amt"].Value = 0;
            ansGridView1.Rows[rowIndex].Cells["exp10amt"].Value = 0;
            ansGridView1.Rows[rowIndex].Cells["exp11amt"].Value = 0;

            ansGridView1.Rows[rowIndex].Cells["exp1mr"].Value = funs.DecimalPoint(double.Parse(dt.Rows[rowIndex].Cells["exp1mr"].Value.ToString()), 2);
            ansGridView1.Rows[rowIndex].Cells["exp2mr"].Value = funs.DecimalPoint(double.Parse(dt.Rows[rowIndex].Cells["exp2mr"].Value.ToString()), 2);
            ansGridView1.Rows[rowIndex].Cells["exp3mr"].Value = funs.DecimalPoint(double.Parse(dt.Rows[rowIndex].Cells["exp3mr"].Value.ToString()), 2);
            ansGridView1.Rows[rowIndex].Cells["exp4mr"].Value = funs.DecimalPoint(double.Parse(dt.Rows[rowIndex].Cells["exp4mr"].Value.ToString()), 2);
            ansGridView1.Rows[rowIndex].Cells["exp5mr"].Value = funs.DecimalPoint(double.Parse(dt.Rows[rowIndex].Cells["exp5mr"].Value.ToString()), 2);
            ansGridView1.Rows[rowIndex].Cells["exp6mr"].Value = funs.DecimalPoint(double.Parse(dt.Rows[rowIndex].Cells["exp6mr"].Value.ToString()), 2);
            ansGridView1.Rows[rowIndex].Cells["exp7mr"].Value = funs.DecimalPoint(double.Parse(dt.Rows[rowIndex].Cells["exp7mr"].Value.ToString()), 2);
            ansGridView1.Rows[rowIndex].Cells["exp8mr"].Value = funs.DecimalPoint(double.Parse(dt.Rows[rowIndex].Cells["exp8mr"].Value.ToString()), 2);
            ansGridView1.Rows[rowIndex].Cells["exp9mr"].Value = funs.DecimalPoint(double.Parse(dt.Rows[rowIndex].Cells["exp9mr"].Value.ToString()), 2);
            ansGridView1.Rows[rowIndex].Cells["exp10mr"].Value = funs.DecimalPoint(double.Parse(dt.Rows[rowIndex].Cells["exp10mr"].Value.ToString()), 2);
            ansGridView1.Rows[rowIndex].Cells["exp11mr"].Value = funs.DecimalPoint(double.Parse(dt.Rows[rowIndex].Cells["exp11mr"].Value.ToString()), 2);
            ansGridView1.Rows[rowIndex].Cells["freightmr"].Value = funs.DecimalPoint(double.Parse(dt.Rows[rowIndex].Cells["freightmr"].Value.ToString()), 2);

            ansGridView1.Rows[rowIndex].Cells["exp1type"].Value = dt.Rows[rowIndex].Cells["exp1type"].Value.ToString();
            ansGridView1.Rows[rowIndex].Cells["exp2type"].Value = dt.Rows[rowIndex].Cells["exp2type"].Value.ToString();
            ansGridView1.Rows[rowIndex].Cells["exp3type"].Value = dt.Rows[rowIndex].Cells["exp3type"].Value.ToString();
            ansGridView1.Rows[rowIndex].Cells["exp4type"].Value = dt.Rows[rowIndex].Cells["exp4type"].Value.ToString();
            ansGridView1.Rows[rowIndex].Cells["exp5type"].Value = dt.Rows[rowIndex].Cells["exp5type"].Value.ToString();
            ansGridView1.Rows[rowIndex].Cells["exp6type"].Value = dt.Rows[rowIndex].Cells["exp6type"].Value.ToString();
            ansGridView1.Rows[rowIndex].Cells["exp7type"].Value = dt.Rows[rowIndex].Cells["exp7type"].Value.ToString();
            ansGridView1.Rows[rowIndex].Cells["exp8type"].Value = dt.Rows[rowIndex].Cells["exp8type"].Value.ToString();
            ansGridView1.Rows[rowIndex].Cells["exp9type"].Value = dt.Rows[rowIndex].Cells["exp9type"].Value.ToString();
            ansGridView1.Rows[rowIndex].Cells["exp10type"].Value = dt.Rows[rowIndex].Cells["exp10type"].Value.ToString();
            ansGridView1.Rows[rowIndex].Cells["exp11type"].Value = dt.Rows[rowIndex].Cells["exp11type"].Value.ToString();
            ansGridView1.Rows[rowIndex].Cells["per"].Value = dt.Rows[rowIndex].Cells["per"].Value.ToString();

            CalcAmount(rowIndex);
        }

        private void frmBooking_Load(object sender, EventArgs e)
        {
            SideFill();

            dateTimePicker1.CustomFormat = Database.dformat;
            dateTimePicker1.MaxDate = Database.ldate;
            dateTimePicker1.MinDate = Database.stDate;
            dateTimePicker2.CustomFormat = Database.dformat;
            dateTimePicker2.MaxDate = Database.ldate;
            dateTimePicker2.MinDate = Database.stDate;
            //dateTimePicker1.Value = Database.ldate;

            DataTable dtFeture = new DataTable();


            label4.Text = Feature.Available("Name of Expense1");
            label6.Text = Feature.Available("Name of Expense2");
            label7.Text = Feature.Available("Name of Expense3");
            label12.Text = Feature.Available("Name of Expense4");
            label13.Text = Feature.Available("Name of Expense5");
            label14.Text = Feature.Available("Name of Expense6");
            label16.Text = Feature.Available("Name of Expense7");
            label17.Text = Feature.Available("Name of Expense8");
            label18.Text = Feature.Available("Name of Expense9");
            label19.Text = Feature.Available("Name of Expense10");
            label29.Text = Feature.Available("Name of Expense11");

            frm_main obj = (frm_main)this.MdiParent;
            StatusStrip ms = (StatusStrip)obj.Controls["statusStrip1"];
            this.tspb = (ToolStripProgressBar)ms.Items["tspb"];

            this.tspb.Minimum = 0;
            this.tspb.Maximum = 20;
            this.tspb.Value = 0;
            this.tspb.Visible = true;

            if (Feature.Available("Required Charged Weight").ToUpper() == "NO")
            {
                ansGridView1.Columns["ChargedWeight"].Visible = false;
                //ChargedWeight
            }
            else
            {
                ansGridView1.Columns["ChargedWeight"].Visible = true;
            }
        }

        public void LoadData(string vi_id, String frmCaption)
        {
            if (this.tspb != null)
            {
                this.tspb.Value = 0;
            }

            if (Feature.Available("Display Forwarding GRDetails") == "Yes")
            {
                groupBox7.Visible = true;
            }
            else
            {
                groupBox7.Visible = false;
            }
            textBox1.Focus();
            gStr = vi_id.ToString();
            vid = vi_id;
            DisplaySetting();
            DisplayData();
            //dtCompany = new DataTable("company");
            //Database.GetSqlData("select * from company", dtCompany);
            //textBox3.Text = funs.Select_dp_nm(dtCompany.Rows[0]["SId"].ToString());


            //objStop.Start();
            //dtVoucherInfo = new DataTable("Voucherinfos");
            //Database.GetSqlData("select * from Voucherinfos where  Vi_id='" + vi_id + "'", dtVoucherInfo);

            //dtVoucherdet = new DataTable("Voucherdets");
            //Database.GetSqlData("Select vd.*,itm.name as itmName from Voucherdets as vd left join items as itm on vd.Des_ac_id=itm.id where vd.Vi_id='" + vi_id + "'order by itemsr ", dtVoucherdet);
            //objStop.Stop();
            //MessageBox.Show(objStop.Elapsed.ToString());
            dtJournal = new DataTable("Journals");
            string strloadjou = "Select jn.* from Journals as jn where jn.Vi_id='" + vi_id + "'";
            Database.GetSqlData(strloadjou,dtJournal);
            if (dtVoucherInfo == null)
            {
                DataSet dsv = new DataSet();
                string strloadsql = "select * from Voucherinfos where  Vi_id='" + vi_id + "';";
                strloadsql += "Select vd.*,itm.name as itmName from Voucherdets as vd left join items as itm on vd.Des_ac_id=itm.id where vd.Vi_id='" + vi_id + "'order by itemsr ";
             
                Database.GetSqlData(strloadsql, dsv);

                dtVoucherInfo = dsv.Tables[0];
                dtVoucherdet = dsv.Tables[1];
              
            }
            else if (vi_id == "0")
            {

                dtVoucherInfo.Rows.Clear();
                dtVoucherInfo.AcceptChanges();
                dtVoucherdet.Rows.Clear();
                dtVoucherdet.AcceptChanges();

                dtJournal.Rows.Clear();
                dtJournal.AcceptChanges();
            }


            if (dtVoucherInfo.Rows.Count == 0)
            {
                textBox1.Text = "";
                textBox2.Text = "";
                textBox4.Text = "";
                textBox23.Text = "Consigner";
                textBox16.Text = "";
                textBox20.Text = "";
                textBox10.Text = "";
                textBox27.Text = "";
                
                textBox28.Text = "";
                ansGridView1.Rows.Clear();
                checkBox1.Checked = false;
                iscancel = false;
                label28.Visible = false;
                //vtid = 0;
            }
            else
            {
                groupBox6.Enabled = false;
                DataTable TransportDetails = clsCashing.GetTransPortDetail();// new DataTable("TransportDetails");
                //Database.GetSqlData("select * from TransportDetails", TransportDetails);

                DataTable dtViOther = new DataTable();
                string strOthSql = "select ac.name as Consignee,ac.Tin_number as Tin1,ac2.name as Consigner,ac2.Tin_number as Tin2, ";
                strOthSql += "recp.Name as Origin,delp.Name as Destination ,isnull(actr.name,'') as Transporter,vt.name vtype";
                strOthSql += " from VOUCHERINFOs  as vi ";
                strOthSql += " left join ACCOUNTs as ac on vi.Ac_id =ac.ac_id";
                strOthSql += " left join ACCOUNTs as ac2 on vi.Ac_id2 =ac2.ac_id";
                strOthSql += " left join DeliveryPoints as recp on vi.Consigner_Id= recp.DPId ";
                strOthSql += " left join DeliveryPoints as delp on vi.SId = delp.DPId ";
                strOthSql += " left join ACCOUNTs as actr on vi.transporter_id =actr.ac_id ";
                strOthSql += " left join VOUCHERTYPEs as vt on vi.vt_id=vt.vt_id";
                strOthSql += " where vi.Vi_id='" + vi_id + "'";

                Database.GetSqlData(strOthSql, dtViOther);


                vtid = int.Parse(dtVoucherInfo.Rows[0]["Vt_id"].ToString());
                dateTimePicker1.Value = DateTime.Parse(dtVoucherInfo.Rows[0]["Vdate"].ToString());
                vno = int.Parse(dtVoucherInfo.Rows[0]["Vnumber"].ToString());
                label10.Text = vno.ToString();

                textBox1.Text = dtViOther.Rows[0]["Consignee"].ToString();
                textBox2.Text = dtViOther.Rows[0]["Consigner"].ToString();
                textBox3.Text = dtViOther.Rows[0]["Origin"].ToString();
                textBox4.Text = dtViOther.Rows[0]["Destination"].ToString();

                label23.Text = dtViOther.Rows[0]["tin1"].ToString();
                label24.Text = dtViOther.Rows[0]["tin2"].ToString();

                textBox28.Text = dtViOther.Rows[0]["Transporter"].ToString();
                textBox11.Text = dtViOther.Rows[0]["vtype"].ToString(); //funs.Select_vt_nm(int.Parse(dtVoucherInfo.Rows[0]["Vt_id"].ToString());

                textBox9.Text = funs.DecimalPoint(double.Parse(dtVoucherInfo.Rows[0]["Roff"].ToString()), 2);
                RoffChanged = bool.Parse(dtVoucherInfo.Rows[0]["RoffChanged"].ToString());
                Prelocationid = dtVoucherInfo.Rows[0]["Locationid"].ToString();
                textBox5.Text = dtVoucherInfo.Rows[0]["Delivery_adrs"].ToString();
                grno = dtVoucherInfo.Rows[0]["Invoiceno"].ToString();
                textBox23.Text = dtVoucherInfo.Rows[0]["As_Per"].ToString();
                textBox24.Text = dtVoucherInfo.Rows[0]["PaymentMode"].ToString();

                if (dtVoucherInfo.Rows[0]["Paidopt"] == null || dtVoucherInfo.Rows[0]["Paidopt"].ToString() == "")
                {
                    if (textBox24.Text == "Paid")
                    {
                        textBox29.Text = "Cash";
                       

                    }
                    else
                    {

                        textBox29.Text = "Credit";
                        
                    }
                }
                else
                {
                    textBox29.Text = dtVoucherInfo.Rows[0]["Paidopt"].ToString();



                }


                if (textBox24.Text == "FOC")
                {
                   
                    textBox29.Enabled = false;
                    //SendKeys.Send("{tab}");
                }
                else if (textBox24.Text == "T.B.B.")
                {
                    
                    textBox29.Enabled = false;
                    // SendKeys.Send("{tab}");
                }
                else if (textBox24.Text == "To Pay")
                {
                    
                    textBox29.Enabled = false;
                    //SendKeys.Send("{tab}");
                }
                else
                {
                    textBox29.Enabled = true;
                }
                textBox25.Text = dtVoucherInfo.Rows[0]["DeliveryType"].ToString();


                vtid = int.Parse(dtVoucherInfo.Rows[0]["Vt_id"].ToString());
                dateTimePicker2.Value = DateTime.Parse(dtVoucherInfo.Rows[0]["ForGRdate"].ToString());
                textBox27.Text = dtVoucherInfo.Rows[0]["ForGRno"].ToString();

                field1 = dtVoucherInfo.Rows[0]["Transport1"].ToString();
                field2 = dtVoucherInfo.Rows[0]["Transport2"].ToString();
                field3 = dtVoucherInfo.Rows[0]["Transport3"].ToString();
                field4 = dtVoucherInfo.Rows[0]["Transport4"].ToString();
                field5 = dtVoucherInfo.Rows[0]["Transport5"].ToString();
                field6 = dtVoucherInfo.Rows[0]["Transport6"].ToString();
                field7 = dtVoucherInfo.Rows[0]["DeliveryAt"].ToString();

                field8 = dtVoucherInfo.Rows[0]["Grno"].ToString();
                if (dtVoucherInfo.Rows[0]["Iscancel"].ToString() == "")
                {
                    dtVoucherInfo.Rows[0]["Iscancel"] = false;
                }
                if (bool.Parse(dtVoucherInfo.Rows[0]["Iscancel"].ToString()) == true)
                {
                    label28.Visible = true;
                    label28.Text = "Cancelled";
                    iscancel = bool.Parse(dtVoucherInfo.Rows[0]["Iscancel"].ToString());
                }

                create_date = DateTime.Parse(dtVoucherInfo.Rows[0]["create_date"].ToString());

                TextBox tbx1 = this.Controls.Find(TransportDetails.Rows[0]["FName"].ToString(), true).FirstOrDefault() as TextBox;
                tbx1.Text = field1;

                TextBox tbx2 = this.Controls.Find(TransportDetails.Rows[1]["FName"].ToString(), true).FirstOrDefault() as TextBox;
                tbx2.Text = field2;

                TextBox tbx3 = this.Controls.Find(TransportDetails.Rows[2]["FName"].ToString(), true).FirstOrDefault() as TextBox;
                tbx3.Text = field3;

                TextBox tbx4 = this.Controls.Find(TransportDetails.Rows[3]["FName"].ToString(), true).FirstOrDefault() as TextBox;
                tbx4.Text = field4;

                TextBox tbx5 = this.Controls.Find(TransportDetails.Rows[4]["FName"].ToString(), true).FirstOrDefault() as TextBox;
                tbx5.Text = field5;

                TextBox tbx6 = this.Controls.Find(TransportDetails.Rows[5]["FName"].ToString(), true).FirstOrDefault() as TextBox;
                tbx6.Text = field6;

                TextBox tbx7 = this.Controls.Find(TransportDetails.Rows[6]["FName"].ToString(), true).FirstOrDefault() as TextBox;
                tbx7.Text = field7;

                TextBox tbx8 = this.Controls.Find(TransportDetails.Rows[7]["FName"].ToString(), true).FirstOrDefault() as TextBox;
                tbx8.Text = field8;
                if (dtVoucherInfo.Rows[0]["IsSelf"].ToString() == "")
                {
                    dtVoucherInfo.Rows[0]["IsSelf"] = 0;
                }
                if (bool.Parse(dtVoucherInfo.Rows[0]["IsSelf"].ToString()) == true)
                {
                    checkBox1.Checked = true;
                }
                else
                {
                    checkBox1.Checked = false;
                }

                ansGridView1.Rows.Clear();
                for (int i = 0; i < dtVoucherdet.Rows.Count; i++)
                {
                    ansGridView1.Rows.Add();
                    ansGridView1.Rows[i].Cells["sno"].Value = dtVoucherdet.Rows[i]["ItemSr"];
                    ansGridView1.Rows[i].Cells["description"].Value = dtVoucherdet.Rows[i]["itmName"];// funs.Select_item_nm(dtVoucherdet.Rows[i]["Des_ac_id"].ToString());
                    ansGridView1.Rows[i].Cells["itemid"].Value = dtVoucherdet.Rows[i]["des_ac_id"];
                    ansGridView1.Rows[i].Cells["unt"].Value = dtVoucherdet.Rows[i]["packing"];
                    ansGridView1.Rows[i].Cells["Quantity"].Value = funs.DecimalPoint(dtVoucherdet.Rows[i]["Quantity"], 2);
                    ansGridView1.Rows[i].Cells["weight"].Value = funs.DecimalPoint(dtVoucherdet.Rows[i]["weight"], 3);
                    ansGridView1.Rows[i].Cells["Rate_am"].Value = funs.DecimalPoint(dtVoucherdet.Rows[i]["Rate_am"], 2);
                    ansGridView1.Rows[i].Cells["Amount"].Value = funs.DecimalPoint(dtVoucherdet.Rows[i]["Amount"], 2);
                    ansGridView1.Rows[i].Cells["ChargedWeight"].Value = funs.DecimalPoint(dtVoucherdet.Rows[i]["ChargedWeight"], 3);
                    ansGridView1.Rows[i].Cells["Per"].Value = dtVoucherdet.Rows[i]["Per"].ToString();
                    ansGridView1.Rows[i].Cells["multiplier"].Value = dtVoucherdet.Rows[i]["multiplier"].ToString();

                    ansGridView1.Rows[i].Cells["freightmr"].Value = funs.DecimalPoint(dtVoucherdet.Rows[i]["freightmr"], 2);
                    ansGridView1.Rows[i].Cells["bharti"].Value = funs.DecimalPoint(dtVoucherdet.Rows[i]["bharti"], 2);
                    ansGridView1.Rows[i].Cells["exp1rate"].Value = funs.DecimalPoint(dtVoucherdet.Rows[i]["exp1rate"], 2);
                    ansGridView1.Rows[i].Cells["exp2rate"].Value = funs.DecimalPoint(dtVoucherdet.Rows[i]["exp2rate"], 2);
                    ansGridView1.Rows[i].Cells["exp3rate"].Value = funs.DecimalPoint(dtVoucherdet.Rows[i]["exp3rate"], 2);
                    ansGridView1.Rows[i].Cells["exp4rate"].Value = funs.DecimalPoint(dtVoucherdet.Rows[i]["exp4rate"], 2);
                    ansGridView1.Rows[i].Cells["exp5rate"].Value = funs.DecimalPoint(dtVoucherdet.Rows[i]["exp5rate"], 2);
                    ansGridView1.Rows[i].Cells["exp6rate"].Value = funs.DecimalPoint(dtVoucherdet.Rows[i]["exp6rate"], 2);
                    ansGridView1.Rows[i].Cells["exp7rate"].Value = funs.DecimalPoint(dtVoucherdet.Rows[i]["exp7rate"], 2);
                    ansGridView1.Rows[i].Cells["exp8rate"].Value = funs.DecimalPoint(dtVoucherdet.Rows[i]["exp8rate"], 2);
                    ansGridView1.Rows[i].Cells["exp9rate"].Value = funs.DecimalPoint(dtVoucherdet.Rows[i]["exp9rate"], 2);
                    ansGridView1.Rows[i].Cells["exp10rate"].Value = funs.DecimalPoint(dtVoucherdet.Rows[i]["exp10rate"], 2);
                    ansGridView1.Rows[i].Cells["exp11rate"].Value = funs.DecimalPoint(dtVoucherdet.Rows[i]["exp11rate"], 2);

                    ansGridView1.Rows[i].Cells["exp1amt"].Value = funs.DecimalPoint(dtVoucherdet.Rows[i]["exp1amt"], 2);
                    ansGridView1.Rows[i].Cells["exp2amt"].Value = funs.DecimalPoint(dtVoucherdet.Rows[i]["exp2amt"], 2);
                    ansGridView1.Rows[i].Cells["exp3amt"].Value = funs.DecimalPoint(dtVoucherdet.Rows[i]["exp3amt"], 2);
                    ansGridView1.Rows[i].Cells["exp4amt"].Value = funs.DecimalPoint(dtVoucherdet.Rows[i]["exp4amt"], 2);
                    ansGridView1.Rows[i].Cells["exp5amt"].Value = funs.DecimalPoint(dtVoucherdet.Rows[i]["exp5amt"], 2);
                    ansGridView1.Rows[i].Cells["exp6amt"].Value = funs.DecimalPoint(dtVoucherdet.Rows[i]["exp6amt"], 2);
                    ansGridView1.Rows[i].Cells["exp7amt"].Value = funs.DecimalPoint(dtVoucherdet.Rows[i]["exp7amt"], 2);
                    ansGridView1.Rows[i].Cells["exp8amt"].Value = funs.DecimalPoint(dtVoucherdet.Rows[i]["exp8amt"], 2);
                    ansGridView1.Rows[i].Cells["exp9amt"].Value = funs.DecimalPoint(dtVoucherdet.Rows[i]["exp9amt"], 2);
                    ansGridView1.Rows[i].Cells["exp10amt"].Value = funs.DecimalPoint(dtVoucherdet.Rows[i]["exp10amt"], 2);
                    ansGridView1.Rows[i].Cells["exp11amt"].Value = funs.DecimalPoint(dtVoucherdet.Rows[i]["exp11amt"], 2);

                    ansGridView1.Rows[i].Cells["exp1mr"].Value = funs.DecimalPoint(dtVoucherdet.Rows[i]["exp1mr"], 2);
                    ansGridView1.Rows[i].Cells["exp2mr"].Value = funs.DecimalPoint(dtVoucherdet.Rows[i]["exp2mr"], 2);
                    ansGridView1.Rows[i].Cells["exp3mr"].Value = funs.DecimalPoint(dtVoucherdet.Rows[i]["exp3mr"], 2);
                    ansGridView1.Rows[i].Cells["exp4mr"].Value = funs.DecimalPoint(dtVoucherdet.Rows[i]["exp4mr"], 2);
                    ansGridView1.Rows[i].Cells["exp5mr"].Value = funs.DecimalPoint(dtVoucherdet.Rows[i]["exp5mr"], 2);
                    ansGridView1.Rows[i].Cells["exp6mr"].Value = funs.DecimalPoint(dtVoucherdet.Rows[i]["exp6mr"], 2);
                    ansGridView1.Rows[i].Cells["exp7mr"].Value = funs.DecimalPoint(dtVoucherdet.Rows[i]["exp7mr"], 2);
                    ansGridView1.Rows[i].Cells["exp8mr"].Value = funs.DecimalPoint(dtVoucherdet.Rows[i]["exp8mr"], 2);
                    ansGridView1.Rows[i].Cells["exp9mr"].Value = funs.DecimalPoint(dtVoucherdet.Rows[i]["exp9mr"], 2);
                    ansGridView1.Rows[i].Cells["exp10mr"].Value = funs.DecimalPoint(dtVoucherdet.Rows[i]["exp10mr"], 2);
                    ansGridView1.Rows[i].Cells["exp11mr"].Value = funs.DecimalPoint(dtVoucherdet.Rows[i]["exp11mr"], 2);

                    ansGridView1.Rows[i].Cells["exp1type"].Value = dtVoucherdet.Rows[i]["exp1type"];
                    ansGridView1.Rows[i].Cells["exp2type"].Value = dtVoucherdet.Rows[i]["exp2type"];
                    ansGridView1.Rows[i].Cells["exp3type"].Value = dtVoucherdet.Rows[i]["exp3type"];
                    ansGridView1.Rows[i].Cells["exp4type"].Value = dtVoucherdet.Rows[i]["exp4type"];
                    ansGridView1.Rows[i].Cells["exp5type"].Value = dtVoucherdet.Rows[i]["exp5type"];
                    ansGridView1.Rows[i].Cells["exp6type"].Value = dtVoucherdet.Rows[i]["exp6type"];
                    ansGridView1.Rows[i].Cells["exp7type"].Value = dtVoucherdet.Rows[i]["exp7type"];
                    ansGridView1.Rows[i].Cells["exp8type"].Value = dtVoucherdet.Rows[i]["exp8type"];
                    ansGridView1.Rows[i].Cells["exp9type"].Value = dtVoucherdet.Rows[i]["exp9type"];
                    ansGridView1.Rows[i].Cells["exp10type"].Value = dtVoucherdet.Rows[i]["exp10type"];
                    ansGridView1.Rows[i].Cells["exp11type"].Value = dtVoucherdet.Rows[i]["exp11type"];
                    ansGridView1.Rows[i].Cells["totexp"].Value = funs.DecimalPoint(dtVoucherdet.Rows[i]["totexp"], 2);
                    ansGridView1.Rows[i].Cells["ItemAmount"].Value = funs.DecimalPoint(dtVoucherdet.Rows[i]["ItemAmount"], 2);
                }



                ansGridView1.CurrentCell = ansGridView1[2, 0];
                //DataTable dtVoucherCharges1 = new DataTable("VouChargess");
                //Database.GetSqlData("select * from VouChargess where vi_id='" + vi_id + "'  order by Srno", dtVoucherCharges1);
                //DataTable Stock = new DataTable("Stocks");
                //Database.GetSqlData("Select * from Stocks where Vid='" + vi_id + "' ", Stock);
                if (gresave == false)
                {
                    labelCalc();
                }
                labelCalc();
            }
            if (gresave == true)
            {
                object sender = new object();
                EventArgs e = new EventArgs();
                btn_Click(sender, e);
            }
            SetVno();

            //objStop.Stop();
            //MessageBox.Show(objStop.Elapsed.ToString());
        }

        //private DataTable GetValidation()
        //{

        //    DataTable dtMaster = new DataTable();

        //    string stCmd = "select ";
        //    stCmd += " isnull((select Numtype from VOUCHERTYPEs where vt_id=" + vtid + "),'') as numtype,";
        //    stCmd += " isnull((Select top 1 Vi_id from voucherinfos where Vt_id='" + vtid + "' and Vnumber=" + vno + " and Vdate=" + access_sql.Hash + dateTimePicker1.Value.Date.ToString(Database.dformat) + access_sql.Hash + "),'') as numtype1_viid,";

        //    stCmd += " isnull((select top 1 ac_id from ACCOUNTs where name='" + textBox2.Text + "' ),'') as ac2id,";
        //    stCmd += " isnull((select top 1 ac_id from ACCOUNTs where name='" + textBox28.Text + "' ),'') as transporterid,";
        //    stCmd += " isnull((select top 1 DPId  from DeliveryPoints where name='" + textBox3.Text + "' ),'') as orignid,";
        //    stCmd += " isnull((select top 1 DPId from DeliveryPoints where name='" + textBox4.Text + "' ),'') as delpid,";
        //    stCmd += " isnull((Select Aliasname from vouchertypes where vt_id=" + vtid + "),'') as Aliasname,";
        //    stCmd += " isnull((Select prefix from Location where LocationId='" + Database.LocationId + "'),'') as prefix,";
        //    stCmd += " (select isnull(max(Nid),0)+1  from VOUCHERINFOs where locationid='" + Database.LocationId + "') as vi_nid,";
        //    stCmd += " (select isnull(max(Nid),0)+1  from Voucherdets where locationid='" + Database.LocationId + "') as vid_nid,";
        //    stCmd += " (select isnull( max(Nid),0)+1 from VOUCHARGESs where locationid='" + Database.LocationId + "') as vc_nid";

        //    Database.GetSqlData(stCmd, dtMaster);

        //    return dtMaster;

        //}

        private bool validate()
        {
            this.tspb.Value = 1;
            if (textBox11.Text == "")
            {
                MessageBox.Show("Enter Voucher Type");
                textBox11.Focus();
                return false;
            }
            if (textBox1.Text == "")
            {
                MessageBox.Show("Enter Consigner");
                textBox1.Focus();
                return false;
            }
            if (textBox2.Text == "")
            {
                MessageBox.Show("Enter Consignee");
                textBox2.Focus();
                return false;
            }
            if (textBox3.Text == "")
            {
                MessageBox.Show("Enter Source");
                textBox3.Focus();
                return false;
            }
            if (textBox4.Text == "")
            {
                MessageBox.Show("Enter Destination");
                textBox4.Focus();
                return false;
            }
            if (textBox29.Text == "")
            {
                MessageBox.Show("Plaes select Paid Option");
                textBox29.Focus();
                return false;
            }
            if (textBox25.Text == "")
            {
                MessageBox.Show("Enter Delivery Type");
                textBox25.Focus();
                return false;
            }
            if (textBox24.Text == "")
            {
                MessageBox.Show("Enter GR Type");
                textBox24.Focus();
                return false;
            }
            if (Database.LocationExpAcc_id == "")
            {
                MessageBox.Show("Enter First Expense Account with This Location");
                textBox24.Focus();
                return false;
            }

            if (ansGridView1.Rows.Count == 0)
            {
                MessageBox.Show("Please enter at least one item");
                ansGridView1.Focus();
            }

            //if (funs.GenerateVno(vtid, dateTimePicker1.Value.ToString("dd-MMM-yyyy"), vid) == 0 && vno == 0)
            //{
            //    MessageBox.Show("Voucher Number can't be created on this date.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return false;
            //}
            
            SetVno();
            this.tspb.Value += 1;

            if (vno == 0)
            {
                vno = funs.GenerateVno(vtid, dateTimePicker1.Value.ToString("dd-MMM-yyyy"), vid);
            }
            if (vno == 0) {
                   MessageBox.Show("Voucher Number can't be created on this date.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                   return false;
            }
            this.tspb.Value += 1;

            if (vid == "")
            {
                int numtype = funs.chkNumType(vtid);
                if (numtype != 1)
                {
                    vid = Database.GetScalarText("Select Vi_id from voucherinfos where Vt_id='" + vtid + "' and Vnumber=" + vno + " and Vdate=" + access_sql.Hash + dateTimePicker1.Value.Date.ToString(Database.dformat) + access_sql.Hash);
                    if (vid == "")
                    {
                        vid = "0";
                    }
                }
                else
                {
                    if (vid == "0")
                    {
                        string tempvid = "";
                        tempvid = Database.GetScalarText("Select Vi_id from voucherinfos where Vt_id='" + vtid + "' and Vnumber=" + vno);
                        if (tempvid != "")
                        {
                            MessageBox.Show("Voucher can't be created on this No.");
                            return false;
                        }
                        else
                        {
                            vid = tempvid;
                        }
                    }
                }
            }
            this.tspb.Value += 1;
            if (gStr != "0")
            {
                if (iscancel == true)
                {
                    int grcount = Database.GetScalarInt("Select count(*) from Stocks where Gr_id='" + vid + "'");

                    if (grcount > 1 && iscancel == true)
                    {
                        MessageBox.Show("This GR has been Dispatched. Cancellation is not possible.");
                        iscancel = false;
                        return false;
                    }
                }
                instk = true;
            }

            //if (gStr != "0")
            //{
            //    DataTable dtstocksdel = new DataTable("stocks");
            //    Database.GetSqlData("SELECT     dbo.VOUCHERINFOs.Vt_id, dbo.VOUCHERINFOs.Vi_id FROM  dbo.Stocks LEFT OUTER JOIN   dbo.VOUCHERINFOs ON dbo.Stocks.vid = dbo.VOUCHERINFOs.Vi_id WHERE     (dbo.Stocks.GR_id = '" + vid + "') AND (dbo.VOUCHERINFOs.Vt_id = 90)", dtstocksdel);
            //    if (dtstocksdel.Rows.Count >= 1)
            //    {
            //        MessageBox.Show("This GR Can't Be Modified. Because It has been already delivered.");

            //        return false;
            //    }


            //}

            for(int i=0;i<ansGridView1.Rows.Count-1;i++)
            {
                if (ansGridView1.Rows[i].Cells["itemid"].Value == null)
                {
                    MessageBox.Show("Please select Valid Item");
                    return false;
                }
            }

           
            return true;
        }

        private DataTable GetMasterID()
        {

            DataTable dtMaster = new DataTable();

            string stCmd = "select ";
            stCmd += " isnull((select top 1 ac_id from ACCOUNTs where name='" + textBox1.Text + "' ),'') as ac1id,";
            stCmd += " isnull((select top 1 ac_id from ACCOUNTs where name='" + textBox2.Text + "' ),'') as ac2id,";
            stCmd += " isnull((select top 1 ac_id from ACCOUNTs where name='" + textBox28.Text + "' ),'') as transporterid,";
            stCmd += " isnull((select top 1 DPId  from DeliveryPoints where name='" + textBox3.Text + "' ),'') as orignid,";
            stCmd += " isnull((select top 1 DPId from DeliveryPoints where name='" + textBox4.Text + "' ),'') as delpid,";
            stCmd += " isnull((Select Aliasname from vouchertypes where vt_id=" + vtid + "),'') as Aliasname,";
            stCmd += " isnull((Select prefix from Location where LocationId='" + Database.LocationId + "'),'') as prefix,";
            stCmd += " (select isnull(max(Nid),0)+1  from VOUCHERINFOs where locationid='" + Database.LocationId + "') as vi_nid,";
            stCmd += " (select isnull(max(Nid),0)+1  from Voucherdets where locationid='" + Database.LocationId + "') as vid_nid,";
            stCmd += " (select isnull( max(Nid),0)+1 from VOUCHARGESs where locationid='" + Database.LocationId + "') as vc_nid";

            Database.GetSqlData(stCmd, dtMaster);

            return dtMaster;

        }


        private void save()
        {
            this.tspb.Value += 1;

            DataTable dtMaster = GetMasterID();

            if (vid != "0")
            {
                string currLoc = Database.LocationId;
                string EditLoc = dtVoucherInfo.Rows[0]["locationid"].ToString();// Database.GetScalarText("select locationId from voucherinfos where vi_id='" + vid + "'");
                if (currLoc != EditLoc)
                {
                    MessageBox.Show("Your Current Location is " + funs.Select_location_name(currLoc) + " and You are Trying to Edit " + funs.Select_location_name(EditLoc) + "'s Booking. Sorry You Don't Have Permission to do This", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }


            if (dtVoucherInfo.Rows.Count == 0)
            {
                dtVoucherInfo.Rows.Add();
            }

            string prefix = "";
            string postfix = "";
            int padding = 0;
            
            prefix = dtMaster.Rows[0]["prefix"].ToString();// Database.GetScalarText("Select prefix from Location where LocationId='" + Database.LocationId + "'");
            //SetVno();
            //if (vno == 0)
            //{
            //    vno = funs.GenerateVno(vtid, dateTimePicker1.Value.ToString("dd-MMM-yyyy"), vid);
            //}
            
            if (vid == "0")
            {

                long Nid = long.Parse(dtMaster.Rows[0]["vi_nid"].ToString());// Database.GetScalarLong("select isnull(max(Nid),0)+1 as Nid from VOUCHERINFOs where locationid='" + Database.LocationId + "'");
                dtVoucherInfo.Rows[0]["Vi_id"] = Database.LocationId + (Nid);
                dtVoucherInfo.Rows[0]["Nid"] = (Nid);
                dtVoucherInfo.Rows[0]["LocationId"] = Database.LocationId;
                Prelocationid = Database.LocationId;

            }

            this.tspb.Value += 1;

            string invoiceno = vno.ToString();
            dtVoucherInfo.Rows[0]["Invoiceno"] = prefix + invoiceno.PadLeft(padding, '0') + postfix;
            dtVoucherInfo.Rows[0]["Vnumber"] = vno;
            grno = dtVoucherInfo.Rows[0]["Invoiceno"].ToString();
            dtVoucherInfo.Rows[0]["Vt_id"] = vtid;
            dtVoucherInfo.Rows[0]["Ac_id"] = dtMaster.Rows[0]["ac1id"];// funs.Select_ac_id(textBox1.Text);  //consigner 
            dtVoucherInfo.Rows[0]["Vdate"] = dateTimePicker1.Value.Date;
            dtVoucherInfo.Rows[0]["ForGRdate"] = dateTimePicker2.Value.Date;
            dtVoucherInfo.Rows[0]["ForGRno"] = textBox27.Text;
            dtVoucherInfo.Rows[0]["transporter_id"] = dtMaster.Rows[0]["transporterid"];// funs.Select_ac_id(textBox28.Text);
            dtVoucherInfo.Rows[0]["Tdtype"] = false;
            dtVoucherInfo.Rows[0]["Narr"] = "Booking";
            dtVoucherInfo.Rows[0]["Ac_id2"] = dtMaster.Rows[0]["ac2id"];// funs.Select_ac_id(textBox2.Text);//consignee
            dtVoucherInfo.Rows[0]["RoffChanged"] = RoffChanged;
            dtVoucherInfo.Rows[0]["Roff"] = textBox9.Text;
            dtVoucherInfo.Rows[0]["paidopt"] = textBox29.Text;
            dtVoucherInfo.Rows[0]["Totalamount"] = textBox10.Text;
            dtVoucherInfo.Rows[0]["ActWeight"] = textBox20.Text;
            dtVoucherInfo.Rows[0]["SId"] = dtMaster.Rows[0]["delpid"];// funs.Select_dp_id(textBox4.Text);
            dtVoucherInfo.Rows[0]["Consigner_id"] = dtMaster.Rows[0]["orignid"];// funs.Select_dp_id(textBox3.Text);
            dtVoucherInfo.Rows[0]["As_Per"] = textBox23.Text;
            dtVoucherInfo.Rows[0]["PaymentMode"] = textBox24.Text;
            dtVoucherInfo.Rows[0]["DeliveryType"] = textBox25.Text;
            dtVoucherInfo.Rows[0]["Delivery_adrs"] = textBox5.Text;
            dtVoucherInfo.Rows[0]["Iscancel"] = iscancel;
            dtVoucherInfo.Rows[0]["DR"] = 0;
            dtVoucherInfo.Rows[0]["DD"] = 0;
            dtVoucherInfo.Rows[0]["TaxChanged"] = false;
            dtVoucherInfo.Rows[0]["formC"] = false;


            if (checkBox1.Checked == true)
            {
                dtVoucherInfo.Rows[0]["IsSelf"] = true;
            }
            else
            {
                dtVoucherInfo.Rows[0]["IsSelf"] = false;
            }

            if (vid == "0")
            {
                dtVoucherInfo.Rows[0]["CreTime"] = System.DateTime.Now.ToString("HH:mm:ss");
                dtVoucherInfo.Rows[0]["create_date"] = System.DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                dtVoucherInfo.Rows[0]["user_id"] = Database.user_id;
            }
            if (vid != "0")
            {
                dtVoucherInfo.Rows[0]["modifyby_id"] = Database.user_id;
            }
            dtVoucherInfo.Rows[0]["modify_date"] = System.DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
            dtVoucherInfo.Rows[0]["ModTime"] = System.DateTime.Now.ToString("HH:mm:ss");


            DataTable TransportDetails = clsCashing.GetTransPortDetail();
            
            TextBox tbx1 = this.Controls.Find(TransportDetails.Rows[0]["FName"].ToString(), true).FirstOrDefault() as TextBox;
            field1 = tbx1.Text;
            dtVoucherInfo.Rows[0]["Transport1"] = field1;

            TextBox tbx2 = this.Controls.Find(TransportDetails.Rows[1]["FName"].ToString(), true).FirstOrDefault() as TextBox;
            field2 = tbx2.Text;
            dtVoucherInfo.Rows[0]["Transport2"] = field2;

            TextBox tbx3 = this.Controls.Find(TransportDetails.Rows[2]["FName"].ToString(), true).FirstOrDefault() as TextBox;
            field3 = tbx3.Text;
            dtVoucherInfo.Rows[0]["Transport3"] = field3;

            TextBox tbx4 = this.Controls.Find(TransportDetails.Rows[3]["FName"].ToString(), true).FirstOrDefault() as TextBox;
            field4 = tbx4.Text;
            dtVoucherInfo.Rows[0]["Transport4"] = field4;

            TextBox tbx5 = this.Controls.Find(TransportDetails.Rows[4]["FName"].ToString(), true).FirstOrDefault() as TextBox;
            field5 = tbx5.Text;
            dtVoucherInfo.Rows[0]["Transport5"] = field5;

            TextBox tbx6 = this.Controls.Find(TransportDetails.Rows[5]["FName"].ToString(), true).FirstOrDefault() as TextBox;
            field6 = tbx6.Text;
            dtVoucherInfo.Rows[0]["Transport6"] = field6;

            TextBox tbx7 = this.Controls.Find(TransportDetails.Rows[6]["FName"].ToString(), true).FirstOrDefault() as TextBox;
            field7 = tbx7.Text;
            dtVoucherInfo.Rows[0]["DeliveryAt"] = field7;

            TextBox tbx8 = this.Controls.Find(TransportDetails.Rows[7]["FName"].ToString(), true).FirstOrDefault() as TextBox;
            field8 = tbx8.Text;
            dtVoucherInfo.Rows[0]["Grno"] = field8;

            this.tspb.Value += 1;
            dtVoucherInfo.TableName = "voucherinfos";
            Database.SaveData(dtVoucherInfo);
            this.tspb.Value += 1;

            if (vid == "0")
            {
                vid = dtVoucherInfo.Rows[0]["Vi_id"].ToString();
            }

            dtVoucherdet.Rows.Clear();
            dtVoucherdet.AcceptChanges();
            long Nid2 = long.Parse(dtMaster.Rows[0]["vid_nid"].ToString()); // Database.GetScalarLong("select isnull(max(Nid),0)+1 as Nid from Voucherdets where locationid='" + Database.LocationId + "'");
            
            for (int i = 0; i < ansGridView1.Rows.Count - 1; i++)
            {
                DataRow dtrVouDet = dtVoucherdet.Rows.Add();

                dtrVouDet["Nid"] = Nid2;
                dtrVouDet["LocationId"] = Database.LocationId;
                dtrVouDet["vd_id"] = Database.LocationId + dtrVouDet["nid"].ToString();
                dtrVouDet["remarkreq"] = false;
                dtrVouDet["Vi_id"] = vid;
                dtrVouDet["ItemSr"] = ansGridView1.Rows[i].Cells["sno"].Value;
                dtrVouDet["Des_ac_id"] = ansGridView1.Rows[i].Cells["itemid"].Value;// funs.Select_item_id(ansGridView1.Rows[i].Cells["description"].Value.ToString());
                dtrVouDet["Description"] = ansGridView1.Rows[i].Cells["description"].Value.ToString();
                dtrVouDet["packing"] = ansGridView1.Rows[i].Cells["unt"].Value;
                dtrVouDet["Quantity"] = ansGridView1.Rows[i].Cells["Quantity"].Value;
                dtrVouDet["weight"] = ansGridView1.Rows[i].Cells["weight"].Value;
                dtrVouDet["rate_am"] = ansGridView1.Rows[i].Cells["Rate_am"].Value;
                dtrVouDet["Amount"] = ansGridView1.Rows[i].Cells["Amount"].Value;
                dtrVouDet["ChargedWeight"] = ansGridView1.Rows[i].Cells["ChargedWeight"].Value;
                dtrVouDet["Per"] = ansGridView1.Rows[i].Cells["Per"].Value;
                dtrVouDet["bharti"] = double.Parse(ansGridView1.Rows[i].Cells["bharti"].Value.ToString());
                dtrVouDet["LocationId"] = Prelocationid;
                dtrVouDet["freightmr"] = double.Parse(ansGridView1.Rows[i].Cells["freightmr"].Value.ToString());
                dtrVouDet["multiplier"] = double.Parse(ansGridView1.Rows[i].Cells["multiplier"].Value.ToString());

                dtrVouDet["exp1rate"] = double.Parse(ansGridView1.Rows[i].Cells["exp1rate"].Value.ToString());
                dtrVouDet["exp2rate"] = double.Parse(ansGridView1.Rows[i].Cells["exp2rate"].Value.ToString());
                dtrVouDet["exp3rate"] = double.Parse(ansGridView1.Rows[i].Cells["exp3rate"].Value.ToString());
                dtrVouDet["exp4rate"] = double.Parse(ansGridView1.Rows[i].Cells["exp4rate"].Value.ToString());
                dtrVouDet["exp5rate"] = double.Parse(ansGridView1.Rows[i].Cells["exp5rate"].Value.ToString());
                dtrVouDet["exp6rate"] = double.Parse(ansGridView1.Rows[i].Cells["exp6rate"].Value.ToString());
                dtrVouDet["exp7rate"] = double.Parse(ansGridView1.Rows[i].Cells["exp7rate"].Value.ToString());
                dtrVouDet["exp8rate"] = double.Parse(ansGridView1.Rows[i].Cells["exp8rate"].Value.ToString());
                dtrVouDet["exp9rate"] = double.Parse(ansGridView1.Rows[i].Cells["exp9rate"].Value.ToString());
                dtrVouDet["exp10rate"] = double.Parse(ansGridView1.Rows[i].Cells["exp10rate"].Value.ToString());
                dtrVouDet["exp11rate"] = double.Parse(ansGridView1.Rows[i].Cells["exp11rate"].Value.ToString());

                dtrVouDet["exp1amt"] = double.Parse(ansGridView1.Rows[i].Cells["exp1amt"].Value.ToString());
                dtrVouDet["exp2amt"] = double.Parse(ansGridView1.Rows[i].Cells["exp2amt"].Value.ToString());
                dtrVouDet["exp3amt"] = double.Parse(ansGridView1.Rows[i].Cells["exp3amt"].Value.ToString());
                dtrVouDet["exp4amt"] = double.Parse(ansGridView1.Rows[i].Cells["exp4amt"].Value.ToString());
                dtrVouDet["exp5amt"] = double.Parse(ansGridView1.Rows[i].Cells["exp5amt"].Value.ToString());
                dtrVouDet["exp6amt"] = double.Parse(ansGridView1.Rows[i].Cells["exp6amt"].Value.ToString());
                dtrVouDet["exp7amt"] = double.Parse(ansGridView1.Rows[i].Cells["exp7amt"].Value.ToString());
                dtrVouDet["exp8amt"] = double.Parse(ansGridView1.Rows[i].Cells["exp8amt"].Value.ToString());
                dtrVouDet["exp9amt"] = double.Parse(ansGridView1.Rows[i].Cells["exp9amt"].Value.ToString());
                dtrVouDet["exp10amt"] = double.Parse(ansGridView1.Rows[i].Cells["exp10amt"].Value.ToString());
                dtrVouDet["exp11amt"] = double.Parse(ansGridView1.Rows[i].Cells["exp11amt"].Value.ToString());

                dtrVouDet["exp1mr"] = double.Parse(ansGridView1.Rows[i].Cells["exp1mr"].Value.ToString());
                dtrVouDet["exp2mr"] = double.Parse(ansGridView1.Rows[i].Cells["exp2mr"].Value.ToString());
                dtrVouDet["exp3mr"] = double.Parse(ansGridView1.Rows[i].Cells["exp3mr"].Value.ToString());
                dtrVouDet["exp4mr"] = double.Parse(ansGridView1.Rows[i].Cells["exp4mr"].Value.ToString());
                dtrVouDet["exp5mr"] = double.Parse(ansGridView1.Rows[i].Cells["exp5mr"].Value.ToString());
                dtrVouDet["exp6mr"] = double.Parse(ansGridView1.Rows[i].Cells["exp6mr"].Value.ToString());
                dtrVouDet["exp7mr"] = double.Parse(ansGridView1.Rows[i].Cells["exp7mr"].Value.ToString());
                dtrVouDet["exp8mr"] = double.Parse(ansGridView1.Rows[i].Cells["exp8mr"].Value.ToString());
                dtrVouDet["exp9mr"] = double.Parse(ansGridView1.Rows[i].Cells["exp9mr"].Value.ToString());
                dtrVouDet["exp10mr"] = double.Parse(ansGridView1.Rows[i].Cells["exp10mr"].Value.ToString());
                dtrVouDet["exp11mr"] = double.Parse(ansGridView1.Rows[i].Cells["exp11mr"].Value.ToString());

                dtrVouDet["exp1type"] = ansGridView1.Rows[i].Cells["exp1type"].Value.ToString();
                dtrVouDet["exp2type"] = ansGridView1.Rows[i].Cells["exp2type"].Value.ToString();
                dtrVouDet["exp3type"] = ansGridView1.Rows[i].Cells["exp3type"].Value.ToString();
                dtrVouDet["exp4type"] = ansGridView1.Rows[i].Cells["exp4type"].Value.ToString();
                dtrVouDet["exp5type"] = ansGridView1.Rows[i].Cells["exp5type"].Value.ToString();
                dtrVouDet["exp6type"] = ansGridView1.Rows[i].Cells["exp6type"].Value.ToString();
                dtrVouDet["exp7type"] = ansGridView1.Rows[i].Cells["exp7type"].Value.ToString();
                dtrVouDet["exp8type"] = ansGridView1.Rows[i].Cells["exp8type"].Value.ToString();
                dtrVouDet["exp9type"] = ansGridView1.Rows[i].Cells["exp9type"].Value.ToString();
                dtrVouDet["exp10type"] = ansGridView1.Rows[i].Cells["exp10type"].Value.ToString();
                dtrVouDet["exp11type"] = ansGridView1.Rows[i].Cells["exp11type"].Value.ToString();

                dtrVouDet["totexp"] = double.Parse(ansGridView1.Rows[i].Cells["totexp"].Value.ToString());
                dtrVouDet["ItemAmount"] = double.Parse(ansGridView1.Rows[i].Cells["ItemAmount"].Value.ToString());

                dtrVouDet["booking_date"] = dateTimePicker1.Value.Date;
                dtrVouDet["create_date"] = create_date;
                dtrVouDet["modify_date"] = System.DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                dtrVouDet["ch_id"] = null;
                Nid2++;
            }

            foreach (DataRow dtr in dtVoucherdet.Select("", "", DataViewRowState.Unchanged))
            {
                dtr.Delete();
            }

            this.tspb.Value += 1;
            Database.CommandExecutor("delete from voucherdets where vi_id='" + vid + "'");
            dtVoucherdet.TableName = "voucherdets";
            Database.SaveData(dtVoucherdet);
            this.tspb.Value += 1;

            //DataTable dtstocks = new DataTable("stocks");
            string strStockSql = "";
            if (gStr != "0")
            {
                strStockSql = "Select * from stocks where Gr_id='" + vid + "';";
            }
            else
            {
                strStockSql = "Select * from stocks where Vid='" + vid + "';";
            }
            //Database.GetSqlData(strStockSql, dtstocks);

            strStockSql += "Select * from VOUCHARGESs where Vi_id='" + vid + "'";

            //DataTable dtVoucherCharges = new DataTable("VOUCHARGESs");
            //Database.GetSqlData("Select * from VOUCHARGESs where Vi_id='" + vid + "'", dtVoucherCharges);
            DataSet ds = new DataSet();
            Database.GetSqlData(strStockSql, ds);
            
            DataTable dtstocks = ds.Tables[0];
            dtstocks.TableName = "Stocks";
            DataTable dtVoucherCharges = ds.Tables[1];
            this.tspb.Value += 1;

            DataRow dtrStockRow;
            #region oldstock


            if (gStr != "0")
            {

                int grcount = Database.GetScalarInt("Select count(*) from Stocks where Gr_id='" + vid + "'");

             //   if (dtstocks.Rows.Count == 1 && iscancel == true)
                if (iscancel == true)
                {
                    //DataTable dtskchk = new DataTable("stocks");
                    //Database.GetSqlData("Select * from stocks where Vid='" + vid + "'", dtskchk);

                    Database.CommandExecutor("delete from stocks where Vid='" + vid + "'");

                //    for (int j = 0; j < dtskchk.Rows.Count; j++)
                //    {
                //        dtskchk.Rows[j].Delete();
                //    }
                ////    dtstocks.AcceptChanges();
                //    Database.SaveData(dtskchk);
                }
                else if (iscancel == false)
                {
                    Database.GetSqlData("Select * from stocks where Gr_id='" + vid + "'", dtstocks);
                    DataTable dtskchk = new DataTable();
                    Database.GetSqlData("SELECT  Stocks.GR_id FROM  Stocks LEFT OUTER JOIN   VOUCHERINFOs ON  Stocks.vid =  VOUCHERINFOs.Vi_id LEFT OUTER JOIN   VOUCHERTYPEs ON  VOUCHERINFOs.Vt_id =  VOUCHERTYPEs.Vt_id WHERE  ( VOUCHERTYPEs.Type = 'Booking') AND ( Stocks.GR_id = '" + vid + "')", dtskchk);
                    if (dtskchk.Rows.Count == 0)
                    {
                   
                    if (dtstocks.Rows.Count > 0)
                    {
                        dtrStockRow = dtstocks.Rows[0];
                    }
                    else
                    {
                        dtrStockRow = dtstocks.Rows.Add();
                        dtrStockRow["Vid"] = vid;
                        dtrStockRow["GR_id"] = vid;
                        dtrStockRow["Quantity"] = 1;
                        dtrStockRow["Step"] = "Step1";
                        dtrStockRow["Godown_id"] = Database.LocationId;
                        string aliasname = dtMaster.Rows[0]["Aliasname"].ToString();// Database.GetScalarText("Select Aliasname from vouchertypes where vt_id=" + vtid);
                        dtrStockRow["Narration"] = aliasname + " At " + textBox3.Text;
                    }

                    dtrStockRow["GRNo"] = grno;
                    dtrStockRow["GRDate"] = dateTimePicker1.Value.Date;
                    dtrStockRow["Consigner_id"] = dtMaster.Rows[0]["ac1id"];// funs.Select_ac_id(textBox1.Text);
                    dtrStockRow["Consignee_id"] = dtMaster.Rows[0]["ac2id"]; //funs.Select_ac_id(textBox2.Text);
                    dtrStockRow["Source_id"] = dtMaster.Rows[0]["orignid"];// funs.Select_dp_id(textBox3.Text);
                    dtrStockRow["Destination_id"] = dtMaster.Rows[0]["delpid"];// funs.Select_dp_id(textBox4.Text);

                    if (textBox24.Text == "To Pay")
                    {
                        dtrStockRow["ToPay"] = textBox10.Text;
                        dtrStockRow["TBB"] = 0;
                        dtrStockRow["Paid"] = 0;
                        dtrStockRow["FOC"] = 0;
                    }
                    else if (textBox24.Text == "FOC")
                    {
                        dtrStockRow["ToPay"] = 0;
                        dtrStockRow["TBB"] = 0;
                        dtrStockRow["Paid"] = 0;
                        dtrStockRow["FOC"] = textBox10.Text;
                    }
                    else if (textBox24.Text == "Paid")
                    {
                        dtrStockRow["ToPay"] = 0;
                        dtrStockRow["TBB"] = 0;
                        dtrStockRow["Paid"] = textBox10.Text;
                        dtrStockRow["FOC"] = 0;
                    }
                    else if (textBox24.Text == "T.B.B.")
                    {
                        dtrStockRow["ToPay"] = 0;
                        dtrStockRow["TBB"] = textBox10.Text;
                        dtrStockRow["Paid"] = 0;
                        dtrStockRow["FOC"] = 0;
                    }
                    dtrStockRow["TotWeight"] = textBox7.Text;
                    dtrStockRow["TotPkts"] = textBox16.Text;
                    dtrStockRow["Actweight"] = textBox20.Text;
                    dtrStockRow["GRCharge"] = textBox6.Text;
                    dtrStockRow["GRType"] = textBox24.Text;
                    double othch = double.Parse(textBox26.Text) + double.Parse(textBox12.Text) + double.Parse(textBox13.Text) + double.Parse(textBox14.Text) + double.Parse(textBox15.Text) + double.Parse(textBox17.Text) + double.Parse(textBox18.Text) + double.Parse(textBox19.Text) + double.Parse(textBox21.Text) + double.Parse(textBox22.Text) + double.Parse(textBox9.Text);
                    dtrStockRow["OthCharge"] = othch;
                    dtrStockRow["Freight"] = textBox8.Text;

                    int count = ansGridView1.Rows.Count - 1;
                    if (count == 1)
                    {
                        dtrStockRow["ItemName"] = ansGridView1.Rows[0].Cells["description"].Value.ToString();
                    }
                    else
                    {
                        dtrStockRow["ItemName"] = ansGridView1.Rows[0].Cells["description"].Value.ToString() + " + " + (count - 1).ToString();
                    }

                    dtrStockRow["Packing"] = ansGridView1.Rows[0].Cells["unt"].Value.ToString();
                    dtrStockRow["DeliveryType"] = textBox25.Text;
                    dtrStockRow["Private"] = field1;
                    dtrStockRow["Remark"] = field7;

                    }


                    for (int i = 0; i < dtstocks.Rows.Count; i++)
                    {
                        dtstocks.Rows[i]["GRNo"] = grno;
                        dtstocks.Rows[i]["GRDate"] = dateTimePicker1.Value.Date;
                        dtstocks.Rows[i]["Consigner_id"] = dtMaster.Rows[0]["ac1id"]; //funs.Select_ac_id(textBox1.Text);
                        dtstocks.Rows[i]["Consignee_id"] = dtMaster.Rows[0]["ac2id"]; //funs.Select_ac_id(textBox2.Text);
                        dtstocks.Rows[i]["Source_id"] = dtMaster.Rows[0]["orignid"]; //funs.Select_dp_id(textBox3.Text);
                        dtstocks.Rows[i]["Destination_id"] = dtMaster.Rows[0]["delpid"];// funs.Select_dp_id(textBox4.Text);
                        dtstocks.Rows[i]["grno"] = grno;
                        if (textBox24.Text == "To Pay")
                        {
                            dtstocks.Rows[i]["ToPay"] = textBox10.Text;
                            dtstocks.Rows[i]["TBB"] = 0;
                            dtstocks.Rows[i]["Paid"] = 0;
                            dtstocks.Rows[i]["FOC"] = 0;
                        }
                        else if (textBox24.Text == "FOC")
                        {
                            dtstocks.Rows[i]["ToPay"] = 0;
                            dtstocks.Rows[i]["TBB"] = 0;
                            dtstocks.Rows[i]["Paid"] = 0;
                            dtstocks.Rows[i]["FOC"] = textBox10.Text;
                        }
                        else if (textBox24.Text == "Paid")
                        {
                            dtstocks.Rows[i]["ToPay"] = 0;
                            dtstocks.Rows[i]["TBB"] = 0;
                            dtstocks.Rows[i]["Paid"] = textBox10.Text;
                            dtstocks.Rows[i]["FOC"] = 0;
                        }
                        else if (textBox24.Text == "T.B.B.")
                        {
                            dtstocks.Rows[i]["ToPay"] = 0;
                            dtstocks.Rows[i]["TBB"] = textBox10.Text;
                            dtstocks.Rows[i]["Paid"] = 0;
                            dtstocks.Rows[i]["FOC"] = 0;
                        }
                        dtstocks.Rows[i]["TotWeight"] = textBox7.Text;
                        dtstocks.Rows[i]["TotPkts"] = textBox16.Text;
                        dtstocks.Rows[i]["Actweight"] = textBox20.Text;
                        dtstocks.Rows[i]["GRCharge"] = textBox6.Text;
                        dtstocks.Rows[i]["GRType"] = textBox24.Text;
                        double othch = double.Parse(textBox26.Text) + double.Parse(textBox12.Text) + double.Parse(textBox13.Text) + double.Parse(textBox14.Text) + double.Parse(textBox15.Text) + double.Parse(textBox17.Text) + double.Parse(textBox18.Text) + double.Parse(textBox19.Text) + double.Parse(textBox21.Text) + double.Parse(textBox22.Text) + double.Parse(textBox9.Text);
                        dtstocks.Rows[i]["OthCharge"] = othch;
                        dtstocks.Rows[i]["Freight"] = textBox8.Text;

                        int count = ansGridView1.Rows.Count - 1;
                        if (count == 1)
                        {
                            dtstocks.Rows[i]["ItemName"] = ansGridView1.Rows[0].Cells["description"].Value.ToString();
                        }
                        else
                        {
                            dtstocks.Rows[i]["ItemName"] = ansGridView1.Rows[0].Cells["description"].Value.ToString() + " + " + (count - 1).ToString();
                        }

                        dtstocks.Rows[i]["Packing"] = ansGridView1.Rows[0].Cells["unt"].Value.ToString();
                        dtstocks.Rows[i]["DeliveryType"] = textBox25.Text;
                        dtstocks.Rows[i]["Private"] = field1;
                        dtstocks.Rows[i]["Remark"] = field7;
                    }

                    //for (int a = 0; a < dtstocks.Rows.Count; a++)
                    //{
                     //   dtstocks.AcceptChanges();
                   // }
                    Database.SaveData(dtstocks);

                }

            }

            else
            {
               // DataTable dtstocks = new DataTable("stocks");
                Database.GetSqlData("Select * from stocks where Vid='" + vid + "'", dtstocks);

                for (int j = 0; j < dtstocks.Rows.Count; j++)
                {
                    dtstocks.Rows[j].Delete();
                }
                Database.SaveData(dtstocks);

                dtrStockRow = dtstocks.Rows.Add();
                dtrStockRow["Vid"] = vid;
                dtrStockRow["GR_id"] = vid;
                dtrStockRow["Quantity"] = 1;
                dtrStockRow["Step"] = "Step1";
                dtrStockRow["Godown_id"] = Database.LocationId;
                string aliasname = dtMaster.Rows[0]["Aliasname"].ToString(); // Database.GetScalarText("Select Aliasname from vouchertypes where vt_id=" + vtid);
                dtrStockRow["Narration"] = aliasname + " At " + textBox3.Text;
                dtrStockRow["GRNo"] = grno;
                dtrStockRow["GRDate"] = dateTimePicker1.Value.Date;
                dtrStockRow["Consigner_id"] = dtMaster.Rows[0]["ac1id"];// funs.Select_ac_id(textBox1.Text);
                dtrStockRow["Consignee_id"] = dtMaster.Rows[0]["ac2id"];// funs.Select_ac_id(textBox2.Text);
                dtrStockRow["Source_id"] = dtMaster.Rows[0]["orignid"]; //funs.Select_dp_id(textBox3.Text);
                dtrStockRow["Destination_id"] = dtMaster.Rows[0]["delpid"]; //funs.Select_dp_id(textBox4.Text);
                dtrStockRow["grno"] = grno;
                dtrStockRow["Actweight"] = textBox20.Text;

                if (textBox24.Text == "To Pay")
                {
                    dtrStockRow["ToPay"] = textBox10.Text;
                    dtrStockRow["TBB"] = 0;
                    dtrStockRow["Paid"] = 0;
                    dtrStockRow["FOC"] = 0;
                }
                else if (textBox24.Text == "FOC")
                {
                    dtrStockRow["ToPay"] = 0;
                    dtrStockRow["TBB"] = 0;
                    dtrStockRow["Paid"] = 0;
                    dtrStockRow["FOC"] = textBox10.Text;
                }
                else if (textBox24.Text == "Paid")
                {
                    dtrStockRow["ToPay"] = 0;
                    dtrStockRow["TBB"] = 0;
                    dtrStockRow["Paid"] = textBox10.Text;
                    dtrStockRow["FOC"] = 0;
                }
                else if (textBox24.Text == "T.B.B.")
                {
                    dtrStockRow["ToPay"] = 0;
                    dtrStockRow["TBB"] = textBox10.Text;
                    dtrStockRow["Paid"] = 0;
                    dtrStockRow["FOC"] = 0;
                }
                dtrStockRow["TotWeight"] = textBox7.Text;
                dtrStockRow["TotPkts"] = textBox16.Text;
                dtrStockRow["GRCharge"] = textBox6.Text;

                dtrStockRow["GRType"] = textBox24.Text;
                double othch = double.Parse(textBox26.Text) + double.Parse(textBox12.Text) + double.Parse(textBox13.Text) + double.Parse(textBox14.Text) + double.Parse(textBox15.Text) + double.Parse(textBox17.Text) + double.Parse(textBox18.Text) + double.Parse(textBox19.Text) + double.Parse(textBox21.Text) + double.Parse(textBox22.Text) + double.Parse(textBox9.Text);
                dtrStockRow["OthCharge"] = othch;
                dtrStockRow["Freight"] = textBox8.Text;

                int count = ansGridView1.Rows.Count - 1;
                if (count == 1)
                {
                    dtrStockRow["ItemName"] = ansGridView1.Rows[0].Cells["description"].Value.ToString();
                }
                else
                {
                    dtrStockRow["ItemName"] = ansGridView1.Rows[0].Cells["description"].Value.ToString() + " + " + (count - 1).ToString();
                }

                dtrStockRow["Packing"] = ansGridView1.Rows[0].Cells["unt"].Value.ToString();
                dtrStockRow["DeliveryType"] = textBox25.Text;
                dtrStockRow["Private"] = field1;
                dtrStockRow["Remark"] = field7;

            }
            #endregion

           dtrStockRow = null;
            if (gStr != "0")
            {
                if (dtstocks.Rows.Count > 0 && iscancel == true)
                {
                    Database.CommandExecutor("delete from stocks where Vid='" + vid + "'");
                }
            }

            if (dtstocks.Rows.Count > 0)
            {
                dtrStockRow = dtstocks.Rows[0];
            }
            else
            {
                dtrStockRow = dtstocks.Rows.Add();
                dtrStockRow["Vid"] = vid;
                dtrStockRow["GR_id"] = vid;
                dtrStockRow["Quantity"] = 1;
                dtrStockRow["Step"] = "Step1";
                dtrStockRow["Godown_id"] = Database.LocationId;
                string aliasname = dtMaster.Rows[0]["Aliasname"].ToString();// Database.GetScalarText("Select Aliasname from vouchertypes where vt_id=" + vtid);
                dtrStockRow["Narration"] = aliasname + " At " + textBox3.Text;
            }

            if (dtrStockRow != null)
            {
                dtrStockRow["GRNo"] = grno;
                dtrStockRow["GRDate"] = dateTimePicker1.Value.Date;
                dtrStockRow["Consigner_id"] = dtMaster.Rows[0]["ac1id"];// funs.Select_ac_id(textBox1.Text);
                dtrStockRow["Consignee_id"] = dtMaster.Rows[0]["ac2id"]; //funs.Select_ac_id(textBox2.Text);
                dtrStockRow["Source_id"] = dtMaster.Rows[0]["orignid"];// funs.Select_dp_id(textBox3.Text);
                dtrStockRow["Destination_id"] = dtMaster.Rows[0]["delpid"];// funs.Select_dp_id(textBox4.Text);

                if (textBox24.Text == "To Pay")
                {
                    dtrStockRow["ToPay"] = textBox10.Text;
                    dtrStockRow["TBB"] = 0;
                    dtrStockRow["Paid"] = 0;
                    dtrStockRow["FOC"] = 0;
                }
                else if (textBox24.Text == "FOC")
                {
                    dtrStockRow["ToPay"] = 0;
                    dtrStockRow["TBB"] = 0;
                    dtrStockRow["Paid"] = 0;
                    dtrStockRow["FOC"] = textBox10.Text;
                }
                else if (textBox24.Text == "Paid")
                {
                    dtrStockRow["ToPay"] = 0;
                    dtrStockRow["TBB"] = 0;
                    dtrStockRow["Paid"] = textBox10.Text;
                    dtrStockRow["FOC"] = 0;
                }
                else if (textBox24.Text == "T.B.B.")
                {
                    dtrStockRow["ToPay"] = 0;
                    dtrStockRow["TBB"] = textBox10.Text;
                    dtrStockRow["Paid"] = 0;
                    dtrStockRow["FOC"] = 0;
                }
                dtrStockRow["TotWeight"] = textBox7.Text;
                dtrStockRow["TotPkts"] = textBox16.Text;
                dtrStockRow["Actweight"] = textBox20.Text;
                dtrStockRow["GRCharge"] = textBox6.Text;
                dtrStockRow["GRType"] = textBox24.Text;
                double othch = double.Parse(textBox26.Text) + double.Parse(textBox12.Text) + double.Parse(textBox13.Text) + double.Parse(textBox14.Text) + double.Parse(textBox15.Text) + double.Parse(textBox17.Text) + double.Parse(textBox18.Text) + double.Parse(textBox19.Text) + double.Parse(textBox21.Text) + double.Parse(textBox22.Text) + double.Parse(textBox9.Text);
                dtrStockRow["OthCharge"] = othch;
                dtrStockRow["Freight"] = textBox8.Text;

                int count = ansGridView1.Rows.Count - 1;

                if (count == 1)
                {
                    dtrStockRow["ItemName"] = ansGridView1.Rows[0].Cells["description"].Value.ToString();
                }
                else
                {
                    dtrStockRow["ItemName"] = ansGridView1.Rows[0].Cells["description"].Value.ToString() + " + " + (count - 1).ToString();
                }

                dtrStockRow["Packing"] = ansGridView1.Rows[0].Cells["unt"].Value.ToString();
                dtrStockRow["DeliveryType"] = textBox25.Text;
                dtrStockRow["Private"] = field1;
                dtrStockRow["Remark"] = field7;

                dtstocks.TableName = "stocks";
                this.tspb.Value += 1;
                Database.SaveData(dtstocks);
                this.tspb.Value += 1;
            }

            //DataTable dtVoucherCharges = new DataTable("VOUCHARGESs");
            //Database.GetSqlData("Select * from VOUCHARGESs where Vi_id='" + vid + "'", dtVoucherCharges);
            long Nid3 = long.Parse(dtMaster.Rows[0]["vc_nid"].ToString());// Database.GetScalarLong("select isnull( max(Nid),0)+1 as Nid from VOUCHARGESs where locationid='" + Database.LocationId + "'");

            Nid3 = SetVoucherCharges(dtVoucherCharges, vid, 0, "Freight", double.Parse(textBox8.Text), Prelocationid, create_date, System.DateTime.Now, Nid3);

            Nid3 = SetVoucherCharges(dtVoucherCharges, vid, 1, label4.Text, double.Parse(textBox6.Text), Prelocationid, create_date, System.DateTime.Now, Nid3);

            Nid3 = SetVoucherCharges(dtVoucherCharges, vid, 2, label6.Text, double.Parse(textBox12.Text), Prelocationid, create_date, System.DateTime.Now, Nid3);

            Nid3 = SetVoucherCharges(dtVoucherCharges, vid, 3, label7.Text, double.Parse(textBox13.Text), Prelocationid, create_date, System.DateTime.Now, Nid3);

            Nid3 = SetVoucherCharges(dtVoucherCharges, vid, 4, label12.Text, double.Parse(textBox14.Text), Prelocationid, create_date, System.DateTime.Now, Nid3);

            Nid3 = SetVoucherCharges(dtVoucherCharges, vid, 5, label13.Text, double.Parse(textBox15.Text), Prelocationid, create_date, System.DateTime.Now, Nid3);

            Nid3 = SetVoucherCharges(dtVoucherCharges, vid, 6, label14.Text, double.Parse(textBox17.Text), Prelocationid, create_date, System.DateTime.Now, Nid3);

            Nid3 = SetVoucherCharges(dtVoucherCharges, vid, 7, label16.Text, double.Parse(textBox18.Text), Prelocationid, create_date, System.DateTime.Now, Nid3);

            Nid3 = SetVoucherCharges(dtVoucherCharges, vid, 8, label17.Text, double.Parse(textBox19.Text), Prelocationid, create_date, System.DateTime.Now, Nid3);

            Nid3 = SetVoucherCharges(dtVoucherCharges, vid, 9, label18.Text, double.Parse(textBox21.Text), Prelocationid, create_date, System.DateTime.Now, Nid3);

            Nid3 = SetVoucherCharges(dtVoucherCharges, vid, 10, label19.Text, double.Parse(textBox22.Text), Prelocationid, create_date, System.DateTime.Now, Nid3);

            Nid3 = SetVoucherCharges(dtVoucherCharges, vid, 11, label29.Text, double.Parse(textBox26.Text), Prelocationid, create_date, System.DateTime.Now, Nid3);

            foreach (DataRow dtr in dtVoucherCharges.Select("", "", DataViewRowState.Unchanged))
            {
                dtr.Delete();
            }

            dtVoucherCharges.TableName = "VOUCHARGESs";
            this.tspb.Value += 1;
            Database.SaveData(dtVoucherCharges);









            DataTable temp = new DataTable("Journals");
            Database.GetSqlData("Select * from Journals where vi_id='"+ vid+"'",temp);
            for (int i = 0; i < temp.Rows.Count; i++)
            {
                temp.Rows[i].Delete();
            }
            Database.SaveData(temp);

            if (textBox24.Text == "Paid" && iscancel==false)
            {
             

                //debit
                DataRow dtrjou = dtJournal.Rows.Add();
                dtrjou["Vi_id"] = vid;
                dtrjou["vdate"] = dateTimePicker1.Value.Date.ToString(Database.dformat);
                if (textBox29.Text == "Credit")
                {
                    if (textBox23.Text == "Consigner")
                    {
                        dtrjou["Ac_id"] = funs.Select_ac_id(textBox1.Text);
                    }
                    else
                    {
                        dtrjou["Ac_id"] = funs.Select_ac_id(textBox2.Text);
                    }
                }
                else
                {

                    dtrjou["Ac_id"] = Database.LocationCashAcc_id;
                }

                string mainaac = dtrjou["Ac_id"].ToString();

                dtrjou["Opp_Acid"] = Database.LocationExpAcc_id;

                dtrjou["Narr"] = "Booking";
                dtrjou["Sno"] = 1;
                dtrjou["LocationId"] = Prelocationid;
                dtrjou["Amount"] = double.Parse(textBox10.Text);
               
                dtrjou["Narr2"] = "Booking";
                dtrjou["Reffno"] = vno;


                this.tspb.Value += 1;
                //credit
                DataRow dtrjou1 = dtJournal.Rows.Add();
                dtrjou1["Vi_id"] = vid;
                dtrjou1["vdate"] = dateTimePicker1.Value.Date.ToString(Database.dformat);

                dtrjou1["Ac_id"] = Database.LocationExpAcc_id;
               

                dtrjou1["Narr"] = "Booking";
                dtrjou1["Sno"] = 2;
                dtrjou1["LocationId"] = Prelocationid;
                dtrjou1["Amount"] =-1* double.Parse(textBox10.Text);
                dtrjou1["Opp_Acid"] = mainaac;
                dtrjou1["Narr2"] = "Booking";
                dtrjou1["Reffno"] = vno;


                this.tspb.Value += 1;


                    Database.SaveData(dtJournal);
               



            }



            this.tspb.Value = 0;






            funs.ShowBalloonTip("Saved", "Voucher Number: " + vno + " Saved Successfully");


        }

        private long SetVoucherCharges(DataTable dtVC, string strVID, long serial, string strCharge, double Amt, string strlocation, DateTime cretdate, DateTime modfydate, long nid)
        {
            if (Amt == 0) { return nid; }

            long iNID = 0;
            DataRow dtrNew;
            DataRow[] dtRows = dtVC.Select("Charg_Name='" + strCharge + "'");
            if (dtRows.Length > 0)
            {
                dtrNew = dtRows[0];
                if (dtrNew.RowState == DataRowState.Unchanged)
                {
                    dtRows[0]["Amount"] = Amt;
                    dtrNew["Nid"] = nid;
                    iNID = nid + 1;
                }
                else
                {
                    dtrNew["Amount"] = double.Parse(dtrNew["Amount"].ToString()) + Amt;
                    iNID = nid;
                }
            }
            else
            {
                dtrNew = dtVC.Rows.Add();
                dtrNew["Amount"] = Amt;
                dtrNew["Nid"] = nid;
                iNID = nid + 1;
            }

            dtrNew["Vi_id"] = strVID;
            dtrNew["Srno"] = serial;
            dtrNew["Charg_Name"] = strCharge;
            dtrNew["locationid"] = strlocation;
            dtrNew["create_date"] = cretdate;
            dtrNew["modify_date"] = modfydate.ToString("dd-MM-yyyy HH:mm:ss");
            dtrNew["vc_id"] = dtrNew["locationid"] + dtrNew["Nid"].ToString();


            return iNID;
        }

        private void clear()
        {
            if (gStr == "0")
            {
                LoadData("0", "Booking");
            }
            else
            {
                this.Close();
                this.Dispose();
            }
        }


        private void view()
        {

            if (Database.printtype == "DOS")
            {
                string str = DOSReport.voucherprint(vid, "View");
                str = str.Replace("\0", "").Replace("W1 ", "").Replace("W0", "").Replace("W1", "");
                frm_printpre frm = new frm_printpre();
                frm.str = str;
                frm.ShowDialog();
            }
            else
            {


                if (Feature.Available("Ask Copies") == "No")
                {
                    OtherReport rpt = new OtherReport();
                    DataTable dtprintcopy = new DataTable();
                    Database.GetSqlData("Select printcopy from Vouchertypes where Vt_id=" + vtid, dtprintcopy);
                    String[] print_option = dtprintcopy.Rows[0]["printcopy"].ToString().Split(';');

                    for (int j = 0; j < print_option.Length; j++)
                    {
                        if (print_option[j] != "")
                        {
                            String[] defaultcopy = print_option[j].Split(',');

                            if (bool.Parse(defaultcopy[1]) == true)
                            {
                                rpt.voucherprint(this, vtid, vid, defaultcopy[0], true, "View");
                            }
                        }
                    }
                }
                else
                {
                    frm_printcopy frm = new frm_printcopy("View", vid, vtid);
                    frm.ShowDialog();
                }
            }
        }


        private void SendSMS()
        {

            if (Feature.Available("Send Sms") == "Yes")
            {
                string msg = "Dear Sir Your Consignment no. " + grno + " Booked on " + dateTimePicker1.Value.Date.ToString(Database.dformat) + " from " + textBox1.Text + " at " + textBox3.Text + " PKGS." + funs.DecimalPoint(double.Parse(textBox16.Text), 0) + " INV.NO. " + field2 + " . Thanks for being us.";
                if (vid != "0")
                {
                    DialogResult ch = MessageBox.Show(null, "Are you want to send SMS?", "Confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                    if (ch == DialogResult.OK)
                    {
                        if (funs.isDouble(funs.Select_SMSMobile(textBox2.Text)) == true)
                        {

                            if (funs.Select_SMSMobile(textBox2.Text) != "0")
                            {
                                sms objsms = new sms();
                                objsms.send(msg, funs.Select_SMSMobile(textBox2.Text), textBox2.Text);
                                // MessageBox.Show(msg);
                            }
                        }
                    }

                }
                else
                {
                    if (funs.isDouble(funs.Select_SMSMobile(textBox2.Text)) == true)
                    {

                        if (funs.Select_SMSMobile(textBox2.Text) != "0")
                        {
                            sms objsms = new sms();
                            objsms.send(msg, funs.Select_SMSMobile(textBox2.Text), textBox2.Text);
                            // MessageBox.Show(msg);
                        }
                    }
                }

            }

        }

        private void Print()
        {
            if (Database.printtype == "DOS")
            {
                string str = DOSReport.voucherprint(vid);
            }
            else
            {
                if (Feature.Available("Ask Copies") == "No")
                {
                    OtherReport rpt = new OtherReport();
                    DataTable dtprintcopy = new DataTable();
                    Database.GetSqlData("Select printcopy from Vouchertypes where Vt_id=" + vtid, dtprintcopy);
                    String[] print_option = dtprintcopy.Rows[0]["printcopy"].ToString().Split(';');

                    for (int j = 0; j < print_option.Length; j++)
                    {
                        if (print_option[j] != "")
                        {
                            String[] defaultcopy = print_option[j].Split(',');

                            if (bool.Parse(defaultcopy[1]) == true)
                            {
                                rpt.voucherprint(this, vtid, vid, defaultcopy[0], true, "Print");
                            }
                        }
                    }
                }
                else
                {
                    frm_printcopy frm = new frm_printcopy("Print", vid, vtid);
                    frm.ShowDialog();
                }
            }


        }
        private void labelCalc()
        {
            double subtot = 0, totqty = 0, totweight = 0, TotCdAmount = 0, totchargwht = 0, totfreight = 0, totexpense = 0, totexp1 = 0, totexp2 = 0, totexp3 = 0, totexp4 = 0, totexp5 = 0, totexp6 = 0, totexp7 = 0, totexp8 = 0, totexp9 = 0, totexp10 = 0, totexp11 = 0;
            for (int i = 0; i < ansGridView1.Rows.Count - 1; i++)
            {
                totexpense += double.Parse(ansGridView1.Rows[i].Cells["totexp"].Value.ToString());
                totfreight += double.Parse(ansGridView1.Rows[i].Cells["Amount"].Value.ToString());
                totweight += double.Parse(ansGridView1.Rows[i].Cells["weight"].Value.ToString());
                subtot += double.Parse(ansGridView1.Rows[i].Cells["ItemAmount"].Value.ToString());
                totqty += double.Parse(ansGridView1.Rows[i].Cells["Quantity"].Value.ToString());
                totchargwht += double.Parse(ansGridView1.Rows[i].Cells["ChargedWeight"].Value.ToString());

                totexp1 += double.Parse(ansGridView1.Rows[i].Cells["exp1amt"].Value.ToString());
                totexp2 += double.Parse(ansGridView1.Rows[i].Cells["exp2amt"].Value.ToString());
                totexp3 += double.Parse(ansGridView1.Rows[i].Cells["exp3amt"].Value.ToString());
                totexp4 += double.Parse(ansGridView1.Rows[i].Cells["exp4amt"].Value.ToString());
                totexp5 += double.Parse(ansGridView1.Rows[i].Cells["exp5amt"].Value.ToString());
                totexp6 += double.Parse(ansGridView1.Rows[i].Cells["exp6amt"].Value.ToString());
                totexp7 += double.Parse(ansGridView1.Rows[i].Cells["exp7amt"].Value.ToString());
                totexp8 += double.Parse(ansGridView1.Rows[i].Cells["exp8amt"].Value.ToString());
                totexp9 += double.Parse(ansGridView1.Rows[i].Cells["exp9amt"].Value.ToString());
                totexp10 += double.Parse(ansGridView1.Rows[i].Cells["exp10amt"].Value.ToString());
                totexp11 += double.Parse(ansGridView1.Rows[i].Cells["exp11amt"].Value.ToString());
            }

            textBox8.Text = funs.DecimalPoint(totfreight, 2);
            textBox7.Text = funs.DecimalPoint(totchargwht, 3);

            textBox16.Text = funs.DecimalPoint(totqty, 2);
            textBox20.Text = funs.DecimalPoint(totweight, 3);

            textBox6.Text = funs.DecimalPoint(totexp1, 2);
            textBox12.Text = funs.DecimalPoint(totexp2, 2);
            textBox13.Text = funs.DecimalPoint(totexp3, 2);
            textBox14.Text = funs.DecimalPoint(totexp4, 2);
            textBox15.Text = funs.DecimalPoint(totexp5, 2);
            textBox17.Text = funs.DecimalPoint(totexp6, 2);
            textBox18.Text = funs.DecimalPoint(totexp7, 2);

            textBox19.Text = funs.DecimalPoint(totexp8, 2);
            textBox21.Text = funs.DecimalPoint(totexp9, 2);
            textBox22.Text = funs.DecimalPoint(totexp10, 2);
            textBox26.Text = funs.DecimalPoint(totexp11, 2);
            //  textBox18.Text = funs.DecimalPoint(subtot, 0) + ".00";
            textBox10.Text = funs.DecimalPoint(subtot, 0) + ".00";
            if (RoffChanged == false)
            {
                textBox9.Text = funs.DecimalPoint((double.Parse(textBox10.Text) - subtot));
            }
            else
            {
                textBox10.Text = funs.DecimalPoint((subtot - double.Parse(textBox9.Text)));
            }
            textBox10.Text = funs.DecimalPoint((subtot + double.Parse(textBox9.Text)));
        }

        private void SetVno()
        {
            int numtype = funs.Select_NumType(vtid);
            if ((Prelocationid == Database.LocationId) || (Prelocationid == "" && vid == "0"))
            {
                if (numtype == 3 && vno != 0 && vid != "0")
                {
                    DateTime dt1 = dateTimePicker1.Value;
                    DateTime dt2 = DateTime.Parse(Database.GetScalarDate("select vdate from voucherinfos where LocationId='" + Database.LocationId + "' and vi_id='" + vid + "'"));
                    if (dt1 != dt2)
                    {
                        vno = funs.GenerateVno(vtid, dateTimePicker1.Value.ToString("dd-MMM-yyyy"), vid);
                        label10.Text = vno.ToString();
                    }
                    return;
                }

                if (vtid == 0 || (vno != 0 && vid != "0"))
                {
                    return;
                }
                vno = funs.GenerateVno(vtid, dateTimePicker1.Value.ToString("dd-MMM-yyyy"), vid);
                label10.Text = vno.ToString();
            }
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            strCombo = "SELECT [name] from DeliveryPoints";
            string st = e.KeyChar.ToString();
            if (textBox3.Text != "")
            {
                st = textBox3.Text;
            }
            textBox3.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, st, 0);
            if (textBox23.Text == "Consigner")
            {
                Expenses(funs.Select_ac_id(textBox1.Text));
            }
            else if (textBox23.Text == "Consignee")
            {
                Expenses(funs.Select_ac_id(textBox2.Text));
            }
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            strCombo = "SELECT [name] from DeliveryPoints";
            string st = e.KeyChar.ToString();

            if (textBox4.Text != "")
            {
                st = textBox4.Text;
            }
            textBox4.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, st, 0);

            if (textBox23.Text == "Consigner")
            {
                Expenses(funs.Select_ac_id(textBox1.Text));
            }
            else if (textBox23.Text == "Consignee")
            {
                Expenses(funs.Select_ac_id(textBox2.Text));
            }
            DeliveryAdd();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            string strCombo = "SELECT ACCOUNTs.name, ACCOUNTs.Printname, DeliveryPoints.Name AS Station, ACCOUNTs.Address1, ACCOUNTs.Address2,  ACCOUNTs.phone, ACCOUNTs.tin_number, OTHERs.Name AS Staff, CONTRACTORs.Name AS Agent, ACCOUNTs.ac_id FROM ACCOUNTs LEFT OUTER JOIN  ACCOUNTYPEs ON ACCOUNTs.act_id = ACCOUNTYPEs.Act_id LEFT OUTER JOIN CONTRACTORs ON ACCOUNTs.con_id = CONTRACTORs.Name LEFT OUTER JOIN OTHERs ON ACCOUNTs.loc_id = OTHERs.Oth_id LEFT OUTER JOIN DeliveryPoints ON ACCOUNTs.SId = DeliveryPoints.DPId WHERE ( ACCOUNTYPEs.Path LIKE '1;39;%') ORDER BY ACCOUNTs.name ";

            //textBox1.Text = SelectCombo.ComboDt(this, Master.AccInfo, 2);
            // strCombo = "SELECT ACCOUNTs.Name, ACCOUNTs.Printname, DeliveryPoints.Name AS Station, ACCOUNTs.Address1, ACCOUNTs.Address2, ACCOUNTs.Phone, ACCOUNTs.Tin_number, OTHERs.Name AS Staff, CONTRACTORs.Name AS Agent FROM ACCOUNTs LEFT OUTER JOIN CONTRACTORs ON ACCOUNTs.Con_id = CONTRACTORs.Name LEFT OUTER JOIN OTHERs ON ACCOUNTs.Loc_id = OTHERs.Oth_id LEFT OUTER JOIN DeliveryPoints ON ACCOUNTs.SId = DeliveryPoints.DPId WHERE ACCOUNTs.Act_id = 39 ORDER BY ACCOUNTs.Name";
            //textBox1.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 2);
            label23.Text = "";// funs.Select_GST(textBox1.Text);
            textBox1.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 2);
         //   textBox1.Text = SelectCombo.ComboDt(this, e.KeyChar, clsCashing.GetAccounts(39), e.KeyChar.ToString(), 2);

            if (textBox1.Text != "")
            {
                DataTable dtAc1 = new DataTable();
                Database.GetSqlData("Select ac.ac_id,ac.Tin_number,ac.Delivery_type,ac.GR_type,dp.name as sourcepoint from accounts as ac left join DeliveryPoints as dp on ac.sid=dp.dpid where ac.name='" + textBox1.Text + "'", dtAc1);
                if (dtAc1.Rows.Count <= 0) { dtAc1.Rows.Add(); }

                label23.Text = dtAc1.Rows[0]["Tin_number"].ToString();
                if (Feature.Available("Origin is same as Login Location").ToUpper() == "YES")
                {

                    textBox3.Text = funs.Select_dp_nm(Database.CompanyStation_id);
                }
                else
                {
                    textBox3.Text = dtAc1.Rows[0]["sourcepoint"].ToString();
                }
                if (textBox23.Text == "Consigner")
                {
                    Expenses(dtAc1.Rows[0]["ac_id"].ToString());// funs.Select_ac_id(textBox1.Text));
                }
                else if (textBox23.Text == "Consignee" && textBox2.Text != "")
                {
                    Expenses(funs.Select_ac_id(textBox2.Text));
                }
                if (Feature.Available("Details on Booking Acc to Consigner").ToUpper() == "YES")
                {

                    textBox25.Text = dtAc1.Rows[0]["Delivery_type"].ToString();// Database.GetScalarText("select Delivery_type from ACCOUNTs where name='" + textBox1.Text + "'");
                    textBox24.Text = dtAc1.Rows[0]["GR_type"].ToString();// Database.GetScalarText("select GR_type from ACCOUNTs where name='" + textBox1.Text + "'");

                    if (textBox24.Text == "FOC")
                    {
                        textBox29.Text = "Credit";
                        textBox29.Enabled = false;
                       
                    }
                    else if (textBox24.Text == "T.B.B.")
                    {
                        textBox29.Text = "Credit";
                        textBox29.Enabled = false;
                      
                    }
                    else if (textBox24.Text == "To Pay")
                    {
                        textBox29.Text = "Credit";
                        textBox29.Enabled = false;
                      
                    }
                    else
                    {
                        textBox29.Enabled = true;
                    }


                }
                else
                {
                    textBox25.Text = Database.GetScalarText("select Delivery_type from ACCOUNTs where name='" + textBox2.Text + "'");
                    textBox24.Text = Database.GetScalarText("select GR_type from ACCOUNTs where name='" + textBox2.Text + "'");
                    if (textBox24.Text == "FOC")
                    {
                        textBox29.Text = "Credit";
                        textBox29.Enabled = false;
                        //SendKeys.Send("{tab}");
                    }
                    else if (textBox24.Text == "T.B.B.")
                    {
                        textBox29.Text = "Credit";
                        textBox29.Enabled = false;
                        // SendKeys.Send("{tab}");
                    }
                    else if (textBox24.Text == "To Pay")
                    {
                        textBox29.Text = "Credit";
                        textBox29.Enabled = false;
                        //SendKeys.Send("{tab}");
                    }
                    else
                    {
                        textBox29.Enabled = true;
                    }
                }
            }
            DeliveryAdd();

            this.ActiveControl = textBox3;
        }

        private void DeliveryAdd()
        {
            if (textBox2.Text != "")
            {
                if (textBox25.Text == "Godown")
                {
                    textBox5.Text = textBox4.Text;
                }
                else if (textBox25.Text == "Door Delivery")
                {
                    textBox5.Text = Database.GetScalarText("select Address1 from ACCOUNTs where Name='" + textBox2.Text + "'");
                }

                else
                {
                    textBox5.Text = "";
                }
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            ////textBox2.Text = SelectCombo.ComboDt(this, Master.AccInfo, 2);
            //strCombo = "SELECT ACCOUNTs.Name, ACCOUNTs.Printname, DeliveryPoints.Name AS Station, ACCOUNTs.Address1, ACCOUNTs.Address2, ACCOUNTs.Phone, ACCOUNTs.Tin_number, OTHERs.Name AS Staff, CONTRACTORs.Name AS Agent FROM ACCOUNTs LEFT OUTER JOIN CONTRACTORs ON ACCOUNTs.Con_id = CONTRACTORs.Name LEFT OUTER JOIN OTHERs ON ACCOUNTs.Loc_id = OTHERs.Oth_id LEFT OUTER JOIN DeliveryPoints ON ACCOUNTs.SId = DeliveryPoints.DPId WHERE ACCOUNTs.Act_id = 39 ORDER BY ACCOUNTs.Name";
            //textBox2.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 2);
            ////label24.Text =  funs.Select_GST(textBox2.Text);
            ////textBox28.Text =  funs.Select_ac_nm(Database.GetScalarText("Select Transporter_id from Accounts where Name='" + textBox2.Text + "'"));

            //textBox2.Text = SelectCombo.ComboDt(this, e.KeyChar, clsCashing.GetAccounts(39), e.KeyChar.ToString(), 2);

            string strCombo = "SELECT ACCOUNTs.name, ACCOUNTs.Printname, DeliveryPoints.Name AS Station, ACCOUNTs.Address1, ACCOUNTs.Address2,  ACCOUNTs.phone, ACCOUNTs.tin_number, OTHERs.Name AS Staff, CONTRACTORs.Name AS Agent, ACCOUNTs.ac_id FROM ACCOUNTs LEFT OUTER JOIN  ACCOUNTYPEs ON ACCOUNTs.act_id = ACCOUNTYPEs.Act_id LEFT OUTER JOIN CONTRACTORs ON ACCOUNTs.con_id = CONTRACTORs.Name LEFT OUTER JOIN OTHERs ON ACCOUNTs.loc_id = OTHERs.Oth_id LEFT OUTER JOIN DeliveryPoints ON ACCOUNTs.SId = DeliveryPoints.DPId WHERE ( ACCOUNTYPEs.Path LIKE '1;39;%') ORDER BY ACCOUNTs.name ";

            //textBox1.Text = SelectCombo.ComboDt(this, Master.AccInfo, 2);
            // strCombo = "SELECT ACCOUNTs.Name, ACCOUNTs.Printname, DeliveryPoints.Name AS Station, ACCOUNTs.Address1, ACCOUNTs.Address2, ACCOUNTs.Phone, ACCOUNTs.Tin_number, OTHERs.Name AS Staff, CONTRACTORs.Name AS Agent FROM ACCOUNTs LEFT OUTER JOIN CONTRACTORs ON ACCOUNTs.Con_id = CONTRACTORs.Name LEFT OUTER JOIN OTHERs ON ACCOUNTs.Loc_id = OTHERs.Oth_id LEFT OUTER JOIN DeliveryPoints ON ACCOUNTs.SId = DeliveryPoints.DPId WHERE ACCOUNTs.Act_id = 39 ORDER BY ACCOUNTs.Name";
            //textBox1.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 2);
           // label23.Text = "";// funs.Select_GST(textBox1.Text);
            textBox2.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 2);

            DataTable dtAc2 = new DataTable();
            Database.GetSqlData("Select ac.ac_id,ac.Tin_number,ac.Delivery_type,ac.GR_type,trnp.name as transportName,dp.name as destination from accounts as ac left join accounts as trnp on ac.Transporter_id=trnp.ac_id left join DeliveryPoints as dp on ac.sid=dp.dpid where ac.name='" + textBox2.Text + "'", dtAc2);
            if (dtAc2.Rows.Count <= 0) { dtAc2.Rows.Add(); }

            label24.Text = dtAc2.Rows[0]["Tin_number"].ToString();
            textBox28.Text = dtAc2.Rows[0]["transportName"].ToString();

            if (textBox2.Text != "")
            {
                if (textBox1.Text != "" && textBox2.Text != "")
                {
                    if (textBox2.Text != textBox1.Text)
                    {
                        textBox4.Text = dtAc2.Rows[0]["destination"].ToString();
                    }
                }

                if (textBox23.Text == "Consigner")
                {
                    Expenses(funs.Select_ac_id(textBox1.Text));
                }
                else if (textBox23.Text == "Consignee")
                {
                    Expenses(dtAc2.Rows[0]["ac_id"].ToString());//funs.Select_ac_id(textBox2.Text));
                }

            }
            if (Feature.Available("Details on Booking Acc to Consigner").ToUpper() == "YES")
            {

                textBox25.Text = Database.GetScalarText("select Delivery_type from ACCOUNTs where name='" + textBox1.Text + "'");
                textBox24.Text = Database.GetScalarText("select GR_type from ACCOUNTs where name='" + textBox1.Text + "'");
            }
            else
            {
                textBox25.Text = dtAc2.Rows[0]["Delivery_type"].ToString();// Database.GetScalarText("select Delivery_type from ACCOUNTs where name='" + textBox2.Text + "'");
                textBox24.Text = dtAc2.Rows[0]["GR_type"].ToString();// Database.GetScalarText("select GR_type from ACCOUNTs where name='" + textBox2.Text + "'");
            }
            DeliveryAdd();
            this.ActiveControl = textBox4;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            //if (textBox1.Text != "" && textBox2.Text != "")
            //{
            //    if (textBox2.Text != textBox1.Text)
            //    {
            //        if (textBox2.Text != "")
            //        {
            //            DataTable dtStation = new DataTable("Station");
            //            Database.GetSqlData("select SId from ACCOUNTs where [name]='" + textBox2.Text + "'", dtStation);
            //            textBox4.Text = funs.Select_dp_nm(dtStation.Rows[0]["SId"].ToString());
            //        }
            //    }
            //}
            //if (textBox23.Text == "Consigner")
            //{
            //    Expenses(funs.Select_ac_id(textBox1.Text));
            //}
            //else if (textBox23.Text == "Consignee")
            //{
            //    Expenses(funs.Select_ac_id(textBox2.Text));
            //}
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //if (textBox1.Text != "")
            //{
            //    DataTable dtStation = new DataTable();
            //    Database.GetSqlData("select SId from ACCOUNTs where [name]='" + textBox1.Text + "'", dtStation);
            //    textBox3.Text = funs.Select_dp_nm(dtStation.Rows[0]["SId"].ToString());
            //}
            //if (textBox23.Text == "Consigner")
            //{
            //    Expenses(funs.Select_ac_id(textBox1.Text));
            //}
            //else if (textBox23.Text == "Consignee")
            //{
            //    Expenses(funs.Select_ac_id(textBox2.Text));
            //}
        }

        private void ansGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (ansGridView1.CurrentCell.OwningColumn.Name == "sno")
            {
                SendKeys.Send("{right}");
                this.Activate();
            }
            ansGridView1.Rows[e.RowIndex].Cells["sno"].Value = e.RowIndex + 1;
        }


        private void frmBooking_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.E)
            {
                if (ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["description"].Value != "")
                {
                    frm_other_expenses frm = new frm_other_expenses();
                    frm.LoadDate(ansGridView1, Convert.ToInt32(ansGridView1.CurrentCell.RowIndex));
                    frm.ShowDialog();
                    gridValues(frm.gdt, ansGridView1.CurrentCell.RowIndex);
                }
            }
            else if (e.Control && e.KeyCode == Keys.S)
            {
                if (validate() == true)
                {
                    try
                    {
                        Database.BeginTran();

                        if (Database.utype == "Admin" || gStr == "0")
                        {
                            save();
                        }
                        Database.CommitTran();
                    }
                    catch (Exception ex)
                    {
                        Database.RollbackTran();
                        MessageBox.Show("Not Saved Due to an Exception." + ex.Message);
                        this.Close();
                        this.Dispose();
                    }
                    SendSMS();

                    clear();
                }
            }

            else if (e.Control && e.KeyCode == Keys.P)
            {
                if (validate() == true)
                {

                    try
                    {
                        Database.BeginTran();
                        if (Database.utype == "Admin")
                        {
                            save();
                        }

                        else if (gStr == "0")
                        {
                            save();
                        }
                        Database.CommitTran();
                    }
                    catch (Exception ex)
                    {
                        Database.RollbackTran();
                        MessageBox.Show("Not Saved Due to an Exception." + ex.Message);
                        this.Close();
                        this.Dispose();
                    }
                    if (vid != "0")
                    {
                        Print();
                    }
                    SendSMS();
                    clear();
                }
            }

            else if (e.Control && e.KeyCode == Keys.W)
            {
                if (validate() == true)
                {
                    try
                    {
                        Database.BeginTran();
                        if (Database.utype == "Admin")
                        {
                            save();
                        }

                        else if (gStr == "0")
                        {
                            save();
                        }
                        Database.CommitTran();
                    }
                    catch (Exception ex)
                    {
                        Database.RollbackTran();
                        MessageBox.Show("Not Saved Due to an Exception." + ex.Message);
                        this.Close();
                        this.Dispose();
                    }
                    SendSMS();
                    view();
                    clear();
                }
            }

            else if (e.KeyCode == Keys.Escape)
            {
                if (textBox1.Text != "")
                {
                    DialogResult chk = MessageBox.Show("Are u sure?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                    if (chk == DialogResult.No)
                    {
                        e.Handled = false;
                    }
                    else
                    {
                        this.Dispose();
                    }
                }
                else
                {
                    this.Dispose();
                }

            }
            if (e.Control && e.KeyCode == Keys.F12)
            {
                if (Database.utype == "Admin")
                {
                    InputBox box = new InputBox("Enter Administrative password", "", true);
                    box.ShowDialog(this);
                    String pass = box.outStr;
                    if (pass.ToLower() == "admin")
                    {
                        box = new InputBox("Enter Voucher Number", "", false);
                        box.ShowDialog();
                        if (box.outStr == "")
                        {
                            vno = int.Parse(label10.Text);
                        }
                        else
                        {
                            vno = int.Parse(box.outStr);
                        }

                        label10.Text = vno.ToString();
                        int numtype = funs.chkNumType(vtid);
                        if (numtype != 1)
                        {
                            vid = Database.GetScalarText("Select Vi_id from voucherinfos where Vt_id=" + vtid + " and Vnumber=" + vno + " and Vdate=" + access_sql.Hash + dateTimePicker1.Value.Date.ToString(Database.dformat) + access_sql.Hash);
                            if (vid == "")
                            {
                                vid = "0";
                            }
                        }
                        else
                        {
                            string tempvid = "";
                            tempvid = Database.GetScalarText("Select Vi_id from voucherinfos where Vt_id=" + vtid + " and Vnumber=" + vno);
                            if (tempvid != "")
                            {
                                MessageBox.Show("Voucher can't be created on this No.");
                                vno = 0;
                                label10.Text = vno.ToString();
                                //SetVno();
                                return;
                            }
                        }
                        f12used = true;
                    }
                    else
                    {
                        MessageBox.Show("Invalid password");
                    }
                }
            }
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox1);
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox2);
        }

        private void textBox4_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox4);
        }

        private void textBox4_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox4);
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox2);
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox1);
        }

        private void dateTimePicker1_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void ansGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            double ttl_weight = 0;
            if (ansGridView1.CurrentCell.OwningColumn.Name == "Quantity")
            {
                if (ansGridView1.Rows[e.RowIndex].Cells["Quantity"].Value.ToString() == "")
                {
                    ansGridView1.Rows[e.RowIndex].Cells["Quantity"].Value = 0;
                }
                ansGridView1.Rows[e.RowIndex].Cells["weight"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[e.RowIndex].Cells["Quantity"].Value.ToString()) * double.Parse(ansGridView1.Rows[e.RowIndex].Cells["bharti"].Value.ToString()), 3);

                for (int i = 0; i < ansGridView1.Rows.Count - 1; i++)
                {
                    ttl_weight += double.Parse(ansGridView1.Rows[i].Cells["weight"].Value.ToString());
                }

                double charged_weight = Convert.ToDouble(ansGridView1.Rows[e.RowIndex].Cells["weight"].Value) + (Convert.ToDouble(ansGridView1.Rows[e.RowIndex].Cells["weight"].Value) * Convert.ToDouble(ansGridView1.Rows[e.RowIndex].Cells["charged_weight"].Value) / 100);
                if (ttl_weight > Convert.ToDouble(ansGridView1.Rows[e.RowIndex].Cells["miniweight"].Value))
                {
                    ansGridView1.Rows[e.RowIndex].Cells["ChargedWeight"].Value = funs.DecimalPoint(charged_weight, 3);
                }
                else
                {
                    ansGridView1.Rows[e.RowIndex].Cells["ChargedWeight"].Value = funs.DecimalPoint(Convert.ToDouble(ansGridView1.Rows[e.RowIndex].Cells["miniweight"].Value), 3);
                }
            }
            else if (ansGridView1.CurrentCell.OwningColumn.Name == "weight")
            {
                for (int i = 0; i < ansGridView1.Rows.Count - 1; i++)
                {
                    ttl_weight += double.Parse(ansGridView1.Rows[i].Cells["weight"].Value.ToString());
                }

                double charged_weight = Convert.ToDouble(ansGridView1.Rows[e.RowIndex].Cells["weight"].Value) + (Convert.ToDouble(ansGridView1.Rows[e.RowIndex].Cells["weight"].Value) * Convert.ToDouble(ansGridView1.Rows[e.RowIndex].Cells["charged_weight"].Value) / 100);
                if (ttl_weight > Convert.ToDouble(ansGridView1.Rows[e.RowIndex].Cells["miniweight"].Value))
                {
                    ansGridView1.Rows[e.RowIndex].Cells["ChargedWeight"].Value = funs.DecimalPoint(charged_weight, 3);
                }
                else
                {
                    ansGridView1.Rows[e.RowIndex].Cells["ChargedWeight"].Value = funs.DecimalPoint(Convert.ToDouble(ansGridView1.Rows[e.RowIndex].Cells["miniweight"].Value), 3);
                }
            }
            else if (ansGridView1.CurrentCell.OwningColumn.Name == "ChargedWeight")
            {
                ansGridView1.Rows[e.RowIndex].Cells["ChargedWeight"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[e.RowIndex].Cells["ChargedWeight"].Value.ToString()), 3);
            }

            else if (ansGridView1.CurrentCell.OwningColumn.Name == "multiplier")
            {
                if (ansGridView1.Rows[e.RowIndex].Cells["multiplier"].Value == null || double.Parse(ansGridView1.Rows[e.RowIndex].Cells["multiplier"].Value.ToString()) <= 0)
                {
                    ansGridView1.Rows[e.RowIndex].Cells["multiplier"].Value = 1;
                }
            }
            ansGridView1.Rows[e.RowIndex].Cells["ChargedWeight"].Value = RoundUp(Convert.ToDouble(ansGridView1.Rows[e.RowIndex].Cells["ChargedWeight"].Value), Convert.ToInt32(ansGridView1.Rows[e.RowIndex].Cells["rounding_ch"].Value));
            CalcAmount(e.RowIndex);
        }

        private void CalcAmount(int index)
        {
            double runningamt = 0, totexp = 0;
            if (ansGridView1.Rows[index].Cells["Rate_am"].Value == null)
            {
                ansGridView1.Rows[index].Cells["Rate_am"].Value = "0";
            }
            if (ansGridView1.Rows[index].Cells["Per"].Value.ToString() == "/Nug")
            {

                ansGridView1.Rows[index].Cells["Amount"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[index].Cells["Quantity"].Value.ToString()) * double.Parse(ansGridView1.Rows[index].Cells["Rate_am"].Value.ToString()) * double.Parse(ansGridView1.Rows[index].Cells["multiplier"].Value.ToString()), 2);

                if (Convert.ToDouble(ansGridView1.Rows[index].Cells["Amount"].Value) < Convert.ToDouble(ansGridView1.Rows[index].Cells["freightmr"].Value))
                {
                    ansGridView1.Rows[index].Cells["Amount"].Value = Convert.ToDouble(ansGridView1.Rows[index].Cells["freightmr"].Value);
                }
                runningamt += double.Parse(ansGridView1.Rows[index].Cells["Amount"].Value.ToString());
            }
            else if (ansGridView1.Rows[index].Cells["Per"].Value.ToString() == "/Weight")
            {
                if (ansGridView1.Rows[index].Cells["weight"].Value == null)
                {
                    ansGridView1.Rows[index].Cells["weight"].Value = "0";
                }
                ansGridView1.Rows[index].Cells["Amount"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[index].Cells["ChargedWeight"].Value.ToString()) * double.Parse(ansGridView1.Rows[index].Cells["Rate_am"].Value.ToString()), 2);

                if (Convert.ToDouble(ansGridView1.Rows[index].Cells["Amount"].Value) < Convert.ToDouble(ansGridView1.Rows[index].Cells["freightmr"].Value))
                {
                    ansGridView1.Rows[index].Cells["Amount"].Value = Convert.ToDouble(ansGridView1.Rows[index].Cells["freightmr"].Value);
                }
                runningamt += double.Parse(ansGridView1.Rows[index].Cells["Amount"].Value.ToString());
            }
            else if (ansGridView1.Rows[index].Cells["Per"].Value.ToString() == "Flat")
            {
                ansGridView1.Rows[index].Cells["Amount"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[index].Cells["Rate_am"].Value.ToString()), 2);

                if (Convert.ToDouble(ansGridView1.Rows[index].Cells["Amount"].Value) < Convert.ToDouble(ansGridView1.Rows[index].Cells["freightmr"].Value))
                {
                    ansGridView1.Rows[index].Cells["Amount"].Value = Convert.ToDouble(ansGridView1.Rows[index].Cells["freightmr"].Value);
                }
                runningamt += double.Parse(ansGridView1.Rows[index].Cells["Amount"].Value.ToString());
            }

            //Exp1 Calc           

            if (index == 0)
            {
                if (ansGridView1.Rows[index].Cells["exp1type"].Value.ToString() == "/Nug")
                {
                    if (ansGridView1.Rows[index].Cells["exp1rate"].Value == null)
                    {
                        ansGridView1.Rows[index].Cells["exp1rate"].Value = "0";
                    }
                    ansGridView1.Rows[index].Cells["exp1amt"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[index].Cells["Quantity"].Value.ToString()) * double.Parse(ansGridView1.Rows[index].Cells["exp1rate"].Value.ToString()), 2);
                }
                else if (ansGridView1.Rows[index].Cells["exp1type"].Value.ToString() == "/Weight")
                {
                    if (ansGridView1.Rows[index].Cells["weight"].Value == null)
                    {
                        ansGridView1.Rows[index].Cells["weight"].Value = "0";
                    }
                    ansGridView1.Rows[index].Cells["exp1amt"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[index].Cells["ChargedWeight"].Value.ToString()) * double.Parse(ansGridView1.Rows[index].Cells["exp1rate"].Value.ToString()), 2);
                }
                else if (ansGridView1.Rows[index].Cells["exp1type"].Value.ToString() == "Flat")
                {
                    if (ansGridView1.Rows[index].Cells["weight"].Value == null)
                    {
                        ansGridView1.Rows[index].Cells["weight"].Value = "0";
                    }
                    ansGridView1.Rows[index].Cells["exp1amt"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[index].Cells["exp1rate"].Value.ToString()), 2);
                }
                else if (ansGridView1.Rows[index].Cells["exp1type"].Value.ToString() == "% of Freight")
                {
                    ansGridView1.Rows[index].Cells["exp1amt"].Value = funs.DecimalPoint((double.Parse(ansGridView1.Rows[index].Cells["Amount"].Value.ToString()) * double.Parse(ansGridView1.Rows[index].Cells["exp1rate"].Value.ToString())) / 100, 2);
                }
                else if (ansGridView1.Rows[index].Cells["exp1type"].Value.ToString() == "% of Expenses")
                {
                    ansGridView1.Rows[index].Cells["exp1amt"].Value = funs.DecimalPoint((runningamt * double.Parse(ansGridView1.Rows[index].Cells["exp1rate"].Value.ToString())) / 100, 2);
                }

                if (double.Parse(ansGridView1.Rows[index].Cells["exp1amt"].Value.ToString()) < double.Parse(ansGridView1.Rows[index].Cells["exp1mr"].Value.ToString()))
                {
                    ansGridView1.Rows[index].Cells["exp1amt"].Value = double.Parse(ansGridView1.Rows[index].Cells["exp1mr"].Value.ToString());
                    ansGridView1.Rows[index].Cells["exp1amt"].Value = RoundUp(Convert.ToDouble(ansGridView1.Rows[index].Cells["exp1amt"].Value), Convert.ToInt32(ansGridView1.Rows[index].Cells["rounding_ex"].Value));
                    totexp += double.Parse(ansGridView1.Rows[index].Cells["exp1amt"].Value.ToString());
                    runningamt += double.Parse(ansGridView1.Rows[index].Cells["exp1amt"].Value.ToString());
                }
                else
                {
                    ansGridView1.Rows[index].Cells["exp1amt"].Value = RoundUp(Convert.ToDouble(ansGridView1.Rows[index].Cells["exp1amt"].Value), Convert.ToInt32(ansGridView1.Rows[index].Cells["rounding_ex"].Value));
                    totexp += double.Parse(ansGridView1.Rows[index].Cells["exp1amt"].Value.ToString());
                    runningamt += double.Parse(ansGridView1.Rows[index].Cells["exp1amt"].Value.ToString());
                }

                if (ansGridView1.Rows[index].Cells["exp11type"].Value.ToString() == "/Nug")
                {
                    if (ansGridView1.Rows[index].Cells["exp11rate"].Value == null)
                    {
                        ansGridView1.Rows[index].Cells["exp11rate"].Value = "0";
                    }
                    ansGridView1.Rows[index].Cells["exp11amt"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[index].Cells["Quantity"].Value.ToString()) * double.Parse(ansGridView1.Rows[index].Cells["exp11rate"].Value.ToString()), 2);
                }
                else if (ansGridView1.Rows[index].Cells["exp11type"].Value.ToString() == "/Weight")
                {
                    if (ansGridView1.Rows[index].Cells["weight"].Value == null)
                    {
                        ansGridView1.Rows[index].Cells["weight"].Value = "0";
                    }
                    ansGridView1.Rows[index].Cells["exp11amt"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[index].Cells["ChargedWeight"].Value.ToString()) * double.Parse(ansGridView1.Rows[index].Cells["exp11rate"].Value.ToString()), 2);
                }
                else if (ansGridView1.Rows[index].Cells["exp11type"].Value.ToString() == "Flat")
                {
                    if (ansGridView1.Rows[index].Cells["weight"].Value == null)
                    {
                        ansGridView1.Rows[index].Cells["weight"].Value = "0";
                    }
                    ansGridView1.Rows[index].Cells["exp11amt"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[index].Cells["exp11rate"].Value.ToString()), 2);
                }
                else if (ansGridView1.Rows[index].Cells["exp11type"].Value.ToString() == "% of Freight")
                {
                    ansGridView1.Rows[index].Cells["exp11amt"].Value = funs.DecimalPoint((double.Parse(ansGridView1.Rows[index].Cells["Amount"].Value.ToString()) * double.Parse(ansGridView1.Rows[index].Cells["exp11rate"].Value.ToString())) / 100, 2);
                }
                else if (ansGridView1.Rows[index].Cells["exp11type"].Value.ToString() == "% of Expenses")
                {
                    ansGridView1.Rows[index].Cells["exp11amt"].Value = funs.DecimalPoint((runningamt * double.Parse(ansGridView1.Rows[index].Cells["exp11rate"].Value.ToString())) / 100, 2);
                }

                if (double.Parse(ansGridView1.Rows[index].Cells["exp11amt"].Value.ToString()) < double.Parse(ansGridView1.Rows[index].Cells["exp11mr"].Value.ToString()))
                {
                    ansGridView1.Rows[index].Cells["exp11amt"].Value = double.Parse(ansGridView1.Rows[index].Cells["exp11mr"].Value.ToString());
                    ansGridView1.Rows[index].Cells["exp11amt"].Value = RoundUp(Convert.ToDouble(ansGridView1.Rows[index].Cells["exp11amt"].Value), Convert.ToInt32(ansGridView1.Rows[index].Cells["rounding_ex"].Value));
                    totexp += double.Parse(ansGridView1.Rows[index].Cells["exp11amt"].Value.ToString());
                    runningamt += double.Parse(ansGridView1.Rows[index].Cells["exp11amt"].Value.ToString());
                }
                else
                {
                    ansGridView1.Rows[index].Cells["exp11amt"].Value = RoundUp(Convert.ToDouble(ansGridView1.Rows[index].Cells["exp11amt"].Value), Convert.ToInt32(ansGridView1.Rows[index].Cells["rounding_ex"].Value));
                    totexp += double.Parse(ansGridView1.Rows[index].Cells["exp11amt"].Value.ToString());
                    runningamt += double.Parse(ansGridView1.Rows[index].Cells["exp11amt"].Value.ToString());
                }

            }



            //Exp2 Calc
            if (ansGridView1.Rows[index].Cells["exp2type"].Value.ToString() == "/Nug")
            {
                if (ansGridView1.Rows[index].Cells["exp2rate"].Value == null)
                {
                    ansGridView1.Rows[index].Cells["exp2rate"].Value = "0";
                }
                ansGridView1.Rows[index].Cells["exp2amt"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[index].Cells["Quantity"].Value.ToString()) * double.Parse(ansGridView1.Rows[index].Cells["exp2rate"].Value.ToString()), 2);
            }
            else if (ansGridView1.Rows[index].Cells["exp2type"].Value.ToString() == "/Weight")
            {
                if (ansGridView1.Rows[index].Cells["weight"].Value == null)
                {
                    ansGridView1.Rows[index].Cells["weight"].Value = "0";
                }
                ansGridView1.Rows[index].Cells["exp2amt"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[index].Cells["ChargedWeight"].Value.ToString()) * double.Parse(ansGridView1.Rows[index].Cells["exp2rate"].Value.ToString()), 2);
            }
            else if (ansGridView1.Rows[index].Cells["exp2type"].Value.ToString() == "Flat")
            {
                if (ansGridView1.Rows[index].Cells["weight"].Value == null)
                {
                    ansGridView1.Rows[index].Cells["weight"].Value = "0";
                }
                ansGridView1.Rows[index].Cells["exp2amt"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[index].Cells["exp2rate"].Value.ToString()), 2);
            }
            else if (ansGridView1.Rows[index].Cells["exp2type"].Value.ToString() == "% of Freight")
            {
                ansGridView1.Rows[index].Cells["exp2amt"].Value = funs.DecimalPoint((double.Parse(ansGridView1.Rows[index].Cells["Amount"].Value.ToString()) * double.Parse(ansGridView1.Rows[index].Cells["exp2rate"].Value.ToString())) / 100, 2);
            }
            else if (ansGridView1.Rows[index].Cells["exp2type"].Value.ToString() == "% of Expenses")
            {
                ansGridView1.Rows[index].Cells["exp2amt"].Value = funs.DecimalPoint((runningamt * double.Parse(ansGridView1.Rows[index].Cells["exp2rate"].Value.ToString())) / 100, 2);
            }

            if (double.Parse(ansGridView1.Rows[index].Cells["exp2amt"].Value.ToString()) < double.Parse(ansGridView1.Rows[index].Cells["exp2mr"].Value.ToString()))
            {
                ansGridView1.Rows[index].Cells["exp2amt"].Value = double.Parse(ansGridView1.Rows[index].Cells["exp2mr"].Value.ToString());
                ansGridView1.Rows[index].Cells["exp2amt"].Value = RoundUp(Convert.ToDouble(ansGridView1.Rows[index].Cells["exp2amt"].Value), Convert.ToInt32(ansGridView1.Rows[index].Cells["rounding_ex"].Value));
                totexp += double.Parse(ansGridView1.Rows[index].Cells["exp2amt"].Value.ToString());
                runningamt += double.Parse(ansGridView1.Rows[index].Cells["exp2amt"].Value.ToString());
            }
            else
            {
                ansGridView1.Rows[index].Cells["exp2amt"].Value = RoundUp(Convert.ToDouble(ansGridView1.Rows[index].Cells["exp2amt"].Value), Convert.ToInt32(ansGridView1.Rows[index].Cells["rounding_ex"].Value));
                totexp += double.Parse(ansGridView1.Rows[index].Cells["exp2amt"].Value.ToString());
                runningamt += double.Parse(ansGridView1.Rows[index].Cells["exp2amt"].Value.ToString());
            }

            //Exp3 Calc
            if (ansGridView1.Rows[index].Cells["exp3type"].Value.ToString() == "/Nug")
            {
                if (ansGridView1.Rows[index].Cells["exp3rate"].Value == null)
                {
                    ansGridView1.Rows[index].Cells["exp3rate"].Value = "0";
                }
                ansGridView1.Rows[index].Cells["exp3amt"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[index].Cells["Quantity"].Value.ToString()) * double.Parse(ansGridView1.Rows[index].Cells["exp3rate"].Value.ToString()), 2);
            }
            else if (ansGridView1.Rows[index].Cells["exp3type"].Value.ToString() == "/Weight")
            {
                if (ansGridView1.Rows[index].Cells["weight"].Value == null)
                {
                    ansGridView1.Rows[index].Cells["weight"].Value = "0";
                }
                ansGridView1.Rows[index].Cells["exp3amt"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[index].Cells["ChargedWeight"].Value.ToString()) * double.Parse(ansGridView1.Rows[index].Cells["exp3rate"].Value.ToString()), 2);
            }
            else if (ansGridView1.Rows[index].Cells["exp3type"].Value.ToString() == "Flat")
            {
                if (ansGridView1.Rows[index].Cells["weight"].Value == null)
                {
                    ansGridView1.Rows[index].Cells["weight"].Value = "0";
                }
                ansGridView1.Rows[index].Cells["exp3amt"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[index].Cells["exp3rate"].Value.ToString()), 2);
            }
            else if (ansGridView1.Rows[index].Cells["exp3type"].Value.ToString() == "% of Freight")
            {
                ansGridView1.Rows[index].Cells["exp3amt"].Value = funs.DecimalPoint((double.Parse(ansGridView1.Rows[index].Cells["Amount"].Value.ToString()) * double.Parse(ansGridView1.Rows[index].Cells["exp3rate"].Value.ToString())) / 100, 2);
            }
            else if (ansGridView1.Rows[index].Cells["exp3type"].Value.ToString() == "% of Expenses")
            {
                ansGridView1.Rows[index].Cells["exp3amt"].Value = funs.DecimalPoint((runningamt * double.Parse(ansGridView1.Rows[index].Cells["exp3rate"].Value.ToString())) / 100, 2);
            }

            if (double.Parse(ansGridView1.Rows[index].Cells["exp3amt"].Value.ToString()) < double.Parse(ansGridView1.Rows[index].Cells["exp3mr"].Value.ToString()))
            {
                ansGridView1.Rows[index].Cells["exp3amt"].Value = double.Parse(ansGridView1.Rows[index].Cells["exp3mr"].Value.ToString());
                ansGridView1.Rows[index].Cells["exp3amt"].Value = RoundUp(Convert.ToDouble(ansGridView1.Rows[index].Cells["exp3amt"].Value), Convert.ToInt32(ansGridView1.Rows[index].Cells["rounding_ex"].Value));
                totexp += double.Parse(ansGridView1.Rows[index].Cells["exp3amt"].Value.ToString());
                runningamt += double.Parse(ansGridView1.Rows[index].Cells["exp3amt"].Value.ToString());
            }
            else
            {
                ansGridView1.Rows[index].Cells["exp3amt"].Value = RoundUp(Convert.ToDouble(ansGridView1.Rows[index].Cells["exp3amt"].Value), Convert.ToInt32(ansGridView1.Rows[index].Cells["rounding_ex"].Value));
                totexp += double.Parse(ansGridView1.Rows[index].Cells["exp3amt"].Value.ToString());
                runningamt += double.Parse(ansGridView1.Rows[index].Cells["exp3amt"].Value.ToString());
            }


            //Exp4 Calc

            if (ansGridView1.Rows[index].Cells["exp4type"].Value.ToString() == "/Nug")
            {
                if (ansGridView1.Rows[index].Cells["exp4rate"].Value == null)
                {
                    ansGridView1.Rows[index].Cells["exp4rate"].Value = "0";
                }
                ansGridView1.Rows[index].Cells["exp4amt"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[index].Cells["Quantity"].Value.ToString()) * double.Parse(ansGridView1.Rows[index].Cells["exp4rate"].Value.ToString()), 2);
            }
            else if (ansGridView1.Rows[index].Cells["exp4type"].Value.ToString() == "/Weight")
            {
                if (ansGridView1.Rows[index].Cells["weight"].Value == null)
                {
                    ansGridView1.Rows[index].Cells["weight"].Value = "0";
                }
                ansGridView1.Rows[index].Cells["exp4amt"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[index].Cells["ChargedWeight"].Value.ToString()) * double.Parse(ansGridView1.Rows[index].Cells["exp4rate"].Value.ToString()), 2);
            }
            else if (ansGridView1.Rows[index].Cells["exp4type"].Value.ToString() == "Flat")
            {
                if (ansGridView1.Rows[index].Cells["weight"].Value == null)
                {
                    ansGridView1.Rows[index].Cells["weight"].Value = "0";
                }
                ansGridView1.Rows[index].Cells["exp4amt"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[index].Cells["exp4rate"].Value.ToString()), 2);
            }
            else if (ansGridView1.Rows[index].Cells["exp4type"].Value.ToString() == "% of Freight")
            {
                ansGridView1.Rows[index].Cells["exp4amt"].Value = funs.DecimalPoint((double.Parse(ansGridView1.Rows[index].Cells["Amount"].Value.ToString()) * double.Parse(ansGridView1.Rows[index].Cells["exp4rate"].Value.ToString())) / 100, 2);
            }
            else if (ansGridView1.Rows[index].Cells["exp4type"].Value.ToString() == "% of Expenses")
            {
                ansGridView1.Rows[index].Cells["exp4amt"].Value = funs.DecimalPoint((runningamt * double.Parse(ansGridView1.Rows[index].Cells["exp4rate"].Value.ToString())) / 100, 2);
            }

            if (double.Parse(ansGridView1.Rows[index].Cells["exp4amt"].Value.ToString()) < double.Parse(ansGridView1.Rows[index].Cells["exp4mr"].Value.ToString()))
            {
                ansGridView1.Rows[index].Cells["exp4amt"].Value = double.Parse(ansGridView1.Rows[index].Cells["exp4mr"].Value.ToString());
                ansGridView1.Rows[index].Cells["exp4amt"].Value = RoundUp(Convert.ToDouble(ansGridView1.Rows[index].Cells["exp4amt"].Value), Convert.ToInt32(ansGridView1.Rows[index].Cells["rounding_ex"].Value));
                totexp += double.Parse(ansGridView1.Rows[index].Cells["exp4amt"].Value.ToString());
                runningamt += double.Parse(ansGridView1.Rows[index].Cells["exp4amt"].Value.ToString());
            }
            else
            {
                ansGridView1.Rows[index].Cells["exp4amt"].Value = RoundUp(Convert.ToDouble(ansGridView1.Rows[index].Cells["exp4amt"].Value), Convert.ToInt32(ansGridView1.Rows[index].Cells["rounding_ex"].Value));
                totexp += double.Parse(ansGridView1.Rows[index].Cells["exp4amt"].Value.ToString());
                runningamt += double.Parse(ansGridView1.Rows[index].Cells["exp4amt"].Value.ToString());
            }

            //Exp5 Calc

            if (ansGridView1.Rows[index].Cells["exp5type"].Value.ToString() == "/Nug")
            {
                if (ansGridView1.Rows[index].Cells["exp5rate"].Value == null)
                {
                    ansGridView1.Rows[index].Cells["exp5rate"].Value = "0";
                }
                ansGridView1.Rows[index].Cells["exp5amt"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[index].Cells["Quantity"].Value.ToString()) * double.Parse(ansGridView1.Rows[index].Cells["exp5rate"].Value.ToString()), 2);
            }
            else if (ansGridView1.Rows[index].Cells["exp5type"].Value.ToString() == "/Weight")
            {
                if (ansGridView1.Rows[index].Cells["weight"].Value == null)
                {
                    ansGridView1.Rows[index].Cells["weight"].Value = "0";
                }
                ansGridView1.Rows[index].Cells["exp5amt"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[index].Cells["ChargedWeight"].Value.ToString()) * double.Parse(ansGridView1.Rows[index].Cells["exp5rate"].Value.ToString()), 2);
            }
            else if (ansGridView1.Rows[index].Cells["exp5type"].Value.ToString() == "Flat")
            {
                if (ansGridView1.Rows[index].Cells["weight"].Value == null)
                {
                    ansGridView1.Rows[index].Cells["weight"].Value = "0";
                }
                ansGridView1.Rows[index].Cells["exp5amt"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[index].Cells["exp5rate"].Value.ToString()), 2);
            }
            else if (ansGridView1.Rows[index].Cells["exp5type"].Value.ToString() == "% of Freight")
            {
                ansGridView1.Rows[index].Cells["exp5amt"].Value = funs.DecimalPoint((double.Parse(ansGridView1.Rows[index].Cells["Amount"].Value.ToString()) * double.Parse(ansGridView1.Rows[index].Cells["exp5rate"].Value.ToString())) / 100, 2);
            }
            else if (ansGridView1.Rows[index].Cells["exp5type"].Value.ToString() == "% of Expenses")
            {
                ansGridView1.Rows[index].Cells["exp5amt"].Value = funs.DecimalPoint((runningamt * double.Parse(ansGridView1.Rows[index].Cells["exp5rate"].Value.ToString())) / 100, 2);
            }

            if (double.Parse(ansGridView1.Rows[index].Cells["exp5amt"].Value.ToString()) < double.Parse(ansGridView1.Rows[index].Cells["exp5mr"].Value.ToString()))
            {
                ansGridView1.Rows[index].Cells["exp5amt"].Value = double.Parse(ansGridView1.Rows[index].Cells["exp5mr"].Value.ToString());
                ansGridView1.Rows[index].Cells["exp5amt"].Value = RoundUp(Convert.ToDouble(ansGridView1.Rows[index].Cells["exp5amt"].Value), Convert.ToInt32(ansGridView1.Rows[index].Cells["rounding_ex"].Value));
                totexp += double.Parse(ansGridView1.Rows[index].Cells["exp5amt"].Value.ToString());
                runningamt += double.Parse(ansGridView1.Rows[index].Cells["exp5amt"].Value.ToString());
            }
            else
            {
                ansGridView1.Rows[index].Cells["exp5amt"].Value = RoundUp(Convert.ToDouble(ansGridView1.Rows[index].Cells["exp5amt"].Value), Convert.ToInt32(ansGridView1.Rows[index].Cells["rounding_ex"].Value));
                totexp += double.Parse(ansGridView1.Rows[index].Cells["exp5amt"].Value.ToString());
                runningamt += double.Parse(ansGridView1.Rows[index].Cells["exp5amt"].Value.ToString());
            }

            //Exp6 Calc

            if (ansGridView1.Rows[index].Cells["exp6type"].Value.ToString() == "/Nug")
            {
                if (ansGridView1.Rows[index].Cells["exp6rate"].Value == null)
                {
                    ansGridView1.Rows[index].Cells["exp6rate"].Value = "0";
                }
                ansGridView1.Rows[index].Cells["exp6amt"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[index].Cells["Quantity"].Value.ToString()) * double.Parse(ansGridView1.Rows[index].Cells["exp6rate"].Value.ToString()), 2);
            }
            else if (ansGridView1.Rows[index].Cells["exp6type"].Value.ToString() == "/Weight")
            {
                if (ansGridView1.Rows[index].Cells["weight"].Value == null)
                {
                    ansGridView1.Rows[index].Cells["weight"].Value = "0";
                }
                ansGridView1.Rows[index].Cells["exp6amt"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[index].Cells["ChargedWeight"].Value.ToString()) * double.Parse(ansGridView1.Rows[index].Cells["exp6rate"].Value.ToString()), 2);
            }
            else if (ansGridView1.Rows[index].Cells["exp6type"].Value.ToString() == "Flat")
            {
                if (ansGridView1.Rows[index].Cells["weight"].Value == null)
                {
                    ansGridView1.Rows[index].Cells["weight"].Value = "0";
                }
                ansGridView1.Rows[index].Cells["exp6amt"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[index].Cells["exp6rate"].Value.ToString()), 2);
            }
            else if (ansGridView1.Rows[index].Cells["exp6type"].Value.ToString() == "% of Freight")
            {
                ansGridView1.Rows[index].Cells["exp6amt"].Value = funs.DecimalPoint((double.Parse(ansGridView1.Rows[index].Cells["Amount"].Value.ToString()) * double.Parse(ansGridView1.Rows[index].Cells["exp6rate"].Value.ToString())) / 100, 2);
            }
            else if (ansGridView1.Rows[index].Cells["exp6type"].Value.ToString() == "% of Expenses")
            {
                ansGridView1.Rows[index].Cells["exp6amt"].Value = funs.DecimalPoint((runningamt * double.Parse(ansGridView1.Rows[index].Cells["exp6rate"].Value.ToString())) / 100, 2);
            }

            if (double.Parse(ansGridView1.Rows[index].Cells["exp6amt"].Value.ToString()) < double.Parse(ansGridView1.Rows[index].Cells["exp6mr"].Value.ToString()))
            {
                ansGridView1.Rows[index].Cells["exp6amt"].Value = double.Parse(ansGridView1.Rows[index].Cells["exp6mr"].Value.ToString());
                ansGridView1.Rows[index].Cells["exp6amt"].Value = RoundUp(Convert.ToDouble(ansGridView1.Rows[index].Cells["exp6amt"].Value), Convert.ToInt32(ansGridView1.Rows[index].Cells["rounding_ex"].Value));
                totexp += double.Parse(ansGridView1.Rows[index].Cells["exp6amt"].Value.ToString());
                runningamt += double.Parse(ansGridView1.Rows[index].Cells["exp6amt"].Value.ToString());
            }
            else
            {
                ansGridView1.Rows[index].Cells["exp6amt"].Value = RoundUp(Convert.ToDouble(ansGridView1.Rows[index].Cells["exp6amt"].Value), Convert.ToInt32(ansGridView1.Rows[index].Cells["rounding_ex"].Value));
                totexp += double.Parse(ansGridView1.Rows[index].Cells["exp6amt"].Value.ToString());
                runningamt += double.Parse(ansGridView1.Rows[index].Cells["exp6amt"].Value.ToString());
            }

            //Exp7 Calc

            if (ansGridView1.Rows[index].Cells["exp7type"].Value.ToString() == "/Nug")
            {
                if (ansGridView1.Rows[index].Cells["exp7rate"].Value == null)
                {
                    ansGridView1.Rows[index].Cells["exp7rate"].Value = "0";
                }
                ansGridView1.Rows[index].Cells["exp7amt"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[index].Cells["Quantity"].Value.ToString()) * double.Parse(ansGridView1.Rows[index].Cells["exp7rate"].Value.ToString()), 2);
            }
            else if (ansGridView1.Rows[index].Cells["exp7type"].Value.ToString() == "/Weight")
            {
                if (ansGridView1.Rows[index].Cells["weight"].Value == null)
                {
                    ansGridView1.Rows[index].Cells["weight"].Value = "0";
                }
                ansGridView1.Rows[index].Cells["exp7amt"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[index].Cells["ChargedWeight"].Value.ToString()) * double.Parse(ansGridView1.Rows[index].Cells["exp7rate"].Value.ToString()), 2);
            }
            else if (ansGridView1.Rows[index].Cells["exp7type"].Value.ToString() == "Flat")
            {
                if (ansGridView1.Rows[index].Cells["weight"].Value == null)
                {
                    ansGridView1.Rows[index].Cells["weight"].Value = "0";
                }
                ansGridView1.Rows[index].Cells["exp7amt"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[index].Cells["exp7rate"].Value.ToString()), 2);
            }
            else if (ansGridView1.Rows[index].Cells["exp7type"].Value.ToString() == "% of Freight")
            {
                ansGridView1.Rows[index].Cells["exp7amt"].Value = funs.DecimalPoint((double.Parse(ansGridView1.Rows[index].Cells["Amount"].Value.ToString()) * double.Parse(ansGridView1.Rows[index].Cells["exp7rate"].Value.ToString())) / 100, 2);
            }
            else if (ansGridView1.Rows[index].Cells["exp7type"].Value.ToString() == "% of Expenses")
            {
                ansGridView1.Rows[index].Cells["exp7amt"].Value = funs.DecimalPoint((runningamt * double.Parse(ansGridView1.Rows[index].Cells["exp7rate"].Value.ToString())) / 100, 2);
            }

            if (double.Parse(ansGridView1.Rows[index].Cells["exp7amt"].Value.ToString()) < double.Parse(ansGridView1.Rows[index].Cells["exp7mr"].Value.ToString()))
            {
                ansGridView1.Rows[index].Cells["exp7amt"].Value = double.Parse(ansGridView1.Rows[index].Cells["exp7mr"].Value.ToString());
                ansGridView1.Rows[index].Cells["exp7amt"].Value = RoundUp(Convert.ToDouble(ansGridView1.Rows[index].Cells["exp7amt"].Value), Convert.ToInt32(ansGridView1.Rows[index].Cells["rounding_ex"].Value));
                totexp += double.Parse(ansGridView1.Rows[index].Cells["exp7amt"].Value.ToString());
                runningamt += double.Parse(ansGridView1.Rows[index].Cells["exp7amt"].Value.ToString());
            }
            else
            {
                ansGridView1.Rows[index].Cells["exp7amt"].Value = RoundUp(Convert.ToDouble(ansGridView1.Rows[index].Cells["exp7amt"].Value), Convert.ToInt32(ansGridView1.Rows[index].Cells["rounding_ex"].Value));
                totexp += double.Parse(ansGridView1.Rows[index].Cells["exp7amt"].Value.ToString());
                runningamt += double.Parse(ansGridView1.Rows[index].Cells["exp7amt"].Value.ToString());
            }

            //Exp8 Calc
            if (ansGridView1.Rows[index].Cells["exp8type"].Value.ToString() == "/Nug")
            {
                if (ansGridView1.Rows[index].Cells["exp8rate"].Value == null)
                {
                    ansGridView1.Rows[index].Cells["exp8rate"].Value = "0";
                }
                ansGridView1.Rows[index].Cells["exp8amt"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[index].Cells["Quantity"].Value.ToString()) * double.Parse(ansGridView1.Rows[index].Cells["exp8rate"].Value.ToString()), 2);
            }
            else if (ansGridView1.Rows[index].Cells["exp8type"].Value.ToString() == "/Weight")
            {
                if (ansGridView1.Rows[index].Cells["weight"].Value == null)
                {
                    ansGridView1.Rows[index].Cells["weight"].Value = "0";
                }
                ansGridView1.Rows[index].Cells["exp8amt"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[index].Cells["ChargedWeight"].Value.ToString()) * double.Parse(ansGridView1.Rows[index].Cells["exp8rate"].Value.ToString()), 2);
            }
            else if (ansGridView1.Rows[index].Cells["exp8type"].Value.ToString() == "Flat")
            {
                if (ansGridView1.Rows[index].Cells["weight"].Value == null)
                {
                    ansGridView1.Rows[index].Cells["weight"].Value = "0";
                }
                ansGridView1.Rows[index].Cells["exp8amt"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[index].Cells["exp8rate"].Value.ToString()), 2);
            }
            else if (ansGridView1.Rows[index].Cells["exp8type"].Value.ToString() == "% of Freight")
            {
                ansGridView1.Rows[index].Cells["exp8amt"].Value = funs.DecimalPoint((double.Parse(ansGridView1.Rows[index].Cells["Amount"].Value.ToString()) * double.Parse(ansGridView1.Rows[index].Cells["exp8rate"].Value.ToString())) / 100, 2);
            }
            else if (ansGridView1.Rows[index].Cells["exp8type"].Value.ToString() == "% of Expenses")
            {
                ansGridView1.Rows[index].Cells["exp8amt"].Value = funs.DecimalPoint((runningamt * double.Parse(ansGridView1.Rows[index].Cells["exp8rate"].Value.ToString())) / 100, 2);
            }

            if (double.Parse(ansGridView1.Rows[index].Cells["exp8amt"].Value.ToString()) < double.Parse(ansGridView1.Rows[index].Cells["exp8mr"].Value.ToString()))
            {
                ansGridView1.Rows[index].Cells["exp8amt"].Value = double.Parse(ansGridView1.Rows[index].Cells["exp8mr"].Value.ToString());
                ansGridView1.Rows[index].Cells["exp8amt"].Value = RoundUp(Convert.ToDouble(ansGridView1.Rows[index].Cells["exp8amt"].Value), Convert.ToInt32(ansGridView1.Rows[index].Cells["rounding_ex"].Value));
                totexp += double.Parse(ansGridView1.Rows[index].Cells["exp8amt"].Value.ToString());
                runningamt += double.Parse(ansGridView1.Rows[index].Cells["exp8amt"].Value.ToString());
            }
            else
            {
                ansGridView1.Rows[index].Cells["exp8amt"].Value = RoundUp(Convert.ToDouble(ansGridView1.Rows[index].Cells["exp8amt"].Value), Convert.ToInt32(ansGridView1.Rows[index].Cells["rounding_ex"].Value));
                totexp += double.Parse(ansGridView1.Rows[index].Cells["exp8amt"].Value.ToString());
                runningamt += double.Parse(ansGridView1.Rows[index].Cells["exp8amt"].Value.ToString());
            }

            //Exp9 Calc

            if (ansGridView1.Rows[index].Cells["exp9type"].Value.ToString() == "/Nug")
            {
                if (ansGridView1.Rows[index].Cells["exp9rate"].Value == null)
                {
                    ansGridView1.Rows[index].Cells["exp9rate"].Value = "0";
                }
                ansGridView1.Rows[index].Cells["exp9amt"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[index].Cells["Quantity"].Value.ToString()) * double.Parse(ansGridView1.Rows[index].Cells["exp9rate"].Value.ToString()), 2);
            }
            else if (ansGridView1.Rows[index].Cells["exp9type"].Value.ToString() == "/Weight")
            {
                if (ansGridView1.Rows[index].Cells["weight"].Value == null)
                {
                    ansGridView1.Rows[index].Cells["weight"].Value = "0";
                }
                ansGridView1.Rows[index].Cells["exp9amt"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[index].Cells["ChargedWeight"].Value.ToString()) * double.Parse(ansGridView1.Rows[index].Cells["exp9rate"].Value.ToString()), 2);
            }
            else if (ansGridView1.Rows[index].Cells["exp9type"].Value.ToString() == "Flat")
            {
                if (ansGridView1.Rows[index].Cells["weight"].Value == null)
                {
                    ansGridView1.Rows[index].Cells["weight"].Value = "0";
                }
                ansGridView1.Rows[index].Cells["exp9amt"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[index].Cells["exp9rate"].Value.ToString()), 2);
            }
            else if (ansGridView1.Rows[index].Cells["exp9type"].Value.ToString() == "% of Freight")
            {
                ansGridView1.Rows[index].Cells["exp9amt"].Value = funs.DecimalPoint((double.Parse(ansGridView1.Rows[index].Cells["Amount"].Value.ToString()) * double.Parse(ansGridView1.Rows[index].Cells["exp9rate"].Value.ToString())) / 100, 2);
            }
            else if (ansGridView1.Rows[index].Cells["exp9type"].Value.ToString() == "% of Expenses")
            {
                ansGridView1.Rows[index].Cells["exp9amt"].Value = funs.DecimalPoint((runningamt * double.Parse(ansGridView1.Rows[index].Cells["exp9rate"].Value.ToString())) / 100, 2);
            }

            if (double.Parse(ansGridView1.Rows[index].Cells["exp9amt"].Value.ToString()) < double.Parse(ansGridView1.Rows[index].Cells["exp9mr"].Value.ToString()))
            {
                ansGridView1.Rows[index].Cells["exp9amt"].Value = double.Parse(ansGridView1.Rows[index].Cells["exp9mr"].Value.ToString());
                ansGridView1.Rows[index].Cells["exp9amt"].Value = RoundUp(Convert.ToDouble(ansGridView1.Rows[index].Cells["exp9amt"].Value), Convert.ToInt32(ansGridView1.Rows[index].Cells["rounding_ex"].Value));
                totexp += double.Parse(ansGridView1.Rows[index].Cells["exp9amt"].Value.ToString());
                runningamt += double.Parse(ansGridView1.Rows[index].Cells["exp9amt"].Value.ToString());
            }
            else
            {
                ansGridView1.Rows[index].Cells["exp9amt"].Value = RoundUp(Convert.ToDouble(ansGridView1.Rows[index].Cells["exp9amt"].Value), Convert.ToInt32(ansGridView1.Rows[index].Cells["rounding_ex"].Value));
                totexp += double.Parse(ansGridView1.Rows[index].Cells["exp9amt"].Value.ToString());
                runningamt += double.Parse(ansGridView1.Rows[index].Cells["exp9amt"].Value.ToString());
            }

            //Exp10 Calc
            if (ansGridView1.Rows[index].Cells["exp10type"].Value.ToString() == "/Nug")
            {
                if (ansGridView1.Rows[index].Cells["exp10rate"].Value == null)
                {
                    ansGridView1.Rows[index].Cells["exp10rate"].Value = "0";
                }
                ansGridView1.Rows[index].Cells["exp10amt"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[index].Cells["Quantity"].Value.ToString()) * double.Parse(ansGridView1.Rows[index].Cells["exp10rate"].Value.ToString()), 2);
            }
            else if (ansGridView1.Rows[index].Cells["exp10type"].Value.ToString() == "/Weight")
            {
                if (ansGridView1.Rows[index].Cells["weight"].Value == null)
                {
                    ansGridView1.Rows[index].Cells["weight"].Value = "0";
                }
                ansGridView1.Rows[index].Cells["exp10amt"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[index].Cells["ChargedWeight"].Value.ToString()) * double.Parse(ansGridView1.Rows[index].Cells["exp10rate"].Value.ToString()), 2);
            }
            else if (ansGridView1.Rows[index].Cells["exp10type"].Value.ToString() == "Flat")
            {
                if (ansGridView1.Rows[index].Cells["weight"].Value == null)
                {
                    ansGridView1.Rows[index].Cells["weight"].Value = "0";
                }
                ansGridView1.Rows[index].Cells["exp10amt"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[index].Cells["exp10rate"].Value.ToString()), 2);
            }
            else if (ansGridView1.Rows[index].Cells["exp10type"].Value.ToString() == "% of Freight")
            {
                ansGridView1.Rows[index].Cells["exp10amt"].Value = funs.DecimalPoint((double.Parse(ansGridView1.Rows[index].Cells["Amount"].Value.ToString()) * double.Parse(ansGridView1.Rows[index].Cells["exp10rate"].Value.ToString())) / 100, 2);
            }
            else if (ansGridView1.Rows[index].Cells["exp10type"].Value.ToString() == "% of Expenses")
            {
                ansGridView1.Rows[index].Cells["exp10amt"].Value = funs.DecimalPoint((runningamt * double.Parse(ansGridView1.Rows[index].Cells["exp10rate"].Value.ToString())) / 100, 2);
            }

            if (double.Parse(ansGridView1.Rows[index].Cells["exp10amt"].Value.ToString()) < double.Parse(ansGridView1.Rows[index].Cells["exp10mr"].Value.ToString()))
            {
                ansGridView1.Rows[index].Cells["exp10amt"].Value = double.Parse(ansGridView1.Rows[index].Cells["exp10mr"].Value.ToString());
                ansGridView1.Rows[index].Cells["exp10amt"].Value = RoundUp(Convert.ToDouble(ansGridView1.Rows[index].Cells["exp10amt"].Value), Convert.ToInt32(ansGridView1.Rows[index].Cells["rounding_ex"].Value));
                totexp += double.Parse(ansGridView1.Rows[index].Cells["exp10amt"].Value.ToString());
                runningamt += double.Parse(ansGridView1.Rows[index].Cells["exp10amt"].Value.ToString());
            }
            else
            {
                ansGridView1.Rows[index].Cells["exp10amt"].Value = RoundUp(Convert.ToDouble(ansGridView1.Rows[index].Cells["exp10amt"].Value), Convert.ToInt32(ansGridView1.Rows[index].Cells["rounding_ex"].Value));
                totexp += double.Parse(ansGridView1.Rows[index].Cells["exp10amt"].Value.ToString());
                runningamt += double.Parse(ansGridView1.Rows[index].Cells["exp10amt"].Value.ToString());
            }

            ansGridView1.Rows[index].Cells["itemamount"].Value = funs.DecimalPoint(runningamt, 2);
            ansGridView1.Rows[index].Cells["totexp"].Value = funs.DecimalPoint(totexp, 2);
            labelCalc();
        }

        private void ansGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            ansGridView1.CurrentCell.Value = 0;
        }

        private void ansGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            ansGridView1.Rows[e.RowIndex].Cells["description"].Value = "";
            ansGridView1.Rows[e.RowIndex].Cells["Quantity"].Value = 0;
            ansGridView1.Rows[e.RowIndex].Cells["weight"].Value = 0;
            ansGridView1.Rows[e.RowIndex].Cells["totexp"].Value = 0;
            ansGridView1.Rows[e.RowIndex].Cells["Itemamount"].Value = 0;
            ansGridView1.Rows[e.RowIndex].Cells["Rate_am"].Value = 0;
            ansGridView1.Rows[e.RowIndex].Cells["Amount"].Value = 0;
            ansGridView1.Rows[e.RowIndex].Cells["multiplier"].Value = 1;
            ansGridView1.Rows[e.RowIndex].Cells["ChargedWeight"].Value = 0;
        }

        private void textBox9_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (textBox9.Text != "")
                {
                    RoffChanged = true;

                    this.SelectNextControl(this.ActiveControl, true, true, true, true);
                    this.Activate();
                    labelCalc();
                }
            }
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C)
            {
                textBox2.Text = funs.AddAccount();
            }

            if (e.Control && e.KeyCode == Keys.A)
            {
                if (textBox2.Text != "")
                {
                    textBox2.Text = funs.EditAccount(textBox2.Text);
                }
            }
            if (textBox2.Text != "")
            {
                if (textBox23.Text == "Consigner")
                {
                    Expenses(funs.Select_ac_id(textBox1.Text));
                }
                else if (textBox23.Text == "Consignee")
                {
                    Expenses(funs.Select_ac_id(textBox2.Text));
                }
            }
            DeliveryAdd();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            bool isAlter = false;

            if (e.Control && e.KeyCode == Keys.C)
            {
                isAlter = true;
                textBox1.Text = funs.AddAccount();
            }

            if (e.Control && e.KeyCode == Keys.A)
            {
                if (textBox1.Text != "")
                {
                    isAlter = true;
                    textBox1.Text = funs.EditAccount(textBox1.Text);
                }
            }
            if (isAlter = true && textBox1.Text != "")
            {
                if (textBox23.Text == "Consigner")
                {
                    Expenses(funs.Select_ac_id(textBox1.Text));
                }
                else if (textBox23.Text == "Consignee" && textBox2.Text != "")
                {
                    Expenses(funs.Select_ac_id(textBox2.Text));
                }
                textBox25.Text = Database.GetScalarText("select Delivery_type from ACCOUNTs where name='" + textBox1.Text + "'");
                textBox24.Text = Database.GetScalarText("select GR_type from ACCOUNTs where name='" + textBox1.Text + "'");
            }
            DeliveryAdd();
        }

        private void ansGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (ansGridView1.CurrentCell == null)
            {
                return;
            }
            if (e.KeyCode == Keys.Delete)
            {
                if (ansGridView1.CurrentRow.Index == ansGridView1.Rows.Count - 1)
                {
                    ansGridView1.Rows[ansGridView1.CurrentRow.Index].Cells[1].Value = "";
                    ansGridView1.Rows[ansGridView1.CurrentRow.Index].Cells[2].Value = "";
                    labelCalc();
                    return;
                }
                else
                {
                    ansGridView1.Rows.RemoveAt(ansGridView1.CurrentRow.Index);
                    for (int i = 0; i < ansGridView1.Rows.Count; i++)
                    {
                        ansGridView1.Rows[i].Cells["sno"].Value = (i + 1);
                    }
                    labelCalc();
                    return;
                }
            }
            if (ansGridView1.CurrentCell.OwningColumn.Name == "description")
            {
                if (e.Control && e.KeyCode == Keys.A)
                {
                    if (ansGridView1.CurrentCell.Value != null)
                    {
                        ansGridView1.CurrentCell.Value = funs.EditItem(ansGridView1.CurrentCell.Value.ToString());
                    }
                }
                else if (e.Control && e.KeyCode == Keys.C)
                {
                    ansGridView1.CurrentCell.Value = funs.AddItem();
                }
                if (ansGridView1.CurrentCell.Value != null && ansGridView1.CurrentCell.Value.ToString() != "")
                {
                    int i = ansGridView1.CurrentCell.RowIndex;
                    string acid = "";

                    if (textBox23.Text == "Consigner")
                    {
                        acid = funs.Select_ac_id(textBox1.Text);
                    }
                    else if (textBox23.Text == "Consignee")
                    {
                        acid = funs.Select_ac_id(textBox2.Text);
                    }

                    string did = funs.Select_item_name_pack_id(ansGridView1.Rows[i].Cells["description"].Value.ToString());
                    string source = funs.Select_dp_id(textBox3.Text);
                    string destination = funs.Select_dp_id(textBox4.Text);

                    DataTable DtPartyRate = new DataTable();
                    Database.GetSqlData("SELECT * FROM PARTYRATEs WHERE Ac_id = '" + acid + "' AND Des_id = '" + did + "' AND Source_id = '" + source + "' AND Destination_id = '" + destination + "'", DtPartyRate);

                    //ansGridView1.Rows[i].Cells["Quantity"].Value = 0;
                    //ansGridView1.Rows[i].Cells["weight"].Value = 0;
                    //ansGridView1.Rows[i].Cells["chargedweight"].Value = 0;
                    //ansGridView1.Rows[i].Cells["Amount"].Value = 0;
                    ansGridView1.Rows[i].Cells["bharti"].Value = Database.GetScalarDecimal("select bharti from items where id='" + did + "'");

                    if (DtPartyRate.Rows.Count == 1)
                    {
                        ansGridView1.Rows[i].Cells["miniweight"].Value = funs.IndianCurr(double.Parse(DtPartyRate.Rows[0]["Mini_weight"].ToString()));
                        ansGridView1.Rows[i].Cells["charged_weight"].Value = funs.DecimalPoint(double.Parse(DtPartyRate.Rows[0]["charged_weight"].ToString()), 2);
                        ansGridView1.Rows[i].Cells["rounding_ch"].Value = DtPartyRate.Rows[0]["Rounding_ch"].ToString();
                        ansGridView1.Rows[i].Cells["rounding_ex"].Value = DtPartyRate.Rows[0]["Rounding_ex"].ToString();
                        ansGridView1.Rows[i].Cells["bharti"].Value = funs.IndianCurr(double.Parse(DtPartyRate.Rows[0]["St_weight"].ToString()));

                        ansGridView1.Rows[i].Cells["Rate_am"].Value = funs.IndianCurr(double.Parse(DtPartyRate.Rows[0]["Expense0"].ToString()));
                        ansGridView1.Rows[i].Cells["exp1rate"].Value = funs.DecimalPoint(double.Parse(DtPartyRate.Rows[0]["expense1"].ToString()), 2);
                        ansGridView1.Rows[i].Cells["exp2rate"].Value = funs.DecimalPoint(double.Parse(DtPartyRate.Rows[0]["expense2"].ToString()), 2);
                        ansGridView1.Rows[i].Cells["exp3rate"].Value = funs.DecimalPoint(double.Parse(DtPartyRate.Rows[0]["expense3"].ToString()), 2);
                        ansGridView1.Rows[i].Cells["exp4rate"].Value = funs.DecimalPoint(double.Parse(DtPartyRate.Rows[0]["expense4"].ToString()), 2);
                        ansGridView1.Rows[i].Cells["exp5rate"].Value = funs.DecimalPoint(double.Parse(DtPartyRate.Rows[0]["expense5"].ToString()), 2);
                        ansGridView1.Rows[i].Cells["exp6rate"].Value = funs.DecimalPoint(double.Parse(DtPartyRate.Rows[0]["expense6"].ToString()), 2);
                        ansGridView1.Rows[i].Cells["exp7rate"].Value = funs.DecimalPoint(double.Parse(DtPartyRate.Rows[0]["expense7"].ToString()), 2);
                        ansGridView1.Rows[i].Cells["exp8rate"].Value = funs.DecimalPoint(double.Parse(DtPartyRate.Rows[0]["expense8"].ToString()), 2);
                        ansGridView1.Rows[i].Cells["exp9rate"].Value = funs.DecimalPoint(double.Parse(DtPartyRate.Rows[0]["expense9"].ToString()), 2);
                        ansGridView1.Rows[i].Cells["exp10rate"].Value = funs.DecimalPoint(double.Parse(DtPartyRate.Rows[0]["expense10"].ToString()), 2);
                        ansGridView1.Rows[i].Cells["exp11rate"].Value = funs.DecimalPoint(double.Parse(DtPartyRate.Rows[0]["expense11"].ToString()), 2);

                        ansGridView1.Rows[i].Cells["exp1amt"].Value = 0;
                        ansGridView1.Rows[i].Cells["exp2amt"].Value = 0;
                        ansGridView1.Rows[i].Cells["exp3amt"].Value = 0;
                        ansGridView1.Rows[i].Cells["exp4amt"].Value = 0;
                        ansGridView1.Rows[i].Cells["exp5amt"].Value = 0;
                        ansGridView1.Rows[i].Cells["exp6amt"].Value = 0;
                        ansGridView1.Rows[i].Cells["exp7amt"].Value = 0;
                        ansGridView1.Rows[i].Cells["exp8amt"].Value = 0;
                        ansGridView1.Rows[i].Cells["exp9amt"].Value = 0;
                        ansGridView1.Rows[i].Cells["exp10amt"].Value = 0;
                        ansGridView1.Rows[i].Cells["exp11amt"].Value = 0;

                        ansGridView1.Rows[i].Cells["freightmr"].Value = funs.DecimalPoint(double.Parse(DtPartyRate.Rows[0]["mrexpense0"].ToString()), 2);
                        ansGridView1.Rows[i].Cells["exp1mr"].Value = funs.DecimalPoint(double.Parse(DtPartyRate.Rows[0]["mrexpense1"].ToString()), 2);
                        ansGridView1.Rows[i].Cells["exp2mr"].Value = funs.DecimalPoint(double.Parse(DtPartyRate.Rows[0]["mrexpense2"].ToString()), 2);
                        ansGridView1.Rows[i].Cells["exp3mr"].Value = funs.DecimalPoint(double.Parse(DtPartyRate.Rows[0]["mrexpense3"].ToString()), 2);
                        ansGridView1.Rows[i].Cells["exp4mr"].Value = funs.DecimalPoint(double.Parse(DtPartyRate.Rows[0]["mrexpense4"].ToString()), 2);
                        ansGridView1.Rows[i].Cells["exp5mr"].Value = funs.DecimalPoint(double.Parse(DtPartyRate.Rows[0]["mrexpense5"].ToString()), 2);
                        ansGridView1.Rows[i].Cells["exp6mr"].Value = funs.DecimalPoint(double.Parse(DtPartyRate.Rows[0]["mrexpense6"].ToString()), 2);
                        ansGridView1.Rows[i].Cells["exp7mr"].Value = funs.DecimalPoint(double.Parse(DtPartyRate.Rows[0]["mrexpense7"].ToString()), 2);
                        ansGridView1.Rows[i].Cells["exp8mr"].Value = funs.DecimalPoint(double.Parse(DtPartyRate.Rows[0]["mrexpense8"].ToString()), 2);
                        ansGridView1.Rows[i].Cells["exp9mr"].Value = funs.DecimalPoint(double.Parse(DtPartyRate.Rows[0]["mrexpense9"].ToString()), 2);
                        ansGridView1.Rows[i].Cells["exp10mr"].Value = funs.DecimalPoint(double.Parse(DtPartyRate.Rows[0]["mrexpense10"].ToString()), 2);
                        ansGridView1.Rows[i].Cells["exp11mr"].Value = funs.DecimalPoint(double.Parse(DtPartyRate.Rows[0]["mrexpense11"].ToString()), 2);

                        ansGridView1.Rows[i].Cells["per"].Value = DtPartyRate.Rows[0]["expenseType0"];
                        ansGridView1.Rows[i].Cells["exp1type"].Value = DtPartyRate.Rows[0]["expenseType1"];
                        ansGridView1.Rows[i].Cells["exp2type"].Value = DtPartyRate.Rows[0]["expenseType2"];
                        ansGridView1.Rows[i].Cells["exp3type"].Value = DtPartyRate.Rows[0]["expenseType3"];
                        ansGridView1.Rows[i].Cells["exp4type"].Value = DtPartyRate.Rows[0]["expenseType4"];
                        ansGridView1.Rows[i].Cells["exp5type"].Value = DtPartyRate.Rows[0]["expenseType5"];
                        ansGridView1.Rows[i].Cells["exp6type"].Value = DtPartyRate.Rows[0]["expenseType6"];
                        ansGridView1.Rows[i].Cells["exp7type"].Value = DtPartyRate.Rows[0]["expenseType7"];
                        ansGridView1.Rows[i].Cells["exp8type"].Value = DtPartyRate.Rows[0]["expenseType8"];
                        ansGridView1.Rows[i].Cells["exp9type"].Value = DtPartyRate.Rows[0]["expenseType9"];
                        ansGridView1.Rows[i].Cells["exp10type"].Value = DtPartyRate.Rows[0]["expenseType10"];
                        ansGridView1.Rows[i].Cells["exp11type"].Value = DtPartyRate.Rows[0]["expenseType11"];
                        CalcAmount(i);
                    }
                    else
                    {
                        DataTable dtdes = new DataTable();
                        //if (Master.Item.Select("id='" + did + "'").Length < 0)
                        //{
                        //    return;
                        //}
                        //dtdes = Master.Item.Select("id='" + did + "'").CopyToDataTable();

                        Database.GetSqlData("select * from Items where  id='" + did + "'", dtdes);

                        ansGridView1.Rows[i].Cells["miniweight"].Value = funs.IndianCurr(double.Parse(dtdes.Rows[0]["Mini_weight"].ToString()));
                        ansGridView1.Rows[i].Cells["charged_weight"].Value = funs.DecimalPoint(double.Parse(dtdes.Rows[0]["charged_weight"].ToString()), 2);
                        ansGridView1.Rows[i].Cells["rounding_ch"].Value = dtdes.Rows[0]["Rounding_ch"].ToString();
                        ansGridView1.Rows[i].Cells["rounding_ex"].Value = dtdes.Rows[0]["Rounding_ex"].ToString();

                        DataTable Dt1 = new DataTable();


                        Database.GetSqlData("select * from itemdetails where Item_id='" + did + "' and Source_id='" + funs.Select_dp_id(textBox3.Text) + "' and Destination_id='" + funs.Select_dp_id(textBox4.Text) + "'", Dt1);

                        if (Dt1.Rows.Count == 1)
                        {
                            ansGridView1.Rows[i].Cells["Rate_am"].Value = funs.IndianCurr(double.Parse(Dt1.Rows[0]["FreightRate"].ToString()));
                            ansGridView1.Rows[i].Cells["exp1rate"].Value = funs.DecimalPoint(double.Parse(Dt1.Rows[0]["expense1"].ToString()), 2);
                            ansGridView1.Rows[i].Cells["exp2rate"].Value = funs.DecimalPoint(double.Parse(Dt1.Rows[0]["expense2"].ToString()), 2);
                            ansGridView1.Rows[i].Cells["exp3rate"].Value = funs.DecimalPoint(double.Parse(Dt1.Rows[0]["expense3"].ToString()), 2);
                            ansGridView1.Rows[i].Cells["exp4rate"].Value = funs.DecimalPoint(double.Parse(Dt1.Rows[0]["expense4"].ToString()), 2);
                            ansGridView1.Rows[i].Cells["exp5rate"].Value = funs.DecimalPoint(double.Parse(Dt1.Rows[0]["expense5"].ToString()), 2);
                            ansGridView1.Rows[i].Cells["exp6rate"].Value = funs.DecimalPoint(double.Parse(Dt1.Rows[0]["expense6"].ToString()), 2);
                            ansGridView1.Rows[i].Cells["exp7rate"].Value = funs.DecimalPoint(double.Parse(Dt1.Rows[0]["expense7"].ToString()), 2);
                            ansGridView1.Rows[i].Cells["exp8rate"].Value = funs.DecimalPoint(double.Parse(Dt1.Rows[0]["expense8"].ToString()), 2);
                            ansGridView1.Rows[i].Cells["exp9rate"].Value = funs.DecimalPoint(double.Parse(Dt1.Rows[0]["expense9"].ToString()), 2);
                            ansGridView1.Rows[i].Cells["exp10rate"].Value = funs.DecimalPoint(double.Parse(Dt1.Rows[0]["expense10"].ToString()), 2);
                            ansGridView1.Rows[i].Cells["exp11rate"].Value = funs.DecimalPoint(double.Parse(Dt1.Rows[0]["expense11"].ToString()), 2);

                            ansGridView1.Rows[i].Cells["exp1amt"].Value = 0;
                            ansGridView1.Rows[i].Cells["exp2amt"].Value = 0;
                            ansGridView1.Rows[i].Cells["exp3amt"].Value = 0;
                            ansGridView1.Rows[i].Cells["exp4amt"].Value = 0;
                            ansGridView1.Rows[i].Cells["exp5amt"].Value = 0;
                            ansGridView1.Rows[i].Cells["exp6amt"].Value = 0;
                            ansGridView1.Rows[i].Cells["exp7amt"].Value = 0;
                            ansGridView1.Rows[i].Cells["exp8amt"].Value = 0;
                            ansGridView1.Rows[i].Cells["exp9amt"].Value = 0;
                            ansGridView1.Rows[i].Cells["exp10amt"].Value = 0;
                            ansGridView1.Rows[i].Cells["exp10amt"].Value = 0;
                            ansGridView1.Rows[i].Cells["exp11amt"].Value = 0;


                            ansGridView1.Rows[i].Cells["freightmr"].Value = funs.DecimalPoint(double.Parse(Dt1.Rows[0]["MRFreight"].ToString()), 2);
                            ansGridView1.Rows[i].Cells["exp1mr"].Value = funs.DecimalPoint(double.Parse(Dt1.Rows[0]["mrexpense1"].ToString()), 2);
                            ansGridView1.Rows[i].Cells["exp2mr"].Value = funs.DecimalPoint(double.Parse(Dt1.Rows[0]["mrexpense2"].ToString()), 2);
                            ansGridView1.Rows[i].Cells["exp3mr"].Value = funs.DecimalPoint(double.Parse(Dt1.Rows[0]["mrexpense3"].ToString()), 2);
                            ansGridView1.Rows[i].Cells["exp4mr"].Value = funs.DecimalPoint(double.Parse(Dt1.Rows[0]["mrexpense4"].ToString()), 2);
                            ansGridView1.Rows[i].Cells["exp5mr"].Value = funs.DecimalPoint(double.Parse(Dt1.Rows[0]["mrexpense5"].ToString()), 2);
                            ansGridView1.Rows[i].Cells["exp6mr"].Value = funs.DecimalPoint(double.Parse(Dt1.Rows[0]["mrexpense6"].ToString()), 2);
                            ansGridView1.Rows[i].Cells["exp7mr"].Value = funs.DecimalPoint(double.Parse(Dt1.Rows[0]["mrexpense7"].ToString()), 2);
                            ansGridView1.Rows[i].Cells["exp8mr"].Value = funs.DecimalPoint(double.Parse(Dt1.Rows[0]["mrexpense8"].ToString()), 2);
                            ansGridView1.Rows[i].Cells["exp9mr"].Value = funs.DecimalPoint(double.Parse(Dt1.Rows[0]["mrexpense9"].ToString()), 2);
                            ansGridView1.Rows[i].Cells["exp10mr"].Value = funs.DecimalPoint(double.Parse(Dt1.Rows[0]["mrexpense10"].ToString()), 2);
                            ansGridView1.Rows[i].Cells["exp11mr"].Value = funs.DecimalPoint(double.Parse(Dt1.Rows[0]["mrexpense11"].ToString()), 2);

                            ansGridView1.Rows[i].Cells["per"].Value = Dt1.Rows[0]["Freightper"];
                            ansGridView1.Rows[i].Cells["exp1type"].Value = Dt1.Rows[0]["expenseType1"];
                            ansGridView1.Rows[i].Cells["exp2type"].Value = Dt1.Rows[0]["expenseType2"];
                            ansGridView1.Rows[i].Cells["exp3type"].Value = Dt1.Rows[0]["expenseType3"];
                            ansGridView1.Rows[i].Cells["exp4type"].Value = Dt1.Rows[0]["expenseType4"];
                            ansGridView1.Rows[i].Cells["exp5type"].Value = Dt1.Rows[0]["expenseType5"];
                            ansGridView1.Rows[i].Cells["exp6type"].Value = Dt1.Rows[0]["expenseType6"];
                            ansGridView1.Rows[i].Cells["exp7type"].Value = Dt1.Rows[0]["expenseType7"];
                            ansGridView1.Rows[i].Cells["exp8type"].Value = Dt1.Rows[0]["expenseType8"];
                            ansGridView1.Rows[i].Cells["exp9type"].Value = Dt1.Rows[0]["expenseType9"];
                            ansGridView1.Rows[i].Cells["exp10type"].Value = Dt1.Rows[0]["expenseType10"];
                            ansGridView1.Rows[i].Cells["exp11type"].Value = Dt1.Rows[0]["expenseType11"];

                            CalcAmount(i);
                        }
                        else
                        {
                            ansGridView1.Rows[i].Cells["Rate_am"].Value = 0;
                            ansGridView1.Rows[i].Cells["exp1rate"].Value = 0;
                            ansGridView1.Rows[i].Cells["exp2rate"].Value = 0;
                            ansGridView1.Rows[i].Cells["exp3rate"].Value = 0;
                            ansGridView1.Rows[i].Cells["exp4rate"].Value = 0;
                            ansGridView1.Rows[i].Cells["exp5rate"].Value = 0;
                            ansGridView1.Rows[i].Cells["exp6rate"].Value = 0;
                            ansGridView1.Rows[i].Cells["exp7rate"].Value = 0;
                            ansGridView1.Rows[i].Cells["exp8rate"].Value = 0;
                            ansGridView1.Rows[i].Cells["exp9rate"].Value = 0;
                            ansGridView1.Rows[i].Cells["exp10rate"].Value = 0;
                            ansGridView1.Rows[i].Cells["exp11rate"].Value = 0;

                            ansGridView1.Rows[i].Cells["exp1amt"].Value = 0;
                            ansGridView1.Rows[i].Cells["exp2amt"].Value = 0;
                            ansGridView1.Rows[i].Cells["exp3amt"].Value = 0;
                            ansGridView1.Rows[i].Cells["exp4amt"].Value = 0;
                            ansGridView1.Rows[i].Cells["exp5amt"].Value = 0;
                            ansGridView1.Rows[i].Cells["exp6amt"].Value = 0;
                            ansGridView1.Rows[i].Cells["exp7amt"].Value = 0;
                            ansGridView1.Rows[i].Cells["exp8amt"].Value = 0;
                            ansGridView1.Rows[i].Cells["exp9amt"].Value = 0;
                            ansGridView1.Rows[i].Cells["exp10amt"].Value = 0;
                            ansGridView1.Rows[i].Cells["exp11amt"].Value = 0;

                            ansGridView1.Rows[i].Cells["freightmr"].Value = 0;
                            ansGridView1.Rows[i].Cells["exp1mr"].Value = 0;
                            ansGridView1.Rows[i].Cells["exp2mr"].Value = 0;
                            ansGridView1.Rows[i].Cells["exp3mr"].Value = 0;
                            ansGridView1.Rows[i].Cells["exp4mr"].Value = 0;
                            ansGridView1.Rows[i].Cells["exp5mr"].Value = 0;
                            ansGridView1.Rows[i].Cells["exp6mr"].Value = 0;
                            ansGridView1.Rows[i].Cells["exp7mr"].Value = 0;
                            ansGridView1.Rows[i].Cells["exp8mr"].Value = 0;
                            ansGridView1.Rows[i].Cells["exp9mr"].Value = 0;
                            ansGridView1.Rows[i].Cells["exp10mr"].Value = 0;
                            ansGridView1.Rows[i].Cells["exp11mr"].Value = 0;

                            ansGridView1.Rows[i].Cells["per"].Value = "Flat";
                            ansGridView1.Rows[i].Cells["exp1type"].Value = "Flat";
                            ansGridView1.Rows[i].Cells["exp2type"].Value = "Flat";
                            ansGridView1.Rows[i].Cells["exp3type"].Value = "Flat";
                            ansGridView1.Rows[i].Cells["exp4type"].Value = "Flat";
                            ansGridView1.Rows[i].Cells["exp5type"].Value = "Flat";
                            ansGridView1.Rows[i].Cells["exp6type"].Value = "Flat";
                            ansGridView1.Rows[i].Cells["exp7type"].Value = "Flat";
                            ansGridView1.Rows[i].Cells["exp8type"].Value = "Flat";
                            ansGridView1.Rows[i].Cells["exp9type"].Value = "Flat";
                            ansGridView1.Rows[i].Cells["exp10type"].Value = "Flat";
                            ansGridView1.Rows[i].Cells["exp11type"].Value = "Flat";
                            CalcAmount(i);
                        }
                    }
                }
            }
            else if (ansGridView1.CurrentCell.OwningColumn.Name == "unt")
            {
                if (e.Control && e.KeyCode == Keys.A)
                {
                    if (ansGridView1.CurrentCell.Value != null)
                    {
                        ansGridView1.CurrentCell.Value = funs.EditPacking(ansGridView1.CurrentCell.Value.ToString());
                    }
                }
                else if (e.Control && e.KeyCode == Keys.C)
                {
                    ansGridView1.CurrentCell.Value = funs.AddPacking();
                }
            }
            else if (ansGridView1.CurrentCell.OwningColumn.Name == "ItemAmount")
            {
                if (ansGridView1.CurrentCell.Value == null || ansGridView1.Rows[ansGridView1.CurrentRow.Index].Cells["ItemAmount"].Value.ToString() == "")
                {
                    return;
                }
                if (ansGridView1.CurrentRow.Index == ansGridView1.Rows.Count - 1 && ansGridView1.Rows[ansGridView1.CurrentRow.Index].Cells["ItemAmount"].Value.ToString() == "0")
                {
                    SendKeys.Send("{tab}");
                }
            }
        }

        private void radioButton3_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void radioButton2_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void radioButton1_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void Expenses(string acid)
        {
            if (textBox1.Text == "" && textBox2.Text == "" && textBox3.Text == "" && textBox4.Text == "")
            {
                return;
            }
            else
            {
                if (ansGridView1.Rows.Count > 1)
                {

                    string itemname = ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["description"].Value.ToString();

                    string strSql = "select ";
                    strSql += " isnull((Select DPId from DeliveryPoints where [name] ='" + textBox3.Text + "'),'') as sorc,";
                    strSql += " isnull((Select DPId from DeliveryPoints where [name] ='" + textBox4.Text + "'),'') as dest";

                    DataTable dtothInfo = new DataTable();
                    Database.GetSqlData(strSql, dtothInfo);

                    string source = dtothInfo.Rows[0]["sorc"].ToString();// funs.Select_dp_id(textBox3.Text);
                    string destination = dtothInfo.Rows[0]["dest"].ToString();//funs.Select_dp_id(textBox4.Text);

                    for (int i = 0; i < ansGridView1.Rows.Count - 1; i++)
                    {
                        //string did = funs.Select_item_name_pack_id(ansGridView1.Rows[i].Cells["description"].Value.ToString());
                        //string source = funs.Select_dp_id(textBox3.Text);
                        //string destination = funs.Select_dp_id(textBox4.Text);

                        strSql = "select id,bharti from items where [name]='" + ansGridView1.Rows[i].Cells["description"].Value.ToString() + "'";
                        dtothInfo = new DataTable();
                        Database.GetSqlData(strSql, dtothInfo);

                        string did = dtothInfo.Rows[0]["id"].ToString();
                        ansGridView1.Rows[i].Cells["bharti"].Value = dtothInfo.Rows[0]["bharti"];// Database.GetScalarDecimal("select bharti from items where id='" + did + "'");
                        //ansGridView1.Rows[i].Cells["Quantity"].Value = 0;
                        //ansGridView1.Rows[i].Cells["weight"].Value = 0;
                        //ansGridView1.Rows[i].Cells["chargedweight"].Value = 0;
                        ansGridView1.Rows[i].Cells["Amount"].Value = 0;

                        DataTable DtPartyRate = new DataTable();
                        //if (Master.PartyRate.Select("Ac_id = '" + acid + "' AND Des_id = '" + did + "' AND Source_id = '" + source + "' AND Destination_id = '" + destination + "'").Length > 0)
                        //{
                        //    return;
                        //}
                        //DtPartyRate = Master.ItemDetail.Select("Ac_id = '" + acid + "' AND Des_id = '" + did + "' AND Source_id = '" + source + "' AND Destination_id = '" + destination + "'").CopyToDataTable();

                        Database.GetSqlData("SELECT * FROM PARTYRATEs WHERE Ac_id = '" + acid + "' AND Des_id = '" + did + "' AND Source_id = '" + source + "' AND Destination_id = '" + destination + "'", DtPartyRate);

                        if (DtPartyRate.Rows.Count == 1)
                        {
                            ansGridView1.Rows[i].Cells["miniweight"].Value = funs.IndianCurr(double.Parse(DtPartyRate.Rows[0]["Mini_weight"].ToString()));
                            ansGridView1.Rows[i].Cells["charged_weight"].Value = funs.DecimalPoint(double.Parse(DtPartyRate.Rows[0]["charged_weight"].ToString()), 2);
                            ansGridView1.Rows[i].Cells["rounding_ch"].Value = DtPartyRate.Rows[0]["Rounding_ch"].ToString();
                            ansGridView1.Rows[i].Cells["rounding_ex"].Value = DtPartyRate.Rows[0]["Rounding_ex"].ToString();
                            ansGridView1.Rows[i].Cells["bharti"].Value = funs.IndianCurr(double.Parse(DtPartyRate.Rows[0]["St_weight"].ToString()));

                            ansGridView1.Rows[i].Cells["Rate_am"].Value = funs.IndianCurr(double.Parse(DtPartyRate.Rows[0]["Expense0"].ToString()));
                            ansGridView1.Rows[i].Cells["exp1rate"].Value = funs.DecimalPoint(double.Parse(DtPartyRate.Rows[0]["expense1"].ToString()), 2);
                            ansGridView1.Rows[i].Cells["exp2rate"].Value = funs.DecimalPoint(double.Parse(DtPartyRate.Rows[0]["expense2"].ToString()), 2);
                            ansGridView1.Rows[i].Cells["exp3rate"].Value = funs.DecimalPoint(double.Parse(DtPartyRate.Rows[0]["expense3"].ToString()), 2);
                            ansGridView1.Rows[i].Cells["exp4rate"].Value = funs.DecimalPoint(double.Parse(DtPartyRate.Rows[0]["expense4"].ToString()), 2);
                            ansGridView1.Rows[i].Cells["exp5rate"].Value = funs.DecimalPoint(double.Parse(DtPartyRate.Rows[0]["expense5"].ToString()), 2);
                            ansGridView1.Rows[i].Cells["exp6rate"].Value = funs.DecimalPoint(double.Parse(DtPartyRate.Rows[0]["expense6"].ToString()), 2);
                            ansGridView1.Rows[i].Cells["exp7rate"].Value = funs.DecimalPoint(double.Parse(DtPartyRate.Rows[0]["expense7"].ToString()), 2);
                            ansGridView1.Rows[i].Cells["exp8rate"].Value = funs.DecimalPoint(double.Parse(DtPartyRate.Rows[0]["expense8"].ToString()), 2);
                            ansGridView1.Rows[i].Cells["exp9rate"].Value = funs.DecimalPoint(double.Parse(DtPartyRate.Rows[0]["expense9"].ToString()), 2);
                            ansGridView1.Rows[i].Cells["exp10rate"].Value = funs.DecimalPoint(double.Parse(DtPartyRate.Rows[0]["expense10"].ToString()), 2);
                            ansGridView1.Rows[i].Cells["exp11rate"].Value = funs.DecimalPoint(double.Parse(DtPartyRate.Rows[0]["expense11"].ToString()), 2);
                            ansGridView1.Rows[i].Cells["exp1amt"].Value = 0;
                            ansGridView1.Rows[i].Cells["exp2amt"].Value = 0;
                            ansGridView1.Rows[i].Cells["exp3amt"].Value = 0;
                            ansGridView1.Rows[i].Cells["exp4amt"].Value = 0;
                            ansGridView1.Rows[i].Cells["exp5amt"].Value = 0;
                            ansGridView1.Rows[i].Cells["exp6amt"].Value = 0;
                            ansGridView1.Rows[i].Cells["exp7amt"].Value = 0;
                            ansGridView1.Rows[i].Cells["exp8amt"].Value = 0;
                            ansGridView1.Rows[i].Cells["exp9amt"].Value = 0;
                            ansGridView1.Rows[i].Cells["exp10amt"].Value = 0;
                            ansGridView1.Rows[i].Cells["exp11amt"].Value = 0;
                            ansGridView1.Rows[i].Cells["freightmr"].Value = funs.DecimalPoint(double.Parse(DtPartyRate.Rows[0]["mrexpense0"].ToString()), 2);
                            ansGridView1.Rows[i].Cells["exp1mr"].Value = funs.DecimalPoint(double.Parse(DtPartyRate.Rows[0]["mrexpense1"].ToString()), 2);
                            ansGridView1.Rows[i].Cells["exp2mr"].Value = funs.DecimalPoint(double.Parse(DtPartyRate.Rows[0]["mrexpense2"].ToString()), 2);
                            ansGridView1.Rows[i].Cells["exp3mr"].Value = funs.DecimalPoint(double.Parse(DtPartyRate.Rows[0]["mrexpense3"].ToString()), 2);
                            ansGridView1.Rows[i].Cells["exp4mr"].Value = funs.DecimalPoint(double.Parse(DtPartyRate.Rows[0]["mrexpense4"].ToString()), 2);
                            ansGridView1.Rows[i].Cells["exp5mr"].Value = funs.DecimalPoint(double.Parse(DtPartyRate.Rows[0]["mrexpense5"].ToString()), 2);
                            ansGridView1.Rows[i].Cells["exp6mr"].Value = funs.DecimalPoint(double.Parse(DtPartyRate.Rows[0]["mrexpense6"].ToString()), 2);
                            ansGridView1.Rows[i].Cells["exp7mr"].Value = funs.DecimalPoint(double.Parse(DtPartyRate.Rows[0]["mrexpense7"].ToString()), 2);
                            ansGridView1.Rows[i].Cells["exp8mr"].Value = funs.DecimalPoint(double.Parse(DtPartyRate.Rows[0]["mrexpense8"].ToString()), 2);
                            ansGridView1.Rows[i].Cells["exp9mr"].Value = funs.DecimalPoint(double.Parse(DtPartyRate.Rows[0]["mrexpense9"].ToString()), 2);
                            ansGridView1.Rows[i].Cells["exp10mr"].Value = funs.DecimalPoint(double.Parse(DtPartyRate.Rows[0]["mrexpense10"].ToString()), 2);
                            ansGridView1.Rows[i].Cells["exp11mr"].Value = funs.DecimalPoint(double.Parse(DtPartyRate.Rows[0]["mrexpense11"].ToString()), 2);

                            ansGridView1.Rows[i].Cells["per"].Value = DtPartyRate.Rows[0]["expenseType0"];
                            ansGridView1.Rows[i].Cells["exp1type"].Value = DtPartyRate.Rows[0]["expenseType1"];
                            ansGridView1.Rows[i].Cells["exp2type"].Value = DtPartyRate.Rows[0]["expenseType2"];
                            ansGridView1.Rows[i].Cells["exp3type"].Value = DtPartyRate.Rows[0]["expenseType3"];
                            ansGridView1.Rows[i].Cells["exp4type"].Value = DtPartyRate.Rows[0]["expenseType4"];
                            ansGridView1.Rows[i].Cells["exp5type"].Value = DtPartyRate.Rows[0]["expenseType5"];
                            ansGridView1.Rows[i].Cells["exp6type"].Value = DtPartyRate.Rows[0]["expenseType6"];
                            ansGridView1.Rows[i].Cells["exp7type"].Value = DtPartyRate.Rows[0]["expenseType7"];
                            ansGridView1.Rows[i].Cells["exp8type"].Value = DtPartyRate.Rows[0]["expenseType8"];
                            ansGridView1.Rows[i].Cells["exp9type"].Value = DtPartyRate.Rows[0]["expenseType9"];
                            ansGridView1.Rows[i].Cells["exp10type"].Value = DtPartyRate.Rows[0]["expenseType10"];
                            ansGridView1.Rows[i].Cells["exp11type"].Value = DtPartyRate.Rows[0]["expenseType11"];

                            CalcAmount(i);
                        }
                        else
                        {
                            //DataTable dtdes = new DataTable();
                            ////if(Master.Item.Select("id='" + did + "'").Length>0)
                            ////{
                            ////    return;
                            ////}
                            ////dtdes = Master.Item.Select("id='" + did + "'").CopyToDataTable();
                            //Database.GetSqlData("select * from Items where id='" + did + "'", dtdes);

                            //ansGridView1.Rows[i].Cells["miniweight"].Value = funs.IndianCurr(double.Parse(dtdes.Rows[0]["Mini_weight"].ToString()));
                            //ansGridView1.Rows[i].Cells["charged_weight"].Value = funs.DecimalPoint(double.Parse(dtdes.Rows[0]["charged_weight"].ToString()), 2);
                            //ansGridView1.Rows[i].Cells["rounding_ch"].Value = dtdes.Rows[0]["Rounding_ch"].ToString();
                            //ansGridView1.Rows[i].Cells["rounding_ex"].Value = dtdes.Rows[0]["Rounding_ex"].ToString();

                            DataTable Dt1 = new DataTable();
                            ////if (Master.ItemDetail.Select("Item_id='" + did + "' and Source_id='" + funs.Select_dp_id(textBox3.Text) + "' and Destination_id='" + funs.Select_dp_id(textBox4.Text) + "'").Length > 0)
                            ////{
                            ////    return;
                            ////}
                            ////dtdes = Master.ItemDetail.Select("Item_id='" + did + "' and Source_id='" + funs.Select_dp_id(textBox3.Text) + "' and Destination_id='" + funs.Select_dp_id(textBox4.Text) + "'").CopyToDataTable();
                            //Database.GetSqlData("select * from itemdetails where Item_id='" + did + "' and Source_id='" + funs.Select_dp_id(textBox3.Text) + "' and Destination_id='" + funs.Select_dp_id(textBox4.Text) + "'", Dt1);

                            Database.GetSqlData("select itd.*,itm.Mini_weight,itm.charged_weight,itm.Rounding_ch,itm.Rounding_ex from itemdetails as itd left join Items as itm on itd.Item_id=itm.id where  itd.Item_id='" + did + "' and itd.Source_id='" + source + "' and itd.Destination_id='" + destination + "'", Dt1);

                            if (Dt1.Rows.Count == 1)
                            {

                                ansGridView1.Rows[i].Cells["miniweight"].Value = funs.IndianCurr(double.Parse(Dt1.Rows[0]["Mini_weight"].ToString()));
                                ansGridView1.Rows[i].Cells["charged_weight"].Value = funs.DecimalPoint(double.Parse(Dt1.Rows[0]["charged_weight"].ToString()), 2);
                                ansGridView1.Rows[i].Cells["rounding_ch"].Value = Dt1.Rows[0]["Rounding_ch"].ToString();
                                ansGridView1.Rows[i].Cells["rounding_ex"].Value = Dt1.Rows[0]["Rounding_ex"].ToString();


                                ansGridView1.Rows[i].Cells["Rate_am"].Value = funs.IndianCurr(double.Parse(Dt1.Rows[0]["FreightRate"].ToString()));
                                ansGridView1.Rows[i].Cells["exp1rate"].Value = funs.DecimalPoint(double.Parse(Dt1.Rows[0]["expense1"].ToString()), 2);
                                ansGridView1.Rows[i].Cells["exp2rate"].Value = funs.DecimalPoint(double.Parse(Dt1.Rows[0]["expense2"].ToString()), 2);
                                ansGridView1.Rows[i].Cells["exp3rate"].Value = funs.DecimalPoint(double.Parse(Dt1.Rows[0]["expense3"].ToString()), 2);
                                ansGridView1.Rows[i].Cells["exp4rate"].Value = funs.DecimalPoint(double.Parse(Dt1.Rows[0]["expense4"].ToString()), 2);
                                ansGridView1.Rows[i].Cells["exp5rate"].Value = funs.DecimalPoint(double.Parse(Dt1.Rows[0]["expense5"].ToString()), 2);
                                ansGridView1.Rows[i].Cells["exp6rate"].Value = funs.DecimalPoint(double.Parse(Dt1.Rows[0]["expense6"].ToString()), 2);
                                ansGridView1.Rows[i].Cells["exp7rate"].Value = funs.DecimalPoint(double.Parse(Dt1.Rows[0]["expense7"].ToString()), 2);
                                ansGridView1.Rows[i].Cells["exp8rate"].Value = funs.DecimalPoint(double.Parse(Dt1.Rows[0]["expense8"].ToString()), 2);
                                ansGridView1.Rows[i].Cells["exp9rate"].Value = funs.DecimalPoint(double.Parse(Dt1.Rows[0]["expense9"].ToString()), 2);
                                ansGridView1.Rows[i].Cells["exp10rate"].Value = funs.DecimalPoint(double.Parse(Dt1.Rows[0]["expense10"].ToString()), 2);
                                ansGridView1.Rows[i].Cells["exp11rate"].Value = funs.DecimalPoint(double.Parse(Dt1.Rows[0]["expense11"].ToString()), 2);

                                ansGridView1.Rows[i].Cells["exp1amt"].Value = 0;
                                ansGridView1.Rows[i].Cells["exp2amt"].Value = 0;
                                ansGridView1.Rows[i].Cells["exp3amt"].Value = 0;
                                ansGridView1.Rows[i].Cells["exp4amt"].Value = 0;
                                ansGridView1.Rows[i].Cells["exp5amt"].Value = 0;
                                ansGridView1.Rows[i].Cells["exp6amt"].Value = 0;
                                ansGridView1.Rows[i].Cells["exp7amt"].Value = 0;
                                ansGridView1.Rows[i].Cells["exp8amt"].Value = 0;
                                ansGridView1.Rows[i].Cells["exp9amt"].Value = 0;
                                ansGridView1.Rows[i].Cells["exp10amt"].Value = 0;
                                ansGridView1.Rows[i].Cells["exp11amt"].Value = 0;

                                ansGridView1.Rows[i].Cells["freightmr"].Value = funs.DecimalPoint(double.Parse(Dt1.Rows[0]["MRFreight"].ToString()), 2);
                                ansGridView1.Rows[i].Cells["exp1mr"].Value = funs.DecimalPoint(double.Parse(Dt1.Rows[0]["mrexpense1"].ToString()), 2);
                                ansGridView1.Rows[i].Cells["exp2mr"].Value = funs.DecimalPoint(double.Parse(Dt1.Rows[0]["mrexpense2"].ToString()), 2);
                                ansGridView1.Rows[i].Cells["exp3mr"].Value = funs.DecimalPoint(double.Parse(Dt1.Rows[0]["mrexpense3"].ToString()), 2);
                                ansGridView1.Rows[i].Cells["exp4mr"].Value = funs.DecimalPoint(double.Parse(Dt1.Rows[0]["mrexpense4"].ToString()), 2);
                                ansGridView1.Rows[i].Cells["exp5mr"].Value = funs.DecimalPoint(double.Parse(Dt1.Rows[0]["mrexpense5"].ToString()), 2);
                                ansGridView1.Rows[i].Cells["exp6mr"].Value = funs.DecimalPoint(double.Parse(Dt1.Rows[0]["mrexpense6"].ToString()), 2);
                                ansGridView1.Rows[i].Cells["exp7mr"].Value = funs.DecimalPoint(double.Parse(Dt1.Rows[0]["mrexpense7"].ToString()), 2);
                                ansGridView1.Rows[i].Cells["exp8mr"].Value = funs.DecimalPoint(double.Parse(Dt1.Rows[0]["mrexpense8"].ToString()), 2);
                                ansGridView1.Rows[i].Cells["exp9mr"].Value = funs.DecimalPoint(double.Parse(Dt1.Rows[0]["mrexpense9"].ToString()), 2);
                                ansGridView1.Rows[i].Cells["exp10mr"].Value = funs.DecimalPoint(double.Parse(Dt1.Rows[0]["mrexpense10"].ToString()), 2);
                                ansGridView1.Rows[i].Cells["exp11mr"].Value = funs.DecimalPoint(double.Parse(Dt1.Rows[0]["mrexpense11"].ToString()), 2);

                                ansGridView1.Rows[i].Cells["per"].Value = Dt1.Rows[0]["Freightper"];
                                ansGridView1.Rows[i].Cells["exp1type"].Value = Dt1.Rows[0]["expenseType1"];
                                ansGridView1.Rows[i].Cells["exp2type"].Value = Dt1.Rows[0]["expenseType2"];
                                ansGridView1.Rows[i].Cells["exp3type"].Value = Dt1.Rows[0]["expenseType3"];
                                ansGridView1.Rows[i].Cells["exp4type"].Value = Dt1.Rows[0]["expenseType4"];
                                ansGridView1.Rows[i].Cells["exp5type"].Value = Dt1.Rows[0]["expenseType5"];
                                ansGridView1.Rows[i].Cells["exp6type"].Value = Dt1.Rows[0]["expenseType6"];
                                ansGridView1.Rows[i].Cells["exp7type"].Value = Dt1.Rows[0]["expenseType7"];
                                ansGridView1.Rows[i].Cells["exp8type"].Value = Dt1.Rows[0]["expenseType8"];
                                ansGridView1.Rows[i].Cells["exp9type"].Value = Dt1.Rows[0]["expenseType9"];
                                ansGridView1.Rows[i].Cells["exp10type"].Value = Dt1.Rows[0]["expenseType10"];
                                ansGridView1.Rows[i].Cells["exp11type"].Value = Dt1.Rows[0]["expenseType11"];
                                CalcAmount(i);
                            }
                            else
                            {
                                ansGridView1.Rows[i].Cells["miniweight"].Value = 0;
                                ansGridView1.Rows[i].Cells["charged_weight"].Value = 0;
                                ansGridView1.Rows[i].Cells["rounding_ch"].Value = 0;
                                ansGridView1.Rows[i].Cells["rounding_ex"].Value = 0;

                                ansGridView1.Rows[i].Cells["Rate_am"].Value = 0;
                                ansGridView1.Rows[i].Cells["exp1rate"].Value = 0;
                                ansGridView1.Rows[i].Cells["exp2rate"].Value = 0;
                                ansGridView1.Rows[i].Cells["exp3rate"].Value = 0;
                                ansGridView1.Rows[i].Cells["exp4rate"].Value = 0;
                                ansGridView1.Rows[i].Cells["exp5rate"].Value = 0;
                                ansGridView1.Rows[i].Cells["exp6rate"].Value = 0;
                                ansGridView1.Rows[i].Cells["exp7rate"].Value = 0;
                                ansGridView1.Rows[i].Cells["exp8rate"].Value = 0;
                                ansGridView1.Rows[i].Cells["exp9rate"].Value = 0;
                                ansGridView1.Rows[i].Cells["exp10rate"].Value = 0;
                                ansGridView1.Rows[i].Cells["exp11rate"].Value = 0;

                                ansGridView1.Rows[i].Cells["exp1amt"].Value = 0;
                                ansGridView1.Rows[i].Cells["exp2amt"].Value = 0;
                                ansGridView1.Rows[i].Cells["exp3amt"].Value = 0;
                                ansGridView1.Rows[i].Cells["exp4amt"].Value = 0;
                                ansGridView1.Rows[i].Cells["exp5amt"].Value = 0;
                                ansGridView1.Rows[i].Cells["exp6amt"].Value = 0;
                                ansGridView1.Rows[i].Cells["exp7amt"].Value = 0;
                                ansGridView1.Rows[i].Cells["exp8amt"].Value = 0;
                                ansGridView1.Rows[i].Cells["exp9amt"].Value = 0;
                                ansGridView1.Rows[i].Cells["exp10amt"].Value = 0;
                                ansGridView1.Rows[i].Cells["exp11amt"].Value = 0;

                                ansGridView1.Rows[i].Cells["freightmr"].Value = 0;
                                ansGridView1.Rows[i].Cells["exp1mr"].Value = 0;
                                ansGridView1.Rows[i].Cells["exp2mr"].Value = 0;
                                ansGridView1.Rows[i].Cells["exp3mr"].Value = 0;
                                ansGridView1.Rows[i].Cells["exp4mr"].Value = 0;
                                ansGridView1.Rows[i].Cells["exp5mr"].Value = 0;
                                ansGridView1.Rows[i].Cells["exp6mr"].Value = 0;
                                ansGridView1.Rows[i].Cells["exp7mr"].Value = 0;
                                ansGridView1.Rows[i].Cells["exp8mr"].Value = 0;
                                ansGridView1.Rows[i].Cells["exp9mr"].Value = 0;
                                ansGridView1.Rows[i].Cells["exp10mr"].Value = 0;
                                ansGridView1.Rows[i].Cells["exp11mr"].Value = 0;
                                ansGridView1.Rows[i].Cells["per"].Value = "Flat";
                                ansGridView1.Rows[i].Cells["exp1type"].Value = "Flat";
                                ansGridView1.Rows[i].Cells["exp2type"].Value = "Flat";
                                ansGridView1.Rows[i].Cells["exp3type"].Value = "Flat";
                                ansGridView1.Rows[i].Cells["exp4type"].Value = "Flat";
                                ansGridView1.Rows[i].Cells["exp5type"].Value = "Flat";
                                ansGridView1.Rows[i].Cells["exp6type"].Value = "Flat";
                                ansGridView1.Rows[i].Cells["exp7type"].Value = "Flat";
                                ansGridView1.Rows[i].Cells["exp8type"].Value = "Flat";
                                ansGridView1.Rows[i].Cells["exp9type"].Value = "Flat";
                                ansGridView1.Rows[i].Cells["exp10type"].Value = "Flat";
                                ansGridView1.Rows[i].Cells["exp11type"].Value = "Flat";
                                CalcAmount(i);
                            }
                        }
                    }
                }
            }
        }

        private double RoundUp(double val, int Tonext)
        {
            if (Tonext != 0)
            {
                return Math.Ceiling(val / Tonext) * Tonext;
            }
            else
            {
                return val;
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            SetVno();
        }

        private void textBox25_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox25);
        }

        private void textBox25_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox25);
        }

        private void textBox25_KeyPress(object sender, KeyPressEventArgs e)
        {
            DataTable dtcombo = new DataTable();
            dtcombo.Columns.Add("DeliveryType", typeof(string));


            dtcombo.Columns["DeliveryType"].ColumnName = "DeliveryType";
            dtcombo.Rows.Add();
            dtcombo.Rows[0][0] = "Godown";

            dtcombo.Rows.Add();
            dtcombo.Rows[1][0] = "Door Delivery";

            textBox25.Text = SelectCombo.ComboDt(this, dtcombo, 0);
            DeliveryAdd();
            SendKeys.Send("{tab}");
        }

        private void textBox23_KeyPress(object sender, KeyPressEventArgs e)
        {
            string expens = textBox23.Text;

            DataTable dtcombo = new DataTable();
            dtcombo.Columns.Add("ExpensesPerAs", typeof(string));

            dtcombo.Columns["ExpensesPerAs"].ColumnName = "ExpensesPerAs";

            dtcombo.Rows.Add();
            dtcombo.Rows[0][0] = "Consigner";

            dtcombo.Rows.Add();
            dtcombo.Rows[1][0] = "Consignee";


            textBox23.Text = SelectCombo.ComboDt(this, dtcombo, 0);
            if (expens == textBox23.Text)
            {
                return;
            }

            if (textBox23.Text == "Consigner")
            {
                Expenses(funs.Select_ac_id(textBox1.Text));
            }
            else if (textBox23.Text == "Consignee")
            {
                Expenses(funs.Select_ac_id(textBox2.Text));
            }

            SendKeys.Send("{tab}");
        }

        private void textBox23_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox23);
        }

        private void textBox23_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox23);
        }

        private void textBox24_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox24);
        }

        private void textBox24_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox24);
        }

        private void textBox24_KeyPress(object sender, KeyPressEventArgs e)
        {
            DataTable dtcombo = new DataTable();
            dtcombo.Columns.Add("PaymentMode", typeof(string));

            dtcombo.Columns["PaymentMode"].ColumnName = "PaymentMode";
            dtcombo.Rows.Add();
            dtcombo.Rows[0][0] = "Paid";
            dtcombo.Rows.Add();
            dtcombo.Rows[1][0] = "FOC";
            dtcombo.Rows.Add();
            dtcombo.Rows[2][0] = "T.B.B.";
            dtcombo.Rows.Add();
            dtcombo.Rows[3][0] = "To Pay";

            textBox24.Text = SelectCombo.ComboDt(this, dtcombo, 0);
            if (textBox24.Text == "FOC")
            {
                textBox29.Text = "Credit";
                textBox29.Enabled = false;
                //SendKeys.Send("{tab}");
            }
            else if (textBox24.Text == "T.B.B.")
            {
                textBox29.Text = "Credit";
                textBox29.Enabled = false;
               // SendKeys.Send("{tab}");
            }
            else if (textBox24.Text == "To Pay")
            {
                textBox29.Text = "Credit";
                textBox29.Enabled = false;
                //SendKeys.Send("{tab}");
            }
            else
            {
                textBox29.Enabled = true;
            }
            SendKeys.Send("{tab}");
        }

        private void textBox11_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox11);
        }

        private void DisplaySetting()
        {

             DataTable dtvt = new DataTable();
             strCombo = "select Name from vouchertypes where type='Booking' and Active ='true'";

             Database.GetSqlData(strCombo, dtvt);

            if (dtvt.Rows.Count == 1)
            {
                textBox11.Text = dtvt.Rows[0]["name"].ToString();
                vtid = funs.Select_vt_id(textBox11.Text);
                textBox11.Enabled = false;
                SetVno();
            }

        }
        private void textBox11_KeyPress(object sender, KeyPressEventArgs e)
        {
            strCombo = "select Name from vouchertypes where type='Booking' and Active ='true'";
            textBox11.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
            if (textBox11.Text == "")
            {
                return;
            }
            vtid = funs.Select_vt_id(textBox11.Text);
            SetVno();
        }

        private void textBox11_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox11);
        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {

        }

        private void checkBox1_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox28_KeyPress(object sender, KeyPressEventArgs e)
        {
            string wheresrt = "(Path LIKE '8;40;%')";
            strCombo = funs.GetStrCombo(wheresrt);
            //textBox1.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 1);
            //strCombo = "SELECT    Name FROM ACCOUNTs WHERE     (act_id = 40) order by name";
            textBox28.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, textBox28.Text, 0);
        }

        private void textBox28_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox28);
        }

        private void textBox28_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox28);
        }

        private void textBox27_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox27);
        }

        private void textBox27_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox27);
        }

        private void textBox27_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void dateTimePicker2_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(dateTimePicker2);
        }

        private void dateTimePicker2_Enter(object sender, EventArgs e)
        {
            Database.setFocus(dateTimePicker2);
        }

        private void textBox28_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C)
            {
                textBox28.Text = funs.AddAccount();
            }

            if (e.Control && e.KeyCode == Keys.A)
            {
                if (textBox28.Text != "")
                {
                    textBox28.Text = funs.EditAccount(textBox28.Text);
                }
            }


            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void frmBooking_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.tspb != null) {
                this.tspb.Visible = false;
                this.tspb.Value = 0;
            }
        }

        private void textBox29_KeyPress(object sender, KeyPressEventArgs e)
        {

            string expens = textBox23.Text;

            DataTable dtcombo = new DataTable();
            dtcombo.Columns.Add("Paid", typeof(string));

          
            dtcombo.Rows.Add();
            dtcombo.Rows[0][0] = "Cash";

            dtcombo.Rows.Add();
            dtcombo.Rows[1][0] = "Credit";


            textBox29.Text = SelectCombo.ComboDt(this, dtcombo, 0);
        }

        private void textBox29_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox29);
        }

        private void textBox29_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox29);
        }
    }
}
