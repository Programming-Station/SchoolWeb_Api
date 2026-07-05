using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;

namespace School.Domain.AccessControl
{
    public class MenuPermession : AuditEntity<int>
    {
        public Guid Id { get; set; }
        public bool IsVisible { get; set; }
        public int MenuId { get; set; }
        public int? SubMenuId { get; set; }
        public string RoleId { get; set; }
        [ForeignKey("MenuId")]
        public virtual Menu MainMenu { get; set; }
        [ForeignKey("SubMenuId")]
        public virtual SubMenu SubMenu { get; set; }
        [ForeignKey("RoleId")]
        public virtual IdentityRole Role { get; set; }
    }
}
