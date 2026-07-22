using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;

namespace School.Domain.Communication
{
    [Table("PushNotifications", Schema = "Communication")]
    public class PushNotification : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(450)]
        public string RecipientUserId { get; set; } = null!;

        [Required, MaxLength(150)]
        public string Title { get; set; } = null!;

        [Required, MaxLength(1000)]
        public string Body { get; set; } = null!;

        public bool IsRead { get; set; } = false;

        public DateTime SentDate { get; set; } = DateTime.UtcNow;

        public int SchoolRegistrationId { get; set; }
    }
}
