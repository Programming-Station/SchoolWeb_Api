using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;

namespace School.Domain.Administration
{
    [Table("AdminAuditLogs", Schema = "Administration")]
    public class AdminAuditLog : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(450)]
        public string UserId { get; set; } = null!;

        [Required, MaxLength(100)]
        public string TableName { get; set; } = null!;

        [Required, MaxLength(20)]
        public string Action { get; set; } = null!; // Insert, Update, Delete

        [MaxLength(8000)]
        public string? OldValuesJson { get; set; }

        [MaxLength(8000)]
        public string? NewValuesJson { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public int SchoolRegistrationId { get; set; }
    }
}
