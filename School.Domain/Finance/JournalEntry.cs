using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using School.Domain.School;
using static School.Domain.BaseEntity;

namespace School.Domain.Finance
{
    [Table("JournalEntries", Schema = "Finance")]
    public class JournalEntry : AuditEntity<int>, ITenantEntity
    {
        public JournalEntry()
        {
            Lines = new HashSet<JournalEntryLine>();
        }

        [Key]
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string VoucherNo { get; set; } = null!; // Auto generated: JV-YYYY-MM-XXXX

        [Required]
        public DateTime EntryDate { get; set; } = DateTime.UtcNow;

        [MaxLength(1000)]
        public string? Narration { get; set; }

        [MaxLength(200)]
        public string? Reference { get; set; } // Invoice ID, Bill ID, etc.

        [MaxLength(50)]
        public string Source { get; set; } = "Manual"; // Manual, Fees, Payroll, Procurement

        [Required, MaxLength(50)]
        public string VoucherType { get; set; } = "Journal"; // Journal, Payment, Receipt, Contra, CreditNote, DebitNote, Opening, Closing, Adjustment

        [Required, MaxLength(30)]
        public string Status { get; set; } = "Approved"; // Pending, Approved, Rejected (Default Approved for backward compatibility)

        [MaxLength(150)]
        public string? ApprovedBy { get; set; }

        public DateTime? ApprovedDate { get; set; }

        public int? CostCenterId { get; set; }
        [ForeignKey(nameof(CostCenterId))]
        public virtual CostCenter? CostCenter { get; set; }

        [MaxLength(500)]
        public string? AttachmentUrl { get; set; }

        public bool IsPosted { get; set; } = false;

        [Required]
        public int SchoolRegistrationId { get; set; }

        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;

        public virtual ICollection<JournalEntryLine> Lines { get; set; }
    }
}
