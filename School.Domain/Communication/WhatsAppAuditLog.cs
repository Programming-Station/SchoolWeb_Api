using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;

namespace School.Domain.Communication
{
    [Table("WhatsAppAuditLogs", Schema = "Communication")]
    public class WhatsAppAuditLog : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Action { get; set; } = null!;

        [Required, MaxLength(100)]
        public string PerformedBy { get; set; } = null!;

        public DateTime ActionTimestamp { get; set; } = DateTime.UtcNow;

        [MaxLength(50)]
        public string? IPAddress { get; set; }

        [MaxLength(2000)]
        public string? Details { get; set; }

        public int SchoolRegistrationId { get; set; }
    }
}
