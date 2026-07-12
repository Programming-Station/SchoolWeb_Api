using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;
using School.Domain.School;

namespace School.Domain.Payroll
{
    [Table("LoanRepaymentSchedules", Schema = "Payroll")]
    public class LoanRepaymentSchedule : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        public int EmployeeLoanId { get; set; }
        [ForeignKey(nameof(EmployeeLoanId))]
        public virtual EmployeeLoan EmployeeLoan { get; set; } = null!;

        [Required]
        public int InstallmentNo { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        [Required, Column(TypeName = "decimal(10,2)")]
        public decimal PrincipalComponent { get; set; }

        [Required, Column(TypeName = "decimal(10,2)")]
        public decimal InterestComponent { get; set; }

        [Required, Column(TypeName = "decimal(10,2)")]
        public decimal TotalAmount { get; set; }

        [Required, Column(TypeName = "decimal(10,2)")]
        public decimal PaidAmount { get; set; } = 0;

        public DateTime? PaidDate { get; set; }

        [Required, MaxLength(20)]
        public string Status { get; set; } = "Pending"; // Pending, Paid, Waived

        [Required]
        public int SchoolRegistrationId { get; set; }

        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}
