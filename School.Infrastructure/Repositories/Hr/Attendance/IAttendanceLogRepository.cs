using School.Domain.Hr.Attendance;
using School.Infrastructure.UnitOfWork.Interfaces;

namespace School.Infrastructure.Repositories.Hr.Attendance
{
    public interface IAttendanceLogRepository : IRepository<AttendanceLog>
    {
    }
}
