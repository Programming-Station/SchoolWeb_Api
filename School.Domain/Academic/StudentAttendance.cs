using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using School.Domain.School;
using static School.Domain.BaseEntity;

namespace School.Domain.Academic
{
#nullable disable
    public class StudentAttendance : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int StudentId { get; set; }
        [ForeignKey(nameof(StudentId))]
        public virtual Student.Student Student { get; set; }

        public int? SubjectId { get; set; }
        [ForeignKey(nameof(SubjectId))]
        public virtual Subject Subject { get; set; }

        public int? ClassId { get; set; }
        [ForeignKey(nameof(ClassId))]
        public virtual Class Class { get; set; }

        public int? BatchId { get; set; }
        [ForeignKey(nameof(BatchId))]
        public virtual Batch Batch { get; set; }

        [Required]
        public DateTime AttendanceDate { get; set; }

        /// <summary>Present / Absent / Late / Leave / Holiday</summary>
        [Required, MaxLength(20)]
        public string Status { get; set; } = "Present";

        [MaxLength(450)]
        public string MarkedBy { get; set; } // UserId of teacher

        [MaxLength(200)]
        public string Remarks { get; set; }

        /// <summary>Period number (1st period, 2nd period...)</summary>
        public int? PeriodNo { get; set; }

        [Required]
        public int SchoolRegistrationId { get; set; }
        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; }
    }
}
