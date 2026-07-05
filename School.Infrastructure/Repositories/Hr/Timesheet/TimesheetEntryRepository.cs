using School.Infrastructure.Repositories.IRepositories.Hr.Timesheet;
using School.Domain.Hr.Timesheet;
using School.Infrastructure.UnitOfWork;

namespace School.Infrastructure.Repositories.Hr.Timesheet
{
    public class TimesheetEntryRepository : Repository<TimesheetEntry>, ITimesheetEntryRepository
    {
        public TimesheetEntryRepository(DbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}

