using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using School.Domain.School;
using static School.Domain.BaseEntity;

namespace School.Domain.Inventory
{
    [Table("PurchaseOrders", Schema = "Inventory")]
    public class PurchaseOrder : AuditEntity<int>, ITenantEntity
    {
        public PurchaseOrder()
        {
            Items = new HashSet<PurchaseOrderItem>();
        }

        [Key]
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string PoNumber { get; set; } = null!; // PO-YYYY-MM-XXXX

        public int VendorId { get; set; }
        [ForeignKey(nameof(VendorId))]
        public virtual Vendor Vendor { get; set; } = null!;

        [Required]
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        public DateTime? DeliveryDate { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal TaxPercentage { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal DiscountAmount { get; set; } = 0;

        [Required, Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        [Required, MaxLength(30)]
        public string Status { get; set; } = "Draft"; // Draft, Sent, PartiallyReceived, FullyReceived, Cancelled

        [Required]
        public int SchoolRegistrationId { get; set; }

        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;

        public virtual ICollection<PurchaseOrderItem> Items { get; set; }
    }
}
