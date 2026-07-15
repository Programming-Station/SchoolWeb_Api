using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;

namespace School.Domain.Communication
{
    [Table("WhatsAppTemplates", Schema = "Communication")]
    public class WhatsAppTemplate : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string TemplateName { get; set; } = null!;

        [Required, MaxLength(50)]
        public string Category { get; set; } = null!; // Utility, Authentication, Marketing

        [Required, MaxLength(10)]
        public string LanguageCode { get; set; } = "en_US";

        [Required, MaxLength(4000)]
        public string BodyTemplate { get; set; } = null!;

        [Required, MaxLength(20)]
        public string Status { get; set; } = "Approved"; // Approved, Pending, Rejected

        public int SchoolRegistrationId { get; set; }
    }
}
