using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;

namespace School.Domain.AccessControl
{
    public class MenuPermession : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        public bool IsVisible { get; set; } = true;

        [Required]
        public int MenuId { get; set; }

        public int? SubMenuId { get; set; }

        [Required]
        [MaxLength(450)]
        public string RoleId { get; set; } = null!;

        public int SchoolRegistrationId { get; set; }

        [ForeignKey("MenuId")]
        public virtual Menu MainMenu { get; set; } = null!;

        [ForeignKey("SubMenuId")]
        public virtual SubMenu? SubMenu { get; set; }

        [ForeignKey("RoleId")]
        public virtual IdentityRole Role { get; set; } = null!;
    }
}
