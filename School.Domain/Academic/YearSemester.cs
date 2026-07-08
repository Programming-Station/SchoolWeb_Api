using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using School.Domain.School;
using static School.Domain.BaseEntity;

namespace School.Domain
{
    public class YearSemester : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty; // e.g. Semester 1, Year 1

        [Required]
        [MaxLength(50)]
        public string Code { get; set; } = string.Empty;

        public int Sequence { get; set; } = 1; // Display/flow sequence (1, 2, 3...)

        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = "active"; // active, inactive

        public int SchoolRegistrationId { get; set; }

        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}
