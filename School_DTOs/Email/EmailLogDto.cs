namespace School_DTOs.Email
{
    public class EmailLogDto
    {
        public int Id { get; set; }
        public int SchoolRegistrationId { get; set; }
        public string? TemplateName { get; set; }
        public string RecipientEmail { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string? ErrorMessage { get; set; }
        public int RetryCount { get; set; }
        public string? SentTime { get; set; }
        public string? SmtpResponse { get; set; }
        public string CreatedDate { get; set; } = string.Empty;
    }
}
