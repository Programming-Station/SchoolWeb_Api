using System.ComponentModel.DataAnnotations; 
using static School.Domain.BaseEntity;

namespace School.Domain
{
    public class Faculty : AuditEntity<int>
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string Name { get; set; } = null!;

        [MaxLength(50)]
        public string? Code { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }

        public int DisplayOrder { get; set; } = 0;

        [Required, MaxLength(50)]
        public string Status { get; set; } = "active"; // active, inactive

        public virtual ICollection<Department> Departments { get; set; } = new List<Department>();
    }
}

