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
        private DataTable data; // for getting query result to datagridview
        public Interface(Login login, OracleConnection con)
        {
            InitializeComponent();
            comboBox3.Hide();
            // combobox for each privilege info
            comboBox3.DisplayMember = "Text";
            comboBox3.ValueMember = "Value";
            

            // combobox for user/roles
            comboBox2.DisplayMember = "Text";
            comboBox2.ValueMember = "Value";

            //combobox for function
            comboBox1.DisplayMember = "Text";
            comboBox1.ValueMember = "Value";
            // add item to left combo box
            comboBox2.Items.Add(new { Text = "Roles", Value = "Roles" });
            comboBox2.Items.Add(new { Text = "Users", Value = "Users" });
            
            //call selected index changed to auto display list of users
            comboBox2_SelectedIndexChanged(comboBox2, null);

            // set default selected combo box
            //comboBox2.SelectedIndex = 0;
            // add item to right combo box
            //-> add your function name here
            comboBox1.Items.Add(new { Text = "Xem thông tin quyền", Value = "Priv_Info" });
            //

            // add item for privilege info combobox
            comboBox3.Items.Add(new { Text = "Xem quyền sys", Value = "sys_info" });//0
            comboBox3.Items.Add(new { Text = "Xem quyền trên table", Value = "tab_info" });//1
            comboBox3.Items.Add(new { Text = "Xem quyền trên cột", Value = "col_info" });//2
            // set default index to comboBox3
            comboBox3.SelectedIndex = 0;



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
            if (comboBox2.SelectedIndex == 0)
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

        //handle event for combo box selecting function
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            if (comboBox1.SelectedIndex == 0)
            {
                comboBox3.Show();
            }



            // please add comboBox3.Hide() in other if conditions xD
            else
            {
                comboBox3.Hide();
            }
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
                if (comboBox2.SelectedIndex== 1) // is selecting to view user
                {
                    // this is where we select each privs table according to end user selection on comboBox3
                    if (comboBox3.SelectedIndex== 0) // view sys privs info
                    {
                        string query = "select privilege from dba_sys_privs " +
                            "where grantee = upper('" + listBox1.SelectedItem.ToString() +"')";
                        getUserRolePrivsInfo(query, true);
                    }
                    if (comboBox3.SelectedIndex== 1) // view table privs
                    {
                        string query = "select privilege, table_name, grantor, grantable from dba_tab_privs " +
                            "where grantee = upper('" + listBox1.SelectedItem.ToString() + "')";
                        getUserRolePrivsInfo(query, true);
                    }
                    if (comboBox3.SelectedIndex == 2) // view col privs
                    {
                        string query = "select privilege, table_name,column_name,grantor,grantable from dba_col_privs " +
                            "where grantee = upper('" + listBox1.SelectedItem.ToString() + "')";
                        getUserRolePrivsInfo(query, true);
                    }
                }

                else if (comboBox2.SelectedIndex==0)// is selecting to view role
                {
                    // this is where we select each privs table according to end user selection on comboBox3
                    if (comboBox3.SelectedIndex == 0) // view sys privs info
                    {
                        string query = "select privilege from dba_sys_privs " +
                            "where grantee = upper('" + listBox1.SelectedItem.ToString() + "')";
                        getUserRolePrivsInfo(query, true);
                    }
                    if (comboBox3.SelectedIndex == 1) // view table privs
                    {
                        string query = "select privilege, table_name, grantable from role_tab_privs " +
                            "where role = upper('" + listBox1.SelectedItem.ToString() + "')";
                        getUserRolePrivsInfo(query, true);
                    }
                    if (comboBox3.SelectedIndex == 2) // view col privs
                    {
                        string query = "select privilege, table_name,column_name,grantor from dba_col_privs " +
                            "where grantee = upper('" + listBox1.SelectedItem.ToString() + "')";
                        getUserRolePrivsInfo(query, true);
                    }
                }
            }
        }

        //function to get privileges info from oracle
        private void getUserRolePrivsInfo(string query, bool isGetUser)
        {
            // clear the table first
            try
            {
                data.Clear();
                dataGridView2.Refresh();    
            }
            catch
            {
                //do nothing honestly
            }

            // perform oracle select
            try
            {
                OracleCommand command = new OracleCommand(query, con);
                OracleDataReader oraReader = command.ExecuteReader();
                // bind return select result to DataTable
                data = new DataTable();
                data.Load(oraReader);
                // bind data to table aka datagridview
                dataGridView2.DataSource = data;
            }
            catch
            {
                MessageBox.Show("Error getting result!", "Alert");
            }
        }

    }

    
}
