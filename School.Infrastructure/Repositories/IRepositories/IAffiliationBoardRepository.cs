using School.Domain.School;
using System.Linq;
using System.Threading.Tasks;

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


