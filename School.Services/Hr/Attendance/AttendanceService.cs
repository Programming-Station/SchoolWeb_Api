using School.Infrastructure.Repositories.Hr.Attendance;

namespace School.Services.Hr.Attendance
{
    public class AttendanceService : IAttendanceService
    {
        private readonly IAttendanceRepository _repository;
        public AttendanceService(IAttendanceRepository repository)
        {
            _repository = repository;
        }
    }
}
