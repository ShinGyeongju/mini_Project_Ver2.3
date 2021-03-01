using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace _027_01_칼라영상처리2_DB연동_
{
    public partial class SplashForm : Form
    {
        delegate void CloseDelegate();

        public SplashForm()
        {
            InitializeComponent();
        }
        private void SplashForm_Load(object sender, EventArgs e)
        {
            Thread loading = new Thread(Thread);
            loading.Start();
        }
        void Thread()
        {
            System.Threading.Thread.Sleep(2500);
            this.Invoke(new CloseDelegate(FormClose));
        }
        void FormClose()
        {
            this.Close();
        }
    }
}
