using System;
using System.Collections.Generic;
using EmployeeMVC.Models;

namespace EmployeeMVC.Repository
{
    public interface IHolidayRepository: IRepositoryBase<Holiday>
    {
        Task<int> FindExistingDate(DateTime date);
        Task<IEnumerable<Holiday>> LoadHolidaysWithInterval(DateTime start, DateTime end);
    }
}