using School.Domain.Entities;

namespace School.Services.Interfaces
{
    public interface IAuditService
    {
        Task LogAsync(AuditLog auditLog);
    }
}
