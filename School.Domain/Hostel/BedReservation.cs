using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using School.Domain.School;
using static School.Domain.BaseEntity;

namespace School.Domain.Hostel
{
    [Table("BedReservations", Schema = "Hostel")]
    public class BedReservation : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        public int StudentId { get; set; }

        public int BedId { get; set; }

        public DateTime ReservationDate { get; set; }

        public DateTime ExpiryDate { get; set; }

        [Required, MaxLength(20)]
        public string Status { get; set; } = "active"; // active, cancelled, converted

        public int SchoolRegistrationId { get; set; }

        [ForeignKey(nameof(StudentId))]
        public virtual global::School.Domain.Student.Student Student { get; set; } = null!;

        [ForeignKey(nameof(BedId))]
        public virtual Bed Bed { get; set; } = null!;

        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}
