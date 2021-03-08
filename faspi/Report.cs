using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using Microsoft.Office.Core;
using Excel = Microsoft.Office.Interop.Excel;

namespace faspi
{
    public partial class Report : Form
    {
        DataTable dtFinal = new DataTable();
        DataTable tdt = new DataTable();
        DataTable dt = new DataTable();
        private System.ComponentModel.IContainer components = null;
        String sql;
        string strCombo = "";
        DateTime stdt = new DateTime();
        DateTime endt = new DateTime();
        public String frmptyp;
        public String DecsOfReport;
        public string str = "";
        public static string Pagesize = "", gvtid = "";
        public static String frmptyp2;
        public static String DecsOfReport2;
        public static string str2 = "";
        string AccName = "";
        string gGodownName = "";
        string strqyery = "";

        public Report()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (frmptyp == "Destination Wise")
            {
                DestinationWise(dateTimePicker1.Value, dateTimePicker2.Value,textBox1.Text);
            }
            //else if (frmptyp == "Journal")
            //{
            //    Journal(dateTimePicker1.Value, dateTimePicker2.Value,gvtid);
            //}
            else if (frmptyp == "Booking GST Report")
            {
                GSTBooking(dateTimePicker1.Value, dateTimePicker2.Value);
            }
            else if (frmptyp == "GST Report Unloading Challan")
            {
                GSTUnloadingChallan(dateTimePicker1.Value, dateTimePicker2.Value);
            }
            else if (frmptyp == "Ledger")
            {
                Ledger(dateTimePicker1.Value, dateTimePicker2.Value, textBox1.Text);
            }
            else if (frmptyp == "Detail Ledger")
            {
                DetailLedger(dateTimePicker1.Value, dateTimePicker2.Value, textBox1.Text);
            }
            else if (frmptyp == "CashBook")
            {
                CashBook(dateTimePicker1.Value, dateTimePicker2.Value);
            }
            else if (frmptyp == "Standard Trial Balance")
            {
                StandardTrial(dateTimePicker1.Value, dateTimePicker2.Value);
            }
            else if (frmptyp == "Opening Trial Balance")
            {
                OpeningTrial(dateTimePicker1.Value, dateTimePicker2.Value);
            }
            else if (frmptyp == "Grouped Trial Balance")
            {
                GroupedTrial(dateTimePicker1.Value, dateTimePicker2.Value);
            }            
            else if (frmptyp == "Moved Account Summary")
            {
                MovedAccountSummary(dateTimePicker1.Value, dateTimePicker2.Value);
            }
            else if (frmptyp == "Payment Collector Balance")
            {
                AccountGroupBalance(Database.stDate, dateTimePicker2.Value);
            }
            else if (frmptyp == "Profit And Loss")
            {
                ProfitAndLoss(dateTimePicker1.Value, dateTimePicker2.Value);
            }
            else if (frmptyp == "Balance Sheet")
            {
                BalanceSheet(dateTimePicker1.Value, dateTimePicker2.Value);
            }
            else if (frmptyp == "GroupLedger")
            {
                GroupLedger(dateTimePicker1.Value, dateTimePicker2.Value);
            }
            if (dataGridView1.Rows.Count == 0)
            {
                button1.Visible = false;
                button2.Visible = false;
                button4.Visible = false;
                button6.Visible = false;
            }
            else
            {
                button1.Visible = true;
                button2.Visible = true;
                button4.Visible = true;
                button6.Visible = true;
            }
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        //private void SideFill()
        //{
        //    flowLayoutPanel1.Controls.Clear();
        //    DataTable dtsidefill = new DataTable();
        //    dtsidefill.Columns.Add("Name", typeof(string));
        //    dtsidefill.Columns.Add("DisplayName", typeof(string));
        //    dtsidefill.Columns.Add("ShortcutKey", typeof(string));
        //    dtsidefill.Columns.Add("Visible", typeof(bool));
        //    //print
        //    dtsidefill.Rows.Add();
        //    dtsidefill.Rows[0]["Name"] = "print";
        //    dtsidefill.Rows[0]["DisplayName"] = "Print";
        //    dtsidefill.Rows[0]["ShortcutKey"] = "^P";
        //    dtsidefill.Rows[0]["Visible"] = true;

        //    //Preview
        //    dtsidefill.Rows.Add();
        //    dtsidefill.Rows[1]["Name"] = "preview";
        //    dtsidefill.Rows[1]["DisplayName"] = "Print Preview";
        //    dtsidefill.Rows[1]["ShortcutKey"] = "";
        //    dtsidefill.Rows[1]["Visible"] = true;

        //    //pdf
        //    dtsidefill.Rows.Add();
        //    dtsidefill.Rows[2]["Name"] = "pdf";
        //    dtsidefill.Rows[2]["DisplayName"] = "Export to PDF";
        //    dtsidefill.Rows[2]["ShortcutKey"] = "";
        //    if (Feature.Available("Data Export") == "No")
        //    {
        //        dtsidefill.Rows[2]["Visible"] = false;
        //    }
        //    else
        //    {
        //        dtsidefill.Rows[2]["Visible"] = true;
        //    }

        //    //excel
        //    dtsidefill.Rows.Add();
        //    dtsidefill.Rows[3]["Name"] = "excel";
        //    dtsidefill.Rows[3]["DisplayName"] = "Export to Excel";
        //    dtsidefill.Rows[3]["ShortcutKey"] = "";
        //    if (Feature.Available("Data Export") == "No")
        //    {
        //        dtsidefill.Rows[3]["Visible"] = false;
        //    }
        //    else
        //    {
        //        dtsidefill.Rows[3]["Visible"] = true;
        //    }

        //    //close
        //    dtsidefill.Rows.Add();
        //    dtsidefill.Rows[4]["Name"] = "quit";
        //    dtsidefill.Rows[4]["DisplayName"] = "Quit";
        //    dtsidefill.Rows[4]["ShortcutKey"] = "Esc";
        //    dtsidefill.Rows[4]["Visible"] = true;

        //    for (int i = 0; i < dtsidefill.Rows.Count; i++)
        //    {
        //        if (bool.Parse(dtsidefill.Rows[i]["Visible"].ToString()) == true)
        //        {
        //            Button btn = new Button();
        //            btn.Size = new Size(150, 45);
        //            btn.Name = dtsidefill.Rows[i]["Name"].ToString();
        //            btn.Text = "";
        //            Bitmap bmp = new Bitmap(btn.ClientRectangle.Width, btn.ClientRectangle.Height);
        //            Graphics G = Graphics.FromImage(bmp);
        //            G.Clear(btn.BackColor);
        //            string line1 = dtsidefill.Rows[i]["ShortcutKey"].ToString();
        //            string line2 = dtsidefill.Rows[i]["DisplayName"].ToString();
        //            StringFormat SF = new StringFormat();
        //            SF.Alignment = StringAlignment.Near;
        //            SF.LineAlignment = StringAlignment.Center;
        //            System.Drawing.Rectangle RC = btn.ClientRectangle;
        //            System.Drawing.Font font = new System.Drawing.Font("Arial", 14);
        //            G.DrawString(line1, font, Brushes.Red, RC, SF);
        //            G.DrawString("".PadLeft(line1.Length * 2 + 1) + line2, font, Brushes.Black, RC, SF);
        //            btn.Image = bmp;
        //            btn.Click += new EventHandler(btn_Click);
        //            flowLayoutPanel1.Controls.Add(btn);
        //        }
        //    }
        //}

        //void btn_Click(object sender, EventArgs e)
        //{
        //    Button tbtn = (Button)sender;
        //    string name = tbtn.Name.ToString();
        //    if (name == "print")
        //    {
        //        if (dataGridView1.Rows.Count == 0)
        //        {
        //            return;
        //        }
        //        string tPath = Path.GetTempPath() + DateTime.Now.ToString("yyMMddhmmssfff") + ".pdf";
        //        ExportToPdf(tPath);
        //        GC.Collect();
        //        PdfReader frm = new PdfReader();
        //        frm.LoadFile(tPath);
        //        frm.Visible = false;
        //        frm.axAcroPDF1.printWithDialog();
        //    }
        //    else if (name == "preview")
        //    {
        //        if (dataGridView1.Rows.Count == 0)
        //        {
        //            return;
        //        }
        //        string tPath = Path.GetTempPath() + DateTime.Now.ToString("yyMMddhmmssfff") + ".pdf";
        //        ExportToPdf(tPath);
        //        GC.Collect();
        //        PdfReader frm = new PdfReader();
        //        frm.LoadFile(tPath);
        //        frm.Show();
        //    }
        //    else if (name == "pdf")
        //    {
        //        if (dataGridView1.Rows.Count == 0)
        //        {
        //            return;
        //        }
        //        SaveFileDialog ofd = new SaveFileDialog();
        //        ofd.Filter = "Adobe Acrobat(*.pdf) | *.pdf";

        //        if (DialogResult.OK == ofd.ShowDialog())
        //        {
        //            ExportToPdf(ofd.FileName);
        //            MessageBox.Show("Export Successfully!!");
        //        }
        //    }
        //    else if (name == "excel")
        //    {
        //        if (dataGridView1.Rows.Count == 0)
        //        {
        //            return;
        //        }
        //        Object misValue = System.Reflection.Missing.Value;
        //        Excel.Application apl = new Microsoft.Office.Interop.Excel.Application();
        //        Excel.Workbook wb = (Excel.Workbook)apl.Workbooks.Add(misValue);
        //        Excel.Worksheet ws;
        //        ws = (Excel.Worksheet)wb.Worksheets[1];

        //        int lno = 1;
        //        DataTable dtExcel = new DataTable();

        //        DataTable dtRheader = new DataTable();
        //        Database.GetSqlData("select * from company", dtRheader);

        //        ws.Cells[lno, 1] = dtRheader.Rows[0]["name"].ToString();
        //        ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, dataGridView1.Columns.Count]).Merge(Type.Missing);
        //        ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, dataGridView1.Columns.Count]).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
        //        ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, dataGridView1.Columns.Count]).Font.Bold = true;
        //        lno++;

        //        ws.Cells[lno, 1] = dtRheader.Rows[0]["Address1"].ToString();
        //        ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, dataGridView1.Columns.Count]).Merge(Type.Missing);
        //        ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, dataGridView1.Columns.Count]).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
        //        ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, dataGridView1.Columns.Count]).Font.Bold = true;
        //        lno++;

        //        ws.Cells[lno, 1] = dtRheader.Rows[0]["Address2"].ToString();
        //        ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, dataGridView1.Columns.Count]).Merge(Type.Missing);
        //        ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, dataGridView1.Columns.Count]).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
        //        ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, dataGridView1.Columns.Count]).Font.Bold = true;
        //        lno++;

        //        for (int i = 0; i < dataGridView1.Columns.Count; i++)
        //        {
        //            if (dataGridView1.Columns[i].HeaderCell.Style.Alignment == DataGridViewContentAlignment.MiddleRight)
        //            {
        //                ws.get_Range(ws.Cells[5, i + 1], ws.Cells[5, i + 1]).HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
        //            }
        //            ws.get_Range(ws.Cells[i + 1, i + 1], ws.Cells[i + 1, i + 1]).ColumnWidth = dataGridView1.Columns[i].Width / 11.5;
        //            ws.Cells[5, i + 1] = dataGridView1.Columns[i].HeaderText.ToString();
        //        }

        //        for (int i = 0; i < dataGridView1.Rows.Count; i++)
        //        {
        //            for (int j = 0; j < dataGridView1.Columns.Count; j++)
        //            {
        //                if (dataGridView1.Columns[j].HeaderCell.Style.Alignment == DataGridViewContentAlignment.MiddleRight)
        //                {
        //                    ws.get_Range(ws.Cells[i + 6, j + 1], ws.Cells[i + 6, j + 1]).HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
        //                    ws.get_Range(ws.Cells[i + 6, j + 1], ws.Cells[i + 6, j + 1]).NumberFormat = "0,0.00";
        //                }
        //                else
        //                {
        //                    ws.get_Range(ws.Cells[i + 6, j + 1], ws.Cells[i + 6, j + 1]).HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
        //                }
        //                if (dataGridView1.Columns[j].DefaultCellStyle.Font != null)
        //                {
        //                    ws.get_Range(ws.Cells[i + 6, j + 1], ws.Cells[i + 6, j + 1]).Font.Bold = true;
        //                }
        //                if (dataGridView1.Rows[i].Cells[j].Value != null)
        //                {
        //                    ws.Cells[i + 6, j + 1] = dataGridView1.Rows[i].Cells[j].Value.ToString().Replace(",", "");
        //                }
        //            }
        //        }

        //        Excel.Range last = ws.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell, Type.Missing);
        //        ws.get_Range("A1", last).WrapText = true;
        //        apl.Visible = true;
        //    }
        //    else if (name == "quit")
        //    {
        //        this.Close();
        //        this.Dispose();
        //    }
        //}

        private void Report_Load(object sender, EventArgs e)
        {
            dateTimePicker1.CustomFormat = Database.dformat;
            dateTimePicker2.CustomFormat = Database.dformat;
            this.WindowState = FormWindowState.Maximized;
            //foreach (DataGridViewColumn column in dataGridView1.Columns)
            //{
            //    column.SortMode = DataGridViewColumnSortMode.NotSortable;
            //}
            if (dataGridView1.Rows.Count == 0)
            {
                button1.Visible = false;
                button2.Visible = false;
                button4.Visible = false;
                button6.Visible = false;
            }
        }

        public bool CashBook(DateTime DateFrom, DateTime DateTo)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
            double totdr = 0;
            double totcr = 0;
            stdt = DateFrom;
            endt = DateTo;
            dateTimePicker1.Value = DateFrom;
            dateTimePicker2.Value = DateTo;
            label3.Enabled = false;
            textBox1.Enabled = false;
            frmptyp = "CashBook";
            this.Text = frmptyp;
            DecsOfReport = "CashBook, for the period of " + DateFrom.ToString(Database.dformat) + " to " + DateTo.ToString(Database.dformat);

            sql = "SELECT aman.Name,Vdate,DocNumber,Narr as Particular,aman.Dr,aman.Cr , ACCOUNTs.Act_id as AccounTypes FROM (SELECT " + access_sql.Hash + "2/1/1801" + access_sql.Hash + " AS Vdate, 'OPN' AS [Short], 0 AS Vnumber, Y.Name, Y.Dr,Y.Cr, 'Opening Balance' AS Narr,' ' AS DocNumber FROM (SELECT X.Name, Sum(X.Dr) AS Dr, Sum(X.Cr) AS Cr FROM (SELECT 0 AS sr, ACCOUNTs.Name, " + access_sql.fnstring("ACCOUNTs.Balance>0", "ACCOUNTs.Balance", "0") + " AS Dr, " + access_sql.fnstring("ACCOUNTs.Balance<0", "-1*(ACCOUNTs.Balance)", "0") + " AS Cr From ACCOUNTs UNION ALL SELECT 1 AS sr, QryJournal.Name, Sum(QryJournal.Dr) AS Dr, Sum(QryJournal.Cr) AS Cr FROM QryJournal WHERE (((QryJournal.Vdate)<" + access_sql.Hash + DateFrom.ToString(Database.dformat) + access_sql.Hash + ")) GROUP BY QryJournal.Name, QryJournal.A HAVING (((QryJournal.A)=" + access_sql.Singlequote + "True" + access_sql.Singlequote + ")))  AS X GROUP BY X.Name) AS Y UNION ALL  SELECT JOURNALs.Vdate, VOUCHERTYPEs.Short, Voucherinfos.Vnumber, ACCOUNTs.Name, " + access_sql.fnstring("JOURNALs.Amount>0", "JOURNALs.Amount", "0") + " AS Dr, " + access_sql.fnstring("JOURNALs.Amount<0", "-1*(JOURNALs.Amount)", "0") + " AS Cr, Voucherinfos.Narr, " + access_sql.Docnumber + " AS DocNumber FROM JOURNALs, ACCOUNTs, Voucherinfos, VOUCHERTYPEs WHERE (((JOURNALs.Vdate)>=" + access_sql.Hash + DateFrom.ToString(Database.dformat) + access_sql.Hash + ") AND ((JOURNALs.Ac_id)=[ACCOUNTs].[Ac_id]) AND ((JOURNALs.Vi_id)=[VOUCHERINFOs].[Vi_id]) AND ((Voucherinfos.Vt_id)=[VOUCHERTYPEs].[Vt_id]) AND ((VOUCHERTYPEs.A)=" + access_sql.Singlequote + "True" + access_sql.Singlequote + "))) AS aman INNER JOIN ACCOUNTs ON aman.Name = ACCOUNTs.Name";
            
            dt.Clear();
            Database.GetSqlData(sql, dt);
            DataRow[] drow;

            drow = dt.Select("Vdate<=" + access_sql.Hash + DateTo.ToString(Database.dformat) + access_sql.Hash + " and AccountType=3");
            tdt.Clear();
            if (drow.GetLength(0) > 0)
            {
                tdt = drow.CopyToDataTable();
                tdt.Columns.Remove("AccountType");
                tdt.DefaultView.Sort = "Vdate";
                tdt.Columns.Add("RunningBalance", typeof(decimal));
                tdt.Columns.Add("Dr/Cr", typeof(string));
                for (int i = 0; i < tdt.Rows.Count; i++)
                {
                    totdr += double.Parse(tdt.Rows[i]["Dr"].ToString());
                    totcr += double.Parse(tdt.Rows[i]["Cr"].ToString());
                    if (totdr > totcr)
                    {
                        tdt.Rows[i]["RunningBalance"] = totdr - totcr;
                        tdt.Rows[i]["Dr/Cr"] = "Dr.";
                    }
                    else if (totcr > totdr)
                    {
                        tdt.Rows[i]["RunningBalance"] = totcr - totdr;
                        tdt.Rows[i]["Dr/Cr"] = "Cr.";
                    }
                    else
                    {
                        tdt.Rows[i]["RunningBalance"] = "0";
                    }

                    if (DateTime.Parse(tdt.Rows[i]["Vdate"].ToString()).Year.ToString() == "1801")
                    {
                        tdt.Rows[i]["Dr"] = 0;
                        tdt.Rows[i]["Cr"] = 0;
                    }
                }
            }

            if (tdt.Rows.Count == 0)
            {
                return false;
            }

            string[,] col = new string[2, 3] { { "Name", "1", "0" }, { "Vdate", "1", "0" } };

            string[,] Cwidth = new string[8, 6] { 
            { "Acoount", "0", "0","","","" },
            { "Vdate", "0", "0","","","" },
            { "Documant No.", "200", "0","","","" },
            { "Particular", "400", "0","","","" },
            { "Amount Dr.", "120", "1","|sum(Dr)","","" },
            { "Amount Cr.", "120", "1" ,"|sum(Cr)","",""},
            { "Running Balance", "120", "0","","","" }, 
            { "Dr./Cr.", "40", "0","","","" } };

            CreateReport(tdt, col, Cwidth);
            return true;
        }

        public bool DetailLedger(DateTime DateFrom, DateTime DateTo, string accnm)
        {
            DataTable dtReport = new DataTable();
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
            stdt = DateFrom;
            endt = DateTo;
            dateTimePicker1.Value = DateFrom;
            dateTimePicker2.Value = DateTo;
            label3.Enabled = true;
            textBox1.Enabled = true;
            textBox1.Text = accnm;
            frmptyp = "Detail Ledger";
            this.Text = frmptyp;
            accnm = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(accnm.ToLower());
            DecsOfReport = "Ledger of " + accnm.ToUpper() + ", for the period of " + DateFrom.ToString(Database.dformat) + " to " + DateTo.ToString(Database.dformat);

            sql = "SELECT " + access_sql.fnstring("aman.Narr='Opening Balance'", access_sql.fnstring("aman.Cr>aman.Dr", "aman.Cr-aman.Dr", "0"), "aman.cr") + " AS Cr, aman.Name, aman.Vdate, aman.DocNumber, aman.Narr AS Particular, '' AS Narration, aman.Vi_id, VOUCHERDET.Description, VOUCHERDET.Quantity, " + access_sql.fnstring("aman.Narr='Opening Balance'", access_sql.fnstring("aman.Dr>aman.Cr", "aman.Dr-aman.Cr", "0"), "aman.dr") + " AS Dr, VOUCHERDET.Itemsr, VOUCHERTYPE.Type, DESCRIPTION.Pack AS Packing FROM (((((SELECT " + access_sql.Hash + "2/1/1801" + access_sql.Hash + " AS Vdate, 'OPN' AS [Short], 0 AS Vnumber, Y.Name as Name, Y.Dr,Y.Cr, 'Opening Balance' AS Narr,' ' AS DocNumber, 0 as Vi_id  FROM (SELECT X.Name, Sum(X.Dr) AS Dr, Sum(X.Cr) AS Cr FROM (SELECT 0 AS sr, ACCOUNT.Name, " + access_sql.fnstring("ACCOUNT.Balance>0", "ACCOUNT.Balance", "0") + " AS Dr, " + access_sql.fnstring("ACCOUNT.Balance<0", "-1*(ACCOUNT.Balance)", "0") + " AS Cr From ACCOUNT Union ALL  SELECT 1 AS sr, QryJournal.Name, Sum(QryJournal.Dr) AS Dr, Sum(QryJournal.Cr) AS Cr From QryJournal Where (((QryJournal.Vdate) < " + access_sql.Hash + DateFrom.ToString(Database.dformat) + access_sql.Hash + ")) GROUP BY QryJournal.Name)  AS X GROUP BY X.Name) AS Y  Union ALL SELECT JOURNAL.Vdate, VOUCHERTYPE.Short, VOUCHERINFO.Vnumber, ACCOUNT.Name,  " + access_sql.fnstring("JOURNAL.Amount>0", "JOURNAL.Amount", "0") + " AS Dr, " + access_sql.fnstring("JOURNAL.Amount<0", "-1*(JOURNAL.Amount)", "0") + " AS Cr, Journal.Narr," + access_sql.Docnumber + " AS DocNumber,VOUCHERINFO.Vi_id  From JOURNAL, ACCOUNT, Voucherinfo, VOUCHERTYPE WHERE (((JOURNAL.Ac_id)=[ACCOUNT].[Ac_id]) AND ((JOURNAL.Vi_id)=[VOUCHERINFO].[Vi_id]) AND ((VOUCHERINFO.Vt_id)=[VOUCHERTYPE].[Vt_id]) AND ((JOURNAL.Vdate)>=" + access_sql.Hash + DateFrom.ToString(Database.dformat) + access_sql.Hash + ")))  AS aman LEFT JOIN VOUCHERDET ON aman.Vi_id = VOUCHERDET.Vi_id) LEFT JOIN VOUCHERINFO ON aman.Vi_id = VOUCHERINFO.Vi_id) LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id) LEFT JOIN DESCRIPTION ON VOUCHERDET.Des_ac_id = DESCRIPTION.Des_id) ORDER BY VOUCHERDET.Itemsr";
            dt.Clear();
            Database.GetSqlData(sql, dt);
            DataRow[] drow;

            drow = dt.Select("Name='" + accnm + "' and Vdate<=" + access_sql.Hash + DateTo.ToString(Database.dformat) + access_sql.Hash + "");
            tdt.Clear();

            DataTable DtExp = new DataTable();
            Database.GetSqlData("SELECT ACCOUNT.Name, VOUCHARGES.Vi_id, VOUCHARGES.Charg_Name As Tax_Name, Sum(VOUCHARGES.amount) AS Amount FROM VOUCHARGES LEFT JOIN (VOUCHERINFO LEFT JOIN ACCOUNT ON VOUCHERINFO.Ac_id = ACCOUNT.Ac_id) ON VOUCHARGES.Vi_id = VOUCHERINFO.Vi_id GROUP BY ACCOUNT.Name, VOUCHARGES.Vi_id, VOUCHARGES.Charg_Name, VOUCHARGES.Srno HAVING (((ACCOUNT.Name)='" + textBox1.Text + "') AND ((Sum(VOUCHARGES.amount))<>0)) ORDER BY VOUCHARGES.Srno", DtExp);
            if (drow.GetLength(0) > 0)
            {
                tdt = drow.CopyToDataTable();
                tdt.DefaultView.Sort = "Vdate,DocNumber";
                tdt = tdt.DefaultView.ToTable();
                int cdr = tdt.Select("Dr<>0", "").Length;
                int ccr = tdt.Select("Cr<>0", "").Length;

                dtReport.Columns.Add("DocumentNoCr", typeof(string));
                dtReport.Columns.Add("AmountCr", typeof(decimal));
                dtReport.Columns.Add("VdateCr", typeof(DateTime));
                dtReport.Columns.Add("NarrationCr", typeof(string));

                dtReport.Columns.Add("DocumentNoDr", typeof(string));
                dtReport.Columns.Add("AmountDr", typeof(decimal));

                dtReport.Columns.Add("VdateDr", typeof(DateTime));

                dtReport.Columns.Add("NarrationDr", typeof(string));

                if (cdr > ccr)
                {
                    for (int x = 0; x < cdr * 2; x++)
                    {
                        dtReport.Rows.Add();
                        dtReport.Rows[x]["AmountDr"] = 0;
                        dtReport.Rows[x]["VdateDr"] = "01-Feb-1801";
                        dtReport.Rows[x]["AmountCr"] = 0;
                        dtReport.Rows[x]["VdateCr"] = "01-Feb-1801";
                    }
                }
                else
                {
                    for (int x = 0; x < ccr * 2; x++)
                    {
                        dtReport.Rows.Add();
                        dtReport.Rows[x]["AmountDr"] = 0;
                        dtReport.Rows[x]["VdateDr"] = "01-Feb-1801";
                        dtReport.Rows[x]["AmountCr"] = 0;
                        dtReport.Rows[x]["VdateCr"] = "01-Feb-1801";
                    }
                }

                int RowCountCr = 0;
                int RowCountDr = 0;
                string LastDocNo = "0";

                for (int i = 0; i < tdt.Rows.Count; i++)
                {
                    String StrNurr = "";
                    if (tdt.Rows[i]["Description"].ToString() != "")
                    {
                        StrNurr = tdt.Rows[i]["Description"] + " - " + tdt.Rows[i]["Quantity"].ToString() + " X " + tdt.Rows[i]["Packing"].ToString();
                    }

                    if (double.Parse(tdt.Rows[i]["Cr"].ToString()) > 0)
                    {
                        dtReport.Rows[RowCountCr]["AmountCr"] = tdt.Rows[i]["Cr"];
                        dtReport.Rows[RowCountCr]["DocumentNoCr"] = tdt.Rows[i]["DocNumber"];

                        if (LastDocNo == tdt.Rows[i]["DocNumber"].ToString() && tdt.Rows[i]["Description"].ToString() != "")
                        {
                            dtReport.Rows[RowCountCr]["AmountCr"] = 0.00;
                            dtReport.Rows[RowCountCr]["VdateCr"] = "01-Feb-1801";
                        }
                        else
                        {
                            dtReport.Rows[RowCountCr]["AmountCr"] = tdt.Rows[i]["Cr"];
                            dtReport.Rows[RowCountCr]["VdateCr"] = tdt.Rows[i]["Vdate"];
                        }
                        if (StrNurr == "")
                        {
                            dtReport.Rows[RowCountCr]["NarrationCr"] = tdt.Rows[i]["Particular"];
                        }
                        else
                        {
                            dtReport.Rows[RowCountCr]["NarrationCr"] = StrNurr;
                        }
                        RowCountCr++;
                        if (i == tdt.Rows.Count - 1 || tdt.Rows[i + 1]["DocNumber"].ToString() != tdt.Rows[i]["DocNumber"].ToString())
                        {
                            string exp = "";
                            for (int a = 0; a < DtExp.Select("Vi_id=" + tdt.Rows[i]["Vi_id"].ToString()).Length; a++)
                            {
                                if (exp == "")
                                {
                                    exp = "Exp: " + funs.IndianCurr(double.Parse(DtExp.Select("Vi_id=" + tdt.Rows[i]["Vi_id"].ToString())[a]["amount"].ToString()));
                                }
                                else
                                {
                                    exp += " + " + funs.IndianCurr(double.Parse(DtExp.Select("Vi_id=" + tdt.Rows[i]["Vi_id"].ToString())[a]["amount"].ToString()));
                                }
                            }


                            if (exp.Trim() != "")
                            {
                                dtReport.Rows[RowCountCr]["DocumentNoCr"] = tdt.Rows[i]["DocNumber"];
                                dtReport.Rows[RowCountCr]["NarrationCr"] = exp.Trim();
                                RowCountCr++;
                            }
                        }

                    }
                    else if (double.Parse(tdt.Rows[i]["Dr"].ToString()) > 0)
                    {
                        dtReport.Rows[RowCountDr]["AmountDr"] = tdt.Rows[i]["Dr"];

                        dtReport.Rows[RowCountDr]["DocumentNoDr"] = tdt.Rows[i]["DocNumber"];

                        if (LastDocNo == tdt.Rows[i]["DocNumber"].ToString() && tdt.Rows[i]["Description"].ToString() != "")
                        {
                            dtReport.Rows[RowCountDr]["AmountDr"] = 0.00;
                            dtReport.Rows[RowCountDr]["VdateDr"] = "01-Feb-1801";
                        }
                        else
                        {
                            dtReport.Rows[RowCountDr]["AmountDr"] = tdt.Rows[i]["Dr"];
                            dtReport.Rows[RowCountDr]["VdateDr"] = tdt.Rows[i]["Vdate"];
                        }

                        if (StrNurr == "")
                        {
                            dtReport.Rows[RowCountDr]["NarrationDr"] = tdt.Rows[i]["Particular"];
                        }
                        else
                        {
                            dtReport.Rows[RowCountDr]["NarrationDr"] = StrNurr;
                        }

                        RowCountDr++;
                        if (i == tdt.Rows.Count - 1 || tdt.Rows[i + 1]["DocNumber"].ToString() != tdt.Rows[i]["DocNumber"].ToString())
                        {
                            string exp = "";
                            for (int a = 0; a < DtExp.Select("Vi_id=" + tdt.Rows[i]["Vi_id"].ToString()).Length; a++)
                            {
                                if (exp == "")
                                {
                                    exp = "Exp: " + funs.IndianCurr(double.Parse(DtExp.Select("Vi_id=" + tdt.Rows[i]["Vi_id"].ToString())[a]["amount"].ToString()));
                                }
                                else
                                {
                                    exp += " + " + funs.IndianCurr(double.Parse(DtExp.Select("Vi_id=" + tdt.Rows[i]["Vi_id"].ToString())[a]["amount"].ToString()));
                                }
                            }


                            if (exp.Trim() != "")
                            {
                                dtReport.Rows[RowCountDr]["DocumentNoDr"] = tdt.Rows[i]["DocNumber"];
                                dtReport.Rows[RowCountDr]["NarrationDr"] = exp.Trim();
                                RowCountDr++;
                            }
                        }
                    }


                    LastDocNo = tdt.Rows[i]["DocNumber"].ToString();
                }

                int tcount = 0;
                if (RowCountCr > RowCountDr)
                {
                    tcount = RowCountCr;
                }
                else
                {
                    tcount = RowCountDr;
                }
                int cont = dtReport.Rows.Count;
                for (int zco = tcount; zco < cont; zco++)
                {
                    dtReport.Rows.RemoveAt(tcount);
                }

            }

            if (dtReport.Rows.Count == 0)
            {
                return false;
            }
            string BalanceDr = "";
            string BalanceCr = "";
            if (double.Parse(dtReport.Compute("sum(AmountDr)", "").ToString()) > double.Parse(dtReport.Compute("sum(AmountCr)", "").ToString()))
            {
                BalanceDr = "Balance: " + funs.IndianCurr(double.Parse(dtReport.Compute("sum(AmountDr)", "").ToString()) - double.Parse(dtReport.Compute("sum(AmountCr)", "").ToString())) + " Dr.";
            }
            else
            {
                BalanceCr = "Balance: " + funs.IndianCurr(double.Parse(dtReport.Compute("sum(AmountCr)", "").ToString()) - double.Parse(dtReport.Compute("sum(AmountDr)", "").ToString())) + " Cr.";
            }



            string[,] col = new string[0, 0];
            string[,] Cwidth = new string[8, 6] {
            
            { "", "2", "0","" ,"",""},
            { "Credit", "100", "1","|sum(AmountCr)" ,"" ,""},
            { "Date", "100", "0","" ,"",""},
            { "Narration", "298", "0",BalanceDr ,"",""},

            { "", "2", "0","" ,"",""},
            { "Debit", "100", "1","|sum(AmountDr)" ,"" ,""},
            { "Date", "100", "0","" ,"",""},
            { "Narration","298", "0",BalanceCr ,"",""}
            };

            CreateReport(dtReport, col, Cwidth);
            return true;

        }

        private string getmonth(int Month)
        {
            string month = new DateTime(1900, Month, 1).ToString("MMMM");
            return month;
        }

        public bool PendingRegister(DateTime DateFrom, DateTime DateTo, string str)
        {
            stdt = DateFrom;
            endt = DateTo;
            groupBox2.Visible = false;
            frmptyp = "Pending Register";
            dateTimePicker1.Value = DateFrom;
            dateTimePicker2.Value = DateTo;
            dateTimePicker1.Visible = false;
            dateTimePicker2.Visible = false;
            label3.Visible = false;
            textBox1.Visible = false;
            label1.Visible = false;
            label2.Visible = false;
            this.Text = frmptyp;
            DecsOfReport = "Pending Regidter, for the period of " + DateFrom.ToString(Database.dformat) + " to " + DateTo.ToString(Database.dformat);
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();

            sql = "SELECT VOUCHERINFOs.Invoiceno AS GRno, VOUCHERINFOs.Vdate AS Booking_date, ACCOUNTs.name AS Consigner, ACCOUNTs_1.name AS Consignee, DeliveryPoints_1.Name AS source, DeliveryPoints.Name AS destination, Voucherdets.Description, Voucherdets.packing, VOUCHERINFOs.DeliveryType, VOUCHERINFOs.PaymentMode AS GR_type, VOUCHERINFOs.Transport1 AS Private, VOUCHERINFOs.Transport5 AS Remark, Voucherdets.Quantity as Expr1, Voucherdets.weight as Expr2, Voucherdets.ChargedWeight as Expr3, Voucherdets.Rate_am as Expr4, Voucherdets.Amount as Expr15, CASE WHEN VOUCHERINFOs.PaymentMode = 'FOC' THEN VOUCHERINFOs.Totalamount ELSE 0 END AS total_foc, CASE WHEN VOUCHERINFOs.PaymentMode = 'Paid' THEN VOUCHERINFOs.Totalamount ELSE 0 END AS total_paid, CASE WHEN VOUCHERINFOs.PaymentMode = 'To Pay' THEN VOUCHERINFOs.Totalamount ELSE 0 END AS total_pay, CASE WHEN VOUCHERINFOs.PaymentMode = 'T.B.B.' THEN VOUCHERINFOs.Totalamount ELSE 0 END AS total_Billed, Voucherdets.exp1amt as Expr5, Voucherdets.exp2amt as Expr6, Voucherdets.exp3amt as Expr7, Voucherdets.exp4amt as Expr8, Voucherdets.exp5amt as Expr9, Voucherdets.exp6amt as Expr10, Voucherdets.exp7amt as Expr11, Voucherdets.exp8amt as Expr12, Voucherdets.exp9amt as Expr13, Voucherdets.exp10amt as Expr14, USERs.UserName FROM DeliveryPoints AS DeliveryPoints_1 RIGHT OUTER JOIN VOUCHERINFOs ON DeliveryPoints_1.DPId = VOUCHERINFOs.SId LEFT OUTER JOIN DeliveryPoints ON VOUCHERINFOs.Consigner_id = DeliveryPoints.DPId LEFT OUTER JOIN USERs ON VOUCHERINFOs.user_id = USERs.u_id LEFT OUTER JOIN Voucherdets ON VOUCHERINFOs.Vi_id = Voucherdets.Vi_id LEFT OUTER JOIN Voucherdets AS Voucherdets_1 ON VOUCHERINFOs.Vi_id = Voucherdets_1.Booking_id LEFT OUTER JOIN VOUCHERTYPEs ON VOUCHERINFOs.Vt_id = VOUCHERTYPEs.Vt_id LEFT OUTER JOIN ACCOUNTs ON VOUCHERINFOs.Ac_id = ACCOUNTs.ac_id LEFT OUTER JOIN ACCOUNTs AS ACCOUNTs_1 ON VOUCHERINFOs.Ac_id2 = ACCOUNTs_1.ac_id WHERE (VOUCHERINFOs.LocationId = '" + Database.LocationId + "') AND (VOUCHERTYPEs.Type = N'Booking') AND (Voucherdets_1.Booking_id IS NULL) AND (VOUCHERINFOs.Vdate >= " + access_sql.Hash + "" + dateTimePicker1.Value.Date.ToString(Database.dformat) + "" + access_sql.Hash + ") AND (VOUCHERINFOs.Vdate <= " + access_sql.Hash + "" + dateTimePicker2.Value.Date.ToString(Database.dformat) + "" + access_sql.Hash + ") " + str + "  ORDER BY VOUCHERINFOs.Vnumber";

            dt = new DataTable();
            Database.GetSqlData(sql, dt);

            if (dt.Rows.Count == 0)
            {
                return false;
            }

            string[,] col = new string[0, 0] { };

            string[,] Cwidth = new string[32, 6]
                {                
                {"GRno","80","","","",""},
                {"Booking Date","90","","","",""},
                {"Consigner","180","","Total","",""},
                {"Consignee","180","","","",""},
                {"Source","100","","","",""},
                {"Destination","100","","","",""},
                {"Item Name","90","","","",""},
                {"Packing","80","","","",""},
                {"Delivery Type","80","","","",""},
                {"GR Type","80","","","",""},
                {"Private","80","","","",""},
                {"Remark","80","","","",""},
                {"Quantity","80","1","|sum(Expr1)","",""},
                {"Weight","80","1","|sum(Expr2)","",""},
                {"Charged Weight (Kg)","80","1","|sum(Expr3)","",""},
                {"Rate","80","1","|sum(Expr4)","",""},
                {"Freight","80","1","|sum(Expr15)","",""},       
                {"FOC","80","1","|sum(total_foc)","",""},
                {"Paid","80","1","|sum(total_paid)","",""},
                {"To Pay","80","1","|sum(total_pay)","",""},
                {"T.B.B.","80","1","|sum(total_billed)","",""},
                {Feature.Available("Name of Expense1"),"80","1","|sum(Expr5)","",""},
                {Feature.Available("Name of Expense2"),"80","1","|sum(Expr6)","",""},
                {Feature.Available("Name of Expense3"),"80","1","|sum(Expr7)","",""},
                {Feature.Available("Name of Expense4"),"80","1","|sum(Expr8)","",""},
                {Feature.Available("Name of Expense5"),"80","1","|sum(Expr9)","",""},
                {Feature.Available("Name of Expense6"),"80","1","|sum(Expr10)","",""},
                {Feature.Available("Name of Expense7"),"80","1","","|sum(Expr11)",""},
                {Feature.Available("Name of Expense8"),"80","1","|sum(Expr12)","",""},
                {Feature.Available("Name of Expense9"),"80","1","|sum(Expr13)","",""},
                {Feature.Available("Name of Expense10"),"80","1","|sum(Expr14)","",""},                
                {"User","60","1","","",""},
                };

            CreateReport(dt, col, Cwidth);
            return true;
        }

