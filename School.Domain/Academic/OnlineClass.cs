using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using School.Domain.School;
using School.Domain.Hr;
using static School.Domain.BaseEntity;

namespace School.Domain.Academic
{
    #nullable disable
    /// <summary>4.6 — Online Class / Live Session</summary>
    public class OnlineClass : AuditEntity<int>, ITenantEntity
    {
        [Key] public int Id { get; set; }

        [Required, MaxLength(250)] public string Title { get; set; }
        [MaxLength(1000)] public string Description { get; set; }

        [Required] public int SubjectId { get; set; }
        [ForeignKey(nameof(SubjectId))] public virtual Subject Subject { get; set; }

        [Required] public int ClassId { get; set; }
        [ForeignKey(nameof(ClassId))] public virtual Class Class { get; set; }

        public int? BatchId { get; set; }
        [ForeignKey(nameof(BatchId))] public virtual Batch Batch { get; set; }

        public int? TeacherId { get; set; }
        [ForeignKey(nameof(TeacherId))] public virtual Employee Teacher { get; set; }

        [Required] public DateTime ScheduledAt { get; set; }
        public int DurationMinutes { get; set; } = 60;

        /// <summary>Zoom / Google Meet / Teams / Custom</summary>
        [MaxLength(50)] public string Platform { get; set; } = "Zoom";
        [MaxLength(500)] public string MeetingLink { get; set; }
        [MaxLength(100)] public string MeetingId { get; set; }
        [MaxLength(100)] public string MeetingPassword { get; set; }

        /// <summary>Scheduled / Live / Completed / Cancelled</summary>
        [MaxLength(20)] public string Status { get; set; } = "Scheduled";

        /// <summary>Recording link if saved after session</summary>
        [MaxLength(500)] public string RecordingLink { get; set; }

        [Required] public int SchoolRegistrationId { get; set; }
        [ForeignKey(nameof(SchoolRegistrationId))] public virtual SchoolRegistration SchoolRegistration { get; set; }
    }
}
