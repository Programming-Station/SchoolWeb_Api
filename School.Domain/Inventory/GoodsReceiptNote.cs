using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;
using School.Domain.School;

namespace School.Domain.Inventory
{
    [Table("GoodsReceiptNotes", Schema = "Inventory")]
    public class GoodsReceiptNote : AuditEntity<int>, ITenantEntity
    {
        public GoodsReceiptNote()
        {
            Items = new HashSet<GoodsReceiptNoteItem>();
        }

        [Key]
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string GrnNumber { get; set; } = null!; // GRN-YYYY-MM-XXXX

        public int PurchaseOrderId { get; set; }
        [ForeignKey(nameof(PurchaseOrderId))]
        public virtual PurchaseOrder PurchaseOrder { get; set; } = null!;

        [Required]
        public DateTime ReceivedDate { get; set; } = DateTime.UtcNow;

        [MaxLength(100)]
        public string? InvoiceNo { get; set; }

        [Required, MaxLength(150)]
        public string ReceivedBy { get; set; } = null!;

        [Required, MaxLength(30)]
        public string Status { get; set; } = "Draft"; // Draft, Inspected, Accepted, Rejected

        [Required]
        public int SchoolRegistrationId { get; set; }

        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;

        public virtual ICollection<GoodsReceiptNoteItem> Items { get; set; }
    }
}
