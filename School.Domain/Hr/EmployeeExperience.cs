using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;
using School.Domain.School;

namespace School.Domain.Hr
{
    public class EmployeeExperience : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        public int EmployeeId { get; set; }
        [ForeignKey(nameof(EmployeeId))]
        public virtual Employee Employee { get; set; } = null!;

        [Required, MaxLength(200)]
        public string Company { get; set; } = null!;

        [Required, MaxLength(100)]
        public string Designation { get; set; } = null!;

        public DateTime JoiningDate { get; set; }

        public DateTime LeavingDate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? Salary { get; set; }

        [MaxLength(500)]
        public string? ReasonForLeaving { get; set; }

        public int SchoolRegistrationId { get; set; }
        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}
