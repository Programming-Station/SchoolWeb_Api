using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;

namespace School.Domain.Analytics
{
    [Table("DashboardConfigs", Schema = "Analytics")]
    public class DashboardConfig : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(150)]
        public string Name { get; set; } = null!;

        [MaxLength(100)]
        public string? RoleScope { get; set; } // Admin, Teacher, Parent

        public bool IsDefault { get; set; } = false;

        public int SchoolRegistrationId { get; set; }

        public virtual ICollection<DashboardWidget> Widgets { get; set; } = new List<DashboardWidget>();
    }
}
