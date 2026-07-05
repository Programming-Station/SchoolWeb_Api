using School.Infrastructure.Repositories.Hr.LeaveManagement;

namespace School.Services.Hr.LeaveManagement
{
    public class LeaveRequestService : ILeaveRequestService
    {
        private readonly ILeaveRequestRepository _repository;
        public LeaveRequestService(ILeaveRequestRepository repository)
        {
            _repository = repository;
        }
    }
}
