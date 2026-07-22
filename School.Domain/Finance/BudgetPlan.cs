using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using School.Domain.School;
using static School.Domain.BaseEntity;

namespace School.Domain.Finance
{
    [Table("BudgetPlans", Schema = "Finance")]
    public class BudgetPlan : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(20)]
        public string FinancialYear { get; set; } = null!; // e.g. "2026-2027"

        public int DepartmentId { get; set; }
        [ForeignKey(nameof(DepartmentId))]
        public virtual Department Department { get; set; } = null!;

        public int AccountId { get; set; }
        [ForeignKey(nameof(AccountId))]
        public virtual CoaAccount Account { get; set; } = null!;

        [Required, Column(TypeName = "decimal(18,2)")]
        public decimal AllocatedAmount { get; set; }

        [Required, Column(TypeName = "decimal(18,2)")]
        public decimal UtilizedAmount { get; set; } = 0;

        [Required]
        public int SchoolRegistrationId { get; set; }

        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}
