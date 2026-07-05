using School.Domain.School;
using System.Linq;
using System.Threading.Tasks;

namespace School.Infrastructure.Repositories.IRepositories
{
    public interface ISchoolMediumRepository
    {
        IQueryable<SchoolMedium> GetAllQueryable();
        Task<SchoolMedium?> GetByIdAsync(int id);
        Task<int> AddAsync(SchoolMedium entity);
        Task<int> UpdateAsync(SchoolMedium entity);
        Task<int> DeleteAsync(int id);
    }
}


