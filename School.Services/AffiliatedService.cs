using School.Infrastructure.Repositories.IRepositories;
using School.Models.AffiliationCollege;
using School.Services.Interfaces;
using School_DTOs;
using School_DTOs.AffiliationCollege;

namespace School.Services
{
    public class AffiliatedService : IAffiliatedService
    {
        private readonly IAffiliatedRepository _collegeRepository;

        public AffiliatedService(IAffiliatedRepository collegeRepository)
        {
            _collegeRepository = collegeRepository;
        }

        public Task<APIResponse<AffiliatedDto>> AddAffiliationCollegeAsync(AffiliatedModel model)
        {
            return _collegeRepository.AddAffiliationCollegeAsync(model);
        }

        public Task<APIResponse<AffiliatedDto>> GetAffiliationCollegeByIdAsync(int id)
        {
            return _collegeRepository.GetAffiliationCollegeByIdAsync(id);
        }

        public Task<APIResponse<IEnumerable<AffiliatedDto>>> GetAllAffiliationCollegesAsync(
            int? stateId = null,
            int? cityId = null,
            bool? isActive = null)
        {
            return _collegeRepository.GetAllAffiliationCollegesAsync(stateId, cityId, isActive);
        }

        public Task<APIResponse> UpdateAffiliationCollegeAsync(AffiliatedModel model)
        {
            return _collegeRepository.UpdateAffiliationCollegeAsync(model);
        }

        public Task<APIResponse> DeleteAffiliationCollegeAsync(int id)
        {
            return _collegeRepository.DeleteAffiliationCollegeAsync(id);
        }

        public Task<APIResponse> ToggleAffiliationCollegeStatusAsync(int id)
        {
            return _collegeRepository.ToggleAffiliationCollegeStatusAsync(id);
        }
    }
}
