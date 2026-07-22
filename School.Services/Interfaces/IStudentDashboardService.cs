using School_DTOs;
using School_DTOs.Dashboard;

namespace School.Services.Interfaces
{
    public interface IStudentDashboardService
    {
        Task<APIResponse<StudentDashboardDto>> GetStudentDashboardDataAsync(string userId);
    }
}
