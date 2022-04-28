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
    public partial class InfoPT : Form
    {
        private string _userId;
        private OracleConnection _con;
        private InterfacePT _iftPT;
        public InfoPT(InterfacePT iftPT, string userID, OracleConnection con)
        {
            _iftPT = iftPT;
            _userId = userID;
            _con = con;
            InitializeComponent();

            FormClosing += InfoPT_FormClosing;

            InfoPT_Load();
        }

        private void InfoPT_FormClosing(object? sender, FormClosingEventArgs e)
        {
            _iftPT.Show();
        }

        /// <summary>
        /// Lấy thông tin bệnh nhân và hiện lên giao hiện
        /// </summary>
        private void InfoPT_Load()
        {
            /*
             * select bn.tenbn, bn.cmnd, to_char(bn.ngaysinh,'DD/MM/YYYY') BOD,
             * (bn.sonha || ' ' || bn.tenduong || ', ' || bn.quanhuyen || ', ' || bn.tinhtp) address, bn.tiensubenh, bn.tiensubenhgd, bn.diungthuoc,
             * cs.tencsyt, cs.dccsyt
             * from benhnhan bn, admin11.csyt cs where bn.mabn = <....> and cs.macsyt = bn.macsyt
             */
            string query = "select bn.tenbn, bn.cmnd, to_char(bn.ngaysinh,'DD/MM/YYYY') BOD," +
                    "(bn.sonha || ' ' || bn.tenduong || ', ' || bn.quanhuyen || ', ' || bn.tinhtp) address, " +
                    "bn.tiensubenh, bn.tiensubenhgd, bn.diungthuoc, " + "cs.tencsyt, cs.dccsyt" +
                    " from admin11.benhnhan bn, admin11.csyt cs " +
                    "where bn.mabn = '" + _userId.ToUpper()+"' and cs.macsyt = bn.macsyt";

            OracleCommand comm = new OracleCommand(query, _con);
            OracleDataReader reader = comm.ExecuteReader();
            if (reader.HasRows)
            {
                if (reader.Read())
                {
                    textBox1.Text = reader.GetString(0); // họ tên
                    textBox2.Text = reader.GetString(1); // cmnd
                    textBox3.Text = reader.GetString(2);    // ngày sinh
                    textBox5.Text = reader.GetString(3);    //  địa chỉ
                    textBox4.Text = reader.GetString(4);    // tiền sử bệnh
                    textBox8.Text = reader.GetString(5);    // tiền sử bệnh gia đình
                    textBox9.Text = reader.GetString(6);    // dị ứng thuốc

                    textBox6.Text = reader.GetString(7);    // tên bệnh viện đang điều trị
                    textBox7.Text = reader.GetString(8);    // địa chỉ của bệnh viện đang điều trị
                }
            }
        }
    }
}
