using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AzureTestApp.Model
{
    public partial class Guardians
    {
        public int GuardianId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int PhoneNo { get; set; }
    }
}
