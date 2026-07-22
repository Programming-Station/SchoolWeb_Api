using School.Domain.School;

namespace School.Infrastructure.Repositories.School
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


