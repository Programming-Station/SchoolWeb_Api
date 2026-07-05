using School.Domain.Hr.Timesheet;
using School.Infrastructure.UnitOfWork.Interfaces;

namespace School.Infrastructure.Repositories.Hr.Timesheet
{
    public interface ITimesheetEntryRepository : IRepository<TimesheetEntry>
    {
    }
}
