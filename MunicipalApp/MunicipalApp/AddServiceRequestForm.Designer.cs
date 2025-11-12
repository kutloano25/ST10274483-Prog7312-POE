using System.Drawing;
using System.Windows.Forms;

namespace MunicipalApp
{
    partial class AddServiceRequestForm
    {
        private TextBox txtID;
        private TextBox txtTitle;
        private ComboBox cboStatus;
        private TextBox txtPriority;
        private TextBox txtDependencies;
        private Button btnSave;
        private Button btnCancel;

        private void InitializeComponent()
        {
            this.Text = "Add New Service Request";
            this.Width = 500;
            this.Height = 400;
            this.StartPosition = FormStartPosition.CenterParent;

            var lblID = new Label() { Text = "Request ID:", Left = 20, Top = 20, Width = 100 };
            txtID = new TextBox() { Left = 150, Top = 18, Width = 300 };

            var lblTitle = new Label() { Text = "Title:", Left = 20, Top = 60, Width = 100 };
            txtTitle = new TextBox() { Left = 150, Top = 58, Width = 300 };

            var lblStatus = new Label() { Text = "Status:", Left = 20, Top = 100, Width = 100 };
            cboStatus = new ComboBox()
            {
                Left = 150,
                Top = 98,
                Width = 300,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cboStatus.Items.AddRange(new string[] { "Pending", "In Progress", "Completed" });
            cboStatus.SelectedIndex = 0;

            var lblPriority = new Label() { Text = "Priority:", Left = 20, Top = 140, Width = 100 };
            txtPriority = new TextBox() { Left = 150, Top = 138, Width = 300 };

            var lblDeps = new Label() { Text = "Dependencies (IDs):", Left = 20, Top = 180, Width = 130 };
            txtDependencies = new TextBox() { Left = 150, Top = 178, Width = 300 };

            btnSave = new Button() { Text = "Save", Left = 150, Top = 230, Width = 100 };
            btnSave.Click += new System.EventHandler(this.btnSave_Click);

            btnCancel = new Button() { Text = "Cancel", Left = 260, Top = 230, Width = 100 };
            btnCancel.Click += new System.EventHandler(this.btnCancel_Click);

            Controls.AddRange(new Control[]
            {
                lblID, txtID, lblTitle, txtTitle, lblStatus, cboStatus,
                lblPriority, txtPriority, lblDeps, txtDependencies, btnSave, btnCancel
            });
        }
    }
}
