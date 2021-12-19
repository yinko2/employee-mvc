using System;
using System.Collections.Generic;
using EmployeeMVC.Data;
using EmployeeMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeMVC.Repository
{
    public class UserRepository: RepositoryBase<User>, IUserRepository
    {
        public UserRepository(ApiDbContext repositoryContext)
            :base(repositoryContext)
        {
        }

    }
}