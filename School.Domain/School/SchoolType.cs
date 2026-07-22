using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;

namespace School.Domain.School
{
    [Table("SchoolTypes", Schema = "School")]
    public class SchoolType : AuditEntity<int>
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;

        public virtual ICollection<SchoolRegistration> SchoolRegistrations { get; set; } = new List<SchoolRegistration>();
    }
}
