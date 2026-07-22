using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using School.Domain.Hr;
using School.Domain.School;
using static School.Domain.BaseEntity;

namespace School.Domain.Academic
{
#nullable disable
    /// <summary>4.3 — Timetable period slot</summary>
    public class TimetablePeriod : AuditEntity<int>, ITenantEntity
    {
        [Key] public int Id { get; set; }

        [Required] public int ClassId { get; set; }
        [ForeignKey(nameof(ClassId))] public virtual Class Class { get; set; }

        [Required] public int SubjectId { get; set; }
        [ForeignKey(nameof(SubjectId))] public virtual Subject Subject { get; set; }

        public int? TeacherId { get; set; }
        [ForeignKey(nameof(TeacherId))] public virtual Employee Teacher { get; set; }

        /// <summary>Monday=1 … Saturday=6</summary>
        [Required] public int DayOfWeek { get; set; }

        [Required] public int PeriodNo { get; set; }
        [Required, MaxLength(10)] public string StartTime { get; set; } = "08:00";
        [Required, MaxLength(10)] public string EndTime { get; set; } = "09:00";

        [MaxLength(100)] public string RoomNo { get; set; }

        [Required] public int AcademicYearId { get; set; }
        [ForeignKey(nameof(AcademicYearId))] public virtual AcademicYear AcademicYear { get; set; }

        [Required] public int SchoolRegistrationId { get; set; }
        [ForeignKey(nameof(SchoolRegistrationId))] public virtual SchoolRegistration SchoolRegistration { get; set; }
    }
}
