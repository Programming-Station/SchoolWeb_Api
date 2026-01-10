using School.Domain.Student;
using School.Infrastructure.UnitOfWork.Interfaces;

namespace School.Infrastructure.Repositories.IRepositories
{
    public interface IStudentRepository : IRepository<Student>
    {
        Task<Student> AddStudentAsync(Student entity);
        Task<Student> GetStudentByIdAsync(int id);
        Task<Student?> GetStudentByStudentIdAsync(string studentId);
        Task<(IEnumerable<Student> Students, int TotalCount)> GetAllAsync(int pageNumber = 1, int pageSize = 10, string? searchTerm = null, int? statusId = null, string? classFilter = null);
        Task<int> UpdateStudentAsync(Student entity);
        Task<int> DeleteStudentAsync(int id);
        Task<string> GenerateStudentIdAsync();
    }
}
