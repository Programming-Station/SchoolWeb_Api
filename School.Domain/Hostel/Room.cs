using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;
using School.Domain.School;

namespace School.Domain.Hostel
{
    [Table("Rooms", Schema = "Hostel")]
    public class Room : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string RoomNumber { get; set; } = null!;

        public int HostelId { get; set; }

        public int BuildingId { get; set; }

        public int FloorId { get; set; }

        public int RoomCategoryId { get; set; }

        public int Capacity { get; set; }

        public int OccupiedBeds { get; set; }

        public int AvailableBeds { get; set; }

        [MaxLength(1000)]
        public string? FurnitureDetails { get; set; }

        [MaxLength(500)]
        public string? PhotoUrl { get; set; }

        [Required, MaxLength(20)]
        public string Status { get; set; } = "active"; // active, inactive, maintenance

        [MaxLength(500)]
        public string? Remarks { get; set; }

        public int SchoolRegistrationId { get; set; }

        [ForeignKey(nameof(HostelId))]
        public virtual Hostel Hostel { get; set; } = null!;

        [ForeignKey(nameof(BuildingId))]
        public virtual Building Building { get; set; } = null!;

        [ForeignKey(nameof(FloorId))]
        public virtual Floor Floor { get; set; } = null!;

        [ForeignKey(nameof(RoomCategoryId))]
        public virtual RoomCategory RoomCategory { get; set; } = null!;

        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}
