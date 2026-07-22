using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using School.Domain.School;
using static School.Domain.BaseEntity;

namespace School.Domain.Inventory
{
    [Table("VendorQuotations", Schema = "Inventory")]
    public class VendorQuotation : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        public int RfqId { get; set; }
        [ForeignKey(nameof(RfqId))]
        public virtual RequestForQuotation Rfq { get; set; } = null!;

        public int VendorId { get; set; }
        [ForeignKey(nameof(VendorId))]
        public virtual Vendor Vendor { get; set; } = null!;

        [Required]
        public DateTime QuoteDate { get; set; } = DateTime.UtcNow;

        [Required, Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        [Required, MaxLength(30)]
        public string Status { get; set; } = "Submitted"; // Submitted, Approved, Rejected

        [Required]
        public int SchoolRegistrationId { get; set; }

        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}
