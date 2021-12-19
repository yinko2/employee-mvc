using System.ComponentModel.DataAnnotations;

namespace EmployeeMVC.Models.DTOs
{
    public partial class CreateEmployeeDTO
    {
        [Required, StringLength(100)]
        public string Name { get; set; } = null!;

        [DataType(DataType.Date), Display(Name = "Date of Birth")]
        public DateTime DateOfBirth { get; set; }

        [StringLength(200)]
        public string? Address { get; set; }

        [StringLength(20)]
        public string Phone { get; set; } = null!;

        [StringLength(50), DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;
    }

    public partial class EmployeeDTO
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; } = null!;

        [DataType(DataType.Date), Display(Name = "Date of Birth")]
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
