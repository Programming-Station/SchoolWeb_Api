using School_DTOs;
using School_DTOs.Dashboard;

namespace School.Services.Interfaces
{
    public interface IEmployeeDashboardService
    {
        Task<APIResponse<EmployeeDashboardDto>> GetEmployeeDashboardDataAsync(string userId);
        Task<APIResponse<EmployeeStatsDto>> ClockInOutAsync(string userId, bool isClockIn);
    }
}
