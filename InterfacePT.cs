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
        private string _userID;
        private OracleConnection _con;
        public InterfacePT(string name,string userID, OracleConnection con)
        {
            _con = con;
            _userID = userID;
            InitializeComponent();
            label1.Text = name;// patient name
        }

        /// <summary>
        /// Xem thông tin cá nhân button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
