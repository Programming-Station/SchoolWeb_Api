using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using School.Domain.School;
using static School.Domain.BaseEntity;

namespace School.Domain.Hostel
{
    [Table("RoomCategories", Schema = "Hostel")]
    public class RoomCategory : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = null!; // Single, Double, Triple, Dormitory, Luxury, Disabled Friendly

        public bool IsAc { get; set; }

        public bool HasAttachedBathroom { get; set; }

        public bool HasWifi { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal DefaultFee { get; set; }

        public int SchoolRegistrationId { get; set; }

        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}
