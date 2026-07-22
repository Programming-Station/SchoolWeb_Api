using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using School.Domain.School;
using static School.Domain.BaseEntity;

namespace School.Domain.Inventory
{
    [Table("PurchaseRequisitionItems", Schema = "Inventory")]
    public class PurchaseRequisitionItem : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        public int PurchaseRequisitionId { get; set; }
        [ForeignKey(nameof(PurchaseRequisitionId))]
        public virtual PurchaseRequisition PurchaseRequisition { get; set; } = null!;

        public int ItemId { get; set; }
        [ForeignKey(nameof(ItemId))]
        public virtual InventoryItem Item { get; set; } = null!;

        [Required, Column(TypeName = "decimal(12,2)")]
        public decimal Quantity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal EstimatedCost { get; set; }

        [Required]
        public int SchoolRegistrationId { get; set; }

        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}
