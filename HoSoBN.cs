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
    public partial class HoSoBN : Form
    {
        private DataTable data = new DataTable();
        private InterfacePT _itfPT;
        private OracleConnection _con;
        public HoSoBN(InterfacePT iftPT, string userName, OracleConnection con)
        {
            _itfPT = iftPT;
            _con = con;
            InitializeComponent();

            FormClosing += HoSoBN_FormClosing;

            label1.Text = userName;

            hoSoLoad();
            hoSoDichVuLoad();
        }

        private void HoSoBN_FormClosing(object? sender, FormClosingEventArgs e)
        {
            _itfPT.Show();
        }

        /// <summary>
        /// Đọc thông tin hồ sơ bệnh án và hiển thị lên giao diện
        /// </summary>
        private void hoSoLoad()
        {
            /*
            select hs.mabn,hs.ngay,hs.chuandoan,hs.ketluan,k.chuyenkhoa ,cs.tencsyt
            from hsba hs, csyt cs, khoa k
            where hs.mabn = <...> and hs.macsyt = cs.macsyt and k.makhoa = hs.makhoa;
            */
            string query = "select hs.mabn,hs.ngay,k.chuyenkhoa ,cs.tencsyt,hs.chuandoan,hs.ketluan " +
                "from admin11.hsba hs, admin11.csyt cs, admin11.khoa k " +
                "where hs.mabn = '" + _itfPT._userID.ToUpper() + "'and hs.macsyt = cs.macsyt and k.makhoa = hs.makhoa";

            OracleCommand comm = new OracleCommand(query, _con);
            OracleDataReader reader = comm.ExecuteReader();
            if (reader.HasRows)
            {
                if (reader.Read())
                {
                    textBox1.Text = reader.GetString(0); // mã bệnh nhân
                    textBox2.Text = reader.GetString(1);    // ngày lập hồ sơ
                    textBox5.Text = reader.GetString(2);    // tên khoa điều trị
                    textBox6.Text = reader.GetString(3);    // tên cơ sở y tế điều trị
                    textBox3.Text = reader.GetString(4);    // chuẩn đoán
                    textBox4.Text = reader.GetString(5);    // kết luận
                }
            }
        }

        /// <summary>
        /// Đọc thông tin các hồ sơ dịch vụ của khách hàng
        /// </summary>
        private void hoSoDichVuLoad()
        {
            string query = "select * from admin11.hsba_dv where mahsba = '" + _itfPT._userRecordID.ToUpper() + "'";
            OracleCommand com = new OracleCommand(query, _con);
            OracleDataReader dataReader = com.ExecuteReader();

            data.Load(dataReader);

            dataGridView1.DataSource = data;
        }
    }
}
