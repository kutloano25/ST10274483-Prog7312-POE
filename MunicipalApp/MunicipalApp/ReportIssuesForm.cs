using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace MunicipalApp
{
    public class ReportIssuesForm : Form
    {
        private TextBox txtLocation;
        private ComboBox cboCategory;
        private RichTextBox rtbDescription;
        private Button btnAttach;
        private Button btnSubmit;
        private Button btnBack;
        private Label lblEngagement;
        private ProgressBar progressBar;
        private ListView lvIssues;

        // In-memory storage
        private static List<Issue> Issues = new List<Issue>();

        public ReportIssuesForm()
        {
            Text = "Report Issues";
            Width = 800;
            Height = 600;
            StartPosition = FormStartPosition.CenterParent;
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            var lblLocation = new Label() { Text = "Location:", Left = 20, Top = 20, Width = 70 };
            txtLocation = new TextBox() { Left = 100, Top = 18, Width = 300 };

            var lblCategory = new Label() { Text = "Category:", Left = 420, Top = 20, Width = 70 };
            cboCategory = new ComboBox() { Left = 490, Top = 18, Width = 250, DropDownStyle = ComboBoxStyle.DropDownList };
            cboCategory.Items.AddRange(new string[] { "Sanitation", "Roads", "Utilities", "Parks", "Other" });
            cboCategory.SelectedIndex = 0;

            var lblDescription = new Label() { Text = "Description:", Left = 20, Top = 60, Width = 80 };
            rtbDescription = new RichTextBox() { Left = 20, Top = 80, Width = 720, Height = 150 };

            btnAttach = new Button() { Text = "Attach Files...", Left = 20, Top = 240, Width = 120 };
            btnAttach.Click += BtnAttach_Click;

            btnSubmit = new Button() { Text = "Submit Report", Left = 600, Top = 240, Width = 140 };
            btnSubmit.Click += BtnSubmit_Click;

            btnBack = new Button() { Text = "Back to Main Menu", Left = 440, Top = 240, Width = 140 };
            btnBack.Click += (s, e) => Close();

            lblEngagement = new Label()
            {
                Text = "Thanks for taking part — your reports help improve the community!",
                Left = 20,
                Top = 280,
                Width = 720,
                Height = 30
            };

            progressBar = new ProgressBar() { Left = 20, Top = 320, Width = 720, Height = 20, Value = 0 };

            // List of existing issues for feedback
            lvIssues = new ListView()
            {
                Left = 20,
                Top = 350,
                Width = 720,
                Height = 180,
                View = View.Details,
                FullRowSelect = true
            };
            lvIssues.Columns.Add("Submitted", 150);
            lvIssues.Columns.Add("Category", 120);
            lvIssues.Columns.Add("Location", 200);
            lvIssues.Columns.Add("Attachments", 120);
            lvIssues.Columns.Add("Description", 130);

            Controls.AddRange(new Control[] { lblLocation, txtLocation, lblCategory, cboCategory, lblDescription, rtbDescription, btnAttach, btnSubmit, btnBack, lblEngagement, progressBar, lvIssues });

            RefreshIssueList();
        }

        private void BtnAttach_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Multiselect = true;
                dlg.Filter = "Images and Documents|*.png;*.jpg;*.jpeg;*.bmp;*.gif;*.pdf;*.doc;*.docx;*.txt|All files|*.*";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    // We'll store selected file paths temporarily in Tag of Attach button
                    List<string> current = btnAttach.Tag as List<string> ?? new List<string>();
                    current.AddRange(dlg.FileNames);
                    btnAttach.Tag = current;
                    MessageBox.Show($"{dlg.FileNames.Length} file(s) attached.", "Attachment", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void BtnSubmit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtLocation.Text))
            {
                MessageBox.Show("Please enter a location.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(rtbDescription.Text))
            {
                MessageBox.Show("Please enter a description.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var issue = new Issue()
            {
                Location = txtLocation.Text.Trim(),
                Category = cboCategory.SelectedItem?.ToString() ?? "Other",
                Description = rtbDescription.Text.Trim(),
            };

            if (btnAttach.Tag is List<string> attachments)
            {
                // Basic validation: ensure files still exist
                foreach (var path in attachments)
                    if (File.Exists(path)) issue.Attachments.Add(path);
                btnAttach.Tag = null; // clear attachments after submit
            }

            Issues.Add(issue);
            // Update engagement (progress bar increments each submission up to 100)
            progressBar.Value = Math.Min(100, progressBar.Value + 10);
            lblEngagement.Text = $"Thanks! You've submitted {Issues.Count} report(s). Community engagement +{10}%";

            MessageBox.Show("Report submitted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Clear form
            txtLocation.Clear();
            rtbDescription.Clear();
            cboCategory.SelectedIndex = 0;

            RefreshIssueList();
        }

        private void RefreshIssueList()
        {
            lvIssues.Items.Clear();
            foreach (var issue in Issues)
            {
                var lvi = new ListViewItem(issue.SubmittedAt.ToString("g"));
                lvi.SubItems.Add(issue.Category);
                lvi.SubItems.Add(issue.Location);
                lvi.SubItems.Add(issue.Attachments.Count.ToString());
                var desc = issue.Description.Length > 100 ? issue.Description.Substring(0, 100) + "..." : issue.Description;
                lvi.SubItems.Add(desc);
                lvIssues.Items.Add(lvi);
            }
        }
    }
}