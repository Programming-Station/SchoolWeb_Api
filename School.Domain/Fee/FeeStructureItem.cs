using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;

namespace School.Domain.FeeManagnment
{
    public class FeeStructureItem : AuditEntity<int>
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int FeeStructureId { get; set; }

        [Required]
        public int FeeTypeId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [ForeignKey(nameof(FeeStructureId))]
        public virtual FeeStructure FeeStructure { get; set; } = null!;

        [ForeignKey(nameof(FeeTypeId))]
        public virtual FeeType FeeType { get; set; } = null!;
    }
}
