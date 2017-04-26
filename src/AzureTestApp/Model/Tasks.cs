using System;
using System.Collections.Generic;

namespace AzureTestApp.Model
{
    public partial class Tasks
    {
        public int TaskId { get; set; }
        public bool Completed { get; set; }
        public DateTime DueDate { get; set; }
        public string TaskType { get; set; }
    }
}
