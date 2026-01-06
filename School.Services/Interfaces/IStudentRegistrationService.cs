using School.Models.Student;
using School_DTOs;
using School_DTOs.Student;

namespace School.Services.Interfaces
{
    public interface IStudentRegistrationService
    {
        Task<APIResponse<StudentRegistrationDto>> AddStudentRegistrationAsync(StudentRegistrationModel model);
        Task<APIResponse<StudentRegistrationDto>> GetStudentRegistrationByIdAsync(int id);
        Task<PagedResponse<StudentRegistrationDto>> GetAllAsync(int pageNumber = 1, int pageSize = 10, string? searchTerm = null, string? status = null);
        Task<APIResponse> UpdateStudentRegistrationAsync(StudentRegistrationModel model);
        Task<APIResponse> DeleteStudentRegistrationAsync(int id);
        Task<APIResponse<StudentRegistrationDto>> GetByMobileAsync(string mobile);
        Task<APIResponse<StudentRegistrationDto>> GetByAadhaarAsync(string aadhaarNumber);
        Task<APIResponse> UpdateStatusAsync(UpdateStudentRegistrationStatusDto dto);
    }
}

