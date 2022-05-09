using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;

namespace ATBM_DOAN01
{
    public partial class CSYT_HSBA : Form
    {
        private Login _login;
        private OracleConnection _con;
        private DataTable data1, data2;
        private DataTable dataCN1, dataCN2;
        private string mahsba;
        private string macsyt;
        public CSYT_HSBA(Login login, OracleConnection con)
        {
            _login = login;
            _con = con;
            InitializeComponent();
            FormClosing += CSYT_HSBA_FormClosing;

            getName();
            getHSBA();
        }

        private void CSYT_HSBA_FormClosing(object? sender, FormClosingEventArgs e)
        {
            _login.Show();
        }


        /// <summary>
        /// Lấy tên nhân viên
        /// </summary>
        private void getName()
        {
            string query = "select hoten,csyt from admin11.TC6_NHANVIEN";

            OracleCommand comm = new OracleCommand(query, _con);
            OracleDataReader reader = comm.ExecuteReader();
            if (reader.HasRows)
            {
                if (reader.Read())
                {
                    label1.Text = reader.GetString(0);
                    macsyt = reader.GetString(1);
                }
            }
        }


        /// <summary>
        /// Lấy các thông tin hồ sơ bệnh án từ ngày 5 đến ngày 27 của tháng hiện tại
        /// </summary>
        private void getHSBA()
        {
            string query = "select * from admin11.tc3_hsba";
            try
            {
                data1.Clear();
            }
            catch { }
            try
            {
                OracleCommand command = new OracleCommand(query, _con);
                OracleDataReader oraReader = command.ExecuteReader();
                // bind return select result to DataTable
                data1 = new DataTable();

                data1.Load(oraReader);
                // bind data to table aka datagridview        
                dataGridView1.DataSource = data1;

            }
            catch
            {
                MessageBox.Show("Error getting result!", "Alert");
            }
        }

        /// <summary>
        /// Thêm một hồ sơ bệnh án mới
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            List<string?> hoSoBenhAn = new List<string?>();
            for (int i = 0; i < dataGridView2.ColumnCount; i++)
            {
                hoSoBenhAn.Add(dataGridView2.Rows[0].Cells[i].Value.ToString());
            }

            string query = "insert into admin11.tc3_hsba(mahsba,mabn,ngay,chuandoan,mabs,makhoa,macsyt,ketluan) " +
                "Values (:mahsba,:mabn,sysdate,:chuandoan,:mabs,:makhoa,:macsyt,:ketluan)";
            OracleCommand command = new OracleCommand(query, _con);
            // manually add value to parameter
            command.Parameters.Add("mahsba",hoSoBenhAn[0] == null? DBNull.Value: hoSoBenhAn[0].ToUpper());
            command.Parameters.Add("mabn",hoSoBenhAn[1] == null? DBNull.Value: hoSoBenhAn[1].ToUpper());
            command.Parameters.Add("chuandoan",hoSoBenhAn[3] == null? DBNull.Value: hoSoBenhAn[3]);
            command.Parameters.Add("mabs",hoSoBenhAn[4] == null? DBNull.Value: hoSoBenhAn[4].ToUpper());
            command.Parameters.Add("makhoa",hoSoBenhAn[5] == null? DBNull.Value: hoSoBenhAn[5].ToUpper());
            command.Parameters.Add("macsyt",macsyt);
            command.Parameters.Add("ketluan",hoSoBenhAn[7] == null? DBNull.Value: hoSoBenhAn[7]);

            try // will throw exception if violate primary key constraint
            {
                int row = command.ExecuteNonQuery();

                if (row > 0)
                {
                    MessageBox.Show("Thêm hồ sơ bệnh án mới thành công", "Thông báo");
                }
                else
                {
                    MessageBox.Show("Thêm hồ sơ bệnh án mới thất bại!", "Thông báo");
                }
            }
            catch
            {
                MessageBox.Show("Thêm hồ sơ bệnh án mới thất bại!", "Thông báo");
            }
            finally
            {
                getHSBA();//refresh lại danh sách
            }
        }

        /// <summary>
        /// khi người dùng xoá một hồ sơ bệnh án ở khung cập nhật
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            string query = "delete from admin11.tc3_hsba where mahsba = '" + mahsba + "'";
            OracleCommand command = new OracleCommand(query, _con);

