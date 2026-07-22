using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using School.Domain.School;
using static School.Domain.BaseEntity;

namespace School.Domain.Academic
{
    public class Subject : AuditEntity<int>, ITenantEntity
    {
        [Key] public int Id { get; set; }
        [Required, MaxLength(150)] public string Name { get; set; } = null!;
        [MaxLength(500)] public string? Description { get; set; }
        public int DisplayOrder { get; set; } = 0;
        [MaxLength(20)] public string Status { get; set; } = "active";
        public int SchoolRegistrationId { get; set; }
        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}
