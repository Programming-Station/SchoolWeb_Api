using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using School.Domain.Hr;
using School.Domain.School;
using static School.Domain.BaseEntity;

namespace School.Domain.Payroll
{
    [Table("EmployeeLoans", Schema = "Payroll")]
    public class EmployeeLoan : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        public int EmployeeId { get; set; }
        [ForeignKey(nameof(EmployeeId))]
        public virtual Employee Employee { get; set; } = null!;

        [Required, Column(TypeName = "decimal(10,2)")]
        public decimal PrincipalAmount { get; set; }

        [Required, Column(TypeName = "decimal(5,2)")]
        public decimal InterestRate { get; set; } = 0; // percentage

        [Required]
        public int TotalInstallments { get; set; } = 12;

        [Required, Column(TypeName = "decimal(10,2)")]
        public decimal MonthlyInstallment { get; set; }

        [Required, Column(TypeName = "decimal(10,2)")]
        public decimal BalanceAmount { get; set; }

        [MaxLength(200)]
        public string? Purpose { get; set; }

        [Required, MaxLength(20)]
        public string Status { get; set; } = "Pending"; // Pending, Approved, Rejected, Running, Completed, Terminated

        [MaxLength(450)]
        public string? ApprovedBy { get; set; }

        public DateTime? ApprovedDate { get; set; }

        [Required]
        public int SchoolRegistrationId { get; set; }

        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}