            try 
            {
                int row = command.ExecuteNonQuery();

                if (row > 0)
                {
                    MessageBox.Show("Xoá hồ sơ bệnh án mới thành công", "Thông báo");
                }
                else
                {
                    MessageBox.Show("Xoá hồ sơ bệnh án mới thất bại!", "Thông báo");
                }
            }
            catch
            {
                MessageBox.Show("Xoá hồ sơ bệnh án mới thất bại!", "Thông báo");
            }
            finally
            {
                getHSBA();//refresh lại danh sách
                try // thử xoá các dữ liệu của ô cập nhật trên gui
                {
                    dataCN1.Clear();
                }
                catch { }
            }
        }

        /// <summary>
        /// nút thêm hồ sơ bệnh án dịch vụ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            List<string?> hoSoBenhAnDV = new List<string?>();
            for (int i = 0; i < dataGridView3.ColumnCount; i++)
            {
                hoSoBenhAnDV.Add(dataGridView3.Rows[0].Cells[i].Value.ToString());
            }

            string query = "insert into admin11.tc3_hsba_dv(mahsba,madv,ngay,maktv,ketqua) " +
                "Values (:mahsba,:madv,sysdate,:maktv,:ketqua)";
            OracleCommand command = new OracleCommand(query, _con);
            // manually add value to parameter
            command.Parameters.Add("mahsba", hoSoBenhAnDV[0] == null ? DBNull.Value : hoSoBenhAnDV[0].ToUpper());
            command.Parameters.Add("madv", hoSoBenhAnDV[1] == null ? DBNull.Value : hoSoBenhAnDV[1].ToUpper());
            command.Parameters.Add("maktv", hoSoBenhAnDV[3] == null ? DBNull.Value : hoSoBenhAnDV[3]);
            command.Parameters.Add("ketqua", hoSoBenhAnDV[4] == null ? DBNull.Value : hoSoBenhAnDV[4].ToUpper());

            try // will throw exception if violate primary key constraint
            {
                int row = command.ExecuteNonQuery();

                if (row > 0)
                {
                    MessageBox.Show("Thêm hồ sơ bệnh án dịch vụ mới thành công", "Thông báo");
                }
                else
                {
                    MessageBox.Show("Thêm hồ sơ bệnh án dịch vụ mới thất bại!", "Thông báo");
                }
            }
            catch
            {
                MessageBox.Show("Thêm hồ sơ bệnh án dịch vụ mới thất bại!", "Thông báo");
            }
            finally
            {
                getHSBA();//refresh lại danh sách
            }
        }

        /// <summary>
        /// nút xoá hồ sơ bệnh án dịch vụ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Khi người dùng chọn 1 row bên hồ sơ bệnh án dịch vụ thì tự động điền cell vào ô cập nhật
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView4_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            string madv = dataGridView4.Rows[e.RowIndex].Cells[0].Value.ToString();
            string ngay = dataGridView4.Rows[e.RowIndex].Cells[1].Value.ToString().Split(' ')[0];

            string query = "select * from admin11.tc3_hsba_dv where mahsba = '" + mahsba + "' and madv = '" + madv + "' " +
                    "and ngay like to_date('" + ngay + "','MM/DD/YYYY')";

            try
            {
                dataCN2.Clear();
            }
            catch { }

            try
            {
                OracleCommand command = new OracleCommand(query, _con);
                OracleDataReader oraReader = command.ExecuteReader();
                // bind return select result to DataTable
                dataCN2 = new DataTable();

                dataCN2.Load(oraReader);

                // bind data to table aka datagridview        
                dataGridView3.DataSource = dataCN2;

                dataGridView3.Columns["MAHSBA"].ReadOnly = true;
                dataGridView3.Columns["NGAY"].ReadOnly = true;

                dataGridView3.Rows[0].Cells[0].Value = mahsba;
                dataGridView3.Rows[0].Cells[2].Value = DateTime.Now.ToString("dd/MMM/yyyy");
            }
            catch
            {
                MessageBox.Show("Error getting result!", "Alert");
            }
        }
        

        /// <summary>
        /// khi người dùng chọn 1 cell thì tự động điền cell vào ô cập nhập
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            mahsba = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            if (mahsba == null) return;

            string query1 = "select * from admin11.tc3_hsba where mahsba = '"+ mahsba+"'";
            string query2 = "select madv,ngay,maktv,ketqua from admin11.tc3_hsba_dv where mahsba = '" + mahsba+"'";

            try
            {
                dataCN1.Clear();
                data2.Clear();
            }
            catch { }
            try
            {

                OracleCommand command = new OracleCommand(query1, _con);
                OracleDataReader oraReader = command.ExecuteReader();
                // bind return select result to DataTable
                dataCN1 = new DataTable();

                dataCN1.Load(oraReader);

                command = new OracleCommand(query2 , _con);
                oraReader = command.ExecuteReader();

                data2 = new DataTable();
                data2.Load(oraReader);


                // bind data to table aka datagridview        
                dataGridView2.DataSource = dataCN1;
                dataGridView4.DataSource = data2;

                dataGridView2.Columns["NGAY"].ReadOnly = true;
                dataGridView2.Columns["MACSYT"].ReadOnly = true;

                try
                {
                    dataGridView3.Rows[0].Cells[0].Value = mahsba;
                    dataGridView3.Rows[0].Cells[2].Value = DateTime.Now.ToString("dd/MMM/yyyy");
                }
                catch { }

            }
            catch
            {
                MessageBox.Show("Error getting result!", "Alert");
            }

        }
    }
}
