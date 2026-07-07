using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;

namespace School.Domain.AccessControl
{
    public class Module : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = null!;
        
        [MaxLength(500)]
        public string? Description { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string Route { get; set; } = null!;
        
        [MaxLength(100)]
        public string? Icon { get; set; }
        
        [Required]
        public int CategoryModuleId { get; set; }
        
        [ForeignKey("CategoryModuleId")]
        public virtual CategoryModule CategoryModule { get; set; } = null!;
        
        public int Order { get; set; } = 0;
        
        public bool IsActive { get; set; } = true; 

        public int SchoolRegistrationId { get; set; }
    }
}

