using School.Infrastructure.Repositories.IRepositories;
using School.Models.Website;
using School.Services.Interfaces;
using School_DTOs;
using School_DTOs.Website;

namespace School.Services
{
    public class EnquiryService : IEnquiryService
    {
        private readonly IEnquiryRepository _enquiryRepository;

        public EnquiryService(IEnquiryRepository enquiryRepository)
        {
            _enquiryRepository = enquiryRepository;
        }

        public Task<APIResponse<EnquiryDto>> AddEnquiryAsync(EnquiryModel model, string? ipAddress = null, string? userAgent = null)
        {
            return _enquiryRepository.AddEnquiryAsync(model, ipAddress, userAgent);
        }

        public Task<APIResponse<EnquiryDto>> GetEnquiryByIdAsync(int id)
        {
            return _enquiryRepository.GetEnquiryByIdAsync(id);
        }

        public Task<APIResponse<IEnumerable<EnquiryDto>>> GetAllEnquiriesAsync(int? statusId = null, int? pageNumber = null, int? pageSize = null)
        {
            return _enquiryRepository.GetAllEnquiriesAsync(statusId, pageNumber, pageSize);
        }

        public Task<APIResponse> UpdateEnquiryStatusAsync(int id, int statusId, string? adminReply = null, string? repliedBy = null)
        {
            return _enquiryRepository.UpdateEnquiryStatusAsync(id, statusId, adminReply, repliedBy);
        }

        public Task<APIResponse> DeleteEnquiryAsync(int id)
        {
            return _enquiryRepository.DeleteEnquiryAsync(id);
        }

        public Task<APIResponse<int>> GetEnquiryCountAsync(int? statusId = null)
        {
            return _enquiryRepository.GetEnquiryCountAsync(statusId);
        }
    }
}



