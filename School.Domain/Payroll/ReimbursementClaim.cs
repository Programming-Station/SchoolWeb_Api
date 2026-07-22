using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using School.Domain.Hr;
using School.Domain.School;
using static School.Domain.BaseEntity;

namespace School.Domain.Payroll
{
    [Table("ReimbursementClaims", Schema = "Payroll")]
    public class ReimbursementClaim : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        public int EmployeeId { get; set; }
        [ForeignKey(nameof(EmployeeId))]
        public virtual Employee Employee { get; set; } = null!;

        [Required, MaxLength(50)]
        public string ClaimType { get; set; } = "Travel"; // Medical, Travel, Fuel, Food, Internet, Telephone, Education

        [Required, Column(TypeName = "decimal(10,2)")]
        public decimal Amount { get; set; }

        [Required]
        public DateTime ClaimDate { get; set; } = DateTime.UtcNow;

        [MaxLength(1000)]
        public string? Description { get; set; }

        [MaxLength(500)]
        public string? AttachmentPath { get; set; }

        [Required, MaxLength(20)]
        public string Status { get; set; } = "Pending"; // Pending, Approved, Rejected, Settled

        [MaxLength(450)]
        public string? ApprovedBy { get; set; }

        public DateTime? ApprovedDate { get; set; }

        [MaxLength(200)]
        public string? SettlementRef { get; set; }

        [Required]
        public int SchoolRegistrationId { get; set; }

        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}
