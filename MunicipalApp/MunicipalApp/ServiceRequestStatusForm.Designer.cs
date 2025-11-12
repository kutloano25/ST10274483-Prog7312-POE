using System.Drawing;
using System.Windows.Forms;

namespace MunicipalApp
{
    partial class ServiceRequestStatusForm
    {
        private ListView lvRequests;
        private TextBox txtSearchID;
        private Button btnSearch;
        private Button btnShowDependencies;
        private Button btnBack;
        private Button btnAddNew;

        private void InitializeComponent()
        {
            this.Text = "Service Request Status";
            this.Width = 800;
            this.Height = 600;
            this.StartPosition = FormStartPosition.CenterParent;

            lvRequests = new ListView()
            {
                Left = 20,
                Top = 20,
                Width = 740,
                Height = 400,
                View = View.Details,
                FullRowSelect = true

            };
            lvRequests.Columns.Add("ID", 80);
            lvRequests.Columns.Add("Title", 220);
            lvRequests.Columns.Add("Status", 150);
            lvRequests.Columns.Add("Priority", 100);
            Controls.Add(lvRequests);

            var lblSearch = new Label() { Text = "Request ID:", Left = 20, Top = 440, Width = 100 };
            txtSearchID = new TextBox() { Left = 110, Top = 436, Width = 100 };
            btnSearch = new Button() { Text = "Search", Left = 220, Top = 434, Width = 100 };
            btnSearch.Click += new System.EventHandler(this.btnSearch_Click);

            btnShowDependencies = new Button() { Text = "Show Dependencies", Left = 330, Top = 434, Width = 150 };
            btnShowDependencies.Click += new System.EventHandler(this.btnShowDependencies_Click);

            btnBack = new Button() { Text = "Back", Left = 660, Top = 434, Width = 100 };
            btnBack.Click += (s, e) => Close();

            Controls.AddRange(new Control[] { lblSearch, txtSearchID, btnSearch, btnShowDependencies, btnBack });
        }
    }
}
