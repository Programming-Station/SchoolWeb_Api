using School.Domain.School;
using System.Linq;
using System.Threading.Tasks;

namespace School.Infrastructure.Repositories.IRepositories
{
    public interface ISchoolSubscriptionRepository
    {
        IQueryable<SchoolSubscription> GetAllQueryable();
        Task<SchoolSubscription?> GetByIdAsync(int id);
        Task<int> AddAsync(SchoolSubscription entity);
        Task<int> UpdateAsync(SchoolSubscription entity);
        Task<int> DeleteAsync(int id);
    }
}


