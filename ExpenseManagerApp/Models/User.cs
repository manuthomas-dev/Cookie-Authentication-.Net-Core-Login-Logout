using System;
using System.Collections.Generic;

#nullable disable

namespace ExpenseManagerApp.Models
{
    public partial class User
    {
        public User()
        {
            UserLogins = new HashSet<UserLogin>();
        }

        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Photo { get; set; }
        public DateTime Dob { get; set; }
        public bool IsActive { get; set; }

        public virtual ICollection<UserLogin> UserLogins { get; set; }
    }
}
