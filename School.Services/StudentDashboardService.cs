using School.Infrastructure.Repositories.IRepositories;
using School.Services.Interfaces;
using School_DTOs;
using School_DTOs.Dashboard;
using System.Threading.Tasks;

namespace School.Services
{
    public class StudentDashboardService : IStudentDashboardService
    {
        private readonly IStudentDashboardRepository _repository;

        public StudentDashboardService(IStudentDashboardRepository repository)
        {
            _repository = repository;
        }

        public async Task<APIResponse<StudentDashboardDto>> GetStudentDashboardDataAsync(string userId)
        {
            return await _repository.GetStudentDashboardDataAsync(userId);
        }
    }
}
