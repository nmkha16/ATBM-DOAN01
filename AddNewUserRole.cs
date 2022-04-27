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
        private bool isAlter;
        public AddNewUserRole(Interface In, OracleConnection con, bool checkUser_Role) //true == users, false == roles
        {
            
            InitializeComponent();
            isAlter = false;
            this.In = In;
            this.con = con;
            FormClosing += AddNewUserRole_FormClosing;
            isCreatingUser = checkUser_Role;
            checkBox1.Hide();
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

        
        // constructor to edit role password
        public AddNewUserRole(Interface In,OracleConnection con,string name)
        {
            InitializeComponent();
            isAlter = true;
            this.In = In;
            this.con = con;
            FormClosing += AddNewUserRole_FormClosing;
            checkBox1.Show();
            textBox1.Text = name;
            textBox1.Enabled = false;
            label1.Text = "Role";
            //set title
            this.Text="Edit Role Password";

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
                    textBox2.Text + textBox1.Text;
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
            if (isAlter)
            {
                if (checkInput.hasSpecialCharacter(textBox2.Text))
                {
                    MessageBox.Show("Role password should not include special char!", "Alert");
                    return;
                }
                // check if password textbox is injection, role password doesn't need to have special chars.
                //build the query
                string query = "alter role " + textBox1.Text;
                if (checkBox1.Checked)
                {
                    query += " not identified";
                }
                else
                    query += " identified by " + textBox2.Text;
                // call function to send query to oracle
                alterRole(query);
            }
            else NewUserRole(isCreatingUser);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            In.Show();
        }


        // set identified by ..., this will disable passsword textBox
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                textBox2.Enabled = false;
            }
            else
                textBox2.Enabled = true;
        }

        // function to alter role
        private void alterRole(string query)
        {
            OracleCommand oracleCommand = new OracleCommand(query, con);
            try
            {
                oracleCommand.ExecuteNonQuery();
                MessageBox.Show("Alter role " + textBox1.Text + " successfully!", "Alert");
            }
            catch
            {
                MessageBox.Show("Alter role " + textBox1.Text + " failed!", "Alert");
            }
        }

    }
}
