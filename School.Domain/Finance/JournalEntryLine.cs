using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;
using School.Domain.School;

namespace School.Domain.Finance
{
    [Table("JournalEntryLines", Schema = "Finance")]
    public class JournalEntryLine : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        public int JournalEntryId { get; set; }
        [ForeignKey(nameof(JournalEntryId))]
        public virtual JournalEntry JournalEntry { get; set; } = null!;

        public int AccountId { get; set; }
        [ForeignKey(nameof(AccountId))]
        public virtual CoaAccount Account { get; set; } = null!;

        [Column(TypeName = "decimal(18,2)")]
        public decimal DebitAmount { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal CreditAmount { get; set; } = 0;

        [MaxLength(500)]
        public string? Description { get; set; }

        [Required]
        public int SchoolRegistrationId { get; set; }

        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}
