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
    public partial class InfoBN : Form
    {
        private OracleConnection _con;
        private InterfaceBN _iftPT;
        private HoSoBN _hoSoBN;
        ToolTip t = new ToolTip();
        public InfoBN(InterfaceBN iftPT, OracleConnection con)
        {
            _iftPT = iftPT;
            _con = con;
            InitializeComponent();

            FormClosing += InfoBN_FormClosing;
            getBNInfo("Select * from admin11.tc6_benhnhan_vpd");
        }

        public InfoBN(HoSoBN hoSoBN,string maHSBA ,OracleConnection con)
        {
            _hoSoBN=hoSoBN;
            _con=con;
            InitializeComponent();
            FormClosing += InfoBN_FormClosing1;

            button1.Enabled = false;
            textBox1.ReadOnly = true;
            textBox2.ReadOnly = true;
            textBox3.ReadOnly = true;
            textBox4.ReadOnly = true;
            textBox5.ReadOnly = true;
            textBox8.ReadOnly = true;
            textBox9.ReadOnly = true;
            textBox6.ReadOnly = true;
            textBox7.ReadOnly = true;
            textBox10.ReadOnly = true;
            textBox11.ReadOnly = true;

            getBNInfo("select * from admin11.tc4_benhnhan where mabn = '" + maHSBA + "'");
        }

        private void InfoBN_FormClosing1(object? sender, FormClosingEventArgs e)
        {
            _hoSoBN.Show();
        }

        private void InfoBN_FormClosing(object? sender, FormClosingEventArgs e)
        {
            _iftPT.Show();
        }

        /// <summary>
        /// Lấy thông tin bệnh nhân và hiện lên giao hiện
        /// </summary>
        private void getBNInfo(string query)
        {            
            OracleCommand comm = new OracleCommand(query, _con);
            OracleDataReader reader = comm.ExecuteReader();
            if (reader.HasRows)
            {
                if (reader.Read())
                {
                    textBox1.Text = reader.GetString(2); // họ tên
                    textBox2.Text = reader.GetString(3); // cmnd
                    textBox3.Text = reader.GetString(4);    // ngày sinh
                    textBox6.Text = reader.GetString(5);    // số nhà
                    textBox7.Text = reader.GetString(6);    // tên đường
                    textBox10.Text = reader.GetString(7);    // quận/huyện
                    textBox11.Text = reader.GetString(8);    //tỉnh/thànhphố                   
                    textBox4.Text=reader.GetString(9);  // tiền sử bệnh
                    textBox8.Text=reader.GetString(10); // tiền sử bệnh gia đình
                    textBox9.Text=reader.GetString(11); // dị ứng thuốc
                }
            }
        }

        private void concatAddress()
        {
            textBox5.Text = textBox6.Text + ", " + textBox7.Text + ", " + textBox10.Text + ", " + textBox11.Text + ", ";
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            concatAddress();
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            concatAddress();
        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {
            concatAddress();
        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {
            concatAddress();
        }

        private void updateRecord()
        {

            string query = "update admin11.tc6_benhnhan_vpd " +
                "set tenbn = :tenbn," +
                 "cmnd = :cmnd," +
                "ngaysinh = to_date(:ngaysinh,'DD/MM/YYYY')," +
                "sonha = :sonha," +
                "tenduong = :tenduong," +
                "quanhuyen = :quanhuyen," +
                "tinhtp = :tinhtp," +
                "tiensubenh = :tiensubenh," +
                "tiensubenhgd = :tiensubenhgd," +
                "diungthuoc = :diungthuoc ";

            OracleCommand command = new OracleCommand(query, _con);

            command.Parameters.Add("tenbn",textBox1.Text);
            command.Parameters.Add("cmnd",textBox2.Text);
            command.Parameters.Add("ngaysinh",textBox3.Text);
            command.Parameters.Add("sonha",textBox6.Text);
            command.Parameters.Add("tenduong",textBox7.Text);
            command.Parameters.Add("quanhuyen",textBox10.Text);
            command.Parameters.Add("tinhtp",textBox11.Text);
            command.Parameters.Add("tiensubenh",textBox4.Text);
            command.Parameters.Add("tiensubenhgd",textBox8.Text);
            command.Parameters.Add("diungthuoc",textBox9.Text);

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

        private void textBox3_MouseEnter(object sender, EventArgs e)
        {
            t.Show("Nhập định dạng DD/MM/YYYY", textBox3, 3000);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            updateRecord();
        }
    }
}
