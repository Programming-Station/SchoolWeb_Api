using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;

namespace School.Domain.School
{
    [Table("OrganizationProfileAudits", Schema = "School")]
    public class OrganizationProfileAudit : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        public int SchoolRegistrationId { get; set; } // acts as TenantId

        [MaxLength(200)]
        public string? Action { get; set; }

        [MaxLength(200)]
        public string? PropertyName { get; set; }

        public string? OldValue { get; set; }
        public string? NewValue { get; set; }

        [MaxLength(500)]
        public string? Reason { get; set; }

        [MaxLength(200)]
        public string? PerformedBy { get; set; }

        public DateTime PerformedDate { get; set; } = DateTime.UtcNow;
    }
}
