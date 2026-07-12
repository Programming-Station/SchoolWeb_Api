using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;

namespace School.Domain.Analytics
{
    [Table("AnalyticsKpis", Schema = "Analytics")]
    public class AnalyticsKpi : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string Code { get; set; } = null!; // e.g. FEE_COLL_EFF

        [Required, MaxLength(150)]
        public string Name { get; set; } = null!;

        [MaxLength(1000)]
        public string? CalculationFormula { get; set; }

        public decimal TargetValue { get; set; }

        public decimal CurrentValue { get; set; }

        public int SchoolRegistrationId { get; set; }
    }
}
