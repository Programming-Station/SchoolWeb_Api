using School.Domain.School;

namespace School.Infrastructure.Repositories.IRepositories
{
    public interface IAffiliationBoardRepository
    {
        IQueryable<AffiliationBoard> GetAllQueryable();
        Task<AffiliationBoard?> GetByIdAsync(int id);
        Task<int> AddAsync(AffiliationBoard entity);
        Task<int> UpdateAsync(AffiliationBoard entity);
        Task<int> DeleteAsync(int id);
    }
}


