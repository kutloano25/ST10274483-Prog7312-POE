namespace MunicipalApp
{
    public partial class ServiceRequestStatusForm : Form
    {
        private List<ServiceRequest> requests = new List<ServiceRequest>();
        private BinarySearchTree<int> bst = new BinarySearchTree<int>();
        private MinHeap<int> priorityHeap = new MinHeap<int>();
        private Graph dependencies = new Graph();

        public ServiceRequestStatusForm()
        {
            InitializeComponent();
            LoadRequests();
            RefreshList();
        }

        private void LoadRequests()
        {
            // Example requests
            requests.Add(new ServiceRequest { RequestID = 101, Title = "Fix Streetlight", Status = "Pending", Priority = 1 });
            requests.Add(new ServiceRequest { RequestID = 102, Title = "Repair Water Leak", Status = "In Progress", Priority = 2 });
            requests.Add(new ServiceRequest { RequestID = 103, Title = "Replace Traffic Sign", Status = "Completed", Priority = 3 });

            // Populate structures
            foreach (var r in requests)
            {
                bst.Insert(r.RequestID);
                priorityHeap.Add(r.Priority);
            }
           
            dependencies.AddEdge(101, 102);
        }

        private void RefreshList()
        {
            lvRequests.Items.Clear();
            foreach (var req in requests.OrderBy(r => r.Priority))
            {
                var item = new ListViewItem(req.RequestID.ToString());
                item.SubItems.Add(req.Title);
                item.SubItems.Add(req.Status);
                item.SubItems.Add(req.Priority.ToString());
                lvRequests.Items.Add(item);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtSearchID.Text, out int id))
            {
                bool exists = bst.Search(id);
                MessageBox.Show(exists ? $"Request {id} found." : "Not found.", "Search Result");
            }
            else
                MessageBox.Show("Enter a valid numeric ID.");
        }

        private void btnShowDependencies_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtSearchID.Text, out int id))
            {
                var deps = dependencies.GetDependencies(id);
                if (deps.Count == 0)
                    MessageBox.Show("No dependencies.");
                else
                    MessageBox.Show($"Request {id} depends on: {string.Join(", ", deps)}");
            }
        }
    }
}
