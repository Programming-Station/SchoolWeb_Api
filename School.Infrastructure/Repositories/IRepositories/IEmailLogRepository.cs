using School.Domain.Email;

namespace School.Infrastructure.Repositories.IRepositories
{
    public interface IEmailLogRepository
    {
        Task<EmailLog?> GetByIdAsync(int id);
        Task<IEnumerable<EmailLog>> GetAllBySchoolIdAsync(int schoolId, int page = 1, int pageSize = 20);
        Task<int> GetTotalCountAsync(int schoolId);
        Task<int> DeleteAsync(int id);
        Task<int> DeleteOldLogsAsync(int schoolId, int daysOlderThan = 90);
    }
}
