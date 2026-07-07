using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;
using School.Domain.School;

namespace School.Domain.Academic
{
    public class TimetableSlot : AuditEntity<int>, ITenantEntity
    {
        [Key] public int Id { get; set; }
        public int SubjectId { get; set; }
        [ForeignKey(nameof(SubjectId))] public virtual Subject Subject { get; set; } = null!;
        [MaxLength(20)] public string DayOfWeek { get; set; } = null!; // Monday, Tuesday …
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        [MaxLength(50)] public string? Room { get; set; }
        [MaxLength(150)] public string? TeacherName { get; set; }
        [MaxLength(20)] public string Status { get; set; } = "active";
        public int SchoolRegistrationId { get; set; }
        [ForeignKey(nameof(SchoolRegistrationId))] public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}

