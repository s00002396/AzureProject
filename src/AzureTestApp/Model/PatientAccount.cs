using System;
using System.Collections.Generic;

namespace AzureTestApp.Model
{
    public partial class PatientAccount
    {
        public int PpsNo { get; set; }
        public string SocialSecurityNo { get; set; }
        public string DoB { get; set; }
        public string AddressLineOne { get; set; }
        public string City { get; set; }
        public string County { get; set; }
        public string EirCode { get; set; }
        public int GuardianId { get; set; }
        public string Name { get; set; }
        public int OccId { get; set; }
        public int SchoolId { get; set; }
        public DateTime? CloseDate { get; set; }
        public DateTime OpenDate { get; set; }
    }
}
