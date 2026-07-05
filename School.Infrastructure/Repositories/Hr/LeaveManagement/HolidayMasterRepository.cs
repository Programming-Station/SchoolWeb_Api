using School.Infrastructure.Repositories.IRepositories.Hr.LeaveManagement;
using School.Domain.Hr.LeaveManagement;
using School.Infrastructure.UnitOfWork;

namespace School.Infrastructure.Repositories.Hr.LeaveManagement
{
    public class HolidayMasterRepository : Repository<HolidayMaster>, IHolidayMasterRepository
    {
        public HolidayMasterRepository(DbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}

