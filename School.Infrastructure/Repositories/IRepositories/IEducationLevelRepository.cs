using System.Collections.Generic;
using System.Threading.Tasks;
using School.Domain;

namespace School.Infrastructure.Repositories.IRepositories
{
    public interface IEducationLevelRepository
    {
        Task<EducationLevel> AddAsync(EducationLevel entity);
        Task<EducationLevel> GetByIdAsync(int id);
        Task<IEnumerable<EducationLevel>> GetAllAsync(bool? isActive = null);
        Task<int> UpdateAsync(EducationLevel entity);
        Task<int> DeleteAsync(int id);
    }
}
