using Oracle.ManagedDataAccess.Client;
namespace ATBM_DOAN01
{
    public partial class Login : Form
    {
        private OracleConnection? con = null;
        public Login()
        {
            InitializeComponent();
        }

        // function for login
        private void login()
        {
            // create connection
            con = new OracleConnection();

            // create connection string using builder
            OracleConnectionStringBuilder ocsb = new OracleConnectionStringBuilder();

            ocsb.UserID = textBox1.Text;
            ocsb.Password = textBox2.Text;
            ocsb.DataSource = "localhost:1521/QuanLiS";

            // connect
            con.ConnectionString = ocsb.ConnectionString;
            try
            {
                con.Open();
                MessageBox.Show("Connection established (" + con.ServerVersion + ")", "Oracle Connection");
                openInterface();
            }
            catch (Exception ex)
            {
                // show original oracle's error
                //MessageBox.Show(ex.Message, "Oracle Connection");
                MessageBox.Show("Invalid username or password", "Oracle Connection");
            }
        }

        // handle login button
        private void button_Click(object sender, EventArgs e)
        {
            login();
        }

        //open interface form
        private void openInterface(){
            this.Hide();
            Interface itf = new Interface(this, con);
            itf.Show();
        }
    }
}