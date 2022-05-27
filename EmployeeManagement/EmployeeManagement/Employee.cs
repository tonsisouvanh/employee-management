using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EmployeeManagement.DAO;
using System.Text.RegularExpressions;

namespace EmployeeManagement
{
    public partial class Employee : Form
    {
        public Employee()
        {
            InitializeComponent();
        }

        private string sex = "";
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

        private bool checkBirthDay(DateTime dob)
        {
            DateTime currentDate = DateTime.Now;

            int age = 0;
            age = currentDate.Subtract(dob).Days;
            age = age / 365;

            if (age < 18)
            {
                return false;
            }
            return true;
        }

        private bool checkForm()
        {
            string name = tb_empName.Text;
            string idCard = tb_empIdentity.Text;
            string address = tb_empAddresss.Text;
            string phone = tb_empPhone.Text;

            return name != "" && idCard != "" && address != "" && phone != "";
        }

        private void ResetAllTextBox()
        {
            tb_empId.ResetText();
            tb_empName.ResetText();
            dtpicker_birthDay.ResetText();
            tb_empIdentity.ResetText();
            tb_empAddresss.ResetText();
            tb_empPhone.ResetText();
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
            dtgv_emp.Enabled = true;
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
        }



        private void loadDepartment()
        {
            try
            {
                DataTable dtDept = DataProvider.Instance.ExecuteQuery("Select * from DEPARTMENT");

                cb_empDept.DataSource = dtDept;
                cb_empDept.DisplayMember = "deptName";
                cb_empDept.ValueMember = "deptID";

                cb_empDept.SelectedIndex = 0;
            }
            catch (Exception ex)

            {
                MessageBox.Show(ex.GetType().Name);
            }
        }

        private void loadPosition()
        {
            try
            {
                DataTable dtPosition = DataProvider.Instance.ExecuteQuery("Select * from POSITION");

                cb_empPosition.DataSource = dtPosition;
                cb_empPosition.DisplayMember = "positionName";
                cb_empPosition.ValueMember = "positionID";

                cb_empPosition.SelectedIndex = 0;
            }
            catch (Exception ex)

            {
                MessageBox.Show(ex.GetType().Name);
            }
        }

        private void loadBenefit()
        {
            try
            {
                DataTable dtBenefit = DataProvider.Instance.ExecuteQuery("Select * from BENEFIT");

                cb_empBenefit.DataSource = dtBenefit;
                cb_empBenefit.DisplayMember = "benefitName";
                cb_empBenefit.ValueMember = "benefitID";

                cb_empBenefit.SelectedIndex = 0;
            }
            catch (Exception ex)

            {
                MessageBox.Show(ex.GetType().Name);
            }
        }

        void loadEmployee()
        {
            SetBtEdit_Off();

            DataTable dtEmp = DataProvider.Instance.ExecuteQuery("Select * from view_Employee");
            dtgv_emp.DataSource = dtEmp;

            dtgv_emp.Columns["empID"].HeaderText = "ລະຫັດ";
            dtgv_emp.Columns["fullName"].HeaderText = "ພະນັກງານ";
            dtgv_emp.Columns["birthDay"].HeaderText = "ວັນເກີດ";
            dtgv_emp.Columns["address"].HeaderText = "ທີ່ຢູ່";
            dtgv_emp.Columns["identityCard"].HeaderText = "ເລກບັດປະຊາຊົນ";
            dtgv_emp.Columns["phone"].HeaderText = "ເບີໂທ";
            dtgv_emp.Columns["sex"].HeaderText = "ເພດ";
            dtgv_emp.Columns["positionID"].Visible = false;
            dtgv_emp.Columns["deptID"].Visible = false;
            dtgv_emp.Columns["benefitID"].Visible = false;
            dtgv_emp.Columns["positionName"].HeaderText = "ຕຳແໜ່ງ";
            dtgv_emp.Columns["deptName"].HeaderText = "ພະແນກ";
            dtgv_emp.Columns["benefitName"].HeaderText = "ສະວັດດີການ";

        }

        private void Employee_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();
            tb_empId.ReadOnly = true;

            loadDepartment();
            loadPosition();
            loadBenefit();
            loadEmployee();

        }

