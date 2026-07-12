using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;
using School.Domain.School;
using School.Domain.Hr;

namespace School.Domain.Payroll
{
    [Table("EmployeeSalaryAllocations", Schema = "Payroll")]
    public class EmployeeSalaryAllocation : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        public int EmployeeId { get; set; }
        [ForeignKey(nameof(EmployeeId))]
        public virtual Employee Employee { get; set; } = null!;

        public int SalaryStructureId { get; set; }
        [ForeignKey(nameof(SalaryStructureId))]
        public virtual SalaryStructure SalaryStructure { get; set; } = null!;

        [Required]
        public DateTime EffectiveDate { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal BaseSalary { get; set; } // Customized base salary if applicable

        [MaxLength(500)]
        public string? Remarks { get; set; } // e.g. "Annual Increment 2026", "Promotion"

        public bool IsActive { get; set; } = true;

        [Required]
        public int SchoolRegistrationId { get; set; }

        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}
