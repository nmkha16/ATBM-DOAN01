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
    public partial class Interface : Form
    {
        private Login loginForm;
        private OracleConnection con;
        public Interface(Login login, OracleConnection con)
        {
            InitializeComponent();
            loginForm = login;
            this.con = con;
            // add closing event
            FormClosing += Interface_FormClosing;

        }

        // closing event on winform
        private void Interface_FormClosing(object? sender, FormClosingEventArgs e)
        {
            loginForm.Show();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }


    }
}
