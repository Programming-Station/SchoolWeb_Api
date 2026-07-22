using School.Domain.Student;

namespace School.Infrastructure.Repositories.IRepositories
{
    public interface IAdmissionRuleRepository
    {
        Task<AdmissionRule> AddAsync(AdmissionRule entity);
        Task<AdmissionRule> GetByIdAsync(int id);
        Task<IEnumerable<AdmissionRule>> GetRulesAsync(int campusId, int educationLevelId, int? programId = null);
        Task<IEnumerable<AdmissionRule>> GetAllAsync();
        Task<int> UpdateAsync(AdmissionRule entity);
        Task<int> DeleteAsync(int id);
    }
}
