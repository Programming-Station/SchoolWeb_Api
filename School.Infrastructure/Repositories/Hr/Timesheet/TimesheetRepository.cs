using School.Infrastructure.UnitOfWork;

namespace School.Infrastructure.Repositories.Hr.Timesheet
{
    public class TimesheetRepository : Repository<global::School.Domain.Hr.Timesheet.Timesheet>, ITimesheetRepository
    {
        public TimesheetRepository(DbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
