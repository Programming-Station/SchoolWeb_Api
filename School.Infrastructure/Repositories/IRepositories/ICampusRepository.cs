using System.Collections.Generic;
using System.Threading.Tasks;
using School.Domain.School;

namespace School.Infrastructure.Repositories.IRepositories
{
    public interface ICampusRepository
    {
        Task<Campus> AddAsync(Campus entity);
        Task<Campus> GetByIdAsync(int id);
        Task<IEnumerable<Campus>> GetAllAsync(bool? isActive = null);
        Task<int> UpdateAsync(Campus entity);
        Task<int> DeleteAsync(int id);
    }
}
