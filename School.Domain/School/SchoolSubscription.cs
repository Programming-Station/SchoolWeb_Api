using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;

namespace School.Domain.School
{
    [Table("SchoolSubscriptions", Schema = "School")]
    public class SchoolSubscription : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        public int SchoolRegistrationId { get; set; }
        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;

        public int SubscriptionPlanId { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public decimal AmountPaid { get; set; }

        [MaxLength(50)]
        public string PaymentStatus { get; set; } = "Pending"; // Pending, Paid, Failed

        [MaxLength(100)]
        public string? TransactionId { get; set; }

        public bool IsActive { get; set; } = true;
    }
}

