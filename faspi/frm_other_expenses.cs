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
    public partial class frm_other_expenses : Form
    {
        public DataGridView gdt;
        int rindex;
        public frm_other_expenses()
        {
            InitializeComponent();
        }

        private void frm_other_expenses_Load(object sender, EventArgs e)
        {
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
            label8.Text = Feature.Available("Name of Expense11");
        }

        private void textBox6_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox6);
        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && !(e.KeyChar.ToString() == ".") && !(e.KeyChar.ToString() == "-");
        }

        private void textBox6_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox6);
        }

        private void textBox12_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox12);
        }

        private void textBox12_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && !(e.KeyChar.ToString() == ".") && !(e.KeyChar.ToString() == "-");
        }

        private void textBox12_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox12);
        }

        private void textBox11_KeyPress(object sender, KeyPressEventArgs e)
        {
            DataTable dtcombo = TypeDt();
            textBox11.Text = SelectCombo.ComboDt(this, dtcombo, 0);
            SendKeys.Send("{tab}");
        }

        private void textBox10_KeyPress(object sender, KeyPressEventArgs e)
        {
            DataTable dtcombo = TypeDt();
            textBox10.Text = SelectCombo.ComboDt(this, dtcombo, 0);
            SendKeys.Send("{tab}");
        }

        private void textBox9_KeyPress(object sender, KeyPressEventArgs e)
        {
            DataTable dtcombo = TypeDt();
            textBox9.Text = SelectCombo.ComboDt(this, dtcombo, 0);
            SendKeys.Send("{tab}");
        }

        private void textBox8_KeyPress(object sender, KeyPressEventArgs e)
        {
            DataTable dtcombo = TypeDt();
            textBox8.Text = SelectCombo.ComboDt(this, dtcombo, 0);
            SendKeys.Send("{tab}");
        }

        private void textBox7_KeyPress(object sender, KeyPressEventArgs e)
        {
            DataTable dtcombo = TypeDt();
            textBox7.Text = SelectCombo.ComboDt(this, dtcombo, 0);
            SendKeys.Send("{tab}");
        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            DataTable dtcombo = TypeDt();
            textBox5.Text = SelectCombo.ComboDt(this, dtcombo, 0);
            SendKeys.Send("{tab}");
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            DataTable dtcombo = TypeDt();
            textBox4.Text = SelectCombo.ComboDt(this, dtcombo, 0);
            SendKeys.Send("{tab}");
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            DataTable dtcombo = TypeDt();
            textBox3.Text = SelectCombo.ComboDt(this, dtcombo, 0);
            SendKeys.Send("{tab}");
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            DataTable dtcombo = TypeDt();
            textBox2.Text = SelectCombo.ComboDt(this, dtcombo, 0);
            SendKeys.Send("{tab}");
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            DataTable dtcombo = TypeDt();
            textBox1.Text = SelectCombo.ComboDt(this, dtcombo, 0);
            SendKeys.Send("{tab}");
        }

        private void textBox13_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox13);
        }

        private void textBox13_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && !(e.KeyChar.ToString() == ".") && !(e.KeyChar.ToString() == "-");
        }

        private void textBox13_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox13);
        }

        private void textBox14_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox14);
        }

        private void textBox14_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && !(e.KeyChar.ToString() == ".") && !(e.KeyChar.ToString() == "-");
        }

        private void textBox14_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox14);
        }

        private void textBox15_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox15);
        }

        private void textBox15_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && !(e.KeyChar.ToString() == ".") && !(e.KeyChar.ToString() == "-");
        }

        private void textBox15_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox15);
        }

        private void textBox17_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox17);
        }

        private void textBox17_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && !(e.KeyChar.ToString() == ".") && !(e.KeyChar.ToString() == "-");
        }

        private void textBox17_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox17);
        }

        private void textBox18_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox18);
        }

        private void textBox18_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && !(e.KeyChar.ToString() == ".") && !(e.KeyChar.ToString() == "-");
        }

        private void textBox18_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox18);
        }

        private void textBox19_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox19);
        }

        private void textBox19_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && !(e.KeyChar.ToString() == ".") && !(e.KeyChar.ToString() == "-");
        }

        private void textBox19_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox19);
        }

        private void textBox21_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox21);
        }

        private void textBox21_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && !(e.KeyChar.ToString() == ".") && !(e.KeyChar.ToString() == "-");
        }

        private void textBox21_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox21);
        }

        private void textBox22_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox22);
        }

        private void textBox22_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && !(e.KeyChar.ToString() == ".") && !(e.KeyChar.ToString() == "-");
        }

        private void textBox22_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox22);
        }

        private void textBox11_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox11);
        }

        private void textBox11_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox11);
        }

        private void textBox10_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox10);
        }

        private void textBox10_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox10);
        }

        private void textBox9_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox9);
        }

        private void textBox9_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox9);
        }

        private void textBox8_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox8);
        }

        private void textBox8_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox8);
        }

        private void textBox7_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox7);
        }

        private void textBox7_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox7);
        }

        private void textBox5_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox5);
        }

        private void textBox5_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox5);
        }

        private void textBox4_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox4);
        }

        private void textBox4_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox4);
        }

        private void textBox3_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox3);
        }

        private void textBox3_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox3);
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox2);
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox2);
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox1);
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox1);
        }

        private void textBox30_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox30);
        }

        private void textBox30_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && !(e.KeyChar.ToString() == ".") && !(e.KeyChar.ToString() == "-");
        }

        private void textBox30_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox30);
        }

        private void textBox29_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox29);
        }

        private void textBox29_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && !(e.KeyChar.ToString() == ".") && !(e.KeyChar.ToString() == "-");
        }

        private void textBox29_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox29);
        }

        private void textBox28_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox28);
        }

        private void textBox28_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && !(e.KeyChar.ToString() == ".") && !(e.KeyChar.ToString() == "-");
        }

        private void textBox28_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox28);
        }

        private void textBox27_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox27);
        }

        private void textBox27_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && !(e.KeyChar.ToString() == ".") && !(e.KeyChar.ToString() == "-");
        }

        private void textBox27_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox27);
        }

        private void textBox26_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox26);
        }

        private void textBox26_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && !(e.KeyChar.ToString() == ".") && !(e.KeyChar.ToString() == "-");
        }

        private void textBox26_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox26);
        }

        private void textBox25_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox25);
        }

        private void textBox25_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && !(e.KeyChar.ToString() == ".") && !(e.KeyChar.ToString() == "-");
        }

        private void textBox25_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox25);
        }

        private void textBox24_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox24);
        }

        private void textBox24_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && !(e.KeyChar.ToString() == ".") && !(e.KeyChar.ToString() == "-");
        }

        private void textBox24_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox24);
        }

        private void textBox23_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox23);
        }

        private void textBox23_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && !(e.KeyChar.ToString() == ".") && !(e.KeyChar.ToString() == "-");
        }

        private void textBox23_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox23);
        }

        private void textBox20_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox20);
        }

        private void textBox20_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && !(e.KeyChar.ToString() == ".") && !(e.KeyChar.ToString() == "-");
        }

        private void textBox20_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox20);
        }

        private void textBox16_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox16);
        }

        private void textBox16_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && !(e.KeyChar.ToString() == ".") && !(e.KeyChar.ToString() == "-");
        }

        private void textBox16_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox16);
        }

        private DataTable TypeDt()
        {
            DataTable dtcombo = new DataTable();
            dtcombo.Columns.Add("RatePer", typeof(string));

            dtcombo.Columns["RatePer"].ColumnName = "RatePer";

            dtcombo.Rows.Add();
            dtcombo.Rows[0][0] = "/Nug";

            dtcombo.Rows.Add();
            dtcombo.Rows[1][0] = "/Weight";

            dtcombo.Rows.Add();
            dtcombo.Rows[2][0] = "Flat";

            dtcombo.Rows.Add();
            dtcombo.Rows[3][0] = "% of Freight";

            dtcombo.Rows.Add();
            dtcombo.Rows[4][0] = "% of Expenses";

            return dtcombo;
        }

        public void LoadDate(DataGridView dt, int rowIndex)
        {
            gdt = dt;
            rindex = rowIndex;
            textBox6.Focus();

            //rate
            textBox6.Text = funs.IndianCurr(Convert.ToDouble(dt.Rows[rowIndex].Cells["exp1rate"].Value));
            textBox12.Text = funs.IndianCurr(Convert.ToDouble(dt.Rows[rowIndex].Cells["exp2rate"].Value));
            textBox13.Text = funs.IndianCurr(Convert.ToDouble(dt.Rows[rowIndex].Cells["exp3rate"].Value));
            textBox14.Text = funs.IndianCurr(Convert.ToDouble(dt.Rows[rowIndex].Cells["exp4rate"].Value));
            textBox15.Text = funs.IndianCurr(Convert.ToDouble(dt.Rows[rowIndex].Cells["exp5rate"].Value));
            textBox17.Text = funs.IndianCurr(Convert.ToDouble(dt.Rows[rowIndex].Cells["exp6rate"].Value));
            textBox18.Text = funs.IndianCurr(Convert.ToDouble(dt.Rows[rowIndex].Cells["exp7rate"].Value));
            textBox19.Text = funs.IndianCurr(Convert.ToDouble(dt.Rows[rowIndex].Cells["exp8rate"].Value));
            textBox21.Text = funs.IndianCurr(Convert.ToDouble(dt.Rows[rowIndex].Cells["exp9rate"].Value));
            textBox22.Text = funs.IndianCurr(Convert.ToDouble(dt.Rows[rowIndex].Cells["exp10rate"].Value));
            textBox36.Text = funs.IndianCurr(Convert.ToDouble(dt.Rows[rowIndex].Cells["exp11rate"].Value));
            textBox33.Text = funs.IndianCurr(Convert.ToDouble(dt.Rows[rowIndex].Cells["Rate_am"].Value));

            //type
            textBox11.Text = dt.Rows[rowIndex].Cells["exp1type"].Value.ToString();
            textBox10.Text = dt.Rows[rowIndex].Cells["exp2type"].Value.ToString();
            textBox9.Text = dt.Rows[rowIndex].Cells["exp3type"].Value.ToString();
            textBox8.Text = dt.Rows[rowIndex].Cells["exp4type"].Value.ToString();
            textBox7.Text = dt.Rows[rowIndex].Cells["exp5type"].Value.ToString();
            textBox5.Text = dt.Rows[rowIndex].Cells["exp6type"].Value.ToString();
            textBox4.Text = dt.Rows[rowIndex].Cells["exp7type"].Value.ToString();
            textBox3.Text = dt.Rows[rowIndex].Cells["exp8type"].Value.ToString();
            textBox2.Text = dt.Rows[rowIndex].Cells["exp9type"].Value.ToString();
            textBox1.Text = dt.Rows[rowIndex].Cells["exp10type"].Value.ToString();
            textBox35.Text = dt.Rows[rowIndex].Cells["exp11type"].Value.ToString();
            textBox32.Text = dt.Rows[rowIndex].Cells["per"].Value.ToString();

            //Minimum Rate
            textBox30.Text = funs.IndianCurr(Convert.ToDouble(dt.Rows[rowIndex].Cells["exp1mr"].Value));
            textBox29.Text = funs.IndianCurr(Convert.ToDouble(dt.Rows[rowIndex].Cells["exp2mr"].Value));
            textBox28.Text = funs.IndianCurr(Convert.ToDouble(dt.Rows[rowIndex].Cells["exp3mr"].Value));
            textBox27.Text = funs.IndianCurr(Convert.ToDouble(dt.Rows[rowIndex].Cells["exp4mr"].Value));
            textBox26.Text = funs.IndianCurr(Convert.ToDouble(dt.Rows[rowIndex].Cells["exp5mr"].Value));
            textBox25.Text = funs.IndianCurr(Convert.ToDouble(dt.Rows[rowIndex].Cells["exp6mr"].Value));
            textBox24.Text = funs.IndianCurr(Convert.ToDouble(dt.Rows[rowIndex].Cells["exp7mr"].Value));
            textBox23.Text = funs.IndianCurr(Convert.ToDouble(dt.Rows[rowIndex].Cells["exp8mr"].Value));
            textBox20.Text = funs.IndianCurr(Convert.ToDouble(dt.Rows[rowIndex].Cells["exp9mr"].Value));
            textBox16.Text = funs.IndianCurr(Convert.ToDouble(dt.Rows[rowIndex].Cells["exp10mr"].Value));
            textBox34.Text = funs.IndianCurr(Convert.ToDouble(dt.Rows[rowIndex].Cells["exp11mr"].Value));
            textBox31.Text = funs.IndianCurr(Convert.ToDouble(dt.Rows[rowIndex].Cells["freightmr"].Value));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //rate
            gdt.Rows[rindex].Cells["exp1rate"].Value = textBox6.Text;
            gdt.Rows[rindex].Cells["exp2rate"].Value = textBox12.Text;
            gdt.Rows[rindex].Cells["exp3rate"].Value = textBox13.Text;
            gdt.Rows[rindex].Cells["exp4rate"].Value = textBox14.Text;
            gdt.Rows[rindex].Cells["exp5rate"].Value = textBox15.Text;
            gdt.Rows[rindex].Cells["exp6rate"].Value = textBox17.Text;
            gdt.Rows[rindex].Cells["exp7rate"].Value = textBox18.Text;
            gdt.Rows[rindex].Cells["exp8rate"].Value = textBox19.Text;
            gdt.Rows[rindex].Cells["exp9rate"].Value = textBox21.Text;
            gdt.Rows[rindex].Cells["exp10rate"].Value = textBox22.Text;
            gdt.Rows[rindex].Cells["exp11rate"].Value = textBox36.Text;
            gdt.Rows[rindex].Cells["Rate_am"].Value = textBox33.Text;

            //type
            gdt.Rows[rindex].Cells["exp1type"].Value = textBox11.Text;
            gdt.Rows[rindex].Cells["exp2type"].Value = textBox10.Text;
            gdt.Rows[rindex].Cells["exp3type"].Value = textBox9.Text;
            gdt.Rows[rindex].Cells["exp4type"].Value = textBox8.Text;
            gdt.Rows[rindex].Cells["exp5type"].Value = textBox7.Text;
            gdt.Rows[rindex].Cells["exp6type"].Value = textBox5.Text;
            gdt.Rows[rindex].Cells["exp7type"].Value = textBox4.Text;
            gdt.Rows[rindex].Cells["exp8type"].Value = textBox3.Text;
            gdt.Rows[rindex].Cells["exp9type"].Value = textBox2.Text;
            gdt.Rows[rindex].Cells["exp10type"].Value = textBox1.Text;
            gdt.Rows[rindex].Cells["exp11type"].Value = textBox35.Text;
            gdt.Rows[rindex].Cells["per"].Value = textBox32.Text;

            //Minimum Rate
            gdt.Rows[rindex].Cells["exp1mr"].Value = textBox30.Text;
            gdt.Rows[rindex].Cells["exp2mr"].Value = textBox29.Text;
            gdt.Rows[rindex].Cells["exp3mr"].Value = textBox28.Text;
            gdt.Rows[rindex].Cells["exp4mr"].Value = textBox27.Text;
            gdt.Rows[rindex].Cells["exp5mr"].Value = textBox26.Text;
            gdt.Rows[rindex].Cells["exp6mr"].Value = textBox25.Text;
            gdt.Rows[rindex].Cells["exp7mr"].Value = textBox24.Text;
            gdt.Rows[rindex].Cells["exp8mr"].Value = textBox23.Text;
            gdt.Rows[rindex].Cells["exp9mr"].Value = textBox20.Text;
            gdt.Rows[rindex].Cells["exp10mr"].Value = textBox16.Text;
            gdt.Rows[rindex].Cells["exp11mr"].Value = textBox34.Text;
            gdt.Rows[rindex].Cells["freightmr"].Value = textBox31.Text;

            this.Close();
            this.Dispose();
        }

        private void textBox6_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox30_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox12_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox29_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox13_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox28_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox14_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox27_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox15_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox26_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox17_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox25_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox18_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox24_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox19_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox23_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox21_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox20_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox22_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox16_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox33_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox33);
        }

        private void textBox33_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox33_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && !(e.KeyChar.ToString() == ".") && !(e.KeyChar.ToString() == "-");
        }

        private void textBox33_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox33);
        }

        private void textBox32_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox32);
        }

        private void textBox32_KeyPress(object sender, KeyPressEventArgs e)
        {
            DataTable dtcombo = new DataTable();
            dtcombo.Columns.Add("RatePer", typeof(string));

            dtcombo.Columns["RatePer"].ColumnName = "RatePer";

            dtcombo.Rows.Add();
            dtcombo.Rows[0][0] = "/Nug";

            dtcombo.Rows.Add();
            dtcombo.Rows[1][0] = "/Weight";

            dtcombo.Rows.Add();
            dtcombo.Rows[2][0] = "Flat";

            textBox32.Text = SelectCombo.ComboDt(this, dtcombo, 0);
            SendKeys.Send("{tab}");
        }

        private void textBox32_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox32);
        }

        private void textBox31_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox31);
        }

        private void textBox31_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox31_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && !(e.KeyChar.ToString() == ".") && !(e.KeyChar.ToString() == "-");
        }

        private void textBox31_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox31);
        }

        private void textBox30_ImeModeChanged(object sender, EventArgs e)
        {

        }

        private void textBox35_KeyDown(object sender, KeyEventArgs e)
        {
            DataTable dtcombo = TypeDt();
            textBox35.Text = SelectCombo.ComboDt(this, dtcombo, 0);
            SendKeys.Send("{tab}");
        }

        private void textBox36_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && !(e.KeyChar.ToString() == ".") && !(e.KeyChar.ToString() == "-");
        }

        private void textBox34_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && !(e.KeyChar.ToString() == ".") && !(e.KeyChar.ToString() == "-");
        }

        private void textBox36_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox36);
        }

        private void textBox36_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox36);
        }

        private void textBox35_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox35);
        }

        private void textBox35_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox35);
        }

        private void textBox34_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox34);
        }

        private void textBox34_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox34);
        }

        private void textBox36_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox34_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }
    }
}
