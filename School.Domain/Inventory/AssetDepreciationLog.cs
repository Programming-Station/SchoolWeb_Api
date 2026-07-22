using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using School.Domain.School;
using static School.Domain.BaseEntity;

namespace School.Domain.Inventory
{
    [Table("AssetDepreciationLogs", Schema = "Inventory")]
    public class AssetDepreciationLog : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        public int ItemId { get; set; } // Fixed asset item
        [ForeignKey(nameof(ItemId))]
        public virtual InventoryItem Item { get; set; } = null!;

        [Required]
        public DateTime DepreciationDate { get; set; } = DateTime.UtcNow;

        [Required, Column(TypeName = "decimal(18,2)")]
        public decimal DepreciationAmount { get; set; }

        [Required, Column(TypeName = "decimal(18,2)")]
        public decimal BookValueAfterDepreciation { get; set; }

        [MaxLength(500)]
        public string? Remarks { get; set; }

        [Required]
        public int SchoolRegistrationId { get; set; }

        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}
