using School_DTOs;
using School_DTOs.Dashboard;

namespace School.Infrastructure.Repositories.IRepositories
{
    public interface IStudentDashboardRepository
    {
        Task<APIResponse<StudentDashboardDto>> GetStudentDashboardDataAsync(string userId);
    }
}
