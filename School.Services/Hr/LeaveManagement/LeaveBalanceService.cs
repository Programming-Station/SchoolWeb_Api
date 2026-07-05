using School.Infrastructure.Repositories.Hr.LeaveManagement;

namespace School.Services.Hr.LeaveManagement
{
    public class LeaveBalanceService : ILeaveBalanceService
    {
        private readonly ILeaveBalanceRepository _repository;
        public LeaveBalanceService(ILeaveBalanceRepository repository)
        {
            _repository = repository;
        }
    }
}
