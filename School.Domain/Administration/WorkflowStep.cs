using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;

namespace School.Domain.Administration
{
    [Table("WorkflowSteps", Schema = "Administration")]
    public class WorkflowStep : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int WorkflowDefinitionId { get; set; }

        [ForeignKey(nameof(WorkflowDefinitionId))]
        public virtual WorkflowDefinition WorkflowDefinition { get; set; } = null!;

        public int StepNumber { get; set; }

        [Required, MaxLength(100)]
        public string ApproverRole { get; set; } = null!; // e.g. Principal, Admin, AcademicHead

        public int? TimeoutHours { get; set; }

        public int SchoolRegistrationId { get; set; }
    }
}
