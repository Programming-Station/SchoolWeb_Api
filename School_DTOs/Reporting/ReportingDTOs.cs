using System.ComponentModel.DataAnnotations;

namespace School_DTOs.Reporting
{
    // ─── Request DTOs ──────────────────────────────────────────────────────────

    public class ReportExecutionRequest
    {
        /// <summary>Report code or template ID.</summary>
        [Required]
        public string ReportCode { get; set; } = string.Empty;

        public int? ReportTemplateId { get; set; }

        /// <summary>Export format: PDF, EXCEL, WORD, CSV, XML, JSON.</summary>
        public string Format { get; set; } = "PDF";

        /// <summary>Dynamic parameters as key-value pairs.</summary>
        public Dictionary<string, string> Parameters { get; set; } = new();

        /// <summary>Tenant ID override (for admin impersonation).</summary>
        public int? TenantId { get; set; }
    }

    public class ReportEmailRequest
    {
        [Required]
        public string ReportCode { get; set; } = string.Empty;

        public int? ReportTemplateId { get; set; }

        public string Format { get; set; } = "PDF";

        public Dictionary<string, string> Parameters { get; set; } = new();

        [Required]
        public List<string> ToEmails { get; set; } = new();

        public List<string> CcEmails { get; set; } = new();

        public List<string> BccEmails { get; set; } = new();

        public string? Subject { get; set; }

        public string? Body { get; set; }
    }

    public class ReportPrintRequest
    {
        public int ReportHistoryId { get; set; }
        public string PrintedBy { get; set; } = string.Empty;
        public string? PrinterName { get; set; }
    }

    // ─── Category DTOs ─────────────────────────────────────────────────────────

