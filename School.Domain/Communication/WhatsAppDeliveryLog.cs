using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;

namespace School.Domain.Communication
{
    [Table("WhatsAppDeliveryLogs", Schema = "Communication")]
    public class WhatsAppDeliveryLog : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string MetaMessageId { get; set; } = null!;

        [Required, MaxLength(20)]
        public string RecipientPhone { get; set; } = null!;

        [Required, MaxLength(20)]
        public string Status { get; set; } = null!; // Delivered, Read, Failed

        public DateTime StatusTimestamp { get; set; } = DateTime.UtcNow;

        [MaxLength(1000)]
        public string? ErrorMessage { get; set; }

        public int SchoolRegistrationId { get; set; }
    }
}
