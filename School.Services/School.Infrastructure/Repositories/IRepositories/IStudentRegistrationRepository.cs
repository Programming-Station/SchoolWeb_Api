using School.Domain.Student;

namespace School.Infrastructure.Repositories.IRepositories
{
    public interface IStudentRegistrationRepository
    {
        Task<StudentRegistration> AddStudentRegistrationAsync(StudentRegistration entity);
        Task<StudentRegistration> GetStudentRegistrationByIdAsync(int id);
        Task<IEnumerable<StudentRegistration>> GetAllAsync(int pageNumber = 1, int pageSize = 10, string? searchTerm = null, string? status = null);
        Task<int> GetTotalCountAsync(string? searchTerm = null, string? status = null);
        Task<int> UpdateStudentRegistrationAsync(StudentRegistration entity);
        Task<int> DeleteStudentRegistrationAsync(int id);
        Task<StudentRegistration> GetByMobileAsync(string mobile);
        Task<StudentRegistration> GetByAadhaarAsync(string aadhaarNumber);
        Task<bool> ExistsByMobileAsync(string mobile, int? excludeId = null);
        Task<bool> ExistsByAadhaarAsync(string aadhaarNumber, int? excludeId = null);
    }
}

