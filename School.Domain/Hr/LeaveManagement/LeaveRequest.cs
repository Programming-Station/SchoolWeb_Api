using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using School.Domain.School;
using static School.Domain.BaseEntity;

namespace School.Domain.Hr.LeaveManagement
{
    public class LeaveRequest : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        public int EmployeeId { get; set; }
        [ForeignKey(nameof(EmployeeId))]
        public virtual Employee Employee { get; set; } = null!;

        public int LeaveTypeId { get; set; }
        [ForeignKey(nameof(LeaveTypeId))]
        public virtual LeaveType LeaveType { get; set; } = null!;

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal TotalDays { get; set; }

        [MaxLength(1000)]
        public string? Reason { get; set; }

        [MaxLength(50)]
        public string Status { get; set; } = "Pending"; // Pending, Approved, Rejected

        public int? ApprovedById { get; set; }
        [ForeignKey(nameof(ApprovedById))]
        public virtual Employee? ApprovedBy { get; set; }

        public int SchoolRegistrationId { get; set; }
        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}
