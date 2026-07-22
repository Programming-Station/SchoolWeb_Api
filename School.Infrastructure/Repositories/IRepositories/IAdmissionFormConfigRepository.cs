using School.Domain.Student;

namespace School.Infrastructure.Repositories.IRepositories
{
    public interface IAdmissionFormConfigRepository
    {
        Task<AdmissionFormConfig> AddAsync(AdmissionFormConfig entity);
        Task<AdmissionFormConfig> GetByIdAsync(int id);
        Task<AdmissionFormConfig?> GetConfigAsync(int campusId, int educationLevelId, int? programId);
        Task<IEnumerable<AdmissionFormConfig>> GetAllAsync();
        Task<int> UpdateAsync(AdmissionFormConfig entity);
        Task<int> DeleteAsync(int id);
    }
}
