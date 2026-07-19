using School.Domain.Communication.Recipients;
using School.Infrastructure.UnitOfWork.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace School.Infrastructure.Repositories.IRepositories
{
    public interface IRecipientRepository : IRepository<Recipient>
    {
        Task<IEnumerable<Recipient>> GetRecipientsByTenantAsync(int tenantId);
        Task<RecipientGroup?> GetGroupWithMembersAsync(int groupId);
    }
}
