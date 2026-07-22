using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using School.Domain.School;
using static School.Domain.BaseEntity;

namespace School.Domain.Academic
{
    public class ExamResult : AuditEntity<int>, ITenantEntity
    {
        [Key] public int Id { get; set; }
        public int ExamId { get; set; }
        [ForeignKey(nameof(ExamId))] public virtual Exam Exam { get; set; } = null!;
        public int StudentId { get; set; }
        [ForeignKey(nameof(StudentId))]
        public virtual Student.Student Student { get; set; } = null!;
        public int SubjectId { get; set; }
        [ForeignKey(nameof(SubjectId))]
        public virtual Subject Subject { get; set; } = null!;
        [Column(TypeName = "decimal(5, 2)")]
        public decimal MarksObtained { get; set; }
        [Column(TypeName = "decimal(5,2)")]
        public decimal TotalMarks { get; set; }
        [MaxLength(5)] public string? Grade { get; set; }
        [MaxLength(20)] public string Status { get; set; } = "Pass";
        public int SchoolRegistrationId { get; set; }
        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}

