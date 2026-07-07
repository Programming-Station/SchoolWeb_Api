using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using School.Domain.School;
using static School.Domain.BaseEntity;

namespace School.Domain.AccessControl
{
    public class Menu : AuditEntity<int>, ITenantEntity
    {
        public Menu()
        {
            SubMenus = new HashSet<SubMenu>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string MenuName { get; set; } = null!;

        [MaxLength(500)]
        public string? URL { get; set; }

        public int Priority { get; set; }

        [MaxLength(100)]
        public string? MenuIcon { get; set; }

        [MaxLength(200)]
        public string? Controller { get; set; }

        [MaxLength(200)]
        public string? Action { get; set; }

        public bool IsVisible { get; set; } = true;

        public int SchoolRegistrationId { get; set; }

        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;

        public virtual ICollection<SubMenu> SubMenus { get; set; }
    }
}
