using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;

namespace ATBM_DOAN01
{
    public partial class InterfaceTT : Form
    {
        private InterfaceTT0 _itfTT;
        private DataTable data;
        private OracleConnection _con;
        public InterfaceTT(InterfaceTT0 itfTT, OracleConnection con)
        {
            _itfTT = itfTT;
            _con = con;
            InitializeComponent();

            comboBoxTT.DisplayMember = "Text";
            comboBoxTT.ValueMember = "Value";

            
            // add item to  combo box
            comboBoxTT.Items.Add(new { Text = "HSBA", Value = "HSBA" });
            comboBoxTT.Items.Add(new { Text = "DICHVU", Value = "DICHVU" });
            comboBoxTT.Items.Add(new { Text = "HSBA_DV", Value = "HSBA_DV" });
            comboBoxTT.Items.Add(new { Text = "BENHNHAN", Value = "BENHNHAN" });
            comboBoxTT.Items.Add(new { Text = "NHANVIEN", Value = "NHANVIEN" });
            comboBoxTT.Items.Add(new { Text = "CSYT", Value = "CSYT" });
            comboBoxTT.Items.Add(new { Text = "KHOA", Value = "KHOA" });
            
        }

        private void comboBoxTT_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxTT.SelectedIndex == 0) //HSBA
            {
                getTable("select * from admin11.TC2_HSBA");
            }
            else if (comboBoxTT.SelectedIndex == 1) // DICHVU
            {
                getTable("SELECT * FROM admin11.TC2_DICHVU");
            }
            else if (comboBoxTT.SelectedIndex == 2) // HSBA_DV
            {
                getTable("select * from admin11.TC2_HSBA_DV");
            }
            else if (comboBoxTT.SelectedIndex == 3) //BENHNHAN
            {
                getTable("select * from admin11.TC2_BENHNHAN");
            }
            else if (comboBoxTT.SelectedIndex == 4) // NHANVIEN
            {
                getTable("select * from admin11.TC2_NHANVIEN");
            }
            else if (comboBoxTT.SelectedIndex == 5) // CSYT
            {
                getTable("select * from admin11.TC2_CSYT");
            }
            else if (comboBoxTT.SelectedIndex == 6) //KHOA
            {
                getTable("select * from admin11.TC2_KHOA");
            }
        }

        private void getTable(String query)
        {
            try
            {
                data.Clear();

                dataTT.Refresh();
            }
            catch
            {
               
            }
            try
            {
                OracleCommand command = new OracleCommand(query, _con);
                OracleDataReader oraReader = command.ExecuteReader();
                // bind return select result to DataTable
                data = new DataTable();

                data.Load(oraReader);
                // bind data to table aka datagridview        
                dataTT.DataSource = data;

            }
            catch
            {
                MessageBox.Show("Error getting result!", "Alert");
            }
        }

        private void InterfaceTT_FormClosing(object sender, FormClosingEventArgs e)
        {
            _itfTT.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBoxTT.SelectedIndex == 0) //HSBA
            {
                getTable("select * from admin11.TC2_HSBA");
            }
            else if (comboBoxTT.SelectedIndex == 1) // DICHVU
            {
                getTable("SELECT * FROM admin11.TC2_DICHVU");
            }
            else if (comboBoxTT.SelectedIndex == 2) // HSBA_DV
            {
                getTable("select * from admin11.TC2_HSBA_DV");
            }
            else if (comboBoxTT.SelectedIndex == 3) //BENHNHAN
            {
                getTable("select * from admin11.TC2_BENHNHAN");
            }
            else if (comboBoxTT.SelectedIndex == 4) // NHANVIEN
            {
                getTable("select * from admin11.TC2_NHANVIEN");
            }
            else if (comboBoxTT.SelectedIndex == 5) // CSYT
            {
                getTable("select * from admin11.TC2_CSYT");
            }
            else if (comboBoxTT.SelectedIndex == 6) //KHOA
            {
                getTable("select * from admin11.TC2_KHOA");
            }
        }
        
    }
}
