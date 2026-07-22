using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace School.Domain.Communication.Recipients
{
    [Table("RecipientHistories", Schema = "Communication")]
    public class RecipientHistory : BaseEntity.AuditEntity<int>, BaseEntity.ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int SchoolRegistrationId { get; set; }

        public int RecipientId { get; set; }
        [ForeignKey("RecipientId")]
        public virtual Recipient Recipient { get; set; } = null!;

        [Required, StringLength(50)]
        public string Channel { get; set; } = null!;

        [StringLength(255)]
        public string? MessageSubject { get; set; }

        public string? MessageBody { get; set; }

        [Required, StringLength(50)]
        public string DeliveryStatus { get; set; } = "Sent"; // Sent, Failed, Read

        public DateTime SentAt { get; set; } = DateTime.UtcNow;
        public DateTime? DeliveredAt { get; set; }
        public DateTime? ReadAt { get; set; }

        [StringLength(1000)]
        public string? ErrorReason { get; set; }
    }
}

