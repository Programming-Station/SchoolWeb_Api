using School.Models.Website;
using School_DTOs;
using School_DTOs.Website;

namespace School.Infrastructure.Repositories.IRepositories
{
    public interface IEnquiryRepository
    {
        Task<APIResponse<EnquiryDto>> AddEnquiryAsync(EnquiryModel model, string? ipAddress = null, string? userAgent = null);
        Task<APIResponse<EnquiryDto>> GetEnquiryByIdAsync(int id);
        Task<APIResponse<IEnumerable<EnquiryDto>>> GetAllEnquiriesAsync(int? statusId = null, int? pageNumber = null, int? pageSize = null);
        Task<APIResponse> UpdateEnquiryStatusAsync(int id, int statusId, string? adminReply = null, string? repliedBy = null);
        Task<APIResponse> DeleteEnquiryAsync(int id);
        Task<APIResponse<int>> GetEnquiryCountAsync(int? statusId = null);
    }
}



