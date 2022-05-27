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


namespace EmployeeManagement
{
    public partial class Department : Form
    {
        public Department()
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

        private bool checkForm()
        {
            string deptName = tb_deptName.Text.ToString();
            return deptName != "";
        }

        private void ResetAllTextBox()
        {
            tb_deptId.ResetText();
            tb_deptName.ResetText();
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
            dtgv_dept.Enabled = true;
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
            dtgv_dept.Enabled = false;
        }

        void loadDept()
        {
            SetBtEdit_Off();

            DataTable dtDept = DataProvider.Instance.ExecuteQuery("Select * from DEPARTMENT");
            dtgv_dept.DataSource = dtDept;

            dtgv_dept.Columns["deptID"].HeaderText = "ລະຫັດພະແນກ";
            dtgv_dept.Columns["deptName"].HeaderText = "ພະແນກ";
        }

        private void Department_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();
            tb_deptId.ReadOnly = true;
            loadDept();
        }

        private void dtgv_dept_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            ResetAllTextBox();
            if (dtgv_dept.SelectedCells.Count > 0)
            {
                int r = dtgv_dept.CurrentCell.RowIndex;
                tb_deptId.Text = dtgv_dept.Rows[r].Cells[0].Value.ToString();
                tb_deptName.Text = dtgv_dept.Rows[r].Cells[1].Value.ToString();

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
            tb_deptName.Focus();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            loadDept();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            SetBtEdit_Off();
            ResetAllTextBox();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dtgv_dept.SelectedCells.Count > 0)
            {
                add = false;
                dtgv_dept_CellContentClick(null, null);
                SetBtEdit_On();
                tb_deptName.Focus();
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
                        string query = "exec sp_AddDepartment @deptName";
                        int result = DataProvider.Instance.ExecuteNoneQuery(query, new object[] { tb_deptName.Text.ToString() });

                        if (result != 0)
                            MessageBox.Show("Success!", "Info Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        else
                            MessageBox.Show("Failed!", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                }
                else
                {
                    try
                    {
                        int deptId = Int32.Parse(tb_deptId.Text.ToString());
                        string query = "exec sp_UpdateDepartment @deptID , @deptName";
                        int result = DataProvider.Instance.ExecuteNoneQuery(query, new object[] { deptId, tb_deptName.Text.ToString() });

                        if (result != 0)
                            MessageBox.Show("Success!", "Info Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        else
                            MessageBox.Show("Failed!", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                }
                loadDept();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            var confirmResult = MessageBox.Show("Are you sure to delete?",
                                     "Confirm Delete!!",
                                     MessageBoxButtons.YesNo);
            if (confirmResult == DialogResult.Yes)
            {
                if (tb_deptId.Text != "")
                {
                    try
                    {
                        int depId = Int32.Parse(tb_deptId.Text.ToString());

                        string query = "exec sp_DeleteDepartment @deptID";
                        int result = DataProvider.Instance.ExecuteNoneQuery(query, new object[] { depId });
                        string message = (result != 0) ? "Deleted" : "Failed";
                        MessageBox.Show(message, "Info Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    loadDept();
                }
                else
                {
                    MessageBox.Show("Please select department!", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
            }

        }
    }
}

