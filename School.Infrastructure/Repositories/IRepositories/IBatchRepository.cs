using School.Domain;

namespace School.Infrastructure.Repositories.IRepositories
{
    public interface IBatchRepository
    {
        Task<Batch> AddAsync(Batch entity);
        Task<Batch> GetByIdAsync(int id);
        Task<IEnumerable<Batch>> GetAllAsync(int? programId = null, bool? isActive = null);
        Task<int> UpdateAsync(Batch entity);
        Task<int> DeleteAsync(int id);
    }
}
