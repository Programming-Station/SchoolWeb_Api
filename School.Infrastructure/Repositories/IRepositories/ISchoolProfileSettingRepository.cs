using School.Domain.School;
using System.Threading.Tasks;

namespace School.Infrastructure.Repositories.IRepositories
{
    public interface ISchoolProfileSettingRepository
    {
        Task<SchoolProfileSetting?> GetBySchoolIdAsync(int schoolRegistrationId);
        Task<int> UpdateProfileSettingAsync(SchoolProfileSetting entity);
    }
}


