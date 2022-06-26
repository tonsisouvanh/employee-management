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
    public partial class Working : Form
    {
        public Working()
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
            string startAt = dtp_startAt.Text.ToString();
            string stopAt = dtp_stopAt.Text.ToString();
            string createdDate = dtpicker_createdDate.Text.ToString();

            return startAt != "" && stopAt != "" && createdDate != "";
        }

        private void ResetAllTextBox()
        {
            tb_workId.ResetText();
            tb_workHour.ResetText();
            dtpicker_createdDate.ResetText();
            dtp_startAt.ResetText();
            dtp_stopAt.ResetText();
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
            dtgv_working.Enabled = true;
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
            dtgv_working.Enabled = false;
        }



        private void loadEmp()
        {
            try
            {
                DataTable dtEmp = DataProvider.Instance.ExecuteQuery("Select * from EMPLOYEE");

                cb_emp.DataSource = dtEmp;
                cb_emp.DisplayMember = "fullName";
                cb_emp.ValueMember = "empID";

                cb_emp.SelectedIndex = 0;
            }
            catch (Exception ex)

            {
                MessageBox.Show(ex.GetType().Name);
            }
        }


        void loadWorking()
        {
            SetBtEdit_Off();

            DataTable dtWorking = DataProvider.Instance.ExecuteQuery("Select WORK_TIME.*, EMPLOYEE.fullName from WORK_TIME, EMPLOYEE "
                                                                        + "where WORK_TIME.empID = EMPLOYEE.empID");
            dtgv_working.DataSource = dtWorking;

            dtgv_working.Columns["createdDate"].DefaultCellStyle.Format = "dd/MM/yyyy";

            dtgv_working.Columns["workTimeID"].HeaderText = "ລະຫັດ";
            dtgv_working.Columns["startWorkAt"].HeaderText = "ເວລາເຂົ້າວຽກ";
            dtgv_working.Columns["stopWorkAt"].HeaderText = "ເວລາອອກວຽກ";
            dtgv_working.Columns["workHour"].HeaderText = "ຈຳນວນຊົ່ວໂມງ";
            dtgv_working.Columns["createdDate"].HeaderText = "ວັນເຂົ້າວຽກ";
            dtgv_working.Columns["fullName"].HeaderText = "ພະນັກງານ";
            dtgv_working.Columns["empID"].HeaderText = "ລະຫັດພະນັກງານ";

        }

        private void dtgv_working_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            ResetAllTextBox();
            if (dtgv_working.SelectedCells.Count > 0)
            {
                int r = dtgv_working.CurrentCell.RowIndex;
                tb_workId.Text = dtgv_working.Rows[r].Cells[0].Value.ToString();
                dtp_startAt.Text = dtgv_working.Rows[r].Cells[1].Value.ToString();
                dtp_stopAt.Text = dtgv_working.Rows[r].Cells[2].Value.ToString();
                tb_workHour.Text = dtgv_working.Rows[r].Cells[3].Value.ToString();
                dtpicker_createdDate.Text = dtgv_working.Rows[r].Cells[4].Value.ToString();
                cb_emp.SelectedValue = dtgv_working.Rows[r].Cells[5].Value;
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
            dtp_startAt.Focus();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            loadWorking();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            SetBtEdit_Off();
            ResetAllTextBox();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dtgv_working.SelectedCells.Count > 0)
            {
                add = false;
                dtgv_working_CellContentClick(null, null);
                SetBtEdit_On();
                dtp_startAt.Focus();
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
                        DateTime startAt = dtp_startAt.Value;
                        DateTime stopAt = dtp_stopAt.Value;


                        int empId = Int32.Parse(cb_emp.SelectedValue.ToString());

                        string query = "exec sp_AddWorkTime @startAt , @stopAt , @empID";
                        int result = DataProvider.Instance.ExecuteNoneQuery(query, new object[] { startAt, stopAt, empId });

                        if (result != 0)
                            MessageBox.Show("Success!", "Info Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        else
                            MessageBox.Show("Failed!", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                    loadWorking();
                }
                else
                {
                    try
                    {
                        DateTime startAt = dtp_startAt.Value;
                        DateTime stopAt = dtp_stopAt.Value;
                        DateTime createdDate = dtpicker_createdDate.Value;


                        int empId = Int32.Parse(cb_emp.SelectedValue.ToString());
                        int workId = Int32.Parse(tb_workId.Text.ToString());


                        string query = "exec sp_UpdateWorkTime @workId , @startAt , @stopAt , @createdDate , @empID";
                        int result = DataProvider.Instance.ExecuteNoneQuery(query, new object[] {workId, startAt, stopAt,createdDate, empId });

                        if (result != 0)
                            MessageBox.Show("Success!", "Info Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        else
                            MessageBox.Show("Failed!", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                    loadWorking();
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
                if (tb_workId.Text != "")
                {
                    try
                    {
                        int workId = Int32.Parse(tb_workId.Text.ToString());

                        string query = "exec sp_DeleteWorkTime @workId";
                        int result = DataProvider.Instance.ExecuteNoneQuery(query, new object[] { workId });
                        string message = (result != 0) ? "Deleted" : "Failed";
                        MessageBox.Show(message, "Info Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    loadWorking();
                }
                else
                {
                    MessageBox.Show("Please select to remove!", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
            }

        }
        private void Working_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();
            tb_workId.ReadOnly = true;
            tb_workHour.ReadOnly = true;
            dtpicker_createdDate.Enabled = false;


            loadEmp();
            loadWorking();
        }

    }
}
