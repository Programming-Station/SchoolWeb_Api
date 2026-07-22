using School.Domain;

namespace School.Infrastructure.Repositories.IRepositories
{
    public interface IBranchRepository
    {
        Task<Branch> AddAsync(Branch entity);
        Task<Branch> GetByIdAsync(int id);
        Task<IEnumerable<Branch>> GetAllAsync(int? programId = null, bool? isActive = null);
        Task<int> UpdateAsync(Branch entity);
        Task<int> DeleteAsync(int id);
    }
}
