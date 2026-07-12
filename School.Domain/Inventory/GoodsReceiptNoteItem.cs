using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;
using School.Domain.School;

namespace School.Domain.Inventory
{
    [Table("GoodsReceiptNoteItems", Schema = "Inventory")]
    public class GoodsReceiptNoteItem : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        public int GoodsReceiptNoteId { get; set; }
        [ForeignKey(nameof(GoodsReceiptNoteId))]
        public virtual GoodsReceiptNote GoodsReceiptNote { get; set; } = null!;

        public int ItemId { get; set; }
        [ForeignKey(nameof(ItemId))]
        public virtual InventoryItem Item { get; set; } = null!;

        [Required, Column(TypeName = "decimal(12,2)")]
        public decimal QuantityAccepted { get; set; }

        [Column(TypeName = "decimal(12,2)")]
        public decimal QuantityRejected { get; set; } = 0;

        [Required, Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }

        [Required]
        public int SchoolRegistrationId { get; set; }

        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}
