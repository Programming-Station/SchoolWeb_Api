using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using School.Domain.School;
using static School.Domain.BaseEntity;

namespace School.Domain.Hr.Timesheet
{
    public class TimesheetEntry : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        public int TimesheetId { get; set; }
        [ForeignKey(nameof(TimesheetId))]
        public virtual Timesheet Timesheet { get; set; } = null!;

        public DateTime EntryDate { get; set; }

        [Required, MaxLength(200)]
        public string TaskName { get; set; } = null!;

        [MaxLength(200)]
        public string? ProjectName { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal HoursWorked { get; set; } = 0;

        [MaxLength(1000)]
        public string? Description { get; set; }

        public int SchoolRegistrationId { get; set; }
        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}
