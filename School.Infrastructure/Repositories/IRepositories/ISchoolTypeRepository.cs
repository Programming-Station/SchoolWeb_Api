using School.Domain.School;
using System.Linq;
using System.Threading.Tasks;

namespace School.Infrastructure.Repositories.IRepositories
{
    public interface ISchoolTypeRepository
    {
        IQueryable<SchoolType> GetAllQueryable();
        Task<SchoolType?> GetByIdAsync(int id);
        Task<int> AddAsync(SchoolType entity);
        Task<int> UpdateAsync(SchoolType entity);
        Task<int> DeleteAsync(int id);
    }
}


