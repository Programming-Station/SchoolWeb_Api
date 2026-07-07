using School.Domain.Email;

namespace School.Infrastructure.Repositories.IRepositories
{
    public interface IEmailTemplateRepository
    {
        Task<EmailTemplate?> GetByIdAsync(int id);
        Task<IEnumerable<EmailTemplate>> GetAllBySchoolIdAsync(int schoolId);
        Task<EmailTemplate> AddAsync(EmailTemplate entity);
        Task<int> UpdateAsync(EmailTemplate entity);
        Task<int> DeleteAsync(int id);
        Task<int> ToggleStatusAsync(int id);
    }
}
