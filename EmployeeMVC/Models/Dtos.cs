using System;
using System.ComponentModel.DataAnnotations;

namespace EmployeeMVC.Models
{
    //public record CreateEmployeeDTO(
    //    [Required, StringLength(100)]
    //    string Name,

    //    [DataType(DataType.Date), Display(Name = "Date of Birth")]
    //    DateTime DateOfBirth,

    //    [StringLength(200)]
    //    string Address,

    //    [StringLength(20)]
    //    string Phone,

    //    [StringLength(50), DataType(DataType.EmailAddress)]
    //    string Email
    //);

    //public record EmployeeDTO(
    //    int Id,

    //    [Required, StringLength(100)]
    //    string Name,

    //    [DataType(DataType.Date), Display(Name = "Date of Birth")]
    //    DateTime DateOfBirth,

    //    [StringLength(200)]
    //    string Address,

    //    [StringLength(20)]
    //    string Phone,

    //    [StringLength(50), DataType(DataType.EmailAddress)]
    //    string Email,

    //    [DataType(DataType.DateTime), Display(Name = "Created Time")]
    //    DateTime? CreatedTime,

    //    [DataType(DataType.DateTime), Display(Name = "Modified Time")]
    //    DateTime? ModifiedTime
    //);

    public record UserRegisterDTO(
        [Required]
        int UserLevelId,

        [Required]
        [MaxLength(100)]
        string UserName,

        [Required]
        [MaxLength(255)]
        string Password
    );

    public record UserDTO(
        int Id,
        int UserLevelId,
        string UserName,
        DateTime? CreatedTime,
        DateTime? ModifiedTime
    );

    public record UserLoginDTO(
        [Required]
        string UserName,
        [Required]
        string Password
    );
}