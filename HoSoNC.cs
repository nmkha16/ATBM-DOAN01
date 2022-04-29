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
    public partial class HoSoNC : Form
    {
        private InterfaceNC _itfnc;
        private DataTable data;
        private OracleConnection _con;
        public HoSoNC(InterfaceNC itfnc, OracleConnection con)
        {
            _itfnc = itfnc;
            _con = con;
            InitializeComponent();
            comboBoxNC.DisplayMember = "Text";
            comboBoxNC.ValueMember = "Value";


            // add item to  combo box
            comboBoxNC.Items.Add(new { Text = "HSBA", Value = "HSBA" });
            comboBoxNC.Items.Add(new { Text = "HSBA_DV", Value = "DICHVU" });
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (comboBoxNC.SelectedIndex == 0) //HSBA
            {
                getTable("select * from admin11.TC5_HSBA");
            }
            else if (comboBoxNC.SelectedIndex == 1) // HSBA_DV
            {
                getTable("SELECT * FROM admin11.TC5_HSBA_DV");
            }
            
        }

        private void getTable(String query)
        {
            try
            {
                data.Clear();

                dataNC.Refresh();
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
                dataNC.DataSource = data;

            }
            catch
            {
                MessageBox.Show("Error getting result!", "Alert");
            }
        }

        private void HoSoNC_FormClosing(object sender, FormClosingEventArgs e)
        {
            _itfnc.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBoxNC.SelectedIndex == 0) //HSBA
            {
                getTable("select * from admin11.TC5_HSBA");
            }
            else if (comboBoxNC.SelectedIndex == 1) // DICHVU
            {
                getTable("SELECT * FROM admin11.TC5_HSBA_DV");
            }
        }
    }
}