        private void dtgv_emp_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            ResetAllTextBox();
            if (dtgv_emp.SelectedCells.Count > 0)
            {
                int r = dtgv_emp.CurrentCell.RowIndex;
                tb_empId.Text = dtgv_emp.Rows[r].Cells[0].Value.ToString();
                tb_empName.Text = dtgv_emp.Rows[r].Cells[1].Value.ToString();
                dtpicker_birthDay.Text = dtgv_emp.Rows[r].Cells[2].Value.ToString();
                tb_empAddresss.Text = dtgv_emp.Rows[r].Cells[3].Value.ToString();
                tb_empIdentity.Text = dtgv_emp.Rows[r].Cells[4].Value.ToString();
                tb_empPhone.Text = dtgv_emp.Rows[r].Cells[5].Value.ToString();
                if (dtgv_emp.Rows[r].Cells[6].Value.ToString() == "FM")
                {
                    radioFemale.Checked = true;
                    radioMale.Checked = false;

                }
                else
                {
                    radioFemale.Checked = false;
                    radioMale.Checked = true;
                }
                cb_empPosition.SelectedValue = dtgv_emp.Rows[r].Cells[7].Value;
                cb_empDept.SelectedValue = dtgv_emp.Rows[r].Cells[8].Value;
                cb_empBenefit.SelectedValue = dtgv_emp.Rows[r].Cells[9].Value;

                btnEdit.Enabled = true;
            }
            else
            {
                btnEdit.Enabled = false;
            }
        }


        private void radioFemale_CheckedChanged(object sender, EventArgs e)
        {
            sex = "FM";
        }

        private void radioMale_CheckedChanged(object sender, EventArgs e)
        {
            sex = "M";
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            add = true;
            ResetAllTextBox();
            SetBtEdit_On();
            tb_empName.Focus();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            loadEmployee();
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
                dtgv_emp_CellContentClick(null, null);
                SetBtEdit_On();
                tb_empName.Focus();
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
                if (add)
                {
                    try
                    {
                        DateTime birthday = dtpicker_birthDay.Value;

                        if (!checkBirthDay(birthday))
                        {
                            MessageBox.Show("Date is invalid", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        string sex = this.sex;
                        int positionId = Int32.Parse(cb_empPosition.SelectedValue.ToString());
                        int deptId = Int32.Parse(cb_empDept.SelectedValue.ToString());
                        int benefitId = Int32.Parse(cb_empBenefit.SelectedValue.ToString());

                        string query = "exec sp_AddEmployee @fullname , @birthday , @address , @identityCard , @phone , @sex , @positionID , @deptID , @benefitID";
                        int result = DataProvider.Instance.ExecuteNoneQuery(query, new object[] { tb_empName.Text.ToString(), birthday, tb_empAddresss.Text.ToString(),
                                                                                    tb_empIdentity.Text.ToString(),tb_empPhone.Text.ToString(),
                                                                                    sex, positionId, deptId, benefitId});

                        if(result != 0)
                            MessageBox.Show("Success!", "Info Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        else
                            MessageBox.Show("Failed!", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                    loadEmployee();
                }
                else
                {
                    try
                    {
                        DateTime birthday = dtpicker_birthDay.Value;
                        int empId = Int32.Parse(tb_empId.Text.ToString());
                        string sex = this.sex;
                        int positionId = Int32.Parse(cb_empPosition.SelectedValue.ToString());
                        int deptId = Int32.Parse(cb_empDept.SelectedValue.ToString());
                        int benefitId = Int32.Parse(cb_empBenefit.SelectedValue.ToString());

                        string query = "exec sp_UpdateEmployee @empID , @fullname , @birthday , @address , @identityCard , @phone , @sex , @positionID , @deptID , @benefitID";
                        int result = DataProvider.Instance.ExecuteNoneQuery(query, new object[] {empId, tb_empName.Text.ToString(), birthday, tb_empAddresss.Text.ToString(),
                                                                                    tb_empIdentity.Text.ToString(),tb_empPhone.Text.ToString(),
                                                                                    sex, positionId, deptId, benefitId});

                        if (result != 0)
                            MessageBox.Show("Success!", "Info Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        else
                            MessageBox.Show("Failed!", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                    loadEmployee();
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
                if (tb_empId.Text != "")
                {
                    try
                    {
                        int empId = Int32.Parse(tb_empId.Text.ToString());

                        string query = "exec sp_DeleteEmployee @empId";
                        int result = DataProvider.Instance.ExecuteNoneQuery(query, new object[] { empId });
                        string message = (result != 0) ? "Deleted" : "Failed";
                        MessageBox.Show(message, "Info Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    loadEmployee();
                }
                else
                {
                    MessageBox.Show("Please select employee!", "Warning Message",MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
            }

        }

        private void Employee_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Close();
            Menu f = new Menu();
            f.Show();
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            string strId = tb_empSearchId.Text.ToString();
            if (strId != "")
            {
                if (Regex.IsMatch(strId, @"^\d+$"))
                {
                    int id = Int32.Parse(strId.Trim());
                    try
                    {
                        DataTable dtEmp = DataProvider.Instance.ExecuteQuery("SELECT * from EMPLOYEE WHERE empID = " + id);

                        dtgv_emp.DataSource = dtEmp;

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
    }
}
