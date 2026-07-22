using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using School.Domain.School;
using static School.Domain.BaseEntity;

namespace School.Domain.Inventory
{
    [Table("AssetMaintenanceLogs", Schema = "Inventory")]
    public class AssetMaintenanceLog : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        public int ItemId { get; set; } // Asset item
        [ForeignKey(nameof(ItemId))]
        public virtual InventoryItem Item { get; set; } = null!;

        [Required]
        public DateTime MaintenanceDate { get; set; }

        [Required, MaxLength(100)]
        public string MaintenanceType { get; set; } = "Preventative"; // Preventative, Corrective

        [Column(TypeName = "decimal(18,2)")]
        public decimal Cost { get; set; } = 0;

        [MaxLength(1000)]
        public string? ServiceDetails { get; set; }

        [MaxLength(150)]
        public string? PerformedBy { get; set; }

        [Required]
        public int SchoolRegistrationId { get; set; }

        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}
