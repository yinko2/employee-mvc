using System;
using System.Collections.Generic;
using EmployeeMVC.Data;
using EmployeeMVC.Models;
using EmployeeMVC.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace EmployeeMVC.Repository
{
    public class LeaveRepository: RepositoryBase<Leave>, ILeaveRepository
    {
        public LeaveRepository(ApiDbContext repositoryContext)
            :base(repositoryContext)
        {
        }

        public async Task<IEnumerable<LeaveDTO>> LoadLeaveList(DateTime start, DateTime end)
        {
            IEnumerable<LeaveDTO> query;
            if (start != DateTime.MinValue && end != DateTime.MinValue)
            {
                query = await (from l in RepositoryContext.Leaves.Where(x => start <= x.Date && x.Date <= end)
                                   join e in RepositoryContext.Employees on l.EmployeeId equals e.Id
                                   select new LeaveDTO
                                   {
                                       Id = l.Id,
                                       Date = l.Date,
                                       EmployeeId = l.EmployeeId,
                                       EmployeeName = e.Name,
                                       Reason = l.Reason,
                                       CreatedTime = l.CreatedTime,
                                       ModifiedTime = l.ModifiedTime
                                   }).ToListAsync();
            }
            else
            {
                query = await (from l in RepositoryContext.Leaves
                                   join e in RepositoryContext.Employees on l.EmployeeId equals e.Id
                                   select new LeaveDTO
                                   {
                                       Id = l.Id,
                                       Date = l.Date,
                                       EmployeeId = l.EmployeeId,
                                       EmployeeName = e.Name,
                                       Reason = l.Reason,
                                       CreatedTime = l.CreatedTime,
                                       ModifiedTime = l.ModifiedTime
                                   }).ToListAsync();
            }
            return query.OrderByDescending(x => x.Date);
        }

        public async Task<LeaveDTO> LoadLeaveDetails(int id)
        {
            var query = await (from l in RepositoryContext.Leaves
                               join e in RepositoryContext.Employees on l.EmployeeId equals e.Id
                               where l.Id == id
                               select new LeaveDTO
                               {
                                   Id = l.Id,
                                   Date = l.Date,
                                   EmployeeId = l.EmployeeId,
                                   EmployeeName = e.Name,
                                   Reason = l.Reason,
                                   CreatedTime = l.CreatedTime,
                                   ModifiedTime = l.ModifiedTime
                               }).FirstOrDefaultAsync();
            return query!;
        }

    }
}