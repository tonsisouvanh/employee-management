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
    public partial class Benefit : Form
    {
        public Benefit()
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
            string benefName = tb_benefName.Text.ToString();
            return benefName != "";
        }

        private void ResetAllTextBox()
        {
            tb_benefID.ResetText();
            tb_benefName.ResetText();
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
            dtgv_benefit.Enabled = true;
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
            dtgv_benefit.Enabled = false;
        }

        void loadBenefit()
        {
            SetBtEdit_Off();

            DataTable dtDept = DataProvider.Instance.ExecuteQuery("Select * from BENEFIT");
            dtgv_benefit.DataSource = dtDept;
            dtgv_benefit.Columns["benefitID"].HeaderText = "ລະຫັດສະວັດດີການ";
            dtgv_benefit.Columns["benefitName"].HeaderText = "ສະວັດດີການ";
            dtgv_benefit.Columns["enrollNumber"].HeaderText = "ຈຳນວນຄົນ";
        }

        private void Benefit_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();
            tb_benefID.ReadOnly = true;
            loadBenefit();
        }

        private void dtgv_benefit_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            ResetAllTextBox();
            if (dtgv_benefit.SelectedCells.Count > 0)
            {
                int r = dtgv_benefit.CurrentCell.RowIndex;
                tb_benefID.Text = dtgv_benefit.Rows[r].Cells[0].Value.ToString();
                tb_benefName.Text = dtgv_benefit.Rows[r].Cells[1].Value.ToString();

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
            tb_benefName.Focus();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            loadBenefit();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            SetBtEdit_Off();
            ResetAllTextBox();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dtgv_benefit.SelectedCells.Count > 0)
            {
                add = false;
                dtgv_benefit_CellContentClick(null, null);
                SetBtEdit_On();
                tb_benefName.Focus();
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
                        string query = "exec sp_AddBenefit @benefitName";
                        int result = DataProvider.Instance.ExecuteNoneQuery(query, new object[] { tb_benefName.Text.ToString() });

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
                        int benefitId = Int32.Parse(tb_benefID.Text.ToString());
                        string query = "exec sp_UpdateBenefit @benefitID , @benefitName";
                        int result = DataProvider.Instance.ExecuteNoneQuery(query, new object[] { benefitId, tb_benefName.Text.ToString() });

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
                loadBenefit();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

            var confirmResult = MessageBox.Show("Are you sure to delete?",
                                     "Confirm Delete!!",
                                     MessageBoxButtons.YesNo);
            if (confirmResult == DialogResult.Yes)
            {
                if (tb_benefID.Text != "")
                {
                    try
                    {
                        int benefitId = Int32.Parse(tb_benefID.Text.ToString());

                        string query = "exec sp_DeleteBenefit @benefitID";
                        int result = DataProvider.Instance.ExecuteNoneQuery(query, new object[] { benefitId });
                        string message = (result != 0) ? "Deleted" : "Failed";
                        MessageBox.Show(message, "Info Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    loadBenefit();
                }
                else
                {
                    MessageBox.Show("Please select benefit!", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
            }
        }

        private void Benefit_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Close();
            Menu f = new Menu();
            f.Show();
        }
    }
}
