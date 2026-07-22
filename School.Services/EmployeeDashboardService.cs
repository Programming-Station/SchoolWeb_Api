using School.Infrastructure.Repositories.IRepositories;
using School.Services.Interfaces;
using School_DTOs;
using School_DTOs.Dashboard;

namespace School.Services
{
    public class EmployeeDashboardService : IEmployeeDashboardService
    {
        private readonly IEmployeeDashboardRepository _repository;

        public EmployeeDashboardService(IEmployeeDashboardRepository repository)
        {
            _repository = repository;
        }

        public Task<APIResponse<EmployeeDashboardDto>> GetEmployeeDashboardDataAsync(string userId)
        {
            return _repository.GetEmployeeDashboardDataAsync(userId);
        }

        public Task<APIResponse<EmployeeStatsDto>> ClockInOutAsync(string userId, bool isClockIn)
        {
            return _repository.ClockInOutAsync(userId, isClockIn);
        }
    }
}
