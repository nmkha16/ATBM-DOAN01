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
    public partial class InterfaceNC : Form
    {
        
        private OracleConnection _con;
        private Login _login;

        private void button2_Click(object sender, EventArgs e)
        {
            Hide();
            HoSoNC hsNC = new HoSoNC(this,  _con);
            hsNC.Show();
        }

        private void InterfaceNC_FormClosing(object sender, FormClosingEventArgs e)
        {
            _login.Show();
        }

        
        public InterfaceNC(Login login, OracleConnection con)
        {
            _login = login;
            _con = con;
            InitializeComponent();
            getName();
        }

        private void getName()
        {
            string query = "select hoten from admin11.TC6_NHANVIEN";

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
    }
}
