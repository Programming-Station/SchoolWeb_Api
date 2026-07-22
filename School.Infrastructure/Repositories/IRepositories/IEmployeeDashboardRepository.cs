using School_DTOs;
using School_DTOs.Dashboard;

namespace School.Infrastructure.Repositories.IRepositories
{
    public interface IEmployeeDashboardRepository
    {
        Task<APIResponse<EmployeeDashboardDto>> GetEmployeeDashboardDataAsync(string userId);
        Task<APIResponse<EmployeeStatsDto>> ClockInOutAsync(string userId, bool isClockIn);
    }
}