        public bool BookingRegisterold(DateTime DateFrom, DateTime DateTo,string str1,string str2)
        {
            stdt = DateFrom;
            endt = DateTo;
            groupBox2.Visible = false;
            frmptyp = "Booking Register";
            dateTimePicker1.Value = DateFrom;
            dateTimePicker2.Value = DateTo;
            dateTimePicker1.Visible = false;
            dateTimePicker2.Visible = false;
            label3.Visible = false;
            textBox1.Visible = false;
            label1.Visible = false;
            label2.Visible = false;
            this.Text = frmptyp;
            DecsOfReport = "Booking Register, for the period of " + DateFrom.ToString(Database.dformat) + " to " + DateTo.ToString(Database.dformat);
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();

          
            //currnet
            sql = "SELECT VOUCHERINFOs_1.Invoiceno, VOUCHERINFOs_1.Vdate, VOUCHERTYPEs.Name AS Vtype, ACCOUNTs.name AS consigner,  CASE WHEN voucherinfos_1.isself = 'true' THEN ACCOUNTs_1.name + ' (SELF)' ELSE ACCOUNTs_1.name END AS consignee, DeliveryPoints.Name AS source, DeliveryPoints_1.Name AS destination, Voucherdets_1.Description, Voucherdets_1.packing, VOUCHERINFOs_1.DeliveryType, VOUCHERINFOs_1.PaymentMode AS gr_type, VOUCHERINFOs_1.Transport1 AS private, VOUCHERINFOs_1.Transport5 AS remark, Voucherdets_1.Quantity AS Expr1, Voucherdets_1.weight AS Expr2, Voucherdets_1.ChargedWeight AS Expr3, Voucherdets_1.Rate_am AS Expr4, Voucherdets_1.Amount AS Expr15,  CASE WHEN VOUCHERINFOs_1.PaymentMode = 'FOC' AND voucherdets_1.itemsr = 1 THEN VOUCHERINFOs_1.Totalamount ELSE 0 END AS total_foc, CASE WHEN VOUCHERINFOs_1.PaymentMode = 'Paid' AND voucherdets_1.itemsr = 1 THEN VOUCHERINFOs_1.Totalamount ELSE 0 END AS total_paid, CASE WHEN VOUCHERINFOs_1.PaymentMode = 'To Pay' AND voucherdets_1.itemsr = 1 THEN VOUCHERINFOs_1.Totalamount ELSE 0 END AS total_pay, CASE WHEN VOUCHERINFOs_1.PaymentMode = 'T.B.B.' AND voucherdets_1.itemsr = 1 THEN VOUCHERINFOs_1.Totalamount ELSE 0 END AS total_Billed, Voucherdets_1.exp1amt AS Expr5, Voucherdets_1.exp2amt AS Expr6, Voucherdets_1.exp3amt AS Expr7, Voucherdets_1.exp4amt AS Expr8, Voucherdets_1.exp5amt AS Expr9, Voucherdets_1.exp6amt AS Expr10, Voucherdets_1.exp7amt AS Expr11, Voucherdets_1.exp8amt AS Expr12, Voucherdets_1.exp9amt AS Expr13, Voucherdets_1.exp10amt AS Expr14, USERs.UserName, VOUCHERINFOs.Vdate AS challandate, VOUCHERINFOs.Invoiceno  AS challano, VOUCHERINFOs_1.Vi_id FROM VOUCHERINFOs RIGHT OUTER JOIN ACCOUNTs RIGHT OUTER JOIN USERs RIGHT OUTER JOIN VOUCHERINFOs AS VOUCHERINFOs_1 LEFT OUTER JOIN Voucherdets ON VOUCHERINFOs_1.Vi_id = Voucherdets.Booking_id ON USERs.u_id = VOUCHERINFOs_1.user_id LEFT OUTER JOIN Voucherdets AS Voucherdets_1 ON VOUCHERINFOs_1.Vi_id = Voucherdets_1.Vi_id LEFT OUTER JOIN DeliveryPoints AS DeliveryPoints_1 ON VOUCHERINFOs_1.SId = DeliveryPoints_1.DPId LEFT OUTER JOIN DeliveryPoints ON VOUCHERINFOs_1.Consigner_id = DeliveryPoints.DPId ON ACCOUNTs.ac_id = VOUCHERINFOs_1.Ac_id LEFT OUTER JOIN ACCOUNTs AS ACCOUNTs_1 ON VOUCHERINFOs_1.Ac_id2 = ACCOUNTs_1.ac_id LEFT OUTER JOIN VOUCHERTYPEs ON VOUCHERINFOs_1.Vt_id = VOUCHERTYPEs.Vt_id ON VOUCHERINFOs.Vi_id = Voucherdets.Vi_id WHERE " + str2 + " (VOUCHERTYPEs.Type = N'Booking') AND (VOUCHERINFOs_1.Vdate >= '" + dateTimePicker1.Value.Date.ToString(Database.dformat) + "') AND (VOUCHERINFOs_1.Vdate <= '" + dateTimePicker2.Value.Date.ToString(Database.dformat) + "') AND  (VOUCHERINFOs.LocationId IS NULL Or VOUCHERINFOs_1.LocationId = VOUCHERINFOs.LocationId) " + str1 + " AND (VOUCHERINFOs_1.Iscancel = 0) ORDER BY VOUCHERINFOs_1.Vnumber";
            dt = new DataTable();
            Database.GetSqlData(sql, dt);

            if (dt.Rows.Count == 0)
            {
                return false;
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["challandate"].ToString() == "")
                {
                    dt.Rows[i]["challandate"] = DateTime.Parse("01-Feb-1801");
                }
            }

            string[,] col = new string[0, 0] { };

            string[,] Cwidth = new string[36, 6]
                {                
                {"GRno","55","","","",""},
                {"Booking Date","70","","","",""},
                {"Voucher Type","60","","","",""},
                {"Consigner","105","","Total","",""},
                {"Consignee","105","","","",""},
                {"Source","75","","","",""},
                {"Destination","75","","","",""},
                {"Item Name","75","","","",""},
                {"Packing","50","","","",""},
                {"Delivery Type","50","","","",""},
                {"GR Type","50","","","",""},
                {"Private","50","","","",""},
                {"Remark","50","","","",""},
                {"Quantity","50","1","|sum(Expr1)","",""},
                {"Weight","50","1","|sum(Expr2)","",""},
                {"Charged Weight (Kg)","50","1","|sum(Expr3)","",""},
                {"Rate","50","1","|sum(Expr4)","",""},
                {"Freight","50","1","|sum(Expr15)","",""},       
                {"FOC","50","1","|sum(total_foc)","",""},
                {"Paid","50","1","|sum(total_paid)","",""},
                {"To Pay","50","1","|sum(total_pay)","",""},
                {"T.B.B.","50","1","|sum(total_billed)","",""},
                {Feature.Available("Name of Expense1"),"50","1","|sum(Expr5)","",""},
                {Feature.Available("Name of Expense2"),"50","1","|sum(Expr6)","",""},
                {Feature.Available("Name of Expense3"),"50","1","|sum(Expr7)","",""},
                {Feature.Available("Name of Expense4"),"50","1","|sum(Expr8)","",""},
                {Feature.Available("Name of Expense5"),"50","1","|sum(Expr9)","",""},
                {Feature.Available("Name of Expense6"),"50","1","|sum(Expr10)","",""},
                {Feature.Available("Name of Expense7"),"50","1","","|sum(Expr11)",""},
                {Feature.Available("Name of Expense8"),"50","1","|sum(Expr12)","",""},
                {Feature.Available("Name of Expense9"),"50","1","|sum(Expr13)","",""},
                {Feature.Available("Name of Expense10"),"50","1","|sum(Expr14)","",""},                
                {"User","50","1","","",""},
                {"Challan Date","60","1","","",""},
                {"Challan No","50","1","","",""},
                {"vid","0","1","","",""},
                };

