using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AzureTestApp.Model
{
    public partial class PatientAccount
    {
        public int PpsNo { get; set; }
        [Required]
        public string SocialSecurityNo { get; set; }
        [Required]
        public string DoB { get; set; }
        [Required]
        public string AddressLineOne { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string County { get; set; }
        public string EirCode { get; set; }
        public int GuardianId { get; set; }
        [Required]
        public string Name { get; set; }
        public int OccId { get; set; }
        public int SchoolId { get; set; }
        public DateTime? CloseDate { get; set; }
        public DateTime OpenDate { get; set; }
    }
}
