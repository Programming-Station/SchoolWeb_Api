using School_DTOs;
using School_DTOs.Dashboard;
using System.Threading.Tasks;

namespace School.Infrastructure.Repositories.IRepositories
{
    public interface ISuperAdminDashboardRepository
    {
        Task<APIResponse<SuperAdminDashboardDto>> GetSuperAdminDashboardDataAsync();
        Task<APIResponse<SuperAdminSystemHealthDto>> GetSystemHealthAsync();
    }
}
