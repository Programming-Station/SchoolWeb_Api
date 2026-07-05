using School.Domain.Hr.Attendance;
using School.Infrastructure.UnitOfWork;

namespace School.Infrastructure.Repositories.Hr.Attendance
{
    public class WeekOffRepository : Repository<WeekOff>, IWeekOffRepository
    {
        public WeekOffRepository(DbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
