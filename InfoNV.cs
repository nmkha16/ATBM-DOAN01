using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ATBM_DOAN01
{
    public partial class InfoNV : Form
    {
        private InterfaceNC _itf;
        private OracleConnection _con;
        ToolTip t = new ToolTip();
        public InfoNV(InterfaceNC itf, OracleConnection con)
        {
            _itf = itf;
            _con = con;
            InitializeComponent();
            FormClosing += InfoNV_FormClosing;
            getInfo();
        }

        private void InfoNV_FormClosing(object? sender, FormClosingEventArgs e)
        {
            _itf.Show();
        }

        private void getInfo()
        {
            string query = "select * from admin11.tc6_nhanvien_vpd";
            OracleCommand comm = new OracleCommand(query, _con);
            OracleDataReader reader = comm.ExecuteReader();
            if (reader.HasRows)
            {
                if (reader.Read())
                {
                    textBox1.Text = reader.GetString(1);
                    textBox2.Text = reader.GetString(2);
                    textBox3.Text = reader.GetString(3);
                    textBox4.Text = reader.GetString(4);
                    textBox5.Text = reader.GetString(5);
                    textBox6.Text = reader.GetString(6);
                    textBox7.Text = reader.GetString(8);
                }
            }
        }

        private void updateRecord()
        {
            if (textBox2.Text != "Nữ" && textBox2.Text != "Nam")
            {
                MessageBox.Show("Giới tính nhập không hợp lệ", "Thông báo");
                return;
            }


            string query = "update admin11.tc6_nhanvien_vpd " +
                "set hoten = :hoten," +
                 "phai = :phai," +
                "ngaysinh = to_date(:ngaysinh,'DD/MM/YYYY')," +
                "cmnd = :cmnd," +
                "quequan = :quequan," +
                "sodt = :sodt ";

            OracleCommand command = new OracleCommand(query, _con);

            command.Parameters.Add("hoten",textBox1.Text);
            command.Parameters.Add("phai",textBox2.Text);
            command.Parameters.Add("ngaysinh",textBox3.Text);
            command.Parameters.Add("cmnd",textBox4.Text);
            command.Parameters.Add("quequan",textBox5.Text);
            command.Parameters.Add("sodt",textBox6.Text);

            try
            {
                command.ExecuteNonQuery();

                MessageBox.Show("Cập nhật thông tin mới thành công!", "Thông báo");
            }
            catch
            {
                MessageBox.Show("Cập nhật thông tin mới thất bại!", "Thông báo");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            updateRecord();
        }

        private void textBox3_MouseEnter(object sender, EventArgs e)
        {
            t.Show("Nhập định dạng DD/MM/YYYY", textBox3, 3000);
        }
    }
}
