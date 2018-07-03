using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace xUnitFormsTests.Materials
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label1.Text = "Timer ticked.";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var d1 = new Dialog1();
            d1.ShowDialog(this);
            label2.Text = "Dialog1 popped up.";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var d1 = new Dialog1();
            d1.ShowDialog(this);
            d1.ShowDialog(this);
            label3.Text = "Dialog1 popped up twice.";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            label4.Text = MessageBox.Show("Clicked button 3.", "", MessageBoxButtons.OKCancel).ToString();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var d1 = new Dialog1();
            d1.ShowDialog(this);
            d1.ShowDialog(this);

            var d2 = new Dialog2();
            d2.ShowDialog(this);

            MessageBox.Show("Clicked button 4 1st.", "", MessageBoxButtons.OKCancel);
            MessageBox.Show("Clicked button 4 2nd.", "", MessageBoxButtons.OKCancel);

            label5.Text = "Onz";
        }
    }
}
