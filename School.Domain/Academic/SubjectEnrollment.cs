using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using School.Domain.School;
using static School.Domain.BaseEntity;

namespace School.Domain.Academic
{
#nullable disable
    public class SubjectEnrollment : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int StudentId { get; set; }
        [ForeignKey(nameof(StudentId))]
        public virtual Student.Student Student { get; set; }

        [Required]
        public int SubjectId { get; set; }
        [ForeignKey(nameof(SubjectId))]
        public virtual Subject Subject { get; set; }

        public int? BatchId { get; set; }
        [ForeignKey(nameof(BatchId))]
        public virtual Batch Batch { get; set; }

        public int? YearSemesterId { get; set; }
        [ForeignKey(nameof(YearSemesterId))]
        public virtual YearSemester YearSemester { get; set; }

        public int? ClassId { get; set; }
        [ForeignKey(nameof(ClassId))]
        public virtual Class Class { get; set; }

        /// <summary>Enrolled / Dropped / Completed</summary>
        [Required, MaxLength(20)]
        public string Status { get; set; } = "Enrolled";

        public DateTime EnrolledDate { get; set; } = DateTime.Now;

        public DateTime? DroppedDate { get; set; }

        [Required]
        public int SchoolRegistrationId { get; set; }
        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; }
    }
}
