using School_DTOs;
using School_DTOs.Dashboard;
using System.Threading.Tasks;

namespace School.Services.Interfaces
{
    public interface IEmployeeDashboardService
    {
        Task<APIResponse<EmployeeDashboardDto>> GetEmployeeDashboardDataAsync(string userId);
        Task<APIResponse<EmployeeStatsDto>> ClockInOutAsync(string userId, bool isClockIn);
    }
}
