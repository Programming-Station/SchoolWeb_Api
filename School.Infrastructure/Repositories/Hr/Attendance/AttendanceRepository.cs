using School.Domain.Hr.Attendance;
using School.Infrastructure.UnitOfWork;

namespace School.Infrastructure.Repositories.Hr.Attendance
{
    public class AttendanceRepository : Repository<global::School.Domain.Hr.Attendance.Attendance>, IAttendanceRepository
    {
        public AttendanceRepository(DbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
