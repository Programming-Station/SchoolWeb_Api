using School.Domain;

namespace School.Infrastructure.Repositories.IRepositories
{
    public interface IProgramRepository
    {
        Task<Program> AddAsync(Program entity);
        Task<Program> GetByIdAsync(int id);
        Task<IEnumerable<Program>> GetAllAsync(int? educationLevelId = null, bool? isActive = null);
        Task<int> UpdateAsync(Program entity);
        Task<int> DeleteAsync(int id);
    }
}
