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

        ILeaveRepository Leave { get; }

//////Template Place Holder/////
    }
}
