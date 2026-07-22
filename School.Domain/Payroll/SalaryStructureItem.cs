using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using School.Domain.School;
using static School.Domain.BaseEntity;

namespace School.Domain.Payroll
{
    [Table("SalaryStructureItems", Schema = "Payroll")]
    public class SalaryStructureItem : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        public int SalaryStructureId { get; set; }
        [ForeignKey(nameof(SalaryStructureId))]
        public virtual SalaryStructure SalaryStructure { get; set; } = null!;

        public int SalaryComponentId { get; set; }
        [ForeignKey(nameof(SalaryComponentId))]
        public virtual SalaryComponent SalaryComponent { get; set; } = null!;

        [Required, MaxLength(30)]
        public string CalculationType { get; set; } = "Fixed"; // Fixed, PercentageOfBasic, Formula

        [Column(TypeName = "decimal(10,2)")]
        public decimal Value { get; set; } // Fixed amount or percentage value

        [MaxLength(200)]
        public string? Formula { get; set; } // e.g. "Basic * 0.12" or "Basic + HRA"

        public int DisplayOrder { get; set; } = 0;

        [Required]
        public int SchoolRegistrationId { get; set; }

        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}
