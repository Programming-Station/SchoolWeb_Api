using School.Domain.School;

namespace School.Infrastructure.Repositories.School
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


