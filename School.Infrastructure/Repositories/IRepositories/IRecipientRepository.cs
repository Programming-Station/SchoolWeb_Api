using School.Domain.Communication.Recipients;
using School.Infrastructure.UnitOfWork.Interfaces;

namespace School.Infrastructure.Repositories.IRepositories
{
    public interface IRecipientRepository : IRepository<Recipient>
    {
        Task<IEnumerable<Recipient>> GetRecipientsByTenantAsync(int tenantId);
        Task<RecipientGroup?> GetGroupWithMembersAsync(int groupId);
    }
}
