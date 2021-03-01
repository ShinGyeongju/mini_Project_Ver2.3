using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using MySql.Data.MySqlClient;

namespace _027_01_칼라영상처리2_DB연동_
{
    public partial class dbForm : Form
    {
        public byte[,,] outImage = null;
        public int outH = 0, outW = 0;
        const int RGB = 3, RR = 0, GG = 1, BB = 2;
        public String db_Server = "127.0.0.1";
        public String fileName, i_fname, i_extname;
        MySqlConnection mscn;
        MySqlCommand mscm;
        MySqlDataReader msdr;

        public dbForm(String fileName)
        {
            InitializeComponent();

            if (fileName != null) 
            {
                this.fileName = fileName;
                String[] tmp1 = fileName.Split('\\');
                String[] tmp2 = tmp1[tmp1.Length - 1].Split('.');
                this.i_extname = tmp2[1];
                textBox1.Text = tmp2[0];
            }
        }
        private void dbForm_Load(object sender, EventArgs e)
        {
            mscn = new MySqlConnection("Server=" + db_Server + ";Database=image_db;Uid=winuser;Pwd=p@ssw0rd;Charset=UTF8");
            mscn.Open();
            mscm = new MySqlCommand("", mscn);

            select_db();
        }
        private void dbForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            mscn.Close();
        }
        private void bt_upload_Click(object sender, EventArgs e)
        {
            int i_num = 0;

            if (fileName != null)
            {
                if (textBox1.Text == "")
                {
                    MessageBox.Show("# [실패] 파일이름을 입력해주세요.");
                    return;
                }
                mscm.CommandText = "SELECT max(i_num) AS 'i_num' FROM image;";
                msdr = mscm.ExecuteReader();
                msdr.Read();

                if (!(msdr["i_num"] is System.DBNull)) 
                    i_num = (int)msdr["i_num"] + 1;
                msdr.Close();
                progressBar1.PerformStep();

                String i_fname = textBox1.Text;
                String i_user = "temp";
                int i_height = outH;
                int i_width = outW;
                long i_fsize;
                if (fileName.Contains(':'))
                    i_fsize = new FileInfo(fileName).Length;
                else
                    i_fsize = outW * outH;

                mscm.CommandText = "INSERT INTO image VALUES (" + i_num + ", '" + i_fname + "', '" + i_extname + "', " + i_fsize + ", " + i_width + ", " + i_height + ", '" + i_user + "');";
                try
                {
                    mscm.ExecuteNonQuery();
                }
                catch
                {
                    MessageBox.Show("# [실패] 업로드할 수 없습니다.");
                    return;
                }
            }
            else
            {
                MessageBox.Show("# [실패] 파일을 불러와주세요.");
                return;
            }

            progressBar1.Maximum = outH;
            for (int i = 0; i < outH; i++)
            {
                for (int k = 0; k < outW; k++)
                {
                    byte r = outImage[RR, i, k];
                    byte g = outImage[GG, i, k];
                    byte b = outImage[BB, i, k];

                    mscm.CommandText = "INSERT INTO pixel VALUES (" + i_num + ", " + i + ", " + k + ", " + r + ", " + g + ", " + b + ");";
                    mscm.ExecuteNonQuery();
                }
                progressBar1.PerformStep();
           }
            MessageBox.Show("# [성공] 업로드 되었습니다.");
            progressBar1.Value = 0;

            select_db();
        }
        private void bt_download_Click(object sender, EventArgs e)
        {
            if(listBox1.Text == "")
            {
                MessageBox.Show("# [실패] 이미지를 선택해주세요.");
                return;
            }
            String[] tmp = listBox1.Text.Split(' ');
            String i_num = tmp[2].Substring(0, 1);

            mscm.CommandText = "SELECT i_fname, i_extname FROM image WHERE i_num=" + i_num + ";";
            msdr = mscm.ExecuteReader();
            msdr.Read();

            i_fname = (String)msdr["i_fname"];
            i_extname = (String)msdr["i_extname"];

            msdr.Close();

            mscm.CommandText = "SELECT MAX(i_row) AS 'Height', MAX(i_col) AS 'Width' FROM pixel WHERE i_num=" + i_num + ";";
            msdr = mscm.ExecuteReader();
            msdr.Read();

            outH = (int)msdr["Height"] + 1;
            outW = (int)msdr["Width"] + 1;

            msdr.Close();

            mscm.CommandText = "SELECT i_row, i_col, r_value, g_value, b_value FROM pixel WHERE i_num=" + i_num + ";";
            msdr = mscm.ExecuteReader();

            outImage = new byte[RGB, outH, outW];
            while (msdr.Read())
            {
                int i_row = (int)msdr["i_row"];
                int i_col = (int)msdr["i_col"];
                byte r_value = (byte)msdr["r_value"];
                byte g_value = (byte)msdr["g_value"];
                byte b_value = (byte)msdr["b_value"];

                outImage[RR, i_row, i_col] = r_value;
                outImage[GG, i_row, i_col] = g_value;
                outImage[BB, i_row, i_col] = b_value;
            }
            msdr.Close();
            MessageBox.Show("# [성공] 다운로드 되었습니다.");
            this.DialogResult = DialogResult.OK;
        }
        void select_db()
        {
            mscm.CommandText = "SELECT * FROM image;";
            msdr = mscm.ExecuteReader();

            listBox1.Items.Clear();
            while (msdr.Read())
            {
                String str = "  " + msdr["i_num"] + "\t" + msdr["i_fname"] + "." + msdr["i_extname"] + "      \t" + msdr["i_fsize"] + "\t" + msdr["i_width"] + "\t" + msdr["i_height"] + "\t" + msdr["i_user"];
                listBox1.Items.Add(str);
            }

            msdr.Close();
        }
    }
}
