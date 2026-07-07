using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;
using School.Domain.School;

namespace School.Domain
{
    public class Department : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string Name { get; set; } = null!;

        [MaxLength(50)]
        public string? Code { get; set; }

        [Required]
        public int FacultyId { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }

        public int DisplayOrder { get; set; } = 0;

        [Required, MaxLength(50)]
        public string Status { get; set; } = "active"; // active, inactive

        [ForeignKey(nameof(FacultyId))]
        public virtual Faculty Faculty { get; set; } = null!;
        public int SchoolRegistrationId { get; set; }
        [System.ComponentModel.DataAnnotations.Schema.ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}


