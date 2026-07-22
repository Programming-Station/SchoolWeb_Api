using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using School.Domain.School;
using static School.Domain.BaseEntity;

namespace School.Domain.Student
{
    public class EducationalDetail : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }
        public int? StudentRegistrationId { get; set; }
        public string ExamName { get; set; } = null!;
        public string? PassingYear { get; set; }
        public string? InstituteName { get; set; }
        public string? InstituteAddress { get; set; }
        public decimal? TotalMarks { get; set; }
        public decimal? ObtainedMarks { get; set; }
        public string? Certificate { get; set; }

        public int SchoolRegistrationId { get; set; }
        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}



