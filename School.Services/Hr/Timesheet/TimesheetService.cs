using School.Infrastructure.Repositories.Hr.Timesheet;

namespace School.Services.Hr.Timesheet
{
    public class TimesheetService : ITimesheetService
    {
        private readonly ITimesheetRepository _repository;
        public TimesheetService(ITimesheetRepository repository)
        {
            _repository = repository;
        }
    }
}
