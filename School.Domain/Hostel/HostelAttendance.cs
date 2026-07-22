using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using School.Domain.School;
using static School.Domain.BaseEntity;

namespace School.Domain.Hostel
{
    [Table("HostelAttendances", Schema = "Hostel")]
    public class HostelAttendance : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        public int StudentId { get; set; }

        public DateTime Date { get; set; }

        [Required, MaxLength(50)]
        public string AttendanceType { get; set; } = null!; // Morning, Night

        [Required, MaxLength(50)]
        public string Status { get; set; } = null!; // Present, Absent, Leave

        [MaxLength(500)]
        public string? Remarks { get; set; }

        public int SchoolRegistrationId { get; set; }

        [ForeignKey(nameof(StudentId))]
        public virtual global::School.Domain.Student.Student Student { get; set; } = null!;

        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}
