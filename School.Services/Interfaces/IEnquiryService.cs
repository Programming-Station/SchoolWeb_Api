using School.Models.Website;
using School_DTOs;
using School_DTOs.Website;

namespace School.Services.Interfaces
{
    public interface IEnquiryService
    {
        Task<APIResponse<EnquiryDto>> AddEnquiryAsync(EnquiryModel model, string? ipAddress = null, string? userAgent = null);
        Task<APIResponse<EnquiryDto>> GetEnquiryByIdAsync(int id);
        Task<APIResponse<IEnumerable<EnquiryDto>>> GetAllEnquiriesAsync(int? statusId = null, int? pageNumber = null, int? pageSize = null);
        Task<PagedResponse<EnquiryDto>> GetAllPagedAsync(int pageNumber = 1, int pageSize = 10, int? statusId = null);
        Task<APIResponse> UpdateEnquiryAsync(EnquiryModel model);
        Task<APIResponse> UpdateEnquiryStatusAsync(int id, int statusId, string? adminReply = null, string? repliedBy = null);
        Task<APIResponse> DeleteEnquiryAsync(int id);
        Task<APIResponse<int>> GetEnquiryCountAsync(int? statusId = null);
    }
}
