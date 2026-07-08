using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using School.Domain.School;
using static School.Domain.BaseEntity;

namespace School.Domain.Student
{
    public class AdmissionRule : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        public int CampusId { get; set; }

        public int EducationLevelId { get; set; }

        public int? ProgramId { get; set; }

        [Required]
        [MaxLength(200)]
        public string RuleName { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string RuleType { get; set; } = string.Empty; // e.g. MinAge, MaxAge, MinPercentage

        [Required]
        [MaxLength(200)]
        public string RuleValue { get; set; } = string.Empty; // value to check against

        [Required]
        [MaxLength(500)]
        public string ErrorMessage { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        [ForeignKey(nameof(CampusId))]
        public virtual Campus Campus { get; set; } = null!;

        [ForeignKey(nameof(EducationLevelId))]
        public virtual EducationLevel EducationLevel { get; set; } = null!;

        [ForeignKey(nameof(ProgramId))]
        public virtual Program? Program { get; set; }

        public int SchoolRegistrationId { get; set; }

        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}
