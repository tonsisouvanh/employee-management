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
    public partial class Position : Form
    {
        public Position()
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
            string positName = tb_positionName.Text.ToString();
            return positName != "";
        }

        private void ResetAllTextBox()
        {
            tb_positionId.ResetText();
            tb_positionName.ResetText();
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
            dtgv_position.Enabled = true;
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
            dtgv_position.Enabled = false;
        }

        void loadPosition()
        {
            SetBtEdit_Off();

            DataTable dtPosit = DataProvider.Instance.ExecuteQuery("Select * from POSITION");
            dtgv_position.DataSource = dtPosit;

            dtgv_position.Columns["positionID"].HeaderText = "ລະຫັດ";
            dtgv_position.Columns["positionName"].HeaderText = "ຕຳແໜ່ງ";

        }

        private void Position_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();
            tb_positionId.ReadOnly = true;
            loadPosition();
        }

        private void dtgv_position_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            ResetAllTextBox();
            if (dtgv_position.SelectedCells.Count > 0)
            {
                int r = dtgv_position.CurrentCell.RowIndex;
                tb_positionId.Text = dtgv_position.Rows[r].Cells[0].Value.ToString();
                tb_positionName.Text = dtgv_position.Rows[r].Cells[1].Value.ToString();

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
            tb_positionName.Focus();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            loadPosition();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            SetBtEdit_Off();
            ResetAllTextBox();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dtgv_position.SelectedCells.Count > 0)
            {
                add = false;
                dtgv_position_CellContentClick(null, null);
                SetBtEdit_On();
                tb_positionName.Focus();
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
                        string query = "exec sp_AddPosition @positionName";
                        int result = DataProvider.Instance.ExecuteNoneQuery(query, new object[] { tb_positionName.Text.ToString() });

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
                        int positionId = Int32.Parse(tb_positionId.Text.ToString());
                        string query = "exec sp_UpdatePosition @positionID , @positionName";
                        int result = DataProvider.Instance.ExecuteNoneQuery(query, new object[] { positionId, tb_positionName.Text.ToString() });

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
                loadPosition();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

            var confirmResult = MessageBox.Show("Are you sure to delete?",
                                     "Confirm Delete!!",
                                     MessageBoxButtons.YesNo);
            if (confirmResult == DialogResult.Yes)
            {
                if (tb_positionId.Text != "")
                {
                    try
                    {
                        int positId = Int32.Parse(tb_positionId.Text.ToString());

                        string query = "exec sp_DeletePosition @positionID";
                        int result = DataProvider.Instance.ExecuteNoneQuery(query, new object[] { positId });
                        string message = (result != 0) ? "Deleted" : "Failed";
                        MessageBox.Show(message, "Info Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    loadPosition();
                }
                else
                {
                    MessageBox.Show("Please select benefit!", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

        }
    }
}
