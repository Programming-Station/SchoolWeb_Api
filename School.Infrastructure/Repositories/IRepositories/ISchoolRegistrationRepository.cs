using School.Domain.Website;
using School.Infrastructure.UnitOfWork.Interfaces;

namespace School.Infrastructure.Repositories.IRepositories
{
    public interface ISchoolRegistrationRepository : IRepository<SchoolRegistration>
    {
        Task<SchoolRegistration> AddSchoolRegistrationAsync(SchoolRegistration entity);
        Task<SchoolRegistration> GetSchoolRegistrationByIdAsync(int id);
        Task<IEnumerable<SchoolRegistration>> GetAllAsync(string? status = null, int? pageNumber = null, int? pageSize = null);
        Task<int> GetTotalCountAsync(string? status = null);
        Task<int> UpdateSchoolRegistrationAsync(SchoolRegistration entity);
        Task<int> UpdateSchoolRegistrationStatusAsync(int id, int statusId, string? approvedBy = null, string? rejectionReason = null);
        Task<int> DeleteSchoolRegistrationAsync(int id);
        Task<bool> ExistsByEmailAsync(string email, int? excludeId = null);
        Task<bool> ExistsByMobileAsync(string mobile, int? excludeId = null);
        Task<SchoolRegistration> GetByEmailAsync(string email);
        Task<SchoolRegistration> GetByMobileAsync(string mobile);
        Task<string> GenerateRegistrationNumberAsync();
    }
}
