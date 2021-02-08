using System;
using System.Collections.Generic;

#nullable disable

namespace ApiNet5.NorthwndDataAccess
{
    public partial class User
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Salt { get; set; }
        public bool? Status { get; set; }
    }
}
