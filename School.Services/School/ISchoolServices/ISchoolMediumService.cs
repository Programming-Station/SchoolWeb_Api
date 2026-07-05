using School_DTOs;
using School_DTOs.School;
using System.Threading.Tasks;

namespace School.Services.School.ISchoolServices
{
    public interface ISchoolMediumService
    {
        Task<PagedResponse<SchoolMediumDto>> GetAllsAsync(int pageNumber = 1, int pageSize = 10, string? searchTerm = null, bool? isActive = null);
        Task<APIResponse<SchoolMediumDto>> GetByIdAsync(int id);
        Task<APIResponse<SchoolMediumDto>> AddAsync(SchoolMediumModel model);
        Task<APIResponse<SchoolMediumDto>> EditAsync(SchoolMediumModel model);
        Task<APIResponse> DeleteAsync(int id);
    }
}
