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
    public partial class AddNewUserRole : Form
    {
        private Interface In;
        private OracleConnection con;
        public AddNewUserRole(Interface In, OracleConnection con, bool checkUser_Role) //true == users, false == roles
        {
            
            InitializeComponent();
            this.In = In;
            this.con = con;
            FormClosing += AddNewUserRole_FormClosing;
            if(checkUser_Role)
            {
                this.label1.Text = "New user";
            }
            else
            {
                this.label1.Text = "New role";
            }
        }
        private void NewUserRole(bool checkUser_Role)
        {
            string query;
            if(checkUser_Role)
            {
                var commandText = "create user :userName identified by :passWord";
                OracleCommand oracleCommand = new OracleCommand(commandText,con);
                oracleCommand.Parameters.Add("userName", textBox1.Text);
                oracleCommand.Parameters.Add("passWord", textBox2.Text);
                oracleCommand.ExecuteNonQuery();
            }
            else
            {
                query = "create role " + textBox1.Text;
            }

        }

        private void AddNewUserRole_FormClosing(object? sender, FormClosingEventArgs e)
        {
            this.In.Show();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
