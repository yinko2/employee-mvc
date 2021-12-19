using System;
using System.Collections.Generic;

namespace EmployeeMVC.Models
{
    public partial class Userlevel
    {
        public int Id { get; set; }
        public string UserLevel { get; set; } = null!;
        public string? Description { get; set; }
    }
}
