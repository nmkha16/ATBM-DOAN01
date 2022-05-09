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
    public partial class InterfacePT : Form
    {
        public string _userID, _userName;
        private OracleConnection _con;
        private Login _login;
       
        public InterfacePT(Login login, OracleConnection con)
        {
            _login = login; 
            _con = con;
            InitializeComponent();
            FormClosing += Interface_FormClosing;


            getName();
        }

        private void Interface_FormClosing(object? sender, FormClosingEventArgs e)
        {
            _login.Show();
        }

        /// <summary>
        /// Xem thông tin cá nhân button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            Hide();
            InfoPT infoPT = new InfoPT(this,_userID, _con);
            infoPT.Show();
        }

        /// <summary>
        /// Lấy tên của bệnh nhân để hiển thị trên màn hình giao diện
        /// </summary>
        private void getName()
        {
            string query = "select tenbn from admin11.tc6_benhnhan";

            OracleCommand comm = new OracleCommand(query, _con);
            OracleDataReader reader = comm.ExecuteReader();
            if (reader.HasRows)
            {
                if (reader.Read())
                {
                    label1.Text = reader.GetString(0);
                }
            }
        }

        /// <summary>
        /// nút Xem hồ sơ của bệnh nhân
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            Hide();
            HoSoBN hoSoBN = new HoSoBN(this, _con);
            hoSoBN.Show();
        }
    }
}
