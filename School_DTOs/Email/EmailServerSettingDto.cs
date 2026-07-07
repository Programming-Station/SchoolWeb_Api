namespace School_DTOs.Email
{
    public class EmailServerSettingDto
    {
        public int Id { get; set; }
        public int SchoolRegistrationId { get; set; }
        public string DisplayName { get; set; } = string.Empty;
        public string FromEmail { get; set; } = string.Empty;
        public string HostName { get; set; } = string.Empty;
        public int Port { get; set; }
        public string UserName { get; set; } = string.Empty;
        public bool EnableSSL { get; set; }
        public bool UseDefaultCredential { get; set; }
        public bool IsActive { get; set; }
        public string CreatedDate { get; set; } = string.Empty;
        public string UpdatedDate { get; set; } = string.Empty;
        public string? CreatedBy { get; set; }
    }
}
