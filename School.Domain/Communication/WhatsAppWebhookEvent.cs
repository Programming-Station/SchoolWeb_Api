using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;

namespace School.Domain.Communication
{
    [Table("WhatsAppWebhookEvents", Schema = "Communication")]
    public class WhatsAppWebhookEvent : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string EventType { get; set; } = null!;

        [Required, MaxLength(8000)]
        public string Payload { get; set; } = null!; // Raw JSON string

        public bool Processed { get; set; } = false;

        public DateTime ReceivedDate { get; set; } = DateTime.UtcNow;

        public int SchoolRegistrationId { get; set; }
    }
}
