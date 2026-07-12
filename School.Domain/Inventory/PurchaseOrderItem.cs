using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;
using School.Domain.School;

namespace School.Domain.Inventory
{
    [Table("PurchaseOrderItems", Schema = "Inventory")]
    public class PurchaseOrderItem : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        public int PurchaseOrderId { get; set; }
        [ForeignKey(nameof(PurchaseOrderId))]
        public virtual PurchaseOrder PurchaseOrder { get; set; } = null!;

        public int ItemId { get; set; }
        [ForeignKey(nameof(ItemId))]
        public virtual InventoryItem Item { get; set; } = null!;

        [Required, Column(TypeName = "decimal(12,2)")]
        public decimal QuantityOrdered { get; set; }

        [Column(TypeName = "decimal(12,2)")]
        public decimal QuantityReceived { get; set; } = 0;

        [Required, Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }

        [Required]
        public int SchoolRegistrationId { get; set; }

        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}
