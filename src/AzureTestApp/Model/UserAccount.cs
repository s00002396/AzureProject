using System;
using System.Collections.Generic;

namespace AzureTestApp.Model
{
    public partial class UserAccount
    {
        public int UserId { get; set; }
        public string CofirmPassword { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public bool? IsAdmin { get; set; }
    }
}
