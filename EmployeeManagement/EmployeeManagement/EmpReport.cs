using EmployeeManagement.DAO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EmployeeManagement
{
    public partial class EmpReport : Form
    {
        public EmpReport()
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

        void loadEmployee()
        {
            try
            {
                DataTable dtEmp = DataProvider.Instance.ExecuteQuery("Select * from view_EmployeeReport");
                dtgv_emp.DataSource = dtEmp;

                dtgv_emp.Columns["birthDay"].DefaultCellStyle.Format = "dd/MM/yyyy";

                dtgv_emp.Columns["empId"].HeaderText = "ລະຫັດພະນັກງານ";
                dtgv_emp.Columns["fullName"].HeaderText = "ພະນັກງານ";
                dtgv_emp.Columns["birthDay"].HeaderText = "ວັນເກີດ";
                dtgv_emp.Columns["address"].HeaderText = "ທີ່ຢູ່";
                dtgv_emp.Columns["identityCard"].HeaderText = "ເລກບັດປະຊາຊົນ";
                dtgv_emp.Columns["phone"].HeaderText = "ເບີໂທ";
                dtgv_emp.Columns["sex"].HeaderText = "ເພດ";
                dtgv_emp.Columns["positionName"].HeaderText = "ຕຳແໜ່ງ";
                dtgv_emp.Columns["deptName"].HeaderText = "ພະແນກ";
                dtgv_emp.Columns["benefitName"].HeaderText = "ສະວັດດີການ";
            }
            catch (Exception e)
            {
                MessageBox.Show("Can not connect to database");
            }


        }

        private void EmpReport_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();
            loadEmployee();
        }

        private void btn_exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            string strId = tb_empSearchId.Text.ToString();
            if (strId != "")
            {
                if (Regex.IsMatch(strId, @"^\d+$"))
                {
                    int id = Int32.Parse(strId.Trim());
                    try
                    {
                        DataTable dtEmp = DataProvider.Instance.ExecuteQuery("SELECT * from view_EmployeeReport WHERE empID = " + id);

                        dtgv_emp.DataSource = dtEmp;
                        dtgv_emp.Columns["empId"].HeaderText = "ລະຫັດພະນັກງານ";
                        dtgv_emp.Columns["fullName"].HeaderText = "ພະນັກງານ";
                        dtgv_emp.Columns["birthDay"].HeaderText = "ວັນເກີດ";
                        dtgv_emp.Columns["address"].HeaderText = "ທີ່ຢູ່";
                        dtgv_emp.Columns["identityCard"].HeaderText = "ເລກບັດປະຊາຊົນ";
                        dtgv_emp.Columns["phone"].HeaderText = "ເບີໂທ";
                        dtgv_emp.Columns["sex"].HeaderText = "ເພດ";
                        dtgv_emp.Columns["positionName"].HeaderText = "ຕຳແໜ່ງ";
                        dtgv_emp.Columns["deptName"].HeaderText = "ພະແນກ";
                        dtgv_emp.Columns["benefitName"].HeaderText = "ສະວັດດີການ";

                    }
                    catch (Exception ex)

                    {
                        MessageBox.Show(ex.GetType().Name);
                    }
                }
                else
                    MessageBox.Show("Input must be only number!", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);


            }
            else
            {
                MessageBox.Show("Please enter id");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Salary f = new Salary();
            f.ShowDialog();
        }
    }
}
