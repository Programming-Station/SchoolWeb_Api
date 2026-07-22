using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace School.Domain.Communication.Recipients
{
    [Table("RecipientPreferences", Schema = "Communication")]
    public class RecipientPreference : BaseEntity.AuditEntity<int>, BaseEntity.ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int SchoolRegistrationId { get; set; }

        public int RecipientId { get; set; }
        [ForeignKey("RecipientId")]
        public virtual Recipient Recipient { get; set; } = null!;

        [Required, StringLength(50)]
        public string ChannelType { get; set; } = null!; // Email, SMS, WhatsApp, Push

        [Required, StringLength(50)]
        public string MessageCategory { get; set; } = null!; // Circular, Alert, Marketing

        public bool IsOptIn { get; set; } = true;
    }
}

