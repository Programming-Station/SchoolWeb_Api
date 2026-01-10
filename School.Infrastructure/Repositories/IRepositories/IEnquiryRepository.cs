using School.Domain.Website;
using School.Infrastructure.UnitOfWork.Interfaces;

namespace School.Infrastructure.Repositories.IRepositories
{
    public interface IEnquiryRepository : IRepository<Enquiry>
    {
        Task<Enquiry> AddEnquiryAsync(Enquiry entity);
        Task<Enquiry> GetEnquiryByIdAsync(int id);
        Task<IEnumerable<Enquiry>> GetAllAsync(int? statusId = null, int? pageNumber = null, int? pageSize = null);
        Task<int> GetTotalCountAsync(int? statusId = null);
        Task<int> UpdateEnquiryAsync(Enquiry entity);
        Task<int> UpdateEnquiryStatusAsync(int id, int statusId, string? adminReply = null, string? repliedBy = null);
        Task<int> DeleteEnquiryAsync(int id);
        Task<string> GenerateEnquiryNoAsync();
    }
}
