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
        private object[,] dataValue; // for clone the datagridview2 
        public Interface(Login login, OracleConnection con)
        {
            InitializeComponent();
            // sort list box
            listBox1.Sorted = true;


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
            comboBox1.Items.Add(new { Text = "Xem thông tin quyền", Value = "Priv_Info" });//0
            comboBox1.Items.Add(new { Text = "Xem thông tin bảng", Value = "Tab_Info" });//1
            comboBox1.Items.Add(new { Text = "Tạo mới User-Role", Value = "Create_User_Role" });//2
            comboBox1.Items.Add(new { Text = "Xoá object", Value = "Drop_Object" });//3
            comboBox1.Items.Add(new { Text = "Hiệu chỉnh role", Value = "Adjust_Role" });//4
            comboBox1.Items.Add(new { Text = "Cấp phát quyền", Value = "Assign_privilege" });//5
            comboBox1.Items.Add(new { Text = "Cập nhật database", Value = "Update_DB" });//6
            comboBox1.Items.Add(new { Text = "Xem FGA audit", Value = "FGA_Audit" });//7

            // add item for privilege info combobox
            comboBox3.Items.Add(new { Text = "Xem quyền sys", Value = "sys_info" });//0
            comboBox3.Items.Add(new { Text = "Xem quyền trên table", Value = "tab_info" });//1
            comboBox3.Items.Add(new { Text = "Xem quyền trên cột", Value = "col_info" });//2
            comboBox3.Items.Add(new { Text = "Xem các role của user", Value = "user_granted_roles" });
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

        //update button handle
        private void button1_Click(object sender, EventArgs e)
        {
            var confirmResult = MessageBox.Show("Are you sure to update?",
                                                "Confirm Update!",
                                                MessageBoxButtons.YesNo);
            if (confirmResult == DialogResult.Yes)
            {
                //role or user
                if (comboBox2.SelectedIndex == 1 || comboBox2.SelectedIndex == 0)
                {
                    //view privs of table
                    if (comboBox3.SelectedIndex == 1 && comboBox1.SelectedIndex == 0)
                    {
                        for (int x = 0; x < dataValue.GetLength(0); x++)
                        {
                            if (dataValue[x, 0] != dataGridView2.Rows[x].Cells[0].Value || dataValue[x, 1] != dataGridView2.Rows[x].Cells[1].Value)
                            {
                                //first do grant


                                string query = "grant " + dataGridView2.Rows[x].Cells[0].Value + " on "
                                            + dataGridView2.Rows[x].Cells[1].Value + " to " + listBox1.SelectedItem.ToString();

                                OracleCommand command = new OracleCommand(query, con);
                                OracleDataReader oraReader;
                                try
                                {
                                    oraReader = command.ExecuteReader();
                                }
                                catch
                                {
                                    MessageBox.Show("Not valid!!!");
                                    return;

                                }
                                //then do revoke
                                query = "revoke " + dataValue[x, 0] + " on "
                                                + dataValue[x, 1] + " from " + listBox1.SelectedItem.ToString();
                                command = new OracleCommand(query, con);
                                oraReader = command.ExecuteReader();



                            }

                        }
                    }
                    //role of user
                    else if (comboBox3.SelectedIndex == 3 && comboBox1.SelectedIndex == 0)
                    {
                        for (int x = 0; x < dataValue.GetLength(0); x++)
                        {
                            if (dataValue[x, 0] != dataGridView2.Rows[x].Cells[0].Value)
                            {
                                //first do grant


                                string query = "grant " + dataGridView2.Rows[x].Cells[0].Value
                                                + " to " + listBox1.SelectedItem.ToString();

                                OracleCommand command = new OracleCommand(query, con);
                                OracleDataReader oraReader;
                                try
                                {
                                    oraReader = command.ExecuteReader();
                                }
                                catch
                                {
                                    MessageBox.Show("Not valid!!!");
                                    return;

                                }
                                //then do revoke
                                query = "revoke " + dataValue[x, 0] + " from " + listBox1.SelectedItem.ToString();
                                command = new OracleCommand(query, con);
                                oraReader = command.ExecuteReader();



                            }

                        }
                    }
                }

                refresh();
            }



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
            else if (comboBox1.SelectedIndex == 2 && comboBox2.SelectedIndex == 1)
            {

                openAddNewUserForm(true);
            }
            // create new role
            else if (comboBox1.SelectedIndex == 2 && comboBox2.SelectedIndex == 0)
            {
                openAddNewUserForm(false);
            }

            else if (comboBox1.SelectedIndex == 6)
            {
                Hide();
                CSYT_NV_DBA cSYT_NV_DBA = new CSYT_NV_DBA(this, con);
                cSYT_NV_DBA.Show();
            }

            // view fga audit

            else if (comboBox1.SelectedIndex == 7)
            {
                //string query = "select * from dba_fga_audit_trail";
                string query = "select session_id, timestamp, db_user, object_name, sql_text, statement_type from dba_fga_audit_trail";
                getResultByQuery(query);
            }

            else

            {
                comboBox3.Hide();
            }

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
            if (comboBox2.SelectedIndex != 2 && comboBox2.SelectedIndex != 3)
            { 
            // function is view privilege info
            if (comboBox1.SelectedIndex == 0)
            {
                if (comboBox2.SelectedIndex == 1) // is selecting to view user
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
                    if (comboBox3.SelectedIndex == 3) // view which roles granted to user
                    {
                        string query = "select granted_role from dba_role_privs where grantee = upper('" +
                            listBox1.SelectedItem.ToString() + "')";
                        getResultByQuery(query);
                    }
                }

                else if (comboBox2.SelectedIndex == 0)// is selecting to view role
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

            // selecting drop object function in comboBox
            else if (comboBox1.SelectedIndex == 3)
            {
                // droppping role
                if (comboBox2.SelectedIndex == 0)
                {
                    dropSchema("drop role " + listBox1.SelectedItem.ToString());
                }
                //dropping user
                else if (comboBox2.SelectedIndex == 1)
                {
                    dropSchema("drop user " + listBox1.SelectedItem.ToString());
                }
            }


            // adjust role
            else if (comboBox2.SelectedIndex == 0 && comboBox1.SelectedIndex == 4)
            {
                comboBox3.Hide();
                this.Hide();
                // for adjust role
                AddNewUserRole adjustRole = new AddNewUserRole(this, con, listBox1.SelectedItem.ToString());
                adjustRole.Show();
            }

            // granting privilege function
            else if (comboBox1.SelectedIndex == 5)
            {

                Hide();
                // granting on role
                if (comboBox2.SelectedIndex == 0)
                {
                    Assign assign = new Assign(this, con, listBox1.SelectedItem.ToString(), false);
                    assign.Show();

                }
                // granting on user
                else if (comboBox2.SelectedIndex == 1)
                {
                    Assign assign = new Assign(this, con, listBox1.SelectedItem.ToString(), true);
                    assign.Show();

                }
            }
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

                if (comboBox1.SelectedIndex == 0 && (comboBox2.SelectedIndex == 1 || comboBox2.SelectedIndex == 0))
                {
                    dataGridView2.Columns.RemoveAt(dataGridView2.Columns.Count - 1);
                }

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

                // if check privil of role or user, add a checkbox for granted privil
                if (comboBox1.SelectedIndex == 0 && (comboBox2.SelectedIndex == 1 || comboBox2.SelectedIndex == 0))
                {
                    //clone datagridview2
                    dataValue = new object[dataGridView2.Rows.Count, dataGridView2.Columns.Count];
                    for (int x = 0; x < dataValue.GetLength(0); x++)
                        for (int i = 0; i < dataValue.GetLength(1); i++)
                            dataValue[x, i] = dataGridView2.Rows[x].Cells[i].Value;
                    //add checkbox
                    DataGridViewCheckBoxColumn dgvCmb = new DataGridViewCheckBoxColumn();
                    dgvCmb.ValueType = typeof(bool);
                    dgvCmb.Name = "Chk";
                    dgvCmb.HeaderText = "GRANTED";
                    dataGridView2.Columns.Add(dgvCmb);
                    //data.Columns.Add("GRANTED", typeof(bool));
                    int count = dataGridView2.Rows.Count;
                    int count2 = dataGridView2.Columns.Count;
                    for (int i = 0; i < count; i++)
                    {
                        dataGridView2.Rows[i].Cells[count2 - 1].Value = true;
                    }
                    //enable edit column
                    //dataGridView2.Columns[0].ReadOnly = false;
                    dataGridView2.ReadOnly = false;
                    for (int i = 2; i < count2 - 1; i++)
                    {


                        dataGridView2.Columns[i].ReadOnly = true;

                    }

                }


            }
            catch
            {
                MessageBox.Show("Error getting result!", "Alert");
            }
        }



        private void dataGridView2_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            // Exit this event procedure if no rows have been added to the DataGridView yet (during program initialization)
            if (e.RowIndex < 0)
            {
                return;
            }

            // Proceed only if the value in the current cell has been changed since it went into edit mode
            if (dataValue[e.RowIndex, e.ColumnIndex] != dataGridView2.Rows[e.RowIndex].Cells[e.ColumnIndex].Value)
            {
                // Do your cell data manipulation here
                if (comboBox3.SelectedIndex == 1)
                {
                    if (checkInput.hasSpecialCharacter(dataGridView2.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString()))
                    {
                        MessageBox.Show("Only accept letters");
                        dataGridView2.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = dataValue[e.RowIndex, e.ColumnIndex];
                        return;
                    }

                }
            }
        }
        // refresh gridview for check privs
        private void refresh()
        {
            if (comboBox1.SelectedIndex == 0)
            {
                if (comboBox2.SelectedIndex == 1) // is selecting to view user
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
                    if (comboBox3.SelectedIndex == 3) // view which roles granted to user
                    {
                        string query = "select granted_role from dba_role_privs where grantee = upper('" +
                            listBox1.SelectedItem.ToString() + "')";
                        getResultByQuery(query);
                    }
                }

                else if (comboBox2.SelectedIndex == 0)// is selecting to view role
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

            // selecting drop object function in comboBox
            else if (comboBox1.SelectedIndex == 3)
            {
                // droppping role
                if (comboBox2.SelectedIndex == 0)
                {
                    dropSchema("drop role " + listBox1.SelectedItem.ToString());
                }
                //dropping user
                else if (comboBox2.SelectedIndex == 1)
                {
                    dropSchema("drop user " + listBox1.SelectedItem.ToString());
                }
            }


            // adjust role
            else if (comboBox2.SelectedIndex == 0 && comboBox1.SelectedIndex == 4)
            {
                comboBox3.Hide();
                this.Hide();
                // for adjust role
                AddNewUserRole adjustRole = new AddNewUserRole(this, con, listBox1.SelectedItem.ToString());
                adjustRole.Show();
            }

            // granting privilege function
            else if (comboBox1.SelectedIndex == 5)
            {

                Hide();
                // granting on role
                if (comboBox2.SelectedIndex == 0)
                {
                    Assign assign = new Assign(this, con, listBox1.SelectedItem.ToString(), false);
                    assign.Show();

                }
                // granting on user
                else if (comboBox2.SelectedIndex == 1)
                {
                    Assign assign = new Assign(this, con, listBox1.SelectedItem.ToString(), true);
                    assign.Show();

                }
            }
        }
        // handle granted checkbox for revoke
        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //We make DataGridCheckBoxColumn commit changes with single click
            //use index of logout column
            if (e.ColumnIndex == dataGridView2.Columns.Count - 1 && e.RowIndex >= 0)
                this.dataGridView2.CommitEdit(DataGridViewDataErrorContexts.Commit);

            //Check the value of cell
            if (e.ColumnIndex == dataGridView2.Columns.Count - 1)
            {
                if (comboBox2.SelectedIndex == 1 || comboBox2.SelectedIndex == 0)
                {
                    //Privs on table of user/role
                    if (comboBox1.SelectedIndex == 0 && comboBox3.SelectedIndex == 1)
                    {
                        if ((bool)this.dataGridView2.CurrentCell.Value == false)
                        {
                            //confirmation box
                            var confirmResult = MessageBox.Show("Are you sure to revoke this privilege?",
                                                 "Confirm Revoke!",
                                                 MessageBoxButtons.YesNo);
                            if (confirmResult == DialogResult.Yes)
                            {
                                // if 'Yes', do oracle revoke.
                                string query = "revoke " + dataGridView2.Rows[e.RowIndex].Cells[0].Value + " on "
                                            + dataGridView2.Rows[e.RowIndex].Cells[1].Value + " from " + listBox1.SelectedItem.ToString();

                                OracleCommand command = new OracleCommand(query, con);
                                OracleDataReader oraReader = command.ExecuteReader();
                            }
                            else
                            {
                                // if 'No', set checked box true .
                                this.dataGridView2.CurrentCell.Value = true;

                            }
                            //refresh gridview
                            //string queryRefresh = "select privilege, table_name, grantor, grantable from dba_tab_privs " +
                            //            "where grantee = upper('" + listBox1.SelectedItem.ToString() + "')";
                            //getResultByQuery(queryRefresh);
                            refresh();

                        }
                    }
                    //Privs on column table of user/role
                    else if (comboBox1.SelectedIndex == 0 && comboBox3.SelectedIndex == 2)
                    {
                        if ((bool)this.dataGridView2.CurrentCell.Value == false)
                        {
                            //confirmation box
                            var confirmResult = MessageBox.Show("Are you sure to revoke this column privilege? This will have to revoke for complete table!",
                                                 "Confirm Revoke!",
                                                 MessageBoxButtons.YesNo);
                            if (confirmResult == DialogResult.Yes)
                            {
                                // if 'Yes', do oracle revoke.
                                string query = "revoke " + dataGridView2.Rows[e.RowIndex].Cells[0].Value + " on "
                                            + dataGridView2.Rows[e.RowIndex].Cells[1].Value + " from " + listBox1.SelectedItem.ToString();

                                OracleCommand command = new OracleCommand(query, con);
                                OracleDataReader oraReader = command.ExecuteReader();
                            }
                            else
                            {
                                // if 'No', set checked box true .
                                this.dataGridView2.CurrentCell.Value = true;

                            }
                            //refresh gridview
                            //string queryRefresh = "select privilege, table_name,column_name,grantor,grantable from dba_col_privs " +
                            //        "where grantee = upper('" + listBox1.SelectedItem.ToString() + "')";
                            //getResultByQuery(queryRefresh);
                            refresh();

                        }
                    }
                    //Role of user
                    else if (comboBox1.SelectedIndex == 0 && comboBox3.SelectedIndex == 3 && comboBox2.SelectedIndex == 1)
                    {
                        if ((bool)this.dataGridView2.CurrentCell.Value == false)
                        {
                            //confirmation box
                            var confirmResult = MessageBox.Show("Are you sure to revoke this role from user?",
                                                 "Confirm Revoke!",
                                                 MessageBoxButtons.YesNo);
                            if (confirmResult == DialogResult.Yes)
                            {
                                // if 'Yes', do oracle revoke.
                                string query = "revoke " + dataGridView2.Rows[e.RowIndex].Cells[0].Value
                                                + " from " + listBox1.SelectedItem.ToString();

                                OracleCommand command = new OracleCommand(query, con);
                                OracleDataReader oraReader = command.ExecuteReader();
                            }
                            else
                            {
                                // if 'No', set checked box true .
                                this.dataGridView2.CurrentCell.Value = true;

                            }
                            //refresh gridview
                            //string queryRefresh = "select granted_role from dba_role_privs where grantee = upper('" +
                            //        listBox1.SelectedItem.ToString() + "')";
                            //getResultByQuery(queryRefresh);
                            refresh();

                        }
                    }
                    //Sys privs of user/role
                    else if (comboBox1.SelectedIndex == 0 && comboBox3.SelectedIndex == 0)
                    {
                        if ((bool)this.dataGridView2.CurrentCell.Value == false)
                        {
                            //confirmation box
                            var confirmResult = MessageBox.Show("Are you sure to revoke this privilege?",
                                                 "Confirm Revoke!",
                                                 MessageBoxButtons.YesNo);
                            if (confirmResult == DialogResult.Yes)
                            {
                                // if 'Yes', do oracle revoke.
                                string query = "revoke " + dataGridView2.Rows[e.RowIndex].Cells[0].Value
                                                + " from " + listBox1.SelectedItem.ToString();

                                OracleCommand command = new OracleCommand(query, con);
                                OracleDataReader oraReader = command.ExecuteReader();
                            }
                            else
                            {
                                // if 'No', set checked box true .
                                this.dataGridView2.CurrentCell.Value = true;

                            }
                            //refresh gridview
                            //string queryRefresh = "select privilege from dba_sys_privs " +
                            //        "where grantee = upper('" + listBox1.SelectedItem.ToString() + "')";
                            //getResultByQuery(queryRefresh);
                            refresh();
                        }
                    }
                }
            }



        }
        // function to drop user or role
        private void dropSchema(string query)
        {

            OracleCommand oracleCommand = new OracleCommand(query, con);
            try
            {
                oracleCommand.ExecuteNonQuery();
                MessageBox.Show("Drop successfully!!!", "Alert");
                listBox1.Items.RemoveAt(listBox1.SelectedIndex);
            }
            catch
            {
                MessageBox.Show("Drop failed!!!", "Alert");
            }
        }




        
    }
}

