using System;
using System.Collections.Generic;
using EmployeeMVC.Data;
using EmployeeMVC.Models;

namespace EmployeeMVC.Repository
{
    public class UserLevelRepository: RepositoryBase<Userlevel>, IUserLevelRepository
    {
        public UserLevelRepository(ApiDbContext repositoryContext)
            :base(repositoryContext)
        {
        }

    }
}