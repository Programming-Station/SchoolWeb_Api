using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;

namespace School.Domain.AI
{
    [Table("AiChatSessions", Schema = "AI")]
    public class AiChatSession : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public Guid SessionId { get; set; }

        [Required, MaxLength(450)]
        public string UserId { get; set; } = null!;

        public DateTime StartedAt { get; set; } = DateTime.UtcNow;

        public int SchoolRegistrationId { get; set; }

        public virtual ICollection<AiChatMessage> Messages { get; set; } = new List<AiChatMessage>();
    }
}
