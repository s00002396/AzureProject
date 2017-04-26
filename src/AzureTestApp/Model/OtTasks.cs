using System;
using System.Collections.Generic;

namespace AzureTestApp.Model
{
    public partial class OtTasks
    {
        public int OtTaskId { get; set; }
        public int? TaskId { get; set; }
        public int? PpsNo { get; set; }
        public int? OccId { get; set; }
        public bool? Completed { get; set; }
        public DateTime? DueDate { get; set; }
    }
}
