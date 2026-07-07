using School.Models.Email;
using School_DTOs;
using School_DTOs.Email;

namespace School.Services.Interfaces.Email
{
    public interface IEmailTemplateService
    {
        Task<APIResponse<EmailTemplateDto>> GetByIdAsync(int id);
        Task<APIResponse<IEnumerable<EmailTemplateDto>>> GetAllBySchoolIdAsync(int schoolId);
        Task<APIResponse<EmailTemplateDto>> AddAsync(EmailTemplateModel model);
        Task<APIResponse<EmailTemplateDto>> UpdateAsync(EmailTemplateModel model);
        Task<APIResponse<bool>> DeleteAsync(int id);
        Task<APIResponse<bool>> ToggleStatusAsync(int id);
    }
}
