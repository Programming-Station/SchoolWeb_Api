using School_DTOs;
using School_DTOs.School;
using System.Threading.Tasks;

namespace School.Services.School.ISchoolServices
{
    public interface ISchoolOwnerService
    {
        Task<PagedResponse<SchoolOwnerDto>> GetAllsAsync(int pageNumber = 1, int pageSize = 10, string? searchTerm = null, bool? isActive = null);
        Task<APIResponse<SchoolOwnerDto>> GetByIdAsync(int id);
        Task<APIResponse<SchoolOwnerDto>> AddAsync(SchoolOwnerModel model);
        Task<APIResponse<SchoolOwnerDto>> EditAsync(SchoolOwnerModel model);
        Task<APIResponse> DeleteAsync(int id);
    }
}
