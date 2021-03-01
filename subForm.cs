using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _027_01_칼라영상처리2_DB연동_
{
    public partial class subForm01 : Form
    {
        static int tb_max = 0, tb_min = 0;

        public subForm01(string text, int max, int min)
        {
            InitializeComponent();
            label1.Text = text;
            tb_max = max;
            tb_min = min;
            trackBar1.Maximum = tb_max;
            trackBar1.Minimum = tb_min;
            label2.Text = tb_min.ToString();
            label3.Text = tb_max.ToString();
        }
        private void bt_ok_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
        private void bt_cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
        private void nud_ValueChanged(object sender, EventArgs e)
        {
            if (nud.Value > tb_max)
            {
                nud.Value = tb_max;
            } else if (nud.Value < tb_min)
            {
                nud.Value = tb_min;
            }

            trackBar1.Value = (int)nud.Value;
        }
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            nud.Value = trackBar1.Value;
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }
        private void subForm01_Load(object sender, EventArgs e)
        {

        }
    }
}
