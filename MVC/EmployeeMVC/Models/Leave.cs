using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeMVC.Models
{
    [Table("tbl_leave")]
    public partial class Leave
    {
        [Column("ID")]
        public int Id { get; set; }

        [Column("EmployeeID")]
        public int EmployeeId { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required, StringLength(100)]
        public string Reason { get; set; } = null!;

        [DataType(DataType.DateTime), Display(Name = "Created Time")]
        public DateTime? CreatedTime { get; set; }

        [DataType(DataType.DateTime), Display(Name = "Modified Time")]
        public DateTime? ModifiedTime { get; set; }
    }
}
