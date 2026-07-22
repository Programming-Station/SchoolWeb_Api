using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using School.Domain.School;
using static School.Domain.BaseEntity;

namespace School.Domain.FeeManagnment
{
    public class FeeStructure : AuditEntity<int>, ITenantEntity
    {
        public FeeStructure()
        {
            FeeStructureItems = new HashSet<FeeStructureItem>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty; // e.g. B.Tech 2026 Batch Fee Structure

        public int CampusId { get; set; }

        public int ProgramId { get; set; }

        public int BatchId { get; set; }

        public bool IsActive { get; set; } = true;

        [ForeignKey(nameof(CampusId))]
        public virtual Campus Campus { get; set; } = null!;

        [ForeignKey(nameof(ProgramId))]
        public virtual Program Program { get; set; } = null!;

        [ForeignKey(nameof(BatchId))]
        public virtual Batch Batch { get; set; } = null!;

        public virtual ICollection<FeeStructureItem> FeeStructureItems { get; set; }

        public int SchoolRegistrationId { get; set; }

        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}
