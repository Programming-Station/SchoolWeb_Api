using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using School.Domain.School;
using static School.Domain.BaseEntity;

namespace School.Domain.Payroll
{
    [Table("SalaryStructures", Schema = "Payroll")]
    public class SalaryStructure : AuditEntity<int>, ITenantEntity
    {
        public SalaryStructure()
        {
            Items = new HashSet<SalaryStructureItem>();
        }

        [Key]
        public int Id { get; set; }

        [Required, MaxLength(150)]
        public string Name { get; set; } = null!; // e.g. "Teaching Staff Standard"

        [MaxLength(500)]
        public string? Description { get; set; }

        public int PayGroupId { get; set; }
        [ForeignKey(nameof(PayGroupId))]
        public virtual PayGroup PayGroup { get; set; } = null!;

        public bool IsActive { get; set; } = true;

        [Required]
        public int SchoolRegistrationId { get; set; }

        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;

        public virtual ICollection<SalaryStructureItem> Items { get; set; }
    }
}
