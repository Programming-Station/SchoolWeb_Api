using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using School.Domain.School;
using static School.Domain.BaseEntity;

namespace School.Domain.Hr.Assets
{
    public class SchoolAsset : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string AssetCode { get; set; } = string.Empty; // Unique inventory code

        [Required]
        [MaxLength(50)]
        public string Category { get; set; } = string.Empty; // IT, Furniture, Books, Vehicles, etc.

        [MaxLength(100)]
        public string? SerialNo { get; set; }

        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = "Available"; // Available, Assigned, Damaged, Retired

        public int SchoolRegistrationId { get; set; }
        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}
