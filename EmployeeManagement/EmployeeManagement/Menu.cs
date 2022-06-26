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

        private const int CP_DISABLE_CLOSE_BUTTON = 0x200;
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ClassStyle = cp.ClassStyle | CP_DISABLE_CLOSE_BUTTON;
                return cp;
            }
        }

        private void setFullScreenForm()
        {
            this.TopMost = true;
            this.WindowState = FormWindowState.Maximized;
        }



        private void Menu_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();
            //setFullScreenForm();
        }


        private void btnLogout_Click(object sender, EventArgs e)
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

        private void empManageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Employee f = new Employee();
            f.ShowDialog();
        }

        private void benefitManageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Benefit f = new Benefit();
            f.ShowDialog();
        }

        private void deptManageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Department f = new Department();
            f.ShowDialog();
        }

        private void positionManageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Position f = new Position();
            f.ShowDialog();
        }

        private void workTrackingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Working f = new Working();
            f.ShowDialog();
        }

        private void benefitTrackingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Holiday f = new Holiday();
            f.ShowDialog();
        }

        private void empReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EmpReport f = new EmpReport();
            f.ShowDialog();
        }

        private void salaryPaymentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Salary f = new Salary();
            f.ShowDialog();
        }
    }
}
