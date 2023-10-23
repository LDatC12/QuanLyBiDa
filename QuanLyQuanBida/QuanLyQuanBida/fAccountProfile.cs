using QuanLyQuanBida.DAO;
using QuanLyQuanBida.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyQuanBida
{
    public partial class fAccountProfile : Form
    {

        private Account loginAccount;

        public Account LoginAccount
        {
            get { return loginAccount; }
            set { loginAccount = value; ChangeAccount(loginAccount); }
        }

        public fAccountProfile(Account acc)
        {
            InitializeComponent();

            LoginAccount = acc;
        }
        void ChangeAccount(Account acc)
        {
            txtUserName.Text = LoginAccount.UserName;
            txtDisplayName.Text = LoginAccount.DisplayName;
        }

        void UpdateAccountInfo()
        {
            string displayName = txtDisplayName.Text; ;
            string password = txtPassWord.Text;
            string newpass = txtNewPass.Text;
            string reenterPass = txtReEnterPass.Text;
            string userName = txtUserName.Text;

            if (!newpass.Equals(reenterPass))
            {
                MessageBox.Show("Password do not match!");
            } 
            else
            {
                if (AccountDAO.Instance.UpdateAccount(userName, displayName, password, newpass))
                {
                    MessageBox.Show("Update successful!");
                } else
                {
                    MessageBox.Show("Please enter correct password");
                }
            }
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnUpdate_Click_1(object sender, EventArgs e)
        {
            UpdateAccountInfo();
        }
    }
}
