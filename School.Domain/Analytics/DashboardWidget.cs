using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;

namespace School.Domain.Analytics
{
    [Table("DashboardWidgets", Schema = "Analytics")]
    public class DashboardWidget : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int DashboardConfigId { get; set; }

        [ForeignKey(nameof(DashboardConfigId))]
        public virtual DashboardConfig DashboardConfig { get; set; } = null!;

        [Required, MaxLength(100)]
        public string Title { get; set; } = null!;

        [Required, MaxLength(50)]
        public string WidgetType { get; set; } = null!; // BarChart, MetricCard, Grid

        [Required, MaxLength(500)]
        public string SourceApiUrl { get; set; } = null!;

        [MaxLength(4000)]
        public string? ConfigJson { get; set; }

        public int SchoolRegistrationId { get; set; }
    }
}
