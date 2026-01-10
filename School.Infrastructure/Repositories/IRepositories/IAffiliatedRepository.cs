using School.Domain;
using School.Infrastructure.UnitOfWork.Interfaces;

namespace School.Infrastructure.Repositories.IRepositories
{
    public interface IAffiliatedRepository : IRepository<Affiliated>
    {
        Task<Affiliated> AddAffiliationCollegeAsync(Affiliated entity);
        Task<Affiliated> GetAffiliationCollegeByIdAsync(int id);
        Task<IEnumerable<Affiliated>> GetAllAsync(int? stateId = null, int? cityId = null, bool? isActive = null);
        Task<int> UpdateAffiliationCollegeAsync(Affiliated entity);
        Task<int> DeleteAffiliationCollegeAsync(int id);
        Task<int> ToggleAffiliationCollegeStatusAsync(int id);
    }
}
