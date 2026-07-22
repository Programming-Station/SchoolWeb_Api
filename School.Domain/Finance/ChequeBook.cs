using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using School.Domain.School;
using static School.Domain.BaseEntity;

namespace School.Domain.Finance
{
    [Table("ChequeBooks", Schema = "Finance")]
    public class ChequeBook : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        public int BankAccountId { get; set; }
        [ForeignKey(nameof(BankAccountId))]
        public virtual CoaAccount BankAccount { get; set; } = null!;

        [Required, MaxLength(50)]
        public string SeriesPrefix { get; set; } = null!; // e.g. "CHQ"

        public int StartChequeNo { get; set; }

        public int EndChequeNo { get; set; }

        public int NextChequeNo { get; set; }

        public bool IsExhausted { get; set; } = false;

        [Required]
        public int SchoolRegistrationId { get; set; }

        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}
