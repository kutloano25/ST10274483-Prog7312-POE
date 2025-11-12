using System;
using System.Drawing;
using System.Windows.Forms;

namespace MunicipalApp
{
    public class MainForm : Form
    {
        private Button btnReportIssues;
        private Button btnLocalEvents;
        private Button btnServiceStatus;
        private Label lblTitle;

        public MainForm()
        {
            Text = "Municipal Services - Main Menu";
            Width = 420;
            Height = 260;
            StartPosition = FormStartPosition.CenterScreen;
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            lblTitle = new Label()
            {
                Text = "Municipal Services Application",
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Height = 60
            };
            Controls.Add(lblTitle);

            btnReportIssues = new Button()
            {
                Text = "Report Issues",
                Width = 300,
                Height = 40,
                Left = 55,
                Top = 80
            };
            btnReportIssues.Click += (s, e) => { new ReportIssuesForm().ShowDialog(); };

            btnLocalEvents = new Button()
            {
                Text = "Local Events & Announcements (Coming soon)",
                Enabled = true, // we want to implement for Part 2 — button opens EventsForm
                Width = 300,
                Height = 40,
                Left = 55,
                Top = 130
            };
            btnLocalEvents.Click += (s, e) => { new EventsForm().ShowDialog(); };

            btnServiceStatus = new Button()
            {
                Text = "Service Request Status",
                Enabled = true,
                Width = 300,
                Height = 40,
                Left = 55,
                Top = 180
            };
            btnServiceStatus.Click += (s, e) => { new ServiceRequestStatusForm().ShowDialog(); };


            Controls.Add(btnReportIssues);
            Controls.Add(btnLocalEvents);
            Controls.Add(btnServiceStatus);
        }
    }
}
