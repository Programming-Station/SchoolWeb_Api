using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;

namespace School.Domain.Communication
{
    [Table("WhatsAppMessages", Schema = "Communication")]
    public class WhatsAppMessage : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(20)]
        public string RecipientPhone { get; set; } = null!;

        [Required, MaxLength(4000)]
        public string MessageText { get; set; } = null!;

        [Required, MaxLength(20)]
        public string MessageType { get; set; } = "Text"; // Text, Template, Document, Image

        [MaxLength(100)]
        public string? MetaMessageId { get; set; }

        [Required, MaxLength(20)]
        public string Status { get; set; } = "Sent"; // Sent, Delivered, Read, Failed

        public DateTime SentDate { get; set; } = DateTime.UtcNow;

        public int SchoolRegistrationId { get; set; }
    }
}
