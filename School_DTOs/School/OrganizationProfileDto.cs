namespace School_DTOs.School
{
    public class OrganizationProfileDto
    {
        public int Id { get; set; }
        public int SchoolRegistrationId { get; set; }
        public int? BranchId { get; set; }

        public string OrganizationName { get; set; } = string.Empty;
        public string? ShortName { get; set; }
        public string? DisplayName { get; set; }
        public string? SchoolName { get; set; }
        public string? CollegeName { get; set; }
        public string? UniversityName { get; set; }
        public string? CampusName { get; set; }

        public string? AffiliationNumber { get; set; }
        public string? RecognitionNumber { get; set; }
        public string? SchoolCode { get; set; }
        public string? CollegeCode { get; set; }
        public string? UniversityCode { get; set; }

        public string? Board { get; set; }
        public string? University { get; set; }
        public string? RegistrationNumber { get; set; }
        public string? GSTNumber { get; set; }
        public string? PANNumber { get; set; }
        public string? TANNumber { get; set; }
        public string? UDISENumber { get; set; }
        public string? AISHECode { get; set; }

        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? Landmark { get; set; }
        public string? City { get; set; }
        public string? District { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public string? Pincode { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        public string? Phone { get; set; }
        public string? Mobile { get; set; }
        public string? WhatsApp { get; set; }
        public string? Email { get; set; }
        public string? Website { get; set; }
        public string? HelpdeskEmail { get; set; }
        public string? SupportPhone { get; set; }

        public string? FacebookUrl { get; set; }
        public string? InstagramUrl { get; set; }
        public string? LinkedInUrl { get; set; }
        public string? TwitterUrl { get; set; }
        public string? YouTubeUrl { get; set; }
        public string? Telegram { get; set; }

        public string? PrincipalName { get; set; }
        public string? DirectorName { get; set; }
        public string? ChairmanName { get; set; }
        public string? SecretaryName { get; set; }
        public string? RegistrarName { get; set; }
        public string? VicePrincipalName { get; set; }
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
        public string? BarcodePrefix { get; set; }

        public string? PrimaryColor { get; set; }
        public string? SecondaryColor { get; set; }
        public string? Theme { get; set; }
        public string? AccentColor { get; set; }
        public string? FontFamily { get; set; }
        public string? FontSize { get; set; }
        public bool? LightTheme { get; set; }
        public bool? DarkTheme { get; set; }

        public string? CurrentAcademicSession { get; set; }
        public string? FinancialYear { get; set; }
        public string? TimeZone { get; set; }
        public string? Currency { get; set; }
        public string? DateFormat { get; set; }
        public string? TimeFormat { get; set; }

        public string? ReportFooterText { get; set; }
        public string? CopyrightText { get; set; }
        public string? Disclaimer { get; set; }
        public string? TermsAndConditions { get; set; }

        public bool Status { get; set; }
    }
}
