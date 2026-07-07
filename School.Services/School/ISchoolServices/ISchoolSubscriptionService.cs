using School_DTOs;
using School_DTOs.School;
using System.Threading.Tasks;

namespace School.Services.School.ISchoolServices
{
    public interface ISchoolSubscriptionService
    {
        Task<PagedResponse<SchoolSubscriptionDto>> GetAllsAsync(int pageNumber = 1, int pageSize = 10, string? searchTerm = null, bool? isActive = null);
        Task<APIResponse<SchoolSubscriptionDto>> GetByIdAsync(int id);
        Task<APIResponse<SchoolSubscriptionDto>> AddAsync(SchoolSubscriptionModel model);
        Task<APIResponse<SchoolSubscriptionDto>> EditAsync(SchoolSubscriptionModel model);
        Task<APIResponse> DeleteAsync(int id);
    }
}
