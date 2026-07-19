using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using School.Domain.School;
using static School.Domain.BaseEntity;

namespace School.Domain.Email
{
    #nullable disable
    [Table("EmailTemplates", Schema = "Communication")]
    public class EmailTemplate : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int SchoolRegistrationId { get; set; }

        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; }

        [Required, MaxLength(200)]
        public string TemplateName { get; set; }

        [Required, MaxLength(300)]
        public string Subject { get; set; }

        [Required]
        public string BodyHtml { get; set; }
        [MaxLength(500)]
        public string? Placeholder { get; set; }// e.g. {UserName}, {Link}

        [Required]
        public bool IsActive { get; set; } = true;

        public int? EmailServerSettingId { get; set; }

        [ForeignKey("EmailServerSettingId")]
        public EmailServerSetting EmailServerSetting { get; set; }
    }
}
