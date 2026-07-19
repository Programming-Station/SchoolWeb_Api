using System.ComponentModel.DataAnnotations;
using School.Domain.School;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;

namespace School.Domain.Reporting
{
    /// <summary>
    /// Defines the report type (orientation, format, data source).
    /// </summary>
    public enum ReportType
    {
        Tabular = 1,
        Summary = 2,
        Matrix = 3,
        Chart = 4,
        Certificate = 5,
        IdCard = 6,
        Invoice = 7,
        Label = 8,
        Dashboard = 9,
        Custom = 10
    }

    public enum PageOrientation
    {
        Portrait = 1,
        Landscape = 2
    }

    public enum PageSize
    {
        A4 = 1,
        A3 = 2,
        Letter = 3,
        Legal = 4,
        Custom = 5
    }

    public enum ReportFormat
    {
        PDF = 1,
        Excel = 2,
        Word = 3,
        CSV = 4,
        XML = 5,
        JSON = 6
    }

    /// <summary>
    /// Enterprise report template — the core registry of every report in the system.
    /// Replaces the thin Analytics.ReportTemplate with a full metadata model.
    /// </summary>
    [Table("ReportTemplates", Schema = "Reporting")]
    public class ReportTemplate : AuditEntity<int>, ITenantEntity{
        [Key]
        public int Id { get; set; }

        /// <summary>Foreign key to ReportCategory.</summary>
        public int? ReportCategoryId { get; set; }

        [ForeignKey(nameof(ReportCategoryId))]
        public virtual ReportCategory? ReportCategory { get; set; }

        /// <summary>Unique report code e.g. FEE_RECEIPT, STUDENT_MARKSHEET.</summary>
        [Required, MaxLength(100)]
        public string ReportCode { get; set; } = string.Empty;

        /// <summary>Display name shown in the report catalog.</summary>
        [Required, MaxLength(200)]
        public string ReportName { get; set; } = string.Empty;

        /// <summary>Short description of what this report contains.</summary>
        [MaxLength(1000)]
        public string? Description { get; set; }

        /// <summary>Report type enum — tabular, certificate, ID card, etc.</summary>
        public ReportType ReportType { get; set; } = ReportType.Tabular;

        /// <summary>Default export format when user clicks Generate.</summary>
        public ReportFormat DefaultFormat { get; set; } = ReportFormat.PDF;

        /// <summary>Page orientation for RDLC rendering.</summary>
        public PageOrientation PageOrientation { get; set; } = PageOrientation.Portrait;

        /// <summary>Page size for RDLC rendering.</summary>
        public PageSize PageSize { get; set; } = PageSize.A4;

        /// <summary>Custom page width in inches (used when PageSize = Custom).</summary>
        public decimal? CustomPageWidth { get; set; }

        /// <summary>Custom page height in inches (used when PageSize = Custom).</summary>
        public decimal? CustomPageHeight { get; set; }

        /// <summary>File name of the RDLC template (e.g. FeeReceipt.rdlc). Relative to Reports/RdlcFiles/.</summary>
        [MaxLength(255)]
        public string? RdlcFileName { get; set; }

        /// <summary>
        /// Optional: RDLC content stored in DB for tenant-custom overrides.
        /// When not null, this takes precedence over the file system RDLC.
        /// </summary>
        public byte[]? RdlcContent { get; set; }

        /// <summary>Sort order within the category.</summary>
        public int SortOrder { get; set; } = 0;

        /// <summary>System templates cannot be deleted, only customized.</summary>
        public bool IsSystem { get; set; } = false;

        /// <summary>Featured in user's favorites list.</summary>
        public bool IsFavorite { get; set; } = false;

        /// <summary>Whether this report appears in the report catalog.</summary>
        public bool IsVisible { get; set; } = true;

        /// <summary>Whether the report supports watermark.</summary>
        public bool HasWatermark { get; set; } = true;

        /// <summary>Whether the report header includes the organization logo.</summary>
        public bool HasLogo { get; set; } = true;

        /// <summary>Whether the report footer includes signature/seal.</summary>
        public bool HasSignature { get; set; } = false;

        /// <summary>Whether a QR code is embedded in the report.</summary>
        public bool HasQrCode { get; set; } = false;

        /// <summary>Whether a barcode is embedded in the report.</summary>
        public bool HasBarcode { get; set; } = false;

        [Required]
        public int SchoolRegistrationId { get; set; }

        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;

        /// <summary>Base64 thumbnail for the report catalog UI.</summary>
        public string? ThumbnailBase64 { get; set; }

        /// <summary>Module tag for filtering e.g. Fee, Admission, HR.</summary>
        [MaxLength(100)]
        public string? ModuleTag { get; set; }

        /// <summary>Tags for full-text search e.g. receipt,payment,student.</summary>
        [MaxLength(500)]
        public string? SearchTags { get; set; }

        /// <summary>Number of times this report has been generated.</summary>
        public int ExecutionCount { get; set; } = 0;

        // Navigation
        public virtual ICollection<ReportParameter> ReportParameters { get; set; } = new List<ReportParameter>();
        public virtual ICollection<ReportHistory> ReportHistories { get; set; } = new List<ReportHistory>();
        public virtual ICollection<ReportSchedule> ReportSchedules { get; set; } = new List<ReportSchedule>();
        public virtual ICollection<ReportPermission> ReportPermissions { get; set; } = new List<ReportPermission>();
    }
}


