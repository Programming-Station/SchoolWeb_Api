using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace School.Domain.Communication.Recipients
{
    [Table("DistributionLists", Schema = "Communication")]
    public class DistributionList : BaseEntity.AuditEntity<int>, BaseEntity.ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int SchoolRegistrationId { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; } = null!;

        [StringLength(500)]
        public string? Description { get; set; }

        [Required, StringLength(50)]
        public string ListType { get; set; } = null!; // Internal, External, Mixed
    }
}
