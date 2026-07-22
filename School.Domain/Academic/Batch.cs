using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using School.Domain.School;
using static School.Domain.BaseEntity;

namespace School.Domain
{
    public class Batch : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty; // e.g. Batch 2024-2028

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        [Required]
        public int AcademicYearId { get; set; }

        [Required]
        public int ProgramId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = "active"; // active, inactive

        [ForeignKey(nameof(AcademicYearId))]
        public virtual AcademicYear AcademicYear { get; set; } = null!;

        [ForeignKey(nameof(ProgramId))]
        public virtual Program Program { get; set; } = null!;

        public int SchoolRegistrationId { get; set; }

        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}
