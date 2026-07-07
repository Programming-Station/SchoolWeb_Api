using System.ComponentModel.DataAnnotations;

namespace School.Models.Email
{
    public class EmailBrandingModel
    {
        public int Id { get; set; }

        public int SchoolRegistrationId { get; set; }

        [Required, MaxLength(50)]
        public string ThemeColor { get; set; } = "#1e3a8a";

        public string? HeaderHtml { get; set; }

        public string? FooterHtml { get; set; }

        [MaxLength(200), EmailAddress]
        public string? SupportEmail { get; set; }

        [MaxLength(50)]
        public string? SupportPhone { get; set; }

        [MaxLength(200)]
        public string? PrincipalName { get; set; }

        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
