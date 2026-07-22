using School_DTOs;
using School_DTOs.School;

namespace School.Services.School.ISchoolServices
{
    public interface ISchoolTypeService
    {
        Task<PagedResponse<SchoolTypeDto>> GetAllsAsync(int pageNumber = 1, int pageSize = 10, string? searchTerm = null, bool? isActive = null);
        Task<APIResponse<SchoolTypeDto>> GetByIdAsync(int id);
        Task<APIResponse<SchoolTypeDto>> AddAsync(SchoolTypeModel model);
        Task<APIResponse<SchoolTypeDto>> EditAsync(SchoolTypeModel model);
        Task<APIResponse> DeleteAsync(int id);
    }
}
