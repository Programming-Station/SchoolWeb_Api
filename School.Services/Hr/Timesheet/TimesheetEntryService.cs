using School.Infrastructure.Repositories.Hr.Timesheet;
using School.Infrastructure.Repositories.IRepositories.Hr.Timesheet;

namespace School.Services.Hr.Timesheet
{
    public class TimesheetEntryService : ITimesheetEntryService
    {
        private readonly ITimesheetEntryRepository _repository;
        public TimesheetEntryService(ITimesheetEntryRepository repository)
        {
            _repository = repository;
        }
    }
}

