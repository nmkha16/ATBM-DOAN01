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
    public partial class InfoPT : Form
    {
        private string _userId;
        private OracleConnection _con;
        public InfoPT(string userID, OracleConnection con)
        {
            _userId = userID;
            _con = con;
            InitializeComponent();
        }
        /// <summary>
        /// Lấy thông tin bệnh nhân và hiện lên giao hiện
        /// </summary>
        private void InfoPT_Load()
        {
            string query = "";

            OracleCommand comm = new OracleCommand(query, _con);
            OracleDataReader reader = comm.ExecuteReader();
            if (reader.HasRows)
            {
                for(int i = 0; i < reader.RowSize; i++)
                {
                    reader[0].ToString();
                }
            }
        }
    }
}
