using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeMVC.Models.DTOs
{
    public partial class CreateLeaveDTO
    {
        public int EmployeeId { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required, StringLength(100)]
        public string Reason { get; set; } = null!;
    }

    public partial class LeaveDTO
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required, StringLength(100)]
        public string Reason { get; set; } = null!;

        [DataType(DataType.DateTime), Display(Name = "Created Time")]
        public DateTime? CreatedTime { get; set; }

        [DataType(DataType.DateTime), Display(Name = "Modified Time")]
        public DateTime? ModifiedTime { get; set; }

        [Display(Name = "Employee Name")]
        public string? EmployeeName { get; set; }
    }
}
