using School.Domain.Hr.Attendance;
using School.Infrastructure.UnitOfWork;

namespace School.Infrastructure.Repositories.Hr.Attendance
{
    public class ShiftMasterRepository : Repository<ShiftMaster>, IShiftMasterRepository
    {
        public ShiftMasterRepository(DbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
