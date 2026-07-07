using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using School.Domain.School;
using static School.Domain.BaseEntity;

namespace School.Domain.Email
{
    public class EmailBranding : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int SchoolRegistrationId { get; set; }

        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;

        [Required, MaxLength(50)]
        public string ThemeColor { get; set; } = "#1e3a8a"; // Default professional Navy Blue

        public string? HeaderHtml { get; set; }

        public string? FooterHtml { get; set; }

        [MaxLength(200)]
        public string? SupportEmail { get; set; }

        [MaxLength(50)]
        public string? SupportPhone { get; set; }

        [MaxLength(200)]
        public string? PrincipalName { get; set; }
    }
}
