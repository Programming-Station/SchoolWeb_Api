using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;

namespace School.Domain.Communication
{
    [Table("WhatsAppConversations", Schema = "Communication")]
    public class WhatsAppConversation : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(20)]
        public string RecipientPhone { get; set; } = null!;

        [Required, MaxLength(100)]
        public string MetaConversationId { get; set; } = null!;

        public DateTime StartedDate { get; set; } = DateTime.UtcNow;

        public DateTime? ExpiresDate { get; set; }

        [Required, MaxLength(50)]
        public string Category { get; set; } = "Service"; // Utility, Authentication, Marketing, Service

        public int SchoolRegistrationId { get; set; }
    }
}
