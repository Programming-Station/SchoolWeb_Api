using School.Models.AffiliationCollege;
using School_DTOs;
using School_DTOs.AffiliationCollege;

namespace School.Infrastructure.Repositories.IRepositories
{
    public interface IAffiliatedRepository
    {
        Task<APIResponse<AffiliatedDto>> AddAffiliationCollegeAsync(AffiliatedModel model);

        Task<APIResponse<AffiliatedDto>> GetAffiliationCollegeByIdAsync(int id);

        Task<APIResponse<IEnumerable<AffiliatedDto>>> GetAllAffiliationCollegesAsync(
            int? stateId = null,
            int? cityId = null,
            bool? isActive = null
        );

        Task<APIResponse> UpdateAffiliationCollegeAsync(AffiliatedModel model);

        Task<APIResponse> DeleteAffiliationCollegeAsync(int id);

        Task<APIResponse> ToggleAffiliationCollegeStatusAsync(int id);
    }
}
