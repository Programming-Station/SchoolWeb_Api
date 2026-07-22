using System;
using System.ComponentModel.DataAnnotations;

namespace School.Domain.Dtos
{
    public class RolePermissionDto
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(450)]
        public string RoleId { get; set; } = null!;   // IdentityRole Id

        [Required]
        public Guid PermissionId { get; set; }
    }
}
