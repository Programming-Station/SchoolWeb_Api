using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;
using School.Domain.School;

namespace School.Domain.Hostel
{
    [Table("RoomTransferHistories", Schema = "Hostel")]
    public class RoomTransferHistory : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        public int StudentId { get; set; }

        public int FromRoomId { get; set; }

        public int ToRoomId { get; set; }

        public int FromBedId { get; set; }

        public int ToBedId { get; set; }

        public DateTime TransferDate { get; set; }

        [Required, MaxLength(500)]
        public string Reason { get; set; } = null!;

        [MaxLength(200)]
        public string? ApprovedBy { get; set; }

        public int SchoolRegistrationId { get; set; }

        [ForeignKey(nameof(StudentId))]
        public virtual global::School.Domain.Student.Student Student { get; set; } = null!;

        [ForeignKey(nameof(FromRoomId))]
        public virtual Room FromRoom { get; set; } = null!;

        [ForeignKey(nameof(ToRoomId))]
        public virtual Room ToRoom { get; set; } = null!;

        [ForeignKey(nameof(FromBedId))]
        public virtual Bed FromBed { get; set; } = null!;

        [ForeignKey(nameof(ToBedId))]
        public virtual Bed ToBed { get; set; } = null!;

        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}
