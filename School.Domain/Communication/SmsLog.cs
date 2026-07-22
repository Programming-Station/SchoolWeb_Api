using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;

namespace School.Domain.Communication
{
    [Table("SmsLogs", Schema = "Communication")]
    public class SmsLog : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(20)]
        public string RecipientNo { get; set; } = null!;

        [Required, MaxLength(1000)]
        public string Message { get; set; } = null!;

        [Required, MaxLength(20)]
        public string SentStatus { get; set; } = "Pending"; // Sent, Failed, Pending

        public DateTime SentDate { get; set; } = DateTime.UtcNow;

        [MaxLength(1000)]
        public string? ProviderResponse { get; set; }

        public int SchoolRegistrationId { get; set; }
    }
}
