using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using School.Domain.School;
using static School.Domain.BaseEntity;

namespace School.Domain.Hostel
{
    [Table("HostelFeePayments", Schema = "Hostel")]
    public class HostelFeePayment : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        public int AllocationId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal AmountPaid { get; set; }

        public DateTime PaymentDate { get; set; }

        [Required, MaxLength(50)]
        public string PaymentMode { get; set; } = null!; // Cash, UPI, Card, Bank, Online

        [Required, MaxLength(100)]
        public string ReceiptNumber { get; set; } = null!;

        [MaxLength(200)]
        public string? TransactionReference { get; set; }

        [MaxLength(500)]
        public string? Remarks { get; set; }

        public int SchoolRegistrationId { get; set; }

        [ForeignKey(nameof(AllocationId))]
        public virtual HostelFeeAllocation Allocation { get; set; } = null!;

        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}
