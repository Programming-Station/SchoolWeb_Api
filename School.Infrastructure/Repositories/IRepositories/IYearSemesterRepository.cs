using System.Collections.Generic;
using System.Threading.Tasks;
using School.Domain;

namespace School.Infrastructure.Repositories.IRepositories
{
    public interface IYearSemesterRepository
    {
        Task<YearSemester> AddAsync(YearSemester entity);
        Task<YearSemester> GetByIdAsync(int id);
        Task<IEnumerable<YearSemester>> GetAllAsync(bool? isActive = null);
        Task<int> UpdateAsync(YearSemester entity);
        Task<int> DeleteAsync(int id);
    }
}
