using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using School.Domain.School;
using static School.Domain.BaseEntity;

namespace School.Domain.Finance
{
    [Table("CashBankTransactions", Schema = "Finance")]
    public class CashBankTransaction : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        public int? JournalEntryId { get; set; }
        [ForeignKey(nameof(JournalEntryId))]
        public virtual JournalEntry? JournalEntry { get; set; }

        public int AccountId { get; set; }
        [ForeignKey(nameof(AccountId))]
        public virtual CoaAccount Account { get; set; } = null!;

        [Required, MaxLength(20)]
        public string TransactionType { get; set; } = "Deposit"; // Deposit, Withdrawal, Contra, Transfer

        [Required, MaxLength(50)]
        public string PaymentMode { get; set; } = "BankTransfer"; // Cash, Cheque, BankTransfer, UPI

        [MaxLength(100)]
        public string? ReferenceNo { get; set; } // Cheque #, TXN reference

        [MaxLength(50)]
        public string? ChequeNo { get; set; }

        public int? CostCenterId { get; set; }
        [ForeignKey(nameof(CostCenterId))]
        public virtual CostCenter? CostCenter { get; set; }

        [Required]
        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;

        [Required, Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        public bool IsReconciled { get; set; } = false;

        public DateTime? ReconciledDate { get; set; }

        public DateTime? ClearedDate { get; set; }

        [Required]
        public int SchoolRegistrationId { get; set; }

        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}
