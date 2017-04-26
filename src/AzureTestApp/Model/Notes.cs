using System;
using System.Collections.Generic;

namespace AzureTestApp.Model
{
    public partial class Notes
    {
        public int NoteId { get; set; }
        public string Details { get; set; }
        public string NoteTitle { get; set; }
        public DateTime? NoteDate { get; set; }
        public int? PpsNo { get; set; }
    }
}
