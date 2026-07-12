using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;
using School.Domain.School;

namespace School.Domain.Payroll
{
    [Table("PayrollRunDetails", Schema = "Payroll")]
    public class PayrollRunDetail : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        public int PayrollRunId { get; set; }
        [ForeignKey(nameof(PayrollRunId))]
        public virtual PayrollRun PayrollRun { get; set; } = null!;

        public int SalaryComponentId { get; set; }
        [ForeignKey(nameof(SalaryComponentId))]
        public virtual SalaryComponent SalaryComponent { get; set; } = null!;

        [Required, Column(TypeName = "decimal(10,2)")]
        public decimal Amount { get; set; }

        [Required]
        public int SchoolRegistrationId { get; set; }

        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}
