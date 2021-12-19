using System;
using System.Collections.Generic;
using EmployeeMVC.Models;
using EmployeeMVC.Models.DTOs;

namespace EmployeeMVC.Repository
{
    public interface ILeaveRepository: IRepositoryBase<Leave>
    {
        Task<IEnumerable<LeaveDTO>> LoadLeaveList();
        Task<LeaveDTO> LoadLeaveDetails(int id);
    }
}