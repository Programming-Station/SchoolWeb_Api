using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using School.Domain.Hr;
using School.Domain.School;
using static School.Domain.BaseEntity;

namespace School.Domain.Payroll
{
    [Table("SalaryArrears", Schema = "Payroll")]
    public class SalaryArrear : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        public int EmployeeId { get; set; }
        [ForeignKey(nameof(EmployeeId))]
        public virtual Employee Employee { get; set; } = null!;

        [Required, Column(TypeName = "decimal(10,2)")]
        public decimal Amount { get; set; }

        [Required, MaxLength(20)]
        public string ArrearMonth { get; set; } = null!; // e.g. "2026-01"

        [MaxLength(20)]
        public string? PaidInMonth { get; set; } // e.g. "2026-02"

        [MaxLength(500)]
        public string? Reason { get; set; }

        [Required, MaxLength(20)]
        public string Status { get; set; } = "Pending"; // Pending, Paid, Cancelled

        [Required]
        public int SchoolRegistrationId { get; set; }

        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}
