using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;

namespace School.Domain.Communication
{
    [Table("GroupChatMembers", Schema = "Communication")]
    public class GroupChatMember : DeleteEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int RoomId { get; set; }

        [ForeignKey(nameof(RoomId))]
        public virtual GroupChatRoom Room { get; set; } = null!;

        [Required, MaxLength(450)]
        public string UserId { get; set; } = null!;

        public bool IsAdmin { get; set; } = false;

        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;

        public int SchoolRegistrationId { get; set; }
    }
}
