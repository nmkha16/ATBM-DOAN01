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
        private bool isCreatingUser;
        public AddNewUserRole(Interface In, OracleConnection con, bool checkUser_Role) //true == users, false == roles
        {
            
            InitializeComponent();
            this.In = In;
            this.con = con;
            FormClosing += AddNewUserRole_FormClosing;
            isCreatingUser = checkUser_Role;

            // open form as creating user
            if(checkUser_Role)
            {
                this.label1.Text = "New user";
                this.Text = "Creating new user";    // set title
            }
            else
            {
                this.label1.Text = "New role";
                this.Text = "Creating new role"; // set role
                this.label2.Hide();
                this.textBox2.Hide();
            }
        }
        private void NewUserRole(bool checkUser_Role)
        {
            if (checkInput.hasSpecialCharacter(textBox1.Text))
            {
                MessageBox.Show("Only accept letters and numeric alphabet in user!!!");
                return;
            }

            string query;
            if(checkUser_Role)// true mean end user is creating user
            {
                query = "create user " + textBox1.Text + " identified by " + 
                    Hash.getHashSha256(textBox2.Text + textBox1.Text).Substring(0, 30);
                OracleCommand oracleCommand = new OracleCommand(query,con);
                try
                {
                    oracleCommand.ExecuteNonQuery();
                    MessageBox.Show("Create user " + textBox1.Text + " successfully!", "Alert");
                }
                catch
                {
                    MessageBox.Show("Create user " + textBox1.Text + " failed!", "Alert");
                }
            }
            else
            {
                query = "create role " + textBox1.Text;
                OracleCommand oracleCommand = new OracleCommand(query, con);
                try
                {
                    oracleCommand.ExecuteNonQuery();
                    MessageBox.Show("Create role " + textBox1.Text + " successfully!", "Alert");
                }
                catch
                {
                    MessageBox.Show("Create role " + textBox1.Text + " failed!", "Alert");
                }
            }

        }

        private void AddNewUserRole_FormClosing(object? sender, FormClosingEventArgs e)
        {
            this.In.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            NewUserRole(isCreatingUser);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            In.Show();
        }
    }
}
