using School.Infrastructure.Repositories.IRepositories.Hr.Attendance;
using School.Domain.Hr.Attendance;
using School.Infrastructure.UnitOfWork;

namespace School.Infrastructure.Repositories.Hr.Attendance
{
    public class AttendanceLogRepository : Repository<AttendanceLog>, IAttendanceLogRepository
    {
        public AttendanceLogRepository(DbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}

