using System;
using System.Collections.Generic;
using System.Linq;
//using System.Threading.Tasks;

namespace AzureTestApp.Model
{
    public class MyViewModel
    {
            public Tasks vmTaskTable { get; set; }
            public OtTasks   vmTPOT { get; set; }
            public PatientAccount vmPatientTable { get; set; }
            public UserAccount vmUserAcc { get; set; }        
    }
}
