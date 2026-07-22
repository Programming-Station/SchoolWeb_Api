using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using School.Domain.School;
using static School.Domain.BaseEntity;

namespace School.Domain.Academic
{
#nullable disable
    /// <summary>4.4 — Homework assigned to a class/subject</summary>
    public class Homework : AuditEntity<int>, ITenantEntity
    {
        [Key] public int Id { get; set; }

        [Required, MaxLength(250)] public string Title { get; set; }
        public string Description { get; set; }

        [Required] public int SubjectId { get; set; }
        [ForeignKey(nameof(SubjectId))] public virtual Subject Subject { get; set; }

        [Required] public int ClassId { get; set; }
        [ForeignKey(nameof(ClassId))] public virtual Class Class { get; set; }

        public int? BatchId { get; set; }
        [ForeignKey(nameof(BatchId))] public virtual Batch Batch { get; set; }

        [Required] public DateTime AssignedDate { get; set; } = DateTime.Today;
        [Required] public DateTime DueDate { get; set; }

        /// <summary>File path in wwwroot/uploads/homework/</summary>
        [MaxLength(500)] public string AttachmentPath { get; set; }

        [MaxLength(450)] public string AssignedByUserId { get; set; }
        [MaxLength(200)] public string AssignedByName { get; set; }

        /// <summary>Active / Cancelled</summary>
        [MaxLength(20)] public string Status { get; set; } = "Active";

        [Required] public int SchoolRegistrationId { get; set; }
        [ForeignKey(nameof(SchoolRegistrationId))] public virtual SchoolRegistration SchoolRegistration { get; set; }
    }

    /// <summary>4.4 — Student's homework submission</summary>
    public class HomeworkSubmission : AuditEntity<int>, ITenantEntity
    {
        [Key] public int Id { get; set; }

        [Required] public int HomeworkId { get; set; }
        [ForeignKey(nameof(HomeworkId))] public virtual Homework Homework { get; set; }

        [Required] public int StudentId { get; set; }
        [ForeignKey(nameof(StudentId))] public virtual Student.Student Student { get; set; }

        public DateTime? SubmittedDate { get; set; }
        [MaxLength(500)] public string FilePath { get; set; }
        [MaxLength(500)] public string StudentRemarks { get; set; }

        /// <summary>Submitted / Late / Missing / Graded</summary>
        [MaxLength(20)] public string Status { get; set; } = "Missing";

        [MaxLength(10)] public string Grade { get; set; }
        [MaxLength(500)] public string TeacherFeedback { get; set; }

        [Required] public int SchoolRegistrationId { get; set; }
        [ForeignKey(nameof(SchoolRegistrationId))] public virtual SchoolRegistration SchoolRegistration { get; set; }
    }
}
