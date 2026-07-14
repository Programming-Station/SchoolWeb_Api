namespace School_DTOs.School
{
    public class ThemeSettingsDto
    {
        public string? PrimaryColor { get; set; }
        public string? SecondaryColor { get; set; }
        public string? AccentColor { get; set; }
        public string? FontFamily { get; set; }
        public string? FontSize { get; set; }
        public string? Theme { get; set; }
        public bool? LightTheme { get; set; }
        public bool? DarkTheme { get; set; }
    }

    public class LogoSettingsDto
    {
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
    }

    public class SignatureSettingsDto
    {
        public string? ChairmanSignature { get; set; }
        public string? DirectorSignature { get; set; }
        public string? PrincipalSignature { get; set; }
        public string? RegistrarSignature { get; set; }
        public string? DigitalSignature { get; set; }
        public string? OfficialSeal { get; set; }
        public string? RoundSeal { get; set; }
        public string? RectangleSeal { get; set; }
    }

    public class WatermarkSettingsDto
    {
        public string? ReportWatermark { get; set; }
        public string? ReportBackground { get; set; }
        public string? WatermarkImage { get; set; }
    }
}
