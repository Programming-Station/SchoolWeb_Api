using School.Domain.Entities;
using School.Infrastructure;
using School.Services.Interfaces;

namespace School.Services
{
    public class AuditService : IAuditService
    {
        private readonly SchoolDbContext _context;
        public AuditService(SchoolDbContext context)
        {
            _context = context;
        }
        public async Task LogAsync(AuditLog auditLog)
        {
            if (auditLog == null) throw new ArgumentNullException(nameof(auditLog));
            await _context.AuditLogs.AddAsync(auditLog);
            await _context.SaveChangesAsync();
        }
    }
}
