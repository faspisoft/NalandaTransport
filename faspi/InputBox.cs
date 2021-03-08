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
    public partial class InputBox : Form
    {
        public String outStr;
        public InputBox(String msg,String defaultVal, bool password)
        {
            InitializeComponent();
            if (password == true)
            {
                textBox1.PasswordChar = '*';
            }
            textBox1.Text = defaultVal;
            SendKeys.Send("{right}");
            label1.Text = msg;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            outStr = "";
            this.Close();
        }

        private void InputBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                if (outStr == "OTP")
                {
                    outStr = "";
                    this.Close();
                    Environment.Exit(0);
                }
                else
                {
                    outStr = "";
                    this.Close();
                }

            }
            if (e.KeyCode == Keys.Enter)
            {
                if (outStr == "OTP")
                {
                    if (textBox1.Text == "")
                    {
                        textBox1.Text = "0";
                    }

                    if (Database.OTP == int.Parse(textBox1.Text))
                    {
                        this.Close();
                        this.Dispose();
                    }
                    else
                    {
                        MessageBox.Show("You have Entered wrong OTP.");
                        Environment.Exit(0);
                    }
                }
                else
                {
                    outStr = textBox1.Text;
                    this.Close();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            outStr = textBox1.Text;
            this.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
