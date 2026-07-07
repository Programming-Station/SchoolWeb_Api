using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using School.Domain.School;
using static School.Domain.BaseEntity;

namespace School.Domain.Email
{
    public class EmailLog : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int SchoolRegistrationId { get; set; }

        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;

        [MaxLength(200)]
        public string? TemplateName { get; set; }

        [Required, MaxLength(250)]
        public string RecipientEmail { get; set; } = string.Empty;

        [Required, MaxLength(300)]
        public string Subject { get; set; } = string.Empty;

        [Required]
        public string BodyHtml { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string Status { get; set; } = "Pending"; // Pending, Sent, Failed, Retrying

        public string? ErrorMessage { get; set; }

        public int RetryCount { get; set; }

        public DateTime? SentTime { get; set; }

        [MaxLength(500)]
        public string? SmtpResponse { get; set; }
    }
}
