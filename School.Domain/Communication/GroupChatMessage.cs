using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;

namespace School.Domain.Communication
{
    [Table("GroupChatMessages", Schema = "Communication")]
    public class GroupChatMessage : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int RoomId { get; set; }

        [ForeignKey(nameof(RoomId))]
        public virtual GroupChatRoom Room { get; set; } = null!;

        [Required, MaxLength(450)]
        public string SenderUserId { get; set; } = null!;

        [Required, MaxLength(4000)]
        public string MessageContent { get; set; } = null!;

        [MaxLength(500)]
        public string? AttachmentPath { get; set; }

        public DateTime SentTime { get; set; } = DateTime.UtcNow;

        public int SchoolRegistrationId { get; set; }
    }
}
