using School.Domain.School;

namespace School.Infrastructure.Repositories.School
{
    public interface ISchoolRepository
    {
        Task<IEnumerable<SchoolRegistration>> GetAllSchoolsAsync();
        IQueryable<SchoolRegistration> GetAllSchoolsQueryable();

        Task<SchoolRegistration?> GetSchoolByIdAsync(int id);

        Task<SchoolRegistration> AddSchoolAsync(SchoolRegistration entity);

        Task<int> UpdateSchoolAsync(SchoolRegistration entity);

        Task<int> DeleteSchoolAsync(int id);
    }
}
