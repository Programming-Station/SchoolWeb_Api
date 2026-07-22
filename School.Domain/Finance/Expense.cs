using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using School.Domain.School;
using School.Domain.Inventory;
using static School.Domain.BaseEntity;

namespace School.Domain.Finance
{
    [Table("Expenses", Schema = "Finance")]
    public class Expense : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string ExpenseNumber { get; set; } = null!;

        [Required]
        public int CoaAccountId { get; set; }
        [ForeignKey(nameof(CoaAccountId))]
        public virtual CoaAccount CoaAccount { get; set; } = null!;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;

        public int? VendorId { get; set; }
        [ForeignKey(nameof(VendorId))]
        public virtual Vendor? Vendor { get; set; }

        [MaxLength(100)]
        public string? ReferenceNumber { get; set; }

        [MaxLength(500)]
        public string? ReceiptUrl { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        [Required, MaxLength(30)]
        public string Status { get; set; } = "Paid"; // Draft, Approved, Paid

        [Required]
        public int SchoolRegistrationId { get; set; }

        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}
