using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;
using School.Domain.School;

namespace School.Domain.Hr
{
    public class EmployeeSalaryDetail : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        public int EmployeeId { get; set; }
        [ForeignKey(nameof(EmployeeId))]
        public virtual Employee Employee { get; set; } = null!;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Basic { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal HRA { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal DA { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal PF { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal ESI { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Bonus { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Allowances { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Deductions { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal NetSalary { get; set; } = 0;

        public int SchoolRegistrationId { get; set; }
        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}
