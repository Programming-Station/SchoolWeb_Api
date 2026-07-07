using School.Domain.Hr.LeaveManagement;
using School.Infrastructure.UnitOfWork;

namespace School.Infrastructure.Repositories.Hr.LeaveManagement
{
    public class LeaveRequestRepository : Repository<LeaveRequest>, ILeaveRequestRepository
    {
        public LeaveRequestRepository(DbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
