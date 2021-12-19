using System;
using System.Collections.Generic;
using EmployeeMVC.Data;
using EmployeeMVC.Models;

namespace EmployeeMVC.Repository
{
    public class EmployeeRepository: RepositoryBase<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(ApiDbContext repositoryContext)
            :base(repositoryContext)
        {
        }

    }
}