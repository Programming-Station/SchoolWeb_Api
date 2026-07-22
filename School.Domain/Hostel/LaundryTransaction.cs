using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using School.Domain.School;
using static School.Domain.BaseEntity;

namespace School.Domain.Hostel
{
    [Table("LaundryTransactions", Schema = "Hostel")]
    public class LaundryTransaction : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        public int StudentId { get; set; }

        [Required, MaxLength(100)]
        public string TokenNumber { get; set; } = null!;

        public int ItemCount { get; set; }

        public DateTime PickupDate { get; set; }

        public DateTime ExpectedDelivery { get; set; }

        public DateTime? ActualDelivery { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Charges { get; set; }

        [Required, MaxLength(50)]
        public string Status { get; set; } = "Collected"; // Collected, InProgress, Ready, Delivered

        public int SchoolRegistrationId { get; set; }

        [ForeignKey(nameof(StudentId))]
        public virtual global::School.Domain.Student.Student Student { get; set; } = null!;

        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}
