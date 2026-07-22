using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using School.Domain.School;
using static School.Domain.BaseEntity;

namespace School.Domain.Reporting
{
    /// <summary>Status of a report generation execution.</summary>
    public enum ReportExecutionStatus
    {
        Pending = 0,
        Running = 1,
        Success = 2,
        Failed = 3,
        Cancelled = 4
    }

    /// <summary>
    /// Audit log of every report generation event in the system.
    /// Supports history browsing, re-download, and usage analytics.
    /// </summary>
    [Table("ReportHistories", Schema = "Reporting")]
    public class ReportHistory : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int SchoolRegistrationId { get; set; }

        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;

        public int? ReportTemplateId { get; set; }

        [ForeignKey(nameof(ReportTemplateId))]
        public virtual ReportTemplate? ReportTemplate { get; set; }

        /// <summary>Snapshot of report name at the time of generation.</summary>
        [Required, MaxLength(200)]
        public string ReportName { get; set; } = string.Empty;

        /// <summary>User who triggered the report generation.</summary>
        [MaxLength(200)]
        public string? GeneratedBy { get; set; }

        public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;

        /// <summary>Export format used: PDF, EXCEL, WORD, CSV, XML, JSON.</summary>
        [MaxLength(20)]
        public string Format { get; set; } = "PDF";

        /// <summary>Size of the generated file in bytes.</summary>
        public long? FileSizeBytes { get; set; }

        /// <summary>Total execution time in milliseconds.</summary>
        public long? ExecutionMs { get; set; }

        /// <summary>JSON snapshot of parameters used.</summary>
        public string? ParametersJson { get; set; }

        public ReportExecutionStatus Status { get; set; } = ReportExecutionStatus.Pending;

        [MaxLength(2000)]
        public string? ErrorMessage { get; set; }

        // ─── Usage tracking ────────────────────────────────────────────────────

        /// <summary>Whether the report was emailed to recipients.</summary>
        public bool IsEmailed { get; set; } = false;

        /// <summary>Email recipients (semicolon separated).</summary>
        [MaxLength(1000)]
        public string? EmailedTo { get; set; }

        public DateTime? EmailedAt { get; set; }

        /// <summary>Whether the report was downloaded.</summary>
        public bool IsDownloaded { get; set; } = false;

        public int DownloadCount { get; set; } = 0;

        public DateTime? LastDownloadAt { get; set; }

        /// <summary>Whether a print event was logged.</summary>
        public bool IsPrinted { get; set; } = false;

        public int PrintCount { get; set; } = 0;

        /// <summary>Optional path if report is persisted to disk for re-download.</summary>
        [MaxLength(500)]
        public string? FileStoragePath { get; set; }

        /// <summary>Number of data rows returned by the report query.</summary>
        public int? RowCount { get; set; }

        /// <summary>Whether this was triggered by a scheduled job (vs. manual).</summary>
        public bool IsScheduled { get; set; } = false;

        public int? ReportScheduleId { get; set; }

        // Navigation
        public virtual ICollection<ReportExecution> Executions { get; set; } = new List<ReportExecution>();
    }
}


