using System;
using System.Collections.Generic;
using EmployeeMVC.Data;
using EmployeeMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeMVC.Repository
{
    public class HolidayRepository: RepositoryBase<Holiday>, IHolidayRepository
    {
        public HolidayRepository(ApiDbContext repositoryContext)
            :base(repositoryContext)
        {
        }

        public async Task<int> FindExistingDate(DateTime date)
        {
            var query = await (from h in RepositoryContext.Holidays
                         where h.Date == date
                         select h.Id).FirstOrDefaultAsync();
            return query;
        }

    }
}