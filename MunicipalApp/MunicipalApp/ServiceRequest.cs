using System;
using System.Collections.Generic;


namespace MunicipalApp
{
    public class ServiceRequest
    {
        public int RequestID { get; set; }
        public string Title { get; set; }
        public string Status { get; set; } // "Pending", "In Progress", "Completed"
        public int Priority { get; set; }  // Lower number = higher priority
        public List<int> Dependencies { get; set; } = new List<int>(); // Related requests

        public override string ToString()
        {
            return $"{RequestID} - {Title} [{Status}] (Priority: {Priority})";
        }
    }
}
