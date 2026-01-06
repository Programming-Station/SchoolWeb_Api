using System.ComponentModel.DataAnnotations;
using static School.Domain.BaseEntity;

namespace School.Domain
{
    public class CategoryModule : AuditEntity<int>
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;
        
        [MaxLength(500)]
        public string? Description { get; set; }
        
        public int Order { get; set; } = 0;
        
        public bool IsActive { get; set; } = true; 
    }
}

