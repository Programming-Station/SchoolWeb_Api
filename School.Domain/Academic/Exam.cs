using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;
using School.Domain.School;

namespace School.Domain.Academic
{
    public class Exam : AuditEntity<int>, ITenantEntity
    {
        [Key] public int Id { get; set; }
        [Required, MaxLength(150)] public string Name { get; set; } = null!;
        [MaxLength(50)] public string? ExamType { get; set; }     // Midterm, Final, Quiz
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        [MaxLength(20)] public string Status { get; set; } = "Scheduled";
        [MaxLength(500)] public string? Description { get; set; }
        public int SchoolRegistrationId { get; set; }
        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
        public virtual ICollection<ExamResult> Results { get; set; } = new List<ExamResult>();
    }
}

