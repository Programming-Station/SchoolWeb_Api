using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;

namespace School.Domain.Administration
{
    [Table("WorkflowInstances", Schema = "Administration")]
    public class WorkflowInstance : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int WorkflowDefinitionId { get; set; }

        [ForeignKey(nameof(WorkflowDefinitionId))]
        public virtual WorkflowDefinition WorkflowDefinition { get; set; } = null!;

        [Required, MaxLength(100)]
        public string TargetEntityName { get; set; } = null!; // AdmissionApplication, ExpenseClaim

        [Required]
        public int TargetEntityId { get; set; }

        public int CurrentStepId { get; set; }

        [Required, MaxLength(30)]
        public string Status { get; set; } = "Pending"; // Pending, Approved, Rejected

        public int SchoolRegistrationId { get; set; }

        public virtual ICollection<ApprovalLog> ApprovalLogs { get; set; } = new List<ApprovalLog>();
    }
}
