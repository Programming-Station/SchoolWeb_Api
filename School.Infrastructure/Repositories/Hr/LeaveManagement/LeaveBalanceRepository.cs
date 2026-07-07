using School.Domain.Hr.LeaveManagement;
using School.Infrastructure.UnitOfWork;

namespace School.Infrastructure.Repositories.Hr.LeaveManagement
{
    public class LeaveBalanceRepository : Repository<LeaveBalance>, ILeaveBalanceRepository
    {
        public LeaveBalanceRepository(DbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
