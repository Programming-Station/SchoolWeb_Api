using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using School.Domain.School;
using static School.Domain.BaseEntity;

namespace School.Domain.Reporting
{
    /// <summary>
    /// Database-driven report schedule.
    /// Each record defines a cron-based schedule that auto-generates and emails a report.
    /// </summary>
    [Table("ReportSchedules", Schema = "Reporting")]
    public class ReportSchedule : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int SchoolRegistrationId { get; set; }

        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;

        public int ReportTemplateId { get; set; }

        [ForeignKey(nameof(ReportTemplateId))]
        public virtual ReportTemplate? ReportTemplate { get; set; }

        [Required, MaxLength(200)]
        public string ScheduleName { get; set; } = string.Empty;

        /// <summary>
        /// Standard 5-field cron expression e.g. "0 17 * * 5" = every Friday at 5 PM.
        /// Also supports friendly presets: DAILY_5PM, WEEKLY_FRIDAY, MONTHLY_1ST.
        /// </summary>
        [Required, MaxLength(100)]
        public string CronExpression { get; set; } = string.Empty;

        /// <summary>Semicolon-separated email addresses for delivery.</summary>
        [Required, MaxLength(2000)]
        public string RecipientEmails { get; set; } = string.Empty;

        /// <summary>CC recipients (semicolon-separated).</summary>
        [MaxLength(2000)]
        public string? CcEmails { get; set; }

        /// <summary>BCC recipients (semicolon-separated).</summary>
        [MaxLength(2000)]
        public string? BccEmails { get; set; }

        /// <summary>Export format for the scheduled report.</summary>
        [MaxLength(20)]
        public string Format { get; set; } = "PDF";

        /// <summary>JSON snapshot of parameters to use when auto-generating.</summary>
        public string? ParametersJson { get; set; }

        /// <summary>Email subject template. Supports {ReportName}, {Date} placeholders.</summary>
        [MaxLength(500)]
        public string? EmailSubjectTemplate { get; set; }

        /// <summary>Email body template.</summary>
        public string? EmailBodyTemplate { get; set; }

        public DateTime? LastRunAt { get; set; }

        public DateTime? NextRunAt { get; set; }

        [MaxLength(20)]
        public string LastRunStatus { get; set; } = "Pending";

        /// <summary>Number of consecutive failures.</summary>
        public int FailureCount { get; set; } = 0;

        [MaxLength(2000)]
        public string? LastError { get; set; }

        /// <summary>Maximum retry attempts on failure.</summary>
        public int MaxRetries { get; set; } = 3;

        public override bool IsActive { get; set; } = true;
    }
}