    public class ReportCategoryDto
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? IconClass { get; set; }
        public string? ColorHex { get; set; }
        public string? Description { get; set; }
        public int SortOrder { get; set; }
        public int ReportCount { get; set; }
        public bool IsActive { get; set; }
    }

    public class CreateReportCategoryRequest
    {
        [Required, MaxLength(50)]
        public string Code { get; set; } = string.Empty;

        [Required, MaxLength(150)]
        public string Name { get; set; } = string.Empty;

        public string? IconClass { get; set; }
        public string? ColorHex { get; set; }
        public string? Description { get; set; }
        public int SortOrder { get; set; } = 0;
    }

    // ─── Template DTOs ─────────────────────────────────────────────────────────

    public class ReportTemplateDto
    {
        public int Id { get; set; }
        public int? ReportCategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string? CategoryIcon { get; set; }
        public string? CategoryColor { get; set; }
        public string ReportCode { get; set; } = string.Empty;
        public string ReportName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string ReportType { get; set; } = string.Empty;
        public string DefaultFormat { get; set; } = "PDF";
        public string PageOrientation { get; set; } = "Portrait";
        public string PageSize { get; set; } = "A4";
        public string? RdlcFileName { get; set; }
        public bool IsSystem { get; set; }
        public bool IsFavorite { get; set; }
        public bool IsVisible { get; set; }
        public bool HasWatermark { get; set; }
        public bool HasLogo { get; set; }
        public bool HasSignature { get; set; }
        public bool HasQrCode { get; set; }
        public bool HasBarcode { get; set; }
        public string? ModuleTag { get; set; }
        public string? ThumbnailBase64 { get; set; }
        public int ExecutionCount { get; set; }
        public List<ReportParameterDto> Parameters { get; set; } = new();
    }

    public class CreateReportTemplateRequest
    {
        [Required]
        public string ReportCode { get; set; } = string.Empty;

        [Required]
        public string ReportName { get; set; } = string.Empty;

        public int? ReportCategoryId { get; set; }
        public string? Description { get; set; }
        public string ReportType { get; set; } = "Tabular";
        public string DefaultFormat { get; set; } = "PDF";
        public string PageOrientation { get; set; } = "Portrait";
        public string PageSize { get; set; } = "A4";
        public string? RdlcFileName { get; set; }
        public bool HasWatermark { get; set; } = true;
        public bool HasLogo { get; set; } = true;
        public bool HasSignature { get; set; } = false;
        public bool HasQrCode { get; set; } = false;
        public bool HasBarcode { get; set; } = false;
        public string? ModuleTag { get; set; }
        public string? SearchTags { get; set; }
        public int SortOrder { get; set; } = 0;
        public List<ReportParameterDto> Parameters { get; set; } = new();
    }

    // ─── Parameter DTOs ────────────────────────────────────────────────────────

    public class ReportParameterDto
    {
        public int Id { get; set; }
        public int ReportTemplateId { get; set; }
        public string ParameterName { get; set; } = string.Empty;
        public string DisplayLabel { get; set; } = string.Empty;
        public string DataType { get; set; } = "String";
        public bool IsRequired { get; set; }
        public string? DefaultValue { get; set; }
        public string? EnumValuesJson { get; set; }
        public string? LookupApiEndpoint { get; set; }
        public string? PlaceholderText { get; set; }
        public string? HelpText { get; set; }
        public int SortOrder { get; set; }
        public string? DependsOnParameter { get; set; }
        public string? DependsOnValue { get; set; }
        // Runtime value (filled by user in parameter dialog)
        public string? Value { get; set; }
    }

    // ─── History DTOs ──────────────────────────────────────────────────────────

    public class ReportHistoryDto
    {
        public int Id { get; set; }
        public int? ReportTemplateId { get; set; }
        public string ReportName { get; set; } = string.Empty;
        public string? GeneratedBy { get; set; }
        public DateTime GeneratedAt { get; set; }
        public string Format { get; set; } = string.Empty;
        public long? FileSizeBytes { get; set; }
        public long? ExecutionMs { get; set; }
        public string Status { get; set; } = string.Empty;
        public bool IsEmailed { get; set; }
        public bool IsDownloaded { get; set; }
        public bool IsPrinted { get; set; }
        public int DownloadCount { get; set; }
        public bool IsScheduled { get; set; }
        public int? RowCount { get; set; }
        public string? ErrorMessage { get; set; }
    }

    public class ReportHistoryPagedResult
    {
        public List<ReportHistoryDto> Items { get; set; } = new();
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }

    // ─── Schedule DTOs ─────────────────────────────────────────────────────────

    public class ReportScheduleDto
    {
        public int Id { get; set; }
        public int ReportTemplateId { get; set; }
        public string? ReportName { get; set; }
        public string ScheduleName { get; set; } = string.Empty;
        public string CronExpression { get; set; } = string.Empty;
        public string RecipientEmails { get; set; } = string.Empty;
        public string? CcEmails { get; set; }
        public string Format { get; set; } = "PDF";
        public Dictionary<string, string> Parameters { get; set; } = new();
        public DateTime? LastRunAt { get; set; }
        public DateTime? NextRunAt { get; set; }
        public string LastRunStatus { get; set; } = "Pending";
        public int FailureCount { get; set; }
        public bool IsActive { get; set; }
    }

    public class CreateReportScheduleRequest
    {
        [Required]
        public int ReportTemplateId { get; set; }

        [Required]
        public string ScheduleName { get; set; } = string.Empty;

        [Required]
        public string CronExpression { get; set; } = string.Empty;

        [Required]
        public string RecipientEmails { get; set; } = string.Empty;

        public string? CcEmails { get; set; }
        public string? BccEmails { get; set; }
        public string Format { get; set; } = "PDF";
        public Dictionary<string, string> Parameters { get; set; } = new();
        public string? EmailSubjectTemplate { get; set; }
        public string? EmailBodyTemplate { get; set; }
    }

    // ─── Permission DTOs ───────────────────────────────────────────────────────

    public class ReportPermissionDto
    {
        public int Id { get; set; }
        public int ReportTemplateId { get; set; }
        public string? ReportName { get; set; }
        public string RoleName { get; set; } = string.Empty;
        public bool CanView { get; set; }
        public bool CanExportPdf { get; set; }
        public bool CanExportExcel { get; set; }
        public bool CanExportWord { get; set; }
        public bool CanExportCsv { get; set; }
        public bool CanPrint { get; set; }
        public bool CanEmail { get; set; }
        public bool CanSchedule { get; set; }
        public bool CanManageTemplate { get; set; }
    }

    // ─── Branding DTOs ─────────────────────────────────────────────────────────

    public class ReportBrandingDto
    {
        public int Id { get; set; }
        public int TenantId { get; set; }
        public string? SchoolName { get; set; }
        public string? OrganizationName { get; set; }
        public string? TagLine { get; set; }
        public string? EstablishedYear { get; set; }
        public string? HeaderLogo { get; set; }
        public string? FooterLogo { get; set; }
        public string? LogoLight { get; set; }
        public string? LogoDark { get; set; }
        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? PinCode { get; set; }
        public string? Phone { get; set; }
        public string? Mobile { get; set; }
        public string? Email { get; set; }
        public string? Website { get; set; }
        public string? AffiliationNumber { get; set; }
        public string? RegistrationNumber { get; set; }
        public string? PrincipalName { get; set; }
        public string? PrincipalSignature { get; set; }
        public string? DirectorName { get; set; }
        public string? DirectorSignature { get; set; }
        public string? OfficialSeal { get; set; }
        public string? DigitalSignature { get; set; }
        public string? ReportWatermark { get; set; }
        public string? WatermarkText { get; set; }
        public string? ReportFooterText { get; set; }
        public string? CopyrightText { get; set; }
        public string? Disclaimer { get; set; }
        public string? PrimaryColor { get; set; }
        public string? SecondaryColor { get; set; }
        public string? AccentColor { get; set; }
        public string? FontFamily { get; set; }
        public decimal ReportMarginTop { get; set; }
        public decimal ReportMarginBottom { get; set; }
        public decimal ReportMarginLeft { get; set; }
        public decimal ReportMarginRight { get; set; }
        public string? QrVerificationBaseUrl { get; set; }
        public string? BarcodePrefix { get; set; }
        public string? CurrentAcademicSession { get; set; }
        public string? CampusName { get; set; }
    }

    // ─── QR / Barcode DTOs ─────────────────────────────────────────────────────

    public class QrCodeRequest
    {
        [Required]
        public string Data { get; set; } = string.Empty;
        public int Size { get; set; } = 250;
        public bool AsBase64 { get; set; } = false;
    }

    public class BarcodeRequest
    {
        [Required]
        public string Data { get; set; } = string.Empty;
        public string Format { get; set; } = "Code128"; // Code128, EAN13, QR, Code39
        public int Width { get; set; } = 300;
        public int Height { get; set; } = 100;
        public bool AsBase64 { get; set; } = false;
    }
}
