using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using School.Domain.School;
using static School.Domain.BaseEntity;

namespace School.Domain
{
    public class Class : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)] 
        public string Name { get; set; } = null!;

        [MaxLength(50)]
        public string? Section { get; set; }

        [Required]
        public int CourseId { get; set; } 

        [Required]
        [MaxLength(20)]
        public string AcademicYear { get; set; } = null!;

        [Required]
        public int Capacity { get; set; }

        public int CurrentStrength { get; set; } = 0;

        public int? ClassTeacherId { get; set; }
        [ForeignKey(nameof(ClassTeacherId))]
        public virtual global::School.Domain.Hr.Employee? ClassTeacher { get; set; }

        [MaxLength(50)]
        public string? RoomNumber { get; set; }

        [Required, MaxLength(50)] 
        public string Status { get; set; } = "active"; // active or inactive

        [ForeignKey(nameof(CourseId))]
        public virtual Course Course { get; set; } = null!;

        public int SchoolRegistrationId { get; set; }
        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}

