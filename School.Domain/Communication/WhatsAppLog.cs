using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;

namespace School.Domain.Communication
{
    [Table("WhatsAppLogs", Schema = "Communication")]
    public class WhatsAppLog : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(20)]
        public string RecipientPhone { get; set; } = null!;

        [Required, MaxLength(2000)]
        public string Message { get; set; } = null!;

        [Required, MaxLength(20)]
        public string Status { get; set; } = "Pending"; // Sent, Delivered, Read, Failed

        public DateTime SentDate { get; set; } = DateTime.UtcNow;

        public int SchoolRegistrationId { get; set; }
    }
}
