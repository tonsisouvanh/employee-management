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
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();
        }

        private void empToolStripMenuItem_Click(object sender, EventArgs e)
        {

            Employee f = new Employee();
            this.Hide();
            f.Show();
            
        }

        private void deptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Department f = new Department();
            this.Hide();
            f.Show();
           
        }


        private void benefitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Benefit f = new Benefit();
            this.Hide();
            f.Show();
            
        }
        private void PositionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Position f = new Position();
            this.Hide();
            f.Show();
            
        }


        private void Menu_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();
        }

        private void LogoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var confirmResult = MessageBox.Show("Are you sure to logout?",
                                    "Confirm Logout!!",
                                    MessageBoxButtons.YesNo);
            if (confirmResult == DialogResult.Yes)
            {
                this.Close();
                Login f = new Login();
                f.Show();
            }
           
        }

        private void breakToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Holiday f = new Holiday();
            this.Hide();
            f.Show();
           
        }

        private void workingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Working f = new Working();
            this.Hide();
            f.Show();
           
        }

        private void Menu_FormClosed(object sender, FormClosedEventArgs e)
        {
            //this.Close();
            //Login f = new Login();
            //f.Show();
        }
    }
}
