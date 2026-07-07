using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using School.Domain.Hr;
using School.Domain.School;
using static School.Domain.BaseEntity;

namespace School.Domain.Hr.Training
{
    public class TrainingEnrollment : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        public int TrainingProgramId { get; set; }
        [ForeignKey(nameof(TrainingProgramId))]
        public virtual TrainingProgram TrainingProgram { get; set; } = null!;

        public int EmployeeId { get; set; }
        [ForeignKey(nameof(EmployeeId))]
        public virtual Employee Employee { get; set; } = null!;

        public DateTime EnrollmentDate { get; set; } = DateTime.UtcNow;

        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = "Enrolled"; // Enrolled, Completed, Absent

        [MaxLength(500)]
        public string? Feedback { get; set; }

        public int SchoolRegistrationId { get; set; }
        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}
