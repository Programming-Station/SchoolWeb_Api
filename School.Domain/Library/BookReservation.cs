using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;
using School.Domain.School;

namespace School.Domain.Library
{
    [Table("BookReservations")]
    public class BookReservation : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        public int BookId { get; set; }
        [ForeignKey(nameof(BookId))]
        public virtual Book Book { get; set; } = null!;

        public int MemberId { get; set; }
        [ForeignKey(nameof(MemberId))]
        public virtual LibraryMember Member { get; set; } = null!;

        public DateTime ReservationDate { get; set; } = DateTime.UtcNow;
        public DateTime ExpiryDate { get; set; }

        public int QueuePosition { get; set; } = 1;

        /// <summary>Pending | Ready | Allocated | Cancelled | Expired</summary>
        [MaxLength(20)]
        public string Status { get; set; } = "Pending";

        public DateTime? NotifiedAt { get; set; }

        [MaxLength(500)]
        public string? Remarks { get; set; }

        public int SchoolRegistrationId { get; set; }
        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration? SchoolRegistration { get; set; }
    }
}
