using School_DTOs;
using School_DTOs.Dashboard;
using System.Threading.Tasks;

namespace School.Services.Interfaces
{
    public interface ISuperAdminDashboardService
    {
        Task<APIResponse<SuperAdminDashboardDto>> GetSuperAdminDashboardDataAsync();
        Task<APIResponse<SuperAdminSystemHealthDto>> GetSystemHealthAsync();
    }
}
