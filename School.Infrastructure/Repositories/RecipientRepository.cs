using Microsoft.EntityFrameworkCore;
using School.Domain.Communication.Recipients;
using School.Infrastructure.Repositories.IRepositories;
using School.Infrastructure.UnitOfWork;

namespace School.Infrastructure.Repositories
{
    public class RecipientRepository : Repository<Recipient>, IRecipientRepository
    {
        private readonly SchoolDbContext _context;

        public RecipientRepository(DbFactory dbFactory, SchoolDbContext context) : base(dbFactory)
        {
            _context = context;
        }

        public async Task<IEnumerable<Recipient>> GetRecipientsByTenantAsync(int tenantId)
        {
            return await _context.Recipients
                .Include(r => r.Tags)
                .Include(r => r.Preferences)
                .Where(r => r.SchoolRegistrationId == tenantId && !r.IsDeleted)
                .ToListAsync();
        }

        public async Task<RecipientGroup?> GetGroupWithMembersAsync(int groupId)
        {
            return await _context.RecipientGroups
                .Include(g => g.Members)
                .ThenInclude(m => m.Recipient)
                .FirstOrDefaultAsync(g => g.Id == groupId);
        }
    }
}
