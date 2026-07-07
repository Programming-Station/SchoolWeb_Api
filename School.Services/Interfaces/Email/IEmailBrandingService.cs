using School.Models.Email;
using School_DTOs;
using School_DTOs.Email;

namespace School.Services.Interfaces.Email
{
    public interface IEmailBrandingService
    {
        Task<APIResponse<EmailBrandingDto>> GetByIdAsync(int id);
        Task<APIResponse<EmailBrandingDto>> GetBySchoolIdAsync(int schoolId);
        Task<APIResponse<EmailBrandingDto>> AddAsync(EmailBrandingModel model);
        Task<APIResponse<EmailBrandingDto>> UpdateAsync(EmailBrandingModel model);
    }
}
