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

            // combobox for user/roles
            comboBox2.DisplayMember = "Text";
            comboBox2.ValueMember = "Value";
            //combobox for function
            comboBox1.DisplayMember = "Text";
            comboBox1.ValueMember = "Value";
            // add item to left combo box
            comboBox2.Items.Add(new { Text = "Roles", Value = "Roles" });
            comboBox2.Items.Add(new { Text = "Users", Value = "Users" });
            // set default selected combo box
            //comboBox2.SelectedIndex = 0;
            // add item to right combo box
            //-> add your function name here
            comboBox1.Items.Add(new { Text = "Xem thông tin quyền", Value = "Priv_Info" });
            //
            comboBox1.Items.Add(new { Text = "Tạo mới user", Value = "Create_User" });


            // set variable 
            loginForm = login;
            this.con = con;

            // add closing event
            FormClosing += Interface_FormClosing;

        }

        // closing event on winform
        private void Interface_FormClosing(object? sender, FormClosingEventArgs e)
        {
            con.Close();
            loginForm.Show();
        }

        /// <summary>
        /// handle comboxbox selecting user or role
        /// will print users or roles in pdb
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedIndex == 0) //roles
            {
                getUserRole("select role from dba_roles where common = 'NO'");
            }
            else if (comboBox2.SelectedIndex == 1) // users
            {
                getUserRole("SELECT username FROM dba_users " +
                    "where oracle_maintained = 'N' and username not like 'ADMIN%'");
            }            
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
        //select index change
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            // create new user
           if(comboBox1.SelectedIndex == 1 && comboBox2.SelectedIndex==1)
            {
                openAddNewUserForm(true);
            }
           else if (comboBox1.SelectedIndex == 1 && comboBox2.SelectedIndex == 0)
            {
                openAddNewUserForm(false);
            }
        }
        private void openAddNewUserForm(bool checkUser_Role)
        {
            this.Hide();
            AddNewUserRole itf = new AddNewUserRole(this, con,checkUser_Role);
            itf.Show();
        }

        // method to handle select user/roles combobox
        // populate list box
        private void getUserRole(string query)
        {
            // clear the listbox first to prevent stacking item
            listBox1.Items.Clear();

            OracleCommand command = new OracleCommand(query, con);
            OracleDataReader oraReader = command.ExecuteReader();
            if (oraReader.HasRows)
            {
                while (oraReader.Read())
                {
                    listBox1.Items.Add(oraReader.GetString(0));
                }
            }
            else
            {
                listBox1.Items.Clear();
            }
            oraReader.Close();
        }


        // handle list of user\role listbox
        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            // function is view privilege info
            if (comboBox1.SelectedIndex == 0)
            {

            }
        }
    }

    
}
