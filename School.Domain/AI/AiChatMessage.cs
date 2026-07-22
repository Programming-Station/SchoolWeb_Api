using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;

namespace School.Domain.AI
{
    [Table("AiChatMessages", Schema = "AI")]
    public class AiChatMessage : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int AiChatSessionId { get; set; }

        [ForeignKey(nameof(AiChatSessionId))]
        public virtual AiChatSession AiChatSession { get; set; } = null!;

        [Required, MaxLength(20)]
        public string Sender { get; set; } = null!; // User, AI

        [Required, MaxLength(4000)]
        public string MessageText { get; set; } = null!;

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public int SchoolRegistrationId { get; set; }
    }
}
