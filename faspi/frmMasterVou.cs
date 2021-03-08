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
    public partial class frmMasterVou : Form
    {
        string gstr = "";
        string gFrmCaption = "";
        DataTable dt;
        DataTable dtvou = new DataTable();
        ToolTip tooltip = new ToolTip();

        public frmMasterVou()                         
        {
            InitializeComponent();
            dateTimePicker1.CustomFormat = Database.dformat;
            dateTimePicker2.CustomFormat = Database.dformat;
            dateTimePicker1.MinDate = Database.stDate;
            dateTimePicker1.MaxDate = Database.ldate;
            dateTimePicker2.MinDate = Database.stDate;
            dateTimePicker2.MaxDate = Database.ldate;
            dateTimePicker1.Value = Database.ldate;
            dateTimePicker2.Value = Database.ldate;
        }

        private void Excelexport()
        {
            if (ansGridView5.Rows.Count == 0)
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
            Database.GetSqlData("select * from dbo.Location", dtRheader);

            ws.Cells[lno, 1] = dtRheader.Rows[0]["name"].ToString();
            ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, ansGridView5.Columns.Count]).Merge(Type.Missing);
            ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, ansGridView5.Columns.Count]).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, ansGridView5.Columns.Count]).Font.Bold = true;
            lno++;

            ws.Cells[lno, 1] = dtRheader.Rows[0]["Address1"].ToString();
            ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, ansGridView5.Columns.Count]).Merge(Type.Missing);
            ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, ansGridView5.Columns.Count]).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, ansGridView5.Columns.Count]).Font.Bold = true;
            lno++;

            ws.Cells[lno, 1] = dtRheader.Rows[0]["Address2"].ToString();
            ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, ansGridView5.Columns.Count]).Merge(Type.Missing);
            ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, ansGridView5.Columns.Count]).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, ansGridView5.Columns.Count]).Font.Bold = true;
            lno++;

            int a = 0;

            for (int i = 5; i < 10; i++)
            {

                if (ansGridView5.Columns[a].HeaderCell.Style.Alignment == DataGridViewContentAlignment.MiddleRight)
                {
                    ws.get_Range(ws.Cells[5, a + 1], ws.Cells[5, a + 1]).HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                }
                ws.get_Range(ws.Cells[i + 1, a + 1], ws.Cells[a + 1, a + 1]).ColumnWidth = ansGridView5.Columns[i].Width / 11.5;
                ws.Cells[5, a + 1] = ansGridView5.Columns[i].HeaderText.ToString();
                a++;
            }
           
            for (int i = 0; i < ansGridView5.Rows.Count; i++)
            {
                int b = 0;
                for (int j = 5; j < 10; j++)
                {
                    if (ansGridView5.Columns[j].HeaderCell.Style.Alignment == DataGridViewContentAlignment.MiddleRight)
                    {
                        ws.get_Range(ws.Cells[i + 6, b + 1], ws.Cells[i + 6, b + 1]).HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                        ws.get_Range(ws.Cells[i + 6, b + 1], ws.Cells[i + 6, b + 1]).NumberFormat = "0,0.00";
                    }

                    else
                    {
                        ws.get_Range(ws.Cells[i + 6, b + 1], ws.Cells[i + 6, b + 1]).HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                    }

                    if (ansGridView5.Columns[j].DefaultCellStyle.Font != null)
                    {
                        ws.get_Range(ws.Cells[i + 6, b + 1], ws.Cells[i + 6, b + 1]).Font.Bold = true;
                    }

                    if (ansGridView5.Rows[i].Cells[j].Value != null)
                    {
                        ws.Cells[i + 6, b + 1] = ansGridView5.Rows[i].Cells[j].Value.ToString().Replace(",", "");
                    }

                    b++;
                }
            }

            Excel.Range last = ws.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell, Type.Missing);
            ws.get_Range("A1", last).WrapText = true;
            ws.Columns.AutoFit();
            apl.Visible = true;
        }

        public void LoadData(string str, string FrmCaption)
        {
            gstr = str;
            gFrmCaption = FrmCaption;
            string sql = "";

            if (str == "Booking")
            {
                sql = "SELECT CONVERT(nvarchar, VOUCHERINFOs.Vdate, 106) AS Vdate, VOUCHERTYPEs.Name, ACCOUNTs.name AS consigner, ACCOUNTs_1.name AS consignee, VOUCHERINFOs.Vnumber, USERs_1.UserName AS CreatedBy, VOUCHERINFOs.Totalamount AS Amount, VOUCHERINFOs.printcount AS PrintCount, VOUCHERINFOs.Vi_id, USERs.UserName AS ModifiedBy, CONVERT(nvarchar, VOUCHERINFOs.modify_date, 106) AS Modify_date, VOUCHERINFOs.ModTime AS Modify_time FROM ACCOUNTs RIGHT OUTER JOIN VOUCHERINFOs LEFT OUTER JOIN USERs ON VOUCHERINFOs.modifyby_id = USERs.u_id LEFT OUTER JOIN USERs AS USERs_1 ON VOUCHERINFOs.user_id = USERs_1.u_id LEFT OUTER JOIN VOUCHERTYPEs ON VOUCHERINFOs.Vt_id = VOUCHERTYPEs.Vt_id LEFT OUTER JOIN ACCOUNTs AS ACCOUNTs_1 ON VOUCHERINFOs.Ac_id2 = ACCOUNTs_1.ac_id ON ACCOUNTs.ac_id = VOUCHERINFOs.Ac_id WHERE (VOUCHERTYPEs.Type = 'Booking') AND (VOUCHERTYPEs.A = 1) AND (VOUCHERINFOs.Vdate >= '" + dateTimePicker1.Value.Date.ToString(Database.dformat) + "') AND (VOUCHERINFOs.Vdate <= '" + dateTimePicker2.Value.Date.ToString(Database.dformat) + "') AND (VOUCHERINFOs.LocationId = '" + Database.LocationId + "') ORDER BY VOUCHERINFOs.Vnumber DESC";
            }
            else if (str == "Stock Transfer")
            {
                sql = "SELECT CONVERT(nvarchar, VOUCHERINFOs_1.Vdate, 106) AS VDate, VOUCHERTYPEs.Name, ACCOUNTs.name AS Driver, VOUCHERINFOs_1.Vnumber, USERs_1.UserName AS CreatedBy, VOUCHERINFOs_1.Totalamount AS Amount, VOUCHERINFOs_1.printcount AS PrintCount, VOUCHERINFOs_1.Vi_id, Gaddis.Gaddi_name, USERs.UserName AS ModifiedBy, DeliveryPoints.Name AS source, DeliveryPoints_1.Name AS destination, CONVERT(nvarchar, VOUCHERINFOs_1.modify_date, 106) as Modify_date, VOUCHERINFOs_1.ModTime as Modify_time FROM Gaddis RIGHT OUTER JOIN Voucherdets LEFT OUTER JOIN VOUCHERINFOs ON Voucherdets.Booking_id = VOUCHERINFOs.Vi_id RIGHT OUTER JOIN VOUCHERINFOs AS VOUCHERINFOs_1 ON Voucherdets.Vi_id = VOUCHERINFOs_1.Vi_id LEFT OUTER JOIN DeliveryPoints AS DeliveryPoints_1 ON VOUCHERINFOs_1.SId = DeliveryPoints_1.DPId LEFT OUTER JOIN ACCOUNTs ON VOUCHERINFOs_1.Driver_name = ACCOUNTs.ac_id LEFT OUTER JOIN DeliveryPoints ON VOUCHERINFOs_1.Consigner_id = DeliveryPoints.DPId LEFT OUTER JOIN VOUCHERTYPEs ON VOUCHERINFOs_1.Vt_id = VOUCHERTYPEs.Vt_id LEFT OUTER JOIN USERs ON VOUCHERINFOs_1.modifyby_id = USERs.u_id LEFT OUTER JOIN USERs AS USERs_1 ON VOUCHERINFOs_1.user_id = USERs_1.u_id ON Gaddis.Gaddi_id = VOUCHERINFOs_1.Gaddi_id WHERE (VOUCHERTYPEs.Type = 'Stock Transfer') AND (VOUCHERINFOs_1.LocationId = '" + Database.LocationId + "') AND (VOUCHERINFOs_1.Vdate >= '" + dateTimePicker1.Value.Date.ToString(Database.dformat) + "') AND (VOUCHERINFOs_1.Vdate <= '" + dateTimePicker2.Value.Date.ToString(Database.dformat) + "') GROUP BY CONVERT(nvarchar, VOUCHERINFOs_1.Vdate, 106), VOUCHERTYPEs.Name, ACCOUNTs.name, VOUCHERINFOs_1.Vnumber, USERs_1.UserName, VOUCHERINFOs_1.printcount, VOUCHERINFOs_1.Vi_id, Gaddis.Gaddi_name, USERs.UserName, DeliveryPoints.Name, DeliveryPoints_1.Name, VOUCHERINFOs_1.Totalamount, VOUCHERINFOs_1.Modify_date, VOUCHERINFOs_1.ModTime ORDER BY VDate DESC, VOUCHERINFOs_1.Vnumber DESC";
            }
            else if (str == "Challan")
            {
                sql = "SELECT CONVERT(nvarchar, VOUCHERINFOs_1.Vdate, 106) AS VDate, VOUCHERTYPEs.Name, ACCOUNTs.name AS Driver, VOUCHERINFOs_1.Vnumber, USERs_1.UserName AS CreatedBy, VOUCHERINFOs_1.Totalamount AS Amount, VOUCHERINFOs_1.printcount AS PrintCount, VOUCHERINFOs_1.Vi_id, Gaddis.Gaddi_name, USERs.UserName AS ModifiedBy, DeliveryPoints.Name AS source, DeliveryPoints_1.Name AS destination, CONVERT(nvarchar, VOUCHERINFOs_1.modify_date, 106) as Modify_date, VOUCHERINFOs_1.ModTime as Modify_time FROM Gaddis RIGHT OUTER JOIN Voucherdets LEFT OUTER JOIN VOUCHERINFOs ON Voucherdets.Booking_id = VOUCHERINFOs.Vi_id RIGHT OUTER JOIN VOUCHERINFOs AS VOUCHERINFOs_1 ON Voucherdets.Vi_id = VOUCHERINFOs_1.Vi_id LEFT OUTER JOIN DeliveryPoints AS DeliveryPoints_1 ON VOUCHERINFOs_1.SId = DeliveryPoints_1.DPId LEFT OUTER JOIN ACCOUNTs ON VOUCHERINFOs_1.Driver_name = ACCOUNTs.ac_id LEFT OUTER JOIN DeliveryPoints ON VOUCHERINFOs_1.Consigner_id = DeliveryPoints.DPId LEFT OUTER JOIN VOUCHERTYPEs ON VOUCHERINFOs_1.Vt_id = VOUCHERTYPEs.Vt_id LEFT OUTER JOIN USERs ON VOUCHERINFOs_1.modifyby_id = USERs.u_id LEFT OUTER JOIN USERs AS USERs_1 ON VOUCHERINFOs_1.user_id = USERs_1.u_id ON Gaddis.Gaddi_id = VOUCHERINFOs_1.Gaddi_id WHERE (VOUCHERTYPEs.Type = 'Challan') AND (VOUCHERINFOs_1.LocationId = '" + Database.LocationId + "') AND (VOUCHERINFOs_1.Vdate >= '" + dateTimePicker1.Value.Date.ToString(Database.dformat) + "') AND (VOUCHERINFOs_1.Vdate <= '" + dateTimePicker2.Value.Date.ToString(Database.dformat) + "') GROUP BY CONVERT(nvarchar, VOUCHERINFOs_1.Vdate, 106), VOUCHERTYPEs.Name, ACCOUNTs.name, VOUCHERINFOs_1.Vnumber, USERs_1.UserName, VOUCHERINFOs_1.printcount, VOUCHERINFOs_1.Vi_id, Gaddis.Gaddi_name, USERs.UserName, DeliveryPoints.Name, DeliveryPoints_1.Name, VOUCHERINFOs_1.Totalamount, VOUCHERINFOs_1.Modify_date, VOUCHERINFOs_1.ModTime ORDER BY VDate DESC,VOUCHERINFOs_1.Vnumber DESC";
            }
            else if (str == "Unloading")
            {
                sql = "SELECT CONVERT(nvarchar, VOUCHERINFOs.Vdate, 106) AS VDate, VOUCHERTYPEs.Name, VOUCHERINFOs.Vnumber, VOUCHERINFOs_1.Invoiceno AS Challan_no, USERs_1.UserName AS CreatedBy, USERs.UserName AS ModifiedBy, VOUCHERINFOs.printcount AS PrintCount, VOUCHERINFOs.Vi_id, VOUCHERINFOs.Totalamount AS Amount, CONVERT(nvarchar, VOUCHERINFOs.modify_date, 106) as Modify_date, VOUCHERINFOs.ModTime as Modify_time FROM VOUCHERINFOs LEFT OUTER JOIN VOUCHERTYPEs ON VOUCHERINFOs.Vt_id = VOUCHERTYPEs.Vt_id LEFT OUTER JOIN VOUCHERINFOs AS VOUCHERINFOs_1 ON VOUCHERINFOs.Challan_id = VOUCHERINFOs_1.Vi_id LEFT OUTER JOIN USERs ON VOUCHERINFOs.modifyby_id = USERs.u_id LEFT OUTER JOIN USERs AS USERs_1 ON VOUCHERINFOs.user_id = USERs_1.u_id WHERE (VOUCHERINFOs.Vdate >= '" + dateTimePicker1.Value.Date.ToString(Database.dformat) + "') AND (VOUCHERINFOs.Vdate <= '" + dateTimePicker2.Value.Date.ToString(Database.dformat) + "') AND (VOUCHERTYPEs.Type = N'Unloading') AND (VOUCHERINFOs.LocationId = '" + Database.LocationId + "') ORDER BY VOUCHERINFOs.Vdate DESC, VOUCHERINFOs.Vnumber DESC";
            }
            else if (str == "Sale")
            {
                //sql = "SELECT CONVERT(nvarchar, VOUCHERINFOs.Vdate, 106) AS VDate, VOUCHERTYPEs.Name, VOUCHERINFOs.Vnumber, VOUCHERINFOs_1.Invoiceno AS Challan_no, USERs_1.UserName AS CreatedBy, USERs.UserName AS ModifiedBy, VOUCHERINFOs.printcount AS PrintCount, VOUCHERINFOs.Vi_id, VOUCHERINFOs.Totalamount AS Amount, CONVERT(nvarchar, VOUCHERINFOs.modify_date, 106) as Modify_date, VOUCHERINFOs.ModTime as Modify_time FROM VOUCHERINFOs LEFT OUTER JOIN VOUCHERTYPEs ON VOUCHERINFOs.Vt_id = VOUCHERTYPEs.Vt_id LEFT OUTER JOIN VOUCHERINFOs AS VOUCHERINFOs_1 ON VOUCHERINFOs.Challan_id = VOUCHERINFOs_1.Vi_id LEFT OUTER JOIN USERs ON VOUCHERINFOs.modifyby_id = USERs.u_id LEFT OUTER JOIN USERs AS USERs_1 ON VOUCHERINFOs.user_id = USERs_1.u_id WHERE (VOUCHERINFOs.Vdate >= '" + dateTimePicker1.Value.Date.ToString(Database.dformat) + "') AND (VOUCHERINFOs.Vdate <= '" + dateTimePicker2.Value.Date.ToString(Database.dformat) + "') AND (VOUCHERTYPEs.Type = N'Unloading') AND (VOUCHERINFOs.LocationId = '" + Database.LocationId + "') ORDER BY VOUCHERINFOs.Vdate DESC, VOUCHERINFOs.Vnumber DESC";
                sql = "SELECT CONVERT(nvarchar, VOUCHERINFOs.Vdate, 106) AS VDate, VOUCHERTYPEs.Name, ACCOUNTs.name AS Acc_name, VOUCHERINFOs.Vnumber, VOUCHERINFOs.Totalamount AS Amount, USERs_1.UserName AS CreatedBy, VOUCHERINFOs.printcount AS PrintCount, USERs.UserName AS ModifiedBy, VOUCHERINFOs.Vi_id, CONVERT(nvarchar, VOUCHERINFOs.modify_date, 106) as Modify_date, VOUCHERINFOs.ModTime as Modify_time FROM VOUCHERTYPEs RIGHT OUTER JOIN USERs USERs_1 RIGHT OUTER JOIN VOUCHERINFOs LEFT OUTER JOIN USERs ON VOUCHERINFOs.modifyby_id = USERs.u_id ON USERs_1.u_id = VOUCHERINFOs.user_id LEFT OUTER JOIN ACCOUNTs ON VOUCHERINFOs.Ac_id = ACCOUNTs.ac_id ON VOUCHERTYPEs.Vt_id = VOUCHERINFOs.Vt_id WHERE (VOUCHERTYPEs.Type = '" + str + "') AND (VOUCHERINFOs.LocationId = '" + Database.LocationId + "') AND (VOUCHERINFOs.Vdate >= '" + dateTimePicker1.Value.Date.ToString(Database.dformat) + "') AND (VOUCHERINFOs.Vdate <= '" + dateTimePicker2.Value.Date.ToString(Database.dformat) + "') ORDER BY VOUCHERINFOs.Vnumber DESC";
            }
            else if (str == "GRByChallan")
            {

                sql = "SELECT  CONVERT(nvarchar, VOUCHERINFOs_1.Vdate, 106) AS Vdate, VOUCHERINFOs_1.Vnumber, VOUCHERINFOs_1.Invoiceno AS GRNo,   VOUCHERINFOs_1.Totalamount AS Amount, VOUCHERINFOs_1.Vi_id,   USERs_1.UserName AS CreatedBy, USERs.UserName AS ModifiedBy, VOUCHERINFOs_1.modify_date, VOUCHERINFOs_1.ModTime FROM VOUCHERTYPEs RIGHT OUTER JOIN  Voucherdets RIGHT OUTER JOIN  USERs AS USERs_1 RIGHT OUTER JOIN  VOUCHERINFOs AS VOUCHERINFOs_1 LEFT OUTER JOIN  USERs ON VOUCHERINFOs_1.modifyby_id = USERs.u_id ON USERs_1.u_id = VOUCHERINFOs_1.user_id ON  Voucherdets.Vi_id = VOUCHERINFOs_1.Vi_id ON VOUCHERTYPEs.Vt_id = VOUCHERINFOs_1.Vt_id FULL OUTER JOIN DeliveryPoints AS DeliveryPoints_1 RIGHT OUTER JOIN VOUCHERINFOs ON DeliveryPoints_1.DPId = VOUCHERINFOs.Consigner_id LEFT OUTER JOIN  DeliveryPoints ON VOUCHERINFOs.SId = DeliveryPoints.DPId ON VOUCHERINFOs_1.Grno = VOUCHERINFOs.Vi_id FULL OUTER JOIN  ACCOUNTs AS ACCOUNTs_1 ON VOUCHERINFOs.Ac_id2 = ACCOUNTs_1.ac_id WHERE (VOUCHERINFOs_1.LocationId = '" + Database.LocationId + "') GROUP BY VOUCHERTYPEs.Type, VOUCHERINFOs_1.Vdate, VOUCHERINFOs_1.Vnumber, VOUCHERINFOs_1.Invoiceno, VOUCHERINFOs_1.Totalamount,   ACCOUNTs_1.name, DeliveryPoints_1.Name, VOUCHERINFOs_1.Vi_id, USERs_1.UserName, USERs.UserName, VOUCHERINFOs_1.modify_date,   VOUCHERINFOs_1.ModTime HAVING ( VOUCHERTYPEs.Type = 'GRByChallan') AND (VOUCHERINFOs_1.Vdate >= '" + dateTimePicker1.Value.Date.ToString(Database.dformat) + "') AND (VOUCHERINFOs_1.Vdate <= '" + dateTimePicker2.Value.Date.ToString(Database.dformat) + "') ORDER BY Vdate DESC, VOUCHERINFOs_1.Vnumber DESC";
            
            }
            else if (str == "Receipt")
            {

                sql = "SELECT CONVERT(nvarchar, VOUCHERINFOs.Vdate, 106) AS VDate, VOUCHERTYPEs.Name, VOUCHERINFOs.Vnumber,   ACCOUNTs.name AS Acc_Name, ACCOUNTs_1.name AS Party, VOUCHERINFOs.Totalamount AS Amount, VOUCHERINFOs.Vi_id,   USERs_1.UserName AS CreatedBy, USERs.UserName AS ModifiedBy, CONVERT(nvarchar, VOUCHERINFOs.modify_date, 106) as modify_date, VOUCHERINFOs.ModTime as Modify_time FROM VOUCHERINFOs LEFT OUTER JOIN  USERs ON VOUCHERINFOs.modifyby_id = USERs.u_id LEFT OUTER JOIN  USERs AS USERs_1 ON VOUCHERINFOs.user_id = USERs_1.u_id LEFT OUTER JOIN  VOUCHERTYPEs ON VOUCHERINFOs.Vt_id = VOUCHERTYPEs.Vt_id LEFT OUTER JOIN ACCOUNTs ON VOUCHERINFOs.Ac_id = ACCOUNTs.ac_id LEFT OUTER JOIN  ACCOUNTs AS ACCOUNTs_1 ON VOUCHERINFOs.Cr_ac_id = ACCOUNTs_1.ac_id WHERE ( VOUCHERINFOs.Vdate >= '" + dateTimePicker1.Value.Date.ToString(Database.dformat) + "') AND ( VOUCHERINFOs.Vdate <= '" + dateTimePicker2.Value.Date.ToString(Database.dformat) + "') AND ( VOUCHERTYPEs.Type = 'Receipt') AND (VOUCHERINFOs.LocationId = '" + Database.LocationId + "') ORDER BY VOUCHERINFOs.Vdate DESC, VOUCHERTYPEs.Name, VOUCHERINFOs.Vnumber DESC";

            }
            else if (str == "Payment")
            {

                sql = "SELECT CONVERT(nvarchar, VOUCHERINFOs.Vdate, 106) AS VDate, VOUCHERTYPEs.Name, VOUCHERINFOs.Vnumber, ACCOUNTs.name AS Acc_Name,  ACCOUNTs_1.name AS Party,  VOUCHERINFOs.Totalamount AS Amount, VOUCHERINFOs.Vi_id,   USERs_1.UserName AS CreatedBy, USERs.UserName AS ModifiedBy, CONVERT(nvarchar, VOUCHERINFOs.modify_date, 106) as modify_date, VOUCHERINFOs.ModTime as Modify_time FROM VOUCHERINFOs LEFT OUTER JOIN  USERs ON VOUCHERINFOs.modifyby_id = USERs.u_id LEFT OUTER JOIN  USERs AS USERs_1 ON VOUCHERINFOs.user_id = USERs_1.u_id LEFT OUTER JOIN  VOUCHERTYPEs ON VOUCHERINFOs.Vt_id = VOUCHERTYPEs.Vt_id LEFT OUTER JOIN ACCOUNTs ON VOUCHERINFOs.Ac_id = ACCOUNTs.ac_id LEFT OUTER JOIN  ACCOUNTs AS ACCOUNTs_1 ON VOUCHERINFOs.Cr_ac_id = ACCOUNTs_1.ac_id WHERE ( VOUCHERINFOs.Vdate >= '" + dateTimePicker1.Value.Date.ToString(Database.dformat) + "') AND ( VOUCHERINFOs.Vdate <= '" + dateTimePicker2.Value.Date.ToString(Database.dformat) + "') AND ( VOUCHERTYPEs.Type = 'Payment') AND (VOUCHERINFOs.LocationId = '" + Database.LocationId + "') ORDER BY VOUCHERINFOs.Vdate DESC, VOUCHERTYPEs.Name, VOUCHERINFOs.Vnumber DESC";

            }
            else if (str == "Contra")
            {

                sql = "SELECT CONVERT(nvarchar, VOUCHERINFOs.Vdate, 106) AS VDate, VOUCHERTYPEs.Name, VOUCHERINFOs.Vnumber, ACCOUNTs.name AS Acc_Name, VOUCHERINFOs.Totalamount AS Amount, VOUCHERINFOs.Vi_id,   USERs_1.UserName AS CreatedBy, USERs.UserName AS ModifiedBy,CONVERT(nvarchar, VOUCHERINFOs.modify_date, 106) as modify_date, VOUCHERINFOs.ModTime as Modify_time FROM VOUCHERINFOs LEFT OUTER JOIN  USERs ON VOUCHERINFOs.modifyby_id = USERs.u_id LEFT OUTER JOIN  USERs AS USERs_1 ON VOUCHERINFOs.user_id = USERs_1.u_id LEFT OUTER JOIN  VOUCHERTYPEs ON VOUCHERINFOs.Vt_id = VOUCHERTYPEs.Vt_id LEFT OUTER JOIN ACCOUNTs ON VOUCHERINFOs.Ac_id = ACCOUNTs.ac_id WHERE ( VOUCHERINFOs.Vdate >= '" + dateTimePicker1.Value.Date.ToString(Database.dformat) + "') AND ( VOUCHERINFOs.Vdate <= '" + dateTimePicker2.Value.Date.ToString(Database.dformat) + "') AND ( VOUCHERTYPEs.Type = 'Contra') AND (VOUCHERINFOs.LocationId = '" + Database.LocationId + "') ORDER BY VOUCHERINFOs.Vdate DESC, VOUCHERTYPEs.Name, VOUCHERINFOs.Vnumber DESC";

            }
            else if (str == "Journal")
            {

                sql = "SELECT CONVERT(nvarchar, VOUCHERINFOs.Vdate, 106) AS VDate, VOUCHERTYPEs.Name, VOUCHERINFOs.Vnumber,  VOUCHERINFOs.Totalamount AS Amount, VOUCHERINFOs.Vi_id,   USERs_1.UserName AS CreatedBy, USERs.UserName AS ModifiedBy, CONVERT(nvarchar, VOUCHERINFOs.modify_date, 106) as modify_date, VOUCHERINFOs.ModTime as Modify_time FROM VOUCHERINFOs LEFT OUTER JOIN  USERs ON VOUCHERINFOs.modifyby_id = USERs.u_id LEFT OUTER JOIN  USERs AS USERs_1 ON VOUCHERINFOs.user_id = USERs_1.u_id LEFT OUTER JOIN  VOUCHERTYPEs ON VOUCHERINFOs.Vt_id = VOUCHERTYPEs.Vt_id WHERE ( VOUCHERINFOs.Vdate >= '" + dateTimePicker1.Value.Date.ToString(Database.dformat) + "') AND ( VOUCHERINFOs.Vdate <= '" + dateTimePicker2.Value.Date.ToString(Database.dformat) + "') AND ( VOUCHERTYPEs.Type = '" + str + "') AND (VOUCHERINFOs.LocationId = '" + Database.LocationId + "') ORDER BY VOUCHERINFOs.Vdate DESC, VOUCHERTYPEs.Name, VOUCHERINFOs.Vnumber DESC";

            }
            else if (str == "Delivery")
            {

              
              //  sql = "SELECT  CONVERT(nvarchar, VOUCHERINFOs_1.Vdate, 106) AS Vdate, VOUCHERINFOs_1.Vnumber, Stocks.GRNo, ACCOUNTs.name AS Consigner,  DeliveryPoints.Name AS Source, ACCOUNTs_1.name AS Consignee, DeliveryPoints_1.Name AS Destination, VOUCHERINFOs_1.Totalamount AS Amount,  VOUCHERINFOs_1.Vi_id, USERs_1.UserName AS CreatedBy, USERs.UserName AS ModifiedBy, CONVERT(nvarchar, VOUCHERINFOs_1.modify_date, 106) as modify_date,  VOUCHERINFOs_1.ModTime FROM VOUCHERTYPEs RIGHT OUTER JOIN  VOUCHERINFOs AS VOUCHERINFOs_1 LEFT OUTER JOIN  USERs ON VOUCHERINFOs_1.modifyby_id = USERs.u_id LEFT OUTER JOIN  USERs AS USERs_1 ON VOUCHERINFOs_1.user_id = USERs_1.u_id ON VOUCHERTYPEs.Vt_id = VOUCHERINFOs_1.Vt_id FULL OUTER JOIN  DeliveryPoints AS DeliveryPoints_1 RIGHT OUTER JOIN  ACCOUNTs AS ACCOUNTs_1 RIGHT OUTER JOIN  ACCOUNTs RIGHT OUTER JOIN DeliveryPoints RIGHT OUTER JOIN  Stocks ON DeliveryPoints.DPId = Stocks.Source_id ON ACCOUNTs.ac_id = Stocks.Consigner_id ON   ACCOUNTs_1.ac_id = Stocks.Consignee_id ON DeliveryPoints_1.DPId = Stocks.Destination_id FULL OUTER JOIN Voucherdets ON Stocks.vid = Voucherdets.Vi_id ON VOUCHERINFOs_1.Vi_id = Voucherdets.Vi_id WHERE (VOUCHERINFOs_1.LocationId = '" + Database.LocationId + "') GROUP BY VOUCHERTYPEs.Type, VOUCHERINFOs_1.Vdate, VOUCHERINFOs_1.Vnumber, VOUCHERINFOs_1.Totalamount, ACCOUNTs_1.name, DeliveryPoints_1.Name,   VOUCHERINFOs_1.Vi_id, USERs_1.UserName, Stocks.GRNo, USERs.UserName, VOUCHERINFOs_1.modify_date, VOUCHERINFOs_1.ModTime,  ACCOUNTs.name, DeliveryPoints.Name HAVING ( VOUCHERTYPEs.Type = 'Delivery') AND (VOUCHERINFOs_1.Vdate >= '" + dateTimePicker1.Value.Date.ToString(Database.dformat) + "') AND (VOUCHERINFOs_1.Vdate <= '" + dateTimePicker2.Value.Date.ToString(Database.dformat) + "') ORDER BY Vdate DESC, VOUCHERINFOs_1.Vnumber DESC";
                sql = "SELECT CONVERT(nvarchar, VOUCHERINFOs_1.Vdate, 106) AS Vdate, VOUCHERINFOs_1.Vnumber, Stocks.GRNo, ACCOUNTs.name AS Consigner,  DeliveryPoints.Name AS Source, ACCOUNTs_1.name AS Consignee, DeliveryPoints_1.Name AS Destination, VOUCHERINFOs_1.Totalamount AS Amount,   VOUCHERINFOs_1.Vi_id, USERs_1.UserName AS CreatedBy, USERs.UserName AS ModifiedBy, CONVERT(nvarchar, VOUCHERINFOs_1.modify_date, 106)   AS modify_date, VOUCHERINFOs_1.ModTime FROM DeliveryPoints AS DeliveryPoints_1 RIGHT OUTER JOIN  ACCOUNTs AS ACCOUNTs_1 RIGHT OUTER JOIN  ACCOUNTs RIGHT OUTER JOIN  VOUCHERTYPEs RIGHT OUTER JOIN  VOUCHERINFOs AS VOUCHERINFOs_1 LEFT OUTER JOIN  Stocks ON VOUCHERINFOs_1.Grno = Stocks.GR_id LEFT OUTER JOIN  USERs ON VOUCHERINFOs_1.modifyby_id = USERs.u_id LEFT OUTER JOIN  USERs AS USERs_1 ON VOUCHERINFOs_1.user_id = USERs_1.u_id ON VOUCHERTYPEs.Vt_id = VOUCHERINFOs_1.Vt_id LEFT OUTER JOIN  DeliveryPoints ON Stocks.Source_id = DeliveryPoints.DPId ON ACCOUNTs.ac_id = Stocks.Consigner_id ON   ACCOUNTs_1.ac_id = Stocks.Consignee_id ON DeliveryPoints_1.DPId = Stocks.Destination_id WHERE (VOUCHERINFOs_1.LocationId = '" + Database.LocationId + "') GROUP BY VOUCHERTYPEs.Type, VOUCHERINFOs_1.Vdate, VOUCHERINFOs_1.Vnumber, VOUCHERINFOs_1.Totalamount, ACCOUNTs_1.name, DeliveryPoints_1.Name,   VOUCHERINFOs_1.Vi_id, USERs_1.UserName, Stocks.GRNo, USERs.UserName, VOUCHERINFOs_1.modify_date, VOUCHERINFOs_1.ModTime,   ACCOUNTs.name, DeliveryPoints.Name HAVING ( VOUCHERTYPEs.Type = 'Delivery') AND (VOUCHERINFOs_1.Vdate >= '" + dateTimePicker1.Value.Date.ToString(Database.dformat) + "') AND (VOUCHERINFOs_1.Vdate <= '" + dateTimePicker2.Value.Date.ToString(Database.dformat) + "') ORDER BY Vdate DESC, VOUCHERINFOs_1.Vnumber DESC ";
            }
            else if (str == "DBill")
            {

                sql = "SELECT CONVERT(nvarchar, VOUCHERINFOs_1.Vdate, 106) AS Vdate, VOUCHERINFOs_1.Vnumber, ACCOUNTs.name,  VOUCHERINFOs_1.Totalamount AS Amount, VOUCHERINFOs_1.Vi_id, USERs_1.UserName AS CreatedBy, USERs.UserName AS ModifiedBy, CONVERT(nvarchar,   VOUCHERINFOs_1.modify_date, 106) AS modify_date, VOUCHERINFOs_1.ModTime FROM VOUCHERTYPEs RIGHT OUTER JOIN  USERs AS USERs_1 RIGHT OUTER JOIN  VOUCHERINFOs AS VOUCHERINFOs_1 LEFT OUTER JOIN  ACCOUNTs ON VOUCHERINFOs_1.Ac_id = ACCOUNTs.ac_id LEFT OUTER JOIN  USERs ON VOUCHERINFOs_1.modifyby_id = USERs.u_id ON USERs_1.u_id = VOUCHERINFOs_1.user_id ON   VOUCHERTYPEs.Vt_id = VOUCHERINFOs_1.Vt_id WHERE (VOUCHERINFOs_1.LocationId = '"+Database.LocationId+"') GROUP BY VOUCHERTYPEs.Type, VOUCHERINFOs_1.Vdate, VOUCHERINFOs_1.Vnumber, VOUCHERINFOs_1.Totalamount, VOUCHERINFOs_1.Vi_id, USERs_1.UserName,  USERs.UserName, VOUCHERINFOs_1.modify_date, VOUCHERINFOs_1.ModTime, ACCOUNTs.name HAVING ( VOUCHERTYPEs.Type = 'DBill') AND (VOUCHERINFOs_1.Vdate >= '" + dateTimePicker1.Value.Date.ToString(Database.dformat) + "') AND (VOUCHERINFOs_1.Vdate <= '" + dateTimePicker2.Value.Date.ToString(Database.dformat) + "') ORDER BY Vdate DESC, VOUCHERINFOs_1.Vnumber DESC ";
              //  sql = "SELECT  CONVERT(nvarchar, VOUCHERINFOs_1.Vdate, 106) AS Vdate, VOUCHERINFOs_1.Vnumber, Stocks.GRNo, ACCOUNTs_1.name AS Consignee, VOUCHERINFOs_1.Totalamount AS Amount,  VOUCHERINFOs_1.Vi_id, USERs_1.UserName AS CreatedBy, USERs.UserName AS ModifiedBy, CONVERT(nvarchar, VOUCHERINFOs_1.modify_date, 106) as modify_date,  VOUCHERINFOs_1.ModTime FROM VOUCHERTYPEs RIGHT OUTER JOIN  VOUCHERINFOs AS VOUCHERINFOs_1 LEFT OUTER JOIN  USERs ON VOUCHERINFOs_1.modifyby_id = USERs.u_id LEFT OUTER JOIN  USERs AS USERs_1 ON VOUCHERINFOs_1.user_id = USERs_1.u_id ON VOUCHERTYPEs.Vt_id = VOUCHERINFOs_1.Vt_id FULL OUTER JOIN  DeliveryPoints AS DeliveryPoints_1 RIGHT OUTER JOIN  ACCOUNTs AS ACCOUNTs_1 RIGHT OUTER JOIN  ACCOUNTs RIGHT OUTER JOIN DeliveryPoints RIGHT OUTER JOIN  Stocks ON DeliveryPoints.DPId = Stocks.Source_id ON ACCOUNTs.ac_id = Stocks.Consigner_id ON   ACCOUNTs_1.ac_id = Stocks.Consignee_id ON DeliveryPoints_1.DPId = Stocks.Destination_id FULL OUTER JOIN Voucherdets ON Stocks.vid = Voucherdets.Vi_id ON VOUCHERINFOs_1.Vi_id = Voucherdets.Vi_id WHERE (VOUCHERINFOs_1.LocationId = '" + Database.LocationId + "') GROUP BY VOUCHERTYPEs.Type, VOUCHERINFOs_1.Vdate, VOUCHERINFOs_1.Vnumber, VOUCHERINFOs_1.Totalamount, ACCOUNTs_1.name, DeliveryPoints_1.Name,   VOUCHERINFOs_1.Vi_id, USERs_1.UserName, Stocks.GRNo, USERs.UserName, VOUCHERINFOs_1.modify_date, VOUCHERINFOs_1.ModTime,  ACCOUNTs.name, DeliveryPoints.Name HAVING ( VOUCHERTYPEs.Type = 'DBill') AND (VOUCHERINFOs_1.Vdate >= '" + dateTimePicker1.Value.Date.ToString(Database.dformat) + "') AND (VOUCHERINFOs_1.Vdate <= '" + dateTimePicker2.Value.Date.ToString(Database.dformat) + "') ORDER BY Vdate DESC, VOUCHERINFOs_1.Vnumber DESC";

            }
            else
            {
              
                sql = "SELECT  CONVERT(nvarchar, VOUCHERINFOs_1.Vdate, 106) AS Vdate, VOUCHERINFOs_1.Vnumber, VOUCHERINFOs.Invoiceno AS GRNo,   ACCOUNTs_1.name AS Consignee, DeliveryPoints_1.Name AS Destination, VOUCHERINFOs_1.Totalamount AS Amount, VOUCHERINFOs_1.Vi_id,   USERs_1.UserName AS CreatedBy, USERs.UserName AS ModifiedBy, VOUCHERINFOs_1.modify_date, VOUCHERINFOs_1.ModTime FROM VOUCHERTYPEs RIGHT OUTER JOIN  Voucherdets RIGHT OUTER JOIN  USERs AS USERs_1 RIGHT OUTER JOIN  VOUCHERINFOs AS VOUCHERINFOs_1 LEFT OUTER JOIN  USERs ON VOUCHERINFOs_1.modifyby_id = USERs.u_id ON USERs_1.u_id = VOUCHERINFOs_1.user_id ON  Voucherdets.Vi_id = VOUCHERINFOs_1.Vi_id ON VOUCHERTYPEs.Vt_id = VOUCHERINFOs_1.Vt_id FULL OUTER JOIN DeliveryPoints AS DeliveryPoints_1 RIGHT OUTER JOIN VOUCHERINFOs ON DeliveryPoints_1.DPId = VOUCHERINFOs.Consigner_id LEFT OUTER JOIN  DeliveryPoints ON VOUCHERINFOs.SId = DeliveryPoints.DPId ON VOUCHERINFOs_1.Grno = VOUCHERINFOs.Vi_id FULL OUTER JOIN  ACCOUNTs AS ACCOUNTs_1 ON VOUCHERINFOs.Ac_id = ACCOUNTs_1.ac_id WHERE (VOUCHERINFOs_1.LocationId = '"+Database.LocationId+"') GROUP BY VOUCHERTYPEs.Type, VOUCHERINFOs_1.Vdate, VOUCHERINFOs_1.Vnumber, VOUCHERINFOs.Invoiceno, VOUCHERINFOs_1.Totalamount,   ACCOUNTs_1.name, DeliveryPoints_1.Name, VOUCHERINFOs_1.Vi_id, USERs_1.UserName, USERs.UserName, VOUCHERINFOs_1.modify_date,   VOUCHERINFOs_1.ModTime HAVING ( VOUCHERTYPEs.Type = 'Delivery') AND (VOUCHERINFOs_1.Vdate >= '" + dateTimePicker1.Value.Date.ToString(Database.dformat) + "') AND (VOUCHERINFOs_1.Vdate <= '" + dateTimePicker2.Value.Date.ToString(Database.dformat) + "') ORDER BY Vdate DESC, VOUCHERINFOs_1.Vnumber DESC";
            
            }

            Database.GetSqlData(sql, dtvou);
            SideFill();
            double total = 0;
            for (int i = 0; i < dtvou.Rows.Count; i++)
            {

                total = total + double.Parse(dtvou.Rows[i]["Amount"].ToString());
                if (dtvou.Columns["Amount"].DataType.Name == "Decimal")
                {
                    dtvou.Rows[i]["Amount"] = funs.DecimalPoint(double.Parse(dtvou.Rows[i]["Amount"].ToString()));
                }
            }


            label1.Text = funs.IndianCurr(total);
            ansGridView5.DataSource = dtvou;
         //   label1.Text = funs.IndianCurr(total);

            if (str == "Booking")
            {
                ansGridView5.Columns["Vdate"].DisplayIndex = 0;
                ansGridView5.Columns["Name"].DisplayIndex = 1;
                ansGridView5.Columns["Vnumber"].DisplayIndex = 2;
                ansGridView5.Columns["consigner"].DisplayIndex = 3;
                ansGridView5.Columns["consignee"].DisplayIndex = 4;
                ansGridView5.Columns["Amount"].DisplayIndex = 5;
                ansGridView5.Columns["CreatedBy"].DisplayIndex = 6;
                ansGridView5.Columns["ModifiedBy"].DisplayIndex = 7;
                ansGridView5.Columns["Modify_date"].DisplayIndex = 8;
                ansGridView5.Columns["Modify_time"].DisplayIndex = 9;
                ansGridView5.Columns["view"].DisplayIndex = 10;
                ansGridView5.Columns["print"].DisplayIndex = 11;
                ansGridView5.Columns["Edit"].DisplayIndex = 12;
                ansGridView5.Columns["Delet"].DisplayIndex = 13;
                ansGridView5.Columns["Vi_id"].DisplayIndex = 14;
                ansGridView5.Columns["Modify_time"].HeaderText = "Time";
                ansGridView5.Columns["PrintCount"].Visible = false;
                ansGridView5.Columns["Modify_date"].HeaderText = "Date";
            }
            else if(str=="Stock Transfer")
            {
                ansGridView5.Columns["Vdate"].DisplayIndex = 0;
                ansGridView5.Columns["Name"].DisplayIndex = 1;
                ansGridView5.Columns["Vnumber"].DisplayIndex = 2;
                ansGridView5.Columns["Gaddi_name"].DisplayIndex = 3;
                ansGridView5.Columns["Driver"].DisplayIndex = 4;
                ansGridView5.Columns["source"].DisplayIndex = 5;
                ansGridView5.Columns["destination"].DisplayIndex = 6;
                ansGridView5.Columns["Amount"].DisplayIndex = 7;
                ansGridView5.Columns["CreatedBy"].DisplayIndex = 8;
                ansGridView5.Columns["ModifiedBy"].DisplayIndex = 9;
                ansGridView5.Columns["Modify_date"].DisplayIndex = 10;
                ansGridView5.Columns["Modify_time"].DisplayIndex = 11;
                ansGridView5.Columns["view"].DisplayIndex = 12;
                ansGridView5.Columns["print"].DisplayIndex = 13;
                ansGridView5.Columns["Edit"].DisplayIndex = 14;
                ansGridView5.Columns["Delet"].DisplayIndex = 15;
                ansGridView5.Columns["Vi_id"].DisplayIndex = 16;
                ansGridView5.Columns["Modify_time"].HeaderText = "Time";

                ansGridView5.Columns["PrintCount"].Visible = false;
                ansGridView5.Columns["Modify_date"].HeaderText = "Date";
            }
            else if (str == "Challan")
            {
                ansGridView5.Columns["Vdate"].DisplayIndex = 0;
                ansGridView5.Columns["Name"].DisplayIndex = 1;
                ansGridView5.Columns["Vnumber"].DisplayIndex = 2;
                ansGridView5.Columns["Gaddi_name"].DisplayIndex = 3;
                ansGridView5.Columns["Driver"].DisplayIndex = 4;
                ansGridView5.Columns["source"].DisplayIndex = 5;
                ansGridView5.Columns["destination"].DisplayIndex = 6;
                ansGridView5.Columns["Amount"].DisplayIndex = 7;
                ansGridView5.Columns["CreatedBy"].DisplayIndex = 8;
                ansGridView5.Columns["ModifiedBy"].DisplayIndex = 9;
                ansGridView5.Columns["Modify_date"].DisplayIndex = 10;
                ansGridView5.Columns["Modify_time"].DisplayIndex = 11;
                ansGridView5.Columns["view"].DisplayIndex = 12;
                ansGridView5.Columns["print"].DisplayIndex = 13;
                ansGridView5.Columns["Edit"].DisplayIndex = 14;
                ansGridView5.Columns["Delet"].DisplayIndex = 15;
                ansGridView5.Columns["Vi_id"].DisplayIndex = 16;
                ansGridView5.Columns["Modify_time"].HeaderText = "Time";

                ansGridView5.Columns["PrintCount"].Visible = false;
                ansGridView5.Columns["Modify_date"].HeaderText = "Date";
            }
            else if (str == "Unloading")
            {
                ansGridView5.Columns["Vdate"].DisplayIndex = 0;
                ansGridView5.Columns["Name"].DisplayIndex = 1;
                ansGridView5.Columns["Vnumber"].DisplayIndex = 2;
                ansGridView5.Columns["Challan_no"].DisplayIndex = 3;
              
                ansGridView5.Columns["Amount"].DisplayIndex = 4;
                ansGridView5.Columns["CreatedBy"].DisplayIndex = 5;
                ansGridView5.Columns["ModifiedBy"].DisplayIndex = 6;
                ansGridView5.Columns["Modify_date"].DisplayIndex = 7;
                ansGridView5.Columns["Modify_time"].DisplayIndex = 8;
                ansGridView5.Columns["view"].DisplayIndex = 9;
                ansGridView5.Columns["print"].DisplayIndex = 10;
                ansGridView5.Columns["Edit"].DisplayIndex = 11;
                ansGridView5.Columns["Delet"].DisplayIndex = 12;
                ansGridView5.Columns["Vi_id"].DisplayIndex = 13;
                ansGridView5.Columns["Modify_date"].HeaderText = "Date";
                ansGridView5.Columns["PrintCount"].Visible = false;
            }
            else if (str == "GRByChallan")
            {
                ansGridView5.Columns["Vdate"].DisplayIndex = 0;
                ansGridView5.Columns["Vnumber"].DisplayIndex = 1;
                ansGridView5.Columns["GRNo"].DisplayIndex = 2;
                ansGridView5.Columns["Amount"].DisplayIndex = 3;
                ansGridView5.Columns["CreatedBy"].DisplayIndex = 4;
                ansGridView5.Columns["ModifiedBy"].DisplayIndex = 5;
                ansGridView5.Columns["Modify_date"].DisplayIndex = 6;
                ansGridView5.Columns["Modtime"].DisplayIndex = 7;
                ansGridView5.Columns["view"].DisplayIndex = 8;
                ansGridView5.Columns["print"].DisplayIndex = 9;
                ansGridView5.Columns["Edit"].DisplayIndex = 10;
                ansGridView5.Columns["Delet"].DisplayIndex = 11;
                ansGridView5.Columns["Vi_id"].DisplayIndex = 12;
                ansGridView5.Columns["Modify_date"].HeaderText = "Date";
                //  ansGridView5.Columns["PrintCount"].Visible = false;
            }
            else if (str == "Delivery")
            {
                ansGridView5.Columns["Vdate"].DisplayIndex = 0;
                ansGridView5.Columns["Vnumber"].DisplayIndex = 1;
                ansGridView5.Columns["GRNo"].DisplayIndex = 2;
                ansGridView5.Columns["Consigner"].DisplayIndex = 3;
                ansGridView5.Columns["Source"].DisplayIndex = 4;
                ansGridView5.Columns["Consignee"].DisplayIndex = 5;
                ansGridView5.Columns["Destination"].DisplayIndex = 6;
                ansGridView5.Columns["Amount"].DisplayIndex = 7;
                ansGridView5.Columns["CreatedBy"].DisplayIndex = 8;
                ansGridView5.Columns["ModifiedBy"].DisplayIndex = 9;
                ansGridView5.Columns["Modify_date"].DisplayIndex = 10;
                ansGridView5.Columns["Modtime"].DisplayIndex = 11;
                ansGridView5.Columns["view"].DisplayIndex = 12;
                ansGridView5.Columns["print"].DisplayIndex = 13;
                ansGridView5.Columns["Edit"].DisplayIndex = 14;
                ansGridView5.Columns["Delet"].DisplayIndex = 15;
                ansGridView5.Columns["Vi_id"].DisplayIndex = 16;
                ansGridView5.Columns["Modify_date"].HeaderText = "Date";
            }

            else if (str == "Receipt" || str == "Payment")
            {
                ansGridView5.Columns["Vdate"].DisplayIndex = 0;
                ansGridView5.Columns["Name"].DisplayIndex = 1;
                ansGridView5.Columns["Vnumber"].DisplayIndex = 2;
                ansGridView5.Columns["Acc_Name"].DisplayIndex = 3;
                ansGridView5.Columns["Party"].DisplayIndex = 4;
                ansGridView5.Columns["Amount"].DisplayIndex = 5;
                ansGridView5.Columns["CreatedBy"].DisplayIndex = 6;
                ansGridView5.Columns["ModifiedBy"].DisplayIndex = 7;
                ansGridView5.Columns["Modify_date"].DisplayIndex = 8;
                ansGridView5.Columns["Modify_time"].DisplayIndex = 9;
                ansGridView5.Columns["view"].DisplayIndex = 10;
                ansGridView5.Columns["print"].DisplayIndex = 11;
                ansGridView5.Columns["Edit"].DisplayIndex = 12;
                ansGridView5.Columns["Delet"].DisplayIndex = 13;
                ansGridView5.Columns["Vi_id"].DisplayIndex = 14;

                ansGridView5.Columns["Modify_time"].HeaderText = "Time";

                //ansGridView5.Columns["PrintCount"].Visible = false;
            }
            else if (str == "Contra")
            {
                ansGridView5.Columns["Vdate"].DisplayIndex = 0;
                ansGridView5.Columns["Name"].DisplayIndex = 1;
                ansGridView5.Columns["Vnumber"].DisplayIndex = 2;
                ansGridView5.Columns["Acc_Name"].DisplayIndex = 3;

                ansGridView5.Columns["Amount"].DisplayIndex = 4;
                ansGridView5.Columns["CreatedBy"].DisplayIndex = 5;
                ansGridView5.Columns["ModifiedBy"].DisplayIndex = 6;
                ansGridView5.Columns["Modify_date"].DisplayIndex = 7;
                ansGridView5.Columns["Modify_time"].DisplayIndex = 8;
                ansGridView5.Columns["view"].DisplayIndex = 9;
                ansGridView5.Columns["print"].DisplayIndex = 10;
                ansGridView5.Columns["Edit"].DisplayIndex = 11;
                ansGridView5.Columns["Delet"].DisplayIndex = 12;
                ansGridView5.Columns["Vi_id"].DisplayIndex = 13;

                ansGridView5.Columns["Modify_time"].HeaderText = "Time";

                //ansGridView5.Columns["PrintCount"].Visible = false;
            }
            else if (str == "Journal")
            {
                ansGridView5.Columns["Vdate"].DisplayIndex = 0;
                ansGridView5.Columns["Name"].DisplayIndex = 1;
                ansGridView5.Columns["Vnumber"].DisplayIndex = 2;
               
                ansGridView5.Columns["Amount"].DisplayIndex = 3;
                ansGridView5.Columns["CreatedBy"].DisplayIndex = 4;
                ansGridView5.Columns["ModifiedBy"].DisplayIndex = 5;
                ansGridView5.Columns["Modify_date"].DisplayIndex = 6;
                ansGridView5.Columns["Modify_time"].DisplayIndex = 7;
                ansGridView5.Columns["view"].DisplayIndex = 8;
                ansGridView5.Columns["print"].DisplayIndex = 9;
                ansGridView5.Columns["Edit"].DisplayIndex = 10;
                ansGridView5.Columns["Delet"].DisplayIndex = 11;
                ansGridView5.Columns["Vi_id"].DisplayIndex = 12;

                ansGridView5.Columns["Modify_time"].HeaderText = "Time";

                //ansGridView5.Columns["PrintCount"].Visible = false;
            }
            else if (str == "DBill")
            {
                ansGridView5.Columns["Vdate"].DisplayIndex = 0;
           
                ansGridView5.Columns["Vnumber"].DisplayIndex = 1;
                ansGridView5.Columns["Name"].DisplayIndex = 2;
                ansGridView5.Columns["Amount"].DisplayIndex = 3;
                ansGridView5.Columns["CreatedBy"].DisplayIndex = 4;
                ansGridView5.Columns["ModifiedBy"].DisplayIndex = 5;
                ansGridView5.Columns["Modify_date"].DisplayIndex = 6;
               // ansGridView5.Columns["Modify_time"].DisplayIndex = 7;
                ansGridView5.Columns["view"].DisplayIndex = 8;
                ansGridView5.Columns["print"].DisplayIndex = 9;
                ansGridView5.Columns["Edit"].DisplayIndex = 10;
                ansGridView5.Columns["Delet"].DisplayIndex = 11;
                ansGridView5.Columns["Vi_id"].DisplayIndex = 12;

              // ansGridView5.Columns["Modify_time"].HeaderText = "Time";

                //ansGridView5.Columns["PrintCount"].Visible = false;
            }
            else
            {
                ansGridView5.Columns["Vdate"].DisplayIndex = 0;
                ansGridView5.Columns["Name"].DisplayIndex = 1;
                ansGridView5.Columns["Vnumber"].DisplayIndex = 2;
                ansGridView5.Columns["Acc_name"].DisplayIndex = 3;
                ansGridView5.Columns["Amount"].DisplayIndex = 5;
                ansGridView5.Columns["CreatedBy"].DisplayIndex = 6;
                ansGridView5.Columns["ModifiedBy"].DisplayIndex = 7;
                ansGridView5.Columns["Modify_date"].DisplayIndex = 8;
                ansGridView5.Columns["Modify_time"].DisplayIndex = 9;
                ansGridView5.Columns["view"].DisplayIndex = 10;
                ansGridView5.Columns["print"].DisplayIndex = 11;
                ansGridView5.Columns["Edit"].DisplayIndex = 12;
                ansGridView5.Columns["Delet"].DisplayIndex = 13;
                ansGridView5.Columns["Vi_id"].DisplayIndex = 14;
                ansGridView5.Columns["Modify_time"].HeaderText = "Time";

                ansGridView5.Columns["PrintCount"].Visible = false;
                ansGridView5.Columns["Modify_date"].HeaderText = "Date";
            }

            
           
            ansGridView5.Columns["Vi_id"].Visible = false;
            ansGridView5.Columns["Entered"].Visible = false;
           

            for (int i = 0; i < dtvou.Columns.Count; i++)
            {
                if (dtvou.Columns[i].DataType.Name == "Decimal")
                {
                    ansGridView5.Columns[dtvou.Columns[i].ColumnName].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    ansGridView5.Columns[dtvou.Columns[i].ColumnName].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
                if (dtvou.Columns[i].DataType.Name == "Int32")
                {
                    ansGridView5.Columns[dtvou.Columns[i].ColumnName].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    ansGridView5.Columns[dtvou.Columns[i].ColumnName].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                if (dtvou.Columns[i].DataType.Name == "Double")
                {
                    ansGridView5.Columns[dtvou.Columns[i].ColumnName].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    ansGridView5.Columns[dtvou.Columns[i].ColumnName].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
            }

            if (Database.utype == "User")
            {
                ansGridView5.Columns["delet"].Visible = false;
            }
            this.Text = gFrmCaption;
           
        }

        private void SideFill()
        {
            flowLayoutPanel1.Controls.Clear();
            DataTable dtsidefill = new DataTable();
            dtsidefill.Columns.Add("Name", typeof(string));
            dtsidefill.Columns.Add("DisplayName", typeof(string));
            dtsidefill.Columns.Add("ShortcutKey", typeof(string));
            dtsidefill.Columns.Add("Visible", typeof(bool));

            //createnew
            dtsidefill.Rows.Add();
            dtsidefill.Rows[0]["Name"] = "add";
            dtsidefill.Rows[0]["DisplayName"] = "Create New";
            dtsidefill.Rows[0]["ShortcutKey"] = "^C";
            dtsidefill.Rows[0]["Visible"] = true;

            //refresh
            dtsidefill.Rows.Add();
            dtsidefill.Rows[1]["Name"] = "refresh";
            dtsidefill.Rows[1]["DisplayName"] = "Refresh";
            dtsidefill.Rows[1]["ShortcutKey"] = "^R";
            dtsidefill.Rows[1]["Visible"] = true;

            //close
            dtsidefill.Rows.Add();
            dtsidefill.Rows[2]["Name"] = "quit";
            dtsidefill.Rows[2]["DisplayName"] = "Quit";
            dtsidefill.Rows[2]["ShortcutKey"] = "Esc";
            dtsidefill.Rows[2]["Visible"] = true;

            for (int i = 0; i < dtsidefill.Rows.Count; i++)
            {
                if (bool.Parse(dtsidefill.Rows[i]["Visible"].ToString()) == true)
                {
                    Button btn = new Button();
                    btn.Size = new Size(150, 30);
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
                    System.Drawing.Rectangle RC = btn.ClientRectangle;
                    System.Drawing.Font font = new System.Drawing.Font("Arial", 12);
                    G.DrawString(line1, font, Brushes.Red, RC, SF);
                    G.DrawString("".PadLeft(line1.Length * 2 + 1) + line2, font, Brushes.Black, RC, SF);
                    btn.Image = bmp;
                    btn.Click += new EventHandler(btn_Click);
                    flowLayoutPanel1.Controls.Add(btn);
                }
            }
        }

        private void ADD()
        {
            if (gstr == "Unloading")
            {
                frm_unloading frm = new frm_unloading();
                frm.MdiParent = this.MdiParent;
                frm.LoadData("0", "Unloading");
                frm.Show();
            }
            else if (gstr == "Delivery")
            {
                frm_Delivery frm = new frm_Delivery();
                frm.MdiParent = this.MdiParent;
                frm.LoadData("0", "Delivery");
                frm.Show();
            }
            else if (gstr == "DBill")
            {
                frm_dbill frm = new frm_dbill();
                frm.MdiParent = this.MdiParent;
                frm.LoadData("0", "Delivery Bill");
                frm.Show();
            }
            else if (gstr == "Booking")
            {
                frmBooking frm = new frmBooking();
                frm.LoadData("0", "Booking");
                frm.MdiParent = this.MdiParent;
                frm.Show();
            }
            else if (gstr == "GRByChallan")
            {
                frm_newunloading frm = new frm_newunloading();
                frm.LoadData("0", "GRByChallan");
                frm.MdiParent = this.MdiParent;
                frm.Show();
            }
            else if (gstr == "Receipt")
            {
                frmCashRec frm = new frmCashRec();
                frm.recpay = "Receipt";
                frm.LoadData("0", "Receipt");
                frm.MdiParent = this.MdiParent;
                frm.Show();
            }
            else if (gstr == "Payment")
            {
                frmCashRec frm = new frmCashRec();
                frm.recpay = "Payment";
                frm.LoadData("0", "Payment");
                frm.MdiParent = this.MdiParent;
                frm.Show();
            }
            else if (gstr == "Contra")
            {
                frmCashRec frm = new frmCashRec();
                frm.recpay = "Contra";
                frm.LoadData("0", "Contra");
                frm.MdiParent = this.MdiParent;
                frm.Show();
            }
            else if (gstr == "Journal")
            {
                frmJournal frm = new frmJournal();
                frm.LoadData("0", "Journal Voucher");
                frm.MdiParent = this.MdiParent;
                frm.Show();
            }
            else if (gstr == "Stock Transfer")
            {
                frmStockTransfer frm = new frmStockTransfer();
                frm.MdiParent = this.MdiParent;
                frm.LoadData("0", "Stock Transfer");
                frm.Show();
            }
            else if (gstr == "Challan")
            {
                frm_Challan frm = new frm_Challan();
                frm.MdiParent = this.MdiParent;
                frm.LoadData("0", "Challan");
                frm.Show();
            }
            else if (gstr == "Sale")
            {
                frm_bill frm = new frm_bill();
                frm.MdiParent = this.MdiParent;
                frm.LoadData("0", "Sale");
                frm.Show();
            }
        }
        void btn_Click(object sender, EventArgs e)
        {
            Button tbtn = (Button)sender;
            string name = tbtn.Name.ToString();

            if (name == "add")
            {
                ADD();
            }
            else if (name == "refresh")
            {
                LoadData(gstr, gFrmCaption);
            }

            else if (name == "quit")
            {
                this.Close();
                this.Dispose();
            }
        }

        public void ExportToPdf(string tPath)
        {
            string str = "";

            FileStream fs = new FileStream(tPath, FileMode.Create, FileAccess.Write, FileShare.None);
            iTextSharp.text.Rectangle rec;
            Document document;
            int Twidth = 0;
            for (int i = 5; i < 10; i++)
            {
                Twidth += ansGridView5.Columns[i].Width;
            }
            if (Twidth == 2000)
            {
                document = new Document(PageSize.A4.Rotate(), 20f, 10f, 20f, 10f);
            }

            document = new Document(PageSize.A4, 20f, 10f, 20f, 10f);

            PdfWriter writer = PdfWriter.GetInstance(document, fs);
            writer.PageEvent = new MainTextEventsHandler();
            document.Open();
            HTMLWorker hw = new HTMLWorker(document);
            str = "";
            str += @"<body> <font size='1'><table border=1> <tr>";
            for (int i = 5; i < 10; i++)
            {
                string align = "";
                string bold = "";
                int width = 0;

                if (Twidth == 2000)
                {
                    width = ansGridView5.Columns[i].Width / 20;
                }
                else
                {
                    width = ansGridView5.Columns[i].Width / 10;
                }

                if (ansGridView5.Columns[i].HeaderCell.Style.Alignment == DataGridViewContentAlignment.MiddleRight)
                {
                    align = "text-align:right;";
                }

                bold = "font-weight: bold;";

                if (width != 0)
                {
                    str += "<th width=" + width + "%  style='" + align + bold + "'>" + ansGridView5.Columns[i].HeaderText.ToString() + "</th> ";
                }
            }

            str += "</tr>";

            for (int i = 0; i < ansGridView5.Rows.Count; i++)
            {
                str += "<tr> ";
                for (int j = 5; j < 10; j++)
                {
                    int width = 0;
                    if (Twidth == 2000)
                    {
                        width = ansGridView5.Rows[i].Cells[j].Size.Width / 20;
                    }
                    else
                    {
                        width = ansGridView5.Rows[i].Cells[j].Size.Width / 10;
                    }

                    if (width != 0)
                    {
                        if (ansGridView5.Rows[i].Cells[j].Value != null)
                        {
                            string align = "";
                            string bold = "";
                            string colspan = "";

                            if (ansGridView5.Columns[j].DefaultCellStyle.Alignment == DataGridViewContentAlignment.MiddleRight)
                            {
                                align = "text-align:right;";
                            }

                            if (ansGridView5.Rows[i].Cells[j].Style.Font != null && ansGridView5.Rows[i].Cells[j].Style.Font.Bold == true)
                            {
                                bold = "font-weight: bold;";
                            }

                            if (j == 0 && ansGridView5.Rows[i].Cells[0].Value.ToString() != "" && ansGridView5.Rows[i].Cells[1].Value == null && ansGridView5.Rows[i].Cells[2].Value == null)
                            {
                                colspan = "colspan= '2'";
                            }

                            if (ansGridView5.Rows[i].Cells[j].Value.ToString().Trim() == "")
                            {
                                str += "<td> &nbsp; </td>";
                            }
                            else
                            {
                                str += "<td " + colspan + "  style='" + align + bold + "'>" + ansGridView5.Rows[i].Cells[j].Value.ToString() + "</td> ";
                            }

                            if (j == 0 && ansGridView5.Rows[i].Cells[0].Value.ToString() != "" && ansGridView5.Rows[i].Cells[1].Value == null && ansGridView5.Rows[i].Cells[2].Value == null)
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

        internal class MainTextEventsHandler : PdfPageEventHelper
        {
            public override void OnStartPage(PdfWriter writer, Document document)
            {
                base.OnStartPage(writer, document);

                //bool sta = Database.GetScalarBool("select Stationary from Vouchertype where Name='" + Report.frmptyp2 + "' ");
                DataTable dtRheader = new DataTable();
                Database.GetSqlData("select * from company", dtRheader);
                PdfPTable table = new PdfPTable(1);
                PdfPCell cell = new PdfPCell();

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

                document.Add(table);
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

                cb.SetTextMatrix(530, 8);

                cb.ShowText(text);
                cb.EndText();
            }
        }

        private bool Validatedel()
        {
            if (ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString() == "0")
            {
                return false;
            }
            string vid = ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString();
            DataTable dtgrbychallan = new DataTable();


            Database.GetSqlData("SELECT     GR_id FROM Stocks WHERE  vid = '" + vid + "'", dtgrbychallan);
            for (int i = 0; i < dtgrbychallan.Rows.Count; i++)
            {

                int gridcount = Database.GetScalarInt("SELECT COUNT(*) AS Cnt  FROM Stocks LEFT OUTER JOIN  VOUCHERINFOs ON Stocks.vid = VOUCHERINFOs.Vi_id LEFT OUTER JOIN  VOUCHERTYPEs ON VOUCHERINFOs.Vt_id = VOUCHERTYPEs.Vt_id WHERE ( Stocks.GR_id = '" + dtgrbychallan.Rows[i][0].ToString() + "') AND ( VOUCHERTYPEs.Type <> 'GRByChallan')");
                if (gridcount >= 1)
                {
                    MessageBox.Show("It cann't be Deleted... It has been Dispatched");

                    return false;
                }


            }

            
           
          

           

            return true;
        }

        private void frmMasterVou_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.P)
            {

                
                LoadData(gstr, gFrmCaption);
                if (ansGridView5.Rows.Count == 0)
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

            if (e.Control && e.KeyCode == Keys.E)
            {
                LoadData(gstr, gFrmCaption);
                Excelexport();
            }

            if (e.Control && e.KeyCode == Keys.C)
            {
                ADD();
            }

            else if (e.Control && e.KeyCode == Keys.S)
            {
                if (gstr == "Challan" || gstr == "Stock Transfer" || gstr == "Booking" || gstr == "Unloading" || gstr == "Delivery" || gstr == "GRByChallan")
                {
                    InputBox box = new InputBox("Enter Password", "", true);
                    box.outStr = "";


                    box.ShowInTaskbar = false;
                    box.ShowDialog(this);

                    if (box.outStr == "admin")
                    {
                        if (Database.databaseName != "")
                        {
                            if (Database.SqlCnn.State == ConnectionState.Open)
                            {
                                Database.CloseConnection();
                            }
                            string pathbackup = Application.StartupPath + "\\System\\rs" + Database.databaseName + DateTime.Now.ToString("yyyyMMddhmmff") + ".bak";
                            Database.CommandExecutor("Backup database " + Database.databaseName + " to disk='" + pathbackup + "'");
                        }

                        for (int i = 0; i < ansGridView5.Rows.Count; i++)
                        {
                            string oid = ansGridView5.Rows[i].Cells["Vi_id"].Value.ToString();
                            funs.OpenFrm(this, oid, true);
                        }
                        LoadData(gstr, gFrmCaption);
                        MessageBox.Show("Done Successfully");
                    }

                    else
                    {
                        MessageBox.Show("Wrong Password..");
                    }
                }
            }

            else if (e.Control && e.Alt == false && e.KeyCode == Keys.R)
            {
                LoadData(gstr, gFrmCaption);
            }
            
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
                this.Dispose();
            }
        }

        private void ansGridView5_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (ansGridView5.CurrentCell == null)
            {
                return;
            }

            if (ansGridView5.CurrentCell.OwningColumn.Name == "delet")
            {
                if (Database.utype != "Admin")
                {
                    return;
                }

                if (ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString() == "0")
                {
                    return;
                }

                DialogResult res = MessageBox.Show("Are you sure?", "Confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (res == DialogResult.OK)
                {
                    if (Feature.Available("Freeze Transaction") == "No")
                    {
                        try
                        {
                            Database.BeginTran();

                            if (gstr != "GRByChallan")
                            {
                                delete(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString());
                            }
                            else
                            {
                                if (Validatedel() == true)
                                {
                                    delete(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString());
                                }
                            }
                            Database.CommitTran();
                        }
                        catch(Exception ex)
                        {
                            Database.RollbackTran();
                        }

                    }
                    else
                    {
                        DataTable dtfrz = new DataTable();
                        Database.GetSqlData("Select selected_value from Firmsetups where Features='Freeze Transaction'", dtfrz);

                        string vdate = Database.GetScalarText("Select Vdate from Voucherinfos where vi_id='" + ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString() + "'");

                        if (DateTime.Parse(vdate) > DateTime.Parse(dtfrz.Rows[0][0].ToString()))
                        {
                            if (gstr != "GRByChallan")
                            {
                                delete(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString());
                            }
                            else
                            {
                                if (Validatedel() == true)
                                {
                                    delete(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString());
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Your Voucher is Freezed");
                        }
                    }
                }
                LoadData(gstr, gFrmCaption);
            }

            else if (ansGridView5.CurrentCell.OwningColumn.Name == "print")
            {
                if (ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString() == "0")
                {
                    return;
                }
                if (gstr == "Booking")
                {
                    if (Database.printtype == "DOS")
                    {
                        DOSReport.voucherprint(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString());
                    }
                    else
                    {
                        if (Feature.Available("Ask Copies") == "No")
                        {
                            OtherReport rpt = new OtherReport();
                            DataTable dtprintcopy = new DataTable();
                            Database.GetSqlData("Select printcopy from Vouchertypes where Vt_id=" + funs.Select_vtid(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString()), dtprintcopy);
                            String[] print_option = dtprintcopy.Rows[0]["printcopy"].ToString().Split(';');

                            for (int j = 0; j < print_option.Length; j++)
                            {
                                if (print_option[j] != "")
                                {
                                    String[] defaultcopy = print_option[j].Split(',');
                                    if (bool.Parse(defaultcopy[1]) == true)
                                    {
                                        rpt.voucherprint(this, funs.Select_vtid(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString()), ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString(), defaultcopy[0], true, "Print");
                                    }
                                }
                            }

                        }
                        else
                        {
                            frm_printcopy frm = new frm_printcopy("Print", ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString(), funs.Select_vtid(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString()));
                            frm.ShowDialog();
                        }
                    }
                }
                else if (gstr == "Stock Transfer" || gstr == "Challan")
                {
                    if (Database.printtype == "DOS")
                    {
                        DOSReport.voucherprintChallan(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString());
                    }
                    else
                    {

                        if (Feature.Available("Ask Copies") == "No")
                        {
                            OtherReport rpt = new OtherReport();
                            DataTable dtprintcopy = new DataTable();
                            Database.GetSqlData("Select printcopy from Vouchertypes where Vt_id=" + funs.Select_vtid(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString()), dtprintcopy);
                            String[] print_option = dtprintcopy.Rows[0]["printcopy"].ToString().Split(';');

                            for (int j = 0; j < print_option.Length; j++)
                            {
                                if (print_option[j] != "")
                                {
                                    String[] defaultcopy = print_option[j].Split(',');
                                    if (bool.Parse(defaultcopy[1]) == true)
                                    {
                                        rpt.voucherprint(this, funs.Select_vtid(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString()), ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString(), defaultcopy[0], true, "Print");
                                    }
                                }
                            }

                        }
                        else
                        {
                            frm_printcopy frm = new frm_printcopy("Print", ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString(), funs.Select_vtid(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString()));
                            frm.ShowDialog();
                        }

                        //frm_printcopy frm = new frm_printcopy("Print", ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString(), funs.Select_vtid(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString()));
                        //frm.ShowDialog();
                    }
                }
                else
                {
                    frm_printcopy frm = new frm_printcopy("Print", ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString(), funs.Select_vtid(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString()));
                    frm.ShowDialog();
                }

                LoadData(gstr, gFrmCaption);
            }

            else if (ansGridView5.CurrentCell.OwningColumn.Name == "view")
            {
                if (ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString() == "0")
                {
                    return;
                }

                if (gstr == "Booking")
                {
                    if (Database.printtype == "DOS")
                    {
                        string str = DOSReport.voucherprint(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString(), "View");
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
                            Database.GetSqlData("Select printcopy from Vouchertypes where Vt_id=" + funs.Select_vtid(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString()), dtprintcopy);
                            String[] print_option = dtprintcopy.Rows[0]["printcopy"].ToString().Split(';');

                            for (int j = 0; j < print_option.Length; j++)
                            {
                                if (print_option[j] != "")
                                {
                                    String[] defaultcopy = print_option[j].Split(',');

                                    if (bool.Parse(defaultcopy[1]) == true)
                                    {
                                        rpt.voucherprint(this, funs.Select_vtid(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString()),ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString(), defaultcopy[0], true, "View");
                                    }
                                }
                            }
                           
                        }
                        else
                        {
                            frm_printcopy frm = new frm_printcopy("View", ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString(), funs.Select_vtid(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString()));
                            frm.Show();
                        }
                    }
                }
                else if (gstr == "Stock Transfer" || gstr == "Challan")
                {
                    if (Database.printtype == "DOS")
                    {
                        string str = DOSReport.voucherprintChallan(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString(), "View");
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
                            Database.GetSqlData("Select printcopy from Vouchertypes where Vt_id=" + funs.Select_vtid(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString()), dtprintcopy);
                            String[] print_option = dtprintcopy.Rows[0]["printcopy"].ToString().Split(';');

                            for (int j = 0; j < print_option.Length; j++)
                            {
                                if (print_option[j] != "")
                                {
                                    String[] defaultcopy = print_option[j].Split(',');
                                    if (bool.Parse(defaultcopy[1]) == true)
                                    {
                                        rpt.voucherprint(this, funs.Select_vtid(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString()), ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString(), defaultcopy[0], true, "View");
                                    }
                                }
                            }

                        }
                        else
                        {
                            frm_printcopy frm = new frm_printcopy("View", ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString(), funs.Select_vtid(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString()));
                            frm.ShowDialog();
                        }
                        //frm_printcopy frm = new frm_printcopy("View", ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString(), funs.Select_vtid(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString()));
                        //frm.Show();
                    }
                }

                else
                {
                    frm_printcopy frm = new frm_printcopy("View", ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString(), funs.Select_vtid(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString()));
                    frm.Show();
                }
            }

            else if (ansGridView5.CurrentCell.OwningColumn.Name == "Edit")
            {
                if (gstr == "Sale")
                {
                    if (ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString() == "0")
                    {
                        return;
                    }
                    frm_bill frm = new frm_bill();
                    frm.LoadData(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString(), "Bill");
                    frm.MdiParent = this.MdiParent;
                    frm.Show();
                }


                else if (gstr == "Delivery")
                {
                    if (ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString() == "0")
                    {
                        return;
                    }
                    frm_Delivery frm = new frm_Delivery();
                    frm.LoadData(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString(), "Delivery");
                    frm.MdiParent = this.MdiParent;
                    frm.Show();
                }

                else if (gstr == "DBill")
                {
                    if (ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString() == "0")
                    {
                        return;
                    }
                    frm_dbill frm = new frm_dbill();
                    frm.LoadData(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString(), "Delivery Bill");
                    frm.MdiParent = this.MdiParent;
                    frm.Show();
                }
                else if (gstr == "Booking")
                {
                    if (ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString() == "0")
                    {
                        return;
                    }
                    frmBooking frm = new frmBooking();
                    frm.LoadData(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString(), "Booking");
                    frm.MdiParent = this.MdiParent;
                    frm.Show();
                }
                else if (gstr == "Receipt")
                {
                    if (ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString() == "")
                    {
                        return;
                    }
                    frmCashRec frm = new frmCashRec();
                    frm.recpay = "Receipt";
                    frm.cmdnm = "edit";
                    frm.Text = "Edit Receipt";
                    frm.LoadData(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString(), frm.Text);
                    frm.MdiParent = this.MdiParent;
                    frm.Show();
                }
                else if (gstr == "Payment")
                {
                    if (ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString() == "")
                    {
                        return;
                    }
                    frmCashRec frm = new frmCashRec();
                    frm.recpay = "Payment";
                    frm.cmdnm = "edit";
                    frm.Text = "Edit Payment";
                    frm.LoadData(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString(), frm.Text);
                    frm.MdiParent = this.MdiParent;
                    frm.Show();
                }
                else if (gstr == "Contra")
                {
                    if (ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString() == "0")
                    {
                        return;
                    }
                    frmCashRec frm = new frmCashRec();
                    frm.recpay = "Contra";
                    frm.cmdnm = "edit";
                    frm.Text = "Edit Contra";
                    frm.LoadData(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString(), frm.Text);
                    frm.MdiParent = this.MdiParent;
                    frm.Show();
                }
                else if (gstr == "Journal")
                {
                    if (ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString() == "0")
                    {
                        return;
                    }
                    frmJournal frm = new frmJournal();
                    frm.LoadData(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString(), "Journal Voucher");
                    frm.MdiParent = this.MdiParent;
                    frm.Show();
                }
                else if (gstr == "Stock Transfer")
                {
                    if (ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString() == "0")
                    {
                        return;
                    }
                    frmStockTransfer frm = new frmStockTransfer();
                    frm.LoadData(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString(), "Stock Transfer");
                    frm.MdiParent = this.MdiParent;
                    frm.Show();
                }
                else if (gstr == "Challan")
                {
                    if (ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString() == "0")
                    {
                        return;
                    }
                    frm_Challan frm = new frm_Challan();
                    frm.LoadData(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString(), "Challan");
                    frm.MdiParent = this.MdiParent;
                    frm.Show();
                }
                else if (gstr == "Unloading")
                {
                    if (ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString() == "0")
                    {
                        return;
                    }
                    frm_unloading frm = new frm_unloading();
                    frm.LoadData(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString(), "Challan");
                    frm.MdiParent = this.MdiParent;
                    frm.Show();
                }
                else if (gstr == "GRByChallan")
                {
                    if (ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString() == "0")
                    {
                        return;
                    }
                    frm_newunloading frm = new frm_newunloading();
                    frm.LoadData(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString(), "GRByChallan");
                    frm.MdiParent = this.MdiParent;
                    frm.Show();
                }
            }
        }


        private void deletenew(string vid)
        {
            DataTable dtTemp = new DataTable();

            if (gstr == "Booking")
            {

                string strValidSql = "select ";
                strValidSql += "(Select count(*) from Stocks where Gr_id='" + vid + "') as grcount,";
                strValidSql += "(SELECT count(*) FROM VOUCHERdets WHERE Booking_id='" + vid + "') as bokingcount,";
                strValidSql += "(SELECT count(*) FROM VOUCHERdets WHERE Bill_Booking_id='" + vid + "') as bilcount";

                DataTable dtValid = new DataTable();
                Database.GetSqlData(strValidSql, dtValid);

                //int grcount = Database.GetScalarInt("Select count(*) from Stocks where Gr_id='" + vid + "'");

                if (long.Parse(dtValid.Rows[0]["grcount"].ToString()) > 1)
                {
                    MessageBox.Show("This GR has been Dispatched. Deletion is not possible.");

                    return;
                }

                if (long.Parse(dtValid.Rows[0]["bokingcount"].ToString()) > 0) //Database.GetScalarInt("SELECT count(*) FROM VOUCHERdets WHERE Booking_id='" + vid + "'") != 0)
                {
                    MessageBox.Show("Booking is used in Stock Transfer or in Challan");
                    return;
                }
                if (long.Parse(dtValid.Rows[0]["bilcount"].ToString()) > 0) //Database.GetScalarInt("SELECT count(*) FROM VOUCHERdets WHERE Bill_Booking_id='" + vid+"' ") != 0)
                {
                    MessageBox.Show("Booking is used in Billing");
                    return;
                }
                //if (Database.GetScalarInt("SELECT count(*) FROM Stocks WHERE Gr_id='" + vid + "'") != 0)
                //{
                //    MessageBox.Show("Booking is in used.");
                //    return;
                //}
            }
            else if (gstr == "Stock Transfer")
            {
                if (Database.GetScalarInt("SELECT count(*) FROM VOUCHERinfos WHERE Challan_id='" + vid + "' ") != 0)
                {
                    MessageBox.Show("Stock Transfer is Used in Unloading");
                    return;
                }
            }

            try
            {

                Database.BeginTran();

                if (gstr == "Receipt" || gstr == "Payment" || gstr == "Journal")
                {
                    //dtTemp = new DataTable("Voucheractotals");
                    //Database.GetSqlData("select * from Voucheractotals where vi_id='" + vid + "'", dtTemp);

                    //for (int i = 0; i < dtTemp.Rows.Count; i++)
                    //{
                    //    dtTemp.Rows[i].Delete();
                    //}
                    //Database.SaveData(dtTemp);

                    Database.CommandExecutor("delete from Voucheractotals where vi_id='" + vid + "'");
                }
                else
                {
                    //dtTemp = new DataTable("Voucherdets");
                    //Database.GetSqlData("select * from Voucherdets where vi_id='" + vid + "'", dtTemp);

                    //for (int i = 0; i < dtTemp.Rows.Count; i++)
                    //{
                    //    dtTemp.Rows[i].Delete();
                    //}
                    //Database.SaveData(dtTemp);

                    //dtTemp = new DataTable("ChallanUnloadings");
                    //Database.GetSqlData("select * from ChallanUnloadings where vi_id='" + vid + "'", dtTemp);

                    //for (int i = 0; i < dtTemp.Rows.Count; i++)
                    //{
                    //    dtTemp.Rows[i].Delete();
                    //}
                    //Database.SaveData(dtTemp);

                    //dtTemp = new DataTable("Vouchargess");
                    //Database.GetSqlData("select * from Vouchargess where vi_id='" + vid + "'", dtTemp);

                    //for (int i = 0; i < dtTemp.Rows.Count; i++)
                    //{
                    //    dtTemp.Rows[i].Delete();
                    //}
                    //Database.SaveData(dtTemp);
                    //dtTemp = new DataTable("Stocks");
                    //Database.GetSqlData("select * from Stocks where vid='" + vid + "'", dtTemp);

                    //for (int i = 0; i < dtTemp.Rows.Count; i++)
                    //{
                    //    dtTemp.Rows[i].Delete();
                    //}
                    //Database.SaveData(dtTemp);

                    Database.CommandExecutor("delete from Voucherdets where vi_id='" + vid + "'");
                    Database.CommandExecutor("delete from ChallanUnloadings where vi_id='" + vid + "'");
                    Database.CommandExecutor("delete from Vouchargess where vi_id='" + vid + "'");
                    Database.CommandExecutor("delete from Stocks where vi_id='" + vid + "'");
                }

                //dtTemp = new DataTable("voucherinfos");
                //Database.GetSqlData("select * from voucherinfos where vi_id='" + vid + "'", dtTemp);
                //for (int i = 0; i < dtTemp.Rows.Count; i++)
                //{
                //    dtTemp.Rows[i].Delete();
                //}
                //Database.SaveData(dtTemp);

                Database.CommandExecutor("delete from voucherinfos where vi_id='" + vid + "'");
                Database.CommitTran();
                MessageBox.Show("Deleted successfully");
            }
            catch (Exception ex)
            {
                Database.RollbackTran();
                MessageBox.Show(this, "Deletion Fail, Try Again.", "Delete Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void delete(string vid)
        {            
            DataTable dtTemp = new DataTable();

            if (gstr == "Booking")
            {

                int grcount = Database.GetScalarInt("Select count(*) from Stocks where Gr_id='" + vid + "'");

                if (grcount > 1)
                {
                    MessageBox.Show("This GR has been Dispatched. Deletion is not possible.");
                   
                    return;
                }



                if (Database.GetScalarInt("SELECT count(*) FROM VOUCHERdets WHERE Booking_id='" + vid + "'") != 0)
                {
                    MessageBox.Show("Booking is used in Stock Transfer or in Challan");
                    return;
                }

                if (Database.GetScalarInt("SELECT count(*) FROM VOUCHERdets WHERE Bill_Booking_id='" + vid+"' ") != 0)
                {
                    MessageBox.Show("Booking is used in Billing");
                    return;
                }

                //if (Database.GetScalarInt("SELECT count(*) FROM Stocks WHERE Gr_id='" + vid + "'") != 0)
                //{
                //    MessageBox.Show("Booking is in used.");
                //    return;
                //}
            }
            else if (gstr == "Stock Transfer")
            {
                if (Database.GetScalarInt("SELECT count(*) FROM VOUCHERinfos WHERE Challan_id='" + vid + "' ") != 0)
                {
                    MessageBox.Show("Stock Transfer is Used in Unloading");
                    return;
                }
            }

            else if (gstr == "Delivery")
            {
                if (Database.GetScalarInt("SELECT count(*) FROM VOUCHERdets WHERE Delivery_id='" + vid + "' ") != 0)
                {
                    MessageBox.Show("This delivery is billed");
                    return;
                }
            }
            else if (gstr == "DBill")
            {


                DataTable dt = new DataTable();
                Database.GetSqlData("Select Delivery_id from voucherdets where Vi_id='" + vid + "'", dt);

              for(int k=0;k<dt.Rows.Count;k++)
              {
                  Database.CommandExecutor("update voucherinfos set Dbilled='false' where vi_id='" + dt.Rows[k]["Delivery_id"].ToString() + "'");
              }
            }

            if (gstr == "Receipt" || gstr == "Payment" || gstr == "Journal")
            {
                dtTemp = new DataTable("Voucheractotals");
                Database.GetSqlData("select * from Voucheractotals where vi_id='" + vid + "'", dtTemp);
                
                for (int i = 0; i < dtTemp.Rows.Count; i++)
                {
                    dtTemp.Rows[i].Delete();
                }
                Database.SaveData(dtTemp);
            }
            else
            {
                dtTemp = new DataTable("Voucherdets");
                Database.GetSqlData("select * from Voucherdets where vi_id='" + vid + "'", dtTemp);
                
                for (int i = 0; i < dtTemp.Rows.Count; i++)
                {
                    dtTemp.Rows[i].Delete();
                }
                Database.SaveData(dtTemp);

                dtTemp = new DataTable("ChallanUnloadings");
                Database.GetSqlData("select * from ChallanUnloadings where vi_id='" + vid + "'", dtTemp);

                for (int i = 0; i < dtTemp.Rows.Count; i++)
                {
                    dtTemp.Rows[i].Delete();
                }
                Database.SaveData(dtTemp);

                dtTemp = new DataTable("Vouchargess");
                Database.GetSqlData("select * from Vouchargess where vi_id='" + vid + "'", dtTemp);
                
                for (int i = 0; i < dtTemp.Rows.Count; i++)
                {
                    dtTemp.Rows[i].Delete();
                }
                Database.SaveData(dtTemp);
                dtTemp = new DataTable("Stocks");
                Database.GetSqlData("select * from Stocks where vid='" + vid + "'", dtTemp);

                for (int i = 0; i < dtTemp.Rows.Count; i++)
                {
                    dtTemp.Rows[i].Delete();
                }
                Database.SaveData(dtTemp);
            }
            
            dtTemp = new DataTable("voucherinfos");
            Database.GetSqlData("select * from voucherinfos where vi_id='" + vid + "'", dtTemp);
            
            for (int i = 0; i < dtTemp.Rows.Count; i++)
            {
                dtTemp.Rows[i].Delete();
            }
            Database.SaveData(dtTemp);

            dtTemp = new DataTable("Journals");
            Database.GetSqlData("select * from Journals where vi_id='" + vid + "'", dtTemp);

            for (int i = 0; i < dtTemp.Rows.Count; i++)
            {
                dtTemp.Rows[i].Delete();
            }
            Database.SaveData(dtTemp);
            MessageBox.Show("Deleted successfully");
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            LoadData(gstr, gFrmCaption);
        }

        private void frmMasterVou_Load(object sender, EventArgs e)
        {
            SideFill();
            //if (gstr == "Booking")
            //{
            //    ansGridView5.Columns["Vdate"].DisplayIndex = 0;
            //    ansGridView5.Columns["Name"].DisplayIndex = 1;
            //    ansGridView5.Columns["Vnumber"].DisplayIndex = 2;
            //    ansGridView5.Columns["consigner"].DisplayIndex = 3;
            //    ansGridView5.Columns["consignee"].DisplayIndex = 4;
            //    ansGridView5.Columns["Amount"].DisplayIndex = 5;
            //    ansGridView5.Columns["CreatedBy"].DisplayIndex = 6;
            //    ansGridView5.Columns["ModifiedBy"].DisplayIndex = 7;
            //    ansGridView5.Columns["Modify_date"].DisplayIndex = 8;
            //    ansGridView5.Columns["Modify_time"].DisplayIndex = 9;
            //    ansGridView5.Columns["view"].DisplayIndex = 10;
            //    ansGridView5.Columns["print"].DisplayIndex = 11;
            //    ansGridView5.Columns["Edit"].DisplayIndex = 12;
            //    ansGridView5.Columns["Delet"].DisplayIndex = 13;
            //    ansGridView5.Columns["Vi_id"].DisplayIndex = 14;
            //    ansGridView5.Columns["Modify_time"].HeaderText = "Time";

            //    ansGridView5.Columns["PrintCount"].Visible = false;
            //}
            //else if (gstr == "Stock Transfer" || gstr=="Challan")
            //{
            //    ansGridView5.Columns["Vdate"].DisplayIndex = 0;
            //    ansGridView5.Columns["Name"].DisplayIndex = 1;
            //    ansGridView5.Columns["Vnumber"].DisplayIndex = 2;
            //    ansGridView5.Columns["Gaddi_name"].DisplayIndex = 3;
            //    ansGridView5.Columns["Driver"].DisplayIndex = 4;
            //    ansGridView5.Columns["source"].DisplayIndex = 5;
            //    ansGridView5.Columns["destination"].DisplayIndex = 6;
            //    ansGridView5.Columns["Amount"].DisplayIndex = 7;
            //    ansGridView5.Columns["CreatedBy"].DisplayIndex = 8;
            //    ansGridView5.Columns["ModifiedBy"].DisplayIndex = 9;
            //    ansGridView5.Columns["Modify_date"].DisplayIndex = 10;
            //    ansGridView5.Columns["Modify_time"].DisplayIndex = 11;
            //    ansGridView5.Columns["view"].DisplayIndex = 12;
            //    ansGridView5.Columns["print"].DisplayIndex = 13;
            //    ansGridView5.Columns["Edit"].DisplayIndex = 14;
            //    ansGridView5.Columns["Delet"].DisplayIndex = 15;
            //    ansGridView5.Columns["Vi_id"].DisplayIndex = 16;
            //    ansGridView5.Columns["Modify_time"].HeaderText = "Time";

            //    ansGridView5.Columns["PrintCount"].Visible = false;
            //}
            //else if (gstr == "Unloading")
            //{
            //    ansGridView5.Columns["Vdate"].DisplayIndex = 0;
            //    ansGridView5.Columns["Name"].DisplayIndex = 1;
            //    ansGridView5.Columns["Vnumber"].DisplayIndex = 2;
            //    ansGridView5.Columns["Challan_no"].DisplayIndex = 3;
            //    ansGridView5.Columns["Amount"].DisplayIndex = 5;
            //    ansGridView5.Columns["CreatedBy"].DisplayIndex = 6;
            //    ansGridView5.Columns["ModifiedBy"].DisplayIndex = 7;
            //    ansGridView5.Columns["Modify_date"].DisplayIndex = 8;
            //    ansGridView5.Columns["Modify_time"].DisplayIndex = 9;
            //    ansGridView5.Columns["view"].DisplayIndex = 10;
            //    ansGridView5.Columns["print"].DisplayIndex = 11;
            //    ansGridView5.Columns["Edit"].DisplayIndex = 12;
            //    ansGridView5.Columns["Delet"].DisplayIndex = 13;
            //    ansGridView5.Columns["Vi_id"].DisplayIndex = 14;
            //    ansGridView5.Columns["Modify_time"].HeaderText = "Time";

            //    ansGridView5.Columns["PrintCount"].Visible = false;
            //}
            //else if (gstr == "Delivery")
            //{
            //    ansGridView5.Columns["view"].DisplayIndex = 13;
            //    ansGridView5.Columns["print"].DisplayIndex = 14;
            //    ansGridView5.Columns["Edit"].DisplayIndex = 15;
            //    ansGridView5.Columns["Delet"].DisplayIndex = 15;
              
            //}
            //else if (gstr == "GRByChallan")
            //{

            //    ansGridView5.Columns["Vdate"].DisplayIndex = 0;

            //    ansGridView5.Columns["Vnumber"].DisplayIndex = 1;
            //    ansGridView5.Columns["GRNo"].DisplayIndex = 2;

            //    ansGridView5.Columns["Amount"].DisplayIndex = 3;
            //    ansGridView5.Columns["CreatedBy"].DisplayIndex = 4;
            //    ansGridView5.Columns["ModifiedBy"].DisplayIndex = 5;
            //    ansGridView5.Columns["Modify_date"].DisplayIndex = 6;

            //    ansGridView5.Columns["Modtime"].DisplayIndex = 7;
            //    ansGridView5.Columns["view"].DisplayIndex = 8;
            //    ansGridView5.Columns["print"].DisplayIndex = 9;
            //    ansGridView5.Columns["Edit"].DisplayIndex = 10;
            //    ansGridView5.Columns["Delet"].DisplayIndex = 11;
            //    ansGridView5.Columns["Vi_id"].DisplayIndex = 12;

            //    //  ansGridView5.Columns["PrintCount"].Visible = false;
            //}
            //else
            //{
            //    ansGridView5.Columns["Vdate"].DisplayIndex = 0;
            //    ansGridView5.Columns["Name"].DisplayIndex = 1;
            //    ansGridView5.Columns["Vnumber"].DisplayIndex = 2;
            //    ansGridView5.Columns["Acc_name"].DisplayIndex = 3;
            //    ansGridView5.Columns["Amount"].DisplayIndex = 5;
            //    ansGridView5.Columns["CreatedBy"].DisplayIndex = 6;
            //    ansGridView5.Columns["ModifiedBy"].DisplayIndex = 7;
            //    ansGridView5.Columns["Modify_date"].DisplayIndex = 8;
            //    ansGridView5.Columns["Modify_time"].DisplayIndex = 9;
            //    ansGridView5.Columns["view"].DisplayIndex = 10;
            //    ansGridView5.Columns["print"].DisplayIndex = 11;
            //    ansGridView5.Columns["Edit"].DisplayIndex = 12;
            //    ansGridView5.Columns["Delet"].DisplayIndex = 13;
            //    ansGridView5.Columns["Vi_id"].DisplayIndex = 14;
            //    ansGridView5.Columns["Modify_time"].HeaderText = "Time";

            //    ansGridView5.Columns["PrintCount"].Visible = false;
            //}

            //ansGridView5.Columns["Modify_date"].HeaderText = "Date";
           
            //ansGridView5.Columns["Vi_id"].Visible = false;
            //ansGridView5.Columns["Entered"].Visible = false;
           
        }

        private void frmMasterVou_Enter(object sender, EventArgs e)
        {
            this.Size = this.MdiParent.Size;
            this.WindowState = FormWindowState.Maximized;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            LoadData(gstr, gFrmCaption);
        }

        private void ansGridView5_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            ansGridView5.CurrentCell.Value = 0;
        }
    }
}
