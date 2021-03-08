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
    public partial class frm_printpre : Form
    {
        public string str = "";

        public frm_printpre()
        {
            InitializeComponent();
        }

        private void frm_printpre_Load(object sender, EventArgs e)
        {
            richTextBox1.Text = str;
        }

        private void frm_printpre_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
                this.Dispose();
            }
        }
    }
}
