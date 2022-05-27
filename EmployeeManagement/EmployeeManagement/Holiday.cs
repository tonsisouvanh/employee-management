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
    public partial class Holiday : Form
    {
        public Holiday()
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
            string reason = tb_holiReason.Text;
            string create = dtpicker_createdDate.Text;
            string start = dtp_startedDate.Text;
            string end = dtp_endDate.Text;

            return reason != "" && create != "" && start != "" && end != "";
        }

        private void ResetAllTextBox()
        {
            tb_holiId.ResetText();
            tb_holiReason.ResetText();
            tb_holiDay.ResetText();
            dtpicker_createdDate.ResetText();
            dtp_startedDate.ResetText();
            dtp_endDate.ResetText();
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
            dtgv_holiday.Enabled = true;
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
            dtgv_holiday.Enabled = false;
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

        
        void loadHoliday()
        {
            SetBtEdit_Off();

            DataTable dtHoliday = DataProvider.Instance.ExecuteQuery("Select HOLIDAY.*, EMPLOYEE.fullName from HOLIDAY, EMPLOYEE "
                                                                        + "where HOLIDAY.empID = EMPLOYEE.empID");
            dtgv_holiday.DataSource = dtHoliday;

            dtgv_holiday.Columns["holidayID"].HeaderText = "ລະຫັດ";
            dtgv_holiday.Columns["reason"].HeaderText = "ເຫດຜົນ";
            dtgv_holiday.Columns["restDays"].HeaderText = "ຈຳນວນວັນພັກ";
            dtgv_holiday.Columns["createdDate"].HeaderText = "ວັນຂໍພັກ";
            dtgv_holiday.Columns["startedDate"].HeaderText = "ວັນເລີ່ມພັກ";
            dtgv_holiday.Columns["endDate"].HeaderText = "ຈົນເຖິງ";
            dtgv_holiday.Columns["fullName"].HeaderText = "ພະນັກງານ";
            dtgv_holiday.Columns["empID"].HeaderText = "ລະຫັດພະນັກງານ";

        }

        private void Holiday_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();
            tb_holiId.ReadOnly = true;
            tb_holiDay.ReadOnly = true;
            dtpicker_createdDate.Enabled = false;


            loadEmp();
            loadHoliday();

        }

        private void dtgv_holiday_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            ResetAllTextBox();
            if (dtgv_holiday.SelectedCells.Count > 0)
            {
                int r = dtgv_holiday.CurrentCell.RowIndex;
                tb_holiId.Text = dtgv_holiday.Rows[r].Cells[0].Value.ToString();
                tb_holiReason.Text = dtgv_holiday.Rows[r].Cells[1].Value.ToString();
                tb_holiDay.Text = dtgv_holiday.Rows[r].Cells[2].Value.ToString();
                dtpicker_createdDate.Text = dtgv_holiday.Rows[r].Cells[3].Value.ToString();
                dtp_startedDate.Text = dtgv_holiday.Rows[r].Cells[4].Value.ToString();
                dtp_endDate.Text = dtgv_holiday.Rows[r].Cells[5].Value.ToString();
                cb_emp.SelectedValue = dtgv_holiday.Rows[r].Cells[6].Value;
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
            tb_holiReason.Focus();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            loadHoliday();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            SetBtEdit_Off();
            ResetAllTextBox();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dtgv_holiday.SelectedCells.Count > 0)
            {
                add = false;
                dtgv_holiday_CellContentClick(null, null);
                SetBtEdit_On();
                tb_holiReason.Focus();
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
                        DateTime startedDate = dtp_startedDate.Value;
                        DateTime endDate = dtp_endDate.Value;

                        
                        int empId = Int32.Parse(cb_emp.SelectedValue.ToString());

                        string query = "exec sp_AddHoliday @reason , @startedDate , @endDate , @empID";
                        int result = DataProvider.Instance.ExecuteNoneQuery(query, new object[] { tb_holiReason.Text.ToString(), startedDate, endDate, empId});

                        if (result != 0)
                            MessageBox.Show("Success!", "Info Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        else
                            MessageBox.Show("Failed!", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                    loadHoliday();
                }
                else
                {
                    try
                    {
                        DateTime startedDate = dtp_startedDate.Value;
                        DateTime endDate = dtp_endDate.Value;


                        int empId = Int32.Parse(cb_emp.SelectedValue.ToString());
                        int holidayId = Int32.Parse(tb_holiId.Text.ToString());

                        string query = "exec sp_UpdateHoliday @holidayID , @reason , @startedDate , @endDate , @empID";
                        int result = DataProvider.Instance.ExecuteNoneQuery(query, new object[] { holidayId, tb_holiReason.Text.ToString(), startedDate, endDate, empId });

                        if (result != 0)
                            MessageBox.Show("Success!", "Info Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        else
                            MessageBox.Show("Failed!", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                    loadHoliday();
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
                if (tb_holiId.Text != "")
                {
                    try
                    {
                        int holiId = Int32.Parse(tb_holiId.Text.ToString());

                        string query = "exec sp_DeleteHoliday @holidayID";
                        int result = DataProvider.Instance.ExecuteNoneQuery(query, new object[] { holiId });
                        string message = (result != 0) ? "Deleted" : "Failed";
                        MessageBox.Show(message, "Info Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    loadHoliday();
                }
                else
                {
                    MessageBox.Show("Please select to remove!", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
            }
            

        }

        private void Holiday_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Close();
            Menu f = new Menu();
            f.Show();
        }
    }
}
