using School.Domain.Hr.LeaveManagement;
using School.Infrastructure.UnitOfWork;

namespace School.Infrastructure.Repositories.Hr.LeaveManagement
{
    public class LeaveTypeRepository : Repository<LeaveType>, ILeaveTypeRepository
    {
        public LeaveTypeRepository(DbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
