using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using School.Domain.School;

namespace School.Domain.Email
{
    [Table("EmailAttachments", Schema = "Communication")]
    public class EmailAttachment : BaseEntity.AuditEntity<int>, BaseEntity.ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int SchoolRegistrationId { get; set; }

        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;

        [Required]
        public int EmailLogId { get; set; }

        [ForeignKey(nameof(EmailLogId))]
        public virtual EmailLog EmailLog { get; set; } = null!;

        [Required, MaxLength(255)]
        public string FileName { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string ContentType { get; set; } = string.Empty;

        [Required]
        public byte[] FileBytes { get; set; } = Array.Empty<byte>();
    }
}
