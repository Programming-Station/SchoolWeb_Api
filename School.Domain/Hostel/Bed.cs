using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;
using School.Domain.School;

namespace School.Domain.Hostel
{
    [Table("Beds", Schema = "Hostel")]
    public class Bed : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        public int RoomId { get; set; }

        [Required, MaxLength(50)]
        public string BedNumber { get; set; } = null!;

        [Required, MaxLength(20)]
        public string Status { get; set; } = "vacant"; // vacant, occupied, reserved, repair

        [MaxLength(100)]
        public string? LockerNumber { get; set; }

        [MaxLength(100)]
        public string? CupboardNumber { get; set; }

        [MaxLength(100)]
        public string? MattressNumber { get; set; }

        [MaxLength(500)]
        public string? QrCode { get; set; }

        [MaxLength(500)]
        public string? Barcode { get; set; }

        [MaxLength(500)]
        public string? RfidTag { get; set; }

        [MaxLength(500)]
        public string? CleaningStatus { get; set; } = "clean"; // clean, dirty, in-progress

        public int SchoolRegistrationId { get; set; }

        [ForeignKey(nameof(RoomId))]
        public virtual Room Room { get; set; } = null!;

        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}
