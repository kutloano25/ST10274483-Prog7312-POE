using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MunicipalApp
{
    public class EventsForm : Form
    {
        private ListView lvEvents;
        private TextBox txtSearchCategory;
        private DateTimePicker dtpSearchDate;
        private Button btnSearch;
        private Button btnRecommend;
        private Button btnBack;
        private Label lblInfo;
        private List<EventItem> masterEvents = new List<EventItem>();

        // Data structures
        private SortedDictionary<DateTime, List<EventItem>> eventsByDate = new SortedDictionary<DateTime, List<EventItem>>();
        private Dictionary<string, int> searchCounts = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        private HashSet<string> categorySet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        private Stack<EventItem> recentlyViewed = new Stack<EventItem>();
        private Queue<string> notifications = new Queue<string>();
        private PriorityQueue<EventItem> upcomingEvents;

        public EventsForm()
        {
            Text = "Local Events & Announcements";
            Width = 900;
            Height = 640;
            StartPosition = FormStartPosition.CenterParent;
            upcomingEvents = new PriorityQueue<EventItem>(new EventDateComparer());
            InitializeComponents();
            LoadSampleEvents();
            PopulateUIFromData();
        }

        private void InitializeComponents()
        {
            var lblTitle = new Label() { Text = "Local Events & Announcements", Font = new Font("Segoe UI", 12, FontStyle.Bold), Left = 10, Top = 10, Width = 400 };
            Controls.Add(lblTitle);

            lvEvents = new ListView() { Left = 10, Top = 70, Width = 860, Height = 400, View = View.Details, FullRowSelect = true };
            lvEvents.Columns.Add("Date", 120);
            lvEvents.Columns.Add("Category", 120);
            lvEvents.Columns.Add("Title", 280);
            lvEvents.Columns.Add("Location", 160);
            lvEvents.Columns.Add("Description", 160);
            lvEvents.DoubleClick += LvEvents_DoubleClick;
            Controls.Add(lvEvents);

            var lblCat = new Label() { Text = "Category:", Left = 10, Top = 480 };
            txtSearchCategory = new TextBox() { Left = 80, Top = 476, Width = 180 };

            var lblDate = new Label() { Text = "Date:", Left = 280, Top = 480 };
            dtpSearchDate = new DateTimePicker() { Left = 320, Top = 476, Width = 150, Format = DateTimePickerFormat.Short };

            btnSearch = new Button() { Text = "Search", Left = 490, Top = 474, Width = 100 };
            btnSearch.Click += BtnSearch_Click;

            btnRecommend = new Button() { Text = "Show Recommendations", Left = 600, Top = 474, Width = 150 };
            btnRecommend.Click += BtnRecommend_Click;

            btnBack = new Button() { Text = "Back", Left = 760, Top = 474, Width = 110 };
            btnBack.Click += (s, e) => Close();

            lblInfo = new Label() { Text = "Tip: Double-click an event to view details & mark as viewed.", Left = 10, Top = 510, Width = 860, Height = 40 };

            Controls.AddRange(new Control[] { lblCat, txtSearchCategory, lblDate, dtpSearchDate, btnSearch, btnRecommend, btnBack, lblInfo });
        }

        private void LoadSampleEvents()
        {
            // Seed some sample events (for demonstration and testing)
            var now = DateTime.Today;
            masterEvents.Add(new EventItem() { Title = "Community Clean-up", Category = "Parks", Description = "Join volunteers to tidy up the park.", StartDate = now.AddDays(3), Location = "Green Park" });
            masterEvents.Add(new EventItem() { Title = "Road Safety Workshop", Category = "Roads", Description = "Workshop for road safety awareness.", StartDate = now.AddDays(7), Location = "Community Hall" });
            masterEvents.Add(new EventItem() { Title = "Water Conservation Talk", Category = "Utilities", Description = "Learn about saving water.", StartDate = now.AddDays(5), Location = "Library Auditorium" });
            masterEvents.Add(new EventItem() { Title = "Sanitation Awareness Day", Category = "Sanitation", Description = "Educate about sanitation.", StartDate = now.AddDays(10), Location = "Town Square" });
            masterEvents.Add(new EventItem() { Title = "Local Market", Category = "Other", Description = "Local vendors with crafts and food.", StartDate = now.AddDays(2), Location = "Market Street" });

            // populate data structures
            foreach (var ev in masterEvents)
            {
                var dateKey = ev.StartDate.Date;
                if (!eventsByDate.ContainsKey(dateKey)) eventsByDate[dateKey] = new List<EventItem>();
                eventsByDate[dateKey].Add(ev);
                categorySet.Add(ev.Category);
                upcomingEvents.Enqueue(ev);
            }

            // sample notifications
            notifications.Enqueue("New event added: Community Clean-up");
            notifications.Enqueue("Reminder: Water Conservation Talk in 5 days");
        }

        private void PopulateUIFromData(IEnumerable<EventItem> events = null)
        {
            lvEvents.Items.Clear();
            var list = events ?? masterEvents.OrderBy(e => e.StartDate);
            foreach (var ev in list)
            {
                var lvi = new ListViewItem(ev.StartDate.ToShortDateString());
                lvi.SubItems.Add(ev.Category);
                lvi.SubItems.Add(ev.Title);
                lvi.SubItems.Add(ev.Location);
                var desc = ev.Description.Length > 80 ? ev.Description.Substring(0, 80) + "..." : ev.Description;
                lvi.SubItems.Add(desc);
                lvi.Tag = ev;
                lvEvents.Items.Add(lvi);
            }
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            string cat = txtSearchCategory.Text.Trim();
            DateTime date = dtpSearchDate.Value.Date;
            IEnumerable<EventItem> results = masterEvents;

            if (!string.IsNullOrEmpty(cat))
            {
                results = results.Where(ev => ev.Category.Equals(cat, StringComparison.OrdinalIgnoreCase));
                // record the search for recommendation purposes
                if (!searchCounts.ContainsKey(cat)) searchCounts[cat] = 0;
                searchCounts[cat]++;
            }

            // filter by date if user selected a date (we'll allow search even if date is today)
            if (date != default(DateTime))
                results = results.Where(ev => ev.StartDate.Date == date);

            PopulateUIFromData(results);

            // Add a short notification and enqueue
            notifications.Enqueue($"Search performed for category '{cat}' on {date:yyyy-MM-dd}");
            if (notifications.Count > 10) notifications.Dequeue();

            MessageBox.Show($"Found {lvEvents.Items.Count} event(s).", "Search Results", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnRecommend_Click(object sender, EventArgs e)
        {
            // Recommendation strategy: top searched categories + upcoming events from priority queue
            List<EventItem> recommended = new List<EventItem>();

            // 1. Top searched categories
            var topCategories = searchCounts.OrderByDescending(kv => kv.Value).Select(kv => kv.Key).Take(3).ToList();
            foreach (var cat in topCategories)
            {
                recommended.AddRange(masterEvents.Where(ev => ev.Category.Equals(cat, StringComparison.OrdinalIgnoreCase)).Take(3));
            }

            // 2. Fill with soonest upcoming events from priority queue (peek through a copy)
            var pqCopy = new PriorityQueue<EventItem>(new EventDateComparer());
            foreach (var ev in masterEvents) pqCopy.Enqueue(ev);
            int take = 5;
            while (pqCopy.Count > 0 && take-- > 0)
            {
                recommended.Add(pqCopy.Dequeue());
            }

            // Remove duplicates and show top 10
            var unique = recommended.DistinctBy(e => e.Id).Take(10).ToList();
            if (unique.Count == 0)
            {
                MessageBox.Show("No recommendations available yet. Try searching for categories to improve recommendations.", "Recommendations", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            PopulateUIFromData(unique);
            MessageBox.Show($"Showing {unique.Count} recommended event(s).", "Recommendations", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void LvEvents_DoubleClick(object sender, EventArgs e)
        {
            if (lvEvents.SelectedItems.Count == 0) return;
            var lvi = lvEvents.SelectedItems[0];
            var ev = lvi.Tag as EventItem;
            if (ev == null) return;

            // push to recently viewed stack
            recentlyViewed.Push(ev);

            // show details
            var details = $"Title: {ev.Title}\nCategory: {ev.Category}\nDate: {ev.StartDate:yyyy-MM-dd}\nLocation: {ev.Location}\n\n{ev.Description}";
            MessageBox.Show(details, "Event Details", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // add a small notification if event is soon
            if ((ev.StartDate - DateTime.Today).TotalDays <= 7)
            {
                notifications.Enqueue($"Event '{ev.Title}' is happening on {ev.StartDate:yyyy-MM-dd} — don't miss it!");
                if (notifications.Count > 10) notifications.Dequeue();
            }
        }
    }

    // small helper extension to DistinctBy (since .NET Framework may not have it)
    static class LinqExtensions
    {
        public static IEnumerable<T> DistinctBy<T, TKey>(this IEnumerable<T> source, Func<T, TKey> keySelector)
        {
            var set = new HashSet<TKey>();
            foreach (var item in source)
            {
                if (set.Add(keySelector(item))) yield return item;
            }
        }
    }
}
