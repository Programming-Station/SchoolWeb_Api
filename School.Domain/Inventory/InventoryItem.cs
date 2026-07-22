using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using School.Domain.Finance;
using School.Domain.School;
using static School.Domain.BaseEntity;

namespace School.Domain.Inventory
{
    [Table("InventoryItems", Schema = "Inventory")]
    public class InventoryItem : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string Sku { get; set; } = null!; // Unique stock keeping unit

        [Required, MaxLength(150)]
        public string Name { get; set; } = null!;

        public int CategoryId { get; set; }
        [ForeignKey(nameof(CategoryId))]
        public virtual ItemCategory Category { get; set; } = null!;

        [Required, MaxLength(20)]
        public string Uom { get; set; } = "pcs"; // pcs, kgs, boxes

        [Column(TypeName = "decimal(12,2)")]
        public decimal MinStockLevel { get; set; } = 0;

        [Column(TypeName = "decimal(12,2)")]
        public decimal CurrentStock { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; } = 0;

        public int? AssetAccountId { get; set; }
        [ForeignKey(nameof(AssetAccountId))]
        public virtual CoaAccount? AssetAccount { get; set; }

        public int? ExpenseAccountId { get; set; }
        [ForeignKey(nameof(ExpenseAccountId))]
        public virtual CoaAccount? ExpenseAccount { get; set; }

        [Required]
        public int SchoolRegistrationId { get; set; }

        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}
