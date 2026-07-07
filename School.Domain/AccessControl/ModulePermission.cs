using School.Domain.Auth;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;

namespace School.Domain.AccessControl
{
    public class ModulePermission : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int ModuleId { get; set; }
        
        [ForeignKey("ModuleId")]
        public virtual Module Module { get; set; } = null!;
        
        [Required]
        [MaxLength(450)]
        public string UserId { get; set; } = null!;
        
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; } = null!;
        
        [MaxLength(450)]
        public string? RoleId { get; set; }
        
        [ForeignKey("RoleId")]
        public virtual IdentityRole? Role { get; set; }
        
        public bool IsActive { get; set; } = true; 

        public int SchoolRegistrationId { get; set; }
    }
}

