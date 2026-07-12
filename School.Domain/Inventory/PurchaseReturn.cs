using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;
using School.Domain.School;

namespace School.Domain.Inventory
{
    [Table("PurchaseReturns", Schema = "Inventory")]
    public class PurchaseReturn : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string ReturnNo { get; set; } = null!; // PR-YYYY-XXXXX

        public int GrnId { get; set; }
        [ForeignKey(nameof(GrnId))]
        public virtual GoodsReceiptNote Grn { get; set; } = null!;

        [Required]
        public DateTime ReturnDate { get; set; } = DateTime.UtcNow;

        [Required, Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        [Required, MaxLength(250)]
        public string Reason { get; set; } = null!;

        [MaxLength(100)]
        public string? CreditNoteNo { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal RefundAmount { get; set; } = 0;

        [Required, MaxLength(30)]
        public string Status { get; set; } = "Draft"; // Draft, Approved, Dispatched, CreditReceived

        [Required]
        public int SchoolRegistrationId { get; set; }

        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}
