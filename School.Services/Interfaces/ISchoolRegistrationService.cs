using School.Models.Website;
using School_DTOs;
using School_DTOs.Website;

namespace School.Services.Interfaces
{
    public interface ISchoolRegistrationService
    {
        Task<APIResponse<SchoolRegistrationDto>> AddSchoolRegistrationAsync(SchoolRegistrationModel model, string? ipAddress = null, string? userAgent = null);
        Task<APIResponse<SchoolRegistrationDto>> GetSchoolRegistrationByIdAsync(int id);
        Task<APIResponse<SchoolRegistrationDto>> GetSchoolRegistrationByIdAsync(string id);
        Task<APIResponse<IEnumerable<SchoolRegistrationDto>>> GetAllSchoolRegistrationsAsync(string? status = null, int? pageNumber = null, int? pageSize = null);
        Task<PagedResponse<SchoolRegistrationDto>> GetAllPagedAsync(int pageNumber = 1, int pageSize = 10, string? status = null);
        Task<APIResponse> UpdateSchoolRegistrationAsync(SchoolRegistrationModel model);
        Task<APIResponse> UpdateSchoolRegistrationStatusAsync(int id, int statusId, string? approvedBy = null, string? rejectionReason = null);
        Task<APIResponse> DeleteSchoolRegistrationAsync(int id);
        Task<APIResponse<bool>> CheckEmailExistsAsync(string email);
        Task<APIResponse<bool>> CheckMobileExistsAsync(string mobile);
        Task<APIResponse<int>> GetSchoolRegistrationCountAsync(string? status = null);
    }
}
