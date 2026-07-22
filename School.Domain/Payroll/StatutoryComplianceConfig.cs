using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using School.Domain.School;
using static School.Domain.BaseEntity;

namespace School.Domain.Payroll
{
    [Table("StatutoryComplianceConfigs", Schema = "Payroll")]
    public class StatutoryComplianceConfig : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        [Required, Column(TypeName = "decimal(5,2)")]
        public decimal PfEmployerRate { get; set; } = 12.00m; // percentage

        [Required, Column(TypeName = "decimal(5,2)")]
        public decimal PfEmployeeRate { get; set; } = 12.00m; // percentage

        [Required, Column(TypeName = "decimal(10,2)")]
        public decimal PfMaxBasicLimit { get; set; } = 15000.00m; // e.g. max basic salary to compute PF on

        [Required, Column(TypeName = "decimal(5,2)")]
        public decimal EsiEmployerRate { get; set; } = 3.25m; // percentage

        [Required, Column(TypeName = "decimal(5,2)")]
        public decimal EsiEmployeeRate { get; set; } = 0.75m; // percentage

        [Required, Column(TypeName = "decimal(10,2)")]
        public decimal EsiMaxGrossLimit { get; set; } = 21000.00m; // gross salary limit for ESI applicability

        [MaxLength(2000)]
        public string? ProfessionalTaxSlabJson { get; set; } // PT bracket configurations

        public bool EnableGratuity { get; set; } = false;

        [Required]
        public int SchoolRegistrationId { get; set; }

        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}
