using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;
using School.Domain.School;

namespace School.Domain.Hr.Attendance
{
    public class AttendanceLog : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        public int EmployeeId { get; set; }
        [ForeignKey(nameof(EmployeeId))]
        public virtual Employee Employee { get; set; } = null!;

        public DateTime LogTime { get; set; }

        [Required, MaxLength(50)]
        public string LogType { get; set; } = null!; // In, Out

        [MaxLength(100)]
        public string? Source { get; set; } // Biometric, Manual, App

        [MaxLength(100)]
        public string? DeviceId { get; set; }

        public int SchoolRegistrationId { get; set; }
        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}
