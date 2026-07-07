using School.Domain.Email;

namespace School.Infrastructure.Repositories.IRepositories
{
    public interface IEmailBrandingRepository
    {
        Task<EmailBranding?> GetByIdAsync(int id);
        Task<EmailBranding?> GetBySchoolIdAsync(int schoolId);
        Task<EmailBranding> AddAsync(EmailBranding entity);
        Task<int> UpdateAsync(EmailBranding entity);
    }
}
