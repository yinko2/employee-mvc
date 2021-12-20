using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeMVC.Models
{
    [Table("tbl_holiday")]
    public partial class Holiday
    {
        [Column("ID")]
        public int Id { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; } = null!;
    }
}
