using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using School.Domain.Hr;
using School.Domain.School;
using static School.Domain.BaseEntity;

namespace School.Domain.Payroll
{
    [Table("SalaryAdvances", Schema = "Payroll")]
    public class SalaryAdvance : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        public int EmployeeId { get; set; }
        [ForeignKey(nameof(EmployeeId))]
        public virtual Employee Employee { get; set; } = null!;

        [Required, Column(TypeName = "decimal(10,2)")]
        public decimal AdvanceAmount { get; set; }

        [Required]
        public DateTime RequestDate { get; set; } = DateTime.UtcNow;

        [Required, MaxLength(20)]
        public string Status { get; set; } = "Pending"; // Pending, Approved, Rejected, Recovered

        [MaxLength(450)]
        public string? ApprovedBy { get; set; }

        public DateTime? ApprovedDate { get; set; }

        [MaxLength(20)]
        public string? TargetRecoveryMonth { get; set; } // e.g. "2026-02"

        [Required]
        public int SchoolRegistrationId { get; set; }

        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}
