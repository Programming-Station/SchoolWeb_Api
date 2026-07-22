using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;

namespace School.Domain.Communication
{
    [Table("ParentTeacherChats", Schema = "Communication")]
    public class ParentTeacherChat : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(450)]
        public string SenderUserId { get; set; } = null!;

        [Required, MaxLength(450)]
        public string ReceiverUserId { get; set; } = null!;

        [Required, MaxLength(2000)]
        public string MessageContent { get; set; } = null!;

        public DateTime SentTime { get; set; } = DateTime.UtcNow;

        public bool IsRead { get; set; } = false;

        public int SchoolRegistrationId { get; set; }
    }
}
