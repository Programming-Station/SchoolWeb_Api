using School_DTOs;
using School_DTOs.School;
using System.Threading.Tasks;

namespace School.Services.School.ISchoolServices
{
    public interface ISchoolProfileSettingService
    {
        Task<APIResponse<SchoolProfileSettingDto>> GetMyProfileSettingsAsync();
        Task<APIResponse<SchoolProfileSettingDto>> UpdateMyProfileSettingsAsync(SchoolProfileSettingModel model);
    }
}
