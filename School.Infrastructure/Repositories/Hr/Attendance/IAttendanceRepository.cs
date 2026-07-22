using School.Infrastructure.UnitOfWork.Interfaces;

namespace School.Infrastructure.Repositories.Hr.Attendance
{
    public interface IAttendanceRepository : IRepository<Domain.Hr.Attendance.Attendance>
    {
    }
}
