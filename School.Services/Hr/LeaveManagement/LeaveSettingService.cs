using School.Infrastructure.Repositories.Hr.LeaveManagement;

namespace School.Services.Hr.LeaveManagement
{
    public class LeaveSettingService : ILeaveSettingService
    {
        private readonly ILeaveSettingRepository _repository;
        public LeaveSettingService(ILeaveSettingRepository repository)
        {
            _repository = repository;
        }
    }
}
