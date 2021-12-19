using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeMVC.Models
{
    [Table("tbl_employee")]
    public partial class Employee
    {
        [Column("ID")]
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; } = null!;

        [DataType(DataType.Date), Display(Name ="Date of Birth")]
        public DateTime DateOfBirth { get; set; }

        [StringLength(200)]
        public string? Address { get; set; }

        [StringLength(20)]
        public string Phone { get; set; } = null!;

        [StringLength(50), DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;

        [DataType(DataType.DateTime), Display(Name = "Created Time")]
        public DateTime? CreatedTime { get; set; }

        [DataType(DataType.DateTime), Display(Name = "Modified Time")]
        public DateTime? ModifiedTime { get; set; }
    }
}
