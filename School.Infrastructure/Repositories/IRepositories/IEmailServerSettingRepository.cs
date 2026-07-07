using School.Domain.Email;

namespace School.Infrastructure.Repositories.IRepositories
{
    public interface IEmailServerSettingRepository
    {
        Task<EmailServerSetting?> GetByIdAsync(int id);
        Task<IEnumerable<EmailServerSetting>> GetAllBySchoolIdAsync(int schoolId);
        Task<EmailServerSetting> AddAsync(EmailServerSetting entity);
        Task<int> UpdateAsync(EmailServerSetting entity);
        Task<int> DeleteAsync(int id);
        Task<int> ToggleStatusAsync(int id);
    }
}
