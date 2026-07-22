using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;

namespace School.Domain.Administration
{
    [Table("WorkflowDefinitions", Schema = "Administration")]
    public class WorkflowDefinition : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(150)]
        public string Name { get; set; } = null!;

        [Required, MaxLength(50)]
        public string TriggerType { get; set; } = null!; // AdmissionApproval, ExpenseApproval, LeaveApproval

        public bool IsActive { get; set; } = true;

        public int SchoolRegistrationId { get; set; }

        public virtual ICollection<WorkflowStep> Steps { get; set; } = new List<WorkflowStep>();
    }
}
