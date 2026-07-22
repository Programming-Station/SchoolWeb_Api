using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;

namespace School.Domain.Communication
{
    [Table("CommunicationMeetings", Schema = "Communication")]
    public class CommunicationMeeting : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(250)]
        public string Title { get; set; } = null!;

        [MaxLength(1000)]
        public string? Description { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        [Required, MaxLength(50)]
        public string Platform { get; set; } = "Zoom"; // Zoom, GoogleMeet, Teams, Physical

        [MaxLength(1000)]
        public string? MeetingLink { get; set; }

        [MaxLength(100)]
        public string? MeetingId { get; set; }

        [MaxLength(100)]
        public string? MeetingPassword { get; set; }

        [MaxLength(2000)]
        public string? Agenda { get; set; }

        [MaxLength(8000)]
        public string? MinutesOfMeeting { get; set; }

        [MaxLength(1000)]
        public string? RecordingLink { get; set; }

        [Required, MaxLength(50)]
        public string Status { get; set; } = "Scheduled"; // Scheduled, Live, Completed, Cancelled

        [Required, MaxLength(50)]
        public string TargetAudience { get; set; } = "All"; // All, Students, Parents, Employees

        public int SchoolRegistrationId { get; set; }
    }
}
