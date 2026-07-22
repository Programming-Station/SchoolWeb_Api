using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using School.Domain.School;
using static School.Domain.BaseEntity;

namespace School.Domain.Finance
{
    [Table("Incomes", Schema = "Finance")]
    public class Income : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string IncomeNumber { get; set; } = null!;

        [Required]
        public int CoaAccountId { get; set; }
        [ForeignKey(nameof(CoaAccountId))]
        public virtual CoaAccount CoaAccount { get; set; } = null!;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;

        [Required, MaxLength(150)]
        public string ReceivedFrom { get; set; } = null!;

        [MaxLength(100)]
        public string? ReferenceNumber { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        [Required, MaxLength(30)]
        public string Status { get; set; } = "Cleared"; // Draft, Cleared, Paid

        [Required]
        public int SchoolRegistrationId { get; set; }

        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}
