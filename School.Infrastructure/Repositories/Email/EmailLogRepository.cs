using Microsoft.EntityFrameworkCore;
using School.Domain.Email;
using School.Infrastructure.Repositories.IRepositories;
using School.Infrastructure.UnitOfWork;
using School.Infrastructure.UnitOfWork.Interfaces;

namespace School.Infrastructure.Repositories.Email
{
    public class EmailLogRepository : Repository<EmailLog>, IEmailLogRepository
    {
        private readonly SchoolDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public EmailLogRepository(DbFactory dbFactory, SchoolDbContext context, IUnitOfWork unitOfWork)
            : base(dbFactory)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public async Task<EmailLog?> GetByIdAsync(int id)
            => await FindAsync(x => x.Id == id && !x.IsDeleted);

        public async Task<IEnumerable<EmailLog>> GetAllBySchoolIdAsync(int schoolId, int page = 1, int pageSize = 20)
            => await List(x => !x.IsDeleted && x.SchoolRegistrationId == schoolId)
                     .OrderByDescending(x => x.CreatedDate)
                     .Skip((page - 1) * pageSize)
                     .Take(pageSize)
                     .ToListAsync();

        public async Task<int> GetTotalCountAsync(int schoolId)
            => await DbSet.CountAsync(x => !x.IsDeleted && x.SchoolRegistrationId == schoolId);

        public async Task<int> DeleteAsync(int id)
        {
            var entity = await FindAsync(x => x.Id == id);
            if (entity == null) return 0;
            Delete(entity);
            return await _unitOfWork.CommitAsync();
        }

        public async Task<int> DeleteOldLogsAsync(int schoolId, int daysOlderThan = 90)
        {
            var cutoff = DateTime.UtcNow.AddDays(-daysOlderThan);
            var oldLogs = await List(x =>
                x.SchoolRegistrationId == schoolId &&
                x.CreatedDate < cutoff).ToListAsync();

            if (!oldLogs.Any()) return 0;

            foreach (var log in oldLogs)
                Delete(log);

            return await _unitOfWork.CommitAsync();
        }
    }
}
