using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;

namespace School.Domain.Communication
{
    [Table("CommunicationTemplates", Schema = "Communication")]
    public class CommunicationTemplate : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = null!;

        [Required, MaxLength(20)]
        public string Type { get; set; } = "Email"; // Email, SMS, WhatsApp, Push

        [MaxLength(250)]
        public string? SubjectTemplate { get; set; } // Only relevant for email/push

        [Required, MaxLength(4000)]
        public string BodyTemplate { get; set; } = null!;

        public bool IsActive { get; set; } = true;

        public int SchoolRegistrationId { get; set; }
    }
}
