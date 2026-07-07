using School_DTOs;
using School_DTOs.Email;

namespace School.Services.Interfaces.Email
{
    public interface IEmailLogService
    {
        Task<APIResponse<EmailLogDto>> GetByIdAsync(int id);
        Task<APIResponse<object>> GetAllBySchoolIdAsync(int schoolId, int page = 1, int pageSize = 20);
        Task<APIResponse<bool>> DeleteAsync(int id);
        Task<APIResponse<bool>> DeleteOldLogsAsync(int schoolId, int daysOlderThan = 90);
    }
}
