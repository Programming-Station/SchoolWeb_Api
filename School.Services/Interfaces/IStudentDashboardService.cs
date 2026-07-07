using School_DTOs;
using School_DTOs.Dashboard;
using System.Threading.Tasks;

namespace School.Services.Interfaces
{
    public interface IStudentDashboardService
    {
        Task<APIResponse<StudentDashboardDto>> GetStudentDashboardDataAsync(string userId);
    }
}
