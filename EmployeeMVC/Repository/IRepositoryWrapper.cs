using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeMVC.Repository
{
    public interface IRepositoryWrapper
    {

        IHolidayRepository Holiday { get; }
        IEventlogRepository Eventlog { get; }

        IEmployeeRepository Employee { get; }

        IUserRepository User { get; }

        ILeaveRepository Leave { get; }

        IUserLevelRepository UserLevel { get; }
//////Template Place Holder/////
    }
}
