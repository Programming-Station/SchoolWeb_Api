using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using School.Domain.School;
using static School.Domain.BaseEntity;

namespace School.Domain.Hr
{
    public class EmployeeEducation : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        public int EmployeeId { get; set; }
        [ForeignKey(nameof(EmployeeId))]
        public virtual Employee Employee { get; set; } = null!;

        [Required, MaxLength(200)]
        public string Degree { get; set; } = null!;

        [MaxLength(200)]
        public string? Board { get; set; }

        [Required, MaxLength(200)]
        public string University { get; set; } = null!;

        [MaxLength(20)]
        public string? PassingYear { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal? Percentage { get; set; }

        [MaxLength(500)]
        public string? Certificates { get; set; } // Path to files if any

        public int SchoolRegistrationId { get; set; }
        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}
