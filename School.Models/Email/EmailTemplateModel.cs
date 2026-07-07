using System.ComponentModel.DataAnnotations;

namespace School.Models.Email
{
    public class EmailTemplateModel
    {
        public int Id { get; set; }

        public int SchoolRegistrationId { get; set; }

        [Required, MaxLength(200)]
        public string TemplateName { get; set; } = string.Empty;

        [Required, MaxLength(300)]
        public string Subject { get; set; } = string.Empty;

        [Required]
        public string BodyHtml { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Placeholder { get; set; }

        public bool IsActive { get; set; } = true;

        public int? EmailServerSettingId { get; set; }

        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
