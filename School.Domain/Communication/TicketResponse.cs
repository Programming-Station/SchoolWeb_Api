using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;

namespace School.Domain.Communication
{
    [Table("TicketResponses", Schema = "Communication")]
    public class TicketResponse : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int TicketId { get; set; }

        [ForeignKey(nameof(TicketId))]
        public virtual SupportTicket Ticket { get; set; } = null!;

        [Required, MaxLength(450)]
        public string SenderUserId { get; set; } = null!;

        [Required, MaxLength(4000)]
        public string Content { get; set; } = null!;

        public bool IsInternalNote { get; set; } = false; // Only visible to staff

        [MaxLength(500)]
        public string? AttachmentPath { get; set; }

        public int SchoolRegistrationId { get; set; }
    }
}
