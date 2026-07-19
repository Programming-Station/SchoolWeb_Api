using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using School.Domain.School;
using School.Domain.Communication.Recipients;
using static School.Domain.BaseEntity;

namespace School.Domain.Email
{
    [Table("EmailLogs", Schema = "Communication")]
    public class EmailLog : AuditEntity<int>, ITenantEntity
    {
        public EmailLog()
        {
            EmailRecipients = new HashSet<EmailRecipient>();
            EmailAttachments = new HashSet<EmailAttachment>();
        }

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
        public string Status { get; set; } = "Pending"; // Pending, Sent, Failed, Retrying, Draft

        public string? ErrorMessage { get; set; }

        public int RetryCount { get; set; }

        public DateTime? SentTime { get; set; }

        [MaxLength(500)]
        public string? SmtpResponse { get; set; }

        // Compose / Draft / Scheduled Email Features
        public bool IsDraft { get; set; } = false;

        public bool IsScheduled { get; set; } = false;

        public DateTime? ScheduledTime { get; set; }

        // Navigation
        public virtual ICollection<EmailRecipient> EmailRecipients { get; set; }
        public virtual ICollection<EmailAttachment> EmailAttachments { get; set; }
    }
}
