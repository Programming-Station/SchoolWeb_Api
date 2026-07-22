using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using School.Domain.School;
using static School.Domain.BaseEntity;

namespace School.Domain.FeeManagnment
{
#nullable disable
    public class FeePayment : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int FeeInstallmentId { get; set; }
        [ForeignKey(nameof(FeeInstallmentId))]
        public virtual FeeInstallment FeeInstallment { get; set; }

        [Required]
        public int StudentId { get; set; }
        [ForeignKey(nameof(StudentId))]
        public virtual Student.Student Student { get; set; }

        [Required, Column(TypeName = "decimal(18,2)")]
        public decimal AmountPaid { get; set; }

        [Required]
        public DateTime PaymentDate { get; set; } = DateTime.Now;

        /// <summary>Cash / Cheque / Online / DD / NEFT / UPI</summary>
        [Required, MaxLength(30)]
        public string PaymentMode { get; set; } = "Cash";

        [MaxLength(100)]
        public string TransactionRef { get; set; } // Cheque no, UTR, etc.

        [Required, MaxLength(50)]
        public string ReceiptNo { get; set; } // Auto-generated unique receipt

        [MaxLength(450)]
        public string CollectedBy { get; set; } // UserId of staff

        [MaxLength(500)]
        public string Remarks { get; set; }

        /// <summary>Completed / Cancelled / Refunded</summary>
        [MaxLength(20)]
        public string Status { get; set; } = "Completed";

        [Required]
        public int SchoolRegistrationId { get; set; }
        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; }
    }
}
