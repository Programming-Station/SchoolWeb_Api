using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using School.Domain.School;
using static School.Domain.BaseEntity;
namespace School.Domain.Payroll
{
    [Table("SalaryComponents", Schema = "Payroll")]
    public class SalaryComponent : AuditEntity<int>, ITenantEntity
    {
        [Key] public int Id { get; set; }
        [Required, MaxLength(100)] public string Name { get; set; } = null!;
        [MaxLength(20)] public string Type { get; set; } = "Earning"; // Earning, Deduction
        [Column(TypeName = "decimal(10,2)")]
        public decimal Amount { get; set; }
        [MaxLength(20)] public string Status { get; set; } = "active";
        public int SchoolRegistrationId { get; set; }
        [ForeignKey(nameof(SchoolRegistrationId))] public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}

