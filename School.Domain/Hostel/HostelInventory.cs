using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;
using School.Domain.School;

namespace School.Domain.Hostel
{
    [Table("HostelInventories", Schema = "Hostel")]
    public class HostelInventory : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        public int RoomId { get; set; }

        [Required, MaxLength(200)]
        public string AssetName { get; set; } = null!; // Bed, Chair, Table, Cupboard, Locker, AC, Fan, etc.

        [Required, MaxLength(100)]
        public string AssetTag { get; set; } = null!;

        [MaxLength(100)]
        public string? Barcode { get; set; }

        [Required, MaxLength(50)]
        public string Status { get; set; } = "Active"; // Active, Damaged, Replaced

        public DateTime? AuditDate { get; set; }

        public int SchoolRegistrationId { get; set; }

        [ForeignKey(nameof(RoomId))]
        public virtual Room Room { get; set; } = null!;

        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}
