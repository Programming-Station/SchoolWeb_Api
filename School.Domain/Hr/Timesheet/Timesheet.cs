using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using School.Domain.School;
using static School.Domain.BaseEntity;

namespace School.Domain.Hr.Timesheet
{
    public class Timesheet : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        public int EmployeeId { get; set; }
        [ForeignKey(nameof(EmployeeId))]
        public virtual Employee Employee { get; set; } = null!;

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        [MaxLength(50)]
        public string Status { get; set; } = "Draft"; // Draft, Submitted, Approved, Rejected

        public int? ApprovedById { get; set; }
        [ForeignKey(nameof(ApprovedById))]
        public virtual Employee? ApprovedBy { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal TotalHours { get; set; } = 0;

        public int SchoolRegistrationId { get; set; }
        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;

        public virtual ICollection<TimesheetEntry> Entries { get; set; } = new List<TimesheetEntry>();
    }
}
