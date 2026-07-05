using School.Infrastructure.Repositories.Hr.Attendance;
using School.Infrastructure.Repositories.IRepositories.Hr.Attendance;

namespace School.Services.Hr.Attendance
{
    public class AttendanceLogService : IAttendanceLogService
    {
        private readonly IAttendanceLogRepository _repository;
        public AttendanceLogService(IAttendanceLogRepository repository)
        {
            _repository = repository;
        }
    }
}

