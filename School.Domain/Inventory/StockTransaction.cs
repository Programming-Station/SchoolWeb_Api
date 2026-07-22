using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using School.Domain.School;
using static School.Domain.BaseEntity;

namespace School.Domain.Inventory
{
    [Table("StockTransactions", Schema = "Inventory")]
    public class StockTransaction : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        public int ItemId { get; set; }
        [ForeignKey(nameof(ItemId))]
        public virtual InventoryItem Item { get; set; } = null!;

        [Required, MaxLength(25)]
        public string TransactionType { get; set; } = "Inward"; // Inward, Outward, Adjustment, Transfer

        [Required, Column(TypeName = "decimal(12,2)")]
        public decimal Quantity { get; set; }

        [MaxLength(100)]
        public string? ReferenceNo { get; set; } // GRN #, issue slip #

        [Required]
        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;

        [MaxLength(100)]
        public string? WarehouseLoc { get; set; }

        [Required]
        public int SchoolRegistrationId { get; set; }

        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}
