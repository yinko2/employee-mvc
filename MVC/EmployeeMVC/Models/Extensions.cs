using EmployeeMVC.Models.DTOs;

namespace EmployeeMVC.Models
{
    public static class Extensions
    {

        public static EmployeeDTO AsDTO(this Employee item)
        {
            return new EmployeeDTO
            {
                Id = item.Id,
                Name = item.Name,
                DateOfBirth = item.DateOfBirth,
                Address = item.Address!,
                Email = item.Email,
                Phone = item.Phone,
                CreatedTime = item.CreatedTime,
                ModifiedTime =  item.ModifiedTime
            };
        }

        public static LeaveDTO AsDTO(this Leave item)
        {
            return new LeaveDTO
            {
                Id = item.Id,
                EmployeeId = item.EmployeeId,
                Reason = item.Reason,
                Date = item.Date,
                CreatedTime = item.CreatedTime,
                ModifiedTime = item.ModifiedTime
            };
        }
    }
}