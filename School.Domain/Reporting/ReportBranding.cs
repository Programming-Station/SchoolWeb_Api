using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using School.Domain.School;
using static School.Domain.BaseEntity;

namespace School.Domain.Reporting
{
    /// <summary>
    /// Tenant-specific organization branding configuration.
    /// Every report uses these settings for logo, signature, colors, watermark etc.
    /// One record per tenant (TenantId is unique).
    /// </summary>
    [Table("ReportBrandings", Schema = "Reporting")]
    public class ReportBranding : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int SchoolRegistrationId { get; set; }

        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;

        // ─── Identity ──────────────────────────────────────────────────────────

        [MaxLength(300)]
        public string? SchoolName { get; set; }

        [MaxLength(300)]
        public string? OrganizationName { get; set; }

        [MaxLength(500)]
        public string? TagLine { get; set; }

        [MaxLength(20)]
        public string? EstablishedYear { get; set; }

        // ─── Logos ─────────────────────────────────────────────────────────────

        /// <summary>Primary header logo path or base64.</summary>
        [MaxLength(1000)]
        public string? HeaderLogo { get; set; }

        /// <summary>Footer logo path or base64.</summary>
        [MaxLength(1000)]
        public string? FooterLogo { get; set; }

        /// <summary>Light version logo (for dark backgrounds).</summary>
        [MaxLength(1000)]
        public string? LogoLight { get; set; }

        /// <summary>Dark version logo (for light backgrounds).</summary>
        [MaxLength(1000)]
        public string? LogoDark { get; set; }

        // ─── Contact ───────────────────────────────────────────────────────────

        [MaxLength(500)]
        public string? AddressLine1 { get; set; }

        [MaxLength(500)]
        public string? AddressLine2 { get; set; }

        [MaxLength(100)]
        public string? City { get; set; }

        [MaxLength(100)]
        public string? State { get; set; }

        [MaxLength(20)]
        public string? PinCode { get; set; }

        [MaxLength(50)]
        public string? Phone { get; set; }

        [MaxLength(50)]
        public string? Mobile { get; set; }

        [MaxLength(200)]
        public string? Email { get; set; }

        [MaxLength(300)]
        public string? Website { get; set; }

        [MaxLength(100)]
        public string? AffiliationNumber { get; set; }

        [MaxLength(100)]
        public string? RegistrationNumber { get; set; }

        // ─── Authorities ───────────────────────────────────────────────────────

        [MaxLength(200)]
        public string? PrincipalName { get; set; }

        [MaxLength(1000)]
        public string? PrincipalSignature { get; set; }

        [MaxLength(200)]
        public string? DirectorName { get; set; }

        [MaxLength(1000)]
        public string? DirectorSignature { get; set; }

        [MaxLength(200)]
        public string? ChairmanName { get; set; }

        [MaxLength(1000)]
        public string? ChairmanSignature { get; set; }

        [MaxLength(1000)]
        public string? OfficialSeal { get; set; }

        [MaxLength(1000)]
        public string? DigitalSignature { get; set; }

        // ─── Report Appearance ────────────────────────────────────────────────

        [MaxLength(1000)]
        public string? ReportWatermark { get; set; }

        /// <summary>Watermark text (alternative to image watermark).</summary>
        [MaxLength(200)]
        public string? WatermarkText { get; set; }

        [MaxLength(1000)]
        public string? ReportFooterText { get; set; }

        [MaxLength(500)]
        public string? CopyrightText { get; set; }

        [MaxLength(2000)]
        public string? Disclaimer { get; set; }

        [MaxLength(5000)]
        public string? TermsAndConditions { get; set; }

        // ─── Theme ─────────────────────────────────────────────────────────────

        [MaxLength(20)]
        public string? PrimaryColor { get; set; } = "#1e3a8a";

        [MaxLength(20)]
        public string? SecondaryColor { get; set; } = "#2563eb";

        [MaxLength(20)]
        public string? AccentColor { get; set; } = "#f59e0b";

        [MaxLength(100)]
        public string? FontFamily { get; set; } = "Arial";

        // ─── Page Settings ─────────────────────────────────────────────────────

        public decimal ReportMarginTop { get; set; } = 0.5m;
        public decimal ReportMarginBottom { get; set; } = 0.5m;
        public decimal ReportMarginLeft { get; set; } = 0.75m;
        public decimal ReportMarginRight { get; set; } = 0.75m;

        // ─── QR / Barcode ─────────────────────────────────────────────────────

        /// <summary>Base URL for QR-based online verification (e.g. https://school.com/verify).</summary>
        [MaxLength(500)]
        public string? QrVerificationBaseUrl { get; set; }

        /// <summary>Prefix prepended to barcodes e.g. SCH001.</summary>
        [MaxLength(50)]
        public string? BarcodePrefix { get; set; }

        // ─── Academic ─────────────────────────────────────────────────────────

        [MaxLength(50)]
        public string? CurrentAcademicSession { get; set; }

        [MaxLength(200)]
        public string? CampusName { get; set; }
    }
}


