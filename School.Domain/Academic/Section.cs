using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using School.Domain.School;
using static School.Domain.BaseEntity;

namespace School.Domain
{
    public class Section : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public int ClassId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = "active"; // active, inactive

        [ForeignKey(nameof(ClassId))]
        public virtual Class Class { get; set; } = null!;

        public int SchoolRegistrationId { get; set; }

        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}
