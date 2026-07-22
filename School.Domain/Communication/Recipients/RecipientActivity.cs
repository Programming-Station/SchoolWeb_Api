using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace School.Domain.Communication.Recipients
{
    [Table("RecipientActivities", Schema = "Communication")]
    public class RecipientActivity : BaseEntity.AuditEntity<int>, BaseEntity.ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int SchoolRegistrationId { get; set; }

        public int RecipientId { get; set; }
        [ForeignKey("RecipientId")]
        public virtual Recipient Recipient { get; set; } = null!;

        [Required, StringLength(100)]
        public string ActivityType { get; set; } = null!;

        [StringLength(500)]
        public string? Description { get; set; }

        public DateTime ActivityDate { get; set; } = DateTime.UtcNow;
    }
}
