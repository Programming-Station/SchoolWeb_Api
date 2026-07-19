using System.ComponentModel.DataAnnotations;
using School.Domain.School;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;

namespace School.Domain.Reporting
{
    /// <summary>
    /// Fine-grained execution telemetry for a single report generation event.
    /// Tracks each stage (data fetch, RDLC render, export, email) with timing.
    /// </summary>
    [Table("ReportExecutions")]
    public class ReportExecution : AuditEntity<int>, ITenantEntity{
        [Key]
        public int Id { get; set; }

        public int ReportHistoryId { get; set; }

        [ForeignKey(nameof(ReportHistoryId))]
        public virtual ReportHistory? ReportHistory { get; set; }

        [Required]
        public int SchoolRegistrationId { get; set; }

        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;

        public DateTime StartedAt { get; set; } = DateTime.UtcNow;

        public DateTime? CompletedAt { get; set; }

        /// <summary>Current pipeline stage e.g. DataFetch, RdlcRender, Export, Email, Done.</summary>
        [MaxLength(50)]
        public string ExecutionStage { get; set; } = "DataFetch";

        /// <summary>Number of data rows processed.</summary>
        public int RowsProcessed { get; set; } = 0;

        /// <summary>Peak memory allocated in bytes during execution.</summary>
        public long PeakMemoryBytes { get; set; } = 0;

        /// <summary>Time spent fetching data from the database (ms).</summary>
        public long DataFetchMs { get; set; } = 0;

        /// <summary>Time spent rendering the RDLC report (ms).</summary>
        public long RenderMs { get; set; } = 0;

        /// <summary>Time spent exporting to target format (ms).</summary>
        public long ExportMs { get; set; } = 0;

        /// <summary>Correlation ID for distributed tracing.</summary>
        [MaxLength(100)]
        public string? CorrelationId { get; set; }

        [MaxLength(200)]
        public string? ServerInstance { get; set; }

        public bool IsSuccess { get; set; } = false;

        [MaxLength(2000)]
        public string? ErrorDetail { get; set; }
    }
}


