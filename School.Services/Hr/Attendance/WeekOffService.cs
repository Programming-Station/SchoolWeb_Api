using School.Infrastructure.Repositories.Hr.Attendance;

namespace School.Services.Hr.Attendance
{
    public class WeekOffService : IWeekOffService
    {
        private readonly IWeekOffRepository _repository;
        public WeekOffService(IWeekOffRepository repository)
        {
            _repository = repository;
        }
    }
}
