using School.Utilities.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using School.Domain.School;
using static School.Domain.BaseEntity;

namespace School.Domain
{
    public class Course : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(200)] 
        public string Name { get; set; } = null!;

        [Required, MaxLength(50)] 
        public string CourseCode { get; set; } = null!;

        [Required, MaxLength(20)]
        public CourseType? CourseType { get; set; }

        [MaxLength(100)]
        public string? Duration { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Fees { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;
        public string? ImagePath { get; set; } = null!;

        public int SchoolRegistrationId { get; set; }
        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}

