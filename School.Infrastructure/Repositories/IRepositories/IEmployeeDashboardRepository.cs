using School_DTOs;
using School_DTOs.Dashboard;
using System.Threading.Tasks;

namespace School.Infrastructure.Repositories.IRepositories
{
    public interface IEmployeeDashboardRepository
    {
        Task<APIResponse<EmployeeDashboardDto>> GetEmployeeDashboardDataAsync(string userId);
        Task<APIResponse<EmployeeStatsDto>> ClockInOutAsync(string userId, bool isClockIn);
    }
}
