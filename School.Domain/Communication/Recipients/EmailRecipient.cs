using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using School.Domain.Email;
using School.Domain.School;

namespace School.Domain.Communication.Recipients
{
    public enum RecipientType
    {
        To = 1,
        Cc = 2,
        Bcc = 3
    }

    [Table("EmailRecipients", Schema = "Communication")]
    public class EmailRecipient : BaseEntity.AuditEntity<int>, BaseEntity.ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int SchoolRegistrationId { get; set; }

        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;

        [Required]
        public int EmailMessageId { get; set; }

        [ForeignKey(nameof(EmailMessageId))]
        public virtual EmailLog EmailMessage { get; set; } = null!;

        public int? AddressBookId { get; set; }

        [ForeignKey(nameof(AddressBookId))]
        public virtual Recipient? AddressBook { get; set; }

        [Required, MaxLength(250)]
        public string EmailAddress { get; set; } = string.Empty;

        [Required]
        public RecipientType RecipientType { get; set; }

        [MaxLength(200)]
        public string? DisplayName { get; set; }
    }
}
