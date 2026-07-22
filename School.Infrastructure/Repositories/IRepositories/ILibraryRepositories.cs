using School.Domain.Library;
using School.Infrastructure.UnitOfWork.Interfaces;

namespace School.Infrastructure.Repositories.IRepositories
{
    public interface IBookRepository : IRepository<Book>
    {
        Task<Book?> GetByIdAsync(int id);
        Task<IEnumerable<Book>> GetAllBySchoolAsync(int schoolId);
        Task<int> AddAsync(Book entity);
        Task<int> UpdateAsync(Book entity);
        Task<int> DeleteAsync(int id);
    }

    public interface IBookIssueLogRepository : IRepository<BookIssueLog>
    {
        Task<BookIssueLog?> GetByIdAsync(int id);
        Task<IEnumerable<BookIssueLog>> GetByStudentAsync(int studentId, int schoolId);
        Task<IEnumerable<BookIssueLog>> GetAllBySchoolAsync(int schoolId);
        Task<int> AddAsync(BookIssueLog entity);
        Task<int> UpdateAsync(BookIssueLog entity);
        Task<int> DeleteAsync(int id);
    }
}
