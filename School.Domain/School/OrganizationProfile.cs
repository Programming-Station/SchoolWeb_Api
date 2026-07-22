using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;

namespace School.Domain.School
{
    [Table("OrganizationProfiles", Schema = "School")]
    public class OrganizationProfile : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        public int SchoolRegistrationId { get; set; }
        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;

        public int? BranchId { get; set; }

        [Required, MaxLength(200)]
        public string OrganizationName { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? ShortName { get; set; }

        [MaxLength(200)]
        public string? DisplayName { get; set; }

        [MaxLength(200)]
        public string? SchoolName { get; set; }

        [MaxLength(200)]
        public string? CollegeName { get; set; }

        [MaxLength(200)]
        public string? UniversityName { get; set; }

        [MaxLength(200)]
        public string? CampusName { get; set; }

        [MaxLength(100)]
        public string? AffiliationNumber { get; set; }

        [MaxLength(100)]
        public string? RecognitionNumber { get; set; }

        [MaxLength(100)]
        public string? SchoolCode { get; set; }

        [MaxLength(100)]
        public string? CollegeCode { get; set; }

        [MaxLength(100)]
        public string? UniversityCode { get; set; }

        [MaxLength(200)]
        public string? Board { get; set; }

        [MaxLength(200)]
        public string? University { get; set; }

        [MaxLength(100)]
        public string? RegistrationNumber { get; set; }

        [MaxLength(50)]
        public string? GSTNumber { get; set; }

        [MaxLength(50)]
        public string? PANNumber { get; set; }

        [MaxLength(50)]
        public string? TANNumber { get; set; }

        [MaxLength(50)]
        public string? UDISENumber { get; set; }

        [MaxLength(50)]
        public string? AISHECode { get; set; }

        [MaxLength(500)]
        public string? AddressLine1 { get; set; }

        [MaxLength(500)]
        public string? AddressLine2 { get; set; }

        [MaxLength(200)]
        public string? Landmark { get; set; }

        [MaxLength(100)]
        public string? City { get; set; }

        [MaxLength(100)]
        public string? District { get; set; }

        [MaxLength(100)]
        public string? State { get; set; }

        [MaxLength(100)]
        public string? Country { get; set; }

        [MaxLength(20)]
        public string? Pincode { get; set; }

        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        [MaxLength(50)]
        public string? Phone { get; set; }

        [MaxLength(50)]
        public string? Mobile { get; set; }

        [MaxLength(50)]
        public string? WhatsApp { get; set; }

        [MaxLength(200)]
        public string? Email { get; set; }

        [MaxLength(200)]
        public string? Website { get; set; }

        [MaxLength(200)]
        public string? HelpdeskEmail { get; set; }

        [MaxLength(50)]
        public string? SupportPhone { get; set; }

        [MaxLength(500)]
        public string? FacebookUrl { get; set; }

        [MaxLength(500)]
        public string? InstagramUrl { get; set; }

        [MaxLength(500)]
        public string? LinkedInUrl { get; set; }

        [MaxLength(500)]
        public string? TwitterUrl { get; set; }

        [MaxLength(500)]
        public string? YouTubeUrl { get; set; }

        [MaxLength(500)]
        public string? Telegram { get; set; }

        [MaxLength(200)]
        public string? PrincipalName { get; set; }

        [MaxLength(200)]
        public string? DirectorName { get; set; }

        [MaxLength(200)]
        public string? ChairmanName { get; set; }

        [MaxLength(200)]
        public string? SecretaryName { get; set; }

        [MaxLength(200)]
        public string? RegistrarName { get; set; }

        [MaxLength(200)]
        public string? VicePrincipalName { get; set; }

        [MaxLength(200)]
        public string? AccountantName { get; set; }

        public string? ChairmanSignature { get; set; }
        public string? DirectorSignature { get; set; }
        public string? PrincipalSignature { get; set; }
        public string? RegistrarSignature { get; set; }
        public string? DigitalSignature { get; set; }
        public string? OfficialSeal { get; set; }
        public string? RoundSeal { get; set; }
        public string? RoundStamp { get; set; }
        public string? SquareStamp { get; set; }
        public string? RectangleSeal { get; set; }

        public string? LogoLight { get; set; }
        public string? LogoDark { get; set; }
        public string? HeaderLogo { get; set; }
        public string? FooterLogo { get; set; }
        public string? LoginLogo { get; set; }
        public string? Favicon { get; set; }
        public string? MobileAppIcon { get; set; }
        public string? SplashScreenLogo { get; set; }
        public string? EmailLogo { get; set; }
        public string? PDFLogo { get; set; }
        public string? ReportWatermark { get; set; }
        public string? ReportBackground { get; set; }
        public string? QRCode { get; set; }

        [MaxLength(50)]
        public string? BarcodePrefix { get; set; }

        [MaxLength(50)]
        public string? PrimaryColor { get; set; }

        [MaxLength(50)]
        public string? SecondaryColor { get; set; }

        [MaxLength(50)]
        public string? Theme { get; set; }

        [MaxLength(50)]
        public string? AccentColor { get; set; }

        [MaxLength(100)]
        public string? FontFamily { get; set; }

        [MaxLength(50)]
        public string? FontSize { get; set; }

        public bool? LightTheme { get; set; }
        public bool? DarkTheme { get; set; }

        [MaxLength(100)]
        public string? CurrentAcademicSession { get; set; }

        [MaxLength(100)]
        public string? FinancialYear { get; set; }

        [MaxLength(100)]
        public string? TimeZone { get; set; }

        [MaxLength(50)]
        public string? Currency { get; set; }

        [MaxLength(50)]
        public string? DateFormat { get; set; }

        [MaxLength(50)]
        public string? TimeFormat { get; set; }

        [MaxLength(1000)]
        public string? ReportFooterText { get; set; }

        [MaxLength(1000)]
        public string? CopyrightText { get; set; }

        [MaxLength(2000)]
        public string? Disclaimer { get; set; }

        public string? TermsAndConditions { get; set; }

        public bool Status { get; set; } = true;
    }
}
