using School.Infrastructure.Repositories.IRepositories;
using School.Services.Interfaces;
using School_DTOs;
using School_DTOs.Dashboard;

namespace School.Services
{
    public class SuperAdminDashboardService : ISuperAdminDashboardService
    {
        private readonly ISuperAdminDashboardRepository _dashboardRepository;

        public SuperAdminDashboardService(ISuperAdminDashboardRepository dashboardRepository)
        {
            _dashboardRepository = dashboardRepository;
        }

        public Task<APIResponse<SuperAdminDashboardDto>> GetSuperAdminDashboardDataAsync()
        {
            return _dashboardRepository.GetSuperAdminDashboardDataAsync();
        }

        public Task<APIResponse<SuperAdminSystemHealthDto>> GetSystemHealthAsync()
        {
            return _dashboardRepository.GetSystemHealthAsync();
        }
    }
}
