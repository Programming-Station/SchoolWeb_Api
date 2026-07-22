using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;

namespace School.Domain.Student
{
    public class ParentStudentMapping : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(450)]
        public string ParentUserId { get; set; } = null!;

        [Required]
        public int StudentId { get; set; }

        [Required]
        public int SchoolRegistrationId { get; set; }

        [MaxLength(50)]
        public string? Relationship { get; set; }

        public bool IsPrimaryGuardian { get; set; } = true;

        [ForeignKey(nameof(ParentUserId))]
        public virtual global::School.Domain.Auth.ApplicationUser ParentUser { get; set; } = null!;

        [ForeignKey(nameof(StudentId))]
        public virtual global::School.Domain.Student.Student Student { get; set; } = null!;

        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual global::School.Domain.School.SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}
