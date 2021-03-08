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
    public partial class Search : Form
    {
        int pid = 0,shadecardId = 0;
        public String outStr;
        

        public Search()
        {
            InitializeComponent();
        }

        private void Search_Load(object sender, EventArgs e)
        {
            DataTable dtTree = new DataTable();
            
            LoadDataAccess("SELECT ProductId,Product.ProductName FROM Product" ,dtTree);
            if (dtTree.Rows.Count > 0)
            {
                for (int i = 0; i < dtTree.Rows.Count; i++)
                {
                    TreeNode parent = new TreeNode();

                    parent.Text = dtTree.Rows[i]["ProductName"].ToString();
                    treeView1.Nodes.Add(parent);
                    DataTable dtChild = new DataTable();
                    LoadDataAccess("select ShadeCardName from ShadeCard where ProductId=" + dtTree.Rows[i]["ProductId"], dtChild);
                    for (int j = 0; j < dtChild.Rows.Count; j++)
                    {
                        TreeNode child = new TreeNode();
                        child.Text = dtChild.Rows[j]["ShadeCardName"].ToString();
                        parent.Nodes.Add(child);
                    }
                }
            }
        }
        void LoadDataAccess(string SQL, DataTable dt)
        {
            OleDbConnection conn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Application.StartupPath + "\\tinting.mdb");
            OleDbDataAdapter Dda = new OleDbDataAdapter(SQL, conn);
            Dda.Fill(dt);
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            DataTable dtKey = new DataTable();
            DataTable dtPid = new DataTable();
            DataTable dtShadeCardId = new DataTable();
            LoadDataAccess("select ProductId from product where ProductName='" + e.Node.Text + "'", dtPid);
            if (dtPid.Rows.Count > 0)
            {
                pid = int.Parse(dtPid.Rows[0]["ProductId"].ToString());
            }
            LoadDataAccess("select ShadeCardId from ShadeCard where ShadeCardName='" + e.Node.Text + "'", dtShadeCardId);
            if (dtShadeCardId.Rows.Count > 0)
            {
                shadecardId = int.Parse(dtShadeCardId.Rows[0]["ShadeCardId"].ToString());
            }
            //MessageBox.Show(pid + " " + shadecardId);
            LoadDataAccess("select key1,key2,key3 from formula where ShadecardId in (select ShadecardId from ShadeCard where ShadeCardName='" + e.Node.Text + "')", dtKey);
            if (dtKey.Rows.Count > 0)
            {
                ansGridView1.DataSource = dtKey;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataTable dtKey1 = new DataTable();
            dtKey1.Clear();
            LoadDataAccess("select key1,key2,key3 from formula where Key1 like '" + textBox1.Text + "%' and ShadecardId=" + shadecardId + " and ProductId=" + pid, dtKey1);
            if (dtKey1.Rows.Count > 0)
            {
                ansGridView1.DataSource = null;
                ansGridView1.DataSource = dtKey1;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            DataTable dtKey1 = new DataTable();
            dtKey1.Clear();
            LoadDataAccess("select key1,key2,key3 from formula where (Key1 like '" + textBox1.Text + "%' or Key2 like '" + textBox1.Text + "%' or Key3 like '" + textBox1.Text + "%') and ShadecardId=" + shadecardId + " and ProductId=" + pid, dtKey1);
            if (dtKey1.Rows.Count > 0)
            {
                ansGridView1.DataSource = null;
                ansGridView1.DataSource = dtKey1;
            }
        }

        private void ansGridView1_DoubleClick(object sender, EventArgs e)
        {
            String Key1, Key2, Key3;
            Key1 = ansGridView1.Rows[ansGridView1.CurrentRow.Index].Cells["Key1"].Value.ToString();
            Key2 = ansGridView1.Rows[ansGridView1.CurrentRow.Index].Cells["Key2"].Value.ToString();
            Key3 = ansGridView1.Rows[ansGridView1.CurrentRow.Index].Cells["Key3"].Value.ToString();
            //frm.k1 = Key1;
            //frm.k2 = Key2;
            //frm.k3 = Key3;
            String str = "";
            if (Key1 != "")
            {
                str = "Key1='" + Key1 + "'";
            }
            if (Key2 != "")
            {
                if (Key1 == "")
                {
                    str = " Key2='" + Key2 + "'";
                }
                str += " and Key2='" + Key2 + "'";
            }
            if (Key3 != "")
            {
                
                str += " and Key3='" + Key3 + "'";
            }
            String formula;
            DataTable dtFormula = new DataTable();
            LoadDataAccess("select Formula,BASE_ID from Formula where " + str + " and ProductId=" + pid + " and ShadecardId=" + shadecardId, dtFormula);
            if (dtFormula.Rows.Count > 0)
            {
                formula = dtFormula.Rows[0]["Formula"].ToString();

                DataTable dtBaseName = new DataTable();
                LoadDataAccess("select BaseName from base where CompanyBaseId=" + dtFormula.Rows[0]["BASE_ID"], dtBaseName);
                if (dtBaseName.Rows.Count > 0)
                {
                    //frm.basenm = dtBaseName.Rows[0]["BaseName"].ToString();
                }
            }
            DataTable dtProductName = new DataTable();
            LoadDataAccess("select ProductName from product where ProductId=" + pid, dtProductName);
            if (dtProductName.Rows.Count > 0)
            {
                //frm.pnm = dtProductName.Rows[0]["ProductName"].ToString();
            }
            DataTable dtShadeCard = new DataTable();
            LoadDataAccess("select ShadeCardName from ShadeCard where ShadeCardId=" + shadecardId, dtShadeCard);
            if (dtShadeCard.Rows.Count > 0)
            {
                //frm.shadecard = dtShadeCard.Rows[0]["ShadeCardName"].ToString();
            }
            /*frm.LoadData();
            frm.ShowInTaskbar = false;
            frm.ShowDialog(this);*/

            //DataTable dtPrice = new DataTable();
            //dtPrice.Columns.Add("qty");
            //dtPrice.Columns.Add("amt", typeof(double));
            //LoadDataAccess("select ComColorId,ColorantCode,Price from colorant", dtPrice);
            //frm_main.clearDisplay2();
            //frm_main.dtDisplay2.Columns.Add("Item");
            //frm_main.dtDisplay2.Columns.Add("Description");

            //if (dtPrice.Rows.Count > 0)
            //{
            //    frm_main.dtDisplay2.Rows.Add();
            //    frm_main.dtDisplay2.Rows[0]["Item"] = "ComColorId";
            //    frm_main.dtDisplay2.Rows[0]["Description"] = dtPrice.Rows[0]["ComColorId"];
            //    frm_main.dtDisplay2.Rows[1]["Item"] = "ColorantCode";
            //    frm_main.dtDisplay2.Rows[1]["Description"] = dtPrice.Rows[0]["ColorantCode"];
            //    frm_main.dtDisplay2.Rows[2]["Item"] = "Price";
            //    frm_main.dtDisplay2.Rows[2]["Description"] = dtPrice.Rows[0]["Price"];
            //}
            outStr = Key1;
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

    }
}
