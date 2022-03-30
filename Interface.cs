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
            comboBox2.SelectedIndex = 0;
            // add item to right combo box
            //-> add your function name here
            comboBox1.Items.Add(new { Text = "Xem danh sách người dùng", Value = 0 });
            //



            //set default select combo box
            comboBox1.SelectedIndex = 0;

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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }


    }
}
