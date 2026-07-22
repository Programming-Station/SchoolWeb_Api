using School.Domain.School;

namespace School.Infrastructure.Repositories.School
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


