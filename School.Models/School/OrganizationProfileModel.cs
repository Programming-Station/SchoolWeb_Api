using System;
using System.ComponentModel.DataAnnotations;

namespace School.Models.School
{
    public class OrganizationProfileModel
    {
        public int Id { get; set; }
        public int SchoolRegistrationId { get; set; }
        public int? BranchId { get; set; }

        [Required(ErrorMessage = "Organization Name is required")]
        [StringLength(200, ErrorMessage = "Organization Name cannot exceed 200 characters")]
        public string OrganizationName { get; set; } = string.Empty;

        [StringLength(50)]
        public string? ShortName { get; set; }

        [StringLength(200)]
        public string? DisplayName { get; set; }

        [StringLength(200)]
        public string? SchoolName { get; set; }

        [StringLength(200)]
        public string? CollegeName { get; set; }

        [StringLength(200)]
        public string? UniversityName { get; set; }

        [StringLength(200)]
        public string? CampusName { get; set; }

        [StringLength(100)]
        public string? AffiliationNumber { get; set; }

        [StringLength(100)]
        public string? RecognitionNumber { get; set; }

        [StringLength(100)]
        public string? SchoolCode { get; set; }

        [StringLength(100)]
        public string? CollegeCode { get; set; }

        [StringLength(100)]
        public string? UniversityCode { get; set; }

        [StringLength(200)]
        public string? Board { get; set; }

        [StringLength(200)]
        public string? University { get; set; }

        [StringLength(100)]
        public string? RegistrationNumber { get; set; }

        [StringLength(50)]
        public string? GSTNumber { get; set; }

        [StringLength(50)]
        public string? PANNumber { get; set; }

        [StringLength(50)]
        public string? TANNumber { get; set; }

        [StringLength(50)]
        public string? UDISENumber { get; set; }

        [StringLength(50)]
        public string? AISHECode { get; set; }

        [StringLength(500)]
        public string? AddressLine1 { get; set; }

        [StringLength(500)]
        public string? AddressLine2 { get; set; }

        [StringLength(200)]
        public string? Landmark { get; set; }

        [StringLength(100)]
        public string? City { get; set; }

        [StringLength(100)]
        public string? District { get; set; }

        [StringLength(100)]
        public string? State { get; set; }

        [StringLength(100)]
        public string? Country { get; set; }

        [StringLength(20)]
        public string? Pincode { get; set; }

        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        [StringLength(50)]
        public string? Phone { get; set; }

        [StringLength(50)]
        public string? Mobile { get; set; }

        [StringLength(50)]
        public string? WhatsApp { get; set; }

        [StringLength(200)]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string? Email { get; set; }

        [StringLength(200)]
        public string? Website { get; set; }

        [StringLength(200)]
        public string? HelpdeskEmail { get; set; }

        [StringLength(50)]
        public string? SupportPhone { get; set; }

        [StringLength(500)]
        public string? FacebookUrl { get; set; }

        [StringLength(500)]
        public string? InstagramUrl { get; set; }

        [StringLength(500)]
        public string? LinkedInUrl { get; set; }

        [StringLength(500)]
        public string? TwitterUrl { get; set; }

        [StringLength(500)]
        public string? YouTubeUrl { get; set; }

        [StringLength(500)]
        public string? Telegram { get; set; }

        [StringLength(200)]
        public string? PrincipalName { get; set; }

        [StringLength(200)]
        public string? DirectorName { get; set; }

        [StringLength(200)]
        public string? ChairmanName { get; set; }

        [StringLength(200)]
        public string? SecretaryName { get; set; }

        [StringLength(200)]
        public string? RegistrarName { get; set; }

        [StringLength(200)]
        public string? VicePrincipalName { get; set; }

        [StringLength(200)]
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

        [StringLength(50)]
        public string? BarcodePrefix { get; set; }

        [StringLength(50)]
        public string? PrimaryColor { get; set; }

        [StringLength(50)]
        public string? SecondaryColor { get; set; }

        [StringLength(50)]
        public string? Theme { get; set; }

        [StringLength(50)]
        public string? AccentColor { get; set; }

        [StringLength(100)]
        public string? FontFamily { get; set; }

        [StringLength(50)]
        public string? FontSize { get; set; }

        public bool? LightTheme { get; set; }
        public bool? DarkTheme { get; set; }

        [StringLength(100)]
        public string? CurrentAcademicSession { get; set; }

        [StringLength(100)]
        public string? FinancialYear { get; set; }

        [StringLength(100)]
        public string? TimeZone { get; set; }

        [StringLength(50)]
        public string? Currency { get; set; }

        [StringLength(50)]
        public string? DateFormat { get; set; }

        [StringLength(50)]
        public string? TimeFormat { get; set; }

        [StringLength(1000)]
        public string? ReportFooterText { get; set; }

        [StringLength(1000)]
        public string? CopyrightText { get; set; }

        [StringLength(2000)]
        public string? Disclaimer { get; set; }

        public string? TermsAndConditions { get; set; }

        public bool Status { get; set; } = true;
    }
}
