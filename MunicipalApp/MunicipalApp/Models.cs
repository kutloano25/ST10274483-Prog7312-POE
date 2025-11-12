using System;
using System.Collections.Generic;

namespace MunicipalApp
{
    public class Issue
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Location { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public List<string> Attachments { get; set; } = new List<string>();
        public DateTime SubmittedAt { get; set; } = DateTime.Now;
        public override string ToString() => $"{SubmittedAt:g} | {Category} | {Location}";
    }

    public class EventItem
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public string Location { get; set; }
        public override string ToString() => $"{StartDate:yyyy-MM-dd} | {Category} | {Title}";
    }
}
