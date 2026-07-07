using System.ComponentModel.DataAnnotations;

namespace School.Models.Email
{
    public class EmailServerSettingModel
    {
        public int Id { get; set; }

        public int SchoolRegistrationId { get; set; }

        [Required, MaxLength(200)]
        public string DisplayName { get; set; } = string.Empty;

        [Required, EmailAddress, MaxLength(200)]
        public string FromEmail { get; set; } = string.Empty;

        [Required, MaxLength(200)]
        public string HostName { get; set; } = string.Empty;

        [Required, Range(1, 65535)]
        public int Port { get; set; }

        [Required, MaxLength(200)]
        public string UserName { get; set; } = string.Empty;

        /// <summary>Plain-text password — will be encrypted in service before saving.</summary>
        [MaxLength(500)]
        public string? Password { get; set; }

        public bool EnableSSL { get; set; } = true;
        public bool UseDefaultCredential { get; set; } = false;
        public bool IsActive { get; set; } = true;

        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
