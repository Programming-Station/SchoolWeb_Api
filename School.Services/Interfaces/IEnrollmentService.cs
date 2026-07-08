using System.Threading.Tasks;
using School_DTOs;

namespace School.Services.Interfaces
{
    public interface IEnrollmentService
    {
        Task<APIResponse> EnrollStudentAsync(int applicationId, string username);
    }
}
