using School.Domain.Hr.LeaveManagement;
using School.Infrastructure.UnitOfWork;

namespace School.Infrastructure.Repositories.Hr.LeaveManagement
{
    public class LeaveSettingRepository : Repository<LeaveSetting>, ILeaveSettingRepository
    {
        public LeaveSettingRepository(DbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
