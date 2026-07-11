using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;
using School.Domain.School;

namespace School.Domain.Hostel
{
    [Table("Floors", Schema = "Hostel")]
    public class Floor : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        public int BuildingId { get; set; }

        public int FloorNumber { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        public int SchoolRegistrationId { get; set; }

        [ForeignKey(nameof(BuildingId))]
        public virtual Building Building { get; set; } = null!;

        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}
