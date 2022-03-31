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
            //stretch last columnn

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
            comboBox2.Items.Add(new { Text = "Tables", Value = "Tables" });
            comboBox2.Items.Add(new { Text = "Views", Value = "Views" });
            //call selected index changed to auto display list of users
            // set default selected combo box
            //comboBox2.SelectedIndex = 0;
            // add item to right combo box
            //-> add your function name here
            comboBox1.Items.Add(new { Text = "Xem thông tin quyền", Value = "Priv_Info" });
            comboBox1.Items.Add(new { Text = "Xem thông tin bảng", Value = "Tab_Info" });
            comboBox1.Items.Add(new { Text = "Tạo mới User-Role", Value = "Create_User_Role" });


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
            if (comboBox2.SelectedIndex == 0) //roles
            {
                getDBObject("select role from dba_roles where common = 'NO'");
            }
            else if (comboBox2.SelectedIndex == 1) // users
            {
                getDBObject("SELECT username FROM dba_users " +
                    "where oracle_maintained = 'N' and username not like 'ADMIN%'");
            }
            else if (comboBox2.SelectedIndex == 2) // table
            {
                getDBObject("select TABLE_NAME from user_tables");
            }
            else if (comboBox2.SelectedIndex == 3) //views
            {
                getDBObject("select view_name from user_views");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        //select index change
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
             //throw new NotImplementedException();
            if (comboBox1.SelectedIndex == 0)
            {
                comboBox3.Show();
            }

            // please add comboBox3.Hide() in other if conditions xD

            // view table info, must select Tables value on left comboBox          


            //throw new NotImplementedException();
            // create new user
            else if(comboBox1.SelectedIndex == 2 && comboBox2.SelectedIndex==1)
            {
                comboBox3.Hide();
                openAddNewUserForm(true);
            }
            // create new role
            else if (comboBox1.SelectedIndex == 2 && comboBox2.SelectedIndex == 0)
            {
                comboBox3.Hide();
                openAddNewUserForm(false);
            }
            
            /*else
            {
                comboBox3.Hide();
            }*/
        }
        private void openAddNewUserForm(bool checkUser_Role)
        {
            this.Hide();
            AddNewUserRole itf = new AddNewUserRole(this, con, checkUser_Role);
            itf.Show();
        }

        // method to handle select user/roles combobox
        // populate list box
        private void getDBObject(string query)
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
                        getResultByQuery(query);
                    }
                    if (comboBox3.SelectedIndex== 1) // view table privs
                    {
                        string query = "select privilege, table_name, grantor, grantable from dba_tab_privs " +
                            "where grantee = upper('" + listBox1.SelectedItem.ToString() + "')";
                        getResultByQuery(query);
                    }
                    if (comboBox3.SelectedIndex == 2) // view col privs
                    {
                        string query = "select privilege, table_name,column_name,grantor,grantable from dba_col_privs " +
                            "where grantee = upper('" + listBox1.SelectedItem.ToString() + "')";
                        getResultByQuery(query);
                    }
                }

                else if (comboBox2.SelectedIndex==0)// is selecting to view role
                {
                    // this is where we select each privs table according to end user selection on comboBox3
                    if (comboBox3.SelectedIndex == 0) // view sys privs info
                    {
                        string query = "select privilege from dba_sys_privs " +
                            "where grantee = upper('" + listBox1.SelectedItem.ToString() + "')";
                        getResultByQuery(query);
                    }
                    if (comboBox3.SelectedIndex == 1) // view table privs
                    {
                        string query = "select privilege, table_name, grantable from role_tab_privs " +
                            "where role = upper('" + listBox1.SelectedItem.ToString() + "')";
                        getResultByQuery(query);
                    }
                    if (comboBox3.SelectedIndex == 2) // view col privs
                    {
                        string query = "select privilege, table_name,column_name,grantor from dba_col_privs " +
                            "where grantee = upper('" + listBox1.SelectedItem.ToString() + "')";
                        getResultByQuery(query);
                    }
                }
            }

            // double click on table name to view its infomation
            else if (comboBox1.SelectedIndex == 1 && comboBox2.SelectedIndex == 2)
            {
                getResultByQuery("select * from " + listBox1.SelectedItem.ToString());
            }
        }

        //function to get privileges info from oracle
        private void getResultByQuery(string query)
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
