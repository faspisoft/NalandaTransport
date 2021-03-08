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
    public partial class SelectAcc : Form
    {
        DataTable gdt;
        public  String outStr;
        BindingSource bs = new BindingSource();
        Boolean flag = false;
        int GUptoIndex;
        public SelectAcc()
        { 
            InitializeComponent();
        }
        public  void Select(DataTable dt, String DefaultText, int ShowFieldUptoIndex)
        {            
            outStr = "";
            dt.Columns.Add("Temp1", typeof(string));
            dt.Columns.Add("Temp2", typeof(string));
            dt.Columns.Add("Temp3",typeof(string));

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["Temp1"] = dt.Rows[i][0].ToString().Replace(" ", string.Empty);
                dt.Rows[i]["Temp1"] = dt.Rows[i]["Temp1"].ToString().Replace(",", string.Empty);
                dt.Rows[i]["Temp1"] = dt.Rows[i]["Temp1"].ToString().Replace(".", string.Empty);
                
                String[] strtemp = dt.Rows[i][0].ToString().Split(' ');
                for (int j = 0; j < strtemp.Length;j++ )
                {
                    if (strtemp[j]!="")
                    {
                        dt.Rows[i]["Temp2"] +=  strtemp[j][0].ToString() + " ";
                    }
                }
            }

            gdt = dt;
            GUptoIndex = ShowFieldUptoIndex; 
            textBox1.Text = DefaultText.TrimEnd();
            textBox1.Select(textBox1.Text.Length, 0);
            bs.Filter = null;
            bs.DataSource = gdt;
            bs.Sort = "Temp3";
            filter();

        }

        private void Form2_Load(object sender, EventArgs e)
        {
            
            dataGridView1.DataSource = bs;
            dataGridView1.Columns["Temp1"].Visible = false;
            dataGridView1.Columns["Temp2"].Visible = false;
            dataGridView1.Columns["Temp3"].Visible = false;

            for (int i = GUptoIndex+1; i <dataGridView1.Columns.Count-3 ; i++)
            {
                dataGridView1.Columns[i].Visible = false;
            }
            
        }

        private void Form2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                clear();
                
                //this.Dispose();
            }
        }

       

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down && flag == true)
            {
                dataGridView1.Select();
                SendKeys.Send("{down}");
            }
            else if (e.KeyCode == Keys.Enter && flag == true)
            
            
            {
                dataGridView1.Select();
                SendKeys.Send("{enter}");

            }
            flag = true;
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)

        {
            
            if (e.KeyCode == Keys.Enter)
            {
                if (dataGridView1.Rows.Count != 0 && dataGridView1.CurrentCell.RowIndex==0)
                {
                    

                    outStr = dataGridView1.Rows[0].Cells[0].Value.ToString();
                    clear();
                    //this.Dispose();
                }
                else 
                
                {
                    if (dataGridView1.Rows.Count != 0 && dataGridView1.CurrentCell.RowIndex > 0)
                    {
                        outStr = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[0].Value.ToString();
                             
                        
                    }
                    clear();
                    //this.Dispose();
                }
            }
            
           
           
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            filter(); 
            
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            outStr = dataGridView1[0, e.RowIndex].Value.ToString();
            clear();
            //this.Dispose();
           
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Columns.Count >= 4)
            {
                l1.Text = dataGridView1.Columns[0].Name + ": " + dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            }
            else
            {
                l1.Text = "";
            }

            if (dataGridView1.Columns.Count >= 5)
            {
                l2.Text = dataGridView1.Columns[1].Name + ": " + dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            }
            else
            {
                l2.Text = "";
            }

            if (dataGridView1.Columns.Count >= 6)
            {
                l3.Text = dataGridView1.Columns[2].Name + ": " + dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            }
            else
            {
                l3.Text = "";
            }

            if (dataGridView1.Columns.Count >= 7)
            {
                l4.Text = dataGridView1.Columns[3].Name + ": " + dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            }
            else
            {
                l4.Text = "";
            }

            if (dataGridView1.Columns.Count >= 8)
            {
                l5.Text = dataGridView1.Columns[4].Name + ": " + dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
            }
            else
            {
                l5.Text = "";
            }

            if (dataGridView1.Columns.Count >= 9)
            {
                l6.Text = dataGridView1.Columns[5].Name + ": " + dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();
            }
            else
            {
                l6.Text = "";
            }

            if (dataGridView1.Columns.Count >= 10)
            {
                l7.Text = dataGridView1.Columns[6].Name + ": " + dataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString();
            }
            else
            {
                l7.Text = "";
            }

            if (dataGridView1.Columns.Count >= 11)
            {
                l8.Text = dataGridView1.Columns[7].Name + ": " + dataGridView1.Rows[e.RowIndex].Cells[7].Value.ToString();
            }
            else
            {
                l8.Text = "";
            }

            if (dataGridView1.Columns.Count >= 12)
            {
                l9.Text = dataGridView1.Columns[8].Name + ": " + dataGridView1.Rows[e.RowIndex].Cells[8].Value.ToString();
            }
            else
            {
                l9.Text = "";
            }

            if (dataGridView1.Columns.Count >= 13)
            {
                l10.Text = dataGridView1.Columns[9].Name + ": " + dataGridView1.Rows[e.RowIndex].Cells[9].Value.ToString();
            }
            else
            {
                l10.Text = "";
            }


        }

        private void dataGridView1_CellLeave(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (char.IsLetter(e.KeyChar) || char.IsNumber(e.KeyChar) || e.KeyChar == ' ' )
            {
                textBox1.Text += e.KeyChar.ToString();
                textBox1.Select(textBox1.Text.Length, 0);
                textBox1.Select();

              
            }

        }
        private void filter()
        {
            String strTemp = textBox1.Text;

            strTemp = strTemp.Replace("%", "?");
            strTemp = strTemp.Replace("[", string.Empty);
            strTemp = strTemp.Replace("]", string.Empty);

            string strfilter = "";
            for (int i = 0; i < gdt.Columns.Count; i++)
            {
                if (gdt.Columns[i].DataType.Name != "String")
                {
                    continue;
                }

                if (strfilter != "")
                {
                    strfilter += " or ";
                }
                
                    strfilter += "([" + gdt.Columns[i].ColumnName + "] like '*" + strTemp + "*'" + ")";
                
            }
            bs.Filter = null;
            bs.DataSource = gdt;
            bs.Filter = strfilter;


            foreach (DataRowView dv in bs.List)
            {
                dv.Row["Temp3"] = dv.Row[0].ToString().ToUpper().IndexOf(strTemp.ToUpper());
            }

            if (dataGridView1.Rows.Count > 0)
            {
                dataGridView1.CurrentCell = dataGridView1.Rows[0].Cells[0];
            }
            else
            {
                l1.Text = "";
                l2.Text = "";
                l3.Text = "";
                l4.Text = "";
                l5.Text = "";
                l6.Text = "";
                l7.Text = "";
                l8.Text = "";
                l9.Text = "";
                l10.Text = "";

            }

        }
        
        private void clear()
        {
            gdt.Clear();
            bs.DataSource = gdt;
            l1.Text = "";
            l2.Text = "";
            l3.Text = "";
            l4.Text = "";
            l5.Text = "";
            l6.Text = "";
            l7.Text = "";
            l8.Text = "";
            l9.Text = "";
            l10.Text = "";
            
            textBox1.Text = "";
            textBox1.Select();
            flag = false;
            
            textBox1.Text = string.Empty;
            textBox1.Select();
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
            
            this.Close();

        }
    }
}
