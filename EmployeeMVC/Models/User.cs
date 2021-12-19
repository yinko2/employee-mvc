using System;
using System.Collections.Generic;

namespace EmployeeMVC.Models
{
    public partial class User
    {
        public int Id { get; set; }
        public int UserLevelId { get; set; }
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string PasswordSalt { get; set; } = null!;
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }
}
