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
        private DataTable data;
        private InterfaceBN _itfPT;
        private InterfaceNC _itfNC;
        private OracleConnection _con;

        public HoSoBN(InterfaceNC iftNC, OracleConnection con)
        {
            _itfNC = iftNC;
            _con = con;
            InitializeComponent();
            FormClosing += HoSoBN_FormClosing;

            label1.Text = _itfNC.getName(0);
        }

        public HoSoBN(InterfaceBN iftPT, OracleConnection con)
        {
            _itfPT = iftPT;
            _con = con;
            InitializeComponent();
            FormClosing += HoSoBN_FormClosing1;

            label10.Hide();
            comboBox1.Hide();
            textBox7.Hide();
            button1.Hide();
            button2.Hide();

            label1.Text = getName();

            getHSBA("select * from admin11.tc6_HSBA");
            getHSBA_DV("select * from admin11.tc6_hsba_dv");
        }

        private void HoSoBN_FormClosing1(object? sender, FormClosingEventArgs e)
        {
            _itfPT.Show();
        }

        private void HoSoBN_FormClosing(object? sender, FormClosingEventArgs e)
        {
            _itfNC.Show();
        }

        private void getHSBA(string query)
        {
            string maHSBA = "";
            OracleCommand command = new OracleCommand(query,_con);
            OracleDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                if (reader.Read())
                {
                    maHSBA = reader.GetString(0);
                    textBox1.Text = reader.GetString(1);
                    textBox2.Text = reader.GetString(2);
                    textBox3.Text = reader.GetString(3);
                    textBox4.Text = reader.GetString(7);
                }
                getHSBA_DV("select * from admin11.tc4_hsba_dv where mahsba = '" + maHSBA + "'");
            }
            else
            {
                MessageBox.Show("Không tìm thấy dữ liệu cho " + textBox7.Text + "!!!", "Thông báo");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (checkInput.hasSpecialCharacter(textBox7.Text))
            {
                MessageBox.Show("Ô tìm kiếm nhập không hợp lệ", "Thông báo");
                return;
            }

            string query = "";
            if (comboBox1.SelectedIndex == 0)
            {
                query = "select * from admin11.tc4_hsba where mabn like '" + textBox7.Text +"%'";
            }
            else if(comboBox1.SelectedIndex == 1)
            {
                query = "select * from admin11.tc4_hsba where mabn like '" + getMaBN() + "%'";
            }

            if (query == "") return;

            getHSBA(query);
        }


        private string getMaBN()
        {
            string query = "select mabn from admin11.tc4_benhnhan where cmnd like '" + textBox7.Text + "%' and rownum = 1";
            OracleCommand command = new OracleCommand(query, _con);
            OracleDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                if (reader.Read())
                {
                    return reader.GetString(0);
                }
            }
            return null;
        }

        private string getName()
        {
            string query = "select tenbn from admin11.tc6_benhnhan_vpd";
            OracleCommand command = new OracleCommand(query, _con);
            OracleDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                if (reader.Read())
                {
                    return reader.GetString(0);
                }
            }
            return null;
        }

        private void getHSBA_DV(string query) {            
            try
            {
                data.Clear();
            }
            catch { }
            try
            {
                OracleCommand command = new OracleCommand(query, _con);
                OracleDataReader oraReader = command.ExecuteReader();
                // bind return select result to DataTable
                data = new DataTable();

                data.Load(oraReader);
                // bind data to table aka datagridview        
                dataGridView1.DataSource = data;

            }
            catch
            {
                //MessageBox.Show("Error getting result!", "Alert");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Hide();
            InfoBN infoBN = new InfoBN(this,textBox1.Text,_con);
            infoBN.Show();
        }
    }
}
