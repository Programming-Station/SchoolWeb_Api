using School_DTOs;
using School_DTOs.Dashboard;
using System.Threading.Tasks;

namespace School.Infrastructure.Repositories.IRepositories
{
    public interface IStudentDashboardRepository
    {
        Task<APIResponse<StudentDashboardDto>> GetStudentDashboardDataAsync(string userId);
    }
}
