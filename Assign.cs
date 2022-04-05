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
    public partial class Assign : Form
    {
        private Interface ift;
        private OracleConnection con;
        private bool isGrantingUser;
        public Assign(Interface ift,OracleConnection con,string name,bool isGrantingUser)
        {
            InitializeComponent();
            this.ift = ift;
            this.con = con;
            label1.Text = name;
            this.isGrantingUser = isGrantingUser;

            FormClosing += Assign_FormClosing;

            //set title
            if (!isGrantingUser)
            {
                Text = "Granting Privilege To Role";
                checkedListBox2.Enabled = false;
            }
            else
            {
                Text = "Granting Privilege To User";
                //call function to populate listbox with list of roles in db
                // query to view all tables
                //
            }
            populateTableQuery(listBox1, "select TABLE_NAME from user_tables");
            populateTableQuery(checkedListBox2, "select role from dba_roles where common ='NO'");
            
            // adding select, update, insert, delete option
            comboBox1.DisplayMember = "Text";
            comboBox1.ValueMember = "Value";
            // add item to left combo box
            comboBox1.Items.Add(new { Text = "Select", Value = "Select" });
            comboBox1.Items.Add(new { Text = "Insert", Value = "Insert" });
            comboBox1.Items.Add(new { Text = "Update", Value = "Update" });
            comboBox1.Items.Add(new { Text = "Delete", Value = "Delete" });

        }

        private void Assign_FormClosing(object? sender, FormClosingEventArgs e)
        {
            
            ift.Show();
        }


        // combobox for selecting a operation  to grant
        //select, insert, update, delete
        // only insert, update can be only granted to column level
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0 || comboBox1.SelectedIndex == 3)
            {
                checkedListBox1.Enabled = false;
            }
            else checkedListBox1.Enabled = true;
        }




        // get list of column when selecting a table or granting operation on table
        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            // if operation selection is select and delete
            if (comboBox1.SelectedIndex == 0 || comboBox1.SelectedIndex == 3)
            {
                // build query to execute
                string query = "grant " + comboBox1.Text + " on " + listBox1.SelectedItem.ToString() + " to " + label1.Text;
                grant(query);
            }
            else if (comboBox1.SelectedIndex == 1 || comboBox1.SelectedIndex == 2)
            {
                populateTableQuery(checkedListBox1, "select column_name from dba_tab_columns where table_name = '" +
                                      listBox1.SelectedItem.ToString() + "'");
            }
        }


        // grant privilege to the user or role every time a checkbox column is ticked
        private void checkedListBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            // build a query then call grant function everytime a box is ticked
            if (checkedListBox1.GetItemCheckState(checkedListBox1.SelectedIndex) == CheckState.Checked)
            {
                // grant <operation> (<column_name>) on <table_name> to <user\role_name>
                string query = "grant " + comboBox1.Text + " (" +
                    checkedListBox1.SelectedItem.ToString() + ") on " + listBox1.SelectedItem.ToString() +
                    " to " + label1.Text;
                // if with grant option is checked
                if (checkBox1.Checked)
                {
                    query += " with grant option";
                }
                grant(query);
            }
        }


        // grant privilege to a user\role or grant role
        private void grant(string query)
        {
            OracleCommand oracleCommand = new OracleCommand(query, con);
            try
            {
                oracleCommand.ExecuteNonQuery();
                MessageBox.Show("Grant successfully!!!", "Alert");
            }
            catch
            {
                MessageBox.Show("Grant failed!!!", "Alert");
            }
        }


        // populate table
        private void populateTableQuery(ListBox target, string query)
        {
            // perform oracle select
            target.Items.Clear();
            try
            {
                OracleCommand command = new OracleCommand(query, con);
                OracleDataReader oraReader = command.ExecuteReader();
                if (oraReader.HasRows)
                {
                    while (oraReader.Read())
                    {
                        target.Items.Add(oraReader.GetString(0));
                    }
                }
                else
                {
                    target.Items.Clear();
                }
                oraReader.Close();
            }
            catch
            {
                MessageBox.Show("Error getting result!", "Alert");
            }
        }


        // grant role to a user if a role is ticked
        private void checkedListBox2_SelectedValueChanged(object sender, EventArgs e)
        {
            // build a query then call grant function everytime a box is ticked
            if (checkedListBox2.GetItemCheckState(checkedListBox2.SelectedIndex) == CheckState.Checked)
            {
                // grant <role_name> to <user_name>
                string query = "grant " + checkedListBox2.SelectedItem.ToString() +
                    " to " + label1.Text;
                // if with grant option is checked
                if (checkBox1.Checked)
                {
                    query += " with grant option";
                }
                grant(query);
            }
        }
    }
}
