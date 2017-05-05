using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AzureTestApp.Model
{
    public partial class UserAccount
    {
        public int UserId { get; set; }
        [Required]
        public string CofirmPassword { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string UserName { get; set; }
        public bool? IsAdmin { get; set; }
    }
}
