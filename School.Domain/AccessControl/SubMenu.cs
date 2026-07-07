using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using School.Domain.School;
using static School.Domain.BaseEntity;

namespace School.Domain.AccessControl
{
    public class SubMenu : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string SubMenuName { get; set; } = null!;

        [MaxLength(500)]
        public string? URL { get; set; }

        public int Priority { get; set; }

        [MaxLength(100)]
        public string? Icon { get; set; }

        [MaxLength(200)]
        public string? Controller { get; set; }

        [MaxLength(200)]
        public string? Action { get; set; }

        public bool IsVisible { get; set; } = true;

        [MaxLength(500)]
        public string? AccesibleFor { get; set; }

        [Required]
        public int MenuId { get; set; }

        public int SchoolRegistrationId { get; set; }

        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;

        [ForeignKey("MenuId")]
        public virtual Menu? MainMenu { get; set; }
    }
}
