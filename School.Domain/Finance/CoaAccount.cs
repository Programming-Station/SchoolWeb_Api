using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using School.Domain.School;
using static School.Domain.BaseEntity;

namespace School.Domain.Finance
{
    [Table("CoaAccounts", Schema = "Finance")]
    public class CoaAccount : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string Code { get; set; } = null!; // e.g. "10000", "11000"

        [Required, MaxLength(150)]
        public string Name { get; set; } = null!;

        [Required, MaxLength(30)]
        public string AccountType { get; set; } = "Asset"; // Asset, Liability, Equity, Revenue, Expense

        public int? ParentAccountId { get; set; }
        [ForeignKey(nameof(ParentAccountId))]
        public virtual CoaAccount? ParentAccount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal CurrentBalance { get; set; } = 0;

        public bool IsActive { get; set; } = true;

        [Required]
        public int SchoolRegistrationId { get; set; }

        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}
