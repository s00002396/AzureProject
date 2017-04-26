using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureTestApp.Model
{
    public class PatientDetailsViewModel
    {
        //public Task_Table vmTaskTable { get; set; }
        public SchoolLists vmSchools { get; set; }
        public PatientAccount vmPatientTable { get; set; }
        public Guardians vmGuardian { get; set; }
        public UserAccount vmUserAcc { get; set; }
        public Notes vmNoteTable { get; set; }
        public OtTasks vmTPOT { get; set; }
        public Tasks vmTaskTable { get; set; }
    }
}
