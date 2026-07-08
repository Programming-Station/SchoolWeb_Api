using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using School.Domain.School;
using static School.Domain.BaseEntity;

namespace School.Domain.Academic
{
    #nullable disable
    /// <summary>4.5 — Assignment (graded project/task)</summary>
    public class Assignment : AuditEntity<int>, ITenantEntity
    {
        [Key] public int Id { get; set; }

        [Required, MaxLength(250)] public string Title { get; set; }
        public string Instructions { get; set; }

        [Required] public int SubjectId { get; set; }
        [ForeignKey(nameof(SubjectId))] public virtual Subject Subject { get; set; }

        [Required] public int ClassId { get; set; }
        [ForeignKey(nameof(ClassId))] public virtual Class Class { get; set; }

        public int? BatchId { get; set; }
        [ForeignKey(nameof(BatchId))] public virtual Batch Batch { get; set; }

        [Required] public DateTime StartDate { get; set; }
        [Required] public DateTime EndDate   { get; set; }

        [Column(TypeName = "decimal(5,2)")] public decimal MaxMarks { get; set; } = 100;

        [MaxLength(500)] public string AttachmentPath { get; set; }
        [MaxLength(450)] public string CreatedByUserId { get; set; }

        /// <summary>Draft / Published / Closed</summary>
        [MaxLength(20)] public string Status { get; set; } = "Published";

        [Required] public int SchoolRegistrationId { get; set; }
        [ForeignKey(nameof(SchoolRegistrationId))] public virtual SchoolRegistration SchoolRegistration { get; set; }
    }

    /// <summary>4.5 — Student Assignment submission with marks</summary>
    public class AssignmentSubmission : AuditEntity<int>, ITenantEntity
    {
        [Key] public int Id { get; set; }

        [Required] public int AssignmentId { get; set; }
        [ForeignKey(nameof(AssignmentId))] public virtual Assignment Assignment { get; set; }

        [Required] public int StudentId { get; set; }
        [ForeignKey(nameof(StudentId))] public virtual Student.Student Student { get; set; }

        public DateTime? SubmittedDate { get; set; }
        [MaxLength(500)] public string FilePath { get; set; }
        [MaxLength(500)] public string StudentRemarks { get; set; }

        /// <summary>Pending / Submitted / Late / Graded</summary>
        [MaxLength(20)] public string Status { get; set; } = "Pending";

        [Column(TypeName = "decimal(5,2)")] public decimal? MarksObtained { get; set; }
        [MaxLength(500)] public string TeacherFeedback { get; set; }

        [Required] public int SchoolRegistrationId { get; set; }
        [ForeignKey(nameof(SchoolRegistrationId))] public virtual SchoolRegistration SchoolRegistration { get; set; }
    }
}
