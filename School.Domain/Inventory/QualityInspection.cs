using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using School.Domain.School;
using static School.Domain.BaseEntity;

namespace School.Domain.Inventory
{
    [Table("QualityInspections", Schema = "Inventory")]
    public class QualityInspection : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        public int GrnId { get; set; }
        [ForeignKey(nameof(GrnId))]
        public virtual GoodsReceiptNote Grn { get; set; } = null!;

        public int ItemId { get; set; }
        [ForeignKey(nameof(ItemId))]
        public virtual InventoryItem Item { get; set; } = null!;

        [Column(TypeName = "decimal(12,2)")]
        public decimal QuantityInspected { get; set; }

        [Column(TypeName = "decimal(12,2)")]
        public decimal QuantityAccepted { get; set; }

        [Column(TypeName = "decimal(12,2)")]
        public decimal QuantityRejected { get; set; }

        [MaxLength(1000)]
        public string? InspectionReport { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal QualityScore { get; set; } = 100.00m;

        [Required, MaxLength(100)]
        public string InspectedBy { get; set; } = null!;

        [Required]
        public int SchoolRegistrationId { get; set; }

        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}
