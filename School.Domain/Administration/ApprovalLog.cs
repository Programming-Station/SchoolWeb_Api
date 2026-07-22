using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;

namespace School.Domain.Administration
{
    [Table("ApprovalLogs", Schema = "Administration")]
    public class ApprovalLog : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int WorkflowInstanceId { get; set; }

        [ForeignKey(nameof(WorkflowInstanceId))]
        public virtual WorkflowInstance WorkflowInstance { get; set; } = null!;

        [Required, MaxLength(450)]
        public string ApprovedByUserId { get; set; } = null!;

        [Required, MaxLength(30)]
        public string Action { get; set; } = null!; // Approved, Returned, Rejected

        [MaxLength(1000)]
        public string? Comments { get; set; }

        public DateTime ActionDate { get; set; } = DateTime.UtcNow;

        public int SchoolRegistrationId { get; set; }
    }
}
