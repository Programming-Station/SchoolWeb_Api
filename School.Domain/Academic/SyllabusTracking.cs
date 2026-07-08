using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using School.Domain.School;
using static School.Domain.BaseEntity;

namespace School.Domain.Academic
{
    #nullable disable
    /// <summary>4.7 — Syllabus chapter/unit definition per subject</summary>
    public class SyllabusChapter : AuditEntity<int>, ITenantEntity
    {
        [Key] public int Id { get; set; }

        [Required] public int SubjectId { get; set; }
        [ForeignKey(nameof(SubjectId))] public virtual Subject Subject { get; set; }

        public int? ClassId { get; set; }
        [ForeignKey(nameof(ClassId))] public virtual Class Class { get; set; }

        public int? AcademicYearId { get; set; }
        [ForeignKey(nameof(AcademicYearId))] public virtual AcademicYear AcademicYear { get; set; }

        [Required] public int ChapterNo { get; set; }
        [Required, MaxLength(250)] public string ChapterName { get; set; }
        [MaxLength(1000)] public string Description { get; set; }

        public int? TotalPeriods { get; set; }       // Planned periods for this chapter
        public int? CompletedPeriods { get; set; } = 0;

        /// <summary>NotStarted / InProgress / Completed</summary>
        [MaxLength(20)] public string Status { get; set; } = "NotStarted";

        public DateTime? StartedDate { get; set; }
        public DateTime? CompletedDate { get; set; }

        [MaxLength(450)] public string UpdatedByUserId { get; set; }

        [Required] public int SchoolRegistrationId { get; set; }
        [ForeignKey(nameof(SchoolRegistrationId))] public virtual SchoolRegistration SchoolRegistration { get; set; }
    }

    /// <summary>4.7 — Lesson Plan: daily planned topic within a chapter</summary>
    public class LessonPlan : AuditEntity<int>, ITenantEntity
    {
        [Key] public int Id { get; set; }

        [Required] public int SyllabusChapterId { get; set; }
        [ForeignKey(nameof(SyllabusChapterId))] public virtual SyllabusChapter SyllabusChapter { get; set; }

        [Required] public int SubjectId { get; set; }
        [ForeignKey(nameof(SubjectId))] public virtual Subject Subject { get; set; }

        [Required] public int ClassId { get; set; }
        [ForeignKey(nameof(ClassId))] public virtual Class Class { get; set; }

        [Required] public DateTime PlannedDate { get; set; }
        [Required, MaxLength(250)] public string Topic { get; set; }
        [MaxLength(2000)] public string Objectives { get; set; }
        [MaxLength(2000)] public string TeachingMethod { get; set; }
        [MaxLength(500)] public string MaterialsRequired { get; set; }
        [MaxLength(500)] public string AttachmentPath { get; set; }

        /// <summary>Planned / Completed / Skipped</summary>
        [MaxLength(20)] public string Status { get; set; } = "Planned";

        [MaxLength(1000)] public string TeacherNotes { get; set; }

        [Required] public int SchoolRegistrationId { get; set; }
        [ForeignKey(nameof(SchoolRegistrationId))] public virtual SchoolRegistration SchoolRegistration { get; set; }
    }
}
