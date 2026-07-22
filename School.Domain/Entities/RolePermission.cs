using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
namespace School.Domain.Entities
{
    [Table("RolePermissions")]
    public class RolePermission
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(450)]
        public string RoleId { get; set; } = null!;

        [Required]
        public Guid PermissionId { get; set; }

        // Navigation properties
        [ForeignKey(nameof(RoleId))]
        public IdentityRole Role { get; set; } = null!;
        public Permission Permission { get; set; } = null!;
    }
}
