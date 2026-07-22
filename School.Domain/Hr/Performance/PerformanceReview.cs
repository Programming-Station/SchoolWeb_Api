using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using School.Domain.School;
using static School.Domain.BaseEntity;

namespace School.Domain.Hr.Performance
{
    public class PerformanceReview : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        public int EmployeeId { get; set; }
        [ForeignKey(nameof(EmployeeId))]
        public virtual Employee Employee { get; set; } = null!;

        public int ReviewerId { get; set; }
        [ForeignKey(nameof(ReviewerId))]
        public virtual Employee Reviewer { get; set; } = null!;

        public DateTime ReviewDate { get; set; }

        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }

        [Column(TypeName = "decimal(5, 2)")]
        public decimal KpiScore { get; set; } // Overall score from 1-10 or 1-100

        [MaxLength(1000)]
        public string? Comments { get; set; }

        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = "Draft"; // Draft, Submitted, Approved

        public int SchoolRegistrationId { get; set; }
        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}
