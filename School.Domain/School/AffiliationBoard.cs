using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static School.Domain.BaseEntity;

namespace School.Domain.School
{
    public class AffiliationBoard : AuditEntity<int>
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
