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
    public partial class PdfReader : Form
    {
        public PdfReader()
        {
            InitializeComponent();
        }

        private void PdfReader_Load(object sender, EventArgs e)
        {
           
        }

        public void LoadFile(string str)
        {
            axAcroPDF1.LoadFile(str);
      
        }

        private void PdfReader_Activated(object sender, EventArgs e)
        {
           
        }


        private void PdfReader_KeyDown(object sender, KeyEventArgs e)
        {
            
        }

        private void axAcroPDF1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
           
        }

        private void axAcroPDF1_OnError(object sender, EventArgs e)
        {

        }

        private void axAcroPDF1_OnError_1(object sender, EventArgs e)
        {

        }


    }
}
