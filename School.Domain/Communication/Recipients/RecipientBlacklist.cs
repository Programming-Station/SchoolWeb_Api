using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace School.Domain.Communication.Recipients
{
    [Table("RecipientBlacklists", Schema = "Communication")]
    public class RecipientBlacklist : BaseEntity.AuditEntity<int>, BaseEntity.ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int SchoolRegistrationId { get; set; }

        public int? RecipientId { get; set; }
        [ForeignKey("RecipientId")]
        public virtual Recipient? Recipient { get; set; }

        [Required, StringLength(100)]
        public string BlockedAddress { get; set; } = null!; // Email or Phone No

        [Required, StringLength(50)]
        public string Channel { get; set; } = null!; // Email, SMS, WhatsApp

        [StringLength(500)]
        public string? Reason { get; set; } // Bounce, Spam Report, OptOut

        public DateTime BlockedAt { get; set; } = DateTime.UtcNow;
    }
}
