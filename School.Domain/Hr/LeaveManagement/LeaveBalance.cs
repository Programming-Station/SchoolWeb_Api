using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;
using School.Domain.School;

namespace School.Domain.Hr.LeaveManagement
{
    public class LeaveBalance : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        public int EmployeeId { get; set; }
        [ForeignKey(nameof(EmployeeId))]
        public virtual Employee Employee { get; set; } = null!;

        public int LeaveTypeId { get; set; }
        [ForeignKey(nameof(LeaveTypeId))]
        public virtual LeaveType LeaveType { get; set; } = null!;

        [Required, MaxLength(10)]
        public string Year { get; set; } = null!; // e.g., 2026

        [Column(TypeName = "decimal(5,2)")]
        public decimal TotalLeaves { get; set; } = 0;

        [Column(TypeName = "decimal(5,2)")]
        public decimal UsedLeaves { get; set; } = 0;

        [Column(TypeName = "decimal(5,2)")]
        public decimal AvailableLeaves { get; set; } = 0;

        public int SchoolRegistrationId { get; set; }
        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}
