using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;

namespace faspi
{
    public partial class GetData : Form
    {

        public GetData()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult res= fbd.ShowDialog();
            String fld = fbd.SelectedPath;
            //MessageBox.Show(fld);
            DataTable Ddt = new DataTable("Colorant");
            LoadDataDbase(fld, "select 1 as CompanyId,CODE as ColorantCode,DESCR as ColorantName,ID as ComColorId,COST as Price from cnts", Ddt);
            saveToAccess(Ddt);
            MessageBox.Show("saved");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult res = fbd.ShowDialog();
            String fld = fbd.SelectedPath;
            DataTable Ddt = new DataTable("Base");
            LoadDataDbase(fld, "select 1 as CompanyId,CODE as BaseName,DESCR as BaseName2,ID as CompanyBaseId from bases", Ddt);
            saveToAccess(Ddt);
            MessageBox.Show("saved");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult res = fbd.ShowDialog();
            String fld = fbd.SelectedPath;
            DataTable Ddt = new DataTable("Product");
            LoadDataDbase(fld, "select 1 as CompanyId,PATH as ProductCode,DESCR as ProductName,ID as CompanyProductId from Products", Ddt);
            saveToAccess(Ddt);
            MessageBox.Show("saved");            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DialogResult res = fbd.ShowDialog();
            String fld = fbd.SelectedPath;
            DataTable dtPro = new DataTable();
            LoadDataAccess("select ProductId,ProductCode from Product", dtPro);
            for (int i = 0; i < dtPro.Rows.Count; i++)
            {
                DataTable dtCard = new DataTable("ShadeCard");
                LoadDataDbase(fld + dtPro.Rows[i]["ProductCode"], "select 1 as CompanyId," + dtPro.Rows[i]["ProductId"] + " as ProductId, DESCR as ShadeCardName,PATH as ShadeCardCode from subprods", dtCard);
                saveToAccess(dtCard);
            }
            MessageBox.Show("saved");            
        }

        

        private void button5_Click(object sender, EventArgs e)
        {
            DialogResult res = fbd.ShowDialog();
            String fld = fbd.SelectedPath;
            DataTable dtProCard = new DataTable();
            LoadDataAccess("SELECT Product.CompanyId, Product.ProductId, ShadeCard.ShadeCardId, Product.ProductCode, ShadeCard.ShadeCardCode FROM Product INNER JOIN ShadeCard ON Product.ProductId = ShadeCard.ProductId", dtProCard);
            for (int i = 0; i < dtProCard.Rows.Count; i++)
            {
                DataTable dtFormula = new DataTable("Formula");
                LoadDataDbase(fld + dtProCard.Rows[i]["ProductCode"] + "\\" + dtProCard.Rows[i]["ShadeCardCode"], "select 1 as CompanyId," + dtProCard.Rows[i]["ProductId"] + " as ProductId," + dtProCard.Rows[i]["ShadeCardId"] + " as ShadecardId,KEY1,KEY2,KEY3, FORMULA, BASE_ID from FRM", dtFormula);
                saveToAccess(dtFormula);
            }
            MessageBox.Show("saved"); 
        }

        void saveToAccess(DataTable dt)
        {
            OleDbConnection conn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Application.StartupPath + "\\tinting.mdb");
            OleDbDataAdapter da = new OleDbDataAdapter("select * from " + dt.TableName, conn);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i].SetAdded();
            }
            OleDbCommandBuilder cb = new OleDbCommandBuilder();
            cb.QuotePrefix = "[";
            cb.QuoteSuffix = "]";
            cb.DataAdapter = da;

            da.Update(dt);
        }

        void LoadDataDbase(string Path, string SQL, DataTable dt)
        {
            OleDbConnection conn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Application.StartupPath + Path + ";Extended Properties=dbase IV;User ID=Admin;Password=;");
            OleDbDataAdapter da = new OleDbDataAdapter(SQL, conn);
            da.Fill(dt);

        }
        void LoadDataAccess(string SQL, DataTable dt)
        {
            OleDbConnection conn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Application.StartupPath + "\\tinting.mdb");
            OleDbDataAdapter Dda = new OleDbDataAdapter(SQL, conn);
            Dda.Fill(dt);
        }

        
    }
}
