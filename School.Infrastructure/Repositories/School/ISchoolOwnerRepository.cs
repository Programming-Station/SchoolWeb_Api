using School.Domain.School;
using System.Linq;
using System.Threading.Tasks;

namespace School.Infrastructure.Repositories.School
{
    public interface ISchoolOwnerRepository
    {
        IQueryable<SchoolOwner> GetAllQueryable();
        Task<SchoolOwner?> GetByIdAsync(int id);
        Task<int> AddAsync(SchoolOwner entity);
        Task<int> UpdateAsync(SchoolOwner entity);
        Task<int> DeleteAsync(int id);
    }
}


