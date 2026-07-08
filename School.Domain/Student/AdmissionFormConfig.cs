using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using School.Domain.School;
using static School.Domain.BaseEntity;

namespace School.Domain.Student
{
    public class AdmissionFormConfig : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CampusId { get; set; }

        [Required]
        public int EducationLevelId { get; set; }

        public int? ProgramId { get; set; }

        [Required]
        public string FormStepsJson { get; set; } = string.Empty; // Array of steps (Personal, Parent, Address, Academic, PrevEducation, Documents, Fees, etc.)

        [Required]
        public string DocumentChecklistJson { get; set; } = string.Empty; // Array of required/optional docs configurations

        public string? CustomFieldsJson { get; set; } // Custom field inputs configuration

        [Required]
        public string AutoGenRulesJson { get; set; } = string.Empty; // Config rules for Admission No, Roll No, Student Code, Login Usernames

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
