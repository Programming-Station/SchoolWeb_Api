using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using School.Domain.School;
using static School.Domain.BaseEntity;

namespace School.Domain
{
    public class Program : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Code { get; set; } = string.Empty;

        [Required]
        public int EducationLevelId { get; set; }

        public int? FacultyId { get; set; }

        public int? DepartmentId { get; set; }

        public int DurationYears { get; set; } = 1;

        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = "active"; // active, inactive

        [ForeignKey(nameof(EducationLevelId))]
        public virtual EducationLevel EducationLevel { get; set; } = null!;

        [ForeignKey(nameof(FacultyId))]
        public virtual Faculty? Faculty { get; set; }

        [ForeignKey(nameof(DepartmentId))]
        public virtual Department? Department { get; set; }

        public int SchoolRegistrationId { get; set; }

        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}
