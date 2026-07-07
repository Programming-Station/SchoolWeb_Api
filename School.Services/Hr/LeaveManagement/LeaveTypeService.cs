using School.Infrastructure.Repositories.Hr.LeaveManagement;

namespace School.Services.Hr.LeaveManagement
{
    public class LeaveTypeService : ILeaveTypeService
    {
        private readonly ILeaveTypeRepository _repository;
        public LeaveTypeService(ILeaveTypeRepository repository)
        {
            _repository = repository;
        }
    }
}
