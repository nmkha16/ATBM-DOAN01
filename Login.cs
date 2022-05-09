using Oracle.ManagedDataAccess.Client;
using static ATBM_DOAN01.Program;
namespace ATBM_DOAN01
{
    public partial class Login : Form
    {
        private OracleConnection? con = null;
        private string _userID;
        public Login()
        {
            InitializeComponent();

            //fast debug hack :))
            //textBox1.Text = "admin11";
            //textBox2.Text = "nhom11";

            FormClosing += Login_FormClosing;
        }

        private void Login_FormClosing(object? sender, FormClosingEventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        // function for login
        private void login()
        {
            //now we check special char in login
            if (checkInput.hasSpecialCharacter(textBox1.Text))
            {
                MessageBox.Show("Only accept letters and numeric alphabet in user!!!");
                return;
            }

            // create connection
            con = new OracleConnection();

            // create connection string using builder
            OracleConnectionStringBuilder ocsb = new OracleConnectionStringBuilder();

            

            ocsb.UserID = textBox1.Text;
            _userID = ocsb.UserID;
            ocsb.Password = textBox2.Text; 

            ocsb.DataSource = "localhost:1521/QuanLiS";

            // connect
            con.ConnectionString = ocsb.ConnectionString;
            try
            {
                con.Open();
                MessageBox.Show("Connection established (" + con.ServerVersion + ")", "Connection");
                // open admin interface
                if (textBox1.Text.ToLower() == "admin11")
                {
                    openInterface(0);
                    return;
                }

                // user specifically for patient
                if (textBox1.Text.ToUpper().Substring(0,2) == "BN")
                {
                    openInterface(1);
                    return;
                }

                string employeeRole = getEmployeeRole();
                if (employeeRole == "CSYT")
                {
                    //
                    openInterface(2);
                }
                else if (employeeRole == "TT")
                {
                    //NV376
                    openInterface(3);
                }
                else if (employeeRole == "NC")
                {
                    
                    openInterface(4);
                }
                else
                {
                    // nghiên cứu dùng chung interface với y bác sĩ
                    openInterface(4);
                }
            }
            catch (Exception)
            {
                // show original oracle's error
                //MessageBox.Show(ex.Message, "Oracle Connection");
                this.Show();
                MessageBox.Show("Invalid username or password", "Connection");

            }
        }

        // handle login button
        private void button_Click(object sender, EventArgs e)
        {
            login();
        }

        //open interface form
        private void openInterface(int k){
            this.Hide();
            switch (k)
            {
                case 0:     // open admin's interface
                    {
                        Interface itf = new Interface(this, con);
                        itf.Show();
                        break;
                    }
                case 1:     // open patience's interface
                    {
                        InterfaceBN itfPT = new InterfaceBN(this, con);
                        itfPT.Show();
                        break;
                    }
                case 2:     // open CSYT's interface
                    {
                        CSYT_HSBA cSYT_HSBA = new CSYT_HSBA(this, con);
                        cSYT_HSBA.Show();
                        break;
                    }
                case 3:     // open Thanh tra's interface
                    {
                        InterfaceTT0 itfTT0 = new InterfaceTT0(this, con);
                        itfTT0.Show();
                        break;
                    }
                case 4:     // open Nghien cuu's interface
                    {
                        InterfaceNC itfNC = new InterfaceNC(this, con);
                        itfNC.Show();
                        break;
                    }
            }           
        }
        /// <summary>
        /// query để xem vai trò của nhân viên là gì để mở interface phù hợp
        /// </summary>
        private string getEmployeeRole()
        {
            string query = "select vaitro from admin11.TC6_NHANVIEN";

            OracleCommand comm = new OracleCommand(query, con);
            OracleDataReader reader = comm.ExecuteReader();
            if (reader.HasRows)
            {
                if (reader.Read())
                {
                    string role = reader.GetString(0);

                    if (role == "CSYT") { return "CSYT"; }
                    else if (role == "Thanh tra") { return "TT"; }
                    else if (role == "Y bác sĩ") { return "YBS"; }
                    else if (role == "Nghiên cứu") { return "NC"; }
                }
            }
            return "";
        }
    }
}