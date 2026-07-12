using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;

namespace School.Domain.Analytics
{
    [Table("ReportTemplates", Schema = "Analytics")]
    public class ReportTemplate : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(150)]
        public string Name { get; set; } = null!;

        [Required, MaxLength(50)]
        public string Category { get; set; } = null!; // Finance, Academics, HR

        [Required, MaxLength(4000)]
        public string QueryDefinitionJson { get; set; } = null!;

        [Required, MaxLength(4000)]
        public string SelectedColumnsJson { get; set; } = null!;

        public int SchoolRegistrationId { get; set; }
    }
}
