using System;
using System.Collections.Generic;

namespace AzureTestApp.Model
{
    public partial class NotePatient
    {
        public int NoteId { get; set; }
        public int PpsNo { get; set; }
        public DateTime NoteDate { get; set; }
    }
}
