using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;

namespace School.Domain.Communication
{
    [Table("WhatsAppQueues", Schema = "Communication")]
    public class WhatsAppQueue : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(20)]
        public string RecipientPhone { get; set; } = null!;

        [Required, MaxLength(4000)]
        public string MessagePayload { get; set; } = null!; // JSON body or raw text

        public DateTime ScheduledTime { get; set; } = DateTime.UtcNow;

        public int RetryCount { get; set; } = 0;

        [Required, MaxLength(20)]
        public string Status { get; set; } = "Pending"; // Pending, Processing, Sent, Failed, DeadLetter

        public int SchoolRegistrationId { get; set; }
    }
}
