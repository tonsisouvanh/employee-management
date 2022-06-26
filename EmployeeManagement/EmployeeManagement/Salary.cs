using EmployeeManagement.DAO;
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
    public partial class Salary : Form
    {
        public Salary()
        {
            InitializeComponent();
        }

        private bool add = false;
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

        private void Salary_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();
            tb_empId.ReadOnly = true;
            loadEmpData();
            tb_salaryDeduction.Text = "0";
        }

        private bool checkForm()
        {
            string salary = tb_empName.Text;
            string createdDate = dtp_createdDate.Text;
            string empId = tb_empId.Text;

            return salary != "" && createdDate != "" && empId != "";
        }

        private void ResetAllTextBox()
        {
            //tb_empId.ResetText();
            //tb_empName.ResetText();
            //tb_empRank.ResetText();
            dtp_createdDate.ResetText();
            tb_salary.ResetText();
            //tb_salaryDeduction.ResetText();
            tb_salaryToPay.ResetText();
        }

        private void SetBtEdit_Off()
        {
            btnSave.Enabled = false;
            btnCancel.Enabled = false;
            panel_form.Enabled = false;
            btnAdd.Enabled = true;
            btnDelete.Enabled = true;
            btnReload.Enabled = true;
            btnExit.Enabled = true;
            btnEdit.Enabled = true;
            dtgv_emp.Enabled = true;
            dtgv_salary.Enabled = true;


            this.add = false;
        }

        private void SetBtEdit_On()
        {
            btnSave.Enabled = true;
            btnCancel.Enabled = true;
            panel_form.Enabled = true;
            btnAdd.Enabled = false;
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
            btnExit.Enabled = false;
            dtgv_emp.Enabled = false;
            dtgv_salary.Enabled = false;
        }

        private void loadSalary(int id)
        {
            SetBtEdit_Off();
            try
            {
                DataTable dtSalary = DataProvider.Instance.ExecuteQuery("Select * from SALARY where empID = " + id +" order by createdDate asc");
                dtgv_salary.DataSource = dtSalary;

                dtgv_salary.Columns["salary"].DefaultCellStyle.Format = "N0";
                dtgv_salary.Columns["salaryDeduction"].DefaultCellStyle.Format = "N0";
                dtgv_salary.Columns["salaryToPay"].DefaultCellStyle.Format = "N0";
                dtgv_salary.Columns["createdDate"].DefaultCellStyle.Format = "dd/MM/yyyy";



                dtgv_salary.Columns["empID"].HeaderText = "ລະຫັດ";
                dtgv_salary.Columns["createdDate"].HeaderText = "ວັນອອກເງິນ";
                dtgv_salary.Columns["salary"].HeaderText = "ເງິນເດືອນຕົວຈິງ";
                dtgv_salary.Columns["salaryDeduction"].HeaderText = "ເງິນເດືອນຫັກອອກ";
                dtgv_salary.Columns["salaryToPay"].HeaderText = "ເງິນເດືອນທີ່ຕ້ອງຈ່າຍ";


            }
            catch (Exception e)
            {
                MessageBox.Show("Can not connect to database");
            }
        }
        private void loadEmpData()
        {
            SetBtEdit_Off();
            try
            {
                DataTable dtEmp = DataProvider.Instance.ExecuteQuery("Select empID,fullName,empRank,positionName,deptName from view_Employee");
                dtgv_emp.DataSource = dtEmp;

                dtgv_emp.Columns["empID"].HeaderText = "ລະຫັດ";
                dtgv_emp.Columns["fullName"].HeaderText = "ພະນັກງານ";
                dtgv_emp.Columns["empRank"].HeaderText = "ຊັ້ນ";
                dtgv_emp.Columns["positionName"].HeaderText = "ຕຳແໜ່ງ";
                dtgv_emp.Columns["deptName"].HeaderText = "ພະແນກ";
            }
            catch (Exception e)
            {
                MessageBox.Show("Can not connect to database");
            }
        }


        private void dtgv_emp_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            ResetAllTextBox();
            if (dtgv_emp.SelectedCells.Count > 0)
            {
                int id = 0;
                int r = dtgv_emp.CurrentCell.RowIndex;
                if(dtgv_emp.Rows[r].Cells[0].Value.ToString() != "")
                {
                    id = Int32.Parse(dtgv_emp.Rows[r].Cells[0].Value.ToString());
                }
                tb_empId.Text = dtgv_emp.Rows[r].Cells[0].Value.ToString();
                tb_empName.Text = dtgv_emp.Rows[r].Cells[1].Value.ToString();
                tb_empRank.Text = dtgv_emp.Rows[r].Cells[2].Value.ToString();

                loadSalary(id);

                btnEdit.Enabled = true;
            }
            else
            {
                btnEdit.Enabled = false;
            }
        }

        private void dtgv_salary_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            ResetAllTextBox();

            if (dtgv_salary.SelectedCells.Count > 0)
            {
                int r = dtgv_salary.CurrentCell.RowIndex;
                //int id = Int32.Parse(dtgv_salary.Rows[r].Cells[0].Value.ToString());
                dtp_createdDate.Text = dtgv_salary.Rows[r].Cells[0].Value.ToString();
                //tb_empId.Text = dtgv_salary.Rows[r].Cells[0].Value.ToString();
                tb_salary.Text = dtgv_salary.Rows[r].Cells[2].Value.ToString();
                tb_salaryDeduction.Text = dtgv_salary.Rows[r].Cells[3].Value.ToString();
                tb_salaryToPay.Text = dtgv_salary.Rows[r].Cells[4].Value.ToString();

                btnEdit.Enabled = true;
            }
            else
            {
                btnEdit.Enabled = false;
            }

        }


        private void btnAdd_Click(object sender, EventArgs e)
        {
            add = true;
            ResetAllTextBox();
            SetBtEdit_On();
            tb_salary.Focus();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            SetBtEdit_Off();
            ResetAllTextBox();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dtgv_emp.SelectedCells.Count > 0)
            {
                add = false;
                //dtgv_emp_CellContentClick(null, null);
                SetBtEdit_On();
                tb_salary.Focus();
            }
            else
            {
                MessageBox.Show($"Số lượng bản ghi bằng \"{dtgv_emp.SelectedCells.Count}\". Không thể sửa.");
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!checkForm())
            {
                MessageBox.Show("Please, Fill all the required fields!", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                int empId = Int32.Parse(tb_empId.Text.ToString());
                if (add)
                {
                    try
                    {
                        DateTime createdDate = dtp_createdDate.Value;

                        //if (!checkBirthDay(birthday))
                        //{
                        //    MessageBox.Show("Date is invalid", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        //    return;
                        //}
                        int salary = Int32.Parse(tb_salary.Text.ToString());
                        int salaryDeduction = Int32.Parse(tb_salaryDeduction.Text.ToString());

                        string query = "exec sp_AddSalary @createdDate , @empId , @salary , @salaryDeduction";
                        int result = DataProvider.Instance.ExecuteNoneQuery(query, new object[] { createdDate, empId, salary,salaryDeduction});

                        if (result != 0)
                            MessageBox.Show("Success!", "Info Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        else
                            MessageBox.Show("Failed!", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                    loadEmpData();
                    loadSalary(empId);
                }
                else
                {
                    try
                    {
                        DateTime createdDate = dtp_createdDate.Value;

                        //if (!checkBirthDay(birthday))
                        //{
                        //    MessageBox.Show("Date is invalid", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        //    return;
                        //}
                        int salary = Int32.Parse(tb_salary.Text.ToString());
                        int salaryDeduction = Int32.Parse(tb_salaryDeduction.Text.ToString());

                        string query = "exec sp_UpdateSalary @createdDate , @empId , @salary , @salaryDeduction";
                        int result = DataProvider.Instance.ExecuteNoneQuery(query, new object[] { createdDate, empId, salary, salaryDeduction });

                        if (result != 0)
                            MessageBox.Show("Success!", "Info Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        else
                            MessageBox.Show("Failed!", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                    loadEmpData();
                    loadSalary(empId);
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            var confirmResult = MessageBox.Show("Are you sure to delete?",
                                     "Confirm Delete!!",
                                     MessageBoxButtons.YesNo);
            if (confirmResult == DialogResult.Yes)
            {
                if (tb_empId.Text != "" && dtp_createdDate.Text != "")
                {
                    int empId = Int32.Parse(tb_empId.Text.ToString());
                    try
                    {
                        DateTime createdDate = dtp_createdDate.Value;


                        string query = "exec sp_DeleteSalary @empId , @createdDate";
                        int result = DataProvider.Instance.ExecuteNoneQuery(query, new object[] { empId,createdDate });
                        string message = (result != 0) ? "Deleted" : "Failed";
                        MessageBox.Show(message, "Info Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    loadEmpData();
                    loadSalary(empId);

                }
                else
                {
                    MessageBox.Show("Please select employee and salary date!", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
            }

        }


        private void btnReload_Click(object sender, EventArgs e)
        {
            loadEmpData();
        }
    }
}
