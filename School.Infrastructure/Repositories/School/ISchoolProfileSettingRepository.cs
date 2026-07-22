using School.Domain.School;

namespace School.Infrastructure.Repositories.School
{
    public interface ISchoolProfileSettingRepository
    {
        Task<SchoolProfileSetting?> GetBySchoolIdAsync(int schoolRegistrationId);
        Task<int> UpdateProfileSettingAsync(SchoolProfileSetting entity);
    }
}


