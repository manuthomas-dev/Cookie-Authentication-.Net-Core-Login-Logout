using System;
using System.Collections.Generic;

#nullable disable

namespace ExpenseManagerApp.Models
{
    public partial class UserLogin
    {
        public int UserLoginId { get; set; }
        public int? UserId { get; set; }
        public string Username { get; set; }
        public string UserPassword { get; set; }

        public virtual User User { get; set; }
    }
}
