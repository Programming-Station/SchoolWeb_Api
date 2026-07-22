using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;

namespace School.Domain.Communication
{
    [Table("WhatsAppAccounts", Schema = "Communication")]
    public class WhatsAppAccount : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string PhoneNumberId { get; set; } = null!;

        [Required, MaxLength(50)]
        public string BusinessAccountId { get; set; } = null!;

        [Required, MaxLength(2000)]
        public string PermanentAccessToken { get; set; } = null!;

        [MaxLength(100)]
        public string? WebhookVerifyToken { get; set; }

        [MaxLength(2000)]
        public string? WebhookSecret { get; set; }

        [MaxLength(200)]
        public string? BaseUrl { get; set; }

        public bool IsSandbox { get; set; } = false;

        [Required, MaxLength(20)]
        public string Status { get; set; } = "Active"; // Active, Pending, Disabled

        public int SchoolRegistrationId { get; set; }
    }
}
