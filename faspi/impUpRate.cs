using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Office.Core;
using Excel = Microsoft.Office.Interop.Excel;


namespace faspi
{
    public partial class impUpRate : Form
    {
        String fName = "";
        DataTable dtname = new DataTable("Description");
        static Object misValue = System.Reflection.Missing.Value;
        static Excel.Application apl = new Microsoft.Office.Interop.Excel.Application();
        Excel.Workbook wb;
        Excel.Worksheet ws;


        public impUpRate()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dtupdate();
        }
        private void dtupdate()
        {
            wb = (Excel.Workbook)apl.Workbooks.Open(openFileDialog1.FileName, true, true, misValue, null, null, false, misValue, null, false, false, misValue, misValue, misValue, false);
            ws = (Excel.Worksheet)wb.Worksheets[1];
            int sheetusedcol = 0;
            int i = 0;
            int lno = 0;
            Excel.Range range = ws.UsedRange;
            dtname.Rows.Clear();
            dtname.Columns.Clear();

            dtname.Columns.Add("Description", typeof(string));
            dtname.Columns.Add("Pvalue", typeof(decimal));
            dtname.Columns.Add("Purchase_rate", typeof(decimal));
            dtname.Columns.Add("Wholesale", typeof(decimal));
            dtname.Columns.Add("Retail", typeof(decimal));
            dtname.Columns.Add("Rate_X", typeof(decimal));
            dtname.Columns.Add("Rate_Y", typeof(decimal));
            dtname.Columns.Add("Rate_Z", typeof(decimal));
            dtname.Columns.Add("MRP", typeof(decimal));

            while ((range.Cells[(i + 2), 1] as Excel.Range).Value2 != null)
            {


                sheetusedcol = range.Columns.Count;

                for (int j = 1; j < sheetusedcol; j++)
                {
                    if ((range.Cells[(i + 2), j + 1] as Excel.Range).Value2 != null)
                    {
                        dtname.Rows.Add();
                        dtname.Rows[lno]["Description"] = (range.Cells[(i + 2), 1] as Excel.Range).Value2.ToString().Replace("  ", " ").Trim();
                        dtname.Rows[lno]["Pvalue"] = (range.Cells[(1), j + 1] as Excel.Range).Value2;
                        dtname.Rows[lno]["Purchase_rate"] = (range.Cells[(i + 2), j + 1] as Excel.Range).Value2;
                        lno++;
                    }

                }
                i++;
            }
            //sheet2
            ws = (Excel.Worksheet)wb.Worksheets[2];
            range = ws.UsedRange;
            i = 0;
            lno = 0;
            while ((range.Cells[(i + 2), 1] as Excel.Range).Value2 != null)
            {

                sheetusedcol = range.Columns.Count;
                for (int j = 1; j < sheetusedcol; j++)
                {
                    if ((range.Cells[(i + 2), j + 1] as Excel.Range).Value2 != null)
                    {
                        dtname.Rows[lno]["Wholesale"] = (range.Cells[(i + 2), j + 1] as Excel.Range).Value2;
                        lno++;
                    }

                }
                i++;
            }

            //sheet3
            ws = (Excel.Worksheet)wb.Worksheets[3];
            range = ws.UsedRange;
            i = 0;
            lno = 0;
            while ((range.Cells[(i + 2), 1] as Excel.Range).Value2 != null)
            {


                sheetusedcol = range.Columns.Count;

                for (int j = 1; j < sheetusedcol; j++)
                {
                    if ((range.Cells[(i + 2), j + 1] as Excel.Range).Value2 != null)
                    {
                        dtname.Rows[lno]["Retail"] = (range.Cells[(i + 2), j + 1] as Excel.Range).Value2;
                        lno++;
                    }

                }
                i++;
            }

            //sheet4
            ws = (Excel.Worksheet)wb.Worksheets[4];
            range = ws.UsedRange;
            i = 0;
            lno = 0;
            while ((range.Cells[(i + 2), 1] as Excel.Range).Value2 != null)
            {


                sheetusedcol = range.Columns.Count;

                for (int j = 1; j < sheetusedcol; j++)
                {
                    if ((range.Cells[(i + 2), j + 1] as Excel.Range).Value2 != null)
                    {

                        dtname.Rows[lno]["Rate_X"] = (range.Cells[(i + 2), j + 1] as Excel.Range).Value2;
                        lno++;
                    }

                }
                i++;
            }

            //sheet5
            ws = (Excel.Worksheet)wb.Worksheets[5];
            range = ws.UsedRange;
            i = 0;
            lno = 0;
            while ((range.Cells[(i + 2), 1] as Excel.Range).Value2 != null)
            {
                sheetusedcol = range.Columns.Count;
                for (int j = 1; j < sheetusedcol; j++)
                {
                    if ((range.Cells[(i + 2), j + 1] as Excel.Range).Value2 != null)
                    {

                        dtname.Rows[lno]["Rate_Y"] = (range.Cells[(i + 2), j + 1] as Excel.Range).Value2;
                        lno++;
                    }
                }
                i++;
            }

            //sheet6
            ws = (Excel.Worksheet)wb.Worksheets[6];
            range = ws.UsedRange;
            i = 0;
            lno = 0;
            while ((range.Cells[(i + 2), 1] as Excel.Range).Value2 != null)
            {
                sheetusedcol = range.Columns.Count;
                for (int j = 1; j < sheetusedcol; j++)
                {
                    if ((range.Cells[(i + 2), j + 1] as Excel.Range).Value2 != null)
                    {
                        dtname.Rows[lno]["Rate_Z"] = (range.Cells[(i + 2), j + 1] as Excel.Range).Value2;
                        lno++;
                    }
                }
                i++;
            }

            //sheet7
            ws = (Excel.Worksheet)wb.Worksheets[7];
            range = ws.UsedRange;
            i = 0;
            lno = 0;
            while ((range.Cells[(i + 2), 1] as Excel.Range).Value2 != null)
            {
                sheetusedcol = range.Columns.Count;
                for (int j = 1; j < sheetusedcol; j++)
                {
                    if ((range.Cells[(i + 2), j + 1] as Excel.Range).Value2 != null)
                    {
                        dtname.Rows[lno]["MRP"] = (range.Cells[(i + 2), j + 1] as Excel.Range).Value2;
                        lno++;
                    }
                }
                i++;
            }

            string strquery = "";
            for (int j = 0; j < dtname.Rows.Count; j++)
            {
                if (dtname.Rows[j]["Purchase_rate"].ToString() == "")
                {
                    dtname.Rows[j]["Purchase_rate"] = 0;
                }
                if (dtname.Rows[j]["Wholesale"].ToString() == "")
                {
                    dtname.Rows[j]["Wholesale"] = 0;
                }
                if (dtname.Rows[j]["Retail"].ToString() == "")
                {
                    dtname.Rows[j]["Retail"] = 0;
                }
                if (dtname.Rows[j]["Rate_X"].ToString() == "")
                {
                    dtname.Rows[j]["Rate_X"] = 0;
                }
                if (dtname.Rows[j]["Rate_Y"].ToString() == "")
                {
                    dtname.Rows[j]["Rate_Y"] = 0;
                }
                if (dtname.Rows[j]["Rate_Z"].ToString() == "")
                {
                    dtname.Rows[j]["Rate_Z"] = 0;
                }
                if (dtname.Rows[j]["MRP"].ToString() == "")
                {
                    dtname.Rows[j]["MRP"] = 0;
                }

                strquery = strquery + "Update Description LEFT JOIN PACKING ON DESCRIPTION.Pack_id = PACKING.Pack_id SET Purchase_rate=" + double.Parse(dtname.Rows[j]["Purchase_rate"].ToString()) + ",Wholesale=" + double.Parse(dtname.Rows[j]["Wholesale"].ToString()) + ",Retail=" + double.Parse(dtname.Rows[j]["Retail"].ToString()) + ",Rate_X=" + double.Parse(dtname.Rows[j]["Rate_X"].ToString()) + ",Rate_Y=" + double.Parse(dtname.Rows[j]["Rate_Y"].ToString()) + ",Rate_Z=" + double.Parse(dtname.Rows[j]["Rate_Z"].ToString()) + ",MRP=" + double.Parse(dtname.Rows[j]["MRP"].ToString()) + " where Description='" + dtname.Rows[j]["Description"].ToString() + "' and Packing.Pvalue=" + dtname.Rows[j]["Pvalue"].ToString() + "; ";
            }
            string[] ar = strquery.Split(';');
            for (int a = 0; a < ar.Length; a++)
            {
                Database.CommandExecutor(ar[a]);
            }


            MessageBox.Show("Done");

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (DialogResult.OK == openFileDialog1.ShowDialog())
            {
                fName = openFileDialog1.FileName;
                textBox1.Text = fName;
            }

        }

    }
}
