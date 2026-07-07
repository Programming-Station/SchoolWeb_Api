namespace School_DTOs.Email
{
    public class EmailBrandingDto
    {
        public int Id { get; set; }
        public int SchoolRegistrationId { get; set; }
        public string ThemeColor { get; set; } = string.Empty;
        public string? HeaderHtml { get; set; }
        public string? FooterHtml { get; set; }
        public string? SupportEmail { get; set; }
        public string? SupportPhone { get; set; }
        public string? PrincipalName { get; set; }
        public string CreatedDate { get; set; } = string.Empty;
        public string UpdatedDate { get; set; } = string.Empty;
        public string? CreatedBy { get; set; }
    }
}
