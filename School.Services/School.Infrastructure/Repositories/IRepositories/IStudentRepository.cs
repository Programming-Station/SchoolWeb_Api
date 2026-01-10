using School.Models.Student;
using School_DTOs;
using School_DTOs.Student;

namespace School.Infrastructure.Repositories.IRepositories
{
    public interface IStudentRepository
    {
        Task<APIResponse<StudentDto>> AddStudentAsync(StudentModel model);
        Task<APIResponse<StudentDto>> GetStudentByIdAsync(int id);
        Task<APIResponse<StudentDto>> GetStudentByStudentIdAsync(string studentId);
        Task<PagedResponse<StudentDto>> GetAllStudentsAsync(int pageNumber = 1, int pageSize = 10, string? searchTerm = null, string? status = null, string? classFilter = null);
        Task<APIResponse> UpdateStudentAsync(StudentModel model);
        Task<APIResponse> DeleteStudentAsync(int id);
        Task<string> GenerateStudentIdAsync();
    }
}

