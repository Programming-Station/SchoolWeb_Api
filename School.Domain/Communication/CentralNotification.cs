using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;

namespace School.Domain.Communication
{
    [Table("CentralNotifications", Schema = "Communication")]
    public class CentralNotification : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(450)]
        public string RecipientUserId { get; set; } = null!;

        [Required, MaxLength(250)]
        public string Title { get; set; } = null!;

        [Required, MaxLength(2000)]
        public string Body { get; set; } = null!;

        [Required, MaxLength(50)]
        public string Category { get; set; } = "General"; // Fee, Attendance, Leave, Exam, Homework, Transport, Hostel, Support, Notice, General

        [Required, MaxLength(20)]
        public string Priority { get; set; } = "Medium"; // Low, Medium, High, Urgent

        [MaxLength(500)]
        public string? ActionUrl { get; set; } // Redirect link in Angular app

        public bool IsRead { get; set; } = false;

        public bool IsStarred { get; set; } = false;

        public bool IsArchived { get; set; } = false;

        public DateTime SentDate { get; set; } = DateTime.UtcNow;

        public int SchoolRegistrationId { get; set; }
    }
}
