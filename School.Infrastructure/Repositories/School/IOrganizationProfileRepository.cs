using School.Domain.School;

namespace School.Infrastructure.Repositories.School
{
    public interface IOrganizationProfileRepository
    {
        Task<OrganizationProfile?> GetByTenantIdAsync(int tenantId);
        Task<OrganizationProfile> AddAsync(OrganizationProfile entity);
        Task<int> UpdateAsync(OrganizationProfile entity);
    }
}
