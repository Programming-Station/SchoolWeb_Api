namespace School_DTOs.Email
{
    public class EmailTemplateDto
    {
        public int Id { get; set; }
        public int SchoolRegistrationId { get; set; }
        public string TemplateName { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string BodyHtml { get; set; } = string.Empty;
        public string? Placeholder { get; set; }
        public bool IsActive { get; set; }
        public int? EmailServerSettingId { get; set; }
        public string CreatedDate { get; set; } = string.Empty;
        public string UpdatedDate { get; set; } = string.Empty;
        public string? CreatedBy { get; set; }
    }
}
