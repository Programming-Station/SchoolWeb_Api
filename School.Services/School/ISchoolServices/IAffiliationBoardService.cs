using School_DTOs;
using School_DTOs.School;

namespace School.Services.School.ISchoolServices
{
    public interface IAffiliationBoardService
    {
        Task<PagedResponse<AffiliationBoardDto>> GetAllsAsync(int pageNumber = 1, int pageSize = 10, string? searchTerm = null, bool? isActive = null);
        Task<APIResponse<AffiliationBoardDto>> GetByIdAsync(int id);
        Task<APIResponse<AffiliationBoardDto>> AddAsync(AffiliationBoardModel model);
        Task<APIResponse<AffiliationBoardDto>> EditAsync(AffiliationBoardModel model);
        Task<APIResponse> DeleteAsync(int id);
    }
}
