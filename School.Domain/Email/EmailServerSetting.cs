using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using School.Domain.School;
#nullable disable
using static School.Domain.BaseEntity;

namespace School.Domain.Email
{
    public class EmailServerSetting : AuditEntity<int>, ITenantEntity
    {
        public EmailServerSetting()
        {
            EmailTemplates = new HashSet<EmailTemplate>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public int SchoolRegistrationId { get; set; }

        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; }

        [Required, MaxLength(200)]
        public string FromEmail { get; set; }

        [Required, MaxLength(200)]
        public string HostName { get; set; }

        [Required]
        public int Port { get; set; }

        [Required, MaxLength(200)]
        public string UserName { get; set; }

        [Required, MaxLength(500)]
        public string Password { get; set; }

        [Required]
        public bool EnableSSL { get; set; } = true;

        [MaxLength(200)]
        public string? DisplayName { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;
        public bool UseDefaultCredential { get; set; }

        public virtual ICollection<EmailTemplate> EmailTemplates { get; set; }
    }
}
