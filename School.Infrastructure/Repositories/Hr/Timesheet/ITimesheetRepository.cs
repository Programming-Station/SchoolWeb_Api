using School.Infrastructure.UnitOfWork.Interfaces;

namespace School.Infrastructure.Repositories.Hr.Timesheet
{
    public interface ITimesheetRepository : IRepository<Domain.Hr.Timesheet.Timesheet>
    {
    }
}
