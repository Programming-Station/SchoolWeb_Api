using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using School.Domain.School;
using static School.Domain.BaseEntity;

namespace School.Domain.Finance
{
    [Table("TaxConfigs", Schema = "Finance")]
    public class TaxConfig : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string TaxName { get; set; } = null!; // e.g. "CGST 9%"

        [Required, Column(TypeName = "decimal(5,2)")]
        public decimal Percentage { get; set; }

        public int AccountId { get; set; } // Associated liability ledger
        [ForeignKey(nameof(AccountId))]
        public virtual CoaAccount Account { get; set; } = null!;

        public bool IsActive { get; set; } = true;

        [Required]
        public int SchoolRegistrationId { get; set; }

        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}
