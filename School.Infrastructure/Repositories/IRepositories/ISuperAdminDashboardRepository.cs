using School_DTOs;
using School_DTOs.Dashboard;

namespace School.Infrastructure.Repositories.IRepositories
{
    public interface ISuperAdminDashboardRepository
    {
        Task<APIResponse<SuperAdminDashboardDto>> GetSuperAdminDashboardDataAsync();
        Task<APIResponse<SuperAdminSystemHealthDto>> GetSystemHealthAsync();
    }
}
