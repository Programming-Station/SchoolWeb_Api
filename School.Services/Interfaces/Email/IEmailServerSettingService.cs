using School.Models.Email;
using School_DTOs;
using School_DTOs.Email;

namespace School.Services.Interfaces.Email
{
    public interface IEmailServerSettingService
    {
        Task<APIResponse<EmailServerSettingDto>> GetByIdAsync(int id);
        Task<APIResponse<IEnumerable<EmailServerSettingDto>>> GetAllBySchoolIdAsync(int schoolId);
        Task<APIResponse<EmailServerSettingDto>> AddAsync(EmailServerSettingModel model);
        Task<APIResponse<EmailServerSettingDto>> UpdateAsync(EmailServerSettingModel model);
        Task<APIResponse<bool>> UpdatePasswordAsync(int id, string newPassword, string updatedBy);
        Task<APIResponse<bool>> DeleteAsync(int id);
        Task<APIResponse<bool>> ToggleStatusAsync(int id);
    }
}