            CreateReport(dt, col, Cwidth);
            dtFinal = dt.Copy();
            return true;
        }
        public bool BookingRegister(DateTime DateFrom, DateTime DateTo, string str1, string str2)
        {
            stdt = DateFrom;
            endt = DateTo;
            groupBox2.Visible = false;
            frmptyp = "Booking Register";
            dateTimePicker1.Value = DateFrom;
            dateTimePicker2.Value = DateTo;
            dateTimePicker1.Visible = false;
            dateTimePicker2.Visible = false;
            label3.Visible = false;
            textBox1.Visible = false;
            label1.Visible = false;
            label2.Visible = false;
            this.Text = frmptyp;
            DecsOfReport = "Booking Register, for the period of " + DateFrom.ToString(Database.dformat) + " to " + DateTo.ToString(Database.dformat);
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();


            //currnet
            sql = "SELECT Stocks.GRNo, Stocks.GRDate, VOUCHERTYPEs.Name, ACCOUNTs.name AS Consigner,  CASE WHEN VOUCHERINFOs.isself = 'true' THEN ACCOUNTs_1.name + ' (SELF)' ELSE ACCOUNTs_1.name END AS Consignee, DeliveryPoints.Name AS Source,   DeliveryPoints_1.Name AS Destination, Stocks.ItemName, Stocks.Packing, Stocks.DeliveryType, Stocks.GRType, Stocks.Private,  Stocks.Remark, Stocks.TotPkts,Stocks.ActWeight , Stocks.TotWeight,  Stocks.Freight, Stocks.GRCharge, Stocks.OthCharge, Stocks.FOC, Stocks.Paid,   Stocks.ToPay, Stocks.TBB, USERs.UserName, Stocks.vid as Vi_id FROM USERs RIGHT OUTER JOIN  VOUCHERINFOs ON USERs.u_id = VOUCHERINFOs.user_id LEFT OUTER JOIN  ACCOUNTs RIGHT OUTER JOIN  ACCOUNTs AS ACCOUNTs_1 RIGHT OUTER JOIN  DeliveryPoints AS DeliveryPoints_1 RIGHT OUTER JOIN  Stocks ON DeliveryPoints_1.DPId = Stocks.Destination_id ON ACCOUNTs_1.ac_id = Stocks.Consignee_id ON   ACCOUNTs.ac_id = Stocks.Consigner_id LEFT OUTER JOIN  DeliveryPoints ON Stocks.Source_id = DeliveryPoints.DPId ON VOUCHERINFOs.Vi_id = Stocks.Vid LEFT OUTER JOIN  VOUCHERTYPEs ON VOUCHERINFOs.Vt_id = VOUCHERTYPEs.Vt_id WHERE " + str2 + " (VOUCHERTYPEs.Type = 'Booking') AND (VOUCHERINFOs.Iscancel = 0)  AND   (Stocks.GRDate >= '" + dateTimePicker1.Value.Date.ToString(Database.dformat) + "') AND (Stocks.GRDate <= '" + dateTimePicker2.Value.Date.ToString(Database.dformat) + "') " + str1 + " ORDER BY VOUCHERINFOs.Vnumber ";
           // sql = "SELECT VOUCHERINFOs_1.Invoiceno, VOUCHERINFOs_1.Vdate, VOUCHERTYPEs.Name AS Vtype, ACCOUNTs.name AS consigner,  CASE WHEN voucherinfos_1.isself = 'true' THEN ACCOUNTs_1.name + ' (SELF)' ELSE ACCOUNTs_1.name END AS consignee, DeliveryPoints.Name AS source, DeliveryPoints_1.Name AS destination, Voucherdets_1.Description, Voucherdets_1.packing, VOUCHERINFOs_1.DeliveryType, VOUCHERINFOs_1.PaymentMode AS gr_type, VOUCHERINFOs_1.Transport1 AS private, VOUCHERINFOs_1.Transport5 AS remark, Voucherdets_1.Quantity AS Expr1, Voucherdets_1.weight AS Expr2, Voucherdets_1.ChargedWeight AS Expr3, Voucherdets_1.Rate_am AS Expr4, Voucherdets_1.Amount AS Expr15,  CASE WHEN VOUCHERINFOs_1.PaymentMode = 'FOC' AND voucherdets_1.itemsr = 1 THEN VOUCHERINFOs_1.Totalamount ELSE 0 END AS total_foc, CASE WHEN VOUCHERINFOs_1.PaymentMode = 'Paid' AND voucherdets_1.itemsr = 1 THEN VOUCHERINFOs_1.Totalamount ELSE 0 END AS total_paid, CASE WHEN VOUCHERINFOs_1.PaymentMode = 'To Pay' AND voucherdets_1.itemsr = 1 THEN VOUCHERINFOs_1.Totalamount ELSE 0 END AS total_pay, CASE WHEN VOUCHERINFOs_1.PaymentMode = 'T.B.B.' AND voucherdets_1.itemsr = 1 THEN VOUCHERINFOs_1.Totalamount ELSE 0 END AS total_Billed, Voucherdets_1.exp1amt AS Expr5, Voucherdets_1.exp2amt AS Expr6, Voucherdets_1.exp3amt AS Expr7, Voucherdets_1.exp4amt AS Expr8, Voucherdets_1.exp5amt AS Expr9, Voucherdets_1.exp6amt AS Expr10, Voucherdets_1.exp7amt AS Expr11, Voucherdets_1.exp8amt AS Expr12, Voucherdets_1.exp9amt AS Expr13, Voucherdets_1.exp10amt AS Expr14, USERs.UserName, VOUCHERINFOs.Vdate AS challandate, VOUCHERINFOs.Invoiceno  AS challano, VOUCHERINFOs_1.Vi_id FROM VOUCHERINFOs RIGHT OUTER JOIN ACCOUNTs RIGHT OUTER JOIN USERs RIGHT OUTER JOIN VOUCHERINFOs AS VOUCHERINFOs_1 LEFT OUTER JOIN Voucherdets ON VOUCHERINFOs_1.Vi_id = Voucherdets.Booking_id ON USERs.u_id = VOUCHERINFOs_1.user_id LEFT OUTER JOIN Voucherdets AS Voucherdets_1 ON VOUCHERINFOs_1.Vi_id = Voucherdets_1.Vi_id LEFT OUTER JOIN DeliveryPoints AS DeliveryPoints_1 ON VOUCHERINFOs_1.SId = DeliveryPoints_1.DPId LEFT OUTER JOIN DeliveryPoints ON VOUCHERINFOs_1.Consigner_id = DeliveryPoints.DPId ON ACCOUNTs.ac_id = VOUCHERINFOs_1.Ac_id LEFT OUTER JOIN ACCOUNTs AS ACCOUNTs_1 ON VOUCHERINFOs_1.Ac_id2 = ACCOUNTs_1.ac_id LEFT OUTER JOIN VOUCHERTYPEs ON VOUCHERINFOs_1.Vt_id = VOUCHERTYPEs.Vt_id ON VOUCHERINFOs.Vi_id = Voucherdets.Vi_id WHERE " + str2 + " (VOUCHERTYPEs.Type = N'Booking') AND (VOUCHERINFOs_1.Vdate >= '" + dateTimePicker1.Value.Date.ToString(Database.dformat) + "') AND (VOUCHERINFOs_1.Vdate <= '" + dateTimePicker2.Value.Date.ToString(Database.dformat) + "') AND  (VOUCHERINFOs.LocationId IS NULL Or VOUCHERINFOs_1.LocationId = VOUCHERINFOs.LocationId) " + str1 + " AND (VOUCHERINFOs_1.Iscancel = 0) ORDER BY VOUCHERINFOs_1.Vnumber";
            dt = new DataTable();
            Database.GetSqlData(sql, dt);

            if (dt.Rows.Count == 0)
            {
                return false;
            }

            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    if (dt.Rows[i]["challandate"].ToString() == "")
            //    {
            //        dt.Rows[i]["challandate"] = DateTime.Parse("01-Feb-1801");
            //    }
            //}

            string[,] col = new string[0, 0] { };

            string[,] Cwidth = new string[25, 6]
                {                
                {"GRno","60","","","",""},
                {"Booking Date","70","","","",""},
                {"Voucher Type","70","","","",""},

                {"Consigner","125","","Total","",""},
                {"Consignee","125","","","",""},

                {"Source","100","","","",""},
                {"Destination","100","","","",""},

                {"Item Name","100","","","",""},
                {"Packing","50","","","",""},
                {"Delivery Type","80","","","",""},

                {"GR Type","80","","","",""},
                {"Private","75","","","",""},


                {"Remark","100","","","",""},

                {"Quantity","70","1","|sum(TotPkts)","",""},
                {"Act Weight","75","1","|sum(ActWeight)","",""},
                {"Charged Weight","75","1","|sum(TotWeight)","",""},

                {"Freight","70","1","|sum(Freight)","",""},
                {"GR Charge","70","1","|sum(GRCharge)","",""},

                {"Oth Charge","50","1","|sum(OthCharge)","",""},    
       
                {"FOC","100","1","|sum(foc)","",""},
                {"Paid","100","1","|sum(paid)","",""},
                {"To Pay","100","1","|sum(topay)","",""},

                {"T.B.B.","100","1","|sum(tbb)","",""},     
                {"User","50","1","","",""},

                //{"Challan Date","60","1","","",""},
                //{"Challan No","50","1","","",""},
                {"vid","0","1","","",""},
                };

            CreateReport(dt, col, Cwidth);
            dtFinal = dt.Copy();
            return true;
        }
        public bool BookingRegisterNew(DateTime DateFrom, DateTime DateTo, string str1, string str2)
        {
            stdt = DateFrom;
            endt = DateTo;
            groupBox2.Visible = false;
            frmptyp = "Booking Register N";
            dateTimePicker1.Value = DateFrom;
            dateTimePicker2.Value = DateTo;
            dateTimePicker1.Visible = false;
            dateTimePicker2.Visible = false;
            label3.Visible = false;
            textBox1.Visible = false;
            label1.Visible = false;
            label2.Visible = false;
            this.Text = frmptyp;
            DecsOfReport = "Booking Register, for the period of " + DateFrom.ToString(Database.dformat) + " to " + DateTo.ToString(Database.dformat);
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();


            //currnet
            sql = "SELECT VOUCHERINFOs_1.Invoiceno, VOUCHERINFOs_1.Vdate, VOUCHERTYPEs.Name AS Vtype, ACCOUNTs.name AS consigner, CASE WHEN voucherinfos_1.isself = 'true' THEN ACCOUNTs_1.name + ' (SELF)' ELSE ACCOUNTs_1.name END AS consignee,DeliveryPoints.Name AS source, DeliveryPoints_1.Name AS destination, Voucherdets_1.Description, Voucherdets_1.packing, VOUCHERINFOs_1.DeliveryType, VOUCHERINFOs_1.PaymentMode AS gr_type, VOUCHERINFOs_1.Transport1 AS private, VOUCHERINFOs_1.Transport5 AS remark, Voucherdets_1.Quantity AS Expr1, Voucherdets_1.weight AS Expr2, Voucherdets_1.ChargedWeight AS Expr3, Voucherdets_1.Rate_am AS Expr4, Voucherdets_1.Amount AS Expr15,  CASE WHEN VOUCHERINFOs_1.PaymentMode = 'FOC' AND voucherdets_1.itemsr = 1 THEN VOUCHERINFOs_1.Totalamount ELSE 0 END AS total_foc, CASE WHEN VOUCHERINFOs_1.PaymentMode = 'Paid' AND voucherdets_1.itemsr = 1 THEN VOUCHERINFOs_1.Totalamount ELSE 0 END AS total_paid, CASE WHEN VOUCHERINFOs_1.PaymentMode = 'To Pay' AND voucherdets_1.itemsr = 1 THEN VOUCHERINFOs_1.Totalamount ELSE 0 END AS total_pay, CASE WHEN VOUCHERINFOs_1.PaymentMode = 'T.B.B.' AND voucherdets_1.itemsr = 1 THEN VOUCHERINFOs_1.Totalamount ELSE 0 END AS total_Billed, Voucherdets_1.exp1amt AS Expr5, Voucherdets_1.exp2amt AS Expr6, Voucherdets_1.exp3amt AS Expr7, Voucherdets_1.exp4amt AS Expr8, Voucherdets_1.exp5amt AS Expr9, Voucherdets_1.exp6amt AS Expr10, Voucherdets_1.exp7amt AS Expr11, Voucherdets_1.exp8amt AS Expr12, Voucherdets_1.exp9amt AS Expr13, Voucherdets_1.exp10amt AS Expr14, USERs.UserName, VOUCHERINFOs.Vdate AS challandate, VOUCHERINFOs.Invoiceno  AS challano, VOUCHERINFOs_1.Vi_id FROM VOUCHERINFOs RIGHT OUTER JOIN ACCOUNTs RIGHT OUTER JOIN USERs RIGHT OUTER JOIN VOUCHERINFOs AS VOUCHERINFOs_1 LEFT OUTER JOIN Voucherdets ON VOUCHERINFOs_1.Vi_id = Voucherdets.Booking_id ON USERs.u_id = VOUCHERINFOs_1.user_id LEFT OUTER JOIN Voucherdets AS Voucherdets_1 ON VOUCHERINFOs_1.Vi_id = Voucherdets_1.Vi_id LEFT OUTER JOIN DeliveryPoints AS DeliveryPoints_1 ON VOUCHERINFOs_1.SId = DeliveryPoints_1.DPId LEFT OUTER JOIN DeliveryPoints ON VOUCHERINFOs_1.Consigner_id = DeliveryPoints.DPId ON ACCOUNTs.ac_id = VOUCHERINFOs_1.Ac_id LEFT OUTER JOIN ACCOUNTs AS ACCOUNTs_1 ON VOUCHERINFOs_1.Ac_id2 = ACCOUNTs_1.ac_id LEFT OUTER JOIN VOUCHERTYPEs ON VOUCHERINFOs_1.Vt_id = VOUCHERTYPEs.Vt_id ON VOUCHERINFOs.Vi_id = Voucherdets.Vi_id WHERE " + str1 + " AND (VOUCHERINFOs_1.Iscancel = 0) ORDER BY VOUCHERINFOs_1.Vnumber";
            dt = new DataTable();
            Database.GetSqlData(sql, dt);

            if (dt.Rows.Count == 0)
            {
                return false;
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["challandate"].ToString() == "")
                {
                    dt.Rows[i]["challandate"] = DateTime.Parse("01-Feb-1801");
                }
            }

            string[,] col = new string[0, 0] { };

            string[,] Cwidth = new string[36, 6]
                {                
                {"GRno","55","","","",""},
                {"Booking Date","70","","","",""},
                {"Voucher Type","60","","","",""},
                {"Consigner","105","","Total","",""},

                {"Consignee","105","","","",""},
                {"Source","75","","","",""},
                {"Destination","75","","","",""},
                {"Item Name","75","","","",""},
                {"Packing","50","","","",""},
                {"Delivery Type","50","","","",""},
                {"GR Type","50","","","",""},
                {"Private","50","","","",""},
                {"Remark","50","","","",""},
                {"Quantity","50","1","|sum(Expr1)","",""},
                {"Weight","50","1","|sum(Expr2)","",""},
                {"Charged Weight (Kg)","50","1","|sum(Expr3)","",""},
                {"Rate","50","1","|sum(Expr4)","",""},
                {"Freight","50","1","|sum(Expr15)","",""},       
                {"FOC","50","1","|sum(total_foc)","",""},
                {"Paid","50","1","|sum(total_paid)","",""},
                {"To Pay","50","1","|sum(total_pay)","",""},
                {"T.B.B.","50","1","|sum(total_billed)","",""},
                {Feature.Available("Name of Expense1"),"50","1","|sum(Expr5)","",""},
                {Feature.Available("Name of Expense2"),"50","1","|sum(Expr6)","",""},
                {Feature.Available("Name of Expense3"),"50","1","|sum(Expr7)","",""},
                {Feature.Available("Name of Expense4"),"50","1","|sum(Expr8)","",""},
                {Feature.Available("Name of Expense5"),"50","1","|sum(Expr9)","",""},
                {Feature.Available("Name of Expense6"),"50","1","|sum(Expr10)","",""},
                {Feature.Available("Name of Expense7"),"50","1","","|sum(Expr11)",""},
                {Feature.Available("Name of Expense8"),"50","1","|sum(Expr12)","",""},
                {Feature.Available("Name of Expense9"),"50","1","|sum(Expr13)","",""},
                {Feature.Available("Name of Expense10"),"50","1","|sum(Expr14)","",""},                
                {"User","50","1","","",""},
                {"Challan Date","60","1","","",""},
                {"Challan No","50","1","","",""},
                {"vid","0","1","","",""},
                };

            CreateReport(dt, col, Cwidth);
            dtFinal = dt.Copy();
            return true;
        }



        public bool GSTUnloadingChallan(DateTime DateFrom, DateTime DateTo)
        {
            stdt = DateFrom;
            endt = DateTo;
            //  groupBox2.Visible = false;
            frmptyp = "GST Report Unloading Challan";
            dateTimePicker1.Value = DateFrom;
            dateTimePicker2.Value = DateTo;
            dateTimePicker1.Visible = true;
            dateTimePicker2.Visible = true;
            label3.Visible = false;
            textBox1.Visible = false;
            label1.Visible = true;
            label2.Visible = true;
            this.Text = frmptyp;
            DecsOfReport = "GST Report (Unloading Challan), for the period of " + DateFrom.ToString(Database.dformat) + " to " + DateTo.ToString(Database.dformat);
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();




            //currnet
           // sql = "SELECT  CONVERT(nvarchar, VOUCHERINFOs.Vdate, 106) AS Vdate, VOUCHERINFOs.Invoiceno, States.Sname,   DeliveryPoints.Name AS Destination, ACCOUNTs.name AS consigner, ACCOUNTs.tin_number AS GSTIN, ACCOUNTs_1.name AS consignee,   ACCOUNTs_1.tin_number AS GSTINCon, Stocks.TotPkts, Stocks.ItemName, Stocks.ActWeight, Stocks.Freight, Stocks.GRCharge,   Stocks.OthCharge, VOUCHERINFOs.Totalamount AS Amount FROM ACCOUNTs AS ACCOUNTs_1 RIGHT OUTER JOIN  VOUCHERINFOs RIGHT OUTER JOIN  Stocks ON VOUCHERINFOs.Vi_id = Stocks.vid LEFT OUTER JOIN  DeliveryPoints ON VOUCHERINFOs.SId = DeliveryPoints.DPId LEFT OUTER JOIN  ACCOUNTs ON VOUCHERINFOs.Ac_id = ACCOUNTs.ac_id ON ACCOUNTs_1.ac_id = VOUCHERINFOs.Ac_id2 LEFT OUTER JOIN  VOUCHERTYPEs ON VOUCHERINFOs.Vt_id = VOUCHERTYPEs.Vt_id LEFT OUTER JOIN  States ON ACCOUNTs_1.state_id = States.State_id WHERE ( VOUCHERTYPEs.Type = 'Booking') AND ( VOUCHERTYPEs.A = 1) AND ( VOUCHERINFOs.Vdate >= '" + DateFrom.ToString(Database.dformat) + "') AND   ( VOUCHERINFOs.Vdate <= '" + DateTo.ToString(Database.dformat) + "') AND ( VOUCHERINFOs.LocationId = '" + Database.LocationId + "') ";

            sql = "SELECT CONVERT(nvarchar, challanunloadings.grdate, 106) AS Vdate, challanunloadings.grno, States.Sname, DeliveryPoints.Name AS Destination,   ACCOUNTs.name AS consigner, ACCOUNTs.tin_number AS GSTIN, ACCOUNTs_1.name AS consignee, ACCOUNTs_1.tin_number AS GSTINCon,   challanunloadings.Quantity AS TotPkts, items.name AS ItemName, challanunloadings.ActWeight, challanunloadings.Rate_am AS Freight,   challanunloadings.grcharge AS GRcharge, challanunloadings.othcharge AS OthCharge, challanunloadings.Amount FROM ACCOUNTs AS ACCOUNTs_1 RIGHT OUTER JOIN  items RIGHT OUTER JOIN  challanunloadings ON items.Id = challanunloadings.Des_ac_id LEFT OUTER JOIN  ACCOUNTs ON challanunloadings.consigner_id = ACCOUNTs.ac_id LEFT OUTER JOIN  DeliveryPoints ON challanunloadings.destination_id = DeliveryPoints.DPId ON   ACCOUNTs_1.ac_id = challanunloadings.consignee_id RIGHT OUTER JOIN  VOUCHERINFOs ON challanunloadings.Vi_id = VOUCHERINFOs.Vi_id LEFT OUTER JOIN  VOUCHERTYPEs ON VOUCHERINFOs.Vt_id = VOUCHERTYPEs.Vt_id LEFT OUTER JOIN  States ON ACCOUNTs_1.state_id = States.State_id WHERE (VOUCHERTYPEs.Type = 'GRbyChallan') AND (VOUCHERTYPEs.A = 1) AND (challanunloadings.grdate >= '" + DateFrom.ToString(Database.dformat) + "') AND   (challanunloadings.grdate <= '" + DateTo.ToString(Database.dformat) + "') AND (VOUCHERINFOs.LocationId = '" + Database.LocationId + "') order by challanunloadings.grdate";
            dt = new DataTable();
            Database.GetSqlData(sql, dt);

            if (dt.Rows.Count == 0)
            {
                return false;
            }


            string[,] col = new string[0, 0] { };

            string[,] Cwidth = new string[15, 6]
                {       
                {"GRDate","100","","","",""},
                {"GRno","100","","","",""},
              {"StateName","150","","","",""},
              {"Destination","150","","","",""},
                {"Consigner","250","","","",""},
                 {"Consigner GSTIN","150","","","",""},
                {"Consignee","250","","","",""},
                  {"Consignee GSTIN","150","","","",""},

                 {"Quantity","100","1","|sum(Totpkts)","",""},
                {"Item Name","100","","","",""},

              
                {"Weight","100","1","|sum(ActWeight)","",""},

               
                {"Freight","100","1","|sum(Freight)","",""},       
                           
                {"GR Charge","100","1","|sum(GRCharge)","",""},
                {"Oth Charge","100","1","|sum(OthCharge)","",""},
                {"Amount","100","1","|sum(amount)","",""},
               
                };

            CreateReport(dt, col, Cwidth);
            dtFinal = dt.Copy();
            return true;
        }

        public bool GSTBooking(DateTime DateFrom, DateTime DateTo)
        {
            stdt = DateFrom;
            endt = DateTo;
          //  groupBox2.Visible = false;
            frmptyp = "Booking GST Report";
            dateTimePicker1.Value = DateFrom;
            dateTimePicker2.Value = DateTo;
            dateTimePicker1.Visible = true;
            dateTimePicker2.Visible = true;
            label3.Visible = false;
            textBox1.Visible = false;
            label1.Visible = true;
            label2.Visible = true;
            this.Text = frmptyp;
            DecsOfReport = "GST Report (Booking), for the period of " + DateFrom.ToString(Database.dformat) + " to " + DateTo.ToString(Database.dformat);
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();




            //currnet
            sql = "SELECT  CONVERT(nvarchar, VOUCHERINFOs.Vdate, 106) AS Vdate, VOUCHERINFOs.Invoiceno, States.Sname,   DeliveryPoints.Name AS Destination, ACCOUNTs.name AS consigner, ACCOUNTs.tin_number AS GSTIN, ACCOUNTs_1.name AS consignee,   ACCOUNTs_1.tin_number AS GSTINCon, Stocks.TotPkts, Stocks.ItemName, Stocks.ActWeight, Stocks.Freight, Stocks.GRCharge,   Stocks.OthCharge, VOUCHERINFOs.Totalamount AS Amount FROM ACCOUNTs AS ACCOUNTs_1 RIGHT OUTER JOIN  VOUCHERINFOs RIGHT OUTER JOIN  Stocks ON VOUCHERINFOs.Vi_id = Stocks.vid LEFT OUTER JOIN  DeliveryPoints ON VOUCHERINFOs.SId = DeliveryPoints.DPId LEFT OUTER JOIN  ACCOUNTs ON VOUCHERINFOs.Ac_id = ACCOUNTs.ac_id ON ACCOUNTs_1.ac_id = VOUCHERINFOs.Ac_id2 LEFT OUTER JOIN  VOUCHERTYPEs ON VOUCHERINFOs.Vt_id = VOUCHERTYPEs.Vt_id LEFT OUTER JOIN  States ON ACCOUNTs_1.state_id = States.State_id WHERE ( VOUCHERTYPEs.Type = 'Booking') AND ( VOUCHERTYPEs.A = 1) AND ( VOUCHERINFOs.Vdate >= '" + DateFrom.ToString(Database.dformat) + "') AND   ( VOUCHERINFOs.Vdate <= '" + DateTo.ToString(Database.dformat) + "') AND ( VOUCHERINFOs.LocationId = '" + Database.LocationId + "') order by vdate"; 
            dt = new DataTable();
            Database.GetSqlData(sql, dt);

            if (dt.Rows.Count == 0)
            {
                return false;
            }

          
            string[,] col = new string[0, 0] { };

            string[,] Cwidth = new string[15, 6]
                {       
                {"Booking Date","100","","","",""},
                {"GRno","100","","","",""},
              {"StateName","150","","","",""},
              {"Destination","150","","","",""},
                {"Consigner","250","","","",""},
                 {"Consigner GSTIN","150","","","",""},
                {"Consignee","250","","","",""},
                  {"Consignee GSTIN","150","","","",""},

                 {"Quantity","100","1","|sum(Totpkts)","",""},
                {"Item Name","100","","","",""},

              
                {"Weight","100","1","|sum(ActWeight)","",""},

               
                {"Freight","100","1","|sum(Freight)","",""},       
                           
                {"GR Charge","100","1","|sum(GRCharge)","",""},
                {"Oth Charge","100","1","|sum(OthCharge)","",""},
                {"Amount","100","1","|sum(amount)","",""},
               
                };

            CreateReport(dt, col, Cwidth);
            dtFinal = dt.Copy();
            return true;
        }

        public bool Delivery(DateTime DateFrom, DateTime DateTo, string str1,string str2)
        {
            str = str1;
            stdt = DateFrom;
            endt = DateTo;
            groupBox2.Visible = false;
            frmptyp = "Delivery Report";
            dateTimePicker1.Value = DateFrom;
            dateTimePicker2.Value = DateTo;
            dateTimePicker1.Visible = false;
            dateTimePicker2.Visible = false;
            label3.Visible = false;
            textBox1.Visible = false;
            label1.Visible = false;
            label2.Visible = false;
            this.Text = frmptyp;
            DecsOfReport = "Delivery Report, for the period of " + DateFrom.ToString(Database.dformat) + " to " + DateTo.ToString(Database.dformat);
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();


          
          
            //sql = "SELECT VOUCHERINFOs_1.PaymentMode AS PaymentMode, VOUCHERINFOs_1.Invoiceno AS DRNo, Stocks.GRDate AS BookingDate, Stocks.GRNo,   ACCOUNTs_1.name AS Consigner, ACCOUNTs.name AS Consignee, DeliveryPoints.Name AS Destination, SUM(Voucherdets_1.Quantity) AS Nug,   DeliveredBys.Name AS DeliveredBy, VOUCHERINFOs_1.Remarks, CASE WHEN Stocks.GRType = 'To Pay' THEN topay ELSE 0 END AS topayamt,  SUM(Voucherdets_1.exp1amt) AS Exp1rate, SUM(Voucherdets_1.exp2amt) AS Exp2rate, SUM(Voucherdets_1.exp3amt) AS Exp3rate, SUM(Voucherdets_1.exp4amt)  AS Exp4rate FROM ACCOUNTs AS ACCOUNTs_1 RIGHT OUTER JOIN";
            //sql += " VOUCHERINFOs AS VOUCHERINFOs_1 LEFT OUTER JOIN  ACCOUNTs RIGHT OUTER JOIN  DeliveryPoints RIGHT OUTER JOIN  Stocks RIGHT OUTER JOIN  Voucherdets AS Voucherdets_1 ON Stocks.vid = Voucherdets_1.Vi_id ON DeliveryPoints.DPId = Stocks.Destination_id ON   ACCOUNTs.ac_id = Stocks.Consignee_id ON VOUCHERINFOs_1.Vi_id = Voucherdets_1.Vi_id LEFT OUTER JOIN  DeliveredBys ON VOUCHERINFOs_1.Db_id = DeliveredBys.D_id LEFT OUTER JOIN  VOUCHERTYPEs ON VOUCHERINFOs_1.Vt_id = VOUCHERTYPEs.Vt_id ON ACCOUNTs_1.ac_id = Stocks.Consigner_id WHERE (VOUCHERTYPEs.Type = 'Delivery') AND (VOUCHERINFOs_1.LocationId = '" + str1 + "') AND (VOUCHERINFOs_1.Iscancel = 0) GROUP BY VOUCHERINFOs_1.Invoiceno, VOUCHERINFOs_1.Vdate, ACCOUNTs_1.name, ACCOUNTs.name, DeliveryPoints.Name, DeliveredBys.Name,   VOUCHERINFOs_1.Remarks, Stocks.GRDate, Stocks.GRNo, VOUCHERINFOs_1.PaymentMode, CASE WHEN Stocks.GRType = 'To Pay' THEN topay ELSE 0 END HAVING (VOUCHERINFOs_1.Vdate >=  '" + DateFrom.ToString(Database.dformat) + "') AND (VOUCHERINFOs_1.Vdate <=  '" + DateTo.ToString(Database.dformat) + "')  "+ str2 +"";


            sql = "SELECT VOUCHERINFOs_1.Vdate as DlDate, VOUCHERINFOs_1.PaymentMode, VOUCHERINFOs_1.Invoiceno AS DRNo, Stocks.GRDate AS BookingDate, Stocks.GRNo,   ACCOUNTs_1.name AS Consigner, ACCOUNTs.name AS Consignee, DeliveryPoints.Name AS Destination, SUM(Voucherdets_1.Quantity) AS Nug,   DeliveredBys.Name AS DeliveredBy, VOUCHERINFOs_1.Remarks, CASE WHEN Stocks.GRType = 'To Pay' THEN topay ELSE 0 END AS topayamt,   SUM(Voucherdets_1.exp1amt) AS Exp1rate, SUM(Voucherdets_1.exp2amt) AS Exp2rate, SUM(Voucherdets_1.exp3amt) AS Exp3rate, SUM(Voucherdets_1.exp4amt)   AS Exp4rate FROM ACCOUNTs AS ACCOUNTs_1 RIGHT OUTER JOIN ";
            sql += " VOUCHERINFOs AS VOUCHERINFOs_1 LEFT OUTER JOIN  ACCOUNTs RIGHT OUTER JOIN  DeliveryPoints RIGHT OUTER JOIN  Stocks RIGHT OUTER JOIN  Voucherdets AS Voucherdets_1 ON Stocks.vid = Voucherdets_1.Vi_id ON DeliveryPoints.DPId = Stocks.Destination_id ON   ACCOUNTs.ac_id = Stocks.Consignee_id ON VOUCHERINFOs_1.Vi_id = Voucherdets_1.Vi_id LEFT OUTER JOIN  DeliveredBys ON VOUCHERINFOs_1.Db_id = DeliveredBys.D_id LEFT OUTER JOIN  VOUCHERTYPEs ON VOUCHERINFOs_1.Vt_id = VOUCHERTYPEs.Vt_id ON ACCOUNTs_1.ac_id = Stocks.Consigner_id WHERE ( VOUCHERTYPEs.Type = 'Delivery') AND (VOUCHERINFOs_1.LocationId ='" + str1 + "') AND (VOUCHERINFOs_1.Iscancel = 0) GROUP BY VOUCHERINFOs_1.Invoiceno, VOUCHERINFOs_1.Vdate, ACCOUNTs_1.name, ACCOUNTs.name, DeliveryPoints.Name, DeliveredBys.Name,   VOUCHERINFOs_1.Remarks, Stocks.GRDate, Stocks.GRNo, VOUCHERINFOs_1.PaymentMode,   CASE WHEN Stocks.GRType = 'To Pay' THEN topay ELSE 0 END HAVING (VOUCHERINFOs_1.Vdate >=  '" + DateFrom.ToString(Database.dformat) + "' AND VOUCHERINFOs_1.Vdate <=  '" + DateTo.ToString(Database.dformat) + "')" + str2 + "";
            dt = new DataTable();
            Database.GetSqlData(sql, dt);



            if (dt.Rows.Count == 0)
            {
                return false;
            }
            dt.Columns.Add("Total",typeof(decimal));
            for (int i = 0; i < dt.Rows.Count; i++)
            {


                dt.Rows[i]["Total"] = double.Parse(dt.Rows[i]["topayamt"].ToString()) + double.Parse(dt.Rows[i]["exp1rate"].ToString()) + double.Parse(dt.Rows[i]["exp2rate"].ToString()) + double.Parse(dt.Rows[i]["exp3rate"].ToString()) + double.Parse(dt.Rows[i]["exp4rate"].ToString());

            }

            //dt.Columns.Remove("Vdate");
            string[,] col = new string[2, 3] {{"DLDate","1","1"},{"PaymentMode","1","1"} };

            string[,] Cwidth = new string[17, 6]
                {
                 {"DLDate","0","1","","",""},
                {"PaymentMode","0","1","","",""},
                {"DR No","100","","","",""},
                {"Bk. Date","140","","","",""}, 
                {"GrNo","100","","","",""},
              
               
                {"Consigner","245","","Total","",""},
                {"Consignee","245","","","",""},
               
                {"Destination","150","1","","",""},
                {"Nug","110","1","|sum(Nug)","|sum(Nug)","|sum(Nug)"},
                {"DeliveredBy","160","1","","",""},
               
                {"Rmk","80","1","","",""},

                {"Freight","140","1","|sum(topayAmt)","|sum(topayAmt)","|sum(topayAmt)"},
                {"D.C.","130","1","|sum(exp1rate)","|sum(exp1rate)","|sum(exp1rate)"},
                {"Misc.","80","1","|sum(exp2rate)","|sum(exp2rate)","|sum(exp2rate)"},
                {"STCharg","120","1","|sum(exp3rate)","|sum(exp3rate)","|sum(exp3rate)"},
                {"X.RBT","80","1","|sum(exp4rate)","|sum(exp4rate)","|sum(exp4rate)"},
                {"TotalAmt","120","1","|sum(Total)","|sum(Total)","|sum(Total)"},
                
                };

            CreateReport(dt, col, Cwidth);
            dtFinal = dt.Copy();
            return true;
        }




        public bool ChallanRegister(DateTime DateFrom, DateTime DateTo, string str1)
        {
            str = str1;
            stdt = DateFrom;
            endt = DateTo;
            groupBox2.Visible = false;
            frmptyp = "Challan Register";
            dateTimePicker1.Value = DateFrom;
            dateTimePicker2.Value = DateTo;
            dateTimePicker1.Visible = false;
            dateTimePicker2.Visible = false;
            label3.Visible = false;
            textBox1.Visible = false;
            label1.Visible = false;
            label2.Visible = false;
            this.Text = frmptyp;
            DecsOfReport = "Challan Register, for the period of " + DateFrom.ToString(Database.dformat) + " to " + DateTo.ToString(Database.dformat);
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
            

            //sql = "SELECT  Invoiceno, Vdate, Source, Destination, Drivername, GaddiNo, SUM(Qty) AS Quantity, SUM(Wht) AS Weight, SUM(total_Pay) AS toPay,   SUM(total_Paid) AS ToPaid, Unloading, CAST(Grno AS float) AS LessDC, CAST(Transport2 AS float) AS LorryFreight, CAST(Transport5 AS float) AS AdvPaid,   CAST(Transport6 AS float) AS BalFreight, CAST(Transport3 AS float) AS FreightPay, CAST(DeliveryAt AS float) AS CrossingChg, CAST(DD AS float) AS DD1,   CAST(Transport4 AS float) AS PaidFreight, CAST(DR AS float) AS DR1, Vi_id AS vid FROM(SELECT  VOUCHERINFOs_1.Vnumber, VOUCHERINFOs_1.Vi_id, VOUCHERINFOs_1.Invoiceno, VOUCHERINFOs_1.Vdate, DeliveryPoints_1.Name AS Source,    DeliveryPoints.Name AS Destination,  ACCOUNTs.name AS Drivername,  Gaddis.Gaddi_name AS GaddiNo, VOUCHERINFOs_1.LocationId,    Location.nick_name AS Unloading, VOUCHERINFOs_1.Grno, VOUCHERINFOs_1.Transport2, VOUCHERINFOs_1.Transport5,   VOUCHERINFOs_1.Transport6, VOUCHERINFOs_1.Transport3, VOUCHERINFOs_1.DeliveryAt, VOUCHERINFOs_1.DD, VOUCHERINFOs_1.Transport4,   VOUCHERINFOs_1.DR,  Stocks.TotPkts AS Qty,  Stocks.TotWeight AS Wht,  Stocks.ToPay AS total_Pay,     Stocks.TBB +  Stocks.Paid +  Stocks.FOC AS total_Paid,Stocks.Grno as gr  FROM  Voucherdets AS Voucherdets_1 LEFT OUTER JOIN   Stocks ON Voucherdets_1.Vi_id =  Stocks.vid FULL OUTER JOIN   DeliveryPoints AS DeliveryPoints_1 RIGHT OUTER JOIN   VOUCHERINFOs AS VOUCHERINFOs_1 LEFT OUTER JOIN  Location ON VOUCHERINFOs_1.unloadingpoint_id =  Location.LocationId LEFT OUTER JOIN   VOUCHERTYPEs ON VOUCHERINFOs_1.Vt_id =  VOUCHERTYPEs.Vt_id LEFT OUTER JOIN  ACCOUNTs ON VOUCHERINFOs_1.Driver_name =  ACCOUNTs.ac_id LEFT OUTER JOIN  DeliveryPoints ON VOUCHERINFOs_1.SId =  DeliveryPoints.DPId ON DeliveryPoints_1.DPId = VOUCHERINFOs_1.Consigner_id LEFT OUTER JOIN   Gaddis ON VOUCHERINFOs_1.Gaddi_id =  Gaddis.Gaddi_id ON Voucherdets_1.Vi_id = VOUCHERINFOs_1.Vi_id   WHERE( VOUCHERTYPEs.Type = 'Challan') AND (VOUCHERINFOs_1.Iscancel = 0)   GROUP BY VOUCHERINFOs_1.Vnumber, VOUCHERINFOs_1.Vi_id, VOUCHERINFOs_1.Invoiceno, VOUCHERINFOs_1.Vdate, DeliveryPoints_1.Name,    DeliveryPoints.Name,  ACCOUNTs.name,  Gaddis.Gaddi_name, VOUCHERINFOs_1.LocationId,  Location.nick_name,   VOUCHERINFOs_1.Grno, VOUCHERINFOs_1.Transport2, VOUCHERINFOs_1.Transport5, VOUCHERINFOs_1.Transport6, VOUCHERINFOs_1.Transport3,   VOUCHERINFOs_1.DeliveryAt, VOUCHERINFOs_1.DD, VOUCHERINFOs_1.Transport4, VOUCHERINFOs_1.DR,  Stocks.TotPkts,  Stocks.TotWeight,  Stocks.Grno, Stocks.ToPay,  Stocks.TBB +  Stocks.Paid +  Stocks.FOC) AS res GROUP BY Vnumber, Invoiceno, Vdate, Source, Destination, Drivername, GaddiNo, LocationId, Unloading, CAST(Grno AS float), CAST(Transport2 AS float),   CAST(Transport5 AS float), CAST(Transport6 AS float), CAST(Transport3 AS float), CAST(DeliveryAt AS float), CAST(DD AS float), CAST(Transport4 AS float),   CAST(DR AS float), Vi_id";
            //sql += " HAVING(Vdate >= '" + DateFrom.ToString(Database.dformat) + "') AND (Vdate <= '" + DateTo.ToString(Database.dformat) + "') " + str + " ORDER BY Vdate, LocationId, Vnumber";

            
            //sql = "SELECT   Invoiceno, Vdate, Source, Destination, Drivername, GaddiNo, SUM(Qty) AS Quantity, SUM(ACWeight) AS ActWeight, SUM(Wht) AS Weight,    SUM(total_Pay) AS toPay, SUM(total_Paid) AS ToPaid, Unloading, CAST(Grno AS float) AS LessDC, CAST(Transport2 AS float) AS LorryFreight, CAST(Transport5 AS float)    AS AdvPaid, CAST(Transport6 AS float) AS BalFreight, CAST(Transport3 AS float) AS FreightPay, CAST(DeliveryAt AS float) AS CrossingChg, CAST(DD AS float) AS DD1,    CAST(Transport4 AS float) AS PaidFreight, CAST(DR AS float) AS DR1, Vi_id AS vid FROM  (SELECT  VOUCHERINFOs_1.Vnumber, VOUCHERINFOs_1.Vi_id, VOUCHERINFOs_1.Invoiceno, VOUCHERINFOs_1.Vdate, DeliveryPoints_1.Name AS Source,    DeliveryPoints.Name AS Destination,  ACCOUNTs.name AS Drivername,  Gaddis.Gaddi_name AS GaddiNo, VOUCHERINFOs_1.LocationId,    Location.nick_name AS Unloading, VOUCHERINFOs_1.Grno, VOUCHERINFOs_1.Transport2, VOUCHERINFOs_1.Transport5,    VOUCHERINFOs_1.Transport6, VOUCHERINFOs_1.Transport3, VOUCHERINFOs_1.DeliveryAt, VOUCHERINFOs_1.DD, VOUCHERINFOs_1.Transport4,    VOUCHERINFOs_1.DR,  Stocks.TotPkts AS Qty,  Stocks.TotWeight AS Wht,  Stocks.ToPay AS total_Pay,    Stocks.TBB +  Stocks.Paid +  Stocks.FOC AS total_Paid,  Stocks.GRNo AS gr, SUM(VOUCHERINFOs_2.ActWeight) AS ACWeight   FROM  VOUCHERINFOs AS VOUCHERINFOs_2 RIGHT OUTER JOIN   Stocks ON VOUCHERINFOs_2.Vi_id =  Stocks.GR_id RIGHT OUTER JOIN   Voucherdets AS Voucherdets_1 ON  Stocks.vid = Voucherdets_1.Vi_id FULL OUTER JOIN   ACCOUNTs FULL OUTER JOIN   DeliveryPoints AS DeliveryPoints_1 FULL OUTER JOIN   DeliveryPoints FULL OUTER JOIN   Gaddis RIGHT OUTER JOIN   VOUCHERINFOs AS VOUCHERINFOs_1 LEFT OUTER JOIN   Location ON VOUCHERINFOs_1.unloadingpoint_id =  Location.LocationId ON  Gaddis.Gaddi_id = VOUCHERINFOs_1.Gaddi_id ON    DeliveryPoints.DPId = VOUCHERINFOs_1.SId ON DeliveryPoints_1.DPId = VOUCHERINFOs_1.Consigner_id ON    ACCOUNTs.ac_id = VOUCHERINFOs_1.Driver_name FULL OUTER JOIN   VOUCHERTYPEs ON VOUCHERINFOs_1.Vt_id =  VOUCHERTYPEs.Vt_id ON Voucherdets_1.Vi_id = VOUCHERINFOs_1.Vi_id   WHERE  ( VOUCHERTYPEs.Type = 'Challan') AND (VOUCHERINFOs_1.Iscancel = 0)   GROUP BY VOUCHERINFOs_1.Vnumber, VOUCHERINFOs_1.Vi_id, VOUCHERINFOs_1.Invoiceno, VOUCHERINFOs_1.Vdate, DeliveryPoints_1.Name,    DeliveryPoints.Name,  ACCOUNTs.name,  Gaddis.Gaddi_name, VOUCHERINFOs_1.LocationId,  Location.nick_name,    VOUCHERINFOs_1.Grno, VOUCHERINFOs_1.Transport2, VOUCHERINFOs_1.Transport5, VOUCHERINFOs_1.Transport6, VOUCHERINFOs_1.Transport3,    VOUCHERINFOs_1.DeliveryAt, VOUCHERINFOs_1.DD, VOUCHERINFOs_1.Transport4, VOUCHERINFOs_1.DR,  Stocks.TotPkts,  Stocks.TotWeight, ";
            //sql += "   Stocks.GRNo,  Stocks.ToPay,  Stocks.TBB +  Stocks.Paid +  Stocks.FOC) AS res GROUP BY Vnumber, Invoiceno, Vdate, Source, Destination, Drivername, GaddiNo, LocationId, Unloading, CAST(Grno AS float), CAST(Transport2 AS float),    CAST(Transport5 AS float), CAST(Transport6 AS float), CAST(Transport3 AS float), CAST(DeliveryAt AS float), CAST(DD AS float), CAST(Transport4 AS float),    CAST(DR AS float), Vi_id HAVING  (Vdate >= '" + DateFrom.ToString(Database.dformat) + "' ) AND (Vdate <= '" + DateTo.ToString(Database.dformat) + "') "+ str+" ORDER BY Vdate, LocationId, Vnumber ";

            sql = "SELECT Invoiceno, Vdate, Source, Destination, Drivername, GaddiNo, SUM(Qty) AS Quantity, isnull(SUM(ActWeight),SUM(Wht)) AS Actweight, SUM(Wht) AS Weight,   SUM(total_Pay) AS toPay, SUM(total_Paid) AS ToPaid, Unloading, CAST(Grno AS float) AS LessDC, CAST(Transport2 AS float) AS LorryFreight, CAST(Transport5 AS float)   AS AdvPaid, CAST(Transport6 AS float) AS BalFreight, CAST(Transport3 AS float) AS FreightPay, CAST(DeliveryAt AS float) AS CrossingChg, CAST(DD AS float) AS DD1,  CAST(Transport4 AS float) AS PaidFreight, CAST(DR AS float) AS DR1, Vi_id AS vid FROM (SELECT VOUCHERINFOs_1.Vnumber, VOUCHERINFOs_1.Vi_id, VOUCHERINFOs_1.Invoiceno, VOUCHERINFOs_1.Vdate, DeliveryPoints_1.Name AS Source,  DeliveryPoints.Name AS Destination, ACCOUNTs.name AS Drivername, Gaddis.Gaddi_name AS GaddiNo, VOUCHERINFOs_1.LocationId,  Location.nick_name AS Unloading, VOUCHERINFOs_1.Grno, VOUCHERINFOs_1.Transport2, VOUCHERINFOs_1.Transport5,  VOUCHERINFOs_1.Transport6, VOUCHERINFOs_1.Transport3, VOUCHERINFOs_1.DeliveryAt, VOUCHERINFOs_1.DD, VOUCHERINFOs_1.Transport4,  VOUCHERINFOs_1.DR, Stocks.TotPkts AS Qty, Stocks.TotWeight AS Wht, Stocks.ToPay AS total_Pay,  Stocks.TBB + Stocks.Paid + Stocks.FOC AS total_Paid, Stocks.GRNo AS gr, Stocks.ActWeight FROM VOUCHERTYPEs FULL OUTER JOIN ACCOUNTs FULL OUTER JOIN Voucherdets AS Voucherdets_1 LEFT OUTER JOIN";
            sql += "  Stocks LEFT OUTER JOIN VOUCHERINFOs ON Stocks.GR_id = VOUCHERINFOs.Vi_id ON Voucherdets_1.Vi_id = Stocks.vid FULL OUTER JOIN DeliveryPoints FULL OUTER JOIN Gaddis RIGHT OUTER JOIN VOUCHERINFOs AS VOUCHERINFOs_1 LEFT OUTER JOIN Location ON VOUCHERINFOs_1.unloadingpoint_id = Location.LocationId ON Gaddis.Gaddi_id = VOUCHERINFOs_1.Gaddi_id ON  DeliveryPoints.DPId = VOUCHERINFOs_1.SId FULL OUTER JOIN DeliveryPoints AS DeliveryPoints_1 ON VOUCHERINFOs_1.Consigner_id = DeliveryPoints_1.DPId ON  Voucherdets_1.Vi_id = VOUCHERINFOs_1.Vi_id ON ACCOUNTs.ac_id = VOUCHERINFOs_1.Driver_name ON  VOUCHERTYPEs.Vt_id = VOUCHERINFOs_1.Vt_id WHERE ( VOUCHERTYPEs.Type = 'Challan') AND (VOUCHERINFOs_1.Iscancel = 0) GROUP BY VOUCHERINFOs_1.Vnumber, VOUCHERINFOs_1.Vi_id, VOUCHERINFOs_1.Invoiceno, VOUCHERINFOs_1.Vdate, DeliveryPoints_1.Name,  DeliveryPoints.Name, ACCOUNTs.name, Gaddis.Gaddi_name, VOUCHERINFOs_1.LocationId, Location.nick_name,  VOUCHERINFOs_1.Grno, VOUCHERINFOs_1.Transport2, VOUCHERINFOs_1.Transport5, VOUCHERINFOs_1.Transport6, VOUCHERINFOs_1.Transport3,  VOUCHERINFOs_1.DeliveryAt, VOUCHERINFOs_1.DD, VOUCHERINFOs_1.Transport4, VOUCHERINFOs_1.DR, Stocks.TotPkts, Stocks.TotWeight,  Stocks.GRNo, Stocks.ToPay, Stocks.TBB + Stocks.Paid + Stocks.FOC, Stocks.ActWeight) AS res GROUP BY Vnumber, Invoiceno, Vdate, Source, Destination, Drivername, GaddiNo, LocationId, Unloading, CAST(Grno AS float), CAST(Transport2 AS float),  CAST(Transport5 AS float), CAST(Transport6 AS float), CAST(Transport3 AS float), CAST(DeliveryAt AS float), CAST(DD AS float), CAST(Transport4 AS float),  CAST(DR AS float), Vi_id HAVING (Vdate >= '" + DateFrom.ToString(Database.dformat) + "') AND (Vdate <= '" + DateTo.ToString(Database.dformat) + "') " + str + " ORDER BY Vdate, LocationId, Vnumber";
            dt = new DataTable();
            Database.GetSqlData(sql, dt);

            if (dt.Rows.Count == 0)
            {
                return false;
            }

          
            string[,] col = new string[0, 0] { };

            string[,] Cwidth = new string[22, 6]
                {                
                {"Challan No","100","","","",""},
                {"Challan Date","100","","","",""},
                {"Source","120","","","",""},
                {"Destination","120","","","",""},
                {"DriverName","100","","Total","",""},
                {"TruckNo","90","","","",""},
                {"Qty","70","1","|sum(Quantity)","",""},
                {"ActWeight","70","1","|sum(ActWeight)","",""},
                {"Wht","70","1","|sum(Weight)","",""},
                {"ToPay","80","1","|sum(ToPay)","",""},
                {"ToPaid/T.B.B.","100","1","|sum(ToPaid)","",""},
                {"UnloPoint","100","1","","",""},

                {"LessDC","100","1","|sum(LessDC)","",""},
                {"LorryFreight","100","1","|sum(LorryFreight)","",""},
                {"AdvPaid","100","1","|sum(AdvPaid)","",""},
                {"BalFreight","90","1","|sum(BalFreight)","",""},
                {"FreightPay","90","1","|sum(FreightPay)","",""},
                {"CrossingChg","100","1","|sum(CrossingChg)","",""},
                {"DD","100","1","|sum(DD1)","",""},
                {"PaidFreight","100","1","|sum(PaidFreight)","",""},
                {"DR","100","1","|sum(DR1)","",""},
                  {"vid","0","1","","",""},
                };

            CreateReport(dt, col, Cwidth);
            dtFinal = dt.Copy();
            return true;
        }

        public bool StkTransRegister(DateTime DateFrom, DateTime DateTo, string str1)
        {
            str = str1;
            stdt = DateFrom;
            endt = DateTo;
            groupBox2.Visible = false;
            frmptyp = "Stock Transfer Register";
            dateTimePicker1.Value = DateFrom;
            dateTimePicker2.Value = DateTo;
            dateTimePicker1.Visible = false;
            dateTimePicker2.Visible = false;
            label3.Visible = false;
            textBox1.Visible = false;
            label1.Visible = false;
            label2.Visible = false;
            this.Text = frmptyp;
            DecsOfReport = "Stock Transfer Register, for the period of " + DateFrom.ToString(Database.dformat) + " to " + DateTo.ToString(Database.dformat);
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();

            //sql = "SELECT  Invoiceno, Vdate, Source, Destination, Drivername, GaddiNo, SUM(Qty) AS Quantity, SUM(Wht) AS Weight, SUM(total_Pay) AS toPay,   SUM(total_Paid) AS ToPaid, Unloading, CAST(Grno AS float) AS LessDC, CAST(Transport2 AS float) AS LorryFreight, CAST(Transport5 AS float) AS AdvPaid,   CAST(Transport6 AS float) AS BalFreight, CAST(Transport3 AS float) AS FreightPay, CAST(DeliveryAt AS float) AS CrossingChg, CAST(DD AS float) AS DD1,  CAST(Transport4 AS float) AS PaidFreight, CAST(DR AS float) AS DR1, Vi_id AS vid FROM (SELECT VOUCHERINFOs_1.Vnumber, VOUCHERINFOs_1.Vi_id, VOUCHERINFOs_1.Invoiceno, VOUCHERINFOs_1.Vdate, DeliveryPoints_1.Name AS Source,  DeliveryPoints.Name AS Destination, ACCOUNTs.name AS Drivername, gaddis.Gaddi_name AS GaddiNo, VOUCHERINFOs_1.LocationId,   Location.nick_name AS Unloading, VOUCHERINFOs_1.Grno, VOUCHERINFOs_1.Transport2, VOUCHERINFOs_1.Transport5,   VOUCHERINFOs_1.Transport6, VOUCHERINFOs_1.Transport3, VOUCHERINFOs_1.DeliveryAt, VOUCHERINFOs_1.DD, VOUCHERINFOs_1.Transport4,   VOUCHERINFOs_1.DR, Stocks.TotPkts AS Qty, Stocks.TotWeight AS Wht, Stocks.ToPay AS total_Pay,   Stocks.TBB + Stocks.Paid + Stocks.FOC AS total_Paid FROM Voucherdets AS Voucherdets_1 FULL OUTER JOIN  Voucherdets RIGHT OUTER JOIN";
            //sql += " VOUCHERINFOs INNER JOIN  Stocks ON VOUCHERINFOs.Vi_id = Stocks.vid ON Voucherdets.Vi_id = VOUCHERINFOs.Vi_id ON   Voucherdets_1.Booking_id = VOUCHERINFOs.Vi_id FULL OUTER JOIN  DeliveryPoints AS DeliveryPoints_1 RIGHT OUTER JOIN  VOUCHERINFOs AS VOUCHERINFOs_1 LEFT OUTER JOIN  Location ON VOUCHERINFOs_1.unloadingpoint_id = Location.LocationId LEFT OUTER JOIN  VOUCHERTYPEs ON VOUCHERINFOs_1.Vt_id = VOUCHERTYPEs.Vt_id LEFT OUTER JOIN  ACCOUNTs ON VOUCHERINFOs_1.Driver_name = ACCOUNTs.ac_id LEFT OUTER JOIN  DeliveryPoints ON VOUCHERINFOs_1.SId = DeliveryPoints.DPId ON DeliveryPoints_1.DPId = VOUCHERINFOs_1.Consigner_id LEFT OUTER JOIN  gaddis ON VOUCHERINFOs_1.Gaddi_id = gaddis.Gaddi_id ON Voucherdets_1.Vi_id = VOUCHERINFOs_1.Vi_id  WHERE ( VOUCHERTYPEs.Type = 'Stock Transfer') AND (VOUCHERINFOs_1.Iscancel = 0)) AS res GROUP BY Vnumber, Invoiceno, Vdate, Source, Destination, Drivername, GaddiNo, LocationId, Unloading, CAST(Grno AS float), CAST(Transport2 AS float),   CAST(Transport5 AS float), CAST(Transport6 AS float), CAST(Transport3 AS float), CAST(DeliveryAt AS float), CAST(DD AS float), CAST(Transport4 AS float),  CAST(DR AS float), Vi_id HAVING (Vdate >= '" + DateFrom.ToString(Database.dformat) + "') AND (Vdate <= '" + DateTo.ToString(Database.dformat) + "')  " + str + " ORDER BY Vdate, LocationId, Vnumber";
          
            //sql = "SELECT Invoiceno, Vdate, Source, Destination, Drivername, GaddiNo, SUM(Qty) AS Quantity, SUM(Wht) AS Weight, SUM(total_Pay) AS ToPay,   SUM(total_Paid) AS ToPaid, Unloading, CAST(Grno AS float) AS LessDC, CAST(Transport2 AS float) AS LorryFreight, CAST(Transport5 AS float) AS AdvPaid,  CAST(Transport6 AS float) AS BalFreight, CAST(Transport3 AS float) AS FreightPay, CAST(DeliveryAt AS float) AS CrossingChg, CAST(DD AS float) AS DD1,  CAST(Transport4 AS float) AS PaidFreight, CAST(DR AS float) AS DR1,vid FROM (SELECT VOUCHERINFOs_1.Vnumber,VOUCHERINFOs_1.Vi_id as Vid ,VOUCHERINFOs_1.Invoiceno, VOUCHERINFOs_1.Vdate, DeliveryPoints_1 .Name AS Source,  DeliveryPoints.Name AS Destination, ACCOUNTs.name AS Drivername, Gaddis.Gaddi_name AS GaddiNo, Voucherdets.Quantity AS Qty,  Voucherdets.weight AS Wht, CASE WHEN VOUCHERINFOs.PaymentMode = 'To Pay' AND  Voucherdets.itemsr = 1 THEN VOUCHERINFOs.Totalamount ELSE 0 END AS total_pay, CASE WHEN VOUCHERINFOs.PaymentMode = 'Paid' AND  Voucherdets.itemsr = 1 THEN VOUCHERINFOs.Totalamount ELSE CASE WHEN VOUCHERINFOs.PaymentMode = 'T.B.B.' AND  Voucherdets.itemsr = 1 THEN VOUCHERINFOs.Totalamount ELSE 0 END END AS total_Paid, VOUCHERINFOs_1.LocationId,   Location.Nick_Name AS Unloading, VOUCHERINFOs_1.Grno, VOUCHERINFOs_1.Transport2, VOUCHERINFOs_1.Transport5, VOUCHERINFOs_1.Transport6,  VOUCHERINFOs_1.Transport3, VOUCHERINFOs_1.DeliveryAt, VOUCHERINFOs_1.DD, VOUCHERINFOs_1.Transport4, VOUCHERINFOs_1.DR  FROM DeliveryPoints AS DeliveryPoints_1 RIGHT OUTER JOIN VOUCHERINFOs AS VOUCHERINFOs_1 LEFT OUTER JOIN  Location ON VOUCHERINFOs_1.unloadingpoint_id = Location.LocationId LEFT OUTER JOIN";
            //sql += " VOUCHERTYPEs ON VOUCHERINFOs_1.Vt_id = VOUCHERTYPEs.Vt_id LEFT OUTER JOIN  ACCOUNTs ON VOUCHERINFOs_1.Driver_name = ACCOUNTs.ac_id LEFT OUTER JOIN  DeliveryPoints ON VOUCHERINFOs_1.SId = DeliveryPoints.DPId ON DeliveryPoints_1.DPId = VOUCHERINFOs_1.Consigner_id LEFT OUTER JOIN  Gaddis ON VOUCHERINFOs_1.Gaddi_id = Gaddis.Gaddi_id FULL OUTER JOIN  Voucherdets AS Voucherdets_1 FULL OUTER JOIN  VOUCHERINFOs LEFT OUTER JOIN  Voucherdets ON VOUCHERINFOs.Vi_id = Voucherdets.Vi_id ON Voucherdets_1.Booking_id = VOUCHERINFOs.Vi_id ON   VOUCHERINFOs_1.Vi_id = Voucherdets_1.Vi_id  WHERE VOUCHERTYPEs.Type = 'Stock Transfer'   and Voucherinfos_1.iscancel=0 ) AS res GROUP BY Vnumber, Invoiceno, Vdate, Source, Destination, Drivername, GaddiNo, LocationId, Unloading, CAST(Grno AS float) , CAST(Transport2 AS float), CAST(Transport5 AS float) ,  CAST(Transport6 AS float) , CAST(Transport3 AS float) , CAST(DeliveryAt AS float) , CAST(DD AS float),  CAST(Transport4 AS float) , CAST(DR AS float) ,vid HAVING      (Vdate >= '" + DateFrom.ToString(Database.dformat) + "' AND Vdate <= '" + DateTo.ToString(Database.dformat) + "') " + str + " ORDER BY Vdate, LocationId, Vnumber";
            //sql = "SELECT  Invoiceno, Vdate, Source, Destination, Drivername, GaddiNo, SUM(Qty) AS Quantity, SUM(Wht) AS Weight, SUM(total_Pay) AS toPay,   SUM(total_Paid) AS ToPaid, Unloading, CAST(Grno AS float) AS LessDC, CAST(Transport2 AS float) AS LorryFreight, CAST(Transport5 AS float) AS AdvPaid,   CAST(Transport6 AS float) AS BalFreight, CAST(Transport3 AS float) AS FreightPay, CAST(DeliveryAt AS float) AS CrossingChg, CAST(DD AS float) AS DD1,   CAST(Transport4 AS float) AS PaidFreight, CAST(DR AS float) AS DR1, Vi_id AS vid FROM(SELECT  VOUCHERINFOs_1.Vnumber, VOUCHERINFOs_1.Vi_id, VOUCHERINFOs_1.Invoiceno, VOUCHERINFOs_1.Vdate, DeliveryPoints_1.Name AS Source,    DeliveryPoints.Name AS Destination,  ACCOUNTs.name AS Drivername,  Gaddis.Gaddi_name AS GaddiNo, VOUCHERINFOs_1.LocationId,    Location.nick_name AS Unloading, VOUCHERINFOs_1.Grno, VOUCHERINFOs_1.Transport2, VOUCHERINFOs_1.Transport5,   VOUCHERINFOs_1.Transport6, VOUCHERINFOs_1.Transport3, VOUCHERINFOs_1.DeliveryAt, VOUCHERINFOs_1.DD, VOUCHERINFOs_1.Transport4,   VOUCHERINFOs_1.DR,  Stocks.TotPkts AS Qty,  Stocks.TotWeight AS Wht,  Stocks.ToPay AS total_Pay,     Stocks.TBB +  Stocks.Paid +  Stocks.FOC AS total_Paid ,Stocks.Grno as gr FROM  Voucherdets AS Voucherdets_1 LEFT OUTER JOIN   Stocks ON Voucherdets_1.Vi_id =  Stocks.vid FULL OUTER JOIN   DeliveryPoints AS DeliveryPoints_1 RIGHT OUTER JOIN   VOUCHERINFOs AS VOUCHERINFOs_1 LEFT OUTER JOIN  Location ON VOUCHERINFOs_1.unloadingpoint_id =  Location.LocationId LEFT OUTER JOIN   VOUCHERTYPEs ON VOUCHERINFOs_1.Vt_id =  VOUCHERTYPEs.Vt_id LEFT OUTER JOIN  ACCOUNTs ON VOUCHERINFOs_1.Driver_name =  ACCOUNTs.ac_id LEFT OUTER JOIN  DeliveryPoints ON VOUCHERINFOs_1.SId =  DeliveryPoints.DPId ON DeliveryPoints_1.DPId = VOUCHERINFOs_1.Consigner_id LEFT OUTER JOIN   Gaddis ON VOUCHERINFOs_1.Gaddi_id =  Gaddis.Gaddi_id ON Voucherdets_1.Vi_id = VOUCHERINFOs_1.Vi_id   WHERE( VOUCHERTYPEs.Type = 'Stock Transfer') AND (VOUCHERINFOs_1.Iscancel = 0)   GROUP BY Stocks.Grno, VOUCHERINFOs_1.Vnumber, VOUCHERINFOs_1.Vi_id, VOUCHERINFOs_1.Invoiceno, VOUCHERINFOs_1.Vdate, DeliveryPoints_1.Name,    DeliveryPoints.Name,  ACCOUNTs.name,  Gaddis.Gaddi_name, VOUCHERINFOs_1.LocationId,  Location.nick_name,   VOUCHERINFOs_1.Grno, VOUCHERINFOs_1.Transport2, VOUCHERINFOs_1.Transport5, VOUCHERINFOs_1.Transport6, VOUCHERINFOs_1.Transport3,   VOUCHERINFOs_1.DeliveryAt, VOUCHERINFOs_1.DD, VOUCHERINFOs_1.Transport4, VOUCHERINFOs_1.DR,  Stocks.TotPkts,  Stocks.TotWeight,   Stocks.ToPay,  Stocks.TBB +  Stocks.Paid +  Stocks.FOC) AS res GROUP BY Vnumber, Invoiceno, Vdate, Source, Destination, Drivername, GaddiNo, LocationId, Unloading, CAST(Grno AS float), CAST(Transport2 AS float),   CAST(Transport5 AS float), CAST(Transport6 AS float), CAST(Transport3 AS float), CAST(DeliveryAt AS float), CAST(DD AS float), CAST(Transport4 AS float),   CAST(DR AS float), Vi_id";
            //sql += " HAVING(Vdate >= '" + DateFrom.ToString(Database.dformat) + "') AND (Vdate <= '" + DateTo.ToString(Database.dformat) + "') " + str + " ORDER BY Vdate, LocationId, Vnumber";

            sql = "SELECT Invoiceno, Vdate, Source, Destination, Drivername, GaddiNo, SUM(Qty) AS Quantity,isnull(SUM(ActWeight),SUM(Wht)) AS Actweight, SUM(Wht) AS Weight,   SUM(total_Pay) AS toPay, SUM(total_Paid) AS ToPaid, Unloading, CAST(Grno AS float) AS LessDC, CAST(Transport2 AS float) AS LorryFreight, CAST(Transport5 AS float)   AS AdvPaid, CAST(Transport6 AS float) AS BalFreight, CAST(Transport3 AS float) AS FreightPay, CAST(DeliveryAt AS float) AS CrossingChg, CAST(DD AS float) AS DD1,  CAST(Transport4 AS float) AS PaidFreight, CAST(DR AS float) AS DR1, Vi_id AS vid FROM (SELECT VOUCHERINFOs_1.Vnumber, VOUCHERINFOs_1.Vi_id, VOUCHERINFOs_1.Invoiceno, VOUCHERINFOs_1.Vdate, DeliveryPoints_1.Name AS Source,  DeliveryPoints.Name AS Destination, ACCOUNTs.name AS Drivername, Gaddis.Gaddi_name AS GaddiNo, VOUCHERINFOs_1.LocationId,  Location.nick_name AS Unloading, VOUCHERINFOs_1.Grno, VOUCHERINFOs_1.Transport2, VOUCHERINFOs_1.Transport5,  VOUCHERINFOs_1.Transport6, VOUCHERINFOs_1.Transport3, VOUCHERINFOs_1.DeliveryAt, VOUCHERINFOs_1.DD, VOUCHERINFOs_1.Transport4,  VOUCHERINFOs_1.DR, Stocks.TotPkts AS Qty, Stocks.TotWeight AS Wht, Stocks.ToPay AS total_Pay,  Stocks.TBB + Stocks.Paid + Stocks.FOC AS total_Paid, Stocks.GRNo AS gr, Stocks.ActWeight FROM VOUCHERTYPEs FULL OUTER JOIN ACCOUNTs FULL OUTER JOIN Voucherdets AS Voucherdets_1 LEFT OUTER JOIN";
            sql += "  Stocks LEFT OUTER JOIN VOUCHERINFOs ON Stocks.GR_id = VOUCHERINFOs.Vi_id ON Voucherdets_1.Vi_id = Stocks.vid FULL OUTER JOIN DeliveryPoints FULL OUTER JOIN Gaddis RIGHT OUTER JOIN VOUCHERINFOs AS VOUCHERINFOs_1 LEFT OUTER JOIN Location ON VOUCHERINFOs_1.unloadingpoint_id = Location.LocationId ON Gaddis.Gaddi_id = VOUCHERINFOs_1.Gaddi_id ON  DeliveryPoints.DPId = VOUCHERINFOs_1.SId FULL OUTER JOIN DeliveryPoints AS DeliveryPoints_1 ON VOUCHERINFOs_1.Consigner_id = DeliveryPoints_1.DPId ON  Voucherdets_1.Vi_id = VOUCHERINFOs_1.Vi_id ON ACCOUNTs.ac_id = VOUCHERINFOs_1.Driver_name ON  VOUCHERTYPEs.Vt_id = VOUCHERINFOs_1.Vt_id WHERE ( VOUCHERTYPEs.Type = 'Stock Transfer') AND (VOUCHERINFOs_1.Iscancel = 0) GROUP BY VOUCHERINFOs_1.Vnumber, VOUCHERINFOs_1.Vi_id, VOUCHERINFOs_1.Invoiceno, VOUCHERINFOs_1.Vdate, DeliveryPoints_1.Name,  DeliveryPoints.Name, ACCOUNTs.name, Gaddis.Gaddi_name, VOUCHERINFOs_1.LocationId, Location.nick_name,  VOUCHERINFOs_1.Grno, VOUCHERINFOs_1.Transport2, VOUCHERINFOs_1.Transport5, VOUCHERINFOs_1.Transport6, VOUCHERINFOs_1.Transport3,  VOUCHERINFOs_1.DeliveryAt, VOUCHERINFOs_1.DD, VOUCHERINFOs_1.Transport4, VOUCHERINFOs_1.DR, Stocks.TotPkts, Stocks.TotWeight,  Stocks.GRNo, Stocks.ToPay, Stocks.TBB + Stocks.Paid + Stocks.FOC, Stocks.ActWeight) AS res GROUP BY Vnumber, Invoiceno, Vdate, Source, Destination, Drivername, GaddiNo, LocationId, Unloading, CAST(Grno AS float), CAST(Transport2 AS float),  CAST(Transport5 AS float), CAST(Transport6 AS float), CAST(Transport3 AS float), CAST(DeliveryAt AS float), CAST(DD AS float), CAST(Transport4 AS float),  CAST(DR AS float), Vi_id HAVING (Vdate >= '" + DateFrom.ToString(Database.dformat) + "') AND (Vdate <= '" + DateTo.ToString(Database.dformat) + "') " + str + " ORDER BY Vdate, LocationId, Vnumber";
            dt = new DataTable();
            Database.GetSqlData(sql, dt);

            if (dt.Rows.Count == 0)
            {
                return false;
            }


            string[,] col = new string[0, 0] { };

            string[,] Cwidth = new string[22, 6]
                {                
                {"StkTrans No","100","","","",""},
                {"Date","100","","","",""},
                {"Source","120","","","",""},
                {"Destination","120","","","",""},
                {"DriverName","100","","Total","",""},
                {"TruckNo","90","","","",""},
                {"Qty","70","1","|sum(Quantity)","",""},
                {"ActWeight","70","1","|sum(ActWeight)","",""},
                {"Wht","70","1","|sum(Weight)","",""},
                {"ToPay","80","1","|sum(ToPay)","",""},
                {"ToPaid/T.B.B.","100","1","|sum(ToPaid)","",""},
                {"UnloPoint","100","1","","",""},

                {"LessDC","100","1","|sum(LessDC)","",""},
                {"LorryFreight","100","1","|sum(LorryFreight)","",""},
                {"AdvPaid","100","1","|sum(AdvPaid)","",""},
                {"BalFreight","80","1","|sum(BalFreight)","",""},
                {"FreightPay","100","1","|sum(FreightPay)","",""},
                {"CrossingChg","100","1","|sum(CrossingChg)","",""},
                {"DD","100","1","|sum(DD1)","",""},
                {"PaidFreight","100","1","|sum(PaidFreight)","",""},
                {"DR","100","1","|sum(DR1)","",""},
                {"vid","0","1","","",""},
                };

            CreateReport(dt, col, Cwidth);
            dtFinal = dt.Copy();
            return true;
        }


        public bool DestinationWise(DateTime DateFrom, DateTime DateTo, string station)
        {
            stdt = DateFrom;
            endt = DateTo;
            frmptyp = "Destination Wise";
            dateTimePicker1.Value = DateFrom;
            dateTimePicker2.Value = DateTo;
            dateTimePicker1.Visible = false;
            dateTimePicker2.Visible = false;
            label3.Text = "Station";
            label1.Visible = false;
            label2.Visible = false;
            //AccName = station;
            this.Text = frmptyp;
            textBox1.Text = station;
            DecsOfReport = "Destination Wise Report, for the period of " + DateFrom.ToString(Database.dformat) + " to " + DateTo.ToString(Database.dformat);
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();

            sql = "SELECT VOUCHERINFOs.Vi_id, VOUCHERINFOs.Vdate AS Booking_date, VOUCHERINFOs.Vnumber AS GRno, ACCOUNTs.name AS Consigner, ACCOUNTs_1.name AS Consignee, DeliveryPoints_1.Name AS source, DeliveryPoints.Name AS destination, VOUCHERINFOs.DeliveryType, VOUCHERINFOs.PaymentMode AS GR_type, VOUCHERINFOs.Transport1 AS Private, VOUCHERINFOs.Transport5 AS Remark, SUM(Voucherdets.Quantity) AS Total_quantity, SUM(Voucherdets.weight) AS Total_weight, VOUCHERINFOs.Totalamount AS total_amount, SUM(Voucherdets.Rate_am) AS Freight, Voucherdets.exp8amt AS door_delivery, CASE WHEN VOUCHERINFOs.PaymentMode = 'FOC' THEN VOUCHERINFOs.Totalamount ELSE 0 END AS total_foc, CASE WHEN VOUCHERINFOs.PaymentMode = 'Paid' THEN VOUCHERINFOs.Totalamount ELSE 0 END AS total_paid, CASE WHEN VOUCHERINFOs.PaymentMode = 'To Pay' THEN VOUCHERINFOs.Totalamount ELSE 0 END AS total_pay, CASE WHEN VOUCHERINFOs.PaymentMode = 'T.B.B.' THEN VOUCHERINFOs.Totalamount ELSE 0 END AS total_Billed FROM Voucherdets RIGHT OUTER JOIN VOUCHERINFOs ON Voucherdets.Vi_id = VOUCHERINFOs.Vi_id LEFT OUTER JOIN Voucherdets AS Voucherdets_1 ON VOUCHERINFOs.Vi_id = Voucherdets_1.Booking_id LEFT OUTER JOIN VOUCHERTYPEs ON VOUCHERINFOs.Vt_id = VOUCHERTYPEs.Vt_id LEFT OUTER JOIN DeliveryPoints ON VOUCHERINFOs.SId = DeliveryPoints.DPId LEFT OUTER JOIN ACCOUNTs ON VOUCHERINFOs.Ac_id = ACCOUNTs.ac_id LEFT OUTER JOIN ACCOUNTs AS ACCOUNTs_1 ON VOUCHERINFOs.Ac_id2 = ACCOUNTs_1.ac_id LEFT OUTER JOIN DeliveryPoints AS DeliveryPoints_1 ON VOUCHERINFOs.Consigner_id = DeliveryPoints_1.DPId WHERE (VOUCHERINFOs.LocationId = '" + Database.LocationId + "') AND (VOUCHERTYPEs.Type = N'Booking') AND (Voucherdets_1.Booking_id IS NULL) GROUP BY VOUCHERINFOs.Vi_id, VOUCHERINFOs.Vdate, VOUCHERINFOs.Vnumber, ACCOUNTs.name, ACCOUNTs_1.name, DeliveryPoints_1.Name, DeliveryPoints.Name, VOUCHERINFOs.DeliveryType, VOUCHERINFOs.PaymentMode, VOUCHERINFOs.Transport1, VOUCHERINFOs.Transport5, VOUCHERINFOs.Totalamount, Voucherdets.exp8amt HAVING (DeliveryPoints.Name = '" + station + "')";

            dt = new DataTable();
            Database.GetSqlData(sql, dt);

            if (dt.Rows.Count == 0)
            {
                return false;
            }

            dt.Columns.Remove("Vi_id");

            string[,] col = new string[0, 3] {};

            string[,] Cwidth = new string[19, 6]
                {
                {"Booking Date","90","","","",""},
                {"GRno","80","","","",""},
                {"Consigner","180","","","",""},
                {"Consignee","180","","","",""},
                {"Source","120","","","",""},
                {"Destination","120","","","",""},
                {"Delivery Type","80","","","",""},
                {"GR Type","80","","","",""},
                {"Private","80","","","",""},
                {"Remark","80","","","",""},
                {"Quantity","80","1","","",""},
                {"Weight","80","1","","",""},
                {"Amount","80","1","","",""},
                {"Freight","80","1","","",""},
                {"Door Delivery","80","1","","",""},
                {"FOC","80","1","","",""},
                {"Paid","80","1","","",""},
                {"Pay","80","1","","",""},
                {"Billed","80","1","","",""}
                };

            CreateReport(dt, col, Cwidth);
            return true;
        }

        public bool Insurance(DateTime DateFrom, DateTime DateTo)
        {
            stdt = DateFrom;
            endt = DateTo;
            frmptyp = "Insurance Due Date";
            dateTimePicker1.Value = DateFrom;
            dateTimePicker2.Value = DateTo;
            label3.Visible = false;
            textBox1.Visible = false;
            label3.Enabled = true;
            textBox1.Enabled = true;
            this.Text = frmptyp;
            DecsOfReport = "Customer Report, for the period of " + DateFrom.ToString(Database.dformat) + " to " + DateTo.ToString(Database.dformat);
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();

            sql = "SELECT Gaddis.Induedate, Gaddis.Gaddi_name, ACCOUNTs.Name AS Driver, ACCOUNTs.Phone FROM Gaddis LEFT JOIN ACCOUNTs ON Gaddis.Driver_id = ACCOUNTs.Ac_id WHERE (((Gaddis.Induedate)>=" + access_sql.Hash + "" + dateTimePicker1.Value.Date.ToString(Database.dformat) + "" + access_sql.Hash + " And (Gaddis.Induedate)<=" + access_sql.Hash + "" + dateTimePicker2.Value.Date.ToString(Database.dformat) + "" + access_sql.Hash + ")) ORDER BY Gaddis.Induedate DESC, Gaddis.Gaddi_name, ACCOUNTs.Name";
            dt = new DataTable();
            Database.GetSqlData(sql, dt);
            if (dt.Rows.Count == 0)
            {
                return false;
            }
            string[,] col = new string[1, 3] { { "Induedate", "1", "0" } };

            string[,] Cwidth = new string[4, 6]
                {
                {"InsuranceDate","","","","",""},
                {"Gaadi No","350","","","",""},
                {"Driver","330","","","",""},
                {"Contact No ","300","","","",""}
                };

            CreateReport(dt, col, Cwidth);
            return true;
        }

        public bool permit(DateTime DateFrom, DateTime DateTo)
        {
            stdt = DateFrom;
            endt = DateTo;
            frmptyp = "Permit Due Date";
            dateTimePicker1.Value = DateFrom;
            dateTimePicker2.Value = DateTo;
            label3.Visible = false;
            textBox1.Visible = false;
            label3.Enabled = true;
            textBox1.Enabled = true;
            this.Text = frmptyp;
            DecsOfReport = "Customer Report, for the period of " + DateFrom.ToString(Database.dformat) + " to " + DateTo.ToString(Database.dformat);
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
           
            sql = "SELECT Gaddis.Perduedate, Gaddis.Gaddi_name, ACCOUNTs.Name AS Driver, ACCOUNTs.Phone FROM Gaddis LEFT JOIN ACCOUNTs ON Gaddis.Driver_id = ACCOUNTs.Ac_id WHERE (((Gaddis.Perduedate)>=" + access_sql.Hash + "" + dateTimePicker1.Value.Date.ToString(Database.dformat) + "" + access_sql.Hash + " And (Gaddis.Perduedate)<=" + access_sql.Hash + "" + dateTimePicker2.Value.Date.ToString(Database.dformat) + "" + access_sql.Hash + ")) ORDER BY Gaddis.Perduedate DESC, Gaddis.Gaddi_name, ACCOUNTs.Name";
            dt = new DataTable();
            Database.GetSqlData(sql, dt);

            if (dt.Rows.Count == 0)
            {
                return false;
            }

            string[,] col = new string[1, 3] { { "Perduedate", "1", "0" } };

            string[,] Cwidth = new string[4, 6]
                {
                {"PermitDate","","","","",""},
                {"Gaadi No","350","","","",""},
                {"Driver","330","","","",""},
                {"Contact No ","300","","","",""}
                };

            CreateReport(dt, col, Cwidth);
            return true;
        }

        public bool Fitness(DateTime DateFrom, DateTime DateTo)
        {
            stdt = DateFrom;
            endt = DateTo;
            frmptyp = "Fitness Due Date";
            dateTimePicker1.Value = DateFrom;
            dateTimePicker2.Value = DateTo;
            label3.Visible = false;

            //AccName = accnm;
            textBox1.Visible = false;
            label3.Enabled = true;
            textBox1.Enabled = true;
            this.Text = frmptyp;
            DecsOfReport = "Customer Report, for the period of " + DateFrom.ToString(Database.dformat) + " to " + DateTo.ToString(Database.dformat);
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
            //sql = "SELECT ACCOUNT.fitduedate, ACCOUNT.AccName, ACCOUNT.Dname, ACCOUNT.Dlno FROM ACCOUNT WHERE (((ACCOUNT.fitduedate)>=#" + dateTimePicker1.Value.Date.ToString(Database.dformat) + "# And (ACCOUNT.fitduedate)<=#" + dateTimePicker2.Value.Date.ToString(Database.dformat) + "#) AND ((ACCOUNT.Act_id)=51)) ORDER BY ACCOUNT.fitduedate, ACCOUNT.AccName";
            sql = "SELECT Gaddis.fitduedate, Gaddis.Gaddi_name, ACCOUNTs.Name AS Driver, ACCOUNTs.Phone FROM Gaddis LEFT JOIN ACCOUNTs ON Gaddis.Driver_id = ACCOUNTs.Ac_id WHERE (((Gaddis.fitduedate)>=" + access_sql.Hash + "" + dateTimePicker1.Value.Date.ToString(Database.dformat) + "" + access_sql.Hash + " And (Gaddis.fitduedate)<=" + access_sql.Hash + "" + dateTimePicker2.Value.Date.ToString(Database.dformat) + "" + access_sql.Hash + ")) ORDER BY Gaddis.fitduedate DESC, Gaddis.Gaddi_name, ACCOUNTs.Name";
            dt = new DataTable();
            Database.GetSqlData(sql, dt);

            if (dt.Rows.Count == 0)
            {
                return false;
            }





            string[,] col = new string[1, 3] { { "fitduedate", "1", "0" } };

            string[,] Cwidth = new string[4, 6]
                {
                {"FitnessDate","","","","",""},
                {"Gaadi No","350","","","",""},
                {"Driver","330","","","",""},
                {"Contact No ","300","","","",""}
                }

                ;

            CreateReport(dt, col, Cwidth);
            return true;

        }


        public bool Fiveyears(DateTime DateFrom, DateTime DateTo)
        {
            stdt = DateFrom;
            endt = DateTo;
            frmptyp = "Fiveyears Due Date";
            dateTimePicker1.Value = DateFrom;
            dateTimePicker2.Value = DateTo;
            label3.Visible = false;

            //AccName = accnm;
            textBox1.Visible = false;
            label3.Enabled = true;
            textBox1.Enabled = true;
            this.Text = frmptyp;
            DecsOfReport = "Customer Report, for the period of " + DateFrom.ToString(Database.dformat) + " to " + DateTo.ToString(Database.dformat);
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
            //sql="SELECT ACCOUNT.fiveduedate, ACCOUNT.AccName, ACCOUNT.Dname, ACCOUNT.Dlno FROM ACCOUNT WHERE (((ACCOUNT.fiveduedate)>=#" + dateTimePicker1.Value.Date.ToString(Database.dformat) + "# And (ACCOUNT.fiveduedate)<=#" + dateTimePicker2.Value.Date.ToString(Database.dformat) + "#) AND ((ACCOUNT.Act_id)=51)) ORDER BY ACCOUNT.fiveduedate, ACCOUNT.AccName";
            sql = "SELECT Gaddis.fiveduedate, Gaddis.Gaddi_name, ACCOUNTs.Name AS Driver, ACCOUNTs.Phone FROM Gaddis LEFT JOIN ACCOUNTs ON Gaddis.Driver_id = ACCOUNTs.Ac_id WHERE (((Gaddis.fiveduedate)>=" + access_sql.Hash + "" + dateTimePicker1.Value.Date.ToString(Database.dformat) + "" + access_sql.Hash + " And (Gaddis.fiveduedate)<=" + access_sql.Hash + "" + dateTimePicker2.Value.Date.ToString(Database.dformat) + "" + access_sql.Hash + ")) ORDER BY Gaddis.fiveduedate DESC, Gaddis.Gaddi_name, ACCOUNTs.Name";
            dt = new DataTable();
            Database.GetSqlData(sql, dt);

            if (dt.Rows.Count == 0)
            {
                return false;
            }





            string[,] col = new string[1, 3] { { "fiveduedate", "1", "0" } };

            string[,] Cwidth = new string[4, 6]
                {
                {"FiveYearsDate","","","","",""},
                {"Gaadi No","350","","","",""},
                {"Driver","330","","","",""},
                {"Contact No ","300","","","",""}
                }

                ;

            CreateReport(dt, col, Cwidth);
            return true;

        }


        public bool Pollution(DateTime DateFrom, DateTime DateTo)
        {
            stdt = DateFrom;
            endt = DateTo;
            frmptyp = "Pollution Due Date";
            dateTimePicker1.Value = DateFrom;
            dateTimePicker2.Value = DateTo;
            label3.Visible = false;

            textBox1.Visible = false;
            label3.Enabled = true;
            textBox1.Enabled = true;
            this.Text = frmptyp;
            DecsOfReport = "Customer Report, for the period of " + DateFrom.ToString(Database.dformat) + " to " + DateTo.ToString(Database.dformat);
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
            
            sql = "SELECT Gaddis.pollduedate, Gaddis.Gaddi_name, ACCOUNTs.Name AS Driver, ACCOUNTs.Phone FROM Gaddis LEFT JOIN ACCOUNTs ON Gaddis.Driver_id = ACCOUNTs.Ac_id WHERE (((Gaddis.pollduedate)>=" + access_sql.Hash + "" + dateTimePicker1.Value.Date.ToString(Database.dformat) + "" + access_sql.Hash + " And (Gaddis.pollduedate)<=" + access_sql.Hash + "" + dateTimePicker2.Value.Date.ToString(Database.dformat) + "" + access_sql.Hash + ")) ORDER BY Gaddis.pollduedate DESC, Gaddis.Gaddi_name, ACCOUNTs.Name";
            dt = new DataTable();
            Database.GetSqlData(sql, dt);

            if (dt.Rows.Count == 0)
            {
                return false;
            }





            string[,] col = new string[1, 3] { { "pollduedate", "1", "0" } };

            string[,] Cwidth = new string[4, 6]
                {
                {"PollutionDate","","","","",""},
                {"Gaadi No","350","","","",""},
                {"Driver","330","","","",""},
                {"Contact No ","300","","","",""}
                }

                ;

            CreateReport(dt, col, Cwidth);
            return true;

        }

        public bool Ledger(DateTime DateFrom, DateTime DateTo, string accnm)
        {
            checkBox1.Text = "Summarized";
           //checkBox1.Checked = true;
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
            double totdr = 0;
            double totcr = 0;
            stdt = DateFrom;
            endt = DateTo;
            dateTimePicker1.Value = DateFrom;
            dateTimePicker2.Value = DateTo;
            label3.Enabled = true;
            textBox1.Enabled = true;
            textBox1.Text = accnm;
            frmptyp = "Ledger";
            this.Text = frmptyp;
            checkBox1.Visible = true;
            DecsOfReport = "Ledger \n " + accnm + "\n" + DateFrom.ToString(Database.dformat) + " - " + DateTo.ToString(Database.dformat);
            double op1 = 0, op2 = 0, opening = 0;

           
                op1 = Database.GetScalarDecimal("SELECT Balance FROM ACCOUNTs WHERE Name = '" + accnm + "'");
            
            if (Database.DatabaseType == "sql")
            {
                
                    op2 = Database.GetScalarDecimal("SELECT SUM(Journals.Amount) AS Amount FROM VOUCHERTYPEs RIGHT OUTER JOIN VOUCHERINFOs ON VOUCHERTYPEs.Vt_id = VOUCHERINFOs.Vt_id RIGHT OUTER JOIN Journals LEFT OUTER JOIN ACCOUNTs ON Journals.Ac_id = ACCOUNTs.Ac_id ON VOUCHERINFOs.Vi_id = Journals.Vi_id WHERE (ACCOUNTs.Name = '" + accnm + "') AND (Journals.Vdate < " + access_sql.Hash + DateFrom.ToString(Database.dformat) + access_sql.Hash + ") AND (VOUCHERTYPEs.A = " + access_sql.Singlequote + "true" + access_sql.Singlequote + ")");

                    if (checkBox1.Checked==true)
                    {
                        sql = "SELECT Journals.Vdate, VOUCHERTYPEs.Type AS vtype,VOUCHERINFOs.Invoiceno, '' as ToBy, ACCOUNTs_1.Name AS Particular, 0.001 AS Amount,Journals.Narr,Journals.Reffno as Reff , " + access_sql.fnstring("SUM(Journals.Amount) > 0", "SUM(Journals.Amount)", "0") + " AS Dr, " + access_sql.fnstring("SUM(Journals.Amount) < 0", "-1 * SUM(Journals.Amount)", "0") + " AS Cr, 0.001 as RunningBalance, '' as Dr_Cr, Journals.Vi_id FROM VOUCHERINFOs LEFT OUTER JOIN VOUCHERTYPEs ON VOUCHERINFOs.Vt_id = VOUCHERTYPEs.Vt_id RIGHT OUTER JOIN ACCOUNTs RIGHT OUTER JOIN Journals LEFT OUTER JOIN ACCOUNTs AS ACCOUNTs_1 ON Journals.Opp_acid = ACCOUNTs_1.Ac_id ON ACCOUNTs.Ac_id = Journals.Ac_id ON VOUCHERINFOs.Vi_id = Journals.Vi_id WHERE (VOUCHERTYPEs.A =  " + access_sql.Singlequote + "true" + access_sql.Singlequote + ") GROUP BY Journals.Vdate, VOUCHERINFOs.Invoiceno, Journals.Vi_id, Journals.Narr,Journals.Reffno, ACCOUNTs.Name, ACCOUNTs_1.Name, VOUCHERTYPEs.Type HAVING (Journals.Vdate >=" + access_sql.Hash + DateFrom.ToString(Database.dformat) + access_sql.Hash + ") AND (Journals.Vdate <= " + access_sql.Hash + DateTo.ToString(Database.dformat) + access_sql.Hash + ") AND (ACCOUNTs.Name = '" + accnm + "')  ORDER BY Journals.Vdate, VOUCHERTYPEs.Type, VOUCHERINFOs.Invoiceno, Max(Journals.Sno) ";
                    }

                    else 
                    {
                        sql = "SELECT     dbo.Journals.Vdate,dbo.VOUCHERTYPEs.Type AS vtype, dbo.VOUCHERINFOs.Invoiceno,  '' AS ToBy, CASE WHEN SUM(Journals.amount) > 0 THEN ACCOUNTs.Name ELSE ACCOUNTs_3.Name END AS Particular, dbo.Journals.Narr,'' As Reff, CASE WHEN SUM(Journals.Amount) > 0 THEN SUM(Journals.Amount)  ELSE 0 END AS Dr, CASE WHEN SUM(Journals.Amount) < 0 THEN - 1 * SUM(Journals.Amount) ELSE 0 END AS Cr, 0.001 AS RunningBalance, '' AS Dr_Cr, dbo.Journals.Vi_id FROM         dbo.VOUCHERINFOs LEFT OUTER JOIN   dbo.ACCOUNTs ON dbo.VOUCHERINFOs.Cr_Ac_id = dbo.ACCOUNTs.Ac_id LEFT OUTER JOIN   dbo.ACCOUNTs AS ACCOUNTs_3 ON dbo.VOUCHERINFOs.Dr_Ac_id = ACCOUNTs_3.Ac_id LEFT OUTER JOIN  dbo.VOUCHERTYPEs ON dbo.VOUCHERINFOs.Vt_id = dbo.VOUCHERTYPEs.Vt_id RIGHT OUTER JOIN  dbo.ACCOUNTs AS ACCOUNTs_2 RIGHT OUTER JOIN  dbo.Journals LEFT OUTER JOIN   dbo.ACCOUNTs AS ACCOUNTs_1 ON dbo.Journals.Opp_acid = ACCOUNTs_1.Ac_id ON ACCOUNTs_2.Ac_id = dbo.Journals.Ac_id ON   dbo.VOUCHERINFOs.Vi_id = dbo.Journals.Vi_id WHERE     (dbo.VOUCHERTYPEs.A = 'true') GROUP BY dbo.Journals.Vdate, dbo.VOUCHERINFOs.Invoiceno, dbo.Journals.Vi_id, dbo.Journals.Narr, ACCOUNTs_2.Name, dbo.VOUCHERTYPEs.Type, ACCOUNTs_3.Name,  dbo.ACCOUNTs.Name HAVING      (dbo.Journals.Vdate >= '" + DateFrom.ToString(Database.dformat) + "') AND (dbo.Journals.Vdate <= '" + DateTo.ToString(Database.dformat) + "') AND (ACCOUNTs_2.Name = '" + accnm + "')";

                    }


                
            }

            

            opening = op1 + op2;

            tdt = new DataTable();
            Database.GetSqlData(sql, tdt);

            if (opening > 0.00)
            {
                tdt.Rows.Add();
                tdt.Rows[tdt.Rows.Count - 1]["Vdate"] = DateFrom.AddDays(-1).ToString(Database.dformat);
                tdt.Rows[tdt.Rows.Count - 1]["Invoiceno"] = "";
                tdt.Rows[tdt.Rows.Count - 1]["Particular"] = "Balance";
                tdt.Rows[tdt.Rows.Count - 1]["vtype"] = "";
                tdt.Rows[tdt.Rows.Count - 1]["ToBy"] = "By";
                tdt.Rows[tdt.Rows.Count - 1]["Reff"] = "";

                if (checkBox1.Checked == true)
                {
                    tdt.Rows[tdt.Rows.Count - 1]["Narr"] = "";
                    tdt.Rows[tdt.Rows.Count - 1]["Amount"] = opening;

                }
                else 
                {
                    tdt.Rows[tdt.Rows.Count - 1]["Narr"] = "";
                    tdt.Rows[tdt.Rows.Count - 1]["Reff"] = "";
                }


                tdt.Rows[tdt.Rows.Count - 1]["Dr"] = opening;
                tdt.Rows[tdt.Rows.Count - 1]["Cr"] = 0;
                tdt.Rows[tdt.Rows.Count - 1]["Vi_id"] = 0;
            }
            else if (opening < 0)
            {
                tdt.Rows.Add();
                tdt.Rows[tdt.Rows.Count - 1]["Vdate"] = DateFrom.AddDays(-1).ToString(Database.dformat);
                tdt.Rows[tdt.Rows.Count - 1]["Invoiceno"] = "";
                tdt.Rows[tdt.Rows.Count - 1]["Particular"] = "Balance";
                tdt.Rows[tdt.Rows.Count - 1]["vtype"] = "";
                tdt.Rows[tdt.Rows.Count - 1]["ToBy"] = "To";


                if (checkBox1.Checked==true)
                {
                    tdt.Rows[tdt.Rows.Count - 1]["Narr"] = "";
                    tdt.Rows[tdt.Rows.Count - 1]["Amount"] = -1 * opening;

                }
                else 
                {
                    tdt.Rows[tdt.Rows.Count - 1]["Narr"] = "";
                    tdt.Rows[tdt.Rows.Count - 1]["Reff"] = "";
                }


                tdt.Rows[tdt.Rows.Count - 1]["Dr"] = 0;
                tdt.Rows[tdt.Rows.Count - 1]["Cr"] = -1 * opening;
                tdt.Rows[tdt.Rows.Count - 1]["Vi_id"] = 0;
            }

            DataView view = tdt.DefaultView;
            view.Sort = "Vdate";
            tdt = view.ToTable();



            DataTable dtref = new DataTable();
            Database.GetSqlData("SELECT Journals.Vi_id, " + access_sql.fnstring("Journals.Reffno is null", "' '", "Journals.Reffno") + " as Reffno FROM Journals WHERE (((Journals.Ac_id)='" + funs.Select_ac_id(accnm) + "')) GROUP BY Journals.Vi_id, Journals.Reffno, Journals.Sno order by Sno", dtref);

            string lastvid = "-1";
            for (int i = 0; i < tdt.Rows.Count; i++)
            {
                if (double.Parse(tdt.Rows[i]["Dr"].ToString()) > 0)
                {
                    tdt.Rows[i]["ToBy"] = "To";
                    if (checkBox1.Checked == false)
                    {
                        if (dtref.Select("Vi_id='" + tdt.Rows[i]["Vi_id"].ToString()+"'").Length == 0)
                        {
                            tdt.Rows[i]["Reff"] = "";
                        }
                        else
                        {
                            tdt.Rows[i]["Reff"] = dtref.Select("Vi_id='" + tdt.Rows[i]["Vi_id"].ToString()+"'").FirstOrDefault()["Reffno"].ToString();
                        }
                    }


                    if (checkBox1.Checked == true)
                    {
                        tdt.Rows[i]["Amount"] = tdt.Rows[i]["Dr"].ToString();
                    }
                    
                }
                else if (double.Parse(tdt.Rows[i]["Cr"].ToString()) > 0)
                {
                    tdt.Rows[i]["ToBy"] = "By";
                    if (checkBox1.Checked == false)
                    {

                        if (dtref.Select("Vi_id='" + tdt.Rows[i]["Vi_id"].ToString()+"'").Length == 0)
                        {
                            tdt.Rows[i]["Reff"] = "";
                        }
                        else
                        {
                            tdt.Rows[i]["Reff"] = dtref.Select("Vi_id='" + tdt.Rows[i]["Vi_id"].ToString()+"'").FirstOrDefault()["Reffno"].ToString();
                        }
                    }

                    if (checkBox1.Checked == true)
                    {
                        tdt.Rows[i]["Amount"] = tdt.Rows[i]["Cr"].ToString();
                    }
                   
                }



                if (lastvid != tdt.Rows[i]["Vi_id"].ToString())
                {
                    double amt = 0;
                    amt = double.Parse(tdt.Compute("sum(Dr)-Sum(Cr)", "Vi_id='" + tdt.Rows[i]["Vi_id"].ToString()+"'").ToString());
                    if (amt > 0)
                    {
                        tdt.Rows[i]["Dr"] = amt;
                    }
                    else
                    {
                        tdt.Rows[i]["Cr"] = -1 * amt;
                    }
                    
                        totdr += double.Parse(tdt.Rows[i]["Dr"].ToString());
                        totcr += double.Parse(tdt.Rows[i]["Cr"].ToString());


                        if (totdr > totcr)
                        {
                            tdt.Rows[i]["RunningBalance"] = totdr - totcr;
                            tdt.Rows[i]["Dr_Cr"] = "Dr.";
                        }
                        else if (totcr > totdr)
                        {
                            tdt.Rows[i]["RunningBalance"] = totcr - totdr;
                            tdt.Rows[i]["Dr_Cr"] = "Cr.";
                        }
                        else
                        {
                            tdt.Rows[i]["RunningBalance"] = "0";
                        }
                }
                else
                {
                    tdt.Rows[i]["Dr"] = 0;
                    tdt.Rows[i]["Cr"] = 0;
                   
                    tdt.Rows[i]["Vtype"] = "";
                    tdt.Rows[i]["Invoiceno"] = "";

                }
                lastvid = tdt.Rows[i]["Vi_id"].ToString();
            }


            if (tdt.Rows.Count == 0)
            {
                return false;
            }

           

            string[,] col = new string[0, 0];

            if (checkBox1.Checked == true)
            {
                
                    string[,] Cwidth = new string[13, 8] { 
                        { "Vdate", "100", "0","" ,"","","",""},
                        { "Vch Type", "100", "0","" ,"","","",""  },
                        { "Vch No", "80", "0","" ,"","","",""  },
                        
                        { "", "50", "0","" ,"","","",""  }, 
                        { "Particular", "100", "0","" ,"","","",""  },
                         { "Amount", "100", "0","","" ,"","",""  },
                        { "Narration", "100", "0","" ,"","","",""  },
                        { "Reff", "80", "0","" ,"","","",""  },
                       
                        { "Debit", "80", "1","|sum(Dr)","" ,"","",""  },
                        { "Credit", "80", "1","|sum(Cr)" ,"" ,"","","" },
                        { "Balance", "90", "1","","" ,"","",""  },
                        { "D/C", "40", "1","" ,"" ,"","","" },
                        { "vid", "0", "0","","" ,"","",""  },
                    };
                    CreateReport(tdt, col, Cwidth);
                

            }
            else if (checkBox1.Checked == false)
            {
                tdt.Columns.Remove("ToBy");
                
                    string[,] Cwidth = new string[11, 8]
                    { 
                        { "Vdate", "100", "0","" ,"","","",""},
                         { "Vch Type", "100", "0","" ,"","","",""  },
                        { "Vch No", "100", "0","" ,"","","",""  },
                       
                        
                        { "Particular", "100", "0","" ,"","","",""  },
                        { "Narration", "130", "0","" ,"","","",""  },
                        { "Reff", "100", "0","" ,"","","",""  },
                        { "Debit", "100", "1","|sum(Dr)","" ,"","",""  },
                        { "Credit", "100", "1","|sum(Cr)" ,"" ,"","","" },
                        { "Balance", "120", "0","","" ,"","",""  },
                        { "D/C", "50", "1","" ,"" ,"","","" },
                        { "vid", "0", "0","","" ,"","",""  },
                    };
                    CreateReport(tdt, col, Cwidth);
              
            }

            dtFinal = tdt.Copy();
            return true;
        }




        public bool Stock(DateTime DateFrom, DateTime DateTo, string location, string step,string str)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
            double totdr = 0;
            double totcr = 0;
            frmptyp = "Stock";
            dateTimePicker1.Value = DateFrom;
            dateTimePicker2.Value = DateTo;
            this.Text = frmptyp;
            DecsOfReport = "Stock Report";
            groupBox2.Visible = false;

            sql = "SELECT Stocks.Godown_id AS locationid, Stocks.Step, Stocks.GRDate AS Vdate, Stocks.GRNo, ACCOUNTs_1.name AS Consigner,   DeliveryPoints_1.Name AS Origin, ACCOUNTs.name AS Consignee, DeliveryPoints.Name AS Destination, Stocks.ItemName AS Description,  Stocks.Packing, Stocks.TotPkts AS Quantity,  ISNULL( Stocks.ActWeight, Stocks.TotWeight) AS ActWeight, Stocks.TotWeight AS weight,  Stocks.GRType, Stocks.DeliveryType, Stocks.Private,  Stocks.Remark, Stocks.Freight, Stocks.GRCharge, Stocks.OthCharge, Stocks.FOC, Stocks.Paid, Stocks.ToPay, Stocks.TBB FROM ACCOUNTs RIGHT OUTER JOIN  ACCOUNTs AS ACCOUNTs_1 RIGHT OUTER JOIN  DeliveryPoints RIGHT OUTER JOIN  DeliveryPoints AS DeliveryPoints_1 RIGHT OUTER JOIN VOUCHERINFOs RIGHT OUTER JOIN  Stocks ON VOUCHERINFOs.Vi_id = Stocks.GR_id ON DeliveryPoints_1.DPId = Stocks.Source_id ON   DeliveryPoints.DPId = Stocks.Destination_id ON ACCOUNTs_1.ac_id = Stocks.Consigner_id ON ACCOUNTs.ac_id = Stocks.Consignee_id GROUP BY Stocks.Step, Stocks.TotPkts, Stocks.TotWeight, Stocks.GRDate, Stocks.GRNo, ACCOUNTs_1.name, ACCOUNTs.name,   DeliveryPoints_1.Name, DeliveryPoints.Name, Stocks.Godown_id, Stocks.ItemName, Stocks.Packing, Stocks.GRType, Stocks.DeliveryType,  Stocks.Private, Stocks.Remark, Stocks.Freight, Stocks.GRCharge, Stocks.OthCharge, Stocks.FOC, Stocks.Paid, Stocks.ToPay,   Stocks.TBB, Stocks.ActWeight HAVING (SUM( Stocks.Quantity) > 0) " + str + " ORDER BY Stocks.GRNo";
           // sql = "SELECT Stocks.Godown_id AS locationid, Stocks.Step, Stocks.GRDate AS Vdate, Stocks.GRNo, ACCOUNTs_1.name AS Consigner,   DeliveryPoints_1.Name AS Origin, ACCOUNTs.name AS Consignee, DeliveryPoints.Name AS Destination, Stocks.ItemName AS Description,  Stocks.Packing, Stocks.TotPkts AS Quantity,  ISNULL( Stocks.ActWeight, Stocks.TotWeight) AS ActWeight, Stocks.TotWeight AS weight,  Stocks.GRType, Stocks.DeliveryType, Stocks.Private,  Stocks.Remark, Stocks.Freight, Stocks.GRCharge, Stocks.OthCharge, Stocks.FOC, Stocks.Paid, Stocks.ToPay, Stocks.TBB,Stocks.GR_id as Vi_id FROM ACCOUNTs RIGHT OUTER JOIN  ACCOUNTs AS ACCOUNTs_1 RIGHT OUTER JOIN  DeliveryPoints RIGHT OUTER JOIN  DeliveryPoints AS DeliveryPoints_1 RIGHT OUTER JOIN VOUCHERINFOs RIGHT OUTER JOIN  Stocks ON VOUCHERINFOs.Vi_id = Stocks.GR_id ON DeliveryPoints_1.DPId = Stocks.Source_id ON   DeliveryPoints.DPId = Stocks.Destination_id ON ACCOUNTs_1.ac_id = Stocks.Consigner_id ON ACCOUNTs.ac_id = Stocks.Consignee_id WHERE     (dbo.VOUCHERINFOs.Iscancel = 0)   GROUP BY Stocks.Step, Stocks.TotPkts, Stocks.TotWeight, Stocks.GRDate, Stocks.GRNo, ACCOUNTs_1.name, ACCOUNTs.name,   DeliveryPoints_1.Name, DeliveryPoints.Name, Stocks.Godown_id, Stocks.ItemName, Stocks.Packing, Stocks.GRType, Stocks.DeliveryType,  Stocks.Private, Stocks.Remark, Stocks.Freight, Stocks.GRCharge, Stocks.OthCharge, Stocks.FOC, Stocks.Paid, Stocks.ToPay,   Stocks.TBB, Stocks.ActWeight,Stocks.GR_id HAVING (SUM( Stocks.Quantity) > 0) " + str + " ORDER BY Stocks.GRNo";
            dt.Clear();
            Database.GetSqlData(sql, dt);
             DataRow[] drow;
             drow = dt.Select("LocationId is not null And Step is not null");
             if (location != "" && step != "")
             {
                 string locid = Database.GetScalarText("Select locationid from location where nick_name='"+location+"'");
                 drow = dt.Select("LocationId='" + locid+"' And Step='"+step.Replace("Booked","Step1").Replace("To Be Delivered","Step2")+"'");
                 
             }
            
             else if (location != "" && step == "")
             {
                 string locid = Database.GetScalarText("Select locationid from location where nick_name='" + location + "'");
                 drow = dt.Select("LocationId='" + locid+"' And Step is not null");
             }

             else if (location == "" && step != "")
             {

                 drow = dt.Select("Step='" + step.Replace("Booked", "Step1").Replace("To Be Delivered", "Step2") + "' And locationid is not null");
             }
             
            if (drow.GetLength(0) > 0)
             {
                 tdt = drow.CopyToDataTable();
                 tdt.Columns.Remove("locationid");
                 tdt.Columns.Remove("step");
                 tdt.DefaultView.Sort = "Vdate";
                 tdt.DefaultView.ToTable();
             }
           




          
            
            if (tdt.Rows.Count == 0)
            {
                return false;
            }

            string[,] col = new string[1, 3] { { "Vdate", "1", "0" } };

            string[,] Cwidth = new string[23, 6] { 
            { "Vdate", "0", "0","","","" },
            { "GRNo", "100", "0","" ,"",""},
            { "Consigner", "120", "0","" ,"",""},
            { "Origin", "150", "0","" ,"",""  },
            { "Consignee", "120", "1","","" ,""  },
            { "Destination", "150", "1","" ,"" ,"" },
            { "Item Name", "120", "0","" ,"" ,"" }, 
            { "Packing", "75", "0","" ,"" ,"" }, 
            { "Quantity", "75", "0","|sum(Quantity)","" ,""  },
            { "ActWeight", "75", "0","|sum(ActWeight)","" ,""  },
            {"Weight","75","0","|sum(Weight)","",""},

            { "GR Type", "75", "0","" ,"" ,"" }, 
            { "DeliveryType", "75", "0","" ,"" ,"" }, 
            { "Private", "75", "0","" ,"" ,"" }, 
            { "Remark", "75", "0","" ,"" ,"" }, 
            { "Freight", "75", "0","|sum(Freight)","" ,""  },
            { "GR Charge", "75", "0","|sum(GRCharge)","" ,""  },
            { "Oth Charge", "90", "0","|sum(OthCharge)","" ,""  },
            { "FOC", "100", "0","|sum(FOC)","" ,""  },
            { "Paid", "100", "0","|sum(Paid)","" ,""  },
            { "ToPay", "100", "0","|sum(ToPay)","" ,""  },
            { "TBB", "100", "0","|sum(TBB)","" ,""  },
            {"vid","0","1","","",""},

        };
            CreateReport(tdt, col, Cwidth);
            return true;
        }
        public bool GroupLedger(DateTime DateFrom, DateTime DateTo)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
            double totdr = 0;
            double totcr = 0;

            stdt = DateFrom;
            endt = DateTo;
            dateTimePicker1.Value = DateFrom;
            dateTimePicker2.Value = DateTo;
            label3.Enabled = true;
            textBox1.Enabled = false;

            frmptyp = "GroupLedger";
            this.Text = frmptyp;
            DecsOfReport = "GroupLedger, for the period of " + DateFrom.ToString(Database.dformat) + " to " + DateTo.ToString(Database.dformat);

            sql = "Select Name,Vdate,DocNumber,Narr as Particular,Dr,Cr FROM QryLedger where AccountType=10 or AccountType=9 or AccountType=5 or AccountType=12 or AccountType=11 or AccountType=20 or AccountType=28 order by AccountType,Name";
            dt.Clear();
            Database.GetSqlData(sql, dt);

            DataRow[] drow;


            drow = dt.Select("Vdate<=#" + DateTo.ToString(Database.dformat) + "#");


            tdt.Clear();
            if (drow.GetLength(0) > 0)
            {
                tdt = drow.CopyToDataTable();
                tdt.DefaultView.Sort = "Vdate";
                tdt.Columns.Add("RunningBalance", typeof(decimal));
                tdt.Columns.Add("Dr/Cr", typeof(string));
                for (int i = 0; i < tdt.Rows.Count; i++)
                {
                    totdr += double.Parse(tdt.Rows[i]["Dr"].ToString());
                    totcr += double.Parse(tdt.Rows[i]["Cr"].ToString());
                    if (totdr > totcr)
                    {
                        tdt.Rows[i]["RunningBalance"] = totdr - totcr;
                        tdt.Rows[i]["Dr/Cr"] = "Dr.";
                    }
                    else if (totcr > totdr)
                    {
                        tdt.Rows[i]["RunningBalance"] = totcr - totdr;
                        tdt.Rows[i]["Dr/Cr"] = "Cr.";
                    }
                    else
                    {
                        tdt.Rows[i]["RunningBalance"] = "0";
                    }

                }

            }

            if (tdt.Rows.Count == 0)
            {
                return false;
            }

            string[,] col = new string[2, 3] { { "Name", "1", "0" }, { "Vdate", "1", "0" } };

            string[,] Cwidth = new string[8, 6] { 
            { "Account", "0", "0","","","" },
            { "Vdate", "0", "0","" ,"",""},
            { "Document No.", "200", "0","" ,"",""},
            { "Particular", "330", "0","" ,"",""  },
            { "Amount Dr.", "140", "1","|sum(Dr)","" ,""  },
            { "Amount Cr.", "140", "1","|sum(Cr)" ,"" ,"" },
            { "Running Balance", "140", "0","" ,"" ,"" }, 
            { "Dr./Cr.", "50", "0","","" ,""  } };

            CreateReport(tdt, col, Cwidth);
            return true;
        }

        public bool SingleGroupedTrial(DateTime DateFrom, DateTime DateTo, string accnm)
        {
            double totdr = 0;
            double totcr = 0;
            stdt = DateFrom;
            endt = DateTo;
            dateTimePicker1.Value = DateFrom;
            dateTimePicker2.Value = DateTo;
            frmptyp = "Grouped Trial Balance";
            this.Text = frmptyp;
            label3.Enabled = true;
            textBox1.Enabled = true;
            textBox1.Text = accnm;
            DecsOfReport = "Group Balance, as on " + DateTo.ToString(Database.dformat);

            //if (Database.IsKacha == false)
            //{
                sql = "SELECT ACCOUNTYPEs.Name as ACCOUNTYPE,X.Name as Name, Sum(X.Dr) AS Dr, Sum(X.Cr) AS Cr  FROM ((SELECT QryJournal.Name, sum(QryJournal.Dr) as Dr, sum(QryJournal.Cr) as Cr From QryJournal Where (((QryJournal.Vdate)  <= " + access_sql.Hash + DateTo.ToString(Database.dformat) + access_sql.Hash + "))GROUP BY QryJournal.Name  UNION ALL SELECT QryAccountinfo.Name, QryAccountinfo.Dr as Dr, QryAccountinfo.Cr as Cr FROM QryAccountinfo)  AS X LEFT JOIN ACCOUNTs ON X.Name = ACCOUNTs.Name) LEFT JOIN ACCOUNTYPEs ON ACCOUNTs.Act_id = ACCOUNTYPEs.Act_id GROUP BY X.Name, ACCOUNTYPEs.Name";
            //}
            //else
            //{
            //    sql = "SELECT ACCOUNTYPE.Name as ACCOUNTYPE,X.Name as Name, Sum(X.Dr) AS Dr, Sum(X.Cr) AS Cr  FROM ((SELECT QryJournal.Name, sum(QryJournal.Dr) as Dr, sum(QryJournal.Cr) as Cr From QryJournal Where (((QryJournal.Vdate)  <= " + access_sql.Hash + DateTo.ToString(Database.dformat) + access_sql.Hash + "))GROUP BY QryJournal.Name, QryJournal.B HAVING (((QryJournal.B)=" + access_sql.Singlequote + "True" + access_sql.Singlequote + ")) UNION ALL SELECT QryAccountinfo.Name, QryAccountinfo.Dr2 as Dr, QryAccountinfo.Cr2 as Cr FROM QryAccountinfo)  AS X LEFT JOIN ACCOUNT ON X.Name = ACCOUNT.Name) LEFT JOIN ACCOUNTYPE ON ACCOUNT.Act_id = ACCOUNTYPE.Act_id GROUP BY X.Name, ACCOUNTYPE.Name";
            //}


            dt = new DataTable();
            Database.GetSqlData(sql, dt);


            tdt.Clear();


            dt.DefaultView.Sort = "ACCOUNTYPE,Name";

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                totdr = double.Parse(dt.Rows[i]["Dr"].ToString());
                totcr = double.Parse(dt.Rows[i]["Cr"].ToString());
                if (totdr > totcr)
                {
                    dt.Rows[i]["Dr"] = totdr - totcr;
                    dt.Rows[i]["Cr"] = 0;
                }
                else if (totcr > totdr)
                {
                    dt.Rows[i]["Dr"] = 0;
                    dt.Rows[i]["Cr"] = totcr - totdr;
                }
                else
                {
                    dt.Rows[i]["Dr"] = 0;
                    dt.Rows[i]["Cr"] = 0;
                }
            }

            if (dt.Select("ACCOUNTYPE='" + accnm + "'").Length == 0)
            {
                return false;
            }
            tdt = dt.Select("ACCOUNTYPE='" + accnm + "'").CopyToDataTable();
            tdt.DefaultView.Sort = "ACCOUNTYPE,Name";

            if (tdt.Select("not (Dr=0 and Cr=0)").Length == 0)
            {
                return false;
            }
            tdt = tdt.Select("not (Dr=0 and Cr=0)").CopyToDataTable();

            if (tdt.Rows.Count == 0)
            {
                return false;
            }
            string[,] col = new string[1, 3] { { "ACCOUNTYPE", "1", "1" } };
            string[,] Cwidth = new string[4, 6] { 
            { "Name", "0", "0" ,"","" ,"" }, 
            { "Account", "700", "0" ,"Total Amount","" ,"" }, 
            { "Amount (Dr.)", "150", "1","|sum(Dr)","","" }, 
            { "Amount (Cr.)", "150", "1","|sum(Cr)","","" } };
            CreateReport(tdt, col, Cwidth);
            return true;

        }


        public bool GroupedTrial(DateTime DateFrom, DateTime DateTo)
        {
            double totdr = 0;
            double totcr = 0;
            stdt = DateFrom;
            endt = DateTo;
            dateTimePicker1.Value = DateFrom;
            dateTimePicker2.Value = DateTo;
            frmptyp = "Grouped Trial Balance";
            this.Text = frmptyp;
            label3.Enabled = false;
            textBox1.Enabled = false;
            checkBox1.Visible = true;
            DecsOfReport = "Group Trial Balance, as on " + DateTo.ToString(Database.dformat);
            if (checkBox1.Checked == true)
            {

                //if (Database.IsKacha == false)
                //{
                    sql = "SELECT ACCOUNTYPEs.Name as ACCOUNTYPE,X.Name as Name, Sum(X.Dr) AS Dr, Sum(X.Cr) AS Cr  FROM ((SELECT QryJournal.Name, sum(QryJournal.Dr) as Dr, sum(QryJournal.Cr) as Cr From QryJournal Where (((QryJournal.Vdate)  <= " + access_sql.Hash + DateTo.ToString(Database.dformat) + access_sql.Hash + "))GROUP BY QryJournal.Name UNION ALL SELECT QryAccountinfo.Name, QryAccountinfo.Dr as Dr, QryAccountinfo.Cr as Cr FROM QryAccountinfo)  AS X LEFT JOIN ACCOUNTs ON X.Name = ACCOUNTs.Name) LEFT JOIN ACCOUNTYPEs ON ACCOUNTs.Act_id = ACCOUNTYPEs.Act_id GROUP BY X.Name, ACCOUNTYPEs.Name";
                //}
                //else
                //{
                //    sql = "SELECT ACCOUNTYPE.Name as ACCOUNTYPE,X.Name as Name, Sum(X.Dr) AS Dr, Sum(X.Cr) AS Cr  FROM ((SELECT QryJournal.Name, sum(QryJournal.Dr) as Dr, sum(QryJournal.Cr) as Cr From QryJournal Where (((QryJournal.Vdate)  <= " + access_sql.Hash + DateTo.ToString(Database.dformat) + access_sql.Hash + "))GROUP BY QryJournal.Name, QryJournal.B HAVING (((QryJournal.B)=" + access_sql.Singlequote + "True" + access_sql.Singlequote + ")) UNION ALL SELECT QryAccountinfo.Name, QryAccountinfo.Dr2 as Dr, QryAccountinfo.Cr2 as Cr FROM QryAccountinfo)  AS X LEFT JOIN ACCOUNT ON X.Name = ACCOUNT.Name) LEFT JOIN ACCOUNTYPE ON ACCOUNT.Act_id = ACCOUNTYPE.Act_id GROUP BY X.Name, ACCOUNTYPE.Name";
                //}
            }
            else
            {
                //if (Database.IsKacha == false)
                //{

                
                   sql = "SELECT ACCOUNTYPEs.Name as ACCOUNTYPE, Sum(X.Dr) AS Dr, Sum(X.Cr) AS Cr FROM ((SELECT QryJournal.Name, sum(QryJournal.Dr) as Dr, sum(QryJournal.Cr) as Cr From QryJournal Where (((QryJournal.Vdate)  <= " + access_sql.Hash + DateTo.ToString(Database.dformat) + access_sql.Hash + "))GROUP BY QryJournal.Name, QryJournal.A HAVING (((QryJournal.A)=" + access_sql.Singlequote + "True" + access_sql.Singlequote + ")) UNION ALL SELECT QryAccountinfo.Name, QryAccountinfo.Dr as Dr, QryAccountinfo.Cr as Cr FROM QryAccountinfo)  AS X LEFT JOIN ACCOUNTs ON X.Name = ACCOUNTs.Name) LEFT JOIN ACCOUNTYPEs ON ACCOUNTs.Act_id = ACCOUNTYPEs.Act_id GROUP BY ACCOUNTYPEs.Name";

                //}
                //else
                //{
                //    sql = "SELECT ACCOUNTYPE.Name as ACCOUNTYPE, Sum(X.Dr) AS Dr, Sum(X.Cr) AS Cr  FROM ((SELECT QryJournal.Name, sum(QryJournal.Dr) as Dr, sum(QryJournal.Cr) as Cr From QryJournal Where (((QryJournal.Vdate)  <= " + access_sql.Hash + DateTo.ToString(Database.dformat) + access_sql.Hash + "))GROUP BY QryJournal.Name, QryJournal.B HAVING (((QryJournal.B)=" + access_sql.Singlequote + "True" + access_sql.Singlequote + ")) UNION ALL SELECT QryAccountinfo.Name, QryAccountinfo.Dr2 as Dr, QryAccountinfo.Cr2 as Cr FROM QryAccountinfo)  AS X LEFT JOIN ACCOUNT ON X.Name = ACCOUNT.Name) LEFT JOIN ACCOUNTYPE ON ACCOUNT.Act_id = ACCOUNTYPE.Act_id GROUP BY ACCOUNTYPE.Name";
                //}
            }
            dt = new DataTable();
            Database.GetSqlData(sql, dt);


            tdt.Clear();


            dt.DefaultView.Sort = "ACCOUNTYPE";

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                totdr = double.Parse(dt.Rows[i]["Dr"].ToString());
                totcr = double.Parse(dt.Rows[i]["Cr"].ToString());
                if (totdr > totcr)
                {
                    dt.Rows[i]["Dr"] = totdr - totcr;
                    dt.Rows[i]["Cr"] = 0;
                }
                else if (totcr > totdr)
                {
                    dt.Rows[i]["Dr"] = 0;
                    dt.Rows[i]["Cr"] = totcr - totdr;
                }
                else
                {
                    dt.Rows[i]["Dr"] = 0;
                    dt.Rows[i]["Cr"] = 0;
                }
            }
            if (dt.Select("not (Dr=0 and Cr=0)").Length == 0)
            {
                return false;
            }


            tdt = dt.Select("not (Dr=0 and Cr=0)").CopyToDataTable();
            if (tdt.Rows.Count == 0)
            {
                return false;
            }

            totdr = double.Parse(tdt.Compute("sum(Dr)", "").ToString());
            totcr = double.Parse(tdt.Compute("sum(Cr)", "").ToString());


            if (checkBox1.Checked == true)
            {
                if (totdr > totcr)
                {
                    tdt.Rows.Add("", "Difference in Opening Trial", "0", totdr - totcr);
                }
                else
                {
                    tdt.Rows.Add("", "Difference in Opening Trial", totcr - totdr, "0");
                }

                string[,] col = new string[1, 3] { { "ACCOUNTYPE", "1", "1" } };
                string[,] Cwidth = new string[4, 6] { 
                { "Name", "0", "0" ,"","" ,"" }, 
                { "Account", "700", "0" ,"Total Amount","" ,"" }, 
                { "Amount (Dr.)", "150", "1","|sum(Dr)","+sum(Dr)-sum(Cr)","" }, 
                { "Amount (Cr.)", "150", "1","|sum(Cr)","+sum(Cr)-sum(Dr)","" } };
                CreateReport(tdt, col, Cwidth);
            }
            else
            {
                if (totdr > totcr)
                {
                    tdt.Rows.Add("Difference in Opening Trial", "0", totdr - totcr);
                }
                else
                {
                    tdt.Rows.Add("Difference in Opening Trial", totcr - totdr, "0");
                }
                string[,] col = new string[0, 0];
                string[,] Cwidth = new string[3, 6] { 
                { "Account", "700", "0" ,"Total Amount","" ,"" }, 
                { "Amount (Dr.)", "150", "1","|sum(Dr)","+sum(Dr)-sum(Cr)","" }, 
                { "Amount (Cr.)", "150", "1","|sum(Cr)","+sum(Cr)-sum(Dr)","" } };

                CreateReport(tdt, col, Cwidth);
            }


            return true;

        }


        private int FillTFormat(DataTable dtReport, string AccountType, string Caption, char Side, int Counter)
        {
            tdt.Clear();
            if (Caption == "Closing Stock")
            {
                double closing = 0;
                int closingcounter;
                if (Counter == dtReport.Rows.Count)
                {
                    dtReport.Rows.Add();
                    dtReport.Rows[Counter]["AmtL"] = 0;
                    dtReport.Rows[Counter]["AmtL2"] = 0;
                    dtReport.Rows[Counter]["AmtR"] = 0;
                    dtReport.Rows[Counter]["AmtR2"] = 0;
                }

                dtReport.Rows[Counter]["NameR"] = "<b>" + Caption;
                closingcounter = Counter;
                Counter++;

                DataTable dtStock = new DataTable();
                Database.GetSqlData("SELECT ACCOUNTs.Name , ACCOUNTs.Balance as Balance, ACCOUNTs.Closing_Bal FROM ACCOUNTs LEFT JOIN ACCOUNTYPEs ON ACCOUNTs.Act_id = ACCOUNTYPEs.Act_id WHERE (((ACCOUNTYPEs.Name)='STOCK-IN-HAND')) ORDER BY ACCOUNTs.Name", dtStock);

                for (int x = 0; x < dtStock.Rows.Count; x++)
                {
                    double closingchk;
                    //if (Database.IsKacha == false)
                    //{
                        if (dtStock.Rows[x]["Closing_Bal"].ToString() == "")
                        {
                            closingchk = double.Parse(dtStock.Rows[x]["Balance"].ToString());
                        }
                        else
                        {
                            closingchk = double.Parse(dtStock.Rows[x]["Closing_Bal"].ToString());
                        }
                    //}
                    //else
                    //{
                    //    if (dtStock.Rows[x]["Closing_Bal2"].ToString() == "")
                    //    {
                    //        closingchk = double.Parse(dtStock.Rows[x]["Balance2"].ToString());
                    //    }
                    //    else
                    //    {
                    //        closingchk = double.Parse(dtStock.Rows[x]["Closing_Bal2"].ToString());
                    //    }
                    //}

                    if (closingchk == 0)
                    {
                        continue;
                    }
                    if (Counter == dtReport.Rows.Count)
                    {
                        dtReport.Rows.Add();
                        dtReport.Rows[Counter]["AmtL"] = 0;
                        dtReport.Rows[Counter]["AmtL2"] = 0;
                        dtReport.Rows[Counter]["AmtR"] = 0;
                        dtReport.Rows[Counter]["AmtR2"] = 0;
                    }

                    dtReport.Rows[Counter]["NameR"] = dtStock.Rows[x]["Name"].ToString();
                    dtReport.Rows[Counter]["AmtR"] = closingchk;
                    closing = closing + closingchk;
                    Counter++;

                }

                dtReport.Rows[closingcounter]["AmtR2"] = closing;

            }

            else if (Caption == "Opening Stock")
            {
                double closing = 0;
                int closingcounter;
                if (Counter == dtReport.Rows.Count)
                {
                    dtReport.Rows.Add();
                    dtReport.Rows[Counter]["AmtL"] = 0;
                    dtReport.Rows[Counter]["AmtL2"] = 0;
                    dtReport.Rows[Counter]["AmtR"] = 0;
                    dtReport.Rows[Counter]["AmtR2"] = 0;
                }

                dtReport.Rows[Counter]["NameL"] = "<b>" + Caption;
                closingcounter = Counter;
                Counter++;

                DataTable dtStock = new DataTable();
                Database.GetSqlData("SELECT ACCOUNTs.Name , ACCOUNTs.Balance as Balance FROM ACCOUNTs LEFT JOIN ACCOUNTYPEs ON ACCOUNTs.Act_id = ACCOUNTYPEs.Act_id WHERE (((ACCOUNTYPEs.Name)='STOCK-IN-HAND')) ORDER BY ACCOUNTs.Name", dtStock);


                for (int x = 0; x < dtStock.Rows.Count; x++)
                {
                    double closingchk;
                    //if (Database.IsKacha == false)
                    //{
                        if (dtStock.Rows[x]["Balance"].ToString() == "")
                        {
                            closingchk = double.Parse(dtStock.Rows[x]["Balance"].ToString());
                        }
                        else
                        {
                            closingchk = double.Parse(dtStock.Rows[x]["Balance"].ToString());
                        }
                    //}
                    //else
                    //{
                    //    if (dtStock.Rows[x]["Balance2"].ToString() == "")
                    //    {
                    //        closingchk = double.Parse(dtStock.Rows[x]["Balance2"].ToString());
                    //    }
                    //    else
                    //    {
                    //        closingchk = double.Parse(dtStock.Rows[x]["Balance2"].ToString());
                    //    }
                    //}

                    if (closingchk == 0)
                    {
                        continue;
                    }
                    if (Counter == dtReport.Rows.Count)
                    {
                        dtReport.Rows.Add();
                        dtReport.Rows[Counter]["AmtL"] = 0;
                        dtReport.Rows[Counter]["AmtL2"] = 0;
                        dtReport.Rows[Counter]["AmtR"] = 0;
                        dtReport.Rows[Counter]["AmtR2"] = 0;
                    }

                    dtReport.Rows[Counter]["NameL"] = dtStock.Rows[x]["Name"].ToString();
                    dtReport.Rows[Counter]["AmtL"] = closingchk;
                    closing = closing + closingchk;
                    Counter++;

                }

                dtReport.Rows[closingcounter]["AmtL2"] = closing;

            }
            else
            {
                if (dt.Select("Aname in(" + AccountType + ") And Amount<>0").Length > 0)
                {
                    tdt = dt.Select("Aname in(" + AccountType + ") And Amount<>0").CopyToDataTable();
                    tdt.DefaultView.Sort = "Name";
                    tdt = tdt.DefaultView.ToTable();
                }
                if (tdt.Rows.Count > 0)
                {
                    if (Counter == dtReport.Rows.Count)
                    {
                        dtReport.Rows.Add();
                        dtReport.Rows[Counter]["AmtL"] = 0;
                        dtReport.Rows[Counter]["AmtL2"] = 0;
                        dtReport.Rows[Counter]["AmtR"] = 0;
                        dtReport.Rows[Counter]["AmtR2"] = 0;
                    }

                    if (Side == 'L')
                    {
                        dtReport.Rows[Counter]["NameL"] = "<b>" + Caption;
                        dtReport.Rows[Counter]["AmtL2"] = tdt.Compute("sum(Amount)", "").ToString();
                    }

                    else if (Side == 'R')
                    {
                        dtReport.Rows[Counter]["NameR"] = "<b>" + Caption;
                        dtReport.Rows[Counter]["AmtR2"] = double.Parse(tdt.Compute("sum(Amount)", "").ToString()) * -1;
                    }

                    Counter++;
                }

                for (int x = 0; x < tdt.Rows.Count; x++)
                {
                    if (Counter == dtReport.Rows.Count)
                    {
                        dtReport.Rows.Add();
                        dtReport.Rows[Counter]["AmtL"] = 0;
                        dtReport.Rows[Counter]["AmtL2"] = 0;
                        dtReport.Rows[Counter]["AmtR"] = 0;
                        dtReport.Rows[Counter]["AmtR2"] = 0;
                    }

                    if (Side == 'L')
                    {
                        dtReport.Rows[Counter]["NameL"] = tdt.Rows[x]["Name"].ToString();
                        dtReport.Rows[Counter]["AmtL"] = tdt.Rows[x]["Amount"].ToString();
                    }

                    else if (Side == 'R')
                    {
                        dtReport.Rows[Counter]["NameR"] = tdt.Rows[x]["Name"].ToString();
                        dtReport.Rows[Counter]["AmtR"] = double.Parse(tdt.Rows[x]["Amount"].ToString()) * -1;
                    }

                    Counter++;
                }
            }

            return Counter;
        }



        public bool ProfitAndLoss(DateTime DateFrom, DateTime DateTo)
        {
            DataTable dtReport = new DataTable();
            int RcL = 0;
            int RcR = 0;
            stdt = DateFrom;
            endt = DateTo;
            dateTimePicker1.Value = DateFrom;
            dateTimePicker2.Value = DateTo;
            frmptyp = "Profit And Loss";

            this.Text = frmptyp;
            label3.Enabled = false;
            textBox1.Enabled = false;
            DecsOfReport = "Profit and Loss, From  " + DateFrom.ToString(Database.dformat) + " To  " + DateTo.ToString(Database.dformat);

            //if (Database.IsKacha == false)
            //{
            //    //  sql = "SELECT ACCOUNTYPE.Name AS Aname, X.Name,Sum(X.Dr)-Sum(X.Cr) AS Amount FROM ((SELECT QryJournal.Name, sum(QryJournal.Dr) as Dr, sum(QryJournal.Cr) as Cr From QryJournal Where (((QryJournal.Vdate)  <= " + access_sql.Hash + DateTo.ToString(Database.dformat) + access_sql.Hash + ")) GROUP BY QryJournal.Name, QryJournal.A HAVING (((QryJournal.A)=" + access_sql.Singlequote + "True" + access_sql.Singlequote + ")) UNION ALL SELECT QryAccountinfo.Name, QryAccountinfo.Dr as Dr, QryAccountinfo.Cr as Cr FROM QryAccountinfo)  AS X LEFT JOIN ACCOUNT ON X.Name = ACCOUNT.Name) LEFT JOIN ACCOUNTYPE ON ACCOUNT.Act_id = ACCOUNTYPE.Act_id GROUP BY ACCOUNTYPE.Name, X.Name ORDER BY ACCOUNTYPE.Name, X.Name";
               sql = "SELECT ACCOUNTYPEs.Name AS Aname, X.Name,Sum(X.Dr)-Sum(X.Cr) AS Amount FROM ( (SELECT QryJournal.Name, sum(QryJournal.Dr) as Dr, sum(QryJournal.Cr) as Cr From QryJournal Where  QryJournal.Vdate  >= " + access_sql.Hash + DateFrom.ToString(Database.dformat) + access_sql.Hash + " And QryJournal.Vdate  <= " + access_sql.Hash + DateTo.ToString(Database.dformat) + access_sql.Hash + " GROUP BY QryJournal.Name, QryJournal.A HAVING (((QryJournal.A)=" + access_sql.Singlequote + "True" + access_sql.Singlequote + ")) )  AS X LEFT JOIN ACCOUNTs ON X.Name = ACCOUNTs.Name) LEFT JOIN ACCOUNTYPEs ON ACCOUNTs.Act_id = ACCOUNTYPEs.Act_id GROUP BY ACCOUNTYPEs.Name, X.Name ORDER BY ACCOUNTYPEs.Name, X.Name";
            //}
            //else
            //{
            //    sql = "SELECT ACCOUNTYPE.Name AS Aname, X.Name,Sum(X.Dr)-Sum(X.Cr) AS Amount FROM ( (SELECT QryJournal.Name, sum(QryJournal.Dr) as Dr, sum(QryJournal.Cr) as Cr From QryJournal Where  QryJournal.Vdate  >= " + access_sql.Hash + DateFrom.ToString(Database.dformat) + access_sql.Hash + " And QryJournal.Vdate  <= " + access_sql.Hash + DateTo.ToString(Database.dformat) + access_sql.Hash + " GROUP BY QryJournal.Name, QryJournal.B HAVING (((QryJournal.B)=" + access_sql.Singlequote + "True" + access_sql.Singlequote + ")) )  AS X LEFT JOIN ACCOUNT ON X.Name = ACCOUNT.Name) LEFT JOIN ACCOUNTYPE ON ACCOUNT.Act_id = ACCOUNTYPE.Act_id GROUP BY ACCOUNTYPE.Name, X.Name ORDER BY ACCOUNTYPE.Name, X.Name";
            //}

            dt.Clear();
            Database.GetSqlData(sql, dt);
            dtReport.Columns.Add("NameL", typeof(string));
            dtReport.Columns.Add("AmtL", typeof(decimal));
            dtReport.Columns.Add("AmtL2", typeof(decimal));
            dtReport.Columns.Add("NameR", typeof(string));
            dtReport.Columns.Add("AmtR", typeof(decimal));
            dtReport.Columns.Add("AmtR2", typeof(decimal));


            //Trading Expenses
            RcL = FillTFormat(dtReport, "'STOCK-IN-HAND'", "Opening Stock", 'L', RcL);
            RcL = FillTFormat(dtReport, "'PURCHASE ACCOUNTS'", "Purchase", 'L', RcL);
            RcL = FillTFormat(dtReport, "'EXPENDITURE ACCOUNT (Direct)'", "Direct Expenses", 'L', RcL);

            //Trading Income
            RcR = FillTFormat(dtReport, "'SALES ACCOUNTS'", "Sales", 'R', RcR);
            RcR = FillTFormat(dtReport, "'INCOME (Direct)'", "Direct Income", 'R', RcR);

            RcR = FillTFormat(dtReport, "'STOCK-IN-HAND'", "Closing Stock", 'R', RcR);
            int Trows = 0;
            if (RcL > RcR)
            {
                Trows = RcL;
            }
            else
            {
                Trows = RcR;
            }


            double TradingExpence = double.Parse(dtReport.Compute("sum(AmtL2)", "").ToString());
            double TradingIncome = double.Parse(dtReport.Compute("sum(AmtR2)", "").ToString());
            double GrossProfit = 0;
            double GrossLoss = 0;

            if (TradingIncome > TradingExpence)
            {
                GrossProfit = TradingIncome - TradingExpence;
                dtReport.Rows.Add();
                dtReport.Rows[Trows]["NameL"] = "<b>Gross Income";
                dtReport.Rows[Trows]["AmtL"] = 0;
                dtReport.Rows[Trows]["AmtL2"] = GrossProfit;
                dtReport.Rows[Trows]["NameR"] = "";
                dtReport.Rows[Trows]["AmtR"] = 0;
                dtReport.Rows[Trows]["AmtR2"] = 0;
                Trows++;

                dtReport.Rows.Add();
                dtReport.Rows[Trows]["NameL"] = "<b>Gross Total";
                dtReport.Rows[Trows]["AmtL"] = 0;
                dtReport.Rows[Trows]["AmtL2"] = TradingExpence + GrossProfit;
                dtReport.Rows[Trows]["NameR"] = "<b>Gross Total";
                dtReport.Rows[Trows]["AmtR"] = 0;
                dtReport.Rows[Trows]["AmtR2"] = TradingExpence + GrossProfit;
                Trows++;

                dtReport.Rows.Add();
                dtReport.Rows[Trows]["NameL"] = "";
                dtReport.Rows[Trows]["AmtL"] = 0;
                dtReport.Rows[Trows]["AmtL2"] = 0;
                dtReport.Rows[Trows]["NameR"] = "";
                dtReport.Rows[Trows]["AmtR"] = 0;
                dtReport.Rows[Trows]["AmtR2"] = 0;
                Trows++;

                dtReport.Rows.Add();
                dtReport.Rows[Trows]["NameL"] = "";
                dtReport.Rows[Trows]["AmtL"] = 0;
                dtReport.Rows[Trows]["AmtL2"] = 0;
                dtReport.Rows[Trows]["NameR"] = "<b>Gross Income";
                dtReport.Rows[Trows]["AmtR"] = 0;
                dtReport.Rows[Trows]["AmtR2"] = GrossProfit;
                Trows++;
                RcL = Trows - 1;
                RcR = Trows;
            }

            else if (TradingExpence > TradingIncome) //if gross loss
            {
                GrossLoss = TradingExpence - TradingIncome;
                dtReport.Rows.Add();
                dtReport.Rows[Trows]["NameL"] = "";
                dtReport.Rows[Trows]["AmtL"] = 0;
                dtReport.Rows[Trows]["AmtL2"] = 0;
                dtReport.Rows[Trows]["NameR"] = "<b>Gross Loss";
                dtReport.Rows[Trows]["AmtR"] = 0;
                dtReport.Rows[Trows]["AmtR2"] = GrossLoss;
                Trows++;
                dtReport.Rows.Add();
                dtReport.Rows[Trows]["NameL"] = "<b>Gross Total";
                dtReport.Rows[Trows]["AmtL"] = 0;
                dtReport.Rows[Trows]["AmtL2"] = TradingIncome + GrossLoss;
                dtReport.Rows[Trows]["NameR"] = "<b>Gross Total";
                dtReport.Rows[Trows]["AmtR"] = 0;
                dtReport.Rows[Trows]["AmtR2"] = TradingIncome + GrossLoss;
                Trows++;

                dtReport.Rows.Add();
                dtReport.Rows[Trows]["NameL"] = "";
                dtReport.Rows[Trows]["AmtL"] = 0;
                dtReport.Rows[Trows]["AmtL2"] = 0;
                dtReport.Rows[Trows]["NameR"] = "";
                dtReport.Rows[Trows]["AmtR"] = 0;
                dtReport.Rows[Trows]["AmtR2"] = 0;
                Trows++;

                dtReport.Rows.Add();
                dtReport.Rows[Trows]["NameL"] = "<b>Gross Loss";
                dtReport.Rows[Trows]["AmtL"] = 0;
                dtReport.Rows[Trows]["AmtL2"] = GrossLoss;
                dtReport.Rows[Trows]["NameR"] = "";
                dtReport.Rows[Trows]["AmtR"] = 0;
                dtReport.Rows[Trows]["AmtR2"] = 0;
                Trows++;
                RcL = Trows;
                RcR = Trows - 1;
            }



            //P&L Expenses
            RcL = FillTFormat(dtReport, "'EXPENDITURE ACCOUNT (Indirect )'", "Indirect Expenses", 'L', RcL);
            //P&L Income
            RcR = FillTFormat(dtReport, "'INCOME (Indirect)'", "Indirect Income", 'R', RcR);

            double NetExp = 0;
            if (dt.Select("Aname='EXPENDITURE ACCOUNT (Indirect )'").Length > 0)
            {

                NetExp = GrossLoss + double.Parse(dt.Compute("sum(Amount)", "Aname='EXPENDITURE ACCOUNT (Indirect )'").ToString());
            }
            double NetIncome = 0;
            if (dt.Compute("sum(Amount)", "Aname='INCOME (Indirect)'").ToString().Length > 0)
            {
                NetIncome = GrossProfit + (-1 * double.Parse(dt.Compute("sum(Amount)", "Aname='INCOME (Indirect)'").ToString()));
            }
            else
            {
                NetIncome = GrossProfit;
            }
            if (RcL > RcR)
            {
                Trows = RcL;
            }
            else
            {
                Trows = RcR;
            }
            if (NetIncome > NetExp)
            {
                dtReport.Rows.Add();
                dtReport.Rows[Trows]["NameL"] = "<b>Net Profit";
                dtReport.Rows[Trows]["AmtL"] = 0;
                dtReport.Rows[Trows]["AmtL2"] = NetIncome - NetExp;
                dtReport.Rows[Trows]["NameR"] = "";
                dtReport.Rows[Trows]["AmtR"] = 0;
                dtReport.Rows[Trows]["AmtR2"] = 0;
                Trows++;
                dtReport.Rows.Add();
                dtReport.Rows[Trows]["NameL"] = "<b>Net Total";
                dtReport.Rows[Trows]["AmtL"] = 0;
                dtReport.Rows[Trows]["AmtL2"] = NetIncome;
                dtReport.Rows[Trows]["NameR"] = "<b>Net Total";
                dtReport.Rows[Trows]["AmtR"] = 0;
                dtReport.Rows[Trows]["AmtR2"] = NetIncome;
                Trows++;
            }
            else if (NetExp > NetIncome)
            {
                dtReport.Rows.Add();
                dtReport.Rows[Trows]["NameL"] = "";
                dtReport.Rows[Trows]["AmtL"] = 0;
                dtReport.Rows[Trows]["AmtL2"] = 0;
                dtReport.Rows[Trows]["NameR"] = "<b>Net Loss";
                dtReport.Rows[Trows]["AmtR"] = 0;
                dtReport.Rows[Trows]["AmtR2"] = NetExp - NetIncome;
                Trows++;


                dtReport.Rows.Add();
                dtReport.Rows[Trows]["NameL"] = "<b>Net Total";
                dtReport.Rows[Trows]["AmtL"] = 0;
                dtReport.Rows[Trows]["AmtL2"] = NetExp;
                dtReport.Rows[Trows]["NameR"] = "<b>Net Total";
                dtReport.Rows[Trows]["AmtR"] = 0;
                dtReport.Rows[Trows]["AmtR2"] = NetExp;
                Trows++;
            }


            string[,] col = new string[0, 0];
            string[,] Cwidth = new string[6, 6] { 
          
            { "Account", "260", "0","","","" }, 
            { "", "120", "1","" ,"",""}, 
            { "Expenses", "120", "1","" ,"",""}, 
         
            { "Account", "260", "0","","","" }, 
            { "", "120", "1","" ,"",""},
            { "Income", "120", "1","" ,"",""}
            };

            CreateReport(dtReport, col, Cwidth);
            return true;

        }



        public bool BalanceSheet(DateTime DateFrom, DateTime DateTo)
        {
            DataTable dtReport = new DataTable();
            int RcL = 0;
            int RcR = 0;
            stdt = DateFrom;
            endt = DateTo;
            dateTimePicker1.Value = DateFrom;
            dateTimePicker2.Value = DateTo;
            frmptyp = "Balance Sheet";

            this.Text = frmptyp;
            label3.Enabled = false;
            textBox1.Enabled = false;
            DecsOfReport = "Balance Sheet, as on " + DateTo.ToString(Database.dformat);

            //if (Database.IsKacha == false)
            //{
               sql = "SELECT ACCOUNTYPEs.Name AS Aname, X.Name,Sum(X.Cr)-Sum(X.Dr) AS Amount FROM ((SELECT QryJournal.Name, sum(QryJournal.Dr) as Dr, sum(QryJournal.Cr) as Cr From QryJournal Where (((QryJournal.Vdate)  <= " + access_sql.Hash + DateTo.ToString(Database.dformat) + access_sql.Hash + ")) GROUP BY QryJournal.Name, QryJournal.A HAVING (((QryJournal.A)=" + access_sql.Singlequote + "True" + access_sql.Singlequote + ")) UNION ALL SELECT QryAccountinfo.Name, QryAccountinfo.Dr as Dr, QryAccountinfo.Cr as Cr FROM QryAccountinfo)  AS X LEFT JOIN ACCOUNTs ON X.Name = ACCOUNTs.Name) LEFT JOIN ACCOUNTYPEs ON ACCOUNTs.Act_id = ACCOUNTYPEs.Act_id GROUP BY ACCOUNTYPEs.Name,  X.Name ORDER BY ACCOUNTYPEs.Name, X.Name";

            //}
            //else
            //{
            //    sql = "SELECT  ACCOUNTYPE.Name AS Aname, X.Name,Sum(X.Cr)-Sum(X.Dr) AS Amount FROM ((SELECT QryJournal.Name, sum(QryJournal.Dr) as Dr, sum(QryJournal.Cr) as Cr From QryJournal Where (((QryJournal.Vdate)  <= " + access_sql.Hash + DateTo.ToString(Database.dformat) + access_sql.Hash + ")) GROUP BY QryJournal.Name, QryJournal.B HAVING (((QryJournal.B)=" + access_sql.Singlequote + "True" + access_sql.Singlequote + ")) UNION ALL SELECT QryAccountinfo.Name, QryAccountinfo.Dr2 as Dr, QryAccountinfo.Cr2 as Cr FROM QryAccountinfo)  AS X LEFT JOIN ACCOUNT ON X.Name = ACCOUNT.Name) LEFT JOIN ACCOUNTYPE ON ACCOUNT.Act_id = ACCOUNTYPE.Act_id GROUP BY ACCOUNTYPE.Name,  X.Name ORDER BY ACCOUNTYPE.Name, X.Name";
            //}

            dt.Clear();
            Database.GetSqlData(sql, dt);

            dtReport.Columns.Add("NameL", typeof(string));
            dtReport.Columns.Add("AmtL", typeof(decimal));
            dtReport.Columns.Add("AmtL2", typeof(decimal));
            dtReport.Columns.Add("NameR", typeof(string));
            dtReport.Columns.Add("AmtR", typeof(decimal));
            dtReport.Columns.Add("AmtR2", typeof(decimal));

            //Libilities
            RcL = FillTFormat(dtReport, "'CAPITAL ACCOUNT'", "Capital", 'L', RcL);
            int PnL = RcL;
            dtReport.Rows.Add();
            dtReport.Rows[RcL]["NameL"] = "Profit/Loss";
            dtReport.Rows[RcL]["AmtL"] = 0;
            dtReport.Rows[RcL]["AmtL2"] = 0;
            dtReport.Rows[RcL]["NameR"] = "";
            dtReport.Rows[RcL]["AmtR"] = 0;
            dtReport.Rows[RcL]["AmtR2"] = 0;
            RcL++;
            RcL = FillTFormat(dtReport, "'CURRENT LIABILITIES'", "Current Libilities", 'L', RcL);
            RcL = FillTFormat(dtReport, "'DUTIES & TAXES'", "Duties & Taxes", 'L', RcL);
            RcL = FillTFormat(dtReport, "'SUNDRY CREDITORS'", "Sundry Creditors", 'L', RcL);
            RcL = FillTFormat(dtReport, "'RESERVES  & SURPLUS'", "Reserves & Surplus", 'L', RcL);
            RcL = FillTFormat(dtReport, "'SUSPENSE ACCOUNT (Temporary A/C)'", "Suspense", 'L', RcL);
            RcL = FillTFormat(dtReport, "'BANK OCC A/C'", "Bank Occ A/c", 'L', RcL);
            RcL = FillTFormat(dtReport, "'PROVISIONS'", "Provisions", 'L', RcL);
            RcL = FillTFormat(dtReport, "'SECURE LOANS'", "Secure Loans", 'L', RcL);
            RcL = FillTFormat(dtReport, "'UNSECURE LOANS'", "Unsecure Loans", 'L', RcL);

            double dif = Database.GetScalarDecimal("SELECT Sum(ACCOUNTs.Balance) AS Diff FROM ACCOUNTs");
            if (dif != 0)
            {
                if (RcL == dtReport.Rows.Count)
                {
                    dtReport.Rows.Add();
                    dtReport.Rows[RcL]["AmtL"] = 0;
                    dtReport.Rows[RcL]["AmtL2"] = 0;
                    dtReport.Rows[RcL]["AmtR"] = 0;
                    dtReport.Rows[RcL]["AmtR2"] = 0;
                }
                dtReport.Rows[RcL]["NameL"] = "<b>" + "Difference in Opening Trial";
                dtReport.Rows[RcL]["AmtL2"] = dif;
                RcL++;
            }

            //Assets
            RcR = FillTFormat(dtReport, "'FIXED ASSETS'", "Fixed Assets", 'R', RcR);
            RcR = FillTFormat(dtReport, "'CURRENT ASSETS'", "Currents Assets", 'R', RcR);
            RcR = FillTFormat(dtReport, "'INVESTMENTS'", "Investments", 'R', RcR);
            RcR = FillTFormat(dtReport, "'SUNDRY DEBTORS'", "Sundry Debitors", 'R', RcR);
            RcR = FillTFormat(dtReport, "'SECURITY & DEPOSITS (Assets)'", "Security & Deposits", 'R', RcR);
            RcR = FillTFormat(dtReport, "'LOAN & ADVANCES (Assests)'", "Loan & Advances", 'R', RcR);
            RcR = FillTFormat(dtReport, "'STOCK-IN-HAND'", "Closing Stock", 'R', RcR);
            RcR = FillTFormat(dtReport, "'CASH-IN-HAND'", "Cash in Hand", 'R', RcR);
            RcR = FillTFormat(dtReport, "'BANK ACCOUNTS'", "Bank in Hand", 'R', RcR);


            double TLibilities = double.Parse(dtReport.Compute("sum(AmtL2)", "").ToString());
            double TAssets = double.Parse(dtReport.Compute("sum(AmtR2)", "").ToString());
            double Profit = 0;
            double Loss = 0;
            if (TAssets > TLibilities)
            {
                Profit = TAssets - TLibilities;

                dtReport.Rows[PnL]["NameL"] = "<b>Net Profit";
                dtReport.Rows[PnL]["AmtL"] = 0;
                dtReport.Rows[PnL]["AmtL2"] = Profit;

            }
            else if (TLibilities > TAssets)
            {
                Loss = TLibilities - TAssets;
                dtReport.Rows[PnL]["NameL"] = "<b>Net Loss";
                dtReport.Rows[PnL]["AmtL"] = 0;
                dtReport.Rows[PnL]["AmtL2"] = Loss * -1;
            }

            string[,] col = new string[0, 0];
            string[,] Cwidth = new string[6, 6] { 
          
            { "Libilities", "260", "0","Total","","" }, 
            { "", "120", "1","" ,"",""}, 
            { "Amount", "120", "1","|Sum(AmtL2)" ,"",""}, 
         
            { "Assets", "260", "0","Total","","" }, 
            { "", "120", "1","" ,"",""},
            { "Amount", "120", "1","|Sum(AmtR2)" ,"",""}
            };

            CreateReport(dtReport, col, Cwidth);
            return true;


        }

        public bool AccountGroupBalance(DateTime DateFrom, DateTime DateTo)
        {

            double totdr = 0;
            double totcr = 0;
            stdt = DateFrom;
            endt = DateTo;
            dateTimePicker1.Value = DateFrom;
            dateTimePicker1.Visible = false;
            label1.Visible = false;
            dateTimePicker2.Value = DateTo;
            frmptyp = "Payment Collector Balance";
            this.Text = frmptyp;
            label3.Enabled = false;
            textBox1.Enabled = false;


            //if (Database.IsKacha == false)
            //{

                sql = "SELECT " + access_sql.fnstring("[GroupName]=null", "ActName", "[GroupName]") + " AS AccountGroup, Test.Name, Sum(Test.Dr) AS Dr, Sum(Test.Cr) AS Cr FROM (SELECT ACCOUNTYPEs.Name as ActName, OTHERs.Name as GroupName, ACCOUNTs.Name,  " + access_sql.fnstring("ACCOUNTs.Balance>0", "ACCOUNTs.Balance", "0") + " AS Dr, " + access_sql.fnstring("ACCOUNTs.Balance<0", "-1*(ACCOUNTs.Balance)", "0") + " AS Cr FROM (ACCOUNTs LEFT JOIN ACCOUNTYPEs ON ACCOUNTs.Act_id = ACCOUNTYPEs.Act_id) LEFT JOIN OTHER ON ACCOUNTs.Loc_id = OTHER.Oth_id union all SELECT ACCOUNTYPEs.Name as ActName, OTHERs.Name AS GroupName, ACCOUNTs.Name," + access_sql.fnstring("JOURNALs.Amount>0", "JOURNALs.Amount", "0") + " AS Dr, " + access_sql.fnstring("JOURNALs.Amount<0", "-1*(JOURNALs.Amount)", "0") + " AS Cr FROM (VOUCHERINFOs LEFT JOIN VOUCHERTYPEs ON VOUCHERINFOs.Vt_id = VOUCHERTYPEs.Vt_id) LEFT JOIN (((JOURNALs LEFT JOIN ACCOUNTs ON JOURNALs.Ac_id = ACCOUNTs.Ac_id) LEFT JOIN ACCOUNTYPEs ON ACCOUNTs.Act_id = ACCOUNTYPEs.Act_id) LEFT JOIN OTHER ON ACCOUNTs.Loc_id = OTHERs.Oth_id) ON VOUCHERINFOs.Vi_id = JOURNALs.Vi_id WHERE (((JOURNALs.Vdate)<=" + access_sql.Hash + DateTo.ToString(Database.dformat) + access_sql.Hash + ") AND ((VOUCHERTYPEs.A)=" + access_sql.Singlequote + "True" + access_sql.Singlequote + ")))  AS Test GROUP BY " + access_sql.fnstring("[GroupName]=null", "ActName", "[GroupName]") + ", Test.Name";
            //}
            //else
            //{
            //    sql = "SELECT " + access_sql.fnstring("[GroupName]=null", "ActName", "[GroupName]") + " AS AccountGroup, Test.Name, Sum(Test.Dr) AS Dr, Sum(Test.Cr) AS Cr FROM (SELECT ACCOUNTYPE.Name as ActName, OTHER.Name as GroupName, ACCOUNT.Name,  " + access_sql.fnstring("ACCOUNT.Balance2>0", "ACCOUNT.Balance2", "0") + " AS Dr, " + access_sql.fnstring("ACCOUNT.Balance<0", "-1*(ACCOUNT.Balance)", "0") + " AS Cr FROM (ACCOUNT LEFT JOIN ACCOUNTYPE ON ACCOUNT.Act_id = ACCOUNTYPE.Act_id) LEFT JOIN OTHER ON ACCOUNT.Loc_id = OTHER.Oth_id union all SELECT ACCOUNTYPE.Name as ActName, OTHER.Name AS GroupName, ACCOUNT.Name" + access_sql.fnstring("JOURNAL.Amount>0", "JOURNAL.Amount", "0") + " AS Dr, " + access_sql.fnstring("JOURNAL.Amount<0", "-1*(JOURNAL.Amount)", "0") + " AS Cr FROM (VOUCHERINFO LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id) LEFT JOIN (((JOURNAL LEFT JOIN ACCOUNT ON JOURNAL.Ac_id = ACCOUNT.Ac_id) LEFT JOIN ACCOUNTYPE ON ACCOUNT.Act_id = ACCOUNTYPE.Act_id) LEFT JOIN OTHER ON ACCOUNT.Loc_id = OTHER.Oth_id) ON VOUCHERINFO.Vi_id = JOURNAL.Vi_id WHERE (((JOURNAL.Vdate)<=" + access_sql.Hash + DateTo.ToString(Database.dformat) + access_sql.Hash + ") AND ((VOUCHERTYPE.A)=" + access_sql.Singlequote + "True" + access_sql.Singlequote + ")))  AS Test GROUP BY " + access_sql.fnstring("[GroupName]=null", "ActName", "[GroupName]") + ", Test.Name";
            //}

            dt.Clear();
            Database.GetSqlData(sql, dt);
            dt.DefaultView.Sort = "AccountGroup,Name";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                totdr = double.Parse(dt.Rows[i]["Dr"].ToString());
                totcr = double.Parse(dt.Rows[i]["Cr"].ToString());
                if (totdr > totcr)
                {
                    dt.Rows[i]["Dr"] = totdr - totcr;
                    dt.Rows[i]["Cr"] = 0;
                }
                else if (totcr > totdr)
                {
                    dt.Rows[i]["Dr"] = 0;
                    dt.Rows[i]["Cr"] = totcr - totdr;
                }
                else
                {
                    dt.Rows[i]["Dr"] = 0;
                    dt.Rows[i]["Cr"] = 0;
                }
            }

            if (dt.Select("not (Dr=0 and Cr=0)").Length == 0)
            {
                return false;
            }

            tdt = dt.Select("not (Dr=0 and Cr=0)").CopyToDataTable();
            if (tdt.Rows.Count == 0)
            {
                return false;
            }
            totdr = double.Parse(tdt.Compute("sum(Dr)", "").ToString());
            totcr = double.Parse(tdt.Compute("sum(Cr)", "").ToString());
            if (totdr > totcr)
            {
                tdt.Rows.Add("", "Difference in Opening Trial", "0", totdr - totcr);
            }
            else if (totdr <= totcr)
            {
                tdt.Rows.Add("", "Difference in Opening Trial", totcr - totdr, "0");
            }

            string[,] col = new string[1, 3] { { "AccountGroup", "1", "1" } };
            string[,] Cwidth = new string[4, 6] { 
            { "Account Group", "0", "0","","","" }, 
            { "Account Name", "700", "0","Total Amount","","" }, 
            { "Amount (Dr.)", "150", "1","|sum(Dr)","+sum(Dr)-sum(Cr)","" }, 
            { "Amount (Cr.)", "150", "1","|sum(Cr)","+sum(Cr)-sum(Dr)","" } };
            CreateReport(tdt, col, Cwidth);
            return true;

        }

        public bool StandardTrial(DateTime DateFrom, DateTime DateTo)
        {
            double totdr = 0;
            double totcr = 0;
            stdt = DateFrom;
            endt = DateTo;
            frmptyp = "Standard Trial Balance";
            dateTimePicker1.Value = DateFrom;
            dateTimePicker2.Value = DateTo;
            label3.Enabled = false;
            textBox1.Enabled = false;
            this.Text = frmptyp;
            DecsOfReport = "Trial Balance, as on " + DateTo.ToString(Database.dformat);
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();

            //if (Database.IsKacha == false)
            //{
               sql = "SELECT X.Name, sum(X.Dr) AS Dr, sum(X.Cr) AS Cr FROM (SELECT QryJournal.Name, Sum(QryJournal.Dr) AS Dr, Sum(QryJournal.Cr) AS Cr FROM QryJournal WHERE (((QryJournal.Vdate)<=" + access_sql.Hash + DateTo.ToString(Database.dformat) + access_sql.Hash + ")) GROUP BY QryJournal.Name, QryJournal.A HAVING (((QryJournal.A)=" + access_sql.Singlequote + "True" + access_sql.Singlequote + ")) UNION ALL SELECT QryAccountinfo.Name, QryAccountinfo.Dr as Dr, QryAccountinfo.Cr as Cr FROM QryAccountinfo)  AS X GROUP BY x.Name having (not sum(X.Dr)=0 or not sum(X.Cr)=0)";
            //}
            //else
            //{
            //    sql = "SELECT X.Name, sum(X.Dr) AS Dr, sum(X.Cr) AS Cr FROM (SELECT QryJournal.Name, Sum(QryJournal.Dr) AS Dr, Sum(QryJournal.Cr) AS Cr FROM QryJournal WHERE (((QryJournal.Vdate)<=" + access_sql.Hash + DateTo.ToString(Database.dformat) + access_sql.Hash + ")) GROUP BY QryJournal.Name, QryJournal.B HAVING (((QryJournal.B)=" + access_sql.Singlequote + "True" + access_sql.Singlequote + ")) UNION ALL SELECT QryAccountinfo.Name, QryAccountinfo.Dr2 as Dr, QryAccountinfo.Cr2 as Cr FROM QryAccountinfo)  AS X GROUP BY x.Name having (not sum(X.Dr)=0 or not sum(X.Cr)=0)";
            //}
            dt.Clear();
            Database.GetSqlData(sql, dt);

            dt.DefaultView.Sort = "Name";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                totdr = double.Parse(dt.Rows[i]["Dr"].ToString());
                totcr = double.Parse(dt.Rows[i]["Cr"].ToString());
                if (totdr > totcr)
                {
                    dt.Rows[i]["Dr"] = totdr - totcr;
                    dt.Rows[i]["Cr"] = 0;
                }
                else if (totcr > totdr)
                {
                    dt.Rows[i]["Dr"] = 0;
                    dt.Rows[i]["Cr"] = totcr - totdr;
                }
                else
                {
                    dt.Rows[i]["Dr"] = 0;
                    dt.Rows[i]["Cr"] = 0;
                }
            }

            if (dt.Rows.Count == 0)
            {
                return false;
            }

            tdt = dt.Select("not (Dr=0 and Cr=0)").CopyToDataTable();
            if (tdt.Rows.Count == 0)
            {
                return false;
            }
            totdr = double.Parse(tdt.Compute("sum(Dr)", "").ToString());
            totcr = double.Parse(tdt.Compute("sum(Cr)", "").ToString());
            if (totdr > totcr)
            {
                tdt.Rows.Add("Difference in Opening Trial", "0", totdr - totcr);
            }
            else if (totdr < totcr)
            {
                tdt.Rows.Add("Difference n Opening Trial", totcr - totdr, "0");
            }

            string[,] col = new string[0, 0];
            string[,] Cwidth = new string[3, 6] { 
            { "Account", "700", "0","Total Amount" ,"",""}, 
            { "Amount (Dr.)", "150", "1","|sum(Dr)","","" }, 
            { "Amount (Cr.)", "150", "1","|sum(Cr)" ,"",""} };

            CreateReport(tdt, col, Cwidth);

            return true;
        }

        public bool CustomerDetailBillWise(DateTime DateFrom, DateTime DateTo, string accnm)
        {
            stdt = DateFrom;
            endt = DateTo;
            frmptyp = "Customer Detail Bill Wise";
            dateTimePicker1.Value = DateFrom;
            dateTimePicker2.Value = DateTo;
            AccName = accnm;
            textBox1.Text = accnm;
            label3.Enabled = true;
            textBox1.Enabled = true;
            this.Text = frmptyp;
            DecsOfReport = "Customer Detail Bill Wise, for the period of " + DateFrom.ToString(Database.dformat) + " to " + DateTo.ToString(Database.dformat);
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();

            //if (Database.IsKacha == false)
            //{
               sql = "SELECT QryItemTranjection.Type, QryItemTranjection.Vdate,QryItemTranjection.DocNumber,  [ACCOUNT.Name] , QryItemTranjection.Description, QryItemTranjection.Quantity, QryItemTranjection.Rate_am, QryItemTranjection.Packing, QryItemTranjection.ItemAmount, QryItemTranjection.VoucherNetAmt FROM QryItemTranjection WHERE (((QryItemTranjection.[Type])='Sale' Or (QryItemTranjection.[Type])='Return') AND ((QryItemTranjection.A)=" + access_sql.Singlequote + "True" + access_sql.Singlequote + "))";
            //}
            //else
            //{
            //    sql = "SELECT QryItemTranjection.Type, QryItemTranjection.Vdate, QryItemTranjection.DocNumber, [ACCOUNT.Name], QryItemTranjection.Description, QryItemTranjection.Quantity, QryItemTranjection.Rate_am, QryItemTranjection.Packing, QryItemTranjection.ItemAmount, QryItemTranjection.VoucherNetAmt FROM QryItemTranjection WHERE (((QryItemTranjection.[Type])='Sale' Or (QryItemTranjection.[Type])='Return') AND ((QryItemTranjection.B)=" + access_sql.Singlequote + "True" + access_sql.Singlequote + "))";
            //}
            dt = new DataTable();
            Database.GetSqlData(sql, dt);


            dt.Columns.Add("TAmount", typeof(decimal));
            DataRow[] drow;
            if (accnm == "")
            {
                drow = dt.Select("[ACCOUNT.Name] is not null and Vdate>=" + access_sql.Hash + DateFrom.ToString(Database.dformat) + access_sql.Hash + " and Vdate<=" + access_sql.Hash + DateTo.ToString(Database.dformat) + access_sql.Hash + "", "Type,DocNumber");
            }
            else
            {
                drow = dt.Select("[ACCOUNT.Name]='" + accnm + "' and Vdate>=" + access_sql.Hash + DateFrom.ToString(Database.dformat) + access_sql.Hash + " and Vdate<=" + access_sql.Hash + DateTo.ToString(Database.dformat) + access_sql.Hash + "", "Type,DocNumber");
            }
            tdt.Clear();


            //double Total = Database.GetScalarDecimal("Select sum(Totalamount) as Totalamount from voucherinfo where Ac_id=" + funs.Select_ac_id(accnm) + " and Vdate>=#" + DateFrom.ToString(Database.dformat) + "# and Vdate<=#" + DateTo.ToString(Database.dformat) + "#");

            if (drow.GetLength(0) > 0)
            {
                tdt = drow.CopyToDataTable();
                for (int i = 0; i < tdt.Rows.Count; i++)
                {
                    //if (Database.IsKacha == false)
                    //{
                        tdt.Rows[i]["Tamount"] = Database.GetScalarDecimal("SELECT Sum(" + access_sql.fnstring("type='Sale' Or type='Purchase'", "VOUCHERINFO.Totalamount", "-1*(VOUCHERINFO.Totalamount)") + ") AS total FROM VOUCHERINFO LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id WHERE (((VOUCHERINFO.Ac_id)='" + funs.Select_ac_id(accnm) + "') AND ((VOUCHERINFO.Vdate)>=" + access_sql.Hash + DateFrom.ToString(Database.dformat) + access_sql.Hash + " And (VOUCHERINFO.Vdate)<=" + access_sql.Hash + DateTo.ToString(Database.dformat) + access_sql.Hash + ") AND ((VOUCHERTYPE.A)=" + access_sql.Singlequote + "True" + access_sql.Singlequote + "))");
                    //}
                    //else
                    //{
                    //    tdt.Rows[i]["Tamount"] = Database.GetScalarDecimal("SELECT Sum(" + access_sql.fnstring("type='Sale' Or type='Purchase'", "VOUCHERINFO.Totalamount", "-1*(VOUCHERINFO.Totalamount)") + ") AS total FROM VOUCHERINFO LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id WHERE (((VOUCHERINFO.Ac_id)='" + funs.Select_ac_id(accnm) + "') AND ((VOUCHERINFO.Vdate)>=" + access_sql.Hash + DateFrom.ToString(Database.dformat) + access_sql.Hash + " And (VOUCHERINFO.Vdate)<=" + access_sql.Hash + DateTo.ToString(Database.dformat) + access_sql.Hash + ") AND ((VOUCHERTYPE.B)=" + access_sql.Singlequote + "True" + access_sql.Singlequote + "))");
                    //}
                    if (tdt.Rows[i]["Type"].ToString() == "Return" || tdt.Rows[i]["Type"].ToString() == "P Return")
                    {
                        tdt.Rows[i]["Quantity"] = double.Parse(tdt.Rows[i]["Quantity"].ToString()) * -1;
                        tdt.Rows[i]["ItemAmount"] = double.Parse(tdt.Rows[i]["ItemAmount"].ToString()) * -1;
                    }
                }

                tdt.Columns.Remove("Type");
                tdt.DefaultView.Sort = "Vdate";
                tdt = tdt.DefaultView.ToTable();

            }
            if (tdt.Rows.Count == 0)
            {
                return false;
            }

            string[,] col = new string[2, 3] {
            
            {"Vdate", "1", "0" },
            {"DocNumber", "0", "1" } };


            string[,] Cwidth = new string[10, 6] { 
            { "Vdate", "", "0","","","" },
            { "DocNumber", "200", "0","","","" },
            { "Party Name", "150", "0","","","" },
            { "Description", "250", "0","TotalAmount","","Amount" },
            { "Quantity", "100", "0","","","|sum(ItemAmount)" },
            { "Rate_am", "100", "0","","","(+)" },
            { "Packing", "100", "0","","","|max(VoucherNetAmt)-sum(ItemAmount)" },
            { "ItemAmount", "100", "0","|max(Tamount)","","|max(VoucherNetAmt)" },
            { "VoucherNetAmt", "0", "0","0","",""},
            { "TAmount", "0", "0","0","",""}
            };


            CreateReport(tdt, col, Cwidth);
            return true;

        }


        public bool ItemLedger(DateTime DateFrom, DateTime DateTo, string GodownName, int des_id)
        {
            double totdr = 0;
            double totcr = 0;
            gGodownName = GodownName;
            stdt = DateFrom;
            endt = DateTo;
            frmptyp = "Item Ledger";
            dateTimePicker1.Value = DateFrom;
            dateTimePicker2.Value = DateTo;

            string description = Database.GetScalarText("SELECT DESCRIPTION.Description " + access_sql.Concat + " ' (' " + access_sql.Concat + " PACK AS des FROM DESCRIPTION  WHERE DESCRIPTION.Des_id=" + des_id + " GROUP BY DESCRIPTION.Description " + access_sql.Concat + " ' (' " + access_sql.Concat + " PACK");

            textBox1.Text = description;

            label3.Enabled = true;
            textBox1.Enabled = true;
            this.Text = frmptyp;
            DecsOfReport = "Item Ledger of <" + description + ")>, for the period of " + DateFrom.ToString(Database.dformat) + " to " + DateTo.ToString(Database.dformat);
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();

            //if (Database.IsKacha == false)
            //{
            sql = "SELECT res.Godown, res.Vdate, res.DocNumber, res.Party, Sum(res.Receive) AS Receive, Sum(res.Issue) AS Issue, res.Did, res.godown_id FROM (SELECT   " + access_sql.fnstring("ACCOUNT.Name Is Null", "'<MAIN>'", "ACCOUNT.Name") + " AS Godown, " + access_sql.Hash + "2/1/1801" + access_sql.Hash + " as Vdate, 'Opening' AS DocNumber, '' AS Party, Sum(Stock.Receive) -Sum(Stock.Issue) AS Receive,0 as Issue, DESCRIPTION.Des_id as Did, Stock.godown_id FROM (((Stock LEFT JOIN DESCRIPTION ON Stock.Did = DESCRIPTION.Des_id) LEFT JOIN VOUCHERINFO ON Stock.Vid = VOUCHERINFO.Vi_id) LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id) LEFT JOIN ACCOUNT ON Stock.godown_id = ACCOUNT.Ac_id GROUP BY ACCOUNT.Name, VOUCHERINFO.Vdate, DESCRIPTION.Des_id, Stock.godown_id, Stock.marked HAVING (((VOUCHERINFO.Vdate)<" + access_sql.Hash + dateTimePicker1.Value.Date.ToString(Database.dformat) + access_sql.Hash + ") AND ((Stock.marked)=" + access_sql.Singlequote + "False" + access_sql.Singlequote + ")) or (Voucherinfo.Vdate is null) ";
            sql += " Union all SELECT " + access_sql.fnstring("ACCOUNT.Name Is Null", "'<MAIN>'", "ACCOUNT.Name") + " AS Godown, VOUCHERINFO.Vdate,  " + access_sql.Docnumber + " AS DocNumber, ACCOUNT_1.Name AS Party, Sum(Stock.Receive) AS Receive, Sum(Stock.Issue) AS Issue, DESCRIPTION.Des_id  as Did, Stock.godown_id FROM ((((Stock LEFT JOIN DESCRIPTION ON Stock.Did = DESCRIPTION.Des_id) LEFT JOIN VOUCHERINFO ON Stock.Vid = VOUCHERINFO.Vi_id) LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id) LEFT JOIN ACCOUNT ON Stock.godown_id = ACCOUNT.Ac_id) LEFT JOIN ACCOUNT AS ACCOUNT_1 ON VOUCHERINFO.Ac_id = ACCOUNT_1.Ac_id GROUP BY ACCOUNT.Name, VOUCHERINFO.Vdate,  " + access_sql.Docnumber + ", ACCOUNT_1.Name, DESCRIPTION.Des_id, Stock.godown_id, Stock.marked HAVING (((VOUCHERINFO.Vdate)>=" + access_sql.Hash + dateTimePicker1.Value.Date.ToString(Database.dformat) + access_sql.Hash + " And (VOUCHERINFO.Vdate)<=" + access_sql.Hash + dateTimePicker2.Value.Date.ToString(Database.dformat) + access_sql.Hash + ") AND ((Stock.marked)=" + access_sql.Singlequote + "False" + access_sql.Singlequote + ")) )  AS res GROUP BY res.Godown, res.Vdate, res.DocNumber, res.Party, res.Did, res.godown_id;";
            //}
            //else
            //{
            //    sql = "SELECT res.Godown, res.Vdate, res.DocNumber, res.Party, Sum(res.Receive) AS Receive, Sum(res.Issue) AS Issue, res.Did, res.godown_id FROM (SELECT   " + access_sql.fnstring("ACCOUNT.Name Is Null", "'<MAIN>'", "ACCOUNT.Name") + " AS Godown, " + access_sql.Hash + "2/1/1801" + access_sql.Hash + " as Vdate, 'Opening' AS DocNumber, '' AS Party, Sum(Stock.Receive) -Sum(Stock.Issue) AS Receive,0 as Issue, DESCRIPTION.Des_id  as Did, Stock.godown_id FROM (((Stock LEFT JOIN DESCRIPTION ON Stock.Did = DESCRIPTION.Des_id) LEFT JOIN VOUCHERINFO ON Stock.Vid = VOUCHERINFO.Vi_id) LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id) LEFT JOIN ACCOUNT ON Stock.godown_id = ACCOUNT.Ac_id GROUP BY ACCOUNT.Name, VOUCHERINFO.Vdate, DESCRIPTION.Des_id, Stock.godown_id, Stock.marked HAVING (((VOUCHERINFO.Vdate)<" + access_sql.Hash + dateTimePicker1.Value.Date.ToString(Database.dformat) + access_sql.Hash + ") AND ((Stock.marked)=" + access_sql.Singlequote + "true" + access_sql.Singlequote + ")) or (Voucherinfo.Vdate is null) ";
            //    sql += " Union all SELECT " + access_sql.fnstring("ACCOUNT.Name Is Null", "'<MAIN>'", "ACCOUNT.Name") + " AS Godown, VOUCHERINFO.Vdate,  " + access_sql.Docnumber + " AS DocNumber, ACCOUNT_1.Name AS Party, Sum(Stock.Receive) AS Receive, Sum(Stock.Issue) AS Issue, DESCRIPTION.Des_id  as Did, Stock.godown_id FROM ((((Stock LEFT JOIN DESCRIPTION ON Stock.Did = DESCRIPTION.Des_id) LEFT JOIN VOUCHERINFO ON Stock.Vid = VOUCHERINFO.Vi_id) LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id) LEFT JOIN ACCOUNT ON Stock.godown_id = ACCOUNT.Ac_id) LEFT JOIN ACCOUNT AS ACCOUNT_1 ON VOUCHERINFO.Ac_id = ACCOUNT_1.Ac_id GROUP BY ACCOUNT.Name, VOUCHERINFO.Vdate,  " + access_sql.Docnumber + ", ACCOUNT_1.Name, DESCRIPTION.Des_id, Stock.godown_id, Stock.marked HAVING (((VOUCHERINFO.Vdate)>=" + access_sql.Hash + dateTimePicker1.Value.Date.ToString(Database.dformat) + access_sql.Hash + " And (VOUCHERINFO.Vdate)<=" + access_sql.Hash + dateTimePicker2.Value.Date.ToString(Database.dformat) + access_sql.Hash + ") AND ((Stock.marked)=" + access_sql.Singlequote + "true" + access_sql.Singlequote + ")) )  AS res GROUP BY res.Godown, res.Vdate, res.DocNumber, res.Party, res.Did, res.godown_id;";
            //}
            dt.Clear();
            Database.GetSqlData(sql, dt);
            DataRow[] drow;
            if (GodownName == "")
            {
                drow = dt.Select("Godown is not null And  Did='" + des_id + "' ", "Vdate");
            }
            else
            {
                drow = dt.Select("Godown='" + GodownName + "' And  Did=" + des_id + " ", "Vdate");
            }
            tdt.Clear();
            if (drow.GetLength(0) > 0)
            {
                tdt = drow.CopyToDataTable();

                tdt.DefaultView.Sort = "Godown,Vdate,DocNumber";
                tdt = tdt.DefaultView.ToTable();
                tdt.Columns.Add("TBalance", typeof(decimal));
                for (int i = 0; i < tdt.Rows.Count; i++)
                {
                    totdr += double.Parse(tdt.Rows[i]["receive"].ToString());
                    totcr += double.Parse(tdt.Rows[i]["issue"].ToString());
                    if (totdr > totcr)
                    {
                        tdt.Rows[i]["TBalance"] = totdr - totcr;
                    }
                    else if (totcr > totdr)
                    {
                        tdt.Rows[i]["TBalance"] = (totcr - totdr) * -1;
                    }
                    else
                    {
                        tdt.Rows[i]["TBalance"] = "0";
                    }

                }


            }

            if (tdt.Rows.Count == 0)
            {

                return false;
            }

            string[,] col = new string[1, 3] { { "Godown", "1", "1" } };

            string[,] Cwidth = new string[9, 6] { 
            { "Godown", "0", "0","","","" },
            { "Vdate", "150", "0","","","" },
            { "DocNumber", "150", "0","Total","Total","" },
            { "Party Name", "200", "0","","","" },
            { "Inflow", "150", "1","|sum(receive)","|sum(receive)","" },
            { "Outflow", "150","1","|sum(issue)","|sum(issue)","" },
            { "Des_ac_id", "0", "0","","","" }, 
            { "godown_id", "0", "0","" ,"" ,"" }  ,
            { "Closing Stock", "200", "0","" ,"" ,""} 

            };


            CreateReport(tdt, col, Cwidth);
            return true;
        }

        public bool AddressPrinting(string accnm)
        {
            string tPath = Path.GetTempPath() + DateTime.Now.ToString("yyMMddhmmssfff") + ".pdf";
            PdfPrinting(tPath, accnm);
            PdfReader frm = new PdfReader();
            frm.LoadFile(tPath);
            frm.Show();
            return true;
        }

        private void PdfPrinting(string Path, string name)
        {
            FileStream fs = new FileStream(Path, FileMode.Create, FileAccess.Write, FileShare.None);
            iTextSharp.text.Rectangle rec;
            Document document = new Document(PageSize.A5, 30f, 10f, 10f, 10f);
            PdfWriter writer = PdfWriter.GetInstance(document, fs);
            document.Open();
            HTMLWorker hw = new HTMLWorker(document);
            DataTable dtacc = new DataTable();
            //int ac_id = funs.Select_ac_id(name);
            string ac_id = funs.Select_ac_id(name);
            Database.GetSqlData("Select Name,Address1,Address2,Phone from ACCOUNT where Ac_id='" + ac_id + "'", dtacc);
            string str = "";
            str += @"<body> <font size='1'> <table>";
            for (int i = 0; i < dtacc.Rows.Count; i++)
            {
                str += "<tr>";
                str += "<td>";
                str += "To,";
                str += "</td>";
                str += "</tr>";
                for (int j = 0; j < dtacc.Columns.Count; j++)
                {
                    if (dtacc.Rows[i][j].ToString() != "None")
                    {
                        if (dtacc.Rows[i][j].ToString() != "0".ToString())
                        {
                            str += "<tr>";
                            str += "<td> " + dtacc.Rows[i][j].ToString() + " </td> ";
                            str += "</tr>";
                        }
                    }
                }
            }


            str += "</table></font></body>";
            StringReader sr = new StringReader(str);
            hw.Parse(sr);
            document.Close();
        }

        public bool OpeningTrial(DateTime DateFrom, DateTime DateTo)
        {
            double totdr = 0;
            double totcr = 0;
            stdt = DateFrom;
            endt = DateTo;
            frmptyp = "Opening Trial Balance";
            dateTimePicker1.Value = DateFrom;
            dateTimePicker2.Value = DateTo;
            label3.Enabled = false;
            textBox1.Enabled = false;
            this.Text = frmptyp;
            DecsOfReport = "Opening Trial Balance, as on " + DateTo.ToString(Database.dformat);
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
            //if (Database.IsKacha == false)
            //{
               sql = "Select Name, " + access_sql.fnstring("ACCOUNTs.Balance>0", "ACCOUNTs.Balance", "0") + " AS Dr, " + access_sql.fnstring("ACCOUNTs.Balance<0", "-1*(ACCOUNTs.Balance)", "0") + " AS Cr  from Accounts";
            //}
            //else
            //{
            //    sql = "Select Name, " + access_sql.fnstring("ACCOUNT.Balance2>0", "ACCOUNT.Balance2", "0") + " AS Dr, " + access_sql.fnstring("ACCOUNT.Balance2<0", "-1*(ACCOUNT.Balance2)", "0") + " AS Cr  from Account";
            //}


            dt.Clear();
            Database.GetSqlData(sql, dt);
            dt.DefaultView.Sort = "Name";
            if (dt.Select("not (Dr=0 and Cr=0)").Length == 0)
            {
                return false;
            }

            tdt = dt.Select("not (Dr=0 and Cr=0)").CopyToDataTable();
            if (tdt.Rows.Count == 0)
            {
                return false;
            }
            totdr = double.Parse(tdt.Compute("sum(Dr)", "").ToString());
            totcr = double.Parse(tdt.Compute("sum(Cr)", "").ToString());
            if (totdr > totcr)
            {
                tdt.Rows.Add("Difference", "0", totdr - totcr);
            }
            else if (totdr < totcr)
            {
                tdt.Rows.Add("Difference", totcr - totdr, "0");
            }

            string[,] col = new string[0, 0];
            string[,] Cwidth = new string[3, 6] { 
            { "Account", "700", "0" ,"Total Amount","",""}, 
            { "Amount (Dr.)", "150", "1","|sum(Dr)","","" }, 
            { "Amount (Cr.)", "150", "1","|sum(Cr)" ,"",""} };

            CreateReport(tdt, col, Cwidth);

            return true;
        }


        public bool MovedAccountSummary(DateTime DateFrom, DateTime DateTo)
        {
            double totdr = 0;
            double totcr = 0;
            stdt = DateFrom;
            endt = DateTo;
            frmptyp = "Moved Account Summary";
            dateTimePicker1.Value = DateFrom;
            dateTimePicker2.Value = DateTo;
            label3.Enabled = false;
            textBox1.Enabled = false;
            this.Text = frmptyp;
            DecsOfReport = "Moved Account Summary, for the period of " + DateFrom.ToString(Database.dformat) + " to " + DateTo.ToString(Database.dformat);
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
            //if (Database.IsKacha == false)
            //{
                sql = "SELECT QryJournal.Name, Sum(QryJournal.Dr) AS Dr, Sum(QryJournal.Cr) AS Cr FROM QryJournal where Vdate>=" + access_sql.Hash + DateFrom.ToString(Database.dformat) + access_sql.Hash + " and Vdate<=" + access_sql.Hash + DateTo.ToString(Database.dformat) + access_sql.Hash + " group by QryJournal.Name, QryJournal.A HAVING (((QryJournal.A)=" + access_sql.Singlequote + "True" + access_sql.Singlequote + "))";
            //}
            //else
            //{
            //    sql = "SELECT QryJournal.Name, Sum(QryJournal.Dr) AS Dr, Sum(QryJournal.Cr) AS Cr FROM QryJournal where Vdate>=" + access_sql.Hash + DateFrom.ToString(Database.dformat) + access_sql.Hash + " and Vdate<=" + access_sql.Hash + DateTo.ToString(Database.dformat) + access_sql.Hash + " group by QryJournal.Name, QryJournal.B HAVING (((QryJournal.B)=" + access_sql.Singlequote + "True" + access_sql.Singlequote + "))";
            //}
            dt.Clear();
            Database.GetSqlData(sql, dt);
            dt.DefaultView.Sort = "Name";
            if (dt.Select("not (Dr=0 and Cr=0)").Length == 0)
            {
                return false;

            }
            tdt = dt.Select("not (Dr=0 and Cr=0)").CopyToDataTable();
            if (tdt.Rows.Count == 0)
            {
                return false;
            }

            string[,] col = new string[0, 0];


            string[,] Cwidth = new string[3, 6] { 
            { "Account", "700", "0","Total Amount","","" }, 
            { "Amount (Dr.)", "150", "0","|sum(Dr)","","" }, 
            { "Amount (Cr.)", "150", "0","|sum(Cr)","","" } };

            CreateReport(tdt, col, Cwidth);

            return true;
        }


        public bool Journal(DateTime DateFrom, DateTime DateTo, string str, string loc)
        {
            gvtid = str;
            dataGridView1.Rows.Clear();
            frmptyp = "Journal";
            this.Text = frmptyp;
            stdt = DateFrom;
            endt = DateTo;
            dateTimePicker1.Value = DateFrom;
            dateTimePicker2.Value = DateTo;
            label3.Enabled = false;
            textBox1.Enabled = false;
            groupBox2.Visible = false;
            DecsOfReport = "Journal, for the period of " + DateFrom.ToString(Database.dformat) + " to " + DateTo.ToString(Database.dformat);

            if (str != "")
            {
                str = "(" + str + ") and ";
            }
            
               
            //sql = "SELECT CONVERT(nvarchar, Journals.Vdate, 106) AS Vdate, VOUCHERTYPEs.Short + ' ' + CONVERT(nvarchar, VOUCHERINFOs.Vdate, 112) + ' ' + CAST(VOUCHERINFOs.Vnumber AS nvarchar(10)) AS DocNumber, ACCOUNTs.Name, Journals.Narr, SUM(Journals.Amount) AS SumOfAmount, CASE WHEN SUM(Journals.Amount) > 0 THEN SUM(Journals.Amount) ELSE 0 END AS Dr, CASE WHEN SUM(Journals.Amount) < 0 THEN - 1 * SUM(Journals.Amount) ELSE 0 END AS Cr FROM ACCOUNTs RIGHT OUTER JOIN Journals LEFT OUTER JOIN VOUCHERTYPEs RIGHT OUTER JOIN VOUCHERINFOs ON VOUCHERTYPEs.Vt_id = VOUCHERINFOs.Vt_id ON Journals.Vi_id = VOUCHERINFOs.Vi_id ON ACCOUNTs.Ac_id = Journals.Ac_id WHERE " + str + " (Journals.Vdate >= " + access_sql.Hash + DateFrom.ToString(Database.dformat) + access_sql.Hash + ") AND (Journals.Vdate <= " + access_sql.Hash + DateTo.ToString(Database.dformat) + access_sql.Hash + ") GROUP BY Journals.Vdate, VOUCHERTYPEs.Short + ' ' + CONVERT(nvarchar, VOUCHERINFOs.Vdate, 112) + ' ' + CAST(VOUCHERINFOs.Vnumber AS nvarchar(10)), ACCOUNTs.Name, Journals.Narr, VOUCHERTYPEs.Short ORDER BY Journals.Vdate, VOUCHERTYPEs.Short, SumOfAmount DESC";
            sql = "SELECT  CONVERT(nvarchar, Journals.vdate, 106) AS Vdate,   VOUCHERINFOs.LocationId + ' ' + VOUCHERTYPEs.Short + ' ' + CONVERT(nvarchar, VOUCHERINFOs.Vdate, 112)   + ' ' + CAST( VOUCHERINFOs.Vnumber AS nvarchar(10)) AS DocNumber, ACCOUNTs.name, Journals.Narr, SUM( Journals.Amount) AS SumOfAmount,   CASE WHEN SUM(Journals.Amount) > 0 THEN SUM(Journals.Amount) ELSE 0 END AS Dr, CASE WHEN SUM(Journals.Amount) < 0 THEN - 1 * SUM(Journals.Amount)   ELSE 0 END AS Cr FROM ACCOUNTs RIGHT OUTER JOIN  Journals LEFT OUTER JOIN  VOUCHERTYPEs RIGHT OUTER JOIN  VOUCHERINFOs ON VOUCHERTYPEs.Vt_id = VOUCHERINFOs.Vt_id ON Journals.vi_id = VOUCHERINFOs.Vi_id ON   ACCOUNTs.ac_id = Journals.Ac_id WHERE " + str + loc +" (Journals.Vdate >= " + access_sql.Hash + DateFrom.ToString(Database.dformat) + access_sql.Hash + ") AND (Journals.Vdate <= " + access_sql.Hash + DateTo.ToString(Database.dformat) + access_sql.Hash + ")  GROUP BY Journals.vdate, VOUCHERINFOs.LocationId + ' ' + VOUCHERTYPEs.Short + ' ' + CONVERT(nvarchar, VOUCHERINFOs.Vdate, 112)   + ' ' + CAST( VOUCHERINFOs.Vnumber AS nvarchar(10)), ACCOUNTs.name, Journals.Narr, VOUCHERTYPEs.Short,  VOUCHERINFOs.LocationId ORDER BY Journals.vdate,Voucherinfos.Locationid,VOUCHERTYPEs.Short, SumOfAmount DESC";
            dt.Clear();

            Database.GetSqlData(sql, dt);

            if (dt.Rows.Count == 0)
            {
                return false;
            }

            dt.Columns.Remove("SumOfAmount");
            string[,] col = new string[2, 3] { { "Vdate", "1", "1" }, { "DocNumber", "0", "0" } };

            string[,] Cwidth = new string[6, 8] { 
            { "Vdate", "0", "0","","","" ,"",""}, 
          
            { "Doc Number", "200", "0","","","","",""}, 
            { "Account", "250", "0","" ,"","","",""}, 
            { "Narration", "350", "0","Total Amount","Day Total","Doc. Total","","" }, 
            { "Amount (Dr.)", "100", "1","|sum(Dr)","|sum(Dr)","|sum(Dr)" ,"",""}, 
            { "Amount (Cr.)", "100", "1","|sum(Cr)","|sum(Cr)","|sum(Cr)" ,"",""} };

            CreateReport(dt, col, Cwidth);
            dtFinal = dt.Copy();
            return true;
        }


        public bool ChallanReport(DataTable dt, DateTime DateFrom, DateTime DateTo)
        {
            dataGridView1.Rows.Clear();

            frmptyp = "Challan";
            this.Text = frmptyp;
            stdt = DateFrom;
            endt = DateTo;
            dateTimePicker1.Value = DateFrom;
            dateTimePicker2.Value = DateTo;
            label3.Enabled = false;
            textBox1.Enabled = false;
            DecsOfReport = "Challan Report, for the period of " + DateFrom.ToString(Database.dformat) + " to " + DateTo.ToString(Database.dformat);
            //sql = "SELECT DESCRIPTION.Description, sum(stock.qty) FROM DESCRIPTION LEFT JOIN stock ON DESCRIPTION.Des_id = stock.Des_id group by  DESCRIPTION.Description";

            //dt.Clear();
            //Database.GetSqlData(sql, dt);

            //DataRow[] drow;



            if (dt.Rows.Count == 0)
            {
                return false;
            }


            string[,] col = new string[0, 0]; // { { "Name", "1", "1" }, { "DocNo", "0", "0" } };

            string[,] Cwidth = new string[7, 6] { 
            { "Consigner", "225", "0","","","" },
            { "Consignee", "225", "0","","","" }, 
            { "GR No.", "100", "0","" ,"",""}, 
            { "No. Of Pack.", "150", "0","","","" },
            { "Goods", "100", "0","" ,"",""}, 
            { "Weight", "100", "0","","","" }, 
            { "Freight", "100", "0","","","" }};

            CreateReport(dt, col, Cwidth);

            return true;
        }

        public bool BillReport(string name, DateTime DateFrom, DateTime DateTo)
        {
            dataGridView1.Rows.Clear();

            frmptyp = "Bill";
            this.Text = frmptyp;
            stdt = DateFrom;
            endt = DateTo;
            dateTimePicker1.Value = DateFrom;
            dateTimePicker2.Value = DateTo;
            label3.Enabled = false;
            textBox1.Enabled = false;
            DecsOfReport = "Bill , for the period of " + DateFrom.ToString(Database.dformat) + " to " + DateTo.ToString(Database.dformat);
            if (Database.DatabaseType == "access")
            {
                sql = "SELECT ACCOUNT.Name AS Consigner, ACCOUNT_1.Name AS Consignee,  VOUCHERINFO.Vnumber as GRNo,  Sum(Voucherdet.Quantity) AS NoOfPack, item.name AS Goods, Sum(Voucherdet.weight) AS Weight, VOUCHERINFO.Totalamount AS Freight FROM ((((VOUCHERINFO LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id) LEFT JOIN Voucherdet ON VOUCHERINFO.Vi_id = Voucherdet.Vi_id) LEFT JOIN item ON Voucherdet.Des_ac_id = item.Id) LEFT JOIN ACCOUNT ON VOUCHERINFO.Ac_id = ACCOUNT.Ac_id) LEFT JOIN ACCOUNT AS ACCOUNT_1 ON VOUCHERINFO.Ac_id2 = ACCOUNT_1.Ac_id where ACCOUNT_1.Name= '" + name + "' and (((VOUCHERINFO.PaymentMode)='T.B.B.')) and VOUCHERINFO.Vdate>=" + access_sql.Hash + DateFrom.ToString(Database.dformat) + access_sql.Hash + " And (VOUCHERINFO.Vdate)<=" + access_sql.Hash + DateTo.ToString(Database.dformat) + access_sql.Hash + " GROUP BY ACCOUNT.Name, VOUCHERINFO.Vnumber, VOUCHERINFO.Totalamount, item.name, ACCOUNT_1.Name";
            }
            else
            {
                sql = "SELECT ACCOUNT.Name AS Consigner, ACCOUNT_1.Name AS Consignee,  VOUCHERINFO.Vnumber as GRNo,  Sum(Voucherdet.Quantity) AS NoOfPack, item.name AS Goods, Sum(Voucherdet.weight) AS Weight, VOUCHERINFO.Totalamount AS Freight FROM ((((VOUCHERINFO LEFT OUTER JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id) LEFT OUTER JOIN Voucherdet ON VOUCHERINFO.Vi_id = Voucherdet.Vi_id) LEFT OUTER JOIN item ON Voucherdet.Des_ac_id = item.Id) LEFT OUTER JOIN ACCOUNT ON VOUCHERINFO.Ac_id = ACCOUNT.Ac_id) LEFT OUTER JOIN ACCOUNT AS ACCOUNT_1 ON VOUCHERINFO.Ac_id2 = ACCOUNT_1.Ac_id where ACCOUNT_1.Name= '" + name + "' and (((VOUCHERINFO.PaymentMode)='T.B.B.')) and VOUCHERINFO.Vdate>=" + access_sql.Hash + DateFrom.ToString(Database.dformat) + access_sql.Hash + " And (VOUCHERINFO.Vdate)<=" + access_sql.Hash + DateTo.ToString(Database.dformat) + access_sql.Hash + " GROUP BY ACCOUNT.Name, VOUCHERINFO.Vnumber, VOUCHERINFO.Totalamount, item.name, ACCOUNT_1.Name";
            }
            dt.Clear();
            Database.GetSqlData(sql, dt);


            if (dt.Rows.Count == 0)
            {
                return false;
            }


            string[,] col = new string[0, 0]; // { { "Name", "1", "1" }, { "DocNo", "0", "0" } };

            string[,] Cwidth = new string[7, 6] { 
            { "Consigner", "125", "0","","","" },
            { "Consignee", "125", "0","","","" }, 
            { "GR No.", "150", "0","" ,"",""}, 
            { "No. Of Pack.", "150", "0","","","" },
            { "Goods", "150", "0","" ,"",""}, 
            { "Weight", "150", "0","","","" }, 
            { "Freight", "150", "0","","","" }};

            CreateReport(dt, col, Cwidth);

            return true;
        }



        private void CreateReport(DataTable dt, string[,] col, string[,] Cwidth)
        {
            double TotBrokerage = 0;
            double TotRunn = 0;
            dataGridView1.Columns.Clear();
            for (int i1 = 0; i1 < dt.Columns.Count; i1++)
            {
                if (i1 >= col.GetLength(0) || col[i1, 1] == "0")
                {
                    dataGridView1.Columns.Add(dt.Columns[i1].ColumnName, Cwidth[i1, 0]);
                    dataGridView1.Columns[dt.Columns[i1].ColumnName].Width = int.Parse(Cwidth[i1, 1]);
                    if (int.Parse(Cwidth[i1, 1]) == 0)
                    {
                        dataGridView1.Columns[dt.Columns[i1].ColumnName].Visible = false;

                    }
                    if (dt.Columns[i1].DataType.Name == "Decimal")
                    {
                        dataGridView1.Columns[dt.Columns[i1].ColumnName].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
                        dataGridView1.Columns[dt.Columns[i1].ColumnName].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                    }
                    if (dt.Columns[i1].DataType.Name == "Int32")
                    {
                        dataGridView1.Columns[dt.Columns[i1].ColumnName].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
                        dataGridView1.Columns[dt.Columns[i1].ColumnName].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                    }
                    if (dt.Columns[i1].DataType.Name == "Double")
                    {
                        dataGridView1.Columns[dt.Columns[i1].ColumnName].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
                        dataGridView1.Columns[dt.Columns[i1].ColumnName].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                    }
                }

            }

            dataGridView1.Rows.Clear();
            dataGridView1.Rows.Add();
            if (col.GetLength(0) > 0)
            {
                DataTable dtGp1 = dt.DefaultView.ToTable(true, col[0, 0]);
                for (int i1 = 0; i1 < dtGp1.Rows.Count; i1++)
                {
                    DataRow[] dr1 = dt.Select(col[0, 0] + "='" + dtGp1.Rows[i1][0] + "'");
                    if (col[0, 1] == "1")//Group one Header
                    {
                        if (dt.Columns[0].DataType.Name == "DateTime" && DateTime.Parse(dtGp1.Rows[i1][col[0, 0]].ToString()).ToString("yyyy") == "1801")
                        {
                            dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[0].Value = "";
                        }
                        else if (dt.Columns[0].DataType.Name == "DateTime")
                        {
                            dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[0].Value = DateTime.Parse(dtGp1.Rows[i1][col[0, 0]].ToString()).ToString("dd-MMM-yyyy");
                        }
                        else
                        {
                            dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[0].Value = dtGp1.Rows[i1][col[0, 0]].ToString();
                        }

                        dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[0].Style.Font = new System.Drawing.Font(dataGridView1.Font, FontStyle.Bold);
                        dataGridView1.Rows.Add();
                    }


                    if (col.GetLength(0) > 1)
                    {
                        DataTable dt2 = dr1.CopyToDataTable();
                        DataTable dtGp2 = dt2.DefaultView.ToTable(true, col[1, 0]);
                        for (int i2 = 0; i2 < dtGp2.Rows.Count; i2++)
                        {
                            DataRow[] dr2 = dt2.Select(col[1, 0] + "='" + dtGp2.Rows[i2][0] + "'");
                            if (col[1, 1] == "1") //Group Two Header
                            {
                                if (dt2.Columns[1].DataType.Name == "DateTime" && DateTime.Parse(dtGp2.Rows[i2][col[1, 0]].ToString()).ToString("yyyy") == "1801")
                                {
                                    dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[0].Value = "";
                                }
                                else if (dt2.Columns[1].DataType.Name == "DateTime")
                                {
                                    dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[0].Value = DateTime.Parse(dtGp2.Rows[i2][col[1, 0]].ToString()).ToString("dd-MMM-yyyy");
                                }
                                else
                                {
                                    dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[0].Value = dtGp2.Rows[i2][col[1, 0]].ToString();
                                }
                                dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[0].Style.Font = new System.Drawing.Font(dataGridView1.Font, FontStyle.Bold);
                                dataGridView1.Rows.Add();
                            }

                            //detail section if two group
                            for (int j2 = 0; j2 < dr2.Length; j2++)
                            {
                                for (int k2 = 0; k2 < dt2.Columns.Count; k2++)
                                {
                                    if (k2 >= col.GetLength(0) || col[k2, 1] == "0")
                                    {
                                        if (j2 != 0 && dr2[j2][k2].ToString() == dr2[j2 - 1][k2].ToString() && k2 < 2)
                                        {
                                            dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[dt2.Columns[k2].ColumnName].Value = "";
                                        }
                                        else if (dt.Columns[dt2.Columns[k2].ColumnName].DataType.Name == "DateTime" && DateTime.Parse(dr2[j2][k2].ToString()).ToString("yyyy") == "1801")
                                        {
                                            dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[dt2.Columns[k2].ColumnName].Value = "";
                                        }

                                        else if (dt.Columns[dt2.Columns[k2].ColumnName].DataType.Name == "DateTime")
                                        {
                                            dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[dt2.Columns[k2].ColumnName].Value = DateTime.Parse(dr2[j2][k2].ToString()).ToString("dd-MMM-yyyy");
                                        }
                                        else if (dt.Columns[dt2.Columns[k2].ColumnName].DataType.Name == "Decimal" && Cwidth[k2, 2] == "1" && double.Parse(dr2[j2][k2].ToString()) == 0)
                                        {
                                            dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[dt2.Columns[k2].ColumnName].Value = "";
                                        }
                                        else if (dt.Columns[dt2.Columns[k2].ColumnName].DataType.Name == "Decimal")
                                        {

                                            dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[dt2.Columns[k2].ColumnName].Value = funs.IndianCurr(double.Parse(dr2[j2][k2].ToString()));
                                        }
                                        else if (dt.Columns[dt2.Columns[k2].ColumnName].DataType.Name == "Int32" && Cwidth[k2, 2] == "1" && double.Parse(dr2[j2][k2].ToString()) == 0)
                                        {
                                            dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[dt2.Columns[k2].ColumnName].Value = "";
                                        }
                                        else if (dt.Columns[dt2.Columns[k2].ColumnName].DataType.Name == "Double" && Cwidth[k2, 2] == "1" && double.Parse(dr2[j2][k2].ToString()) == 0)
                                        {
                                            dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[dt2.Columns[k2].ColumnName].Value = "";
                                        }
                                        else if (dt.Columns[dt2.Columns[k2].ColumnName].DataType.Name == "Double")
                                        {

                                            dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[dt2.Columns[k2].ColumnName].Value = funs.IndianCurr(double.Parse(dr2[j2][k2].ToString()));
                                        }

                                        else
                                        {
                                            dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[dt2.Columns[k2].ColumnName].Value = dr2[j2][k2].ToString();
                                        }

                                    }
                                }
                                dataGridView1.Rows.Add();

                            }
                            if (col[1, 2] == "1") //Group two Footer
                            {
                                DataTable dtSum2 = dr2.CopyToDataTable();
                                for (int k2 = 0; k2 < dtSum2.Columns.Count; k2++)
                                {
                                    if (Cwidth[k2, 5] == "")
                                    {

                                    }
                                    else if (Cwidth[k2, 5].ToString().Substring(0, 1) == "|")
                                    {
                                        dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[dtSum2.Columns[k2].ColumnName].Value = funs.IndianCurr(double.Parse(dtSum2.Compute(Cwidth[k2, 5].ToString().TrimStart('|'), "").ToString()));
                                        dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[dtSum2.Columns[k2].ColumnName].Style.Font = new System.Drawing.Font(dataGridView1.Font, FontStyle.Bold);
                                    }
                                    else if (Cwidth[k2, 5].ToString().Substring(0, 1) == "+")
                                    {
                                        double val = double.Parse(dtSum2.Compute(Cwidth[k2, 5].ToString().TrimStart('+'), "").ToString());
                                        if (val > 0)
                                        {
                                            dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[dtSum2.Columns[k2].ColumnName].Value = funs.IndianCurr(val);
                                        }
                                        else
                                        {
                                            dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[dtSum2.Columns[k2].ColumnName].Value = funs.IndianCurr(0);
                                        }
                                        dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[dtSum2.Columns[k2].ColumnName].Style.Font = new System.Drawing.Font(dataGridView1.Font, FontStyle.Bold);
                                    }
                                    else if (Cwidth[k2, 5].ToString().Substring(0, 1) == ">")
                                    {

                                        double val = double.Parse(dtSum2.Compute(Cwidth[k2, 5].ToString().Split('>')[2], "").ToString());
                                        if (val <= 0 || val > double.Parse(Cwidth[k2, 5].ToString().Split('>')[1]))
                                        {
                                            dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[dtSum2.Columns[k2].ColumnName].Value = funs.IndianCurr(val);
                                            TotBrokerage += val;
                                        }
                                        else
                                        {
                                            dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[dtSum2.Columns[k2].ColumnName].Value = funs.IndianCurr(0);
                                        }
                                        dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[dtSum2.Columns[k2].ColumnName].Style.Font = new System.Drawing.Font(dataGridView1.Font, FontStyle.Bold);

                                    }
                                    else if (Cwidth[k2, 5].ToString().Substring(0, 1) == "^")
                                    {
                                        double val = double.Parse(dtSum2.Compute(Cwidth[k2, 5].ToString().TrimStart('^'), "").ToString());
                                        TotRunn = TotRunn + val;
                                        if (TotRunn > 0)
                                        {
                                            dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[dtSum2.Columns[k2].ColumnName].Value = funs.IndianCurr(TotRunn);
                                            dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells["Dr/Cr"].Value = "Dr.";
                                        }
                                        else
                                        {
                                            dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[dtSum2.Columns[k2].ColumnName].Value = funs.IndianCurr(-1 * TotRunn);
                                            dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells["Dr/Cr"].Value = "Cr.";
                                        }
                                        dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[dtSum2.Columns[k2].ColumnName].Style.Font = new System.Drawing.Font(dataGridView1.Font, FontStyle.Bold);
                                        dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells["Dr/Cr"].Style.Font = new System.Drawing.Font(dataGridView1.Font, FontStyle.Bold);
                                    }
                                    else
                                    {
                                        dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[dtSum2.Columns[k2].ColumnName].Value = Cwidth[k2, 5].ToString();
                                        dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[dtSum2.Columns[k2].ColumnName].Style.Font = new System.Drawing.Font(dataGridView1.Font, FontStyle.Bold);


                                    }
                                }

                                dataGridView1.Rows.Add();
                            }
                        }
                    }

                    else //detail section if only one group
                    {
                        for (int j1 = 0; j1 < dr1.Length; j1++)
                        {
                            for (int k1 = 0; k1 < dt.Columns.Count; k1++)
                            {
                                if (k1 >= col.GetLength(0) || col[k1, 0] == "0")
                                {

                                    if (j1 != 0 && dr1[j1][k1].ToString() == dr1[j1 - 1][k1].ToString() && k1 < 1)
                                    {
                                        dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[dt.Columns[k1].ColumnName].Value = "";
                                    }

                                    else if (dt.Columns[dt.Columns[k1].ColumnName].DataType.Name == "DateTime" && DateTime.Parse(dr1[j1][k1].ToString()).ToString("yyyy") == "1801")
                                    {
                                        dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[dt.Columns[k1].ColumnName].Value = "";
                                    }
                                    else if (dt.Columns[dt.Columns[k1].ColumnName].DataType.Name == "DateTime")
                                    {
                                        dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[dt.Columns[k1].ColumnName].Value = DateTime.Parse(dr1[j1][k1].ToString()).ToString("dd-MMM-yyyy");
                                    }
                                    else if (dt.Columns[dt.Columns[k1].ColumnName].DataType.Name == "Decimal" && Cwidth[k1, 2] == "1" && double.Parse(dr1[j1][k1].ToString()) == 0)
                                    {

                                        dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[dt.Columns[k1].ColumnName].Value = "";
                                    }
                                    else if (dt.Columns[dt.Columns[k1].ColumnName].DataType.Name == "Decimal")
                                    {
                                        if (dr1[j1][k1].ToString() != "")
                                        {

                                            dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[dt.Columns[k1].ColumnName].Value = funs.IndianCurr(double.Parse(dr1[j1][k1].ToString()));
                                        }
                                    }
                                    else if (dt.Columns[dt.Columns[k1].ColumnName].DataType.Name == "Int32" && Cwidth[k1, 2] == "1" && double.Parse(dr1[j1][k1].ToString()) == 0)
                                    {

                                        dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[dt.Columns[k1].ColumnName].Value = "";
                                    }
                                    else if (dt.Columns[dt.Columns[k1].ColumnName].DataType.Name == "Int32")
                                    {

                                        dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[dt.Columns[k1].ColumnName].Value = funs.IndianCurr(double.Parse(dr1[j1][k1].ToString()));
                                    }
                                    else if (dt.Columns[dt.Columns[k1].ColumnName].DataType.Name == "Double" && Cwidth[k1, 2] == "1" && double.Parse(dr1[j1][k1].ToString()) == 0)
                                    {

                                        dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[dt.Columns[k1].ColumnName].Value = "";
                                    }
                                    else if (dt.Columns[dt.Columns[k1].ColumnName].DataType.Name == "Double")
                                    {

                                        dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[dt.Columns[k1].ColumnName].Value = funs.IndianCurr(double.Parse(dr1[j1][k1].ToString()));
                                    }
                                    else
                                    {
                                        dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[dt.Columns[k1].ColumnName].Value = dr1[j1][k1].ToString();
                                    }

                                }

                            }
                            dataGridView1.Rows.Add();

                        }
                    }

                    if (col[0, 2] == "1")//Group one Footer
                    {
                        DataTable dtSum1 = dr1.CopyToDataTable();
                        for (int k1 = 0; k1 < dtSum1.Columns.Count; k1++)
                        {
                            if (Cwidth[k1, 4] == "")
                            {

                            }
                            else if (Cwidth[k1, 4].ToString().Substring(0, 1) == "|")
                            {
                                dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[dtSum1.Columns[k1].ColumnName].Value = funs.IndianCurr(double.Parse(dtSum1.Compute(Cwidth[k1, 4].ToString().TrimStart('|'), "").ToString()));
                                dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[dtSum1.Columns[k1].ColumnName].Style.Font = new System.Drawing.Font(dataGridView1.Font, FontStyle.Bold);
                            }
                            else if (Cwidth[k1, 4].ToString().Substring(0, 1) == "+")
                            {
                                double val = double.Parse(dtSum1.Compute(Cwidth[k1, 4].ToString().TrimStart('+'), "").ToString());
                                if (val > 0)
                                {
                                    dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[dtSum1.Columns[k1].ColumnName].Value = funs.IndianCurr(val);
                                }
                                else
                                {
                                    dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[dtSum1.Columns[k1].ColumnName].Value = funs.IndianCurr(0);
                                }
                                dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[dtSum1.Columns[k1].ColumnName].Style.Font = new System.Drawing.Font(dataGridView1.Font, FontStyle.Bold);
                            }
                            else
                            {
                                dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[dtSum1.Columns[k1].ColumnName].Value = Cwidth[k1, 4].ToString();
                                dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[dtSum1.Columns[k1].ColumnName].Style.Font = new System.Drawing.Font(dataGridView1.Font, FontStyle.Bold);
                            }

                        }

                        dataGridView1.Rows.Add();

                    }

                }

            }

            else //detail section if no group valable
            {

                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (i != 0 && dt.Rows[i][j].ToString() == dt.Rows[i - 1][j].ToString() && j < 1)
                        {
                            dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[dt.Columns[j].ColumnName].Value = "";
                        }
                        else if (dt.Columns[dt.Columns[j].ColumnName].DataType.Name == "DateTime")
                        {
                            dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[dt.Columns[j].ColumnName].Value = DateTime.Parse(dt.Rows[i][j].ToString()).ToString("dd-MMM-yyyy").Replace("01-Feb-1801", "");
                        }
                        else if (dt.Columns[dt.Columns[j].ColumnName].DataType.Name == "Decimal" && Cwidth[j, 2] == "1" && double.Parse(dt.Rows[i][j].ToString()) == 0)
                        {
                            dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[dt.Columns[j].ColumnName].Value = "";
                        }
                        else if (dt.Columns[dt.Columns[j].ColumnName].DataType.Name == "Decimal")
                        {
                            if (dt.Rows[i][j].ToString() == "")
                            {
                                dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[dt.Columns[j].ColumnName].Value = "";
                            }
                            else
                            {
                                dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[dt.Columns[j].ColumnName].Value = funs.IndianCurr(double.Parse(dt.Rows[i][j].ToString()));
                            }

                        }
                        else if (dt.Columns[dt.Columns[j].ColumnName].DataType.Name == "Int32" && Cwidth[j, 2] == "1" && double.Parse(dt.Rows[i][j].ToString()) == 0)
                        {

                            dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[dt.Columns[j].ColumnName].Value = "";
                        }
                        else if (dt.Columns[dt.Columns[j].ColumnName].DataType.Name == "Int32")
                        {

                            dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[dt.Columns[j].ColumnName].Value = funs.IndianCurr(double.Parse(dt.Rows[i][j].ToString()));

                        }
                        else if (dt.Columns[dt.Columns[j].ColumnName].DataType.Name == "Double" && Cwidth[j, 2] == "1" && double.Parse(dt.Rows[i][j].ToString()) == 0)
                        {

                            dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[dt.Columns[j].ColumnName].Value = "";
                        }

                        else if (dt.Columns[dt.Columns[j].ColumnName].DataType.Name == "Double")
                        {
                            dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[dt.Columns[j].ColumnName].Value = funs.IndianCurr(double.Parse(dt.Rows[i][j].ToString()));

                        }
                        else if (dt.Rows[i][j].ToString().IndexOf("<b>") > -1)
                        {
                            dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[dt.Columns[j].ColumnName].Value = dt.Rows[i][j].ToString().Replace("<b>", "");
                            dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[dt.Columns[j].ColumnName].Style.Font = new System.Drawing.Font(dataGridView1.Font, FontStyle.Bold);
                        }
                        else
                        {
                            dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[dt.Columns[j].ColumnName].Value = dt.Rows[i][j].ToString();
                        }

                    }
                    dataGridView1.Rows.Add();
                }

            }


            for (int k = 0; k < dt.Columns.Count; k++)
            {

                if (Cwidth[k, 3] == "")
                {
                }
                else if (Cwidth[k, 3].ToString().Substring(0, 1) == "|")
                {
                    dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[dt.Columns[k].ColumnName].Value = funs.IndianCurr(double.Parse(dt.Compute(Cwidth[k, 3].ToString().TrimStart('|'), "").ToString()));
                    dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[dt.Columns[k].ColumnName].Style.Font = new System.Drawing.Font(dataGridView1.Font, FontStyle.Bold);
                }
                else if (Cwidth[k, 3].ToString().Substring(0, 1) == "+")
                {
                    double val = double.Parse(dt.Compute(Cwidth[k, 3].ToString().TrimStart('|'), "").ToString());
                    if (val > 0)
                    {
                        dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[dt.Columns[k].ColumnName].Value = funs.IndianCurr(val);
                    }
                    else
                    {
                        dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[dt.Columns[k].ColumnName].Value = funs.IndianCurr(0);
                    }
                    dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[dt.Columns[k].ColumnName].Style.Font = new System.Drawing.Font(dataGridView1.Font, FontStyle.Bold);
                }
                else if (Cwidth[k, 3].ToString().Substring(0, 1) == ">")
                {
                    dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[dt.Columns[k].ColumnName].Value = funs.IndianCurr(TotBrokerage);
                    dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[dt.Columns[k].ColumnName].Style.Font = new System.Drawing.Font(dataGridView1.Font, FontStyle.Bold);
                }
                else
                {
                    dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[dt.Columns[k].ColumnName].Value = Cwidth[k, 3].ToString();
                    dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[dt.Columns[k].ColumnName].Style.Font = new System.Drawing.Font(dataGridView1.Font, FontStyle.Bold);


                }

            }



        }

        private void Report_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
                this.Dispose();
            }
            else if (e.KeyCode == Keys.P)
            {
                if (dataGridView1.Rows.Count == 0)
                {
                    return;
                }
                string tPath = Path.GetTempPath() + DateTime.Now.ToString("yyMMddhmmssfff") + ".pdf";
                ExportToPdf(tPath);
                GC.Collect();
                PdfReader frm = new PdfReader();
                frm.LoadFile(tPath);
                frm.Visible = false;
                frm.axAcroPDF1.printWithDialog();

            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex < 0)
            {
                return;
            }

            
            String clkStr = "";
            

           
            if (dataGridView1.CurrentCell.Value != null)
            {
                clkStr = dataGridView1.CurrentCell.Value.ToString();
            }

            if (funs.Select_ac_id(clkStr) != "")
            {
                Report gg = new Report();
                gg.Ledger(stdt, endt, clkStr);
                //gg.MdiParent = this;
                //gg.MdiParent = this.MdiParent;
              //    `a  q1  gg.Show();
                gg.ShowDialog();
            }
            else  if (funs.Select_Refineact_id(clkStr) != 0)
            {
                Report gg = new Report();
                gg.SingleGroupedTrial(stdt, endt, clkStr);
                gg.MdiParent = this.MdiParent;
                //gg.MdiParent = this.MdiParent;
                gg.Show();
                //gg.ShowDialog();

            }
            //else   if (frmptyp == "Challan Register" )
            //{
            //    Report gg = new Report();


            //    gg.BookingRegisterNew(Database.stDate, Database.enDate, "  VOUCHERINFOs.Vi_id = '" + dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells["vid"].Value.ToString() + "'", "");
            //    gg.ShowDialog();
            //}
            // else   if ( frmptyp == "Stock Transfer Register")
            //{
            //    Report gg = new Report();


            //    gg.BookingRegisterNew(Database.stDate, Database.enDate, "  VOUCHERINFOs.Vi_id = '" + dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells["vid"].Value.ToString() + "'", "");
            //    gg.ShowDialog();
            //}
            //else if (IsDocumentNumber(clkStr) != "0")
            //{
            //    funs.OpenFrm(this, IsDocumentNumber(clkStr), false);
            //}
            //else if (dataGridView1.Columns.Contains("Did"))
            //{
            //    if (dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells["Did"].Value != null)
            //    {
            //        int desid = (int)double.Parse(dataGridView1.Rows[e.RowIndex].Cells["Did"].Value.ToString());

            //        Report gg = new Report();
            //        gg.ItemLedger(stdt, endt, gGodownName, desid);
            //        gg.ShowDialog();
            //    }
            //}

            if (dataGridView1.Columns.Contains("Vi_id"))
            {
                if (dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells["Vi_id"].Value != null)
                {
                    string[] vid = dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells["Vi_id"].Value.ToString().Split('.');
                    string vi_id = vid[0];
                    if (vi_id != "0" || vi_id!="")
                    {
                        funs.OpenFrm(this, vi_id, false);
                    }
                }

                //frmBooking frm = new frmBooking();
                //frm.LoadData(dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells["Vid"].Value.ToString(), "Booking");
                //frm.MdiParent = this.MdiParent;
                //frm.Show();

            }
            
           
        }

        private string IsDocumentNumber(String str)
        {
            //  return Database.GetScalarInt("SELECT distinct JOURNAL.Vi_id, " + access_sql.Docnumber + " AS DocNumber FROM JOURNAL, ACCOUNT, VOUCHERINFO, VOUCHERTYPE WHERE (((JOURNAL.Ac_id)=[ACCOUNT].[Ac_id]) AND ((JOURNAL.Vi_id)=[VOUCHERINFO].[Vi_id]) AND ((VOUCHERINFO.Vt_id)=[VOUCHERTYPE].[Vt_id])) and " + access_sql.Docnumber + "='" + str + "'");
            return Database.GetScalarText("SELECT DISTINCT VOUCHERINFOs.Vi_id, " + access_sql.Docnumber + " AS DocNumber FROM (VOUCHERINFOs LEFT JOIN ACCOUNTs ON VOUCHERINFOs.Ac_id = ACCOUNTs.Ac_id) LEFT JOIN VOUCHERTYPEs ON VOUCHERINFOs.Vt_id = VOUCHERTYPEs.Vt_id WHERE (((VOUCHERINFOs.Vt_id)=[VOUCHERTYPEs].[Vt_id]) AND (" + access_sql.Docnumber + "='" + str + "'))");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)
            {
                return;
            }
            string tPath = Path.GetTempPath() + DateTime.Now.ToString("yyMMddhmmssfff") + ".pdf";
            ExportToPdf(tPath);
            GC.Collect();
            PdfReader frm = new PdfReader();
            frm.LoadFile(tPath);
            frm.Visible = false;
            frm.axAcroPDF1.printWithDialog();

        }
       
        public void ExportToPdf1(string tPath)
        {
            frmptyp2 = frmptyp;
            DecsOfReport2 = DecsOfReport;
            str2 = str;
            dataGridView2 = dataGridView1;


            FileStream fs = new FileStream(tPath, FileMode.Create, FileAccess.Write, FileShare.None);
            iTextSharp.text.Rectangle rec;
            Document document;
            int Twidth = 0;
            for (int i = 0; i < dataGridView1.Columns.Count; i++)
            {
                Twidth += dataGridView1.Columns[i].Width;
            }
            if (Twidth == 2000)
            {
                document = new Document(PageSize.A4.Rotate(), 20f, 10f, 20f, 10f);
            }
            else if (GetPapersize().ToUpper() == "A4")
            {
                document = new Document(PageSize.A4, 20f, 10f, 20f, 10f);
            }
            else if (GetPapersize().ToUpper() == "A5")
            {
                document = new Document(PageSize.A5, 20f, 10f, 20f, 10f);

            }
            else
            {
                document = new Document(PageSize.A4, 20f, 10f, 20f, 10f);
            }

            Pagesize = GetPapersize();
            PdfWriter writer = PdfWriter.GetInstance(document, fs);
            writer.PageEvent = new MainTextEventsHandler();
            document.Open();
            HTMLWorker hw = new HTMLWorker(document);
            str = "";
            str += @"<body> <font size='1'><table border=1> <tr>";
            for (int i = 0; i < dataGridView1.Columns.Count; i++)
            {
                string align = "";
                string bold = "";
                int width = 0;

                if (Twidth == 2000)
                {
                    width = dataGridView1.Columns[i].Width / 20;
                }
                else
                {
                    width = dataGridView1.Columns[i].Width / 10;
                }

                if (dataGridView1.Columns[i].HeaderCell.Style.Alignment == DataGridViewContentAlignment.MiddleRight)
                {
                    align = "text-align:right;";
                }

                bold = "font-weight: bold;";

                if (width != 0)
                {
                    str += "<th width=" + width + "%  style='" + align + bold + "'>" + dataGridView1.Columns[i].HeaderText.ToString() + "</th> ";
                }

            }

            str += "</tr>";

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                str += "<tr> ";
                for (int j = 0; j < dataGridView1.Columns.Count; j++)
                {
                    int width = 0;
                    if (Twidth == 2000)
                    {
                        width = dataGridView1.Rows[i].Cells[j].Size.Width / 20;

                    }
                    else
                    {
                        width = dataGridView1.Rows[i].Cells[j].Size.Width / 10;
                    }

                    if (width != 0)
                    {

                        if (dataGridView1.Rows[i].Cells[j].Value != null)
                        {
                            string align = "";
                            string bold = "";
                            string colspan = "";

                            if (dataGridView1.Columns[j].DefaultCellStyle.Alignment == DataGridViewContentAlignment.MiddleRight)
                            {
                                align = "text-align:right;";
                            }

                            if (dataGridView1.Rows[i].Cells[j].Style.Font != null && dataGridView1.Rows[i].Cells[j].Style.Font.Bold == true)
                            {
                                bold = "font-weight: bold;";
                            }

                            if (j == 0 && dataGridView1.Rows[i].Cells[0].Value.ToString() != "" && dataGridView1.Rows[i].Cells[1].Value == null && dataGridView1.Rows[i].Cells[2].Value == null)
                            {
                                colspan = "colspan= '2'";
                            }


                            if (dataGridView1.Rows[i].Cells[j].Value.ToString().Trim() == "")
                            {
                                str += "<td> &nbsp; </td>";
                            }
                            else
                            {
                                str += "<td " + colspan + "  style='" + align + bold + "'>" + dataGridView1.Rows[i].Cells[j].Value.ToString() + "</td> ";
                            }

                            if (j == 0 && dataGridView1.Rows[i].Cells[0].Value.ToString() != "" && dataGridView1.Rows[i].Cells[1].Value == null && dataGridView1.Rows[i].Cells[2].Value == null)
                            {
                                j++;
                            }


                        }
                        else
                        {


                            str += "<td> &nbsp; </td>";

                        }
                    }
                }
                str += "</tr> ";
            }
            str += "</table></font></body>";

            StringReader sr = new StringReader(str);
            hw.Parse(sr);
            document.Close();

        }
        public void ExportToPdf(string tPath)
        {

            frmptyp2 = frmptyp;
            DecsOfReport2 = DecsOfReport;
            //DecsOfReportFooter2 = DecsOfReportFooter;
            str2 = str;
            dataGridView2 = dataGridView1;


            FileStream fs = new FileStream(tPath, FileMode.Create, FileAccess.Write, FileShare.None);
            iTextSharp.text.Rectangle rec;
            Document document = new Document();
            int Twidth = 0;
            for (int i = 0; i < dataGridView1.Columns.Count; i++)
            {
                Twidth += dataGridView1.Columns[i].Width;
            }
            if (Twidth > 1900)
            {
                document = new Document(PageSize.A4.Rotate(), 5f, 5f, 5f, 5f);
            }
            else
            {
                document = new Document(PageSize.A4, 20f, 10f, 20f, 10f);
            }
            PdfWriter writer = PdfWriter.GetInstance(document, fs);
            writer.PageEvent = new MainTextEventsHandler();

            var normalFont = FontFactory.GetFont(FontFactory.HELVETICA, 9);
            var boldFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10);
            PdfPTable table = new PdfPTable(dataGridView1.Columns.Count);

            document.Open();
            float[] widths = new float[dataGridView1.Columns.Count];
            for (int i = 0; i < dataGridView1.Columns.Count; i++)
            {
                if (Twidth > 1900)
                {
                    widths[i] += (dataGridView1.Rows[0].Cells[i].Size.Width / 10) / 2;
                }
                else
                {
                    widths[i] += dataGridView1.Rows[0].Cells[i].Size.Width / 10;
                }
            }

            table.SetWidthPercentage(widths, new iTextSharp.text.Rectangle(0f, 0f, 100f, 300f));
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                for (int j = 0; j < dataGridView1.Columns.Count; j++)
                {
                    string val;
                    if (dataGridView1.Rows[i].Cells[j].Value != null)
                    {
                        val = dataGridView1.Rows[i].Cells[j].Value.ToString();
                    }
                    else
                    {
                        val = "";
                    }


                    PdfPCell cell;
                    if (dataGridView1.Rows[i].Cells[j].Style.Font != null && dataGridView1.Rows[i].Cells[j].Style.Font.Bold == true)
                    {
                        cell = new PdfPCell(new Phrase(val, boldFont));

                    }
                    else
                    {
                        cell = new PdfPCell(new Phrase(val, normalFont));
                    }

                    if (dataGridView1.Columns[j].DefaultCellStyle.Alignment == DataGridViewContentAlignment.MiddleRight)
                    {
                        cell.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                    }

                    cell.MinimumHeight = 20f;
                    table.AddCell(cell);
                }

            }

            document.Add(table);

            //PdfPTable table2 = new PdfPTable(2);

            //PdfPCell cell2 = new PdfPCell(new Phrase(DecsOfReportFooter));
            //cell2.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //cell2.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
            //table2.AddCell(cell2);
            //float[] columnWidths = { 65, 35 };

            //table2.SetWidthPercentage(columnWidths, new iTextSharp.text.Rectangle(0f, 0f, 100f, 300f));

            //document.Add(table2);

            //PdfPCell cell3 = new PdfPCell(new Phrase(closingbal2));
            //cell3.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //cell3.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
            //table2.AddCell(cell3);

            //document.Add(table2);
            document.Close();

        }
      
        internal class MainTextEventsHandler : PdfPageEventHelper
        {
            public override void OnStartPage(PdfWriter writer, Document document)
            {
                base.OnStartPage(writer, document);

                bool sta = Database.GetScalarBool("select Stationary from Vouchertypes where Name='" + Report.frmptyp2 + "' ");
                DataTable dtRheader = new DataTable();
                //Database.GetSqlData("select * from company", dtRheader);
                Database.GetSqlData("select * from location where LocationId='" + Database.LocationId + "'", dtRheader);
                PdfPTable table = new PdfPTable(1);
                PdfPCell cell = new PdfPCell();

                if (sta == false)
                {
                    cell.Phrase = new Phrase(dtRheader.Rows[0]["name"].ToString());
                    cell.BorderWidth = 0f;
                    cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    table.AddCell(cell);
                    cell.Phrase = new Phrase(dtRheader.Rows[0]["Address1"].ToString());
                    table.AddCell(cell);
                    cell.Phrase = new Phrase(dtRheader.Rows[0]["Address2"].ToString());
                    table.AddCell(cell);
                    cell.Phrase = new Phrase(Report.DecsOfReport2);
                    table.AddCell(cell);
                    cell.Phrase = new Phrase("\n");
                    table.AddCell(cell);
                }
                else
                {
                    cell.Phrase = new Phrase("\n");
                    cell.BorderWidth = 0f;
                    cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    table.AddCell(cell);
                    cell.Phrase = new Phrase("\n");
                    table.AddCell(cell);
                    cell.Phrase = new Phrase("\n");
                    table.AddCell(cell);
                    cell.Phrase = new Phrase(Report.DecsOfReport2);
                    table.AddCell(cell);
                    cell.Phrase = new Phrase("\n");
                    table.AddCell(cell);
                }
                document.Add(table);


                BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_BOLD, BaseFont.CP1252, false);
                iTextSharp.text.Font TitalFount = new iTextSharp.text.Font(bfTimes, 9, iTextSharp.text.Font.ITALIC, iTextSharp.text.BaseColor.BLACK);
                iTextSharp.text.Font TableFount = new iTextSharp.text.Font(bfTimes, 8, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK);

                PdfPTable table1 = new PdfPTable(dataGridView2.Columns.Count);
                int Twidth = 0;
                for (int i = 0; i < dataGridView2.Columns.Count; i++)
                {
                    Twidth += dataGridView2.Columns[i].Width;
                }

                float[] widths = new float[dataGridView2.Columns.Count];
                for (int i = 0; i < dataGridView2.Columns.Count; i++)
                {
                    if (Twidth == 2000)
                    {
                        widths[i] += (dataGridView2.Rows[0].Cells[i].Size.Width / 10) / 2;
                    }
                    else
                    {
                        widths[i] += dataGridView2.Rows[0].Cells[i].Size.Width / 10;
                    }
                }
                table1.SetWidthPercentage(widths, new iTextSharp.text.Rectangle(0f, 0f, 100f, 300f));



                for (int i = 0; i < dataGridView2.Columns.Count; i++)
                {
                    cell = new PdfPCell(new Phrase(dataGridView2.Columns[i].HeaderText, TableFount));
                    if (dataGridView2.Columns[i].DefaultCellStyle.Alignment == DataGridViewContentAlignment.MiddleRight)
                    {
                        cell.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                    }
                    else
                    {
                        cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    }
                    cell.MinimumHeight = 15f;
                    table1.AddCell(cell);
                }


                document.Add(table1);










            }


            public override void OnEndPage(PdfWriter writer, Document document)
            {

                base.OnEndPage(writer, document);
                string text = "";
                text += "Page No-" + document.PageNumber;
                PdfContentByte cb = writer.DirectContent;
                cb.BeginText();
                BaseFont bf = BaseFont.CreateFont();
                cb.SetFontAndSize(bf, 8);
                if (Pagesize == "A4")
                {
                    cb.SetTextMatrix(530, 8);
                }
                else if (Pagesize == "A5")
                {
                    cb.SetTextMatrix(350, 8);
                }

                cb.ShowText(text);
                cb.EndText();




            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)
            {
                return;
            }

            string tPath = Path.GetTempPath() + DateTime.Now.ToString("yyMMddhmmssfff") + ".pdf";
            ExportToPdf(tPath);
            GC.Collect();
            PdfReader frm = new PdfReader();
            frm.LoadFile(tPath);
            frm.Show();

        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)
            {
                return;
            }

            SaveFileDialog ofd = new SaveFileDialog();
            ofd.Filter = "Adobe Acrobat(*.pdf) | *.pdf";

            if (DialogResult.OK == ofd.ShowDialog())
            {
                ExportToPdf(ofd.FileName);
                MessageBox.Show("Export Successfully!!");
            }

        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)
            {
                return;
            }
            Object misValue = System.Reflection.Missing.Value;
            Excel.Application apl = new Microsoft.Office.Interop.Excel.Application();
            Excel.Workbook wb = (Excel.Workbook)apl.Workbooks.Add(misValue);
            Excel.Worksheet ws;
            ws = (Excel.Worksheet)wb.Worksheets[1];

            int lno = 1;
            DataTable dtExcel = new DataTable();

            DataTable dtRheader = new DataTable();
            Database.GetSqlData("select * from location where LocationId='" + Database.LocationId + "'", dtRheader);
            //Database.GetSqlData("select * from company", dtRheader);

            ws.Cells[lno, 1] = dtRheader.Rows[0]["name"].ToString();
            ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, dataGridView1.Columns.Count]).Merge(Type.Missing);
            ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, dataGridView1.Columns.Count]).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, dataGridView1.Columns.Count]).Font.Bold = true;
            lno++;

            ws.Cells[lno, 1] = dtRheader.Rows[0]["Address1"].ToString();
            ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, dataGridView1.Columns.Count]).Merge(Type.Missing);
            ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, dataGridView1.Columns.Count]).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, dataGridView1.Columns.Count]).Font.Bold = true;
            lno++;

            ws.Cells[lno, 1] = dtRheader.Rows[0]["Address2"].ToString();
            ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, dataGridView1.Columns.Count]).Merge(Type.Missing);
            ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, dataGridView1.Columns.Count]).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, dataGridView1.Columns.Count]).Font.Bold = true;
            lno++;



            for (int i = 0; i < dataGridView1.Columns.Count; i++)
            {
                if (dataGridView1.Columns[i].HeaderCell.Style.Alignment == DataGridViewContentAlignment.MiddleRight)
                {
                    ws.get_Range(ws.Cells[5, i + 1], ws.Cells[5, i + 1]).HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                }
                ws.get_Range(ws.Cells[i + 1, i + 1], ws.Cells[i + 1, i + 1]).ColumnWidth = dataGridView1.Columns[i].Width / 11.5;
                ws.Cells[5, i + 1] = dataGridView1.Columns[i].HeaderText.ToString();

            }

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                for (int j = 0; j < dataGridView1.Columns.Count; j++)
                {
                    if (dataGridView1.Columns[j].HeaderCell.Style.Alignment == DataGridViewContentAlignment.MiddleRight)
                    {
                        ws.get_Range(ws.Cells[i + 6, j + 1], ws.Cells[i + 6, j + 1]).HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                        ws.get_Range(ws.Cells[i + 6, j + 1], ws.Cells[i + 6, j + 1]).NumberFormat = "0,0.00";

                    }
                    else
                    {
                        ws.get_Range(ws.Cells[i + 6, j + 1], ws.Cells[i + 6, j + 1]).HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                    }

                    if (dataGridView1.Columns[j].DefaultCellStyle.Font != null)
                    {
                        ws.get_Range(ws.Cells[i + 6, j + 1], ws.Cells[i + 6, j + 1]).Font.Bold = true;

                    }

                    if (dataGridView1.Rows[i].Cells[j].Value != null)
                    {
                        ws.Cells[i + 6, j + 1] = dataGridView1.Rows[i].Cells[j].Value.ToString().Replace(",", "");
                    }
                }
            }

            Excel.Range last = ws.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell, Type.Missing);
            ws.get_Range("A1", last).WrapText = true;
            apl.Visible = true;
        }

        private void Report_FormClosing(object sender, FormClosingEventArgs e)
        {

            string[] files = Directory.GetFiles(Path.GetTempPath());
            foreach (string file in files)
            {

                try
                {
                    File.Delete(file);
                }
                catch
                {

                }
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            //            if (frmptyp == "Ledger")
            //            {
            //                strCombo = funs.GetStrCombo("*");
            //                textBox1.Text = SelectCombo.ComboKeydown(this, e.KeyCode, strCombo, textBox1.Text, 1);
            //            }
            //            else if (frmptyp == "Deatil Ledger")
            //            {

            //                strCombo = funs.GetStrCombo("*");
            //                textBox1.Text = SelectCombo.ComboKeydown(this, e.KeyCode, strCombo, textBox1.Text, 1);
            //            }
            //            else if (frmptyp == "Customer Detail Bill Wise")
            //            {
            //                strCombo = funs.GetStrCombo("*");

            //                textBox1.Text = SelectCombo.ComboKeydown(this, e.KeyCode, strCombo, textBox1.Text, 1);
            //            }
            //            else if (frmptyp == "Customer Detail Item Wise")
            //            {
            //                strCombo = funs.GetStrCombo("*");

            //                textBox1.Text = SelectCombo.ComboKeydown(this, e.KeyCode, strCombo, textBox1.Text, 1);
            //            }
            //            else if (frmptyp == "Cash Report")
            //            {
            //                strCombo = "SELECT VOUCHERINFO.Formno FROM VOUCHERTYPE LEFT JOIN ((VOUCHERINFO LEFT JOIN ACCOUNT ON VOUCHERINFO.Ac_id = ACCOUNT.Ac_id) LEFT JOIN ACCOUNTYPE ON ACCOUNT.Act_id = ACCOUNTYPE.Act_id) ON VOUCHERTYPE.Vt_id = VOUCHERINFO.Vt_id GROUP BY VOUCHERINFO.Formno, ACCOUNTYPE.Act_id, VOUCHERTYPE.Vt_id HAVING (((VOUCHERINFO.Formno)<>'') AND ((ACCOUNTYPE.Act_id)=3) AND ((VOUCHERTYPE.Vt_id)=15 Or (VOUCHERTYPE.Vt_id)=3))";

            //                textBox1.Text = SelectCombo.ComboKeydown(this, e.KeyCode, strCombo, textBox1.Text, 1);
            //            }
            //            else if (frmptyp == "Customer Brokerage")
            //            {
            //                strCombo = funs.GetStrCombo("*");

            //                textBox1.Text = SelectCombo.ComboKeydown(this, e.KeyCode, strCombo, textBox1.Text, 1);
            //            }

            //            else if (frmptyp == "Customer Pendings")
            //            {
            //                strCombo = funs.GetStrCombo("*");
            //                textBox1.Text = SelectCombo.ComboKeydown(this, e.KeyCode, strCombo, textBox1.Text, 1);
            //            }

            //            else if (frmptyp == "Broker Detail Customer Wise")
            //            {
            //                strCombo = "SELECT name from Contractor order by name";
            //                textBox1.Text = SelectCombo.ComboKeydown(this, e.KeyCode, strCombo, textBox1.Text, 1);
            //            }

            //            else if (frmptyp == "Broker Detail Item Wise")
            //            {
            //                strCombo = "SELECT name from Contractor order by name";
            //                textBox1.Text = SelectCombo.ComboKeydown(this, e.KeyCode, strCombo, textBox1.Text, 1);
            //            }

            //            else if (frmptyp == "Supplier Detail Bill Wise")
            //            {
            //                strCombo = funs.GetStrCombo("*");

            //                textBox1.Text = SelectCombo.ComboKeydown(this, e.KeyCode, strCombo, textBox1.Text, 1);
            //            }

            //            else if (frmptyp == "Supplier Detail Item Wise")
            //            {
            //                strCombo = funs.GetStrCombo("*");

            //                textBox1.Text = SelectCombo.ComboKeydown(this, e.KeyCode, strCombo, textBox1.Text, 1);
            //            }
            //            else if (frmptyp == "Item Lifting")
            //            {
            //                strCombo = "Select Name from Other where Type=14 order by Name";
            //                textBox1.Text = SelectCombo.ComboKeydown(this, e.KeyCode, strCombo, textBox1.Text, 1);
            //            }
            //            else if (frmptyp == "Item Lifting Sale")
            //            {
            //                strCombo = "Select Name from Other where Type=14 order by Name";
            //                textBox1.Text = SelectCombo.ComboKeydown(this, e.KeyCode, strCombo, textBox1.Text, 1);
            //            }
            //            else if (frmptyp == "Item Lifting Detail Sale")
            //            {
            //                strCombo = "Select Name from Other where Type=14 order by Name";
            //                textBox1.Text = SelectCombo.ComboKeydown(this, e.KeyCode, strCombo, textBox1.Text, 1);
            //            }
            //            else if (frmptyp == "Company Wise Report")
            //            {
            //                strCombo = "Select Name from Other where Type=14 order by Name";
            //                textBox1.Text = SelectCombo.ComboKeydown(this, e.KeyCode, strCombo, textBox1.Text, 1);
            //            }

            //            else if (frmptyp == "Customer Billwise")
            //            {
            //                strCombo = funs.GetStrCombo("*");
            //                textBox1.Text = SelectCombo.ComboKeydown(this, e.KeyCode, strCombo, textBox1.Text, 1);
            //            }
            //            else if (frmptyp == "InBill Charges")
            //            {
            //                string strCombo = funs.GetStrCombo("*");
            //                textBox1.Text = SelectCombo.ComboKeydown(this, e.KeyCode, strCombo, textBox1.Text, 1);
            //            }

            //            else if (frmptyp == "Item Ledger")
            //            {
            //                strCombo = "SELECT DESCRIPTION.Description "+ access_sql.Concat +" ' (' "+ access_sql.Concat +" PACKING.Name  As Description  FROM DESCRIPTION INNER JOIN PACKING ON DESCRIPTION.Pack_id = PACKING.Pack_id";
            //                textBox1.Text = SelectCombo.ComboKeydown(this, e.KeyCode, strCombo, textBox1.Text, 1);
            //            }
            //            else if (frmptyp == "Stock Summary")
            //            {
            //                if (Feature.Available("Company Colour") == "No")
            //                {
            //                    textBox1.Enabled = false;
            //                }
            //                else
            //                {
            //                    strCombo = "Select Name from Other where Type=14 order by Name";
            //                    textBox1.Text = SelectCombo.ComboKeydown(this, e.KeyCode, strCombo, textBox1.Text, 1);  
            //                }



            //            }
            //            else if (frmptyp == "Below Stock Warning")
            //            {
            //                strCombo = "Select Name from Other where Type=14 order by Name";
            //                textBox1.Text = SelectCombo.ComboKeydown(this, e.KeyCode, strCombo, textBox1.Text, 1);
            //            }
            //            else if (frmptyp == "Stock TaxSlab Wise")
            //            {
            //                strCombo = "SELECT  distinct " + strqyery + "  AS Tax_Rate FROM TAXCATEGORYDETAIL GROUP BY TAXCATEGORYDETAIL.Category_Id, TAXCATEGORYDETAIL.SubCategory_Name";
            //                textBox1.Text = SelectCombo.ComboKeydown(this, e.KeyCode, strCombo, textBox1.Text, 1);
            //            }

            //            else if (frmptyp == "Party Price List")
            //            {
            //                strCombo = "SELECT DISTINCT ACCOUNT.Name FROM PARTYRATE LEFT JOIN ACCOUNT ON PARTYRATE.Ac_id = ACCOUNT.Ac_id ORDER BY ACCOUNT.Name";
            //                textBox1.Text = SelectCombo.ComboKeydown(this, e.KeyCode, strCombo, textBox1.Text, 1);
            //            }



            //            else if (frmptyp == "Price List")
            //            {


            //                DataTable dtcombo = new DataTable();
            //                strCombo = "Select Description from Description where Des_id=0";
            //                Database.GetSqlData(strCombo, dtcombo);
            //                dtcombo.Columns["Description"].ColumnName = "Rates";
            //                dtcombo.Rows.Add();
            //                dtcombo.Rows[0][0] = "Purchase_rate";

            //                dtcombo.Rows.Add();
            //                dtcombo.Rows[1][0] = "Retail";

            //                dtcombo.Rows.Add();
            //                dtcombo.Rows[2][0] = "Wholesale";

            //                dtcombo.Rows.Add();
            //                dtcombo.Rows[3][0] = "Rate_X";

            //                dtcombo.Rows.Add();
            //                dtcombo.Rows[4][0] = "Rate_Y";

            //                dtcombo.Rows.Add();
            //                dtcombo.Rows[5][0] = "Rate_Z";

            //                dtcombo.Rows.Add();
            //                dtcombo.Rows[6][0] = "MRP";



            //                textBox1.Text = SelectCombo.ComboDt(this, dtcombo, 0);






            //               // strCombo = "Select Name from Other where Type=14 order by Name";
            ////                textBox1.Text = SelectCombo.ComboKeydown(this, e.KeyCode, strCombo, textBox1.Text, 1);
            //            }

            //            else if (frmptyp == "Price Variation Sale")
            //            {
            //                strCombo = "SELECT DESCRIPTION.Description & ' (' & PACKING.Name  As Description  FROM DESCRIPTION INNER JOIN PACKING ON DESCRIPTION.Pack_id = PACKING.Pack_id";
            //                textBox1.Text = SelectCombo.ComboKeydown(this, e.KeyCode, strCombo, textBox1.Text, 1);
            //            }

            //            else if (frmptyp == "Price Variation Purchase")
            //            {
            //                strCombo = "SELECT DESCRIPTION.Description & ' (' & PACKING.Name  As Description  FROM DESCRIPTION INNER JOIN PACKING ON DESCRIPTION.Pack_id = PACKING.Pack_id";
            //                textBox1.Text = SelectCombo.ComboKeydown(this, e.KeyCode, strCombo, textBox1.Text, 1);
            //            }
            //            else if (frmptyp == "Particular Commodity Summary")
            //            {
            //                strCombo = "SELECT TAXCATEGORY.Category_Name, TAXCATEGORY.Commodity_Code FROM TAXCATEGORY ORDER BY TAXCATEGORY.Category_Name";
            //                textBox1.Text = SelectCombo.ComboKeydown(this, e.KeyCode, strCombo, textBox1.Text, 1);
            //            }
            //            else if (frmptyp == "Commodity Sale")
            //            {
            //                strCombo = "SELECT TAXCATEGORY.Category_Name, TAXCATEGORY.Commodity_Code FROM TAXCATEGORY ORDER BY TAXCATEGORY.Category_Name";
            //                textBox1.Text = SelectCombo.ComboKeydown(this, e.KeyCode, strCombo, textBox1.Text, 1);
            //            }
            //            else if (frmptyp == "Commodity Purchase")
            //            {
            //                strCombo = "SELECT TAXCATEGORY.Category_Name, TAXCATEGORY.Commodity_Code FROM TAXCATEGORY ORDER BY TAXCATEGORY.Category_Name";
            //                textBox1.Text = SelectCombo.ComboKeydown(this, e.KeyCode, strCombo, textBox1.Text, 1);
            //            }

            //            else if (frmptyp == "Customer Profit")
            //            {
            //                strCombo = funs.GetStrCombo("*");
            //                textBox1.Text = SelectCombo.ComboKeydown(this, e.KeyCode, strCombo, textBox1.Text, 1);
            //            }
            //            else if (frmptyp == "Supplier Lifting")
            //            {
            //                strCombo = funs.GetStrCombo("*"); 
            //                textBox1.Text = SelectCombo.ComboKeydown(this, e.KeyCode, strCombo, textBox1.Text, 1);
            //            }

            //            else if (frmptyp == "Stock Valuation")
            //            {
            //                strCombo = "Select Name from Other where Type=14 order by Name";
            //                textBox1.Text = SelectCombo.ComboKeydown(this, e.KeyCode, strCombo, textBox1.Text, 1);
            //            }

        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (frmptyp == "Destination Wise")
            {
                strCombo = "SELECT [name] from DeliveryPoint";
                textBox1.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 1);
            }
            else if (frmptyp == "Ledger")
            {
                strCombo = funs.GetStrCombo("*");
                textBox1.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 1);
            }
            else if (frmptyp == "Detail Ledger")
            {
                strCombo = funs.GetStrCombo("*");
                textBox1.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 1);
            }
            else if (frmptyp == "Customer Detail Bill Wise")
            {
                strCombo = funs.GetStrCombo("*");
                textBox1.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 1);
            }
            else if (frmptyp == "Customer Detail Item Wise")
            {
                strCombo = funs.GetStrCombo("*");
                textBox1.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 1);
            }
            else if (frmptyp == "Cash Report")
            {
                strCombo = "SELECT VOUCHERINFO.Formno FROM VOUCHERTYPE LEFT JOIN ((VOUCHERINFO LEFT JOIN ACCOUNT ON VOUCHERINFO.Ac_id = ACCOUNT.Ac_id) LEFT JOIN ACCOUNTYPE ON ACCOUNT.Act_id = ACCOUNTYPE.Act_id) ON VOUCHERTYPE.Vt_id = VOUCHERINFO.Vt_id GROUP BY VOUCHERINFO.Formno, ACCOUNTYPE.Act_id, VOUCHERTYPE.Vt_id HAVING (((VOUCHERINFO.Formno)<>'') AND ((ACCOUNTYPE.Act_id)=3) AND ((VOUCHERTYPE.Vt_id)=15 Or (VOUCHERTYPE.Vt_id)=3))";
                textBox1.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 1);
            }
            else if (frmptyp == "Customer Brokerage")
            {
                strCombo = funs.GetStrCombo("*");
                textBox1.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 1);
            }
            else if (frmptyp == "Stock Summary Cross")
            {
                strCombo = "Select Name from Other where Type=14 order by Name";
                textBox1.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 1);
            }
            else if (frmptyp == "Customer Pendings")
            {
                strCombo = funs.GetStrCombo("*");
                textBox1.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 1);
            }

            else if (frmptyp == "Customer Billwise")
            {
                strCombo = funs.GetStrCombo("*");
                textBox1.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 1);
            }

            else if (frmptyp == "Broker Detail Customer Wise")
            {
                strCombo = "SELECT name from Contractor order by name";
                textBox1.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 1);
            }

            else if (frmptyp == "Broker Detail Item Wise")
            {
                strCombo = "SELECT name from Contractor order by name";
                textBox1.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 1);
            }

            else if (frmptyp == "Supplier Detail Bill Wise")
            {
                strCombo = funs.GetStrCombo("*");
                textBox1.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 1);
            }

            else if (frmptyp == "Supplier Detail Item Wise")
            {
                strCombo = funs.GetStrCombo("*");
                textBox1.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 1);
            }
            else if (frmptyp == "Item Lifting")
            {
                strCombo = "Select Name from Other where Type=14 order by Name";
                textBox1.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 1);
            }
            else if (frmptyp == "Item Lifting Sale")
            {
                strCombo = "Select Name from Other where Type=14 order by Name";
                textBox1.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 1);
            }
            else if (frmptyp == "Item Lifting Detail Sale")
            {
                strCombo = "Select Name from Other where Type=14 order by Name";
                textBox1.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 1);
            }

            else if (frmptyp == "Company Wise Report")
            {
                strCombo = "Select Name from Other where Type=14 order by Name";
                textBox1.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 1);
            }

            else if (frmptyp == "InBill Charges")
            {
                string strCombo = funs.GetStrCombo("*");
                textBox1.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 1);
            }
            else if (frmptyp == "Item Ledger")
            {
                strCombo = "SELECT DESCRIPTION.Description " + access_sql.Concat + " ' (' " + access_sql.Concat + " PACK  As Description  FROM DESCRIPTION";
                textBox1.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 1);
            }
            else if (frmptyp == "Stock Summary")
            {
                if (Feature.Available("Company Colour") == "No")
                {
                    textBox1.Enabled = false;
                }
                else
                {
                    strCombo = "Select Name from Other where Type=14 order by Name";
                    textBox1.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 1);
                }
            }
            else if (frmptyp == "Below Stock Warning")
            {
                strCombo = "Select Name from Other where Type=14 order by Name";
                textBox1.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 1);
            }

            else if (frmptyp == "Stock TaxSlab Wise")
            {
                strCombo = " SELECT distinct " + strqyery + "  AS Tax_Rate FROM TAXCATEGORYDETAIL GROUP BY TAXCATEGORYDETAIL.Category_Id, TAXCATEGORYDETAIL.SubCategory_Name";
                textBox1.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 1);
            }

            else if (frmptyp == "Party Price List")
            {
                strCombo = "SELECT DISTINCT ACCOUNT.Name FROM PARTYRATE LEFT JOIN ACCOUNT ON PARTYRATE.Ac_id = ACCOUNT.Ac_id ORDER BY ACCOUNT.Name";
                textBox1.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 1);
            }
            else if (frmptyp == "Price List")
            {

                DataTable dtcombo = new DataTable();
                strCombo = "Select Description from Description where Des_id=0";
                Database.GetSqlData(strCombo, dtcombo);
                dtcombo.Columns["Description"].ColumnName = "Rates";
                dtcombo.Rows.Add();
                dtcombo.Rows[0][0] = "Purchase_rate";

                dtcombo.Rows.Add();
                dtcombo.Rows[1][0] = "Retail";

                dtcombo.Rows.Add();
                dtcombo.Rows[2][0] = "Wholesale";

                dtcombo.Rows.Add();
                dtcombo.Rows[3][0] = "Rate_X";

                dtcombo.Rows.Add();
                dtcombo.Rows[4][0] = "Rate_Y";

                dtcombo.Rows.Add();
                dtcombo.Rows[5][0] = "Rate_Z";

                dtcombo.Rows.Add();
                dtcombo.Rows[6][0] = "MRP";
                textBox1.Text = SelectCombo.ComboDt1(this, dtcombo, 0);

                //   strCombo = "Select Name from Other where Type=14 order by Name";
                // textBox1.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 1);
            }
            else if (frmptyp == "Price Variation Sale")
            {
                strCombo = "SELECT DESCRIPTION.Description & ' (' & PACKING.Name  As Description  FROM DESCRIPTION INNER JOIN PACKING ON DESCRIPTION.Pack_id = PACKING.Pack_id";
                textBox1.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 1);
            }
            else if (frmptyp == "Price Variation Purchase")
            {
                strCombo = "SELECT DESCRIPTION.Description & ' (' & PACKING.Name  As Description  FROM DESCRIPTION INNER JOIN PACKING ON DESCRIPTION.Pack_id = PACKING.Pack_id";
                textBox1.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 1);
            }
            else if (frmptyp == "Particular Commodity Summary")
            {
                strCombo = "SELECT TAXCATEGORY.Category_Name, TAXCATEGORY.Commodity_Code FROM TAXCATEGORY ORDER BY TAXCATEGORY.Category_Name";
                textBox1.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 1);
            }
            else if (frmptyp == "Commodity Sale")
            {
                strCombo = "SELECT TAXCATEGORY.Category_Name, TAXCATEGORY.Commodity_Code FROM TAXCATEGORY ORDER BY TAXCATEGORY.Category_Name";
                textBox1.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 1);
            }
            else if (frmptyp == "Commodity Purchase")
            {
                strCombo = "SELECT TAXCATEGORY.Category_Name, TAXCATEGORY.Commodity_Code FROM TAXCATEGORY ORDER BY TAXCATEGORY.Category_Name";
                textBox1.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 1);
            }
            else if (frmptyp == "Customer Profit")
            {
                strCombo = funs.GetStrCombo("*");
                textBox1.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 1);
            }
            else if (frmptyp == "Supplier Lifting")
            {
                strCombo = funs.GetStrCombo("*");
                textBox1.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 1);
            }

            else if (frmptyp == "Stock Valuation")
            {
                strCombo = "Select Name from Other where Type=14 order by Name";
                textBox1.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 1);
            }
            else if (frmptyp == "Stock Valuation ")
            {
                strCombo = "select distinct '<MAIN>' as name from account union all SELECT ACCOUNT.Name as name FROM ACCOUNT LEFT JOIN ACCOUNTYPE ON ACCOUNT.Act_id = ACCOUNTYPE.Act_id WHERE (((ACCOUNTYPE.Name)='Godown')) GROUP BY ACCOUNT.Name";
                textBox1.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 1);
            }
        }

        private string GetPapersize()
        {
            return Database.GetScalarText("Select PaperSize from VOUCHERTYPEs where Name='" + frmptyp + "' ");
        }

      
        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Enter)
            //{
            //    String clkStr = "";
            //    if (dataGridView1.CurrentCell.Value != null)
            //    {
            //        clkStr = dataGridView1.CurrentCell.Value.ToString();
            //    }


            //    //if (funs.Select_ac_id(clkStr) != 0)
            //    if (funs.Select_ac_id(clkStr) != "")
            //    {
            //        Report gg = new Report();
            //        gg.Ledger(stdt, endt, clkStr);
            //        gg.ShowDialog();
            //        e.Handled = true;
            //    }
            //    else if (funs.Select_Refineact_id(clkStr) != 0)
            //    {
            //        Report gg = new Report();
            //        gg.SingleGroupedTrial(stdt, endt, clkStr);
            //        gg.ShowDialog();
            //        e.Handled = true;
            //    }
            //    else if (IsDocumentNumber(clkStr) != "0")
            //    {
            //        funs.OpenFrm(this, IsDocumentNumber(clkStr), false);
            //        e.Handled = true;
            //    }
            //}
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

            if (frmptyp == "Ledger")
            {
                if (checkBox1.Checked == false)
                {
                    checkBox1.Text = "Summarized";
                   // checkBox1.Checked = true;
                }
                else
                {
                    checkBox1.Text = "Detailed";
                }
            }
            button3_Click(sender, e);

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button6_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ContextMenu cm = new ContextMenu();
                //cm.MenuItems.Add("Export to Excel", new EventHandler(Item1_Click));
                cm.MenuItems.Add("Export Data Only", new EventHandler(Item2_Click));
                button6.ContextMenu = cm;
            }
        }

        void Item1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)
            {
                return;
            }
            Object misValue = System.Reflection.Missing.Value;
            Excel.Application apl = new Microsoft.Office.Interop.Excel.Application();
            Excel.Workbook wb = (Excel.Workbook)apl.Workbooks.Add(misValue);
            Excel.Worksheet ws;
            ws = (Excel.Worksheet)wb.Worksheets[1];

            int lno = 1;
            DataTable dtExcel = new DataTable();

            DataTable dtRheader = new DataTable();
            Database.GetSqlData("select * from company", dtRheader);

            ws.Cells[lno, 1] = dtRheader.Rows[0]["name"].ToString();
            ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, dataGridView1.Columns.Count]).Merge(Type.Missing);
            ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, dataGridView1.Columns.Count]).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, dataGridView1.Columns.Count]).Font.Bold = true;
            lno++;

            ws.Cells[lno, 1] = dtRheader.Rows[0]["Address1"].ToString();
            ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, dataGridView1.Columns.Count]).Merge(Type.Missing);
            ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, dataGridView1.Columns.Count]).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, dataGridView1.Columns.Count]).Font.Bold = true;
            lno++;

            ws.Cells[lno, 1] = dtRheader.Rows[0]["Address2"].ToString();
            ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, dataGridView1.Columns.Count]).Merge(Type.Missing);
            ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, dataGridView1.Columns.Count]).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, dataGridView1.Columns.Count]).Font.Bold = true;
            lno++;



            for (int i = 0; i < dataGridView1.Columns.Count; i++)
            {
                if (dataGridView1.Columns[i].HeaderCell.Style.Alignment == DataGridViewContentAlignment.MiddleRight)
                {
                    ws.get_Range(ws.Cells[5, i + 1], ws.Cells[5, i + 1]).HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                }
                ws.get_Range(ws.Cells[i + 1, i + 1], ws.Cells[i + 1, i + 1]).ColumnWidth = dataGridView1.Columns[i].Width / 11.5;
                ws.Cells[5, i + 1] = dataGridView1.Columns[i].HeaderText.ToString();

            }

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                for (int j = 0; j < dataGridView1.Columns.Count; j++)
                {
                    if (dataGridView1.Columns[j].HeaderCell.Style.Alignment == DataGridViewContentAlignment.MiddleRight)
                    {
                        ws.get_Range(ws.Cells[i + 6, j + 1], ws.Cells[i + 6, j + 1]).HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                        ws.get_Range(ws.Cells[i + 6, j + 1], ws.Cells[i + 6, j + 1]).NumberFormat = "0,0.00";

                    }
                    else
                    {
                        ws.get_Range(ws.Cells[i + 6, j + 1], ws.Cells[i + 6, j + 1]).HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                    }

                    if (dataGridView1.Columns[j].DefaultCellStyle.Font != null)
                    {
                        ws.get_Range(ws.Cells[i + 6, j + 1], ws.Cells[i + 6, j + 1]).Font.Bold = true;

                    }

                    if (dataGridView1.Rows[i].Cells[j].Value != null)
                    {
                        ws.Cells[i + 6, j + 1] = dataGridView1.Rows[i].Cells[j].Value.ToString().Replace(",", "");
                    }
                }
            }

            Excel.Range last = ws.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell, Type.Missing);
            ws.get_Range("A1", last).WrapText = true;
            apl.Visible = true;
        }

        void Item2_Click(object sender, EventArgs e)
        {
            if (dtFinal.Rows.Count == 0)
            {
                return;
            }

            Object misValue = System.Reflection.Missing.Value;
            Excel.Application apl = new Microsoft.Office.Interop.Excel.Application();
            Excel.Workbook wb = (Excel.Workbook)apl.Workbooks.Add(misValue);
            Excel.Worksheet ws;
            ws = (Excel.Worksheet)wb.Worksheets[1];

            for (int i = 0; i < dataGridView1.Columns.Count; i++)
            {
                ws.Cells[1, i + 1] = dataGridView1.Columns[i].HeaderText;
            }
            
            int firstRow = 1, firstCol = 0, lastRow = dtFinal.Rows.Count, lastCol = dtFinal.Columns.Count;

            Excel.Range all = (Excel.Range)ws.get_Range(ws.Cells[firstRow + 1, firstCol + 1], ws.Cells[lastRow+1, lastCol+1]);
            string[,] arrayDT = new string[dt.Rows.Count, dt.Columns.Count];
            for (int i = 0; i < dt.Rows.Count; i++)
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    arrayDT[i, j] = dt.Rows[i][j].ToString();
                }
            all.Value2 = arrayDT;
            apl.Visible = true;
        }
    }
}
