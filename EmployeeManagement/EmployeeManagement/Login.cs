using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EmployeeManagement
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }
        private String username = "admin";
        private String password = "admin";

        private void Login_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (tb_username.Text != "" || tb_password.Text != "")
            {
                if (tb_username.Text == username && tb_password.Text == password)
                {
                    this.Hide();
                    Menu f = new Menu();
                    f.Show();
                }
                else
                {
                    MessageBox.Show("Username or password incorrect!");
                }
            }
            else
            {
                MessageBox.Show("Please enter all username and password!");
            }
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
