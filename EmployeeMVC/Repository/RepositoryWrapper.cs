using EmployeeMVC;
using EmployeeMVC.Data;
using EmployeeMVC.Models;
namespace EmployeeMVC.Repository
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private readonly ApiDbContext _repoContext;

        private IHolidayRepository? oHoliday;
        public IHolidayRepository Holiday
        {
            get
            {
                if (oHoliday == null)
                {
                    oHoliday = new HolidayRepository(_repoContext);
                }

                return oHoliday;
            }
        }

        private IEventlogRepository? oEventlog;
        public IEventlogRepository Eventlog
        {
            get
            {
                if (oEventlog == null)
                {
                    oEventlog = new EventlogRepository(_repoContext);
                }

                return oEventlog;
            }
        }

        private IEmployeeRepository? oEmployee;
        public IEmployeeRepository Employee
        {
            get
            {
                if (oEmployee == null)
                {
                    oEmployee = new EmployeeRepository(_repoContext);
                }

                return oEmployee;
            }
        }

        private IUserRepository? oUser;
        public IUserRepository User
        {
            get
            {
                if (oUser == null)
                {
                    oUser = new UserRepository(_repoContext);
                }

                return oUser;
            }
        }

        private ILeaveRepository? oLeave;
        public ILeaveRepository Leave
        {
            get
            {
                if (oLeave == null)
                {
                    oLeave = new LeaveRepository(_repoContext);
                }

                return oLeave;
            }
        }

        private IUserLevelRepository? oUserLevel;
        public IUserLevelRepository UserLevel
        {
            get
            {
                if (oUserLevel == null)
                {
                    oUserLevel = new UserLevelRepository(_repoContext);
                }

                return oUserLevel;
            }
        }
//////Template Place Holder/////
        public RepositoryWrapper(ApiDbContext repositoryContext)
        {
            _repoContext = repositoryContext;
        }
    }
}
