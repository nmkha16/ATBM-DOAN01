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
    public partial class CSYT_NV_DBA : Form
    {
        private Interface _itf;
        private OracleConnection _con;
        private DataTable data, dataCN;
        public CSYT_NV_DBA(Interface itf,OracleConnection con)
        {
            _itf = itf;
            _con = con;
            InitializeComponent();
            FormClosing += CSYT_NV_DBA_FormClosing;
        }

        private void CSYT_NV_DBA_FormClosing(object? sender, FormClosingEventArgs e)
        {
            _itf.Show();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            refresh();
        }


        private void refresh()
        {
            string query = "";
            if (comboBox1.SelectedIndex == 0) // bảng nhân viên
            {
                query = "select * from nhanvien";
            }
            else if (comboBox1.SelectedIndex == 1) // bảng CSYT
            {
                query = "select * from csyt";
            }


            try
            {
                data.Clear();
            }
            catch { }


            OracleCommand command = new OracleCommand(query, _con);
            OracleDataReader reader = command.ExecuteReader();
            data = new DataTable();
            data.Load(reader);

            dataGridView1.DataSource = data;
        }

        /// <summary>
        /// nút thêm
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            string query;
            OracleCommand command = null;
            List<string?> list = new List<string?>();
            if (comboBox1.SelectedIndex == 0) // bảng nhân viên
            {              
                for (int i = 0; i < dataGridView2.ColumnCount; i++)
                {
                    list.Add(dataGridView2.Rows[0].Cells[i].Value.ToString());
                }

                query = "insert into nhanvien(manv,hoten,phai,ngaysinh,cmnd,quequan,sodt,csyt,vaitro,makhoa) " +
                    "values(:manv,:hoten,:phai,:ngaysinh,:cmnd,:quequan,:sodt,:csyt,:vaitro,:makhoa)";

                command = new OracleCommand(query, _con);
                command.Parameters.Add("manv",list[0] == null ? DBNull.Value : list[0].ToUpper());
                command.Parameters.Add("hoten", list[1] == null ? DBNull.Value : list[1]);
                command.Parameters.Add("phai", list[2] == null ? DBNull.Value : list[2]);
                command.Parameters.Add("ngaysinh", list[3] == null ? DBNull.Value : DateTime.Parse(list[3]));
                command.Parameters.Add("cmnd", list[4] == null ? DBNull.Value : list[4]);
                command.Parameters.Add("quequan", list[5] == null ? DBNull.Value : list[5]);
                command.Parameters.Add("sodt", list[6] == null ? DBNull.Value : list[6]);
                command.Parameters.Add("csyt", list[7] == null ? DBNull.Value : list[7].ToUpper());
                command.Parameters.Add("vaitro", list[8] == null ? DBNull.Value : list[8]);
                command.Parameters.Add("makhoa", list[9] == null ? DBNull.Value : list[9].ToUpper());
            }
            else if (comboBox1.SelectedIndex == 1) // bảng CSYT
            {
                for (int i = 0; i < dataGridView2.ColumnCount; i++)
                {
                    list.Add(dataGridView2.Rows[0].Cells[i].Value.ToString());
                }
                query = "insert into csyt(macsyt,tencsyt,dccsyt,sdtcsyt) " +
                    "values (:macsyt,:tencsyt,:dccsyt,:sdtcsyt)";
                command = new OracleCommand(query, _con);
                command.Parameters.Add("macsyt", list[0] == null ? DBNull.Value : list[0].ToUpper());
                command.Parameters.Add("tencsyt", list[1] == null ? DBNull.Value : list[1]);
                command.Parameters.Add("dccsyt", list[2] == null ? DBNull.Value : list[2]);
                command.Parameters.Add("sdtcsyt", list[3] == null ? DBNull.Value : list[3]);
            }

            if (command == null) return;

            try
            {
                command.ExecuteNonQuery();
                MessageBox.Show("Thêm dữ liệu thành công!", "Thông báo");
                refresh();
            }
            catch (OracleException ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// nút sửa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            string query = "";
            OracleCommand command = null;
            List<string?> list = new List<string?>();

            if (comboBox1.SelectedIndex == 0) // bảng nhân viên
            {
                for (int i = 0; i < dataGridView2.ColumnCount; i++)
                {
                    list.Add(dataGridView2.Rows[0].Cells[i].Value.ToString());
                }

                query = "update nhanvien " +
                    "set manv = :manv," +
                    "hoten = :hoten," +
                    "phai = :phai," +
                    "ngaysinh = :ngaysinh," +
                    "cmnd = :cmnd," +
                    "quequan = :quequan," +
                    "sodt = :sodt," +
                    "csyt = :csyt," +
                    "vaitro = :vaitro," +
                    "makhoa = :makhoa " +
                    "where manv = :manv1";

                command = new OracleCommand(query, _con);

                command.Parameters.Add("manv", list[0] == null ? DBNull.Value : list[0].ToUpper());
                command.Parameters.Add("hoten", list[1] == null ? DBNull.Value : list[1]);
                command.Parameters.Add("phai", list[2] == null ? DBNull.Value : list[2]);
                command.Parameters.Add("ngaysinh", list[3] == null ? DBNull.Value : DateTime.Parse(list[3]));
                command.Parameters.Add("cmnd", list[4] == null ? DBNull.Value : list[4]);
                command.Parameters.Add("quequan", list[5] == null ? DBNull.Value : list[5]);
                command.Parameters.Add("sodt", list[6] == null ? DBNull.Value : list[6]);
                command.Parameters.Add("csyt", list[7] == null ? DBNull.Value : list[7].ToUpper());
                command.Parameters.Add("vaitro", list[8] == null ? DBNull.Value : list[8]);
                command.Parameters.Add("makho", list[9] == null ? DBNull.Value : list[9].ToUpper());
                command.Parameters.Add("manv1",list[0] == null ? DBNull.Value : list[0].ToUpper());
            }
            else if (comboBox1.SelectedIndex == 1) // bảng CSYT
            {
                for (int i = 0; i < dataGridView2.ColumnCount; i++)
                {
                    list.Add(dataGridView2.Rows[0].Cells[i].Value.ToString());
                }
                query = "update csyt " +
                    "set macsyt = :macsyt," +
                    "tencsyt = :tencsyt," +
                    "dccsyt = :dccsyt," +
                    "sdtcsyt = :sdtcsyt " +
                    "where macsyt = :macsyt1";

                command = new OracleCommand(query, _con);

                command.Parameters.Add("macsyt", list[0] == null ? DBNull.Value : list[0].ToUpper());
                command.Parameters.Add("tencsyt", list[1] == null ? DBNull.Value : list[1]);
                command.Parameters.Add("dccsyt", list[2] == null ? DBNull.Value : list[2]);
                command.Parameters.Add("sdtcsyt", list[3] == null ? DBNull.Value : list[3]);
                command.Parameters.Add("macsyt1", list[0] == null ? DBNull.Value : list[0].ToUpper());

            }

            if (command == null) return;

            try
            {
                command.ExecuteNonQuery();
                MessageBox.Show("Cập nhật dữ liệu thành công", "Thông báo");
                refresh();
            }
            catch (OracleException ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// nút xoá
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            string query = "";
            
            if (comboBox1.SelectedIndex == 0) // bảng nhân viên
            {
                query = "delete from nhanvien where manv = '" + dataGridView2.Rows[0].Cells[0].Value.ToString()+"'";
            }
            else if (comboBox1.SelectedIndex == 1) // bảng CSYT
            {
                query = "delete from csyt where macsyt = '" + dataGridView2.Rows[0].Cells[0].Value.ToString() + "'";
            }
            OracleCommand command = new OracleCommand(query,_con);
            try
            {
                command.ExecuteNonQuery();
                MessageBox.Show("Xoá dữ liệu thành công", "Thông báo");
                refresh();
            }
            catch (OracleException ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            string query = "";
            if (comboBox1.SelectedIndex == 0) // bảng nhân viên
            {
                query = "select * from nhanvien where manv = '" + dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString()+"'";
            }
            else if (comboBox1.SelectedIndex == 1) // bảng CSYT
            {
                query = "select * from csyt where macsyt = '" + dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString() + "'";
            }

            try
            {
                dataCN.Clear();
            }
            catch { }


            OracleCommand command = new OracleCommand(query, _con);
            OracleDataReader reader = command.ExecuteReader();
            dataCN = new DataTable();
            dataCN.Load(reader);

            dataGridView2.DataSource = dataCN;
        }
    }
}
