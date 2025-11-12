using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace MunicipalApp
{
    public partial class AddServiceRequestForm : Form
    {
        // We'll use this to send the new request back to ServiceRequestStatusForm
        public ServiceRequest CreatedRequest { get; private set; }

        public AddServiceRequestForm()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Validate all inputs
            if (!int.TryParse(txtID.Text.Trim(), out int id))
            {
                MessageBox.Show("Enter a valid numeric Request ID.", "Error");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                MessageBox.Show("Title cannot be empty.", "Error");
                return;
            }

            if (!int.TryParse(txtPriority.Text.Trim(), out int priority))
            {
                MessageBox.Show("Priority must be a number.", "Error");
                return;
            }

            // Dependencies can be comma separated IDs
            var deps = new List<int>();
            if (!string.IsNullOrWhiteSpace(txtDependencies.Text))
            {
                deps = txtDependencies.Text.Split(',')
                    .Select(x => x.Trim())
                    .Where(x => int.TryParse(x, out _))
                    .Select(int.Parse)
                    .ToList();
            }

            // Create the new request
            CreatedRequest = new ServiceRequest
            {
                RequestID = id,
                Title = txtTitle.Text.Trim(),
                Status = cboStatus.SelectedItem?.ToString() ?? "Pending",
                Priority = priority,
                Dependencies = deps
            };

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
