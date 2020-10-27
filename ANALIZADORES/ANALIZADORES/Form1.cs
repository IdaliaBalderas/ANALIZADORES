using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ANALIZADORES
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form formLexico = new Form2();
            formLexico.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form formSintactico = new Form3();
            formSintactico.Show();
        }



    }
}
